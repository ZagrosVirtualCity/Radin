using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Darmangah_Sandogh
{
    public partial class Login : RadForm
    {
        private MyDB db = new MyDB();
        private DataSet ds = new DataSet();
        internal Form1 sch = new Form1();
        private HdwCheck hdw = new HdwCheck();

        public Login()
        {
            InitializeComponent();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (msg.Msg == 256 && keyData == System.Windows.Forms.Keys.Enter)
            {
                if (this.ActiveControl.ToString().Contains("Telerik.WinControls.UI.RadButton"))
                    return base.ProcessCmdKey(ref msg, keyData);
                else
                {
                    System.Windows.Forms.SendKeys.Send("{TAB}");
                    return true;
                }

            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Login_Load(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
            backgroundWorker2.RunWorkerAsync();
            Thread.CurrentThread.CurrentCulture = new CultureInfo("EN-US");
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            try
            {
                db.connect();
            }
            catch
            {
                toolStripStatusLabel2.Text = "اتصال برقرار نیست";
                toolStripStatusLabel2.ForeColor = Color.Red;
                db.disconnect();
            }

            uname.Focus();
            var language = new CultureInfo("EN-US");
            InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(language);
            uname.Focus();

            db.SetCommand("Select * From Shifts");
            ds = db.GetData();
            shiftBox.DataSource = ds.Tables[0];
            shiftBox.DisplayMember = "ShiftName";
            shiftBox.ValueMember = "ShiftID";
            ///////////////////
            db.SetCommand("Select * From Personel");
            ds = db.GetData();
            personelBox.DataSource = ds.Tables[0];
            personelBox.DisplayMember = "PersonelName";
            personelBox.ValueMember = "PersonelID";
            ///////////////////
            db.SetCommand("Select * From Doctors");
            ds = db.GetData();
            drBox.DataSource = ds.Tables[0];
            drBox.DisplayMember = "doctorName";
            drBox.ValueMember = "doctorID";
            ///////////////////
            db.SetCommand("Select * From nurse");
            ds = db.GetData();

            comboBox1.DataSource = ds.Tables[0];
            comboBox1.DisplayMember = "nurseName";
            comboBox1.ValueMember = "id";

            db.SetCommand("Select * From nurse");
            ds = db.GetData();

            comboBox2.DataSource = ds.Tables[0];
            comboBox2.DisplayMember = "nurseName";
            comboBox2.ValueMember = "id";
            ///////////////////
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            //var dialogResult = MessageBox.Show(@" مایل به خروج هستید؟", @"خروج از برنامه", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);

            //if (dialogResult == DialogResult.No)
            //{
            //    e.Cancel = true;
            //}
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (uname.Text.Replace("'", "''") == "" || pwd.Text.Replace("'", "''") == "")
                {
                    if (uname.Text == string.Empty)
                    {
                        Err.SetError(uname, "وارد کردن نام کاربری اجباریست");
                        uname.Focus();
                    }
                    else
                    {
                        Err.SetError(uname, string.Empty);
                    }
                    if (pwd.Text == "")
                    {
                        Err.SetError(pwd, "وارد کردن رمز عبور اجباریست");
                        pwd.Focus();
                    }
                    else
                    {
                        Err.SetError(pwd, string.Empty);
                    }
                }
                else //if not empty
                {
                    Err.SetError(pwd, "");
                    Err.SetError(uname, "");

                    db.connect();

                    db.SetCommand("SELECT * FROM Sys_Users WHERE uname='" + uname.Text.Replace("'", "''") + "' AND pwd='" + CryptorEngine.Encrypt(pwd.Text.Replace("'", "''"), true) + "'");
                    ds = db.GetData();
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        RadMessageBox.Show("نام کاربری یا رمز عبور صحیح نمی باشد");
                    }
                    else //If USer Or pass Correct
                    {

                        db.SetCommand(@"Select drPercent from Doctors where doctorID = @doctorID");
                        db.SetParameter(@"doctorID", drBox.SelectedValue);
                        DataTable dt_DrPercenter = db.GetData2();
                        GlobalVariables.drPercent = Convert.ToInt32(dt_DrPercenter.Rows[0]["drPercent"]);

                        bool active = Convert.ToBoolean(ds.Tables[0].Rows[0]["active"]);
                        if (active)
                        {
                            bool paziresh = Convert.ToBoolean(ds.Tables[0].Rows[0]["paziresh"]);
                            bool virayesh = Convert.ToBoolean(ds.Tables[0].Rows[0]["virayesh"]);
                            bool gozareshat = Convert.ToBoolean(ds.Tables[0].Rows[0]["gozareshat"]);
                            bool savabegh = Convert.ToBoolean(ds.Tables[0].Rows[0]["savabegh"]);
                            bool maghadir_paye = Convert.ToBoolean(ds.Tables[0].Rows[0]["maghadir_paye"]);
                            bool modiriat = Convert.ToBoolean(ds.Tables[0].Rows[0]["modiriat"]);

                            sch.ribbonTab1.Enabled = paziresh;
                            sch.ribbonTab7.Enabled = gozareshat;
                            sch.ribbonTab4.Enabled = maghadir_paye;
                            sch.ribbonTab3.Enabled = modiriat;
                            sch.ribbonTab6.Enabled = virayesh;
                            sch.ribbonTab2.Enabled = savabegh;
                            sch.UserName.Text = uname.Text;
                            GlobalVariables.LoggedInUser = Convert.ToInt16(ds.Tables[0].Rows[0]["uid"]);
                            var language = new CultureInfo("FA-IR");
                            InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(language);
                            GlobalVariables.persID = Convert.ToInt16(personelBox.SelectedValue);
                            GlobalVariables.shftID = Convert.ToInt16(shiftBox.SelectedValue);
                            GlobalVariables.pezID = Convert.ToInt16(drBox.SelectedValue);
                            GlobalVariables.nurseID1 = Convert.ToInt16(comboBox1.SelectedValue);
                            GlobalVariables.nurseID2 = Convert.ToInt16(comboBox2.SelectedValue);
                            GlobalVariables.nurseName1 = comboBox1.Text;
                            GlobalVariables.nurseName2 = comboBox2.Text;
                            Hide();
                            sch.Show();
                        }
                        else
                        {
                            RadMessageBox.Show("UserName or Password Is InCorrect");
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                RadMessageBox.Show("خطا در ورود ااطلاعات");
                RadMessageBox.Show(ex.Message);
            }
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                string xmlUrl = "http://updater.zagroscity.com/Radin/XMLConfig.xml?random=" + Guid.NewGuid().ToString();
                string exeFileName = "Darmangah_Sandogh.exe";
                string processName = "Darmangah_Sandogh";
                string curVer = Application.ProductVersion;
                string arg = xmlUrl + "|" + exeFileName + "|" + processName + "|" + curVer;
                Process.Start("RadinUpdater.exe", arg);
            }
            catch (Exception ex)
            {
                LogDA.LogError(ex.Message);
            }
        }

        private void backgroundWorker2_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            hdw.ExUAct();
            hdw.LockCheckz();
        }
    }
}
