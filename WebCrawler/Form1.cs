using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Web;
using System.Net;
using System.Text.RegularExpressions;
using System.Net.WebSockets;
using System.Diagnostics;
using System.Threading;
using System.Windows;


namespace WebCrawler
{
    public partial class Form1 : Form
    {
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private int add;
        public Form1()
        {
            InitializeComponent();
            timer.Tick += new EventHandler(timer1_Tick); // Everytime timer ticks, timer_Tick will be called
            timer.Interval = 1000;             // Timer will tick every 1 seconds
            timer.Enabled = true;                       // Enable the timer
            if (!Directory.Exists(@"C:\CrawlePages"))
                System.IO.Directory.CreateDirectory(@"C:\CrawlePages");
            timer.Start();
            add = 0;
        }
        public static long DirSize(DirectoryInfo Path)
        {
            long Size = 0;
            // Add file sizes.
            FileInfo[] Files = Path.GetFiles();
            foreach (FileInfo File in Files)
            {
                Size += File.Length;
            }
            // Add subdirectory sizes.
            DirectoryInfo[] GetDirectories = Path.GetDirectories();
            foreach (DirectoryInfo GetDirectorie in GetDirectories)
            {
                Size += DirSize(GetDirectorie);
            }
            return (Size);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            label3.Text = SheardData.FoundURL.Count.ToString();
            label5.Text = SheardData.DownloadedPagesNumber.ToString();
            label11.Text = SheardData.Watch.Elapsed.ToString();
            label7.Text = SheardData.ErrorsNumber.ToString();
            try
            {
                if (SheardData.FoundURL.Count > add)
                {
                    label8.Text = DirSize(new DirectoryInfo(@"C:\CrawlePages")).ToString() + " Bytes";
                    int temp = SheardData.FoundURL.Count;
                    for (int i = add; i < temp; i++)
                    {
                        {
                            string[] arr = { add.ToString(), SheardData.FoundURL[i].WebDepth.ToString(), SheardData.FoundURL[i].Link.ToString(), SheardData.FoundURL[i].Parent };
                            ListViewItem item = new ListViewItem(arr);
                            listView1.Items.Add(item);
                            add++;
                        }
                    }
                }
            }
            catch (Exception){}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            URL url = new URL(textBox1.Text.ToString(), null, 0);
            SheardData.Watch.Start();
            SheardData.FoundURL.Add(url);
            SheardData.CrawledUrls.Add(url);
            SheardData.CrawledPagesNumber++;
            Thread thread = new Thread(() => SheardData.Parser.Parsing(url));
            thread.IsBackground = true;
            thread.Priority = ThreadPriority.Lowest;
            thread.Start();
        }
    }
}
