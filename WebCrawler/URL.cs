using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler
{
    // Url class that contail all Url relative
    class URL
    {

        public string Link; // the main url
        public string Parent; // the parent of url
        public string Host; // the host of url "like www.google.com"
        public int WebDepth; // the web depth of curnt url
        public URL()
        {
            Link = Parent = null;
            WebDepth = -1;
        }
        public URL(string link, string parent, int depth)
        {
            Link = link;
            Parent = parent;
            WebDepth = depth;
            Host = new Uri(link).Host.ToString();
        }
    }
}
