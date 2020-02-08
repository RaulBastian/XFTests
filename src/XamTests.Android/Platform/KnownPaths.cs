using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using XamTests.Types;

namespace XamTests.Droid.Platform
{
    public class KnownPaths : IKnownPaths
    {
        public string Database { get { return System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData); } }

        public string Photos => throw new NotImplementedException();
    }
}