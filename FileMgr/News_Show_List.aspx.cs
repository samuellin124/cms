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
using System.Text ;

public partial class FileMgr_News_Show_List :BasePage 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            LoadFormData();
        }
    }
    //------------------------------------------------------------------------------
    public void LoadFormData()
    {
        string SysDate = Util.DateTime2String(DateTime.Now, DateType.yyyyMMddHHmmss, EmptyType.ReturnEmpty);
        string strSql = "select news.* from News\n";
        strSql += "where @SysDate between NewsBeginDate and NewsEndDate\n";
        strSql +="order by NewsRegDate Desc ";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("SysDate", SysDate);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);

        StringBuilder sb = new StringBuilder();
        foreach(DataRow dr in dt.Rows )
        {
            //單筆資料第一行
            sb.AppendLine("<div style='text-align:left'>");
            sb.AppendLine(@"<span style='width:4%;text-align:right;'>");
            sb.AppendLine(@"<img border='0' src='../images/DIR_tri.gif'/>");
            sb.AppendLine("</span>");
            sb.AppendLine(@"<span style='width:96%:text-align:left;color:#660000'>");
            sb.AppendLine(@"<a href=""../filemgr/news_show.aspx?NewsUID=" + dr["uid"].ToString() + @""" class='news'>" + "【" + dr["NewsType"].ToString() + "】" + dr["NewsSubject"].ToString() + "</a> (" + Convert.ToDateTime(dr["NewsBeginDate"].ToString()).ToString("yyyy/MM/dd") + "～" + Convert.ToDateTime(dr["NewsEndDate"].ToString()).ToString("yyyy/MM/dd") + ")");
            sb.AppendLine("</span>");
            sb.AppendLine("</div>");
            //單筆資料第二行
            sb.AppendLine(@"<div style='text-align:left'>");
            sb.AppendLine(@"<span style='width:4%;text-align:right;height:5px;'>&nbsp;");
            sb.AppendLine("</span>");
            sb.AppendLine(@"<span style='width:96%:text-align:left;color:#4B4B4B;height:5px;'>");
            sb.AppendLine(dr["NewsBrief"].ToString());
            sb.AppendLine("</span>");
            sb.AppendLine("</div>");
        }
        Grid_List.Text = sb.ToString(); 
    }
    //------------------------------------------------------------------------------
}