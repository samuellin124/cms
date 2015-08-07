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
using System.Text ;

public partial class FileMgr_News_Edit:BasePage 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["ProID"] = "News";
        //�v������
        AuthrityControl();

        //���o�Ҧ�, �s�W("ADD")�έק�("")
        HFD_Mode.Value = Util.GetQueryString("Mode");

        //�]�w�@�ǫ��䪺 Visible �ݩ�
        //�`�N:�n�b AuthrityControl() �Ψ��o HFD_Mode.Value �I�s
        SetButton();

        ShowSysMsg();
        if (!IsPostBack)
        {
            HFD_NewsUID.Value = Util.GetQueryString("NewsUID");
            if (HFD_Mode.Value == "ADD")
            {
                //�]�w�@�Ǫ�l��
            }
            LoadFormData();
            Data_List_DataBind();
        }
    }
    //--------------------------------------------------------------------------
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
            Dictionary<string, object> dict = new Dictionary<string, object>();
            strSql = "select * from news where uid=@uid";
            dict.Add("uid", HFD_NewsUID.Value);
            dt = NpoDB.GetDataTableS(strSql, dict);
            if (dt.Rows.Count <= 0)
            {
                SetSysMsg("��Ʋ��`");
                Response.Redirect("News.aspx");
                return;
            }
            dr = dt.Rows[0];
        }
        //�o�G���
        lblCheckBoxList.Text = LoadCheckBoxListData((HFD_Mode.Value == "ADD") ? "" : dr["DeptName"].ToString());
        //�T�����O
        Util.GetDdlIndex(ddlNewsType, (HFD_Mode.Value == "ADD") ? "" : dr["NewsType"].ToString());
        //�T�����D
        txtNewsSubject.Text = (HFD_Mode.Value == "ADD") ? "" : dr["NewsSubject"].ToString();
        //�T���K�n
        txtNewsBrief.Text = (HFD_Mode.Value == "ADD") ? "" : dr["NewsBrief"].ToString();
        //�o�G��
        txtNewsAuthor.Text = (HFD_Mode.Value == "ADD") ? "" : dr["NewsAuthor"].ToString();
        //�o�G���
        if (HFD_Mode.Value == "ADD")
        {
            txtNewsRegDate.Text = Util.DateTime2String(DateTime.Now, DateType.yyyyMMdd, EmptyType.ReturnEmpty);
        }
        else
        {
            txtNewsRegDate.Text = Util.DateTime2String(dr["NewsRegDate"], DateType.yyyyMMdd, EmptyType.ReturnEmpty);
        }
        //�o�G����
        txtNewsBeginDate.Text = (HFD_Mode.Value == "ADD") ? "" : Util.DateTime2String(dr["NewsBeginDate"], DateType.yyyyMMdd, EmptyType.ReturnEmpty);
        txtNewsEndDate.Text = (HFD_Mode.Value == "ADD") ? "" : Util.DateTime2String(dr["NewsEndDate"], DateType.yyyyMMdd, EmptyType.ReturnEmpty);
        //�T�����e(html)
        txtNewsContent.Text = (HFD_Mode.Value == "ADD") ? "" : dr["NewsContent"].ToString();
    }
    //--------------------------------------------------------------------------
    public string LoadCheckBoxListData(string DeptValues)
    {
        string strSql = "select DeptName, DeptID from dept order by Seq";
        DataTable dt = NpoDB.GetDataTableS(strSql, null);

        List<ControlData> list = new List<ControlData>();
        list.Clear();
        foreach (DataRow dr in dt.Rows)
        {
            string DeptName = dr["DeptName"].ToString();
            bool ShowBR = false;
            list.Add(new ControlData("Checkbox", "GN_Dept", "CB_Dept", DeptName, DeptName, ShowBR, DeptValues));
        }
        return HtmlUtil.RenderControl(list);
    }
    //--------------------------------------------------------------------------
    public void Data_List_DataBind()
    {
        ////***�ܼƫŧi***//
        //string strSql = "",ser_no; 
        //DataTable  dt ;
        //Dictionary<string, object> dict = new Dictionary<string, object>(); 
        ////***�d�߳]�w***//
        //ser_no = HFD_NewsUID.Value;
        //strSql = "select * from upload where Ap_Name='news' and Ser_no =@ser_no";
        //dict.Add("ser_no", ser_no);
        //dt = NpoDB.GetDataTableS(strSql, dict); 
        ////***�e���B�z***//
        //StringBuilder sb =new StringBuilder();
        //foreach(DataRow dr in dt.Rows)
        //{
        //    sb.AppendLine("<div style='text-align:left;'>");
        //    sb.AppendLine("<span style='text-align:right;width:14%;height:22px;' ></span>");
        //    sb.AppendLine("<span style='text-align:right;width:85%;height:22px;' >");

        //    if (dr["attach_type"].ToString() == "doc")
        //    {
        //        sb.AppendLine(@"<img border=""0"" src=""../images/save.GIF"" align=""absmiddle"">");
        //    }
        //    else
        //    {
        //        sb.AppendLine(@"<img border=""0"" src="".." + dr["upload_fileurl"] + @""" width=""120"">");
        //    }
        //    sb.AppendLine(@"<font color=""#000080"">");
        //    sb.AppendLine(@"<a href=""");
        //    sb.AppendLine(".."+dr["upload_fileURL"].ToString()+@""" target=""_blank"">");
        //    sb.AppendLine(dr["upload_filename"].ToString());
        //    sb.AppendLine("</a>");
        //    sb.AppendLine(@"</font>");
        //    sb.AppendLine(@"<img border=""0"" src=""../images/delete2.gif"" style=""cursor:hand"" align=""absmiddle"" OnClick='JavaScript:if(confirm(""�O�_�T�w�n�R�� ?"")){window.location.href=""content_file_delete.aspx?item=news&item_ser_no=" + this.HFD_NewsUID.Value + "&ser_no=" + dr["ser_no"] + @""";}'>");
        //    sb.AppendLine("</div>");
        //}
        //Data_List.Text = sb.ToString();
    }
    //--------------------------------------------------------------------------
    private bool CheckDate()
    {
        if (txtNewsBeginDate.Text.Trim() == "" || txtNewsEndDate.Text.Trim() == "")
        {
            return false;
        }
        return true;
    }
    //--------------------------------------------------------------------------
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (CheckDate() == false)
        {
            ShowSysMsg("�o�G����������J!");
            return;
        }
        Dictionary<string, object> dict = new Dictionary<string, object>();
        List<ColumnData> list = new List<ColumnData>();
        SetColumnData(list);
        string strSql = Util.CreateInsertCommand("News", list, dict);
        NpoDB.ExecuteSQLS(strSql, dict);
        Session["Msg"] = "�s�W��Ʀ��\";
        Response.Redirect(Util.RedirectByTime("News.aspx"));
    }
    //--------------------------------------------------------------------------
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (CheckDate() == false)
        {
            ShowSysMsg("�o�G����������J!");
            return;
        }
        Dictionary<string, object> dict = new Dictionary<string, object>();
        List<ColumnData> list = new List<ColumnData>();
        SetColumnData(list);
        string strSql = Util.CreateUpdateCommand("News", list, dict);
        NpoDB.ExecuteSQLS(strSql, dict);
        Session["Msg"] = "�ק��Ʀ��\";
        Response.Redirect(Util.RedirectByTime("News_Edit.aspx", "NewsUID=" + HFD_NewsUID.Value));
    }
    //--------------------------------------------------------------------------
    private void SetColumnData(List<ColumnData> list)
    {
        list.Add(new ColumnData("Uid", HFD_NewsUID.Value, false, false, true));

        //�o�G���
        list.Add(new ColumnData("DeptName", Util.GetControlValue("GN_Dept"), true, true, false));
        //�T�����O
        list.Add(new ColumnData("NewsType", ddlNewsType.SelectedValue, true, true, false));
        //�T�����D
        list.Add(new ColumnData("NewsSubject", txtNewsSubject.Text, true, true, false));
        //�T���K�n
        list.Add(new ColumnData("NewsBrief", txtNewsBrief.Text, true, true, false));
        //�o�G��
        list.Add(new ColumnData("NewsAuthor", txtNewsAuthor.Text, true, true, false));
        //�o�G���
        list.Add(new ColumnData("NewsRegDate", Util.DateTime2String(txtNewsRegDate.Text, DateType.yyyyMMdd, EmptyType.ReturnNull), true, true, false));

        //�o�G����
        list.Add(new ColumnData("NewsBeginDate", Util.DateTime2String(txtNewsBeginDate.Text, DateType.yyyyMMdd, EmptyType.ReturnNull), true, true, false));
        list.Add(new ColumnData("NewsEndDate", Util.DateTime2String(txtNewsEndDate.Text, DateType.yyyyMMdd, EmptyType.ReturnNull), true, true, false));
        //�T�����e(html)
        list.Add(new ColumnData("NewsContent", txtNewsContent.Text, true, true, false));
    }
    //--------------------------------------------------------------------------
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        string strSql = "update News set IsDelete=1 where uid=@uid;\n";
        //�p�G�� upload file, �h�n�R��
        //strSql += "delete from upload where Ap_Name='news' and object_id=@ser_no\n";
        dict.Add("uid", HFD_NewsUID.Value);
        NpoDB.ExecuteSQLS(strSql, dict);

        Session["Msg"] = "��ƧR�����\ !";
        Response.Redirect("News.aspx");
    }
    //--------------------------------------------------------------------------
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect(Util.RedirectByTime("News.aspx"));
    }
    //--------------------------------------------------------------------------
}
