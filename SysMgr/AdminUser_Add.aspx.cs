using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class SysMgr_AdminUser_Add : BasePage 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropDownListData();
        }
    }
    //----------------------------------------------------------------------
    public void LoadDropDownListData()
    {
        //部門名稱
        string strSql = "select DeptID, DeptName from dept order by DeptID";
        Util.FillDropDownList(ddlDept, strSql, "DeptName", "DeptID", true);

        //使用者群組
        strSql = "select uid, GroupName from AdminGroup order by uid";
        Util.FillDropDownList(ddlGroup, strSql, "GroupName", "uid", true);
        //月份

        Util.FillDropDownList(ddlMonth, 1, 12, false, 0, false, 2, '0');
        ddlMonth.Items.Add("");
        ddlMonth.SelectedValue = "";
        //日期
        Util.FillDropDownList(ddlDay, 1, 31, false, 0, false, 2, '0');
        ddlDay.Items.Add("");
        ddlDay.SelectedValue = "";
    }
    //----------------------------------------------------------------------
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (txtPassword.Text == txtRePassword.Text && txtPassword.Text != "")
        {
            if (txtPwdValidDate.Text == "" || Convert.ToDateTime(txtPwdValidDate.Text) >= System.DateTime.Now)
            {
                string strSql = " insert into  AdminUser ";
                strSql += " (UserID, OrgID, DeptID, GroupID, Password, StartData, birthday, PwdValidDate, UserName, CreateDate, Email, MobilNo, Memo, IsUse) values";
                strSql += "(@UserID,@OrgID,@DeptID,@GroupID,@Password,@StartData,@birthday,@PwdValidDate,@UserName, GetDate(), @Email,@MobilNo,@Memo,@IsUse); ";
                strSql += "\n";
                strSql += "select @@IDENTITY";

                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("UserID", txtUserID.Text);
                dict.Add("OrgID", SessionInfo.OrgID);
                dict.Add("DeptID", ddlDept.SelectedValue);
                dict.Add("GroupID", ddlGroup.SelectedValue);
                dict.Add("Password", txtPassword.Text);
                dict.Add("StartData", Util.DateTime2String(txtStartData.Text, DateType.yyyyMMdd, EmptyType.ReturnNull));
                dict.Add("birthday", ddlMonth.SelectedValue + "/" + ddlDay.SelectedValue);
                dict.Add("PwdValidDate", Util.DateTime2String(txtPwdValidDate.Text, DateType.yyyyMMdd, EmptyType.ReturnNull));
                dict.Add("UserName", txtUserName.Text);
                dict.Add("Email", txtEmail.Text);
                dict.Add("MobilNo", txtMobilNo.Text);
                dict.Add("Memo", txtMemo.Text);
                dict.Add("IsUse", rdoIsUse.SelectedIndex == 0 ? 1 : 0);

                HFD_Uid.Value = NpoDB.GetScalarS(strSql, dict);
                Session["Msg"] = "新增資料成功!!!";
                Response.Redirect(Util.RedirectByTime("AdminUser.aspx"));
            }
            else
            {
                ShowSysMsg("密碼最後有效日期不得小於今天日期!");
            }
        }
        else
        {
            ShowSysMsg("密碼與再確認密碼不符合!");
        }
    }
    //----------------------------------------------------------------------
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("AdminUser.aspx?rand=" + DateTime.Now.Ticks.ToString());
    }
    //----------------------------------------------------------------------
}
