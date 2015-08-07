using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;


public partial class CaseMgr_MargerPeopleDetail_Edit : BasePage
{
    GroupAuthrity ga = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        lblHeaderText.Text = "選取人員";

        //權限控制
        AuthrityControl();

        if (!IsPostBack)
        {
           HFD_Mode.Value = Util.GetQueryString("Mode");   //判斷要選取的是 主要人的 uid 還是要被合併人的uid
            //載入下拉式選單資料
            LoadDropDownListData();
            //載入資料
         //   LoadFormData();
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
        if (Authrity.PageRight("_Focus") == false)
        {
            return;
        }
        Authrity.CheckButtonRight("_AddNew", btnAdd);
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
                        case isnull(uid, '') when '' then 0 else 1 end as selected  ,                      
                         uid, CName As  姓名,Memo as 備註,Phone As 電話
                        from Member 
                        where 1=1 
                        and isnull(IsDelete, '') != 'Y'
                    ";

        if (txtPhone.Text.Trim() != "")
        {
            strSql += " and Phone like @Phone\n";
        }

        if (txtName.Text.Trim() != "")
        {
            strSql += " and CName like @CName\n";
        }

        if (txtMemo.Text.Trim() != "")
        {
            strSql += " and Memo like @Memo\n";
        }
        
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("CName", "%" + txtName.Text.Trim() + "%");
        dict.Add("Phone", '%' + txtPhone.Text.Trim() + '%');
        dict.Add("Memo", '%' + txtMemo.Text.Trim() + '%');
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
        col.ColumnType = NPOColumnType.Checkbox;
        col.ControlKeyColumn.Add("uid");
        col.CaptionWithControl = false;

        col.ControlName.Add("chkSelectUid");
        col.ControlId.Add("chkSelectUid");
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
        col = new NPOGridViewColumn("備註");
        col.ColumnType = NPOColumnType.NormalText;
        col.ColumnName.Add("備註");
        npoGridView.Columns.Add(col);
        //-------------------------------------------------------------------------
        col = new NPOGridViewColumn("電話");
        col.ColumnType = NPOColumnType.NormalText;
        col.ColumnName.Add("電話");
        npoGridView.Columns.Add(col);
        //-------------------------------------------------------------------------

        lblGridList.Text = npoGridView.Render();
    }
    //-------------------------------------------------------------------------------------------------------------
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string[] strArray;
        string uids = "";
        foreach (string str in Request.Form.Keys)
        {
            if (str.StartsWith("chkSelectUid_"))
            {
                strArray = str.Split('_');
                uids += strArray[1] + ",";
            }
        }
        uids = uids.TrimEnd(',');

        int iUid = uids.IndexOf(",");
        //if (HFD_Mode.Value == "Main")
        //{
            if (iUid != -1)
            {
                ShowSysMsg("此處為單選，請確認!");
                return;
            }
       // }
        objNpoDB.BeginTrans();
        try
        {
            //先刪除 EstateMap 已有的資料
            //Delete();
            //新增選取到的資料
            //Insert(uids);
            objNpoDB.CommitTrans();
        }
        catch (Exception ex)
        {
            objNpoDB.RollbackTrans();
            throw ex;
        }
        //Response.Write(@"<script>self.opener.LoadMargerPeople();self.opener.document.getElementById('HFD_PeopleUID').value='" + uids + "';window.close();</script>");
        Response.Write(@"<script>self.opener.document.getElementById('HFD_PeopleUID').value='" + uids + "';self.opener.document.getElementById('HFD_Mode').value='" + HFD_Mode.Value + "';self.opener.document.getElementById('btnReload').click();window.close();</script>");
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
