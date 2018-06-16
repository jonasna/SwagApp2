using System.Threading.Tasks;
using SwagApp2.Models;

namespace SwagApp2.DialogService.ToDoListDialogs
{
    public interface IToDoListDialogService
    {
        Task<ToDoList> ShowCreateToDoListModalAsync();
        Task<ListItem> ShowCreateListItemModalAsync();
    }
}