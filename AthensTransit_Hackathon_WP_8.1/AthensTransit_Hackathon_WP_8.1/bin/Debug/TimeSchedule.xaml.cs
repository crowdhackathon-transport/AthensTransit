using AthensTransit_Hackathon.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class TimeSchedule : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private List<string> dailyListInASingleLine;
        private List<string> startPositions;
        private string currentNumberLine;
        private bool hasSchedule;
        public TextBlock tbHeader;

        public TimeSchedule()
        {
            this.InitializeComponent();
            //(Application.Current.Resources["progressRing"] as ProgressRing).IsActive = true;

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
            //ProggressBarVisible(true);
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
            //progressRing.IsActive = true;
            this.navigationHelper.OnNavigatedTo(e);

            if (Frame.BackStackDepth != 1)
            {
                do
                {
                    Frame.BackStack.RemoveAt(Frame.BackStackDepth - 1);
                } while (Frame.BackStackDepth != 1);
            }

            //currentNumberLine = e.Parameter.ToString();
            //myMethod(e.Parameter as IEnumerable<System.Xml.Linq.XElement>);
            myMethod();
        }

        private void myMethod()
        {
            currentNumberLine = (from b in App.activeBus
                                   select b.FirstAttribute.Value).ToList()[0]; 

            //var xd = XDocument.Load("all.xml");
            ////int childrenCount = xd.Root.Elements("Bus").Count();

            //var bus = from el in xd.Descendants("Bus")
            //                                            where el.Attribute("lineNumber").Value == navigationParameter
            //                                            select el;

            int childrenCount = App.activeBus.Descendants("Day").Count();
            //var distinctCount = bus.Descendants("Day").Attributes("dayName").Distinct().ToList();

            var days = (from item in App.activeBus.Elements("Day")
                        select item.Attribute("dayName")).Distinct().ToList();

            string myTest1 = "";
            string myTest2 = "";

            var startingPositions = (from item in App.activeBus.Elements("Day")
                                     select item.Attribute("startingPoint")).Distinct().ToList();

            var firstWay = from theFirstOne in App.activeBus.Elements("Stops")
                           where theFirstOne.Attribute("startingPoint").Value == "Αφετηρία"
                           select theFirstOne;

            var sthhh = firstWay.Descendants("Stop").Take(1);
            var sthhh2 = firstWay.Descendants("Stop").Reverse().Take(1);
            foreach (var item in sthhh)
            {
                myTest1 += item.Attribute("stopName").Value;
            }
            foreach (var item in sthhh2)
            {
                myTest2 += item.Attribute("stopName").Value;
            }

            var theStops = from item in App.activeBus.Descendants("Stops")
                             where item.Attribute("startingPoint").Value == "Κυκλική"
                             select item;


            dailyListInASingleLine = new List<string>();
            startPositions = new List<string>();

            var routes = from z in App.activeBus.Descendants("Day")
                         //where (z.Attribute("dayName").Value.Equals("Δευτέρα εως Παρασκευή") &&
                         //(z.Attribute("startingPoint").Value.Equals("Αφετηρία")))
                         select new
                         {
                             ElementContent = z.Value + "\n"
                         };

            //from z in bus.Descendants("Day")
            //         where (z.Attribute("dayName").Value.Equals(days[i]) &&
            //         (z.Attribute("startingPoint").Value.Equals(startingPositions[i])))
            //         select new
            //         {
            //             ElementContent = z.Value + "\n"
            //         };
            int myTempTest = 0;
            foreach (var item in startingPositions)
            {
                if (item.Value.Equals("Κυκλική"))
                {
                    for (int i = 0; i < childrenCount; i++)
                    {
                        startPositions.Add(item.Value);
                    }
                    changeStartPoint.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    break;
                }
                else
                {
                    if (myTempTest < childrenCount / 2)
                        startPositions.Add(myTest1);
                    else
                        startPositions.Add(myTest2);
                    myTempTest++;
                }
            }

            hasSchedule = true;
            if (startPositions.Count == 0)
            {
                hasSchedule = false;
                busNumber.Text = currentNumberLine;
                tbHeader = new TextBlock { Text = "Δεν βρέθηκε πρόγραμμα",
                                           Foreground = App.pivotHeaderColors[0],
                                           FontSize = 20
                };

                pivotDays.Items.Add(new PivotItem()
                {
                    Name = "PivotItem1",
                    Header = tbHeader,
                    Foreground = App.pivotHeaderColors[0]
                    //ContentTemplate = Resources["TestDataTemplate"] as DataTemplate
                });

                changeStartPoint.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                moveToStops.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

                return;
            }

            int aaaa = 0;
            foreach (var item in routes)
            {
                string datedate = item.ElementContent;
                dailyListInASingleLine.Add(datedate);
                //datedate = "";
                aaaa++;
            }

            //var hhhhh = days.GroupBy(x => x.Value).Where(x => x.Count() > 1);
            //int duplicates = hhhhh.Distinct().Count();

            busNumber.Text = currentNumberLine + " " + startPositions[0];
            List<string> dailyFinal;
            List<helpMeShow> helpMeShowItems;


            for (int i = 0; i < days.Count; i++)
            {
                dailyFinal = new List<string>();
                helpMeShowItems = new List<helpMeShow>();
                //string daily = dailyList[i];
                bool even = true;

                string currentBigHour;

                for (int j = 0; j < dailyListInASingleLine[i].Length - 1; j += 4)
                {
                    dailyFinal.Add(dailyListInASingleLine[i].Substring(j, 4));

                    //helpMeShowItems.Add(new helpMeShow(dailyFinal[dailyFinal.Count - 1], even));
                    //even = !even;
                }

                //int llll = dailyFinal.Count;

                //string s1, s2, stotal;

                //s1 = dailyFinal[0];
                //s2 = dailyFinal[1];
                //stotal = s1 + s2;

                bool train = false;

                if (currentNumberLine.Contains("m") || currentNumberLine.Contains("t"))
                {
                    var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
                    train = true;
                    helpMeShowItems.Add(new helpMeShow(loader.GetString("firstRoute"), "#bed62f", true, true));
                    helpMeShowItems.Add(new helpMeShow(dailyFinal[0], "White", false, true));
                    helpMeShowItems.Add(new helpMeShow("", "", true, true));
                    helpMeShowItems.Add(new helpMeShow(loader.GetString("lastRoute"), "#bed62f", false, true));
                    helpMeShowItems.Add(new helpMeShow(dailyFinal[1], "White", true, true));
                    helpMeShowItems.Add(new helpMeShow("", "", false, true));
                    helpMeShowItems.Add(new helpMeShow(loader.GetString("frequencyRoutes"), "#bed62f", true, true));

                    var currentDay = from b in App.activeBus.Elements("Day")
                                     where b.Attribute("dayName") == days[i]
                                     select b;

                    var routeFrequency = from b in currentDay.Descendants("Frequency")
                                         select b;

                    bool evensth = false;
                    foreach (var item in routeFrequency)
                    {
                        helpMeShowItems.Add(new helpMeShow(item.Attribute("startTime").Value + item.Attribute("endTime").Value +
                        item.Attribute("freq").Value, "White", evensth, true));
                        evensth = !evensth;
                    }

                }
                else
                {
                    int k = 0;
                    string smallHours = "";
                    currentBigHour = dailyFinal[k].Substring(0, 2);
                    while (k < dailyFinal.Count)
                    {

                        if (dailyFinal[k].Substring(0, 2).Equals(currentBigHour.Substring(0, 2)))
                        {
                            smallHours += dailyFinal[k].Substring(2, 2) + " ";
                            k++;
                        }
                        else
                        {
                            helpMeShowItems.Add(new helpMeShow(currentBigHour, smallHours, even));
                            even = !even;

                            currentBigHour = dailyFinal[k].Substring(0, 2);
                            smallHours = dailyFinal[k].Substring(2, 2) + " ";
                            k++;
                        }
                    }
                    helpMeShowItems.Add(new helpMeShow(currentBigHour, smallHours, even));
                }

                tbHeader = new TextBlock { Text = days[i].Value,
                                           Foreground = App.pivotHeaderColors[1],
                                           FontSize = 25
                };

                SolidColorBrush scb = new SolidColorBrush(Colors.Transparent);

                ListBox listBox = new ListBox();
                listBox.Background = scb;
                //listBox.IsTextScaleFactorEnabled = true;
                listBox.ItemContainerStyle = Resources["styleOfListBoxSO"] as Style;
                //listBox.HorizontalContentAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
                listBox.ItemsSource = helpMeShowItems;

                if (!train)
                    listBox.ItemTemplate = Resources["colorsFinalDataTemplate"] as DataTemplate;
                else
                    listBox.ItemTemplate = Resources["colorsTrainDataTemplate"] as DataTemplate;

                pivotDays.Items.Add(new PivotItem()
                {
                    Name = "PivotItem" + (i + 1),
                    Header = tbHeader,
                    Content = listBox
                    
                });

            }

            pivotDays.SelectedIndex = IndexOfCurrentDate();
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            pivotDays.Items.Clear();
            dailyListInASingleLine.Clear();
            startPositions.Clear();
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void pivotDays_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (hasSchedule)
            {
                int selectedIndex = pivotDays.SelectedIndex;
                int whitespacePosition = busNumber.Text.IndexOf(" ");
                //pivotDays.SelectedIndex = selectedIndex;
                //(sender as Pivot).SelectedIndex = selectedIndex;

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

                //PivotItem pivotTemp = (PivotItem)pivotDays.SelectedItem;
                //pivotTemp.Header = new SolidColorBrush(Colors.White);

                busNumber.Text = App.ChangeEnglishLettersToGreek(busNumber.Text.Substring(0, whitespacePosition));
                busNumber.Text += " - " + startPositions[selectedIndex];
            }
            //titleScrollViewer.ScrollToHorizontalOffset(0);
        }

        private int IndexOfCurrentDate()
        {
            if (pivotDays.Items.Count < 2)
                return 0;

            string nameOfTheDay = string.Empty;
            int currNumOfTheDay = (int)System.DateTime.Now.DayOfWeek;
            List<int> numbersOfTheDays = new List<int>();
            int selectedIndex = 0;

            for (int i =0; i<pivotDays.Items.Count; i++)
            {
                nameOfTheDay = ((pivotDays.Items[i] as PivotItem).Header as TextBlock).Text;

                #region Name of the Day

                switch (nameOfTheDay)
                {
                    case "Δευτέρα εως Παρασκευή":
                        numbersOfTheDays.Add(1);
                        numbersOfTheDays.Add(2);
                        numbersOfTheDays.Add(3);
                        numbersOfTheDays.Add(4);
                        numbersOfTheDays.Add(5);
                        break;
                    case "Σάββατο":
                        numbersOfTheDays.Add(6);
                        break;
                    case "Κυριακή":
                        numbersOfTheDays.Add(0);
                        break;
                    case "Κυριακή και Αργίες":
                        numbersOfTheDays.Add(0);
                        break;
                    case "Παρασκευή":
                        numbersOfTheDays.Add(5);
                        break;
                    case "Δευτέρα εως Τετάρτη":
                        numbersOfTheDays.Add(1);
                        numbersOfTheDays.Add(3);
                        break;
                    case "Τρίτη - Πέμπτη - Παρασκευή":
                        numbersOfTheDays.Add(2);
                        numbersOfTheDays.Add(4);
                        numbersOfTheDays.Add(5);
                        break;
                    case "Δευτέρα εως Σάββατο":
                        numbersOfTheDays.Add(1);
                        numbersOfTheDays.Add(2);
                        numbersOfTheDays.Add(3);
                        numbersOfTheDays.Add(4);
                        numbersOfTheDays.Add(5);
                        numbersOfTheDays.Add(6);
                        break;
                    case "7 Ημέρες":
                        numbersOfTheDays.Add(0);
                        numbersOfTheDays.Add(1);
                        numbersOfTheDays.Add(2);
                        numbersOfTheDays.Add(3);
                        numbersOfTheDays.Add(4);
                        numbersOfTheDays.Add(5);
                        numbersOfTheDays.Add(6);
                        break;
                    case "Σάββατο εως Κυριακή":
                        numbersOfTheDays.Add(6);
                        numbersOfTheDays.Add(0);
                        break;
                    case "Σάββατο εως Κυριακή και Αργίες":
                        numbersOfTheDays.Add(6);
                        numbersOfTheDays.Add(0);
                        break;
                    default:
                        break;
                }

                #endregion

                if (numbersOfTheDays.Exists(element => element == currNumOfTheDay))
                {
                    selectedIndex = i;
                    break;
                }
                else
                {
                    numbersOfTheDays.Clear();
                }

            }
            return selectedIndex;
        }

        //private string ChangeEnglishLettersToGreek(string temp)
        //{
        //    if (temp.Contains("a"))
        //    {
        //        temp = temp.Replace("a", "Α");
        //    }
        //    else if (temp.Contains("b"))
        //    {
        //        temp = temp.Replace("b", "Β");
        //    }
        //    else if (temp.Contains("c"))
        //    {
        //        temp = temp.Replace("c", "Γ");
        //    }
        //    else if (temp.Contains("e"))
        //    {
        //        temp = temp.Replace("e", "Ε");
        //    }
        //    else if (temp.Contains("x"))
        //    {
        //        temp = temp.Replace("x", "Χ");
        //    }
        //    return temp;
        //}

        private void moveToStops_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Stops), currentNumberLine);
        }

        private void changeStartPoint_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if ((startPositions.Count % 2 == 0) && (startPositions.Count != 0))
            {
                //pivotDays.SelectedIndex = (pivotDays.SelectedIndex + startPositions.Count / 2) % startPositions.Count;
                pivotDays.SelectedItem = pivotDays.Items[(pivotDays.SelectedIndex + startPositions.Count / 2) % startPositions.Count];
            }
        }
    }
}
