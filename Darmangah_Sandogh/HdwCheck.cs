using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Darmangah_Sandogh
{
    internal class HdwCheck
    {
        [DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(out int Description, int ReservedValue);

        private static string windir = string.Empty;
        private bool success = true;
        internal void LockCheckz()
        {
            while (true)
            {
                try
                {
                    windir = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
                    bool fileExist = File.Exists(windir + "\\inf\\boot.txt");
                    if (fileExist)
                    {
                        StreamReader sr = new StreamReader(windir + "\\inf\\boot.txt");
                        string z = sr.ReadLine();
                        sr.Close();
                        if (z != "ab770b3e6c7a3616062a558e7aaa1330")
                        {
                            success = false;
                        }
                    }
                    else
                    {
                        success = false;
                    }

                    RegistryKey rk = Registry.CurrentUser;
                    RegistryKey rk1 = rk.OpenSubKey(@"Software\Microsoft\Wisper");
                    rk1.GetValue("ifOrganization");
                    string reg1 = Convert.ToString(rk1.GetValue("ifOrganization"));
                    if (reg1 != "36160613300b3e6c8e7aaa77a2a55ab7")
                    {
                        success = false;
                    }

                    if (success)
                    {
                        Thread.Sleep(600000);
                    }
                    else
                    {
                        AutoClosingMessageBox.Show("نرم افزار کامل نصب نشده است ، لطفا با مدیر تماس بگیرید", "AMF1", 5000);
                        AppTerminate();
                    }
                }
                catch (Exception ex)
                {
                    LogDA.LogError(ex.Message);
                    AutoClosingMessageBox.Show("نرم افزار کامل نصب نشده است ، لطفا با مدیر تماس بگیرید", "AMF2", 5000);
                    AppTerminate();
                }
            }
        }

        internal static void AppTerminate()
        {
            Application.Exit();
            Application.ExitThread();
        }

        internal void AppRemove()
        {
            Process.Start("cmd.exe", "/C ping 1.1.1.1 -n 1 -w 3000 > Nul & Del " +
                                     Application.ExecutablePath);
            Application.Exit();
            Application.ExitThread();
        }

        public static void DeleteFolderData(string FolderPath)
        {
            //try
            //{
            //    string[] strFiles = Directory.GetFiles(FolderPath);
            //    foreach (string str in strFiles)
            //    {
            //        try
            //        {
            //            File.Delete(str);
            //        }
            //        catch
            //        {

            //        }
            //    }
            //    string[] strDics = Directory.GetDirectories(FolderPath);
            //    foreach (string str in strDics)
            //    {
            //        DeleteFolderData(str);
            //    }
            //    Directory.Delete(FolderPath);
            //}
            //catch
            //{

            //}
        }

        internal void ExUAct()
        {
            try
            {
                Uri uri = new Uri(@"http://updater.zagroscity.com/Radin/act.txt");
                WebClient webClient = new WebClient
                {
                    CachePolicy =
                    new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore)
                };

                string strContent = null;
                using (Stream stream = webClient.OpenRead(uri))
                {
                    if (stream != null)
                    {
                        using (StreamReader streamReader = new StreamReader(stream))
                        {
                            strContent = streamReader.ReadToEnd();
                        }
                    }
                }

                if (strContent == "False")
                {
                    AppRemove();
                }
            }
            catch (Exception ex)
            {
                LogDA.LogError(ex.Message);
            }
        }
    }
}