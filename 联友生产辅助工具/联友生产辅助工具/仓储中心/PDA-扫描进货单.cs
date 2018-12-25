using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HarveyZ;

namespace 联友生产辅助工具.仓储中心
{
    public partial class PDA_扫描进货单 : Form
    {
        #region 公用变量设定
        public static string strConnection = Global_Const.strConnection_COMFORT_TEST;
        #endregion

        #region 局部变量设定
        Mssql mssql = new Mssql();
        WebNet webNet = new WebNet();

        Dictionary<string, string> dict = new Dictionary<string, string> { };
        Dictionary<string, string> dict_get = new Dictionary<string, string> { };
        public static string Mode = null;
        public static string URL = FormLogin.HttpURL + "/Client/PDA/JH_LYXA";
        public static string Title = null;
        public static string GetMain = null;
        public static string GetOther = null;
        public static string MaterielMode = null;

        private string SupplierID = null;
        private string TypeID = "3401";
        private string PositionID = "P013";
        private string MaterielID = null;
        private string BarCode = null;
        private string LoginUid = FormLogin.Login_Uid;
        private string LoginUserGroup = null;
        #endregion

        #region 窗口初始化
        public PDA_扫描进货单()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            入库单别.Text = "3401-采购入库单";
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
            panel_Title.Size = new Size(FormWidth, panel_Title.Height);
            DataGridView_List.Location = new Point(0, panel_Title.Height + 3);
            DataGridView_List.Size = new Size(FormWidth, FormHeight - panel_Title.Height - panel_Last.Height);
            panel_Last.Location = new Point(0, panel_Title.Height + DataGridView_List.Height);
            panel_Last.Size = new Size(FormWidth, panel_Last.Height);
        }
        #endregion

        #region 业务逻辑
        private string GetDate()
        {
            return dateTimePicker1.Value.ToString("yyyyMMdd");
        }

        private string GetFlowID()
        {
            Dictionary<string, string> dict = new Dictionary<string, string> { };
            dict.Add("Mode", "Select");
            dict.Add("Parameter", "FlowID");
            dict.Add("Data", null);
            dict = webNet.WebPost(URL, dict);
            string FlowId = "";
            dict.TryGetValue("Data", out FlowId);
            return FlowId;
        }

        private string GetTime()
        {
            string sqlstr = "SELECT (CONVERT(VARCHAR(20), GETDATE(), 112) + REPLACE(CONVERT(VARCHAR(20), GETDATE(), 24), ':', ''))";
            DataTable dt = mssql.SQLselect(strConnection, sqlstr);
            if(dt != null)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return null;
            }
        }

        private void GetUserGroup()
        {
            string sql = "SELECT MF004 FROM ADMMF WHERE MF001 = '{0}' ";
            DataTable dt = mssql.SQLselect(strConnection, string.Format(sql, LoginUid));
            if(dt != null)
            {
                LoginUserGroup = dt.Rows[0][0].ToString();
            }
            else
            {
                LoginUserGroup = "";
            }
        }

        private DataTable GetMaterielInfo(string MaterielID, string SupplierID)
        {
            string sqlstr = "SELECT RTRIM(TD004), RTRIM(MB002), RTRIM(MB003), RTRIM(TC004), PCWNum FROM VPURTDPCB "
                            + "INNER JOIN INVMB ON MB001 = TD004 "
                            + "WHERE TD004 = '{0}' AND TC004 = '{1}' ";
            DataTable dt = mssql.SQLselect(strConnection, string.Format(sqlstr, MaterielID, SupplierID));
            if(dt != null)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }

        private void AddData()
        {
            if (MaterielID != null)
            {
                DataTable dt = GetMaterielInfo(MaterielID, SupplierID);
                if(dt != null)
                {
                    try
                    {
                        float.Parse(数量T.Text);
                        float k = float.Parse(dt.Rows[0][4].ToString());
                        if (float.Parse(dt.Rows[0][4].ToString()) > 0)
                        {
                            int index = this.DataGridView_List.Rows.Add();
                            this.DataGridView_List.Rows[index].Cells[0].Value = MaterielID;
                            this.DataGridView_List.Rows[index].Cells[1].Value = dt.Rows[0][1];
                            this.DataGridView_List.Rows[index].Cells[2].Value = dt.Rows[0][2];
                            this.DataGridView_List.Rows[index].Cells[3].Value = 数量T.Text;
                            this.DataGridView_List.Rows[index].Cells[4].Value = PositionID;
                            this.DataGridView_List.Rows[index].Cells[5].Value = SupplierID;
                            this.DataGridView_List.Rows[index].Cells[6].Value = SupplierID;
                            this.DataGridView_List.Rows[index].Cells[7].Value = 送货单号T.Text;
                            this.DataGridView_List.Rows[index].Cells[8].Value = MaterielID + SupplierID;
                        }
                        else
                        {
                            MessageBox.Show("此条码没有未入库量", "错误");
                            条码T.Text = "";
                        }
                    }
                    catch
                    {
                        MessageBox.Show("数量输入错误", "错误");
                        数量T.Text = "";
                    }
                }
                else
                {
                    MessageBox.Show("品号：" + MaterielID + ",供应商编号：" + SupplierID + "存在错误", "错误");
                }
                MaterielID = null;
            }
            else
            {
                MessageBox.Show("没有获取到品号信息", "错误");
            }
            SetEnable();
        }

        private void Insert()
        {
            string sql = "INSERT INTO JH_LYXA "
                        + "(COMPANY, CREATOR, USR_GROUP, CREATE_DATE, JHXA001, JHXA002, JHXA003, "
                        + "JHXA004, JHXA005, JHXA007, JHXA008, JHXA009, "
                        + "JHXA011, JHXA012, JHXA013, JHXA014, JHXA015, ID) "
                        + "VALUES('COMFORT', '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', "
                        + "'{6}', '{7}', '{8}', '{9}', '{10}', 'N', 'N', "
                        + "'{11}', '********************', '{12}', '{13}')";
            string FlowId = GetFlowID();
            string Time = GetTime();
            GetUserGroup();
            int Count = DataGridView_List.RowCount;

            for (int Index = 0; Index < Count; Index++)
            {
                string JHXA001 = TypeID;
                string JHXA002 = DataGridView_List.Rows[0].Cells[6].Value.ToString();
                string JHXA003 = DataGridView_List.Rows[0].Cells[4].Value.ToString();
                string JHXA004 = GetDate();
                string JHXA007 = DataGridView_List.Rows[0].Cells[0].Value.ToString();
                string JHXA008 = DataGridView_List.Rows[0].Cells[8].Value.ToString();
                string JHXA009 = DataGridView_List.Rows[0].Cells[3].Value.ToString();
                string JHXA013 = DataGridView_List.Rows[0].Cells[7].Value.ToString();
                string JHXA015 = DataGridView_List.Rows[0].Cells[6].Value.ToString();
                string ID = (Index+1).ToString();
                string sqlstr = string.Format(sql, LoginUid, LoginUserGroup, Time, JHXA001, JHXA002, JHXA003, 
                    JHXA004, FlowId, JHXA007, JHXA008, JHXA009, JHXA013, JHXA015, ID);

                mssql.SQLexcute(strConnection, sqlstr);

                DataGridView_List.Rows.Remove(DataGridView_List.Rows[0]);
            }
            string tell = "上传成功！\n\r处理流水号为：" + FlowId;
            string GetBack = Upload(FlowId, Count.ToString());
            if(GetBack != null)
            {
                tell += "\n\r进货单号为：" + GetBack;
            }
            if (MessageBox.Show(tell, "提示", MessageBoxButtons.OK) == DialogResult.OK)
            {
                条码T.SelectAll();
                条码T.Select();
                panel_Last.Enabled = false;
            }
        }

        private string Upload(string FlowId, string Count)
        {
            Dictionary<string, string> dict = new Dictionary<string, string> { };
            dict.Add("Uid", LoginUid);
            dict.Add("Mode", "Complete");
            dict.Add("Parameter", FlowId);
            dict.Add("Data", "");
            dict.Add("RowCount", Count);
            dict = webNet.WebPost(URL, dict);
            string GetBack = null;
            if (dict != null)
            {
                dict.TryGetValue("Data", out GetBack);
            }
            return GetBack;
        }

        private void SetEnable()
        {
            if (DataGridView_List.RowCount > 0)
            {
                panel_Last.Enabled = true;
                入库单别查.Enabled = false;
                入库仓库查.Enabled = false;
                供应商查.Enabled = false;
                dateTimePicker1.Enabled = false;
                送货单号T.Enabled = false;
            }
            if (DataGridView_List.RowCount <= 0)
            {
                panel_Last.Enabled = false;
                入库单别查.Enabled = true;
                入库仓库查.Enabled = true;
                供应商查.Enabled = true;
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

        #region UI按钮

        private void 送货单别查_Click(object sender, EventArgs e)
        {
            Mode = "TypeID";
            Title = "单别";
            //Title = "单别|单据简称";
            PDA_扫描进货单_获取单据信息 frm = new PDA_扫描进货单_获取单据信息();
            if(frm.ShowDialog() == DialogResult.Cancel)
            {
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

        private void 供应商查_Click(object sender, EventArgs e)
        {
            Mode = "SupplierID";
            Title = "供应商编号";
            //Title = "供应商编号|简称";
            PDA_扫描进货单_获取单据信息 frm = new PDA_扫描进货单_获取单据信息();
            if (frm.ShowDialog() == DialogResult.Cancel)
            {
                if (GetMain != null)
                {
                    供应商.Text = GetMain + "-" + GetOther;
                    SupplierID = GetMain;
                    GetMain = null;
                    GetOther = null;
                }
                frm.Dispose();
            }
        }

        private void 入库仓库查_Click(object sender, EventArgs e)
        {
            Mode = "PositionID";
            Title = "仓库编号";
            //Title = "仓库编号|仓库名称";
            PDA_扫描进货单_获取单据信息 frm = new PDA_扫描进货单_获取单据信息();
            if (frm.ShowDialog() == DialogResult.Cancel)
            {
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

        private void 送货单号_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                条码T.Select();
            }
        }

        private void 条码_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                条码T.Text = 条码T.Text.ToUpper();
                if (入库单别.Text == "" || 供应商.Text == "" || 入库仓库.Text == "" || 送货单号T.Text == "")
                {
                    MessageBox.Show("请先查询或填写 进货单别，供应商，入库仓库，送货单号 的信息！", "错误");
                    条码T.Select();
                    条码T.SelectAll();
                }
                else if(条码T.Text.Length > 9)
                {
                    if(!CheckSupplierID(条码T.Text.Substring(条码T.Text.Length-5, 5)))
                    {
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
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    float.Parse(数量T.Text);
                }
                catch
                {
                    MessageBox.Show("数量输入错误！", "错误");
                    数量T.SelectAll();
                }
                AddData();
                条码T.SelectAll();
                条码T.Select();
            }
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
        }

        private void buttonUpload_Click(object sender, EventArgs e)
        {
            Insert();
            SetEnable();
        }
        #endregion
    }
}
