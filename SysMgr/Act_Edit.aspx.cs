using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Text;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

public partial class SysMgr_Act_Edit : BasePage 
{
    GroupAuthrity ga = null;
    #region NpoGridView 處理換頁相關程式碼
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
    #endregion NpoGridView 處理換頁相關程式碼
    //---------------------------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {

        Session["ProgID"] = "ActQry";
        //權控處理
        AuthrityControl();
        //有 npoGridView 時才需要
        Util.ShowSysMsgWithScript("$('#btnPreviousPage').hide();$('#btnNextPage').hide();$('#btnGoPage').hide();");
        //取得模式, 新增("ADD")或修改("")
        HFD_Mode.Value = Util.GetQueryString("Mode");
        HFD_UID.Value = Util.GetQueryString("Uid");


        if (!IsPostBack)
        {
            SetButton();
            getActCnt();
            LoadDropDownListData();
            LoadFormData();
        }
    }
    private void AuthrityControl()
    {
        if (Authrity.PageRight("_Focus") == false)
        {
            return;
        }
       // Authrity.CheckButtonRight("_AddNew", btnAdd);
        //Authrity.CheckButtonRight("_Query", btnQuery);
    }
    //-------------------------------------------------------------------------
    private void SetButton()
    {

       // btnDelete.Visible = (SessionInfo.GroupName == "系統管理員") ? true : false;
        if (HFD_Mode.Value == "Edit")
        {
            //修改時, 新增 查詢沒動作
            btnUpdate.Visible = true;
            btnDelete.Visible = true;
            btnExit.Visible = true;
            btnAdd.Visible = false;
            //btnQuery.Visible = false;
        }
        else
        {
            //載入畫面,修改 刪除鍵沒動作
            btnUpdate.Visible = false;
          //  btnDelete.Visible = false;
            btnExit.Visible = true;
            btnAdd.Visible = true;
            //btnQuery.Visible = true;
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    private void LoadFormData()
    {
        //呼叫資料庫GetDataTable
        DataTable dt = GetDataTable();

        //Grid initial
        NPOGridView npoGridView = new NPOGridView();
        npoGridView.Source = NPOGridViewDataSource.fromDataTable;
        npoGridView.dataTable = dt;
        npoGridView.Keys.Add("uid");
        npoGridView.DisableColumn.Add("uid");
        npoGridView.DisableColumn.Add("UserID");
        npoGridView.DisableColumn.Add("ClassUid");
        npoGridView.DisableColumn.Add("ItemUid");
        //npoGridView.DisableColumn.Add("ServiceData");
        npoGridView.CurrentPage = Util.String2Number(HFD_CurrentPage.Value);
       // npoGridView.EditLink = Util.RedirectByTime("ServiceRecordQry.aspx", "Mode=Edit&Uid=");
        //lblGridList.Text = npoGridView.Render();


        if (HFD_Mode.Value == "Edit")
        {
            DataRow dr = null;
            dr = dt.Rows[0];
            //服務類別
            ddlClassUid.SelectedValue = dr["ClassUid"].ToString();
            //服務項目
            ddlItemUid.SelectedValue = dr["ItemUid"].ToString();
            //活動日期
            txtBegDate.Text = Util.DateTime2String(dr["BegDate"], DateType.yyyyMMdd, EmptyType.ReturnNull);
            //結束日期
            txtEndDate.Text = Util.DateTime2String(dr["EndDate"], DateType.yyyyMMdd, EmptyType.ReturnNull);
            //說明
            txtRemark.Text = dr["Remark"].ToString();
            //名額
            txtPlaces.Text = dr["Places"].ToString();

        }
    }

    //--------------------------------------------------------------------------
    private void SetColumnData(List<ColumnData> list)
    {
        //Key
        list.Add(new ColumnData("uid", HFD_UID.Value, false, false, true));
        //服務類別
        list.Add(new ColumnData("Classuid", ddlClassUid.SelectedValue, true, true, false));
        //服務項目
        list.Add(new ColumnData("Itemuid", ddlItemUid.SelectedValue, true, true, false));
        //活動日期
        list.Add(new ColumnData("BegDate", Util.DateTime2String(txtBegDate.Text, DateType.yyyyMMdd, EmptyType.ReturnNull), true, true, false));
        //結束日期
        list.Add(new ColumnData("EndDate", Util.DateTime2String(txtEndDate.Text, DateType.yyyyMMdd, EmptyType.ReturnNull), true, true, false));
        //說明
        list.Add(new ColumnData("Remark", txtRemark.Text, true, true, false));
        //名額
        list.Add(new ColumnData("Places", txtPlaces.Text, true, true, false));
    }
    //--------------------------------------------------------------------------------------
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        LoadFormData();

    }
    //--------------------------------------------------------------------------------------
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        List<ColumnData> list = new List<ColumnData>();
        SetColumnData(list);
        string strSql = Util.CreateInsertCommand("Act", list, dict);
        NpoDB.ExecuteSQLS(strSql, dict);
        Session["Msg"] = "新增資料成功";
        Response.Redirect(Util.RedirectByTime("ActQry.aspx"));
        //Response.Redirect(Util.RedirectByTime("ServiceItemEdit.aspx?Mode=ADD&"));
    }
    //-------------------------------------------------------------------------------------------------------------
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        List<ColumnData> list = new List<ColumnData>();
        SetColumnData(list);
        string strSql = Util.CreateUpdateCommand("Act", list, dict);
        NpoDB.ExecuteSQLS(strSql, dict);
        Session["Msg"] = "修改資料成功";
        Response.Redirect(Util.RedirectByTime("ActQry.aspx"));
    }
    //--------------------------------------------------------------------------------------
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        //把該筆刪除的資料 IsDelete改成Y
        Dictionary<string, object> dict = new Dictionary<string, object>();
        string strSql = @"
                           update Act set
                           IsDelete='Y',
                           DeleteID=@DeleteID,
                           DeleteDate=GetDate()
                           where uid = @uid
                         ";
        dict.Add("uid", HFD_UID.Value);
        dict.Add("DeleteID", SessionInfo.UserID);
        NpoDB.ExecuteSQLS(strSql, dict);
        Session["Msg"] = "刪除資料成功";
        Response.Redirect(Util.RedirectByTime("ActQry.aspx"));
    }
    //--------------------------------------------------------------------------------------
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("ActQry.aspx");
    }
  
    //-------------------------------------------------------------------------
    public void LoadDropDownListData()
    {
        //志工
        //string strSql = "select UserID,UserName from AdminUser";
        //Util.FillDropDownList(ddlUserName, strSql, "UserName", "UserID", true);
        //服務類別
        string strSql = "select uid , ClassName from ServiceClass";
        Util.FillDropDownList(ddlClassUid, strSql, "ClassName", "uid", true);

        if (HFD_Mode.Value == "Edit")
        {
            DataTable dt = GetDataTable();
            DataRow dr = null;
            dr = dt.Rows[0];
            //服務項目
            strSql = "select uid ,ItemName from ServiceItem  where ClassUid ='" + dr["ClassUid"].ToString() + "'";
            Util.FillDropDownList(ddlItemUid, strSql, "ItemName", "uid", true);
        }
    }
    //----------------------------------------------------------------------------------------------------------


    private void getActCnt()
    {
        string strSql = @"
                       SELECT COUNT(*) AS Cnt FROM Act  a INNER JOIN ActMap b ON a.uid = b.ActUid WHERE b.ActUid =@ActUid
                        ";
        DataRow dr = null;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("ActUid", HFD_UID.Value);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count == 0)
        {
           // btnDelete.Visible = false;
        }
        dr = dt.Rows[0];
       if (dr["Cnt"].ToString() != "0")
       {
           //btnDelete.Visible = false;
       }
    }

    //----------------------------------------------------------------------------------------------------------
    private DataTable GetDataTable()
    {
        //讀取資料庫
        string strSql = @"
                    SELECT  a.uid as uid,ClassName , itemname,Remark,BegDate ,EndDate ,Places,itemuid,a.classuid as classuid
                    FROM Act a INNER JOIN ServiceItem s ON a.ItemUid = s.uid
                    INNER JOIN ServiceClass s2 ON s2.uid = s.ClassUid 
                    WHERE ISNULL(a.IsDelete ,'')!='Y'
                        ";
        //if (ddlUserName.SelectedValue != "")
        //{
        //    strSql += " and r.UserID =@UserID";
        //}
        //if (ddlClassUid.SelectedValue != "")
        //{
        //    strSql += " and r.ClassUid =@ClassUid";
        //}
        //if (ddlItemUid.SelectedValue != "")
        //{
        //    strSql += " and r.ItemUid =@ItemUid";
        //}
        //if (txtServiceData.Text != "")
        //{
        //    strSql += " and CONVERT(varchar(100), r.ServiceData, 111) = @ServiceData ";
        //}
        if (HFD_Mode.Value == "Edit")
        {
            strSql += " and a.uid=@Uid";
        }
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("Uid", HFD_UID.Value);
        //dict.Add("UserID", ddlUserName.SelectedValue);
        //dict.Add("ClassUid", ddlClassUid.SelectedValue);
        //dict.Add("ItemUid", ddlItemUid.SelectedValue);
        //dict.Add("ServiceData", txtServiceData.Text);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);

        return dt;

    }

    protected void ddlClassUid_SelectedIndexChanged(object sender, EventArgs e)
    {
        //服務項目
        string strSql = "select uid ,ItemName from ServiceItem  where ClassUid ='" + ddlClassUid.SelectedValue + "'";
        Util.FillDropDownList(ddlItemUid, strSql, "ItemName", "uid", true);
    }
    // 報表************************************************************************************************************************
    private string GetReport()
    {
        //HtmlTable table = new HtmlTable();
        //HtmlTableRow row;
        //HtmlTableCell cell;
        //CssStyleCollection css;
        //StringWriter sw = new StringWriter();
        //HtmlTextWriter htw = new HtmlTextWriter(sw);
        ////table.Border = 0;
        ////table.BorderColor = "black";
        ////table.CellPadding = 0;
        ////table.CellSpacing = 0;
        ////table.Align = "center";
        ////css = table.Style;
        ////css.Add("font-size", "32pt");
        ////css.Add("font-family", "標楷體");
        ////css.Add("width", "900px");
        ////--------------------------------------------
        //DataTable dt = GetDataTable();

        //if (dt.Rows.Count == 0)
        //{

        //    lblRpt.Text = "";
        //    ShowSysMsg("查無資料");
        //    return "";
        //}

        //foreach (DataRow dr in dt.Rows)
        //{
        //    string strFontSize = "16px";

        //    table = new HtmlTable();
        //    table.Border = 0;
        //    table.BorderColor = "grey";
        //    table.CellPadding = 0;
        //    table.CellSpacing = 0;
        //    table.Align = "center";
        //    css = table.Style;
        //    css.Add("font-size", strFontSize);
        //    css.Add("font-family", "標楷體");
        //    css.Add("width", "900px");

        //    string strColor = "lightgrey";  //字的顏色
        //    string strBackGround = "darkseagreen"; //cell 顏色
        //    string strMarkColor = "'background-color:'''"; // 字的底色
        //    string strMemberColor = "color:crimson"; //聯絡人的字顏色
        //    string strWeight = "bold"; //字體
        //    string strHeight = "20px";//欄高

        //    row = new HtmlTableRow();
        //    cell = new HtmlTableCell();
        //    css = cell.Style;
        //    css.Add("text-align", "left");
        //    css.Add("font-size", strFontSize);
        //    css.Add("font-weight", strWeight);
        //    css.Add("height", "20px");
        //    Util.AddTDLine(css, 123, strColor);
        //    cell.InnerHtml = "1.電話：<span style= " + strMemberColor + " >" + dr["Phone"].ToString() + "</span>";// font-weight:bold;
        //    row.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    css = cell.Style;
        //    css.Add("text-align", "left");
        //    css.Add("font-size", strFontSize);
        //    css.Add("font-weight", strWeight);
        //    Util.AddTDLine(css, 23, strColor);
        //    cell.InnerHtml = "4.年齡約：<span style=" + strMemberColor + "> " + dr["age"].ToString() + "歲</span>";
        //    row.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    css = cell.Style;
        //    css.Add("text-align", "left");
        //    css.Add("font-size", strFontSize);
        //    css.Add("font-weight", strWeight);
        //    Util.AddTDLine(css, 23, strColor);
        //    cell.InnerHtml = "7.轉介教會：<span style=" + strMemberColor + ">" + dr["ChurchYN"].ToString() + "</span>";
        //    row.Cells.Add(cell);
        //    table.Rows.Add(row);


        //    row = new HtmlTableRow();
        //    cell = new HtmlTableCell();
        //    css = cell.Style;
        //    css.Add("text-align", "left");
        //    css.Add("font-size", strFontSize);
        //    css.Add("font-weight", strWeight);
        //    css.Add("height", "25px");
        //    Util.AddTDLine(css, 123, strColor);
        //    cell.InnerHtml = "2.姓名：<span style=" + strMemberColor + " >" + dr["Cname"].ToString() + "</span> ";
        //    row.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    css = cell.Style;
        //    css.Add("text-align", "left");
        //    css.Add("font-size", strFontSize);
        //    css.Add("font-weight", strWeight);
        //    Util.AddTDLine(css, 23, strColor);
        //    cell.InnerHtml = "5.婚姻：<span style=" + strMemberColor + " >" + Util.TrimLastChar(dr["Marry"].ToString(), ',') + "</span>&nbsp;";
        //    row.Cells.Add(cell);


        //    cell = new HtmlTableCell();
        //    css = cell.Style;
        //    css.Add("text-align", "left");
        //    css.Add("font-size", strFontSize);
        //    css.Add("font-weight", strWeight);
        //    Util.AddTDLine(css, 23, strColor);
        //    cell.RowSpan = 2;
        //    //cell.ColSpan = 2;
        //    cell.InnerHtml = "8.地址：<span style=" + strMemberColor + " >" + dr["FullAddress"].ToString() + "</span> ";
        //    row.Cells.Add(cell);
        //    table.Rows.Add(row);



        //    row = new HtmlTableRow();
        //    cell = new HtmlTableCell();
        //    css = cell.Style;
        //    css.Add("text-align", "left");
        //    css.Add("font-size", strFontSize);
        //    css.Add("font-weight", strWeight);
        //    css.Add("height", "30px");
        //    Util.AddTDLine(css, 123, strColor);
        //    cell.InnerHtml = "3.性別：<span style=" + strMemberColor + " >" + dr["Sex"].ToString() + "</span>";
        //    row.Cells.Add(cell);


        //    cell = new HtmlTableCell();
        //    css = cell.Style;
        //    css.Add("text-align", "left");
        //    css.Add("font-size", strFontSize);
        //    css.Add("font-weight", strWeight);
        //    Util.AddTDLine(css, 23, strColor);
        //    cell.InnerHtml = "6.基督徒：<span style=" + strMemberColor + " >" + dr["Christian"].ToString() + "</span>";
        //    row.Cells.Add(cell);
        //    table.Rows.Add(row);
        //    table.RenderControl(htw);

        //    table = new HtmlTable();
        //    table.Border = 0;
        //    table.BorderColor = "black";
        //    table.CellPadding = 0;
        //    table.CellSpacing = 0;
        //    table.Align = "center";
        //    css = table.Style;
        //    css.Add("font-size", strFontSize);
        //    css.Add("font-family", "標楷體");
        //    css.Add("width", "900px");

        //    row = new HtmlTableRow();
        //    cell = new HtmlTableCell();
        //    css = cell.Style;
        //    css.Add("text-align", "left");
        //    css.Add("font-size", strFontSize);
        //    css.Add("font-weight", strWeight);
        //    css.Add("height", "30px");
        //    Util.AddTDLine(css, 123, strColor);
        //    cell.ColSpan = 1;
        //    cell.InnerHtml = "志工：<span style=color:darkseagreen >" + dr["ServiceUser"].ToString() + "</span>";
        //    row.Cells.Add(cell);
        //    string TotalTalkTime = "";
        //    DateTime t1, t2;
        //    if (dr["CreateDate"].ToString() != "" && dr["EndDate"].ToString() != "")
        //    {
        //        t1 = DateTime.Parse(Util.DateTime2String(dr["CreateDate"].ToString(), DateType.yyyyMMddHHmmss, EmptyType.ReturnNull));
        //        t2 = DateTime.Parse(Util.DateTime2String(dr["EndDate"].ToString(), DateType.yyyyMMddHHmmss, EmptyType.ReturnNull));

        //        TotalTalkTime = DateDiff(t1, t2);
        //    }


        //    cell = new HtmlTableCell();
        //    css = cell.Style;
        //    css.Add("text-align", "left");
        //    css.Add("font-size", strFontSize);
        //    css.Add("font-weight", strWeight);
        //    Util.AddTDLine(css, 23, strColor);
        //    cell.ColSpan = 3;
        //    cell.InnerHtml = "建檔時間：<span style=" + strMarkColor + " >" + Util.DateTime2String(dr["CreateDate"].ToString(), DateType.yyyyMMddHHmmss, EmptyType.ReturnNull) + " " + "</span> &nbsp;結束時間：<span style=" + strMarkColor + " >" + Util.DateTime2String(dr["EndDate"].ToString(), DateType.yyyyMMddHHmmss, EmptyType.ReturnNull) + "</span>&nbsp;" + TotalTalkTime;
        //    row.Cells.Add(cell);
        //    table.Rows.Add(row);

        //    table.RenderControl(htw);

        //    table = new HtmlTable();
        //    table.Border = 0;
        //    table.BorderColor = "black";
        //    table.CellPadding = 0;
        //    table.CellSpacing = 0;
        //    table.Align = "center";
        //    css = table.Style;
        //    css.Add("font-size", strFontSize);
        //    css.Add("font-family", "標楷體");
        //    css.Add("width", "900px");



        //    row = new HtmlTableRow();
        //    cell = new HtmlTableCell();
        //    css = cell.Style;
        //    css.Add("text-align", "center");
        //    css.Add("font-size", "26px");
        //    css.Add("background", strBackGround);
        //    Util.AddTDLine(css, 123, strColor);
        //    cell.Width = "88mm";
        //    cell.RowSpan = 6;
        //    cell.InnerHtml = "S";
        //    row.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    css = cell.Style;
        //    css.Add("text-align", "center");
        //    css.Add("font-size", strFontSize);
        //    css.Add("background", strBackGround);
        //    Util.AddTDLine(css, 23, strColor);
        //    cell.InnerHtml = "<span style=font-weight:bold>求助者的主訴(用第一人稱 '我' 敘述)</span>";
        //    cell.ColSpan = 3;
        //    row.Cells.Add(cell);
        //    table.Rows.Add(row);

        //    row = new HtmlTableRow();
        //    cell = new HtmlTableCell();
        //    css = cell.Style;
        //    css.Add("text-align", "left");
        //    css.Add("font-size", strFontSize);
        //    //css.Add("font-weight", strWeight);
        //    css.Add("height", strHeight);
        //    Util.AddTDLine(css, 23, strColor);
        //    cell.ColSpan = 3;
        //    cell.InnerHtml = "<span style=font-weight:bold>A(事件)：</span>" + dr["Event"].ToString() + "&nbsp;";
        //    row.Cells.Add(cell);
        //    table.Rows.Add(row);

        //    row = new HtmlTableRow();
        //    cell = new HtmlTableCell();
        //    css = cell.Style;
        //    css.Add("text-align", "left");
        //    css.Add("font-size", strFontSize);
        //    css.Add("height", strHeight);
        //    // css.Add("font-weight", strWeight);
        //    Util.AddTDLine(css, 23, strColor);
        //    cell.InnerHtml = "<span style=font-weight:bold>B(想法)：</span>" + dr["Think"].ToString() + "&nbsp;";
        //    cell.ColSpan = 3;
        //    row.Cells.Add(cell);
        //    table.Rows.Add(row);

        //    row = new HtmlTableRow();
        //    cell = new HtmlTableCell();
        //    css = cell.Style;
        //    css.Add("text-align", "left");
        //    css.Add("font-size", strFontSize);
        //    css.Add("height", strHeight);
        //    //  css.Add("font-weight", strWeight);
        //    Util.AddTDLine(css, 23, strColor);
        //    cell.InnerHtml = "<span style=font-weight:bold>C(感受)：</span>" + dr["Feel"].ToString() + "&nbsp;";
        //    cell.ColSpan = 3;
        //    row.Cells.Add(cell);
        //    table.Rows.Add(row);

        //    row = new HtmlTableRow();
        //    cell = new HtmlTableCell();
        //    css = cell.Style;
        //    css.Add("text-align", "left");
        //    css.Add("font-size", strFontSize);
        //    css.Add("height", strHeight);
        //    //css.Add("font-weight", strWeight);
        //    Util.AddTDLine(css, 23, strColor);
        //    cell.ColSpan = 3;
        //    cell.InnerHtml = "<span style=font-weight:bold>補充說明：</span>" + dr["Comment"].ToString() + "&nbsp;";
        //    row.Cells.Add(cell);
        //    table.Rows.Add(row);

        //    row = new HtmlTableRow();
        //    cell = new HtmlTableCell();
        //    css = cell.Style;
        //    css.Add("text-align", "left");
        //    css.Add("font-size", strFontSize);
        //    Util.AddTDLine(css, 23, strColor);
        //    cell.ColSpan = 3;
        //    cell.InnerHtml = "&nbsp;";
        //    row.Cells.Add(cell);
        //    table.Rows.Add(row);

        //    row = new HtmlTableRow();
        //    cell = new HtmlTableCell();
        //    css = cell.Style;
        //    css.Add("text-align", "center");
        //    css.Add("font-size", "26px");
        //    css.Add("background", strBackGround);
        //    Util.AddTDLine(css, 123, strColor);
        //    cell.RowSpan = 6;
        //    cell.InnerHtml = "O";
        //    row.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    css = cell.Style;
        //    css.Add("text-align", "center");
        //    css.Add("font-size", strFontSize);
        //    css.Add("background", strBackGround);
        //    //css.Add("font-weight", strWeight);
        //    Util.AddTDLine(css, 23, strColor);
        //    cell.ColSpan = 3;
        //    cell.InnerHtml = "<span style=font-weight:bold>客觀分析(諮商類別)</span>";
        //    row.Cells.Add(cell);
        //    table.Rows.Add(row);


        //    row = new HtmlTableRow();
        //    cell = new HtmlTableCell();
        //    css = cell.Style;
        //    css.Add("text-align", "left");
        //    css.Add("font-size", strFontSize);
        //    // css.Add("font-weight", strWeight)
        //    css.Add("height", strHeight); ;
        //    Util.AddTDLine(css, 23, strColor);
        //    cell.ColSpan = 3;
        //    cell.InnerHtml = "<span style=font-weight:bold>來電諮詢別(大類)：</span>" + Util.TrimLastChar(dr["ConsultantMain"].ToString(), ',') + "&nbsp;";
        //    row.Cells.Add(cell);
        //    table.Rows.Add(row);

        //    row = new HtmlTableRow();
        //    cell = new HtmlTableCell();
        //    css = cell.Style;
        //    css.Add("text-align", "left");
        //    css.Add("font-size", strFontSize);
        //    // css.Add("font-weight", strWeight);
        //    css.Add("height", strHeight);
        //    Util.AddTDLine(css, 23, strColor);
        //    cell.ColSpan = 3;
        //    cell.InnerHtml = "<span style=font-weight:bold >來電諮詢別(分項)：</span>" + Util.TrimLastChar(dr["ConsultantItem"].ToString(), ',') + "&nbsp;";
        //    row.Cells.Add(cell);
        //    table.Rows.Add(row);

        //    row = new HtmlTableRow();
        //    cell = new HtmlTableCell();
        //    css = cell.Style;
        //    css.Add("text-align", "left");
        //    css.Add("font-size", strFontSize);
        //    css.Add("height", strHeight);
        //    // css.Add("font-weight", strWeight);
        //    Util.AddTDLine(css, 23, strColor);
        //    cell.ColSpan = 3;
        //    if (dr["HarassOther"].ToString() != "")
        //    {
        //        cell.InnerHtml = "<span style=font-weight:bold >騷擾電話：</span>" + Util.TrimLastChar(dr["HarassPhone"].ToString().Replace("其他", ""), ',').TrimEnd(',') + "<span style=font-weight:bold >其他： </span>" + dr["HarassOther"].ToString() + "&nbsp;";
        //        //cell.InnerHtml = "<span style=font-weight:bold >騷擾電話：</span>" + Util.TrimLastChar(dr["HarassPhone"].ToString(), ',') + "<span style=font-weight:bold >其他： </span>" + dr["HarassOther"].ToString() + "&nbsp;";
        //    }
        //    else
        //    {
        //        cell.InnerHtml = "<span style=font-weight:bold>騷擾電話：</span>" + Util.TrimLastChar(dr["HarassPhone"].ToString(), ',') + "&nbsp;";
        //    }

        //    row.Cells.Add(cell);
        //    table.Rows.Add(row);

        //    row = new HtmlTableRow();
        //    cell = new HtmlTableCell();
        //    css = cell.Style;
        //    css.Add("text-align", "left");
        //    css.Add("font-size", strFontSize);
        //    Util.AddTDLine(css, 23, strColor);
        //    css.Add("height", strHeight);
        //    // css.Add("font-weight", strWeight);
        //    cell.ColSpan = 3;
        //    cell.InnerHtml = "<span style= font-weight:bold>轉介單位(告知電話)：</span>" + Util.TrimLastChar(dr["IntroductionUnit"].ToString(), ',') + "&nbsp;";
        //    row.Cells.Add(cell);
        //    table.Rows.Add(row);

        //    row = new HtmlTableRow();
        //    cell = new HtmlTableCell();
        //    css = cell.Style;
        //    css.Add("text-align", "left");
        //    css.Add("font-size", strFontSize);
        //    //   css.Add("font-weight", strWeight);
        //    css.Add("height", strHeight);
        //    Util.AddTDLine(css, 23, strColor);
        //    cell.ColSpan = 3;
        //    cell.InnerHtml = "<span style= font-weight:bold>緊急轉介(撥打電話)：</span>" + Util.TrimLastChar(dr["CrashIntroduction"].ToString(), ',') + "&nbsp;";
        //    row.Cells.Add(cell);
        //    table.Rows.Add(row);

        //    row = new HtmlTableRow();
        //    cell = new HtmlTableCell();
        //    css = cell.Style;
        //    css.Add("text-align", "center");
        //    css.Add("font-size", "26px");
        //    css.Add("background", strBackGround);
        //    css.Add("font-weight", strWeight);
        //    Util.AddTDLine(css, 123, strColor);
        //    cell.RowSpan = 2;
        //    cell.InnerHtml = "A";
        //    row.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    css = cell.Style;
        //    css.Add("text-align", "center");
        //    css.Add("font-size", strFontSize);
        //    css.Add("background", strBackGround);
        //    // css.Add("font-weight", strWeight);
        //    Util.AddTDLine(css, 23, strColor);
        //    cell.ColSpan = 3;
        //    cell.InnerHtml = "<span style=font-weight:bold >問題評估</span>&nbsp;";
        //    row.Cells.Add(cell);
        //    table.Rows.Add(row);

        //    row = new HtmlTableRow();
        //    cell = new HtmlTableCell();
        //    css = cell.Style;
        //    css.Add("text-align", "left");
        //    css.Add("font-size", strFontSize);
        //    css.Add("height", strHeight);
        //    Util.AddTDLine(css, 23, strColor);
        //    cell.ColSpan = 1;
        //    cell.InnerHtml = "<span style=" + strMarkColor + " > " + dr["Problem"].ToString() + " </span>&nbsp;";
        //    row.Cells.Add(cell);
        //    table.Rows.Add(row);

        //    //row = new HtmlTableRow();
        //    //cell = new HtmlTableCell();
        //    //css = cell.Style;
        //    //css.Add("text-align", "left");
        //    //css.Add("font-size", strFontSize);
        //    //Util.AddTDLine(css, 23);
        //    //cell.ColSpan = 3;
        //    //cell.InnerHtml = "2.因" + dr["Reason2"].ToString() + "造成" + dr["Trouble2"].ToString() + "問題";
        //    //row.Cells.Add(cell);
        //    //table.Rows.Add(row);

        //    //row = new HtmlTableRow();
        //    //cell = new HtmlTableCell();
        //    //css = cell.Style;
        //    //css.Add("text-align", "left");
        //    //css.Add("font-size", strFontSize);
        //    //Util.AddTDLine(css, 23);
        //    //cell.ColSpan = 3;
        //    //cell.InnerHtml = "3.因" + dr["Reason3"].ToString() + "造成" + dr["Trouble3"].ToString() + "問題";
        //    //row.Cells.Add(cell);
        //    //table.Rows.Add(row);


        //    row = new HtmlTableRow();
        //    cell = new HtmlTableCell();
        //    css = cell.Style;
        //    css.Add("text-align", "center");
        //    css.Add("font-size", "26px");
        //    css.Add("background", strBackGround);
        //    css.Add("height", strHeight);
        //    //css.Add("font-weight", strWeight);
        //    Util.AddTDLine(css, 1234, strColor);
        //    cell.RowSpan = 8;
        //    cell.InnerHtml = "P";
        //    row.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    css = cell.Style;
        //    css.Add("text-align", "center");
        //    css.Add("font-size", strFontSize);
        //    css.Add("background", strBackGround);
        //    //css.Add("font-weight", strWeight);
        //    Util.AddTDLine(css, 23, strColor);
        //    cell.ColSpan = 3;
        //    cell.InnerHtml = "<span style= font-weight:bold>計畫</span>";
        //    row.Cells.Add(cell);
        //    table.Rows.Add(row);

        //    row = new HtmlTableRow();
        //    cell = new HtmlTableCell();
        //    css = cell.Style;
        //    css.Add("text-align", "left");
        //    css.Add("font-size", strFontSize);
        //    // css.Add("font-weight", strWeight);
        //    css.Add("height", strHeight);
        //    Util.AddTDLine(css, 23, strColor);
        //    cell.ColSpan = 3;
        //    cell.InnerHtml = "<span style=font-weight:bold >1.找出案主心中的假設：</span>" + dr["FindAssume"].ToString() + "</span>&nbsp;";
        //    row.Cells.Add(cell);
        //    table.Rows.Add(row);

        //    row = new HtmlTableRow();
        //    cell = new HtmlTableCell();
        //    css = cell.Style;
        //    css.Add("text-align", "left");
        //    css.Add("font-size", strFontSize);
        //    //css.Add("font-weight", strWeight);
        //    css.Add("height", strHeight);
        //    Util.AddTDLine(css, 23, strColor);
        //    cell.ColSpan = 3;
        //    cell.InnerHtml = "<span style=font-weight:bold >2.與案主討論與解釋假設： </span>" + dr["Discuss"].ToString() + "</span>&nbsp;";
        //    row.Cells.Add(cell);
        //    table.Rows.Add(row);

        //    row = new HtmlTableRow();
        //    cell = new HtmlTableCell();
        //    css = cell.Style;
        //    css.Add("text-align", "left");
        //    css.Add("font-size", strFontSize);
        //    css.Add("height", strHeight);
        //    // css.Add("font-weight", strWeight);
        //    Util.AddTDLine(css, 23, strColor);
        //    cell.ColSpan = 3;
        //    cell.InnerHtml = "<span style=font-weight:bold >3.加入新的元素：</span>" + dr["Element"].ToString() + "&nbsp;";
        //    row.Cells.Add(cell);
        //    table.Rows.Add(row);

        //    row = new HtmlTableRow();
        //    cell = new HtmlTableCell();
        //    css = cell.Style;
        //    css.Add("text-align", "left");
        //    css.Add("font-size", strFontSize);
        //    css.Add("height", strHeight);
        //    //css.Add("font-weight", strWeight);
        //    Util.AddTDLine(css, 23, strColor);
        //    cell.ColSpan = 3;
        //    cell.InnerHtml = " <span style=font-weight:bold >4.改變案主原先的期待：</span>" + dr["Expect"].ToString() + "&nbsp;";
        //    row.Cells.Add(cell);
        //    table.Rows.Add(row);

        //    row = new HtmlTableRow();
        //    cell = new HtmlTableCell();
        //    css = cell.Style;
        //    css.Add("text-align", "left");
        //    css.Add("font-size", strFontSize);
        //    css.Add("height", strHeight);
        //    // css.Add("font-weight", strWeight);
        //    Util.AddTDLine(css, 23, strColor);
        //    cell.InnerHtml = " <span style=font-weight:bold >5.給予案主祝福與盼望：</span>" + dr["Blessing"].ToString() + "&nbsp;";
        //    cell.ColSpan = 3;
        //    row.Cells.Add(cell);
        //    table.Rows.Add(row);

        //    row = new HtmlTableRow();
        //    cell = new HtmlTableCell();
        //    css = cell.Style;
        //    css.Add("text-align", "left");
        //    css.Add("font-size", strFontSize);
        //    css.Add("height", strHeight);
        //    // css.Add("font-weight", strWeight);
        //    Util.AddTDLine(css, 23, strColor);
        //    cell.ColSpan = 3;
        //    cell.InnerHtml = "<span style=font-weight:bold >6.帶領決志禱告：</span>" + dr["IntroductionChurch"].ToString() + "&nbsp;";
        //    row.Cells.Add(cell);
        //    table.Rows.Add(row);

        //    row = new HtmlTableRow();
        //    cell = new HtmlTableCell();
        //    css = cell.Style;
        //    css.Add("text-align", "left");
        //    css.Add("font-size", strFontSize);
        //    css.Add("height", strHeight);
        //    //  css.Add("font-weight", strWeight);
        //    Util.AddTDLine(css, 234, strColor);
        //    cell.ColSpan = 3;
        //    cell.InnerHtml = "<span style=font-weight:bold >7.我對個案的幫助程度：</span>" + dr["HelpLvMark"].ToString() + "&nbsp;";
        //    row.Cells.Add(cell);
        //    table.Rows.Add(row);

        //    row = new HtmlTableRow();
        //    cell = new HtmlTableCell();
        //    css = cell.Style;
        //    css.Add("text-align", "left");
        //    css.Add("height", strHeight);
        //    css.Add("font-size", strFontSize);
        //    //css.Add("font-weight", strWeight);
        //    Util.AddTDLine(css, 134, strColor);
        //    cell.ColSpan = 6;
        //    cell.InnerHtml = "<span style=font-weight:bold >小叮嚀：</span>" + dr["Memo"].ToString() + "&nbsp;";
        //    row.Cells.Add(cell);
        //    table.Rows.Add(row);

        //    cell = new HtmlTableCell();
        //    row = new HtmlTableRow();
        //    css = cell.Style;
        //    css.Add("text-align", "left");
        //    css.Add("font-size", strFontSize);
        //    cell.InnerHtml = "&nbsp; <BR>";
        //    row.Cells.Add(cell);
        //    table.Rows.Add(row);
        //    table.RenderControl(htw);
        //}
        ////轉成 html 碼========================================================================

        //return htw.InnerWriter.ToString();

        return "";
    }
    //----------------------------------------------------------------------------------------------------------
    //報表抬頭
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
        strTemp = "&nbsp;";// "" + ddlYear.SelectedValue + "年熱線志工班表  第 " + ddlQuerter.SelectedValue + " 季 ";
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
}

