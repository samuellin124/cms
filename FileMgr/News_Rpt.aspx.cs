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
using System.Text;

public partial class FileMgr_News_Rpt :BasePage 
{
    private string ImgPosition = "";
    private string NewsContent = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["ser_no"] != null && Request.QueryString["ser_no"].ToString() != "")
            {
                HFD_SER_NO.Value = Request.QueryString["ser_no"].ToString();
                //訊息基本資訊
                NewData_DataBind();
                //檔案下載
                FileDownLoad_DataBind();
            }
        }
    }

    public void NewData_DataBind()
    {
        //****變數宣告****//
        string strSql, ser_no, news_ImgPosition;
        DataTable dt;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        //****變數設定****//
        ser_no = this.HFD_SER_NO.Value;
        //****設定查詢****//
        strSql = "select news.*,dept_desc from news left join dept on news.dept_id=dept.dept_id where ser_no=@ser_no ";
        dict.Add("ser_no", ser_no);

        //****執行語法****//
        NpoDB.ExecuteSQLS(strSql, dict);
        dt = NpoDB.GetDataTableS(strSql, dict);

        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            this.ImgPosition = dr["news_ImgPosition"].ToString();
            this.NewsContent = dr["news_content"].ToString();
            this.FD_dept_desc.Text = dr["dept_desc"].ToString(); ;
            this.FD_news_RegDate.Text = dr["news_RegDate"].ToString(); ;
            this.FD_news_subject.Text = dr["news_subject"].ToString(); ;
        }
    }
    public void FileDownLoad_DataBind()
    {
        //****變數宣告****//
        string strSql, ser_no;
        DataTable dt;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        //****變數設定****//
        ser_no = this.HFD_SER_NO.Value;
        //****設定查詢****//
        strSql = "select * from upload where Ap_Name='news' and attach_type<>'doc' and Ser_no=@ser_no ";
        dict.Add("ser_no", ser_no);

        //****執行語法****//
        NpoDB.ExecuteSQLS(strSql, dict);
        dt = NpoDB.GetDataTableS(strSql, dict);

        //****畫面處理****//
        StringBuilder sb = new StringBuilder();

        if (this.ImgPosition == "bottom")
            sb.AppendLine(this.NewsContent);

        sb.AppendLine(@"<table cellspacing=""0"" cellpadding=""2"" width=""98"" align=""" + this.ImgPosition + @""" border=""0"">");
        sb.AppendLine("<tbody>");
        foreach (DataRow dr in dt.Rows)
        {
            sb.AppendLine("<tr>");
            sb.AppendLine(@"<td align=""middle"">");
            sb.AppendLine(@"<img src=""../upload/" + dr["Upload_fileURL"] + @""" border=""0""></td>");
            sb.AppendLine("</tr");
            sb.AppendLine("<!-- 圖說 -->");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td>");
            sb.AppendLine(@"<font color=""#4B640B"">");
            sb.AppendLine(dr["upload_filename"].ToString());
            sb.AppendLine("</font>");
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
        }
        sb.AppendLine("</tbody>");
        sb.AppendLine("</table>");

        if (this.ImgPosition != "bottom")
            sb.AppendLine(this.NewsContent);

        lbl_FileDownLoad.Text = sb.ToString();
    }
}
