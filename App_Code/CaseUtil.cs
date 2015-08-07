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
            if (LogoutTime != "") //�p�G�P�@�Ѧ� logout, �h�t�s�@��
            {
                uid = "";
            }
            //�O���n�J�ɶ�
            if (uid != "") //�w��UserID+LogTime ��ƦC
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
            //�O���ʧ@�ɶ�
            //�O���n�X�ɶ�
            if (uid == "") //Action �S���۹����� login ���
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
            if (uid == "") //logout �S���۹����� login ���,���B�z, �j�]�|�����D
            {
                return;
            }
            //�O���n�X�ɶ�
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
        //�������ť�
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
            //�������j�󤵤�
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
            //5�����e, �i�H���J�W�Ӥ몺���
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
            //�W�L5��, �h�����J�W�Ӥ몺���
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
    //���ͤW���ɮת� JavaScript �X
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
        Btn.Text = "�g�a�򥻸��";
        Btn.Style = "style='width:35mm'";  //�ݭn���P�e�׮�,�i�H�b���]�w�s��
        Btn.ImgSrc = "../images/toolbar_exit.gif";
        Btn.OnClick = "LandData_Edit.aspx?LandUID=" + strLandUID;
        //Btn.Style = "style='font-size:12px'";  //�]�w�r���j�p
        Btn.Title = "�g�a�򥻸��";
        ButtonList.Add(Btn);

        Btn = new CButton();
        Btn.TabNo = 2;
        Btn.Text = "�ϥα���";
        Btn.Style = "style='width:25mm'";  //�ݭn���P�e�׮�,�i�H�b���]�w�s��
        Btn.ImgSrc = "../images/toolbar_modify.gif";
        Btn.OnClick = "LandData_Edit2.aspx?LandUID=" + strLandUID;
        //Btn.Style = "style='font-size:16px'";  //�]�w�r���j�p
        Btn.Title = "�ϥα���";
        ButtonList.Add(Btn);

        Btn = new CButton();
        Btn.TabNo = 3;
        Btn.Text = "�b������";
        Btn.Style = "style='width:25mm'";  //�ݭn���P�e�׮�,�i�H�b���]�w�s��
        Btn.ImgSrc = "../images/toolbar_familytree.gif";
        Btn.OnClick = "LandData_Edit3.aspx?LandUID=" + strLandUID;
        //Btn.Style = "style='font-size:16px'";  //�]�w�r���j�p
        Btn.Title = "�b������";
        ButtonList.Add(Btn);

        Btn = new CButton();
        Btn.TabNo = 4;
        Btn.Text = "�ӳ��Τ��i�a��";
        Btn.Style = "style='width:35mm'";  //�ݭn���P�e�׮�,�i�H�b���]�w�s��
        Btn.ImgSrc = "../images/toolbar_new.gif";
        Btn.OnClick = "LandDataQry4.aspx?LandUID=" + strLandUID;
        //Btn.Style = "style='font-size:16px'";  //�]�w�r���j�p
        Btn.Title = "�ӳ��Τ��i�a��";
        ButtonList.Add(Btn);

        Btn = new CButton();
        Btn.TabNo = 5;
        Btn.Text = "�ɮפW��";
        Btn.Style = "style='width:35mm'";  //�ݭn���P�e�׮�,�i�H�b���]�w�s��
        Btn.ImgSrc = "../images/toolbar_upload.gif";
        Btn.OnClick = "LandDataUpload.aspx?LandUID=" + strLandUID;
        //Btn.Style = "style='font-size:16px'";  //�]�w�r���j�p
        Btn.Title = "�ɮפW��";
        ButtonList.Add(Btn);

        return MenuList(ButtonList, SelectedTab);
    }
    //-------------------------------------------------------------------------
    public static string MakeHouseMenu(string strHouseUID, int SelectedTab)
    {
        List<CButton> ButtonList = new List<CButton>();
        CButton Btn = new CButton();
        Btn.TabNo = 1;
        Btn.Text = "�Ыΰ򥻸��";
        Btn.Style = "style='width:35mm'";  //�ݭn���P�e�׮�,�i�H�b���]�w�s��
        Btn.ImgSrc = "../images/toolbar_exit.gif";
        Btn.OnClick = "HouseData_Edit.aspx?HouseUID=" + strHouseUID;
        //Btn.Style = "style='font-size:12px'";  //�]�w�r���j�p
        Btn.Title = "�Ыΰ򥻸��";
        ButtonList.Add(Btn);

        Btn = new CButton();
        Btn.TabNo = 2;
        Btn.Text = "�ϥα���";
        Btn.Style = "style='width:25mm'";  //�ݭn���P�e�׮�,�i�H�b���]�w�s��
        Btn.ImgSrc = "../images/toolbar_modify.gif";
        Btn.OnClick = "HouseData_Edit2.aspx?HouseUID=" + strHouseUID;
        //Btn.Style = "style='font-size:16px'";  //�]�w�r���j�p
        Btn.Title = "�ϥα���";
        ButtonList.Add(Btn);

        Btn = new CButton();
        Btn.TabNo = 3;
        Btn.Text = "�b������";
        Btn.Style = "style='width:25mm'";  //�ݭn���P�e�׮�,�i�H�b���]�w�s��
        Btn.ImgSrc = "../images/toolbar_familytree.gif";
        Btn.OnClick = "HouseData_Edit3.aspx?HouseUID=" + strHouseUID;
        //Btn.Style = "style='font-size:16px'";  //�]�w�r���j�p
        Btn.Title = "�b������";
        ButtonList.Add(Btn);

        Btn = new CButton();
        Btn.TabNo = 4;
        Btn.Text = "�Цa�|��";
        Btn.Style = "style='width:35mm'";  //�ݭn���P�e�׮�,�i�H�b���]�w�s��
        Btn.ImgSrc = "../images/toolbar_new.gif";
        Btn.OnClick = "HouseDataQry4.aspx?HouseUID=" + strHouseUID;
        //Btn.Style = "style='font-size:16px'";  //�]�w�r���j�p
        Btn.Title = "�Цa�|��";
        ButtonList.Add(Btn);

        Btn = new CButton();
        Btn.TabNo = 5;
        Btn.Text = "�в�����";
        Btn.Style = "style='width:35mm'";  //�ݭn���P�e�׮�,�i�H�b���]�w�s��
        Btn.ImgSrc = "../images/toolbar_group.gif";
        Btn.OnClick = "HouseDataQry5.aspx?HouseUID=" + strHouseUID;
        //Btn.Style = "style='font-size:16px'";  //�]�w�r���j�p
        Btn.Title = "�в�����";
        ButtonList.Add(Btn);

        Btn = new CButton();
        Btn.TabNo = 6;
        Btn.Text = "�ɮפW��";
        Btn.Style = "style='width:35mm'";  //�ݭn���P�e�׮�,�i�H�b���]�w�s��
        Btn.ImgSrc = "../images/toolbar_upload.gif";
        Btn.OnClick = "HouseDataUpload.aspx?HouseUID=" + strHouseUID;
        //Btn.Style = "style='font-size:16px'";  //�]�w�r���j�p
        Btn.Title = "�ɮפW��";
        ButtonList.Add(Btn);

        return MenuList(ButtonList, SelectedTab);
    }
    //-------------------------------------------------------------------------
    public static string MakeMenuMove(string strCaseID, int SelectedTab)
    {
        List<CButton> ButtonList = new List<CButton>();
        CButton Btn = new CButton();
        Btn.TabNo = 1;
        Btn.Text = "�l�F�]�����ʳ�";
        Btn.Style = "style='width:40mm'";  //�ݭn���P�e�׮�,�i�H�b���]�w�s��
        Btn.ImgSrc = "../images/toolbar_exit.gif";
        Btn.OnClick = "MoveMgrQry.aspx";
        //Btn.Style = "style='font-size:12px'";  //�]�w�r���j�p
        //Btn.Title = "�����@�~�e�Х��s�ɡI";
        ButtonList.Add(Btn);

        Btn = new CButton();
        Btn.TabNo = 2;
        Btn.Text = "�]����l�q����";
        Btn.Style = "style='width:40mm'";  //�ݭn���P�e�׮�,�i�H�b���]�w�s��
        Btn.ImgSrc = "../images/toolbar_modify.gif";
        Btn.OnClick = "AssetsCut_Edit.aspx";
        //Btn.Style = "style='font-size:16px'";  //�]�w�r���j�p
        // Btn.Title = "�����@�~�e�Х��s�ɡI";
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
    //���o�f�͹�����	
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
    //���o�����|�Ҹ��	
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
    //���o�����ӷ����
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
    //���o�����ӷ����
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
    //���o�����ӷ����
    public static DataTable GetFromType()
    {
        DataTable dt = GetCaseCode("�����ӷ�");
        return dt;
    }
    //-------------------------------------------------------------------------
    //���o�����O�]�ӤH�^���
    public static DataTable GetDonorTypePerson()
    {
        DataTable dt = GetCaseCode("�����OP");
        return dt;
    }
    //-------------------------------------------------------------------------
    //���o�����O�]����^���
    public static DataTable GetDonorTypeGroup()
    {
        DataTable dt = GetCaseCode("�����OG");
        return dt;
    }
    //-------------------------------------------------------------------------
    //���o�pô���p���
    public static DataTable GetContactStatus()
    {
        DataTable dt = GetCaseCode("�pô���p");
        return dt;
    }
    //-------------------------------------------------------------------------
    //���o�̤j���ץ�s���A�ץ�s�� ex: A0100001
    public static string GetNewMaxCaseID()
    {
        string strRet = "";

        string strSql = @"select isnull(MAX(CaseID),'') as CaseID from CaseApply ";
        DataTable dt = NpoDB.GetDataTableS(strSql, null);
        //����~��2��
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
    //���o�P�ץ�s�����̤j�ץ������s���A������s�� ex: E0001�A�[�WCaseID���ߤ@��
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
    //���o�P�ץ�s�����̤j�ץ�A�Ȭ�����s���A�A�Ȭ�����s�� ex: S0001�A�[�WCaseID���ߤ@��
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
    //�ˮ֮ץ�s��CaseID�O�_�s�b�ǤJ��Table��
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
    //���o�ǤJ��Table���ץ�s��CaseID���ץ󪬺A
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
    //��s�ǤJ��Table���ץ�s��CaseID���ץ󪬺A
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
    //���o���ɩ��ݳ�줧���u���W��g�JDropdownList
    public static void GetSocialWorkerByDept(DropDownList ddlTarget, string strDeptID, string strGroupID)
    {
        ddlTarget.Items.Clear();
        string strSql = @"select * from AdminUser ";
        strSql += " where 1=1 ";
        //�t�κ޲z���ΥD���i�����U���
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
    //���o���ɩ��ݳ�줧���u���W��^��
    public static string GetSocialWorkerByDept(string strDeptID, string strGroupID)
    {
        string strRet = "";
        string strSql = @"select * from AdminUser ";
        strSql += " where 1=1 ";
        //�t�κ޲z���ΥD���i�����U���
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
    //�ǤJ���u��ID�A���o���u���m�W�^��
    public static string GetSocialWorkerByID(string strUserID)
    {
        string strRet=Util.GetDBValue ("AdminUser","UserName","UserID",strUserID);
        return strRet;
    }
    //---------------------------------------------------------------------------
    //�C�Xi�Ӫťզr��
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
                            hd.HouseAddress as [�a�W���a�}(�Ы�)],
                            hd.HouseNo as [�ظ�]
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
        //�S���ݭn�S�O�B�z������
        NPOGridView npoGridView = new NPOGridView();
        npoGridView.Source = NPOGridViewDataSource.fromDataTable;
        npoGridView.dataTable = dt;
        npoGridView.Keys.Add("uid");
        npoGridView.DisableColumn.Add("uid");
        npoGridView.ShowPage = false;
        npoGridView.EditLink = Util.RedirectByTime("HouseData_Edit.aspx", "HouseUID=");

        NPOGridViewColumn col;
        //-------------------------------------------------------------------------
        col = new NPOGridViewColumn("�a�W���a�}(�Ы�)");
        col.ColumnType = NPOColumnType.NormalText;
        col.ColumnName.Add("�a�W���a�}(�Ы�)");
        npoGridView.Columns.Add(col);
        //-------------------------------------------------------------------------
        col = new NPOGridViewColumn("�ظ�");
        col.ColumnType = NPOColumnType.NormalText;
        col.ColumnName.Add("�ظ�");
        npoGridView.Columns.Add(col);
        //-------------------------------------------------------------------------

        return npoGridView.Render();
    }
    //-------------------------------------------------------------------------------------------------------------
    public static string GetLandList(string HouseUID)
    {
        string strSql = @"
                            select ld.uid,
                            ld.LandNo as [�g�a�a��],
                            ld.LandArea as [�v�a���n]
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
        //�S���ݭn�S�O�B�z������
        NPOGridView npoGridView = new NPOGridView();
        npoGridView.Source = NPOGridViewDataSource.fromDataTable;
        npoGridView.dataTable = dt;
        npoGridView.Keys.Add("uid");
        npoGridView.DisableColumn.Add("uid");
        npoGridView.ShowPage = false;
        npoGridView.EditLink = Util.RedirectByTime("LandData_Edit.aspx", "LandUID=");

        NPOGridViewColumn col;
        //-------------------------------------------------------------------------
        col = new NPOGridViewColumn("�g�a�a��");
        col.ColumnType = NPOColumnType.NormalText;
        col.ColumnName.Add("�g�a�a��");
        npoGridView.Columns.Add(col);
        //-------------------------------------------------------------------------
        col = new NPOGridViewColumn("�v�a���n");
        col.ColumnType = NPOColumnType.NormalText;
        col.ColumnName.Add("�v�a���n");
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
        Btn.Text = "�Ӯװ򥻸��";
        //Btn.Style = "style='width:200px'";  //�ݭn���P�e�׮�,�i�H�b���]�w�s��
        Btn.ImgSrc = "../images/toolbar_exit.gif";
        Btn.OnClick = "AdvisoryBasic_Edit.aspx?CaseID=" + strCaseID;
        Btn.Style = "style='font-size:16px'";  //�]�w�r���j�p
        ButtonList.Add(Btn);

        Btn = new CButton();
        Btn.TabNo = 2;
        Btn.Text = "�श�٦�з|";
        Btn.ImgSrc = "../images/toolbar_export.gif";
        Btn.OnClick = "ClinicCourse_Edit.aspx?CaseID=" + strCaseID;
        Btn.Style = "style='font-size:16px'";  //�]�w�r���j�p
        ButtonList.Add(Btn);

        Btn = new CButton();
        Btn.TabNo = 3;
        Btn.Text = "�Ӯ׬���";
        Btn.ImgSrc = "../images/toolbar_folder.gif";
        Btn.OnClick = "AdviseRecord_Edit.aspx?CaseID=" + strCaseID;
        Btn.Style = "style='font-size:16px'";  //�]�w�r���j�p
        ButtonList.Add(Btn);

        Btn = new CButton();
        Btn.TabNo = 4;
        Btn.Text = "�l�ܫ�ĳ";
        //Btn.ShowBR = true;
        Btn.ImgSrc = "../images/toolbar_new.gif";
        Btn.OnClick = "TraceSuggest_Edit.aspx?CaseID=" + strCaseID;
        Btn.Style = "style='font-size:16px'";  //�]�w�r���j�p
        ButtonList.Add(Btn);

        Btn = new CButton();
        Btn.TabNo = 5;
        Btn.Text = "�Ӯ׾��{";
        //Btn.ShowBR = true;
        Btn.ImgSrc = "../images/book.gif";
        Btn.OnClick = "AdviseLog.aspx?CaseID=" + strCaseID;
        Btn.Style = "style='font-size:16px'";  //�]�w�r���j�p
        ButtonList.Add(Btn);

        return MenuList(ButtonList, SelectedTab);
    }
    //���o�O�_�O�n�J�̥��H���ץ�
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
