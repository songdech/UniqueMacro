using System;
using System.Data;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{
    public partial class Protocol : UserControl
    {
        private ConfigurationController objconfig = new ConfigurationController();
        private ProtocalM objProtocolM = new ProtocalM();

        public Protocol()
        {
            InitializeComponent();
            objconfig = new ConfigurationController();
        }

        public Action checkEnableButton { get; set; }
        public Action EnableDupButton { get; set; }

        public string strModule
        {
            get
            {
                return "protocol";
            }
        }

        private void loadData()
        {
            try
            {
                ConfigurationController objConfigProtocolM = new ConfigurationController();

                DataTable dt = null;

                dt = objConfigProtocolM.GetDICTProtocals(txtSearchCode.Text.Trim(), txtSearchName.Text.Trim() );

                if (dt.Rows.Count > 0)
                {
                    Action instance = EnableDupButton;
                    if (instance != null)
                        instance();

                    Action instance2 = checkEnableButton;
                    if (instance2 != null)
                        instance2();

                    simpleBtn_Delete.Visible = true;
                }
                else
                {
                    Action instance2 = checkEnableButton;
                    if (instance2 != null)
                        instance2();
                }

                dataGridView2.DataSource = dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void dataGridView2_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    ProtocalM objProtocolM = new ProtocalM();

                    DataGridViewRow row = dataGridView2.Rows[e.RowIndex];

                    objProtocolM.PROTOCOLID = Convert.ToInt32(row.Cells[0].Value.ToString());
                    objProtocolM.PROTOCOLCODE = row.Cells[1].Value.ToString();
                    objProtocolM.PROTOCOL_NAME = row.Cells[2].Value.ToString();
                    objProtocolM.PROTOCOL_DESCRIPTION = row.Cells[3].Value.ToString();

                    objProtocolM.REPORTFORMA = Convert.ToInt32(row.Cells[6].Value.ToString());

                    if (row.Cells[7].Value.ToString() != "" )
                    {
                        objProtocolM.PROTOCOL_SPMGROUP_ID = row.Cells[7].Value.ToString();
                    }
                    else
                    {
                        objProtocolM.PROTOCOL_SPMGROUP_ID = "0";
                    }
                    if (row.Cells[8].Value.ToString() !="" )
                    {
                        objProtocolM.PROTOCOL_SPMGROUP_NAME = row.Cells[8].Value.ToString();
                    }
                    else
                    {
                        objProtocolM.PROTOCOL_SPMGROUP_NAME = "";
                    }

                    ControlProtocol.ProtocolID = Convert.ToString(objProtocolM.PROTOCOLID);
                    ControlParameter.ProtocalM = objProtocolM;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void dataGridView2_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView2.Rows[e.RowIndex];
                string strProtocolID = row.Cells[0].Value.ToString();
                ConfigurationFld.frmAddProtocol fm = new ConfigurationFld.frmAddProtocol("edit", strProtocolID);
                fm.ShowDialog();
            }
        }

        private void txtSearchCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                loadData();
            }
        }
        private void txtSearchName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                loadData();
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            loadData();
        }

        private void simpleBtn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult Yes = MessageBox.Show("คำเตือน!! คุณต้องการลบใช่หรือไม่? ข้อมูล Protocol configure จะถูกลบออกทั้งหมด ", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Yes == DialogResult.Yes)
                {
                    DataTable dt = null;
                    objProtocolM = ControlParameter.ProtocalM;

                    ConfigurationController objConfig = new ConfigurationController();

                    // STEP CLEAR & DELETE
                    // - Check result in MB_REQUESTS

                    dt = objConfig.Check_Result_PROTOCOLS(objProtocolM);

                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("Can't delete Found Result in database", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                    else
                    {
                        // 1. DICT_MB_CHEMISTRYS ----> DELETE 
                        // DICT_MB_CHEMISTRYS
                        // Check and Delete DIC_MB_PROTOCOLS
                        // DICT_MB_PROTOCOL_STAINS
                        // DICT_MB_PROTOCOL_MEDIA

                        objConfig.DeleteProtocol_In_DICT(objProtocolM);
                        objConfig.DeleteProtocolStainDict(objProtocolM);
                        objConfig.DeleteProtocolMediaDict(objProtocolM);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
