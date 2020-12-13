using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HarveyZ;

namespace HarveyZ.仓储中心
{
    public partial class 录入退货单 : Form
    {
        #region 局部变量设定
        Mssql mssql = new Mssql();
        private static string connYF = FormLogin.infObj.connYF;
        ERP_Create_Purti createPurti = new ERP_Create_Purti(connYF);

        private bool newFlag = false;
        private bool editFlag = false;
        private bool delFlag = false;
        private bool outFlag = false;
        private bool lockFlag = false;
        private bool printFlag = false;

        public static string Mode = null;
        public static string Title = null;
        public static string GetMain = null;
        public static string GetOther = null;
        public static string MaterielMode = null;

        private string SupplierID = null;
        private string TypeID = "3501";
        private string PositionID = "P013";
        private string MaterielID = null;
        private string LoginUid = FormLogin.infObj.userId;
        private string LoginUserGroup = FormLogin.infObj.userGroup;

        private bool MsgFlag = false;
        #endregion

        #region 窗口初始化
        public 录入退货单(string text = "")
        {
            InitializeComponent();
            this.Text = text == "" ? this.Text : text;
            FormLogin.infObj.userPermission.GetPermUserDetail(FormLogin.infObj.userId, this.Text, out newFlag, out editFlag, out delFlag, out outFlag, out lockFlag, out printFlag);
            Init();

            DgvOpt.SetRowBackColor(DataGridView_List);
            DgvOpt.SetColHeadMiddleCenter(DataGridView_List);
        }

        private void Init()
        {
            入库单别.Text = "3501-采购退货单";
            入库仓库.Text = "P013-仓储原材料仓";

            panel_Title.Enabled = true;
            panel_Last.Enabled = false;
        }
        #endregion

        #region 窗口大小变化设置
        private void FormMain_Resized(object sender, EventArgs e)
        {
            FormMain_Resized_Work();
        }

        private void FormMain_Resized_Work()
        {
            //窗框大小
            int FormWidth, FormHeight;
            FormWidth = Width;
            FormHeight = Height;
            panel_Title.Location = new Point(1, 1);
            panel_Title.Size = new Size(FormWidth - 2, panel_Title.Height);

            panel_Last.Location = new Point(panel_Title.Location.X, panel_Title.Height + DataGridView_List.Height);
            panel_Last.Size = new Size(panel_Title.Width, panel_Last.Height);

            DataGridView_List.Location = new Point(panel_Title.Location.X, panel_Title.Location.Y + panel_Title.Height + 1);
            DataGridView_List.Size = new Size(panel_Title.Width, FormHeight - (panel_Title.Height + 1) - (panel_Last.Height + 1));
        }
        #endregion

        #region UI按钮
        private void 入库仓库L_MouseClick(object sender, MouseEventArgs e)
        {
            Mode = "PositionID";
            Title = "仓库编号";
            //Title = "仓库编号|仓库名称";
            录入退货单_获取单据信息 frm = new 录入退货单_获取单据信息(connYF, Title, Mode);
            if (frm.ShowDialog() == DialogResult.Cancel)
            {
                GetMain = frm.GetMain;
                GetOther = frm.GetOther;

                if (GetMain != null)
                {
                    入库仓库.Text = GetMain + "-" + GetOther;
                    PositionID = GetMain;
                    GetMain = null;
                    GetOther = null;
                }
                frm.Dispose();
            }
        }

        private void 供应商L_MouseClick(object sender, MouseEventArgs e)
        {
            Mode = "SupplierID";
            Title = "供应商编号";
            //Title = "供应商编号|简称";
            录入退货单_获取单据信息 frm = new 录入退货单_获取单据信息(connYF, Title, Mode);
            if (frm.ShowDialog() == DialogResult.Cancel)
            {
                GetMain = frm.GetMain;
                GetOther = frm.GetOther;

                if (GetMain != null)
                {
                    供应商.Text = GetMain + "-" + GetOther;
                    SupplierID = GetMain;
                    GetMain = null;
                    GetOther = null;
                }
                frm.Dispose();
                MsgFlag = true;
                条码T.Select();
                条码T.SelectAll();
            }
        }

        private void 入库单别L_MouseClick(object sender, MouseEventArgs e)
        {
            Mode = "TypeID";
            Title = "单别";
            //Title = "单别|单据简称";
            录入退货单_获取单据信息 frm = new 录入退货单_获取单据信息(connYF, Title, Mode);
            if (frm.ShowDialog() == DialogResult.Cancel)
            {
                GetMain = frm.GetMain;
                GetOther = frm.GetOther;

                if (GetMain != null)
                {
                    入库单别.Text = GetMain + "-" + GetOther;
                    TypeID = GetMain;
                    GetMain = null;
                    GetOther = null;
                }
                frm.Dispose();
            }
        }

        private void 送货单号_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && MsgFlag == true)
            {
                MsgFlag = false;
                return;
            }
            else
            {
                MsgFlag = false;
            }

            if ((MsgFlag == true) && (e.KeyCode == Keys.Enter))
            {
                MsgFlag = false;
                return;
            }
            if (e.KeyCode == Keys.Enter)
            {
                条码T.Select();
            }
        }

        private void 条码_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter && MsgFlag == true)
            {
                MsgFlag = false;
                return;
            }
            else
            {
                MsgFlag = false;
            }

            if (e.KeyCode == Keys.Enter)
            {
                条码T.Text = 条码T.Text.ToUpper();
                if (入库单别.Text == "" || 供应商.Text == "" || 入库仓库.Text == "" || 送货单号T.Text == "")
                {
                    MsgFlag = true;
                    MessageBox.Show("请先查询或填写 退货单别，供应商，退货仓库 的信息！", "错误");
                    条码T.Select();
                    条码T.SelectAll();
                }
                else if(条码T.Text.Length > 9)
                {
                    if(!CheckSupplierID(条码T.Text.Substring(条码T.Text.Length-5, 5)))
                    {
                        MsgFlag = true;
                        MessageBox.Show("条码中的供应商编码与选择的不对应！请检测条码。", "错误");
                        条码T.Select();
                        条码T.SelectAll();
                    }
                    else
                    {
                        MaterielID = 条码T.Text.Substring(0, 条码T.Text.Length - 5);
                        数量T.Select();
                    }
                }
                if(数量T.Text == "")
                {
                    数量T.Text = "1";
                    数量T.SelectAll();
                }
                else
                {
                    数量T.SelectAll();
                }
            }
        }

        private void 数量_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter && MsgFlag == true)
            {
                MsgFlag = false;
                return;
            }
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    float.Parse(数量T.Text);
                    AddData();
                    GetIndex();
                }
                catch
                {
                    MsgFlag = true;
                    MessageBox.Show("数量输入错误！", "错误");
                    数量T.SelectAll();
                }
            }
            selectLastRow(this.DataGridView_List);
            DgvOpt.SetRowBackColor(this.DataGridView_List);
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in this.DataGridView_List.SelectedRows)
            {
                this.DataGridView_List.Rows.Remove(item);
            }
            条码T.SelectAll();
            条码T.Select();
            SetEnable();
            GetIndex();
        }

        private void buttonUpload_Click(object sender, EventArgs e)
        {
            Insert();
            SetEnable();
            GetIndex();
        }

        private void selectLastRow(DataGridView dgv = null)
        {
            int kk = dgv.RowCount;
            if (dgv.RowCount > 0 && dgv != null)
            {
                dgv.CurrentCell = dgv.Rows[dgv.RowCount - 1].Cells[0];
            }
        }
        #endregion

        #region 前端业务逻辑
        private string GetDate()
        {
            return dateTimePicker1.Value.ToString("yyyyMMdd");
        }

        private string GetFlowID(string flowId = null)
        {
            string time = mssql.SQLselect(connYF, "SELECT dbo.f_getTime(1) ").Rows[0][0].ToString();

            if (flowId == null)
            {
                flowId = "TH" + time + "0001";
            }
            else
            {
                if (mssql.SQLselect(connYF, string.Format("SELECT RTRIM(THXA005) FROM dbo.TH_LYXA WHERE THXA005 = '{0}' ", flowId)) == null)
                {
                    return GetFlowID(flowId);
                }
            }
            return flowId;
        }

        private string GetTime()
        {
            string slqStr = "SELECT dbo.f_getTime(1) ";
            DataTable dt = mssql.SQLselect(connYF, slqStr);
            if(dt != null)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return null;
            }
        }

        private DataTable GetMaterielInfo(string MaterielID, string SupplierID)
        {
            string slqStr = "SELECT RTRIM(TH004), RTRIM(MB002), RTRIM(MB003), RTRIM(TG005), SL FROM V_PURTH_SL_WG "
                            + "INNER JOIN INVMB ON MB001 = TH004 "
                            + "WHERE TH004 = '{0}' AND TG005 = '{1}' ";
            DataTable dt = mssql.SQLselect(connYF, string.Format(slqStr, MaterielID, SupplierID));
            if(dt != null)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }

        private bool GetMaterielExist(string MaterielID)
        {
            string slqStr = "SELECT MB001 FROM INVMB WHERE MB001 = '{0}' ";
            DataTable dt = mssql.SQLselect(connYF, string.Format(slqStr, MaterielID));
            if(dt != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void GetIndex()//重排dgv的序号并且设置部分列居中显示
        {
            if(DataGridView_List.Rows.Count > 0)
            {
                int Count = DataGridView_List.Rows.Count;
                int Index = 0;
                for(Index = 0; Index < Count; Index++)
                {
                    DataGridView_List.Rows[Index].Cells[0].Value = (Index + 1).ToString().PadLeft(3, '0');
                    DgvOpt.SetColMiddleCenter(DataGridView_List, 0);
                    DgvOpt.SetColMiddleCenter(DataGridView_List, 4);
                    DgvOpt.SetColMiddleCenter(DataGridView_List, 5);
                    DgvOpt.SetColMiddleCenter(DataGridView_List, 6);
                    DgvOpt.SetColMiddleCenter(DataGridView_List, 7);
                }
            }
            else
            {
                return;
            }
        }

        private void AddData()
        {
            if (MaterielID != null)
            {
                DataTable dt = GetMaterielInfo(MaterielID, SupplierID);
                if(dt != null)
                {
                    float shuliang = -2;
                    try
                    {
                        shuliang = float.Parse(数量T.Text);
                        try
                        {
                            float k = float.Parse(dt.Rows[0][4].ToString());
                            if (k >= shuliang)
                            {
                                int index = this.DataGridView_List.Rows.Add();
                                this.DataGridView_List.Rows[index].Cells[1].Value = MaterielID;
                                this.DataGridView_List.Rows[index].Cells[2].Value = dt.Rows[0][1];
                                this.DataGridView_List.Rows[index].Cells[3].Value = dt.Rows[0][2];
                                this.DataGridView_List.Rows[index].Cells[4].Value = 数量T.Text;
                                this.DataGridView_List.Rows[index].Cells[5].Value = PositionID;
                                this.DataGridView_List.Rows[index].Cells[6].Value = SupplierID;
                                this.DataGridView_List.Rows[index].Cells[7].Value = SupplierID;
                                this.DataGridView_List.Rows[index].Cells[8].Value = 送货单号T.Text;
                                this.DataGridView_List.Rows[index].Cells[9].Value = MaterielID + SupplierID;
                                条码T.SelectAll();
                                条码T.Select();
                            }
                            else
                            {
                                MsgFlag = true;
                                MessageBox.Show("此条码可入库数量为" + k.ToString() + "少于输入的数量。\n\r请重新输入", "错误");
                                数量T.Text = "";
                                数量T.SelectAll();
                                数量T.Select();
                            }
                        }
                        catch
                        {
                            MsgFlag = true;
                            MessageBox.Show("查询未进货量返回错误", "错误");
                            条码T.SelectAll();
                            条码T.Select();
                        }
                    }
                    catch
                    {
                        MsgFlag = true;
                        MessageBox.Show("数量输入错误", "错误");
                        数量T.Select();
                        数量T.SelectAll();
                    }
                }
                else
                {
                    if (GetMaterielExist(MaterielID))
                    {
                        MsgFlag = true;
                        MessageBox.Show("品号：" + MaterielID + " 可入库数量为零，请重新输入条码", "错误");
                        条码T.SelectAll();
                        条码T.Select();
                    }
                    else
                    {
                        MsgFlag = true;
                        MessageBox.Show("品号：" + MaterielID + " 存在错误，请重新输入条码", "错误");
                        条码T.SelectAll();
                        条码T.Select();
                    }
                }
                MaterielID = null;
            }
            else
            {
                if (条码T.Text != "")
                {
                    if (!CheckSupplierID(条码T.Text.Substring(条码T.Text.Length - 5, 5)))
                    {
                        MsgFlag = true;
                        MessageBox.Show("条码中的供应商编码与选择的不对应！请检测条码。", "错误");
                        条码T.Select();
                        条码T.SelectAll();
                    }
                    else
                    {
                        MaterielID = 条码T.Text.Substring(0, 条码T.Text.Length - 5);
                        数量T.Select();
                        AddData();
                    }
                }
                else
                {
                    MsgFlag = true;
                    MessageBox.Show("没有获取到品号信息", "错误");
                    条码T.Select();
                    条码T.SelectAll();
                }
            }
            SetEnable();
        }

        private void Insert()
        {
            string sql = "INSERT INTO TH_LYXA "
                        + "(COMPANY, CREATOR, USR_GROUP, CREATE_DATE, FLAG, THXA001, THXA002, THXA003, "
                        + "THXA004, THXA005, THXA007, THXA008, THXA009, "
                        + "THXA011, THXA012, THXA013, THXA014, THXA015, ID) "
                        + "VALUES('COMFORT', '{0}', '{1}', '{2}', 1, '{3}', '{4}', '{5}', "
                        + "'{6}', '{7}', '{8}', '{9}', '{10}', 'N', 'N', "
                        + "'{11}', '********************', '{12}', '{13}')";
            string flowId = GetFlowID();
            string Time = GetTime();
            int Count = DataGridView_List.RowCount;

            for (int Index = 0; Index < Count; Index++)
            {
                string THXA001 = TypeID;
                string THXA002 = DataGridView_List.Rows[0].Cells[7].Value.ToString();
                string THXA003 = DataGridView_List.Rows[0].Cells[5].Value.ToString();
                string THXA004 = GetDate();
                string THXA007 = DataGridView_List.Rows[0].Cells[1].Value.ToString();
                string THXA008 = DataGridView_List.Rows[0].Cells[9].Value.ToString();
                string THXA009 = DataGridView_List.Rows[0].Cells[4].Value.ToString();
                string THXA013 = DataGridView_List.Rows[0].Cells[8].Value.ToString();
                string THXA015 = DataGridView_List.Rows[0].Cells[7].Value.ToString();
                string ID = (Index+1).ToString();
                string slqStr = string.Format(sql, LoginUid, LoginUserGroup, Time, THXA001, THXA002, THXA003, 
                    THXA004, flowId, THXA007, THXA008, THXA009, THXA013, THXA015, ID);

                mssql.SQLexcute(connYF, slqStr);

                DataGridView_List.Rows.Remove(DataGridView_List.Rows[0]);
            }
            string tell = "\n\r处理流水号为：" + flowId;
            string getBackStr = createPurti.HandelDef(flowId);
            if(getBackStr != null)
            {
                tell += "\n退货单号为：" + getBackStr;
            }
            else
            {
                tell += "\n\r处理失败，请把记下流水处理号并联系咨询部。";
            }
            if (MessageBox.Show(tell, "提示", MessageBoxButtons.OK) == DialogResult.OK)
            {
                MsgFlag = true;
                送货单号T.SelectAll();
                送货单号T.Select();
                panel_Last.Enabled = false;
            }
        }

        private void SetEnable()
        {
            if (DataGridView_List.RowCount > 0)
            {
                panel_Last.Enabled = true;
                入库单别L.Enabled = false;
                入库仓库L.Enabled = false;
                供应商L.Enabled = false;
                dateTimePicker1.Enabled = false;
                送货单号T.Enabled = false;
            }
            if (DataGridView_List.RowCount <= 0)
            {
                panel_Last.Enabled = false;
                入库单别L.Enabled = true;
                入库仓库L.Enabled = true;
                供应商L.Enabled = true;
                dateTimePicker1.Enabled = true;
                送货单号T.Enabled = true;
            }
        }

        private bool CheckSupplierID(string Supplier)
        {
            if(SupplierID == Supplier)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
