using System;
using System.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;

public partial class FileMgr_News :BasePage 
{
    #region NpoGridView 處理換頁相關程式碼
    Button btnNextPage, btnPreviousPage, btnGoPage;
    HiddenField HFD_CurrentPage, HFD_CurrentQuerye;

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

        HFD_CurrentQuerye = new HiddenField();
        HFD_CurrentQuerye.Value = "Query";
        HFD_CurrentQuerye.ID = "HFHFD_CurrentQuerye";
        Form1.Controls.Add(HFD_CurrentQuerye);
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
    #endregion NpoGridView 處理換頁相關程式碼
    //------------------------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["ProgID"] = "News";
        //權控處理
        AuthrityControl();

        //有 npoGridView 時才需要
        Util.ShowSysMsgWithScript("$('#btnPreviousPage').hide();$('#btnNextPage').hide();$('#btnGoPage').hide();");

        if (!IsPostBack)
        {
            LoadDropDownListData();
            LoadFormData();
        }
    }
    //---------------------------------------------------------------------------
    public void LoadDropDownListData()
    {
        string strSql = "select DeptName, DeptID from Dept";
        DataTable dt = NpoDB.GetDataTableS(strSql, null);
        Util.FillDropDownList(ddlDept, dt, "DeptName", "DeptId", true);
    }
    //---------------------------------------------------------------------------
    public void AuthrityControl()
    {
        if (Authrity.PageRight("_Focus") == false)
        {
            return;
        }
        Authrity.CheckButtonRight("_AddNew", btnAdd);
        Authrity.CheckButtonRight("_Query", btnQuery);
        Authrity.CheckButtonRight("_Query", btnHistoryQuery);
    }
    //----------------------------------------------------------------------
    //QueryAction，查詢歷史資料或查詢一般資料
    //Query：查詢一般資料
    //History:查詢歷史資料
    public void LoadFormData()
    {
        string strSql;
        DataTable dt;
        strSql = "select News.uid, NewsSubject as 標題, NewsType as 類別, DeptName as 發佈單位,NewsRegDate as 發佈日期,\n";
        strSql += "NewsAuthor as 發佈者, NewsBeginDate as 起始日期, NewsEndDate as 結束日期\n";
        strSql += "from News\n";
        strSql += "where (IsDelete=0 or IsDelete is null)\n";
        if (HFD_CurrentQuerye.Value == "Query")
        {
            strSql += "and @SysDate between NewsBeginDate and NewsEndDate\n";
        }
        else
        {
            strSql += "and @SysDate > NewsEndDate\n";
        }
        if (txtNewsSubject.Text != "")
        {
            strSql += "and NewsSubject like @NewsSubject)\n";
        }
        if (ddlDept.SelectedValue != "")
        {
            strSql += " and news.DeptName=@DeptName)\n";
        }
        strSql += "order by NewsRegDate desc\n";

        string SysDate = Util.DateTime2String(DateTime.Now, DateType.yyyyMMdd, EmptyType.ReturnEmpty);

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("SysDate", SysDate);
        dict.Add("NewsSubject", "%" + txtNewsSubject.Text + "%");
        dict.Add("DeptName", ddlDept.SelectedValue);

        dt = NpoDB.GetDataTableS(strSql, dict);

        //Grid initial
        NPOGridView npoGridView = new NPOGridView();
        npoGridView.Source = NPOGridViewDataSource.fromDataTable;
        npoGridView.dataTable = dt;
        npoGridView.Keys.Add("uid");
        npoGridView.DisableColumn.Add("uid");

        npoGridView.CurrentPage = Util.String2Number(HFD_CurrentPage.Value);
        npoGridView.EditLink = Util.RedirectByTime("News_Edit.aspx", "NewsUID=");
        lblGridList.Text = npoGridView.Render();
    }
    //---------------------------------------------------------------------------
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        HFD_CurrentQuerye.Value = "Query";
        LoadFormData();
    }
    //---------------------------------------------------------------------------
    protected void btnHistoryQuery_Click(object sender, EventArgs e)
    {
        HFD_CurrentQuerye.Value = "History";
        LoadFormData();
    }
    //---------------------------------------------------------------------------
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect(Util.RedirectByTime("News_Edit.aspx?Mode=ADD&"));
    }
}
