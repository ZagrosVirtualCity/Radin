using System;
using System.Data;
using System.Windows.Forms;
using Stimulsoft.Report;
using Telerik.WinControls.UI;

namespace Darmangah_Sandogh
{
    public partial class VisInvoice : RadForm
    {
        MyDB db = new MyDB();
        private DataSet ds = new DataSet();
        public VisInvoice()
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
            db.SetCommand(@"Select doctorName = N'" + comboBox1.Text + "', reg_date = '" + regBox1.Date + "', reg_date2 = '" + regBox2.Date + "'");
            DataTable info = db.GetData2();

            db.SetCommand(@"SELECT COUNT(*) AS cnt, SUM(dbo.khUsage.cost) AS total
FROM            dbo.khUsage INNER JOIN
                         dbo.Patients ON dbo.khUsage.PatientID = dbo.Patients.patientid INNER JOIN
                         dbo.khType ON dbo.khUsage.KhID = dbo.khType.khID
where patients.reg_date between '" + regBox1.Date + "' and '" + regBox2.Date + "' AND dbo.khType.khGrpID = '2' AND patients.DoctorID = '" + comboBox1.SelectedValue + "' AND Patients.Free = 'False'");
            DataTable ds = db.GetData2();

            StiReport stiReport1 = new StiReport();
            string path = "Reports\\VisRep.mrt";
            stiReport1.Load(path);
            stiReport1.ResetRenderedState();
            DataSet ds2 = new DataSet();
            info.TableName = "info";
            ds.TableName = "ds";
            ds2.Tables.Add(info);
            ds2.Tables.Add(ds);
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

            string now = PersianDateTime.Now.ToString("yyyy/MM/dd");
            string[] str = now.Split('/');
            regBox1.Date = str[0] + "/" + str[1] + "/" + "01";
            regBox2.Date = str[0] + "/" + str[1] + "/" + "31";
        }
    }
}
