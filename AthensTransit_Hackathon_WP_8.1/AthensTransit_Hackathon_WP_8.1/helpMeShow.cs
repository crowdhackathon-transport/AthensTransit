using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace AthensTransit_Hackathon
{
    public class helpMeShow
    {
        public bool even { get; set; }

        public string textBackgroundColor { get; set; }

        public string bigHour { get; set; }

        public string smallHour { get; set; }
        public string trainRoute { get; set; }

        public string trainRouteColor { get; set; }

        public helpMeShow (string t, bool e)
        {
            this.trainRoute = t;
            this.even = e;

            if (e)
                textBackgroundColor = "LightGray";
            else                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             
                textBackgroundColor = "DarkGray";
        }

        public helpMeShow(string big, string small, bool e)
        {
            this.bigHour = big;
            this.smallHour = small;
            this.even = e;

            if (e)
                textBackgroundColor = "#50696969";
            else
                textBackgroundColor = "#35a9a9a9";
        }
        public helpMeShow(string trainMessage, string color, bool e, bool train)
        {
            this.trainRoute = trainMessage;
            this.trainRouteColor = color;
            this.even = e;

            if (IsDigitsOnly(trainRoute))
            {
                if (trainRoute.Length == 4)
                {
                    this.trainRoute = trainMessage.Substring(0, 2) + ":" + trainMessage.Substring(2, 2);
                }
                else if (trainRoute.Length == 9 || trainRoute.Length == 10)
                {
                    this.trainRoute = trainMessage.Substring(0, 2) + "." + trainMessage.Substring(2, 2) + "-" +
                        trainMessage.Substring(4, 2) + "." + trainMessage.Substring(6, 2) + ": " + trainMessage.Substring(8) + "'";
                }
                else if (trainRoute.Length == 11)
                {
                    this.trainRoute = trainMessage.Substring(0, 2) + "." + trainMessage.Substring(2, 2) + "-" +
                        trainMessage.Substring(4, 2) + "." + trainMessage.Substring(6, 2) + ": "
                         + trainMessage.Substring(8, 1) + "'" + trainMessage.Substring(9, 2) + "\"";
                }
                else if (trainRoute.Length == 12)
                {
                    this.trainRoute = trainMessage.Substring(0, 2) + "." + trainMessage.Substring(2, 2) + "-" +
                        trainMessage.Substring(4, 2) + "." + trainMessage.Substring(6, 2) + ": "
                         + trainMessage.Substring(8, 2) + "'" + trainMessage.Substring(10, 2) + "\"";
                }
                else
                {
                    this.trainRoute = "INFORM ME!!!";
                }
            }

            if (e)
                textBackgroundColor = "#50696969";
            else
                textBackgroundColor = "#35a9a9a9";
        }

        private bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }


    }
}
