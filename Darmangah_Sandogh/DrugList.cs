using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml;
using Stimulsoft.Report;
using Telerik.WinControls.UI;

namespace Darmangah_Sandogh
{
    public partial class DrugList : Form
    {
        readonly MyDB db = new MyDB();
        private DataSet ds = new DataSet();
        private string PID = string.Empty;
        private string drugID = string.Empty;


        public DrugList()
        {
            InitializeComponent();
        }

        private void FillGrid()
        {
            db.SetCommand(@"SELECT ROW_NUMBER() OVER(ORDER BY dbo.patients.patientid) AS Radif, dbo.Patients.patientid, dbo.Patients.fname, dbo.Patients.lname, dbo.Doctors.doctorName, dbo.Patients.reg_date, dbo.Shifts.ShiftName, dbo.Personel.PersonelName
FROM            dbo.Patients INNER JOIN
                         dbo.Doctors ON dbo.Patients.doctorID = dbo.Doctors.doctorID INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN
                         dbo.Personel ON dbo.khUsage.PersonelID = dbo.Personel.PersonelID INNER JOIN
                         dbo.ShftPers ON dbo.Personel.PersonelID = dbo.ShftPers.PersonelID INNER JOIN
                         dbo.Shifts ON dbo.khUsage.ShiftID = dbo.Shifts.ShiftID AND dbo.ShftPers.ShiftID = dbo.Shifts.ShiftID
WHERE dbo.patients.reg_date BETWEEN '" + PersianDateTime.Now.ToString("yyyy/MM/dd") + "' AND '" + PersianDateTime.Now.ToString("yyyy/MM/dd") + "' group by dbo.Patients.patientid, dbo.Patients.fname, dbo.Patients.lname, dbo.Doctors.doctorName, dbo.Patients.reg_date, dbo.Shifts.ShiftName, dbo.Personel.PersonelName");
            ds = db.GetData();
            dataGridView1.DataSource = ds.Tables[0];
        }


        private void buttonX5_Click(object sender, EventArgs e)
        {
            db.SetCommand(@"SELECT ROW_NUMBER() OVER(ORDER BY dbo.patients.patientid) AS Radif, dbo.Patients.patientid, dbo.Patients.fname, dbo.Patients.lname, dbo.Doctors.doctorName, dbo.Patients.reg_date, dbo.Shifts.ShiftName, dbo.Personel.PersonelName
FROM            dbo.Patients INNER JOIN
                         dbo.Doctors ON dbo.Patients.doctorID = dbo.Doctors.doctorID INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN
                         dbo.Personel ON dbo.khUsage.PersonelID = dbo.Personel.PersonelID INNER JOIN
                         dbo.ShftPers ON dbo.Personel.PersonelID = dbo.ShftPers.PersonelID INNER JOIN
                         dbo.Shifts ON dbo.khUsage.ShiftID = dbo.Shifts.ShiftID AND dbo.ShftPers.ShiftID = dbo.Shifts.ShiftID
WHERE dbo.patients.reg_date BETWEEN '" + uc1.Shamsi + "' AND '" + uc2.Shamsi + "' group by dbo.Patients.patientid, dbo.Patients.fname, dbo.Patients.lname, dbo.Doctors.doctorName, dbo.Patients.reg_date, dbo.Shifts.ShiftName, dbo.Personel.PersonelName");
            ds = db.GetData();
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            db.SetCommand(@"SELECT ROW_NUMBER() OVER(ORDER BY dbo.patients.patientid) AS Radif, dbo.Patients.patientid, dbo.Patients.fname, dbo.Patients.lname, dbo.Doctors.doctorName, dbo.Patients.reg_date, dbo.Shifts.ShiftName, dbo.Personel.PersonelName
FROM            dbo.Patients INNER JOIN
                         dbo.Doctors ON dbo.Patients.doctorID = dbo.Doctors.doctorID INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN
                         dbo.Personel ON dbo.khUsage.PersonelID = dbo.Personel.PersonelID INNER JOIN
                         dbo.ShftPers ON dbo.Personel.PersonelID = dbo.ShftPers.PersonelID INNER JOIN
                         dbo.Shifts ON dbo.khUsage.ShiftID = dbo.Shifts.ShiftID AND dbo.ShftPers.ShiftID = dbo.Shifts.ShiftID
WHERE dbo.patients.fname LIKE N'%" + name_box.Text + "%' AND dbo.patients.lname LIKE N'%" + family_box.Text + "%' group by dbo.Patients.patientid, dbo.Patients.fname, dbo.Patients.lname, dbo.Doctors.doctorName, dbo.Patients.reg_date, dbo.Shifts.ShiftName, dbo.Personel.PersonelName");
            ds = db.GetData();
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void Patients_Edit_Load(object sender, EventArgs e)
        {
            uc1.Today_Click(null, null);
            uc2.Today_Click(null, null);
            uc1.FirstDayOfMonth_Click(null, null);
            uc2.LastDayOfMonth_Click(null, null);
            string[] str = uc1.Shamsi.Split('/');
            for (int i = 0; i < str.Length; i++) ;
            string mn1 = Convert.ToString(Convert.ToInt32(str[1]) - 1);

            if (mn1.Length == 1)
            {
                mn1 = 0 + mn1;
            }
            string[] str2 = uc2.Shamsi.Split('/');
            for (int i = 0; i < str2.Length; i++) ;
            string mn2 = Convert.ToString(Convert.ToInt32(str2[1]) - 1);

            if (mn2.Length == 1)
            {
                mn2 = 0 + mn2;
            }

            uc1.Text = str[0] + "/" + mn1.ToString() + "/" + str[2];
            uc2.Text = str2[0] + "/" + mn2.ToString() + "/" + str2[2];
            FillGrid();
            drug();
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            cell_Click();
        }

        private void cell_Click()
        {

            try
            {
                int row = dataGridView1.CurrentRow.Index;
                PID = dataGridView1[1, row].Value.ToString();

                db.SetCommand(@"SELECT ROW_NUMBER() OVER(ORDER BY dbo.drugUsage.drugUsageID) AS Radif, dbo.drugUsage.drugUsageID, dbo.drugUsage.patientID, dbo.Patients.fname, dbo.Patients.lname, dbo.drug.drugName, dbo.drugUsage.countz
FROM            dbo.Patients INNER JOIN
                         dbo.drugUsage ON dbo.Patients.patientid = dbo.drugUsage.patientID INNER JOIN
                         dbo.drug ON dbo.drugUsage.drugID = dbo.drug.drugID WHERE dbo.drugUsage.patientID = '" + PID +"'");
                ds = db.GetData();
                radGridView1.DataSource = ds.Tables[0];
                sort();
            }
            catch
            {
            }
        }

        private void sort()
        {
            radGridView1.EnableFiltering = true;
            radGridView1.MasterTemplate.ShowHeaderCellButtons = true;
            radGridView1.MasterTemplate.ShowFilteringRow = false;
            radGridView1.MasterTemplate.BestFitColumns();
            foreach (GridViewDataColumn column in radGridView1.Columns)
            {
                column.BestFit();
            }
        }

        private void radGridView1_UserDeletingRow(object sender, GridViewRowCancelEventArgs e)
        {
            if (MessageBox.Show("آیا مطمئن هستید ؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                DialogResult.Yes)
            {

                db.SetCommand("Delete From drugUsage WHERE drugUsageID = @drugUsageID");
                db.SetParameter(@"drugUsageID", drugID);
                db.exec();
                cell_Click();
            }
            else
            {
                cell_Click();
            }
        }

        private void radGridView1_CellClick(object sender, GridViewCellEventArgs e)
        {
            drugID = radGridView1.SelectedRows[0].Cells[1].Value.ToString();
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            db.SetCommand(@"insert into drugUsage (drugID,patientID,countz) VALUES (@drugID,@patientID,@countz)");
            db.SetParameter(@"drugID", comboBox2.SelectedValue);
            db.SetParameter(@"patientID", PID);
            db.SetParameter(@"countz", txtProNet6.Text);
            db.exec();
            cell_Click();
        }

        private void drug()
        {
            db.SetCommand("Select * From drug");
            ds = db.GetData();
            db.exec();
            comboBox2.DataSource = ds.Tables[0];
            comboBox2.DisplayMember = "drugName";
            comboBox2.ValueMember = "drugID";
        }
    }
}