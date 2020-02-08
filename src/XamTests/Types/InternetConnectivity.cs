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
        private static readonly string google_host = "https://www.google.es";
        private static bool? has_access = null;

        private static int counter_fast_ping = 5;
        private static readonly int fast_interval = 3000;
        private static readonly int slow_interval = 20000;

        private static bool pause_slow = false;

        public static event EventHandler Lost;
        public static event EventHandler Restored;

        static InternetConnectivity()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

            StartSlowCheck();
        }

        private static void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            Log.Information("Connection changed..");

            //We need to start checks again..
            has_access = null;

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
                //Either it doesn't have network access or it hasn't been calculated yet...
                if (!HasNetworkAccess || !has_access.HasValue)
                {
                    return false;
                }

                return has_access.Value;
            }
        }

        private static void StartFastCheck()
        {
            Log.Debug("Enters StartFastCheck");

            //No network access, no need to continue
            if (!HasNetworkAccess)
            {
                CompareNewToPreviousStatus(false, has_access);
                return;
            }

            Task.Factory.StartNew(async () =>
            {
                try
                {
                    pause_slow = true;

                    if (!has_access.HasValue || !has_access.Value)
                    {
                        //We need to check has_internet_access , just in case it was queried in the slow before disabling...
                        while (!has_access.HasValue || !has_access.Value)
                        {
                            has_access = await PingGoogle();

                            //If we already have access, no need to continue or if we have reached the max.. we'll leave it to the slow ping...
                            if (has_access.Value || counter_fast_ping > 5)
                            {
                                break;
                            }

                            Thread.Sleep(fast_interval);

                            counter_fast_ping++;
                        }
                    }
                }
                catch(Exception ex)
                {
                    Log.Error("Exits StartFastCheck");
                }
                finally
                {
                    pause_slow = false;
                    counter_fast_ping = 0;

                    CompareNewToPreviousStatus(has_access.HasValue ? has_access.Value : false, null);
                }
            });

            Log.Debug("Exits StartFastCheck");
        }

        private static void StartSlowCheck()
        {
            Log.Debug("Enters StartSlowCheck");

            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    if (!pause_slow)
                    {
                        if (HasNetworkAccess)
                        {
                            var result = await PingGoogle();
                            Log.Information("Pinged google: {0}",result);

                            CompareNewToPreviousStatus(result, has_access);
                        }
                        else
                        {
                            CompareNewToPreviousStatus(false, has_access);
                        }
                    }

                    Thread.Sleep(slow_interval);
                }
            });

            Log.Debug("Exits StartSlowCheck");
        }

        private static void CompareNewToPreviousStatus(bool newStatus, bool? previousStatus)
        {
            Log.Debug("Enters CompareNewToPreviousStatus");
            Log.Information("NewStatus: {0}, PreviousStatus: {1}",newStatus,previousStatus);

            //Used for comparisons
            var internal_newstatus = newStatus;

            //We set so HasInternetAccess returns correct value
            has_access = newStatus;

            if (!internal_newstatus && (!previousStatus.HasValue || previousStatus.Value))
            {
                //If we don't have a connection now, but previously we did...
                OnLost();
            }
            else if (internal_newstatus && (!previousStatus.HasValue || !previousStatus.Value))
            {
                //If we have a connection now, but previously we didn't...
                OnRestored();
            }

            Log.Debug("Exits CompareNewToPreviousStatus");
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
