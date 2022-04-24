using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using Stimulsoft.Report;
using Telerik.WinControls.UI;
using Telerik.WinControls;

namespace Darmangah_Sandogh
{
    public partial class Patients_Edit : RadForm
    {
        private MyDB DB = new MyDB();
        private DataSet _ds;
        public string Pid = string.Empty;
        public int userID;
        private string khDate;

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
            sort();
        }

        private void Patients_Edit_Load(object sender, EventArgs e)
        {
            string now = PersianDateTime.Now.ToString("yyyy/MM/dd");
            string[] str = now.Split('/');
            uc1.Date = str[0] + "/" + str[1] + "/" + "01";
            uc2.Date = str[0] + "/" + str[1] + "/" + "31";

            FillGrid();
            pezeshk();

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

            try
            {
                fname.Text = _ds.Tables[0].Rows[0]["fname"].ToString();
                lname.Text = _ds.Tables[0].Rows[0]["lname"].ToString();
                reg_date.Text = _ds.Tables[0].Rows[0]["reg_date"].ToString();
                khDate = _ds.Tables[0].Rows[0]["reg_date"].ToString();

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
            drBox.DataSource = _ds.Tables[0];
            drBox.DisplayMember = "doctorName";
            drBox.ValueMember = "doctorID";
        }

        private void btnDateSearch_Click(object sender, EventArgs e)
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
WHERE dbo.patients.reg_date BETWEEN '" + uc1.Date + "' AND '" + uc2.Date + "' GROUP BY dbo.Patients.patientid, dbo.Patients.fname, dbo.Patients.lname, dbo.Doctors.doctorName, dbo.Patients.reg_date, dbo.Sys_Users.uname, dbo.Patients.hourz, dbo.Patients.Free, dbo.Shifts.ShiftName, dbo.Personel.PersonelName, dbo.Patients.UserID");
            _ds = DB.GetData();
            radGridView1.DataSource = _ds.Tables[0];
            sort();
        }

        private void btnFullNameSearch_Click(object sender, EventArgs e)
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
            sort();
        }

        private void btnSabt_Click(object sender, EventArgs e)
        {
            if (GlobalVariables.EditAccess)
            {
                int a = DifDate(khDate, PersianDateTime.Now.ToString("yyyy/MM/dd"));
                if (a == 0)
                {
                    DB.SetCommand(@"Update patients SET fname = @fname, lname = @lname,  doctorID = @doctorID, reg_date = @reg_date, Free = @Free WHERE patientid = @patientid");
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
                    DB.SetParameter(@"ShiftID", GlobalVariables.shftID);
                    DB.SetParameter(@"PersonelID", GlobalVariables.persID);
                    DB.exec();

                    RadMessageBox.Show("عملیات با موفقیت انجام شد");
                    FillGrid();
                }
                else
                {
                    RadMessageBox.Show("زمان ویرایش این رکورد گذشته است و نمی توان آن را ویرایش کرد");
                }

            }
            else
            {
                RadMessageBox.Show("شما دسترسی ویرایش این رکورد را ندارید");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (GlobalVariables.DelAccess)
            {
                if (MessageBox.Show("آیا مطمئن هستید ؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                DialogResult.Yes)
                {
                    try
                    {
                        int a = DifDate(khDate, PersianDateTime.Now.ToString("yyyy/MM/dd"));
                        if (a == 0)
                        {
                            DB.SetCommand("Delete From patients WHERE patientid = @patientid");
                            DB.SetParameter(@"patientid", Pid);
                            DB.exec();
                            RadMessageBox.Show("بیمار حذف شد");
                            FillGrid();
                        }
                        else
                        {
                            RadMessageBox.Show("زمان حذف این رکورد گذشته است و نمی توان آن را حذف کرد");
                        }
                    }
                    catch
                    {
                        RadMessageBox.Show("خطا در حذف بیمار");
                    }
                }
            }
            else
            {
                RadMessageBox.Show("شما دسترسی حذف این رکورد را ندارید");
            }
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