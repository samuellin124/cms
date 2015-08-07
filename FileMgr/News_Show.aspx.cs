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
using System.Text;
using System.Data;

public partial class FileMgr_News_Show :BasePage 
{
    private string ImgPosition = "";
    private string NewsContent = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HFD_NewsUID.Value = Util.GetQueryString("NewsUID");
            //訊息基本資訊
            LoadNewData();
            ////檔案下載
            //FileDownLoad_DataBind();
            ////文件下載
            //DocDownLoad_DataBind();
        }
    }
    //------------------------------------------------------------------------------
    public void LoadNewData()
    {
        string strSql = "select news.* from news\n";
        strSql += "where uid=@uid";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("uid", HFD_NewsUID.Value);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);

        if(dt.Rows.Count > 0 )
        {
            DataRow dr = dt.Rows[0] ; 
            //ImgPosition  = dr["news_ImgPosition"].ToString();
            NewsContent = dr["NewsContent"].ToString();
            lblDeptName.Text = dr["DeptName"].ToString();
            lblNewsRegDate.Text = Convert.ToDateTime(dr["NewsRegDate"].ToString()).ToString("yyyy/MM/dd");
            lblNewsSubject.Text = dr["NewsSubject"].ToString();
            lblNewsContent.Text = dr["NewsContent"].ToString();
            lblNewsType.Text = "【" + dr["NewsType"].ToString() + "】";
            lblPeriod.Text = Convert.ToDateTime(dr["NewsBeginDate"].ToString()).ToString("yyyy/MM/dd") + "～" + Convert.ToDateTime(dr["NewsEndDate"].ToString()).ToString("yyyy/MM/dd"); 
            //FD_PRINT_LINK.NavigateUrl = "News_Rpt.aspx?uid=" + HFD_NewsUID.Value;  
        }
    }
    //---------------------------------------------------------------------------
    public void FileDownLoad_DataBind()
    {
        ////****變數宣告****//
        //string strSql, ser_no;
        //DataTable dt;
        //Dictionary<string, object> dict = new Dictionary<string, object>();
        ////****變數設定****//
        //ser_no = HFD_SER_NO.Value;
        ////****設定查詢****//
        //strSql = "select * from upload where Ap_Name='news' and attach_type<>'doc' and Ser_no=@ser_no ";
        //dict.Add("ser_no", ser_no);

        ////****執行語法****//
        //NpoDB.ExecuteSQLS(strSql, dict);
        //dt = NpoDB.GetDataTableS(strSql, dict);

        ////****畫面處理****//
        //StringBuilder sb = new StringBuilder();

        //if (ImgPosition == "bottom")
        //   sb.AppendLine(NewsContent);

        //sb.AppendLine(@"<table cellspacing=""0"" cellpadding=""2"" width=""98"" align="""+ImgPosition+@""" border=""0"">");
        //sb.AppendLine("<tbody>");
        //foreach(DataRow dr in dt.Rows )
        //{
        //    sb.AppendLine("<tr>");
        //    sb.AppendLine(@"<td align=""middle"">");
        //    sb.AppendLine(@"<img src=""../upload/"+dr["Upload_fileURL"]+@""" border=""0""></td>");
        //    sb.AppendLine("</tr");
        //    sb.AppendLine("<!-- 圖說 -->");
        //    sb.AppendLine("<tr>");
        //    sb.AppendLine("<td>");
        //    sb.AppendLine(@"<font color=""#4B640B"">");
        //    sb.AppendLine(dr["upload_filename"].ToString());
        //    sb.AppendLine("</font>");
        //    sb.AppendLine("</td>");
        //    sb.AppendLine("</tr>");
        //}
        //sb.AppendLine("</tbody>");
        //sb.AppendLine("</table>");

        //if (ImgPosition != "bottom")
        //    sb.AppendLine(NewsContent);

        //lbl_FileDownLoad.Text = sb.ToString();
    }
    public void DocDownLoad_DataBind()
    {
    //    //****變數宣告****//
    //    string strSql, ser_no;
    //    DataTable dt;
    //    Dictionary<string, object> dict = new Dictionary<string, object>();
    //    //****變數設定****//
    //    ser_no = HFD_NewsUID.Value;
    //    //****設定查詢****//
    //    strSql = "select * from upload where Ap_Name='news' and attach_type='doc' and Ser_no=@ser_no ";
    //    dict.Add("ser_no", ser_no);

    //    //****執行語法****//
    //    NpoDB.ExecuteSQLS(strSql, dict);
    //    dt = NpoDB.GetDataTableS(strSql, dict);
    //    //****畫面處理****//
    //    StringBuilder sb = new StringBuilder();

    //    foreach (DataRow dr in dt.Rows )
    //    {
    //        sb.AppendLine(@"<div>文件下載：<a href=""../upload/" + dr["Upload_fileURL"] + @""" class=""contentbar"" target=""_blank""><img border=""0"" src=""../images/save.GIF"" align=""absmiddle"">" + dr["upload_filename"] + "</a></div>");
    //    }
    //    lbl_DocDownLoad.Text = sb.ToString(); 
    }
    //------------------------------------------------------------------------------
}
