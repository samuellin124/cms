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

public partial class StatusOfAnalysis : BasePage   //System.Web.UI.Page
	{
    string allQuery = "";
    string City = "";
		# region Fields

		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label Label4;

		#endregion
        private string getCnt(string count)
        {
            string strSql = "";
            //Ū����Ʈw
            if (City == "NCity")
            {
                strSql = @" 
                            select count(*) AS SUM from Member 
                            where  FollowUp like @FollowUp
                            and isnull(IsDelete, '') != 'Y' 
                            and ChangeChurchSta='�w�B�z' and city is null
                          ";
            }
            else
            {
                strSql = @" 
                            select count(*) AS SUM from Member 
                            where  FollowUp like @FollowUp
                            and isnull(IsDelete, '') != 'Y' 
                            and ChangeChurchSta='�w�B�z'
                          ";
            }
            if (txtBegCreateDate.Text != "" && txtEndCreateDate.Text != "")
            {
                strSql += " and CONVERT(varchar(100),changeChurchDate, 111) Between @BegCreateDate And @EndCreateDate\n";
            }
            
            if (ddlCity.SelectedValue != "" && allQuery != "1")
            {
                strSql += " and city = @city";
            }
            DataRow dr = null;
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("FollowUp", "%" + count + "%");
            dict.Add("city", ddlCity.SelectedValue);
            //dict.Add("area", HFD_Area.Value);
            dict.Add("BegCreateDate", txtBegCreateDate.Text);
            dict.Add("EndCreateDate", txtEndCreateDate.Text);
            DataTable dt = NpoDB.GetDataTableS(strSql, dict);

            if (dt.Rows.Count == 0)
            {
                 return "0";
               
            }
            dr = dt.Rows[0];
            return dr["SUM"].ToString();
           
            
        }
        protected void btnNCity_Click(object sender, EventArgs e)
        {
            ddlCity.SelectedValue = "";
            City = "NCity";
            int[] Yvalue = new int[5];
            string[] xValues = { "���~", "í�w�E�|", "���ӻE�|", "���ֶi�J�з|", "���h��" };
            HFD_Yvalue.Value = getCnt("���~");
            HFD_Yvalue.Value += "," + getCnt("í�w�E�|");
            HFD_Yvalue.Value += "," + getCnt("���ӻE�|");
            HFD_Yvalue.Value += "," + getCnt("���ֶi�J�з|");
            HFD_Yvalue.Value += "," + getCnt("���h��");

            string[] strs = HFD_Yvalue.Value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            LB_FollowUp1.Text = strs[0].ToString();
            LB_FollowUp2.Text = strs[1].ToString();
            LB_FollowUp3.Text = strs[2].ToString();
            LB_FollowUp4.Text = strs[3].ToString();
            LB_FollowUp5.Text = strs[4].ToString();
            int sum = int.Parse(strs[0].ToString()) + int.Parse(strs[1].ToString()) + int.Parse(strs[2].ToString()) + int.Parse(strs[3].ToString()) + int.Parse(strs[4].ToString());
            if (sum == 0)
            {
                lblsum1.Text = 0 + "%";
                lblsum2.Text = 0 + "%";
                lblsum3.Text = 0 + "%";
                lblsum4.Text = 0 + "%";
                lblsum5.Text = 0 + "%";
            }
            else
            {
                lblsum1.Text = Math.Round(((float.Parse(strs[0].ToString()) / sum) * 100), 2, MidpointRounding.AwayFromZero).ToString() + "%";
                lblsum2.Text = Math.Round(((float.Parse(strs[1].ToString()) / sum) * 100), 2, MidpointRounding.AwayFromZero).ToString() + "%";
                lblsum3.Text = Math.Round(((float.Parse(strs[2].ToString()) / sum) * 100), 2, MidpointRounding.AwayFromZero).ToString() + "%";
                lblsum4.Text = Math.Round(((float.Parse(strs[3].ToString()) / sum) * 100), 2, MidpointRounding.AwayFromZero).ToString() + "%";
                lblsum5.Text = Math.Round(((float.Parse(strs[4].ToString()) / sum) * 100), 2, MidpointRounding.AwayFromZero).ToString() + "%";
            }
            Chart1.Series["Default"].Points.DataBindXY(xValues, Yvalue);
            Chart1.Series["Default"].ChartType = SeriesChartType.Pie;
            Chart1.Series["Default"]["PieLabelStyle"] = "Outside";

        }
        protected void btnQuery_Click(object sender, EventArgs e)
        {

            int[] Yvalue = new int[5];
            string[] xValues = { "���~", "í�w�E�|", "���ӻE�|", "���ֶi�J�з|", "���h��" };
            HFD_Yvalue.Value = getCnt("���~");
            HFD_Yvalue.Value += "," + getCnt("í�w�E�|");
            HFD_Yvalue.Value += "," + getCnt("���ӻE�|");
            HFD_Yvalue.Value += "," + getCnt("���ֶi�J�з|");
            HFD_Yvalue.Value += "," + getCnt("���h��");

            string[] strs = HFD_Yvalue.Value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            LB_FollowUp1.Text = strs[0].ToString();
            LB_FollowUp2.Text = strs[1].ToString();
            LB_FollowUp3.Text = strs[2].ToString();
            LB_FollowUp4.Text = strs[3].ToString();
            LB_FollowUp5.Text = strs[4].ToString();
            int sum = int.Parse(strs[0].ToString()) + int.Parse(strs[1].ToString()) + int.Parse(strs[2].ToString()) + int.Parse(strs[3].ToString()) + int.Parse(strs[4].ToString());
            if (sum == 0)
            {
                lblsum1.Text = 0 + "%";
                lblsum2.Text = 0 + "%";
                lblsum3.Text = 0 + "%";
                lblsum4.Text = 0 + "%";
                lblsum5.Text = 0 + "%";
            }
            else
            {
                lblsum1.Text = Math.Round(((float.Parse(strs[0].ToString()) / sum) * 100), 2, MidpointRounding.AwayFromZero).ToString() + "%";
                lblsum2.Text = Math.Round(((float.Parse(strs[1].ToString()) / sum) * 100), 2, MidpointRounding.AwayFromZero).ToString() + "%";
                lblsum3.Text = Math.Round(((float.Parse(strs[2].ToString()) / sum) * 100), 2, MidpointRounding.AwayFromZero).ToString() + "%";
                lblsum4.Text = Math.Round(((float.Parse(strs[3].ToString()) / sum) * 100), 2, MidpointRounding.AwayFromZero).ToString() + "%";
                lblsum5.Text = Math.Round(((float.Parse(strs[4].ToString()) / sum) * 100), 2, MidpointRounding.AwayFromZero).ToString() + "%";
            }
            Chart1.Series["Default"].Points.DataBindXY(xValues, Yvalue);
            Chart1.Series["Default"].ChartType = SeriesChartType.Pie;
            Chart1.Series["Default"]["PieLabelStyle"] = "Outside";

        }
        public void LoadDropDownListData()
        {
            Util.FillCityData(ddlCity);
            //Util.FillAreaData(ddlArea, ddlCity.SelectedValue);
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            //���J�U�Ԧ������
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

                // �]�m�Ϫ������M���D
            Chart1.Series["Default"].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), this.ChartTypeList.SelectedItem.ToString(), true);
            Chart1.Titles[0].Text = ChartTypeList.SelectedItem.ToString() + " Chart";

            // �]�m���Ҽ˦�
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
                    ";//���-�W�k�U��
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(s);
            Util.OutputTxt(sb + lblRpt.Text, "1", DateTime.Now.ToString("yyyyMMddHHmmss"));
        }
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
            strTemp = "�श�D����ЭӮײ{�p";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "24px");
            css.Add("font-family", "�з���");
            css.Add("border-style", "none");
            css.Add("font-weight", "bold");
            //ColSpan���m���X��
            cell.ColSpan = 5;
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
                        "���~",
                        "í�w�E�|",
                        "���ӻE�|",
                        "���ֶi�J�з|",
                        "���h��",           
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
            //��2��,������
            //*********�Ĥ@��*********
            string strFontSize = "18px";
            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 1234);
            cell.InnerHtml = LB_FollowUp1.Text;
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 234);
            cell.InnerHtml = LB_FollowUp2.Text;
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 234);
            cell.InnerHtml = LB_FollowUp3.Text;
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            css.Add("mso-number-format", "\\@");
            Util.AddTDLine(css, 234);
            cell.InnerHtml = LB_FollowUp4.Text;
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 234);
            cell.InnerHtml = LB_FollowUp5.Text;
            row.Cells.Add(cell);
           


           table.Rows.Add(row);

            //�ন html �X
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            table.RenderControl(htw);
            return htw.InnerWriter.ToString();

        }

        protected void btnAllQuery_Click(object sender, EventArgs e)
        {
            allQuery = "1";
            int[] Yvalue = new int[5];
            string[] xValues = { "���~", "í�w�E�|", "���ӻE�|", "���ֶi�J�з|", "���h��" };
            HFD_Yvalue.Value = getCnt("���~");
            HFD_Yvalue.Value +="," + getCnt("í�w�E�|");
            HFD_Yvalue.Value += "," + getCnt("���ӻE�|");
            HFD_Yvalue.Value += "," + getCnt("���ֶi�J�з|");
            HFD_Yvalue.Value += "," + getCnt("���h��");

            string[] strs = HFD_Yvalue.Value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            LB_FollowUp1.Text = strs[0].ToString();
            LB_FollowUp2.Text = strs[1].ToString();
            LB_FollowUp3.Text = strs[2].ToString();
            LB_FollowUp4.Text = strs[3].ToString();
            LB_FollowUp5.Text = strs[4].ToString();
            int sum = int.Parse(strs[0].ToString()) + int.Parse(strs[1].ToString()) + int.Parse(strs[2].ToString()) + int.Parse(strs[3].ToString()) + int.Parse(strs[4].ToString());
            if (sum == 0)
            {
                lblsum1.Text = 0 + "%";
                lblsum2.Text = 0 + "%";
                lblsum3.Text = 0 + "%";
                lblsum4.Text = 0 + "%";
                lblsum5.Text = 0 + "%";
            }
            else
            {
                lblsum1.Text = Math.Round(((float.Parse(strs[0].ToString()) / sum) * 100), 2, MidpointRounding.AwayFromZero).ToString() + "%";
                lblsum2.Text = Math.Round(((float.Parse(strs[1].ToString()) / sum) * 100), 2, MidpointRounding.AwayFromZero).ToString() + "%";
                lblsum3.Text = Math.Round(((float.Parse(strs[2].ToString()) / sum) * 100), 2, MidpointRounding.AwayFromZero).ToString() + "%";
                lblsum4.Text = Math.Round(((float.Parse(strs[3].ToString()) / sum) * 100), 2, MidpointRounding.AwayFromZero).ToString() + "%";
                lblsum5.Text = Math.Round(((float.Parse(strs[4].ToString()) / sum) * 100), 2, MidpointRounding.AwayFromZero).ToString() + "%";
            } 
            Chart1.Series["Default"].Points.DataBindXY(xValues, Yvalue);
            Chart1.Series["Default"].ChartType = SeriesChartType.Pie;
            Chart1.Series["Default"]["PieLabelStyle"] = "Outside";
            if (ddlCity.SelectedValue != "")
            {
                ddlCity.SelectedValue = "";
            }
        }
}
