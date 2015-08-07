using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;

public partial class Rtp_Yaer : BasePage
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
        Session["ProgID"] = "Consult";
        HFD_EditServiceUser.Value = SessionInfo.UserID.ToString();

        litTitle.Text = "年度統計";


        //權限控制
        AuthrityControl();
        //設定一些按鍵的 Visible 屬性
        SetButton();
        Util.ShowSysMsgWithScript("$('#btnPreviousPage').hide();$('#btnNextPage').hide();$('#btnGoPage').hide();");
        if (!IsPostBack)
        {
            //載入下拉式選單資料
            LoadDropDownListData();
            //載入資料
            //  LoadFormData();

        }
        LoadHiddenData();
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
        ga = Authrity.GetGroupRight();
        if (Authrity.CheckPageRight(ga.Focus) == false)
        {
            return;
        }
        //btnAdd.Visible = ga.AddNew;
        //btnQuery.Visible = ga.Query;
    }
    //-------------------------------------------------------------------------
    public void LoadDropDownListData()
    {
        //Util.FillCityData(ddlCity);
        //Util.FillAreaData(ddlArea, ddlCity.SelectedValue);
        LoadDownListYear(ddlYear, (DateTime.Now.Year - 1), "Desc");
    }
    //年度查詢---------------------------------------------------------------------------
    private void LoadDownListYear(DropDownList ddlTarget, int iStartYear, string strOrder)
    {
        int iEndYear = Util.GetDBDateTime().Year;

        if (strOrder == "Desc")
        {
            for (int i = iEndYear; i >= iStartYear; i--)
            {
                ddlTarget.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
        }
        else
        {
            for (int i = iStartYear; i <= iEndYear; i++)
            {
                ddlTarget.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    private void LoadFormData()
    {
        //呼叫資料庫GetDataTable
        DataTable dt = GetDataTable();
        if (dt.Rows.Count == 0)
        {
            lblGridList.Text = "";
            ShowSysMsg("查無資料");
            return;
        }

            //********************************************************************************************
            //自訂欄位
            NPOGridView npoGridView = new NPOGridView();
            //來源種類
            npoGridView.Source = NPOGridViewDataSource.fromDataTable;
            //使用的 DataTable
            npoGridView.dataTable = dt;
            //不要顯示的欄位(可以多個)

            npoGridView.DisableColumn.Add("uid");
            npoGridView.EditLinkTarget = "','target=_blank,help=no,status=yes,resizable=yes,scrollbars=yes,scrolling=yes,scroll=yes,center=yes,width=900px,height=500px";
            //npoGridView.EditLink = Util.RedirectByTime("ConsultEdit2.aspx", "ConsultingUID=");
             
            //使用的 key 欄位,用在組成 QueryString 的 key 值(EditLink 會用到)
            npoGridView.Keys.Add("uid");


            //如果 GridView 要翻頁, 要設定 CurrentPage 的值
            if (npoGridView.ShowPage == true)
            {
                npoGridView.CurrentPage = Util.String2Number(HFD_CurrentPage.Value);
            }


            NPOGridViewColumn col;
            col = new NPOGridViewColumn("姓名"); //產生所需欄位及設定 Title
            col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
            col.ColumnName.Add("姓名"); //使用欄位名稱
            col.CellStyle = "width:100px";
            npoGridView.Columns.Add(col);
        
            col = new NPOGridViewColumn("備註"); //產生所需欄位及設定 Title
            col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
            col.ColumnName.Add("備註"); //使用欄位名稱
            col.Link = Util.RedirectByTime("ConsultEdit2.aspx", "ConsultingUID=");
            col.CellStyle = "width:200px";
            npoGridView.Columns.Add(col);
            //----------------------------------------------
            col = new NPOGridViewColumn("電話"); //產生所需欄位及設定 Title
            col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
            col.ColumnName.Add("電話"); //使用欄位名稱
            col.Link = Util.RedirectByTime("ConsultEdit2.aspx", "ConsultingUID=");
            col.CellStyle = "width:150px";
            npoGridView.Columns.Add(col);
            // lblGridList.Text = npoGridView.Render();
            //----------------------------------------------------------
            col = new NPOGridViewColumn("建檔時間"); //產生所需欄位及設定 Title
            col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
            col.ColumnName.Add("建檔時間"); //使用欄位名稱
            col.Link = Util.RedirectByTime("ConsultEdit2.aspx", "ConsultingUID=");
            col.CellStyle = "width:150px";
            npoGridView.Columns.Add(col);
                //----------------------------------------------------------
            col = new NPOGridViewColumn("來電諮詢別(大類)"); //產生所需欄位及設定 Title
            col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
            col.ColumnName.Add("來電諮詢別(大類)"); //使用欄位名稱
            col.Link = Util.RedirectByTime("ConsultEdit2.aspx", "ConsultingUID=");
            col.CellStyle = "width:200px";
            npoGridView.Columns.Add(col);
            //----------------------------------------------------------
            col = new NPOGridViewColumn("來電諮詢別(分項)"); //產生所需欄位及設定 Title
            col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
            col.ColumnName.Add("來電諮詢別(分項)"); //使用欄位名稱
            col.Link = Util.RedirectByTime("ConsultEdit2.aspx", "ConsultingUID=");
            col.CellStyle = "width:200px";
            npoGridView.Columns.Add(col);
            lblGridList.Text = npoGridView.Render();
        //----------------------------------------------------------
        //********************************************************************************************
    }
    //-------------------------------------------------------------------------------------------------------------
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
    }
    //-------------------------------------------------------------------------------------------------------------
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        LoadFormData();
    }
    //-------------------------------------------------------------------------------------------------------------
  
    private void LoadHiddenData()
    {
        List<ControlData> list = new List<ControlData>();
        list = new List<ControlData>();
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem1", "%經濟%", "經濟", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem2", "%情緒%", "情緒", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem3", "%信仰%", "信仰", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem4", "%人際%", "人際", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem5", "%婆媳%", "婆媳", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem6", "%翁媳%", "翁媳", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem7", "%妯娌%", "妯娌", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem8", "%手足%", "手足", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem9", "%職場%", "職場", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem10", "%教會%", "教會", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem11", "%溝通%", "溝通", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem12", "%性生活%", "性生活", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem13", "%精神疾病%", "精神疾病", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem14", "%身體疾病%", "身體疾病", true, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem15", "%個性%", "個性", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem16", "%外遇%", "外遇", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem17", "%暴力%", "暴力", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem18", "%教育%", "教育", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem19", "%霸凌%", "霸凌", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem20", "%學業%", "學業", false, HFD_ConsultantItem.Value));
        //lblConsultantItem.Text = HtmlUtil.RenderControl(list);
        //婚姻狀況
        list = new List<ControlData>();
        list.Add(new ControlData("Checkbox", "Marry", "CHK_Marry1", "%未婚%", "未婚", false, HFD_Marry.Value));
        list.Add(new ControlData("Checkbox", "Marry", "CHK_Marry2", "%已婚%", "已婚", false, HFD_Marry.Value));
        list.Add(new ControlData("Checkbox", "Marry", "CHK_Marry3", "%離婚%", "離婚", false, HFD_Marry.Value));
        list.Add(new ControlData("Checkbox", "Marry", "CHK_Marry4", "%喪偶%", "喪偶", false, HFD_Marry.Value));
        //lblMarry.Text = HtmlUtil.RenderControl(list);
    }

    private string GetRptHead()
    {
        string strTemp = "";
        HtmlTable table = new HtmlTable();
        HtmlTableRow row;
        HtmlTableCell cell;
        CssStyleCollection css;

        string FontSize = "14px";
        table.Border = 0;
        table.CellPadding = 0;
        table.CellSpacing = 0;
        table.Align = "center";
        css = table.Style;
        css.Add("font-size", "12px");
        css.Add("font-family", "標楷體");
        css.Add("width", "100%");
        //****************************************
        row = new HtmlTableRow();
        cell = new HtmlTableCell();
        //查詢城市代碼名稱
        DataTable dt = GetDataTable();
        foreach (DataRow dr in dt.Rows)
        {
        string city = dr["城市"].ToString();
        strTemp =  city+ "分布表";
        }

        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        //教會跨行置中
        cell.ColSpan = 7;
        css.Add("text-align", "center");
        css.Add("font-size", "24px");
        css.Add("font-family", "標楷體");
        css.Add("border-style", "none");
        css.Add("font-weight", "bold");
        row.Cells.Add(cell);
        table.Rows.Add(row);
        //*************************************
        //轉成 html 碼
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        table.RenderControl(htw);

        return htw.InnerWriter.ToString();
    }
   
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        lblRpt.Text = GetRptHead() + GetReport();
        string s = @"<head>
                        <style>
                        @page Section2
                        { size:841.7pt 595.45pt;
                          mso-page-orientation:landscape;
                        margin:1.5cm 0.5cm 0.5cm 0.5cm;}
                        div.Section2
                        {page:Section2;}
                        </style>
                    </head>
                    <body>
                        <div class=Section2>
                    ";//邊界-上右下左
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(s);
        Util.OutputTxt(sb + lblRpt.Text, "1", DateTime.Now.ToString("yyyyMMddHHmmss"));
    }
    private string GetReport()
    {
        string[] Items = {
                        "姓名",
                        "備註",
                        "電話",
                        "建檔時間",
                        "城市",
                        "來電諮詢別(大類)",
                        "來電諮詢別(分項)",
                   };
        //組 table
        string strTemp = "";
        HtmlTable table = new HtmlTable();
        HtmlTableRow row;
        HtmlTableCell cell;
        CssStyleCollection css;

        table.Border = 0;
        table.BorderColor = "black";
        table.CellPadding = 0;
        table.CellSpacing = 0;
        table.Align = "center";
        css = table.Style;
        css.Add("font-size", "12pt");
        css.Add("font-family", "標楷體");
        css.Add("width", "800px");

        //第1行
        row = new HtmlTableRow();

        for (int i = 0; i < Items.Length; i++)
        {

            cell = new HtmlTableCell();
            strTemp = Items[i].ToString();
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "25px");  //抬頭字型大小
            if (i == 0)
            {
                Util.AddTDLine(css, 123);
            }
            if (i == 1)
            {
                Util.AddTDLine(css, 23);
            }
            if (i == 2)
            {
                Util.AddTDLine(css, 23);
            }
            if (i == 3)
            {
                Util.AddTDLine(css, 23);
            }
            if (i == 4)
            {
                Util.AddTDLine(css, 23);
            }
            if (i == 5)
            {
                Util.AddTDLine(css, 23);
            }
            if (i == 6)
            {
                Util.AddTDLine(css, 23);
            }
            if (i == 7)
            {
                Util.AddTDLine(css, 23);
            }
            if (i == 8)
            {
                Util.AddTDLine(css, 23);
            }
            if (i == 9)
            {
                Util.AddTDLine(css, 23);
            }


            row.Cells.Add(cell);

        }

        cell = new HtmlTableCell();
        string strTemp1 = "";
        cell.InnerHtml = strTemp1;
        css = cell.Style;
        css.Add("text-align", "left");
        css.Add("font-size", "12px");
        cell.Width = "8";
        row.Cells.Add(cell);
        table.Rows.Add(row);
        //--------------------------------------------
        //第2行,資料顯示
        //*********第一排*********
        string strFontSize = "18px";
        DataTable dt = GetDataTable();

        foreach (DataRow dr in dt.Rows)
        {
            row = new HtmlTableRow();

            cell = new HtmlTableCell();
            css = cell.Style;
            cell.InnerHtml = dr["姓名"].ToString();     //從GetDataTable()抓Name值
            css.Add("text-align", "center");            //excel文字置中
            css.Add("font-size", strFontSize);          //excel文字大小
            Util.AddTDLine(css, 1234);                  //excel 一個儲存格左(1)上(2)右(3)下(4) 框線
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            cell.InnerHtml = dr["備註"].ToString();
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 1234);
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            cell.InnerHtml = dr["電話"].ToString();
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 1234);
            row.Cells.Add(cell);


            cell = new HtmlTableCell();
            css = cell.Style;
            cell.InnerHtml = dr["建檔時間"].ToString();
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 1234);
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            cell.InnerHtml = dr["城市"].ToString();
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 1234);
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            cell.InnerHtml = dr["來電諮詢別(大類)"].ToString();
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 1234);
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            cell.InnerHtml = dr["來電諮詢別(分項)"].ToString();
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 1234);
            row.Cells.Add(cell);

            table.Rows.Add(row);
        }

        //轉成 html 碼
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        table.RenderControl(htw);
        return htw.InnerWriter.ToString();
    }
    private DataTable GetDataTable()
    {

        //讀取資料庫
        string strSql = @"
                        select b.uid , --count(*) as Cnt  , 
                        CName As  姓名,b.Memo as 備註,Phone As 電話, convert(varchar(19), CreateDate,120) as 建檔時間,
                        REVERSE(SUBSTRING(REVERSE(ConsultantMain),CHARINDEX(',',REVERSE(ConsultantMain))+1,LEN(ConsultantMain))) [來電諮詢別(大類)],
                        REVERSE(SUBSTRING(REVERSE(ConsultantItem),CHARINDEX(',',REVERSE(ConsultantItem))+1,LEN(ConsultantItem))) [來電諮詢別(分項)]
                        from Consulting a  inner join Member b on a.MemberUID = b.uid 
                        where 1=1 
                        and b.uid in
                        (
	                        select  memberuid from Consulting where 1=1 and isnull(IsDelete, '') != 'Y' 
                            and CONVERT(varchar(10), CreateDate, 111)  Between @BegCreateDate And @EndCreateDate
	                        group by memberuid  having  count(memberuid)=1 
                        )
                        and isnull(a.IsDelete, '') != 'Y'
                    ";
        if (SessionInfo.GroupName == "系統管理員")
        {
        }
        else
        {
            strSql += " and ServiceUser = @ServiceUser\n";
        }
        //if (txtPhone.Text.Trim() != "")
        //{
        //    strSql += " and Phone like @Phone\n";
        //}

        //if (txtName.Text.Trim() != "")
        //{
        //    strSql += " and CName like @CName\n";
        //}

        //if (txtBegCreateDate.Text.Trim() != "" && txtEndCreateDate.Text.Trim() != "")
        //{
        //    strSql += " and CONVERT(varchar(100), CreateDate, 111) Between @BegCreateDate And @EndCreateDate\n";
        //}
        strSql += " group by b.uid ,CName,b.Memo,Phone,CreateDate,ConsultantMain,ConsultantItem";
        //strSql += " having  count(b.uid)=1";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        //dict.Add("CName", "%" + txtName.Text.Trim() + "%");
        //dict.Add("Phone", '%' + txtPhone.Text.Trim() + '%');
        //dict.Add("BegCreateDate", txtBegCreateDate.Text.Trim());
        //dict.Add("EndCreateDate", txtEndCreateDate.Text.Trim());
        dict.Add("ServiceUser", SessionInfo.UserID.ToString());
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        return dt;
    }
    protected void btnAllQuery_Click(object sender, EventArgs e)
    {
        lblAllyear.Text = GetRptHead1() + GetRptHead2() + GetRptHead3() + GetRptHead4() + GetRptHeadAll();
    }

    //----專線來電年度統計第一季(查詢SQL)
    private string getYear1(string ConsultantItem, string year)
    {

        //讀取資料庫
        string strSql = @" 
                            select count(*) as sum
                            from consulting
                            where CONVERT(varchar(4), CreateDate, 111) = @CreateDate
                            and ConsultantItem like @ConsultantItem
                            and CONVERT(varchar(7), CreateDate, 111) Between '" + year + "/01' and '" + year + "/03'"
                          ;
        DataRow dr = null;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("ConsultantItem", ConsultantItem);
        dict.Add("CreateDate", year);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count == 0)
        {
            return "0";
        }
        dr = dt.Rows[0];

        return dr["sum"].ToString();
    }
    //----專線來電年度統計第一季
    private string GetRptHead1()
    {
        //-------------------------------------------------------------
        int[] year = new int[5];
        year[0] = int.Parse(getYear1("情緒", ddlYear.SelectedValue.ToString()));
        year[1] = int.Parse(getYear1("經濟", ddlYear.SelectedValue.ToString()));
        year[2] = int.Parse(getYear1("信仰", ddlYear.SelectedValue.ToString()));
        year[3] = int.Parse(getYear1("身體疾病", ddlYear.SelectedValue.ToString()));
        year[4] = int.Parse(getYear1("性生活", ddlYear.SelectedValue.ToString()));
        int sumyear1 = year[0] + year[1] + year[2] + year[3] + year[4];

        //---------------------------------------------------------------
        string strTemp = "";
        HtmlTable table = new HtmlTable();
        HtmlTableRow row;
        HtmlTableCell cell;
        CssStyleCollection css;
        string strColor = "BLACK"; 
        string FontSize = "14px";
        table.Border = 0;
        table.CellPadding = 0;
        table.CellSpacing = 0;
        table.Align = "center";
        css = table.Style;
        css.Add("font-size", "12px");
        css.Add("font-family", "標楷體");
        css.Add("width", "100%");
        //****************************************
        //----------------------------------- 標題
        row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "關懷專線來電"+ddlYear.SelectedValue+"年年度統計";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "24px");
        css.Add("font-family", "標楷體");
        css.Add("border-style", "none");
        //Util.AddTDLine(css, 1234, strColor);
        css.Add("font-weight", "bold");
        cell.ColSpan = 8;
        row.Cells.Add(cell);
        table.Rows.Add(row);
        //----------------------------------- 1
        //row = new HtmlTableRow();
        //cell = new HtmlTableCell();
        //strTemp = "1. 第一季志工總值班人次：___人次(值班人數X每天3班次X每周五天)";
        //cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        //css = cell.Style;
        //css.Add("text-align", "left");
        //css.Add("font-size", "18px");
        //css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        //Util.AddTDLine(css, 1234, strColor);
        ////css.Add("font-weight", "bold");
        //cell.ColSpan = 8;
        //row.Cells.Add(cell);
        //table.Rows.Add(row);
        ////----------------------------------- 2
        //row = new HtmlTableRow();
        //cell = new HtmlTableCell();
        //strTemp = "2. 第一季值班總班數：__周*_天*_班　　共___班次";
        //cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        //css = cell.Style;
        //css.Add("text-align", "left");
        //css.Add("font-size", "18px");
        //css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        //Util.AddTDLine(css, 134, strColor);
        ////css.Add("font-weight", "bold");
        //cell.ColSpan = 8;
        //row.Cells.Add(cell);
        //table.Rows.Add(row);
        //----------------------------------- 3
        row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "第一季總計來電數為" + sumyear1 + "通";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "left");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        Util.AddTDLine(css, 1234, strColor);
        css.Add("width", "400px");
        //css.Add("font-weight", "bold");
        cell.ColSpan = 1;
        cell.RowSpan = 3;
        row.Cells.Add(cell);

        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "諮商類別:A婚姻B親屬C情緒D經濟E信仰F身心疾病G性生活";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "left");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        css.Add("border-style", "none");
        Util.AddTDLine(css, 234, strColor);
        //css.Add("font-weight", "bold");
        cell.ColSpan = 7;
        row.Cells.Add(cell);
        table.Rows.Add(row);

        //----------------------------------- 4.1
        row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "A";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 4.2
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "B";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 4.3
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "C";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 4.4
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "D";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 4.5
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "E";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 4.6
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "F";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 4.7
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "G";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        table.Rows.Add(row);

        //----------------------------------- 5.1
        row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "A";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 5.2
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "B";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 5.3
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = year[0].ToString();
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 5.4
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = year[1].ToString();
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 5.5
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = year[2].ToString();
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 5.6
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = year[3].ToString();
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 5.7
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = year[4].ToString();
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        table.Rows.Add(row);

        //*************************************
        //轉成 html 碼
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        table.RenderControl(htw);
        return htw.InnerWriter.ToString();
    }
    //----專線來電年度統計第二季(查詢SQL)
    private string getYear2(string ConsultantItem, string year)
    {

        //讀取資料庫
        string strSql = @" 
                            select count(*) as sum
                            from consulting
                            where CONVERT(varchar(4), CreateDate, 111) = @CreateDate
                            and ConsultantItem like @ConsultantItem
                            and CONVERT(varchar(7), CreateDate, 111) Between '" + year + "/04' and '" + year + "/06'"
                          ;
        DataRow dr = null;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("ConsultantItem", ConsultantItem);
        dict.Add("CreateDate", year);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count == 0)
        {
            return "0";
        }
        dr = dt.Rows[0];

        return dr["sum"].ToString();
    }
    //----專線來電年度統計第二季
    private string GetRptHead2()
    {
        //-------------------------------------------------------------
        int[] year = new int[5];
        year[0] = int.Parse(getYear2("情緒", ddlYear.SelectedValue.ToString()));
        year[1] = int.Parse(getYear2("經濟", ddlYear.SelectedValue.ToString()));
        year[2] = int.Parse(getYear2("信仰", ddlYear.SelectedValue.ToString()));
        year[3] = int.Parse(getYear2("身體疾病", ddlYear.SelectedValue.ToString()));
        year[4] = int.Parse(getYear2("性生活", ddlYear.SelectedValue.ToString()));
        int sumyear1 = year[0] + year[1] + year[2] + year[3] + year[4];

        //---------------------------------------------------------------
        string strTemp = "";
        HtmlTable table = new HtmlTable();
        HtmlTableRow row;
        HtmlTableCell cell;
        CssStyleCollection css;
        string strColor = "BLACK";
        string FontSize = "14px";
        table.Border = 0;
        table.CellPadding = 0;
        table.CellSpacing = 0;
        table.Align = "center";
        css = table.Style;
        css.Add("font-size", "12px");
        css.Add("font-family", "標楷體");
        css.Add("width", "100%");
        //****************************************
        ////----------------------------------- 1
        //row = new HtmlTableRow();
        //cell = new HtmlTableCell();
        //strTemp = "1. 第二季志工總值班人次：___人次(值班人數X每天3班次X每周五天)";
        //cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        //css = cell.Style;
        //css.Add("text-align", "left");
        //css.Add("font-size", "18px");
        //css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        //Util.AddTDLine(css, 1234, strColor);
        ////css.Add("font-weight", "bold");
        //cell.ColSpan = 8;
        //row.Cells.Add(cell);
        //table.Rows.Add(row);
        ////----------------------------------- 2
        //row = new HtmlTableRow();
        //cell = new HtmlTableCell();
        //strTemp = "2. 第二季值班總班數：__周*_天*_班　　共___班次";
        //cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        //css = cell.Style;
        //css.Add("text-align", "left");
        //css.Add("font-size", "18px");
        //css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        //Util.AddTDLine(css, 134, strColor);
        ////css.Add("font-weight", "bold");
        //cell.ColSpan = 8;
        //row.Cells.Add(cell);
        //table.Rows.Add(row);
        //----------------------------------- 3
        row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "第二季總計來電數為" + sumyear1 + "通";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "left");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        Util.AddTDLine(css, 134, strColor);
        css.Add("width", "400px");
        //css.Add("font-weight", "bold");
        cell.ColSpan = 1;
        cell.RowSpan = 3;
        row.Cells.Add(cell);

        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "諮商類別:A婚姻B親屬C情緒D經濟E信仰F身心疾病G性生活";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "left");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        css.Add("border-style", "none");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        cell.ColSpan = 7;
        row.Cells.Add(cell);
        table.Rows.Add(row);

        //----------------------------------- 4.1
        row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "A";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 4.2
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "B";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 4.3
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "C";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 4.4
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "D";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 4.5
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "E";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 4.6
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "F";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 4.7
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "G";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        table.Rows.Add(row);

        //----------------------------------- 5.1
        row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "A";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 5.2
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "B";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 5.3
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = year[0].ToString();
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 5.4
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = year[1].ToString();
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 5.5
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = year[2].ToString();
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 5.6
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = year[3].ToString();
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 5.7
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = year[4].ToString();
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        table.Rows.Add(row);

        //*************************************
        //轉成 html 碼
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        table.RenderControl(htw);
        return htw.InnerWriter.ToString();
    }
    //----專線來電年度統計第三季(查詢SQL)
    private string getYear3(string ConsultantItem, string year)
    {

        //讀取資料庫
        string strSql = @" 
                            select count(*) as sum
                            from consulting
                            where CONVERT(varchar(4), CreateDate, 111) = @CreateDate
                            and ConsultantItem like @ConsultantItem
                            and CONVERT(varchar(7), CreateDate, 111) Between '" + year + "/07' and '" + year + "/09'"
                          ;
        DataRow dr = null;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("ConsultantItem", ConsultantItem);
        dict.Add("CreateDate", year);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count == 0)
        {
            return "0";
        }
        dr = dt.Rows[0];

        return dr["sum"].ToString();
    }
    //----專線來電年度統計第三季
    private string GetRptHead3()
    {
        //-------------------------------------------------------------
        int[] year = new int[5];
        year[0] = int.Parse(getYear3("情緒", ddlYear.SelectedValue.ToString()));
        year[1] = int.Parse(getYear3("經濟", ddlYear.SelectedValue.ToString()));
        year[2] = int.Parse(getYear3("信仰", ddlYear.SelectedValue.ToString()));
        year[3] = int.Parse(getYear3("身體疾病", ddlYear.SelectedValue.ToString()));
        year[4] = int.Parse(getYear3("性生活", ddlYear.SelectedValue.ToString()));
        int sumyear1 = year[0] + year[1] + year[2] + year[3] + year[4];

        //---------------------------------------------------------------
        string strTemp = "";
        HtmlTable table = new HtmlTable();
        HtmlTableRow row;
        HtmlTableCell cell;
        CssStyleCollection css;
        string strColor = "BLACK";
        string FontSize = "14px";
        table.Border = 0;
        table.CellPadding = 0;
        table.CellSpacing = 0;
        table.Align = "center";
        css = table.Style;
        css.Add("font-size", "12px");
        css.Add("font-family", "標楷體");
        css.Add("width", "100%");
        //****************************************
        ////----------------------------------- 1
        //row = new HtmlTableRow();
        //cell = new HtmlTableCell();
        //strTemp = "1. 第三季志工總值班人次：___人次(值班人數X每天3班次X每周五天)";
        //cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        //css = cell.Style;
        //css.Add("text-align", "left");
        //css.Add("font-size", "18px");
        //css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        //Util.AddTDLine(css, 1234, strColor);
        ////css.Add("font-weight", "bold");
        //cell.ColSpan = 8;
        //row.Cells.Add(cell);
        //table.Rows.Add(row);
        ////----------------------------------- 2
        //row = new HtmlTableRow();
        //cell = new HtmlTableCell();
        //strTemp = "2. 第三季值班總班數：__周*_天*_班　　共___班次";
        //cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        //css = cell.Style;
        //css.Add("text-align", "left");
        //css.Add("font-size", "18px");
        //css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        //Util.AddTDLine(css, 134, strColor);
        ////css.Add("font-weight", "bold");
        //cell.ColSpan = 8;
        //row.Cells.Add(cell);
        //table.Rows.Add(row);
        //----------------------------------- 3
        row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "第三季總計來電數為" + sumyear1 + "通";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "left");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        Util.AddTDLine(css, 134, strColor);
        css.Add("width", "400px");
        //css.Add("font-weight", "bold");
        cell.ColSpan = 1;
        cell.RowSpan = 3;
        row.Cells.Add(cell);

        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "諮商類別:A婚姻B親屬C情緒D經濟E信仰F身心疾病G性生活";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "left");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        css.Add("border-style", "none");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        cell.ColSpan = 7;
        row.Cells.Add(cell);
        table.Rows.Add(row);

        //----------------------------------- 4.1
        row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "A";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 4.2
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "B";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 4.3
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "C";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 4.4
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "D";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 4.5
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "E";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 4.6
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "F";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 4.7
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "G";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        table.Rows.Add(row);

        //----------------------------------- 5.1
        row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "A";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 5.2
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "B";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 5.3
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = year[0].ToString();
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 5.4
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = year[1].ToString();
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 5.5
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = year[2].ToString();
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 5.6
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = year[3].ToString();
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 5.7
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = year[4].ToString();
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        table.Rows.Add(row);

        //*************************************
        //轉成 html 碼
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        table.RenderControl(htw);
        return htw.InnerWriter.ToString();
    }
    //----專線來電年度統計第四季(查詢SQL)
    private string getYear4(string ConsultantItem, string year)
    {

        //讀取資料庫
        string strSql = @" 
                            select count(*) as sum
                            from consulting
                            where CONVERT(varchar(4), CreateDate, 111) = @CreateDate
                            and ConsultantItem like @ConsultantItem
                            and CONVERT(varchar(7), CreateDate, 111) Between '" + year + "/10' and '" + year + "/12'"
                          ;
        DataRow dr = null;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("ConsultantItem", ConsultantItem);
        dict.Add("CreateDate", year);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count == 0)
        {
            return "0";
        }
        dr = dt.Rows[0];

        return dr["sum"].ToString();
    }
    //----專線來電年度統計第四季
    private string GetRptHead4()
    {
        //-------------------------------------------------------------
        int[] year = new int[5];
        year[0] = int.Parse(getYear4("情緒", ddlYear.SelectedValue.ToString()));
        year[1] = int.Parse(getYear4("經濟", ddlYear.SelectedValue.ToString()));
        year[2] = int.Parse(getYear4("信仰", ddlYear.SelectedValue.ToString()));
        year[3] = int.Parse(getYear4("身體疾病", ddlYear.SelectedValue.ToString()));
        year[4] = int.Parse(getYear4("性生活", ddlYear.SelectedValue.ToString()));
        int sumyear1 = year[0] + year[1] + year[2] + year[3] + year[4];

        //---------------------------------------------------------------
        string strTemp = "";
        HtmlTable table = new HtmlTable();
        HtmlTableRow row;
        HtmlTableCell cell;
        CssStyleCollection css;
        string strColor = "BLACK";
        string FontSize = "14px";
        table.Border = 0;
        table.CellPadding = 0;
        table.CellSpacing = 0;
        table.Align = "center";
        css = table.Style;
        css.Add("font-size", "12px");
        css.Add("font-family", "標楷體");
        css.Add("width", "100%");
        //****************************************
        //----------------------------------- 1
        //row = new HtmlTableRow();
        //cell = new HtmlTableCell();
        //strTemp = "1. 第四季志工總值班人次：___人次(值班人數X每天3班次X每周五天)";
        //cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        //css = cell.Style;
        //css.Add("text-align", "left");
        //css.Add("font-size", "18px");
        //css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        //Util.AddTDLine(css, 1234, strColor);
        ////css.Add("font-weight", "bold");
        //cell.ColSpan = 8;
        //row.Cells.Add(cell);
        //table.Rows.Add(row);
        ////----------------------------------- 2
        //row = new HtmlTableRow();
        //cell = new HtmlTableCell();
        //strTemp = "2. 第四季值班總班數：__周*_天*_班　　共___班次";
        //cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        //css = cell.Style;
        //css.Add("text-align", "left");
        //css.Add("font-size", "18px");
        //css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        //Util.AddTDLine(css, 134, strColor);
        ////css.Add("font-weight", "bold");
        //cell.ColSpan = 8;
        //row.Cells.Add(cell);
        //table.Rows.Add(row);
        //----------------------------------- 3
        row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "第四季總計來電數為" + sumyear1 + "通";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "left");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        Util.AddTDLine(css, 134, strColor);
        css.Add("width", "400px");
        //css.Add("font-weight", "bold");
        cell.ColSpan = 1;
        cell.RowSpan = 3;
        row.Cells.Add(cell);

        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "諮商類別:A婚姻B親屬C情緒D經濟E信仰F身心疾病G性生活";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "left");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        css.Add("border-style", "none");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        cell.ColSpan = 7;
        row.Cells.Add(cell);
        table.Rows.Add(row);

        //----------------------------------- 4.1
        row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "A";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 4.2
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "B";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 4.3
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "C";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 4.4
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "D";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 4.5
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "E";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 4.6
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "F";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 4.7
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "G";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        table.Rows.Add(row);

        //----------------------------------- 5.1
        row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "A";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 5.2
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "B";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 5.3
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = year[0].ToString();
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 5.4
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = year[1].ToString();
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 5.5
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = year[2].ToString();
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 5.6
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = year[3].ToString();
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 5.7
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = year[4].ToString();
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        table.Rows.Add(row);

        //*************************************
        //轉成 html 碼
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        table.RenderControl(htw);
        return htw.InnerWriter.ToString();
    }
    //----專線來電年度統計全季(查詢SQL)
    private string getYearAll(string ConsultantItem, string year)
    {

        //讀取資料庫
        string strSql = @" 
                            select count(*) as sum
                            from consulting
                            where CONVERT(varchar(4), CreateDate, 111) = @CreateDate
                            and ConsultantItem like @ConsultantItem
                           -- and CONVERT(varchar(7), CreateDate, 111) Between '" + year + "/10' and '" + year + "/12'"
                          ;
        DataRow dr = null;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("ConsultantItem", ConsultantItem);
        dict.Add("CreateDate", year);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count == 0)
        {
            return "0";
        }
        dr = dt.Rows[0];

        return dr["sum"].ToString();
    }
    //----專線來電年度統計全季
    private string GetRptHeadAll()
    {
        //-------------------------------------------------------------
        int[] year = new int[5];
        year[0] = int.Parse(getYearAll("情緒", ddlYear.SelectedValue.ToString()));
        year[1] = int.Parse(getYearAll("經濟", ddlYear.SelectedValue.ToString()));
        year[2] = int.Parse(getYearAll("信仰", ddlYear.SelectedValue.ToString()));
        year[3] = int.Parse(getYearAll("身體疾病", ddlYear.SelectedValue.ToString()));
        year[4] = int.Parse(getYearAll("性生活", ddlYear.SelectedValue.ToString()));
        int sumyear1 = year[0] + year[1] + year[2] + year[3] + year[4];

        //---------------------------------------------------------------
        string strTemp = "";
        HtmlTable table = new HtmlTable();
        HtmlTableRow row;
        HtmlTableCell cell;
        CssStyleCollection css;
        string strColor = "BLACK";
        string FontSize = "14px";
        table.Border = 0;
        table.CellPadding = 0;
        table.CellSpacing = 0;
        table.Align = "center";
        css = table.Style;
        css.Add("font-size", "12px");
        css.Add("font-family", "標楷體");
        css.Add("width", "100%");
        //****************************************
        //----------------------------------- 1
        row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "總計來電數為" + sumyear1 + "通";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        Util.AddTDLine(css, 134, strColor);
        css.Add("width", "400px");
        //css.Add("font-weight", "bold");
        cell.ColSpan = 1;
        cell.RowSpan = 3;
        row.Cells.Add(cell);

        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "諮商類別:A婚姻B親屬C情緒D經濟E信仰F身心疾病G性生活";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "left");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        css.Add("border-style", "none");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        cell.ColSpan = 7;
        row.Cells.Add(cell);
        table.Rows.Add(row);

        //----------------------------------- 4.1
        row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "A";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 4.2
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "B";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 4.3
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "C";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 4.4
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "D";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 4.5
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "E";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 4.6
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "F";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 4.7
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "G";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        table.Rows.Add(row);

        //----------------------------------- 5.1
        row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "A";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 5.2
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "B";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 5.3
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = year[0].ToString();
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 5.4
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = year[1].ToString();
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 5.5
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = year[2].ToString();
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 5.6
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = year[3].ToString();
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        //----------------------------------- 5.7
        //row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = year[4].ToString();
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "18px");
        css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        css.Add("width", "100px");
        Util.AddTDLine(css, 34, strColor);
        //css.Add("font-weight", "bold");
        //cell.ColSpan = 7;
        row.Cells.Add(cell);
        table.Rows.Add(row);

        //*************************************
        //轉成 html 碼
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        table.RenderControl(htw);
        return htw.InnerWriter.ToString();
    }
}
