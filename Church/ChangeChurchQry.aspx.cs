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


public partial class Church_ChangeChurchQry : BasePage
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
        //設定一些按鍵的 Visible 屬性
        SetButton();
        Util.ShowSysMsgWithScript("$('#btnPreviousPage').hide();$('#btnNextPage').hide();$('#btnGoPage').hide();");
        if (!IsPostBack)
        {
            //載入下拉式選單資料
            LoadDropDownListData();
            
        }
        //載入資料
        LoadFormData();

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
        btnAdd.Visible = false;//ga.AddNew;
        btnQuery.Visible = ga.Query;
    }
    //-------------------------------------------------------------------------
    public void LoadDropDownListData()
    {
        //ddlChangeChurchSta.Items.Clear();
        ////ddlChangeChurchSta.Items.Add(new ListItem("", ""));
        //ddlChangeChurchSta.Items.Add(new ListItem("已處理", "已處理"));
        //ddlChangeChurchSta.Items.Add(new ListItem("未處理", "未處理"));

        //Util.FillCityData(ddlCity);
        //Util.FillAreaData(ddlArea, ddlCity.SelectedValue);
    }
    //-------------------------------------------------------------------------------------------------------------
    private void LoadFormData()
    {
        string strSql = @"
                         Select  a.uid as uid ,CName as 姓名,
	                  
	                      CASE When ChangeChurchSta  IS NULL THEN '未處理' ELSE '已處理' END AS 處理狀態
	                     , convert(nvarchar(10),  a.changeChurchDate,111) as 轉介日期
                          From Member a 
	                      left outer join Church b on a.ChurchUid=b.uid 
                          Where 1=1 
                          And ChurchYN = '是'
                          And isnull(a.IsDelete, '') != 'Y'
                          And isnull(changechurchsta,'') != '已處理'
                        ";

        if (txtName.Text != "")
        {
            strSql += " and CName like @CName\n";
        }
        if (txtchangeChurchDate.Text != "")
        {
            strSql += " and changeChurchDate = @changeChurchDate\n";
        }
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("CName", "%" + txtName .Text  + "%");
        dict.Add("changeChurchDate", txtchangeChurchDate.Text);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        int count = dt.Rows.Count;

        if (dt.Rows.Count == 0)
        {
            //資料異常
            SetSysMsg("查無資料");
           // Response.Redirect("ChangeChurchQry.aspx");
            return;
        }
        DataRow dr = null;
        dr = dt.Rows[0];
        //Grid initial
        //沒有需要特別處理的欄位時
        NPOGridView npoGridView = new NPOGridView();
        npoGridView.Source = NPOGridViewDataSource.fromDataTable;
        npoGridView.dataTable = dt;
        npoGridView.Keys.Add("uid");
        npoGridView.DisableColumn.Add("uid");
        npoGridView.CurrentPage = Util.String2Number(HFD_CurrentPage.Value);

        npoGridView.EditLinkTarget = "轉介教會資料編輯','help=no,status=yes,resizable=yes,scrollbars=yes,center=yes,width=900px,height=500px";
        //npoGridView.EditLink = Util.RedirectByTime("ChangeChurchEdit.aspx", "Mode=MOD&MoveMainUid=" + Uid + "&uid=");

            npoGridView.EditLink = Util.RedirectByTime("ChangeChurchEdit.aspx", "uid=");

        lblGridList.Text = npoGridView.Render();
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
        strTemp = "好消息家庭關懷專線個案轉介&nbsp;";// "" + ddlYear.SelectedValue + "年熱線志工班表  第 " + ddlQuerter.SelectedValue + " 季 ";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "24px");
        css.Add("font-family", "標楷體");
        css.Add("border-style", "none");
        css.Add("font-weight", "bold");
        cell.ColSpan = 8;
        row.Cells.Add(cell);
        table.Rows.Add(row);
        //*************************************
        //轉成 html 碼
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        table.RenderControl(htw);
        return htw.InnerWriter.ToString();
    }
    //----------------------------------------------------------------------------------------------------------
    private DataTable GetDataTable()
    {
        string strSql = @"
                  select top 1 ISNULL((SELECT [Name] FROM CodeCity WHERE CodeCity.Zipcode = City),'')  + ISNULL((SELECT Name FROM CodeCity WHERE CodeCity.zipcode = b.ZipCode),'') + [address] as FullAddress ,b.Memo as M_Memo,*
                        from Consulting a  
                        inner join Member b on a.MemberUID = b.uid
                        --left join CodeCity c on b.City = c.ZipCode 
                        --left join CodeCity d on b.ZipCode = d.ZipCode     
                  where 1=1 and isnull(a.IsDelete, '') != 'Y' ";
        strSql += "  And a.MemberUID = 825";



        strSql += " Order by  CreateDate Desc";

        Dictionary<string, object> dict = new Dictionary<string, object>();

        //dict.Add("MemberUID", HFD_MemberID.Value.ToString());

        DataTable dt = NpoDB.GetDataTableS(strSql, dict);


        return dt;

    }

    // 報表************************************************************************************************************************
    private string GetReport()
    {
        HtmlTable table = new HtmlTable();
        HtmlTableRow row;
        HtmlTableCell cell;
        CssStyleCollection css;
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);

        DataTable dt = GetDataTable();

        if (dt.Rows.Count == 0)
        {
            lblReport.Text = "";
            ShowSysMsg("查無資料");
            return "";
        }

        foreach (DataRow dr in dt.Rows)
        {
            string strFontSize = "14px";

            table = new HtmlTable();
            table.Border = 1;
            table.BorderColor = "black";
            table.CellPadding = 0;
            table.CellSpacing = 0;
            table.Align = "center";
            css = table.Style;
            css.Add("font-size", strFontSize);
            css.Add("font-family", "標楷體");
            css.Add("width", "900px");


            string strColor = "lightgrey";  //字的顏色
            string strBackGround = "darkseagreen"; //cell 顏色
            string strMarkColor = "'background-color:'''"; // 字的底色
            string strMemberColor = "color:red"; //聯絡人的字顏色
            string strWeight = ""; //字體
            string strHeight = "20px";//欄高


            //------------------------------------------------
            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("font-weight", strWeight);
            css.Add("height", "20px");
            Util.AddTDLine(css, 123, strColor);
            cell.InnerHtml = "<span style= " + strMemberColor + " >103 年 &nbsp; 月 &nbsp; 日</span>";
            row.Cells.Add(cell);
            //---------------------------------------------------
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("font-weight", strWeight);
            css.Add("height", "20px");
            Util.AddTDLine(css, 123, strColor);
            cell.InnerHtml = "<span style= " + strMemberColor + " >轉介教會：&nbsp</span>";
            row.Cells.Add(cell);
            table.Rows.Add(row);
            //----------------------------------------------------
            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("font-weight", strWeight);
            css.Add("height", "20px");
            Util.AddTDLine(css, 123, strColor);
            cell.InnerHtml = "<span style= " + strMemberColor + " >電話：" + dr["Phone"].ToString() + "</span>";// font-weight:bold;
            row.Cells.Add(cell);
            //---------------------------------------------------
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            // css.Add("font-weight", strWeight)
            css.Add("height", strHeight); ;
            Util.AddTDLine(css, 23, strColor);
            cell.InnerHtml = "<span style=" + strMemberColor + ">來電諮詢別(大類)：" + Util.TrimLastChar(dr["ConsultantMain"].ToString(), ',') + "&nbsp;</span>";
            row.Cells.Add(cell);
            table.Rows.Add(row);
            //------------------------------------------------------------------

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("font-weight", strWeight);
            css.Add("height", "25px");
            Util.AddTDLine(css, 123, strColor);
            cell.InnerHtml = "<span style=" + strMemberColor + " >姓名：" + dr["Cname"].ToString() + "</span> ";
            row.Cells.Add(cell);
            //table.Rows.Add(row);

            //------------------------------------------------------------------

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            // css.Add("font-weight", strWeight);
            css.Add("height", strHeight);
            Util.AddTDLine(css, 23, strColor);
            cell.RowSpan = 3;
            cell.InnerHtml = "<span style=" + strMemberColor + " >來電諮詢別(分項)：" + Util.TrimLastChar(dr["ConsultantItem"].ToString(), ',') + "&nbsp;</span>";
            row.Cells.Add(cell);
            table.Rows.Add(row);
            //-------------------------------------------------

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("font-weight", strWeight);
            css.Add("height", "30px");
            Util.AddTDLine(css, 123, strColor);
            cell.InnerHtml = "<span style=" + strMemberColor + " >性別：" + dr["Sex"].ToString() + "</span>";
            row.Cells.Add(cell);
            table.Rows.Add(row);
            //------------------------------------------------------------------
            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("font-weight", strWeight);
            Util.AddTDLine(css, 23, strColor);
            cell.InnerHtml = "<span style=" + strMemberColor + " >婚姻：" + Util.TrimLastChar(dr["Marry"].ToString(), ',') + "</span>&nbsp;";
            row.Cells.Add(cell);
            table.Rows.Add(row);
            //------------------------------------------------------------------

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("font-weight", strWeight);
            Util.AddTDLine(css, 23, strColor);
            //cell.RowSpan = 2;
            cell.ColSpan = 2;
            if (dr["FullAddress"].ToString() != "")
            {
                cell.InnerHtml = "<span style=" + strMemberColor + " >8.地址：" + dr["FullAddress"].ToString() + "</span> ";
            }
            else
            {
                cell.InnerHtml = "<span style=" + strMemberColor + " >8.地址：" + dr["Overseas"].ToString().TrimEnd(',') + "</span> ";
            }
            row.Cells.Add(cell);
            table.Rows.Add(row);

            table.RenderControl(htw);


            table = new HtmlTable();
            table.Border = 1;
            table.BorderColor = "black";
            table.CellPadding = 0;
            table.CellSpacing = 0;
            table.Align = "center";
            css = table.Style;
            css.Add("font-size", strFontSize);
            css.Add("font-family", "標楷體");
            css.Add("width", "900px");



            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            css.Add("background", strBackGround);
            Util.AddTDLine(css, 23, strColor);
            cell.InnerHtml = "<span style=font-weight:bold>求助者的主訴(用第一人稱 '我' 敘述)</span>";
            cell.ColSpan = 3;
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            //css.Add("font-weight", strWeight);
            css.Add("height", strHeight);
            Util.AddTDLine(css, 23, strColor);
            cell.ColSpan = 3;
            cell.InnerHtml = "<span style=font-weight:bold>A(事件)：</span>" + dr["Event"].ToString() + "&nbsp;";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("height", strHeight);
            // css.Add("font-weight", strWeight);
            Util.AddTDLine(css, 23, strColor);
            cell.InnerHtml = "<span style=font-weight:bold>B(想法)：</span>" + dr["Think"].ToString() + "&nbsp;";
            cell.ColSpan = 3;
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("height", strHeight);
            //  css.Add("font-weight", strWeight);
            Util.AddTDLine(css, 23, strColor);
            cell.InnerHtml = "<span style=font-weight:bold>C(感受)：</span>" + dr["Feel"].ToString() + "&nbsp;";
            cell.ColSpan = 3;
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("height", strHeight);
            //css.Add("font-weight", strWeight);
            Util.AddTDLine(css, 23, strColor);
            cell.ColSpan = 3;
            cell.InnerHtml = "<span style=font-weight:bold>補充說明：</span>" + dr["Comment"].ToString() + "&nbsp;";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 23, strColor);
            cell.ColSpan = 3;
            cell.InnerHtml = "&nbsp;";
            row.Cells.Add(cell);
            table.Rows.Add(row);
            table.RenderControl(htw);


            table = new HtmlTable();
            table.Border = 1;
            table.BorderColor = "black";
            table.CellPadding = 0;
            table.CellSpacing = 0;
            table.Align = "center";
            css = table.Style;
            css.Add("font-size", strFontSize);
            css.Add("font-family", "標楷體");
            css.Add("width", "900px");

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 23, strColor);
            cell.ColSpan = 3;
            cell.InnerHtml = "  由轉介之教會填寫 (敬請一個月內回覆告知) &nbsp;";
            row.Cells.Add(cell);
            table.Rows.Add(row);



            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            cell.InnerHtml = " <span style=font-weight:bold >A、問題摘要：&nbsp;<P>";
            cell.InnerHtml += " <span style=font-weight:bold >關懷簡述：  ：&nbsp;<P>";
            cell.InnerHtml += " <span style=font-weight:bold >B、後續跟進：&nbsp;<br>";
            cell.InnerHtml += " <span style=font-weight:bold >1.□受洗：&nbsp;<br>";
            cell.InnerHtml += " <span style=font-weight:bold >2.□穩定聚會(有參加主日或其他小組聚會)：&nbsp;<br>";
            cell.InnerHtml += " <span style=font-weight:bold >3.□偶而參加聚會：&nbsp;<br>";
            cell.InnerHtml += " <span style=font-weight:bold >4.□不肯進入教會   (原因：           ) ：&nbsp;<br>";
            cell.InnerHtml += " <span style=font-weight:bold >5.□其他：  &nbsp;<P>";

            cell.InnerHtml += " <span style=font-weight:bold >C、關懷方式：&nbsp;<BR>";
            cell.InnerHtml += " <span style=font-weight:bold >1.□進入教會的關懷系統(小組/團契等) ：&nbsp;<P>";
            cell.InnerHtml += " <span style=font-weight:bold >2.□繼續保持聯繫(電話或探訪)：&nbsp;<br>";
            cell.InnerHtml += " <span style=font-weight:bold >3.□曾經聯繫過但目前中斷，原因：&nbsp;<br>";
            cell.InnerHtml += " <span style=font-weight:bold >4.□其他：&nbsp;<P>";
            cell.InnerHtml += " <span style=font-weight:bold >D、建 議：&nbsp;<br>";
            cell.InnerHtml += " <span style=font-weight:bold >&nbsp;<br>&nbsp;<br>&nbsp;<br>";


            row.Cells.Add(cell);
            table.Rows.Add(row);
            table.RenderControl(htw);
        }
        //轉成 html 碼========================================================================

        return htw.InnerWriter.ToString();
    }

    private string DateDiff(DateTime DateTime1, DateTime DateTime2)
    {
        string dateDiff = null;
        TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
        TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
        TimeSpan ts = ts1.Subtract(ts2).Duration();
        dateDiff = "通話時間共：<span style=color:red>" + (int.Parse(ts.Days.ToString()) * 1440 + int.Parse(ts.Hours.ToString()) * 60 + int.Parse(ts.Minutes.ToString())).ToString() + "分</span>";
        return dateDiff;
    }


}
