namespace FinancialsApp.Dialogs
{
    partial class HistoricalDividendDialog
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
            this.panelTop = new System.Windows.Forms.Panel();
            this.dateTimePickerEndDate = new System.Windows.Forms.DateTimePicker();
            this.labelEndDate = new System.Windows.Forms.Label();
            this.dateTimePickerStartDate = new System.Windows.Forms.DateTimePicker();
            this.labelStartDate = new System.Windows.Forms.Label();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.textBoxSymbol = new System.Windows.Forms.TextBox();
            this.labelSymbol = new System.Windows.Forms.Label();
            this.panelInfo = new System.Windows.Forms.Panel();
            this.panelContent = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageGraph = new System.Windows.Forms.TabPage();
            this.tabPageDividendData = new System.Windows.Forms.TabPage();
            this.listViewDividendData = new System.Windows.Forms.ListView();
            this.columnHeaderDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderDividend = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.formsPlot1 = new ScottPlot.FormsPlot();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboGraphInterval = new System.Windows.Forms.ToolStripComboBox();
            this.panelTop.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageGraph.SuspendLayout();
            this.tabPageDividendData.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.dateTimePickerEndDate);
            this.panelTop.Controls.Add(this.labelEndDate);
            this.panelTop.Controls.Add(this.dateTimePickerStartDate);
            this.panelTop.Controls.Add(this.labelStartDate);
            this.panelTop.Controls.Add(this.buttonSearch);
            this.panelTop.Controls.Add(this.textBoxSymbol);
            this.panelTop.Controls.Add(this.labelSymbol);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(536, 47);
            this.panelTop.TabIndex = 0;
            // 
            // dateTimePickerEndDate
            // 
            this.dateTimePickerEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerEndDate.Location = new System.Drawing.Point(343, 14);
            this.dateTimePickerEndDate.Name = "dateTimePickerEndDate";
            this.dateTimePickerEndDate.Size = new System.Drawing.Size(95, 20);
            this.dateTimePickerEndDate.TabIndex = 6;
            // 
            // labelEndDate
            // 
            this.labelEndDate.AutoSize = true;
            this.labelEndDate.Location = new System.Drawing.Point(279, 17);
            this.labelEndDate.Name = "labelEndDate";
            this.labelEndDate.Size = new System.Drawing.Size(58, 13);
            this.labelEndDate.TabIndex = 5;
            this.labelEndDate.Text = "Start Date:";
            // 
            // dateTimePickerStartDate
            // 
            this.dateTimePickerStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerStartDate.Location = new System.Drawing.Point(178, 14);
            this.dateTimePickerStartDate.Name = "dateTimePickerStartDate";
            this.dateTimePickerStartDate.Size = new System.Drawing.Size(95, 20);
            this.dateTimePickerStartDate.TabIndex = 4;
            this.dateTimePickerStartDate.Value = new System.DateTime(1980, 1, 1, 0, 0, 0, 0);
            // 
            // labelStartDate
            // 
            this.labelStartDate.AutoSize = true;
            this.labelStartDate.Location = new System.Drawing.Point(114, 17);
            this.labelStartDate.Name = "labelStartDate";
            this.labelStartDate.Size = new System.Drawing.Size(58, 13);
            this.labelStartDate.TabIndex = 3;
            this.labelStartDate.Text = "Start Date:";
            // 
            // buttonSearch
            // 
            this.buttonSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSearch.Location = new System.Drawing.Point(449, 12);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(75, 23);
            this.buttonSearch.TabIndex = 2;
            this.buttonSearch.Text = "button1";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // textBoxSymbol
            // 
            this.textBoxSymbol.Location = new System.Drawing.Point(62, 14);
            this.textBoxSymbol.Name = "textBoxSymbol";
            this.textBoxSymbol.Size = new System.Drawing.Size(46, 20);
            this.textBoxSymbol.TabIndex = 1;
            // 
            // labelSymbol
            // 
            this.labelSymbol.AutoSize = true;
            this.labelSymbol.Location = new System.Drawing.Point(12, 17);
            this.labelSymbol.Name = "labelSymbol";
            this.labelSymbol.Size = new System.Drawing.Size(44, 13);
            this.labelSymbol.TabIndex = 0;
            this.labelSymbol.Text = "Symbol:";
            // 
            // panelInfo
            // 
            this.panelInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelInfo.Location = new System.Drawing.Point(0, 47);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Size = new System.Drawing.Size(536, 47);
            this.panelInfo.TabIndex = 1;
            // 
            // panelContent
            // 
            this.panelContent.Controls.Add(this.tabControl1);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(0, 94);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(536, 174);
            this.panelContent.TabIndex = 2;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageGraph);
            this.tabControl1.Controls.Add(this.tabPageDividendData);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(536, 174);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageGraph
            // 
            this.tabPageGraph.Controls.Add(this.formsPlot1);
            this.tabPageGraph.Controls.Add(this.toolStrip1);
            this.tabPageGraph.Location = new System.Drawing.Point(4, 22);
            this.tabPageGraph.Name = "tabPageGraph";
            this.tabPageGraph.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGraph.Size = new System.Drawing.Size(528, 148);
            this.tabPageGraph.TabIndex = 0;
            this.tabPageGraph.Text = "Graph";
            this.tabPageGraph.UseVisualStyleBackColor = true;
            // 
            // tabPageDividendData
            // 
            this.tabPageDividendData.Controls.Add(this.listViewDividendData);
            this.tabPageDividendData.Location = new System.Drawing.Point(4, 22);
            this.tabPageDividendData.Name = "tabPageDividendData";
            this.tabPageDividendData.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDividendData.Size = new System.Drawing.Size(528, 148);
            this.tabPageDividendData.TabIndex = 1;
            this.tabPageDividendData.Text = "Data";
            this.tabPageDividendData.UseVisualStyleBackColor = true;
            // 
            // listViewDividendData
            // 
            this.listViewDividendData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderDate,
            this.columnHeaderDividend});
            this.listViewDividendData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewDividendData.FullRowSelect = true;
            this.listViewDividendData.HideSelection = false;
            this.listViewDividendData.Location = new System.Drawing.Point(3, 3);
            this.listViewDividendData.MultiSelect = false;
            this.listViewDividendData.Name = "listViewDividendData";
            this.listViewDividendData.Size = new System.Drawing.Size(522, 142);
            this.listViewDividendData.TabIndex = 1;
            this.listViewDividendData.UseCompatibleStateImageBehavior = false;
            this.listViewDividendData.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderDate
            // 
            this.columnHeaderDate.Text = "Date";
            this.columnHeaderDate.Width = 104;
            // 
            // columnHeaderDividend
            // 
            this.columnHeaderDividend.Text = "Dividend";
            this.columnHeaderDividend.Width = 162;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripComboGraphInterval});
            this.toolStrip1.Location = new System.Drawing.Point(3, 3);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(522, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // formsPlot1
            // 
            this.formsPlot1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formsPlot1.Location = new System.Drawing.Point(3, 28);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(522, 117);
            this.formsPlot1.TabIndex = 1;
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(39, 22);
            this.toolStripLabel1.Text = "Show:";
            // 
            // toolStripComboGraphInterval
            // 
            this.toolStripComboGraphInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboGraphInterval.Items.AddRange(new object[] {
            "All",
            "Yearly"});
            this.toolStripComboGraphInterval.Name = "toolStripComboGraphInterval";
            this.toolStripComboGraphInterval.Size = new System.Drawing.Size(121, 25);
            this.toolStripComboGraphInterval.SelectedIndexChanged += new System.EventHandler(this.toolStripComboGraphInterval_SelectedIndexChanged);
            // 
            // HistoricalDividendDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 268);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelInfo);
            this.Controls.Add(this.panelTop);
            this.Name = "HistoricalDividendDialog";
            this.Text = "Historical dividends";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelContent.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPageGraph.ResumeLayout(false);
            this.tabPageGraph.PerformLayout();
            this.tabPageDividendData.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.TextBox textBoxSymbol;
        private System.Windows.Forms.Label labelSymbol;
        private System.Windows.Forms.Panel panelInfo;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageGraph;
        private System.Windows.Forms.TabPage tabPageDividendData;
        private System.Windows.Forms.ListView listViewDividendData;
        private System.Windows.Forms.ColumnHeader columnHeaderDate;
        private System.Windows.Forms.ColumnHeader columnHeaderDividend;
        private System.Windows.Forms.DateTimePicker dateTimePickerEndDate;
        private System.Windows.Forms.Label labelEndDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerStartDate;
        private System.Windows.Forms.Label labelStartDate;
        private ScottPlot.FormsPlot formsPlot1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox toolStripComboGraphInterval;
    }
}