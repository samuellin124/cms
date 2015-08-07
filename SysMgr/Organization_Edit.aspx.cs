using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class SysMgr_Organization_Edit :BasePage 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["ProgID"] = "Organization";
        //權控處理
        AuthrityControl();

        txtOrgID.ReadOnly = true;

        //取得模式, 新增("ADD")或修改("")
        HFD_Mode.Value = Util.GetQueryString("Mode");

        //設定一些按鍵的 Visible 屬性
        //注意:要在 AuthrityControl() 及取得 HFD_Mode.Value 呼叫
        SetButton();

        HFD_OrgUID.Value = Util.GetQueryString("OrgUID");
        
        ShowSysMsg();
        if (!IsPostBack)
        {
            txtOrgID.Text = Util.GetQueryString("OrgID");
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
    private void SetButton()
    {
        //如果是新增,要做一些初始畫的動作
        if (HFD_Mode.Value == "ADD")
        {
            //新增時, 修改及刪除鍵沒動作
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            //btnPrint.Visible = false;
            txtOrgID.ReadOnly = false; 
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
            strSql = "select * from Organization where OrgID=@OrgID";
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("OrgID", HFD_OrgUID.Value);
            dt = NpoDB.GetDataTableS(strSql, dict);
            //資料異常
            if (dt.Rows.Count == 0)
            {
                SetSysMsg("查無資料!");
                Response.Redirect(Util.RedirectByTime("Organization.aspx"));
            }
            dr = dt.Rows[0];
        }
        txtOrgID.Text = (HFD_Mode.Value == "ADD") ? "" : dr["OrgID"].ToString();
        txtOrgName.Text = (HFD_Mode.Value == "ADD") ? "" : dr["OrgName"].ToString();
        txtOrgShortName.Text = (HFD_Mode.Value == "ADD") ? "" : dr["OrgShortName"].ToString();
        txtContactor.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Contactor"].ToString();
        txtEmail.Text = (HFD_Mode.Value == "ADD") ? "" : dr["email"].ToString();
        txtPasswordDay.Text = (HFD_Mode.Value == "ADD") ? "" : dr["PasswordDay"].ToString();
        txtAddress.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Address"].ToString();
        txtTel.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Tel"].ToString();
        txtFax.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Fax"].ToString();
        txtIP.Text = (HFD_Mode.Value == "ADD") ? "" : dr["IP"].ToString();
        txtLimitDay.Text = (HFD_Mode.Value == "ADD") ? "" : dr["LimitDay"].ToString();
    }
    //-------------------------------------------------------------------------
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        List<ColumnData> list = new List<ColumnData>();
        SetColumnData(list);
        string strSql = Util.CreateInsertCommand("Organization", list, dict);
        NpoDB.ExecuteSQLS(strSql, dict);
        Session["Msg"] = "新增資料成功";
        Response.Redirect(Util.RedirectByTime("Organization.aspx"));
    }
    //------------------------------------------------------------------------------
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        List<ColumnData> list = new List<ColumnData>();
        SetColumnData(list);
        string strSql = Util.CreateUpdateCommand("Organization", list, dict);
        NpoDB.ExecuteSQLS(strSql, dict);
        Session["Msg"] = "修改資料成功";
        Response.Redirect(Util.RedirectByTime("Organization.aspx"));
    }
    //-------------------------------------------------------------------------
    private void SetColumnData(List<ColumnData> list)
    {
        if (HFD_Mode.Value == "ADD") //新增時要以 TextBox 的資料為來源
        {
            list.Add(new ColumnData("OrgID", txtOrgID.Text, true, false, true));
        }
        else
        {
            list.Add(new ColumnData("OrgID", HFD_OrgUID.Value, true, false, true));
        }
        list.Add(new ColumnData("OrgName", txtOrgName.Text, true, true, false));
        list.Add(new ColumnData("OrgShortName", txtOrgShortName.Text, true, true, false));
        list.Add(new ColumnData("Contactor", txtContactor.Text, true, true, false));
        list.Add(new ColumnData("Email", txtEmail.Text, true, true, false));
        list.Add(new ColumnData("PasswordDay", txtPasswordDay.Text, true, true, false));
        list.Add(new ColumnData("Address", txtAddress.Text, true, true, false));
        list.Add(new ColumnData("Tel", txtTel.Text, true, true, false));
        list.Add(new ColumnData("Fax", txtFax.Text, true, true, false));
        list.Add(new ColumnData("IP", txtIP.Text, true, true, false));
        list.Add(new ColumnData("LimitDay", txtLimitDay.Text, true, true, false));
    }
    //-------------------------------------------------------------------------
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        string strSql;

        strSql = " select DeptDesc from Dept where OrgID=@OrgID";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("OrgID", txtOrgID.Text);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        //有部門使用此機構代碼，導回機構管理頁面
        if (dt.Rows.Count > 0)
        {
            Session["Msg"] = "" + dt.Rows[0]["DeptDesc"].ToString() + "使用此機構代碼，無法刪除!!";
            Response.Redirect("Organization.aspx?rand=" + DateTime.Now.Ticks.ToString() + "");
            return;
        }

        strSql = " delete Organization Where OrgID=@OrgID";
        NpoDB.ExecuteSQLS(strSql, dict);

        Session["Msg"] = "刪除資料成功!!";
        Response.Redirect("Organization.aspx?rand=" + DateTime.Now.Ticks.ToString() + "");
    }
    //-------------------------------------------------------------------------
    protected void btnSearch_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("Organization.aspx?rand=" + DateTime.Now.Ticks.ToString() + "");
    }
    //-------------------------------------------------------------------------
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("Organization.aspx?rand=" + DateTime.Now.Ticks.ToString() + "");
    }
    //------------------------------------------------------------------------------
}
