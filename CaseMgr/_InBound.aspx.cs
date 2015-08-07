using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using System.IO;

public partial class CaseMgr__InBound : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        string phone = Util.GetQueryString("phone");
        //HttpCookie CookieAgentID = new HttpCookie("AgentID");
        // CookieAgentID  = Request.Cookies["AgentID"];
        HttpCookie CookieAgentID = Request.Cookies["AgentID"];
        //if (phone.Trim() == "885")
        //{
        //    phone = "0";
        //}
        if (CookieAgentID != null)
        {
            //Response.Write("Cooking-->" + CookieAgentID.Value + "<BR>" + "Phone-->" + phone);
            Dictionary<string, object> dict = new Dictionary<string, object>();
            string strSql = @"
                            insert Into InBound (Phone,IP,InsertDate)
                            Values(@Phone,@IP,@InsertDate)
                                 ";
            dict.Add("Phone", phone);
            dict.Add("IP", Server.UrlDecode(CookieAgentID.Value));
            dict.Add("InsertDate", Util.GetToday(DateType.yyyyMMddHHmmss));
            NpoDB.ExecuteSQLS(strSql, dict);
           // Response.Redirect("ConsultEdit.aspx?UID=" + HFD_UID.Value + "&phone=" + HFD_Phone.Value);
        }
        else
        {
            Response.Write("請先登入諮詢系統");
            return;
        }
        if (phone == "0937934051")
        {
            lblMsg.Text = "   <span style=font-size:36px;>有新來電 號碼：" + phone + "<BR></span> " +
                          //" <p><span style=font-size:36px;>請關閉此視窗後接聽電話<strong>" +
                          //    " <p>	<span style='font-size:36px;'>&nbsp;&nbsp; 接聽前請先</span>" +
                          " <span style='color: rgb(255, 0, 0);font-size:36px;'>此為廠商測試電話請不要接聽</span></strong></span></p>" +
                          " <p>	&nbsp;</p>";
        }
        else
        {
            lblMsg.Text = "   <span style=font-size:36px;>有新來電 號碼：" + phone + "</span> " +
                            " <p><span style=font-size:36px;>請關閉此視窗後接聽電話<strong>" +
                                " <p>	<span style='font-size:36px;'>&nbsp;&nbsp; 接聽前請先</span>" +
                            " <span style='color: rgb(255, 0, 0);font-size:36px;'>儲存資料</span></strong></span></p>" +
                            " <p>	&nbsp;</p>";
        }

    }

}