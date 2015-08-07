using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

public partial class XMenu : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblRemoteAddr.Text = Request.ServerVariables["REMOTE_ADDR"];
        if (lblRemoteAddr.Text == "::1")
        {
            lblRemoteAddr.Text = "127.0.0.1";
        }

        DataTable dt = GetTopMenu(); //先選擇第一層功能表
        string menuStr = CreateMenuList(dt); //建立 1,2 層的 menu
        lblMenuContainer.Text = menuStr;
    }
    //---------------------------------------------------------------------------
    //取得選單群組資料
    public DataTable GetTopMenu()
    {
        string DeptID = SessionInfo.DeptID;
        string UserID = SessionInfo.UserID;

        string strSql = "select m.MenuID, m.ParentID, m.MenuName \n";
        strSql += "FROM AdminMenu m \n";
        strSql += "left join AdminRight r on m.MenuID=r.MenuID \n";
        strSql += "where m.IsUse=1 and m.IsMenu=1 and m.ParentID=0 \n";
        strSql += "and r._focus=1 and r.GroupID=@GroupID \n";
        strSql += "order by m.sort \n";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("DeptID", DeptID);
        dict.Add("GroupID", SessionInfo.GroupID);

        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        return dt;
    }
    //---------------------------------------------------------------------------
    //製作選單
    public string CreateMenuList(DataTable dt)
    {
        int count = dt.Rows.Count;
        StringBuilder MenuListSb = new StringBuilder();
        DataRow dr;              

        MenuListSb.AppendLine("<div id='menu'>");
        MenuListSb.AppendLine("<ul>");
        MenuListSb.AppendLine("<li onclick=\"parent.frames[2].location.href='MainDefault.aspx'\"><a href='#'>個人首頁</a></li>");
        for (int i = 1; i <= count; i++)
        {                         
            dr = dt.Rows[i - 1];
            MenuListSb.AppendLine(CreateMenu(dr["MenuID"].ToString(), dr["ParentID"].ToString(), dr["MenuName"].ToString(), i));
        }
        MenuListSb.AppendLine("<li><a href=\"JavaScript:if(confirm('是否確定要登出 ?')){window.parent.location.href='../Default.aspx?logout=true&UserID=" + SessionInfo.UserID + "';} \" target='_top'>登出</a></li>");
       
          

        MenuListSb.AppendLine("</ul>");
        MenuListSb.AppendLine("</div>");
        return MenuListSb.ToString();
    }
    //---------------------------------------------------------------------------
    //製作子選單
    public string CreateMenu(string MenuID, string ParentID, string MenuName, int i)
    {
        StringBuilder sb = new StringBuilder();
        StringBuilder htmlSb = new StringBuilder();
        string DeptID = SessionInfo.DeptID;
        string UserID = SessionInfo.UserID;

        string strSql = "select m.* FROM AdminMenu m\n";
        strSql += "left join AdminRight r on m.MenuID=r.MenuID\n";
        strSql += "where m.IsUse=1 and m.IsMenu=1 and m.ParentID=@MenuID\n";
        strSql += "and r._focus=1 and r.GroupID=@GroupID\n";
        strSql += "order by m.sort\n";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("UserID", UserID);
        dict.Add("DeptID", DeptID);
        dict.Add("MenuID", MenuID);
        dict.Add("GroupID", SessionInfo.GroupID);

        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        int count = 0;
        count = dt.Rows.Count;

        //第一層選單名稱
        htmlSb.AppendLine("<li><a href='#' class='menu'>" + MenuName + "</a>");

        htmlSb.AppendLine("<ul class='submenu'>");
        int j = 0;
        DataRow dr;
        int dataCount = dt.Rows.Count;

        //第二層選單名稱
        for (j = 0; j < dataCount; j++)
        {
            dr = dt.Rows[j];
            string Extension = System.IO.Path.GetExtension(dr["ProgramURL"].ToString()).ToUpper();
            htmlSb.AppendLine("<li>");
            if (Extension == ".ASPX")
            {
                htmlSb.AppendLine("<a href=\"../" + dr["ProgramURL"].ToString() + "?menu_id=" + dr["MenuID"].ToString() + "\" target=\"main\" onfocus=\"this.blur();\">");
            }

            htmlSb.AppendLine(dr["MenuName"].ToString());

            if (Extension == ".ASPX")
            {
                htmlSb.AppendLine("</a><li>");
            }
            htmlSb.AppendLine("</li>");
        }
        htmlSb.AppendLine("</ul>");
        htmlSb.AppendLine("</li>");

        return htmlSb.ToString();
    }
    //---------------------------------------------------------------------------
}