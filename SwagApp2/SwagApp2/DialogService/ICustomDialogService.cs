using System.Threading.Tasks;

namespace SwagApp2.DialogService
{
    public interface ICustomDialogService
    {
        Task<bool> ShowErrorDialogAsync(string title, string message, string btnText);
        Task<bool> ShowInfoDialogAsync(string title, string message, string btnText);
    }
}