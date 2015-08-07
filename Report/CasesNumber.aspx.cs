//kuofly 20150211
//指定時間期間中，在籍個案有來電者，以該區間做統計
//原始資料會錯誤是因為統計基礎中忽略了沒輸入的資料
//sql 雛形
//select 
//city as 區間,
//area as 鄉鎮,
//count(distinct MemberUID) as cnt
//from (
//    select a.MemberUID,
//    isnull((select top 1 name from codecity where zipcode=b.city),'') city,
//    isnull((select top 1 name from codecity where zipcode=b.area),'') area 
//    from Consulting a  
//    left join Member b on a.MemberUID = b.uid 
//    where 1=1
//    and convert(varchar,a.CreateDate,111) between '2014/01/01' and '2014/01/31'
//) table1
//where 1=1  
//GROUP BY city,area
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Web.UI.DataVisualization.Charting;

public partial class CasesNumber : BasePage
{
    GroupAuthrity ga = null;
    int colCount = 0;
    int sum = 0;
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


        //權限控制
        AuthrityControl();
        Util.ShowSysMsgWithScript("$('#btnPreviousPage').hide();$('#btnNextPage').hide();$('#btnGoPage').hide();");
        if (!IsPostBack)
        {
            //載入下拉式選單資料
            LoadDropDownListData();
            //載入資料
            //  LoadFormData();

        }
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
        btnQuery.Visible = ga.Query;
    }
    //-------------------------------------------------------------------------
    public void LoadDropDownListData()
    {
        Util.FillCityData(ddlCity);
        //Util.FillAreaData(ddlArea, ddlCity.SelectedValue);
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
        //----------------------------------------------------------
        col = new NPOGridViewColumn("縣市"); //產生所需欄位及設定 Title
        col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
        col.ColumnName.Add("縣市"); //使用欄位名稱
        col.Link = Util.RedirectByTime("ConsultEdit2.aspx", "ConsultingUID=");
        col.CellStyle = "width:150px";
        npoGridView.Columns.Add(col);
        //----------------------------------------------------------
        col = new NPOGridViewColumn("區域"); //產生所需欄位及設定 Title
        col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
        col.ColumnName.Add("區域"); //使用欄位名稱
        col.Link = Util.RedirectByTime("ConsultEdit2.aspx", "ConsultingUID=");
        col.CellStyle = "width:150px";
        npoGridView.Columns.Add(col);
        lblGridList.Text = npoGridView.Render();
        //********************************************************************************************
    }
    
    //-------------------------------------------------------------------------
    //kuofly 2015/02/11 大餅圖
    private string getCityaAreaCnt(string city)
    {

        //讀取資料庫
//        string strSql = @" 
//                        select count(distinct MemberUID) as cnt,CC1.Name as 區間
//                        from Consulting a  inner join Member b on a.MemberUID = b.uid 
//                        inner join CodeCity CC on CC.ZipCode =City
//                        inner join CodeCity CC1 on CC1.ZipCode = Area
//                        where 1=1  
//                          ";

//        if (rdoSex.SelectedValue != "" && rdoSex.SelectedValue != "全部")
//        {
//            strSql += " and sex =@sex\n";
//        }

//        if (ddlAge.SelectedValue != "")
//        {
//            strSql += " and Age =@Age\n";
//        }
//        if (ddlCity.SelectedValue != "")
//        {
//            strSql += " and City = @City\n";
//        }
//        if (txtBegCreateDate.Text != "" && txtEndCreateDate.Text != "")
//        {
//            strSql += " and CONVERT(varchar(100), CreateDate, 111) Between @BegCreateDate And @EndCreateDate\n";
//        }
//        if (CKB_Firstphone.Checked.ToString() == "True")
//        {
//            strSql += @"  and b.uid in
//                        (
//                        select  memberuid from Consulting where 1=1 and isnull(IsDelete, '') != 'Y' 
//                        group by memberuid  having  count(memberuid)=1 
//                        )";
//        }
//        strSql += "  GROUP BY CC1.Name";
        string strSql = @" 
select 
city as 縣市,
area as 區間,
count(distinct MemberUID) as cnt
from (
	select a.MemberUID,
	isnull((select top 1 name from codecity where zipcode=b.city),'未知縣市') city,
	isnull((select top 1 name from codecity where zipcode=b.area),'未知鄉鎮') area 
	from Consulting a  
	left join Member b on a.MemberUID = b.uid 
	where 1=1
    and isnull(a.IsDelete, '') != 'Y'";
        //內部條件:管理者
        if (SessionInfo.GroupName != "系統管理員")
        {
            strSql += " and a.ServiceUser = @ServiceUser\n";
        }
        //輸入條件1:縣市
        if (ddlCity.SelectedValue != "")
        {
            strSql += " and b.City = @City\n";
        }
        //輸入條件2:電話
        if (txtPhone.Text.Trim() != "")
        {
            strSql += " and b.Phone like @Phone\n";
        }
        //輸入條件3:姓名
        if (txtName.Text.Trim() != "")
        {
            strSql += " and b.CName like @CName\n";
        }
        //輸入條件4:性別
        if (rdoSex.SelectedValue != "" && rdoSex.SelectedValue != "全部")
        {
            strSql += " and b.sex =@sex\n";
        }
        //輸入條件5:年齡層 (例如:'0~10'
        if (ddlAge.SelectedValue != "")
        {
            strSql += " and Age =@Age\n";
        }
        //輸入條件6:建檔時間
        if (txtBegCreateDate.Text.Trim() != "" && txtEndCreateDate.Text.Trim() != "")
        {
            strSql += " and CONVERT(varchar, CreateDate, 111) Between @BegCreateDate And @EndCreateDate\n";
        }
        //輸入條件7:是否為第一次來電
        //kuofly 20150211 修正第一次來電的邏輯
        if (CKB_Firstphone.Checked.ToString() == "True")
        {
            strSql += @"  and a.MemberUID + '_' + (select replace(convert(varchar(8), a.CreateDate, 112)+convert(varchar(8), CreateDate, 114), ':',''))
                                in
	                                (
		                                select 使用者鍵值 from 
		                                (
		                                select Memberuid+ '_' + min(replace(convert(varchar(8), CreateDate, 112)+convert(varchar(8), CreateDate, 114), ':','')) 使用者鍵值,convert(varchar,MIN(CreateDate),111) 第一次來電日期 
		                                from Consulting where isnull(MemberUID,'')<>'' and isnull(IsDelete, '') != 'Y' 
		                                group by Memberuid 
		                                ) table1
		                                where 1=1";
            if (txtBegCreateDate.Text.Trim() != "" && txtEndCreateDate.Text.Trim() != "")
            {
                strSql += " and 第一次來電日期 Between @BegCreateDate And @EndCreateDate\n";
            }
            strSql += @"        and 使用者鍵值 is not null
	                                )
                            ";
        }
        //輸入條件8:是否教會弟兄姊妹
        if (CKB_Christian.Checked.ToString() == "True")
        {
            strSql += " and b.Christian='是'";
        }

        strSql += @"

) table1
where 1=1  
GROUP BY city,area";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("CName", "%" + txtName.Text.Trim() + "%");
        dict.Add("Phone", '%' + txtPhone.Text.Trim() + '%');
        dict.Add("BegCreateDate", txtBegCreateDate.Text.Trim());
        dict.Add("EndCreateDate", txtEndCreateDate.Text.Trim());
        dict.Add("City", ddlCity.SelectedValue);
        dict.Add("sex", rdoSex.SelectedValue);
        dict.Add("Age", ddlAge.SelectedValue);
        //dict.Add("Area", HFD_Area.Value);
        dict.Add("ServiceUser", SessionInfo.UserID.ToString());

        DataRow dr = null;
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count == 0)
        {
            return "0";
        }
        dr = dt.Rows[0];

        int[] Yvalue = new int[dt.Rows.Count];
        string[] Xvalue = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            Xvalue[i] = dt.Rows[i]["縣市"].ToString() + dt.Rows[i]["區間"].ToString();
            Yvalue[i] = int.Parse(dt.Rows[i]["Cnt"].ToString());
        }
        Chart1.Series["Default"].Points.DataBindXY(Xvalue, Yvalue);
        Chart1.Series["Default"].ChartType = SeriesChartType.Pie;
        Chart1.Series["Default"]["PieLabelStyle"] = "Outside";


        return dr["Cnt"].ToString();
    }
    //-------------------------------------------------------------------------------------------------------------
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        LoadFormData();
        getCityaAreaCnt(ddlCity.SelectedValue);
        lblnumber.Text = GetReportAreaCnt();
    }

    //-------------------------------------------------------------------------------------------------------------
    //kuofly 20150211 畫面上方的區塊
    private string GetReportAreaCnt()
    {
        string strTemp = "";
        string retStr = "";

        //查詢資料庫
        DataTable dt = GetDataTable1();

        //統計筆數
        if (dt.Rows.Count != 0)
        {
            sum = 0;
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    sum += int.Parse(dr["cnt"].ToString());
                }
                catch (Exception ex)
                {
                    //忽略所有的計算意外
                }
            }
        }
        //GetDataTableSum();

        //查詢城市代碼名稱
        string Age = "";
        string Sex = "";
        string Firstphone = "";
        string time = "";

        if (ddlAge.SelectedValue != "")
        {
            Age += "約" + ddlAge.SelectedValue + "歲";
        }
        if (rdoSex.SelectedValue != "" && rdoSex.SelectedValue != "全部")
        {
            Sex += rdoSex.SelectedValue + "生";
        }
        if (CKB_Firstphone.Checked == true)
        {
            Firstphone = "第一次來電";
        }
        if (txtBegCreateDate.Text != "" && txtEndCreateDate.Text != "")
        {
            time = txtBegCreateDate.Text + "~" + txtEndCreateDate.Text;
        }
        foreach (DataRow dr in dt.Rows)
        {
            string city = "";
            if (ddlCity.SelectedValue != "")
                city += dr["city"].ToString();
            strTemp = city;
            strTemp += " 個案數分布表";
            if (time != "" || Firstphone != "" || Age != "")
            {
                strTemp += "(" + time + Firstphone + Age + Sex + ")";
            }
        }
        strTemp = (strTemp == "") ? "&nbsp" : strTemp;

        retStr += "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" align=\"center\" style=\"font-size:12px;font-family:標楷體;width:100%;\">\n";
        retStr += "\t<tr>\n";
        retStr += "\t\t<td colspan=\"4\" style=\"text-align:center;font-size:24px;font-family:標楷體;border-style:none;font-weight:bold;\">";
        retStr += strTemp;
        retStr += "</td>\n";
        retStr += "\t</tr>\n";
        // 第一行 ******************************************************
        if (dt.Rows.Count != 0)
        {
            retStr += "\t<tr>\n";
            retStr += "\t\t<td style=\"text-align:center;font-size:18px;border-left:1px solid windowtext;border-top:1px solid windowtext;border-right:1px solid windowtext;border-bottom:1px solid windowtext;\">";
            retStr += "縣市</td>\n";
            retStr += "\t\t<td style=\"text-align:center;font-size:18px;border-left:1px solid windowtext;border-top:1px solid windowtext;border-right:1px solid windowtext;border-bottom:1px solid windowtext;\">";
            retStr += "區域</td>\n";
            retStr += "\t\t<td style=\"text-align:center;font-size:18px;border-left:1px solid windowtext;border-top:1px solid windowtext;border-right:1px solid windowtext;border-bottom:1px solid windowtext;\">";
            retStr += "筆數</td>\n";
            retStr += "\t\t<td style=\"text-align:center;font-size:18px;border-left:1px solid windowtext;border-top:1px solid windowtext;border-right:1px solid windowtext;border-bottom:1px solid windowtext;\">";
            retStr += "百分比</td>\n";
	        retStr += "\t</tr>\n";
        }
        // 第二行 ******************************************************

        foreach (DataRow dr in dt.Rows)
        {
            retStr += "\t<tr>\n";
            retStr += "\t\t<td style=\"text-align:center;font-size:18px;border-left:1px solid windowtext;border-top:1px solid windowtext;border-right:1px solid windowtext;border-bottom:1px solid windowtext;\">";
            retStr += dr["city"].ToString();
            retStr += "</td>\n";
            retStr += "\t\t<td style=\"text-align:center;font-size:18px;border-left:1px solid windowtext;border-top:1px solid windowtext;border-right:1px solid windowtext;border-bottom:1px solid windowtext;\">";
            retStr += dr["area"].ToString();
            retStr += "</td>\n";
            retStr += "\t\t<td style=\"text-align:center;font-size:18px;border-left:1px solid windowtext;border-top:1px solid windowtext;border-right:1px solid windowtext;border-bottom:1px solid windowtext;\">";
            retStr += dr["cnt"].ToString();
            retStr += "</td>\n";
            retStr += "\t\t<td style=\"text-align:center;font-size:18px;border-left:1px solid windowtext;border-top:1px solid windowtext;border-right:1px solid windowtext;border-bottom:1px solid windowtext;\">";
            retStr += Math.Round(((float.Parse(dr["Cnt"].ToString()) / sum) * 100), 2, MidpointRounding.AwayFromZero).ToString() + "%"; 
            retStr += "</td>\n";
            retStr += "\t</tr>\n";
  
        }
        //*************************************
        //轉成 html 碼
        retStr += "</table>\n";
        return retStr;
    }
    //-------------------------------------------------------------------------------------------------------------
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
        //查詢城市代碼名稱
        string Age = "";
        string Sex = "";
        string Firstphone = "";
        string time = "";
        string Christian = "";

        if (ddlAge.SelectedValue != "")
        {
            Age += "約" + ddlAge.SelectedValue + "歲";
        }
        if (rdoSex.SelectedValue != "" && rdoSex.SelectedValue != "全部")
        {
            Sex += rdoSex.SelectedValue + "生";
        }
        if (CKB_Firstphone.Checked == true)
        {
            Firstphone = "第一次來電";
        }
        if (CKB_Christian.Checked.ToString() == "True")
        {
            Christian += " 是基督徒";
        }
        if (txtBegCreateDate.Text != "" && txtEndCreateDate.Text != "")
        {
            time = txtBegCreateDate.Text + "~" + txtEndCreateDate.Text;
        }
        foreach (DataRow dr in dt.Rows)
        {
            string city = "";
            if (ddlCity.SelectedValue != "")
                city += dr["縣市"].ToString();
            strTemp = city;
            strTemp += " 個案數分布表";
            if (time != "" || Firstphone != "" || Age != "" || Christian != "")
            {
                strTemp += "(" + time + Firstphone + Christian + Age + Sex + ")";
            }
        }

        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        //教會跨行置中
        cell.ColSpan = 5;
        //cell.ColSpan = 8;
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
    //-------------------------------------------------------------------------------------------------------------
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
                        "縣市",
                        "區域",
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
            css.Add("mso-number-format", "\\@");
            Util.AddTDLine(css, 1234);
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            cell.InnerHtml = dr["縣市"].ToString();
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 1234);
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            cell.InnerHtml = dr["區域"].ToString();
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
        //姓名,備註,電話,縣市,區域

        //讀取資料庫
        string strSql = @"
                        select distinct a.MemberUID,
                        b.cname 姓名,
	                    b.Memo 備註,
	                    b.Phone 電話,
                        isnull((select top 1 name from codecity where zipcode=b.city),'未知縣市') 縣市,
                        isnull((select top 1 name from codecity where zipcode=b.area),'未知鄉鎮') 區域 
                        from Consulting a  
                        inner  join Member b on a.MemberUID = b.uid 
                        where 1=1 
                        and isnull(a.IsDelete, '') != 'Y'   
                    ";
        //內部條件:管理者
        if (SessionInfo.GroupName != "系統管理員")
        {
            strSql += " and a.ServiceUser = @ServiceUser\n";
        }
        //輸入條件1:縣市
        if (ddlCity.SelectedValue != "")
        {
            strSql += " and b.City = @City\n";
        }
        //輸入條件2:電話
        if (txtPhone.Text.Trim() != "")
        {
            strSql += " and b.Phone like @Phone\n";
        }
        //輸入條件3:姓名
        if (txtName.Text.Trim() != "")
        {
            strSql += " and b.CName like @CName\n";
        }
        //輸入條件4:性別
        if (rdoSex.SelectedValue != "" && rdoSex.SelectedValue != "全部")
        {
            strSql += " and b.sex =@sex\n";
        }
        //輸入條件5:年齡層 (例如:'0~10'
        if (ddlAge.SelectedValue != "")
        {
            strSql += " and Age =@Age\n";
        }
        //輸入條件6:建檔時間
        if (txtBegCreateDate.Text.Trim() != "" && txtEndCreateDate.Text.Trim() != "")
        {
            strSql += " and CONVERT(varchar, CreateDate, 111) Between @BegCreateDate And @EndCreateDate\n";
        }
        //輸入條件7:是否為第一次來電
        //kuofly 20150211 修正第一次來電的邏輯
        if (CKB_Firstphone.Checked.ToString() == "True")
        {
            strSql += @"  and a.MemberUID + '_' + (select replace(convert(varchar(8), a.CreateDate, 112)+convert(varchar(8), CreateDate, 114), ':',''))
                                in
	                                (
		                                select 使用者鍵值 from 
		                                (
		                                select Memberuid+ '_' + min(replace(convert(varchar(8), CreateDate, 112)+convert(varchar(8), CreateDate, 114), ':','')) 使用者鍵值,convert(varchar,MIN(CreateDate),111) 第一次來電日期 
		                                from Consulting where isnull(MemberUID,'')<>'' and isnull(IsDelete, '') != 'Y' 
		                                group by Memberuid 
		                                ) table1
		                                where 1=1";
            if (txtBegCreateDate.Text.Trim() != "" && txtEndCreateDate.Text.Trim() != "")
            {
                strSql += " and 第一次來電日期 Between @BegCreateDate And @EndCreateDate\n";
            }
            strSql += @"        and 使用者鍵值 is not null
	                                )
                            ";
        }
        //輸入條件8:是否教會弟兄姊妹
        if (CKB_Christian.Checked.ToString() == "True")
        {
            strSql += " and b.Christian='是'";
        }
        strSql += " Order by 4,5";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("CName", "%" + txtName.Text.Trim() + "%");
        dict.Add("Phone", '%' + txtPhone.Text.Trim() + '%');
        dict.Add("BegCreateDate", txtBegCreateDate.Text.Trim());
        dict.Add("EndCreateDate", txtEndCreateDate.Text.Trim());
        dict.Add("City", ddlCity.SelectedValue);
        dict.Add("sex", rdoSex.SelectedValue);
        dict.Add("Age", ddlAge.SelectedValue);
        dict.Add("ServiceUser", SessionInfo.UserID.ToString());
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        return dt;
    }

    private DataTable GetDataTable1()
    {
        //讀取資料庫
        string strSql = @" 
select 
city as city,
area as area,
count(distinct MemberUID) as cnt
from (
	select a.MemberUID,
	isnull((select top 1 name from codecity where zipcode=b.city),'') city,
	isnull((select top 1 name from codecity where zipcode=b.area),'') area 
	from Consulting a  
	left join Member b on a.MemberUID = b.uid 
	where 1=1
    and isnull(a.IsDelete, '') != 'Y'";
        //內部條件:管理者
        if (SessionInfo.GroupName != "系統管理員")
        {
            strSql += " and a.ServiceUser = @ServiceUser\n";
        }
        //輸入條件1:縣市
        if (ddlCity.SelectedValue != "")
        {
            strSql += " and b.City = @City\n";
        }
        //輸入條件2:電話
        if (txtPhone.Text.Trim() != "")
        {
            strSql += " and b.Phone like @Phone\n";
        }
        //輸入條件3:姓名
        if (txtName.Text.Trim() != "")
        {
            strSql += " and b.CName like @CName\n";
        }
        //輸入條件4:性別
        if (rdoSex.SelectedValue != "" && rdoSex.SelectedValue != "全部")
        {
            strSql += " and b.sex =@sex\n";
        }
        //輸入條件5:年齡層 (例如:'0~10'
        if (ddlAge.SelectedValue != "")
        {
            strSql += " and Age =@Age\n";
        }
        //輸入條件6:建檔時間
        if (txtBegCreateDate.Text.Trim() != "" && txtEndCreateDate.Text.Trim() != "")
        {
            strSql += " and CONVERT(varchar, CreateDate, 111) Between @BegCreateDate And @EndCreateDate\n";
        }
        //輸入條件7:是否為第一次來電
        //kuofly 20150211 修正第一次來電的邏輯
        if (CKB_Firstphone.Checked.ToString() == "True")
        {
            strSql += @"  and a.MemberUID + '_' + (select replace(convert(varchar(8), a.CreateDate, 112)+convert(varchar(8), CreateDate, 114), ':',''))
                                in
	                                (
		                                select 使用者鍵值 from 
		                                (
		                                select Memberuid+ '_' + min(replace(convert(varchar(8), CreateDate, 112)+convert(varchar(8), CreateDate, 114), ':','')) 使用者鍵值,convert(varchar,MIN(CreateDate),111) 第一次來電日期 
		                                from Consulting where isnull(MemberUID,'')<>'' and isnull(IsDelete, '') != 'Y' 
		                                group by Memberuid 
		                                ) table1
		                                where 1=1";
            if (txtBegCreateDate.Text.Trim() != "" && txtEndCreateDate.Text.Trim() != "")
            {
                strSql += " and 第一次來電日期 Between @BegCreateDate And @EndCreateDate\n";
            }
            strSql += @"        and 使用者鍵值 is not null
	                                )
                            ";
        }
        //輸入條件8:是否教會弟兄姊妹
        if (CKB_Christian.Checked.ToString() == "True")
        {
            strSql += " and b.Christian='是'";
        }

        strSql += @"
	and convert(varchar,a.CreateDate,111) between @BegCreateDate And @EndCreateDate
) table1
where 1=1  
GROUP BY city,area";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("CName", "%" + txtName.Text.Trim() + "%");
        dict.Add("Phone", '%' + txtPhone.Text.Trim() + '%');
        dict.Add("BegCreateDate", txtBegCreateDate.Text.Trim());
        dict.Add("EndCreateDate", txtEndCreateDate.Text.Trim());
        dict.Add("City", ddlCity.SelectedValue);
        dict.Add("sex", rdoSex.SelectedValue);
        dict.Add("Age", ddlAge.SelectedValue);
        //dict.Add("Area", HFD_Area.Value);
        dict.Add("ServiceUser", SessionInfo.UserID.ToString());
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        return dt;

    }
    //-------------------------------------------------------------------------------------------------------------
    private DataTable GetDataTableSum()
    {
        //讀取資料庫
        string strSql = @" Select cc1.zipcode as AreaCode, (select  name from codecity where ZipCode=cc.ZipCode ) as city ,CC1.Name as area,(memberuid)
                       into #1
                        from Consulting a  inner join Member b on a.MemberUID = b.uid 
                        inner join CodeCity CC on CC.ZipCode =City
                        inner join CodeCity CC1 on CC1.ZipCode = Area
                        Where 1=1 
                          ";

        if (rdoSex.SelectedValue != "" && rdoSex.SelectedValue != "全部")
        {
            strSql += " and sex =@sex\n";
        }

        if (ddlAge.SelectedValue != "")
        {
            strSql += " and Age =@Age\n";
        }
        if (ddlCity.SelectedValue != "")
        {
            strSql += " and City = @City\n";
        }
        if (txtBegCreateDate.Text != "" && txtEndCreateDate.Text != "")
        {
            strSql += " and CONVERT(varchar(100), CreateDate, 111) Between @BegCreateDate And @EndCreateDate\n";
        }
        if (CKB_Firstphone.Checked.ToString() == "True")
        {
            strSql += @"  and b.uid in
                        (
                        select  memberuid from Consulting where 1=1 and isnull(IsDelete, '') != 'Y' 
                        and CONVERT(varchar(10), CreateDate, 111)  Between @BegCreateDate And @EndCreateDate
                        group by memberuid  having  count(memberuid)=1 
                        )";
        }
        strSql += "  group by cc.zipcode,CC1.Name,memberuid , cc1.zipcode ";
        strSql += @"
                   Select AreaCode, city , area,count(city) as cnt 
into #2
                    from  #1
                    group by city, area,AreaCode
                    order by AreaCode
                    drop table #1

                    Select sum(cnt) As sum
                    from #2
                    Where  1=1 
                    drop table #2
                    ";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("CName", "%" + txtName.Text.Trim() + "%");
        dict.Add("Phone", '%' + txtPhone.Text.Trim() + '%');
        dict.Add("BegCreateDate", txtBegCreateDate.Text.Trim());
        dict.Add("EndCreateDate", txtEndCreateDate.Text.Trim());
        dict.Add("City", ddlCity.SelectedValue);
        dict.Add("sex", rdoSex.SelectedValue);
        dict.Add("Age", ddlAge.SelectedValue);
        //dict.Add("Area", HFD_Area.Value);
        dict.Add("ServiceUser", SessionInfo.UserID.ToString());
        DataTable dt1 = NpoDB.GetDataTableS(strSql, dict);
        foreach (DataRow dr in dt1.Rows)
        {
            if (dr["sum"].ToString() == "")
            {
                sum = 0;
            }
            else
            {
                sum = int.Parse(dr["sum"].ToString());
            }
        }


        return dt1;
    }
}
