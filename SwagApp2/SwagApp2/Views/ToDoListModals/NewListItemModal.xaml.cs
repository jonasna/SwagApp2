using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwagApp2.Views.CustomDialog;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SwagApp2.Views.ToDoListModals
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewListItemModal : CustomDialogBase
    {
		public NewListItemModal ()
		{
			InitializeComponent ();
		}
	}
}