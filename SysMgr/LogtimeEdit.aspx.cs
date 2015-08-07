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

public partial class CaseMgr_LogtimeEdit : BasePage
{
    GroupAuthrity ga = null;
    string tmpTime = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["ProID"] = "Logtime";
        //權限控制
        AuthrityControl();

        //取得模式, 新增("ADD")或修改("")
        HFD_Mode.Value = Util.GetQueryString("Mode");
        //kuofly 放入該筆資料的UID
        this.HFD_UID.Value = Util.GetQueryString("UID");
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
            btnEdit.Visible = false;
        }
        else
        {
            //修改時, 新增鍵沒動作
            btnAdd.Visible = false;
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
        Util.FillDropDownList(ddlstrHour, 0, 23, true, -1, true, 2, '0');
        Util.FillDropDownList(ddlstrMin, 0, 59, true, -1, true, 2, '0');
        Util.FillDropDownList(ddlEndHour, 0, 23, true, -1, true, 2, '0');
        Util.FillDropDownList(ddlEndMin, 0, 59, true, -1, true, 2, '0');
     }

    //@@...這樣寫好像...怪怪
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
        string strSql = "";
        DataTable dt = null;
        DataRow dr = null;
        if (HFD_Mode.Value != "ADD")
        {

            strSql = @"
                        select l.uid as uid ,l.UserID ,a.UserName as 志工名稱,LogTime as 上班時間, LogoutTime as 下班時間,
                        DATEPART(hour, LogTime) AS strH,DATEPART(minute, LogTime) AS strM 
                        ,DATEPART(hour, LogoutTime) AS EndH,DATEPART(minute, LogoutTime) AS EndM
                        from log l
                        inner join AdminUser a on a.UserID =l.UserID
                        where l.uid =@uid
                      ";


            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("uid", HFD_UID.Value);
            dt = NpoDB.GetDataTableS(strSql, dict);
            if (dt.Rows.Count == 0)
            {
                //資料異常
                SetSysMsg("資料異常!");
                //Response.Redirect("LogtimeQry.aspx");
                return;
            }
            dr = dt.Rows[0];

        }
       //志工編號
        ddlUserName.SelectedValue = (HFD_Mode.Value == "ADD") ? "" : dr["UserID"].ToString();
        //上班時間
        txtlogtDate.Text = (HFD_Mode.Value == "ADD") ? "": Util.DateTime2String(dr["上班時間"].ToString(), DateType.yyyyMMdd, EmptyType.ReturnNull);
        ddlstrHour.SelectedValue = (HFD_Mode.Value == "ADD") ? "" : dr["strH"].ToString();
        ddlstrMin.SelectedValue = (HFD_Mode.Value == "ADD") ? "" : dr["strM"].ToString();

        //下班時間
        txtLogoutDate.Text = (HFD_Mode.Value == "ADD") ?"":  Util.DateTime2String(dr["下班時間"].ToString(), DateType.yyyyMMdd, EmptyType.ReturnNull);
        ddlEndHour.SelectedValue = (HFD_Mode.Value == "ADD") ? "" : dr["EndH"].ToString();
        ddlEndMin.SelectedValue = (HFD_Mode.Value == "ADD") ? "" : dr["EndM"].ToString();
        if (HFD_Mode.Value == "ADD") {
            string strUserID = "";
            if (Util.GetQueryString("UserID") != "")
            {
                strUserID = Util.GetQueryString("UserID");
                ddlUserName.SelectedValue = strUserID;
            }
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        //判斷上下班 日期  時  分  是否空值 
        if (txtlogtDate.Text == "" || ddlstrHour.SelectedValue == "" || ddlstrMin.SelectedValue == "" || txtLogoutDate.Text == "" || ddlEndHour.SelectedValue == "" || ddlEndMin.SelectedValue == "" || ddlUserName.SelectedValue=="") 
        {
            //若空值為上班 日期  時 分  就跳出警示 
            if (txtlogtDate.Text == "" || ddlstrHour.SelectedValue == "" || ddlstrMin.SelectedValue == "")
            {
                ShowSysMsg("請輸入上班時間");
            }
            //若空值為下班 日期  時 分  就跳出警示 
            else if (txtLogoutDate.Text == "" || ddlEndHour.SelectedValue == "" || ddlEndMin.SelectedValue == "")
            {
                ShowSysMsg("請輸入下班時間");
            }
            //若空值為 志工名稱  就跳出警示 
            else if (ddlUserName.SelectedValue == "")
            {
                ShowSysMsg("請輸入志工名稱");
            }
            
        }
        //判斷上班日期與下班日期是否一致
        else if (txtlogtDate.Text != txtLogoutDate.Text)
        {
            ShowSysMsg("上班時間與下班時間不一致");
        }
        else
        {

            Dictionary<string, object> dict = new Dictionary<string, object>();
            string strSql = " select UserID from [Log] where UserID = @UserID and ";
            strSql += " (cast(@dbLogTime as datetime) between LogTime and LogoutTime ";
            strSql += " or cast(@dbLogoutTime as datetime) between LogTime and LogoutTime or ";
            strSql += " (cast(@dbLogTime as datetime) <= LogTime and cast(@dbLogoutTime as datetime) >= LogoutTime)) ";
            Random rnd = new Random();
            int second2 = rnd.Next(0, 59);
            dict.Add("dbLogTime", Util.DateTime2String(txtlogtDate.Text + " " + ddlstrHour.SelectedValue + ":" + ddlstrMin.SelectedValue + ":" + second2, DateType.yyyyMMddHHmmss, EmptyType.ReturnNull));
            dict.Add("dbLogoutTime", Util.DateTime2String(txtLogoutDate.Text + " " + ddlEndHour.SelectedValue + ":" + ddlEndMin.SelectedValue + ":00", DateType.yyyyMMddHHmmss, EmptyType.ReturnNull));
            dict.Add("UserID", ddlUserName.SelectedValue);
            DataTable dt = NpoDB.GetDataTableS(strSql, dict);
            if (dt.Rows.Count > 0)
            {
                //新增的時間範圍已存在
                ShowSysMsg("該時間範圍已存在");
                return;
            }

            dict = new Dictionary<string, object>();
            List<ColumnData> list = new List<ColumnData>();
            SetColumnData(list);
            strSql = Util.CreateInsertCommand("Log", list, dict);
            NpoDB.ExecuteSQLS(strSql, dict);
            if (txtlogtDate.Text != "" && txtLogoutDate.Text != "")
            {
                DateDiff(ddlUserName.SelectedValue);
            }
            Session["Msg"] = "新增資料成功";
            ScriptManager.RegisterStartupScript(this, typeof(string), "", "opener.location.href=opener.location.href;self.close();", true);
        }
        
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        //判斷上下班 日期  時  分  是否空值 
        if (txtlogtDate.Text == "" || ddlstrHour.SelectedValue == "" || ddlstrMin.SelectedValue == "" || txtLogoutDate.Text == "" || ddlEndHour.SelectedValue == "" || ddlEndMin.SelectedValue == "" || ddlUserName.SelectedValue == "")
        {
            //若空值為上班 日期  時 分  就跳出警示 
            if (txtlogtDate.Text == "" || ddlstrHour.SelectedValue == "" || ddlstrMin.SelectedValue == "")
            {
                ShowSysMsg("請輸入上班時間");
            }
            //若空值為下班 日期  時 分  就跳出警示 
            else if (txtLogoutDate.Text == "" || ddlEndHour.SelectedValue == "" || ddlEndMin.SelectedValue == "")
            {
                ShowSysMsg("請輸入下班時間");
            }
            //若空值為 志工名稱  就跳出警示 
            else if (ddlUserName.SelectedValue == "")
            {
                ShowSysMsg("請輸入志工名稱");
            }

        }
        //判斷上班日期與下班日期是否一致
        if (txtlogtDate.Text != txtLogoutDate.Text)
        {
            ShowSysMsg("上班時間與下班時間不一致");
            return;
        }

        string logTime="";
        string logoutTime="";
        logTime = Util.DateTime2String(txtlogtDate.Text + " " + ddlstrHour.SelectedValue + ":" + ddlstrMin.SelectedValue + ":00", DateType.yyyyMMddHHmmss, EmptyType.ReturnNull);
        logoutTime = Util.DateTime2String(txtLogoutDate.Text + " " + ddlEndHour.SelectedValue + ":" + ddlEndMin.SelectedValue + ":00", DateType.yyyyMMddHHmmss, EmptyType.ReturnNull);
        if (Convert.ToDateTime(logoutTime) < Convert.ToDateTime(logTime))
        {
            ShowSysMsg("下班時間必須大於上班時間");
            return;
        }


        Dictionary<string, object> dict = new Dictionary<string, object>();
        string strSql = " select UserID from [Log] where UserID = @UserID and [uid] <> @UID and ";
        strSql += " (cast(@dbLogTime as datetime) between LogTime and LogoutTime ";
        strSql += " or cast(@dbLogoutTime as datetime) between LogTime and LogoutTime or ";
        strSql += " (cast(@dbLogTime as datetime) <= LogTime and cast(@dbLogoutTime as datetime) >= LogoutTime)) ";
        dict.Add("dbLogTime", logTime);
        dict.Add("dbLogoutTime", logoutTime);
        dict.Add("UserID", ddlUserName.SelectedValue);
        dict.Add("UID", this.HFD_UID.Value);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);

        if (dt.Rows.Count > 0)
        {
            //修改的時間範圍已存在
            ShowSysMsg("該時間範圍已存在");
            return;
        }

        //Dictionary<string, object> dict = new Dictionary<string, object>();
        strSql = "";
        strSql += "Update Log Set ";
        strSql += " LogTime=@dbLogTime,";
        strSql += " LogoutTime=@dbLogoutTime ";
        strSql += " where uid=@UID ";
        //dict.Add("dbLogTime", 上班打卡);
        //dict.Add("dbLogoutTime", 下班打卡);
        //if (txtlogtDate.Text != "" && txtLogoutDate.Text != "")
        //{
        //    DateDiff(ddlUserName.SelectedValue);
        //}
        //dict.Add("UID", this.HFD_UID.Value);
        NpoDB.ExecuteSQLS(strSql, dict);
        Session["Msg"] = "修改資料成功";
        ScriptManager.RegisterStartupScript(this, typeof(string), "", "opener.location.href=opener.location.href;self.close();", true);
    }

    protected void btnDel_Click(object sender, EventArgs e)
    {
        if (this.HFD_UID.Value!=""){
            string strSql = "delete from log where uid=@uid";
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("uid", HFD_UID.Value);
            NpoDB.ExecuteSQLS(strSql, dict);
            Session["Msg"] = "刪除資料成功!";
            ScriptManager.RegisterStartupScript(this, typeof(string), "", "opener.location.href=opener.location.href;self.close();", true);
        }   
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
        list.Add(new ColumnData("LogTime", Util.DateTime2String(txtlogtDate.Text + " " + ddlstrHour.SelectedValue + ":" + ddlstrMin.SelectedValue + ":" + second2 , DateType.yyyyMMddHHmmss, EmptyType.ReturnNull), true, true, false));
        //下班時間
        list.Add(new ColumnData("LogoutTime", Util.DateTime2String(txtLogoutDate.Text + " " + ddlEndHour.SelectedValue + ":" + ddlEndMin.SelectedValue + ":00", DateType.yyyyMMddHHmmss, EmptyType.ReturnNull), true, true, false));
    }
    //-------------------------------------------------------------------------------------------------------------


  

 
  
 
    //----------------------------------------------------------------------------------------------------------
    private DataTable GetDataTable()
    {
//        string strSql = @"
//                  select Top 5 ROW_NUMBER() OVER(ORDER BY CreateDate) AS 筆數, (c.Name + d.Name + b.Address) FullAddress, 
//                         REVERSE(SUBSTRING(REVERSE(ConsultantMain),CHARINDEX(',',REVERSE(ConsultantMain))+1,LEN(ConsultantMain))) ConsultantMainALL,
//                         REVERSE(SUBSTRING(REVERSE(ConsultantItem),CHARINDEX(',',REVERSE(ConsultantItem))+1,LEN(ConsultantItem))) ConsultantItemALL
//                        ,*
//                        from Consulting a  
//                        inner join Member b on a.MemberUID = b.uid
//                        left join CodeCity c on b.City = c.ZipCode 
//                        left join CodeCity d on b.ZipCode = d.ZipCode     
//                  where 1=1 
//                  And a.MemberUID=@MemberUID
//                  and isnull(a.IsDelete, '') != 'Y'
//                  Order by CreateDate desc
//                        "; 
//        Dictionary<string, object> dict = new Dictionary<string, object>();
//        dict.Add("MemberUID", HFD_MemberID.Value);
//        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
//        return dt;
//    }

//    private void CreateConsultingData()
//    {
//        Dictionary<string, object> dict = new Dictionary<string, object>();
//        List<ColumnData> list = new List<ColumnData>();
//        string strSql = "";
//        string NewUID = "";
//        SetColumnData(list);

//        List<ColumnData> list2 = new List<ColumnData>();
//        if (HFD_ConsultingUID.Value == "")
//        {
//            SetConsultingColumnData(list2);
//            strSql = Util.CreateInsertCommand("Consulting", list2, dict);
//            strSql += "select @@IDENTITY";
//            HFD_ConsultingUID.Value = NpoDB.GetScalarS(strSql, dict);
//        }
        return null;
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
    protected void btnEndDate_Click(object sender, EventArgs e)
    {
        //DateTime t1, t2;
        //t1 = DateTime.Parse(txtCreateDate.Text + " " + txtCreateTime.Text);
        //if (txtEndTime.Text == "")
        //{
        //    btnEndDate.Text = "計算時間";
        //    txtEndTime.Text = Util.GetToday(DateType.HHmmss);
        //    t2 = DateTime.Parse(txtCreateDate.Text + " " + txtEndTime.Text);
        //    lblTime.Text = DateDiff(t1, t2);
        //}
        //else
        //{
        //    t2 = DateTime.Parse(txtCreateDate.Text + " " + txtEndTime.Text);
        //    lblTime.Text = DateDiff(t1, t2);
        //}
    }


 
}

 