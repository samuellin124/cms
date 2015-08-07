using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;

/// <summary>
/// Summary description for Authrity
/// </summary>
//-------------------------------------------------------------------------------------------------------------
public class Authrity
{
    public static Page page { get { return HttpContext.Current.CurrentHandler as Page; } }

    public static bool PageRight(string Action)
    {
        try
        {
            if (RightCheck(Action) == false)
            {
                Util.CreateJavaScript("<script>alert('權限不足!!!');history.go(-1);</script>");
                //Util.CreateJavaScript("alert('權限不足!!!');history.go(-1);");
                return false;
            }
        }
        catch
        {
            if (page.Session.Contents["User_id"] == null)
            {
                Util.CreateJavaScript("<script>window.parent.location.href='../Default.aspx';</script>");
            }
        }
        return true;
    }
    //---------------------------------------------------------------------------------
    public static bool CheckPageRight(bool PageRight)
    {
        if (PageRight == false)
        {
            Util.CreateJavaScript("<script>alert('權限不足!!!');history.go(-1);</script>");
        }
        return PageRight;
    }
    //---------------------------------------------------------------------------------
    private static string GetMenuID()
    {
        string MenuID = "0";
        string strSql = "select MenuID from AdminMenu where IsUse=1\n";
        strSql += "and ProgramID=@ProgID";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("ProgID", Util.GetSessionValue("ProgID"));
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count != 0)
        {
            if (dt.Rows[0]["MenuID"].ToString() != "")
            {
                MenuID = dt.Rows[0]["MenuID"].ToString();
            }
        }
        return MenuID;
    }
    //-------------------------------------------------------------------------------------------------------------
    public static bool RightCheck(string Action)
    {
        if ((page as BasePage).SessionInfo == null)
        {
            return false;
        }
        string menu_uid = GetMenuID();

        bool bValue = false;
        string strSql = "select r.*\n";
        strSql += " from AdminRight r\n";
        strSql += " inner join AdminMenu m on r.MenuID=m.MenuID\n";
        strSql += " where m.ProgramID=@ProgramID\n";
        strSql += " and r.GroupID=@GroupID";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("ProgramID", Util.GetSessionValue("ProgID"));
        dict.Add("GroupID", (page as BasePage).SessionInfo.GroupID);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count > 0)
        {
            string Value = dt.Rows[0][Action].ToString();
            bValue = Value == "True" ? true : false;
        }
        return bValue;
    }
    //-------------------------------------------------------------------------------------------------------------
    public static GroupAuthrity GetGroupRight()
    {
        string menu_uid = GetMenuID();

        string strSql = @"
                         select r.*
                         from AdminRight r
                         inner join AdminMenu m on r.MenuID=m.MenuID
                         where m.ProgramID=@ProgramID
                         and r.GroupID=@GroupID
                       ";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("ProgramID", Util.GetSessionValue("ProgID"));
        CSessionInfo SessionInfo = page.Session["SessionInfo"] as CSessionInfo;
        dict.Add("GroupID", SessionInfo.GroupID);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        GroupAuthrity ga = new GroupAuthrity();
        if (dt.Rows.Count > 0)
        {
            ga.AddNew = dt.Rows[0]["_AddNew"].ToString() == "True" ? true : false;
            ga.Update = dt.Rows[0]["_Update"].ToString() == "True" ? true : false;
            ga.Delete = dt.Rows[0]["_Delete"].ToString() == "True" ? true : false;
            ga.Query = dt.Rows[0]["_Query"].ToString() == "True" ? true : false;
            ga.Focus = dt.Rows[0]["_Focus"].ToString() == "True" ? true : false;
            ga.Print = dt.Rows[0]["_Print"].ToString() == "True" ? true : false;
            ga.Examine = dt.Rows[0]["_Examine"].ToString() == "True" ? true : false;
            ga.ReExamine1 = dt.Rows[0]["_ReExamine1"].ToString() == "True" ? true : false;
            ga.ReExamine2 = dt.Rows[0]["_ReExamine2"].ToString() == "True" ? true : false;
            ga.Approve = dt.Rows[0]["_Approve"].ToString() == "True" ? true : false;
        }
        return ga;
    }
    //-------------------------------------------------------------------------------------------------------------
    //按扭為 Button
    public static void CheckButtonRight(string Action, Button btn)
    {
        btn.Visible = RightCheck(Action);
    }
    //-------------------------------------------------------------------------------------------------------------
    //按扭為 ImageButton
    public static void CheckButtonRight(string Action, ImageButton btn)
    {
        btn.Visible = RightCheck(Action);
    }
    //-------------------------------------------------------------------------------------------------------------
    //按扭為 LinkButton
    public static void CheckButtonRight(string Action, LinkButton btn)
    {
        btn.Visible = RightCheck(Action);
    }
    //-------------------------------------------------------------------------------------------------------------
} //end of class Authrity

public class GroupAuthrity
{
    public bool AddNew;
    public bool Update;
    public bool Delete;
    public bool Query;
    public bool Focus;
    public bool Print;
    public bool Examine;
    public bool ReExamine1;
    public bool ReExamine2;
    public bool Approve;
}