using AthensTransit_Hackathon.Common;
//using AthensTransit_Hackathon_WP_8._1.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace AthensTransit_Hackathon_WP_8._1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapBus : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public MapBus()
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
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void searchImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!searchBox.Text.Equals(string.Empty) && searchBox.Text.Count() <= 4)
            {
                XDocument loaded = XDocument.Load("allStops.txt");
                var stops = (from el in loaded.Elements("Buses").Descendants("Stop")
                             where (string)el.Attribute("Line") == searchBox.Text && (string)el.Attribute("Dir") == "1"
                             select el.Attribute("StopID").Value).ToList();
                
                Stream stream;

                string fileContent;
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///stops.txt"));
                using (StreamReader sRead = new StreamReader(await file.OpenStreamForReadAsync()))
                {
                    fileContent = await sRead.ReadToEndAsync();
                    string[] stopsGeoData = fileContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                    MapIcon mapIcon = new MapIcon();
                    foreach (string line in stopsGeoData)
                    {
                        if (stops.Contains(line.Split(',')[0]))
                        {
                            string lat = line.Split(',')[4];
                            string lon = line.Split(',')[5];

                            mapIcon = new MapIcon();
                            // Locate your MapIcon  
                            mapIcon.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/AppIcons/ATH_icons_bus_grey.png"));
                            // Show above the MapIcon  
                            mapIcon.Title = line.Split(',')[2];
                            // Setting up MapIcon location  
                            mapIcon.Location = new Geopoint(new BasicGeoposition()
                            {
                                Latitude = Convert.ToDouble(lat),
                                Longitude = Convert.ToDouble(lon)
                            });
                            // Positon of the MapIcon  
                            mapIcon.NormalizedAnchorPoint = new Point(0.5, 0.5);
                            GeoData.MapElements.Add(mapIcon);  
                        }
                    }

                    await GeoData.TrySetViewAsync(mapIcon.Location, 15d, 0, 0, MapAnimationKind.Bow);
                }
            }
        }
    }
}
