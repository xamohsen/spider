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
    class ParsePages
    {
        public string Accumulation_Url(string to_add, string Orgianla)
        {
            Uri absolute = new Uri(Orgianla);
            Uri result = new Uri(absolute, to_add);
            if (!result.ToString().StartsWith("http")) return Orgianla;
            return result.ToString();
        }
        public void Parsing(URL Url)
        {
            List<URL> Href = new List<URL>();
            HtmlAgilityPack.HtmlWeb hw = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = hw.Load(Url.Link);
            if (doc.DocumentNode.SelectNodes("//a[@href]") != null)
                Parallel.ForEach(doc.DocumentNode.SelectNodes("//a[@href]"), link =>
                {
                    SheardData.CrawledPagesNumber++;
                    HtmlAgilityPack.HtmlAttribute Attributes = link.Attributes["href"];
                    string S = Attributes.Value.ToString();
                    S = Accumulation_Url(S, Url.Link);
                    SheardData.FoundURL.Add(new URL(S, Url.Link, Url.WebDepth + 1));
                    if (SheardData.VistUrl.Contains(new URL(S, Url.Link, Url.WebDepth + 1)) == false)
                    {
                        Href.Add(new URL(S, Url.Link, Url.WebDepth + 1));
                        SheardData.VistUrl.Add(new URL(S, Url.Link, Url.WebDepth + 1));
                    }
                }
            );
            var thread = new Thread(() => SheardData.Downloader.DownloadFiles(Href));
            thread.IsBackground = true;
            thread.Priority = ThreadPriority.AboveNormal;
            thread.Start();
            var thread1 = new Thread(() => Parsing(Href));
            thread1.IsBackground = true;
            thread1.Priority = ThreadPriority.AboveNormal;
            thread1.Start();
        }
        public void Parsing(List<URL> Href)
        {
            while (true)
            {
                List<URL> temp = new List<URL>();
                Parallel.For(0, Href.Count, ind =>
                {
                    try
                    {
                        HtmlAgilityPack.HtmlWeb hw = new HtmlAgilityPack.HtmlWeb();
                        HtmlAgilityPack.HtmlDocument doc = hw.Load(Href[ind].Link);
                        if (doc.DocumentNode.SelectNodes("//a[@href]") != null)
                            Parallel.ForEach(doc.DocumentNode.SelectNodes("//a[@href]"), link =>
                            {
                                SheardData.CrawledPagesNumber++;
                                HtmlAgilityPack.HtmlAttribute Attributes = link.Attributes["href"];
                                string S = Accumulation_Url(Attributes.Value.ToString(), Href[ind].Link);
                                SheardData.FoundURL.Add(new URL(S, Href[ind].Link, Href[ind].WebDepth + 1));
                                if (SheardData.VistUrl.Contains(new URL(S, Href[ind].Link, Href[ind].WebDepth + 1)) == false)
                                {
                                    temp.Add(new URL(S, Href[ind].Link, Href[ind].WebDepth + 1));
                                    SheardData.VistUrl.Add(new URL(S, Href[ind].Link, Href[ind].WebDepth + 1));
                                }
                            }
                        );
                    }
                    catch (Exception)
                    {
                        SheardData.ErrorsNumber += 1;
                    }
                }
                );
                Href = temp;
                var thread = new Thread(() => SheardData.Downloader.DownloadFiles(temp));
                thread.IsBackground = true;
                thread.Priority = ThreadPriority.AboveNormal;
                thread.Start();

            }
        }
    }
}
