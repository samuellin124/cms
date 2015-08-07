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

public partial class FileMgr_FileCats_Edit :BasePage 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Inital();
            Form_DataBind();
        }
    }
    //-------------------------------------------------------------------------
    public void Inital()
    {
        HFD_FileCat_ParentID.Value = Util.GetQueryString("FileCat_ParentID");
        HFD_Filecat_Id.Value = Util.GetQueryString("filecat_id");
    }
    //-------------------------------------------------------------------------
    public void Form_DataBind()
    {
        string strSql, filecat_id;
        DataTable dt;
        Dictionary<string, object> dict = new Dictionary<string, object>();

        filecat_id = HFD_Filecat_Id.Value;

        strSql = " select * from FileCats where filecat_id=@filecat_id";
        dict.Add("filecat_id", filecat_id);


        dt = NpoDB.GetDataTableS(strSql, dict);
        //資料異常
        if (dt.Rows.Count == 0)
        {
            Response.Redirect("FileCats.aspx");
        }
        DataRow dr = dt.Rows[0];
        FD_FileCat_Name.Text = dr["filecat_name"].ToString(); ;
        FD_FileCat_SrotID.Text = dr["filecat_sortid"].ToString();
    }
    //-------------------------------------------------------------------------
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string strSql;
        string filecat_id, FileCat_Name, FileCat_ParentID, FileCat_SortID;
        string  FileCat_UpdateBy;
        Dictionary<string, object> dict = new Dictionary<string, object>();

        filecat_id = HFD_Filecat_Id.Value;
        FileCat_Name = FD_FileCat_Name.Text;
        FileCat_ParentID = filecat_id;
        FileCat_SortID = FD_FileCat_SrotID.Text;
        FileCat_UpdateBy = SessionInfo.UserName;
        FileCat_ParentID = HFD_FileCat_ParentID.Value; 

        strSql  = " update FileCats set FileCat_Name = @FileCat_Name ," ; 
        strSql += " FileCat_ParentID = @FileCat_ParentID , " ;     
        strSql += " FileCat_SortID = @FileCat_SortID , ";
        strSql += " FileCat_UpdateBy = @FileCat_UpdateBy ,";
        strSql += " FileCat_UpdateDate = getdate() ";
        strSql += " Where filecat_id=@filecat_id ";

        dict.Add("filecat_id", filecat_id);
        dict.Add("FileCat_Name", FileCat_Name);
        dict.Add("FileCat_ParentID", FileCat_ParentID);
        dict.Add("FileCat_SortID", FileCat_SortID);
        dict.Add("FileCat_UpdateBy", FileCat_UpdateBy);

        NpoDB.ExecuteSQLS(strSql, dict);

        Session["Msg"] = "修改資料夾成功";
        string LocationHref = "FileCats.aspx?filecat_id=" + FileCat_ParentID;
        RegisterStartupScript("js", @"<script language='javascript'>opener.window.location.href='" + LocationHref + @"';window.close();</script>");
    }
    //-------------------------------------------------------------------------
}
