using System;
using System.Data;
using System.Collections.Generic ;
using System.Web.UI.WebControls;

public partial class SysMgr_SelectionData : BasePage
{
    #region NpoGridView �B�z���������{���X
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
    #endregion NpoGridView �B�z���������{���X
    //---------------------------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["ProgID"] = "SelectionData";
        //�v���B�z
        AuthrityControl();

        //�� npoGridView �ɤ~�ݭn
        Util.ShowSysMsgWithScript("$('#btnPreviousPage').hide();$('#btnNextPage').hide();$('#btnGoPage').hide();");

        if (!IsPostBack)
        {
            LoadFormData();
            LoadDropDownListData();
        }
    }
    //------------------------------------------------------------------------------
    public void LoadDropDownListData()
    {
        //���o����w���ﶵ���
        DataTable dt = CaseUtil.GetCaseGroup(false);
        Util.FillDropDownList(ddlGroupName, dt, "CaseGroup", "CaseGroup", true);
    }
    //------------------------------------------------------------------------------
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
    public void LoadFormData()
    {
        string strSql = @"select Uid,
	                            CaseGroup as [�ﶵ�s��],
	                            CaseId as [�Ƨ�],
	                            CaseName as [�ﶵ�W��]
                            from CaseCode 
                            where 1=1
                            ";
        if (ddlGroupName.SelectedValue != "")
        {
            strSql += " and CaseGroup = @CaseGroup ";
        }
        if (txtCaseName.Text != "")
        {
            strSql += " and CaseName=@CaseName ";
        }
        //����w�����
        strSql += " and (islock<>1 or islock is null) ";
        strSql += " order by CaseGroup ,cast( CaseId as int) ";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("CaseGroup", ddlGroupName.SelectedValue);
        dict.Add("CaseName", "%" + txtCaseName.Text + "%");

        DataTable dt = NpoDB.GetDataTableS(strSql, dict);

        //Grid initial
        NPOGridView npoGridView = new NPOGridView();
        npoGridView.Source = NPOGridViewDataSource.fromDataTable;
        npoGridView.dataTable = dt;
        npoGridView.Keys.Add("uid");
        npoGridView.DisableColumn.Add("uid");

        npoGridView.CurrentPage = Util.String2Number(HFD_CurrentPage.Value);
        npoGridView.EditLink = Util.RedirectByTime("SelectionData_Edit.aspx", "DataUID=");
        lblGridList.Text = npoGridView.Render();
    }
    //------------------------------------------------------------------------------
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect(Util.RedirectByTime("SelectionData_Edit.aspx?Mode=ADD&"));
    }
    //------------------------------------------------------------------------------
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        LoadFormData();
    }
    //------------------------------------------------------------------------------
}
