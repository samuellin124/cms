using System;
using System.Web.UI;
using log4net;
using System.Reflection;
using log4net.Appender;

/// <summary>
/// BasePage 的摘要描述
/// </summary>
public class BasePage : System.Web.UI.Page
{
    public CSessionInfo SessionInfo;
    public NpoDB objNpoDB;
    protected ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    bool IsRedirect = false;
    //---------------------------------------------------------------------------
    protected void Page_PreInit(object sender, EventArgs e)
    {
        objNpoDB = new NpoDB();

        //取得使用者 Session 物件, 逾時則轉到 default.aspx
        SessionInfo = GetSessionInfo();
        if (SessionInfo == null)
        {
            IsRedirect = true;
            Response.Write(@"<script>window.parent.location.href='../Default.aspx';</script>");
        }
    }
    //---------------------------------------------------------------------------
    protected override void OnLoad(EventArgs e)
    {
        //當 SessionInfo == null 時,避免 Page 繼續往下執行
        if (IsRedirect == true)
        {
            return;
        }

        base.OnLoad(e);
        if (!IsPostBack)
        {
            //CaseUtil.LogData(CaseUtil.LogType.LogTime, SessionInfo.UserID);
        }
    }
    //---------------------------------------------------------------------------
    //Session["Msg"]有值時，會Alert，並清空
    public void ShowSysMsg()
    {
        if (Session["Msg"] != null && Session["Msg"].ToString() != "")
        {
            Alert(Session["Msg"].ToString());
            Session["Msg"] = "";
        }
    }
    //---------------------------------------------------------------------------
    //直接顯示訊息
    public void ShowSysMsg(string SysMsg)
    {
        SetSysMsg(SysMsg);
        ShowSysMsg();
    }
    //---------------------------------------------------------------------------
    //設定訊息
    public void SetSysMsg(string SysMsg)
    {
        Session["Msg"] = SysMsg;
    }
    //---------------------------------------------------------------------------
    //顯示警告
    public void Alert(string AlertMessage)
    {
        String cstext = "alert('" + AlertMessage + "');";
        
        CreateJavaScript(cstext);
    }
    //---------------------------------------------------------------------------
    //關閉視窗
    public void JS_CloseWindow()
    {
        String cstext = "window.close();";
        CreateJavaScript(cstext);
    }
    //---------------------------------------------------------------------------
    //父視窗Reload And 關閉目前視窗
    public void JS_OpenReloadAndCloseWindow()
    {
        CreateJavaScript("opener.window.location.reload();window.close();");
    }
    //---------------------------------------------------------------------------
    //嵌入JavaScript
    public void CreateJavaScript(string JavaScript)
    {
        String csname = "PageScript";
        Type cstype = GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, csname, JavaScript, true);
    }
    //---------------------------------------------------------------------------
    //取得登入者資訊
    public CSessionInfo GetSessionInfo()
    {
      if (Session["SessionInfo"] != null && Session["SessionInfo"].ToString() != "")
      {
          return (CSessionInfo)Session["SessionInfo"];
      }
      return null;
    }
    //---------------------------------------------------------------------------
    public void ClearSessionInfo()
    {
        SessionInfo.Clear();
    }
    //---------------------------------------------------------------------------
    //Session判斷
    protected void SessionValid()
    {
        if (Session["SessionInfo"] != null && Session["SessionInfo"].ToString() != "")
        {
            Response.Write(@"<script>window.parent.location.href='../Default.aspx';</script>");
            Response.End();
        }
    }
    //---------------------------------------------------------------------------
}
