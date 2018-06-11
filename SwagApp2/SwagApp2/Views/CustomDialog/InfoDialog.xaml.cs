using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using SwagApp2.DialogService;
using SwagApp2.ViewModels.CustomDialog;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SwagApp2.Views.CustomDialog
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class InfoDialog : CustomDialogBase
    {
		public InfoDialog()
		{
			InitializeComponent ();    
        }
    }
}