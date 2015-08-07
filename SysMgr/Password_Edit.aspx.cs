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
        //權限控制
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
        //    ShowSysMsg("新密碼規則不合，請使用英文字及數字夾雜設定!!!");
        //    return;
        //}
        //判斷密碼與確認密碼是否相同
        if (conformPassword != txtPassword.Text)
        {
            ShowSysMsg("新密碼與確認密碼不一致");
            return;
        }
        //判斷舊密碼是否輸入正確
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
            ShowSysMsg("舊密碼輸入錯誤");
            return;
        }

        //更改密碼
        strSql = @"
                   update AdminUser set Password=@NewPassword,
                   OldPassword=@OldPassword
                   where UserID=@UserID and OrgID=@OrgID
                 ";

        dict.Add("NewPassword", txtPassword.Text);
        dict.Add("OldPassword", txtOldPassword.Text);

        NpoDB.ExecuteSQLS(strSql, dict);
        ShowSysMsg("密碼變更成功, 請重新登入才會生效!!!");
    }
    //----------------------------------------------------------------------------
}
