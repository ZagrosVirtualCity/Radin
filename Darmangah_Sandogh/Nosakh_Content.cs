using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml;
using Stimulsoft.Report;
using Telerik.WinControls.UI;

namespace Darmangah_Sandogh
{
    public partial class Nosakh_Content : Form
    {
        readonly MyDB db = new MyDB();
        private DataSet ds = new DataSet();
        private string PID = string.Empty;
        private string khid = string.Empty;
        private object khuid = string.Empty;
        StiReport stiReport1 = new StiReport();
        string path = "Reports\\Recipt.mrt";
        private double tarif;
        private double nurseP;
        private double secP;
        private double drP;
        private double nurseCost;
        private double secCost;
        private double drCost;
        private int UserID;

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
            ds = db.GetData();
            dataGridView1.DataSource = ds.Tables[0];
            db.exec();
        }


        private void buttonX5_Click(object sender, EventArgs e)
        {
            db.SetCommand(@"SELECT ROW_NUMBER() OVER(ORDER BY dbo.patients.patientid) AS Radif, dbo.Patients.patientid, dbo.Patients.fname, dbo.Patients.lname, dbo.Doctors.doctorName, dbo.Patients.reg_date, dbo.Shifts.ShiftName, dbo.Personel.PersonelName, dbo.patients.UserID
FROM            dbo.Patients INNER JOIN
                         dbo.Doctors ON dbo.Patients.doctorID = dbo.Doctors.doctorID INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN
                         dbo.Personel ON dbo.khUsage.PersonelID = dbo.Personel.PersonelID INNER JOIN
                         dbo.ShftPers ON dbo.Personel.PersonelID = dbo.ShftPers.PersonelID INNER JOIN
                         dbo.Shifts ON dbo.khUsage.ShiftID = dbo.Shifts.ShiftID AND dbo.ShftPers.ShiftID = dbo.Shifts.ShiftID
WHERE dbo.patients.reg_date BETWEEN '" + uc1.Shamsi + "' AND '" + uc2.Shamsi + "' group by dbo.Patients.patientid, dbo.Patients.fname, dbo.Patients.lname, dbo.Doctors.doctorName, dbo.Patients.reg_date, dbo.Shifts.ShiftName, dbo.Personel.PersonelName, dbo.patients.UserID");
            ds = db.GetData();
            dataGridView1.DataSource = ds.Tables[0];
            db.exec();
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            db.SetCommand(@"SELECT ROW_NUMBER() OVER(ORDER BY dbo.patients.patientid) AS Radif, dbo.Patients.patientid, dbo.Patients.fname, dbo.Patients.lname, dbo.Doctors.doctorName, dbo.Patients.reg_date, dbo.Shifts.ShiftName, dbo.Personel.PersonelName, dbo.patients.UserID
FROM            dbo.Patients INNER JOIN
                         dbo.Doctors ON dbo.Patients.doctorID = dbo.Doctors.doctorID INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN
                         dbo.Personel ON dbo.khUsage.PersonelID = dbo.Personel.PersonelID INNER JOIN
                         dbo.ShftPers ON dbo.Personel.PersonelID = dbo.ShftPers.PersonelID INNER JOIN
                         dbo.Shifts ON dbo.khUsage.ShiftID = dbo.Shifts.ShiftID AND dbo.ShftPers.ShiftID = dbo.Shifts.ShiftID
WHERE dbo.patients.fname LIKE N'%" + name_box.Text + "%' AND dbo.patients.lname LIKE N'%" + family_box.Text + "%' group by dbo.Patients.patientid, dbo.Patients.fname, dbo.Patients.lname, dbo.Doctors.doctorName, dbo.Patients.reg_date, dbo.Shifts.ShiftName, dbo.Personel.PersonelName, dbo.patients.UserID");
            ds = db.GetData();
            dataGridView1.DataSource = ds.Tables[0];
            db.exec();
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


                db.SetCommand(@"SELECT ROW_NUMBER() OVER(ORDER BY dbo.patients.patientid) AS Radif, dbo.Patients.patientid, dbo.khUsage.KhUsageID, dbo.khUsage.KhID, dbo.Patients.fname, dbo.Patients.lname, dbo.khGroup.khGrpName, dbo.khType.khName, dbo.khUsage.Num, dbo.patients.UserID
                FROM            dbo.khType INNER JOIN
                                         dbo.khGroup ON dbo.khType.khGrpID = dbo.khGroup.khGrpID INNER JOIN
                                         dbo.khUsage ON dbo.khType.khID = dbo.khUsage.KhID INNER JOIN
                                         dbo.Patients ON dbo.khUsage.PatientID = dbo.Patients.patientid
                WHERE dbo.patients.patientid = '" + PID + "'");
                ds = db.GetData();
                radGridView1.DataSource = ds.Tables[0];
                db.exec();
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
            if (UserID == GlobalVariables.LoggedInUser || UserID == 7)
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
                MessageBox.Show("شما دسترسی حذف رکورد را ندارید");
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

            db.SetCommand(@"Select ShiftID ,PersonelID,nurseID1,nurseID2 from khUsage where patientId = @patientID");
            db.SetParameter(@"patientId", PID);
            ds = db.GetData();
            db.exec();

            string shift = ds.Tables[0].Rows[0]["ShiftID"].ToString();
            string personel = ds.Tables[0].Rows[0]["PersonelID"].ToString();
            string nurseID1 = ds.Tables[0].Rows[0]["nurseID1"].ToString();
            string nurseID2 = ds.Tables[0].Rows[0]["nurseID2"].ToString();

            try
            {
                db.SetCommand("Select khCost From khType where khID = '" + comboBox4.SelectedValue + "'");
                ds = db.GetData();
                db.exec();

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
                if (p3.IsChecked)
                {
                    cmd2.Parameters.AddWithValue(@"depID", 1);
                }
                if (n3.IsChecked)
                {
                    cmd2.Parameters.AddWithValue(@"depID", 2);
                }
                cmd2.Parameters.AddWithValue(@"cost", ds.Tables[0].Rows[0]["khCost"].ToString());
                var returnParameter2 = cmd2.Parameters.Add("@khUID", SqlDbType.Int);
                returnParameter2.Direction = ParameterDirection.ReturnValue;
                cmd2.ExecuteNonQuery();
                khuid = returnParameter2.Value;
                cnn2.Close();

                string khidz = comboBox4.SelectedValue.ToString();
                if (khidz == "20" || khidz == "21" || khidz == "23")
                {
                    db.SetCommand(@"Insert into Salary (khUsageID,drCost,nurseCost,secCost)VALUES(@khUsageID,@drCost,@nurseCost,@secCost)");
                    db.SetParameter(@"khUsageID", khuid);
                    if (p3.IsChecked)
                    {
                        db.SetParameter(@"drCost", textBox1.Text);
                        db.SetParameter(@"nurseCost", 0);
                        db.SetParameter(@"secCost", 0);
                    }
                    else if (n3.IsChecked)
                    {
                        if (khidz == "21")
                        {
                            nurseCost = Convert.ToDouble(textBox1.Text)*45.0/100;
                            secCost = Convert.ToDouble(textBox1.Text)*10.0/100;
                            drCost = Convert.ToDouble(textBox1.Text)*45.0/100;

                            db.SetParameter(@"drCost", drCost);
                            db.SetParameter(@"nurseCost", nurseCost);
                            db.SetParameter(@"secCost", secCost);
                        }
                        else if (khidz == "20")
                        {
                            db.SetParameter(@"drCost", textBox1.Text);
                            db.SetParameter(@"nurseCost", 0);
                            db.SetParameter(@"secCost", 0);
                        }
                        else if (khidz == "23")
                        {
                            db.SetParameter(@"drCost", textBox1.Text);
                            db.SetParameter(@"nurseCost", 0);
                            db.SetParameter(@"secCost", 0);
                        }
                    }

                    db.exec();
                }
                else
                {

                    if (p3.IsChecked)
                    {
                        db.SetCommand(@"Insert into Salary (khUsageID,drCost,nurseCost,secCost)VALUES(@khUsageID,@drCost,@nurseCost,@secCost)");
                        db.SetParameter(@"khUsageID", khuid);
                        db.SetParameter(@"drCost", ds.Tables[0].Rows[0]["khCost"].ToString());
                        db.SetParameter(@"nurseCost", 0);
                        db.SetParameter(@"secCost", 0);
                        db.exec();
                    }
                    else if (n3.IsChecked)
                    {
                        db.SetCommand("Select khCost From khType where khID = '" + comboBox4.SelectedValue + "'");
                        ds = db.GetData();
                        db.exec();

                        db.SetCommand(@"Select NursePercent, SecreteryPercent From ShftPers where ShiftID = @ShiftID AND PersonelID = @PersonelID");
                        db.SetParameter(@"ShiftID", shift);
                        db.SetParameter(@"PersonelID", personel);
                        DataSet dz = db.GetData();
                        db.exec();

                        tarif = Convert.ToDouble(txtProNet6.Text) * Convert.ToDouble(ds.Tables[0].Rows[0]["khCost"].ToString());

                        nurseP = Convert.ToDouble(dz.Tables[0].Rows[0]["NursePercent"]);
                        secP = Convert.ToDouble(dz.Tables[0].Rows[0]["SecreteryPercent"]);
                        drP = 100 - (nurseP + secP);

                        nurseCost = (nurseP * tarif) / 100;
                        secCost = (secP * tarif) / 100;
                        drCost = (drP * tarif) / 100;

                        db.SetCommand(@"Insert into Salary (khUsageID,drCost,nurseCost,secCost)VALUES(@khUsageID,@drCost,@nurseCost,@secCost)");
                        db.SetParameter(@"khUsageID", khuid);
                        db.SetParameter(@"drCost", drCost);
                        db.SetParameter(@"nurseCost", nurseCost);
                        db.SetParameter(@"secCost", secCost);
                        db.exec();

                    }

                }
 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show("بروز خطا در ثبت خدمت");
            }

            cell_Click();
        }

        private void kh_group()
        {
            db.SetCommand("Select * From khGroup");
            DataSet DS2 = db.GetData();
            db.exec();
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
                db.exec();
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
                db.exec();
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
                db.exec();
                comboBox4.DataSource = DS2.Tables[0];
                comboBox4.DisplayMember = "khName";
                comboBox4.ValueMember = "khID";
            }
            catch
            {
            }
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            db.SetCommand(
                @"SELECT fname + ' ' + lname as fullName, reg_date, recipt = N'بیمار', hourz FROM dbo.Patients where patientid = '" +
                PID + "'");
            DataTable info = db.GetData2();
            db.exec();

            try
            {
                db.SetCommand(@"SELECT dbo.khType.khName, dbo.khUsage.Num, dbo.Invoice.cost
FROM            dbo.Patients INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN
                         dbo.khType ON dbo.khUsage.KhID = dbo.khType.khID INNER JOIN
                         dbo.Invoice ON dbo.khUsage.KhUsageID = dbo.Invoice.khUsageID where dbo.khUsage.PatientID = '" +
                              PID + "'");
                DataTable KhUsage = db.GetData2();
                db.exec();


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
                stiReport1.Print(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("1خطا در صدور فیش");
                MessageBox.Show(ex.Message);
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
                    db.exec();

                    db.SetCommand(@"SELECT dbo.khType.khName, dbo.khUsage.Num, dbo.Invoice.cost
FROM            dbo.Patients INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN
                         dbo.khType ON dbo.khUsage.KhID = dbo.khType.khID INNER JOIN
                         dbo.Invoice ON dbo.khUsage.KhUsageID = dbo.Invoice.khUsageID where dbo.khUsage.PatientID = '" + PID + "'");
                    DataTable KhUsage2 = db.GetData2();
                    db.exec();

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
                    stiReport1.Print(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("2خطا در صدور فیش");
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            string khidz = comboBox4.SelectedValue.ToString();
            if (khidz == "20" || khidz == "21" || khidz == "23")
            {
                textBox1.Enabled = true;
            }
            else
            {
                textBox1.Enabled = false;
            }
        }
    }
}