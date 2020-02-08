using Prism.Commands;
using Prism.Mvvm;
using Prism.Unity;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace XamTests.ViewModels
{
    public class MainViewModel:BindableBase
    {

        private DelegateCommand<EventArgs> tabChange;
        public DelegateCommand<EventArgs> CurrentPageChanged => tabChange ?? (tabChange = new DelegateCommand<EventArgs>(ExecuteCommandName));

        void ExecuteCommandName(EventArgs args)
        {
            
            try
            {
                //This should fail
                var di = PrismApplication.Current.Container.Resolve(typeof(DependencyInjectionTestViewModel));


                //Ah, it doesn't!! with the unity container it would have, lets try it... ah
                PrismApplication.Current.Container.GetContainer().Resolve(typeof(DependencyInjectionTestViewModel), "DependencyInjectionTestViewModel");


            }
            catch (Exception ex)
            {
                Log.Error(ex, "ExecuteCommandName");
            }
            

        }
    }
}
