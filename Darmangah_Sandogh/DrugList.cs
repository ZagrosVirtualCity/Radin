using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Darmangah_Sandogh
{
    public partial class DrugList : RadForm
    {
        readonly MyDB db = new MyDB();
        private DataSet ds = new DataSet();
        private string PID = string.Empty;
        private string drugID = string.Empty;
        private string _reg_date;


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

        private void Patients_Edit_Load(object sender, EventArgs e)
        {
            string now = PersianDateTime.Now.ToString("yyyy/MM/dd");
            string[] str = now.Split('/');
            uc1.Date = str[0] + "/" + str[1] + "/" + "01";
            uc2.Date = str[0] + "/" + str[1] + "/" + "31";

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

                int row2 = dataGridView1.CurrentRow.Index;
                _reg_date = dataGridView1[5, row].Value.ToString();

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
            if (GlobalVariables.DelAccess)
            {
                if (MessageBox.Show("آیا مطمئن هستید ؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                DialogResult.Yes)
                {
                    int a = DifDate(_reg_date, PersianDateTime.Now.ToString("yyyy/MM/dd"));
                    if (a == 0)
                    {
                        db.SetCommand("Delete From drugUsage WHERE drugUsageID = @drugUsageID");
                        db.SetParameter(@"drugUsageID", drugID);
                        db.exec();
                        cell_Click();
                    }
                    else
                    {
                        RadMessageBox.Show("زمان حذف این رکورد گذشته است و نمی توان آن را حذف کرد");
                    }
                }
                else
                {
                    cell_Click();
                }
            }
            else
            {
                RadMessageBox.Show("شما دسترسی حذف این رکورد را ندارید");
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
            comboBox2.DataSource = ds.Tables[0];
            comboBox2.DisplayMember = "drugName";
            comboBox2.ValueMember = "drugID";
        }

        private void btnDateSearch_Click(object sender, EventArgs e)
        {
            db.SetCommand(@"SELECT ROW_NUMBER() OVER(ORDER BY dbo.patients.patientid) AS Radif, dbo.Patients.patientid, dbo.Patients.fname, dbo.Patients.lname, dbo.Doctors.doctorName, dbo.Patients.reg_date, dbo.Shifts.ShiftName, dbo.Personel.PersonelName
FROM            dbo.Patients INNER JOIN
                         dbo.Doctors ON dbo.Patients.doctorID = dbo.Doctors.doctorID INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN
                         dbo.Personel ON dbo.khUsage.PersonelID = dbo.Personel.PersonelID INNER JOIN
                         dbo.ShftPers ON dbo.Personel.PersonelID = dbo.ShftPers.PersonelID INNER JOIN
                         dbo.Shifts ON dbo.khUsage.ShiftID = dbo.Shifts.ShiftID AND dbo.ShftPers.ShiftID = dbo.Shifts.ShiftID
WHERE dbo.patients.reg_date BETWEEN '" + uc1.Date + "' AND '" + uc2.Date + "' group by dbo.Patients.patientid, dbo.Patients.fname, dbo.Patients.lname, dbo.Doctors.doctorName, dbo.Patients.reg_date, dbo.Shifts.ShiftName, dbo.Personel.PersonelName");
            ds = db.GetData();
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void btnFullNameSearch_Click(object sender, EventArgs e)
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
        public int DifDate(string DateStart, string DateEnd)
        {
            try
            {
                PersianCalendar ps = new PersianCalendar();
                DateTime dt1 = ps.ToDateTime(Int32.Parse(DateStart.Substring(0, 4)), Int32.Parse(DateStart.Substring(5, 2)), Int32.Parse(DateStart.Substring(8, 2)), 0, 0, 0, 0);
                DateTime dt2 = ps.ToDateTime(Int32.Parse(DateEnd.Substring(0, 4)), Int32.Parse(DateEnd.Substring(5, 2)), Int32.Parse(DateEnd.Substring(8, 2)), 0, 0, 0, 0);
                int count = (dt2 - dt1).ToString().IndexOf(".");
                int tm = int.Parse((dt2 - dt1).ToString().Substring(0, count)) * 24;

                return tm;
            }
            catch
            {
                return 0;
            }
        }
    }
}