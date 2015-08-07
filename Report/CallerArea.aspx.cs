//修正第一次來電邏輯，看不懂就不要改免的資料查詢結果都亂掉
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
using System.Web.UI.DataVisualization.Charting;

public partial class CallerArea : BasePage
{
    GroupAuthrity ga = null;
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
    //********************************************************************************************
    }
        //-------------------------------------------------------------------------
    private string getCityaAreaCnt(string city)
    {
        string strSql = @" 
        select isnull(city,'未輸入縣市') 縣市,isnull(area,'未輸入區間') 區間,COUNT(*) cnt 
         from (
                select 
                a.uid as uid, 
                b.CName As  姓名,
                b.Memo 備註,
                b.Phone As 電話, 
                convert(varchar(19), CreateDate,120) as 建檔時間, 
                (select Name from CodeCity where CodeCity.ZipCode=b.city) city,
                (select Name from CodeCity where CodeCity.ZipCode=b.area) area,
                ConsultantMain [來電諮詢別(大類)],
                ConsultantItem [來電諮詢別(分項)]
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

        strSql += @") table1
          group by city,area 
          order by charindex(city,'台北市新北市隆市宜蘭縣桃園市新竹市新竹縣苗栗縣台中市彰化縣南投縣雲林縣嘉義市嘉義縣台南市高雄市屏東縣台東縣花蓮縣澎湖縣金門縣連江縣'),area
        ";


        DataRow dr = null;
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
        if (dt.Rows.Count == 0)
        {
            return "0";
        }
        dr = dt.Rows[0];

            int[] Yvalue = new int[dt.Rows.Count];
            string[] Xvalue = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //這行是大餅圖的條件
                Xvalue[i] = dt.Rows[i]["縣市"].ToString() +dt.Rows[i]["區間"].ToString();
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
    //kuofly 20150211 這個應該是大餅圖上方
    private string GetReportAreaCnt()
    {
        //string strTemp = "";
        string retHTML = "";
        //HtmlTable table = new HtmlTable();
        //HtmlTableRow row;
        //HtmlTableCell cell;
        //CssStyleCollection css;
        //查詢資料庫

        
        DataTable dt = GetDataTable1();
        //將DT的結果自行計算總計的sum
        if (dt.Rows.Count != 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    sum += int.Parse(dr["cnt"].ToString());
                }catch(Exception ex){
                    //忽略所有的計算意外
                }
            }
        }
        //GetDataTableSum();

        //string FontSize = "14px";
        //table.Border = 0;
        //table.CellPadding = 0;
        //table.CellSpacing = 0;
        //table.Align = "center";
        //css = table.Style;
        //css.Add("font-size", "12px");
        //css.Add("font-family", "標楷體");
        //css.Add("width", "100%");
        //string strFontSize = "18px";
        //// 標題 ****************************************
        //row = new HtmlTableRow();
        //cell = new HtmlTableCell();

        retHTML += "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" align=\"center\" style=\"font-size:12px;font-family:標楷體;width:100%;\">\n";

        retHTML += "\t<tr>\n";
        retHTML += "\t\t<td colspan=\"4\" style=\"text-align:center;font-size:24px;font-family:標楷體;border-style:none;font-weight:bold;\">";
        retHTML += " 來電數區域分布表";
        retHTML += "</td>\n";
        retHTML += "\t</tr>\n";
        // strTemp = " 分布表";
        //cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        //css = cell.Style;
        //cell.ColSpan = 4;
        ////cell.ColSpan = 8;
        //css.Add("text-align", "center");
        //css.Add("font-size", "24px");
        //css.Add("font-family", "標楷體");
        //css.Add("border-style", "none");
        //css.Add("font-weight", "bold");
        //row.Cells.Add(cell);
        //table.Rows.Add(row);
        // 第一行 ******************************************************
        if (dt.Rows.Count != 0)
        {
            //row = new HtmlTableRow();
            //cell = new HtmlTableCell();
            //css = cell.Style;
            //cell.InnerHtml = "縣市";
            //css.Add("text-align", "center");            //excel文字置中
            //css.Add("font-size", strFontSize);          //excel文字大小
            //Util.AddTDLine(css, 1234);                  //excel 一個儲存格左(1)上(2)右(3)下(4) 框線
            //row.Cells.Add(cell);

            //cell = new HtmlTableCell();
            //css = cell.Style;
            //cell.InnerHtml = "區域";
            //css.Add("text-align", "center");            //excel文字置中
            //css.Add("font-size", strFontSize);          //excel文字大小
            //Util.AddTDLine(css, 1234);                  //excel 一個儲存格左(1)上(2)右(3)下(4) 框線
            //row.Cells.Add(cell);

            //cell = new HtmlTableCell();
            //css = cell.Style;
            //cell.InnerHtml = "筆數";
            //css.Add("text-align", "center");            //excel文字置中
            //css.Add("font-size", strFontSize);          //excel文字大小
            //Util.AddTDLine(css, 1234);                  //excel 一個儲存格左(1)上(2)右(3)下(4) 框線
            //row.Cells.Add(cell);

            //cell = new HtmlTableCell();
            //css = cell.Style;
            //cell.InnerHtml = "百分比";
            //css.Add("text-align", "center");            //excel文字置中
            //css.Add("font-size", strFontSize);          //excel文字大小
            //Util.AddTDLine(css, 1234);                  //excel 一個儲存格左(1)上(2)右(3)下(4) 框線
            //row.Cells.Add(cell);

            //table.Rows.Add(row);
            retHTML += "\t<tr>\n";
            retHTML += "\t\t<td style=\"text-align:center;font-size:18px;border-left:1px solid windowtext;border-top:1px solid windowtext;border-right:1px solid windowtext;border-bottom:1px solid windowtext;\">縣市</td>\n";
            retHTML += "\t\t<td style=\"text-align:center;font-size:18px;border-left:1px solid windowtext;border-top:1px solid windowtext;border-right:1px solid windowtext;border-bottom:1px solid windowtext;\">區域</td>\n";
            retHTML += "\t\t<td style=\"text-align:center;font-size:18px;border-left:1px solid windowtext;border-top:1px solid windowtext;border-right:1px solid windowtext;border-bottom:1px solid windowtext;\">筆數</td>\n";
            retHTML += "\t\t<td style=\"text-align:center;font-size:18px;border-left:1px solid windowtext;border-top:1px solid windowtext;border-right:1px solid windowtext;border-bottom:1px solid windowtext;\">百分比</td>\n";
            retHTML += "\t</tr>\n";
        }
        // 第二行 ******************************************************

        foreach (DataRow dr in dt.Rows)
        {
            //row = new HtmlTableRow();
            //retHTML += "\t<tr>\n";
            //cell = new HtmlTableCell();
            //css = cell.Style;
            //cell.InnerHtml = dr["city"].ToString();
            //css.Add("text-align", "center");            //excel文字置中
            //css.Add("font-size", strFontSize);          //excel文字大小
            //Util.AddTDLine(css, 1234);                  //excel 一個儲存格左(1)上(2)右(3)下(4) 框線
            //row.Cells.Add(cell);
            //cell = new HtmlTableCell();
            //css = cell.Style;
            //cell.InnerHtml = dr["area"].ToString();
            //css.Add("text-align", "center");            //excel文字置中
            //css.Add("font-size", strFontSize);          //excel文字大小
            //Util.AddTDLine(css, 1234);                  //excel 一個儲存格左(1)上(2)右(3)下(4) 框線
            //row.Cells.Add(cell);
            //cell = new HtmlTableCell();
            //css = cell.Style;
            //cell.InnerHtml = dr["cnt"].ToString();
            //css.Add("text-align", "center");            //excel文字置中
            //css.Add("font-size", strFontSize);          //excel文字大小
            //Util.AddTDLine(css, 1234);                  //excel 一個儲存格左(1)上(2)右(3)下(4) 框線
            //row.Cells.Add(cell);

            //cell = new HtmlTableCell();
            //css = cell.Style;
            //cell.InnerHtml = Math.Round(((float.Parse(dr["Cnt"].ToString()) / sum) * 100), 2, MidpointRounding.AwayFromZero).ToString() + "%";
            //css.Add("text-align", "center");            //excel文字置中
            //css.Add("font-size", strFontSize);          //excel文字大小
            //Util.AddTDLine(css, 1234);                  //excel 一個儲存格左(1)上(2)右(3)下(4) 框線
            //row.Cells.Add(cell);
            //table.Rows.Add(row);
            retHTML += "\t<tr>\n";
            retHTML += "\t\t<td style=\"text-align:center;font-size:18px;border-left:1px solid windowtext;border-top:1px solid windowtext;border-right:1px solid windowtext;border-bottom:1px solid windowtext;\">";
            retHTML += dr["city"].ToString();  //縣市
            retHTML += "</td>\n";
            retHTML += "\t\t<td style=\"text-align:center;font-size:18px;border-left:1px solid windowtext;border-top:1px solid windowtext;border-right:1px solid windowtext;border-bottom:1px solid windowtext;\">";
            retHTML += dr["area"].ToString();  //區域
            retHTML += "</td>\n";
            retHTML += "\t\t<td style=\"text-align:center;font-size:18px;border-left:1px solid windowtext;border-top:1px solid windowtext;border-right:1px solid windowtext;border-bottom:1px solid windowtext;\">";
            retHTML += dr["cnt"].ToString(); //筆數
            retHTML += "</td>\n";
            retHTML += "\t\t<td style=\"text-align:center;font-size:18px;border-left:1px solid windowtext;border-top:1px solid windowtext;border-right:1px solid windowtext;border-bottom:1px solid windowtext;\">";
            //百分比
            retHTML += Math.Round(((float.Parse(dr["Cnt"].ToString()) / sum) * 100), 2, MidpointRounding.AwayFromZero).ToString() + "%";  
            retHTML += "</td>\n";
            retHTML += "\t</tr>\n";
        }
        retHTML += "</table>\n";
        //*************************************
        //轉成 html 碼
        //StringWriter sw = new StringWriter();
        //HtmlTextWriter htw = new HtmlTextWriter(sw);
        //table.RenderControl(htw);
        //return htw.InnerWriter.ToString();
        return retHTML;
    }
    
    //kuofly 20150211 重新修改整個邏輯
    private DataTable GetDataTable1()
    {
        //讀取資料庫
//        string strSql = @" 
//                        select  CC.Name as city,CC2.Name as area,count(*) as cnt
//                        from Consulting a  inner join Member b on a.MemberUID = b.uid inner join CodeCity CC on CC.ZipCode =City
//                        inner join CodeCity CC2 on CC2.ZipCode =Area 
//                        where 1=1 
//                        and isnull(a.IsDelete, '') != 'Y'
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
//        if (CKB_Christian.Checked.ToString() == "True")
//        {
//            strSql += " and Christian='是'";
//        }
//        strSql += "  group by CC.Name,CC2.Name ";
//        Dictionary<string, object> dict = new Dictionary<string, object>();
//        dict.Add("CName", "%" + txtName.Text.Trim() + "%");
//        dict.Add("Phone", '%' + txtPhone.Text.Trim() + '%');
//        dict.Add("BegCreateDate", txtBegCreateDate.Text.Trim());
//        dict.Add("EndCreateDate", txtEndCreateDate.Text.Trim());
//        dict.Add("City", ddlCity.SelectedValue);
//        dict.Add("sex", rdoSex.SelectedValue);
//        dict.Add("Age", ddlAge.SelectedValue);
//        //dict.Add("Area", HFD_Area.Value);
//        dict.Add("ServiceUser", SessionInfo.UserID.ToString());
//        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
//        return dt;
        string strSql = @" 
        select city,area,COUNT(*) cnt 
         from (
                select 
                a.uid as uid, 
                b.CName As  姓名,
                b.Memo 備註,
                b.Phone As 電話, 
                convert(varchar(19), CreateDate,120) as 建檔時間, 
                (select Name from CodeCity where CodeCity.ZipCode=b.city) city,
                (select Name from CodeCity where CodeCity.ZipCode=b.area) area,
                ConsultantMain [來電諮詢別(大類)],
                ConsultantItem [來電諮詢別(分項)]
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
        
        strSql += @") table1
          group by city,area 
          order by charindex(city,'台北市新北市隆市宜蘭縣桃園市新竹市新竹縣苗栗縣台中市彰化縣南投縣雲林縣嘉義市嘉義縣台南市高雄市屏東縣台東縣花蓮縣澎湖縣金門縣連江縣'),area
        ";

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
    //-------------------------------------------------------------------------------------------------------------
    private DataTable GetDataTableSum()
    {
        //讀取資料庫
        string strSql = @" 
                        select count(*) as sum
                        from Consulting a  inner join Member b on a.MemberUID = b.uid inner join CodeCity CC on CC.ZipCode =City
                        inner join CodeCity CC2 on CC2.ZipCode =Area 
                        where 1=1 
                        and isnull(a.IsDelete, '') != 'Y'
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
        if (CKB_Christian.Checked.ToString() == "True")
        {
            strSql += " and Christian='是'";
        }
        //strSql += "  group by CC.Name,CC1.Name,memberuid ";
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
        string Age= "";
        string Sex = "";
        string Christian = "";
        string  Firstphone = "";
        string time= "";

        if (ddlAge.SelectedValue != "")
        {
            Age +="約"+ ddlAge.SelectedValue + "歲";
        }
        if (rdoSex.SelectedValue != "" && rdoSex.SelectedValue != "全部")
        {
            Sex += rdoSex.SelectedValue+"生";
        }
        if (CKB_Christian.Checked.ToString() == "True")
        {
            Christian += " 是基督徒";
        }
        if (CKB_Firstphone.Checked == true)
        {
            Firstphone = "第一次來電";
        }
        if (txtBegCreateDate.Text != "" && txtEndCreateDate.Text !="")
        {
            time = txtBegCreateDate.Text + "~" + txtEndCreateDate.Text;
        }
        foreach (DataRow dr in dt.Rows)
        {
            string city = "";
            if (ddlCity.SelectedValue !="")
                city += dr["縣市"].ToString();
        strTemp =  city;
        strTemp += " 來電數區域分布表";
        if (time != "" || Firstphone != "" || Age != "" || Christian!="")
        {
            strTemp += "(" + time + Firstphone + Age + Sex + Christian + ")";
        }
        }

        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp; 
        css = cell.Style;
        //教會跨行置中
        cell.ColSpan = 8;
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
                        "建檔時間",
                        "縣市",
                        "區域",
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
            else
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
            cell.InnerHtml = dr["建檔時間"].ToString();
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
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
//        string strSql = @"
//                        select a.uid as uid, CName As  姓名,b.Memo as 備註,Phone As 電話, convert(varchar(19), CreateDate,120) as 建檔時間, (select  name from codecity where ZipCode=cc.ZipCode ) as 縣市,
//                        CC2.Name as 區域,
//                         REVERSE(SUBSTRING(REVERSE(ConsultantMain),CHARINDEX(',',REVERSE(ConsultantMain))+1,LEN(ConsultantMain))) [來電諮詢別(大類)],
//                         REVERSE(SUBSTRING(REVERSE(ConsultantItem),CHARINDEX(',',REVERSE(ConsultantItem))+1,LEN(ConsultantItem))) [來電諮詢別(分項)]
//                        from Consulting a  inner join Member b on a.MemberUID = b.uid inner join CodeCity CC on CC.ZipCode =City
//                        inner join CodeCity CC2 on CC2.ZipCode =Area 
//                        where 1=1 
//                        and isnull(a.IsDelete, '') != 'Y'
//
//                    ";


//        if (SessionInfo.GroupName == "系統管理員")
//        {
//        }
//        else
//        {
//            strSql += " and ServiceUser = @ServiceUser\n";
//        }
//        if (ddlCity.SelectedValue  != "")
//        {
//            strSql += " and City = @City\n";
//        }
//        if (txtPhone.Text.Trim() != "")
//        {
//            strSql += " and Phone like @Phone\n";
//        }

//        if (txtName.Text.Trim() != "")
//        {
//            strSql += " and CName like @CName\n";
//        }
//        if (rdoSex.SelectedValue != "" && rdoSex.SelectedValue != "全部")
//        {
//            strSql += " and sex =@sex\n";
//        }

//        if (ddlAge.SelectedValue != "")
//        {
//            strSql += " and Age =@Age\n";
//        }
//        if (txtBegCreateDate.Text.Trim() != "" && txtEndCreateDate.Text.Trim() != "")
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
//        if (CKB_Christian.Checked.ToString() == "True")
//        {
//            strSql += " and Christian='是'";
//        }
//        //strSql += " GROUP BY cc.zipcode";
//        strSql += " Order by cc.zipcode,CreateDate desc";
//        //Response.Write(strSql);
//        //Response.end
//        Dictionary<string, object> dict = new Dictionary<string, object>();
//        dict.Add("CName", "%" + txtName.Text.Trim() + "%");
//        dict.Add("Phone", '%' + txtPhone.Text.Trim() + '%');
//        dict.Add("BegCreateDate", txtBegCreateDate.Text.Trim());
//        dict.Add("EndCreateDate", txtEndCreateDate.Text.Trim());
//        dict.Add("City", ddlCity.SelectedValue);
//        dict.Add("sex", rdoSex.SelectedValue);
//        dict.Add("Age", ddlAge.SelectedValue);
//        //dict.Add("Area", HFD_Area.Value);
//        dict.Add("ServiceUser", SessionInfo.UserID.ToString());
//        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
//        return dt;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        string strSql = @"
                        select 
                        a.uid as uid, 
                        b.CName As  姓名,
                        b.Memo 備註,
                        b.Phone As 電話, 
                        convert(varchar(19), CreateDate,120) as 建檔時間, 
                        (select Name from CodeCity where CodeCity.ZipCode=b.city) 縣市,
                        (select Name from CodeCity where CodeCity.ZipCode=b.area) 區域,
                        ConsultantMain [來電諮詢別(大類)],
                        ConsultantItem [來電諮詢別(分項)]
                        from Consulting a  
                        left join Member b on a.MemberUID = b.uid 
                        where 1=1 
                        and isnull(a.IsDelete, '') != 'Y'
            ";
        //內部條件:管理者
        if (SessionInfo.GroupName != "系統管理員"){
            strSql += " and a.ServiceUser = @ServiceUser\n";
        }
        //輸入條件1:縣市
        if (ddlCity.SelectedValue  != ""){
           strSql += " and b.City = @City\n";
        }
        //輸入條件2:電話
        if (txtPhone.Text.Trim() != ""){
            strSql += " and b.Phone like @Phone\n";
        }
        //輸入條件3:姓名
        if (txtName.Text.Trim() != ""){
            strSql += " and b.CName like @CName\n";
        }
        //輸入條件4:性別
        if (rdoSex.SelectedValue != "" && rdoSex.SelectedValue != "全部"){
            strSql += " and b.sex =@sex\n";
        }
        //輸入條件5:年齡層 (例如:'0~10'
        if (ddlAge.SelectedValue != ""){
            strSql += " and Age =@Age\n";
        }
        //輸入條件6:建檔時間
        if (txtBegCreateDate.Text.Trim() != "" && txtEndCreateDate.Text.Trim() != ""){
            strSql += " and CONVERT(varchar, CreateDate, 111) Between @BegCreateDate And @EndCreateDate\n";
        }
        //輸入條件7:是否為第一次來電
        //kuofly 20150211 修正第一次來電的邏輯 
        if (CKB_Firstphone.Checked.ToString() == "True"){
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
            if (txtBegCreateDate.Text.Trim() != "" && txtEndCreateDate.Text.Trim() != ""){
                strSql += " and 第一次來電日期 Between @BegCreateDate And @EndCreateDate\n";
            }
            strSql += @"        and 使用者鍵值 is not null
	                            )
                        ";
        }
        //輸入條件8:是否教會弟兄姊妹
        if (CKB_Christian.Checked.ToString() == "True"){
                    strSql += " and b.Christian='是'";
        }
        strSql += " Order by convert(varchar,a.CreateDate,111) desc,b.CName,b.city,b.area,b.zipcode,b.Phone";
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
}
