using System;
using System.Data;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Darmangah_Sandogh
{
    public partial class TarefeKhedmat : RadForm
    {
        MyDB db = new MyDB();
        private string khID;
        public TarefeKhedmat()
        {
            InitializeComponent();
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            db.SetCommand(@"Insert into khType (khName,khGrpID,khCost) VALUES (@khName,@khGrpID,@khCost)");
            db.SetParameter(@"khName", radTextBox1.Text);
            db.SetParameter(@"khCost", radTextBox2.Text.Replace(",", string.Empty));
            db.SetParameter(@"khGrpID", comboBox1.SelectedValue);
            db.exec();
            FillGrid();
            RadMessageBox.Show("خدمت با موفقیت افزوده شد");
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            db.SetCommand(@"Update khType set khName = @khName , khGrpID = @khGrpID , khCost = @khCost where khID = @khID");
            db.SetParameter(@"khName", radTextBox1.Text);
            db.SetParameter(@"khCost", radTextBox2.Text.Replace(",", string.Empty));
            db.SetParameter(@"khGrpID", comboBox1.SelectedValue);
            db.SetParameter(@"khID", khID);
            db.exec();
            FillGrid();
            RadMessageBox.Show("خدمت با موفقیت ویرایش شد");
        }

        private void TarefeKhedmat_Load(object sender, EventArgs e)
        {
            khGroup();
            FillGrid();
        }

        private void khGroup()
        {
            db.SetCommand("Select * From khGroup");
            DataSet ds = db.GetData();
            comboBox1.DataSource = ds.Tables[0];
            comboBox1.DisplayMember = "khGrpName";
            comboBox1.ValueMember = "khGrpID";
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
            khID = radGridView1.SelectedRows[0].Cells[1].Value.ToString();

            db.SetCommand(@"SELECT        dbo.khType.khID, dbo.khType.khName, dbo.khType.khCost, dbo.khGroup.khGrpName, dbo.khGroup.khGrpID, dbo.khType.khGrpID AS khgpID
FROM            dbo.khGroup INNER JOIN
                         dbo.khType ON dbo.khGroup.khGrpID = dbo.khType.khGrpID where khType.khID = '" + khID + "'");
            DataSet ds = db.GetData();

            try
            {
                radTextBox1.Text = ds.Tables[0].Rows[0]["khName"].ToString();
                radTextBox2.Text = ds.Tables[0].Rows[0]["khCost"].ToString();

                comboBox1.DataSource = ds.Tables[0];
                comboBox1.DisplayMember = "khGrpName";
                comboBox1.ValueMember = "khGrpID";
                string drID = ds.Tables[0].Rows[0]["khGrpID"].ToString();
                khGroup();
                comboBox1.SelectedValue = drID;
            }
            catch
            {
            }
        }

        private void FillGrid()
        {
            db.SetCommand(@"SELECT ROW_NUMBER() OVER(ORDER BY dbo.khType.khID) AS Radif, dbo.khType.khID, dbo.khGroup.khGrpName, dbo.khType.khName, dbo.khType.khCost
FROM            dbo.khGroup INNER JOIN
                         dbo.khType ON dbo.khGroup.khGrpID = dbo.khType.khGrpID");
            DataSet ds = db.GetData();
            radGridView1.DataSource = ds.Tables[0];
            sort();
        }

        private void radGridView1_UserDeletingRow(object sender, GridViewRowCancelEventArgs e)
        {
            if (MessageBox.Show("آیا مطمئن هستید ؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
    DialogResult.Yes)
            {
                db.SetCommand(@"Delete From khType where khID = @khID");
                db.SetParameter(@"khID", khID);
                db.exec();

                FillGrid();
            }
            else
            {
                FillGrid();
            }
        }

        private void radTextBox2_TextChanged(object sender, EventArgs e)
        {
            if (radTextBox2.Text != "0")
            {
                try
                {
                    if (!(string.IsNullOrEmpty(radTextBox2.Text)))
                    {
                        String S = radTextBox2.Text.Replace(",", "");
                        radTextBox2.Text = (Convert.ToInt64(S)).ToString("#,#");
                        radTextBox2.SelectionStart = radTextBox2.Text.Length;
                        radTextBox2.Focus();
                    }
                }
                catch
                {
                    radTextBox2.Text = "0";
                }
            }
        }
    }
}
