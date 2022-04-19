namespace UNIQUE.Forms.Report_worksheet
{
    partial class Form_Worksheet_printing
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
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem1 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem2 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem2 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip3 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem3 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem3 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip4 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem4 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem4 = new DevExpress.Utils.ToolTipItem();
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.printPreviewBarItem5 = new DevExpress.XtraPrinting.Preview.PrintPreviewBarItem();
            this.printPreviewBarItem6 = new DevExpress.XtraPrinting.Preview.PrintPreviewBarItem();
            this.printPreviewBarItem7 = new DevExpress.XtraPrinting.Preview.PrintPreviewBarItem();
            this.printPreviewBarItem25 = new DevExpress.XtraPrinting.Preview.PrintPreviewBarItem();
            this.zoomTrackBarEditItem1 = new DevExpress.XtraPrinting.Preview.ZoomTrackBarEditItem();
            this.ribbonPage1 = new DevExpress.XtraPrinting.Preview.PrintPreviewRibbonPage();
            this.printPreviewRibbonPageGroup2 = new DevExpress.XtraPrinting.Preview.PrintPreviewRibbonPageGroup();
            this.printPreviewRibbonPageGroup8 = new DevExpress.XtraPrinting.Preview.PrintPreviewRibbonPageGroup();
            this.documentViewer1 = new DevExpress.XtraPrinting.Preview.DocumentViewer();
            this.documentViewerRibbonController1 = new DevExpress.XtraPrinting.Preview.DocumentViewerRibbonController(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentViewerRibbonController1)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.AutoHideEmptyItems = true;
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.printPreviewBarItem5,
            this.printPreviewBarItem6,
            this.printPreviewBarItem7,
            this.printPreviewBarItem25,
            this.zoomTrackBarEditItem1});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 57;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbonControl1.Size = new System.Drawing.Size(1169, 116);
            this.ribbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            this.ribbonControl1.TransparentEditorsMode = DevExpress.Utils.DefaultBoolean.True;
            // 
            // printPreviewBarItem5
            // 
            this.printPreviewBarItem5.Caption = "Print";
            this.printPreviewBarItem5.Command = DevExpress.XtraPrinting.PrintingSystemCommand.Print;
            this.printPreviewBarItem5.Enabled = false;
            this.printPreviewBarItem5.Id = 5;
            this.printPreviewBarItem5.Name = "printPreviewBarItem5";
            superToolTip1.FixedTooltipWidth = true;
            toolTipTitleItem1.Text = "Print (Ctrl+P)";
            toolTipItem1.LeftIndent = 6;
            toolTipItem1.Text = "Select a printer, number of copies and other printing options before printing.";
            superToolTip1.Items.Add(toolTipTitleItem1);
            superToolTip1.Items.Add(toolTipItem1);
            superToolTip1.MaxWidth = 210;
            this.printPreviewBarItem5.SuperTip = superToolTip1;
            // 
            // printPreviewBarItem6
            // 
            this.printPreviewBarItem6.Caption = "Quick Print";
            this.printPreviewBarItem6.Command = DevExpress.XtraPrinting.PrintingSystemCommand.PrintDirect;
            this.printPreviewBarItem6.Enabled = false;
            this.printPreviewBarItem6.Id = 6;
            this.printPreviewBarItem6.Name = "printPreviewBarItem6";
            superToolTip2.FixedTooltipWidth = true;
            toolTipTitleItem2.Text = "Quick Print";
            toolTipItem2.LeftIndent = 6;
            toolTipItem2.Text = "Send the document directly to the default printer without making changes.";
            superToolTip2.Items.Add(toolTipTitleItem2);
            superToolTip2.Items.Add(toolTipItem2);
            superToolTip2.MaxWidth = 210;
            this.printPreviewBarItem6.SuperTip = superToolTip2;
            // 
            // printPreviewBarItem7
            // 
            this.printPreviewBarItem7.Caption = "Custom Margins...";
            this.printPreviewBarItem7.Command = DevExpress.XtraPrinting.PrintingSystemCommand.PageSetup;
            this.printPreviewBarItem7.Enabled = false;
            this.printPreviewBarItem7.Id = 7;
            this.printPreviewBarItem7.Name = "printPreviewBarItem7";
            superToolTip3.FixedTooltipWidth = true;
            toolTipTitleItem3.Text = "Page Setup";
            toolTipItem3.LeftIndent = 6;
            toolTipItem3.Text = "Show the Page Setup dialog.";
            superToolTip3.Items.Add(toolTipTitleItem3);
            superToolTip3.Items.Add(toolTipItem3);
            superToolTip3.MaxWidth = 210;
            this.printPreviewBarItem7.SuperTip = superToolTip3;
            // 
            // printPreviewBarItem25
            // 
            this.printPreviewBarItem25.Caption = "Close";
            this.printPreviewBarItem25.Command = DevExpress.XtraPrinting.PrintingSystemCommand.ClosePreview;
            this.printPreviewBarItem25.Enabled = false;
            this.printPreviewBarItem25.Id = 25;
            this.printPreviewBarItem25.Name = "printPreviewBarItem25";
            superToolTip4.FixedTooltipWidth = true;
            toolTipTitleItem4.Text = "Close Print Preview";
            toolTipItem4.LeftIndent = 6;
            toolTipItem4.Text = "Close Print Preview of the document.";
            superToolTip4.Items.Add(toolTipTitleItem4);
            superToolTip4.Items.Add(toolTipItem4);
            superToolTip4.MaxWidth = 210;
            this.printPreviewBarItem25.SuperTip = superToolTip4;
            this.printPreviewBarItem25.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.printPreviewBarItem25_ItemClick);
            // 
            // zoomTrackBarEditItem1
            // 
            this.zoomTrackBarEditItem1.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.zoomTrackBarEditItem1.Edit = null;
            this.zoomTrackBarEditItem1.EditValue = 90;
            this.zoomTrackBarEditItem1.EditWidth = 140;
            this.zoomTrackBarEditItem1.Enabled = false;
            this.zoomTrackBarEditItem1.Id = 54;
            this.zoomTrackBarEditItem1.Name = "zoomTrackBarEditItem1";
            this.zoomTrackBarEditItem1.Range = new int[] {
        10,
        500};
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.printPreviewRibbonPageGroup2,
            this.printPreviewRibbonPageGroup8});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "Print Preview";
            // 
            // printPreviewRibbonPageGroup2
            // 
            this.printPreviewRibbonPageGroup2.AllowTextClipping = false;
            this.printPreviewRibbonPageGroup2.ItemLinks.Add(this.printPreviewBarItem5);
            this.printPreviewRibbonPageGroup2.ItemLinks.Add(this.printPreviewBarItem6);
            this.printPreviewRibbonPageGroup2.Kind = DevExpress.XtraPrinting.Preview.PrintPreviewRibbonPageGroupKind.Print;
            this.printPreviewRibbonPageGroup2.Name = "printPreviewRibbonPageGroup2";
            this.printPreviewRibbonPageGroup2.ShowCaptionButton = false;
            this.printPreviewRibbonPageGroup2.Text = "Print";
            // 
            // printPreviewRibbonPageGroup8
            // 
            this.printPreviewRibbonPageGroup8.AllowTextClipping = false;
            this.printPreviewRibbonPageGroup8.ItemLinks.Add(this.printPreviewBarItem25);
            this.printPreviewRibbonPageGroup8.Kind = DevExpress.XtraPrinting.Preview.PrintPreviewRibbonPageGroupKind.Close;
            this.printPreviewRibbonPageGroup8.Name = "printPreviewRibbonPageGroup8";
            this.printPreviewRibbonPageGroup8.ShowCaptionButton = false;
            this.printPreviewRibbonPageGroup8.Text = "Close";
            // 
            // documentViewer1
            // 
            this.documentViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.documentViewer1.IsMetric = true;
            this.documentViewer1.Location = new System.Drawing.Point(0, 116);
            this.documentViewer1.Name = "documentViewer1";
            this.documentViewer1.Size = new System.Drawing.Size(1169, 687);
            this.documentViewer1.TabIndex = 5;
            // 
            // documentViewerRibbonController1
            // 
            this.documentViewerRibbonController1.DocumentViewer = this.documentViewer1;
            this.documentViewerRibbonController1.RibbonControl = this.ribbonControl1;
            this.documentViewerRibbonController1.RibbonStatusBar = null;
            // 
            // Form_Worksheet_printing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1169, 803);
            this.Controls.Add(this.documentViewer1);
            this.Controls.Add(this.ribbonControl1);
            this.Name = "Form_Worksheet_printing";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Work sheet printing";
            this.Load += new System.EventHandler(this.Form_Worksheet_printing_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentViewerRibbonController1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraPrinting.Preview.PrintPreviewBarItem printPreviewBarItem5;
        private DevExpress.XtraPrinting.Preview.PrintPreviewBarItem printPreviewBarItem6;
        private DevExpress.XtraPrinting.Preview.PrintPreviewBarItem printPreviewBarItem7;
        private DevExpress.XtraPrinting.Preview.PrintPreviewBarItem printPreviewBarItem25;
        private DevExpress.XtraPrinting.Preview.ZoomTrackBarEditItem zoomTrackBarEditItem1;
        private DevExpress.XtraPrinting.Preview.PrintPreviewRibbonPage ribbonPage1;
        private DevExpress.XtraPrinting.Preview.PrintPreviewRibbonPageGroup printPreviewRibbonPageGroup2;
        private DevExpress.XtraPrinting.Preview.PrintPreviewRibbonPageGroup printPreviewRibbonPageGroup8;
        private DevExpress.XtraPrinting.Preview.DocumentViewer documentViewer1;
        private DevExpress.XtraPrinting.Preview.DocumentViewerRibbonController documentViewerRibbonController1;
    }
}