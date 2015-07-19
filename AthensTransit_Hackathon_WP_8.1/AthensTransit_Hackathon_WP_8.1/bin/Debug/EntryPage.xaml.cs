using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading;
using Windows.ApplicationModel.Resources.Core;
using System.Threading.Tasks;
using Windows.System;
using Windows.ApplicationModel.Resources;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
using Windows.Storage;
//using System.Deployment.Application;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace AthensTransit_Hackathon
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Button previousPushedButton;
        private bool searchClicked;

        public MainPage()
        {
            this.InitializeComponent(); 
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (Frame.BackStackDepth == 0)
            {
                Application.Current.Exit();
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            searchClicked = false;
            //progressBar.IsEnabled = true;
            searchBox.Text = "";
            searchBox.PlaceholderText = searchBox.PlaceholderText;

            string rated = string.Empty;
            string facebook = string.Empty;

            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("Rated") || ApplicationData.Current.LocalSettings.Values["Rated"].ToString() != "1")
            {
                ApplicationData.Current.LocalSettings.Values["Rated"] = "0";
            }
            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("Facebook") || ApplicationData.Current.LocalSettings.Values["Facebook"].ToString() != "1")
            {
                ApplicationData.Current.LocalSettings.Values["Facebook"] = "0";
            }

            //string rated = !ApplicationData.Current.LocalSettings.Values.ContainsKey("Rated") ? "0" : string.Empty;
            //ApplicationData.Current.LocalSettings.Values["Rated"] = rated;

            if (App.timeOfUse % 10 == 0 && ApplicationData.Current.LocalSettings.Values["Rated"].ToString() != "1" && !App.asked)
            {
                App.asked = true;
                var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
                string promptString = loader.GetString("PromptToRate");
                string promptTitle = loader.GetString("RateReviewTitle");

                MessageDialog msg = new MessageDialog(promptString, promptTitle);
                msg.Commands.Add(new UICommand("Later", new UICommandInvokedHandler(CommandHandlersRate)));
                msg.Commands.Add(new UICommand("Ok", new UICommandInvokedHandler(CommandHandlersRate)));

                await msg.ShowAsync();
            }

            if (App.timeOfUse % 10 == 5 && ApplicationData.Current.LocalSettings.Values["Facebook"].ToString() != "1" && !App.asked)
            {
                App.asked = true;
                var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
                string promptString = loader.GetString("Facebook");

                MessageDialog msg = new MessageDialog(promptString, "Facebook");
                msg.Commands.Add(new UICommand("Later", new UICommandInvokedHandler(CommandHandlersFacebook)));
                msg.Commands.Add(new UICommand("Ok", new UICommandInvokedHandler(CommandHandlersFacebook)));

                await msg.ShowAsync();
            }

            #region obsolete
            //something = "";
            //something += CultureInfo.CurrentCulture + ", ";
            //something += CultureInfo.CurrentUICulture;

            //ResourceContext ctx = new Windows.ApplicationModel.Resources.Core.ResourceContext();
            //ctx.Languages = new string[] { "fr" };
            //ResourceMap rmap = ResourceManager.Current.MainResourceMap.GetSubtree("Resources");
            //titleTextBlock.Text = rmap.GetValue("mainTextBlock", ctx).ValueAsString;

            //titleTextBlock.Text = App.LanguageChanges(0);
            //trainCB.Label = App.LanguageChanges(2);
            //favoriteIcon.Label = App.LanguageChanges(3);
            //searchBox.Text = something;
            //something += CultureInfo.CurrentCulture.EnglishName + ", ";
            //something += CultureInfo.InvariantCulture.EnglishName;


            //Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "el-GR";
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
            #endregion
        }

        public async void CommandHandlersRate(IUICommand commandLabel)
        {
            var Actions = commandLabel.Label;
            switch (Actions)
            {
                //Okay Button.
                case "Ok":
                    ApplicationData.Current.LocalSettings.Values["Rated"] = "1";
                    await Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=39b25eac-930e-49ab-b9ff-7f8e79e5bf90"));
                    break;
                //Later Button.
                case "Later":
                    ApplicationData.Current.LocalSettings.Values["Rated"] = "0";
                    //Application.Current.Exit();
                    break;
                //end.
            }
        }

        public async void CommandHandlersFacebook(IUICommand commandLabel)
        {
            var Actions = commandLabel.Label;
            switch (Actions)
            {
                //Okay Button.
                case "Ok":
                    ApplicationData.Current.LocalSettings.Values["Facebook"] = "1";
                    await Launcher.LaunchUriAsync(new Uri("https://www.facebook.com/athenstransitapp"));
                    break;
                //Later Button.
                case "Later":
                    ApplicationData.Current.LocalSettings.Values["Facebook"] = "0";
                    //Application.Current.Exit();
                    break;
                //end.
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        //private void searchBox_GotFocus(object sender, RoutedEventArgs e)
        //{
        //    if (searchBox.Text.Equals(searchBox.PlaceholderText, StringComparison.OrdinalIgnoreCase))
        //    {
        //        searchBox.Text = string.Empty;
        //    }
        //}

        //private void searchBox_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    if (string.IsNullOrEmpty(searchBox.Text))
        //    {
        //        searchBox.PlaceholderText = searchBox.PlaceholderText;
        //    }
        //}

        private async void searchBus_Click(object sender, RoutedEventArgs e)
        {
            //progressBar.Visibility = Windows.UI.Xaml.Visibility.Visible;

            //IEnumerable<System.Xml.Linq.XElement> bus = Bus(searchBox.Text);

            //if (bus == null && searchClicked == false)
            //{
            //    searchClicked = true;

            //    var dialog = new Windows.UI.Popups.MessageDialog(App.LanguageChanges(1));
            //    await dialog.ShowAsync();
            //    searchBox.Text = "";

            //    searchClicked = false;
                
            //    return;
            //} 

            //////progressBar.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            ////progressBar.IsEnabled = true;
            ////var xd = XDocument.Load("all.xml");
            if (!searchBox.Text.Equals("") && searchBox.Text.Count() <= 4 && searchClicked == false)
            {
                App.LoadBus(searchBox.Text);

                if ((App.activeBus.Elements().Count() == 0) && (searchClicked == false))
                {
                    searchClicked = true;
                    previousPushedButton.Style = Resources["RoundNumberButtonTemplate"] as Style;

                    var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
                    var errorString = loader.GetString("Error");

                    var dialog = new Windows.UI.Popups.MessageDialog(errorString);
                    await dialog.ShowAsync();
                    searchBox.Text = "";

                    searchClicked = false;

                    return;
                } 

                searchClicked = true;
                previousPushedButton.Style = Resources["RoundNumberButtonTemplate"] as Style;
                //if (searchBox.Text.Contains("Α"))
                //{
                //    Frame.Navigate(typeof(TimeSchedule), searchBox.Text.Replace("Α", "a"));
                //}
                //else if (searchBox.Text.Contains("Β"))
                //{
                //    Frame.Navigate(typeof(TimeSchedule), searchBox.Text.Replace("Β", "b"));
                //}
                //else if (searchBox.Text.Contains("Γ"))
                //{
                //    Frame.Navigate(typeof(TimeSchedule), searchBox.Text.Replace("Γ", "c"));
                //}
                //else if (searchBox.Text.Contains("Ε"))
                //{
                //    Frame.Navigate(typeof(TimeSchedule), searchBox.Text.Replace("Ε", "e"));
                //}
                //else if (searchBox.Text.Contains("Χ"))
                //{
                //    Frame.Navigate(typeof(TimeSchedule), searchBox.Text.Replace("Χ", "x"));
                //}
                //else
                //{
                    //IEnumerable<System.Xml.Linq.XElement> bus = from el in xd.Descendants("Bus")
                    //                                            where el.Attribute("lineNumber").Value == searchBox.Text
                    //                                            select el;
                    //progressRing.IsActive = false;
                    //App.progressRing.IsEnabled = true;
                    //App.progressRing.IsActive = true;
                    Frame.Navigate(typeof(TimeSchedule));
                //}
            }
        }

        //private IEnumerable<System.Xml.Linq.XElement> Bus(string lineNumber)
        //{
        //    //string lineNumber = searchBox.Text;

        //    //if (searchBox.Text.Equals("0"))
        //    //{
        //    //    await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:REVIEW?PFN=49521EmmanouilChatzipetro.AthensTransit"));
        //    //}


        //    if (!searchBox.Text.Equals(""))
        //    {
        //        previousPushedButton.Style = Resources["RoundNumberButtonTemplate"] as Style;
        //        if (lineNumber.Contains("Α"))
        //        {
        //            lineNumber = searchBox.Text.Replace("Α", "a");
        //            //Frame.Navigate(typeof(TimeSchedule), searchBox.Text.Replace("Α", "a"));
        //        }
        //        else if (lineNumber.Contains("Β"))
        //        {
        //            lineNumber = searchBox.Text.Replace("Β", "b");
        //            //Frame.Navigate(typeof(TimeSchedule), searchBox.Text.Replace("Β", "b"));
        //        }
        //        else if (lineNumber.Contains("Γ"))
        //        {
        //            lineNumber = searchBox.Text.Replace("Γ", "c");
        //            //Frame.Navigate(typeof(TimeSchedule), searchBox.Text.Replace("Γ", "c"));
        //        }
        //        else if (lineNumber.Contains("Ε"))
        //        {
        //            lineNumber = searchBox.Text.Replace("Ε", "e");
        //            //Frame.Navigate(typeof(TimeSchedule), searchBox.Text.Replace("Ε", "e"));
        //        }
        //        else if (lineNumber.Contains("Χ"))
        //        {
        //            lineNumber = searchBox.Text.Replace("Χ", "x");
        //            //Frame.Navigate(typeof(TimeSchedule), searchBox.Text.Replace("Χ", "x"));
        //        }
        //        else
        //        {
        //            //progressRing.IsActive = false;
        //            //App.progressRing.IsEnabled = true;
        //            //App.progressRing.IsActive = true;
        //            //Frame.Navigate(typeof(TimeSchedule), bus);
        //        } 

        //        var xd = XDocument.Load("all.xml");
        //        IEnumerable<System.Xml.Linq.XElement> bus = from el in xd.Descendants("Bus")
        //                                                    where el.Attribute("lineNumber").Value == lineNumber
        //                                                    select el;


        //        try
        //        {
        //            string testLineNumber = (from b in bus
        //                                   select b.FirstAttribute.Value).ToList()[0];
        //            return bus;
        //        }
        //        catch (ArgumentOutOfRangeException)
        //        {
        //            return null;
        //        }

        //        //foreach (var item in busLineNumber)
        //        //{
        //        //    titleTextBlock.Text = item.
        //        //}


        //    }
        //    return null;
        //}

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (previousPushedButton != null)
            {
                //if (previousPushedButton.Name.Equals("buttonA") ||
                //    previousPushedButton.Name.Equals("buttonB") ||
                //    previousPushedButton.Name.Equals("buttonC") ||
                //    previousPushedButton.Name.Equals("buttonE") ||
                //    previousPushedButton.Name.Equals("buttonX"))
                //    previousPushedButton.Style = Resources["RoundBlueButtonTemplate"] as Style;
                //else
                    previousPushedButton.Style = Resources["RoundNumberButtonTemplate"] as Style;
            }
            previousPushedButton = ((Button)sender);

            if (searchBox.Text.Equals(searchBox.PlaceholderText, StringComparison.OrdinalIgnoreCase))
            {
                searchBox.Text = string.Empty;
            }
            ((Button)sender).Style = Resources["RoundChosenButtonTemplate"] as Style;
            searchBox.Text += ((Button)sender).Content;
        }

        private void deleteImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (previousPushedButton != null)
                previousPushedButton.Style = Resources["RoundNumberButtonTemplate"] as Style;
            
            if (!searchBox.Text.Equals(""))
                searchBox.Text = searchBox.Text.Remove(searchBox.Text.Length - 1);
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as AppBarButton).Name)
            {
                case "trainCB":
                    Frame.Navigate(typeof(trainSelection));
                    break;
                case "touristCB":
                    Frame.Navigate(typeof(TouristMode));
                    break;
            }
            
            //Frame.Navigate(typeof(Stops), "t3");
        }

        private async void infoIcon_Click(object sender, RoutedEventArgs e)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            var info = loader.GetString("Info");

            var dialog = new Windows.UI.Popups.MessageDialog(info);
            await dialog.ShowAsync();
        }

        private async void favoriteIcon_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=39b25eac-930e-49ab-b9ff-7f8e79e5bf90"));
            //https://www.facebook.com/athenstransitapp?fref=ts
        }

        private async void facebookIcon_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://www.facebook.com/athenstransitapp"));
        }

    }
}
