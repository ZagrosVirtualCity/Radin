namespace Darmangah_Sandogh
{
    partial class DrugList
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn8 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn9 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn10 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn11 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn12 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn13 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn14 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition2 = new Telerik.WinControls.UI.TableViewDefinition();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Radif = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.patientid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.doctorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reg_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ShiftName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PersonelName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label28 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.buttonX5 = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.uc2 = new BPersianCalender.BPersianCalenderTextBox();
            this.uc1 = new BPersianCalender.BPersianCalenderTextBox();
            this.family_box = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.name_box = new System.Windows.Forms.TextBox();
            this.buttonX3 = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel2 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.radGridView1 = new Telerik.WinControls.UI.RadGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtProNet6 = new System.Windows.Forms.TextBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.radButton1 = new Telerik.WinControls.UI.RadButton();
            this.label10 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupPanel1.SuspendLayout();
            this.groupPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Radif,
            this.patientid,
            this.fname,
            this.lname,
            this.doctorName,
            this.reg_date,
            this.ShiftName,
            this.PersonelName});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Top;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1239, 153);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.Click += new System.EventHandler(this.dataGridView1_Click);
            // 
            // Radif
            // 
            this.Radif.DataPropertyName = "Radif";
            this.Radif.HeaderText = "ردیف";
            this.Radif.Name = "Radif";
            this.Radif.Width = 70;
            // 
            // patientid
            // 
            this.patientid.DataPropertyName = "patientid";
            this.patientid.HeaderText = "کد";
            this.patientid.Name = "patientid";
            this.patientid.Width = 70;
            // 
            // fname
            // 
            this.fname.DataPropertyName = "fname";
            this.fname.HeaderText = "نام";
            this.fname.Name = "fname";
            // 
            // lname
            // 
            this.lname.DataPropertyName = "lname";
            this.lname.HeaderText = "نام خانوادگی";
            this.lname.Name = "lname";
            // 
            // doctorName
            // 
            this.doctorName.DataPropertyName = "doctorName";
            this.doctorName.HeaderText = "پزشک";
            this.doctorName.Name = "doctorName";
            // 
            // reg_date
            // 
            this.reg_date.DataPropertyName = "reg_date";
            this.reg_date.HeaderText = "تاریخ پذیرش";
            this.reg_date.Name = "reg_date";
            // 
            // ShiftName
            // 
            this.ShiftName.DataPropertyName = "ShiftName";
            this.ShiftName.HeaderText = "شیفت";
            this.ShiftName.Name = "ShiftName";
            // 
            // PersonelName
            // 
            this.PersonelName.DataPropertyName = "PersonelName";
            this.PersonelName.HeaderText = "پرستار";
            this.PersonelName.Name = "PersonelName";
            // 
            // label28
            // 
            this.label28.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label28.AutoSize = true;
            this.label28.BackColor = System.Drawing.Color.Transparent;
            this.label28.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label28.Location = new System.Drawing.Point(296, 17);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(16, 13);
            this.label28.TabIndex = 4;
            this.label28.Text = "از";
            // 
            // label27
            // 
            this.label27.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label27.AutoSize = true;
            this.label27.BackColor = System.Drawing.Color.Transparent;
            this.label27.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.Location = new System.Drawing.Point(136, 17);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(27, 13);
            this.label27.TabIndex = 6;
            this.label27.Text = "الی";
            // 
            // buttonX5
            // 
            this.buttonX5.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonX5.ColorTable = DevComponents.DotNetBar.eButtonColor.BlueOrb;
            this.buttonX5.Location = new System.Drawing.Point(120, 53);
            this.buttonX5.Name = "buttonX5";
            this.buttonX5.Size = new System.Drawing.Size(75, 23);
            this.buttonX5.TabIndex = 4;
            this.buttonX5.Text = "جست و جو";
            this.buttonX5.Click += new System.EventHandler(this.buttonX5_Click);
            // 
            // groupPanel1
            // 
            this.groupPanel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.uc2);
            this.groupPanel1.Controls.Add(this.buttonX5);
            this.groupPanel1.Controls.Add(this.uc1);
            this.groupPanel1.Controls.Add(this.label27);
            this.groupPanel1.Controls.Add(this.label28);
            this.groupPanel1.DisabledBackColor = System.Drawing.Color.Empty;
            this.groupPanel1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.groupPanel1.Location = new System.Drawing.Point(628, 159);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(326, 105);
            // 
            // 
            // 
            this.groupPanel1.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel1.Style.BackColorGradientAngle = 90;
            this.groupPanel1.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderBottomWidth = 1;
            this.groupPanel1.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderLeftWidth = 1;
            this.groupPanel1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderRightWidth = 1;
            this.groupPanel1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderTopWidth = 1;
            this.groupPanel1.Style.CornerDiameter = 4;
            this.groupPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel1.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel1.TabIndex = 245;
            this.groupPanel1.Text = "بازه زمانی";
            // 
            // uc2
            // 
            this.uc2.Location = new System.Drawing.Point(9, 14);
            this.uc2.Miladi = new System.DateTime(((long)(0)));
            this.uc2.Name = "uc2";
            this.uc2.NowDateSelected = true;
            this.uc2.ReadOnly = true;
            this.uc2.SelectedDate = "";
            this.uc2.Shamsi = null;
            this.uc2.Size = new System.Drawing.Size(121, 21);
            this.uc2.TabIndex = 354;
            // 
            // uc1
            // 
            this.uc1.Location = new System.Drawing.Point(169, 14);
            this.uc1.Miladi = new System.DateTime(((long)(0)));
            this.uc1.Name = "uc1";
            this.uc1.NowDateSelected = true;
            this.uc1.ReadOnly = true;
            this.uc1.SelectedDate = "";
            this.uc1.Shamsi = null;
            this.uc1.Size = new System.Drawing.Size(121, 21);
            this.uc1.TabIndex = 353;
            // 
            // family_box
            // 
            this.family_box.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.family_box.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.family_box.Location = new System.Drawing.Point(5, 17);
            this.family_box.Name = "family_box";
            this.family_box.Size = new System.Drawing.Size(100, 21);
            this.family_box.TabIndex = 89;
            // 
            // label26
            // 
            this.label26.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label26.AutoSize = true;
            this.label26.BackColor = System.Drawing.Color.Transparent;
            this.label26.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label26.ForeColor = System.Drawing.Color.Black;
            this.label26.Location = new System.Drawing.Point(303, 21);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(22, 13);
            this.label26.TabIndex = 87;
            this.label26.Text = "نام";
            // 
            // label25
            // 
            this.label25.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label25.AutoSize = true;
            this.label25.BackColor = System.Drawing.Color.Transparent;
            this.label25.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label25.ForeColor = System.Drawing.Color.Black;
            this.label25.Location = new System.Drawing.Point(111, 22);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(75, 13);
            this.label25.TabIndex = 90;
            this.label25.Text = "نام خانوادگی";
            // 
            // name_box
            // 
            this.name_box.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.name_box.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.name_box.Location = new System.Drawing.Point(197, 18);
            this.name_box.Name = "name_box";
            this.name_box.Size = new System.Drawing.Size(100, 21);
            this.name_box.TabIndex = 86;
            // 
            // buttonX3
            // 
            this.buttonX3.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonX3.ColorTable = DevComponents.DotNetBar.eButtonColor.BlueOrb;
            this.buttonX3.Location = new System.Drawing.Point(125, 57);
            this.buttonX3.Name = "buttonX3";
            this.buttonX3.Size = new System.Drawing.Size(75, 23);
            this.buttonX3.TabIndex = 91;
            this.buttonX3.Text = "جست و جو";
            this.buttonX3.Click += new System.EventHandler(this.buttonX3_Click);
            // 
            // groupPanel2
            // 
            this.groupPanel2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.groupPanel2.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel2.Controls.Add(this.buttonX3);
            this.groupPanel2.Controls.Add(this.name_box);
            this.groupPanel2.Controls.Add(this.label25);
            this.groupPanel2.Controls.Add(this.label26);
            this.groupPanel2.Controls.Add(this.family_box);
            this.groupPanel2.DisabledBackColor = System.Drawing.Color.Empty;
            this.groupPanel2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.groupPanel2.Location = new System.Drawing.Point(285, 159);
            this.groupPanel2.Name = "groupPanel2";
            this.groupPanel2.Size = new System.Drawing.Size(337, 105);
            // 
            // 
            // 
            this.groupPanel2.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel2.Style.BackColorGradientAngle = 90;
            this.groupPanel2.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel2.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderBottomWidth = 1;
            this.groupPanel2.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel2.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderLeftWidth = 1;
            this.groupPanel2.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderRightWidth = 1;
            this.groupPanel2.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderTopWidth = 1;
            this.groupPanel2.Style.CornerDiameter = 4;
            this.groupPanel2.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel2.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel2.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel2.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.groupPanel2.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel2.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel2.TabIndex = 246;
            this.groupPanel2.Text = "مشخصات فردی";
            // 
            // radGridView1
            // 
            this.radGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radGridView1.BackColor = System.Drawing.SystemColors.Control;
            this.radGridView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.radGridView1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.radGridView1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radGridView1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.radGridView1.Location = new System.Drawing.Point(0, 270);
            // 
            // 
            // 
            this.radGridView1.MasterTemplate.AllowAddNewRow = false;
            this.radGridView1.MasterTemplate.AllowColumnReorder = false;
            this.radGridView1.MasterTemplate.AutoGenerateColumns = false;
            gridViewTextBoxColumn8.EnableExpressionEditor = false;
            gridViewTextBoxColumn8.FieldName = "Radif";
            gridViewTextBoxColumn8.HeaderText = "ردیف";
            gridViewTextBoxColumn8.Name = "Radif";
            gridViewTextBoxColumn8.Width = 31;
            gridViewTextBoxColumn9.EnableExpressionEditor = false;
            gridViewTextBoxColumn9.FieldName = "drugUsageID";
            gridViewTextBoxColumn9.HeaderText = "شماره ثبت";
            gridViewTextBoxColumn9.Name = "drugUsageID";
            gridViewTextBoxColumn9.Width = 60;
            gridViewTextBoxColumn10.EnableExpressionEditor = false;
            gridViewTextBoxColumn10.FieldName = "patientID";
            gridViewTextBoxColumn10.HeaderText = "کد ثبت";
            gridViewTextBoxColumn10.Name = "patientID";
            gridViewTextBoxColumn10.Width = 41;
            gridViewTextBoxColumn11.EnableExpressionEditor = false;
            gridViewTextBoxColumn11.FieldName = "fname";
            gridViewTextBoxColumn11.HeaderText = "نام";
            gridViewTextBoxColumn11.Name = "fname";
            gridViewTextBoxColumn11.Width = 21;
            gridViewTextBoxColumn12.EnableExpressionEditor = false;
            gridViewTextBoxColumn12.FieldName = "lname";
            gridViewTextBoxColumn12.HeaderText = "نام خانوادگی";
            gridViewTextBoxColumn12.Name = "lname";
            gridViewTextBoxColumn12.Width = 69;
            gridViewTextBoxColumn13.EnableExpressionEditor = false;
            gridViewTextBoxColumn13.FieldName = "drugName";
            gridViewTextBoxColumn13.HeaderText = "دارو";
            gridViewTextBoxColumn13.Name = "drugName";
            gridViewTextBoxColumn13.Width = 25;
            gridViewTextBoxColumn14.EnableExpressionEditor = false;
            gridViewTextBoxColumn14.FieldName = "countz";
            gridViewTextBoxColumn14.HeaderText = "تعداد";
            gridViewTextBoxColumn14.Name = "countz";
            gridViewTextBoxColumn14.Width = 31;
            this.radGridView1.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn8,
            gridViewTextBoxColumn9,
            gridViewTextBoxColumn10,
            gridViewTextBoxColumn11,
            gridViewTextBoxColumn12,
            gridViewTextBoxColumn13,
            gridViewTextBoxColumn14});
            this.radGridView1.MasterTemplate.ViewDefinition = tableViewDefinition2;
            this.radGridView1.Name = "radGridView1";
            this.radGridView1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.radGridView1.Size = new System.Drawing.Size(1239, 197);
            this.radGridView1.TabIndex = 253;
            this.radGridView1.Text = "radGridView1";
            this.radGridView1.UserDeletingRow += new Telerik.WinControls.UI.GridViewRowCancelEventHandler(this.radGridView1_UserDeletingRow);
            this.radGridView1.CellClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.radGridView1_CellClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtProNet6);
            this.groupBox1.Controls.Add(this.comboBox2);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.radButton1);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 473);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1239, 174);
            this.groupBox1.TabIndex = 254;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "افزودن خدمت";
            // 
            // txtProNet6
            // 
            this.txtProNet6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProNet6.Location = new System.Drawing.Point(1028, 62);
            this.txtProNet6.Name = "txtProNet6";
            this.txtProNet6.Size = new System.Drawing.Size(130, 21);
            this.txtProNet6.TabIndex = 98;
            // 
            // comboBox2
            // 
            this.comboBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBox2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(975, 35);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(183, 21);
            this.comboBox2.TabIndex = 96;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label7.ForeColor = System.Drawing.Color.Navy;
            this.label7.Location = new System.Drawing.Point(1202, 38);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 101;
            this.label7.Text = "دارو :";
            // 
            // radButton1
            // 
            this.radButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radButton1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radButton1.Location = new System.Drawing.Point(1048, 102);
            this.radButton1.Name = "radButton1";
            this.radButton1.Size = new System.Drawing.Size(110, 24);
            this.radButton1.TabIndex = 99;
            this.radButton1.Text = "افزودن خدمت";
            this.radButton1.Click += new System.EventHandler(this.radButton1_Click);
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label10.ForeColor = System.Drawing.Color.Navy;
            this.label10.Location = new System.Drawing.Point(1193, 67);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(40, 13);
            this.label10.TabIndex = 102;
            this.label10.Text = "تعداد : ";
            // 
            // DrugList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1239, 647);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.radGridView1);
            this.Controls.Add(this.groupPanel2);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.dataGridView1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.Name = "DrugList";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ویرایش بیماران";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Patients_Edit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.groupPanel2.ResumeLayout(false);
            this.groupPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label27;
        private DevComponents.DotNetBar.ButtonX buttonX5;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private System.Windows.Forms.TextBox family_box;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox name_box;
        private DevComponents.DotNetBar.ButtonX buttonX3;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel2;
        private Telerik.WinControls.UI.RadGridView radGridView1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtProNet6;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label7;
        private Telerik.WinControls.UI.RadButton radButton1;
        private System.Windows.Forms.Label label10;
        private BPersianCalender.BPersianCalenderTextBox uc2;
        private BPersianCalender.BPersianCalenderTextBox uc1;
        private System.Windows.Forms.DataGridViewTextBoxColumn patientid;
        private System.Windows.Forms.DataGridViewTextBoxColumn doctorName;
        private System.Windows.Forms.DataGridViewTextBoxColumn reg_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn ShiftName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PersonelName;
        private System.Windows.Forms.DataGridViewTextBoxColumn lname;
        private System.Windows.Forms.DataGridViewTextBoxColumn fname;
        private System.Windows.Forms.DataGridViewTextBoxColumn Radif;
    }
}