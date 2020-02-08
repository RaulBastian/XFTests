using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
            new Tuple<string, string>("AppDataDirectory", FileSystem.AppDataDirectory),
            new Tuple<string, string>("ApplicationData", System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData)),
            new Tuple<string, string>("LocalApplicationData", System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData)),
            new Tuple<string, string>("Databases Folder",Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData),"databases")),
            new Tuple<string, string>("Databases DB",Path.Combine(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData),"databases"),"data.db")),
            new Tuple<string, string>("Logs Path",Path.Combine(FileSystem.CacheDirectory,"logs"))
            }); ;
        }

        private ObservableCollection<Tuple<string, string>> folders = null;

        public ObservableCollection<Tuple<string, string>> Folders {
            get { return folders; }
            set { folders = value; }
        }


    }
}
