using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class CaseMgr_InBound2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        string strThisURL = null;
        strThisURL = Request.ServerVariables["URL"] + "?phone=" + Request.QueryString["phone"];

        string Dir = Server.MapPath("~/");
        StreamWriter sw = File.AppendText(Dir + "input.txt");

        sw.Write("<br/>Time is : ");
        sw.WriteLine(DateTime.Now + "<br/>");
        sw.WriteLine(strThisURL + "<br/>");

        sw.Flush();
        sw.Close();

        Label1.Text = strThisURL;
        //Response.Write(strThisURL);

    }
}
