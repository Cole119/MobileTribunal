using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MobileTribunal
{
    public class CaseInfo
    {
        public String header { get; set; }
        public Uri champImage1 { get; set; }
        public Uri champImage2 { get; set; }
        public Uri champImage3 { get; set; }
        public Uri champImage4 { get; set; }
        public Uri champImage5 { get; set; }
        public String comments { get; set; }
        public String chatlog { get; set; }
    }
}
