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
        //�v���B�z
        AuthrityControl();

        txtOrgID.ReadOnly = true;

        //���o�Ҧ�, �s�W("ADD")�έק�("")
        HFD_Mode.Value = Util.GetQueryString("Mode");

        //�]�w�@�ǫ��䪺 Visible �ݩ�
        //�`�N:�n�b AuthrityControl() �Ψ��o HFD_Mode.Value �I�s
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
        //�p�G�O�s�W,�n���@�Ǫ�l�e���ʧ@
        if (HFD_Mode.Value == "ADD")
        {
            //�s�W��, �ק�ΧR����S�ʧ@
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            //btnPrint.Visible = false;
            txtOrgID.ReadOnly = false; 
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
            strSql = "select * from Organization where OrgID=@OrgID";
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("OrgID", HFD_OrgUID.Value);
            dt = NpoDB.GetDataTableS(strSql, dict);
            //��Ʋ��`
            if (dt.Rows.Count == 0)
            {
                SetSysMsg("�d�L���!");
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
        Session["Msg"] = "�s�W��Ʀ��\";
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
        Session["Msg"] = "�ק��Ʀ��\";
        Response.Redirect(Util.RedirectByTime("Organization.aspx"));
    }
    //-------------------------------------------------------------------------
    private void SetColumnData(List<ColumnData> list)
    {
        if (HFD_Mode.Value == "ADD") //�s�W�ɭn�H TextBox ����Ƭ��ӷ�
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
        //�������ϥΦ����c�N�X�A�ɦ^���c�޲z����
        if (dt.Rows.Count > 0)
        {
            Session["Msg"] = "" + dt.Rows[0]["DeptDesc"].ToString() + "�ϥΦ����c�N�X�A�L�k�R��!!";
            Response.Redirect("Organization.aspx?rand=" + DateTime.Now.Ticks.ToString() + "");
            return;
        }

        strSql = " delete Organization Where OrgID=@OrgID";
        NpoDB.ExecuteSQLS(strSql, dict);

        Session["Msg"] = "�R����Ʀ��\!!";
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
