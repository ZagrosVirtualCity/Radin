using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Windows.Forms;
using System.Xml;
using SharpCompress.Archives;
using SharpCompress.Archives.Rar;
using SharpCompress.Common;

namespace AppUpdater
{
    public partial class FormMain : Form
    {
        string url = string.Empty;
        string xmlFileUrl = "";
        string processName = "";
        string exeFileName = "";
        string currentVersion = "";
        List<AppVersion> appVersionsList = new List<AppVersion>();
        int curDownloadIndex = -1;
        public FormMain(string[] args)
        {
            InitializeComponent();
            var strs = args[0].Split('|');
            xmlFileUrl = strs[0];
            exeFileName = strs[1];
            processName = strs[2];
            currentVersion = strs[3];
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            CheckForUpdate();
        }

        private void CheckForUpdate()
        {
            try
            {
                ReadXml();
            }
            catch (Exception ex)
            {
                File.WriteAllText("uplogger.txt", ex.Message, System.Text.Encoding.UTF8);
                Application.Exit();
            }
        }
        private void ReadXml()
        {
            //System.Diagnostics.Debugger.Launch();

            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(xmlFileUrl);
            List<AppVersion> appVersions = new List<AppVersion>();
            var xvers = xdoc.DocumentElement.SelectNodes("AppVersion");
            foreach (XmlElement xel in xvers)
            {
                appVersions.Add(AppVersion.FromXml(xel));
            }

            List<AppVersion> sorted= appVersions.OrderBy(x => x.Version).ToList();
            appVersionsList = new List<AppVersion>();
            Version cur = new Version(currentVersion);
            for (int i = 0; i < sorted.Count; i++)
            {
                if (sorted[i].Version <= cur)
                    continue;
                appVersionsList.Add(sorted[i]);
            }

            curDownloadIndex = -1;
            if (appVersionsList.Count > 0)
            {
                StopProcess();
                StepByStepUpdate();
            }
            else
            {
                this.Close();
            }
        }
        private void StopProcess()
        {
            foreach (var process in Process.GetProcessesByName(processName))
            {
                process.Kill();
            }
        }
        private void StepByStepUpdate()
        {
            curDownloadIndex++;
            if (curDownloadIndex<0 || curDownloadIndex >= appVersionsList.Count)
                return;

            if (appVersionsList[curDownloadIndex].Silent)
            {
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
            }
            if (this.ShowInTaskbar)
            {
                var res = MessageBox.Show(this, appVersionsList[curDownloadIndex].Version + "\r\n" + appVersionsList[curDownloadIndex].ChangeLog, "Update", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (res == DialogResult.Cancel)
                    this.Close();
            }
            using (WebClient webClientUpdate = new WebClient())
            {
                webClientUpdate.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                webClientUpdate.DownloadProgressChanged += webClientUpdate_DownloadProgressChanged;
                webClientUpdate.DownloadFileCompleted += webClientUpdate_DownloadFileCompleted; ;
                webClientUpdate.DownloadFileAsync(new Uri(appVersionsList[curDownloadIndex].Url + "?random=" + Guid.NewGuid().ToString()), "updates.rar");
            }
        }
        private void WebClientXml_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            ReadXml();
        }
        private void webClientUpdate_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            progressBar1.Value = 0;
            lblText.Text = "";
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
            //using (ZipFile zip = ZipFile.Read("updates.rar"))
            //{
            //    zip.ExtractAll(Application.StartupPath,ExtractExistingFileAction.OverwriteSilently);
            //}
            //System.IO.File.Delete("updates.rar");

            if (curDownloadIndex == (appVersionsList.Count - 1))
            {
                System.Diagnostics.Process.Start(exeFileName);
                Application.Exit();
            }
            else
            {
                StepByStepUpdate();
            }
        }
        private void webClientUpdate_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            lblText.Text = "Downloading version: " + appVersionsList[curDownloadIndex].Version.ToString();
            progressBar1.Value = e.ProgressPercentage;
            progressBar1.Text = e.ProgressPercentage.ToString() + "%";
        }
    }

    public class AppVersion
    {
        public Version Version { get; set; }
        public string Url { get; set; }
        public string ChangeLog { get; set; }
        public bool Silent { get; set; }

        public AppVersion()
        {
            Version = new Version("1.0.0.0");
            Url = "";
            ChangeLog = "";
            Silent = false;
        }

        public static AppVersion FromXml(XmlElement xel)
        {
            AppVersion apv = new AppVersion();
            apv.Version = new Version(xel.SelectSingleNode("version").InnerText.ToString());
            apv.Url = xel.SelectSingleNode("url").InnerText.ToString();
            apv.ChangeLog = xel.SelectSingleNode("changelog").InnerText.ToString();
            apv.Silent = Convert.ToBoolean(xel.SelectSingleNode("silent").InnerText);
            return apv;
        }
    }
}
