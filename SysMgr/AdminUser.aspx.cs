using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data; 

public partial class SysMgr_AdminUser : BasePage
{
    //NpoGridView 處理換頁相關程式碼
    Button btnNextPage, btnPreviousPage, btnGoPage;
    HiddenField HFD_CurrentPage;

    override protected void OnInit(EventArgs e)
    {
        CreatePageControl();
        base.OnInit(e);
    }
    private void CreatePageControl()
    {
        // Create dynamic controls here.
        btnNextPage = new Button();
        btnNextPage.ID = "btnNextPage";
        btnNextPage.Height = 1;
        btnNextPage.Width = 1;
        Form1.Controls.Add(btnNextPage);
        btnNextPage.Click += new System.EventHandler(btnNextPage_Click);

        btnPreviousPage = new Button();
        btnPreviousPage.ID = "btnPreviousPage";
        Form1.Controls.Add(btnPreviousPage);
        btnPreviousPage.Click += new System.EventHandler(btnPreviousPage_Click);

        btnGoPage = new Button();
        btnGoPage.ID = "btnGoPage";
        Form1.Controls.Add(btnGoPage);
        btnGoPage.Click += new System.EventHandler(btnGoPage_Click);

        HFD_CurrentPage = new HiddenField();
        HFD_CurrentPage.Value = "1";
        HFD_CurrentPage.ID = "HFD_CurrentPage";
        Form1.Controls.Add(HFD_CurrentPage);
    }
    protected void btnPreviousPage_Click(object sender, EventArgs e)
    {
        HFD_CurrentPage.Value = Util.MinusStringNumber(HFD_CurrentPage.Value);
        LoadFormData();
    }
    protected void btnNextPage_Click(object sender, EventArgs e)
    {
        HFD_CurrentPage.Value = Util.AddStringNumber(HFD_CurrentPage.Value);
        LoadFormData();
    }
    protected void btnGoPage_Click(object sender, EventArgs e)
    {
        LoadFormData();
    }
    //---------------------------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["ProgID"] = "AdminUser";
        //權控處理
        AuthrityControl();

        Util.ShowSysMsgWithScript("$('#btnPreviousPage').hide();$('#btnNextPage').hide();$('#btnGoPage').hide();");

        if (!IsPostBack)
        {
            LoadDropDownListData();
            LoadFormData();
        }
    }
    //----------------------------------------------------------------------
    public void AuthrityControl()
    {
        if (Authrity.PageRight("_Focus") == false)
        {
            return;
        }

        Authrity.CheckButtonRight("_AddNew", btnAdd);
        Authrity.CheckButtonRight("_Query", btnQuery);
    }
    //----------------------------------------------------------------------
    public void LoadDropDownListData()
    {
        //部門名稱
        string strSql = "select DeptID, DeptName from dept order by DeptID";
        Util.FillDropDownList(ddlDept, strSql, "DeptName", "DeptID", true);
        //使用者群組
        strSql = "select uid, GroupName from AdminGroup order by uid";
        Util.FillDropDownList(ddlGroup, strSql, "GroupName", "uid", true);
    }
    //----------------------------------------------------------------------
    public void LoadFormData()
    {
        //查詢字串
        string strSql = " select u.Uid, u.UserID as 帳號, u.UserName as 使用者名稱,\n";
        strSql += " 部門名稱=d.DeptName,使用者身份=g.GroupName,\n";
        strSql += " case when u.IsUse=1 then '使用' else '停用' end as 是否使用\n";
        strSql += " from AdminUser u \n";
        strSql += " left join AdminGroup g on g.uid=u.GroupID \n";
        strSql += " left join Dept d on (u.DeptID=d.DeptID and u.OrgID=d.OrgID) \n";
        strSql += " where u.OrgID=@OrgID\n";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("OrgID", SessionInfo.OrgID);
        if (txtUserID.Text != "")
        {
            strSql += " and u.UserID=@UserID";
            dict.Add("UserID", txtUserID.Text);
        }
        if (txtUserName.Text != "")
        {
            strSql += " and u.UserName like @UserName";
            dict.Add("UserName", "%" + txtUserName.Text.Trim() + "%");
        }
        if (ddlDept.SelectedValue != "")
        {
            strSql += " and u.DeptID=@DeptID";
            dict.Add("DeptID", ddlDept.SelectedValue);
        }
        if (ddlGroup.SelectedValue != "")
        {
            strSql += " and u.GroupID=@GroupID";
            dict.Add("GroupID", ddlGroup.SelectedValue);
        }
        strSql += "  order by u.UserID";
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);

        NPOGridView npoGridView = new NPOGridView();
        npoGridView.Source = NPOGridViewDataSource.fromDataTable;
        npoGridView.dataTable = dt;
        npoGridView.Keys.Add("uid");
        npoGridView.DisableColumn.Add("uid");

        npoGridView.CurrentPage = Util.String2Number(HFD_CurrentPage.Value);
        npoGridView.EditLink = Util.RedirectByTime("AdminUser_Edit.aspx", "Uid=");
        lblGridList.Text = npoGridView.Render();
    }
    //----------------------------------------------------------------------
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect(Util.RedirectByTime("AdminUser_Add.aspx", ""));
    }
    //----------------------------------------------------------------------
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        LoadFormData();
    }
    //----------------------------------------------------------------------
}
