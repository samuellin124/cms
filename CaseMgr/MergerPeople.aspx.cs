using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class CaseMgr_MergerPeople : BasePage
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
        LoadFormData();
    }
    protected void btnNextPage_Click(object sender, EventArgs e)
    {
        HFD_CurrentPage.Value = Util.AddStringNumber(HFD_CurrentPage.Value);
        LoadFormData();
    }
    protected void btnGoPage_Click(object sender, EventArgs e)
    {
        LoadFormData();
    }
    #endregion NpoGridView 處理換頁相關程式碼
    //---------------------------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["ProgID"] = "MergerPeople";
        litTitle.Text = "人員合併作業";
        //權限控制
        AuthrityControl();
        //設定一些按鍵的 Visible 屬性
        SetButton();
        Util.ShowSysMsgWithScript("$('#btnPreviousPage').hide();$('#btnNextPage').hide();$('#btnGoPage').hide();");
        if (!IsPostBack)
        {
            //載入下拉式選單資料
            LoadDropDownListData();
            //載入資料
            //  LoadFormData();
        }
    }                    
    //-------------------------------------------------------------------------
    private void SetupDefaultValue()
    {
    }
    //-------------------------------------------------------------------------
    private void SetButton()
    {
    }
    //-------------------------------------------------------------------------
    private void AuthrityControl()
    {
        ga = Authrity.GetGroupRight();
        if (Authrity.CheckPageRight(ga.Focus) == false)
        {
            return;
        }
    }
    //-------------------------------------------------------------------------
    public void LoadDropDownListData()
    {
        //Util.FillCityData(ddlCity);
        //Util.FillAreaData(ddlArea, ddlCity.SelectedValue);
    }
    //-------------------------------------------------------------------------------------------------------------
    private void LoadFormData()
    {
           string strSql = @"
                        select a.uid as uid, CName As  姓名,b.Memo as [備註],Phone As 電話, convert(varchar(19), CreateDate,120) as 建檔時間 
                         --REVERSE(SUBSTRING(REVERSE(ConsultantMain),CHARINDEX(',',REVERSE(ConsultantMain))+1,LEN(ConsultantMain))) [來電諮詢別(大類)],
                         --REVERSE(SUBSTRING(REVERSE(ConsultantItem),CHARINDEX(',',REVERSE(ConsultantItem))+1,LEN(ConsultantItem))) [來電諮詢別(分項)]
            --, ConsultantMain as [來電諮詢別(大類)], ConsultantItem  as [來電諮詢別(分項)] 
                        from Consulting a  inner join Member b on a.MemberUID = b.uid
                        where 1=1 
                        and isnull(a.IsDelete, '') != 'Y'
                    ";

        strSql += " Order by CreateDate desc";
        Dictionary<string, object> dict = new Dictionary<string, object>();
       
        dict.Add("ServiceUser", SessionInfo.UserID.ToString());
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);

        if (dt.Rows.Count == 0)
        {
            lblGridList.Text = "";
            ShowSysMsg("查無資料");
            return;
        }

        //********************************************************************************************
        //自訂欄位
        NPOGridView npoGridView = new NPOGridView();
        //來源種類
        npoGridView.Source = NPOGridViewDataSource.fromDataTable;
        //使用的 DataTable
        npoGridView.dataTable = dt;
        //不要顯示的欄位(可以多個)

        npoGridView.DisableColumn.Add("uid");
       // npoGridView.EditLinkTarget = "','target=_blank,help=no,status=yes,resizable=yes,scrollbars=yes,scrolling=yes,scroll=yes,center=yes,width=900px,height=500px";
       // npoGridView.EditLink = Util.RedirectByTime("ConsultEdit2.aspx", "ConsultingUID=");
        //使用的 key 欄位,用在組成 QueryString 的 key 值(EditLink 會用到)
        npoGridView.Keys.Add("uid");
        //如果 GridView 要翻頁, 要設定 CurrentPage 的值
        npoGridView.ShowPage = false;
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
       // col.Link = Util.RedirectByTime("ConsultEdit2.aspx", "ConsultingUID=");
        col.CellStyle = "width:200px";
        npoGridView.Columns.Add(col);
        //----------------------------------------------
        col = new NPOGridViewColumn("電話"); //產生所需欄位及設定 Title
        col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
        col.ColumnName.Add("電話"); //使用欄位名稱
       // col.Link = Util.RedirectByTime("ConsultEdit2.aspx", "ConsultingUID=");
        col.CellStyle = "width:150px";
        npoGridView.Columns.Add(col);
        // lblGridList.Text = npoGridView.Render();
        //----------------------------------------------------------
        col = new NPOGridViewColumn("建檔時間"); //產生所需欄位及設定 Title
        col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
        col.ColumnName.Add("建檔時間"); //使用欄位名稱
       // col.Link = Util.RedirectByTime("ConsultEdit2.aspx", "ConsultingUID=");
        col.CellStyle = "width:150px";
        npoGridView.Columns.Add(col);
        lblGridList.Text = npoGridView.Render();
        //********************************************************************************************
    }
    //-------------------------------------------------------------------------------------------------------------
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
    }
    //-------------------------------------------------------------------------------------------------------------
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        LoadFormData();
    }
    //-------------------------------------------------------------------------------------------------------------

   
    private void LoadFormData2()
    {
        string strSql = @"
                        select  uid, CName As  姓名,Memo as [備註],Phone As 電話
                        from Member 
                        where 1=1 
                        and isnull(IsDelete, '') != 'Y'
                    ";
        if (HFD_PeopleUID.Value != "")
        {
            strSql += " and uid in ("+ HFD_PeopleUID.Value +")";
        }
        Dictionary<string, object> dict = new Dictionary<string, object>();
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);

        if (dt.Rows.Count == 0)
        {
            lblPeople.Text = "";
            ShowSysMsg("查無資料");
            return;
        }
        //********************************************************************************************
        //自訂欄位
        NPOGridView npoGridView = new NPOGridView();
        //來源種類
        npoGridView.Source = NPOGridViewDataSource.fromDataTable;
        //使用的 DataTable
        npoGridView.dataTable = dt;
        //不要顯示的欄位(可以多個)

        npoGridView.DisableColumn.Add("uid");
        //npoGridView.EditLinkTarget = "','target=_blank,help=no,status=yes,resizable=yes,scrollbars=yes,scrolling=yes,scroll=yes,center=yes,width=900px,height=500px";
        //npoGridView.EditLink = Util.RedirectByTime("ConsultEdit2.aspx", "ConsultingUID=");

        //使用的 key 欄位,用在組成 QueryString 的 key 值(EditLink 會用到)
        npoGridView.Keys.Add("uid");

        npoGridView.ShowPage = false;
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
       // col.Link = Util.RedirectByTime("ConsultEdit2.aspx", "ConsultingUID=");
        col.CellStyle = "width:200px";
        npoGridView.Columns.Add(col);
        //----------------------------------------------
        col = new NPOGridViewColumn("電話"); //產生所需欄位及設定 Title
        col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
        col.ColumnName.Add("電話"); //使用欄位名稱
       // col.Link = Util.RedirectByTime("ConsultEdit2.aspx", "ConsultingUID=");
        col.CellStyle = "width:150px";
        npoGridView.Columns.Add(col);

        if (HFD_Mode.Value == "Main")      //HFD_Mode.Value == "Main"  表示從清單來的是 主要人的uid
        {
            //  HFD_MemberID.Value -->主要人的 uid
            //HFD_PeopleUID.Value 從選取清單來的 member uid 
            HFD_MemberID.Value = HFD_PeopleUID.Value;  //HFD_PeopleUID.Value 從選取清單來的 member uid 
            lblGridList.Text = npoGridView.Render();   //主要人的 GridView
        }
        else
        {
            //   HFD_Merger.Value -->要被合併的uid 就是要被取代，合併會消失的人        
            //HFD_PeopleUID.Value 從選取清單來的 member uid 
            HFD_Merger.Value = HFD_PeopleUID.Value;
            lblPeople.Text = npoGridView.Render(); //要被合併的 GridView
        }
        //********************************************************************************************
    }

    //-------------------------------------------------------------------------------------------------------------
    protected void btnReload_Click(object sender, EventArgs e)
    {
        if (HFD_PeopleUID.Value != "")
        {
            LoadFormData2();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        List<ColumnData> list = new List<ColumnData>();
        string strSql = "";
        SetMergerColumnData(list);
        strSql = Util.CreateInsertCommand("Merger", list, dict);
        NpoDB.ExecuteSQLS(strSql, dict);
      

        objNpoDB.BeginTrans();
        try
        {
           
           //1. 刪除要被合併的人
            strSql = "UPDATE Member SET IsDelete  = 'Y' WHERE uid IN (" + HFD_Merger.Value  + ") ";
            objNpoDB.ExecuteSQL(strSql);
           //2.在諮詢資料將已被合併人的memberuid 改成 主要人的 memberuid
            strSql = "UPDATE Consulting SET MemberUID = " + HFD_MemberID.Value  + " WHERE MemberUID IN (" + HFD_Merger.Value + ")";
            objNpoDB.ExecuteSQL(strSql);
         
            objNpoDB.CommitTrans();
        }
        catch (Exception ex)
        {
            objNpoDB.RollbackTrans();
            throw ex;
        }
        ShowSysMsg("合併資料成功");
    }


    private void SetMergerColumnData(List<ColumnData> list)
    {
    
        //主要人員編號
        list.Add(new ColumnData("MasterUid", HFD_MemberID.Value, true, true, false));
        //要被合併的人
        list.Add(new ColumnData("OldUid", HFD_Merger.Value, true, true, false));
        //志工
        list.Add(new ColumnData("UserID", SessionInfo.UserID , true, true, false));

        //合併前的對話紀錄
        if (HFD_Merger.Value != "")
        {
            list.Add(new ColumnData("ConsultingID", getConsultingID(), true, true, false));
        }
       
        //合併日期
        list.Add(new ColumnData("MergerDate", Util.GetToday(DateType.yyyyMMddHHmmss), true, true, false));
        list.Add(new ColumnData("InsertDate", Util.GetToday(DateType.yyyyMMddHHmmss), true, true, false));
        list.Add(new ColumnData("InsertID", SessionInfo.UserID, true, true, false));
    }


    public string   getConsultingID()
    {
        //讀取資料庫
        string strSql = @"
                    SELECT TOP 1 (SELECT (CONVERT(NVARCHAR(10),uid )  + ',') 
                    FROM Consulting WHERE Memberuid =@Memberuid   GROUP BY uid FOR XML PATH('') ) AS uid 
                    FROM Consulting
                    ";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("Memberuid", HFD_Merger.Value);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        DataRow dr = dt.Rows[0];
      
        if (dt.Rows.Count == 0)
        {
            return "";
        }
        return dr["uid"].ToString(); ;
    }

}
