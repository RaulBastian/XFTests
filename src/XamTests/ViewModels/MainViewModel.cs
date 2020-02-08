using Prism.Commands;
using Prism.Mvvm;
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

        }


    }
}
