using System.Threading.Tasks;

using Windows.ApplicationModel.Background;

namespace DemoUWP.Services
{
    internal interface IBackgroundTaskService
    {
        Task RegisterBackgroundTasksAsync();

        void Start(IBackgroundTaskInstance taskInstance);
    }
}
