using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using Stimulsoft.Report;
using Telerik.WinControls.UI;

namespace Darmangah_Sandogh
{
    public partial class Patients_Edit : RadForm
    {
        private MyDB DB = new MyDB();
        private DataSet _ds;
        public string Pid = string.Empty;
        public int userID;

        public Patients_Edit()
        {
            InitializeComponent();
        }

        private void FillGrid()
        {
            DB.SetCommand(@"SELECT ROW_NUMBER() OVER(ORDER BY dbo.patients.patientid) AS Radif, dbo.Patients.patientid, dbo.Patients.fname, dbo.Patients.lname, dbo.Doctors.doctorName, dbo.Patients.reg_date, dbo.Sys_Users.uname, dbo.Patients.hourz, dbo.Patients.Free, dbo.Shifts.ShiftName, 
                         dbo.Personel.PersonelName, dbo.Patients.UserID
FROM            dbo.ShftPers INNER JOIN
                         dbo.Personel ON dbo.ShftPers.PersonelID = dbo.Personel.PersonelID INNER JOIN
                         dbo.Shifts ON dbo.ShftPers.ShiftID = dbo.Shifts.ShiftID INNER JOIN
                         dbo.khUsage ON dbo.Personel.PersonelID = dbo.khUsage.PersonelID AND dbo.Shifts.ShiftID = dbo.khUsage.ShiftID INNER JOIN
                         dbo.Doctors INNER JOIN
                         dbo.Patients ON dbo.Doctors.doctorID = dbo.Patients.doctorID ON dbo.khUsage.PatientID = dbo.Patients.patientid INNER JOIN
                         dbo.Sys_Users ON dbo.Patients.UserID = dbo.Sys_Users.uid
WHERE dbo.patients.reg_date BETWEEN '" + PersianDateTime.Now.ToString("yyyy/MM/dd") + "' AND '" + PersianDateTime.Now.ToString("yyyy/MM/dd") + "' GROUP BY dbo.Patients.patientid, dbo.Patients.fname, dbo.Patients.lname, dbo.Doctors.doctorName, dbo.Patients.reg_date, dbo.Sys_Users.uname, dbo.Patients.hourz, dbo.Patients.Free, dbo.Shifts.ShiftName,dbo.Personel.PersonelName, dbo.Patients.UserID");
            _ds = DB.GetData();
            radGridView1.DataSource = _ds.Tables[0];
            DB.exec();
            sort();
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            if (userID == GlobalVariables.LoggedInUser || userID == 7)
            {
                DB.SetCommand(@"Update patients SET fname = @fname, lname = @lname,  doctorID = @doctorID, 
reg_date = @reg_date, Free = @Free WHERE patientid = @patientid");
                DB.SetParameter(@"patientid", Pid);
                DB.SetParameter(@"fname", fname.Text);
                DB.SetParameter(@"lname", lname.Text);
                DB.SetParameter(@"doctorID", drBox.SelectedValue);
                DB.SetParameter(@"reg_date", reg_date.Text);

                if (radioButton1.Checked)
                {
                    DB.SetParameter(@"Free", "True");
                }
                if (radioButton2.Checked)
                {
                    DB.SetParameter(@"Free", "False");
                }
                DB.exec();

                DB.SetCommand(@"Update khUsage set ShiftID = @ShiftID, PersonelID = @PersonelID where PatientID = @PatientID");
                DB.SetParameter(@"PatientID", Pid);
                DB.SetParameter(@"ShiftID", shiftBox.SelectedValue);
                DB.SetParameter(@"PersonelID", personelBox.SelectedValue);
                DB.exec();

                MessageBox.Show("عملیات با موفقیت انجام شد");
                FillGrid();
            }
            else
            {
                MessageBox.Show("شما دسترسی ویرایش این رکورد را ندارید");
            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (userID == GlobalVariables.LoggedInUser || userID == 7)
            {
                if (MessageBox.Show("آیا مطمئن هستید ؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                DialogResult.Yes)
                {
                    try
                    {
                        DB.SetCommand("Delete From patients WHERE patientid = @patientid");
                        DB.SetParameter(@"patientid", Pid);
                        DB.exec();
                        MessageBox.Show("بیمار حذف شد");
                        FillGrid();
                    }
                    catch
                    {
                        MessageBox.Show("خطا در حذف بیمار");
                    }
                }
            }
            else
            {
                MessageBox.Show("شما دسترسی حذف این رکورد را ندارید");
            }
        }


        private void buttonX5_Click(object sender, EventArgs e)
        {
            DB.SetCommand(@"SELECT ROW_NUMBER() OVER(ORDER BY dbo.patients.patientid) AS Radif, dbo.Patients.patientid, dbo.Patients.fname, dbo.Patients.lname, dbo.Doctors.doctorName, dbo.Patients.reg_date, dbo.Sys_Users.uname, dbo.Patients.hourz, dbo.Patients.Free, dbo.Shifts.ShiftName, 
                         dbo.Personel.PersonelName, dbo.Patients.UserID
FROM            dbo.ShftPers INNER JOIN
                         dbo.Personel ON dbo.ShftPers.PersonelID = dbo.Personel.PersonelID INNER JOIN
                         dbo.Shifts ON dbo.ShftPers.ShiftID = dbo.Shifts.ShiftID INNER JOIN
                         dbo.khUsage ON dbo.Personel.PersonelID = dbo.khUsage.PersonelID AND dbo.Shifts.ShiftID = dbo.khUsage.ShiftID INNER JOIN
                         dbo.Doctors INNER JOIN
                         dbo.Patients ON dbo.Doctors.doctorID = dbo.Patients.doctorID ON dbo.khUsage.PatientID = dbo.Patients.patientid INNER JOIN
                         dbo.Sys_Users ON dbo.Patients.UserID = dbo.Sys_Users.uid
WHERE dbo.patients.reg_date BETWEEN '" + uc1.Shamsi + "' AND '" + uc2.Shamsi + "' GROUP BY dbo.Patients.patientid, dbo.Patients.fname, dbo.Patients.lname, dbo.Doctors.doctorName, dbo.Patients.reg_date, dbo.Sys_Users.uname, dbo.Patients.hourz, dbo.Patients.Free, dbo.Shifts.ShiftName, dbo.Personel.PersonelName, dbo.Patients.UserID");
            _ds = DB.GetData();
            radGridView1.DataSource = _ds.Tables[0];
            DB.exec();
            sort();
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
            pezeshk();
            shift();
            personel();

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

        private void radGridView1_Resize(object sender, EventArgs e)
        {
            sort();
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            DB.SetCommand(@"SELECT ROW_NUMBER() OVER(ORDER BY dbo.patients.patientid) AS Radif,  dbo.Patients.patientid, dbo.Patients.fname, dbo.Patients.lname, dbo.Doctors.doctorName, dbo.Patients.reg_date, dbo.Sys_Users.uname, dbo.Patients.hourz, dbo.Patients.Free, dbo.Shifts.ShiftName, 
                         dbo.Personel.PersonelName, dbo.Patients.UserID
FROM            dbo.ShftPers INNER JOIN
                         dbo.Personel ON dbo.ShftPers.PersonelID = dbo.Personel.PersonelID INNER JOIN
                         dbo.Shifts ON dbo.ShftPers.ShiftID = dbo.Shifts.ShiftID INNER JOIN
                         dbo.khUsage ON dbo.Personel.PersonelID = dbo.khUsage.PersonelID AND dbo.Shifts.ShiftID = dbo.khUsage.ShiftID INNER JOIN
                         dbo.Doctors INNER JOIN
                         dbo.Patients ON dbo.Doctors.doctorID = dbo.Patients.doctorID ON dbo.khUsage.PatientID = dbo.Patients.patientid INNER JOIN
                         dbo.Sys_Users ON dbo.Patients.UserID = dbo.Sys_Users.uid
WHERE dbo.patients.fname LIKE N'%" +
                          name_box.Text + "%' AND dbo.patients.lname LIKE N'%" + family_box.Text + "%' GROUP BY dbo.Patients.patientid, dbo.Patients.fname, dbo.Patients.lname, dbo.Doctors.doctorName, dbo.Patients.reg_date, dbo.Sys_Users.uname, dbo.Patients.hourz, dbo.Patients.Free, dbo.Shifts.ShiftName, dbo.Personel.PersonelName, dbo.Patients.UserID");
            _ds = DB.GetData();
            radGridView1.DataSource = _ds.Tables[0];
            DB.exec();
            sort();
        }

        private void radGridView1_SortChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            sort();
        }

        private void radGridView1_CellClick(object sender, GridViewCellEventArgs e)
        {
            try
            {
                Pid = radGridView1.SelectedRows[0].Cells[1].Value.ToString();
                userID = Convert.ToInt16(radGridView1.SelectedRows[0].Cells[13].Value);
            }
            catch
            {
            }


            DB.SetCommand(@"SELECT dbo.Patients.patientid, dbo.Patients.fname, dbo.Patients.lname, dbo.Patients.reg_date, dbo.Sys_Users.uname, dbo.Patients.hourz, dbo.Patients.Free, dbo.khUsage.PersonelID AS kh_perID, dbo.Personel.PersonelID, 
                         dbo.Personel.PersonelName, dbo.Patients.doctorID AS p_drID, dbo.Doctors.doctorID AS dr_drID, dbo.Doctors.doctorName, dbo.khUsage.ShiftID AS kh_shID, dbo.Shifts.ShiftID, dbo.Shifts.ShiftName, dbo.Patients.UserID
FROM            dbo.ShftPers INNER JOIN
                         dbo.Personel ON dbo.ShftPers.PersonelID = dbo.Personel.PersonelID INNER JOIN
                         dbo.Shifts ON dbo.ShftPers.ShiftID = dbo.Shifts.ShiftID INNER JOIN
                         dbo.khUsage ON dbo.Personel.PersonelID = dbo.khUsage.PersonelID AND dbo.Shifts.ShiftID = dbo.khUsage.ShiftID INNER JOIN
                         dbo.Doctors INNER JOIN
                         dbo.Patients ON dbo.Doctors.doctorID = dbo.Patients.doctorID ON dbo.khUsage.PatientID = dbo.Patients.patientid INNER JOIN
                         dbo.Sys_Users ON dbo.Patients.UserID = dbo.Sys_Users.uid
WHERE dbo.patients.patientid = '" + Pid + "'");
            _ds = DB.GetData();
            DB.exec();

            try
            {
                fname.Text = Convert.ToString(_ds.Tables[0].Rows[0]["fname"]);
                lname.Text = Convert.ToString(_ds.Tables[0].Rows[0]["lname"]);
                reg_date.Text = Convert.ToString(_ds.Tables[0].Rows[0]["reg_date"]);

                bool free = Convert.ToBoolean(_ds.Tables[0].Rows[0]["Free"]);
                if (free)
                {
                    radioButton1.Checked = true;
                    radioButton2.Checked = false;
                }
                else
                {
                    radioButton1.Checked = false;
                    radioButton2.Checked = true;
                }
                drBox.DataSource = _ds.Tables[0];
                drBox.DisplayMember = "doctorName";
                drBox.ValueMember = "dr_drID";
                string drID = _ds.Tables[0].Rows[0]["p_drID"].ToString();
                pezeshk();
            drBox.SelectedValue = drID;
        }
            catch
            {
            }
        }

    private void pezeshk()
        {
            DB.SetCommand("Select * From Doctors");
            _ds = DB.GetData();
            DB.exec();
            drBox.DataSource = _ds.Tables[0];
            drBox.DisplayMember = "doctorName";
            drBox.ValueMember = "doctorID";
        }

        private void shift()
        {
            DB.SetCommand("Select * From Shifts");
            DataSet ds1 = DB.GetData();
            DB.exec();
            shiftBox.DataSource = ds1.Tables[0];
            shiftBox.DisplayMember = "ShiftName";
            shiftBox.ValueMember = "ShiftID";
        }

        private void personel()
        {
            DB.SetCommand("Select * From Personel");
            DataSet ds2 = DB.GetData();
            DB.exec();
            personelBox.DataSource = ds2.Tables[0];
            personelBox.DisplayMember = "PersonelName";
            personelBox.ValueMember = "PersonelID";
        }
    }
}