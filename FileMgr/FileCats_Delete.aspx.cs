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

public partial class FileMgr_FileCats_Delete :BasePage 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Util.GetQueryString("filecat_id") == "")
        {
            Response.Redirect("FileCat.aspx");
        }

        string strSql, filecat_id ;
        DataTable dt;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        string FileCat_ParentID="",FileCat_Name="",dept_id="";

        filecat_id = Request.QueryString["filecat_id"].ToString();

        strSql = "select uploads.*,FileCat_Name,FileCat_ParentID,uploads.dept_id from uploads join filecats on uploads.AppObject_ID=filecats.filecat_id where AppObject_ID=@filecat_id ";
        dict.Add("filecat_id", filecat_id) ;

        dt = NpoDB.GetDataTableS(strSql, dict);

        //處理邏輯
        if (dt.Rows.Count > 0)
        {
            DataRow dr  = dt.Rows[0];; 
            Session["Msg"] = dr["FileCat_Name"] + "資料夾內還有檔案, 不能刪除此資料夾 !";
            Response.Redirect("FileCats.aspx?dept_id=" + dr["dept_id"] + "&filecat_id=" + dr["FileCat_ParentID"]);
        }
        dt.Dispose();

        strSql = "select * from filecats where filecat_id=@filecat_id";
        dt = NpoDB.GetDataTableS(strSql, dict);
        if(dt.Rows.Count >0)
        {
            DataRow  dr = dt.Rows[0];
            FileCat_ParentID=dr["FileCat_ParentID"].ToString();
            FileCat_Name=dr["FileCat_Name"].ToString();
            dept_id = dr["dept_id"].ToString();
        }

        DataTable dt2;
        strSql = "select * from filecats where FileCat_ParentID=@filecat_id ";
        dt2 = NpoDB.GetDataTableS(strSql, dict);
        if (dt2.Rows.Count > 0)
        {
            DataRow dr = dt2.Rows[0]; ;
            Session["Msg"] = FileCat_Name + "資料夾內還有子資料夾, 不能刪除此資料夾 !";
            Response.Redirect("FileCats.aspx?dept_id=" + dr["dept_id"] + "&filecat_id=" + FileCat_ParentID);
        }

        strSql = " delete from filecats where filecat_id=@filecat_id ";
        NpoDB.ExecuteSQLS(strSql, dict);

        Session["Msg"] = FileCat_Name + "資料夾刪除成功 !";
        Response.Redirect("FileCats.aspx?dept_id=" + dept_id + "&filecat_id=" + FileCat_ParentID);
    }
}
