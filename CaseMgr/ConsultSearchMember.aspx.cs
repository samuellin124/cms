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

public partial class CaseMgr_ConsultSearchMember : BasePage 
                     
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
        HFD_EditServiceUser.Value = SessionInfo.UserID.ToString ();
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
        Util.FillCityData(ddlCity);
        Util.FillAreaData(ddlArea, ddlCity.SelectedValue);
     }
    //-------------------------------------------------------------------------------------------------------------
    private void LoadFormData()
    {
      
            string strSql = @"
                            select distinct a.uid , a.CName AS 姓名 ,a.Phone AS 電話
                            from Member a  
                            inner join Consulting b on  a.uid = b.MemberUID
                            where 1=1 and isnull(a.IsDelete, '') != 'Y'  
                          ";
    
        
        if (txtServiceUser.Text.Trim()  != "")
        {
            strSql += " and ServiceUser = @ServiceUser\n";
        }

        if (HFD_Marry.Value != "")
        {
            strSql += " and Marry like @Marry\n";
        }

        if (txtEvent.Text.Trim() != "")
        {
            strSql += " and Event like @Event\n";
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

        if (txtZipCode.Text.Trim() != "")
        {
            strSql += " And ZipCode=@ZipCode\n";
        }

        if (ddlCity.SelectedValue != "")
        {
            strSql += " And City=@City\n";
        }

        if (txtAddress.Text.Trim() != "")
        {
            strSql += " and Address like @Address\n";
        }

        if (HFD_ConsultantItem.Value != "")
        {
         strSql += " And ConsultantItem like @ConsultantItem\n";
        }

        strSql += " Order by CName desc";
        
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("Event", "%" + txtEvent.Text.Trim() + "%");
        dict.Add("CName", "%" + txtName.Text.Trim() + "%");
        dict.Add("Marry", "%" + HFD_Marry.Value + "%");
        dict.Add("Phone", '%' + txtPhone.Text.Trim() + '%');
        dict.Add("BegCreateDate", txtBegCreateDate.Text.Trim());
        dict.Add("EndCreateDate", txtEndCreateDate.Text.Trim());
        dict.Add("ServiceUser", txtServiceUser.Text.Trim());
        //dict.Add("EditServiceUser", SessionInfo.UserID.ToString());
        dict.Add("ZipCode", txtZipCode.Text.Trim());
        dict.Add("City", ddlCity.SelectedValue);
        dict.Add("Address", "%" + txtAddress.Text.Trim() + "%");
        dict.Add("ConsultantItem", "%" + HFD_ConsultantItem.Value + "%");
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        
        //int count = dt.Rows.Count;
     
        HFD_MemberID.Value = "";
        if (dt.Rows.Count == 0)
        {
            lblGridList.Text = "";
            ShowSysMsg  ("查無資料");
            return;
        }
        else
        {
            //int i = dt.Rows.Count;
            //for (i = 0; i <= dt.Rows.Count - 1; i++)
            //{
            //    HFD_MemberID.Value = HFD_MemberID.Value + dt.Rows[i]["memberuid"].ToString() + ",";
            //}
        }
       // dr = dt.Rows[0];

        //lblReport.Text = GetRptHead() + GetReport();   //報表資料
        //Grid initial
        //沒有需要特別處理的欄位時
        NPOGridView npoGridView = new NPOGridView();
        npoGridView.Source = NPOGridViewDataSource.fromDataTable;
        npoGridView.dataTable = dt;
        npoGridView.Keys.Add("uid");
        npoGridView.DisableColumn.Add("uid");
        npoGridView.CurrentPage = Util.String2Number(HFD_CurrentPage.Value);
        //Response.Write(@"<script>window.parent.location.href='ConsultEdit2.aspx';</script>");
        //npoGridView.EditLinkTarget = "','target=_blank,help=no,status=yes,resizable=yes,scrollbars=yes,scrolling=yes,scroll=yes,center=yes,width=900px,height=500px";
        npoGridView.EditLink = Util.RedirectByTime("QryReport.aspx", "uid=");
        lblGridList.Text = npoGridView.Render();
        //if (Util.GetQueryString("menu_id") == "48")
        //{
        //    lblGridList.Text = npoGridView.Render();
        //}
        //else
        //{
        //    lblReport.Text = GetRptHead() + GetReport();   //報表資料
        //}
        
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
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect(Util.RedirectByTime("ConsultEdit2.aspx?Mode=NEW"));
    }

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
        lblConsultantItem.Text = HtmlUtil.RenderControl(list);
        //婚姻狀況
        list = new List<ControlData>();
        list.Add(new ControlData("Checkbox", "Marry", "CHK_Marry1", "%未婚%", "未婚", false, HFD_Marry.Value));
        list.Add(new ControlData("Checkbox", "Marry", "CHK_Marry2", "%已婚%", "已婚", false, HFD_Marry.Value));
        list.Add(new ControlData("Checkbox", "Marry", "CHK_Marry3", "%離婚%", "離婚", false, HFD_Marry.Value));
        list.Add(new ControlData("Checkbox", "Marry", "CHK_Marry4", "%喪偶%", "喪偶", false, HFD_Marry.Value));
        lblMarry.Text = HtmlUtil.RenderControl(list);
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
    private DataTable GetDataTable()
    {
        string strSql = @"
                  select (c.Name + d.Name + b.Address) FullAddress ,*
                        from Consulting a  
                        inner join Member b on a.MemberUID = b.uid
                        left join CodeCity c on b.City = c.ZipCode 
                        left join CodeCity d on b.ZipCode = d.ZipCode     
                  where 1=1 and isnull(a.IsDelete, '') != 'Y' ";
        strSql += "  And a.MemberUID in  (" + HFD_MemberID.Value.ToString().TrimEnd(',') + ")";
            
                  
        //if (Util.GetQueryString("menu_id") == "48")   //menu_id = 48(對應 Admin_menu) 表示事要修改資料的 所以在查詢時 只能查到自己的資料
        //{
        //    strSql += " and ServiceUser = @EditServiceUser\n";
        //}
        //if (txtServiceUser.Text != "")
        //{
        //    strSql += " and ServiceUser = @ServiceUser\n";
        //}

        //if (HFD_Marry.Value != "")
        //{
        //    strSql += " and Marry like @Marry\n";
        //}

        //if (txtEvent.Text != "")
        //{
        //    strSql += " and Event like @Event\n";
        //}

        //if (txtPhone.Text != "")
        //{
        //    strSql += " and Phone like @Phone\n";
        //}

        //if (txtName.Text != "")
        //{
        //    strSql += " and CName like @CName\n";
        //}

        //if (txtBegCreateDate.Text != "" && txtEndCreateDate.Text != "")
        //{
        //    strSql += " and CONVERT(varchar(100), CreateDate, 111) Between @BegCreateDate And @EndCreateDate\n";
        //}

        //if (txtZipCode.Text != "")
        //{
        //    strSql += " And b.ZipCode=@ZipCode\n";
        //}

        //if (ddlCity.SelectedValue != "")
        //{
        //    strSql += " And City=@City\n";
        //}

        //if (txtAddress.Text != "")
        //{
        //    strSql += " and Address like @Address\n";
        //}

        //if (HFD_ConsultantItem.Value != "")
        //{
        //    strSql += " And ConsultantItem like @ConsultantItem\n";
        //}

        strSql += " Order by  CreateDate Desc ";
      
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("MemberUID", HFD_MemberID.Value.ToString().TrimEnd(','));
        dict.Add("Event", "%" + txtEvent.Text + "%");
        dict.Add("CName", "%" + txtName.Text + "%");
        dict.Add("Marry","%" + HFD_Marry.Value + "%");
        dict.Add("Phone", '%' + txtPhone.Text + '%');
        dict.Add("BegCreateDate", txtBegCreateDate.Text);
        dict.Add("EndCreateDate", txtEndCreateDate.Text);
        dict.Add("ServiceUser", txtServiceUser.Text);
        dict.Add("EditServiceUser", SessionInfo.UserID.ToString());
        dict.Add("ZipCode", txtZipCode.Text);
        dict.Add("City", ddlCity.SelectedValue);
        dict.Add("Address", "%" + txtAddress.Text + "%");
        dict.Add("ConsultantItem", "%" + HFD_ConsultantItem.Value.ToString().TrimEnd(',') + "%");
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        
        return dt;
    }

    // 報表************************************************************************************************************************
    private string GetReport()
    {
        HtmlTable table = new HtmlTable();
        HtmlTableRow row;
        HtmlTableCell cell;
        CssStyleCollection css;
        table.Border = 0;
        table.BorderColor = "black";
        table.CellPadding = 0;
        table.CellSpacing = 0;
        table.Align = "center";
        css = table.Style;
        css.Add("font-size", "32pt");
        css.Add("font-family", "標楷體");
        css.Add("width", "900px");
        //--------------------------------------------
        DataTable dt = GetDataTable();
        foreach (DataRow dr in dt.Rows)
        {
            string strFontSize = "18px";
            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 123);
            cell.InnerHtml = "1.電話：" + dr["Phone"].ToString();
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 23);
            cell.InnerHtml = "4.年齡約：" + dr["age"].ToString() + "歲";
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 23);
            cell.InnerHtml = "7.轉介教會：" + dr["ChurchYN"].ToString();
            row.Cells.Add(cell);
            table.Rows.Add(row);


            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 123);
            cell.InnerHtml = "2.姓名：" + dr["Cname"].ToString();
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 23);
            cell.InnerHtml = "5.婚姻：" + Util.TrimLastChar(dr["Marry"].ToString(), ',') + "&nbsp;";
            row.Cells.Add(cell);


            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 23);
            cell.RowSpan = 2;
            //cell.ColSpan = 2;
            cell.InnerHtml = "8.地址：" + dr["FullAddress"].ToString();
            row.Cells.Add(cell);
            table.Rows.Add(row);



            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 123);
            cell.InnerHtml = "3.性別：" + dr["Sex"].ToString();
            row.Cells.Add(cell);


            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 23);
            cell.InnerHtml = "6.基督徒：" + dr["ChurchYN"].ToString();
            row.Cells.Add(cell);
            table.Rows.Add(row);

           



            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 123);
            cell.ColSpan = 2;
            cell.InnerHtml = "志工：" + dr["ServiceUser"].ToString();
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 23);
            cell.ColSpan = 2;
            cell.InnerHtml = "建檔時間：" + Util.DateTime2String(dr["CreateDate"].ToString(), DateType.yyyyMMddHHmmss, EmptyType.ReturnNull) + "&nbsp;結束時間：" + Util.DateTime2String(dr["EndDate"].ToString(), DateType.yyyyMMddHHmmss, EmptyType.ReturnNull);
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "26px");
            Util.AddTDLine(css, 123);
            cell.RowSpan = 6;
            cell.InnerHtml = "S";
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 23);
            cell.InnerHtml = "求助者的主訴(用第一人稱 '我' 敘述)";
            cell.ColSpan = 3;
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 23);
            cell.ColSpan = 3;
            cell.InnerHtml = "A(事件)：" + dr["Event"].ToString() + "&nbsp;";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 23);
            cell.InnerHtml = "B(想法)：" + dr["Think"].ToString() + "&nbsp;";
            cell.ColSpan = 3;
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 23);
            cell.InnerHtml = "C(感受)：" + dr["Feel"].ToString() + "&nbsp;";
            cell.ColSpan = 3;
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 23);
            cell.ColSpan = 3;
            cell.InnerHtml = "補充說明：" + dr["Comment"].ToString() + "&nbsp;";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 23);
            cell.ColSpan = 3;
            cell.InnerHtml = "&nbsp;";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "26px");
            Util.AddTDLine(css, 123);
            cell.RowSpan = 6;
            cell.InnerHtml = "O";
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 23);
            cell.ColSpan = 3;
            cell.InnerHtml = "客觀分析(諮商類別)";
            row.Cells.Add(cell);
            table.Rows.Add(row);


            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 23);
            cell.ColSpan = 3;
            cell.InnerHtml = "來電諮詢別(大類)：" + Util.TrimLastChar(dr["ConsultantMain"].ToString(), ',') + "&nbsp;";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 23);
            cell.ColSpan = 3;
            cell.InnerHtml = "來電諮詢別(分項)：" + Util.TrimLastChar(dr["ConsultantItem"].ToString(), ',') + "&nbsp;";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 23);
            cell.ColSpan = 3;
            if (dr["HarassOther"].ToString() != "")
            {
                cell.InnerHtml = "騷擾電話：" + Util.TrimLastChar(dr["HarassPhone"].ToString().Replace("其他", ""), ',') + "其他" + dr["HarassOther"].ToString() + "&nbsp;";
            }
            else
            {
                cell.InnerHtml = "騷擾電話：" + Util.TrimLastChar(dr["HarassPhone"].ToString(), ',') + "&nbsp;";
            }

            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 23);
            cell.ColSpan = 3;
            cell.InnerHtml = "轉介單位(告知電話)：" + Util.TrimLastChar(dr["IntroductionUnit"].ToString(), ',') + "&nbsp;";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 23);
            cell.ColSpan = 3;
            cell.InnerHtml = "緊急轉介(撥打電話)：" + Util.TrimLastChar(dr["CrashIntroduction"].ToString(), ',') + "&nbsp;";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "26px");
            Util.AddTDLine(css, 123);
            cell.RowSpan = 4;
            cell.InnerHtml = "A";
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 23);
            cell.ColSpan = 3;
            cell.InnerHtml = "問題評估";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 23);
            cell.ColSpan = 3;
            cell.InnerHtml = "1.因" + dr["Reason1"].ToString() + "造成" + dr["Trouble1"].ToString() + "問題";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 23);
            cell.ColSpan = 3;
            cell.InnerHtml = "2.因" + dr["Reason2"].ToString() + "造成" + dr["Trouble2"].ToString() + "問題";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 23);
            cell.ColSpan = 3;
            cell.InnerHtml = "3.因" + dr["Reason3"].ToString() + "造成" + dr["Trouble3"].ToString() + "問題";
            row.Cells.Add(cell);
            table.Rows.Add(row);


            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "26px");
            Util.AddTDLine(css, 1234);
            cell.RowSpan = 8;
            cell.InnerHtml = "P";
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 23);
            cell.ColSpan = 3;
            cell.InnerHtml = "計畫";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 23);
            cell.ColSpan = 3;
            cell.InnerHtml = "1.找出案主心中的假設：" + dr["FindAssume"].ToString() + "&nbsp;";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 23);
            cell.ColSpan = 3;
            cell.InnerHtml = "2.與案主討論與解釋假設： " + dr["Discuss"].ToString() + "&nbsp;";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 23);
            cell.ColSpan = 3;
            cell.InnerHtml = "3.加入新的元素：" + dr["Element"].ToString() + "&nbsp;";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 23);
            cell.ColSpan = 3;
            cell.InnerHtml = "4.改變案主原先的期待： " + dr["Expect"].ToString() + "&nbsp;";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 23);
            cell.InnerHtml = "5.給予案主祝福與盼望： " + dr["Blessing"].ToString() + "&nbsp;";
            cell.ColSpan = 3;
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 23);
            cell.ColSpan = 3;
            cell.InnerHtml = "6.帶領決志禱告：" + dr["IntroductionChurch"].ToString() + "&nbsp;";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 234);
            cell.ColSpan = 3;
            cell.InnerHtml = "7.我對個案的幫助程度：" + dr["HelpLvMark"].ToString() + "&nbsp;";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 134);
            cell.ColSpan = 6;
            cell.InnerHtml = "小叮嚀：" + dr["Memo"].ToString() + "&nbsp;";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            cell = new HtmlTableCell();
            row = new HtmlTableRow();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            cell.InnerHtml = "&nbsp; <BR>";
            row.Cells.Add(cell);
            table.Rows.Add(row);
        }
        //轉成 html 碼========================================================================
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        table.RenderControl(htw);
        return htw.InnerWriter.ToString();
    }
    //-------------------------------------------------------------------------------------------------------------
}
