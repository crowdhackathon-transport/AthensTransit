using AthensTransit_Hackathon.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class TouristMode : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public TouristMode()
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
            LoadAirportLinesToPivot();
            
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        private void LoadAirportLinesToPivot()
        {
            SolidColorBrush scb = new SolidColorBrush(Colors.Transparent);
            ListBox listBoxBuses = new ListBox();
            ListBox listBoxMetro = new ListBox();
            listBoxBuses.Background = listBoxMetro.Background = scb;
            listBoxBuses.ItemTemplate = listBoxMetro.ItemTemplate = Resources["LinesRepresentation"] as DataTemplate;
            //listBoxBuses.ItemContainerStyle = Style.TargetType.get
            
            listBoxBuses.ItemsSource = new List<LinesRepresentation>() { 
                new LinesRepresentation("X93", "ΣΤ. ΥΠΕΡ. ΛΕΩΦ. ΚΗΦΙΣΟΥ - ΑΕΡ/ΝΑΣ ΑΘΗΝΩΝ (EXPRESS)", false),
                new LinesRepresentation("X95", "ΣΥΝΤΑΓΜΑ - ΑΕΡΟΛ. ΑΘΗΝΩΝ (EXPRESS)", true),
                new LinesRepresentation("X96", "ΠΕΙΡΑΙΑΣ - ΑΕΡΟΛ. ΑΘΗΝΩΝ (EXPRESS)", false),
                new LinesRepresentation("X97", "ΣΤ. ΜΕΤΡΟ ΕΛΛΗΝΙΚΟ - ΑΕΡΟΛ. ΑΘΗΝΩΝ (EXPRESS)", true)
            };
            
            //{ "X93", "X95", "X96", "X97" };
            listBoxMetro.ItemsSource = new List<LinesRepresentation> () { 
                new LinesRepresentation("M3 ", "AGIA MARINA - AIRPORT", false)
            };

            TouristPivot.Items.Add(new PivotItem(){
                Name = "AirportBuses",
                Content = listBoxBuses
            });

            TouristPivot.Items.Add(new PivotItem(){
                Name = "AirportMetro",
                Content = listBoxMetro
            });
        }

        #endregion
    }

    internal class LinesRepresentation
    {
        public string LineNumber { get; set; }
        public string LineName { get; set; }
        public bool Even { get; set; }

        public LinesRepresentation(string lNo, string lName, bool even)
        {
            this.LineNumber = lNo;
            this.LineName = lName;
            this.Even = even;
        }
    }
}
