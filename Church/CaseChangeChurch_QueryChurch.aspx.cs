using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Threading;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Net.Mail;


public partial class Church_CaseChangeChurch_QueryChurch : BasePage
{
    GroupAuthrity ga = null;
    string Name = "";
    string ChurchUid = "";
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
    protected void Page_Load(object sender, EventArgs e)
    {
        lblHeaderText.Text = "選擇教會資料";

        if (!IsPostBack)
        {
            HFD_UID.Value = Util.GetQueryString("uid");
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
        Util.FillCityData(ddlCity);
        Util.FillAreaData(ddlArea, ddlCity.SelectedValue);
    }
    //-------------------------------------------------------------------------------------------------------------
    private void LoadFormData()
    {
        string strSql = @"
                            select 
                            case isnull(c.uid, '') when '' then 0 else 1 end as selected,
                            c.uid, 
                            c.Name as 教會名稱,
                            c.Area as 區號,(select  name from codecity where ZipCode=cc.ZipCode ) as 縣市,cc1.Name as 區域,
                            c.Address As 地址,
                            c.priest as 主任牧師,
                            c.Contact as 聯絡窗口
                            --,c.ChurchEmail
                            From Church as c
                            inner join CodeCity cc on c.City = cc.ZipCode
                            inner join CodeCity cc1 on c.Area = cc1.ZipCode
                            where isnull(IsDelete, '') != 'Y'
                        ";

        if (ddlCity.SelectedValue != "")
        {
            strSql += " and City = @City\n";
        }
        if (HFD_Area.Value != "")
        {
            strSql += " and Area = @Area\n";
        }
        if (txtName.Text != "")
        {
            strSql += " and Name like @Name\n";
        }

        if (txtpriest.Text != "")
        {
            strSql += " and priest like @priest\n";
        }

        if (txtContact.Text != "")
        {
            strSql += " and Contact like @Contact\n";
        }

        if (txtAddress.Text != "")
        {
            strSql += " and Address like @Address\n";
        }

        strSql += "order BY City,cc.zipcode,CC1.Name \n";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("City", ddlCity.SelectedValue);
        dict.Add("Area", HFD_Area.Value);
        dict.Add("Name", "%" + txtName.Text + "%");
        dict.Add("priest", "%" + txtpriest.Text + "%");
        dict.Add("Contact", "%" + txtContact.Text + "%");
        dict.Add("Address", "%" + txtAddress.Text + "%");

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
        col.ControlName.Add("chkSelectLand");
        col.ControlId.Add("chkSelectLand");
        col.ControlValue.Add("0");
        col.ControlText.Add("");
        col.ColumnName.Add("selected");
        npoGridView.Columns.Add(col);
        //-------------------------------------------------------------------------
        col = new NPOGridViewColumn("教會名稱&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
        col.ColumnType = NPOColumnType.NormalText;
        col.ColumnName.Add("教會名稱");
        col.CellStyle = "width:500000mm";
        npoGridView.Columns.Add(col);
        //-------------------------------------------------------------------------
        col = new NPOGridViewColumn("區號");
        col.ColumnType = NPOColumnType.NormalText;
        col.ColumnName.Add("區號");
        npoGridView.Columns.Add(col);
        //-------------------------------------------------------------------------
        col = new NPOGridViewColumn("縣市&nbsp;&nbsp;&nbsp;&nbsp;");
        col.ColumnType = NPOColumnType.NormalText;
        col.ColumnName.Add("縣市");
        npoGridView.Columns.Add(col);
        //-------------------------------------------------------------------------
        col = new NPOGridViewColumn("區域&nbsp;&nbsp;&nbsp;&nbsp;");
        col.ColumnType = NPOColumnType.NormalText;
        col.ColumnName.Add("區域");
        npoGridView.Columns.Add(col);
        //-------------------------------------------------------------------------
        col = new NPOGridViewColumn("地址");
        col.ColumnType = NPOColumnType.NormalText;
        col.ColumnName.Add("地址");
        npoGridView.Columns.Add(col);
        //----------------------------------------------------------------------
        col = new NPOGridViewColumn("主任牧師&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
        col.ColumnType = NPOColumnType.NormalText;
        col.ColumnName.Add("主任牧師");
        npoGridView.Columns.Add(col);
        //-------------------------------------------------------------------------
        col = new NPOGridViewColumn("聯絡窗口&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
        col.ColumnType = NPOColumnType.NormalText;
        col.ColumnName.Add("聯絡窗口");
        npoGridView.Columns.Add(col);

        //col = new NPOGridViewColumn("Email");
        //col.ColumnType = NPOColumnType.NormalText;
        //col.ColumnName.Add("ChurchEmail");
        //npoGridView.Columns.Add(col);
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
            if (str.StartsWith("chkSelectLand_"))
            {
                strArray = str.Split('_');
                uids += strArray[1] + ",";
            }
        }
        uids = uids.TrimEnd(',');
        HFD_ChurchUid.Value = uids;
        int iUid = uids.IndexOf(",");
        if (iUid != -1)
        {
            ShowSysMsg("此處為單選，請確認!");
            return;
        }
        Dictionary<string, object> dict = new Dictionary<string, object>();
        string strSql = @"
                            update  Member
                            set  ChurchUid=@ChurchUid
                                ,ChangeChurchSta='已處理'
                            where 1=1
                            And uid =@uid
                            ";
        dict.Add("uid", HFD_UID.Value);
        dict.Add("ChurchUid", uids);
        if (uids.ToString() != "")
        {
            NpoDB.ExecuteSQLS(strSql, dict);
        }
        ShowSysMsg("儲存資料成功!");
        this.Response.Write(@"<script>self.opener.LoadChurchMember('LoadGird');window.close();</script>");
    }

    //-------------------------------------------------------------------------------------------------------------
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        LoadFormData();
    }
    //-------------------------------------------------------------------------------------------------------------
    protected void btnEmail_Click(object sender, EventArgs e)
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
        //if (iUid != 1)
        //{
        //    ShowSysMsg("請勾選選單");
        //    return;
        //}
        ShowSysMsg();

        //---------------------------------------------------------------

        //Response.Redirect("Footer_Mail.aspx?uid=" + uids);
        ClientScript.RegisterStartupScript(this.Page.GetType(), "", "window.open('Footer_Mail.aspx?uid=" + uids + "','','height=200,width=500,scrollbars=yes,resizable=yes');", true);

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
        strSql += "  And a.MemberUID = " + HFD_UID.Value + "";



        strSql += " Order by  CreateDate Desc";

        Dictionary<string, object> dict = new Dictionary<string, object>();

        //dict.Add("MemberUID", HFD_MemberID.Value.ToString());

        DataTable dt = NpoDB.GetDataTableS(strSql, dict);


        return dt;

    }
    //--------------------------------------------------------------------------
    private void SetColumnData(List<ColumnData> list)
    {
        //Key
        list.Add(new ColumnData("Uid", HFD_UID.Value, false, false, true));
        //轉介教會
        list.Add(new ColumnData("ChurchUid",ChurchUid , true, true, false));
        //轉介日期
       // list.Add(new ColumnData("changeChurchDate", Util.DateTime2String(DateTime.Now.ToShortDateString(), DateType.yyyyMMdd, EmptyType.ReturnNull), true, true, false));
        //轉介處理狀態
        //list.Add(new ColumnData("ChangeChurchSta", "已處理", true, true, false));
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
            //lblReport.Text = "";
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
            css.Add("width", "700px");


            string strColor = "lightgrey";  //字的顏色
            string strBackGround = "darkseagreen"; //cell 顏色
            string strMarkColor = "'background-color:'''"; // 字的底色
            string strMemberColor = "color:red"; //聯絡人的字顏色
            string strDtaeColor = "color:black"; //聯絡人的字顏色
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
            //----------------------------------------------------
            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("font-weight", strWeight);
            css.Add("height", "20px");
            Util.AddTDLine(css, 123, strColor);
            cell.InnerHtml = "<span style= " + strDtaeColor + " >轉介日期：" + DateTime.Now.ToShortDateString() + "</span>";// font-weight:bold;
            row.Cells.Add(cell);

            //---------------------------------------------------
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("font-weight", strWeight);
            css.Add("height", "20px");
            Util.AddTDLine(css, 123, strColor);
            cell.InnerHtml = "<span style= " + strMemberColor + " >轉介教會：" + Name;
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
            css.Add("width", "700px");



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
            css.Add("width", "700px");

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
    protected void btnChangeuid_Click(object sender, EventArgs e)
    {
        GetSelectValue();
        if (GetSelectValue() == "")
        {
            return;
        }
        //寫入資料
        Dictionary<string, object> dict = new Dictionary<string, object>();
        List<ColumnData> list = new List<ColumnData>();
        SetColumnData(list);
        string strSql = Util.CreateUpdateCommand("Member", list, dict);
        NpoDB.ExecuteSQLS(strSql, dict);

        Response.Redirect("CaseChangeChurchEdit.aspx?uid="+HFD_UID.Value);

    }
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("CaseChangeChurchEdit.aspx?uid="+HFD_UID.Value);
    }
    protected void btnChange_Click(object sender, EventArgs e)
    {
        string context = "";
        string FileName = "轉介教會";
        //寫入資料
        Dictionary<string, object> dict = new Dictionary<string, object>();
        List<ColumnData> list = new List<ColumnData>();
        SetColumnData(list);
        string strSql = Util.CreateUpdateCommand("Member", list, dict);
        NpoDB.ExecuteSQLS(strSql, dict);
        
        GetSelectValue();
        if (GetSelectValue() == "")
        {
            return;
        }
        context = GetRptHead() + GetReport();
        Util.OutputTxt(context, "2", FileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"));
    }

    private string GetSelectValue()
    {
        string[] chkSelect;
        string churchname = "";
       foreach (string str in Request.Form.Keys)
        {
            if (str.StartsWith("chkSelectLand_"))
            {
                chkSelect = str.Split('_');
                churchname += chkSelect[1] + ",";
            }
        }
       churchname = churchname.TrimEnd(',');
       int iUid = churchname.IndexOf(",");
       if (iUid != -1)
       {
           ShowSysMsg("此處為單選，請確認!");
           return "";
       }
       ChurchUid = churchname.ToString();
       string strSql = @"select * from Church where uid =@uid";
       Dictionary<string, object> dict = new Dictionary<string, object>();
       dict.Add("uid", churchname);
       DataTable dt = NpoDB.GetDataTableS(strSql, dict);
       int count = dt.Rows.Count;
       if (dt.Rows.Count == 0)
       {
           return "";
       }
       DataRow dr = null;
       dr = dt.Rows[0];
       Name = dr["Name"].ToString();
        return "1";
    }
}
