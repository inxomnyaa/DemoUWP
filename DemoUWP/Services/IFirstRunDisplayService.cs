using System.Threading.Tasks;

namespace DemoUWP.Services
{
    public interface IFirstRunDisplayService
    {
        Task ShowIfAppropriateAsync();
    }
}
