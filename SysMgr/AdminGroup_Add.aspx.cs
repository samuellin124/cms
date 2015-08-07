using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class SysMgr_AdminGroup_Add : BasePage 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
        }
    }
    //----------------------------------------------------------------------
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string strSql = "insert into  AdminGroup\n";
        strSql += "( GroupName, GroupDesc, GroupArea, IsUse) values\n";
        strSql += "(@GroupName,@GroupDesc,@GroupArea,@IsUse);";
        strSql += "\n";
        strSql += "select @@IDENTITY";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("GroupName", txtGroupName.Text);
        dict.Add("GroupDesc", txtGroupDesc.Text);
        dict.Add("GroupArea", ddlGroupArea.SelectedValue);
        dict.Add("IsUse", rdoIsUse.SelectedIndex == 0 ? 1 : 0);

        HFD_Uid.Value = NpoDB.GetScalarS(strSql, dict);
        ShowSysMsg("新增資料成功!");
    }
    //----------------------------------------------------------------------
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("AdminGroup.aspx?rand=" + DateTime.Now.Ticks.ToString());
    }
    //----------------------------------------------------------------------
}
