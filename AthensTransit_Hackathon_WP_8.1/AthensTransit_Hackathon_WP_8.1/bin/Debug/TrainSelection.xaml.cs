using AthensTransit_Hackathon.Common;
using AthensTransit_Hackathon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace AthensTransit_Hackathon
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class trainSelection : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /* phase description
         * 0: entry level
         * 1: metro choosed
         * 2: tram choosed 
         */
        private int phaseNumber;
        public trainSelection()
        {
            this.InitializeComponent();
            //Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState; 

        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);

            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            trainLine.Text = loader.GetString("chooseTrain");

            hsap.Content = loader.GetString("m1");
            metro.Content = loader.GetString("metro");
            tram.Content = loader.GetString("tram");

            //phaseNumber = 0;
            //metro1.Content = "ΗΣΑΠ (Γραμμή 1)";
            //metro2.Content = "Γραμμή 2";
            //metro3.Content = "Γραμμή 3";

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
        private void hsap_Click(object sender, RoutedEventArgs e)
        {
            //var xd = XDocument.Load("trains.xml");
            //IEnumerable<System.Xml.Linq.XElement> bus = from el in xd.Descendants("Bus")
            //                                                where el.Attribute("lineNumber").Value == "m1"
            //                                                select el;

            App.LoadBus("m1");

            Frame.Navigate(typeof(TimeSchedule));
        }

        private void metro_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(metroSelection));
        }

        private void tram_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(tramSelection));
        }
    //    private void firstOld_Click(object sender, RoutedEventArgs e)
    //    {
    //        if (phaseNumber == 0)
    //        {
    //            if ((sender as Button).Name.Equals("first"))
    //            {
    //                //navigate to hsap
    //            }
    //            else if ((sender as Button).Name.Equals("second"))
    //            {
    //                //move to metro page 
    //                switchToGrid(1);
    //            }
    //            else if ((sender as Button).Name.Equals("third"))
    //            {
    //                //move to tram page 
    //                switchToGrid(2);
    //            }
    //        }
    //        else if (phaseNumber == 1)
    //        {
    //            if ((sender as Button).Name.Equals("first"))
    //            {
    //                //navigate to m2
    //            }
    //            else if ((sender as Button).Name.Equals("second"))
    //            {
    //                //navigate to m3
    //            }
    //            else if ((sender as Button).Name.Equals("third"))
    //            {
    //                //navigate to m3 with airport schedule
    //            }
    //        }
    //        else if (phaseNumber == 2)
    //        {
    //            if ((sender as Button).Name.Equals("first"))
    //            {
    //                //navigate to t3
    //            }
    //            else if ((sender as Button).Name.Equals("second"))
    //            {
    //                //navigate to t4
    //            }
    //            else if ((sender as Button).Name.Equals("third"))
    //            {
    //                //navigate to t5
    //            }
    //        }
    //    }

    //    private void switchToGrid(int phase)
    //    {
    //        //innerGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
    //        phaseNumber = phase;

    //        switch (phase)
    //        {
    //            case 0:

    //                trainLine.Text = "ΕΠΙΛΟΓΗ ΜΕΣΟΥ";
    //                first.Content = "ΗΣΑΠ";
    //                second.Content = "ΜΕΤΡΟ";
    //                third.Content = "ΤΡΑΜ";

    //                break;

    //            case 1:

    //                trainLine.Text = "ΕΠΙΛΟΓΗ ΓΡΑΜΜΗΣ";
    //                first.Content = "Μ2";
    //                second.Content = "Μ3";
    //                third.Content = "Μ3_ΑΕΡΟΔΡΟΜΙΟ";

    //                break;

    //            case 2:

    //                trainLine.Text = "ΕΠΙΛΟΓΗ ΓΡΑΜΜΗΣ";
    //                first.Content = "Τ3";
    //                second.Content = "Τ4";
    //                third.Content = "Τ5";

    //                break;
    //        }
    //        //innerGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
    //    }

    //    private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
    //    {
    //        switch (phaseNumber)
    //        {
    //            case 1:
    //            case 2:
    //                switchToGrid(0);
    //                break;
    //            case 0:
    //                if (Frame.CanGoBack)
    //                {
    //                    e.Handled = true;
    //                    Frame.GoBack();
    //                }
    //                //Frame.Navigate(typeof(MainPage));
    //                break;
    //        }
    //    }
    }
}
