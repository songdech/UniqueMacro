using System;
using System.Data;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{
    public partial class frmBreakpoint : Form
    {

        private string moduleType;
        private SystemController objSystemControl;
        private BreakPointM objBreakPointM;

        private ConfigurationController objConfig;

        int rowindex = 0;
        object BREAKP01;
        object BREAKP02;
        object BREAKP03;

        public Action refreshData { get; set; }

        public frmBreakpoint(string moduleType,string CustomizedID)
        {
            InitializeComponent();
            this.moduleType = moduleType;
            objConfig = new ConfigurationController();
            objBreakPointM = new BreakPointM();

            if (moduleType != "add")
            {
                objBreakPointM = ControlParameter.BreakPointM;
            }
        }

        private void frmBreakpoint_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            //DataTable dt;
            try
            {
                if (moduleType == "edit")
                {
                    txtCode.Text = objBreakPointM.CUSTOMIZED_BREAKPOINT_CODE;
                    txtName.Text = objBreakPointM.CUSTOMIZED_BREAKPOINT_TEXT;
                    txtID.Text = objBreakPointM.CUSTOMIZED_BREAKPOINT_ID;
                    txtComment.Text = objBreakPointM.BREAKPOINT_DESCRIPTION;

                    objBreakPointM.LOGUSERID = ControlParameter.loginID.ToString();
                    Get_BreakPoints_Definition(objBreakPointM);
                    Get_BreakPoints_With_Organisms(objBreakPointM);

                    label6.Visible = true;
                    label6.Text = "Edit BreakPoint";
                    this.Text = "Edit BreakPoint";
                }
                else if (moduleType == "dup")
                {
                    txtCode.Select();
                    txtName.Text = objBreakPointM.CUSTOMIZED_BREAKPOINT_TEXT;
                    objBreakPointM.LOGUSERID = ControlParameter.loginID.ToString();
                    Get_BreakPoints_Definition(objBreakPointM);
                    Get_BreakPoints_With_Organisms(objBreakPointM);

                    label6.Visible = true;
                    label6.Text = "Duplicate BreakPoint";
                    this.Text = "Duplicate BreakPoint";
                }
                else if (moduleType == "add")
                {
                    txtName.Select();
                    label6.Visible = true;
                    label6.Text = "Add BreakPoint";
                    this.Text = "Add BreakPoint";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void Get_BreakPoints_Definition(BreakPointM objBreakPointM)
        {
            ConfigurationController objGridLookupBreakPoint = new ConfigurationController();
            DataTable dt = null;
            try
            {
                dt = objGridLookupBreakPoint.GetGridBreakPoint(objBreakPointM);
                // Manual fill dt to datagrid
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dataGridView1.Rows.Add();

                        dataGridView1.Rows[i].Cells[0].Value = dt.Rows[i]["ANTIBIOTICCODE"].ToString();
                        dataGridView1.Rows[i].Cells[1].Value = dt.Rows[i]["FULLTEXT"].ToString();
                        dataGridView1.Rows[i].Cells[2].Value = dt.Rows[i]["MIDS"].ToString();
                        dataGridView1.Rows[i].Cells[3].Value = dt.Rows[i]["MIDSDD"].ToString();
                        dataGridView1.Rows[i].Cells[4].Value = dt.Rows[i]["MIDI"].ToString();
                        dataGridView1.Rows[i].Cells[5].Value = dt.Rows[i]["MIDR"].ToString();
                        dataGridView1.Rows[i].Cells[6].Value = dt.Rows[i]["MICS"].ToString();
                        dataGridView1.Rows[i].Cells[7].Value = dt.Rows[i]["MICSDD"].ToString();
                        dataGridView1.Rows[i].Cells[8].Value = dt.Rows[i]["MICI"].ToString();
                        dataGridView1.Rows[i].Cells[9].Value = dt.Rows[i]["MICR"].ToString();
                        dataGridView1.Rows[i].Cells[10].Value = dt.Rows[i]["ANTIBIOTICID"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            //finally
            //{
            //    objcustomM = null;
            //}
        }

        private void Get_BreakPoints_With_Organisms(BreakPointM objBreakPointM)
        {
            ConfigurationController objGridLookupBreakPoint = new ConfigurationController();
            DataTable dt = null;
            try
            {
                dt = objGridLookupBreakPoint.GetGridBreakPoint_with_Organisms(objBreakPointM);
                // Manual fill dt to datagrid
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dataGridView2.Rows.Add();

                        dataGridView2.Rows[i].Cells[0].Value = dt.Rows[i]["CODE"].ToString();
                        dataGridView2.Rows[i].Cells[1].Value = dt.Rows[i]["TEXT"].ToString();
                        dataGridView2.Rows[i].Cells[2].Value = dt.Rows[i]["ID"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        bool IsNumbericOnly(string strProtocol)
        {
            foreach (char A in strProtocol)
            {
                if (A < '0' || A > '9')
                    return false;
            }
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!this.dataGridView1.Rows[this.rowindex].IsNewRow)
            {
                try
                {
                    this.dataGridView1.Rows.RemoveAt(this.rowindex);

                }
                catch (Exception ex) { }
            }
        }

        private void btnExit_ElementClick_1(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        this.dataGridView1.Rows[e.RowIndex].Selected = true;
                        rowindex = e.RowIndex;
                        this.dataGridView1.CurrentCell = this.dataGridView1.Rows[e.RowIndex].Cells[0];
                        this.contextMenuStrip1.Show(this.dataGridView1, e.Location);
                        contextMenuStrip1.Show(Cursor.Position);
                    }
                }
            }
        }

        private void btnSave_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            try
            {
                if (moduleType == "edit")
                {
                    try
                    {
                        if (txtCode.Text != "" && txtName.Text != "")
                        {
                            try
                            {
                                txtCode.Enabled = false;

                                objBreakPointM.CUSTOMIZED_BREAKPOINT_ID = txtID.Text;
                                objBreakPointM.CUSTOMIZED_BREAKPOINT_CODE = txtCode.Text;
                                objBreakPointM.CUSTOMIZED_BREAKPOINT_TEXT = txtName.Text;
                                objBreakPointM.LOGUSERID = ControlParameter.loginID.ToString();

                                objBreakPointM = objConfig.SaveBreakPoint(objBreakPointM);

                                if (dataGridView1.Rows.Count > 0)
                                {
                                    objBreakPointM = objConfig.DeleteBreakPoint_List_Dict(objBreakPointM);
                                    foreach (DataGridViewRow row in dataGridView1.Rows)
                                    {
                                        objBreakPointM.BREAKPOINT_CODE = row.Cells[0].Value.ToString();
                                        objBreakPointM.BREAKPOINT_TEXT = row.Cells[1].Value.ToString();
                                        objBreakPointM.BREAKPOINT_ID = row.Cells[10].Value.ToString();


                                        // S I R
                                        if (row.Cells[2].Value == null || row.Cells[2].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells[2].Value.ToString()) )
                                        {
                                            objBreakPointM.BREAKPOINT_MIDS = "";
                                        }
                                        else
                                        {
                                            objBreakPointM.BREAKPOINT_MIDS = row.Cells[2].Value.ToString();         //MIDS
                                            
                                        }
                                        if (row.Cells[3].Value == null || row.Cells[3].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells[3].Value.ToString()))
                                        {
                                            objBreakPointM.BREAKPOINT_MIDSDD = "";
                                        }
                                        else
                                        {
                                            objBreakPointM.BREAKPOINT_MIDSDD = row.Cells[3].Value.ToString();         //MIDSDD

                                        }

                                        if (row.Cells[4].Value == null || row.Cells[4].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells[4].Value.ToString()))
                                        {
                                            objBreakPointM.BREAKPOINT_MIDI = "";
                                        }
                                        else
                                        {
                                            objBreakPointM.BREAKPOINT_MIDI = row.Cells[4].Value.ToString();         //MIDI

                                        }
                                        if (row.Cells[5].Value == null || row.Cells[5].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells[5].Value.ToString()))
                                        {
                                            objBreakPointM.BREAKPOINT_MIDR = "";
                                        }
                                        else
                                        {
                                            objBreakPointM.BREAKPOINT_MIDR = row.Cells[5].Value.ToString();         //MIDR

                                        }
                                        if (row.Cells[6].Value == null || row.Cells[6].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells[6].Value.ToString()))
                                        {
                                            objBreakPointM.BREAKPOINT_MICS = "";
                                        }
                                        else
                                        {
                                            objBreakPointM.BREAKPOINT_MICS = row.Cells[6].Value.ToString();         //MICS

                                        }
                                        if (row.Cells[7].Value == null || row.Cells[7].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells[7].Value.ToString()))
                                        {
                                            objBreakPointM.BREAKPOINT_MICSDD = "";
                                        }
                                        else
                                        {
                                            objBreakPointM.BREAKPOINT_MICSDD = row.Cells[7].Value.ToString();         //MICSDD

                                        }

                                        if (row.Cells[8].Value == null || row.Cells[8].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells[8].Value.ToString()))
                                        {
                                            objBreakPointM.BREAKPOINT_MICI = "";
                                        }
                                        else
                                        {
                                            objBreakPointM.BREAKPOINT_MICI = row.Cells[8].Value.ToString();         //MICI

                                        }
                                        if (row.Cells[9].Value == null || row.Cells[9].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells[9].Value.ToString()))
                                        {
                                            objBreakPointM.BREAKPOINT_MICR = "";
                                        }
                                        else
                                        {
                                            objBreakPointM.BREAKPOINT_MICR = row.Cells[9].Value.ToString();         //MICR

                                        }
                                        objBreakPointM = objConfig.SaveBreakPoint_In_DICT_Breakpoint_LIST(objBreakPointM);
                                    }
                                }
                                else
                                {
                                    objBreakPointM = objConfig.DeleteBreakPoint_List_Dict(objBreakPointM);
                                }

                                // SAVE ORGANISMS
                                if (dataGridView2.Rows.Count > 0)
                                {
                                    objBreakPointM = objConfig.ClearBreakPoint_with_Organisms(objBreakPointM);

                                    foreach (DataGridViewRow row in dataGridView2.Rows)
                                    {
                                        objBreakPointM.BREAKPOINT_ORGANISMS_ID = row.Cells[2].Value.ToString();
                                        objBreakPointM = objConfig.SaveBreakPoint_With_Organisms(objBreakPointM);
                                    }
                                }
                                else
                                {
                                    objBreakPointM = objConfig.ClearBreakPoint_with_Organisms(objBreakPointM);
                                }

                                this.Close();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error in Function Save [edit]", ex.Message);
                    }
                }
                else if (moduleType == "dup")
                {
                    try
                    {
                        // Code & Name  Fix do not empty!
                        if (txtCode.Text != "" && txtName.Text != "")
                        {
                            objBreakPointM.CUSTOMIZED_BREAKPOINT_CODE = txtCode.Text;
                            objBreakPointM.CUSTOMIZED_BREAKPOINT_TEXT = txtName.Text;

                            objBreakPointM.LOGUSERID = ControlParameter.loginID.ToString();
                            objBreakPointM = objConfig.SaveBreakPoint(objBreakPointM);

                            if (dataGridView1.Rows.Count > 0)
                            {
                                //objBreakPointM = objConfig.DeleteBreakPoint_List_Dict(objBreakPointM);
                                foreach (DataGridViewRow row in dataGridView1.Rows)
                                {
                                    objBreakPointM.BREAKPOINT_CODE = row.Cells[0].Value.ToString();
                                    objBreakPointM.BREAKPOINT_TEXT = row.Cells[1].Value.ToString();
                                    objBreakPointM.BREAKPOINT_ID = row.Cells[10].Value.ToString();


                                    // S I R
                                    if (row.Cells[2].Value == null || row.Cells[2].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells[2].Value.ToString()))
                                    {
                                        objBreakPointM.BREAKPOINT_MIDS = "";
                                    }
                                    else
                                    {
                                        objBreakPointM.BREAKPOINT_MIDS = row.Cells[2].Value.ToString();         //MIDS

                                    }
                                    if (row.Cells[3].Value == null || row.Cells[3].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells[3].Value.ToString()))
                                    {
                                        objBreakPointM.BREAKPOINT_MIDSDD = "";
                                    }
                                    else
                                    {
                                        objBreakPointM.BREAKPOINT_MIDSDD = row.Cells[3].Value.ToString();         //MIDSDD

                                    }

                                    if (row.Cells[4].Value == null || row.Cells[4].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells[4].Value.ToString()))
                                    {
                                        objBreakPointM.BREAKPOINT_MIDI = "";
                                    }
                                    else
                                    {
                                        objBreakPointM.BREAKPOINT_MIDI = row.Cells[4].Value.ToString();         //MIDI

                                    }
                                    if (row.Cells[5].Value == null || row.Cells[5].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells[5].Value.ToString()))
                                    {
                                        objBreakPointM.BREAKPOINT_MIDR = "";
                                    }
                                    else
                                    {
                                        objBreakPointM.BREAKPOINT_MIDR = row.Cells[5].Value.ToString();         //MIDR

                                    }
                                    if (row.Cells[6].Value == null || row.Cells[6].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells[6].Value.ToString()))
                                    {
                                        objBreakPointM.BREAKPOINT_MICS = "";
                                    }
                                    else
                                    {
                                        objBreakPointM.BREAKPOINT_MICS = row.Cells[6].Value.ToString();         //MICS

                                    }
                                    if (row.Cells[7].Value == null || row.Cells[7].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells[7].Value.ToString()))
                                    {
                                        objBreakPointM.BREAKPOINT_MICSDD = "";
                                    }
                                    else
                                    {
                                        objBreakPointM.BREAKPOINT_MICSDD = row.Cells[7].Value.ToString();         //MICSDD

                                    }

                                    if (row.Cells[8].Value == null || row.Cells[8].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells[8].Value.ToString()))
                                    {
                                        objBreakPointM.BREAKPOINT_MICI = "";
                                    }
                                    else
                                    {
                                        objBreakPointM.BREAKPOINT_MICI = row.Cells[8].Value.ToString();         //MICI

                                    }
                                    if (row.Cells[9].Value == null || row.Cells[9].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells[9].Value.ToString()))
                                    {
                                        objBreakPointM.BREAKPOINT_MICR = "";
                                    }
                                    else
                                    {
                                        objBreakPointM.BREAKPOINT_MICR = row.Cells[9].Value.ToString();         //MICR

                                    }
                                    objBreakPointM = objConfig.SaveBreakPoint_In_DICT_Breakpoint_LIST(objBreakPointM);
                                }
                            }
                            // SAVE ORGANISMS
                            if (dataGridView2.Rows.Count > 0)
                            {
                                objBreakPointM = objConfig.ClearBreakPoint_with_Organisms(objBreakPointM);
                                foreach (DataGridViewRow row in dataGridView2.Rows)
                                {
                                    objBreakPointM.BREAKPOINT_ORGANISMS_ID = row.Cells[2].Value.ToString();
                                    objBreakPointM = objConfig.SaveBreakPoint_With_Organisms(objBreakPointM);
                                }
                            }
                            this.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error in Function Save [dup]", "Warning" + ex.Message);
                    }
                }
                else if (moduleType == "add")
                {
                    try
                    {
                        if (txtCode.Text != "" && txtName.Text != "")
                        {
                            objBreakPointM.CUSTOMIZED_BREAKPOINT_CODE = txtCode.Text;
                            objBreakPointM.CUSTOMIZED_BREAKPOINT_TEXT = txtName.Text;
                            objBreakPointM.BREAKPOINT_DESCRIPTION = txtComment.Text;

                            objBreakPointM.LOGUSERID = ControlParameter.loginID.ToString();
                            objBreakPointM = objConfig.SaveBreakPoint(objBreakPointM);

                            if (dataGridView1.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow row in dataGridView1.Rows)
                                {
                                    objBreakPointM.BREAKPOINT_CODE = row.Cells[0].Value.ToString();
                                    objBreakPointM.BREAKPOINT_TEXT = row.Cells[1].Value.ToString();
                                    objBreakPointM.BREAKPOINT_ID = row.Cells[10].Value.ToString();


                                    // S I R
                                    if (row.Cells[2].Value == null || row.Cells[2].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells[2].Value.ToString()))
                                    {
                                        objBreakPointM.BREAKPOINT_MIDS = "";
                                    }
                                    else
                                    {
                                        objBreakPointM.BREAKPOINT_MIDS = row.Cells[2].Value.ToString();         //MIDS

                                    }
                                    if (row.Cells[3].Value == null || row.Cells[3].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells[3].Value.ToString()))
                                    {
                                        objBreakPointM.BREAKPOINT_MIDSDD = "";
                                    }
                                    else
                                    {
                                        objBreakPointM.BREAKPOINT_MIDSDD = row.Cells[3].Value.ToString();         //MIDSDD

                                    }

                                    if (row.Cells[4].Value == null || row.Cells[4].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells[4].Value.ToString()))
                                    {
                                        objBreakPointM.BREAKPOINT_MIDI = "";
                                    }
                                    else
                                    {
                                        objBreakPointM.BREAKPOINT_MIDI = row.Cells[4].Value.ToString();         //MIDI

                                    }
                                    if (row.Cells[5].Value == null || row.Cells[5].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells[5].Value.ToString()))
                                    {
                                        objBreakPointM.BREAKPOINT_MIDR = "";
                                    }
                                    else
                                    {
                                        objBreakPointM.BREAKPOINT_MIDR = row.Cells[5].Value.ToString();         //MIDR

                                    }
                                    if (row.Cells[6].Value == null || row.Cells[6].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells[6].Value.ToString()))
                                    {
                                        objBreakPointM.BREAKPOINT_MICS = "";
                                    }
                                    else
                                    {
                                        objBreakPointM.BREAKPOINT_MICS = row.Cells[6].Value.ToString();         //MICS

                                    }
                                    if (row.Cells[7].Value == null || row.Cells[7].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells[7].Value.ToString()))
                                    {
                                        objBreakPointM.BREAKPOINT_MICSDD = "";
                                    }
                                    else
                                    {
                                        objBreakPointM.BREAKPOINT_MICSDD = row.Cells[7].Value.ToString();         //MICSDD

                                    }

                                    if (row.Cells[8].Value == null || row.Cells[8].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells[8].Value.ToString()))
                                    {
                                        objBreakPointM.BREAKPOINT_MICI = "";
                                    }
                                    else
                                    {
                                        objBreakPointM.BREAKPOINT_MICI = row.Cells[8].Value.ToString();         //MICI

                                    }
                                    if (row.Cells[9].Value == null || row.Cells[9].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells[9].Value.ToString()))
                                    {
                                        objBreakPointM.BREAKPOINT_MICR = "";
                                    }
                                    else
                                    {
                                        objBreakPointM.BREAKPOINT_MICR = row.Cells[9].Value.ToString();         //MICR

                                    }
                                    objBreakPointM = objConfig.SaveBreakPoint_In_DICT_Breakpoint_LIST(objBreakPointM);
                                }
                            }

                            // SAVE ORGANISMS
                            if (dataGridView2.Rows.Count > 0)
                            {
                                objBreakPointM = objConfig.ClearBreakPoint_with_Organisms(objBreakPointM);
                                foreach (DataGridViewRow row in dataGridView2.Rows)
                                {
                                    objBreakPointM.BREAKPOINT_ORGANISMS_ID = row.Cells[2].Value.ToString();
                                    objBreakPointM = objConfig.SaveBreakPoint_With_Organisms(objBreakPointM);
                                }
                            }
                            this.Close();
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error in Function Save [add]", ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Error Function Save Desc.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Btn_search_Click(object sender, EventArgs e)
        {
            ConfigurationController objConfiguration = new ConfigurationController();
            frmSearchdict fm = new frmSearchdict();

            DataTable dt;
            try
            {
                dt = objConfiguration.GetDICTAntibiotic();

                dt.Columns["CODE"].ColumnName = "code";
                dt.Columns["TEXT"].ColumnName = "name";
                dt.Columns["ID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.RefreshData();
                fm.ShowDialog();

                if (fm.Selected == true)
                {
                    objBreakPointM.CUSTOMIZED_BREAKPOINT_ID = fm.SelectedID.ToString();
                    objBreakPointM.CUSTOMIZED_BREAKPOINT_TEXT = fm.SelectedName;
                    objBreakPointM.CUSTOMIZED_BREAKPOINT_CODE = fm.SelectedCode;

                    BREAKP01 = objBreakPointM.CUSTOMIZED_BREAKPOINT_CODE;
                    BREAKP02 = objBreakPointM.CUSTOMIZED_BREAKPOINT_TEXT;
                    BREAKP03 = objBreakPointM.CUSTOMIZED_BREAKPOINT_ID;

                    int minRowCount = 0;
                    bool CheckBreakPointcode = false;
                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        minRowCount++;
                    }

                    if (minRowCount == 0)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = BREAKP01;
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = BREAKP02;
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[10].Value = BREAKP03;

                    }
                    else
                    {
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (row.Cells[10].Value.ToString().Contains(BREAKP03.ToString()))
                            {
                                CheckBreakPointcode = true;
                            }
                        }
                        if (CheckBreakPointcode == true)
                        {
                            MessageBox.Show("Code Exits please select another code");
                        }
                        else if (CheckBreakPointcode == false)
                        {
                            dataGridView1.Rows.Add();
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = BREAKP01;
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = BREAKP02;
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[10].Value = BREAKP03;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc" + ex.Message);
            }
            finally
            {
                fm = null;
            }
        }
        // For Select Organisms
        private void btn_select_Organisms_Click(object sender, EventArgs e)
        {
            ConfigurationController objConfiguration = new ConfigurationController();
            frmSearchdict fm = new frmSearchdict();

            BREAKP01 = "";
            BREAKP02 = "";
            BREAKP03 = "";

            DataTable dt;
            try
            {
                dt = objConfiguration.GetBreakPoint_Organisms();

                dt.Columns["CODE"].ColumnName = "code";
                dt.Columns["TEXT"].ColumnName = "name";
                dt.Columns["ID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.RefreshData();
                fm.ShowDialog();

                if (fm.Selected == true)
                {
                    objBreakPointM.BREAKPOINT_ORGANISMS_ID = fm.SelectedID.ToString();
                    objBreakPointM.BREAKPOINT_ORGANISMS_TEXT = fm.SelectedName;
                    objBreakPointM.BREAKPOINT_ORGANISMS_CODE = fm.SelectedCode;

                    BREAKP01 = objBreakPointM.BREAKPOINT_ORGANISMS_CODE;
                    BREAKP02 = objBreakPointM.BREAKPOINT_ORGANISMS_TEXT;
                    BREAKP03 = objBreakPointM.BREAKPOINT_ORGANISMS_ID;

                    int minRowCount = 0;
                    bool CheckBreakPointcode = false;
                    for (int i = 0; i < dataGridView2.RowCount; i++)
                    {
                        minRowCount++;
                    }

                    if (minRowCount == 0)
                    {
                        dataGridView2.Rows.Add();
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[0].Value = BREAKP01;
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[1].Value = BREAKP02;
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[2].Value = BREAKP03;

                    }
                    else
                    {
                        foreach (DataGridViewRow row in dataGridView2.Rows)
                        {
                            if (row.Cells[2].Value.ToString().Contains(BREAKP03.ToString()))
                            {
                                CheckBreakPointcode = true;
                            }
                        }
                        if (CheckBreakPointcode == true)
                        {
                            MessageBox.Show("Code Exits please select another code");
                        }
                        else if (CheckBreakPointcode == false)
                        {
                            dataGridView2.Rows.Add();
                            dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[0].Value = BREAKP01;
                            dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[1].Value = BREAKP02;
                            dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[2].Value = BREAKP03;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc" + ex.Message);
            }
            finally
            {
                fm = null;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
        }
    }
}
