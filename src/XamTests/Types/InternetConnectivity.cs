using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Serilog;

namespace Tests
{
    public class InternetConnectivity
    {
        private static HttpClient google_http_client = new HttpClient();
        private static string google_host = "https://www.google.es";
        private static bool has_internet_access = false;

        private static int counter_fast_ping = 5;
        private static int fast_ping_frequency = 3000;
        private static int slow_ping_frequency = 20000;

        private static bool pause_slow_ping = false;

        public static event EventHandler Lost;
        public static event EventHandler Restored;

        static InternetConnectivity()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

            StartSlowCheck();
        }

        private static void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            //We clear its value so we need to calculate it again...
            has_internet_access = false;

            //We do a quick internet check...
            StartFastCheck();
        }

        public static bool HasNetworkAccess {
            get {
                return Connectivity.NetworkAccess == NetworkAccess.Internet || Connectivity.NetworkAccess == NetworkAccess.ConstrainedInternet;
            }
        }

        public static bool HasInternetAccess {
            get {

                if (!HasNetworkAccess)
                {
                    return false;
                }

                return has_internet_access;
            }
        }

        private static void StartFastCheck()
        {
            Task.Factory.StartNew(async () =>
            {
                pause_slow_ping = true;

                try
                {
                    if (!has_internet_access)
                    {
                        //We need to check has_internet_access , just in case it was queried in the slow before disabling...
                        while (!has_internet_access)
                        {
                            if (HasNetworkAccess)
                            {
                                has_internet_access = await PingGoogle();
                            }
                            else
                            {
                                has_internet_access = false;
                                break;
                            }

                            //If we already have access, no need to continue or if we have reached the max.. we'll leave it to the slow ping...
                            if (has_internet_access || counter_fast_ping > 5)
                            {
                                break;
                            }

                            Thread.Sleep(fast_ping_frequency);

                            counter_fast_ping = counter_fast_ping + 1;
                        }
                    }
                }
                finally
                {
                    pause_slow_ping = false;
                    counter_fast_ping = 0;

                    OnNotifyStatus(has_internet_access);
                }
            });
        }

        private static void StartSlowCheck()
        {
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    if (!pause_slow_ping)
                    {
                        if (HasNetworkAccess)
                        {
                            var result = await PingGoogle();

                            CheckNotifyStatusRequired(result, has_internet_access);
                        }
                        else
                        {
                            CheckNotifyStatusRequired(false, has_internet_access);
                        }
                    }

                    Thread.Sleep(slow_ping_frequency);
                }
            });
        }

        private static void OnNotifyStatus(bool newStatus)
        {
            if(newStatus)
            {
                OnRestored();
            }
            else
            {
                OnLost();
            }
        }


        private static void CheckNotifyStatusRequired(bool newStatus, bool previousStatus)
        {
            if(!newStatus && previousStatus)
            {
                //We update before notifying
                has_internet_access = newStatus;

                //If we don't have a connection now, but previously we did...
                OnLost();
            }
            else if(newStatus && !previousStatus)
            {
                //We update before notifying
                has_internet_access = newStatus;

                //If we have a connection now, but previously we didn't...
                OnRestored();
            }
            else
            {
                //Nothing to notify
                has_internet_access = newStatus;
            }
        }

        private static async Task<bool> PingGoogle()
        {
            bool result = false;
            try
            {
                var response = await google_http_client.GetAsync(google_host);

                if (response.IsSuccessStatusCode)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        /// <summary>
        /// Raised if internet connection has been restored
        /// </summary>
        private static void OnRestored()
        {
            Restored?.Invoke(nameof(InternetConnectivity), EventArgs.Empty);
        }

        private static void OnLost()
        {
            Lost?.Invoke(nameof(InternetConnectivity), EventArgs.Empty);
        }
    }
}
