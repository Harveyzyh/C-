using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HarveyZ;

namespace 联友采购平台.供应商
{
    public partial class 录入供应商送货单 : Form
    {
        private Mssql mssql = new Mssql();
        private string conn = FormLogin.infObj.connWG;

        private bool ComboxIndexChangeNotHandelFlag = false;

        private DataTable dtDetailBak = null;

        public 录入供应商送货单()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            DgvOpt.SetRowBackColor(DgvMain);
            DgvMain.DataSource = null;

            TextBoxBarCode.Text = "";
            TextBoxFliter.Text = "";

            LabelScaned.Visible = false;

            ComboBoxVersionList.Items.Clear();
            ComboBoxVersionList.Items.Add("");
            foreach (string tmp in GetVersionList())
            {
                ComboBoxVersionList.Items.Add(tmp);
            }
            ComboBoxVersionList.Items.Add("新版本");
            ComboBoxVersionList.SelectedIndex = 0;

            UI();
        }

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
            PanelTitle.Location = new Point(1, 1);
            PanelTitle.Size = new Size(FormWidth - 2, PanelTitle.Height);

            DgvMain.Location = new Point(PanelTitle.Location.X, PanelTitle.Location.Y + PanelTitle.Height + 2);
            DgvMain.Size = new Size(PanelTitle.Width, FormHeight - PanelTitle.Height - 3);
        }
        #endregion

        #region 界面逻辑
        private void UI()
        {
            if (ComboBoxVersionList.SelectedItem.ToString() != "")
            {
                BtnGet.Enabled = true;
                BtnSave.Enabled = true;
            }
            else
            {
                BtnGet.Enabled = false;
                BtnSave.Enabled = false;
            }

            if (TextBoxBarCode.Text != "")
            {
                BtnPrint.Enabled = true;
            }
            else
            {
                BtnPrint.Enabled = false;
            }

            if (DgvMain.DataSource != null)
            {
                DgvOpt.SetColHeadMiddleCenter(DgvMain);
                DgvOpt.SetColWritable(DgvMain, "送货量");
                List<string> dgvMainColCenterList = new List<string>();
                dgvMainColCenterList.Add("序号");
                dgvMainColCenterList.Add("单位");
                dgvMainColCenterList.Add("当天可送量");
                dgvMainColCenterList.Add("送货量");
                DgvOpt.SetColMiddleCenter(DgvMain, dgvMainColCenterList);

                Dictionary<string, int> dgvMainColWidthDict = new Dictionary<string, int>();
                dgvMainColWidthDict.Add("序号", 50);
                dgvMainColWidthDict.Add("品名", 200);
                dgvMainColWidthDict.Add("规格", 300);
                dgvMainColWidthDict.Add("单位", 50);
                dgvMainColWidthDict.Add("当天可送量", 80);
                dgvMainColWidthDict.Add("送货量", 80);
                DgvOpt.SetColWidth(DgvMain, dgvMainColWidthDict);

                DataTable dtDetail = (DataTable)DgvMain.DataSource;
                if (dtDetail.Rows.Count > 0)
                {
                    BtnSave.Enabled = true;
                    TextBoxFliter.Enabled = true;
                }
                else
                {
                    BtnSave.Enabled = false;
                    TextBoxFliter.Enabled = true;
                    TextBoxFliter.Text = "";
                }

                if (LabelScaned.Visible == true)
                {
                    BtnSave.Enabled = false;
                }
            }
            else
            {
                BtnSave.Enabled = false;
            }
        }

        private void BtnGet_Click(object sender, EventArgs e)
        {
            ShowDtDetail(FormLogin.infObj.userDpt, DtpDate.Value.ToString("yyyyMMdd"), ComboBoxVersionList.SelectedItem.ToString());
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            BtnSaveWork();
            ComboBoxVersionList_SelectedIndexChanged_Work();
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            BtnPrintWork();
        }

        private void DtpDate_ValueChanged(object sender, EventArgs e)
        {
            Init();
        }

        private void ComboBoxVersionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxVersionList_SelectedIndexChanged_Work();
        }
        
        private void TextBoxFliter_TextChanged(object sender, EventArgs e)
        {
            TextBoxFliter_TextChanged_Work();
        }

        private void DgvMain_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DgvMain_CellValueChanged_Work();
        }
        #endregion

        #region 业务逻辑
        private void ComboBoxVersionList_SelectedIndexChanged_Work()
        {
            if (!ComboxIndexChangeNotHandelFlag)
            {
                LabelScaned.Visible = false;
                TextBoxBarCode.Text = "";
                TextBoxFliter.Text = "";
                DgvMain.DataSource = null;
                if (ComboBoxVersionList.SelectedItem.ToString() != "")
                {
                    DataTable dt = GetDtHead(FormLogin.infObj.userDpt, DtpDate.Value.ToString("yyyyMMdd"), ComboBoxVersionList.SelectedItem.ToString());
                    if (dt != null)
                    {
                        TextBoxBarCode.Text = dt.Rows[0]["条码编号"].ToString();
                        if (dt.Rows[0]["已扫描"].ToString() == "True")
                        {
                            LabelScaned.Visible = true;
                        }
                        else
                        {
                            LabelScaned.Visible = false;
                        }
                    }
                    ShowDtDetail(FormLogin.infObj.userDpt, DtpDate.Value.ToString("yyyyMMdd"), ComboBoxVersionList.SelectedItem.ToString());
                }
                UI();
            }
        }

        //筛选框内容修改，实现筛选功能
        private void TextBoxFliter_TextChanged_Work()
        {
            if (dtDetailBak == null)
            {
                DataTable dt = (DataTable)DgvMain.DataSource;
                dtDetailBak = dt.Copy();
            }
            DataView dv = dtDetailBak.DefaultView;
            string fliter = "品号 like '%{0}%' or 品名 like '%{0}%' or 规格 like '%{0}%' ";
            dv.RowFilter = string.Format(fliter, TextBoxFliter.Text);
            DgvMain.DataSource = dv.ToTable();
        }

        private void DgvMain_CellValueChanged_Work()
        {
            if (dtDetailBak != null)
            {
                int rowIndex = 0;
                rowIndex = DgvMain.CurrentRow.Index;
                string xh = DgvMain.Rows[rowIndex].Cells["序号"].Value.ToString();
                string shl = DgvMain.Rows[rowIndex].Cells["送货量"].Value.ToString();
                for (int index = 0; index < dtDetailBak.Rows.Count; index++)
                {
                    if (dtDetailBak.Rows[index]["序号"].ToString() == xh)
                    {
                        dtDetailBak.Rows[index]["送货量"] = shl;
                        break;
                    }
                }
            }
        }

        private void BtnPrintWork()
        {
            if (LabelScaned.Visible == false)
            {
                //打印前先再次执行保存
                BtnSaveWork();
            }
            //先重新保存数据，再获取数据，后调用打印
            DataSet ds = GetPrintDataSet();
            打印供应商送货单 frm = new 打印供应商送货单(GetPrintFrx(), ds);
            if (frm.ShowDialog() == DialogResult.Cancel)
            {
                frm.Dispose();
            }
        }

        private string GetPrintFrx()
        {
            string sqlstr = @"SELECT CONTENT FROM WG_DB.dbo.WG_PRINT WHERE PRINT_TYPE = '联友采购平台' AND PRINT_NAME = '送货单' ";
            DataTable dt = mssql.SQLselect(conn, sqlstr);
            if (dt != null)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return null;
            }
        }

        private DataSet GetPrintDataSet()
        {
            //构建需要打印的数据
            DataSet ds = new DataSet();
            DataTable dtHead = GetDtHead(FormLogin.infObj.userDpt, DtpDate.Value.ToString("yyyyMMdd"), ComboBoxVersionList.SelectedItem.ToString());
            DataTable dtDetailSrc = GetDtDetail(FormLogin.infObj.userDpt, DtpDate.Value.ToString("yyyyMMdd"), ComboBoxVersionList.SelectedItem.ToString());

            dtDetailSrc.Columns.Remove("当天可送量");

            DataTable dtDetail = dtDetailSrc.Clone();

            //不要数量为0的
            for(int rowIndex = 0; rowIndex < dtDetailSrc.Rows.Count; rowIndex++)
            {
                if (dtDetailSrc.Rows[rowIndex]["送货量"].ToString() != "0")
                {
                    dtDetail.ImportRow(dtDetailSrc.Rows[rowIndex]);
                }
            }

            dtHead.TableName = "单头";
            ds.Tables.Add(dtHead);
            dtDetail.TableName = "单身";
            ds.Tables.Add(dtDetail);

            return ds;
        }

        private void BtnSaveWork()
        {
            //创建新的版本号，并选择新的版本号
            if (ComboBoxVersionList.SelectedItem.ToString() == "新版本")
            {
                ComboxIndexChangeNotHandelFlag = true;
                string newVersion = GetNewVersion();
                ComboBoxVersionList.Items.Remove("新版本");
                ComboBoxVersionList.Items.Add(newVersion);
                ComboBoxVersionList.Items.Add("新版本");
                ComboBoxVersionList.SelectedItem = newVersion;
                ComboxIndexChangeNotHandelFlag = false;
            }

            //删除当前的信息，然后重建信息
            string sqlDelH = @"DELETE FROM CG_SupplyHead where SupId = '{0}' and SendDate = '{1}' and SendVersion = '{2}' ";
            string sqlDelD = @"DELETE FROM CG_SupplyDetail where SupId = '{0}' and SendDate = '{1}' and SendVersion = '{2}' ";
            string sqlInsH = @"INSERT INTO CG_SupplyHead(CreateDate, SupId, SendDate, SendVersion) values(getdate(), '{0}', '{1}', '{2}' )";
            string sqlInsD = @"INSERT INTO CG_SupplyDetail(SupId, SendDate, SendVersion, xh, wlno, sl) values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')";

            mssql.SQLselect(conn, string.Format(sqlDelH, FormLogin.infObj.userDpt, DtpDate.Value.ToString("yyyyMMdd"), ComboBoxVersionList.SelectedItem.ToString()));
            mssql.SQLselect(conn, string.Format(sqlDelD, FormLogin.infObj.userDpt, DtpDate.Value.ToString("yyyyMMdd"), ComboBoxVersionList.SelectedItem.ToString()));
            mssql.SQLselect(conn, string.Format(sqlInsH, FormLogin.infObj.userDpt, DtpDate.Value.ToString("yyyyMMdd"), ComboBoxVersionList.SelectedItem.ToString()));

            //不要数量为0的
            DataTable dtDetail = null;
            if (dtDetailBak != null)
            {
                dtDetail = dtDetailBak;
            }
            else
            {
                dtDetail = (DataTable)DgvMain.DataSource;
            }
            if (dtDetail != null)
            {
                for (int rowIndex = 0; rowIndex < dtDetail.Rows.Count; rowIndex++)
                {
                    if (dtDetail.Rows[rowIndex]["送货量"].ToString() != "0")
                    {
                        mssql.SQLselect(conn, string.Format(sqlInsD, FormLogin.infObj.userDpt, DtpDate.Value.ToString("yyyyMMdd"), ComboBoxVersionList.SelectedItem.ToString(),
                            dtDetail.Rows[rowIndex]["序号"].ToString(), dtDetail.Rows[rowIndex]["品号"].ToString(), dtDetail.Rows[rowIndex]["送货量"].ToString()));
                    }
                }
            }
            MessageBox.Show("已保存", "提醒", MessageBoxButtons.OK);
        }

        private List<string> GetVersionList()
        {
            List<string> rtnList = new List<string>();
            string sqlstr = @"SELECT SendVersion from CG_SupplyHead where SupId = '{0}' and SendDate = '{1}' order by SendVersion ";
            DataTable dt = mssql.SQLselect(conn, string.Format(sqlstr, FormLogin.infObj.userDpt, DtpDate.Value.ToString("yyyyMMdd")));
            if (dt != null)
            {
                for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
                {
                    rtnList.Add(dt.Rows[rowIndex][0].ToString());
                }
            }
            return rtnList;
        }

        private DataTable GetDtHead(string supId, string sendDate, string sendVer)
        {
            string sqlstr = @"SELECT SupId 供应商编号, RTRIM(MA003) 供应商名称, substring(SendDate, 1, 4)+'-'+substring(SendDate, 5, 2)+'-'+substring(SendDate, 7, 2) 送货日期, 
                                ScanFlag 已扫描, SupId+'-'+SendDate+'-'+SendVersion 条码编号, '' 打印时间 from CG_SupplyHead 
                                left join COMFORT.dbo.PURMA ON MA001 = SupId  
                                where SupId = '{0}' and SendDate = '{1}' and SendVersion = '{2}' ";
            return mssql.SQLselect(conn, string.Format(sqlstr, supId, sendDate, sendVer));
        }

        private DataTable GetDtDetail(string supId, string sendDate, string sendVer)
        {
            string sqlstr = @"exec COMFORT.dbo.P_CG_SupplySl @SupId = '{0}', @SendDate = '{1}', @SendVersion = '{2}' ";
            return mssql.SQLselect(conn, string.Format(sqlstr, supId, sendDate, sendVer));
        }

        private void ShowDtDetail(string supId, string sendDate, string sendVer)
        {
            DgvMain.DataSource = GetDtDetail(supId, sendDate, sendVer);
        }

        private string GetNewVersion()
        {
            string sqlstr = @"select isnull(right('000' + cast(max(cast(SendVersion as int)) + 1 as varchar(3)), 3), '001') from CG_SupplyHead where SupId = '{0}' and SendDate = '{1}'";
            return mssql.SQLselect(conn, string.Format(sqlstr, FormLogin.infObj.userDpt, DtpDate.Value.ToString("yyyyMMdd"))).Rows[0][0].ToString();
        }

        #endregion
    }
}
