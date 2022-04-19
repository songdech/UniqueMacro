namespace UNIQUE.ConfigurationFld
{
    partial class frmAddSpecimen
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
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem1 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddSpecimen));
            DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem2 = new DevExpress.Utils.ToolTipTitleItem();
            this.tileNavPane1 = new DevExpress.XtraBars.Navigation.TileNavPane();
            this.btnSave = new DevExpress.XtraBars.Navigation.NavButton();
            this.btnExit = new DevExpress.XtraBars.Navigation.NavButton();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCOLLMATERIALCODE = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCOLLMATERIALTEXT = new System.Windows.Forms.TextBox();
            this.txtCOLLMATERIALComment = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupControl5 = new DevExpress.XtraEditors.GroupControl();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Btn_search_G1 = new System.Windows.Forms.Button();
            this.Btn_Clear_G4 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.tileNavPane1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl5)).BeginInit();
            this.groupControl5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tileNavPane1
            // 
            this.tileNavPane1.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.tileNavPane1.Appearance.Options.UseBackColor = true;
            this.tileNavPane1.Buttons.Add(this.btnSave);
            this.tileNavPane1.Buttons.Add(this.btnExit);
            // 
            // tileNavCategory1
            // 
            this.tileNavPane1.DefaultCategory.Name = "tileNavCategory1";
            // 
            // 
            // 
            this.tileNavPane1.DefaultCategory.Tile.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            this.tileNavPane1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tileNavPane1.Location = new System.Drawing.Point(0, 0);
            this.tileNavPane1.Name = "tileNavPane1";
            this.tileNavPane1.Size = new System.Drawing.Size(503, 40);
            this.tileNavPane1.TabIndex = 27;
            this.tileNavPane1.Text = "tileNavPane1";
            // 
            // btnSave
            // 
            this.btnSave.Alignment = DevExpress.XtraBars.Navigation.NavButtonAlignment.Left;
            this.btnSave.Appearance.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Appearance.Options.UseForeColor = true;
            this.btnSave.Caption = "Save";
            this.btnSave.ImageOptions.Image = global::UNIQUE.Properties.Resources.icon_save_32;
            this.btnSave.Name = "btnSave";
            toolTipTitleItem1.Text = "Specimen";
            toolTipItem1.LeftIndent = 6;
            toolTipItem1.Text = "Save";
            superToolTip1.Items.Add(toolTipTitleItem1);
            superToolTip1.Items.Add(toolTipItem1);
            this.btnSave.SuperTip = superToolTip1;
            this.btnSave.ElementClick += new DevExpress.XtraBars.Navigation.NavElementClickEventHandler(this.btnSave_ElementClick);
            // 
            // btnExit
            // 
            this.btnExit.Alignment = DevExpress.XtraBars.Navigation.NavButtonAlignment.Left;
            this.btnExit.Appearance.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Appearance.Options.UseForeColor = true;
            this.btnExit.Caption = "Close";
            this.btnExit.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnExit.ImageOptions.Image")));
            this.btnExit.Name = "btnExit";
            toolTipTitleItem2.Text = "Close";
            superToolTip2.Items.Add(toolTipTitleItem2);
            this.btnExit.SuperTip = superToolTip2;
            this.btnExit.ElementClick += new DevExpress.XtraBars.Navigation.NavElementClickEventHandler(this.btnExit_ElementClick);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(3, 178);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(101, 14);
            this.label7.TabIndex = 54;
            this.label7.Text = "Specimen Group:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(3, 115);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 14);
            this.label5.TabIndex = 31;
            this.label5.Text = "Comment:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 14);
            this.label1.TabIndex = 28;
            this.label1.Text = "Specimen code:";
            // 
            // txtCOLLMATERIALCODE
            // 
            this.txtCOLLMATERIALCODE.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCOLLMATERIALCODE.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCOLLMATERIALCODE.Location = new System.Drawing.Point(109, 59);
            this.txtCOLLMATERIALCODE.MaxLength = 10;
            this.txtCOLLMATERIALCODE.Name = "txtCOLLMATERIALCODE";
            this.txtCOLLMATERIALCODE.Size = new System.Drawing.Size(191, 22);
            this.txtCOLLMATERIALCODE.TabIndex = 29;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 14);
            this.label2.TabIndex = 30;
            this.label2.Text = "Specimen Text:";
            // 
            // txtCOLLMATERIALTEXT
            // 
            this.txtCOLLMATERIALTEXT.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCOLLMATERIALTEXT.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCOLLMATERIALTEXT.Location = new System.Drawing.Point(109, 87);
            this.txtCOLLMATERIALTEXT.MaxLength = 10;
            this.txtCOLLMATERIALTEXT.Name = "txtCOLLMATERIALTEXT";
            this.txtCOLLMATERIALTEXT.Size = new System.Drawing.Size(391, 22);
            this.txtCOLLMATERIALTEXT.TabIndex = 31;
            // 
            // txtCOLLMATERIALComment
            // 
            this.txtCOLLMATERIALComment.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCOLLMATERIALComment.Location = new System.Drawing.Point(109, 115);
            this.txtCOLLMATERIALComment.MaxLength = 10;
            this.txtCOLLMATERIALComment.Multiline = true;
            this.txtCOLLMATERIALComment.Name = "txtCOLLMATERIALComment";
            this.txtCOLLMATERIALComment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCOLLMATERIALComment.Size = new System.Drawing.Size(391, 60);
            this.txtCOLLMATERIALComment.TabIndex = 50;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label6.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label6.Location = new System.Drawing.Point(2, 472);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 20);
            this.label6.TabIndex = 30;
            this.label6.Text = "label6";
            this.label6.Visible = false;
            // 
            // groupControl1
            // 
            this.groupControl1.Appearance.BackColor = System.Drawing.Color.White;
            this.groupControl1.Appearance.Options.UseBackColor = true;
            this.groupControl1.Controls.Add(this.dataGridView2);
            this.groupControl1.Location = new System.Drawing.Point(110, 362);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(390, 103);
            this.groupControl1.TabIndex = 535;
            this.groupControl1.Text = "Default Stain";
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.Location = new System.Drawing.Point(2, 23);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowHeadersVisible = false;
            this.dataGridView2.RowHeadersWidth = 10;
            this.dataGridView2.Size = new System.Drawing.Size(386, 78);
            this.dataGridView2.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "CODE";
            this.dataGridViewTextBoxColumn1.HeaderText = "CODE";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 80;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "TEXT";
            this.dataGridViewTextBoxColumn2.HeaderText = "TEXT";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "ID";
            this.dataGridViewTextBoxColumn3.HeaderText = "ID";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 50;
            // 
            // groupControl5
            // 
            this.groupControl5.Controls.Add(this.dataGridView1);
            this.groupControl5.Location = new System.Drawing.Point(110, 212);
            this.groupControl5.Name = "groupControl5";
            this.groupControl5.Size = new System.Drawing.Size(390, 144);
            this.groupControl5.TabIndex = 637;
            this.groupControl5.Text = "Specimen Group";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn8,
            this.dataGridViewTextBoxColumn13,
            this.dataGridViewTextBoxColumn14});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(2, 23);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 10;
            this.dataGridView1.Size = new System.Drawing.Size(386, 119);
            this.dataGridView1.TabIndex = 0;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "CODE";
            this.dataGridViewTextBoxColumn8.HeaderText = "CODE";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.Width = 80;
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn13.DataPropertyName = "TEXT";
            this.dataGridViewTextBoxColumn13.HeaderText = "TEXT";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.DataPropertyName = "ID";
            this.dataGridViewTextBoxColumn14.HeaderText = "ID";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.Width = 50;
            // 
            // Btn_search_G1
            // 
            this.Btn_search_G1.Location = new System.Drawing.Point(109, 181);
            this.Btn_search_G1.Name = "Btn_search_G1";
            this.Btn_search_G1.Size = new System.Drawing.Size(190, 25);
            this.Btn_search_G1.TabIndex = 636;
            this.Btn_search_G1.Text = "Select Specimen Group";
            this.Btn_search_G1.UseVisualStyleBackColor = true;
            this.Btn_search_G1.Click += new System.EventHandler(this.Btn_search_G1_Click);
            // 
            // Btn_Clear_G4
            // 
            this.Btn_Clear_G4.Location = new System.Drawing.Point(306, 181);
            this.Btn_Clear_G4.Name = "Btn_Clear_G4";
            this.Btn_Clear_G4.Size = new System.Drawing.Size(194, 25);
            this.Btn_Clear_G4.TabIndex = 635;
            this.Btn_Clear_G4.Text = "Clear Specimen Group";
            this.Btn_Clear_G4.UseVisualStyleBackColor = true;
            this.Btn_Clear_G4.Click += new System.EventHandler(this.Btn_Clear_G4_Click);
            // 
            // frmAddSpecimen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 499);
            this.Controls.Add(this.groupControl5);
            this.Controls.Add(this.Btn_search_G1);
            this.Controls.Add(this.Btn_Clear_G4);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtCOLLMATERIALComment);
            this.Controls.Add(this.txtCOLLMATERIALTEXT);
            this.Controls.Add(this.txtCOLLMATERIALCODE);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tileNavPane1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmAddSpecimen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Specimen";
            this.Load += new System.EventHandler(this.frmAddSpecimen_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tileNavPane1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl5)).EndInit();
            this.groupControl5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Navigation.TileNavPane tileNavPane1;
        private DevExpress.XtraBars.Navigation.NavButton btnSave;
        private DevExpress.XtraBars.Navigation.NavButton btnExit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCOLLMATERIALCODE;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCOLLMATERIALTEXT;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtCOLLMATERIALComment;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl5;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private System.Windows.Forms.Button Btn_search_G1;
        private System.Windows.Forms.Button Btn_Clear_G4;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    }
}