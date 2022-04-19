namespace UNIQUE.ConfigurationFld
{
    partial class Detection_test
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Detection_test));
            this.txtSearchName = new System.Windows.Forms.TextBox();
            this.txtSearchCode = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.simpleBtn_Delete = new DevExpress.XtraEditors.SimpleButton();
            this.simpleBtn_Search = new DevExpress.XtraEditors.SimpleButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.DETECTIONID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DECTIONCODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SHORTTEXT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FULLTEXT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DESCRIPTION = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NOTPRINTABLE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LOGUSERID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LOGDATE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Morphodesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSearchName
            // 
            this.txtSearchName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchName.Location = new System.Drawing.Point(306, 15);
            this.txtSearchName.MaxLength = 250;
            this.txtSearchName.Name = "txtSearchName";
            this.txtSearchName.Size = new System.Drawing.Size(263, 22);
            this.txtSearchName.TabIndex = 21;
            this.txtSearchName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearchName_KeyDown);
            // 
            // txtSearchCode
            // 
            this.txtSearchCode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtSearchCode.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchCode.Location = new System.Drawing.Point(65, 15);
            this.txtSearchCode.MaxLength = 20;
            this.txtSearchCode.Name = "txtSearchCode";
            this.txtSearchCode.Size = new System.Drawing.Size(163, 22);
            this.txtSearchCode.TabIndex = 19;
            this.txtSearchCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearchCode_KeyDown);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1112, 66);
            this.panel3.TabIndex = 62;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Teal;
            this.label2.Location = new System.Drawing.Point(4, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 16);
            this.label2.TabIndex = 25;
            this.label2.Text = "Detection Tests";
            // 
            // panel4
            // 
            this.panel4.BackgroundImage = global::UNIQUE.Properties.Resources.laboratory_icon;
            this.panel4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel4.Location = new System.Drawing.Point(1, 8);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(38, 39);
            this.panel4.TabIndex = 23;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 66);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1112, 501);
            this.tabControl1.TabIndex = 66;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridView2);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1104, 475);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Stain";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.MenuBar;
            this.dataGridView2.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView2.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.dataGridView2.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView2.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 8.25F);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dataGridView2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView2.ColumnHeadersHeight = 25;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DETECTIONID,
            this.DECTIONCODE,
            this.SHORTTEXT,
            this.FULLTEXT,
            this.DESCRIPTION,
            this.NOTPRINTABLE,
            this.LOGUSERID,
            this.LOGDATE,
            this.Morphodesc});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 8.25F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Teal;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView2.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.GridColor = System.Drawing.Color.Gainsboro;
            this.dataGridView2.ImeMode = System.Windows.Forms.ImeMode.AlphaFull;
            this.dataGridView2.Location = new System.Drawing.Point(3, 53);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Tahoma", 8.25F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Teal;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView2.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView2.RowHeadersWidth = 15;
            this.dataGridView2.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView2.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView2.RowTemplate.Height = 20;
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.Size = new System.Drawing.Size(1098, 419);
            this.dataGridView2.TabIndex = 66;
            this.dataGridView2.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView2_CellMouseDoubleClick);
            this.dataGridView2.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_RowEnter);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.simpleBtn_Delete);
            this.panel1.Controls.Add(this.simpleBtn_Search);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtSearchCode);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtSearchName);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1098, 50);
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
            // simpleBtn_Search
            // 
            this.simpleBtn_Search.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleBtn_Search.ImageOptions.Image")));
            this.simpleBtn_Search.Location = new System.Drawing.Point(590, 8);
            this.simpleBtn_Search.Name = "simpleBtn_Search";
            this.simpleBtn_Search.Size = new System.Drawing.Size(75, 36);
            this.simpleBtn_Search.TabIndex = 24;
            this.simpleBtn_Search.Text = "Search";
            this.simpleBtn_Search.Click += new System.EventHandler(this.simpleBtn_Search_Click);
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
            // DETECTIONID
            // 
            this.DETECTIONID.DataPropertyName = "DETECTIONID";
            this.DETECTIONID.HeaderText = "DETECTIONID";
            this.DETECTIONID.Name = "DETECTIONID";
            this.DETECTIONID.ReadOnly = true;
            this.DETECTIONID.Visible = false;
            // 
            // DECTIONCODE
            // 
            this.DECTIONCODE.DataPropertyName = "DETECTIONCODE";
            this.DECTIONCODE.HeaderText = "Code";
            this.DECTIONCODE.Name = "DECTIONCODE";
            this.DECTIONCODE.ReadOnly = true;
            this.DECTIONCODE.Width = 120;
            // 
            // SHORTTEXT
            // 
            this.SHORTTEXT.DataPropertyName = "SHORTTEXT";
            this.SHORTTEXT.HeaderText = "Name";
            this.SHORTTEXT.Name = "SHORTTEXT";
            this.SHORTTEXT.ReadOnly = true;
            this.SHORTTEXT.Width = 200;
            // 
            // FULLTEXT
            // 
            this.FULLTEXT.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.FULLTEXT.DataPropertyName = "FULLTEXT";
            this.FULLTEXT.HeaderText = "Full Text";
            this.FULLTEXT.Name = "FULLTEXT";
            this.FULLTEXT.ReadOnly = true;
            // 
            // DESCRIPTION
            // 
            this.DESCRIPTION.DataPropertyName = "DESCRIPTION";
            this.DESCRIPTION.HeaderText = "Description";
            this.DESCRIPTION.Name = "DESCRIPTION";
            this.DESCRIPTION.ReadOnly = true;
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
            this.LOGUSERID.HeaderText = "Log user";
            this.LOGUSERID.Name = "LOGUSERID";
            this.LOGUSERID.ReadOnly = true;
            // 
            // LOGDATE
            // 
            this.LOGDATE.DataPropertyName = "LOGDATE";
            this.LOGDATE.HeaderText = "Log date/time";
            this.LOGDATE.Name = "LOGDATE";
            this.LOGDATE.ReadOnly = true;
            // 
            // Morphodesc
            // 
            this.Morphodesc.DataPropertyName = "MORPHODESC";
            this.Morphodesc.HeaderText = "Morphodesc";
            this.Morphodesc.Name = "Morphodesc";
            this.Morphodesc.ReadOnly = true;
            this.Morphodesc.Visible = false;
            // 
            // Detection_test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel3);
            this.Name = "Detection_test";
            this.Size = new System.Drawing.Size(1112, 567);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtSearchName;
        private System.Windows.Forms.TextBox txtSearchCode;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        public System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.SimpleButton simpleBtn_Delete;
        private DevExpress.XtraEditors.SimpleButton simpleBtn_Search;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridViewTextBoxColumn DETECTIONID;
        private System.Windows.Forms.DataGridViewTextBoxColumn DECTIONCODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn SHORTTEXT;
        private System.Windows.Forms.DataGridViewTextBoxColumn FULLTEXT;
        private System.Windows.Forms.DataGridViewTextBoxColumn DESCRIPTION;
        private System.Windows.Forms.DataGridViewTextBoxColumn NOTPRINTABLE;
        private System.Windows.Forms.DataGridViewTextBoxColumn LOGUSERID;
        private System.Windows.Forms.DataGridViewTextBoxColumn LOGDATE;
        private System.Windows.Forms.DataGridViewTextBoxColumn Morphodesc;
    }
}
