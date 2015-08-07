using System;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class MainTop : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            btnLogout.ImageUrl = "../images/title_logout_bt.gif";  //預設值
            btnLogout.Attributes["onmouseover"] = "EvImageOverChange(this, 'in');";
            btnLogout.Attributes["onmouseout"] = "EvImageOverChange(this, 'out');";
        }

        try
        {
            txtDeptName.Text = SessionInfo.DeptName;
            txtUserGroup.Text = SessionInfo.GroupName + " (" + SessionInfo.UserName + ")";
        }
        catch (Exception ex)
        {
            Response.Write(@"<script>window.parent.location.href='../Default.aspx';</script>");
        }
    }
    //------------------------------------------------------------------------
    protected void btnLogout_Click(object sender, ImageClickEventArgs e)
    {
        string UserID = "";
        if (SessionInfo != null)
        {
            UserID = SessionInfo.UserID;
        }
        Response.Write(@"<script>window.parent.location.href='../Default_Stage.aspx?logout=true&UserID=" + UserID + "';</script>");
    }
    //------------------------------------------------------------------------
    //protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    //{
    //    Response.Write(@"<script>parent.frames['2'].location='../CaseMgr/ConsultFirst.aspx'</script>");
    //}

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        DataRow dr = null;
        DataTable dt = null;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        string strSql;
        strSql = @"
                   select top 1 phone
                   from InBound
                   where IP=@IP  
                   and isnull(IsProcess, '') != 'Y'
                  ";
        strSql += " Order by uid desc";
        HttpCookie CookieAgentID = Request.Cookies["AgentID"];
        dict.Add("IP", Server.UrlDecode(CookieAgentID.Value));//Request.ServerVariables["REMOTE_ADDR"]
        dt = NpoDB.GetDataTableS(strSql, dict);
        //資料異常
        if (dt.Rows.Count == 0)
        {
            // ShowSysMsg("無新來電");
            //Response.Redirect("ConsultEdit.aspx?UID=" + HFD_UID.Value + "&phone=" + HFD_Phone.Value);
            //Response.Redirect("ConsultEdit.aspx?InBound=N");
            return;
        }
      //  dr = dt.Rows[0];
       // HFD_Phone.Value = dr["phone"].ToString().Trim();

        Response.Write(@"<script>parent.frames['2'].location='../CaseMgr/Relay.aspx'</script>");
    }
}
