using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.DataVisualization.Charting;
using System.Text;
using System.Collections.Generic;
using System.IO;

public partial class AreaAnalysis : BasePage   //System.Web.UI.Page
{
    string strBtn = "";
    int sum = 0;
    int colCount = 0;
    # region Fields

    protected System.Web.UI.WebControls.Label Label1;
    protected System.Web.UI.WebControls.Label Label2;
    protected System.Web.UI.WebControls.Label Label3;
    protected System.Web.UI.WebControls.Label Label4;

    #endregion
    private string getCityaAreaCnt(string city, string area)
    {

        //讀取資料庫
        string strSql = @" 
                            Select (select  name from codecity where ZipCode=cc.ZipCode ) as 縣市,CC1.Name as 區間,COUNT(*) As Cnt
                            from Member inner join CodeCity CC on CC.ZipCode =City 
                            inner join CodeCity CC1 on CC1.ZipCode = Area
                            Where city =@city and isnull(IsDelete, '') != 'Y' 
                          ";
        if (HFD_Area.Value != "")
        {
            strSql += "and area =@area";
        }
        if (txtBegCreateDate.Text != "" && txtEndCreateDate.Text != "")
        {
            strSql += " and CONVERT(varchar(100), Member.InsertDate, 111) Between @BegCreateDate And @EndCreateDate\n";
        }
        strSql += " GROUP BY cc.zipcode,City ,CC1.Name";
        DataRow dr = null;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("city", city);
        dict.Add("area", area);
        dict.Add("BegCreateDate", txtBegCreateDate.Text);
        dict.Add("EndCreateDate", txtEndCreateDate.Text);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count == 0)
        {
            return "0";
        }
        dr = dt.Rows[0];
        if (ddlCity.SelectedValue != "")
        {
            int[] Yvalue = new int[dt.Rows.Count];
            string[] Xvalue = new string[dt.Rows.Count];
            colCount = dt.Rows.Count; 
            for (int i = 0; i < dt.Rows.Count; i++)
               
            {
                Xvalue[i] = dt.Rows[i]["區間"].ToString();
                Yvalue[i] = int.Parse(dt.Rows[i]["Cnt"].ToString());
            }
            Chart1.Series["Default"].Points.DataBindXY(Xvalue, Yvalue);
            Chart1.Series["Default"].ChartType = SeriesChartType.Pie;
            Chart1.Series["Default"]["PieLabelStyle"] = "Outside";
        }
        return dr["Cnt"].ToString();
    }
    private string getCityCnt(string name)
    {
        string strSql = "";
        if (name == "")
        {
            //ALL
            strSql = @" 
                         Select (select  name from codecity where ZipCode=cc.ZipCode ) as 縣市,COUNT(*) As Cnt
                         from Member inner join CodeCity CC on CC.ZipCode =City 
                         Where  1=1  and isnull(IsDelete, '') != 'Y' 
                          ";
        }
        else
        {
            //讀取資料庫
            strSql = @" 
                         Select  (select  name from codecity where ZipCode=cc.ZipCode ) as 縣市,COUNT(*) As Cnt
                         from Member inner join CodeCity CC on CC.ZipCode =City 
                         Where  1=1 And name=@name and isnull(IsDelete, '') != 'Y' 
                          ";
        }

        if (txtBegCreateDate.Text != "" && txtEndCreateDate.Text != "")
        {
            strSql += " and CONVERT(varchar(100), Member.InsertDate, 111) Between @BegCreateDate And @EndCreateDate\n";
        }
        if (name == "")
        {
            strSql += @" GROUP BY cc.zipcode";
        }
        else
        {
            //讀取資料庫
            strSql += @" GROUP BY cc.zipcode";
        }
        DataRow dr = null;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("name", name);
        dict.Add("BegCreateDate", txtBegCreateDate.Text);
        dict.Add("EndCreateDate", txtEndCreateDate.Text);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count == 0)
        {
            return "0";
        }
        dr = dt.Rows[0];
        int[] Yvalue = new int[dt.Rows.Count];
        string[] Xvalue = new string[dt.Rows.Count];
        colCount = dt.Rows.Count;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            Xvalue[i] = dt.Rows[i]["縣市"].ToString();
            Yvalue[i] = int.Parse(dt.Rows[i]["Cnt"].ToString());
        }
        Chart1.Series["Default"].Points.DataBindXY(Xvalue, Yvalue);
        Chart1.Series["Default"].ChartType = SeriesChartType.Pie;
        Chart1.Series["Default"]["PieLabelStyle"] = "Outside";
        return dr["Cnt"].ToString();
    }
    public void LoadDropDownListData()
    {
        Util.FillCityData(ddlCity);
        //Util.FillAreaData(ddlArea, ddlCity.SelectedValue);
    }
    protected void btnNCity_Click(object sender, EventArgs e)
    {
        ddlCity.SelectedValue = "";
        strBtn = "NCity";
        GetDataTableSum();
        getNCity();
        
        lblRpt.Text = GetRptHead() + GetReport();
    }
    //查詢資料庫(找尋 縣市 是 NULL 或是空值)
    private string getNCity()
    {
        string strSql = @"
                        Select (select  name from codecity where ZipCode=cc.ZipCode ) as 縣市,COUNT(*) As Cnt
                        from Member inner join CodeCity CC on CC.ZipCode =City 
                        Where  1=1  and isnull(IsDelete, '') = 'Y' 
                        ";
        if (txtBegCreateDate.Text != "" && txtEndCreateDate.Text != "")
        {
            strSql += " and CONVERT(varchar(100), Member.InsertDate, 111) Between @BegCreateDate And @EndCreateDate";
        }
        strSql += " GROUP BY cc.zipcode";
        DataRow dr = null;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("BegCreateDate", txtBegCreateDate.Text);
        dict.Add("EndCreateDate", txtEndCreateDate.Text);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count == 0)
        {
            return "0";
        }
        dr = dt.Rows[0];
        int[] Yvalue = new int[dt.Rows.Count];
        string[] Xvalue = new string[dt.Rows.Count];
        colCount = dt.Rows.Count;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            Xvalue[i] = dt.Rows[i]["縣市"].ToString();
            Yvalue[i] = int.Parse(dt.Rows[i]["Cnt"].ToString());
        }
        Chart1.Series["Default"].Points.DataBindXY(Xvalue, Yvalue);
        Chart1.Series["Default"].ChartType = SeriesChartType.Pie;
        Chart1.Series["Default"]["PieLabelStyle"] = "Outside";
        return dr["Cnt"].ToString();
    }
    protected void btnQuerycity_Click(object sender, EventArgs e)
    {
        strBtn = "All";
        GetDataTableSum();
        getCityCnt("");
        
        lblRpt.Text = GetRptHead() + GetReport();
        string[] xValues = { };
        int[] Yvalue = new int[7];
        if (ddlCity.SelectedValue != "")
        {
            ddlCity.SelectedValue = "";
        }
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        strBtn = "";

        getCityaAreaCnt(ddlCity.SelectedValue, HFD_Area.Value);
      
      
        lblRpt.Text = GetRptHead() + GetReport();
     

    }
    protected void Page_Load(object sender, System.EventArgs e)
    {
        //載入下拉式選單資料
        if (!IsPostBack)
        {
            LoadDropDownListData();
        }
    }
    /*
                if (this.ChartTypeList.SelectedItem.ToString() == "Pie")
                    Chart1.Series["Default"].ChartType = SeriesChartType.Pie;

                else
                    Chart1.Series["Default"].ChartType = SeriesChartType.Doughnut;

                if (this.ShowLegend.Checked)
                    Chart1.Series["Default"]["PieLabelStyle"] = "Disabled";

                else
                    Chart1.Series["Default"]["PieLabelStyle"] = "Inside";

                    // 設置圖表類型和標題
                Chart1.Series["Default"].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), this.ChartTypeList.SelectedItem.ToString(), true);
                Chart1.Titles[0].Text = ChartTypeList.SelectedItem.ToString() + " Chart";

                // 設置標籤樣式
                Chart1.Series["Default"]["PieLabelStyle"] = this.LabelStyleList.SelectedItem.ToString();

                // Set Doughnut hole size
                Chart1.Series["Default"]["DoughnutRadius"] = this.HoleSizeList.SelectedItem.ToString();

                // Disable Doughnut hole size control for Pie chart
                this.HoleSizeList.Enabled = (this.ChartTypeList.SelectedItem.ToString() != "Pie");

                // Explode selected country
                foreach(DataPoint point in Chart1.Series["Default"].Points)
                {
                    point["Exploded"] = "false";
                    if(point.AxisLabel == this.ExplodedPointList.SelectedItem.ToString())
                    {
                        point["Exploded"] = "true";
                    }
                }

                // Enable 3D
                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = CheckboxShow3D.Checked;

                Chart1.Series[0]["PieDrawingStyle"] = this.Dropdownlist1.SelectedItem.ToString();

                // Pie drawing style
                if (this.CheckboxShow3D.Checked)
                    this.Dropdownlist1.Enabled = false;
			
                else
                    this.Dropdownlist1.Enabled = true;

                if (this.ShowLegend.Checked)
                    this.Chart1.Legends[0].Enabled = true;
                else
                    this.Chart1.Legends[0].Enabled = false;

            }*/

    #region Web Form Designer generated code
    override protected void OnInit(EventArgs e)
    {
        //
        // CODEGEN: This call is required by the ASP.NET Web Form Designer.
        //
        InitializeComponent();
        base.OnInit(e);
    }

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {

    }
    #endregion



    protected void btnPrint_Click(object sender, EventArgs e)
    {
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
        css.Add("width", "50%");
        //****************************************
        row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "區域個案數";// "" + ddlYear.SelectedValue + "年熱線志工班表  第 " + ddlQuerter.SelectedValue + " 季 ";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "24px");
        css.Add("font-family", "標楷體");
        css.Add("border-style", "none");
        css.Add("font-weight", "bold");
        cell.ColSpan =  colCount ;
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
        string strSql = "";
        if (strBtn == "All")
        {
            strSql = @"
                         Select  (select  name from codecity where ZipCode=cc.ZipCode ) as Name,COUNT(*) As Cnt
                         from Member inner join CodeCity CC on CC.ZipCode =City 
                         Where  1=1 and isnull(IsDelete, '') != 'Y' 
                        ";
        }
        else if (strBtn == "NCity")
        {
             strSql = @"
                        Select (select  name from codecity where ZipCode=cc.ZipCode ) as Name,COUNT(*) As Cnt
                        from Member inner join CodeCity CC on CC.ZipCode =City 
                        Where  1=1  and isnull(IsDelete, '') = 'Y'   
                        ";
        }
        else 
        {
            //讀取資料庫
            strSql = @" 
                        Select cc1.Name,COUNT(*) As Cnt
                        from Member inner join CodeCity CC on CC.ZipCode =City 
                        inner  join CodeCity CC1 on CC1.ZipCode = Area
                        Where  1=1 And City=@City and isnull(IsDelete, '') != 'Y' 
                       
                          ";
        }

        if (txtBegCreateDate.Text != "" && txtEndCreateDate.Text != "")
        {
            strSql += " and CONVERT(varchar(100), Member.InsertDate, 111) Between @BegCreateDate And @EndCreateDate\n";
        }
        if (strBtn == "All")
        {
            strSql += @" GROUP BY cc.zipcode,City";
        }
        else if (strBtn == "NCity")
        {
            strSql += @" GROUP BY cc.zipcode";
        }
        else
        {
            //讀取資料庫
            strSql += @" GROUP BY CC1.Name";
        }

        Dictionary<string, object> dict = new Dictionary<string, object>();

        dict.Add("City", ddlCity.SelectedValue);
        dict.Add("BegCreateDate", txtBegCreateDate.Text);
        dict.Add("EndCreateDate", txtEndCreateDate.Text);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
       

        return dt;
       
    }

    private DataTable GetDataTableSum()
    {
        string strSql = "";
        if (strBtn == "ALL")
        {
            strSql = @"
                    Select COUNT(*) As Cnt
                    from Member inner join CodeCity CC on CC.ZipCode =City 
                    Where  1=1  and isnull(IsDelete, '') != 'Y' 
                   ";
        }
        else if (strBtn == "NCity")
        {
            strSql = @"
                    Select COUNT(*) As Cnt
                    from Member inner join CodeCity CC on CC.ZipCode =City 
                    Where  1=1  and isnull(IsDelete, '') = 'Y'  
                        ";
        }
        else
        {
            strSql = @"
                    Select COUNT(*) As Cnt
                    from Member inner join CodeCity CC on CC.ZipCode =City 
                    Where  1=1  and isnull(IsDelete, '') != 'Y' 
                   ";
        }
        if (txtBegCreateDate.Text != "" && txtEndCreateDate.Text != "")
        {
            strSql += " and CONVERT(varchar(100), Member.InsertDate, 111) Between @BegCreateDate And @EndCreateDate\n";
        }
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("City", ddlCity.SelectedValue);
        dict.Add("BegCreateDate", txtBegCreateDate.Text);
        dict.Add("EndCreateDate", txtEndCreateDate.Text);
        DataTable dt1 = NpoDB.GetDataTableS(strSql, dict);
        foreach (DataRow dr in dt1.Rows)
        {
            sum = int.Parse(dr["Cnt"].ToString());
        }
        
        return dt1;
    }

    // 報表************************************************************************************************************************
    private string GetReport()
    {
        HtmlTable table = new HtmlTable();
        HtmlTableRow row;
        HtmlTableCell cell;
        CssStyleCollection css;
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        row = new HtmlTableRow();
        //--------------------------------------------
        DataTable dt = GetDataTable();
        GetDataTableSum();
        table.Align = "center";
        string[] Items = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            Items[i] = dt.Rows[i]["Name"].ToString();
        }
        for (int i = 0; i < Items.Length; i++)
        {
            cell = new HtmlTableCell();
            string strTemp = Items[i].ToString();
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "25px");  //抬頭字型大小
            //css.Add("width", "800px");
            row.Cells.Add(cell);
            Util.AddTDLine(css, 1234);
        }
        table.Rows.Add(row);

        //***********************************************

        if (dt.Rows.Count == 0)
        {
            // lblReport.Text = "";
            ShowSysMsg("查無資料");
            return "";
        }
        row = new HtmlTableRow();
        foreach (DataRow dr in dt.Rows)
        {
            string strFontSize = "16px";
            css = table.Style;
            css.Add("font-size", strFontSize);
            css.Add("font-family", "標楷體");
            //css.Add("width", "900px");

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            //css.Add("font-weight", strWeight);
            css.Add("height", "20px");
            cell.InnerHtml = dr["Cnt"].ToString();
            row.Cells.Add(cell);
            table.Rows.Add(row);
            for (int i = 0; i < Items.Length; i++)
            {
                if (i == 0)
                {
                    Util.AddTDLine(css, 134);
                }
                Util.AddTDLine(css, 34);
            }
            table.Rows.Add(row);
        }

        row = new HtmlTableRow();
        foreach (DataRow dr in dt.Rows)
        {
            string strFontSize = "16px";

            css = table.Style;
            css.Add("font-size", strFontSize);
            css.Add("font-family", "標楷體");
            //css.Add("width", "900px");

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            //css.Add("font-weight", strWeight);
            css.Add("height", "20px");
            css.Add("mso-number-format", "\\@");
            cell.InnerHtml = Math.Round(((float.Parse(dr["Cnt"].ToString()) / sum) * 100), 2, MidpointRounding.AwayFromZero).ToString() + "%";
            //"比例" = dr["Cnt"].ToString() / sum *100;
            row.Cells.Add(cell);
            table.Rows.Add(row);
            for (int i = 0; i < Items.Length; i++)
            {
                if (i == 0)
                {
                    Util.AddTDLine(css, 134);
                }
                Util.AddTDLine(css, 34);
            }
            table.Rows.Add(row);
        }


        table.RenderControl(htw);

        //轉成 html 碼========================================================================

        return htw.InnerWriter.ToString();
    }



}
