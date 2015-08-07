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
    //�{�����J�ɭԪ��B�z
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["ProID"] = "SelectionData";
        //�v������
        AuthrityControl();

        //���o�Ҧ�, �s�W("ADD")�έק�("")
        HFD_Mode.Value = Util.GetQueryString("Mode");

        //�]�w�@�ǫ��䪺 Visible �ݩ�
        //�`�N:�n�b AuthrityControl() �Ψ��o HFD_Mode.Value �I�s
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
        //�p�G�O�s�W,�n���@�Ǫ�l�e���ʧ@
        if (HFD_Mode.Value == "ADD")
        {
            //�s�W��, �ק�ΧR����S�ʧ@
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            txtCaseID.ReadOnly = false;
            txtCaseGroup.ReadOnly = false;
        }
        else
        {
            //�ק��, �s�W��S�ʧ@
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
                //��Ʋ��`
                SetSysMsg("��Ʋ��`!");
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
        Session["Msg"] = "�s�W��Ʀ��\";
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
        Session["Msg"] = "�ק��Ʀ��\";
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
        Session["Msg"] = "�R����Ʀ��\";
        Response.Redirect("SelectionData.aspx");
    }
    //------------------------------------------------------------------------------
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("SelectionData.aspx");
    }
    //------------------------------------------------------------------------------
}
