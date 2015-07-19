using AthensTransit_Hackathon.Common;
using AthensTransit_Hackathon;
using System;
using System.Collections.Generic;
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
using AthensTransit_Hackathon_WP_8._1;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace AthensTransit_Hackathon
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainLayout : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public MainLayout()
        {
            this.InitializeComponent();

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

        #region NavigationHelper registration

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        #region TILE EVENTS
        private void btChooseType_Click(object sender, RoutedEventArgs e)
        {
            NavigateToAthensTransitOr();
        }

        

        private void btAirport_Click(object sender, RoutedEventArgs e)
        {
            NavigateToTouristMode();
        }

        

        private void btPlan_Click(object sender, RoutedEventArgs e)
        {
            //mockup
        }

        private void btAround_Click(object sender, RoutedEventArgs e)
        {
            NavigateToMap();
        }

        private void RSS_DummyClick(object sender, RoutedEventArgs e)
        {
            NavigateToRss();
        }

        private void Traffic_DummyClick(object sender, RoutedEventArgs e)
        {
            NavigateToTraffic();
        }
        #endregion

        #region Navigators
        
        
        private void NavigateToTraffic()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame.Content != null)
            {
                rootFrame.Navigate(typeof(TrafficAthens), null);
            }
        }

        private void NavigateToRss()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame.Content != null)
            {
                rootFrame.Navigate(typeof(RssPresenter),null);
            }
        }

        private void NavigateToAthensTransitOr()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame.Content != null)
            {
                rootFrame.Navigate(typeof(MainPage), null);
            }
        }

        private void NavigateToTouristMode()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame.Content != null)
            {
                rootFrame.Navigate(typeof(TouristMode), null);
            }
        }

        private void NavigateToMap()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame.Content != null)
            {
                rootFrame.Navigate(typeof(MapBus), null);
            }
        }
        #endregion
    }
}
