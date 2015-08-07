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

public partial class SysMgr_SelectionData_Edit : BasePage
{
    //程式載入時候的處理
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["ProID"] = "SelectionData";
        //權限控制
        AuthrityControl();

        //取得模式, 新增("ADD")或修改("")
        HFD_Mode.Value = Util.GetQueryString("Mode");

        //設定一些按鍵的 Visible 屬性
        //注意:要在 AuthrityControl() 及取得 HFD_Mode.Value 呼叫
        SetButton();

        if (!IsPostBack)
        {
            HFD_DataUID.Value = Util.GetQueryString("DataUID");
            //LoadDropDownListData();
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
    //------------------------------------------------------------------------------
    private void SetButton()
    {
        //如果是新增,要做一些初始畫的動作
        if (HFD_Mode.Value == "ADD")
        {
            //新增時, 修改及刪除鍵沒動作
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            txtCaseID.ReadOnly = false;
            txtCaseGroup.ReadOnly = false;
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
            strSql = " select CaseGroup, CaseID, CaseName from CaseCode ";
            strSql += " where uid=@uid ";
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("uid", HFD_DataUID.Value);
            dt = NpoDB.GetDataTableS(strSql, dict);
            if (dt.Rows.Count == 0)
            {
                //資料異常
                SetSysMsg("資料異常!");
                Response.Redirect("SelectionData.aspx");
                return;
            }
            dr = dt.Rows[0];
        }
        txtCaseID.Text = (HFD_Mode.Value == "ADD") ? "" : dr["CaseID"].ToString();
        txtCaseName.Text = (HFD_Mode.Value == "ADD") ? "" : dr["CaseName"].ToString();
        txtCaseGroup.Text = (HFD_Mode.Value == "ADD") ? "" : dr["CaseGroup"].ToString();
    }
    //------------------------------------------------------------------------------
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        List<ColumnData> list = new List<ColumnData>();
        SetColumnData(list);
        string strSql = Util.CreateInsertCommand("CaseCode", list, dict);
        NpoDB.ExecuteSQLS(strSql, dict);
        Session["Msg"] = "新增資料成功";
        Response.Redirect(Util.RedirectByTime("SelectionData.aspx"));
    }
    //------------------------------------------------------------------------------
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        List<ColumnData> list = new List<ColumnData>();
        SetColumnData(list);
        string strSql = Util.CreateUpdateCommand("CaseCode", list, dict);
        NpoDB.ExecuteSQLS(strSql, dict);
        Session["Msg"] = "修改資料成功";
        Response.Redirect(Util.RedirectByTime("SelectionData.aspx"));
    }
    //------------------------------------------------------------------------------
    private void SetColumnData(List<ColumnData> list)
    {
        list.Add(new ColumnData("Uid", HFD_DataUID.Value, false, false, true));

        list.Add(new ColumnData("CaseID", txtCaseID.Text, true, true, false));
        list.Add(new ColumnData("CaseName", txtCaseName.Text, true, true, false));
        list.Add(new ColumnData("CaseGroup", txtCaseGroup.Text, true, true, false));
        list.Add(new ColumnData("IsUse", "True", true, true, false));
    }
    //-------------------------------------------------------------------------------------------------------------
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        string strSql = "";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        strSql = "delete from CaseCode where uid=@uid";
        dict.Add("uid", HFD_DataUID.Value);
        NpoDB.ExecuteSQLS(strSql, dict);
        Session["Msg"] = "刪除資料成功";
        Response.Redirect("SelectionData.aspx");
    }
    //------------------------------------------------------------------------------
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("SelectionData.aspx");
    }
    //------------------------------------------------------------------------------
}
