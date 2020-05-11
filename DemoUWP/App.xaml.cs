using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using DemoUWP.Activation;
using DemoUWP.Core.Services;
using DemoUWP.Services;
using DemoUWP.Views;

using Microsoft.Practices.Unity;

using Prism.Mvvm;
using Prism.Unity.Windows;
using Prism.Windows.AppModel;
using Prism.Windows.Navigation;

using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DemoUWP
{
    [Windows.UI.Xaml.Data.Bindable]
    public sealed partial class App : PrismUnityApplication
    {
        public App()
        {
            InitializeComponent();
        }

        // Used to store details between when UWP makes them available and when Prism convention wants to use them.
        private (string activationPath, string arguments) cmdLineDetails;

        protected override void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);

            // Details of the command line launch can only be accessed during App.OnActivated.
            // Storing the details for use when needed in `OnActivateApplicationAsync`.
            if (args.Kind == ActivationKind.CommandLineLaunch)
            {
                var operation = ((CommandLineActivatedEventArgs)args).Operation;
                cmdLineDetails = (operation.CurrentDirectoryPath, operation.Arguments);
            }
        }

        protected override void ConfigureContainer()
        {
            // register a singleton using Container.RegisterType<IInterface, Type>(new ContainerControlledLifetimeManager());
            base.ConfigureContainer();
            Container.RegisterType<IWhatsNewDisplayService, WhatsNewDisplayService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IFirstRunDisplayService, FirstRunDisplayService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IToastNotificationsService, ToastNotificationsService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IBackgroundTaskService, BackgroundTaskService>(new ContainerControlledLifetimeManager());
            Container.RegisterInstance<IResourceLoader>(new ResourceLoaderAdapter(new ResourceLoader()));
            Container.RegisterType<ISampleDataService, SampleDataService>();
        }

        protected override async Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            await LaunchApplicationAsync(PageTokens.MainPage, null);
        }

        private async Task LaunchApplicationAsync(string page, object launchParam)
        {
            await ThemeSelectorService.SetRequestedThemeAsync();
            NavigationService.Navigate(page, launchParam);
            Window.Current.Activate();
            await Container.Resolve<IWhatsNewDisplayService>().ShowIfAppropriateAsync();
            await Container.Resolve<IFirstRunDisplayService>().ShowIfAppropriateAsync();
            Container.Resolve<IToastNotificationsService>().ShowToastNotificationSample();
        }

        protected override async Task OnActivateApplicationAsync(IActivatedEventArgs args)
        {
            if (args.Kind == ActivationKind.ToastNotification && args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                // Handle a toast notification here
                // Since dev center, toast, and Azure notification hub will all active with an ActivationKind.ToastNotification
                // you may have to parse the toast data to determine where it came from and what action you want to take
                // If the app isn't running then launch the app here
                await OnLaunchApplicationAsync(args as LaunchActivatedEventArgs);
            }

            // By default, this handler expects URIs of the format 'wtsapp:sample?paramName1=paramValue1&paramName2=paramValue2'
            if (args.Kind == ActivationKind.Protocol && args is ProtocolActivatedEventArgs protocolArgs && protocolArgs.Uri != null)
            {
                // Create data from activation Uri in ProtocolActivatedEventArgs
                var data = new SchemeActivationData(protocolArgs.Uri);
                if (data.IsValid)
                {
                    await LaunchApplicationAsync(data.PageToken, data.Parameters);
                }
                else if (args.PreviousExecutionState != ApplicationExecutionState.Running)
                {
                    // If the app isn't running and not navigating to a specific page based on the URI, navigate to the home page
                    await OnLaunchApplicationAsync(args as LaunchActivatedEventArgs);
                }
            }

            // Only handle a commandline launch if arguments are passed.
            // Learn more about these EventArgs at https://docs.microsoft.com/en-us/uwp/api/windows.applicationmodel.activation.commandlineactivatedeventargs
            if (args.Kind == ActivationKind.CommandLineLaunch && !string.IsNullOrWhiteSpace(cmdLineDetails.arguments) && args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                // Because these are supplied by the caller, they should be treated as untrustworthy.
                var cmdLineString = cmdLineDetails.arguments;

                // The directory where the command-line activation request was made.
                // This is typically not the install location of the app itself, but could be any arbitrary path.
                var activationPath = cmdLineDetails.activationPath;

                //// TODO WTS: parse the cmdLineString to determine what to do.
                //// If the arguments warrant showing a different view on launch, that can be done here.
                //// await LaunchApplicationAsync(PageTokens.CmdLineActivationSamplePage, cmdLineString);
                //// If you do nothing, the app will launch like normal.
            }

            await Task.CompletedTask;
        }

        protected override void OnBackgroundActivated(BackgroundActivatedEventArgs args)
        {
            base.OnBackgroundActivated(args);
            if (Container == null)
            {
                // Edge case where the in-process background task's activation trigger is handled when the application is just shut down.
                // Known issue: NullReferenceException in the OnSuspending method for the short application activation to handle the trigger.
                // This will be fixed in the next Prism release, more info see https://github.com/Microsoft/WindowsTemplateStudio/issues/2632
                CreateAndConfigureContainer();
            }

            Container.Resolve<IBackgroundTaskService>().Start(args.TaskInstance);
        }

        protected override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            await base.OnInitializeAsync(args);
            await ThemeSelectorService.InitializeAsync().ConfigureAwait(false);

            // We are remapping the default ViewNamePage and ViewNamePageViewModel naming to ViewNamePage and ViewNameViewModel to
            // gain better code reuse with other frameworks and pages within Windows Template Studio
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                var viewModelTypeName = string.Format(CultureInfo.InvariantCulture, "DemoUWP.ViewModels.{0}ViewModel, DemoUWP", viewType.Name.Substring(0, viewType.Name.Length - 4));
                return Type.GetType(viewModelTypeName);
            });
            await WindowManagerService.Current.InitializeAsync();
            await Container.Resolve<IBackgroundTaskService>().RegisterBackgroundTasksAsync();
        }

        protected override IDeviceGestureService OnCreateDeviceGestureService()
        {
            var service = base.OnCreateDeviceGestureService();
            service.UseTitleBarBackButton = false;
            return service;
        }

        public void SetNavigationFrame(Frame frame)
        {
            var sessionStateService = Container.Resolve<ISessionStateService>();
            CreateNavigationService(new FrameFacadeAdapter(frame), sessionStateService);
        }

        protected override UIElement CreateShell(Frame rootFrame)
        {
            var shell = Container.Resolve<ShellPage>();
            shell.SetRootFrame(rootFrame);
            Container.RegisterInstance<IConnectedAnimationService>(new ConnectedAnimationService(rootFrame));
            return shell;
        }
    }
}
