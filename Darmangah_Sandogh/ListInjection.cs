using System;
using System.Data;
using System.Windows.Forms;
using Stimulsoft.Report;
using Telerik.WinControls.UI;

namespace Darmangah_Sandogh
{
    public partial class ListInjection : RadForm
    {
        MyDB db = new MyDB();
        private DataSet ds = new DataSet();
        public ListInjection()
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
            db.SetCommand(@"SELECT TOP (100) PERCENT dbo.Patients.patientid, dbo.Patients.fname + ' ' + dbo.Patients.lname AS fullName, dbo.Patients.reg_date, dbo.khGroup.khGrpName, dbo.Shifts.ShiftName,shifts.ShiftID,SUM(dbo.khUsage.cost) AS costTotal
FROM            dbo.Patients INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN
                         dbo.khType ON dbo.khUsage.KhID = dbo.khType.khID INNER JOIN
                         dbo.khGroup ON dbo.khType.khGrpID = dbo.khGroup.khGrpID INNER JOIN
                         dbo.Shifts ON dbo.khUsage.ShiftID = dbo.Shifts.ShiftID where dbo.patients.reg_date BETWEEN '" +
                          regBox1.Date + "' AND '" + regBox2.Date +
                          "' AND (dbo.khType.khGrpID = 1)  and dbo.khUsage.ShiftID = '" + comboBox2.SelectedValue + "' and dbo.patients.Free = 'False' group by dbo.Patients.patientid, dbo.Patients.fname, dbo.Patients.lname, dbo.Patients.reg_date, dbo.khGroup.khGrpName, dbo.Shifts.ShiftName, shifts.ShiftID Order by patients.reg_date, shifts.ShiftID");
            DataTable info = db.GetData2();

            db.SetCommand(@"SELECT TOP (100) PERCENT dbo.Patients.patientid, dbo.Patients.fname + ' ' + dbo.Patients.lname AS fullName, dbo.Patients.reg_date, dbo.khGroup.khGrpName, dbo.Shifts.ShiftName,shifts.ShiftID,SUM(dbo.khUsage.cost) AS costTotal
FROM            dbo.Patients INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN
                         dbo.khType ON dbo.khUsage.KhID = dbo.khType.khID INNER JOIN
                         dbo.khGroup ON dbo.khType.khGrpID = dbo.khGroup.khGrpID INNER JOIN
                         dbo.Shifts ON dbo.khUsage.ShiftID = dbo.Shifts.ShiftID where dbo.patients.reg_date BETWEEN '" +
              regBox1.Date + "' AND '" + regBox2.Date +
              "' AND (dbo.khType.khGrpID = 1)  and dbo.khUsage.ShiftID = '" + comboBox2.SelectedValue + "' and dbo.patients.Free = 'True' group by dbo.Patients.patientid, dbo.Patients.fname, dbo.Patients.lname, dbo.Patients.reg_date, dbo.khGroup.khGrpName, dbo.Shifts.ShiftName, shifts.ShiftID Order by patients.reg_date, shifts.ShiftID");
            DataTable info2 = db.GetData2();

            StiReport stiReport1 = new StiReport();
            string path = "Reports\\InjectionList.mrt";
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

        private void ListInjection_Load(object sender, EventArgs e)
        {
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
