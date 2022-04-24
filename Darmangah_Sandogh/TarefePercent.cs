using System;
using System.Data;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Darmangah_Sandogh
{
    public partial class TarefePercent : RadForm
    {
        MyDB db = new MyDB();
        private string sID;
        public TarefePercent()
        {
            InitializeComponent();
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            db.SetCommand(@"Update ShftPers set NursePercent = @NursePercent where ShftPersID = @ShftPersID");
            db.SetParameter(@"NursePercent", radTextBox1.Text);
            db.SetParameter(@"ShftPersID", sID);
            db.exec();
            FillGrid();
            RadMessageBox.Show("تعرفه با موفقیت ویرایش شد");
        }

        private void TarefeKhedmat_Load(object sender, EventArgs e)
        {
            shift();
            nurse();
            FillGrid();
        }

        private void shift()
        {
            db.SetCommand("Select * From Shifts");
            DataSet ds = db.GetData();
            comboBox1.DataSource = ds.Tables[0];
            comboBox1.DisplayMember = "ShiftName";
            comboBox1.ValueMember = "ShiftID";
        }
        private void nurse()
        {
            db.SetCommand("Select * From Personel");
            DataSet ds = db.GetData();
            comboBox2.DataSource = ds.Tables[0];
            comboBox2.DisplayMember = "PersonelName";
            comboBox2.ValueMember = "PersonelID";
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

        private void radGridView1_CellClick(object sender, GridViewCellEventArgs e)
        {
            sID = radGridView1.SelectedRows[0].Cells[1].Value.ToString();

            db.SetCommand(@"SELECT dbo.Personel.PersonelName, dbo.ShftPers.NursePercent, dbo.Personel.PersonelID, dbo.ShftPers.PersonelID AS p_pid, dbo.Shifts.ShiftName, dbo.ShftPers.ShftPersID, 
                         dbo.Shifts.ShiftID, dbo.ShftPers.ShiftID AS p_sid
FROM            dbo.Personel INNER JOIN
                         dbo.ShftPers ON dbo.Personel.PersonelID = dbo.ShftPers.PersonelID INNER JOIN
                         dbo.Shifts ON dbo.ShftPers.ShiftID = dbo.Shifts.ShiftID where ShftPers.ShftPersID = '" + sID + "'");
            DataSet ds = db.GetData();

            try
            {
                radTextBox1.Text = ds.Tables[0].Rows[0]["NursePercent"].ToString();

                comboBox1.DataSource = ds.Tables[0];
                comboBox1.DisplayMember = "ShiftName";
                comboBox1.ValueMember = "ShiftID";
                string shiftID = ds.Tables[0].Rows[0]["p_sid"].ToString();
                shift();
                comboBox1.SelectedValue = shiftID;
                //////////////
                comboBox2.DataSource = ds.Tables[0];
                comboBox2.DisplayMember = "PersonelName";
                comboBox2.ValueMember = "PersonelID";
                string perID = ds.Tables[0].Rows[0]["PersonelID"].ToString();
                nurse();
                comboBox2.SelectedValue = perID;
            }
            catch
            {
            }
        }

        private void FillGrid()
        {
            db.SetCommand(@"SELECT ROW_NUMBER() OVER(ORDER BY dbo.ShftPers.ShftPersID) AS Radif, dbo.ShftPers.ShftPersID, dbo.Shifts.ShiftName, dbo.Personel.PersonelName, dbo.ShftPers.NursePercent
FROM            dbo.Personel INNER JOIN
                         dbo.ShftPers ON dbo.Personel.PersonelID = dbo.ShftPers.PersonelID INNER JOIN
                         dbo.Shifts ON dbo.ShftPers.ShiftID = dbo.Shifts.ShiftID");
            DataSet ds = db.GetData();
            radGridView1.DataSource = ds.Tables[0];
            sort();
        }
    }
}
