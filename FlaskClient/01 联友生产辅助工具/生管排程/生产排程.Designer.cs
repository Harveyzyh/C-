namespace HarveyZ.生管排程
{
    partial class 生产排程
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(生产排程));
            this.DgvMain = new System.Windows.Forms.DataGridView();
            this.BtnShow = new System.Windows.Forms.Button();
            this.PanelTitle = new System.Windows.Forms.Panel();
            this.panelStatusType = new System.Windows.Forms.Panel();
            this.CheckBoxStatusTypeNew = new System.Windows.Forms.CheckBox();
            this.CheckBoxStatusTypeSlChange = new System.Windows.Forms.CheckBox();
            this.CheckBoxStatusTypeDptChange = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.CheckBoxStatusTypeEmpty = new System.Windows.Forms.CheckBox();
            this.CheckBoxStatusTypeDateDelay = new System.Windows.Forms.CheckBox();
            this.CheckBoxStatusTypeDateEarly = new System.Windows.Forms.CheckBox();
            this.panelLabelShow = new System.Windows.Forms.Panel();
            this.labelDdSlSum = new System.Windows.Forms.Label();
            this.labelSxSlSum = new System.Windows.Forms.Label();
            this.labelRowCount = new System.Windows.Forms.Label();
            this.labelCpSumSl = new System.Windows.Forms.Label();
            this.labelBcpZdSumSl = new System.Windows.Forms.Label();
            this.labelBcpBdSumSl = new System.Windows.Forms.Label();
            this.labelBcpFsSumSl = new System.Windows.Forms.Label();
            this.labelBcpTzSumSl = new System.Windows.Forms.Label();
            this.labelWorkTime = new System.Windows.Forms.Label();
            this.TxBoxDptWorkTime = new System.Windows.Forms.TextBox();
            this.labelDptWorkTime = new System.Windows.Forms.Label();
            this.TxBoxWlno = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.CmBoxShowType = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.DtpEndDdDate = new System.Windows.Forms.DateTimePicker();
            this.DtpStartDdDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.BtnSetIndex = new System.Windows.Forms.Button();
            this.TxBoxName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TxBoxOrder = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DtpEndWorkDate = new System.Windows.Forms.DateTimePicker();
            this.BtnOutput = new System.Windows.Forms.Button();
            this.DtpStartWorkDate = new System.Windows.Forms.DateTimePicker();
            this.BtnInput = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.CmBoxDptType = new System.Windows.Forms.ComboBox();
            this.LabelReportSelectDpt = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.contextMenuStrip_DgvMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolTipBtnSetIndex = new System.Windows.Forms.ToolTip(this.components);
            this.CheckBoxStatusTypePlanOverTime = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMain)).BeginInit();
            this.PanelTitle.SuspendLayout();
            this.panelStatusType.SuspendLayout();
            this.panelLabelShow.SuspendLayout();
            this.SuspendLayout();
            // 
            // DgvMain
            // 
            this.DgvMain.AllowDrop = true;
            this.DgvMain.AllowUserToAddRows = false;
            this.DgvMain.AllowUserToDeleteRows = false;
            this.DgvMain.AllowUserToOrderColumns = true;
            this.DgvMain.BackgroundColor = System.Drawing.SystemColors.Control;
            this.DgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvMain.Location = new System.Drawing.Point(0, 182);
            this.DgvMain.MultiSelect = false;
            this.DgvMain.Name = "DgvMain";
            this.DgvMain.ReadOnly = true;
            this.DgvMain.RowHeadersVisible = false;
            this.DgvMain.RowTemplate.Height = 23;
            this.DgvMain.Size = new System.Drawing.Size(1589, 471);
            this.DgvMain.TabIndex = 2;
            this.DgvMain.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DgvMain_CellMouseDown);
            // 
            // BtnShow
            // 
            this.BtnShow.Location = new System.Drawing.Point(5, 105);
            this.BtnShow.Name = "BtnShow";
            this.BtnShow.Size = new System.Drawing.Size(79, 26);
            this.BtnShow.TabIndex = 3;
            this.BtnShow.Text = "查询";
            this.BtnShow.UseVisualStyleBackColor = true;
            this.BtnShow.Click += new System.EventHandler(this.BtnShow_Click);
            // 
            // PanelTitle
            // 
            this.PanelTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelTitle.Controls.Add(this.panelStatusType);
            this.PanelTitle.Controls.Add(this.panelLabelShow);
            this.PanelTitle.Controls.Add(this.TxBoxWlno);
            this.PanelTitle.Controls.Add(this.label8);
            this.PanelTitle.Controls.Add(this.CmBoxShowType);
            this.PanelTitle.Controls.Add(this.label7);
            this.PanelTitle.Controls.Add(this.DtpEndDdDate);
            this.PanelTitle.Controls.Add(this.DtpStartDdDate);
            this.PanelTitle.Controls.Add(this.label3);
            this.PanelTitle.Controls.Add(this.BtnSetIndex);
            this.PanelTitle.Controls.Add(this.TxBoxName);
            this.PanelTitle.Controls.Add(this.label2);
            this.PanelTitle.Controls.Add(this.TxBoxOrder);
            this.PanelTitle.Controls.Add(this.label1);
            this.PanelTitle.Controls.Add(this.DtpEndWorkDate);
            this.PanelTitle.Controls.Add(this.BtnOutput);
            this.PanelTitle.Controls.Add(this.DtpStartWorkDate);
            this.PanelTitle.Controls.Add(this.BtnInput);
            this.PanelTitle.Controls.Add(this.BtnShow);
            this.PanelTitle.Controls.Add(this.label6);
            this.PanelTitle.Controls.Add(this.CmBoxDptType);
            this.PanelTitle.Controls.Add(this.LabelReportSelectDpt);
            this.PanelTitle.Controls.Add(this.label5);
            this.PanelTitle.Controls.Add(this.label4);
            this.PanelTitle.Location = new System.Drawing.Point(0, 0);
            this.PanelTitle.Margin = new System.Windows.Forms.Padding(2);
            this.PanelTitle.Name = "PanelTitle";
            this.PanelTitle.Size = new System.Drawing.Size(1146, 140);
            this.PanelTitle.TabIndex = 4;
            // 
            // panelStatusType
            // 
            this.panelStatusType.Controls.Add(this.CheckBoxStatusTypePlanOverTime);
            this.panelStatusType.Controls.Add(this.CheckBoxStatusTypeNew);
            this.panelStatusType.Controls.Add(this.CheckBoxStatusTypeSlChange);
            this.panelStatusType.Controls.Add(this.CheckBoxStatusTypeDptChange);
            this.panelStatusType.Controls.Add(this.label9);
            this.panelStatusType.Controls.Add(this.CheckBoxStatusTypeEmpty);
            this.panelStatusType.Controls.Add(this.CheckBoxStatusTypeDateDelay);
            this.panelStatusType.Controls.Add(this.CheckBoxStatusTypeDateEarly);
            this.panelStatusType.Location = new System.Drawing.Point(83, 66);
            this.panelStatusType.Name = "panelStatusType";
            this.panelStatusType.Size = new System.Drawing.Size(714, 37);
            this.panelStatusType.TabIndex = 52;
            // 
            // CheckBoxStatusTypeNew
            // 
            this.CheckBoxStatusTypeNew.AutoSize = true;
            this.CheckBoxStatusTypeNew.Checked = true;
            this.CheckBoxStatusTypeNew.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxStatusTypeNew.Location = new System.Drawing.Point(138, 9);
            this.CheckBoxStatusTypeNew.Name = "CheckBoxStatusTypeNew";
            this.CheckBoxStatusTypeNew.Size = new System.Drawing.Size(54, 18);
            this.CheckBoxStatusTypeNew.TabIndex = 52;
            this.CheckBoxStatusTypeNew.Text = "新增";
            this.CheckBoxStatusTypeNew.UseVisualStyleBackColor = true;
            this.CheckBoxStatusTypeNew.CheckedChanged += new System.EventHandler(this.CheckBoxStatus_CheckedChanged);
            // 
            // CheckBoxStatusTypeSlChange
            // 
            this.CheckBoxStatusTypeSlChange.AutoSize = true;
            this.CheckBoxStatusTypeSlChange.Checked = true;
            this.CheckBoxStatusTypeSlChange.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxStatusTypeSlChange.Location = new System.Drawing.Point(395, 9);
            this.CheckBoxStatusTypeSlChange.Name = "CheckBoxStatusTypeSlChange";
            this.CheckBoxStatusTypeSlChange.Size = new System.Drawing.Size(82, 18);
            this.CheckBoxStatusTypeSlChange.TabIndex = 50;
            this.CheckBoxStatusTypeSlChange.Text = "数量变更";
            this.CheckBoxStatusTypeSlChange.UseVisualStyleBackColor = true;
            this.CheckBoxStatusTypeSlChange.CheckedChanged += new System.EventHandler(this.CheckBoxStatus_CheckedChanged);
            // 
            // CheckBoxStatusTypeDptChange
            // 
            this.CheckBoxStatusTypeDptChange.AutoSize = true;
            this.CheckBoxStatusTypeDptChange.Checked = true;
            this.CheckBoxStatusTypeDptChange.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxStatusTypeDptChange.Location = new System.Drawing.Point(490, 9);
            this.CheckBoxStatusTypeDptChange.Name = "CheckBoxStatusTypeDptChange";
            this.CheckBoxStatusTypeDptChange.Size = new System.Drawing.Size(82, 18);
            this.CheckBoxStatusTypeDptChange.TabIndex = 51;
            this.CheckBoxStatusTypeDptChange.Text = "部门变更";
            this.CheckBoxStatusTypeDptChange.UseVisualStyleBackColor = true;
            this.CheckBoxStatusTypeDptChange.CheckedChanged += new System.EventHandler(this.CheckBoxStatus_CheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 11F);
            this.label9.Location = new System.Drawing.Point(7, 10);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 15);
            this.label9.TabIndex = 46;
            this.label9.Text = "状态类型：";
            // 
            // CheckBoxStatusTypeEmpty
            // 
            this.CheckBoxStatusTypeEmpty.AutoSize = true;
            this.CheckBoxStatusTypeEmpty.Checked = true;
            this.CheckBoxStatusTypeEmpty.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxStatusTypeEmpty.Location = new System.Drawing.Point(89, 9);
            this.CheckBoxStatusTypeEmpty.Name = "CheckBoxStatusTypeEmpty";
            this.CheckBoxStatusTypeEmpty.Size = new System.Drawing.Size(40, 18);
            this.CheckBoxStatusTypeEmpty.TabIndex = 47;
            this.CheckBoxStatusTypeEmpty.Text = "空";
            this.CheckBoxStatusTypeEmpty.UseVisualStyleBackColor = true;
            this.CheckBoxStatusTypeEmpty.CheckedChanged += new System.EventHandler(this.CheckBoxStatus_CheckedChanged);
            // 
            // CheckBoxStatusTypeDateDelay
            // 
            this.CheckBoxStatusTypeDateDelay.AutoSize = true;
            this.CheckBoxStatusTypeDateDelay.Checked = true;
            this.CheckBoxStatusTypeDateDelay.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxStatusTypeDateDelay.Location = new System.Drawing.Point(297, 9);
            this.CheckBoxStatusTypeDateDelay.Name = "CheckBoxStatusTypeDateDelay";
            this.CheckBoxStatusTypeDateDelay.Size = new System.Drawing.Size(82, 18);
            this.CheckBoxStatusTypeDateDelay.TabIndex = 49;
            this.CheckBoxStatusTypeDateDelay.Text = "上线延后";
            this.CheckBoxStatusTypeDateDelay.UseVisualStyleBackColor = true;
            this.CheckBoxStatusTypeDateDelay.CheckedChanged += new System.EventHandler(this.CheckBoxStatus_CheckedChanged);
            // 
            // CheckBoxStatusTypeDateEarly
            // 
            this.CheckBoxStatusTypeDateEarly.AutoSize = true;
            this.CheckBoxStatusTypeDateEarly.Checked = true;
            this.CheckBoxStatusTypeDateEarly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxStatusTypeDateEarly.Location = new System.Drawing.Point(203, 9);
            this.CheckBoxStatusTypeDateEarly.Name = "CheckBoxStatusTypeDateEarly";
            this.CheckBoxStatusTypeDateEarly.Size = new System.Drawing.Size(82, 18);
            this.CheckBoxStatusTypeDateEarly.TabIndex = 48;
            this.CheckBoxStatusTypeDateEarly.Text = "上线提前";
            this.CheckBoxStatusTypeDateEarly.UseVisualStyleBackColor = true;
            this.CheckBoxStatusTypeDateEarly.CheckedChanged += new System.EventHandler(this.CheckBoxStatus_CheckedChanged);
            // 
            // panelLabelShow
            // 
            this.panelLabelShow.Controls.Add(this.labelDdSlSum);
            this.panelLabelShow.Controls.Add(this.labelSxSlSum);
            this.panelLabelShow.Controls.Add(this.labelRowCount);
            this.panelLabelShow.Controls.Add(this.labelCpSumSl);
            this.panelLabelShow.Controls.Add(this.labelBcpZdSumSl);
            this.panelLabelShow.Controls.Add(this.labelBcpBdSumSl);
            this.panelLabelShow.Controls.Add(this.labelBcpFsSumSl);
            this.panelLabelShow.Controls.Add(this.labelBcpTzSumSl);
            this.panelLabelShow.Controls.Add(this.labelWorkTime);
            this.panelLabelShow.Controls.Add(this.TxBoxDptWorkTime);
            this.panelLabelShow.Controls.Add(this.labelDptWorkTime);
            this.panelLabelShow.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelLabelShow.Font = new System.Drawing.Font("宋体", 11F);
            this.panelLabelShow.Location = new System.Drawing.Point(805, 0);
            this.panelLabelShow.Name = "panelLabelShow";
            this.panelLabelShow.Size = new System.Drawing.Size(339, 138);
            this.panelLabelShow.TabIndex = 41;
            // 
            // labelDdSlSum
            // 
            this.labelDdSlSum.AutoSize = true;
            this.labelDdSlSum.Font = new System.Drawing.Font("宋体", 11F);
            this.labelDdSlSum.Location = new System.Drawing.Point(20, 22);
            this.labelDdSlSum.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelDdSlSum.Name = "labelDdSlSum";
            this.labelDdSlSum.Size = new System.Drawing.Size(97, 15);
            this.labelDdSlSum.TabIndex = 25;
            this.labelDdSlSum.Text = "订单总数量：";
            // 
            // labelSxSlSum
            // 
            this.labelSxSlSum.AutoSize = true;
            this.labelSxSlSum.Font = new System.Drawing.Font("宋体", 11F);
            this.labelSxSlSum.Location = new System.Drawing.Point(20, 41);
            this.labelSxSlSum.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelSxSlSum.Name = "labelSxSlSum";
            this.labelSxSlSum.Size = new System.Drawing.Size(97, 15);
            this.labelSxSlSum.TabIndex = 24;
            this.labelSxSlSum.Text = "上线总数量：";
            // 
            // labelRowCount
            // 
            this.labelRowCount.AutoSize = true;
            this.labelRowCount.Font = new System.Drawing.Font("宋体", 11F);
            this.labelRowCount.Location = new System.Drawing.Point(48, 3);
            this.labelRowCount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelRowCount.Name = "labelRowCount";
            this.labelRowCount.Size = new System.Drawing.Size(67, 15);
            this.labelRowCount.TabIndex = 28;
            this.labelRowCount.Text = "总行数：";
            // 
            // labelCpSumSl
            // 
            this.labelCpSumSl.AutoSize = true;
            this.labelCpSumSl.Font = new System.Drawing.Font("宋体", 11F);
            this.labelCpSumSl.Location = new System.Drawing.Point(20, 60);
            this.labelCpSumSl.Name = "labelCpSumSl";
            this.labelCpSumSl.Size = new System.Drawing.Size(97, 15);
            this.labelCpSumSl.TabIndex = 31;
            this.labelCpSumSl.Text = "成品总数量：";
            // 
            // labelBcpZdSumSl
            // 
            this.labelBcpZdSumSl.AutoSize = true;
            this.labelBcpZdSumSl.Font = new System.Drawing.Font("宋体", 11F);
            this.labelBcpZdSumSl.Location = new System.Drawing.Point(184, 3);
            this.labelBcpZdSumSl.Name = "labelBcpZdSumSl";
            this.labelBcpZdSumSl.Size = new System.Drawing.Size(112, 15);
            this.labelBcpZdSumSl.TabIndex = 32;
            this.labelBcpZdSumSl.Text = "座垫组立数量：";
            // 
            // labelBcpBdSumSl
            // 
            this.labelBcpBdSumSl.AutoSize = true;
            this.labelBcpBdSumSl.Font = new System.Drawing.Font("宋体", 11F);
            this.labelBcpBdSumSl.Location = new System.Drawing.Point(184, 22);
            this.labelBcpBdSumSl.Name = "labelBcpBdSumSl";
            this.labelBcpBdSumSl.Size = new System.Drawing.Size(112, 15);
            this.labelBcpBdSumSl.TabIndex = 33;
            this.labelBcpBdSumSl.Text = "背垫组立数量：";
            // 
            // labelBcpFsSumSl
            // 
            this.labelBcpFsSumSl.AutoSize = true;
            this.labelBcpFsSumSl.Font = new System.Drawing.Font("宋体", 11F);
            this.labelBcpFsSumSl.Location = new System.Drawing.Point(184, 41);
            this.labelBcpFsSumSl.Name = "labelBcpFsSumSl";
            this.labelBcpFsSumSl.Size = new System.Drawing.Size(112, 15);
            this.labelBcpFsSumSl.TabIndex = 33;
            this.labelBcpFsSumSl.Text = "扶手组立数量：";
            // 
            // labelBcpTzSumSl
            // 
            this.labelBcpTzSumSl.AutoSize = true;
            this.labelBcpTzSumSl.Font = new System.Drawing.Font("宋体", 11F);
            this.labelBcpTzSumSl.Location = new System.Drawing.Point(184, 60);
            this.labelBcpTzSumSl.Name = "labelBcpTzSumSl";
            this.labelBcpTzSumSl.Size = new System.Drawing.Size(112, 15);
            this.labelBcpTzSumSl.TabIndex = 33;
            this.labelBcpTzSumSl.Text = "头枕组立数量：";
            // 
            // labelWorkTime
            // 
            this.labelWorkTime.AutoSize = true;
            this.labelWorkTime.Location = new System.Drawing.Point(199, 91);
            this.labelWorkTime.Name = "labelWorkTime";
            this.labelWorkTime.Size = new System.Drawing.Size(97, 15);
            this.labelWorkTime.TabIndex = 36;
            this.labelWorkTime.Text = "排程总工时：";
            // 
            // TxBoxDptWorkTime
            // 
            this.TxBoxDptWorkTime.Location = new System.Drawing.Point(111, 88);
            this.TxBoxDptWorkTime.Name = "TxBoxDptWorkTime";
            this.TxBoxDptWorkTime.Size = new System.Drawing.Size(82, 24);
            this.TxBoxDptWorkTime.TabIndex = 35;
            // 
            // labelDptWorkTime
            // 
            this.labelDptWorkTime.AutoSize = true;
            this.labelDptWorkTime.Location = new System.Drawing.Point(33, 91);
            this.labelDptWorkTime.Name = "labelDptWorkTime";
            this.labelDptWorkTime.Size = new System.Drawing.Size(82, 15);
            this.labelDptWorkTime.TabIndex = 34;
            this.labelDptWorkTime.Text = "生产工时：";
            // 
            // TxBoxWlno
            // 
            this.TxBoxWlno.Location = new System.Drawing.Point(518, 106);
            this.TxBoxWlno.Margin = new System.Windows.Forms.Padding(2);
            this.TxBoxWlno.Name = "TxBoxWlno";
            this.TxBoxWlno.Size = new System.Drawing.Size(146, 23);
            this.TxBoxWlno.TabIndex = 45;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(476, 110);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 14);
            this.label8.TabIndex = 44;
            this.label8.Text = "品号：";
            // 
            // CmBoxShowType
            // 
            this.CmBoxShowType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmBoxShowType.Font = new System.Drawing.Font("宋体", 12F);
            this.CmBoxShowType.FormattingEnabled = true;
            this.CmBoxShowType.Items.AddRange(new object[] {
            "无",
            "总已排数量>ERP订单数量",
            "总已排数量<ERP订单数量",
            "上线数量不等于绑定工单产量",
            "已排但存在变更订单",
            "未排订单"});
            this.CmBoxShowType.Location = new System.Drawing.Point(374, 38);
            this.CmBoxShowType.Name = "CmBoxShowType";
            this.CmBoxShowType.Size = new System.Drawing.Size(285, 24);
            this.CmBoxShowType.TabIndex = 43;
            this.CmBoxShowType.SelectedIndexChanged += new System.EventHandler(this.CmBoxShowType_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(304, 42);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 14);
            this.label7.TabIndex = 42;
            this.label7.Text = "显示模式：";
            // 
            // DtpEndDdDate
            // 
            this.DtpEndDdDate.Checked = false;
            this.DtpEndDdDate.CustomFormat = "yyyy-MM-dd";
            this.DtpEndDdDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpEndDdDate.Location = new System.Drawing.Point(668, 5);
            this.DtpEndDdDate.Name = "DtpEndDdDate";
            this.DtpEndDdDate.ShowCheckBox = true;
            this.DtpEndDdDate.Size = new System.Drawing.Size(129, 23);
            this.DtpEndDdDate.TabIndex = 40;
            this.DtpEndDdDate.ValueChanged += new System.EventHandler(this.DtpEndDdDate_ValueChanged);
            // 
            // DtpStartDdDate
            // 
            this.DtpStartDdDate.Checked = false;
            this.DtpStartDdDate.CustomFormat = "yyyy-MM-dd";
            this.DtpStartDdDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpStartDdDate.Location = new System.Drawing.Point(521, 5);
            this.DtpStartDdDate.Name = "DtpStartDdDate";
            this.DtpStartDdDate.ShowCheckBox = true;
            this.DtpStartDdDate.Size = new System.Drawing.Size(129, 23);
            this.DtpStartDdDate.TabIndex = 38;
            this.DtpStartDdDate.ValueChanged += new System.EventHandler(this.DtpStartDdDate_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(451, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 14);
            this.label3.TabIndex = 37;
            this.label3.Text = "订单日期：";
            // 
            // BtnSetIndex
            // 
            this.BtnSetIndex.Enabled = false;
            this.BtnSetIndex.Location = new System.Drawing.Point(5, 71);
            this.BtnSetIndex.Margin = new System.Windows.Forms.Padding(2);
            this.BtnSetIndex.Name = "BtnSetIndex";
            this.BtnSetIndex.Size = new System.Drawing.Size(79, 26);
            this.BtnSetIndex.TabIndex = 30;
            this.BtnSetIndex.Tag = "";
            this.BtnSetIndex.Text = "同步序号";
            this.toolTipBtnSetIndex.SetToolTip(this.BtnSetIndex, "把排程序号同步到相应的工单。\r\n以达到把工单与排程绑定。\r\n注意：只同步排程及工单只有一笔的数据");
            this.BtnSetIndex.UseVisualStyleBackColor = true;
            this.BtnSetIndex.Click += new System.EventHandler(this.BtnSetIndex_Click);
            // 
            // TxBoxName
            // 
            this.TxBoxName.Location = new System.Drawing.Point(345, 106);
            this.TxBoxName.Margin = new System.Windows.Forms.Padding(2);
            this.TxBoxName.Name = "TxBoxName";
            this.TxBoxName.Size = new System.Drawing.Size(121, 23);
            this.TxBoxName.TabIndex = 23;
            this.TxBoxName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxBoxName_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(303, 110);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 14);
            this.label2.TabIndex = 22;
            this.label2.Text = "品名：";
            // 
            // TxBoxOrder
            // 
            this.TxBoxOrder.Location = new System.Drawing.Point(150, 106);
            this.TxBoxOrder.Margin = new System.Windows.Forms.Padding(2);
            this.TxBoxOrder.Name = "TxBoxOrder";
            this.TxBoxOrder.Size = new System.Drawing.Size(146, 23);
            this.TxBoxOrder.TabIndex = 21;
            this.TxBoxOrder.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxBoxOrder_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 11F);
            this.label1.Location = new System.Drawing.Point(90, 110);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 20;
            this.label1.Text = "订单号：";
            // 
            // DtpEndWorkDate
            // 
            this.DtpEndWorkDate.CustomFormat = "yyyy-MM-dd";
            this.DtpEndWorkDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpEndWorkDate.Location = new System.Drawing.Point(314, 5);
            this.DtpEndWorkDate.Margin = new System.Windows.Forms.Padding(2);
            this.DtpEndWorkDate.Name = "DtpEndWorkDate";
            this.DtpEndWorkDate.ShowCheckBox = true;
            this.DtpEndWorkDate.Size = new System.Drawing.Size(129, 23);
            this.DtpEndWorkDate.TabIndex = 19;
            this.DtpEndWorkDate.ValueChanged += new System.EventHandler(this.DtpEndWorkDate_ValueChanged);
            // 
            // BtnOutput
            // 
            this.BtnOutput.Enabled = false;
            this.BtnOutput.Location = new System.Drawing.Point(5, 37);
            this.BtnOutput.Margin = new System.Windows.Forms.Padding(2);
            this.BtnOutput.Name = "BtnOutput";
            this.BtnOutput.Size = new System.Drawing.Size(79, 26);
            this.BtnOutput.TabIndex = 5;
            this.BtnOutput.Text = "导出";
            this.BtnOutput.UseVisualStyleBackColor = true;
            this.BtnOutput.Click += new System.EventHandler(this.BtnOutput_Click);
            // 
            // DtpStartWorkDate
            // 
            this.DtpStartWorkDate.CustomFormat = "yyyy-MM-dd";
            this.DtpStartWorkDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpStartWorkDate.Location = new System.Drawing.Point(165, 5);
            this.DtpStartWorkDate.Margin = new System.Windows.Forms.Padding(2);
            this.DtpStartWorkDate.Name = "DtpStartWorkDate";
            this.DtpStartWorkDate.ShowCheckBox = true;
            this.DtpStartWorkDate.Size = new System.Drawing.Size(129, 23);
            this.DtpStartWorkDate.TabIndex = 18;
            this.DtpStartWorkDate.TabStop = false;
            this.DtpStartWorkDate.ValueChanged += new System.EventHandler(this.DtpStartWorkDate_ValueChanged);
            // 
            // BtnInput
            // 
            this.BtnInput.Enabled = false;
            this.BtnInput.Location = new System.Drawing.Point(5, 3);
            this.BtnInput.Margin = new System.Windows.Forms.Padding(2);
            this.BtnInput.Name = "BtnInput";
            this.BtnInput.Size = new System.Drawing.Size(79, 26);
            this.BtnInput.TabIndex = 4;
            this.BtnInput.Text = "导入";
            this.BtnInput.UseVisualStyleBackColor = true;
            this.BtnInput.Click += new System.EventHandler(this.BtnInput_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 11F);
            this.label6.Location = new System.Drawing.Point(293, 9);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(23, 15);
            this.label6.TabIndex = 16;
            this.label6.Text = "--";
            // 
            // CmBoxDptType
            // 
            this.CmBoxDptType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmBoxDptType.Font = new System.Drawing.Font("宋体", 12F);
            this.CmBoxDptType.FormattingEnabled = true;
            this.CmBoxDptType.Location = new System.Drawing.Point(165, 37);
            this.CmBoxDptType.Margin = new System.Windows.Forms.Padding(2);
            this.CmBoxDptType.Name = "CmBoxDptType";
            this.CmBoxDptType.Size = new System.Drawing.Size(129, 24);
            this.CmBoxDptType.TabIndex = 17;
            this.CmBoxDptType.TabStop = false;
            this.CmBoxDptType.SelectedIndexChanged += new System.EventHandler(this.CmBoxDptType_SelectedIndexChanged);
            // 
            // LabelReportSelectDpt
            // 
            this.LabelReportSelectDpt.AutoSize = true;
            this.LabelReportSelectDpt.Font = new System.Drawing.Font("宋体", 11F);
            this.LabelReportSelectDpt.Location = new System.Drawing.Point(90, 41);
            this.LabelReportSelectDpt.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LabelReportSelectDpt.Name = "LabelReportSelectDpt";
            this.LabelReportSelectDpt.Size = new System.Drawing.Size(82, 15);
            this.LabelReportSelectDpt.TabIndex = 14;
            this.LabelReportSelectDpt.Text = "生产部门：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 11F);
            this.label5.Location = new System.Drawing.Point(90, 9);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 15);
            this.label5.TabIndex = 15;
            this.label5.Text = "上线日期：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(649, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 14);
            this.label4.TabIndex = 39;
            this.label4.Text = "--";
            // 
            // contextMenuStrip_DgvMain
            // 
            this.contextMenuStrip_DgvMain.Name = "contextMenuStrip_DgvMain";
            this.contextMenuStrip_DgvMain.Size = new System.Drawing.Size(61, 4);
            this.contextMenuStrip_DgvMain.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip_DgvMain_ItemClicked);
            // 
            // toolTipBtnSetIndex
            // 
            this.toolTipBtnSetIndex.AutoPopDelay = 10000;
            this.toolTipBtnSetIndex.InitialDelay = 500;
            this.toolTipBtnSetIndex.ReshowDelay = 100;
            // 
            // CheckBoxStatusTypePlanOverTime
            // 
            this.CheckBoxStatusTypePlanOverTime.AutoSize = true;
            this.CheckBoxStatusTypePlanOverTime.Checked = true;
            this.CheckBoxStatusTypePlanOverTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxStatusTypePlanOverTime.Location = new System.Drawing.Point(580, 10);
            this.CheckBoxStatusTypePlanOverTime.Name = "CheckBoxStatusTypePlanOverTime";
            this.CheckBoxStatusTypePlanOverTime.Size = new System.Drawing.Size(138, 18);
            this.CheckBoxStatusTypePlanOverTime.TabIndex = 53;
            this.CheckBoxStatusTypePlanOverTime.Text = "上线日期跨度过大";
            this.CheckBoxStatusTypePlanOverTime.UseVisualStyleBackColor = true;
            // 
            // 生产排程
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1149, 719);
            this.Controls.Add(this.PanelTitle);
            this.Controls.Add(this.DgvMain);
            this.Font = new System.Drawing.Font("宋体", 10F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "生产排程";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "生管排程导入";
            this.SizeChanged += new System.EventHandler(this.FormMain_Resized);
            ((System.ComponentModel.ISupportInitialize)(this.DgvMain)).EndInit();
            this.PanelTitle.ResumeLayout(false);
            this.PanelTitle.PerformLayout();
            this.panelStatusType.ResumeLayout(false);
            this.panelStatusType.PerformLayout();
            this.panelLabelShow.ResumeLayout(false);
            this.panelLabelShow.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView DgvMain;
        private System.Windows.Forms.Button BtnShow;
        private System.Windows.Forms.Panel PanelTitle;
        private System.Windows.Forms.Button BtnOutput;
        private System.Windows.Forms.Button BtnInput;
        private System.Windows.Forms.DateTimePicker DtpEndWorkDate;
        private System.Windows.Forms.DateTimePicker DtpStartWorkDate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox CmBoxDptType;
        private System.Windows.Forms.Label LabelReportSelectDpt;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TxBoxName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxBoxOrder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelSxSlSum;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_DgvMain;
        private System.Windows.Forms.Label labelDdSlSum;
        private System.Windows.Forms.Label labelRowCount;
        private System.Windows.Forms.Button BtnSetIndex;
        private System.Windows.Forms.ToolTip toolTipBtnSetIndex;
        private System.Windows.Forms.Label labelBcpTzSumSl;
        private System.Windows.Forms.Label labelBcpFsSumSl;
        private System.Windows.Forms.Label labelBcpBdSumSl;
        private System.Windows.Forms.Label labelBcpZdSumSl;
        private System.Windows.Forms.Label labelCpSumSl;
        private System.Windows.Forms.Label labelWorkTime;
        private System.Windows.Forms.TextBox TxBoxDptWorkTime;
        private System.Windows.Forms.Label labelDptWorkTime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker DtpStartDdDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker DtpEndDdDate;
        private System.Windows.Forms.ComboBox CmBoxShowType;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panelLabelShow;
        private System.Windows.Forms.TextBox TxBoxWlno;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panelStatusType;
        private System.Windows.Forms.CheckBox CheckBoxStatusTypeSlChange;
        private System.Windows.Forms.CheckBox CheckBoxStatusTypeDptChange;
        private System.Windows.Forms.CheckBox CheckBoxStatusTypeEmpty;
        private System.Windows.Forms.CheckBox CheckBoxStatusTypeDateDelay;
        private System.Windows.Forms.CheckBox CheckBoxStatusTypeDateEarly;
        private System.Windows.Forms.CheckBox CheckBoxStatusTypeNew;
        private System.Windows.Forms.CheckBox CheckBoxStatusTypePlanOverTime;
    }
}

