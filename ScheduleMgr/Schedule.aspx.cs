using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic ;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.IO;

public partial class ScheduleMgr_Schedule : BasePage 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["ProgID"] = "MoveMgr";

        if (!IsPostBack)
        {
            LoadDropDownListData();
            LoadFormData();
            setButton();
        }
    }
    //設定編輯班別的按鈕
    public void setButton()
    {
        bool strEnable;
        //若是在個人班表 Table ScheduleDetail 的該年 與該季 有資料的話 即鎖定 每季的排班表不得再編輯
        if (GetQuarterDateCount(ddlYear.SelectedValue, ddlQuerter.SelectedValue) != "0")
        {
            strEnable = true;
        }
        else
        {
            strEnable=true;
        }
        btnMon1.Enabled = strEnable;
        btnMon2.Enabled = strEnable;
        btnMon3.Enabled = strEnable;
        btnTue1.Enabled = strEnable;
        btnTue2.Enabled = strEnable;
        btnTue3.Enabled = strEnable;
        btnWed1.Enabled = strEnable;
        btnWed2.Enabled = strEnable;
        btnWed3.Enabled = strEnable;
        btnThu1.Enabled = strEnable;
        btnThu2.Enabled = strEnable;
        btnThu3.Enabled = strEnable;
        btnFri1.Enabled = strEnable;
        btnFri2.Enabled = strEnable;
        btnFri3.Enabled = strEnable;
        btnSat1.Enabled = strEnable;
        btnSat2.Enabled = strEnable;
        btnSat3.Enabled = strEnable;
        btnSun1.Enabled = strEnable;
        btnSun2.Enabled = strEnable;
        btnSun3.Enabled = strEnable;
    }

    //設定 年 與 季 的 DownList
    public void LoadDropDownListData()
    {
        
        LoadDownListYear(ddlYear, (DateTime.Now.Year-1) , "Desc");
        //ddlQuerter.Items.Add(new ListItem("1","1"));
        ddlQuerter.Items.Add(new ListItem("1","1"));
        ddlQuerter.Items.Add(new ListItem("2","2"));
        ddlQuerter.Items.Add(new ListItem("3","3"));
        ddlQuerter.Items.Add(new ListItem("4","4"));


    }
    //---------------------------------------------------------------------------
    public void LoadFormData()
    {
        ClearLabel(); //清空 所有的班表上的名字
        //查詢年與季的班表
        string strSql = @"
                           Select UserName,DayOfWeek+periodID As DayClass from Schedule sh 
                           inner join ScheduleMap sm on sh.uid = sm.ScheduleID 
                           inner join AdminUser AU on au.UserID=sm.UserID 
                           --inner join ScheduleDetail SD on sd.UserID =au.UserID 
                           where 
                            SchYear = @SchYear
                           And Quarter =@Quarter
                          order by DayOfWeek,periodID
                        ";

        DataRow dr = null;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("SchYear", ddlYear.SelectedValue);
        //dict.Add("BegCreateDate", txtBegCreateDate.Text);
        //dict.Add("EndCreateDate", txtEndCreateDate.Text);
        dict.Add("Quarter", ddlQuerter.SelectedValue );
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        //if (txtBegCreateDate.Text.Trim() != "" && txtEndCreateDate.Text.Trim() != "")
        //{
        //    strSql += " and CONVERT(varchar(100), CreateDate, 111) Between @BegCreateDate And @EndCreateDate\n";
        //}
        if (dt.Rows.Count != 0)
        {
            dr = dt.Rows[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                switch (dt.Rows[i]["DayClass"].ToString())
                {
                    case "MonA":
                        lblMon1.Text += dt.Rows[i]["UserName"].ToString() + "<br>";
                        break;
                    case "MonB":
                        lblMon2.Text += dt.Rows[i]["UserName"].ToString() + "<br>";
                        break;
                    case "MonC":
                        lblMon3.Text += dt.Rows[i]["UserName"].ToString() + "<br>";
                        break;
                    case "TueA":
                        lblTue1.Text += dt.Rows[i]["UserName"].ToString() + "<br>";
                        break;
                    case "TueB":
                        lblTue2.Text += dt.Rows[i]["UserName"].ToString() + "<br>";
                        break;
                    case "TueC":
                        lblTue3.Text += dt.Rows[i]["UserName"].ToString() + "<br>";
                        break;
                    case "WedA":
                        lblWed1.Text += dt.Rows[i]["UserName"].ToString() + "<br>";
                        break;
                    case "WedB":
                        lblWed2.Text += dt.Rows[i]["UserName"].ToString() + "<br>";
                        break;
                    case "WedC":
                        lblWed3.Text += dt.Rows[i]["UserName"].ToString() + "<br>";
                        break;
                    case "ThuA":
                        lblThu1.Text += dt.Rows[i]["UserName"].ToString() + "<br>";
                        break;
                    case "ThuB":
                        lblThu2.Text += dt.Rows[i]["UserName"].ToString() + "<br>";
                        break;
                    case "ThuC":
                        lblThu3.Text += dt.Rows[i]["UserName"].ToString() + "<br>";
                        break;
                    case "FriA":
                        lblFri1.Text += dt.Rows[i]["UserName"].ToString() + "<br>";
                        break;
                    case "FriB":
                        lblFri2.Text += dt.Rows[i]["UserName"].ToString() + "<br>";
                        break;
                    case "FriC":
                        lblFri3.Text += dt.Rows[i]["UserName"].ToString() + "<br>";
                        break;
                    case "SatA":
                        lblSat1.Text += dt.Rows[i]["UserName"].ToString() + "<br>";
                        break;
                    case "SatB":
                        lblSat2.Text += dt.Rows[i]["UserName"].ToString() + "<br>";
                        break;
                    case "SatC":
                        lblSat3.Text += dt.Rows[i]["UserName"].ToString() + "<br>";
                        break;
                    case "SunA":
                        lblSun1.Text += dt.Rows[i]["UserName"].ToString() + "<br>";
                        break;
                    case "SunB":
                        lblSun2.Text += dt.Rows[i]["UserName"].ToString() + "<br>";
                        break;
                    case "SunC":
                        lblSun3.Text += dt.Rows[i]["UserName"].ToString() + "<br>";
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            ClearLabel();
            return;
        }
     }
    //----------------------------------------------------------------------------------------------------------
    //DownList的年份
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
    //----------------------------------------------------------------------------------------------------------
    //班表的名字 都清空
    private void ClearLabel()
    {
        lblMon1.Text = "";
        lblMon2.Text = "";
        lblMon3.Text = "";
        lblTue1.Text = "";
        lblTue2.Text = "";
        lblTue3.Text = "";
        lblWed1.Text = "";
        lblWed2.Text = "";
        lblWed3.Text = "";
        lblThu1.Text = "";
        lblThu2.Text = "";
        lblThu3.Text = "";
        lblFri1.Text = "";
        lblFri2.Text = "";
        lblFri3.Text = "";
        lblSat1.Text = "";
        lblSat2.Text = "";
        lblSat3.Text = "";
        lblSun1.Text = "";
        lblSun2.Text = "";
        lblSun3.Text = "";
    }
    //----------------------------------------------------------------------------------------------------------
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        LoadFormData();
        setButton();
    }
    //----------------------------------------------------------------------------------------------------------
    // 報表************************************************************************************************************************
    private string GetReport()
    {
        string[] Items = {
                        "星期",
                        "星期一",
                        "星期二",
                        "星期三",
                        "星期四",
                        "星期五",
                        "星期六",
                        "星期日",
                        
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
        string strTemp1 = "";// "" + strTitle + "";
        cell.InnerHtml = strTemp1;
        css = cell.Style;
        css.Add("text-align", "left");
        css.Add("font-size", "12px");
        cell.Width = "8";
        row.Cells.Add(cell);
        table.Rows.Add(row);
        //--------------------------------------------
        //第2行,資料顯示
        //*********早班*********
        string strFontSize = "12px";
        row = new HtmlTableRow();
        cell = new HtmlTableCell();
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "12px");
        Util.AddTDLine(css, 123);
        cell.InnerHtml = "早班";
        row.Cells.Add(cell);

        cell = new HtmlTableCell();
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 23);
        cell.InnerHtml = lblMon1.Text; 
        row.Cells.Add(cell);

        cell = new HtmlTableCell();
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 23);
        cell.InnerHtml = lblTue1.Text; 
        row.Cells.Add(cell);

        cell = new HtmlTableCell();
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 23);
        cell.InnerHtml = lblWed1.Text;
        row.Cells.Add(cell);

        cell = new HtmlTableCell();
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 23);
        cell.InnerHtml = lblThu1.Text;
        row.Cells.Add(cell);

        cell = new HtmlTableCell();
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 23);
        cell.InnerHtml = lblFri1.Text;
        row.Cells.Add(cell);

        cell = new HtmlTableCell();
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 23);
        cell.InnerHtml = lblSat1.Text;
        row.Cells.Add(cell);

        cell = new HtmlTableCell();
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 23);
        cell.InnerHtml = lblSun1.Text;
        row.Cells.Add(cell);
        table.Rows.Add(row);

        //*********中班*********
        row = new HtmlTableRow();
        cell = new HtmlTableCell();
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "12px");
        Util.AddTDLine(css, 123);
        cell.InnerHtml = "中班";
        row.Cells.Add(cell);

        cell = new HtmlTableCell();
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 23);
        cell.InnerHtml = lblMon2.Text;
        row.Cells.Add(cell);

        cell = new HtmlTableCell();
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 23);
        cell.InnerHtml = lblTue2.Text;
        row.Cells.Add(cell);

        cell = new HtmlTableCell();
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 23);
        cell.InnerHtml = lblWed2.Text;
        row.Cells.Add(cell);

        cell = new HtmlTableCell();
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 23);
        cell.InnerHtml = lblThu2.Text;
        row.Cells.Add(cell);

        cell = new HtmlTableCell();
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 23);
        cell.InnerHtml = lblFri2.Text;
        row.Cells.Add(cell);

        cell = new HtmlTableCell();
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 23);
        cell.InnerHtml = lblSat2.Text;
        row.Cells.Add(cell);

        cell = new HtmlTableCell();
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 23);
        cell.InnerHtml = lblSun2.Text;
        row.Cells.Add(cell);
        table.Rows.Add(row);

        //*********晚班*********
        row = new HtmlTableRow();
        cell = new HtmlTableCell();
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "12px");
        Util.AddTDLine(css, 1234);
        cell.InnerHtml = "晚班";
        row.Cells.Add(cell);

        cell = new HtmlTableCell();
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 234);
        cell.InnerHtml = lblMon3.Text;
        row.Cells.Add(cell);

        cell = new HtmlTableCell();
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 234);
        cell.InnerHtml = lblTue3.Text;
        row.Cells.Add(cell);

        cell = new HtmlTableCell();
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 234);
        cell.InnerHtml = lblWed3.Text;
        row.Cells.Add(cell);

        cell = new HtmlTableCell();
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 234);
        cell.InnerHtml = lblThu3.Text;
        row.Cells.Add(cell);

        cell = new HtmlTableCell();
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 234);
        cell.InnerHtml = lblFri3.Text;
        row.Cells.Add(cell);

        cell = new HtmlTableCell();
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 234);
        cell.InnerHtml = lblSat3.Text;
        row.Cells.Add(cell);

        cell = new HtmlTableCell();
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 234);
        cell.InnerHtml = lblSun3.Text;
        row.Cells.Add(cell);
        table.Rows.Add(row);
        //轉成 html 碼
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        table.RenderControl(htw);
        return htw.InnerWriter.ToString();
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

        string   FontSize = "14px";
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
        strTemp = ""+ ddlYear.SelectedValue  +"年熱線志工班表  第 "+ ddlQuerter.SelectedValue  +" 季 ";
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
    //列印班表
    protected void btnPrint_Click(object sender, EventArgs e)
    {
      
        lblRpt.Text =GetRptHead() + GetReport();
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
    //----------------------------------------------------------------------------------------------------------
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (GetDateCount(ddlYear.SelectedValue, ddlQuerter.SelectedValue) == "0")
        {
            ShowSysMsg("目前班表無資料請確認!");
            return;
        }
        // 格式化日期
        DateTime startDate = DateTime.Parse(GetQuarterDate (ddlYear.SelectedValue, ddlQuerter.SelectedValue,"Start"));
        DateTime endDate = DateTime.Parse(GetQuarterDate(ddlYear.SelectedValue, ddlQuerter.SelectedValue, "End"));

        DateTime currentDate = new DateTime();
        List<DateTime> matchDates = new List<DateTime>();

        //以季為單位 日期一天一天跑  照日期 Insert Into scheduleDetail  的 table       寫入 日期 班別 與 UserID
        for (int i = 0; i <= (endDate.Subtract(startDate).Days); i++)
        {
            currentDate = startDate.AddDays(i);
            switch (currentDate.DayOfWeek.ToString())
            {
                case "Monday":
                    InsertScheduleDetail(currentDate, ddlQuerter.SelectedValue.ToString(),"Mon");
                    break;
                case "Tuesday":
                    InsertScheduleDetail(currentDate, ddlQuerter.SelectedValue.ToString(),"Tue");
                    break;
                case "Wednesday":
                    InsertScheduleDetail(currentDate, ddlQuerter.SelectedValue.ToString(),"Wed");
                    break;
                case "Thursday":
                    InsertScheduleDetail(currentDate, ddlQuerter.SelectedValue.ToString(),"Thu");
                    break;
                case "Friday":
                    InsertScheduleDetail(currentDate, ddlQuerter.SelectedValue.ToString(),"Fri");
                    break;
                case "Saturday":
                    InsertScheduleDetail(currentDate, ddlQuerter.SelectedValue.ToString(),"Sat");
                    break;
                case "Sunday":
                    InsertScheduleDetail(currentDate, ddlQuerter.SelectedValue.ToString(),"Sun");
                    break;
                default :
                    break;
            }
        }
        setButton();       
        //ShowSysMsg("新增資料成功!");
        //Response.Redirect("Schedule.aspx");
    }
    //----------------------------------------------------------------------------------------------------------
    //寫入個人班表 ScheduleDetail 傳入 日期，季 與 星期幾
    private void InsertScheduleDetail(DateTime dtDate, string strQuarter, string strDayOfWeek)
    {
        DataRow dr = null;
        DataTable dt = null;
        //Select 出 班表 與  UserID 資訊
        string strSql = @"
                            select *  
                            from Schedule S
                            inner join ScheduleMap M
                            on S.uid=M.scheduleid
                            where DayOfWeek=@DayOfWeek
                            and SchYear=@SchYear
                            and Quarter=@Quarter
                            And isnull(S.IsDelete, '') != 'Y'
                        ";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("SchYear", ddlYear.SelectedValue);
        dict.Add("Quarter", ddlQuerter.SelectedValue);
        dict.Add("DayOfWeek", strDayOfWeek);
        dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count == 0)
        {
            //資料異常
            SetSysMsg("資料異常!");
            return;
        }
        dr = dt.Rows[0];
        //**************************************************************
        
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            Dictionary<string, object> dict2 = new Dictionary<string, object>();
            
            //用 Exist 函式 查詢個人班表有沒有資料
            //先以 日期 與 userID 查詢 有沒有資料若有 blnCount = true 則 update  ScheduleDetail
            //                                   若無 blnCount = false 則 Insert  ScheduleDetail   
            if (dt.Rows[i]["UserID"].ToString() == "John")
            {
                string aa = "";
            }
            string ExtraCondition = " and UserID='" + dt.Rows[i]["UserID"].ToString() + "'";
            bool blnCount = Util.Exist("ScheduleDetail", "schDate", Util.DateTime2String(dtDate, DateType.yyyyMMdd, EmptyType.ReturnNull), ExtraCondition);            
            string period = "period" + dt.Rows[i]["periodID"].ToString();
            string strSql2 = "";


            if (blnCount == false) //表示 個人班表 無資料 執行Insert 動作
            {
                strSql2 = "Insert into ScheduleDetail";
                strSql2 += "(schDate," + period + ",UserID) Values";
                strSql2 += "(@schDate,@period,@UserID)";
            }
            else
            {         //表示 個人班表 無資料 執行Update 動作
                //update
                strSql2 = "Update ScheduleDetail Set";
                strSql2 += " " + period + "=@period";
                strSql2 += " Where 1=1";
                strSql2 += " And schDate=@schDate ";
                strSql2 += " And UserID=@UserID";
            }
            dict2.Add("schDate", Util.DateTime2String(dtDate, DateType.yyyyMMdd, EmptyType.ReturnNull));
            dict2.Add("period","正常班");
            dict2.Add("UserID", dt.Rows[i]["UserID"].ToString());
            NpoDB.ExecuteSQLS(strSql2, dict2);
         }
    }
    //----------------------------------------------------------------------------------------------------------
    //查看傳入的年度與季有無資料
    public string   GetQuarterDateCount(string strYear ,string strQuarter)
    {
        switch (strQuarter)
        {
            case "1":
               return objNpoDB.QueryGetScalar("select Count(*) As Cnt from scheduledetail where schdate  between '" + strYear + "/01/01' and '" + strYear + "/03/31' ");
                break;
            case "2":
                return objNpoDB.QueryGetScalar("select Count(*) As Cnt from scheduledetail where schdate  between '" + strYear + "/04/01' and '" + strYear + "/06/30' ");
                break;
            case "3":
                return objNpoDB.QueryGetScalar("select Count(*) As Cnt from scheduledetail where schdate  between '" + strYear + "/07/01' and '" + strYear + "/09/30' ");
                break;
            case "4":
                return objNpoDB.QueryGetScalar("select Count(*) As Cnt from scheduledetail where schdate  between '" + strYear + "/10/01' and '" + strYear + "/12/31' ");
                break;
        }
        return "0";
    }
    //----------------------------------------------------------------------------------------------------------
    //傳入 年度 季 與 要查詢季的第一天或最後一天  之日期
    public string GetQuarterDate(string strYear, string strQuarter,string StartOrEnd)
    {
        if (StartOrEnd == "Start") //StartOrEnd == "Start" 表示要抓季的 第一天
        {
            switch (strQuarter)
            {
                case "1":
                    return strYear + "/01/01";
                    break;
                case "2":
                    return strYear + "/04/01";
                    break;
                case "3":
                    return strYear + "/07/01";
                    break;
                case "4":
                    return strYear + "/10/01";
                    break;
            }
        }
        else                //StartOrEnd == "End"
        {                  //要抓季的 最後一天
            switch (strQuarter)
            {
                case "1":
                    return strYear + "/03/31";
                    break;
                case "2":
                    return strYear + "/06/30";
                    break;
                case "3":
                    return strYear + "/09/30";
                    break;
                case "4":
                    return strYear + "/12/31";
                    break;
            }
        }
        return "0";
    }
    //----------------------------------------------------------------------------------------------------------

    //查看班表有無資料
    public string GetDateCount(string strYear, string strQuarter)
    {
        return objNpoDB.QueryGetScalar("select Count(*) As Cnt from schedule a inner join ScheduleMap b on a.uid = b.ScheduleID where schYear ='" + strYear + "' And Quarter= '" + strQuarter + "'");
    }




 
}
