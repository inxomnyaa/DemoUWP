﻿using System;

using DemoUWP.ViewModels;

using Windows.UI.Xaml.Controls;

namespace DemoUWP.Views
{
    // TODO WTS: Change the icons and titles for all NavigationViewItems in ShellPage.xaml.
    public sealed partial class ShellPage : Page
    {
        private ShellViewModel ViewModel => DataContext as ShellViewModel;

        public Frame ShellFrame => shellFrame;

        public ShellPage()
        {
            InitializeComponent();
        }

        public void SetRootFrame(Frame frame)
        {
            shellFrame.Content = frame;
            navigationViewHeaderBehavior.Initialize(frame);
            ViewModel.Initialize(frame, navigationView);
        }
    }
}
