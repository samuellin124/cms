using System;
using System.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
public partial class SysMgr_Church : BasePage
{
    #region NpoGridView 處理換頁相關程式碼
    Button btnNextPage, btnPreviousPage, btnGoPage;
    HiddenField HFD_CurrentPage;

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
        Session["ProgID"] = "Church";
        //權控處理
        AuthrityControl();

        //有 npoGridView 時才需要
        Util.ShowSysMsgWithScript("$('#btnPreviousPage').hide();$('#btnNextPage').hide();$('#btnGoPage').hide();");

        if (!IsPostBack)
        {
            LoadFormData(); 
            LoadDropDownListData();

        }
    }
    //------------------------------------------------------------------------------
    public void AuthrityControl()
    {
        if (Authrity.PageRight("_Focus") == false)
        {
            return;
        }
        Authrity.CheckButtonRight("_AddNew", btnAdd);
        Authrity.CheckButtonRight("_Query", btnQuery);
    }
    //----------------------------------------------------------------------
    public void LoadFormData()
    {

        //呼叫資料庫GetDataTable
        DataTable dt = GetDataTable();


        //Grid initial
        NPOGridView npoGridView = new NPOGridView();
        npoGridView.Source = NPOGridViewDataSource.fromDataTable;
        npoGridView.dataTable = dt;
        npoGridView.Keys.Add("uid");
        npoGridView.DisableColumn.Add("uid");
        npoGridView.DisableColumn.Add("教會Email");
        npoGridView.DisableColumn.Add("主任牧師Email");

        npoGridView.CurrentPage = Util.String2Number(HFD_CurrentPage.Value);
        npoGridView.EditLink = Util.RedirectByTime("Church_Edit.aspx", "ChurchUID=");
        lblGridList.Text = npoGridView.Render();
    }
    //-------------------------------------------------------------------------
    public void LoadDropDownListData()
    {
        Util.FillCityData(ddlCity);
        Util.FillAreaData(ddlArea, ddlCity.SelectedValue);
    }
    //------------------------------------------------------------------------------
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect(Util.RedirectByTime("Church_Edit.aspx?Mode=ADD&"));
    }
    //------------------------------------------------------------------------------
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        LoadFormData();
    }
    //------------------------------------------------------------------------------
    protected void btnPrint_Click(object sender, EventArgs e)
    {

        lblRpt.Text = GetRptHead() + GetReport();
        string s = @"<head>
                        <style>
                        @page Section2
                        { size:841.7pt 595.45pt;
                          mso-page-orientation:landscape;
                        margin:1.5cm 0.5cm 0.5cm 0.5cm;}
                        div.Section2
                        {page:Section2;}
                        </style>
                    </head>
                    <body>
                        <div class=Section2>
                    ";//邊界-上右下左
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(s);
        Util.OutputTxt(sb + lblRpt.Text, "1", DateTime.Now.ToString("yyyyMMddHHmmss"));
    }
    //----------------------------------------------------------------------------------------------------------
    //報表抬頭
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
        strTemp = "GOOD TV 夥伴教會資料";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        //教會跨行置中
        cell.ColSpan = 11;
        css.Add("text-align", "center");
        css.Add("font-size", "24px");
        css.Add("font-family", "標楷體");
        css.Add("border-style", "none");
        css.Add("font-weight", "bold");
        row.Cells.Add(cell);
        table.Rows.Add(row);
        //*************************************
        //轉成 html 碼
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        table.RenderControl(htw);

        return htw.InnerWriter.ToString();
    }
    private string GetReport()
    {
        string[] Items = {
                        "教會名稱",
                        //"縣市",
                        //"區域",
                        "教會地址",
                        "教會電話",
                        "教會Email",
                        "主任牧師",
                        "主任牧師電話",
                        "主任牧師Email",
                        "教會事工",
                        "聯絡窗口",
                        "聯絡窗口手機",
                        "聯絡窗口Email",
                        
                   };
        //組 table
        string strTemp = "";
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
        css.Add("font-size", "12pt");
        css.Add("font-family", "標楷體");
        css.Add("width", "800px");

        //第1行
        row = new HtmlTableRow();

        for (int i = 0; i < Items.Length; i++)
        {

            cell = new HtmlTableCell();
            strTemp = Items[i].ToString();
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "25px");  //抬頭字型大小
            if (i == 0)
            {
                Util.AddTDLine(css, 123);
            }
            if (i == 1)
            {
                Util.AddTDLine(css, 23);
            }
            if (i == 2)
            {
                Util.AddTDLine(css, 23);
            }
            if (i == 3)
            {
                Util.AddTDLine(css, 23);
            }
            if (i == 4)
            {
                Util.AddTDLine(css, 23);
            }
            if (i == 5)
            {
                Util.AddTDLine(css, 23);
            }
            if (i == 6)
            {
                Util.AddTDLine(css, 23);
            }
            if (i == 7)
            {
                Util.AddTDLine(css, 23);
            }
            if (i == 8)
            {
                Util.AddTDLine(css, 23);
            }
            if (i == 9)
            {
                Util.AddTDLine(css, 23);
            }
            if (i == 10)
            {
                Util.AddTDLine(css, 23);
            }
            //if (i == 11)
            //{
            //    Util.AddTDLine(css, 23);
            //}
            //if (i == 12)
            //{
            //    Util.AddTDLine(css, 23);
            //}


            row.Cells.Add(cell);

        }

        cell = new HtmlTableCell();
        string strTemp1 = "";
        cell.InnerHtml = strTemp1;
        css = cell.Style;
        css.Add("text-align", "left");
        css.Add("font-size", "12px");
        cell.Width = "8";
        row.Cells.Add(cell);
        table.Rows.Add(row);
        //--------------------------------------------
        //第2行,資料顯示
        //*********第一排*********
        string strFontSize = "18px";
        DataTable dt = GetDataTable();

        foreach (DataRow dr in dt.Rows)
        {
            row = new HtmlTableRow();

            cell = new HtmlTableCell();
            css = cell.Style;
            cell.InnerHtml = dr["教會名稱"].ToString();     //從GetDataTable()抓Name值
            css.Add("text-align", "center");            //excel文字置中
            css.Add("font-size", strFontSize);          //excel文字大小
            Util.AddTDLine(css, 1234);                  //excel 一個儲存格左(1)上(2)右(3)下(4) 框線
            row.Cells.Add(cell);

            //cell = new HtmlTableCell();
            //css = cell.Style;
            //cell.InnerHtml = dr["縣市"].ToString();
            //css.Add("text-align", "center");
            //css.Add("font-size", strFontSize);
            //Util.AddTDLine(css, 1234);
            //row.Cells.Add(cell);

            //cell = new HtmlTableCell();
            //css = cell.Style;
            //cell.InnerHtml = dr["區域"].ToString();
            //css.Add("text-align", "center");
            //css.Add("font-size", strFontSize);
            //Util.AddTDLine(css, 1234);
            //row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            cell.InnerHtml = dr["教會地址"].ToString();
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 1234);
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            cell.InnerHtml = dr["教會電話"].ToString();
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            css.Add("mso-number-format", "\\@");
            Util.AddTDLine(css, 1234);
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            cell.InnerHtml = dr["教會Email"].ToString();
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 1234);
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            cell.InnerHtml = dr["主任牧師"].ToString();
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 1234);
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            cell.InnerHtml = dr["主任牧師電話"].ToString();
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            css.Add("mso-number-format", "\\@");
            Util.AddTDLine(css, 1234);
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            cell.InnerHtml = dr["主任牧師Email"].ToString();
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 1234);
            row.Cells.Add(cell);

            string Churchministry = "";
            if (dr["教會事工"].ToString() != "")
            {
                string tmp = dr["教會事工"].ToString();
                string[] split = tmp.Split(new Char[] { ',' });
                split[0].ToString();
                split[1].ToString();
                split[2].ToString();
                split[3].ToString();
                split[4].ToString();
                split[5].ToString();
                split[6].ToString();
                if (split[0] == "婚前")
                    Churchministry = "婚前,";
               // Churchministry = ",";
                if (split[1] == "婚姻")
                    Churchministry += "婚姻,";
                if (split[2] == "青少")
                    Churchministry = "青少,";
                if (split[3] == "老人")
                    Churchministry += "老人,";
                if (split[4] == "家庭關懷")
                    Churchministry += "家庭關懷,";
                if (split[5] == "協談輔導")
                    Churchministry += "協談輔導,";
                if (split[6] == "其他")
                    Churchministry += "其他";
            }
            cell = new HtmlTableCell();
            css = cell.Style;
            cell.InnerHtml = Churchministry;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 1234);
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            cell.InnerHtml = dr["聯絡窗口"].ToString();
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 1234);
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            cell.InnerHtml = dr["窗口電話"].ToString();
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            css.Add("mso-number-format", "\\@");
            Util.AddTDLine(css, 1234);
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            cell.InnerHtml = dr["窗口Email"].ToString();
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 1234);
            row.Cells.Add(cell);

            table.Rows.Add(row);
        }

        //轉成 html 碼
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        table.RenderControl(htw);
        return htw.InnerWriter.ToString();
    }

    private DataTable GetDataTable()
    {


        //讀取資料庫
        string strSql = @"
                        select c.uid , c.Name as 教會名稱 ,(select  name from codecity where ZipCode=cc1.ZipCode ) +CC.Name+c.Address as 教會地址 ,
                        c.ChurchPhone as 教會電話,c.ChurchEmail2 as 教會Email, c.Priest as 主任牧師 ,
                        c.PriestPhone as 主任牧師電話,c.PriestEmail as 主任牧師Email,c.Churchministry as 教會事工,
                        c.Contact as 聯絡窗口 , c.ChurchMphone as 窗口電話 ,
                        c.ChurchEmail as 窗口Email
                        from Church as c
                        inner join codecity CC on CC.ZipCode = Area
                        inner join codecity CC1 on CC1.ZipCode = City
                        where 1=1 and isnull(IsDelete, '') != 'Y'  
                        ";
        if (txtName.Text != "")
        {
            strSql += "and c.Name like @Name\n";
        }
        if (txtContact.Text != "")
        {
            strSql += "and c.Contact like @Contact\n";
        }
        if (txtPriest.Text != "")
        {
            strSql += "and c.Priest like @Priest\n";
        }
        if (ddlCity.SelectedValue != "")
        {
            strSql += " and City = @City\n";
        }
        if (HFD_Area.Value != "")
        {
            strSql += " and Area = @Area\n";
        }
        if (txtBegCreateDate.Text.Trim() != "" && txtEndCreateDate.Text.Trim() != "")
        {
            strSql += " and CONVERT(varchar(100), InsertDate, 111) Between @BegCreateDate And @EndCreateDate\n";
        }
        //strSql += "GROUP BY cc.zipcode,City,CC.Name,c.uid";
        strSql += "order BY City,cc.zipcode,CC.Name";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("Name", "%" + txtName.Text + "%");
        dict.Add("Contact", "%" + txtContact.Text + "%");
        dict.Add("Priest", "%" + txtPriest.Text + "%");
        dict.Add("City",  ddlCity.SelectedValue );
        dict.Add("Area",  HFD_Area.Value );
        dict.Add("BegCreateDate", txtBegCreateDate.Text);
        dict.Add("EndCreateDate", txtEndCreateDate.Text);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        return dt;
    }


}