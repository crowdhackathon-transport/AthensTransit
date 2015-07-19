using AthensTransit_Hackathon.Common;
using RssReader.RssReaders;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class RssPresenter : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        private ObservableCollection<oasaFeedItem> oasaFeedCollection;
        private ObservableCollection<apergiaFeedItem> apergiaFeedCollection;

        private CollectionViewSource collectionViewSource;

        private const string oasaUrl = "http://www.oasa.gr/news/el/anaknews.xml";
        private const string apergiaMMMUrl = "http://feeds.feedburner.com/calendar-mmm-all?format=xml";
        private const string apergiaAllUrl = "http://feeds.feedburner.com/apergiagr?format=xml";

        public RssPresenter()
        {
            this.InitializeComponent();

            oasaFeedCollection = new ObservableCollection<oasaFeedItem>();
            apergiaFeedCollection = new ObservableCollection<apergiaFeedItem>();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }
        
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }
                
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);

            progressRing.IsActive = true;
            spButtons.IsTapEnabled = false;

            oasaFeedCollection = await OasaReader.getFeedsAsync(oasaUrl);
            
            apergiaFeedCollection = await ApergiaReader.getFeedsAsync(apergiaMMMUrl);

            progressRing.IsActive = false;
            spButtons.IsTapEnabled = true;

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        private void bOasa_Click(object sender, RoutedEventArgs e)
        {
            ListView.ItemsSource = oasaFeedCollection;
            ListView.ItemTemplate = (DataTemplate)this.FindName("oasaTemplate");
        }

        private void bApergia_Click(object sender, RoutedEventArgs e)
        {
            ListView.ItemsSource = apergiaFeedCollection;
            ListView.ItemTemplate = (DataTemplate)this.FindName("apergiaTemplate");
        }

        private void bTraffic_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame.Content != null)
            {
                rootFrame.Navigate(typeof(TrafficAthens), null);
            }
        }
    }
}
