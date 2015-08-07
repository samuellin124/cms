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

public partial class SysMgr_AttendanceHours : BasePage
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
    string tmpTime = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["ProgID"] = "AttendanceHours";
        //權限控制
        AuthrityControl();
        Util.ShowSysMsgWithScript("$('#btnPreviousPage').hide();$('#btnNextPage').hide();$('#btnGoPage').hide();");
        //取得模式, 新增("ADD")或修改("")
        //HFD_Mode.Value = Util.GetQueryString("Mode");
        //HFD_UserID.Value = Util.GetQueryString("UID");
        //設定一些按鍵的 Visible 屬性
        //注意:要在 AuthrityControl() 及取得 HFD_Mode.Value 呼叫
        SetButton();
        
        if (!IsPostBack)
        {
            

            //載入下拉式選單資料
            LoadDropDownListData();

            LoadFormData();

        }
     }

    private void SetupDefaultValue()
    {
    }
    //-------------------------------------------------------------------------
    private void SetButton()
    {
        
        if (HFD_Mode.Value == "ADD")
        {
            ////新增時, 修改及刪除鍵沒動作
            //btnUpdate.Visible = false;
        }
        else
        {
            //修改時, 新增鍵沒動作
            //btnAdd.Visible = false;
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
        //btnUpdate.Visible = ga.Update;
    }
    //-------------------------------------------------------------------------
    public void LoadDropDownListData()
    {
        //志工姓名
        string strSql = "select userid,username from AdminUser";
        Util.FillDropDownList(ddlUserName, strSql, "username", "userid", true);
        //Util.FillDropDownList(ddlstrHour, 0, 23, true, -1, true, 2, '0');
        //Util.FillDropDownList(ddlstrMin, 0, 59, true, -1, true, 2, '0');
        //Util.FillDropDownList(ddlEndHour, 0, 23, true, -1, true, 2, '0');
        //Util.FillDropDownList(ddlEndMin, 0, 59, true, -1, true, 2, '0');
     }
    private string DateDiff(string UserID)
    {
        string strSql = @"
                        select min(logtime)  as logtime , max(logouttime)  as logouttime  from log 
                        where userid =@userid  
                        and logtime between '" + txtlogtDate.Text + " 00:00:01' and '" + txtLogoutDate.Text + @" 23:59:59' 
                       ";
        //從SQL 去比較2個時間差多少個小時、分、秒
        //                string strSql = @"
        //                         select min(logtime) as logtime , max(logouttime)  as logouttime 
        //                        , CONVERT(varchar, DATEADD(s, DATEDIFF(s,min(logtime),max(logouttime) ), 0), 108) as datetime
        //                        from log 
        //                       "



        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("userid", UserID);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        dt = NpoDB.GetDataTableS(strSql, dict);
        DataRow dr = null;
        if (dt.Rows.Count == 0)
        {

        }
        dr = dt.Rows[0];
        string dateDiff = null;
        TimeSpan ts1 = new TimeSpan(DateTime.Parse(dr["logtime"].ToString()).Ticks);
        if (dr["logouttime"].ToString() == "" && dr["logouttime"].ToString() == "")
        {
            return "0";
        }
        TimeSpan ts2 = new TimeSpan(DateTime.Parse(dr["logouttime"].ToString()).Ticks);
        TimeSpan ts = ts1.Subtract(ts2).Duration();
        //ts.Days.ToString()) * 24 一天*24小時   +ts.Hours.ToString() 小時 
        tmpTime = (int.Parse(ts.Days.ToString()) * 24 + int.Parse(ts.Hours.ToString())).ToString();//+":" + int.Parse(ts.Minutes.ToString())).ToString() + ":" ;
        //update
        strSql = @"
                update log set
                dateDiff =@dateDiff
                where UserID = @UserID
                and logtime between '"  + txtlogtDate.Text + " 00:00:01' and '" + txtLogoutDate.Text + @" 23:59:59' "
                ;
        //dict.Add("UserID", UserID);
        dict.Add("dateDiff", tmpTime);
        NpoDB.ExecuteSQLS(strSql, dict);

        return dateDiff;
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
        npoGridView.CurrentPage = Util.String2Number(HFD_CurrentPage.Value);
       lblGridList.Text = npoGridView.Render();
    }

   protected void btnQuery_Click(object sender, EventArgs e)
    {
        LoadFormData();
    }
    //-------------------------------------------------------------------------------------------------------------
    //基本資料
    private void SetColumnData(List<ColumnData> list)
    {
        Random rnd = new Random();
        int second2 = rnd.Next(0, 59);
        //志工編號
        list.Add(new ColumnData("UserID", ddlUserName.SelectedValue, true, true, false));
        //上班時間
        //list.Add(new ColumnData("LogTime", Util.DateTime2String(txtlogtDate.Text + " " + ddlstrHour.SelectedValue + ":" + ddlstrMin.SelectedValue + ":" + second2 , DateType.yyyyMMddHHmmss, EmptyType.ReturnNull), true, true, false));
        ////下班時間
        //list.Add(new ColumnData("LogoutTime", Util.DateTime2String(txtLogoutDate.Text + " " + ddlEndHour.SelectedValue + ":" + ddlEndMin.SelectedValue + ":00", DateType.yyyyMMddHHmmss, EmptyType.ReturnNull), true, true, false));

    }
    //-------------------------------------------------------------------------------------------------------------
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("LogtimeQry.aspx");
    }

  

 
  
 
    //----------------------------------------------------------------------------------------------------------
    private DataTable GetDataTable()
    {
        string strSql = "";
        //讀取資料庫
      
        strSql = @" 
                    select  a.userid 志工編號, a.UserName as 志工名稱,min(logtime) as 上班時間 , max(logouttime)  as 下班時間 , table3.時數
                    from log l
                    inner join AdminUser a on a.UserID = l.UserID
                    inner join (
									select Userid,時數 from 
									(
										select UserID,
										sum(分鐘數) 總上班分鐘數,
										cast(sum(分鐘數)/60 as varchar) + '：' 
										+ Case LEN(cast(sum(分鐘數)%60 as varchar)) when 0 then '00' when 1 then '0' else '' end 
										+ cast(sum(分鐘數)%60 as varchar) + '' 時數 
										from 
										(
											select UserID,LogTime,LogoutTime 下班時間
											,dateDiff(minute,LogTime,LogoutTime) 分鐘數
											from Log where 1=1 ";
        if (txtlogtDate.Text != "" || txtLogoutDate.Text != "")
        {
            strSql += " and convert(varchar,logtime,111) between '" + txtlogtDate.Text + "' and '" + txtLogoutDate.Text + "'";
        }
        if (ddlUserName.Text != "")
        {
            strSql += " and userid =@userid  ";
        }
        strSql += @"
										) table1 
										group by UserID
									) table2 
					) table3 on table3.UserID=l.UserID
                    where 1=1";

                  //當上班時間與下班時間都有填寫時在去查詢時間判斷
            if (txtlogtDate.Text != "" || txtLogoutDate.Text!="")
            {
                strSql += " and convert(varchar,logtime,111) between '" + txtlogtDate.Text + "' and '" + txtLogoutDate.Text + "' ";
            }
            if (ddlUserName.Text !="")
            {
                strSql += " and a.userid =@userid  ";
            }

            strSql += " group by a.userid,a.UserName,table3.時數 order by 1";
        //Response.Write(strSql);
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("userid", ddlUserName.Text);
        //dict.Add("LogTime", txtlogtDate.Text);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        return dt;
     }


    //日期檢查
    public bool isDate(string dateStr)
    {
        bool _isDate = false;
        string matchStr = "";
        matchStr += @"^((?!0000)[0-9]{4}/((0?[1-9]|1[0-2])/(0?[1-9]|1[0-9]|2[0-8])|(0[13-9]|1[0-2])-(29|30)|(0[13578]|1[02])-31)|([0-9]{2}(0[48]|[2468][048]|[13579][26])|(0[48]|[2468][048]|[13579][26])00)-02-29)$  ";
        RegexOptions option = (RegexOptions.IgnoreCase | (RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace));

        if (Regex.IsMatch(dateStr, matchStr, option))
            _isDate = true;
        else
            _isDate = false;
        return _isDate;
    }
    public bool isTime(string dateStr)
    {
        bool _isTime = false;
        string matchStr = "";
        matchStr += @"^((20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$";
        RegexOptions option = (RegexOptions.IgnoreCase | (RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace));

        if (Regex.IsMatch(dateStr, matchStr, option))
            _isTime = true;
        else
            _isTime = false;

        return _isTime;
     }

    private string chkField()
    {
        //if (isDate(txtCreateDate.Text) == false)
        //{
        //    return "日期格式錯誤，格式為 yyyy/mm/dd";
        //}

        //if (isTime(txtCreateTime.Text) == false)
        //{
        //    return "時間格式錯誤，格式為 hh:mm:ss";
        //}

        return "";
    }
    protected void btnReload_Click(object sender, EventArgs e)
    {
        LoadFormData();
    }
    
    protected void btnPrint_Click(object sender, EventArgs e)
    {

        lblRpt.Text = 報表抬頭() + GetReport();
        string s = @"<head>
                        <style>
                        @page Section2
                        { size:841.7pt 595.45pt;
                          mso-page-orientation:landscape;
                        margin:1.5cm 0.5cm 0.5cm 0.5cm;}
                        div.Section2
                        {page:Section2;}";
                s += "td{mso-number-format:\\@;}";
                        s += @"</style>
                    </head>
                    <body>
                        <div class=Section2>
                    ";//邊界-上右下左
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(s);
        Util.OutputTxt(sb + lblRpt.Text, "1", DateTime.Now.ToString("yyyyMMddHHmmss"));
    }
    //----------------------------------------------------------------------------------------------------------
    //報表抬頭
    private string 報表抬頭()
    {
        string retStr = "";
        retStr +="<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" align=\"center\" style=\"font-size:12px;font-family:標楷體;width:100%;\">\n";
        retStr +="<tr>\n";
        retStr +="\t<td colspan=\"5\" style=\"text-align:center;font-size:24px;font-family:標楷體;border-style:none;font-weight:bold;\">出勤時數統計</td>\n";
        retStr +="</tr>\n";
        retStr += "</table>\n";

        return retStr;
    }
    private string GetReport()
    {
        string retStr = "";
        retStr += "<table x:str border=\"0\" bordercolor=\"black\" cellpadding=\"0\" cellspacing=\"0\" align=\"center\" style=\"font-size:12pt;font-family:標楷體;width:800px;\">\n";
        retStr += "\t<tr>\n";
        retStr += "\t\t<td style=\"text-align:center;font-size:25px;border-left:1px solid windowtext;border-top:1px solid windowtext;border-right:1px solid windowtext;\">";
        retStr += "志工編號</td>\n";
        retStr += "\t\t<td style=\"text-align:center;font-size:25px;border-left:1px solid windowtext;border-top:1px solid windowtext;border-right:1px solid windowtext;\">";
        retStr += "志工名稱</td>\n";
        retStr += "\t\t<td style=\"text-align:center;font-size:25px;border-top:1px solid windowtext;border-right:1px solid windowtext;\">";
        retStr += "上班時間</td>\n";
        retStr += "\t\t<td style=\"text-align:center;font-size:25px;border-top:1px solid windowtext;border-right:1px solid windowtext;\">";
        retStr += "下班時間</td>\n";
        retStr += "\t\t<td style=\"text-align:center;font-size:25px;border-top:1px solid windowtext;border-right:1px solid windowtext;\">";
        retStr += "時數</td>\n";
        retStr += "\t<td style=\"text-align:left;font-size:12px;\" width=\"8\"></td>\n";
        retStr += "</tr>\n";

        DataTable dt = GetDataTable();
        foreach (DataRow dr in dt.Rows)
        {
            retStr += "<tr>\n";
            retStr += "\t<td style=\"text-align:center;font-size:18px;border-left:1px solid windowtext;border-top:1px solid windowtext;border-right:1px solid windowtext;border-bottom:1px solid windowtext;\">";
            retStr += dr["志工編號"].ToString();     //從GetDataTable()抓Name值
            retStr += "</td>";
            retStr += "\t<td style=\"text-align:center;font-size:18px;border-left:1px solid windowtext;border-top:1px solid windowtext;border-right:1px solid windowtext;border-bottom:1px solid windowtext;\">";
            retStr += dr["志工名稱"].ToString();     //從GetDataTable()抓Name值
            retStr += "</td>";
            retStr += "\t<td style=\"text-align:right;font-size:18px;border-left:1px solid windowtext;border-top:1px solid windowtext;border-right:1px solid windowtext;border-bottom:1px solid windowtext;\">";
            retStr += dr["上班時間"].ToString();
            retStr += "</td>";
            retStr += "\t<td style=\"text-align:right;font-size:18px;border-left:1px solid windowtext;border-top:1px solid windowtext;border-right:1px solid windowtext;border-bottom:1px solid windowtext;\">";
            retStr += dr["下班時間"].ToString();
            retStr += "</td>";
            retStr += "\t<td style=\"text-align:right;font-size:18px;border-left:1px solid windowtext;border-top:1px solid windowtext;border-right:1px solid windowtext;border-bottom:1px solid windowtext;\">";
            retStr += dr["時數"].ToString();
            retStr += "</td>";
            retStr += "</tr>";
        }
        retStr += "</table>";
        return retStr;
    }

 
}

 