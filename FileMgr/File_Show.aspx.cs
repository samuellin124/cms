using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic ;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class FileMgr_File_Show :BasePage 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Util.GetQueryString("upload_id") == "")
        {
            Response.Redirect("FileCats.aspx");
        }
        DataTable dt;
        Dictionary<string,object> dict = new Dictionary<string,object>();

        string strSql ;
        string upload_id;
        upload_id  = Request.QueryString["upload_id"].ToString();
        strSql  = "select * from uploads where upload_id=@upload_id ;\n";
        strSql += "update uploads set download_count=download_count+1 where upload_id=@upload_id\n";
        dict.Add("upload_id",upload_id);
        dt = NpoDB.GetDataTableS(strSql, dict); 

        string upload_filename ;
        string Extension;

        upload_filename = dt.Rows[0]["Upload_FileName"].ToString();

        Extension = System.IO.Path.GetExtension(upload_filename ).ToUpper();

        string folderPath = ".." + Util.GetAppSetting("DocUploadPath");
        if (Extension == ".GIF" || Extension == ".JPG")
        {
            lblFileDownLoad.Text = "<img border='0' src='" + folderPath + upload_filename + "'>";
        }
        else
        {
            RegisterStartupScript("js", "<script language='javascript'>location.href='" + folderPath + upload_filename + "'</script>");
        }
    }
}
