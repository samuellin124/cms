using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;

public partial class CaseMgr_SelectMember : BasePage 
{
    GroupAuthrity ga = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        lblHeaderText.Text = "選擇會友資料";

        if (!IsPostBack)
        {
            HFD_HouseUID.Value = Util.GetQueryString("HouseUID");

            //載入下拉式選單資料
            LoadDropDownListData();
            //載入資料
            LoadFormData();
        }
    }
    //-------------------------------------------------------------------------
    private void SetupDefaultValue()
    {
    }
    //-------------------------------------------------------------------------
    private void SetButton()
    {
    }
    //-------------------------------------------------------------------------
    private void AuthrityControl()
    {
    }
    //-------------------------------------------------------------------------
    public void LoadDropDownListData()
    {
    }
    //-------------------------------------------------------------------------------------------------------------
    private void LoadFormData()
    {
        string strSql = @"
                            select 
                            case isnull(uid, '') when '' then 0 else 1 end as selected,
                            uid, CName as [姓名],
                            Phone as [電話],
                            FileName as [檔案名稱]
                            From Member
                            where isnull(IsDelete, '') != 'Y'
                        ";

        //if (txtFileName.Text != "")
        //{
        //    strSql += " and FileName like @FileName\n";
        //}

        if (txtPhone.Text != "")
        {
            strSql += " and Phone = @Phone\n";
        }

        if (txtName.Text != "")
        {
            strSql += " and CName like @CName\n";
        }

        Dictionary<string, object> dict = new Dictionary<string, object>();
        //dict.Add("FileName", "%" + txtFileName.Text + "%");
        dict.Add("CName", "%" + txtName.Text + "%");
        dict.Add("Phone", txtPhone.Text);

        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        int count = dt.Rows.Count;

        //Grid initial
        //沒有需要特別處理的欄位時
        NPOGridView npoGridView = new NPOGridView();
        npoGridView.Source = NPOGridViewDataSource.fromDataTable;
        npoGridView.dataTable = dt;
        npoGridView.Keys.Add("uid");
        npoGridView.DisableColumn.Add("uid");
        npoGridView.ShowPage = false;

        NPOGridViewColumn col;
        //-------------------------------------------------------------------------
        col = new NPOGridViewColumn("選擇");
        col.ColumnType = NPOColumnType.Checkbox ;
        col.ControlKeyColumn.Add("uid");
        col.CaptionWithControl = false;

        col.ControlName.Add("chkSelectLand");
        col.ControlId.Add("chkSelectLand");
        col.ControlValue.Add("0");
        col.ControlText.Add("");
        col.ColumnName.Add("selected");

        npoGridView.Columns.Add(col);
        //-------------------------------------------------------------------------
        col = new NPOGridViewColumn("姓名");
        col.ColumnType = NPOColumnType.NormalText;
        col.ColumnName.Add("姓名");
        npoGridView.Columns.Add(col);
        //-------------------------------------------------------------------------
        col = new NPOGridViewColumn("電話");
        col.ColumnType = NPOColumnType.NormalText;
        col.ColumnName.Add("電話");
        npoGridView.Columns.Add(col);
        //-------------------------------------------------------------------------
        col = new NPOGridViewColumn("檔案名稱");
        col.ColumnType = NPOColumnType.NormalText;
        col.ColumnName.Add("檔案名稱");
        npoGridView.Columns.Add(col);
        
        lblGridList.Text = npoGridView.Render();
    }
    //-------------------------------------------------------------------------------------------------------------
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string[] strArray;
        string uids = "";
        foreach (string str in Request.Form.Keys)
        {
            if (str.StartsWith("chkSelectLand_"))
            {
                strArray = str.Split('_');
                uids += strArray[1] + ",";
            }
        }
        uids = uids.TrimEnd(',');
        int iUid = uids.IndexOf(",");
        if (iUid != -1)
        {
            ShowSysMsg("此處為單選，請確認!");
            return;
        }
     
        // ============Start 撈資料===================================
        string strSql;
        DataRow dr = null;
        DataTable dt = null;
        Dictionary<string, object> dict = new Dictionary<string, object>();

        strSql = @"
                   select *
                   from Member
                   where uid=@uid
                   and isnull(IsDelete, '') != 'Y'
                  ";
        dict.Add("uid", uids);
        dt = NpoDB.GetDataTableS(strSql, dict);
        //資料異常
        if (dt.Rows.Count == 0)
        {
            return;
        }
        dr = dt.Rows[0];


        //財產編號                 
        string strFirst = dr["First"].ToString().Trim();
        string strChurchYN_0 = dr["ChurchYN"].ToString().Trim();
        string strChurchYN_1 = dr["ChurchYN"].ToString().Trim();
        if (strFirst == "false")
        {
            strFirst = "";
        }
        else
            strFirst = "true";

        if (dr["ChurchYN"].ToString().Trim() == "是")
        {
            strChurchYN_0 = "1";
            strChurchYN_1 = "";
        }
        else
        {
            strChurchYN_0 = "";
            strChurchYN_1 = "1";
        }
        // ============End 撈資料===================================

        Response.Write(@"<script>self.opener.document.getElementById('HFD_MemberID').value='" + uids + "';self.opener.document.getElementById('btnReload').click();window.close();</script>");
     }
    //-------------------------------------------------------------------------------------------------------------
    private void Insert(string uids)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        List<ColumnData> list = new List<ColumnData>();

        string[] UIDArr = uids.Split(',');
        foreach (string uid in UIDArr)
        {
            list.Clear();
            dict.Clear();
            list.Add(new ColumnData("HouseUID", HFD_HouseUID.Value, true, true, false));
            list.Add(new ColumnData("LandUID", uid, true, true, false));
            string strSql = Util.CreateInsertCommand("EstateMap", list, dict);
            objNpoDB.ExecuteSQL(strSql, dict);
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    private void Delete()
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        string strSql = @"
                            delete from EstateMap
                            where 1=1
                            and HouseUID=@HouseUID
                         ";
        dict.Add("HouseUID", HFD_HouseUID.Value);
        objNpoDB.ExecuteSQL(strSql, dict);
    }
    //------------------------------------------------------------------
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Write(@"<script>window.close();</script>");
    }
    //-------------------------------------------------------------------------------------------------------------
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        LoadFormData();
    }
    //-------------------------------------------------------------------------------------------------------------
}
