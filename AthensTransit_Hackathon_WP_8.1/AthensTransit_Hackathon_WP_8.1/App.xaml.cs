using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace AthensTransit_Hackathon
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
        private TransitionCollection transitions;
        //public static ProgressRing progressRing;
        public static SolidColorBrush[] pivotHeaderColors;
        public static IEnumerable<System.Xml.Linq.XElement> activeBus;
        public static int timeOfUse;
        public static bool asked;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;

            pivotHeaderColors = new SolidColorBrush[2];
            pivotHeaderColors[0] = new SolidColorBrush(Colors.White);
            pivotHeaderColors[1] = new SolidColorBrush(Color.FromArgb(128, 160, 160, 160));
        }

        public static void LoadBus(string lineNumber)
        {
            bool train = false;
            lineNumber = ChangeGreekLettersToEnglish(lineNumber);

            if (lineNumber.Contains("m") || lineNumber.Contains("t"))
            {
                train = true;
                if (lineNumber.Contains("M"))
                    lineNumber = lineNumber.Replace("M", "m");
                else
                    lineNumber = lineNumber.Replace("T", "t");
            }

            XDocument xd;

            if (!train)
            {
                xd = XDocument.Load("all.xml");
            }
            else
            {
                xd = XDocument.Load("trains.xml");
            }
            activeBus = from el in xd.Descendants("Bus")
                        where el.Attribute("lineNumber").Value == lineNumber
                        select el;
        }

        public static string ChangeGreekLettersToEnglish(string lineNumber)
        {
            if (lineNumber.Contains("Α"))
                lineNumber = lineNumber.Replace("Α", "a");
            else if (lineNumber.Contains("Β"))
                lineNumber = lineNumber.Replace("Β", "b");
            else if (lineNumber.Contains("Γ"))
                lineNumber = lineNumber.Replace("Γ", "c");
            else if (lineNumber.Contains("Ε"))
                lineNumber = lineNumber.Replace("Ε", "e");
            else if (lineNumber.Contains("Χ"))
                lineNumber = lineNumber.Replace("Χ", "x");
            return lineNumber;
        }

        public static string ChangeEnglishLettersToGreek(string temp)
        {
            if (temp.Contains("a"))
            {
                temp = temp.Replace("a", "Α");
            }
            else if (temp.Contains("b"))
            {
                temp = temp.Replace("b", "Β");
            }
            else if (temp.Contains("c"))
            {
                temp = temp.Replace("c", "Γ");
            }
            else if (temp.Contains("e"))
            {
                temp = temp.Replace("e", "Ε");
            }
            else if (temp.Contains("x"))
            {
                temp = temp.Replace("x", "Χ");
            }
            else if (temp.Contains("m"))
            {
                temp = temp.Replace("m", "M");
            }
            else if (temp.Contains("t"))
            {
                temp = temp.Replace("t", "T");
            }
            return temp;
        }


        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif


            timeOfUse = !ApplicationData.Current.LocalSettings.Values.ContainsKey("FirstTime") ? 0 : Convert.ToInt32(ApplicationData.Current.LocalSettings.Values["FirstTime"]);
            ApplicationData.Current.LocalSettings.Values["FirstTime"] = ++timeOfUse;

            Frame rootFrame = Window.Current.Content as Frame;
            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                // TODO: change this value to a cache size that is appropriate for your application
                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // Removes the turnstile navigation for startup.
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(MainLayout), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Ensure the current window is active
            Window.Current.Activate();

        }

        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}