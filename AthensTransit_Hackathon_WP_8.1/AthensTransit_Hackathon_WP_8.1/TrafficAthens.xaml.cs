using AthensTransit_Hackathon.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web;
using System.Net.Http;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using System.Text;
using Windows.UI;
using Windows.Networking.Connectivity;
using Windows.UI.Core;
using Windows.UI.Popups;


namespace AthensTransit_Hackathon
{
    public sealed partial class TrafficAthens : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        //private string url = new Uri(base.BaseUri, "http://www.astynomia.gr/traffic-athens.php");

        private string url = "http://www.astynomia.gr/traffic-athens.php";

        private double startingHeight = 0;
        private double startigWidth;
        private double lastKnownWidth;
        
        private List<Road> feedList = new List<Road>();

        private bool listReduced = false;
        private bool offlineMode = false;
        private bool firstTimeVisibility;
        private bool refreshButtonAvailable;
        private bool autoRefreshAvailable;
        private bool firstTimeWidthHeightCalibration;

        private DispatcherTimer timer = new DispatcherTimer();
        private DispatcherTimer refreshTimer = new DispatcherTimer();
        private DispatcherTimer autoRefreshTimer = new DispatcherTimer();

        private ConnectionProfile InternetConnectionProfile;
        
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public TrafficAthens()
        {
            this.InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            
            listReduced = false;
            refreshButtonAvailable = true;
            firstTimeVisibility = true;
            autoRefreshAvailable = true;
            firstTimeWidthHeightCalibration = true;

            try
            {
                startingHeight = Window.Current.Bounds.Height - 140;
                StreetList.Height = startingHeight;
            }
            catch (Exception)
            {
                Debug.WriteLine("Exception caught in TrafficAthens()");
            }
        }

        
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            Debug.WriteLine("klisi on navigatedTo");

            base.OnNavigatedTo(e);

            firstTimeVisibility = true;

            await parseHtml(url);

            StreetList.ItemsSource = null;
            StreetList.ItemsSource = feedList;

           // EncodingInfo ei = Encoding.GetEncodings();
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            refreshFeedList();
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (!progress.IsActive && refreshButtonAvailable)
            {
                try
                {
                    refreshTimer.Interval = TimeSpan.FromSeconds(10);
                    refreshTimer.Tick += refreshTick;
                    refreshTimer.Start();
                    refreshButtonAvailable = false;
                    feedList.Clear(); //edw to refresh ginetai me mpinia kanonika 8a eprepe na kalei tin parse alla douleuei
                }
                catch (Exception)
                {
                }
            }
        }

        private async Task<List<Road>> parseHtml(String Url)
        {
            string page = "";
            string[] seperator = new string[] { "<tr" };
            string[] tdsep = new string[] { "<td" };
            string[] trSep;
            string[] cropped;

            feedList.Clear();//1.0.1.1

            try
            {
                progress.IsActive = true;

                //Encoding iso = Encoding.GetEncoding("ISO-8859-7");
                //Encoding utf8 = Encoding.UTF8;
                
                //byte[] utfBytes = utf8.GetBytes(Url);
                //byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);
                //string msg = iso.GetString(isoBytes,0,isoBytes.Length);


                HttpClient http = new HttpClient();
                HttpResponseMessage response = await http.GetAsync(url);
                //page = await response.Content.ReadAsStringAsync();
                byte[] isoBytes = await http.GetByteArrayAsync(url);
                //Encoding utf8 = Encoding.Unicode;
                //System.Text.Encoding greek = ;
                //greek.WebName = "iso-8859-7";
                //Encoding greekEnc = Encoding.GetEncoding(;
                //Encoding unicode = Encoding.GetEncoding("Windows-1252");
                //UnicodeEncoding greek = new UnicodeEncoding();
                //System.Text.
                Encoding unicode = Encoding.UTF8;
                //byte[] utfBytes = utf8.GetBytes(Url);
                //Encoding init = new Windows1250Encoding();
                //isoBytes = Encoding.Convert(utf8, greekEnc,isoBytes);
                page = unicode.GetString(isoBytes, 0, isoBytes.Length);

                //foreach (EncodingInfo ei in Encoding.GetEncodings())
                //{
                //    Encoding e = ei.GetEncoding();

                //    Console.Write("{0,-6} {1,-25} ", ei.CodePage, ei.Name);
                //    Console.Write("{0,-8} {1,-8} ", e.IsBrowserDisplay, e.IsBrowserSave);
                //    Console.Write("{0,-8} {1,-8} ", e.IsMailNewsDisplay, e.IsMailNewsSave);
                //    Console.WriteLine("{0,-8} {1,-8} ", e.IsSingleByte, e.IsReadOnly);
                //}
                progress.IsActive = false;
                offlineMode = false;


                try
                {
                    Stream stream;
                    StreamWriter writer;
                        stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync("offlineAthens.txt", CreationCollisionOption.ReplaceExisting);
                    stream.Flush();
                    writer = new StreamWriter(stream);

                    await writer.WriteLineAsync(page);
                    writer.Flush();

                    stream.Dispose();
                }
                catch (Exception)
                {
                    Debug.WriteLine("-----------------------------------------------------------");
                }
            }
            catch (HttpRequestException)
            {
                progress.IsActive = false;
            }

            if (page.Length == 0)
            {
                Stream stream;
                StreamReader reader;

                try
                {
                    stream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("offlineAthens.txt");//------------peirazw debugging
                    reader = new StreamReader(stream);
                    page = reader.ReadToEnd();
                    offlineMode = true;
                }
                catch (FileNotFoundException)//********periptwsi pou anoigei xwris internet den vriskei to offline.txt*********
                {
                    Road error = new Road();
                    error.labeling = "Παρουσιάστηκε σφάλμα στην φόρτωση των δεδομένων παρακαλώ βεβαιωθείτε ότι έχετε ενεργοποιήσει την κίνηση δεδομένων";
                    feedList.Add(error);
                    return feedList;
                }
            }

            trSep = page.Split(seperator, StringSplitOptions.RemoveEmptyEntries);
            //---------------------------------------------------------------------------------------------pairnei tin teleutaia enimerwsi
            try
            {
                trSep[0] = trSep[0].Split(new string[] { "<b>", "</b>" }, StringSplitOptions.None)[5];
                foreach (char c in trSep[0])
                {
                    if ((c >= '0' && c <= '9') || c == '/' || c == '-' || c == ' ' || c == ':')
                        continue;
                    else
                        throw new IndexOutOfRangeException();
                }
            }
            catch (IndexOutOfRangeException)
            {
                trSep[0] = "Αγνωστο";
            }

            //Debug.WriteLine("Τελευταία ενημέρωση: " + trSep[0]);

            showDate(trSep[0]);

            //---------------------------------------------------------------------------------------------end teleutaia enimerwsi

            for (int i = 3; i < trSep.Length; i++)// mama for gia olous tous dromous
            {

                Road road = new Road();

                cropped = trSep[i].Split(tdsep, StringSplitOptions.RemoveEmptyEntries);


                cropped[1] = cropped[1].Split(new string[] { "<", ">" }, StringSplitOptions.RemoveEmptyEntries)[1];//pairnei to dromo

                if (!cropped[1].Contains("/td") && cropped[1].Trim().Length > 3)//proste8ike ekdosi 1.0.1.1
                {
                    road.name = cropped[1];
                }
                else
                {
                    continue;
                }


                //---------------------------------------------------------------------------------------------status dromou
                if (cropped[2].Length > cropped[3].Length && cropped[2].Length > cropped[4].Length)
                {
                    road.status = "Κανονική ροή";
                }
                else if (cropped[3].Length > cropped[4].Length)
                {
                    road.status = "Αυξημένη ροή";
                }
                else if (cropped[2].Length == cropped[3].Length && cropped[3].Length == cropped[4].Length)
                {
                    road.status = "";
                }
                else
                {
                    road.status = "Πολύ αυξημένη ροή";
                }
                //---------------------------------------------------------------------------------------------end status

                //---------------------------------------------------------------------------------------------------------------------labeling dromou
                cropped[5] = cropped[5].Split(new string[] { "<", ">" }, StringSplitOptions.None)[1];

                if (cropped[5].Trim().Length == 0)
                {
                    road.labeling = "";//"Δεν υπάρχουν διαθέσιμες πληροφορίες για τον δρόμο"; //1.0.1.1
                }
                else
                {
                    road.labeling = cropped[5];
                }

                //-----------------------------------------------------------------------------------------------------------------------end labeling


                //*****debugging*****
                //road.name = "ΚΛΕΙΣΤΗ Η ΛΕΩΦ. ΔΕΚΕΛΕΙΑΣ ΣΤΗ Ν.ΦΙΛΑΔΕΛΦΕΙΑ, ΛΟΓΩ ΕΟΡΤΑΣΜΟΥ ΤΗΣ ΠΡΩΤΟΜΑΓΙΑΣ ΑΠΌ ΤΟ ΥΨΟΣ ΤΗΣ ΧΑΛΚΙΔΟΣ ΕΩΣ ΕΛ.ΒΕΝΙΖΕΛΟΥ (ΦΑΝΑΡΙΑ ΤΟΥ ΒΛΑΧΟΥ) ΚΑΙ Η ΧΑΛΚΙΔΟΣ ΑΠΌ ΠΑΤΗΣΙΩΝ ΕΩΣ ΔΕΚΕΛΕΙΑΣ." +
                //    "ΚΛΕΙΣΤΗ Η ΛΕΩΦ. ΔΕΚΕΛΕΙΑΣ ΣΤΗ Ν.ΦΙΛΑΔΕΛΦΕΙΑ, ΛΟΓΩ ΕΟΡΤΑΣΜΟΥ ΤΗΣ ΠΡΩΤΟΜΑΓΙΑΣ ΑΠΌ ΤΟ ΥΨΟΣ ΤΗΣ ΧΑΛΚΙΔΟΣ ΕΩΣ ΕΛ.ΒΕΝΙΖΕΛΟΥ (ΦΑΝΑΡΙΑ ΤΟΥ ΒΛΑΧΟΥ) ΚΑΙ Η ΧΑΛΚΙΔΟΣ ΑΠΌ ΠΑΤΗΣΙΩΝ ΕΩΣ ΔΕΚΕΛΕΙΑΣ.";
                //road.status = "ΚΛΕΙΣΤΗ Η ΛΕΩΦ. ΔΕΚΕΛΕΙΑΣ ΣΤΗ Ν.ΦΙΛΑΔΕΛΦΕΙΑ, ΛΟΓΩ ΕΟΡΤΑΣΜΟΥ ΤΗΣ ΠΡΩΤΟΜΑΓΙΑΣ ΑΠΌ ΤΟ ΥΨΟΣ ΤΗΣ ΧΑΛΚΙΔΟΣ ΕΩΣ ΕΛ.ΒΕΝΙΖΕΛΟΥ (ΦΑΝΑΡΙΑ ΤΟΥ ΒΛΑΧΟΥ) ΚΑΙ Η ΧΑΛΚΙΔΟΣ ΑΠΌ ΠΑΤΗΣΙΩΝ ΕΩΣ ΔΕΚΕΛΕΙΑΣ." +
                //    "ΚΛΕΙΣΤΗ Η ΛΕΩΦ. ΔΕΚΕΛΕΙΑΣ ΣΤΗ Ν.ΦΙΛΑΔΕΛΦΕΙΑ, ΛΟΓΩ ΕΟΡΤΑΣΜΟΥ ΤΗΣ ΠΡΩΤΟΜΑΓΙΑΣ ΑΠΌ ΤΟ ΥΨΟΣ ΤΗΣ ΧΑΛΚΙΔΟΣ ΕΩΣ ΕΛ.ΒΕΝΙΖΕΛΟΥ (ΦΑΝΑΡΙΑ ΤΟΥ ΒΛΑΧΟΥ) ΚΑΙ Η ΧΑΛΚΙΔΟΣ ΑΠΌ ΠΑΤΗΣΙΩΝ ΕΩΣ ΔΕΚΕΛΕΙΑΣ.";
                //road.labeling = "ΚΛΕΙΣΤΗ Η ΛΕΩΦ. ΔΕΚΕΛΕΙΑΣ ΣΤΗ Ν.ΦΙΛΑΔΕΛΦΕΙΑ, ΛΟΓΩ ΕΟΡΤΑΣΜΟΥ ΤΗΣ ΠΡΩΤΟΜΑΓΙΑΣ ΑΠΌ ΤΟ ΥΨΟΣ ΤΗΣ ΧΑΛΚΙΔΟΣ ΕΩΣ ΕΛ.ΒΕΝΙΖΕΛΟΥ (ΦΑΝΑΡΙΑ ΤΟΥ ΒΛΑΧΟΥ) ΚΑΙ Η ΧΑΛΚΙΔΟΣ ΑΠΌ ΠΑΤΗΣΙΩΝ ΕΩΣ ΔΕΚΕΛΕΙΑΣ." +
                //    "ΚΛΕΙΣΤΗ Η ΛΕΩΦ. ΔΕΚΕΛΕΙΑΣ ΣΤΗ Ν.ΦΙΛΑΔΕΛΦΕΙΑ, ΛΟΓΩ ΕΟΡΤΑΣΜΟΥ ΤΗΣ ΠΡΩΤΟΜΑΓΙΑΣ ΑΠΌ ΤΟ ΥΨΟΣ ΤΗΣ ΧΑΛΚΙΔΟΣ ΕΩΣ ΕΛ.ΒΕΝΙΖΕΛΟΥ (ΦΑΝΑΡΙΑ ΤΟΥ ΒΛΑΧΟΥ) ΚΑΙ Η ΧΑΛΚΙΔΟΣ ΑΠΌ ΠΑΤΗΣΙΩΝ ΕΩΣ ΔΕΚΕΛΕΙΑΣ.";
                //*****end debugging*****

                //if (road.status.Length == 0 || road.status.Equals("Πολύ αυξημένη ροή") || road.status.Equals("Αυξημένη ροή") || road.labeling.Length != 0)//1.0.1.1
                    feedList.Add(road);//fortwnei tin lista pou periexei mono au3imeni kinisi
            }

            if (feedList.Count == 0)
            {
                Road r = new Road();
                r.name = "Όλοι οι οδικοί άξονες παρουσιάζουν ομαλή ροή οχημάτων";
                feedList.Add(r);
            }

            if (feedList.Count == 0)//1.0.1.1
            {
                Road r = new Road();
                r.name = "Παρουσιάστηκε κάποιο πρόβλημα με την φόρτωση των δεδομένων παρακαλώ προσπαθήστε ξανά σε λίγο";
                feedList.Add(r);
            }

            return feedList;
        }


        private async void refreshFeedList()
        {//-------------------------------------------------------------------refresh dedomenwn
            feedList.Clear();
            List<Road> list = await parseHtml(url);

            StreetList.ItemsSource = null;

            StreetList.ItemsSource = feedList;

            if (feedList.Count == 0)//fix tou bug xamilis taxititas
            {
                await parseHtml(url);
                StreetList.ItemsSource = null;
                StreetList.ItemsSource = feedList;
            }

            InternetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();

            if (offlineMode && InternetConnectionProfile != null && InternetConnectionProfile.GetNetworkConnectivityLevel() ==
                NetworkConnectivityLevel.InternetAccess)
            {
                await parseHtml(url);

                if (!offlineMode)
                {
                    StreetList.ItemsSource = null;
                    StreetList.ItemsSource = feedList;
                }
            }
        }//------------------------------------------------------------------------------------------------------------------

        private void showDate(String str)// 14/04/2014 - 19:13*************passed
        {
            try
            {
                string[] date;
                str = str.Trim();
                date = str.Split(new string[] { "/", "-" }, StringSplitOptions.RemoveEmptyEntries);

                if (date[0].Length == 2 && date[0].StartsWith("0"))
                    date[0] = date[0].Substring(1, 1);

                if (date[1].Length == 2 && date[1].StartsWith("0"))
                    date[1] = date[1].Substring(1, 1);

                if (Convert.ToInt32(date[0]) == DateTime.Today.Day)
                {
                    date[0] = "Σήμερα";
                    date[1] = "";
                    date[2] = "";
                }
                else if (Convert.ToInt32(date[0]) == DateTime.Today.AddDays(-1).Day)//&& DateTime.Today.Day - 1 > 0)
                {
                    date[0] = "Εχθές";
                    date[1] = "";
                    date[2] = "";
                }

                if (date[1].Length != 0)
                    DateTB.Text = "Τελευταία ενημέρωση \nαπό Ελληνική Αστυνομία: " + date[0] + "/" + date[1] + "/" + date[2];// +"  Ώρα: " + date[3];
                else
                    DateTB.Text = "Τελευταία ενημέρωση \nαπό Ελληνική Αστυνομία: " + date[0]; //+"  Ώρα: " + date[3];

                if (offlineMode)
                {
                    DateTB.Text = DateTB.Text + "  Offline mode";
                    DateTB.Foreground = new SolidColorBrush(Colors.Red);
                }
                else
                    DateTB.Foreground = new SolidColorBrush(Colors.White);

                TimeTB.Text = "Ώρα τελευταίας ενημέρωσης: " + date[3];
            }
            catch (IndexOutOfRangeException ex)
            {
                Debug.WriteLine(ex.StackTrace);
            }

        }

        void timer_Tick(object sender, object e)
        {
            CurrentTimeTB.Text = "Τρέχουσα ώρα: " + DateTime.Now.ToString("HH:mm:ss");
        }

        private void refreshTick(object sender, object e)
        {
            refreshButtonAvailable = true;

            refreshButton.IsEnabled = true;

            refreshTimer.Stop();
        }

        private void infoIcon_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
