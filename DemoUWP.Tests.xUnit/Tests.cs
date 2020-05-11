using System;

using DemoUWP.Core.Services;
using DemoUWP.Services;
using DemoUWP.ViewModels;

using Moq;

using Prism.Windows.Navigation;

using Xunit;

namespace DemoUWP.Tests.XUnit
{
    // TODO WTS: Add appropriate tests
    public class Tests
    {
        [Fact]
        public void TestMethod1()
        {
        }

        // TODO WTS: Add tests for functionality you add to ContentGridViewModel.
        [Fact]
        public void TestContentGridViewModelCreation()
        {
            // This test is trivial. Add your own tests for the logic you add to the ViewModel.
            var mockNavService = new Mock<INavigationService>();
            var mockDataService = new Mock<ISampleDataService>();
            var mockAnimationService = new Mock<IConnectedAnimationService>();
            var vm = new ContentGridViewModel(mockNavService.Object, mockDataService.Object, mockAnimationService.Object);
            Assert.NotNull(vm);
        }

        // TODO WTS: Add tests for functionality you add to DataGridViewModel.
        [Fact]
        public void TestDataGridViewModelCreation()
        {
            // This test is trivial. Add your own tests for the logic you add to the ViewModel.
            var mockDataService = new Mock<ISampleDataService>();
            var vm = new DataGridViewModel(mockDataService.Object);
            Assert.NotNull(vm);
        }

        // TODO WTS: Add tests for functionality you add to MainViewModel.
        [Fact]
        public void TestMainViewModelCreation()
        {
            // This test is trivial. Add your own tests for the logic you add to the ViewModel.
            var vm = new MainViewModel();
            Assert.NotNull(vm);
        }

        // TODO WTS: Add tests for functionality you add to MasterDetailViewModel.
        [Fact]
        public void TestMasterDetailViewModelCreation()
        {
            // This test is trivial. Add your own tests for the logic you add to the ViewModel.
            var mockNavService = new Mock<INavigationService>();
            var mockDataService = new Mock<ISampleDataService>();
            var vm = new MasterDetailViewModel(mockNavService.Object, mockDataService.Object);
            Assert.NotNull(vm);
        }

        // TODO WTS: Add tests for functionality you add to SettingsViewModel.
        [Fact]
        public void TestSettingsViewModelCreation()
        {
            // This test is trivial. Add your own tests for the logic you add to the ViewModel.
            var vm = new SettingsViewModel();
            Assert.NotNull(vm);
        }

        // TODO WTS: Add tests for functionality you add to TelerikDataGridViewModel.
        [Fact]
        public void TestTelerikDataGridViewModelCreation()
        {
            // This test is trivial. Add your own tests for the logic you add to the ViewModel.
            var mockDataService = new Mock<ISampleDataService>();
            var vm = new TelerikDataGridViewModel(mockDataService.Object);
            Assert.NotNull(vm);
        }
    }
}
