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
/********************************************
 * 程式的HEADSCRIPT要放這一段
<script type="text/javascript"  language="javascript">
     function openFileCatWindow(Ap_Name)
     {
        window.open("FileCats_Add.aspx","filecat","scrollbars=no,status=no,toolbar=no,top=100,left=120,width=580,height=200");
     }
</script>
 * 程式的原始碼裡面要放這一段
<asp:Button runat="server" Text=" 新增資料夾 " ID="btnNewFolder" CssClass="cbutton"  OnClientClick="openFileCatWindow('FileSys');return false;" />
********************************************/
public partial class FileMgr_FileCats_Add :BasePage 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Inital();
        }
    }
    //-------------------------------------------------------------------------
    public void Inital()
    {
        HFD_dept_id.Value = Util.GetQueryString("dept_id");
        HFD_filecat_id.Value =  Util.GetQueryString("filecat_id");
    }
    //-------------------------------------------------------------------------
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string strSql,sys_date;
        string filecat_id, dept_id, FileCat_Name, FileCat_ParentID, FileCat_SortID;
        string FileCat_CreatedBy,FileCat_CreatedDate,FileCat_UpdateDate, FileCat_UpdateBy;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        DataTable dt;

        sys_date = Util.DateTime2String(DateTime.Now, DateType.yyyyMMddHHmmss, EmptyType.ReturnNull);
        dept_id = HFD_dept_id.Value;
        filecat_id = HFD_filecat_id.Value; 
        FileCat_Name = FD_FileCat_Name.Text;
        FileCat_ParentID = filecat_id;
        FileCat_SortID = FD_FileCat_SrotID.Text;
        FileCat_CreatedBy = SessionInfo.UserName;
        FileCat_CreatedDate = sys_date;
        FileCat_UpdateBy = SessionInfo.UserName;
        FileCat_UpdateDate = sys_date;

        strSql  = " insert into filecats(  dept_id, FileCat_Name, FileCat_ParentID, FileCat_SortID, ";
        strSql += " FileCat_CreatedBy, FileCat_CreatedDate, FileCat_UpdateBy, ";
        strSql += " FileCat_UpdateDate ) ";
        strSql += " values(  @dept_id, @FileCat_Name, @FileCat_ParentID, @FileCat_SortID, ";
        strSql += " @FileCat_CreatedBy, @FileCat_CreatedDate, @FileCat_UpdateBy, ";
        strSql += " @FileCat_UpdateDate ";
        strSql += " )";

        dict.Clear();
        dict.Add("dept_id", dept_id);
        dict.Add("FileCat_Name", FileCat_Name);
        dict.Add("FileCat_ParentID", FileCat_ParentID);
        dict.Add("FileCat_SortID", FileCat_SortID);
        dict.Add("FileCat_CreatedBy", FileCat_CreatedBy);
        dict.Add("FileCat_CreatedDate", FileCat_CreatedDate);
        dict.Add("FileCat_UpdateBy", FileCat_UpdateBy);
        dict.Add("FileCat_UpdateDate", FileCat_UpdateDate);

        NpoDB.ExecuteSQLS(strSql, dict);

        Session["Msg"] = "新增資料夾成功";
        string LocationHref = "FileCats.aspx?dept_id="+dept_id+"&filecat_id="+filecat_id;
        //RegisterStartupScript("js", @"<script language='javascript'>opener.window.location.href='" + LocationHref + @"';window.close();</script>");
        this.ClientScript.RegisterStartupScript(this.GetType(), "js", @"<script language='javascript'>opener.window.location.href='" + LocationHref + @"';window.close();</script>");
    }
    //-------------------------------------------------------------------------
}
