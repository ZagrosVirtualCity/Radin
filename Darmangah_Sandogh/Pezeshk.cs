using System;
using System.Data;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Darmangah_Sandogh
{
    public partial class Pezeshk : RadForm
    {
        readonly MyDB db = new MyDB();
        private DataSet ds = new DataSet();
        private string PID = string.Empty;

        public Pezeshk()
        {
            InitializeComponent();
        }

        private void FillGrid()
        {
            db.SetCommand(@"SELECT * From Doctors");
            ds = db.GetData();
            dataGridView1.DataSource = ds.Tables[0];
            sort();
        }
        private void sort()
        {
            dataGridView1.EnableFiltering = true;
            dataGridView1.MasterTemplate.ShowHeaderCellButtons = true;
            dataGridView1.MasterTemplate.ShowFilteringRow = false;
            dataGridView1.MasterTemplate.BestFitColumns();
            foreach (GridViewDataColumn column in dataGridView1.Columns)
            {
                column.BestFit();
            }
        }
        private void radButton1_Click(object sender, EventArgs e)
        {
            try
            {
                db.SetCommand("Insert into Doctors (doctorName,drPercent) VALUES (@doctorName,@drPercent)");
                db.SetParameter(@"doctorName", txtDrName.Text);
                db.SetParameter(@"drPercent", txtDrPercent.Text);
                db.exec();
                RadMessageBox.Show("افزودن پزشک انجام شد");
                txtDrName.Clear();
                txtDrName.Focus();
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message);
            }
            FillGrid();
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            db.SetCommand("Update doctors set doctorName = @doctorName, drPercent=@drPercent WHERE doctorID = @doctorID");
            db.SetParameter(@"doctorID", PID);
            db.SetParameter(@"doctorName", txtDrName.Text);
            db.SetParameter(@"drPercent", txtDrPercent.Text);

            db.exec();
            FillGrid();
            RadMessageBox.Show("ویرایش پزشک انجام شد");
            txtDrName.Clear();
            txtDrName.Focus();
        }

        private void Pezeshk_Load(object sender, EventArgs e)
        {
            FillGrid();
        }

        private void Pezeshk_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
            }
        }


        private void dataGridView1_UserDeletingRow(object sender, GridViewRowCancelEventArgs e)
        {
            if (MessageBox.Show("آیا مطمئن هستید ؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
DialogResult.Yes)
            {
                db.SetCommand("Delete From doctors WHERE doctorID = @doctorID");
                db.SetParameter(@"doctorID", PID);
                db.exec();
                FillGrid();
            }
            else
            {
                FillGrid();
            }
        }

        private void dataGridView1_CellClick(object sender, GridViewCellEventArgs e)
        {
            try
            {
                PID = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            }
            catch
            {
            }
            db.SetCommand(@"SELECT  * From Doctors WHERE dbo.Doctors.doctorID = '" + PID + "'");
            ds = db.GetData();
            db.exec();

            try
            {
                txtDrName.Text = Convert.ToString(ds.Tables[0].Rows[0]["doctorName"]);
                txtDrPercent.Text = Convert.ToString(ds.Tables[0].Rows[0]["drPercent"]);
            }
            catch
            {
            }
        }

        private void Pezeshk_ResizeEnd(object sender, EventArgs e)
        {
            sort();
        }

        private void dataGridView1_SortChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            sort();
        }
    }
}