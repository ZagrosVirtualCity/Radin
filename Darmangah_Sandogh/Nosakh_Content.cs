using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;
using System.Xml;
using Stimulsoft.Report;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Darmangah_Sandogh
{
    public partial class Nosakh_Content : RadForm
    {
        readonly MyDB db = new MyDB();
        private string PID = string.Empty;
        private string khid = string.Empty;
        private object khuid = string.Empty;
        StiReport stiReport1 = new StiReport();
        string path = "Reports\\Recipt.mrt";
        private double nurseP;
        private double nurseCost;
        private double centerCost;
        private int UserID;
        private string _reg_date;

        public Nosakh_Content()
        {
            InitializeComponent();
        }

        private void FillGrid()
        {
            db.SetCommand(@"SELECT ROW_NUMBER() OVER(ORDER BY dbo.patients.patientid) AS Radif, dbo.Patients.patientid, dbo.Patients.fname, dbo.Patients.lname, dbo.Doctors.doctorName, dbo.Patients.reg_date, dbo.Shifts.ShiftName, dbo.Personel.PersonelName, dbo.patients.UserID
FROM            dbo.Patients INNER JOIN
                         dbo.Doctors ON dbo.Patients.doctorID = dbo.Doctors.doctorID INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN
                         dbo.Personel ON dbo.khUsage.PersonelID = dbo.Personel.PersonelID INNER JOIN
                         dbo.ShftPers ON dbo.Personel.PersonelID = dbo.ShftPers.PersonelID INNER JOIN
                         dbo.Shifts ON dbo.khUsage.ShiftID = dbo.Shifts.ShiftID AND dbo.ShftPers.ShiftID = dbo.Shifts.ShiftID
WHERE dbo.patients.reg_date BETWEEN '" + PersianDateTime.Now.ToString("yyyy/MM/dd") + "' AND '" + PersianDateTime.Now.ToString("yyyy/MM/dd") + "' group by dbo.Patients.patientid, dbo.Patients.fname, dbo.Patients.lname, dbo.Doctors.doctorName, dbo.Patients.reg_date, dbo.Shifts.ShiftName, dbo.Personel.PersonelName, dbo.patients.UserID");
            DataTable _dtFilGrid = db.GetData2();
            dataGridView1.DataSource = _dtFilGrid;
        }

        private void Patients_Edit_Load(object sender, EventArgs e)
        {
            string now = PersianDateTime.Now.ToString("yyyy/MM/dd");
            string[] str = now.Split('/');
            uc1.Date = str[0] + "/" + str[1] + "/" + "01";
            uc2.Date = str[0] + "/" + str[1] + "/" + "31";

            FillGrid();
            kh_group();
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


                db.SetCommand(@"SELECT ROW_NUMBER() OVER(ORDER BY dbo.patients.patientid) AS Radif, dbo.Patients.patientid, dbo.khUsage.KhUsageID, dbo.khUsage.KhID, dbo.Patients.fname, dbo.Patients.lname, dbo.khGroup.khGrpName, dbo.khType.khName, dbo.khUsage.Num, dbo.patients.UserID
                FROM            dbo.khType INNER JOIN
                                         dbo.khGroup ON dbo.khType.khGrpID = dbo.khGroup.khGrpID INNER JOIN
                                         dbo.khUsage ON dbo.khType.khID = dbo.khUsage.KhID INNER JOIN
                                         dbo.Patients ON dbo.khUsage.PatientID = dbo.Patients.patientid
                WHERE dbo.patients.patientid = '" + PID + "'");
                DataTable _dtFillGridByPID = db.GetData2();
                radGridView1.DataSource = _dtFillGridByPID;
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
                int a = DifDate(_reg_date, PersianDateTime.Now.ToString("yyyy/MM/dd"));
                if (a == 0)
                {
                    if (MessageBox.Show("آیا مطمئن هستید ؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                    DialogResult.Yes)
                    {

                        db.SetCommand("Delete From khUsage WHERE khUsageID = @khUsageID");
                        db.SetParameter(@"khUsageID", khuid);
                        db.exec();
                        cell_Click();
                    }
                    else
                    {
                        cell_Click();
                    }
                }
                else
                {
                    RadMessageBox.Show("زمان حذف این رکورد گذشته است و نمی توان آن را حذف کرد");
                }
            }
            else
            {
                RadMessageBox.Show("شما دسترسی حذف رکورد را ندارید");
            }
        }

        private void radGridView1_CellClick(object sender, GridViewCellEventArgs e)
        {
            PID = radGridView1.SelectedRows[0].Cells[1].Value.ToString();
            khid = radGridView1.SelectedRows[0].Cells[3].Value.ToString();
            khuid = radGridView1.SelectedRows[0].Cells[2].Value.ToString();
            UserID = Convert.ToInt16(radGridView1.SelectedRows[0].Cells[9].Value);
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            db.SetCommand(@"Select NursePercent From ShftPers where ShiftID = @ShiftID AND PersonelID = @PersonelID");
            db.SetParameter(@"ShiftID", GlobalVariables.shftID);
            db.SetParameter(@"PersonelID", GlobalVariables.persID);
            DataTable _NursePercent = db.GetData2();

            nurseP = Convert.ToDouble(_NursePercent.Rows[0]["NursePercent"]);


            db.SetCommand(@"Select ShiftID,PersonelID,nurseID1,nurseID2 from khUsage where patientId = @patientID");
            db.SetParameter(@"patientId", PID);
            DataTable ShiftInfoByKhUsage = db.GetData2();

            string shift = ShiftInfoByKhUsage.Rows[0]["ShiftID"].ToString();
            string personel = ShiftInfoByKhUsage.Rows[0]["PersonelID"].ToString();
            string nurseID1 = ShiftInfoByKhUsage.Rows[0]["nurseID1"].ToString();
            string nurseID2 = ShiftInfoByKhUsage.Rows[0]["nurseID2"].ToString();

            try
            {
                db.SetCommand("Select khCost From khType where khID = '" + comboBox4.SelectedValue + "'");
                DataTable _khCost = db.GetData2();

                nurseCost = (nurseP * Convert.ToInt32(_khCost.Rows[0]["khCost"])) / 100;
                centerCost = ((100 - nurseP) * Convert.ToInt32(_khCost.Rows[0]["khCost"])) / 100;

                var cnn2 = new SqlConnection(db.Cnn);
                cnn2.Open();
                SqlCommand cmd2 = cnn2.CreateCommand();
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.CommandText = "KhAdd";
                cmd2.Parameters.AddWithValue(@"PatientID", PID);
                cmd2.Parameters.AddWithValue(@"ShiftID", shift);
                cmd2.Parameters.AddWithValue(@"PersonelID", personel);
                cmd2.Parameters.AddWithValue(@"nurseID1", nurseID1);
                cmd2.Parameters.AddWithValue(@"nurseID2", nurseID2);
                cmd2.Parameters.AddWithValue(@"KhID", comboBox4.SelectedValue);
                cmd2.Parameters.AddWithValue(@"Num", txtProNet6.Text);
                if (comboBox2.SelectedValue.ToString() == "1")
                {
                    cmd2.Parameters.AddWithValue(@"depID", 1);
                }
                else
                {
                    cmd2.Parameters.AddWithValue(@"depID", 2);
                }
                cmd2.Parameters.AddWithValue(@"cost", _khCost.Rows[0]["khCost"].ToString());
                var returnParameter2 = cmd2.Parameters.Add("@khUID", SqlDbType.Int);
                returnParameter2.Direction = ParameterDirection.ReturnValue;
                cmd2.ExecuteNonQuery();
                khuid = returnParameter2.Value;
                cnn2.Close();


                db.SetCommand(@"Insert into Salary (khUsageID,nurseCost,centerCost,gender)VALUES(@khUsageID,@nurseCost,@centerCost,@gender)");
                db.SetParameter(@"khUsageID", khuid);
                if (comboBox2.SelectedValue.ToString() == "1")
                {
                    db.SetParameter(@"nurseCost", 0);
                    db.SetParameter(@"centerCost", _khCost.Rows[0]["khCost"].ToString());
                    db.SetParameter(@"gender", DBNull.Value);
                }
                else
                {
                    db.SetParameter(@"centerCost", centerCost);
                    db.SetParameter(@"nurseCost", nurseCost);
                    db.SetParameter(@"gender", DBNull.Value);

                }
                db.exec();
            }
            catch (Exception ex)
            {
                LogDA.LogError(ex.Message);
                RadMessageBox.Show("بروز خطا در ثبت خدمت");
            }

            cell_Click();
        }

        private void kh_group()
        {
            db.SetCommand("Select * From khGroup");
            DataSet DS2 = db.GetData();
            comboBox2.DataSource = DS2.Tables[0];
            comboBox2.DisplayMember = "khGrpName";
            comboBox2.ValueMember = "khGrpID";
        }

        private void comboBox2_Leave(object sender, EventArgs e)
        {
            try
            {
                comboBox4.DataSource = null;
                db.SetCommand("Select * From khType where khGrpID = @khGrpID");
                db.SetParameter(@"khGrpID", comboBox2.SelectedValue);
                DataSet DS2 = db.GetData();
                comboBox4.DataSource = DS2.Tables[0];
                comboBox4.DisplayMember = "khName";
                comboBox4.ValueMember = "khID";
            }
            catch
            {
            }
        }

        private void comboBox2_Click(object sender, EventArgs e)
        {

            try
            {
                comboBox4.DataSource = null;
                db.SetCommand("Select * From khType where khGrpID = @khGrpID");
                db.SetParameter(@"khGrpID", comboBox2.SelectedValue);
                DataSet DS2 = db.GetData();
                comboBox4.DataSource = DS2.Tables[0];
                comboBox4.DisplayMember = "khName";
                comboBox4.ValueMember = "khID";
            }
            catch
            {
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                comboBox4.DataSource = null;
                db.SetCommand("Select * From khType where khGrpID = @khGrpID");
                db.SetParameter(@"khGrpID", comboBox2.SelectedValue);
                DataSet DS2 = db.GetData();
                comboBox4.DataSource = DS2.Tables[0];
                comboBox4.DisplayMember = "khName";
                comboBox4.ValueMember = "khID";

                if (comboBox4.SelectedIndex == 1)
                {
                    radLabel3.Visible = false;
                    gender.Visible = false;
                }
                else
                {
                    radLabel3.Visible = true;
                    gender.Visible = true;
                }
            }
            catch
            {
            }
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            db.SetCommand(@"SELECT fname + ' ' + lname as fullName, reg_date, recipt = N'بیمار', hourz FROM dbo.Patients where patientid = '" +
                PID + "'");
            DataTable info = db.GetData2();

            try
            {
                db.SetCommand(@"SELECT dbo.khType.khName, dbo.khUsage.Num, dbo.Invoice.cost
FROM            dbo.Patients INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN
                         dbo.khType ON dbo.khUsage.KhID = dbo.khType.khID INNER JOIN
                         dbo.Invoice ON dbo.khUsage.KhUsageID = dbo.Invoice.khUsageID where dbo.khUsage.PatientID = '" +
                              PID + "'");
                DataTable KhUsage = db.GetData2();


                stiReport1.Load(path);
                stiReport1.ResetRenderedState();
                DataSet ds2 = new DataSet();
                info.TableName = "info";
                KhUsage.TableName = "KhUsage";
                ds2.Tables.Add(info);
                ds2.Tables.Add(KhUsage);
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
            catch (Exception ex)
            {
                RadMessageBox.Show("1خطا در صدور فیش");
                LogDA.LogError(ex.Message);
            }
            ///////////////////////
            if (MessageBox.Show("آیا مایل به چاپ رسید المثنی می باشید؟", "چاپ رسید المثنی", MessageBoxButtons.OKCancel,
                    MessageBoxIcon.None, MessageBoxDefaultButton.Button1) == DialogResult.OK)
            {
                try
                {
                    db.SetCommand(@"SELECT fname + ' ' + lname as fullName, reg_date, recipt = N'المثنی', hourz FROM dbo.Patients where patientid = '" +
                        PID + "'");
                    DataTable info2 = db.GetData2();

                    db.SetCommand(@"SELECT dbo.khType.khName, dbo.khUsage.Num, dbo.Invoice.cost
FROM            dbo.Patients INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN
                         dbo.khType ON dbo.khUsage.KhID = dbo.khType.khID INNER JOIN
                         dbo.Invoice ON dbo.khUsage.KhUsageID = dbo.Invoice.khUsageID where dbo.khUsage.PatientID = '" + PID + "'");
                    DataTable KhUsage2 = db.GetData2();

                    stiReport1.Load(path);
                    stiReport1.ResetRenderedState();
                    DataSet ds3 = new DataSet();
                    info2.TableName = "info";
                    KhUsage2.TableName = "KhUsage";
                    ds3.Tables.Add(info2);
                    ds3.Tables.Add(KhUsage2);
                    stiReport1.Dictionary.Clear();
                    stiReport1.RegData(ds3);
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
                catch (Exception ex)
                {
                    RadMessageBox.Show("2خطا در صدور فیش");
                    LogDA.LogError(ex.Message);
                }
            }
        }

        private void btnDateSearch_Click(object sender, EventArgs e)
        {
            db.SetCommand(@"SELECT ROW_NUMBER() OVER(ORDER BY dbo.patients.patientid) AS Radif, dbo.Patients.patientid, dbo.Patients.fname, dbo.Patients.lname, dbo.Doctors.doctorName, dbo.Patients.reg_date, dbo.Shifts.ShiftName, dbo.Personel.PersonelName, dbo.patients.UserID
FROM            dbo.Patients INNER JOIN
                         dbo.Doctors ON dbo.Patients.doctorID = dbo.Doctors.doctorID INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN
                         dbo.Personel ON dbo.khUsage.PersonelID = dbo.Personel.PersonelID INNER JOIN
                         dbo.ShftPers ON dbo.Personel.PersonelID = dbo.ShftPers.PersonelID INNER JOIN
                         dbo.Shifts ON dbo.khUsage.ShiftID = dbo.Shifts.ShiftID AND dbo.ShftPers.ShiftID = dbo.Shifts.ShiftID
WHERE dbo.patients.reg_date BETWEEN '" + uc1.Date + "' AND '" + uc2.Date + "' group by dbo.Patients.patientid, dbo.Patients.fname, dbo.Patients.lname, dbo.Doctors.doctorName, dbo.Patients.reg_date, dbo.Shifts.ShiftName, dbo.Personel.PersonelName, dbo.patients.UserID");
            DataTable _dtFillGridByDate = db.GetData2();
            dataGridView1.DataSource = _dtFillGridByDate;
        }

        private void btnFullNameSearch_Click(object sender, EventArgs e)
        {
            db.SetCommand(@"SELECT ROW_NUMBER() OVER(ORDER BY dbo.patients.patientid) AS Radif, dbo.Patients.patientid, dbo.Patients.fname, dbo.Patients.lname, dbo.Doctors.doctorName, dbo.Patients.reg_date, dbo.Shifts.ShiftName, dbo.Personel.PersonelName, dbo.patients.UserID
FROM            dbo.Patients INNER JOIN
                         dbo.Doctors ON dbo.Patients.doctorID = dbo.Doctors.doctorID INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN
                         dbo.Personel ON dbo.khUsage.PersonelID = dbo.Personel.PersonelID INNER JOIN
                         dbo.ShftPers ON dbo.Personel.PersonelID = dbo.ShftPers.PersonelID INNER JOIN
                         dbo.Shifts ON dbo.khUsage.ShiftID = dbo.Shifts.ShiftID AND dbo.ShftPers.ShiftID = dbo.Shifts.ShiftID
WHERE dbo.patients.fname LIKE N'%" + name_box.Text + "%' AND dbo.patients.lname LIKE N'%" + family_box.Text + "%' group by dbo.Patients.patientid, dbo.Patients.fname, dbo.Patients.lname, dbo.Doctors.doctorName, dbo.Patients.reg_date, dbo.Shifts.ShiftName, dbo.Personel.PersonelName, dbo.patients.UserID");
            DataTable _dtFillGridByName = db.GetData2();
            dataGridView1.DataSource = _dtFillGridByName;
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