using System;

using DemoUWP.ViewModels;

using Windows.UI.Xaml.Controls;

namespace DemoUWP.Views
{
    public sealed partial class ContentGridPage : Page
    {
        private ContentGridViewModel ViewModel => DataContext as ContentGridViewModel;

        public ContentGridPage()
        {
            InitializeComponent();
        }
    }
}
