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

public partial class SysMgr_Dept_Edit : BasePage
{
    //程式載入時候的處理
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["ProID"] = "Dept";
        //權限控制
        AuthrityControl();

        //取得模式, 新增("ADD")或修改("")
        HFD_Mode.Value = Util.GetQueryString("Mode");

        //設定一些按鍵的 Visible 屬性
        //注意:要在 AuthrityControl() 及取得 HFD_Mode.Value 呼叫
        SetButton();

        if (!IsPostBack)
        {
            HFD_DeptUID.Value = Util.GetQueryString("DeptUID");
            LoadDropDownListData();
            //if (HFD_Mode.Value != "ADD")
            //{
                ddlOrgID.Enabled = false;
            //}
            LoadFormData();
        }
    }
    //------------------------------------------------------------------------------
    private void AuthrityControl()
    {
        if (Authrity.PageRight("_Focus") == false)
        {
            return;
        }
        Authrity.CheckButtonRight("_Update", btnUpdate);
        Authrity.CheckButtonRight("_Delete", btnDelete);
        //Authrity.CheckButtonRight("_Print", btnExit);
    }
    //-------------------------------------------------------------------------------------------------------------
    public void LoadDropDownListData()
    {
        string strSql = " select OrgName, OrgID from Organization";
        DataTable dt = NpoDB.GetDataTableS(strSql, null);
        Util.FillDropDownList(ddlOrgID, dt, "OrgName", "OrgID", true);
    }
    //------------------------------------------------------------------------------
    private void SetButton()
    {
        //如果是新增,要做一些初始畫的動作
        if (HFD_Mode.Value == "ADD")
        {
            //新增時, 修改及刪除鍵沒動作
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            txtDeptID.ReadOnly = false;
        }
        else
        {
            //修改時, 新增鍵沒動作
            btnAdd.Visible = false;
        }
    }
    //-------------------------------------------------------------------------
    public void LoadFormData()
    {
        string strSql = "";
        DataTable dt = null;
        DataRow dr = null;
        if (HFD_Mode.Value != "ADD")
        {
            strSql = "select OrgID, DeptID, DeptName from Dept\n";
            strSql += "where uid=@uid\n";
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("uid", HFD_DeptUID.Value);
            dt = NpoDB.GetDataTableS(strSql, dict);
            if (dt.Rows.Count == 0)
            {
                //資料異常
                SetSysMsg("資料異常!");
                Response.Redirect("Dept.aspx");
                return;
            }
            dr = dt.Rows[0];
        }
        txtDeptID.Text = (HFD_Mode.Value == "ADD") ? "" : dr["DeptID"].ToString();
        txtDeptName.Text = (HFD_Mode.Value == "ADD") ? "" : dr["DeptName"].ToString();
        if (HFD_Mode.Value == "ADD")
        {
            Util.SetDdlIndex(ddlOrgID, SessionInfo.OrgID);
        }
        else
        {
            Util.SetDdlIndex(ddlOrgID, dr["OrgID"].ToString());
        }
    }
    //------------------------------------------------------------------------------
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (txtDeptID.Text.Trim() == "" || txtDeptName.Text.Trim() == "")
        {
            ShowSysMsg("部門代號及部門名稱不可空白！請重新輸入。");
            return;
        }
        if (CheckDuplDeptID())
        {
            ShowSysMsg("部門代號重複！請重新輸入。");
            return;
        }
        Dictionary<string, object> dict = new Dictionary<string, object>();
        List<ColumnData> list = new List<ColumnData>();
        SetColumnData(list);
        string strSql = Util.CreateInsertCommand("Dept", list, dict);
        NpoDB.ExecuteSQLS(strSql, dict);
        Session["Msg"] = "新增資料成功";
        Response.Redirect(Util.RedirectByTime("Dept.aspx"));
    }
    //------------------------------------------------------------------------------
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (txtDeptID.Text.Trim() == "" || txtDeptName.Text.Trim() == "")
        {
            ShowSysMsg("部門代號及部門名稱不可空白！請重新輸入。");
            return;
        }
        Dictionary<string, object> dict = new Dictionary<string, object>();
        List<ColumnData> list = new List<ColumnData>();
        SetColumnData(list);
        string strSql = Util.CreateUpdateCommand("Dept", list, dict);
        NpoDB.ExecuteSQLS(strSql, dict);
        Session["Msg"] = "修改資料成功";
        Response.Redirect(Util.RedirectByTime("Dept.aspx"));
    }
    //------------------------------------------------------------------------------
    private bool CheckDuplDeptID()
    {
        bool bolRet = false;

        string strSql = "";

        strSql = "select * from Dept where DeptID=@DeptID and OrgID=@OrgID ";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("DeptID", txtDeptID.Text);
        dict.Add("OrgID", ddlOrgID.SelectedValue);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count > 0)
        {
            bolRet = true;
        }
        return bolRet;
    }
    //------------------------------------------------------------------------------
    private void SetColumnData(List<ColumnData> list)
    {
        list.Add(new ColumnData("Uid", HFD_DeptUID.Value, false, false, true));

        list.Add(new ColumnData("OrgID", ddlOrgID.SelectedValue, true, false, false));
        list.Add(new ColumnData("DeptID", txtDeptID.Text, true, true, false));
        list.Add(new ColumnData("DeptName", txtDeptName.Text, true, true, false));
    }
    //-------------------------------------------------------------------------------------------------------------
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        string strSql = "";

        strSql = "select UserName from AdminUser where DeptID=@DeptID";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("DeptID", txtDeptID.Text);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        //有帳號使用此部門代碼，導回機構管理頁面
        if (dt.Rows.Count > 0)
        {
            Session["Msg"] = "" + dt.Rows[0]["UserName"].ToString() + "使用此部門代碼，無法刪除";
            Response.Redirect("Dept.aspx");
            return;
        }

        dict.Clear();
        dict.Add("uid", HFD_DeptUID.Value);
        strSql = "delete from Dept where uid=@uid";
        NpoDB.ExecuteSQLS(strSql, dict);
        Session["Msg"] = "刪除資料成功";
        Response.Redirect("Dept.aspx");
    }
    //------------------------------------------------------------------------------
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("Dept.aspx");
    }
    //------------------------------------------------------------------------------
}
