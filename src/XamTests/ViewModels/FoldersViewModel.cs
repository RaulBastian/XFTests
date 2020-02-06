using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Essentials;

namespace XamTests.ViewModels
{
    public class FoldersViewModel : BindableBase
    {
        public FoldersViewModel()
        {
            folders = new ObservableCollection<Tuple<string, string>>(new Tuple<string, string>[]{
            new Tuple<string, string>("CacheDirectory:", FileSystem.CacheDirectory),
            new Tuple<string, string>("AppDataDirectory", FileSystem.AppDataDirectory)
            });
        }

        private ObservableCollection<Tuple<string, string>> folders = null;

        public ObservableCollection<Tuple<string, string>> Folders {
            get { return folders; }
            set { folders = value; }
        }


    }
}
