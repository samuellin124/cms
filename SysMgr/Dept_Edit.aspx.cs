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
    //�{�����J�ɭԪ��B�z
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["ProID"] = "Dept";
        //�v������
        AuthrityControl();

        //���o�Ҧ�, �s�W("ADD")�έק�("")
        HFD_Mode.Value = Util.GetQueryString("Mode");

        //�]�w�@�ǫ��䪺 Visible �ݩ�
        //�`�N:�n�b AuthrityControl() �Ψ��o HFD_Mode.Value �I�s
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
        //�p�G�O�s�W,�n���@�Ǫ�l�e���ʧ@
        if (HFD_Mode.Value == "ADD")
        {
            //�s�W��, �ק�ΧR����S�ʧ@
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            txtDeptID.ReadOnly = false;
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
            strSql = "select OrgID, DeptID, DeptName from Dept\n";
            strSql += "where uid=@uid\n";
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("uid", HFD_DeptUID.Value);
            dt = NpoDB.GetDataTableS(strSql, dict);
            if (dt.Rows.Count == 0)
            {
                //��Ʋ��`
                SetSysMsg("��Ʋ��`!");
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
            ShowSysMsg("�����N���γ����W�٤��i�ťաI�Э��s��J�C");
            return;
        }
        if (CheckDuplDeptID())
        {
            ShowSysMsg("�����N�����ơI�Э��s��J�C");
            return;
        }
        Dictionary<string, object> dict = new Dictionary<string, object>();
        List<ColumnData> list = new List<ColumnData>();
        SetColumnData(list);
        string strSql = Util.CreateInsertCommand("Dept", list, dict);
        NpoDB.ExecuteSQLS(strSql, dict);
        Session["Msg"] = "�s�W��Ʀ��\";
        Response.Redirect(Util.RedirectByTime("Dept.aspx"));
    }
    //------------------------------------------------------------------------------
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (txtDeptID.Text.Trim() == "" || txtDeptName.Text.Trim() == "")
        {
            ShowSysMsg("�����N���γ����W�٤��i�ťաI�Э��s��J�C");
            return;
        }
        Dictionary<string, object> dict = new Dictionary<string, object>();
        List<ColumnData> list = new List<ColumnData>();
        SetColumnData(list);
        string strSql = Util.CreateUpdateCommand("Dept", list, dict);
        NpoDB.ExecuteSQLS(strSql, dict);
        Session["Msg"] = "�ק��Ʀ��\";
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
        //���b���ϥΦ������N�X�A�ɦ^���c�޲z����
        if (dt.Rows.Count > 0)
        {
            Session["Msg"] = "" + dt.Rows[0]["UserName"].ToString() + "�ϥΦ������N�X�A�L�k�R��";
            Response.Redirect("Dept.aspx");
            return;
        }

        dict.Clear();
        dict.Add("uid", HFD_DeptUID.Value);
        strSql = "delete from Dept where uid=@uid";
        NpoDB.ExecuteSQLS(strSql, dict);
        Session["Msg"] = "�R����Ʀ��\";
        Response.Redirect("Dept.aspx");
    }
    //------------------------------------------------------------------------------
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("Dept.aspx");
    }
    //------------------------------------------------------------------------------
}
