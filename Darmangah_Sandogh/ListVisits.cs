using System;
using System.Data;
using System.Windows.Forms;
using Stimulsoft.Report;
using Telerik.WinControls.UI;

namespace Darmangah_Sandogh
{
    public partial class ListVisits : RadForm
    {
        MyDB db = new MyDB();
        private DataSet ds = new DataSet();
        public ListVisits()
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
        private void radButton1_Click(object sender, EventArgs e)
        {
            db.SetCommand(@"SELECT dbo.Patients.patientid, dbo.Patients.fname + ' ' + dbo.Patients.lname AS fullName, dbo.Patients.reg_date, dbo.khGroup.khGrpName, dbo.khType.khName, drName = N'" + comboBox1.Text + "' FROM dbo.Patients INNER JOIN dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN dbo.khType ON dbo.khUsage.KhID = dbo.khType.khID INNER JOIN dbo.khGroup ON dbo.khType.khGrpID = dbo.khGroup.khGrpID where dbo.patients.reg_date BETWEEN '" + regBox1.Date + "' AND '" + regBox2.Date + "' AND (dbo.khType.khGrpID = 2) and patients.doctorID = '" + comboBox1.SelectedValue+"' AND dbo.khUsage.ShiftID = '" + comboBox2.SelectedValue + "' and dbo.patients.Free = 'False' order by patients.reg_date, dbo.khUsage.ShiftID");
            DataTable info = db.GetData2();

            db.SetCommand(@"SELECT dbo.Patients.patientid, dbo.Patients.fname + ' ' + dbo.Patients.lname AS fullName, dbo.Patients.reg_date, dbo.khGroup.khGrpName, dbo.khType.khName, drName = N'" + comboBox1.Text + "' FROM dbo.Patients INNER JOIN dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN dbo.khType ON dbo.khUsage.KhID = dbo.khType.khID INNER JOIN dbo.khGroup ON dbo.khType.khGrpID = dbo.khGroup.khGrpID where dbo.patients.reg_date BETWEEN '" + regBox1.Date + "' AND '" + regBox2.Date + "' AND (dbo.khType.khGrpID = 2) and patients.doctorID = '" + comboBox1.SelectedValue+"' AND dbo.khUsage.ShiftID = '" + comboBox2.SelectedValue + "' and dbo.patients.Free = 'True' order by patients.reg_date, dbo.khUsage.ShiftID");
            DataTable info2 = db.GetData2();


            StiReport stiReport1 = new StiReport();
            string path = "Reports\\VISList.mrt";
            stiReport1.Load(path);
            stiReport1.ResetRenderedState();
            DataSet ds2 = new DataSet();
            info.TableName = "info";
            info2.TableName = "info2";
            ds2.Tables.Add(info);
            ds2.Tables.Add(info2);
            stiReport1.Dictionary.Clear();
            stiReport1.RegData(ds2);
            stiReport1.Dictionary.Synchronize();
            if (Properties.Settings.Default.FishPrinter.ToString() == "True")
            {
                stiReport1.Print(false);
            }
            else
            {
                stiReport1.Show();
            }
        }

        private void ListVisits_Load(object sender, EventArgs e)
        {
            db.SetCommand("Select * From Doctors");
            ds = db.GetData();
            comboBox1.DataSource = ds.Tables[0];
            comboBox1.DisplayMember = "doctorName";
            comboBox1.ValueMember = "doctorID";

            db.SetCommand("Select * From Shifts");
            ds = db.GetData();
            comboBox2.DataSource = ds.Tables[0];
            comboBox2.DisplayMember = "ShiftName";
            comboBox2.ValueMember = "ShiftID";

            string now = PersianDateTime.Now.ToString("yyyy/MM/dd");
            string[] str = now.Split('/');
            regBox1.Date = str[0] + "/" + str[1] + "/" + "01";
            regBox2.Date = str[0] + "/" + str[1] + "/" + "31";
        }
    }
}
