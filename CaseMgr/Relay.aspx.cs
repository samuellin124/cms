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

public partial class CaseMgr_Relay : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
      string strSql;
        DataRow dr = null;
        DataTable dt = null;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        Dictionary<string, object> dict2 = new Dictionary<string, object>();
        Dictionary<string, object> dict3 = new Dictionary<string, object>();

        //由電話系統取得電話**************************************************************
        strSql = @"
                   select top 1 phone
                   from InBound
                   where IP=@IP  
                   and isnull(IsProcess, '') != 'Y'
                  ";
        strSql += " Order by uid desc";
        HttpCookie CookieAgentID = Request.Cookies["AgentID"];  
        dict.Add("IP",  Server.UrlDecode(CookieAgentID.Value));//Request.ServerVariables["REMOTE_ADDR"]
        dt = NpoDB.GetDataTableS(strSql, dict);
        //資料異常
        if (dt.Rows.Count == 0)
        {
           // ShowSysMsg("無新來電");
            //Response.Redirect("ConsultEdit.aspx?UID=" + HFD_UID.Value + "&phone=" + HFD_Phone.Value);
            Response.Redirect("ConsultEdit.aspx?InBound=N");
            //return;
        }
        dr = dt.Rows[0];
        HFD_Phone.Value = dr["phone"].ToString().Trim();
        //Response.Write("UID=>" + HFD_Phone.Value);

        //將此client IP 的是否已接聽得值 都update 為 已接聽***************************************************************

        strSql = @"
                   Update InBound 
                   Set IsProcess='Y',
                       UpdateID=@UpdateID,
                       UpdateDate=@UpdateDate
                   where 1=1
                   And IP=@IP
                   and isnull(IsProcess, '') != 'Y'
                  ";
       
        dict2.Add("IP", Server.UrlDecode(CookieAgentID.Value));//Request.ServerVariables["REMOTE_ADDR"]
        dict2.Add("UpdateID", SessionInfo.UserID);
        dict2.Add("UpdateDate", Util.GetToday(DateType.yyyyMMddHHmmss));
        NpoDB.ExecuteSQLS(strSql, dict2);

        //從電話號碼找會員資料*******************************************
        strSql = @"
                   select top 1 uid,phone from Member
                   where phone like @phone
                   and isnull(IsDelete, '') != 'Y'
                  ";
        //dict3.Add("phone", HFD_Phone.Value);
        if ((HFD_Phone.Value.ToString().Length) >= 6)
        {
            dict3.Add("phone", "%" + HFD_Phone.Value + "%");
        }
        else
        {
            dict3.Add("phone", "" + HFD_Phone.Value + "");
        }
        dt = NpoDB.GetDataTableS(strSql, dict3);
        //資料異常
        if (dt.Rows.Count == 0)
        {
            HFD_UID.Value = "";
        }
        else
        {
            dr = dt.Rows[0];
            HFD_UID.Value = dr["uid"].ToString();
        }
        if (HFD_Phone.Value == "unknown")
        {
          //  HFD_UID.Value = ""; 
            HFD_Phone.Value = "0";
        }

        Response.Redirect("ConsultEdit.aspx?UID=" + HFD_UID.Value + "&phone=" + HFD_Phone.Value);
    }
    
}