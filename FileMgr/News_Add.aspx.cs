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
using System.Data;

public partial class FileMgr_News_Add :BasePage 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckBox_List_DataBind();
            //if(!DBFunction.IsEmpty(Session["dept_desc"] ))
            //    FD_news_author.Text = Session["dept_desc"].ToString() + " / " + Session["user_name"].ToString();
            FD_news_author.Text = Session["dept_name"].ToString() + " / " + Session["user_name"].ToString(); ;
            //FD_news_RegDate.Text = DBFunction.GetDateShortString(DateTime.Now); 
            FD_news_RegDate.Text = Util.DateTime2String(DateTime.Now, DateType.yyyyMMdd, EmptyType.ReturnEmpty); 
        }
    }
    //--------------------------------------------------------------------------
    public void CheckBox_List_DataBind()
    {
        //****變數宣告****//
        string strSql;
        DataTable dt;

        //****查詢設定****//
        strSql = "select dept_desc,dept_id from dept";
        dt = NpoDB.GetDataTableS(strSql, null);
        //****製作下拉選單****//
        //CheckBox_List.Text = DBFunction.CheckBoxList(dt, "dept_id", "dept_desc", "", ""); 
    }
    //--------------------------------------------------------------------------
    protected void btnSave_Click(object sender, ImageClickEventArgs e)
    {
        //****變數宣告****//
        string strSql;
        string  new_subject,news_author,news_BeginDate ,news_brief,news_content,news_EndDate ,news_RegDate ;
        string dept_id_values;
        string news_type,news_showhome,news_showsubpage,news_ImgPosition,news_ImgShow,news_ad;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        DataTable dt;
        //****取得輸入值****//
        new_subject = FD_new_subject.Text;
        news_author = FD_news_author.Text;
        news_BeginDate = FD_news_BeginDate.Text;
        news_brief = FD_news_brief.Text;
        news_content = FD_news_content.Text;
        news_EndDate = FD_news_EndDate.Text;
        news_RegDate = FD_news_RegDate.Text;
        //dept_id_values = DBFunction.getRequestFrom(this,"dept_id");
        dept_id_values = Util.GetQueryString("dept_id");
        news_type="最新訊息";
        news_showhome="Y";
        news_showsubpage="Y";
        news_ImgPosition="left";
        news_ImgShow="Y";
        news_ad="N";

        strSql  =" insert into  news (  dept_id, news_Subject, news_brief, news_Content, news_type, ";
        strSql +=" news_RegDate, news_ShowHome, news_ShowSubPage, ";
        strSql +=" news_BeginDate, news_EndDate, ";
        strSql +=" news_ImgPosition, news_Author, news_ImgShow, news_ad)";
        strSql +=" values(  @dept_id, @news_subject, @news_brief, @news_Content, @news_type, ";
        strSql +="  @news_RegDate, @news_ShowHome, @news_ShowSubPage, ";
        strSql +=" @news_BeginDate, @news_EndDate," ;
        strSql +=" @news_ImgPosition, @news_Author, @news_ImgShow, @news_ad";
        strSql += ")";
        
        dict.Add("news_subject", new_subject);
        dict.Add("news_author", news_author);
        dict.Add("news_BeginDate", news_BeginDate);
        dict.Add("news_brief", news_brief);
        dict.Add("news_content", news_content);
        dict.Add("news_EndDate", news_EndDate);
        dict.Add("news_RegDate", news_RegDate);
        dict.Add("dept_id", dept_id_values);
        dict.Add("news_type", news_type);
        dict.Add("news_showhome", news_showhome);
        dict.Add("news_showsubpage", news_showsubpage);
        dict.Add("news_ImgPosition", news_ImgPosition);
        dict.Add("news_ImgShow", news_ImgShow);
        dict.Add("news_ad", news_ad);

        NpoDB.ExecuteSQLS(strSql, dict);

        Session["Msg"] = "新增資料成功！";

        string max_ser_no = "";
        strSql ="select max(ser_no) as ser_no from news";
        max_ser_no = NpoDB.GetScalarS(strSql, null);

        Response.Redirect("News_Edit.aspx?ser_no=" + max_ser_no);
    }
    //--------------------------------------------------------------------------
    protected void btnCancel_Click(object sender, ImageClickEventArgs e)
    {

    }
}
