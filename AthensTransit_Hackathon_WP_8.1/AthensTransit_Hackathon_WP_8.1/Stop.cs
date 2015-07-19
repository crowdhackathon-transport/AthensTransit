using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AthensTransit_Hackathon
{
    public class Stop
    {
        public string stopName { get; set; }

        private List <string> _OtherStops;
        public string numberOfConnectingStops { get; set; }

        public string imagePath { get; set; }
        public List<string> OtherStops
        {
            get { return _OtherStops; }
            set { _OtherStops = value; }
        }

        public Stop(string stopName, List<string> list, int numberOfCStops, int stopPosition)
        {
            this.stopName = stopName;
            this._OtherStops = list;

            if (numberOfCStops != 1)
                this.numberOfConnectingStops = "( " + Convert.ToString(numberOfCStops - 1) + " )";

            if (stopPosition == 0)
                this.imagePath = "Assets/AppIcons/stop_with_lines.png";
            else if (stopPosition == -1)
                this.imagePath = "Assets/AppIcons/dot_for_start_with_lines.png";
            else if (stopPosition == 1)
                this.imagePath = "Assets/AppIcons/dot_for_end_with_lines.png";

        }
    }
}
