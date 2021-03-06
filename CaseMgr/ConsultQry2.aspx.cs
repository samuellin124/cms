﻿using System;
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

public partial class CaseMgr_ConsultQry2 : BasePage
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
        Session["ProgID"] = "Consult";
        HFD_EditServiceUser.Value = SessionInfo.UserID.ToString();
      
            litTitle.Text = "修改作業";


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
        LoadHiddenData();
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
        //btnAdd.Visible = ga.AddNew;
        btnQuery.Visible = ga.Query;
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
                        select a.uid as uid, CName As  姓名,b.Memo as [備註],Phone As 電話, convert(varchar(19), CreateDate,120) as 建檔時間, 
                         REVERSE(SUBSTRING(REVERSE(ConsultantMain),CHARINDEX(',',REVERSE(ConsultantMain))+1,LEN(ConsultantMain))) [來電諮詢別(大類)],
                         REVERSE(SUBSTRING(REVERSE(ConsultantItem),CHARINDEX(',',REVERSE(ConsultantItem))+1,LEN(ConsultantItem))) [來電諮詢別(分項)]
            --, ConsultantMain as [來電諮詢別(大類)], ConsultantItem  as [來電諮詢別(分項)] 
                        from Consulting a  inner join Member b on a.MemberUID = b.uid
                        where 1=1 
                        and isnull(a.IsDelete, '') != 'Y'
                    ";


        if (SessionInfo.GroupName == "系統管理員")
        {
        }
        else
        {
            strSql += " and ServiceUser = @ServiceUser\n";
        }
       
        

        if (txtPhone.Text.Trim() != "")
        {
            strSql += " and Phone like @Phone\n";
        }

        if (txtName.Text.Trim() != "")
        {
            strSql += " and CName like @CName\n";
        }

        if (txtBegCreateDate.Text.Trim() != "" && txtEndCreateDate.Text.Trim() != "")
        {
            strSql += " and CONVERT(varchar(100), CreateDate, 111) Between @BegCreateDate And @EndCreateDate\n";
        }
        strSql += " Order by CreateDate desc";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("CName", "%" + txtName.Text.Trim() + "%");
        dict.Add("Phone", '%' + txtPhone.Text.Trim() + '%');
        dict.Add("BegCreateDate", txtBegCreateDate.Text.Trim());
        dict.Add("EndCreateDate", txtEndCreateDate.Text.Trim());
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
            npoGridView.EditLinkTarget = "','target=_blank,help=no,status=yes,resizable=yes,scrollbars=yes,scrolling=yes,scroll=yes,center=yes,width=900px,height=500px";
            npoGridView.EditLink = Util.RedirectByTime("ConsultEdit2.aspx", "ConsultingUID=");
             
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
            col.Link = Util.RedirectByTime("ConsultEdit2.aspx", "ConsultingUID=");
            col.CellStyle = "width:200px";
            npoGridView.Columns.Add(col);
            //----------------------------------------------
            col = new NPOGridViewColumn("電話"); //產生所需欄位及設定 Title
            col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
            col.ColumnName.Add("電話"); //使用欄位名稱
            col.Link = Util.RedirectByTime("ConsultEdit2.aspx", "ConsultingUID=");
            col.CellStyle = "width:150px;word-break: break-all;";
            npoGridView.Columns.Add(col);
            // lblGridList.Text = npoGridView.Render();
            //----------------------------------------------------------
            col = new NPOGridViewColumn("建檔時間"); //產生所需欄位及設定 Title
            col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
            col.ColumnName.Add("建檔時間"); //使用欄位名稱
            col.Link = Util.RedirectByTime("ConsultEdit2.aspx", "ConsultingUID=");
            col.CellStyle = "width:150px";
            npoGridView.Columns.Add(col);
            //----------------------------------------------------------
            col = new NPOGridViewColumn("來電諮詢別(大類)"); //產生所需欄位及設定 Title
            col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
            col.ColumnName.Add("來電諮詢別(大類)"); //使用欄位名稱
            col.Link = Util.RedirectByTime("ConsultEdit2.aspx", "ConsultingUID=");
            col.CellStyle = "width:200px";
            npoGridView.Columns.Add(col);
            //----------------------------------------------------------
            col = new NPOGridViewColumn("來電諮詢別(分項)"); //產生所需欄位及設定 Title
            col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
            col.ColumnName.Add("來電諮詢別(分項)"); //使用欄位名稱
            col.Link = Util.RedirectByTime("ConsultEdit2.aspx", "ConsultingUID=");
            col.CellStyle = "width:200px";
            npoGridView.Columns.Add(col);
            lblGridList.Text = npoGridView.Render();
        //----------------------------------------------------------
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
  
    private void LoadHiddenData()
    {
        List<ControlData> list = new List<ControlData>();
        list = new List<ControlData>();
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem1", "%經濟%", "經濟", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem2", "%情緒%", "情緒", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem3", "%信仰%", "信仰", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem4", "%人際%", "人際", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem5", "%婆媳%", "婆媳", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem6", "%翁媳%", "翁媳", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem7", "%妯娌%", "妯娌", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem8", "%手足%", "手足", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem9", "%職場%", "職場", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem10", "%教會%", "教會", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem11", "%溝通%", "溝通", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem12", "%性生活%", "性生活", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem13", "%精神疾病%", "精神疾病", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem14", "%身體疾病%", "身體疾病", true, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem15", "%個性%", "個性", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem16", "%外遇%", "外遇", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem17", "%暴力%", "暴力", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem18", "%教育%", "教育", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem19", "%霸凌%", "霸凌", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem20", "%學業%", "學業", false, HFD_ConsultantItem.Value));
        //lblConsultantItem.Text = HtmlUtil.RenderControl(list);
        //婚姻狀況
        list = new List<ControlData>();
        list.Add(new ControlData("Checkbox", "Marry", "CHK_Marry1", "%未婚%", "未婚", false, HFD_Marry.Value));
        list.Add(new ControlData("Checkbox", "Marry", "CHK_Marry2", "%已婚%", "已婚", false, HFD_Marry.Value));
        list.Add(new ControlData("Checkbox", "Marry", "CHK_Marry3", "%離婚%", "離婚", false, HFD_Marry.Value));
        list.Add(new ControlData("Checkbox", "Marry", "CHK_Marry4", "%喪偶%", "喪偶", false, HFD_Marry.Value));
        //lblMarry.Text = HtmlUtil.RenderControl(list);
    }

    private string GetRptHead()
    {
        string strTemp = "";
        HtmlTable table = new HtmlTable();
        HtmlTableRow row;
        HtmlTableCell cell;
        CssStyleCollection css;

        string FontSize = "14px";
        table.Border = 0;
        table.CellPadding = 0;
        table.CellSpacing = 0;
        table.Align = "center";
        css = table.Style;
        css.Add("font-size", "12px");
        css.Add("font-family", "標楷體");
        css.Add("width", "100%");
        //****************************************
        row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "&nbsp;";// "" + ddlYear.SelectedValue + "年熱線志工班表  第 " + ddlQuerter.SelectedValue + " 季 ";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "24px");
        css.Add("font-family", "標楷體");
        css.Add("border-style", "none");
        css.Add("font-weight", "bold");
        cell.ColSpan = 8;
        row.Cells.Add(cell);
        table.Rows.Add(row);
        //*************************************
        //轉成 html 碼
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        table.RenderControl(htw);
        return htw.InnerWriter.ToString();
    }
    //----------------------------------------------------------------------------------------------------------
    

    
    //-------------------------------------------------------------------------------------------------------------
}
