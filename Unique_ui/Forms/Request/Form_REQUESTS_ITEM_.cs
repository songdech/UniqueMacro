using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList.StyleFormatConditions;

using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;


namespace UNIQUE.OEM
{
    public partial class Form_REQUESTS_ITEM_ : Form
    {
        private string cellaccessnumber;
        private string cellrequestID;

        private int count;

        TreeListNode childNode;

        SqlConnection conn;
        System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("en-US");
        string strDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("hu-HU"));

        public Form_REQUESTS_ITEM_(string cellaccessnumber,string cellrequestID)
        {
            InitializeComponent();
            this.cellaccessnumber = cellaccessnumber;
            this.cellrequestID = cellrequestID;
        }

        private void Form_REQUESTS_ITEM__Load(object sender, EventArgs e)
        {
            conn = new Connection_ORM().Connect();
            Query_protocol();
        }

        private void CreateColumns(TreeList tl, DataSet ds)
        {
            // Create three columns.
            tl.BeginUpdate();
            tl.Columns.Add();
            tl.Columns[0].Caption = "<i><b>RequestID</i></b>";
            tl.Columns[0].VisibleIndex = 0;
            tl.Columns[0].Width = 80;
            tl.Columns[0].Visible = false;
            tl.OptionsView.AllowHtmlDrawHeaders = true;
            tl.RowHeight = 23;
            tl.Columns.Add();

            tl.Columns[1].Caption = "<i>Request</i><b>Protocol</b>";
            tl.Columns[1].VisibleIndex = 1;
            tl.Columns[1].Width = 150;
            tl.Columns[1].Visible = true;
            tl.OptionsView.AllowHtmlDrawHeaders = true;
            tl.Columns.Add();

            tl.Columns[2].Caption = "Receive Date";
            tl.Columns[2].VisibleIndex = 2;
            tl.Columns[2].Width = 200;
            tl.Columns[2].Visible = true;
            tl.Columns.Add();

            tl.Columns[3].Caption = "Collection Date";
            tl.Columns[3].VisibleIndex = 3;
            tl.Columns[3].Width = 200;
            tl.Columns[3].Visible = true;
            tl.Columns.Add();

            tl.Columns[4].Caption = "Status";
            tl.Columns[4].VisibleIndex = 4;
            tl.Columns[4].Width = 200;
            tl.Columns[4].Visible = true;
            tl.Columns.Add();

            tl.EndUpdate();
        }


        private void Query_protocol()
        {
            try
            {
                string sql_Query_protocol_item = @"SELECT REQUESTS.REQUESTID
,MB_REQUESTS.MBREQNUMBER
,MB_REQUESTS.COLLECTIONDATE,MB_REQUESTS.RECEIVEDDATE
,MB_REQUESTS.COMPLETED,MB_REQUESTS.STATUSSTART
,MB_REQUESTS.PRELIMINATIONSTATUS,MB_REQUESTS.LASTREVIEWDATE
,MB_REQUESTS.LASTUPDTESTDATE,MB_REQUESTS.COLLMATERIALTXT
,MB_REQUESTS.LOGDATE,MB_REQUESTS.VALIDATIONSTATUS
,MB_REQUESTS.MBREQEUSTSTATUS
FROM MB_REQUESTS 
LEFT OUTER JOIN REQUESTS ON REQUESTS.REQUESTID = MB_REQUESTS.REQUESTID
WHERE MB_REQUESTS.REQUESTID='" + cellrequestID + "' ";

                SqlCommand cmd = new SqlCommand(sql_Query_protocol_item, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
                ds.Clear();
                adp.Fill(ds, "sql_Query_protocol_item");
                cmd.ExecuteReader();
                if (ds.Tables["sql_Query_protocol_item"].Rows.Count > 0)
                {
                    CreateColumns(treeList1, ds);
                    treeList1.BeginUnboundLoad();
                    treeList1.ClearNodes();
                    TreeListNode parentForRootNodes = null;

                    for (int i = 0; i < ds.Tables["sql_Query_protocol_item"].Rows.Count; i++)
                    {
                        count++;
                        childNode = treeList1.AppendNode(new object[] {
                        ds.Tables["sql_Query_protocol_item"].Rows[i]["REQUESTID"].ToString(),
                        ds.Tables["sql_Query_protocol_item"].Rows[i]["MBREQNUMBER"].ToString(),
                        ds.Tables["sql_Query_protocol_item"].Rows[i]["RECEIVEDDATE"].ToString(),
                        ds.Tables["sql_Query_protocol_item"].Rows[i]["COLLECTIONDATE"].ToString(),
                        ds.Tables["sql_Query_protocol_item"].Rows[i]["MBREQEUSTSTATUS"].ToString() }, parentForRootNodes);

                        treeList1.EndUnboundLoad();
                        treeList1.ExpandAll();
                    }
                    labelControl7.Text = "Total Line " + count.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(strDateTime + "  Log: Q1000 :Request Item application  ?" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void treeList1_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {

        }
    }
    }
