using System;
using System.Data;
using System.Collections.Generic;
using System.Web.UI;

public partial class FileMgr_Password_Edit : BasePage
{
    GroupAuthrity ga = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        Session["ProgID"] = "PasswordEdit";
        //�v������
        AuthrityControl();

        txtUserID.Text = SessionInfo.UserID;
        txtUserName.Text = SessionInfo.UserName;
        txtOrgID.Value = SessionInfo.OrgID;
        LoadFormData();
    }
    //----------------------------------------------------------------------------
    private void AuthrityControl()
    {
        ga = Authrity.GetGroupRight();
        if (Authrity.CheckPageRight(ga.Focus) == false)
        {
            return;
        }
        btnUpdate.Visible = ga.Update;
    }
    //-----------------------------------------------------------------------------
    public void LoadFormData()
    {
    }
    //----------------------------------------------------------------------------
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        string UserID = txtUserID.Text;
        string OrgID = txtOrgID.Value;
        string newPassword = txtPassword.Text;
        string conformPassword = txtCPassword.Text;
        string strSql = "";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        //if (GSSNPO.CheckText(newPassword) == false)
        //{
        //    ShowSysMsg("�s�K�X�W�h���X�A�Шϥέ^��r�μƦr�����]�w!!!");
        //    return;
        //}
        //�P�_�K�X�P�T�{�K�X�O�_�ۦP
        if (conformPassword != txtPassword.Text)
        {
            ShowSysMsg("�s�K�X�P�T�{�K�X���@�P");
            return;
        }
        //�P�_�±K�X�O�_��J���T
        strSql = @"
                   select UserID from AdminUser where
                   UserID=@UserID
                   and Password = @Password
                   and OrgID = @OrgID
                  ";

        dict.Add("UserID", UserID);
        dict.Add("Password", txtOldPassword.Text);
        dict.Add("OrgID", OrgID);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);

        if (dt.Rows.Count != 1)
        {
            ShowSysMsg("�±K�X��J���~");
            return;
        }

        //���K�X
        strSql = @"
                   update AdminUser set Password=@NewPassword,
                   OldPassword=@OldPassword
                   where UserID=@UserID and OrgID=@OrgID
                 ";

        dict.Add("NewPassword", txtPassword.Text);
        dict.Add("OldPassword", txtOldPassword.Text);

        NpoDB.ExecuteSQLS(strSql, dict);
        ShowSysMsg("�K�X�ܧ󦨥\, �Э��s�n�J�~�|�ͮ�!!!");
    }
    //----------------------------------------------------------------------------
}
