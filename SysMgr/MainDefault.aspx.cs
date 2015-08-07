using System;
using System.Web.UI;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.IO;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Threading;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class MainDefault : BasePage
{
    GroupAuthrity ga = null;
    #region NpoGridView 處理換頁相關程式碼
    Button btnNextPage, btnPreviousPage, btnGoPage;
    HiddenField HFD_CurrentPage;
    //排序用
    HiddenField HFD_CurrentSortField;
    HiddenField HFD_CurrentSortDir;

    override protected void OnInit(EventArgs e)
    {
        CreatePageControl();
        base.OnInit(e);
    }
    private void CreatePageControl()
    {
        // Create dynamic controls here.
        btnNextPage = new Button();
        btnNextPage.ID = "btnNextPage";
        btnNextPage.Height = 1;
        btnNextPage.Width = 1;
        Form1.Controls.Add(btnNextPage);
        btnNextPage.Click += new System.EventHandler(btnNextPage_Click);

        btnPreviousPage = new Button();
        btnPreviousPage.ID = "btnPreviousPage";
        Form1.Controls.Add(btnPreviousPage);
        btnPreviousPage.Click += new System.EventHandler(btnPreviousPage_Click);

        btnGoPage = new Button();
        btnGoPage.ID = "btnGoPage";
        Form1.Controls.Add(btnGoPage);
        btnGoPage.Click += new System.EventHandler(btnGoPage_Click);

        HFD_CurrentPage = new HiddenField();
        HFD_CurrentPage.Value = "1";
        HFD_CurrentPage.ID = "HFD_CurrentPage";
        Form1.Controls.Add(HFD_CurrentPage);

        HFD_CurrentSortField = new HiddenField();
        HFD_CurrentSortField.Value = "";
        HFD_CurrentSortField.ID = "HFD_CurrentSortField";
        Form1.Controls.Add(HFD_CurrentSortField);

        HFD_CurrentSortDir = new HiddenField();
        HFD_CurrentSortDir.Value = "asc";
        HFD_CurrentSortDir.ID = "HFD_CurrentSortDir";
        Form1.Controls.Add(HFD_CurrentSortDir);
    }
    protected void btnPreviousPage_Click(object sender, EventArgs e)
    {
        HFD_CurrentPage.Value = Util.MinusStringNumber(HFD_CurrentPage.Value);
        GetConsultData();// LoadFormData();
    }
    protected void btnNextPage_Click(object sender, EventArgs e)
    {
        HFD_CurrentPage.Value = Util.AddStringNumber(HFD_CurrentPage.Value);
        GetConsultData(); //LoadFormData();
    }
    protected void btnGoPage_Click(object sender, EventArgs e)
    {
        GetConsultData();// LoadFormData();
    }
    #endregion NpoGridView 處理換頁相關程式碼
    protected void Page_Load(object sender, EventArgs e)
    {
        ShowSysMsg();
        Util.ShowSysMsgWithScript("$('#btnPreviousPage').hide();$('#btnNextPage').hide();$('#btnGoPage').hide();");
        if (!IsPostBack)
        {
            //公告訊息
            HFD_UserID.Value = SessionInfo.UserID;
           // HFD_GroupID.Value = SessionInfo.GroupID;
            lblNews.Text = GetNewData();

            //本月壽星
            getToMonth();
            //下月壽星
            getNextMonth();
           
            GetConsultData();
        }

        CookieProcess();
    }
    //-------------------------------------------------------------------------
    private void CookieProcess()
    {
        if (HttpContext.Current.Response.Cookies["AgentID"] != null)
        {
            //HttpCookie CookieAgentID = new HttpCookie("AgentID");
            // CookieAgentID  = Request.Cookies["AgentID"];
           // HttpCookie CookieAgentID = Request.Cookies["AgentID"];
         
            HttpCookie CookieAgentID = new HttpCookie("AgentID");
            CookieAgentID.Value = SessionInfo.UserID;
            CookieAgentID.Expires = DateTime.Now.AddYears(1);
            Response.Cookies.Add(CookieAgentID);
            
        }
        else
        {
            HttpCookie CookieAgentID = new HttpCookie("AgentID");
            // Set the cookie value.
            CookieAgentID.Value = SessionInfo.UserID;
            // Set the cookie expiration date.
            CookieAgentID.Expires = DateTime.Now.AddYears(1);
            // Add the cookie.
            Response.Cookies.Add(CookieAgentID);
         }
       
    }

    //本月壽星
       //DateTime.Now.AddMonths(1)
       //         string sql="select uid,UserName as 志工名稱,birthday as 生日 FROM AdminUser WHERE birthday like '"+  DateTime.Now.Month +"/%'";
    private string getToMonth()
    {

        //讀取資料庫
        string strSql = @" 
                        select uid,UserName as 志工名稱,birthday as 生日 FROM AdminUser WHERE birthday like '"+  DateTime.Now.Month +"/%'";
        DataRow dr = null;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);

        if (dt.Rows.Count == 0)
        {
            return "";
        }
        dr = dt.Rows[0];
        string[] Xvalue = new string[dt.Rows.Count];
        int colCount = 0;
        lblTBirth.Text = "";
        colCount = dt.Rows.Count;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            Xvalue[i] = dt.Rows[i]["志工名稱"].ToString();
        lblTBirth.Text += Xvalue[i] + "、";
        }
        lblTBirth.Text = lblTBirth.Text.TrimEnd('、');
        return "";
    }
    private string getNextMonth()
    {

        //讀取資料庫
        string strSql = @" 
                        select uid,UserName as 志工名稱,birthday as 生日 FROM AdminUser WHERE birthday like '" + DateTime.Now.AddMonths(1).Month + "/%'";
        DataRow dr = null;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count == 0)
        {
            return "";
        }
        dr = dt.Rows[0];
        string[] Xvalue = new string[dt.Rows.Count];
        int colCount = 0;
        lblNBirth.Text = "";
        colCount = dt.Rows.Count;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            Xvalue[i] = dt.Rows[i]["志工名稱"].ToString();

            lblNBirth.Text += Xvalue[i] + "、";

        }
        lblNBirth.Text = lblNBirth.Text.TrimEnd('、');
        return "";
    }
    //------------------------------------------------------------------------------
    public string GetNewData()
    {
        string strSql = "";
        DataTable dt;
        string SysDate =  Util.DateTime2String(DateTime.Now, DateType.yyyyMMdd, EmptyType.ReturnEmpty);
        strSql = "select top 2 News.* from news\n";
        strSql += "where @SysDate between NewsBeginDate and NewsEndDate\n";
        strSql += "and DeptName like @DeptName\n";
        strSql += "order by NewsRegDate Desc";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("SysDate", SysDate);
        dict.Add("DeptName", "%" + SessionInfo.DeptName + "%");
        dt = NpoDB.GetDataTableS(strSql, dict);

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("<table width='98%' id='AutoNumber1' class='table_v' >");
        sb.AppendLine("<tr>");
        sb.AppendLine("<td width='100%' colspan='2'> </td>");
        sb.AppendLine("</tr>");
        foreach (DataRow dr in dt.Rows)
        {
            sb.AppendLine("<tr>");
            sb.AppendLine(" <td width='4%' align='right'><img border='0' src='../images/DIR_tri.gif' /></td>");
            sb.AppendLine(@" <td width='96%' style='color:#660000' align='left'>");
            //sb.AppendLine("<a href='../filemgr/News_Show.aspx?NewsUID=" + dr["uid"].ToString() + "' class='news'>" + dr["NewsSubject"].ToString() + "</a> ");
            sb.AppendLine("<a href='../filemgr/News_Show.aspx?NewsUID=" + dr["uid"].ToString() + "' class='news'>" +"【"+ dr["NewsType"].ToString() +"】"+ dr["NewsSubject"].ToString() + "</a> ");
            //sb.AppendLine("(" + dr["NewsRegDate"] + ")");
            //sb.AppendLine("(" + Convert.ToDateTime(dr["NewsBeginDate"].ToString()).ToString("yyyy/MM/dd") + "～" + Convert.ToDateTime(dr["NewsEndDate"].ToString()).ToString("yyyy/MM/dd") + ")");
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td width='4%' height='5'></td>");
            sb.AppendLine("<td width='96%' style='color:#4B4B4B' height='5'  align='left'> ");
            sb.AppendLine(dr["NewsBrief"].ToString());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
        }
        sb.AppendLine("</table>");
        return sb.ToString();
    }
    //------------------------------------------------------------------------------

    public void   GetConsultData()
    {
        string strSql = @"
                        select  a.uid as uid, CName As  姓名,b.Memo as 備註,Phone As 電話, convert(varchar(19), CreateDate,120) as 建檔時間, ConsultantMain as [來電諮詢別(大類)], ConsultantItem  as [來電諮詢別(分項)] 
                        from Consulting a  inner join Member b on a.MemberUID = b.uid
                        where 1=1 
                        and isnull(a.IsDelete, '') != 'Y' and isnull(b.IsDelete, '') != 'Y'
                        And
                        (((ISNULL(Overseas,'')='' AND ISNULL(city,'')=''))
                        or ((ISNULL(HarassPhone,'') <> '' AND   ISNULL(Special,'') <>'')  AND  (ISNULL(ConsultantMain,'')='' or isnull(ConsultantItem,'')=''))
                        OR ((ISNULL(HarassPhone,'') = '' AND ISNULL(Special,'')='')  AND  (ISNULL(ConsultantMain,'')='' or isnull(ConsultantItem,'')=''))
                        OR( ISNULL(CName,'')='' OR ISNULL(sex,'')='' OR ISNULL(Marry,'')='' OR ISNULL(Christian,'')='' OR ISNULL(Event,'')='' OR  ISNULL(CreateDate,'')='' OR  ISNULL(EndDate,'')='') )
                        ";
        strSql += " And ServiceUser=@ServiceUser ";
        //if (SessionInfo.GroupID != "1")
        //{
        //    strSql += " And ServiceUser=@ServiceUser ";
        //}
        strSql += " Order by CreateDate desc";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("ServiceUser", SessionInfo.UserID.ToString());
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        int count = dt.Rows.Count;

        //Grid initial
        //沒有需要特別處理的欄位時
        NPOGridView npoGridView = new NPOGridView();
        npoGridView.Source = NPOGridViewDataSource.fromDataTable;
        //npoGridView.dataTable = dt;
        //npoGridView.Keys.Add("uid");
        //npoGridView.DisableColumn.Add("uid");
        //npoGridView.CurrentPage = Util.String2Number(HFD_CurrentPage.Value);
        //Response.Write(@"<script>window.parent.location.href='ConsultEdit2.aspx';</script>");
        //npoGridView.EditLinkTarget = "','target=_blank,help=no,status=yes,resizable=yes,scrollbars=yes,scrolling=yes,scroll=yes,center=yes,width=900px,height=500px";
        //npoGridView.EditLink = Util.RedirectByTime("../CaseMgr/ConsultEdit2.aspx", "ConsultingUID=");
        //return npoGridView.Render();
         //lblConsult.Text = npoGridView.Render();
        //********************************************************************************************
        //自訂欄位
        //NPOGridView npoGridView = new NPOGridView();
        //來源種類
        npoGridView.Source = NPOGridViewDataSource.fromDataTable;
        //使用的 DataTable
        npoGridView.dataTable = dt;
        //不要顯示的欄位(可以多個)
               
        npoGridView.DisableColumn.Add("uid");
        npoGridView.EditLinkTarget = "','target=_blank,help=no,status=yes,resizable=yes,scrollbars=yes,scrolling=yes,scroll=yes,center=yes,width=900px,height=500px";
        npoGridView.EditLink = Util.RedirectByTime("../CaseMgr/ConsultEdit2.aspx", "ConsultingUID=");

        //使用的 key 欄位,用在組成 QueryString 的 key 值(EditLink 會用到)
        npoGridView.Keys.Add("uid");

        //如果 GridView 要翻頁, 要設定 CurrentPage 的值
        if (npoGridView.ShowPage == true)
        {
            npoGridView.CurrentPage = Util.String2Number(HFD_CurrentPage.Value);
        }
        NPOGridViewColumn col;
        col = new NPOGridViewColumn("姓名"); //產生所需欄位及設定 Title
        col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
        col.ColumnName.Add("姓名"); //使用欄位名稱
        col.CellStyle = "width:100px";
        npoGridView.Columns.Add(col);

        col = new NPOGridViewColumn("備註"); //產生所需欄位及設定 Title
        col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
        col.ColumnName.Add("備註"); //使用欄位名稱
        col.Link = Util.RedirectByTime("../CaseMgr/ConsultEdit2.aspx", "ConsultingUID=");
        col.CellStyle = "width:200px";
        npoGridView.Columns.Add(col);
        //----------------------------------------------
        col = new NPOGridViewColumn("電話"); //產生所需欄位及設定 Title
        col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
        col.ColumnName.Add("電話"); //使用欄位名稱
        col.Link = Util.RedirectByTime("../CaseMgr/ConsultEdit2.aspx", "ConsultingUID=");
        col.CellStyle = "width:150px";
        npoGridView.Columns.Add(col);
        // lblGridList.Text = npoGridView.Render();
        //----------------------------------------------------------
        col = new NPOGridViewColumn("建檔時間"); //產生所需欄位及設定 Title
        col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
        col.ColumnName.Add("建檔時間"); //使用欄位名稱
        col.Link = Util.RedirectByTime("../CaseMgr/ConsultEdit2.aspx", "ConsultingUID=");
        col.CellStyle = "width:150px";
        npoGridView.Columns.Add(col);
        //----------------------------------------------------------
        col = new NPOGridViewColumn("來電諮詢別(大類)"); //產生所需欄位及設定 Title
        col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
        col.ColumnName.Add("來電諮詢別(大類)"); //使用欄位名稱
        col.Link = Util.RedirectByTime("../CaseMgr/ConsultEdit2.aspx", "ConsultingUID=");
        col.CellStyle = "width:200px";
        npoGridView.Columns.Add(col);
        //----------------------------------------------------------
        col = new NPOGridViewColumn("來電諮詢別(分項)"); //產生所需欄位及設定 Title
        col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
        col.ColumnName.Add("來電諮詢別(分項)"); //使用欄位名稱
        col.Link = Util.RedirectByTime("../CaseMgr/ConsultEdit2.aspx", "ConsultingUID=");
        col.CellStyle = "width:200px";
        npoGridView.Columns.Add(col);
        lblConsult.Text = npoGridView.Render();
        //----------------------------------------------------------
        //********************************************************************************************

    }
}
