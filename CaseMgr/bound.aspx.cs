using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class bound : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string txtValue = null;
        string Dir = Server.MapPath("~/");
        StreamReader MySF = new StreamReader(Dir + "input.txt", System.Text.Encoding.UTF8);
        txtValue = MySF.ReadToEnd();
        MySF.Close();
        Label1.Text = txtValue;

    }
}
