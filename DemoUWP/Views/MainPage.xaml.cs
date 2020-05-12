using System;
using System.Diagnostics;
using DemoUWP.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using System.IO;
using Windows.ApplicationModel;

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
                await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync("CMDGroup");
            }
        }
    }
}
