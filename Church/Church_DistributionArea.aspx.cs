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

public partial class Church_DistributionArea : BasePage   //System.Web.UI.Page
	{
		# region Fields

		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label Label4;

		#endregion
        private string getCityCnt(string cityname)
        {

            //讀取資料庫
            string strSql = @" 
                         Select CC.Name as 縣市,COUNT(*) As Cnt
                         from Member inner join CodeCity CC on CC.ZipCode =City 
                         Where  name=@name 
                         GROUP BY CC.Name
                          ";
            DataRow dr = null;
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("name", cityname);
            DataTable dt = NpoDB.GetDataTableS(strSql, dict);
           
            if (dt.Rows.Count == 0)
            {
                //  ShowSysMsg("無新來電");
                 return "0";
               
            }
            dr = dt.Rows[0];
            return dr["Cnt"].ToString();
            
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            //填充序列數據 
            string[] xValues = { "新北市", "桃竹區", "中彰區", "嘉南區", "高雄區", "基宜花", "台北市" };
            int[] Yvalue = new int[7];
            Yvalue[0] = int.Parse(getCityCnt("新北市"));
            Yvalue[1] = int.Parse(getCityCnt("桃園縣")) + int.Parse(getCityCnt("新竹縣")) + int.Parse(getCityCnt("新竹市"));
            Yvalue[2] = int.Parse(getCityCnt("彰化縣")) + int.Parse(getCityCnt("彰化縣"));
            Yvalue[3] = int.Parse(getCityCnt("嘉義市")) + int.Parse(getCityCnt("嘉義縣")) + int.Parse(getCityCnt("台南市"));
            Yvalue[4] = int.Parse(getCityCnt("高雄市"));
            Yvalue[5] = int.Parse(getCityCnt("基隆市")) + int.Parse(getCityCnt("宜蘭縣")) + int.Parse(getCityCnt("花蓮縣"));
            Yvalue[6] = int.Parse(getCityCnt("台北市"));
            LB_NTCity.Text = Yvalue[0].ToString();
            LB_THArea.Text = Yvalue[1].ToString();
            LB_TCArea.Text = Yvalue[2].ToString();
            LB_CTArea.Text = Yvalue[3].ToString();
            LB_KaohsiungArea.Text = Yvalue[4].ToString();
            LB_KIHArea.Text = Yvalue[5].ToString();
            LB_TaipeiCity.Text = Yvalue[6].ToString();
            citysum.Text = (int.Parse(Yvalue[0].ToString()) + int.Parse(Yvalue[1].ToString()) + int.Parse(Yvalue[2].ToString()) + int.Parse(Yvalue[3].ToString()) + int.Parse(Yvalue[4].ToString()) + int.Parse(Yvalue[5].ToString()) + int.Parse(Yvalue[6].ToString())).ToString(); 
            Chart1.Series["Default"].Points.DataBindXY(xValues, Yvalue);
            Chart1.Series["Default"].ChartType = SeriesChartType.Pie;
            Chart1.Series["Default"]["PieLabelStyle"] = "Outside";
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
            strTemp = " 轉介個案區域比例";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "24px");
            css.Add("font-family", "標楷體");
            css.Add("border-style", "none");
            css.Add("font-weight", "bold");
            //ColSpan跨行置中幾格
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
        private string GetReport()
        {
            string[] Items = {
                        "新北市",
                        "桃竹區",
                        "中彰區",
                        "嘉南區",
                        "高雄區",
                        "基宜花",
                        "台北市",
                        "總計",
                       
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
                  
                    Util.AddTDLine(css, 1234);
                }
                if (i == 1)
                {
                    
                    Util.AddTDLine(css, 234);
                }
                if (i == 2)
                {
                    Util.AddTDLine(css, 234);
                }
                if (i == 3)
                {
                    Util.AddTDLine(css, 234);
                }
                if (i == 4)
                {
                    Util.AddTDLine(css, 234);
                }
                if (i == 5)
                {
                    Util.AddTDLine(css, 234);
                }
                if (i == 6)
                {
                    Util.AddTDLine(css, 234);
                }
                if (i == 7)
                {
                    Util.AddTDLine(css, 234);
                }
                if (i == 8)
                {
                    Util.AddTDLine(css, 234);
                }
                if (i == 9)
                {
                    Util.AddTDLine(css, 234);
                }
                if (i == 10)
                {
                    Util.AddTDLine(css, 234);
                }
                if (i == 11)
                {
                    Util.AddTDLine(css, 234);
                }
                if (i == 12)
                {
                    Util.AddTDLine(css, 234);
                }

                row.Cells.Add(cell);
            }
            cell = new HtmlTableCell();
            string strTemp1 = "";// "" + strTitle + "";
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
            row = new HtmlTableRow();
            cell = new HtmlTableCell();

            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 1234);
            cell.InnerHtml = LB_NTCity.Text;
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 234);
            cell.InnerHtml = LB_THArea.Text;
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 234);
            cell.InnerHtml = LB_TCArea.Text;
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            css.Add("mso-number-format", "\\@");
            Util.AddTDLine(css, 234);
            cell.InnerHtml = LB_CTArea.Text;
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 234);
            cell.InnerHtml = LB_KaohsiungArea.Text;
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 234);
            cell.InnerHtml = LB_KIHArea.Text;
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            css.Add("mso-number-format", "\\@");
            Util.AddTDLine(css, 234);
            cell.InnerHtml = LB_TaipeiCity.Text;
            row.Cells.Add(cell);
            table.Rows.Add(row);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            css.Add("mso-number-format", "\\@");
            Util.AddTDLine(css, 234);
            cell.InnerHtml = citysum.Text;
            row.Cells.Add(cell);
            table.Rows.Add(row);

           table.Rows.Add(row);

            //轉成 html 碼
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            table.RenderControl(htw);
            return htw.InnerWriter.ToString();

        }
}
