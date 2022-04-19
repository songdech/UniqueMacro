namespace UNIQUE.ConfigurationFld
{
    partial class Antibitotic
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Antibitotic));
            this.panel3 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.txtSearchCode = new System.Windows.Forms.TextBox();
            this.txtSearchName = new System.Windows.Forms.TextBox();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet_Antibiotic = new UNIQUE.ConfigurationFld.DataSet_Antibiotic();
            this.dICT_MB_ANTIBIOTICSTableAdapter = new UNIQUE.ConfigurationFld.DataSet_AntibioticTableAdapters.DICT_MB_ANTIBIOTICSTableAdapter();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.MBSTAINID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ANTIBIOTICCODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FULLTEXT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DESCRIPTION = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NOTPRINTABLE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LOGUSERID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LOGDATE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FAMNAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.simpleBtn_Delete = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Antibiotic)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1141, 66);
            this.panel3.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Teal;
            this.label2.Location = new System.Drawing.Point(23, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 16);
            this.label2.TabIndex = 27;
            this.label2.Text = "Antibiotic";
            // 
            // panel4
            // 
            this.panel4.BackgroundImage = global::UNIQUE.Properties.Resources.antibiotic;
            this.panel4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel4.Location = new System.Drawing.Point(26, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(38, 36);
            this.panel4.TabIndex = 23;
            // 
            // txtSearchCode
            // 
            this.txtSearchCode.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchCode.Location = new System.Drawing.Point(68, 15);
            this.txtSearchCode.Name = "txtSearchCode";
            this.txtSearchCode.Size = new System.Drawing.Size(163, 22);
            this.txtSearchCode.TabIndex = 19;
            // 
            // txtSearchName
            // 
            this.txtSearchName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchName.Location = new System.Drawing.Point(306, 15);
            this.txtSearchName.Name = "txtSearchName";
            this.txtSearchName.Size = new System.Drawing.Size(263, 22);
            this.txtSearchName.TabIndex = 21;
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataMember = "DICT_MB_ANTIBIOTICS";
            this.bindingSource1.DataSource = this.dataSet_Antibiotic;
            // 
            // dataSet_Antibiotic
            // 
            this.dataSet_Antibiotic.DataSetName = "DataSet_Antibiotic";
            this.dataSet_Antibiotic.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dICT_MB_ANTIBIOTICSTableAdapter
            // 
            this.dICT_MB_ANTIBIOTICSTableAdapter.ClearBeforeFill = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 66);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1141, 380);
            this.tabControl1.TabIndex = 64;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1133, 354);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Antibiotic";
            this.tabPage1.UseVisualStyleBackColor = true;
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
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.PaleTurquoise;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.ColumnHeadersHeight = 25;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MBSTAINID,
            this.ANTIBIOTICCODE,
            this.FULLTEXT,
            this.DESCRIPTION,
            this.NOTPRINTABLE,
            this.LOGUSERID,
            this.LOGDATE,
            this.FAMNAME});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.GridColor = System.Drawing.Color.Gainsboro;
            this.dataGridView1.ImeMode = System.Windows.Forms.ImeMode.AlphaFull;
            this.dataGridView1.Location = new System.Drawing.Point(3, 53);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Tahoma", 8.25F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Teal;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView1.RowHeadersWidth = 15;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowTemplate.Height = 20;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1127, 298);
            this.dataGridView1.TabIndex = 63;
            this.dataGridView1.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDoubleClick);
            this.dataGridView1.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_RowEnter);
            // 
            // MBSTAINID
            // 
            this.MBSTAINID.DataPropertyName = "ANTIBIOTICID";
            this.MBSTAINID.HeaderText = "ANTIBIOTICID";
            this.MBSTAINID.Name = "MBSTAINID";
            this.MBSTAINID.ReadOnly = true;
            this.MBSTAINID.Visible = false;
            // 
            // ANTIBIOTICCODE
            // 
            this.ANTIBIOTICCODE.DataPropertyName = "ANTIBIOTICCODE";
            this.ANTIBIOTICCODE.HeaderText = "Code";
            this.ANTIBIOTICCODE.Name = "ANTIBIOTICCODE";
            this.ANTIBIOTICCODE.ReadOnly = true;
            this.ANTIBIOTICCODE.Width = 80;
            // 
            // FULLTEXT
            // 
            this.FULLTEXT.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.FULLTEXT.DataPropertyName = "FULLTEXT";
            this.FULLTEXT.HeaderText = "Name";
            this.FULLTEXT.Name = "FULLTEXT";
            this.FULLTEXT.ReadOnly = true;
            // 
            // DESCRIPTION
            // 
            this.DESCRIPTION.DataPropertyName = "DESCRIPTION";
            this.DESCRIPTION.HeaderText = "Description";
            this.DESCRIPTION.Name = "DESCRIPTION";
            this.DESCRIPTION.ReadOnly = true;
            this.DESCRIPTION.Width = 120;
            // 
            // NOTPRINTABLE
            // 
            this.NOTPRINTABLE.DataPropertyName = "NOTPRINTABLE";
            this.NOTPRINTABLE.HeaderText = "Printable";
            this.NOTPRINTABLE.Name = "NOTPRINTABLE";
            this.NOTPRINTABLE.ReadOnly = true;
            this.NOTPRINTABLE.Width = 80;
            // 
            // LOGUSERID
            // 
            this.LOGUSERID.DataPropertyName = "LOGUSERID";
            this.LOGUSERID.HeaderText = "LoguserID";
            this.LOGUSERID.Name = "LOGUSERID";
            this.LOGUSERID.ReadOnly = true;
            this.LOGUSERID.Width = 80;
            // 
            // LOGDATE
            // 
            this.LOGDATE.DataPropertyName = "LOGDATE";
            this.LOGDATE.HeaderText = "Logdate";
            this.LOGDATE.Name = "LOGDATE";
            this.LOGDATE.ReadOnly = true;
            this.LOGDATE.Width = 120;
            // 
            // FAMNAME
            // 
            this.FAMNAME.DataPropertyName = "FAMNAME";
            this.FAMNAME.HeaderText = "Family";
            this.FAMNAME.Name = "FAMNAME";
            this.FAMNAME.ReadOnly = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.simpleBtn_Delete);
            this.panel1.Controls.Add(this.simpleButton2);
            this.panel1.Controls.Add(this.txtSearchCode);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtSearchName);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1127, 50);
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
            this.simpleBtn_Delete.Visible = false;
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
            // Antibitotic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel3);
            this.Name = "Antibitotic";
            this.Size = new System.Drawing.Size(1141, 446);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Antibiotic)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox txtSearchCode;
        private System.Windows.Forms.TextBox txtSearchName;
        private System.Windows.Forms.BindingSource bindingSource1;
        private DataSet_Antibiotic dataSet_Antibiotic;
        private DataSet_AntibioticTableAdapters.DICT_MB_ANTIBIOTICSTableAdapter dICT_MB_ANTIBIOTICSTableAdapter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        public System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.SimpleButton simpleBtn_Delete;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridViewTextBoxColumn MBSTAINID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ANTIBIOTICCODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn FULLTEXT;
        private System.Windows.Forms.DataGridViewTextBoxColumn DESCRIPTION;
        private System.Windows.Forms.DataGridViewTextBoxColumn NOTPRINTABLE;
        private System.Windows.Forms.DataGridViewTextBoxColumn LOGUSERID;
        private System.Windows.Forms.DataGridViewTextBoxColumn LOGDATE;
        private System.Windows.Forms.DataGridViewTextBoxColumn FAMNAME;
    }
}
