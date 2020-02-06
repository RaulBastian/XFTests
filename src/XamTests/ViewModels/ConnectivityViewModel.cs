using Prism.Mvvm;
using System;
using Tests;

namespace XamTests.ViewModels
{
    public class ConnectivityViewModel : BindableBase
    {
        public ConnectivityViewModel()
        {
            InternetConnectivity.Restored += InternetConnectivity_InternetConnectionRestored;
            InternetConnectivity.Lost += InternetConnectivity_InternetConnectionLost;  
        }

        private void InternetConnectivity_InternetConnectionRestored(object sender, EventArgs e)
        {
            HasInternetAccess = InternetConnectivity.HasInternetAccess;

            Status = "Restored...";
        }

        private void InternetConnectivity_InternetConnectionLost(object sender, EventArgs e)
        {
            HasInternetAccess = InternetConnectivity.HasInternetAccess;

            Status = "Lost...";
        }

        private string status = "";
        public string Status {
            get {
                return status;
            }
            set {
                SetProperty(ref status, value);
            }
        }

        private bool hasInternetAccess = false;
        public bool HasInternetAccess {
            get {
                return hasInternetAccess;
            }
            set {
                SetProperty(ref hasInternetAccess, value);
            }
        }
    }
}
