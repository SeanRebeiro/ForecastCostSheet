namespace ForecastCostSheet
{
    partial class ForecastCostSheets
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ForecastCostSheets));
            this.List_Panel = new System.Windows.Forms.Panel();
            this.Content_Panel = new System.Windows.Forms.Panel();
            this.CostSheets_GridControl = new DevExpress.XtraGrid.GridControl();
            this.CostSheets_GridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.New_barButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.List_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CostSheets_GridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CostSheets_GridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // List_Panel
            // 
            this.List_Panel.AutoScroll = true;
            this.List_Panel.Controls.Add(this.Content_Panel);
            this.List_Panel.Controls.Add(this.CostSheets_GridControl);
            this.List_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.List_Panel.Location = new System.Drawing.Point(0, 47);
            this.List_Panel.Name = "List_Panel";
            this.List_Panel.Size = new System.Drawing.Size(1203, 590);
            this.List_Panel.TabIndex = 1;
            // 
            // Content_Panel
            // 
            this.Content_Panel.AutoScroll = true;
            this.Content_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Content_Panel.Location = new System.Drawing.Point(237, 0);
            this.Content_Panel.Name = "Content_Panel";
            this.Content_Panel.Size = new System.Drawing.Size(966, 590);
            this.Content_Panel.TabIndex = 1;
            // 
            // CostSheets_GridControl
            // 
            this.CostSheets_GridControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.CostSheets_GridControl.Location = new System.Drawing.Point(0, 0);
            this.CostSheets_GridControl.MainView = this.CostSheets_GridView;
            this.CostSheets_GridControl.Name = "CostSheets_GridControl";
            this.CostSheets_GridControl.Size = new System.Drawing.Size(237, 590);
            this.CostSheets_GridControl.TabIndex = 0;
            this.CostSheets_GridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.CostSheets_GridView});
            // 
            // CostSheets_GridView
            // 
            this.CostSheets_GridView.GridControl = this.CostSheets_GridControl;
            this.CostSheets_GridView.Name = "CostSheets_GridView";
            this.CostSheets_GridView.OptionsBehavior.Editable = false;
            this.CostSheets_GridView.OptionsBehavior.ReadOnly = true;
            this.CostSheets_GridView.OptionsView.ShowGroupPanel = false;
            this.CostSheets_GridView.OptionsView.ShowIndicator = false;
            this.CostSheets_GridView.DoubleClick += new System.EventHandler(this.CostSheets_GridView_DoubleClick);
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1,
            this.bar3});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.New_barButtonItem});
            this.barManager1.MaxItemId = 1;
            this.barManager1.StatusBar = this.bar3;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.FloatLocation = new System.Drawing.Point(561, 130);
            this.bar1.FloatSize = new System.Drawing.Size(46, 29);
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.New_barButtonItem)});
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Tools";
            // 
            // New_barButtonItem
            // 
            this.New_barButtonItem.Caption = "New Cost Sheet";
            this.New_barButtonItem.Id = 0;
            this.New_barButtonItem.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItem1.ImageOptions.Image")));
            this.New_barButtonItem.Name = "New_barButtonItem";
            this.New_barButtonItem.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.New_barButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.New_barButtonItem_ItemClick);
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(1203, 47);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 637);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(1203, 23);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 47);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 590);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1203, 47);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 590);
            // 
            // ForecastCostSheets
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.List_Panel);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "ForecastCostSheets";
            this.Size = new System.Drawing.Size(1203, 660);
            this.List_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CostSheets_GridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CostSheets_GridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel List_Panel;
        private DevExpress.XtraGrid.GridControl CostSheets_GridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView CostSheets_GridView;
        private System.Windows.Forms.Panel Content_Panel;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem New_barButtonItem;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
    }
}
