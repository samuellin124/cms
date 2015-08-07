using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Web.UI.DataVisualization.Charting;


public partial class Church_CaseChangeChurch : BasePage 
{
    GroupAuthrity ga = null;

    #region NpoGridView 處理換頁相關程式碼
    Button btnNextPage, btnPreviousPage, btnGoPage;
    HiddenField HFD_CurrentPage;
    //排序用
    HiddenField HFD_CurrentSortField;
    HiddenField HFD_CurrentSortDir;

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

        HFD_CurrentSortField = new HiddenField();
        HFD_CurrentSortField.Value = "";
        HFD_CurrentSortField.ID = "HFD_CurrentSortField";
        Form1.Controls.Add(HFD_CurrentSortField);

        HFD_CurrentSortDir = new HiddenField();
        HFD_CurrentSortDir.Value = "asc";
        HFD_CurrentSortDir.ID = "HFD_CurrentSortDir";
        Form1.Controls.Add(HFD_CurrentSortDir);
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
    //---------------------------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["ProgID"] = "ConsultQry";
        //權限控制
        AuthrityControl();
        Util.ShowSysMsgWithScript("$('#btnPreviousPage').hide();$('#btnNextPage').hide();$('#btnGoPage').hide();");
        if (!IsPostBack)
        {
            //載入下拉式選單資料
            LoadDropDownListData();
            //載入資料
            LoadFormData();
        }
        // MainMenu.Text = CaseUtil.MakeMenuMove("1", 2); //傳入目前選擇的 tab
    }
    //-------------------------------------------------------------------------
    private void AuthrityControl()
    {
        ga = Authrity.GetGroupRight();
        if (Authrity.CheckPageRight(ga.Focus) == false)
        {
            return;
        }
        //btnAdd.Visible = false;//ga.AddNew;
        btnQuery.Visible = ga.Query;
    }
    //-------------------------------------------------------------------------
    public void LoadDropDownListData()
    {
        //ddlChangeChurchSta.Items.Clear();
        //ddlChangeChurchSta.Items.Add(new ListItem ("",""));
        //ddlChangeChurchSta.Items.Add(new ListItem("已處理", "已處理"));
        //ddlChangeChurchSta.Items.Add(new ListItem("未處理", "未處理"));
        
        //Util.FillCityData(ddlCity);
        //Util.FillAreaData(ddlArea, ddlCity.SelectedValue);
    }
    //-------------------------------------------------------------------------------------------------------------
    private void LoadFormData()
    {
        //查詢資料庫
        DataTable dt = GetDataTable();
        //沒有需要特別處理的欄位時
        NPOGridView npoGridView = new NPOGridView();
        npoGridView.Source = NPOGridViewDataSource.fromDataTable;
        npoGridView.dataTable = dt;
        npoGridView.Keys.Add("uid");
        //不顯示欄位
        npoGridView.DisableColumn.Add("uid");
        npoGridView.CurrentPage = Util.String2Number(HFD_CurrentPage.Value);

        npoGridView.EditLinkTarget = "轉介教會資料編輯','help=no,status=yes,resizable=yes,scrollbars=yes,center=yes,width=900px,height=500px";
        //npoGridView.EditLink = Util.RedirectByTime("ChangeChurchEdit.aspx", "Mode=MOD&MoveMainUid=" + Uid + "&uid=");
        npoGridView.EditLink = Util.RedirectByTime("CaseChangeChurchEdit.aspx", "uid=");
        lblGridList.Text = npoGridView.Render();
    }
    //-------------------------------------------------------------------------------------------------------------
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        LoadFormData();
    }
    //----------------------------------------------------------------------------------------------------------
    private DataTable GetDataTable()
    {
        string strSql = @"
                            Select  a.uid as uid ,CName as 姓名,
                            a.changeChurchDate as 轉介時間,
                            b.Name As 教會 
                            From Member a 
                            left outer join Church b on a.ChurchUid=b.uid 
                            Where 1=1 
                            And ChurchYN = '是'
                            And isnull(a.IsDelete, '') != 'Y'
                            and ChangeChurchSta='已處理'

                        ";

        if (txtChurch.Text != "")
        {
            strSql += " and b.Name like @ChurchName\n";
        }
        if (txtName.Text != "")
        {
            strSql += " and CName like @CName\n";
        }
        if (txtBegCreateDate.Text != "" && txtEndCreateDate.Text != "")
        {
            strSql += " and CONVERT(varchar(100), a.changeChurchDate, 111) Between @BegCreateDate And @EndCreateDate\n";
        }
        strSql += " order by a.changeChurchDate DESC";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("CName", "%" + txtName.Text + "%");
        dict.Add("ChurchName", "%" + txtChurch.Text + "%");
        dict.Add("BegCreateDate", txtBegCreateDate.Text);
        dict.Add("EndCreateDate", txtEndCreateDate.Text);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        int count = dt.Rows.Count;



        return dt;

    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        ExportDataTable Export = new ExportDataTable();
        DataTable dt = GetDataTable();
        Export.dataTable = dt;
        //不顯示的欄位
        Export.DisableColumn.Add("uid");
        string name = "個案轉介管理";
        string time ="";
        if (txtBegCreateDate.Text != "" && txtEndCreateDate.Text != "")
        {
            time = "("+txtBegCreateDate.Text + "~" + txtEndCreateDate.Text+")";
        }
        string Title = Export.GetTitle(name + time , 3);
        string ExportData = Title + Export.Render();
        Util.OutputTxt(ExportData, "1", "個案轉介管理" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"));

    }
 
   


}
