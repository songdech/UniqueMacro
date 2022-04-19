namespace UNIQUE.ConfigurationFld
{
    partial class Specimen
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Specimen));
            this.POSTALCODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.STATE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CITY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet_Specimen = new UNIQUE.ConfigurationFld.DataSet_Specimen();
            this.dICT_COLL_MATERIALSTableAdapter = new UNIQUE.ConfigurationFld.DataSet_SpecimenTableAdapters.DICT_COLL_MATERIALSTableAdapter();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.cOLLMATERIALIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cOLLMATERIALCODEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cOLLMATERIALTEXTDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cOLLMATERIALCOMMENTDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cOLLMATERIALCREDATEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sTARTVALIDDATEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eNDVALIDDATEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lOGUSERIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lOGDATEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SPMG_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SPMG_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.simpleBtn_Delete = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Specimen)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // POSTALCODE
            // 
            this.POSTALCODE.DataPropertyName = "POSTALCODE";
            this.POSTALCODE.HeaderText = "POSTALCODE";
            this.POSTALCODE.Name = "POSTALCODE";
            this.POSTALCODE.ReadOnly = true;
            this.POSTALCODE.Visible = false;
            // 
            // STATE
            // 
            this.STATE.DataPropertyName = "STATE";
            this.STATE.HeaderText = "STATE";
            this.STATE.Name = "STATE";
            this.STATE.ReadOnly = true;
            this.STATE.Visible = false;
            // 
            // CITY
            // 
            this.CITY.DataPropertyName = "CITY";
            this.CITY.HeaderText = "CITY";
            this.CITY.Name = "CITY";
            this.CITY.ReadOnly = true;
            this.CITY.Visible = false;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1204, 66);
            this.panel3.TabIndex = 58;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Teal;
            this.label2.Location = new System.Drawing.Point(18, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 16);
            this.label2.TabIndex = 26;
            this.label2.Text = "Specimen";
            // 
            // panel4
            // 
            this.panel4.BackgroundImage = global::UNIQUE.Properties.Resources._844746_medical_512x512;
            this.panel4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel4.Location = new System.Drawing.Point(23, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(40, 37);
            this.panel4.TabIndex = 23;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.MenuBar;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 8.25F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Red;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.ColumnHeadersHeight = 25;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CITY,
            this.STATE,
            this.POSTALCODE});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 8.25F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.GridColor = System.Drawing.Color.Gainsboro;
            this.dataGridView1.ImeMode = System.Windows.Forms.ImeMode.AlphaFull;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Tahoma", 8.25F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView1.RowHeadersWidth = 15;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowTemplate.Height = 20;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1204, 575);
            this.dataGridView1.TabIndex = 59;
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataMember = "DICT_COLL_MATERIALS";
            this.bindingSource1.DataSource = this.dataSet_Specimen;
            // 
            // dataSet_Specimen
            // 
            this.dataSet_Specimen.DataSetName = "DataSet_Specimen";
            this.dataSet_Specimen.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dICT_COLL_MATERIALSTableAdapter
            // 
            this.dICT_COLL_MATERIALSTableAdapter.ClearBeforeFill = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 66);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1204, 509);
            this.tabControl1.TabIndex = 69;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridView2);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1196, 483);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Specimen";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.AllowUserToResizeRows = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.MenuBar;
            this.dataGridView2.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView2.AutoGenerateColumns = false;
            this.dataGridView2.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.dataGridView2.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView2.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Tahoma", 8.25F);
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.PaleTurquoise;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dataGridView2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridView2.ColumnHeadersHeight = 25;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cOLLMATERIALIDDataGridViewTextBoxColumn,
            this.cOLLMATERIALCODEDataGridViewTextBoxColumn,
            this.cOLLMATERIALTEXTDataGridViewTextBoxColumn,
            this.cOLLMATERIALCOMMENTDataGridViewTextBoxColumn,
            this.cOLLMATERIALCREDATEDataGridViewTextBoxColumn,
            this.sTARTVALIDDATEDataGridViewTextBoxColumn,
            this.eNDVALIDDATEDataGridViewTextBoxColumn,
            this.lOGUSERIDDataGridViewTextBoxColumn,
            this.lOGDATEDataGridViewTextBoxColumn,
            this.SPMG_NAME,
            this.SPMG_ID});
            this.dataGridView2.DataSource = this.bindingSource1;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView2.DefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.GridColor = System.Drawing.Color.Gainsboro;
            this.dataGridView2.ImeMode = System.Windows.Forms.ImeMode.AlphaFull;
            this.dataGridView2.Location = new System.Drawing.Point(3, 53);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Tahoma", 8.25F);
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.Teal;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView2.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridView2.RowHeadersWidth = 15;
            this.dataGridView2.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView2.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView2.RowTemplate.Height = 20;
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.Size = new System.Drawing.Size(1190, 427);
            this.dataGridView2.TabIndex = 62;
            this.dataGridView2.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView2_CellMouseDoubleClick);
            this.dataGridView2.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_RowEnter);
            // 
            // cOLLMATERIALIDDataGridViewTextBoxColumn
            // 
            this.cOLLMATERIALIDDataGridViewTextBoxColumn.DataPropertyName = "COLLMATERIALID";
            this.cOLLMATERIALIDDataGridViewTextBoxColumn.HeaderText = "COLLMATERIALID";
            this.cOLLMATERIALIDDataGridViewTextBoxColumn.Name = "cOLLMATERIALIDDataGridViewTextBoxColumn";
            this.cOLLMATERIALIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.cOLLMATERIALIDDataGridViewTextBoxColumn.Visible = false;
            // 
            // cOLLMATERIALCODEDataGridViewTextBoxColumn
            // 
            this.cOLLMATERIALCODEDataGridViewTextBoxColumn.DataPropertyName = "COLLMATERIALCODE";
            this.cOLLMATERIALCODEDataGridViewTextBoxColumn.HeaderText = "Code";
            this.cOLLMATERIALCODEDataGridViewTextBoxColumn.Name = "cOLLMATERIALCODEDataGridViewTextBoxColumn";
            this.cOLLMATERIALCODEDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // cOLLMATERIALTEXTDataGridViewTextBoxColumn
            // 
            this.cOLLMATERIALTEXTDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cOLLMATERIALTEXTDataGridViewTextBoxColumn.DataPropertyName = "COLLMATERIALTEXT";
            this.cOLLMATERIALTEXTDataGridViewTextBoxColumn.HeaderText = "Name";
            this.cOLLMATERIALTEXTDataGridViewTextBoxColumn.Name = "cOLLMATERIALTEXTDataGridViewTextBoxColumn";
            this.cOLLMATERIALTEXTDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // cOLLMATERIALCOMMENTDataGridViewTextBoxColumn
            // 
            this.cOLLMATERIALCOMMENTDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cOLLMATERIALCOMMENTDataGridViewTextBoxColumn.DataPropertyName = "COLLMATERIALCOMMENT";
            this.cOLLMATERIALCOMMENTDataGridViewTextBoxColumn.HeaderText = "Comment";
            this.cOLLMATERIALCOMMENTDataGridViewTextBoxColumn.Name = "cOLLMATERIALCOMMENTDataGridViewTextBoxColumn";
            this.cOLLMATERIALCOMMENTDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // cOLLMATERIALCREDATEDataGridViewTextBoxColumn
            // 
            this.cOLLMATERIALCREDATEDataGridViewTextBoxColumn.DataPropertyName = "COLLMATERIALCREDATE";
            this.cOLLMATERIALCREDATEDataGridViewTextBoxColumn.HeaderText = "COLLMATERIALCREDATE";
            this.cOLLMATERIALCREDATEDataGridViewTextBoxColumn.Name = "cOLLMATERIALCREDATEDataGridViewTextBoxColumn";
            this.cOLLMATERIALCREDATEDataGridViewTextBoxColumn.ReadOnly = true;
            this.cOLLMATERIALCREDATEDataGridViewTextBoxColumn.Visible = false;
            // 
            // sTARTVALIDDATEDataGridViewTextBoxColumn
            // 
            this.sTARTVALIDDATEDataGridViewTextBoxColumn.DataPropertyName = "STARTVALIDDATE";
            this.sTARTVALIDDATEDataGridViewTextBoxColumn.HeaderText = "Log user";
            this.sTARTVALIDDATEDataGridViewTextBoxColumn.Name = "sTARTVALIDDATEDataGridViewTextBoxColumn";
            this.sTARTVALIDDATEDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // eNDVALIDDATEDataGridViewTextBoxColumn
            // 
            this.eNDVALIDDATEDataGridViewTextBoxColumn.DataPropertyName = "ENDVALIDDATE";
            this.eNDVALIDDATEDataGridViewTextBoxColumn.HeaderText = "End Date";
            this.eNDVALIDDATEDataGridViewTextBoxColumn.Name = "eNDVALIDDATEDataGridViewTextBoxColumn";
            this.eNDVALIDDATEDataGridViewTextBoxColumn.ReadOnly = true;
            this.eNDVALIDDATEDataGridViewTextBoxColumn.Visible = false;
            // 
            // lOGUSERIDDataGridViewTextBoxColumn
            // 
            this.lOGUSERIDDataGridViewTextBoxColumn.DataPropertyName = "LOGUSERID";
            this.lOGUSERIDDataGridViewTextBoxColumn.HeaderText = "LOGUSERID";
            this.lOGUSERIDDataGridViewTextBoxColumn.Name = "lOGUSERIDDataGridViewTextBoxColumn";
            this.lOGUSERIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.lOGUSERIDDataGridViewTextBoxColumn.Visible = false;
            // 
            // lOGDATEDataGridViewTextBoxColumn
            // 
            this.lOGDATEDataGridViewTextBoxColumn.DataPropertyName = "LOGDATE";
            this.lOGDATEDataGridViewTextBoxColumn.HeaderText = "Log date/time";
            this.lOGDATEDataGridViewTextBoxColumn.Name = "lOGDATEDataGridViewTextBoxColumn";
            this.lOGDATEDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // SPMG_NAME
            // 
            this.SPMG_NAME.DataPropertyName = "SPM_NAME";
            this.SPMG_NAME.HeaderText = "Group name ";
            this.SPMG_NAME.Name = "SPMG_NAME";
            this.SPMG_NAME.ReadOnly = true;
            this.SPMG_NAME.Width = 180;
            // 
            // SPMG_ID
            // 
            this.SPMG_ID.DataPropertyName = "SPM_GROUP_ID";
            this.SPMG_ID.HeaderText = "Group ID";
            this.SPMG_ID.Name = "SPMG_ID";
            this.SPMG_ID.ReadOnly = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.simpleBtn_Delete);
            this.panel1.Controls.Add(this.simpleButton2);
            this.panel1.Controls.Add(this.txtCode);
            this.panel1.Controls.Add(this.txtName);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1190, 50);
            this.panel1.TabIndex = 61;
            // 
            // simpleBtn_Delete
            // 
            this.simpleBtn_Delete.ImageOptions.Image = global::UNIQUE.Properties.Resources.close_32x32;
            this.simpleBtn_Delete.Location = new System.Drawing.Point(671, 8);
            this.simpleBtn_Delete.Name = "simpleBtn_Delete";
            this.simpleBtn_Delete.Size = new System.Drawing.Size(75, 36);
            this.simpleBtn_Delete.TabIndex = 25;
            this.simpleBtn_Delete.Text = "Delete";
            this.simpleBtn_Delete.Click += new System.EventHandler(this.simpleBtn_Delete_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.ImageOptions.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(590, 8);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(75, 36);
            this.simpleButton2.TabIndex = 24;
            this.simpleButton2.Text = "Search";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // txtCode
            // 
            this.txtCode.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCode.Location = new System.Drawing.Point(64, 15);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(163, 22);
            this.txtCode.TabIndex = 19;
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.Location = new System.Drawing.Point(306, 15);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(263, 22);
            this.txtName.TabIndex = 21;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(258, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 14);
            this.label3.TabIndex = 22;
            this.label3.Text = "Name:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(14, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 14);
            this.label4.TabIndex = 20;
            this.label4.Text = "Code";
            // 
            // Specimen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Specimen";
            this.Size = new System.Drawing.Size(1204, 575);
            this.Load += new System.EventHandler(this.Specimen_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Specimen)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.DataGridViewTextBoxColumn POSTALCODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn STATE;
        private System.Windows.Forms.DataGridViewTextBoxColumn CITY;
        private System.Windows.Forms.Panel panel3;
        public System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource bindingSource1;
        private DataSet_Specimen dataSet_Specimen;
        private DataSet_SpecimenTableAdapters.DICT_COLL_MATERIALSTableAdapter dICT_COLL_MATERIALSTableAdapter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.SimpleButton simpleBtn_Delete;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DataGridViewTextBoxColumn cOLLMATERIALIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cOLLMATERIALCODEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cOLLMATERIALTEXTDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cOLLMATERIALCOMMENTDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cOLLMATERIALCREDATEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sTARTVALIDDATEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eNDVALIDDATEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lOGUSERIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lOGDATEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn SPMG_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn SPMG_ID;
    }
}
