using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Essentials;

namespace XamTests.ViewModels
{
    public class LogsViewModel : BindableBase
    {
        private string log;
        public string Log {
            get { return log; }
            set { SetProperty(ref log, value); }
        }


        public void Refresh()
        {
            var latest_file = Directory.GetFiles(Path.Combine(FileSystem.CacheDirectory, "logs"))
                                 ?.Select(f => new FileInfo(f))
                                  .OrderByDescending(f => f.CreationTimeUtc)
                                  .FirstOrDefault();

            if (latest_file != null)
            {
                Log = File.ReadAllText(latest_file.FullName);
            }
        }
    }
}
