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
        //權限控制
        AuthrityControl();

        //取得模式, 新增("ADD")或修改("")
        HFD_Mode.Value = Util.GetQueryString("Mode");

        //設定一些按鍵的 Visible 屬性
        //注意:要在 AuthrityControl() 及取得 HFD_Mode.Value 呼叫
        SetButton();

        ShowSysMsg();
        if (!IsPostBack)
        {
            HFD_NewsUID.Value = Util.GetQueryString("NewsUID");
            if (HFD_Mode.Value == "ADD")
            {
                //設定一些初始值
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
        //如果是新增,要做一些初始畫的動作
        if (HFD_Mode.Value == "ADD")
        {
            //新增時, 修改及刪除鍵沒動作
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            //btnPrint.Visible = false;
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
            Dictionary<string, object> dict = new Dictionary<string, object>();
            strSql = "select * from news where uid=@uid";
            dict.Add("uid", HFD_NewsUID.Value);
            dt = NpoDB.GetDataTableS(strSql, dict);
            if (dt.Rows.Count <= 0)
            {
                SetSysMsg("資料異常");
                Response.Redirect("News.aspx");
                return;
            }
            dr = dt.Rows[0];
        }
        //發佈單位
        lblCheckBoxList.Text = LoadCheckBoxListData((HFD_Mode.Value == "ADD") ? "" : dr["DeptName"].ToString());
        //訊息類別
        Util.GetDdlIndex(ddlNewsType, (HFD_Mode.Value == "ADD") ? "" : dr["NewsType"].ToString());
        //訊息標題
        txtNewsSubject.Text = (HFD_Mode.Value == "ADD") ? "" : dr["NewsSubject"].ToString();
        //訊息摘要
        txtNewsBrief.Text = (HFD_Mode.Value == "ADD") ? "" : dr["NewsBrief"].ToString();
        //發佈者
        txtNewsAuthor.Text = (HFD_Mode.Value == "ADD") ? "" : dr["NewsAuthor"].ToString();
        //發佈日期
        if (HFD_Mode.Value == "ADD")
        {
            txtNewsRegDate.Text = Util.DateTime2String(DateTime.Now, DateType.yyyyMMdd, EmptyType.ReturnEmpty);
        }
        else
        {
            txtNewsRegDate.Text = Util.DateTime2String(dr["NewsRegDate"], DateType.yyyyMMdd, EmptyType.ReturnEmpty);
        }
        //發佈期間
        txtNewsBeginDate.Text = (HFD_Mode.Value == "ADD") ? "" : Util.DateTime2String(dr["NewsBeginDate"], DateType.yyyyMMdd, EmptyType.ReturnEmpty);
        txtNewsEndDate.Text = (HFD_Mode.Value == "ADD") ? "" : Util.DateTime2String(dr["NewsEndDate"], DateType.yyyyMMdd, EmptyType.ReturnEmpty);
        //訊息內容(html)
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
        ////***變數宣告***//
        //string strSql = "",ser_no; 
        //DataTable  dt ;
        //Dictionary<string, object> dict = new Dictionary<string, object>(); 
        ////***查詢設定***//
        //ser_no = HFD_NewsUID.Value;
        //strSql = "select * from upload where Ap_Name='news' and Ser_no =@ser_no";
        //dict.Add("ser_no", ser_no);
        //dt = NpoDB.GetDataTableS(strSql, dict); 
        ////***畫面處理***//
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
        //    sb.AppendLine(@"<img border=""0"" src=""../images/delete2.gif"" style=""cursor:hand"" align=""absmiddle"" OnClick='JavaScript:if(confirm(""是否確定要刪除 ?"")){window.location.href=""content_file_delete.aspx?item=news&item_ser_no=" + this.HFD_NewsUID.Value + "&ser_no=" + dr["ser_no"] + @""";}'>");
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
            ShowSysMsg("發佈期間必須輸入!");
            return;
        }
        Dictionary<string, object> dict = new Dictionary<string, object>();
        List<ColumnData> list = new List<ColumnData>();
        SetColumnData(list);
        string strSql = Util.CreateInsertCommand("News", list, dict);
        NpoDB.ExecuteSQLS(strSql, dict);
        Session["Msg"] = "新增資料成功";
        Response.Redirect(Util.RedirectByTime("News.aspx"));
    }
    //--------------------------------------------------------------------------
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (CheckDate() == false)
        {
            ShowSysMsg("發佈期間必須輸入!");
            return;
        }
        Dictionary<string, object> dict = new Dictionary<string, object>();
        List<ColumnData> list = new List<ColumnData>();
        SetColumnData(list);
        string strSql = Util.CreateUpdateCommand("News", list, dict);
        NpoDB.ExecuteSQLS(strSql, dict);
        Session["Msg"] = "修改資料成功";
        Response.Redirect(Util.RedirectByTime("News_Edit.aspx", "NewsUID=" + HFD_NewsUID.Value));
    }
    //--------------------------------------------------------------------------
    private void SetColumnData(List<ColumnData> list)
    {
        list.Add(new ColumnData("Uid", HFD_NewsUID.Value, false, false, true));

        //發佈單位
        list.Add(new ColumnData("DeptName", Util.GetControlValue("GN_Dept"), true, true, false));
        //訊息類別
        list.Add(new ColumnData("NewsType", ddlNewsType.SelectedValue, true, true, false));
        //訊息標題
        list.Add(new ColumnData("NewsSubject", txtNewsSubject.Text, true, true, false));
        //訊息摘要
        list.Add(new ColumnData("NewsBrief", txtNewsBrief.Text, true, true, false));
        //發佈者
        list.Add(new ColumnData("NewsAuthor", txtNewsAuthor.Text, true, true, false));
        //發佈日期
        list.Add(new ColumnData("NewsRegDate", Util.DateTime2String(txtNewsRegDate.Text, DateType.yyyyMMdd, EmptyType.ReturnNull), true, true, false));

        //發佈期間
        list.Add(new ColumnData("NewsBeginDate", Util.DateTime2String(txtNewsBeginDate.Text, DateType.yyyyMMdd, EmptyType.ReturnNull), true, true, false));
        list.Add(new ColumnData("NewsEndDate", Util.DateTime2String(txtNewsEndDate.Text, DateType.yyyyMMdd, EmptyType.ReturnNull), true, true, false));
        //訊息內容(html)
        list.Add(new ColumnData("NewsContent", txtNewsContent.Text, true, true, false));
    }
    //--------------------------------------------------------------------------
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        string strSql = "update News set IsDelete=1 where uid=@uid;\n";
        //如果有 upload file, 則要刪除
        //strSql += "delete from upload where Ap_Name='news' and object_id=@ser_no\n";
        dict.Add("uid", HFD_NewsUID.Value);
        NpoDB.ExecuteSQLS(strSql, dict);

        Session["Msg"] = "資料刪除成功 !";
        Response.Redirect("News.aspx");
    }
    //--------------------------------------------------------------------------
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect(Util.RedirectByTime("News.aspx"));
    }
    //--------------------------------------------------------------------------
}
