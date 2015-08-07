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
    #region NpoGridView �B�z���������{���X
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
    #endregion NpoGridView �B�z���������{���X
    //---------------------------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["ProgID"] = "Church";
        //�v���B�z
        AuthrityControl();

        //�� npoGridView �ɤ~�ݭn
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

        //�I�s��ƮwGetDataTable
        DataTable dt = GetDataTable();


        //Grid initial
        NPOGridView npoGridView = new NPOGridView();
        npoGridView.Source = NPOGridViewDataSource.fromDataTable;
        npoGridView.dataTable = dt;
        npoGridView.Keys.Add("uid");
        npoGridView.DisableColumn.Add("uid");
        npoGridView.DisableColumn.Add("�з|Email");
        npoGridView.DisableColumn.Add("�D�����vEmail");

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
                    ";//���-�W�k�U��
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(s);
        Util.OutputTxt(sb + lblRpt.Text, "1", DateTime.Now.ToString("yyyyMMddHHmmss"));
    }
    //----------------------------------------------------------------------------------------------------------
    //������Y
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
        css.Add("font-family", "�з���");
        css.Add("width", "100%");
        //****************************************
        row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "GOOD TV �٦�з|���";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        //�з|���m��
        cell.ColSpan = 11;
        css.Add("text-align", "center");
        css.Add("font-size", "24px");
        css.Add("font-family", "�з���");
        css.Add("border-style", "none");
        css.Add("font-weight", "bold");
        row.Cells.Add(cell);
        table.Rows.Add(row);
        //*************************************
        //�ন html �X
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        table.RenderControl(htw);

        return htw.InnerWriter.ToString();
    }
    private string GetReport()
    {
        string[] Items = {
                        "�з|�W��",
                        //"����",
                        //"�ϰ�",
                        "�з|�a�}",
                        "�з|�q��",
                        "�з|Email",
                        "�D�����v",
                        "�D�����v�q��",
                        "�D�����vEmail",
                        "�з|�Ƥu",
                        "�p�����f",
                        "�p�����f���",
                        "�p�����fEmail",
                        
                   };
        //�� table
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
        css.Add("font-family", "�з���");
        css.Add("width", "800px");

        //��1��
        row = new HtmlTableRow();

        for (int i = 0; i < Items.Length; i++)
        {

            cell = new HtmlTableCell();
            strTemp = Items[i].ToString();
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "25px");  //���Y�r���j�p
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
        //��2��,������
        //*********�Ĥ@��*********
        string strFontSize = "18px";
        DataTable dt = GetDataTable();

        foreach (DataRow dr in dt.Rows)
        {
            row = new HtmlTableRow();

            cell = new HtmlTableCell();
            css = cell.Style;
            cell.InnerHtml = dr["�з|�W��"].ToString();     //�qGetDataTable()��Name��
            css.Add("text-align", "center");            //excel��r�m��
            css.Add("font-size", strFontSize);          //excel��r�j�p
            Util.AddTDLine(css, 1234);                  //excel �@���x�s�楪(1)�W(2)�k(3)�U(4) �ؽu
            row.Cells.Add(cell);

            //cell = new HtmlTableCell();
            //css = cell.Style;
            //cell.InnerHtml = dr["����"].ToString();
            //css.Add("text-align", "center");
            //css.Add("font-size", strFontSize);
            //Util.AddTDLine(css, 1234);
            //row.Cells.Add(cell);

            //cell = new HtmlTableCell();
            //css = cell.Style;
            //cell.InnerHtml = dr["�ϰ�"].ToString();
            //css.Add("text-align", "center");
            //css.Add("font-size", strFontSize);
            //Util.AddTDLine(css, 1234);
            //row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            cell.InnerHtml = dr["�з|�a�}"].ToString();
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 1234);
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            cell.InnerHtml = dr["�з|�q��"].ToString();
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            css.Add("mso-number-format", "\\@");
            Util.AddTDLine(css, 1234);
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            cell.InnerHtml = dr["�з|Email"].ToString();
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 1234);
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            cell.InnerHtml = dr["�D�����v"].ToString();
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 1234);
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            cell.InnerHtml = dr["�D�����v�q��"].ToString();
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            css.Add("mso-number-format", "\\@");
            Util.AddTDLine(css, 1234);
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            cell.InnerHtml = dr["�D�����vEmail"].ToString();
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 1234);
            row.Cells.Add(cell);

            string Churchministry = "";
            if (dr["�з|�Ƥu"].ToString() != "")
            {
                string tmp = dr["�з|�Ƥu"].ToString();
                string[] split = tmp.Split(new Char[] { ',' });
                split[0].ToString();
                split[1].ToString();
                split[2].ToString();
                split[3].ToString();
                split[4].ToString();
                split[5].ToString();
                split[6].ToString();
                if (split[0] == "�B�e")
                    Churchministry = "�B�e,";
               // Churchministry = ",";
                if (split[1] == "�B��")
                    Churchministry += "�B��,";
                if (split[2] == "�C��")
                    Churchministry = "�C��,";
                if (split[3] == "�ѤH")
                    Churchministry += "�ѤH,";
                if (split[4] == "�a�x���h")
                    Churchministry += "�a�x���h,";
                if (split[5] == "��ͻ���")
                    Churchministry += "��ͻ���,";
                if (split[6] == "��L")
                    Churchministry += "��L";
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
            cell.InnerHtml = dr["�p�����f"].ToString();
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 1234);
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            cell.InnerHtml = dr["���f�q��"].ToString();
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            css.Add("mso-number-format", "\\@");
            Util.AddTDLine(css, 1234);
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            cell.InnerHtml = dr["���fEmail"].ToString();
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 1234);
            row.Cells.Add(cell);

            table.Rows.Add(row);
        }

        //�ন html �X
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        table.RenderControl(htw);
        return htw.InnerWriter.ToString();
    }

    private DataTable GetDataTable()
    {


        //Ū����Ʈw
        string strSql = @"
                        select c.uid , c.Name as �з|�W�� ,(select  name from codecity where ZipCode=cc1.ZipCode ) +CC.Name+c.Address as �з|�a�} ,
                        c.ChurchPhone as �з|�q��,c.ChurchEmail2 as �з|Email, c.Priest as �D�����v ,
                        c.PriestPhone as �D�����v�q��,c.PriestEmail as �D�����vEmail,c.Churchministry as �з|�Ƥu,
                        c.Contact as �p�����f , c.ChurchMphone as ���f�q�� ,
                        c.ChurchEmail as ���fEmail
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