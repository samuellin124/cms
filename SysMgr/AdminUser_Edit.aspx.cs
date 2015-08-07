using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class SysMgr_AdminUser_Edit : BasePage 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["ProgID"] = "AdminUser";
        //權控處理
        AuthrityControl();

        ShowSysMsg();

        if (!IsPostBack)
        {
            HFD_Uid.Value = Util.GetQueryString("Uid");
            LoadDropDownListData();
            LoadFormData();
            //ShowPic();
        }
    }
    //----------------------------------------------------------------------
    public void AuthrityControl()
    {
        if (Authrity.PageRight("_Focus") == false)
        {
            return;
        }

        //Authrity.CheckButtonRight("_Delete", btnDelete);
        Authrity.CheckButtonRight("_Update", btnUpdate);
    }
    //----------------------------------------------------------------------
    private void ShowPic()
    {
        //Dictionary<string, object> dict = new Dictionary<string, object>();
        ////帶出相關圖檔資料
        //string strSql = " select Ap_Name, Upload_FileURL from Upload\n";
        //strSql += " where PersonProfile_Uid=@uid and Ap_Name=@Ap_Name\n";
        //dict.Add("uid", HFD_Uid.Value);
        //dict.Add("Ap_Name", "AdminUser");
        //DataTable dt = CmsADODB.QueryGetTable(this, strSql, dict);
        //string Pict_URL = "";
        //if (dt.Rows.Count != 0)
        //{
        //    Pict_URL = dt.Rows[0]["Upload_FileURL"].ToString(); ;
        //}
        ////如果沒有URL時(代表無上傳圖檔)，關閉圖檔刪除按鈕
        //if (Pict_URL == "")
        //{
        //    btnDeletePic.Visible = false;
        //}
        //else
        //{
        //    btnUploadPic.Visible = false;
        //}
        ////以此URL顯示圖片
        //LB_Pic.Text = showPic(Pict_URL);
    }
    //----------------------------------------------------------------------
    protected string showPic(string PictFileURL)
    {
        string RetStr = "";
        if (PictFileURL == "")
        {
            RetStr = "";
        }
        else
        {
            RetStr = "<img src=\".." + PictFileURL + "\" border=\"0\" style=\"width:75pt;height:75pt;cursor:hand \" onclick=\"var x=window.open('','','height=480, width=640, toolbar=no, menubar=no, scrollbars=1, resizable=yes, location=no, status=no');x.document.write('<img src=.." + PictFileURL + " border=0>');\" alt=\"點選看放大圖\">";
        }
        return RetStr;
    }
    //----------------------------------------------------------------------
    protected void btnDeletePic_Click(object sender, EventArgs e)
    {
        //string strSql = " select Ser_no, Upload_FileURL from Upload where PersonProfile_Uid = '" + HFD_Uid.Value + "' and Ap_Name = 'AdminUser' ";

        //DataTable dt = CmsADODB.QueryGetTable(this, strSql);

        //if (dt.Rows.Count != 0)
        //{
        //    string DeletePictFile = " delete from Upload where Ser_no = " + dt.Rows[0][0].ToString() + " ";
        //    CmsADODB.ExecuteSQL(this, DeletePictFile);
        //    string FilePath = Server.MapPath(".." + dt.Rows[0][1].ToString());
        //    File.Delete(FilePath);
        //}

        //Session["Msg"] = "圖檔刪除成功!";
        //Response.Redirect("AdminUser_Edit.aspx?rand=" + DateTime.Now.Ticks.ToString() + "&uid=" + HFD_Uid.Value + "");
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
        Util.FillDropDownList(ddlMonth,1, 12, false, 0, false, 2, '0');
        ddlMonth.Items.Add("");
        ddlMonth.SelectedValue = "";
        //日期
        Util.FillDropDownList(ddlDay, 1, 31, false, 0, false, 2, '0');
        ddlDay.Items.Add("");
        ddlDay.SelectedValue = "";
    }
    //----------------------------------------------------------------------
    public void LoadFormData()
    {
        string strSql = "select * from AdminUser where uid=@uid";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("uid", HFD_Uid.Value);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count == 0)
        {
            Response.Redirect("AdminUser.aspx?rand=" + DateTime.Now.Ticks.ToString());
        }

        DataRow dr = dt.Rows[0];

        //部門名稱
        ddlDept.SelectedValue = dr["DeptID"].ToString();
        //使用者群組
        ddlGroup.SelectedValue = dr["GroupID"].ToString();
        //帳號
        txtUserID.Text = dr["UserID"].ToString();

        //使用者名稱
        txtUserName.Text = dr["UserName"].ToString();
        //密碼
        txtPassword.Text = dr["Password"].ToString();
        //再確認密碼
        txtRePassword.Text = dr["Password"].ToString();
        //到職日
        txtStartData.Text = Util.DateTime2String(dr["StartData"], DateType.yyyyMMdd, EmptyType.ReturnEmpty);
        //生日
        //txtBirthDay.Text = Util.DateTime2String(dr["BirthDay1"], DateType.yyyyMMdd, EmptyType.ReturnEmpty);
        if (dr["birthday"].ToString() != "")
        {
        string tmp = dr["birthday"].ToString();
        string[] split = tmp.Split(new Char[] { '/' });

        split[0].ToString();
        split[1].ToString();
        ddlMonth.SelectedValue = split[0].ToString();
        ddlDay.SelectedValue = split[1].ToString();
        }
        //密碼最後有效日期
        //txtPwdValidDate.Text = DateTimeString(dr["PwdValidDate"]);
        txtPwdValidDate.Text = Util.DateTime2String(dr["PwdValidDate"], DateType.yyyyMMdd, EmptyType.ReturnEmpty);
        //電子郵件地址
        txtEmail.Text = dr["Email"].ToString();
        //手機號碼
        txtMobilNo.Text = dr["MobilNo"].ToString();
        //備註
        txtMemo.Text = dr["Memo"].ToString();
 
        //是否使用
        if (dr["IsUse"].ToString() == "True")
        {
            rdoIsUse.SelectedIndex = 0;
        }
        else
        {
            rdoIsUse.SelectedIndex = 1;
        }
    }
    //----------------------------------------------------------------------
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (txtPassword.Text == txtRePassword.Text)
        {
            if (txtPwdValidDate.Text == "" || Convert.ToDateTime(txtPwdValidDate.Text) >= System.DateTime.Now)
            {
                string strSql = " update AdminUser set ";
                strSql += "  UserID = @UserID";
                strSql += ", OrgID = @OrgID";
                strSql += ", DeptID = @DeptID";
                strSql += ", GroupID = @GroupID";
                if (txtPassword.Text != "")
                {
                    strSql += ", Password = @Password";
                }
                strSql += ", PwdValidDate = @PwdValidDate";
                strSql += ", UserName = @UserName";
                strSql += ", Email = @Email";
                strSql += ", MobilNo = @MobilNo";
                strSql += ", Memo = @Memo";
                strSql += ", IsUse = @IsUse";
                strSql += ", StartData = @StartData";
                strSql += ", birthday = @birthday";

                strSql += " where uid = @uid";

                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("uid", HFD_Uid.Value);
                dict.Add("UserID", txtUserID.Text);
                dict.Add("OrgID", SessionInfo.OrgID);
                dict.Add("DeptID", ddlDept.SelectedValue);
                dict.Add("GroupID", ddlGroup.SelectedValue);
                if (txtPassword.Text != "")
                {
                    dict.Add("Password", txtPassword.Text);
                }
                dict.Add("PwdValidDate", Util.DateTime2String(txtPwdValidDate.Text, DateType.yyyyMMdd, EmptyType.ReturnNull));
                dict.Add("UserName", txtUserName.Text);
                dict.Add("Email", txtEmail.Text);
                dict.Add("MobilNo", txtMobilNo.Text);
                dict.Add("Memo", txtMemo.Text);
                dict.Add("IsUse", rdoIsUse.SelectedIndex == 0 ? 1 : 0);
                dict.Add("StartData", Util.DateTime2String(txtStartData.Text, DateType.yyyyMMdd, EmptyType.ReturnNull));
                dict.Add("birthday", ddlMonth.SelectedValue + "/" + ddlDay.SelectedValue );

                NpoDB.ExecuteSQLS(strSql, dict);
                Session["Msg"] = "修改資料成功!!!";
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
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        string strSql = "delete from  AdminUser\n";
        strSql += "where uid =@uid\n";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("uid", HFD_Uid.Value);
        NpoDB.ExecuteSQLS(strSql, dict);
        Session["Msg"] = "刪除資料成功!!!";
        Response.Redirect(Util.RedirectByTime("AdminUser.aspx"));
    }
    //----------------------------------------------------------------------
}
