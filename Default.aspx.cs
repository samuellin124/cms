using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Collections;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        txtPassword.Attributes["onkeypress"] = "if(event.keyCode == 13){document.forms[0].submit();}";

        //開發人員通道
        if (Request.ServerVariables["REMOTE_ADDR"] == "127.0.0.1" ||
            Request.ServerVariables["REMOTE_ADDR"] == "::1")  //IPV6
        {
            string scriptString = "<script>";
            scriptString += "document.getElementById('txtUserID').value='';";
            scriptString += "document.getElementById('txtPassword').value='';";
            scriptString += "</script>";
            Page.ClientScript.RegisterStartupScript(GetType(), "Test", scriptString);
        }
        //------------------------------------------------------------------------
        //如果今天登入錯誤三次，看不到畫面了
        //if (Request.Cookies["ErrorCount"] != null)
        //{
        //    if (Convert.ToInt16(Request.Cookies["ErrorCount"].Value) >= 3)
        //    {
        //        ShowSysMsg("登入錯誤達三次，您的帳號已經暫時封鎖");
        //        return;
        //    }
        //}

        if (!IsPostBack)
        {
            //檢查第一次進入或是 logout 轉入此頁面
            string Logout = Util.GetQueryString("logout");
            if (Logout == "true")
            {
                if (Session["SessionInfo"] != null && Session["SessionInfo"].ToString() != "")
                {
                    Session.Clear();
                }

                string UserID = Util.GetQueryString("UserID");
                CaseUtil.LogData(CaseUtil.LogType.LogoutTime, UserID);
            }
            
            GetOrgData();

            //為登入人員記載登入資訊
            if (Request.Cookies["OrgID"] != null)
            {
                ddlOrg.SelectedIndex = int.Parse(Request.Cookies["OrgID"].Value.ToString());
            }
            if (Request.Cookies["UserID"] != null)
            {
                txtUserID.Text = Request.Cookies["UserID"].Value.ToString();
            }
        }
        else 
        {
            Login();
        }
    }
    //------------------------------------------------------------------------------
    public enum LogType
    {
        LogTime,
        ActionTime,
        LogoutTime
    }
    //------------------------------------------------------------------------------
    //機構名稱
    public void GetOrgData()
    {
        string strSql = "";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        strSql = "select distinct OrgShortName, OrgID from Organization order by 2";
        DataTable dt = NpoDB.GetDataTableS(strSql, null);
        Util.FillDropDownList(ddlOrg, dt, "OrgShortName", "OrgID", false);
    }
    //------------------------------------------------------------------------------
    //檢查系統登入是否成功函數
    public bool CheckLogin(string OrgID, string UserID, string Password)
    {
        bool flag;
        string strSql = "";
        strSql = "select o.OrgShortName, u.PwdValidDate,u.uid,u.UserID,\n";
        strSql += "u.UserName, g.GroupName, u.OrgID, o.OrgName,\n";
        strSql += "u.DeptID, d.DeptName, g.uid as GroupID,\n";
        strSql += "d.uid as DeptID\n";
        strSql += "from AdminUser u\n";
        strSql += "left join AdminGroup g on g.uid=u.GroupID\n";
        strSql += "left join Organization o on u.OrgID=o.OrgID\n";
        strSql += "left join Dept d on u.DeptID=d.DeptID\n";
        strSql += "where u.UserID=@UserID and u.Password=@Password\n";
        strSql += "and u.OrgID=@OrgID and u.IsUse=1\n"; //鎖定使用者無法使用
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("UserID", UserID);
        dict.Add("Password", Password);
        dict.Add("OrgID", OrgID);

        DataTable dt = NpoDB.GetDataTableS(strSql, dict);

        if (dt.Rows.Count == 1)
        {
            SetSeesionInfo(dt.Rows[0]);
            flag = true;  //登入成功
        }
        else
        {
            flag = false;
        }
        return flag;
    }
    //------------------------------------------------------------------------------
    protected void Login()
    {
        bool flag;
        //判斷系統登入是否成功，成功之後導入main.aspx
        flag = CheckLogin(ddlOrg.SelectedValue, txtUserID.Text, txtPassword.Text);
        if (flag == true)
        {
            //Expires 為Cookies加上時間性的有效性
            Response.Cookies["UserID"].Value = txtUserID.Text;
            Response.Cookies["UserID"].Expires = DateTime.Now.AddMonths(6);
            Response.Cookies["OrgID"].Value = ddlOrg.SelectedIndex.ToString();
            Response.Cookies["OrgID"].Expires = DateTime.Now.AddMonths(6);

            Response.Redirect("SysMgr/Main.aspx");
        }
        else
        {
            this.Page.RegisterStartupScript("s", "<script>alert('帳號密碼輸入錯誤或已停用帳號！');</script>");
            //Util.CreateJavaScript("<script>alert('帳號密碼輸入錯誤');setCookie('ErrorCount',eval(getCookie('ErrorCount'))+1);</script>"); 
        }
    }
    //------------------------------------------------------------------------------
    //設定Session物件
    public void SetSeesionInfo(DataRow dr)
    {
        CSessionInfo SessionInfo = new CSessionInfo();

        SessionInfo.OrgID = dr["OrgID"].ToString();          //機構代號
        SessionInfo.OrgName = dr["OrgName"].ToString();      //機構名稱
        SessionInfo.UserID = dr["UserID"].ToString();        //使用者代號
        SessionInfo.UserName = dr["UserName"].ToString();    //使用者名稱
        SessionInfo.DeptID = dr["DeptID"].ToString();        //部門ID
        SessionInfo.DeptName = dr["DeptName"].ToString();    //部門名稱
        SessionInfo.GroupID = dr["GroupID"].ToString();      //使用者權限群組
        SessionInfo.GroupName = dr["GroupName"].ToString();  //使用者權限群組名稱

        Session["SessionInfo"] = SessionInfo;

        CaseUtil.LogData(CaseUtil.LogType.LogTime, SessionInfo.UserID);
    }
}
