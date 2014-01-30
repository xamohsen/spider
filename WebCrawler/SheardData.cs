using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Web;
using System.Net;
using System.Text.RegularExpressions;
using System.Net.WebSockets;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Forms;


namespace WebCrawler
{
    class SheardData
    {
        public static List<URL> UrlsToDownload = new List<URL>(); // Container that contain All Linkes that need to dowanload
        public static List<URL> UrlsToCrawle = new List<URL>(); // Container that contain All Linkes that need to crawl
        public static List<URL> FoundURL = new List<URL>(); // Container that contain All href founded in any page
        public static HashSet<string> Downloded = new HashSet<string>(); // Container that contain All downloaded Pages
        public static HashSet<URL> CrawledUrls = new HashSet<URL>();     // Container that contain All crawled pages  
        public static HashSet<URL> VistUrl = new HashSet<URL>();
        public static DownloadPages Downloader = new DownloadPages(); // Create object of DownloadPages Class
        public static ParsePages Parser = new ParsePages(); // Create object of CrawlerPages Class
        public static Stopwatch Watch = new Stopwatch(); // Watch to calc the total time begin from start crawl
        public static int CrawledPagesNumber; // number of crawler pages
        public static int ErrorsNumber; // Number of Error 
        public static int DownloadedPagesNumber; // Number of Downloaded pages
        SheardData()
        {
            CrawledPagesNumber = 0;
            DownloadedPagesNumber = 0;
            ErrorsNumber = 0;
        }
    }
}
