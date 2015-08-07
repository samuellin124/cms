using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class SysMgr_AdminGroup_Edit : BasePage 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["ProgID"] = "AdminGroup";
        //權控處理
        AuthrityControl();

        if (!IsPostBack)
        {
            HFD_Uid.Value = Util.GetQueryString("Uid");
            LoadFormData();
        }
    }
    //----------------------------------------------------------------------
    public void AuthrityControl()
    {
        if (Authrity.PageRight("_Focus") == false)
        {
            return;
        }
        Authrity.CheckButtonRight("_Delete", btnDelete);
        Authrity.CheckButtonRight("_Update", btnUpdate);
    }
    //----------------------------------------------------------------------
    public void LoadFormData()
    {
        string strSql = " select * from AdminGroup where uid=@uid";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("uid", HFD_Uid.Value);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        //資料異常
        if (dt.Rows.Count <= 0)
        {
            Response.Redirect("Default.aspx");
        }

        DataRow dr = dt.Rows[0];

        //群組名稱
        txtGroupName.Text = dr["GroupName"].ToString();

        //群組描述
        txtGroupDesc.Text = dr["GroupDesc"].ToString();

        ////權限順位
        //Util.SetDdlIndex(DDL_GroupArea, dr["GroupArea"].ToString());

        //是否使用
        if (dr["IsUse"].ToString() == "True")
        {
            rdoIsUse.SelectedIndex = 0;
        }
        else
        {
            rdoIsUse.SelectedIndex = 1;
        }
        ////是否為護士
        //if (dr["IsNurse"].ToString() == "True")
        //{
        //    chkIsNurse.Checked = true;
        //}
        ////是否為社工
        //if (dr["IsSocietyWorker"].ToString() == "True")
        //{
        //    chkIsSocietyWorker.Checked = true;
        //}
    }
    //----------------------------------------------------------------------
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        string strSql = " update AdminGroup set ";
        strSql += "  GroupName = @GroupName";
        strSql += ", GroupDesc = @GroupDesc";
        strSql += ", GroupArea = @GroupArea";
        strSql += ", IsUse = @IsUse";
        //strSql += ", IsNurse = @IsNurse";
        //strSql += ", IsSocietyWorker = @IsSocietyWorker";
        strSql += " where uid = @uid";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("uid", HFD_Uid.Value);
        dict.Add("GroupName", txtGroupName.Text);
        dict.Add("GroupDesc", txtGroupDesc.Text);
        dict.Add("GroupArea", ddlGroupArea.SelectedValue);
        dict.Add("IsUse", rdoIsUse.SelectedIndex == 0 ? 1 : 0);
        //dict.Add("IsNurse", chkIsNurse.Checked);
        //dict.Add("IsSocietyWorker", chkIsSocietyWorker.Checked);

        if (NpoDB.ExecuteSQLS(strSql, dict) == 0)
        {
            ShowSysMsg("修改資料失敗!");
            return;
        }
        SetSysMsg("修改資料成功!");
        Response.Redirect(Util.RedirectByTime("AdminGroup.aspx"));
    }
    //----------------------------------------------------------------------
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("AdminGroup.aspx?rand=" + DateTime.Now.Ticks.ToString());
    }
    //----------------------------------------------------------------------
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        bool flag = false;
        objNpoDB.BeginTrans();
        try
        {
            Delete();
            SetSysMsg("刪除資料成功");
            objNpoDB.CommitTrans();
            flag = true;
        }
        catch (Exception ex)
        {
            objNpoDB.RollbackTrans();
            throw ex;
        }
        if (flag == true)
        {
            Response.Redirect(Util.RedirectByTime("AdminGroup.aspx"));
        }
    }
    //----------------------------------------------------------------------
    private void Delete()
    {
        string strSql = "delete from  AdminGroup where uid=@uid";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("uid", HFD_Uid.Value);
        objNpoDB.ExecuteSQL(strSql, dict);

        strSql = "delete from  AdminRight where GroupID=@uid";
        objNpoDB.ExecuteSQL(strSql, dict);
    }
    //----------------------------------------------------------------------
}
