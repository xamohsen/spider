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
    class DownloadPages
    {
        //create folder for evrey Host of url
        private void CreatFolder(string path)
        {
            if (!Directory.Exists(@path)) // check if folder is already exists
                System.IO.Directory.CreateDirectory(@path); // create folder if it is not exists yet
        }
        // download function that download all pages in Href list local host
        public void DownloadFiles(List<URL> Hrfs)
        {
            try
            {
                Parallel.For(0, Hrfs.Count, ind =>
                {
                    if (SheardData.Downloded.Contains(Hrfs[ind].Link) == false) // check if page already downloaded before
                    {
                        Uri uri = new Uri(Hrfs[ind].Link); // create a Uri object of url link
                        string str = Hrfs[ind].Link;
                        Regex rgx = new Regex("[^a-zA-Z0-9 -]");
                        str = rgx.Replace(str, "");
                        using (WebClient client = new WebClient())
                        {
                            client.DownloadFileCompleted += DownloadCompleted; //Download file Completed event
                            string path = System.IO.Path.Combine(@"C:\CrawlePages\", Hrfs[ind].Host); // create a path
                            CreatFolder(path); // creat folder
                            path = System.IO.Path.Combine(path, str + ".html"); // create final path
                            client.DownloadFileAsync(new Uri(Hrfs[ind].Link), @path, new Uri(Hrfs[ind].Link)); // Download File Async to allow thread working
                        }
                    }
                }
               );
            }
            catch (Exception)
            {
                SheardData.ErrorsNumber += 1;// increace the number of errors
            }
        }
        private void DownloadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Uri url = e.UserState as Uri;
            SheardData.DownloadedPagesNumber++; ;// increace the number of download pages when download end
            SheardData.Downloded.Add(url.ToString()); // insert page downloaded link in downloaded list "Make it visted"
        }
    }
}
