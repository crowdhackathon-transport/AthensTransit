using AthensTransit_Hackathon.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI;
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
    public sealed partial class Stops : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public Stops()
        {
            this.InitializeComponent();

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
            //Dispatcher.Invoke(new Action(() => { }), DispatcherPriority.ContextIdle, null);
           
            string currentBusNumber = (from b in App.activeBus
                                   select b.FirstAttribute.Value).ToList()[0];
            string stopName = "";
            
            SolidColorBrush scb = new SolidColorBrush(Colors.Transparent);
            TextBlock tbHeader;

            ListBox listBox1 = new ListBox();
            listBox1.Background = scb;
            listBox1.ItemTemplate = Resources["stopsRepresentation"] as DataTemplate;
            listBox1.ItemContainerStyle = Resources["styleOfListBoxSO"] as Style;
            //listBox1.Tapped += listBox1_Tapped;

            //var xd = XDocument.Load("all.xml");
            //var bus = from el in xd.Descendants("Bus")
            //          where el.Attribute("lineNumber").Value == currentBusNumber
            //          select el;


            int stopsCount = App.activeBus.Descendants("Stops").Count();



            var listFromStart = App.activeBus.Descendants("Stops").Take(1).Elements("Stop").ToList();
            List<Stop> myList1 = new List<Stop>();

            for (int i = 0; i < listFromStart.Count; i++)
            {
                stopName = listFromStart[i].Attribute("stopName").Value;

                var aaaaa = listFromStart[i].Elements("C").ToList();
                List<string> tempList = new List<string>();

                foreach (var innerStop in aaaaa)
                {
                    tempList.Add(innerStop.Value);
                }

                if (i==0)
                    myList1.Add(new Stop(stopName, tempList, tempList.Count, -1));
                else if (i == listFromStart.Count - 1)
                    myList1.Add(new Stop(stopName, tempList, tempList.Count, 1));
                else
                    myList1.Add(new Stop(stopName, tempList, tempList.Count, 0));
                //myList1.Add(new Stop(stopName, tempList, tempList.Count));
            }


            listBox1.ItemsSource = myList1;
            listBox1.Tapped += listBox1_Tapped;
            currentBusNumber = App.ChangeEnglishLettersToGreek(currentBusNumber);
            if (stopsCount == 1)
            {
                tbHeader = new TextBlock { Text = "Κυκλική \"" + currentBusNumber + "\"", FontSize = 28 };
            }
            else
            {
                tbHeader = new TextBlock { Text = "Αφετηρία \"" + currentBusNumber + "\"", FontSize = 28 };
            }
            pivotStops.Items.Add(new PivotItem()
            {
                Name = "PivotItem1",
                Header = tbHeader,
                Content = listBox1
            });

            if (stopsCount == 2)
            {
                //currentBusNumber = (string)e.Parameter;
                ListBox listBox2 = new ListBox();
                listBox2.Background = scb;
                listBox2.ItemTemplate = Resources["stopsRepresentation"] as DataTemplate;
                listBox2.ItemContainerStyle = Resources["styleOfListBoxSO"] as Style;

                var listFromFinal = App.activeBus.Descendants("Stops").Reverse().Take(1).Elements("Stop").ToList();

                List<Stop> myList2 = new List<Stop>();

                for (int i = 0; i < listFromFinal.Count; i++ )
                {
                    stopName = listFromFinal[i].Attribute("stopName").Value;

                    var aaaaa = listFromFinal[i].Elements("C").ToList();
                    List<string> tempList = new List<string>();

                    foreach (var innerStop in aaaaa)
                    {
                        tempList.Add(innerStop.Value);
                    }

                    //myList2.Add(new Stop(stopName, tempList, tempList.Count, 0));

                    if (i == 0)
                        myList2.Add(new Stop(stopName, tempList, tempList.Count, -1));
                    else if (i == listFromFinal.Count - 1)
                        myList2.Add(new Stop(stopName, tempList, tempList.Count, 1));
                    else
                        myList2.Add(new Stop(stopName, tempList, tempList.Count, 0));
                }
                listBox2.ItemsSource = myList2;
                listBox2.Tapped += listBox1_Tapped;
                currentBusNumber = App.ChangeEnglishLettersToGreek(currentBusNumber);

                tbHeader = new TextBlock { Text = "Τέρμα \"" + currentBusNumber + "\"", FontSize = 28 };
                pivotStops.Items.Add(new PivotItem()
                {
                    Name = "PivotItem2",
                    Header = tbHeader,
                    Content = listBox2
                });
            }
            currentBusNumber = "";
        }

        private void listBox1_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //int item = (sender as ListBox).Items.Count;
            //int item2 = (sender as ListBox).SelectedIndex;
            var lbitem = (sender as ListBox).SelectedItem as Stop;

            if (lbitem != null)
                Frame.Navigate(typeof(innerStops), lbitem);
            //var sttt = (e.OriginalSource as String);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void pivotStops_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = (sender as Pivot).SelectedIndex;

            if (e.AddedItems.Count > 0)
            {
                PivotItem currentItem = e.AddedItems[0] as PivotItem;

                if (currentItem != null)
                {
                    (currentItem.Header as TextBlock).Foreground = App.pivotHeaderColors[0];
                }
            }

            if (e.RemovedItems.Count > 0)
            {
                PivotItem currentItem = e.RemovedItems[0] as PivotItem;

                if (currentItem != null)
                {
                    (currentItem.Header as TextBlock).Foreground = App.pivotHeaderColors[1];
                }
            }
        }

    }
}
