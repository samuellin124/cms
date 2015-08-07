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
using System.IO;

public partial class FileMgr_FileUpLoad_Delete :BasePage 
{
    private string folderPath;

    protected void Page_Load(object sender, EventArgs e)
    {
        folderPath = Util.GetAppSetting("DocUploadPath");

        if (Util.GetQueryString("upload_id") == "")
        {
            Response.Redirect("FileCats.aspx");
        }

        string strSql,upload_id;
        string AppObject_ID, Upload_FileName, dept_id;
        DataTable dt;
        Dictionary<string, object> dict = new Dictionary<string, object>();

        upload_id = Request.QueryString["upload_id"].ToString();

        strSql = "select * from uploads where upload_id=@upload_id";

        dict.Add("upload_id", upload_id);

        dt = NpoDB.GetDataTableS(strSql, dict);

        if (dt.Rows.Count <= 0)
        {
            Response.Redirect("FileCats.aspx");
        }

        DataRow dr = dt.Rows[0];
        dept_id = dr["dept_id"].ToString();
        AppObject_ID = dr["AppObject_ID"].ToString();
        Upload_FileName = dr["Upload_FileName"].ToString();

        strSql = "delete from uploads where upload_id=@upload_id";
        NpoDB.ExecuteSQLS(strSql, dict);

        string UploadFileFolderPath = Server.MapPath("~" + folderPath);
        string UploadFilePath = UploadFileFolderPath + Upload_FileName;

        File.Delete(UploadFilePath);
        Session["Msg"] = "檔案刪除成功 !";
        Response.Redirect("FileCats.aspx?dept_id=" + dept_id + "&FileCat_id=" + AppObject_ID);
    }
}
