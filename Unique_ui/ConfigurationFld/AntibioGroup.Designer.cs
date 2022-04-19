namespace UNIQUE.ConfigurationFld
{
    partial class AntibioGroup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AntibioGroup));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.panel4 = new System.Windows.Forms.Panel();
            this.txtSearchAnticode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSearchAntiname = new System.Windows.Forms.TextBox();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.aNTIBIOTICSFAMILYIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.aNTIBIOTICSFAMILYCODEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fAMNAMEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.STARTVALIDDATE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet_Antibiotic_Group = new UNIQUE.ConfigurationFld.DataSet_Antibiotic_Group();
            this.dICT_MB_ANTIBIOTIC_GROUPTableAdapter = new UNIQUE.ConfigurationFld.DataSet_Antibiotic_GroupTableAdapters.DICT_MB_ANTIBIOTIC_GROUPTableAdapter();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Antibiotic_Group)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.simpleButton1);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.txtSearchAnticode);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.txtSearchAntiname);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1162, 66);
            this.panel3.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Teal;
            this.label2.Location = new System.Drawing.Point(20, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 16);
            this.label2.TabIndex = 28;
            this.label2.Text = "Antibiotic Family";
            // 
            // simpleButton1
            // 
            this.simpleButton1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.ImageOptions.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(852, 13);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(75, 36);
            this.simpleButton1.TabIndex = 24;
            this.simpleButton1.Text = "Search";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // panel4
            // 
            this.panel4.BackgroundImage = global::UNIQUE.Properties.Resources.medical__76_;
            this.panel4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel4.Location = new System.Drawing.Point(23, 7);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(44, 38);
            this.panel4.TabIndex = 23;
            // 
            // txtSearchAnticode
            // 
            this.txtSearchAnticode.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchAnticode.Location = new System.Drawing.Point(307, 27);
            this.txtSearchAnticode.Name = "txtSearchAnticode";
            this.txtSearchAnticode.Size = new System.Drawing.Size(163, 22);
            this.txtSearchAnticode.TabIndex = 19;
            this.txtSearchAnticode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearchDoccode_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(504, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 14);
            this.label1.TabIndex = 22;
            this.label1.Text = "Name:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(249, 35);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 14);
            this.label7.TabIndex = 20;
            this.label7.Text = "Code";
            // 
            // txtSearchAntiname
            // 
            this.txtSearchAntiname.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchAntiname.Location = new System.Drawing.Point(552, 27);
            this.txtSearchAntiname.Name = "txtSearchAntiname";
            this.txtSearchAntiname.Size = new System.Drawing.Size(278, 22);
            this.txtSearchAntiname.TabIndex = 21;
            this.txtSearchAntiname.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearchDocname_KeyDown);
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 66);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.splitContainerControl1.Panel1.Controls.Add(this.dataGridView1);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(1162, 458);
            this.splitContainerControl1.SplitterPosition = 546;
            this.splitContainerControl1.TabIndex = 4;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.MenuBar;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.AutoGenerateColumns = false;
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
            this.aNTIBIOTICSFAMILYIDDataGridViewTextBoxColumn,
            this.aNTIBIOTICSFAMILYCODEDataGridViewTextBoxColumn,
            this.fAMNAMEDataGridViewTextBoxColumn,
            this.STARTVALIDDATE});
            this.dataGridView1.DataSource = this.bindingSource1;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 8.25F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption;
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
            this.dataGridView1.Size = new System.Drawing.Size(542, 454);
            this.dataGridView1.TabIndex = 60;
            this.dataGridView1.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_RowEnter);
            this.dataGridView1.Enter += new System.EventHandler(this.dataGridView1_Enter);
            // 
            // aNTIBIOTICSFAMILYIDDataGridViewTextBoxColumn
            // 
            this.aNTIBIOTICSFAMILYIDDataGridViewTextBoxColumn.DataPropertyName = "ANTIBIOTICSFAMILYID";
            this.aNTIBIOTICSFAMILYIDDataGridViewTextBoxColumn.HeaderText = "ID";
            this.aNTIBIOTICSFAMILYIDDataGridViewTextBoxColumn.Name = "aNTIBIOTICSFAMILYIDDataGridViewTextBoxColumn";
            this.aNTIBIOTICSFAMILYIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.aNTIBIOTICSFAMILYIDDataGridViewTextBoxColumn.Visible = false;
            // 
            // aNTIBIOTICSFAMILYCODEDataGridViewTextBoxColumn
            // 
            this.aNTIBIOTICSFAMILYCODEDataGridViewTextBoxColumn.DataPropertyName = "ANTIBIOTICSFAMILYCODE";
            this.aNTIBIOTICSFAMILYCODEDataGridViewTextBoxColumn.HeaderText = "CODE";
            this.aNTIBIOTICSFAMILYCODEDataGridViewTextBoxColumn.Name = "aNTIBIOTICSFAMILYCODEDataGridViewTextBoxColumn";
            this.aNTIBIOTICSFAMILYCODEDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // fAMNAMEDataGridViewTextBoxColumn
            // 
            this.fAMNAMEDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.fAMNAMEDataGridViewTextBoxColumn.DataPropertyName = "FAMNAME";
            this.fAMNAMEDataGridViewTextBoxColumn.HeaderText = "NAME";
            this.fAMNAMEDataGridViewTextBoxColumn.Name = "fAMNAMEDataGridViewTextBoxColumn";
            this.fAMNAMEDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // STARTVALIDDATE
            // 
            this.STARTVALIDDATE.DataPropertyName = "STARTVALIDDATE";
            this.STARTVALIDDATE.HeaderText = "STARTVALIDDATE";
            this.STARTVALIDDATE.Name = "STARTVALIDDATE";
            this.STARTVALIDDATE.ReadOnly = true;
            this.STARTVALIDDATE.Visible = false;
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataMember = "DICT_MB_ANTIBIOTIC_GROUP";
            this.bindingSource1.DataSource = this.dataSet_Antibiotic_Group;
            // 
            // dataSet_Antibiotic_Group
            // 
            this.dataSet_Antibiotic_Group.DataSetName = "DataSet_Antibiotic_Group";
            this.dataSet_Antibiotic_Group.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dICT_MB_ANTIBIOTIC_GROUPTableAdapter
            // 
            this.dICT_MB_ANTIBIOTIC_GROUPTableAdapter.ClearBeforeFill = true;
            // 
            // AntibioGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerControl1);
            this.Controls.Add(this.panel3);
            this.Name = "AntibioGroup";
            this.Size = new System.Drawing.Size(1162, 524);
            this.Load += new System.EventHandler(this.AntibioGroup_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Antibiotic_Group)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox txtSearchAnticode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSearchAntiname;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        public System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource bindingSource1;
        private DataSet_Antibiotic_Group dataSet_Antibiotic_Group;
        private DataSet_Antibiotic_GroupTableAdapters.DICT_MB_ANTIBIOTIC_GROUPTableAdapter dICT_MB_ANTIBIOTIC_GROUPTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn aNTIBIOTICSFAMILYIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn aNTIBIOTICSFAMILYCODEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fAMNAMEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn STARTVALIDDATE;
        private System.Windows.Forms.Label label2;
    }
}
