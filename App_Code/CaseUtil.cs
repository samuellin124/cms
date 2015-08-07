using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Web;
using System.Web.SessionState;


/// <summary>
/// Summary description for CaseUtil
/// </summary>

public class CaseUtil
{
    public static HttpSessionState Session { get { return HttpContext.Current.Session; } }
    public static HttpRequest Request { get { return HttpContext.Current.Request; } }
    public static HttpResponse Response { get { return HttpContext.Current.Response; } }
    public static HttpServerUtility Server { get { return HttpContext.Current.Server; } }
    public static Page page { get { return HttpContext.Current.CurrentHandler as Page; } }

    //-------------------------------------------------------------------------------------------------------------
    public enum LogType
    {
        LogTime,
        ActionTime,
        LogoutTime
    }
    public static void LogData(LogType logType, string UserID)
    {
        string Today = Util.GetToday(DateType.yyyyMMdd);
        DataTable dt = Util.GetDataTable("Log", "UserID", UserID, "CONVERT(char(10), LogTime, 111)", Today, "LogTime", "desc");

        string uid = "";
        string LogoutTime = "";
        if (dt.Rows.Count != 0)
        {
            uid = dt.Rows[0]["uid"].ToString();
            LogoutTime = dt.Rows[0]["LogoutTime"].ToString();
        }

        Dictionary<string, object> dict = new Dictionary<string, object>();
        List<ColumnData> list = new List<ColumnData>();
        string strSql = "";
        if (logType == LogType.LogTime)
        {
            if (LogoutTime != "") //如果同一天有 logout, 則另存一筆
            {
                uid = "";
            }
            //記錄登入時間
            if (uid != "") //已有UserID+LogTime 資料列
            {
                SetColumnData4Log(list, LogType.ActionTime, UserID, uid);
                strSql = Util.CreateUpdateCommand("Log", list, dict);
            }
            else
            {
                SetColumnData4Log(list, LogType.LogTime, UserID, uid);
                strSql = Util.CreateInsertCommand("Log", list, dict);
            }
        }
        else if (logType == LogType.ActionTime)
        {
            //記錄動作時間
            //記錄登出時間
            if (uid == "") //Action 沒有相對應的 login 資料
            {
                SetColumnData4Log(list, LogType.LogTime, UserID, uid);
                strSql = Util.CreateInsertCommand("Log", list, dict);
            }
            else
            {
                SetColumnData4Log(list, LogType.ActionTime, UserID, uid);
                strSql = Util.CreateUpdateCommand("Log", list, dict);
            }
        }
        else if (logType == LogType.LogoutTime)
        {
            if (uid == "") //logout 沒有相對應的 login 資料,不處理, 隔夜會有問題
            {
                return;
            }
            //記錄登出時間
            SetColumnData4Log(list, LogType.LogoutTime, UserID, uid);
            strSql = Util.CreateUpdateCommand("Log", list, dict);
        }
        NpoDB.ExecuteSQLS(strSql, dict);
    }
    //---------------------------------------------------------------------------
    private static void SetColumnData4Log(List<ColumnData> list, LogType logType, string UserID, string uid)
    {
        list.Add(new ColumnData("uid", uid, false, false, true));
        if (logType == LogType.LogTime)
        {
            list.Add(new ColumnData("UserID", UserID, true, true, false));
            list.Add(new ColumnData("LogTime", Util.GetToday(DateType.yyyyMMddHHmmss), true, true, false));
            list.Add(new ColumnData("ActionTime", Util.GetToday(DateType.yyyyMMddHHmmss), true, true, false));
        }
        else if (logType == LogType.ActionTime)
        {
            list.Add(new ColumnData("ActionTime", Util.GetToday(DateType.yyyyMMddHHmmss), true, true, false));
        }
        else if (logType == LogType.LogoutTime)
        {
            list.Add(new ColumnData("LogoutTime", Util.GetToday(DateType.yyyyMMddHHmmss), true, true, false));
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    public static bool CheckDate(string Date, int LimitDay)
    {
        //日期不能空白
        if (Date == "")
        {
            return false;
        }
        DateTime LimitStartDate = Util.GetStartDateOfMonth(DateTime.Now.AddMonths(-1));
        DateTime LimitEndDate = Util.GetEndDateOfMonth(DateTime.Now.AddMonths(-1));
        DateTime ServiceDate;
        bool flag = false;
        try
        {
            ServiceDate = Convert.ToDateTime(Date);
            //日期不能大於今天
            if (ServiceDate > DateTime.Now)
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            return false;
        }

        if (DateTime.Now.Day <= LimitDay)
        {
            //5號之前, 可以能輸入上個月的資料
            if (ServiceDate >= LimitStartDate)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            //超過5號, 則不能輸入上個月的資料
            if (ServiceDate < LimitEndDate)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    //產生上傳檔案的 JavaScript 碼
    public static string GetUploadJavaScriptCode()
    {
        string Script = "";
        Script += "function openUploadWindow(Ap_Name) {\n";
        Script += "var PersonProfile_Uid = document.getElementById('HFD_Uid').value;\n";
        Script += "window.open('../Common/UpLoad.aspx?PersonProfile_Uid=' + PersonProfile_Uid + '&Ap_Name=' + Ap_Name + '&AllowType=img', 'upload', 'scrollbars=no,status=no,toolbar=no,top=100,left=120,width=560,height=120');\n";
        Script += "}\n";
        return Script;
    }
    //-------------------------------------------------------------------------------------------------------------
    public static string MakeLandMenu(string strLandUID, int SelectedTab)
    {
        List<CButton> ButtonList = new List<CButton>();
        CButton Btn = new CButton();
        Btn.TabNo = 1;
        Btn.Text = "土地基本資料";
        Btn.Style = "style='width:35mm'";  //需要不同寬度時,可以在此設定新值
        Btn.ImgSrc = "../images/toolbar_exit.gif";
        Btn.OnClick = "LandData_Edit.aspx?LandUID=" + strLandUID;
        //Btn.Style = "style='font-size:12px'";  //設定字型大小
        Btn.Title = "土地基本資料";
        ButtonList.Add(Btn);

        Btn = new CButton();
        Btn.TabNo = 2;
        Btn.Text = "使用情形";
        Btn.Style = "style='width:25mm'";  //需要不同寬度時,可以在此設定新值
        Btn.ImgSrc = "../images/toolbar_modify.gif";
        Btn.OnClick = "LandData_Edit2.aspx?LandUID=" + strLandUID;
        //Btn.Style = "style='font-size:16px'";  //設定字型大小
        Btn.Title = "使用情形";
        ButtonList.Add(Btn);

        Btn = new CButton();
        Btn.TabNo = 3;
        Btn.Text = "帳面價值";
        Btn.Style = "style='width:25mm'";  //需要不同寬度時,可以在此設定新值
        Btn.ImgSrc = "../images/toolbar_familytree.gif";
        Btn.OnClick = "LandData_Edit3.aspx?LandUID=" + strLandUID;
        //Btn.Style = "style='font-size:16px'";  //設定字型大小
        Btn.Title = "帳面價值";
        ButtonList.Add(Btn);

        Btn = new CButton();
        Btn.TabNo = 4;
        Btn.Text = "申報及公告地價";
        Btn.Style = "style='width:35mm'";  //需要不同寬度時,可以在此設定新值
        Btn.ImgSrc = "../images/toolbar_new.gif";
        Btn.OnClick = "LandDataQry4.aspx?LandUID=" + strLandUID;
        //Btn.Style = "style='font-size:16px'";  //設定字型大小
        Btn.Title = "申報及公告地價";
        ButtonList.Add(Btn);

        Btn = new CButton();
        Btn.TabNo = 5;
        Btn.Text = "檔案上傳";
        Btn.Style = "style='width:35mm'";  //需要不同寬度時,可以在此設定新值
        Btn.ImgSrc = "../images/toolbar_upload.gif";
        Btn.OnClick = "LandDataUpload.aspx?LandUID=" + strLandUID;
        //Btn.Style = "style='font-size:16px'";  //設定字型大小
        Btn.Title = "檔案上傳";
        ButtonList.Add(Btn);

        return MenuList(ButtonList, SelectedTab);
    }
    //-------------------------------------------------------------------------
    public static string MakeHouseMenu(string strHouseUID, int SelectedTab)
    {
        List<CButton> ButtonList = new List<CButton>();
        CButton Btn = new CButton();
        Btn.TabNo = 1;
        Btn.Text = "房屋基本資料";
        Btn.Style = "style='width:35mm'";  //需要不同寬度時,可以在此設定新值
        Btn.ImgSrc = "../images/toolbar_exit.gif";
        Btn.OnClick = "HouseData_Edit.aspx?HouseUID=" + strHouseUID;
        //Btn.Style = "style='font-size:12px'";  //設定字型大小
        Btn.Title = "房屋基本資料";
        ButtonList.Add(Btn);

        Btn = new CButton();
        Btn.TabNo = 2;
        Btn.Text = "使用情形";
        Btn.Style = "style='width:25mm'";  //需要不同寬度時,可以在此設定新值
        Btn.ImgSrc = "../images/toolbar_modify.gif";
        Btn.OnClick = "HouseData_Edit2.aspx?HouseUID=" + strHouseUID;
        //Btn.Style = "style='font-size:16px'";  //設定字型大小
        Btn.Title = "使用情形";
        ButtonList.Add(Btn);

        Btn = new CButton();
        Btn.TabNo = 3;
        Btn.Text = "帳面價值";
        Btn.Style = "style='width:25mm'";  //需要不同寬度時,可以在此設定新值
        Btn.ImgSrc = "../images/toolbar_familytree.gif";
        Btn.OnClick = "HouseData_Edit3.aspx?HouseUID=" + strHouseUID;
        //Btn.Style = "style='font-size:16px'";  //設定字型大小
        Btn.Title = "帳面價值";
        ButtonList.Add(Btn);

        Btn = new CButton();
        Btn.TabNo = 4;
        Btn.Text = "房地稅金";
        Btn.Style = "style='width:35mm'";  //需要不同寬度時,可以在此設定新值
        Btn.ImgSrc = "../images/toolbar_new.gif";
        Btn.OnClick = "HouseDataQry4.aspx?HouseUID=" + strHouseUID;
        //Btn.Style = "style='font-size:16px'";  //設定字型大小
        Btn.Title = "房地稅金";
        ButtonList.Add(Btn);

        Btn = new CButton();
        Btn.TabNo = 5;
        Btn.Text = "房產租賃";
        Btn.Style = "style='width:35mm'";  //需要不同寬度時,可以在此設定新值
        Btn.ImgSrc = "../images/toolbar_group.gif";
        Btn.OnClick = "HouseDataQry5.aspx?HouseUID=" + strHouseUID;
        //Btn.Style = "style='font-size:16px'";  //設定字型大小
        Btn.Title = "房產租賃";
        ButtonList.Add(Btn);

        Btn = new CButton();
        Btn.TabNo = 6;
        Btn.Text = "檔案上傳";
        Btn.Style = "style='width:35mm'";  //需要不同寬度時,可以在此設定新值
        Btn.ImgSrc = "../images/toolbar_upload.gif";
        Btn.OnClick = "HouseDataUpload.aspx?HouseUID=" + strHouseUID;
        //Btn.Style = "style='font-size:16px'";  //設定字型大小
        Btn.Title = "檔案上傳";
        ButtonList.Add(Btn);

        return MenuList(ButtonList, SelectedTab);
    }
    //-------------------------------------------------------------------------
    public static string MakeMenuMove(string strCaseID, int SelectedTab)
    {
        List<CButton> ButtonList = new List<CButton>();
        CButton Btn = new CButton();
        Btn.TabNo = 1;
        Btn.Text = "郵政財產異動單";
        Btn.Style = "style='width:40mm'";  //需要不同寬度時,可以在此設定新值
        Btn.ImgSrc = "../images/toolbar_exit.gif";
        Btn.OnClick = "MoveMgrQry.aspx";
        //Btn.Style = "style='font-size:12px'";  //設定字型大小
        //Btn.Title = "切換作業前請先存檔！";
        ButtonList.Add(Btn);

        Btn = new CButton();
        Btn.TabNo = 2;
        Btn.Text = "財產減損通知單";
        Btn.Style = "style='width:40mm'";  //需要不同寬度時,可以在此設定新值
        Btn.ImgSrc = "../images/toolbar_modify.gif";
        Btn.OnClick = "AssetsCut_Edit.aspx";
        //Btn.Style = "style='font-size:16px'";  //設定字型大小
        // Btn.Title = "切換作業前請先存檔！";
        ButtonList.Add(Btn);



        return MenuList(ButtonList, SelectedTab);
    }
    
    
    private static string MenuList(List<CButton> ButtonList, int SelectedTab)
    {
        string s = "";
        foreach (CButton btn in ButtonList)
        {
            if (btn.TabNo == SelectedTab)
            {
                btn.CssClass = "tabSelected";
            }
            else
            {
                btn.CssClass = "tabNormal";
            }

            //s += "<button type='button' " + btn.Style + " class='" + btn.CssClass + "' onclick=\"javascript:location.href='" + btn.OnClick + "' \"><img src='" + btn.ImgSrc + "' width='" + btn.Width + "' height='" + btn.Height + "' align='" + btn.Align + "' />" + btn.Text + "</button>\n";
            s += "<button type='button' title='" + btn.Title + "' class='" + btn.CssClass + "' " + btn.Style + " onclick=\"javascript:location.href='" + btn.OnClick + "' \"><img src='" + btn.ImgSrc + "' width='" + btn.Width + "' height='" + btn.Height + "' align='" + btn.Align + "' />" + btn.Text + "</button>\n";
            if (btn.ShowBR == true)
            {
                s += "<br/>";
            }
        }
        return s;
    }
    //-------------------------------------------------------------------------
    //取得病友團體資料	
    public static DataTable GetPatientGroup()
    {
        string strSql = "select GroupName, GroupName from PatientGroup";
        strSql += " where deletedate is null ";
        strSql += " and GroupName is not null ";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        return dt;
    }
    //-------------------------------------------------------------------------
    //取得醫療院所資料	
    public static DataTable GetHospital()
    {
        string strSql = "select Counseling as GroupName, Counseling as GroupName from PatientGroup";
        strSql += " where deletedate is null ";
        strSql += " and Counseling is not null ";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        return dt;
    }
    //-------------------------------------------------------------------------
    //取得身份來源資料
    public static DataTable GetCaseGroup(bool bolIsLock)
    {
        string strSql = @"select CaseGroup 
                        from CaseCode
                        where IsUse =1 ";
        if (bolIsLock)
        {
            strSql += "     and IsLock = 1 ";
        }
        else
        {
            strSql += "     and (IsLock =0 or islock is null) ";
        }
        strSql += @"    group by CaseGroup 
                        order by CaseGroup ";
        Dictionary<string, object> dict = new Dictionary<string, object>();

        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        return dt;
    }
    //-------------------------------------------------------------------------
    //取得身份來源資料
    public static DataTable GetCaseCode(string strCaseGroup)
    {
        string strSql = "select CaseName, CaseId from CaseCode";
        strSql += " where 1=1 ";// IsUse =1 "; Nash
       // strSql += " and CaseGroup =@CaseGroup ";
        strSql += " order by cast(caseid as int) ";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("CaseGroup", strCaseGroup);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        return dt;
    }
    //-------------------------------------------------------------------------
    //取得身份來源資料
    public static DataTable GetFromType()
    {
        DataTable dt = GetCaseCode("身份來源");
        return dt;
    }
    //-------------------------------------------------------------------------
    //取得身份別（個人）資料
    public static DataTable GetDonorTypePerson()
    {
        DataTable dt = GetCaseCode("身份別P");
        return dt;
    }
    //-------------------------------------------------------------------------
    //取得身份別（團體）資料
    public static DataTable GetDonorTypeGroup()
    {
        DataTable dt = GetCaseCode("身份別G");
        return dt;
    }
    //-------------------------------------------------------------------------
    //取得聯繫狀況資料
    public static DataTable GetContactStatus()
    {
        DataTable dt = GetCaseCode("聯繫狀況");
        return dt;
    }
    //-------------------------------------------------------------------------
    //取得最大之案件編號，案件編號 ex: A0100001
    public static string GetNewMaxCaseID()
    {
        string strRet = "";

        string strSql = @"select isnull(MAX(CaseID),'') as CaseID from CaseApply ";
        DataTable dt = NpoDB.GetDataTableS(strSql, null);
        //民國年取2位
        string strRocYear2 = "A" + ((DateTime.Now.Year - 1911) % 100).ToString("00");
        if (dt.Rows.Count > 0)
        {
            string strMaxCaseID=dt.Rows[0]["CaseID"].ToString();
            if (strMaxCaseID == "")
            {
                strRet = strRocYear2 + "00001";
            }
            else
            {
                if (strMaxCaseID.Substring(0, 3) == strRocYear2)
                {
                    strRet = strRocYear2 + (Convert.ToInt32(strMaxCaseID.Substring(3, 5)) + 1).ToString("00000");
                }
                else
                {
                    strRet = strRocYear2 + "00001";
                }
            }
        }

        return strRet;
    }
    //-------------------------------------------------------------------------
    //取得同案件編號之最大案件評估表編號，評估表編號 ex: E0001，加上CaseID為唯一值
    public static string GetNewMaxEvaluateID(string strCaseID)
    {
        string strRet = "";

        string strSql = @"select isnull(MAX(EvaluateID),'') as EvaluateID from vwEvaluateID ";
        strSql += " where CaseID='" + strCaseID + "'";
        DataTable dt = NpoDB.GetDataTableS(strSql, null);
        if (dt.Rows.Count > 0)
        {
            string strMaxEvaluateID = dt.Rows[0]["EvaluateID"].ToString();
            if (strMaxEvaluateID == "")
            {
                strRet = "E0001";
            }
            else
            {
                strRet = "E" + (Convert.ToInt32(strMaxEvaluateID.Substring(1, 4)) + 1).ToString("0000");
            }
        }

        return strRet;
    }
    //-------------------------------------------------------------------------
    //取得同案件編號之最大案件服務紀錄表編號，服務紀錄表編號 ex: S0001，加上CaseID為唯一值
    public static string GetNewMaxServiceID(string strCaseID)
    {
        string strRet = "";

        string strSql = @"select isnull(MAX(ServiceID),'') as ServiceID from CaseRecord ";
        strSql += " where CaseID='" + strCaseID + "'";
        DataTable dt = NpoDB.GetDataTableS(strSql, null);
        if (dt.Rows.Count > 0)
        {
            string strMaxServiceID = dt.Rows[0]["ServiceID"].ToString();
            if (strMaxServiceID == "")
            {
                strRet = "S0001";
            }
            else
            {
                strRet = "S" + (Convert.ToInt32(strMaxServiceID.Substring(1, 4)) + 1).ToString("0000");
            }
        }

        return strRet;
    }
    //-------------------------------------------------------------------------
    //檢核案件編號CaseID是否存在傳入之Table中
    public static bool CheckCaseID(string strCaseID,string strTableName)
    {
        bool bolRet = false;

        string strSql = @"select CaseID from " + strTableName;
        strSql += " where CaseID='" + strCaseID + "'";
        DataTable dt = NpoDB.GetDataTableS(strSql, null);
        if (dt.Rows.Count > 0)
        {
            bolRet = true;
        }

        return bolRet;
    }
    //-------------------------------------------------------------------------
    //取得傳入之Table中案件編號CaseID之案件狀態
    public static string GetCaseStatus(string strCaseID, string strTableName)
    {
        string strRet = "";

        string strSql = @"select isnull(CaseStatus,'') from " + strTableName;
        strSql += " where CaseID='" + strCaseID + "'";
        DataTable dt = NpoDB.GetDataTableS(strSql, null);
        if (dt.Rows.Count > 0)
        {
            strRet = dt.Rows[0]["CaseStatus"].ToString();
        }

        return strRet;
    }
    //-------------------------------------------------------------------------
    //更新傳入之Table中案件編號CaseID之案件狀態
    public static bool UpdateCaseStatus(string strCaseID, string strTableName, string strCaseStatus)
    {
        bool bolRet = false;

        string strSql = @"update " + strTableName + " set CaseStatus='" + strCaseStatus + "'";
        strSql += " where CaseID='" + strCaseID + "'";
        int iCount = NpoDB.ExecuteSQLS(strSql, null);
        if (iCount > 0)
        {
            bolRet = true;
        }

        return bolRet;
    }
    //-------------------------------------------------------------------------
    //取得督導所屬單位之社工員名單寫入DropdownList
    public static void GetSocialWorkerByDept(DropDownList ddlTarget, string strDeptID, string strGroupID)
    {
        ddlTarget.Items.Clear();
        string strSql = @"select * from AdminUser ";
        strSql += " where 1=1 ";
        //系統管理員及主任可指派各單位
        if (strGroupID != "1" && strGroupID != "2")
        {
            strSql += " and DeptID='" + strDeptID + "'";
        }
        strSql += " and GroupID='5'";
        DataTable dt = NpoDB.GetDataTableS(strSql, null);
        if (dt.Rows.Count > 0)
        {
            Util.FillDropDownList(ddlTarget, dt, "UserName", "UserID", true);
        }
    }
    //-------------------------------------------------------------------------
    //取得督導所屬單位之社工員名單回傳
    public static string GetSocialWorkerByDept(string strDeptID, string strGroupID)
    {
        string strRet = "";
        string strSql = @"select * from AdminUser ";
        strSql += " where 1=1 ";
        //系統管理員及主任可指派各單位
        if (strGroupID != "1" && strGroupID != "2")
        {
            strSql += " and DeptID='" + strDeptID + "'";
        }
        strSql += " and GroupID='5'";
        DataTable dt = NpoDB.GetDataTableS(strSql, null);
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                strRet += dr["UserID"].ToString() + ",";
            }
        }
        if (strRet != "")
        {
            strRet = strRet.Substring(0, strRet.Length - 1);
            strRet = strRet.Replace(",", "','");
        }
        return strRet;
    }
    //-------------------------------------------------------------------------
    //傳入社工員ID，取得社工員姓名回傳
    public static string GetSocialWorkerByID(string strUserID)
    {
        string strRet=Util.GetDBValue ("AdminUser","UserName","UserID",strUserID);
        return strRet;
    }
    //---------------------------------------------------------------------------
    //列出i個空白字串
    public static string Space(int i)
    {
        string Str = "";
        for (int j = 1; j <= i; j++)
        {
            Str += "&nbsp;&nbsp;";
        }
        return Str;
    }
    //---------------------------------------------------------------------------
    public static void FillCaseCode(DropDownList ddl, string GroupName, bool AddEmpty)
    {
        string strSql = "";
        strSql = @"
                   select CaseID, CaseName
                   from CaseCode
                   where GroupName=@GroupName
                   order by CaseID
                  ";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("GroupName", GroupName);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        Util.FillDropDownList(ddl, dt, "CaseName", "CaseID", AddEmpty, "", "");
    }
    //---------------------------------------------------------------------------
    public static string GetHouseList(string LandUID)
    {
        string strSql = @"
                            select hd.uid,
                            hd.HouseAddress as [地上物地址(房屋)],
                            hd.HouseNo as [建號]
                            from EstateMap m
                            inner join HouseData hd on m.HouseUID=hd.uid
                            where 1=1
                            and isnull(hd.IsDelete, '') != 'Y'
                            and LandUID=@LandUID
                        ";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("LandUID", LandUID);

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
        npoGridView.EditLink = Util.RedirectByTime("HouseData_Edit.aspx", "HouseUID=");

        NPOGridViewColumn col;
        //-------------------------------------------------------------------------
        col = new NPOGridViewColumn("地上物地址(房屋)");
        col.ColumnType = NPOColumnType.NormalText;
        col.ColumnName.Add("地上物地址(房屋)");
        npoGridView.Columns.Add(col);
        //-------------------------------------------------------------------------
        col = new NPOGridViewColumn("建號");
        col.ColumnType = NPOColumnType.NormalText;
        col.ColumnName.Add("建號");
        npoGridView.Columns.Add(col);
        //-------------------------------------------------------------------------

        return npoGridView.Render();
    }
    //-------------------------------------------------------------------------------------------------------------
    public static string GetLandList(string HouseUID)
    {
        string strSql = @"
                            select ld.uid,
                            ld.LandNo as [土地地號],
                            ld.LandArea as [宗地面積]
                            from EstateMap m
                            inner join LandData ld on m.LandUID=ld.uid
                            where 1=1
                            and isnull(ld.IsDelete, '') != 'Y'
                            and HouseUID=@HouseUID
                        ";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("HouseUID", HouseUID);

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
        npoGridView.EditLink = Util.RedirectByTime("LandData_Edit.aspx", "LandUID=");

        NPOGridViewColumn col;
        //-------------------------------------------------------------------------
        col = new NPOGridViewColumn("土地地號");
        col.ColumnType = NPOColumnType.NormalText;
        col.ColumnName.Add("土地地號");
        npoGridView.Columns.Add(col);
        //-------------------------------------------------------------------------
        col = new NPOGridViewColumn("宗地面積");
        col.ColumnType = NPOColumnType.NormalText;
        col.ColumnName.Add("宗地面積");
        npoGridView.Columns.Add(col);
        //-------------------------------------------------------------------------

        return npoGridView.Render();
    }


    //**********************************temp****************************Nash
    public static string MakeMenu(string strCaseID, int SelectedTab)
    {
        List<CButton> ButtonList = new List<CButton>();
        CButton Btn = new CButton();
        Btn.TabNo = 1;
        Btn.Text = "個案基本資料";
        //Btn.Style = "style='width:200px'";  //需要不同寬度時,可以在此設定新值
        Btn.ImgSrc = "../images/toolbar_exit.gif";
        Btn.OnClick = "AdvisoryBasic_Edit.aspx?CaseID=" + strCaseID;
        Btn.Style = "style='font-size:16px'";  //設定字型大小
        ButtonList.Add(Btn);

        Btn = new CButton();
        Btn.TabNo = 2;
        Btn.Text = "轉介夥伴教會";
        Btn.ImgSrc = "../images/toolbar_export.gif";
        Btn.OnClick = "ClinicCourse_Edit.aspx?CaseID=" + strCaseID;
        Btn.Style = "style='font-size:16px'";  //設定字型大小
        ButtonList.Add(Btn);

        Btn = new CButton();
        Btn.TabNo = 3;
        Btn.Text = "個案紀錄";
        Btn.ImgSrc = "../images/toolbar_folder.gif";
        Btn.OnClick = "AdviseRecord_Edit.aspx?CaseID=" + strCaseID;
        Btn.Style = "style='font-size:16px'";  //設定字型大小
        ButtonList.Add(Btn);

        Btn = new CButton();
        Btn.TabNo = 4;
        Btn.Text = "追蹤建議";
        //Btn.ShowBR = true;
        Btn.ImgSrc = "../images/toolbar_new.gif";
        Btn.OnClick = "TraceSuggest_Edit.aspx?CaseID=" + strCaseID;
        Btn.Style = "style='font-size:16px'";  //設定字型大小
        ButtonList.Add(Btn);

        Btn = new CButton();
        Btn.TabNo = 5;
        Btn.Text = "個案歷程";
        //Btn.ShowBR = true;
        Btn.ImgSrc = "../images/book.gif";
        Btn.OnClick = "AdviseLog.aspx?CaseID=" + strCaseID;
        Btn.Style = "style='font-size:16px'";  //設定字型大小
        ButtonList.Add(Btn);

        return MenuList(ButtonList, SelectedTab);
    }
    //取得是否是登入者本人之案件
    public static bool CheckIsSelfCase(string CaseID, string UserID)
    {
        bool bolRet = false;

        string strSql = @"select * from AdvisoryCase
                        where CaseID=@CaseID and UserID=@UserID
                        ";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("CaseID", CaseID);
        dict.Add("UserID", UserID);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count > 0)
        {
            bolRet = true;
        }
        return bolRet;
    }



} //end of class CaseUtil

public class CButton
{
    private int _TabNo;
    private string _CssClass;
    private string _OnClick;
    private string _ImgSrc;
    private string _Width = "20";
    private string _Height = "20";
    private string _Align = "absbottom";
    private string _Title = "";
    private string _Text;
    private string _Style;
    private bool _ShowBR = false;

    public int TabNo
    {
        get { return _TabNo; }
        set { _TabNo = value; }
    }
    public string CssClass
    {
        get { return _CssClass; }
        set { _CssClass = value; }
    }
    public string OnClick
    {
        get { return _OnClick; }
        set { _OnClick = value; }
    }
    public string ImgSrc
    {
        get { return _ImgSrc; }
        set { _ImgSrc = value; }
    }
    public string Width
    {
        get { return _Width; }
        set { _Width = value; }
    }
    public string Height
    {
        get { return _Height; }
        set { _Height = value; }
    }
    public string Align
    {
        get { return _Align; }
        set { _Align = value; }
    }
    public string Title
    {
        get { return _Title; }
        set { _Title = value; }
    }
    public string Text
    {
        get { return _Text; }
        set { _Text = value; }
    }
    public string Style
    {
        get { return _Style; }
        set { _Style = value; }
    }
    public bool ShowBR
    {
        get { return _ShowBR; }
        set { _ShowBR = value; }
    }
}
