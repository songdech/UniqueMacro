namespace UNIQUE.TabbedView.WidgetView
{
    partial class DashboardSR
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
            DevExpress.XtraEditors.TileItemElement tileItemElement13 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemElement tileItemElement14 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemElement tileItemElement15 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemElement tileItemElement16 = new DevExpress.XtraEditors.TileItemElement();
            this.tileControl1 = new DevExpress.XtraEditors.TileControl();
            this.tileGroup4 = new DevExpress.XtraEditors.TileGroup();
            this.tileItem9 = new DevExpress.XtraEditors.TileItem();
            this.tileItem3 = new DevExpress.XtraEditors.TileItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorkerSR = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // tileControl1
            // 
            this.tileControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tileControl1.BackColor = System.Drawing.Color.White;
            this.tileControl1.Groups.Add(this.tileGroup4);
            this.tileControl1.Location = new System.Drawing.Point(0, 0);
            this.tileControl1.MaxId = 4;
            this.tileControl1.Name = "tileControl1";
            this.tileControl1.Padding = new System.Windows.Forms.Padding(0);
            this.tileControl1.RowCount = 1;
            this.tileControl1.Size = new System.Drawing.Size(259, 183);
            this.tileControl1.TabIndex = 4;
            this.tileControl1.Text = "tileControl1";
            // 
            // tileGroup4
            // 
            this.tileGroup4.Items.Add(this.tileItem9);
            this.tileGroup4.Items.Add(this.tileItem3);
            this.tileGroup4.Name = "tileGroup4";
            // 
            // tileItem9
            // 
            this.tileItem9.AppearanceItem.Normal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.tileItem9.AppearanceItem.Normal.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.tileItem9.AppearanceItem.Normal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.tileItem9.AppearanceItem.Normal.Options.UseBackColor = true;
            this.tileItem9.AppearanceItem.Normal.Options.UseBorderColor = true;
            this.tileItem9.AppearanceItem.Normal.Options.UseForeColor = true;
            tileItemElement13.Appearance.Hovered.Font = new System.Drawing.Font("Segoe UI Light", 49F);
            tileItemElement13.Appearance.Hovered.Options.UseFont = true;
            tileItemElement13.Appearance.Hovered.Options.UseTextOptions = true;
            tileItemElement13.Appearance.Hovered.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            tileItemElement13.Appearance.Hovered.TextOptions.Trimming = DevExpress.Utils.Trimming.Character;
            tileItemElement13.Appearance.Hovered.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            tileItemElement13.Appearance.Normal.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            tileItemElement13.Appearance.Normal.Options.UseFont = true;
            tileItemElement13.Appearance.Normal.Options.UseTextOptions = true;
            tileItemElement13.Appearance.Normal.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            tileItemElement13.Appearance.Normal.TextOptions.Trimming = DevExpress.Utils.Trimming.Character;
            tileItemElement13.Appearance.Normal.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            tileItemElement13.Appearance.Selected.Font = new System.Drawing.Font("Segoe UI Light", 49F);
            tileItemElement13.Appearance.Selected.Options.UseFont = true;
            tileItemElement13.Appearance.Selected.Options.UseTextOptions = true;
            tileItemElement13.Appearance.Selected.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            tileItemElement13.Appearance.Selected.TextOptions.Trimming = DevExpress.Utils.Trimming.Character;
            tileItemElement13.Appearance.Selected.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            tileItemElement13.MaxWidth = 5;
            tileItemElement13.Text = "1";
            tileItemElement13.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.TopCenter;
            tileItemElement13.TextLocation = new System.Drawing.Point(0, 10);
            tileItemElement14.Appearance.Hovered.Font = new System.Drawing.Font("Segoe UI", 9F);
            tileItemElement14.Appearance.Hovered.Options.UseFont = true;
            tileItemElement14.Appearance.Hovered.Options.UseTextOptions = true;
            tileItemElement14.Appearance.Hovered.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            tileItemElement14.Appearance.Hovered.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter;
            tileItemElement14.Appearance.Hovered.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            tileItemElement14.Appearance.Normal.Font = new System.Drawing.Font("Segoe UI", 9F);
            tileItemElement14.Appearance.Normal.Options.UseFont = true;
            tileItemElement14.Appearance.Normal.Options.UseTextOptions = true;
            tileItemElement14.Appearance.Normal.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            tileItemElement14.Appearance.Normal.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter;
            tileItemElement14.Appearance.Normal.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            tileItemElement14.Appearance.Selected.Font = new System.Drawing.Font("Segoe UI", 9F);
            tileItemElement14.Appearance.Selected.Options.UseFont = true;
            tileItemElement14.Appearance.Selected.Options.UseTextOptions = true;
            tileItemElement14.Appearance.Selected.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            tileItemElement14.Appearance.Selected.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter;
            tileItemElement14.Appearance.Selected.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            tileItemElement14.MaxWidth = 140;
            tileItemElement14.Text = "Urgent";
            tileItemElement14.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            tileItemElement14.TextLocation = new System.Drawing.Point(3, 15);
            this.tileItem9.Elements.Add(tileItemElement13);
            this.tileItem9.Elements.Add(tileItemElement14);
            this.tileItem9.Id = 1;
            this.tileItem9.ItemSize = DevExpress.XtraEditors.TileItemSize.Medium;
            this.tileItem9.Name = "tileItem9";
            this.tileItem9.ItemClick += new DevExpress.XtraEditors.TileItemClickEventHandler(this.tileItem9_ItemClick);
            // 
            // tileItem3
            // 
            this.tileItem3.AppearanceItem.Normal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.tileItem3.AppearanceItem.Normal.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.tileItem3.AppearanceItem.Normal.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tileItem3.AppearanceItem.Normal.ForeColor = System.Drawing.Color.Black;
            this.tileItem3.AppearanceItem.Normal.Options.UseBackColor = true;
            this.tileItem3.AppearanceItem.Normal.Options.UseBorderColor = true;
            this.tileItem3.AppearanceItem.Normal.Options.UseFont = true;
            this.tileItem3.AppearanceItem.Normal.Options.UseForeColor = true;
            tileItemElement15.Appearance.Normal.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            tileItemElement15.Appearance.Normal.Options.UseFont = true;
            tileItemElement15.Text = "1";
            tileItemElement15.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.TopCenter;
            tileItemElement15.TextLocation = new System.Drawing.Point(0, 10);
            tileItemElement16.Text = "Routine";
            tileItemElement16.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            tileItemElement16.TextLocation = new System.Drawing.Point(0, 15);
            this.tileItem3.Elements.Add(tileItemElement15);
            this.tileItem3.Elements.Add(tileItemElement16);
            this.tileItem3.Id = 3;
            this.tileItem3.ItemSize = DevExpress.XtraEditors.TileItemSize.Medium;
            this.tileItem3.Name = "tileItem3";
            this.tileItem3.ItemClick += new DevExpress.XtraEditors.TileItemClickEventHandler(this.tileItem3_ItemClick);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // backgroundWorkerSR
            // 
            this.backgroundWorkerSR.WorkerReportsProgress = true;
            this.backgroundWorkerSR.WorkerSupportsCancellation = true;
            this.backgroundWorkerSR.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerSR_DoWork);
            this.backgroundWorkerSR.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerSR_ProgressChanged);
            this.backgroundWorkerSR.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerSR_RunWorkerCompleted);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 158);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Wait..";
            this.label1.Visible = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(84, 158);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(160, 10);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 6;
            this.progressBar1.Visible = false;
            // 
            // DashboardSR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tileControl1);
            this.Name = "DashboardSR";
            this.Size = new System.Drawing.Size(259, 183);
            this.Load += new System.EventHandler(this.DashboardSR_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TileControl tileControl1;
        private DevExpress.XtraEditors.TileGroup tileGroup4;
        private DevExpress.XtraEditors.TileItem tileItem9;
        private DevExpress.XtraEditors.TileItem tileItem3;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.ComponentModel.BackgroundWorker backgroundWorkerSR;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}
