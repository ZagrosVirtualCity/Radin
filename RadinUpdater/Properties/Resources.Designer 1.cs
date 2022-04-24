using SharpCompress.Archives;
using SharpCompress.Archives.Rar;
using SharpCompress.Common;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Windows.Forms;

namespace RescueMode
{
    public partial class Form2 : Form
    {
        private string url;
        public RequestCachePolicy CachePolicy { get; private set; }
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            foreach (var process in Process.GetProcessesByName("Darmangah_Sandogh"))
            {
                process.Kill();
            }

            try
            {
                Uri uri = new Uri(@"http://updater.zagroscity.com/Radin/rescue.txt");
                WebClient webClient = new WebClient
                {
                    CachePolicy =
                    new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore)
                };

                using (Stream stream = webClient.OpenRead(uri))
                {
                    if (stream != null)
                    {
                        using (StreamReader streamReader = new StreamReader(stream))
                        {
                            url = streamReader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            using (WebClient webClient = new WebClient())
            {
                webClient.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
                webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
                webClient.DownloadFileAsync(new Uri(url), "updates.rar");
            }
        }
        private void WebClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {

            string Path = Application.StartupPath;
            using (var archive = RarArchive.Open("updates.rar"))
            {
                foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                {
                    entry.WriteToDirectory(Path, new ExtractionOptions()
                    {
                        ExtractFullPath = true,
                        Overwrite = true
                    });
                }
            }

            //using (ZipFile zip = ZipFile.Read("updates.zip"))
            //{
            //    zip.ExtractAll(Application.StartupPath, ExtractExistingFileAction.OverwriteSilently);
            //}
   