using System.Threading.Tasks;

namespace DemoUWP.Services
{
    public interface IWhatsNewDisplayService
    {
        Task ShowIfAppropriateAsync();
    }
}
