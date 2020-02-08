using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamTests.ViewModels;

namespace XamTests.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void TabbedPage_CurrentPageChanged(object sender, EventArgs e)
        {
            if (this.CurrentPage is LogsPage l && l.BindingContext is LogsViewModel vm)
            {
                vm.Refresh();
            }
        }
    }
}