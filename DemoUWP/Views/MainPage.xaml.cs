using System;
using System.Diagnostics;
using DemoUWP.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using System.IO;
using Windows.ApplicationModel;
using Windows.Storage;

namespace DemoUWP.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel => DataContext as MainViewModel;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void RunCommandButton_Click(object sender, RoutedEventArgs e)
        {
            //Button button = (Button)sender;
            TextBox commandTextBox = (TextBox)Command;
            string command = commandTextBox.Text;
            Debug.WriteLine(command);
            if (ApiInformation.IsApiContractPresent("Windows.ApplicationModel.FullTrustAppContract", 1, 0))
            {
                //await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync("CMDGroup");
                // store command line parameters in local settings
                // so the Lancher can retrieve them and pass them on
                ApplicationData.Current.LocalSettings.Values["parameters"] = command;

                await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync("CMD");
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ApiInformation.IsApiContractPresent("Windows.ApplicationModel.FullTrustAppContract", 1, 0))
            {
                await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync("RemoteDesktop");
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (ApiInformation.IsApiContractPresent("Windows.ApplicationModel.FullTrustAppContract", 1, 0))
            {
                //await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync("CMDGroup");
                // store command line parameters in local settings
                // so the Lancher can retrieve them and pass them on
                ApplicationData.Current.LocalSettings.Values["parameters"] = "/c \"taskkill /IM cmd.exe /F\"";

                await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync("CMD");
            }
        }
    }
}
