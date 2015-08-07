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

public partial class CallerCategory : BasePage   //System.Web.UI.Page
	{
    int[] Yvalue = new int[10];
    
		# region Fields

		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label Label4;

		#endregion
        private string getConsultant(string Consultant, string year,string number)
        {
            
            //Ū����Ʈw
            string strSql = @" 
                            select count(*) as sum
                            from consulting
                            where isnull(IsDelete, '') != 'Y'
                            
                          ";
            if (number.ToString() =="�j��")
            {
                strSql += " and ConsultantMain like @Consultant";
            }
            else if (number.ToString() == "����")
            {
                strSql += " and ConsultantItem like @Consultant";
            }
            //if (ddlYear1.SelectedValue != "" || ddlQuerter.SelectedValue!= "")
            //{
            //    strSql += " and CONVERT(varchar(4), CreateDate, 111) = @CreateDate";
            // }
            //if (ddlQuerter.SelectedValue == "1")
            //{
            //    strSql += " and CONVERT(varchar(7), CreateDate, 111) Between '" + year + "/01' and '" + year + "/03'";
            //}
            //if (ddlQuerter.SelectedValue == "2")
            //{
            //    strSql += " and CONVERT(varchar(7), CreateDate, 111) Between '" + year + "/04' and '" + year + "/06'";
            //}
            //if (ddlQuerter.SelectedValue == "3")
            //{
            //    strSql += " and CONVERT(varchar(7), CreateDate, 111) Between '" + year + "/07' and '" + year + "/09'";
            //}
            //if (ddlQuerter.SelectedValue == "4")
            //{
            //    strSql += " and CONVERT(varchar(7), CreateDate, 111) Between '" + year + "/10' and '" + year + "/12'";
            //}
            //if (ddlYear1.SelectedValue == "�W�b�~")
            //{
            //    strSql += " and CONVERT(varchar(7), CreateDate, 111) Between '" + year + "/01' and '" + year + "/06'";
            //}
            //if (ddlYear1.SelectedValue == "�U�b�~")
            //{
            //    strSql += " and CONVERT(varchar(7), CreateDate, 111) Between '" + year + "/07' and '" + year + "/12'";
            //}
            //if (txtBegCreateDate.Text.Trim() != "" && txtEndCreateDate.Text.Trim() != "")
            //{
            //    strSql += " and CONVERT(varchar(100), CreateDate, 111) Between @BegCreateDate And @EndCreateDate\n";
            //}
            if (ddlYear1.SelectedValue != "" || ddlQuerter.SelectedValue != "")
            {
                strSql += " and year(CreateDate) = @CreateDate";
            }
            if (ddlQuerter.SelectedValue == "1")
            {
                strSql += " and CONVERT(varchar, CreateDate, 111) Between '" + year + "/01/01' and '" + year + "/03/30'";
            }
            if (ddlQuerter.SelectedValue == "2")
            {
                strSql += " and CONVERT(varchar, CreateDate, 111) Between '" + year + "/04/01' and '" + year + "/06/30'";
            }
            if (ddlQuerter.SelectedValue == "3")
            {
                strSql += " and CONVERT(varchar, CreateDate, 111) Between '" + year + "/07/01' and '" + year + "/09/30'";
            }
            if (ddlQuerter.SelectedValue == "4")
            {
                strSql += " and CONVERT(varchar, CreateDate, 111) Between '" + year + "/10/01' and '" + year + "/12/31'";
            }
            if (ddlYear1.SelectedValue == "�W�b�~")
            {
                strSql += " and CONVERT(varchar, CreateDate, 111) Between '" + year + "/01/01' and '" + year + "/06/30'";
            }
            if (ddlYear1.SelectedValue == "�U�b�~")
            {
                strSql += " and CONVERT(varchar, CreateDate, 111) Between '" + year + "/07/01' and '" + year + "/12/31'";
            }
            if (txtBegCreateDate.Text.Trim() != "" && txtEndCreateDate.Text.Trim() != "")
            {
                strSql += " and CONVERT(varchar, CreateDate, 111) Between @BegCreateDate And @EndCreateDate\n";
            }
            DataRow dr = null;
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("Consultant", "%" + Consultant + "%");
            dict.Add("CreateDate", year);
            dict.Add("BegCreateDate", txtBegCreateDate.Text.Trim());
            dict.Add("EndCreateDate", txtEndCreateDate.Text.Trim());
            DataTable dt = NpoDB.GetDataTableS(strSql, dict);
            if (dt.Rows.Count == 0)
            {
                 return "0";
            }
            dr = dt.Rows[0];

            return dr["sum"].ToString();
        }
      
        public void LoadDropDownListData()
        {
            LoadDownListYear(ddlYear, (DateTime.Now.Year - 1), "Desc");
            ddlQuerter.Items.Add(new ListItem("", ""));
            ddlQuerter.Items.Add(new ListItem("1", "1"));
            ddlQuerter.Items.Add(new ListItem("2", "2"));
            ddlQuerter.Items.Add(new ListItem("3", "3"));
            ddlQuerter.Items.Add(new ListItem("4", "4"));
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {

            lblRpt.Text = GetRptHead() + GetReport();
            string[] xValues = { "�B�e", "�B��", "���", "�ˤl", "���ծa�x", "�j�N�a�x", "�q�B�a�x", "�����a�x", "���~���D","�Ѧ~���D"}; //,  //<<�j��
                           //      "�g��", "����", "�H��", "�H��", "�C�@", "�η@", "�o�[", "�⨬", "¾��", "�з|", "���q", "�ʥͬ�",//<<����
                               //  "�믫�e�f", "����e�f", "�ө�", "�~�J", "�ɤO", "�Ш|", "�Q��", "�Ƿ~", "���g", "�ʧO���D"};//<<����
            Yvalue[0] = int.Parse(getConsultant("�B�e", ddlYear.SelectedValue.ToString(),"�j��"));
            Yvalue[1] = int.Parse(getConsultant("�B��", ddlYear.SelectedValue.ToString(), "�j��"));
            Yvalue[2] = int.Parse(getConsultant("���", ddlYear.SelectedValue.ToString(), "�j��"));
            Yvalue[3] = int.Parse(getConsultant("�ˤl", ddlYear.SelectedValue.ToString(), "�j��"));
            Yvalue[4] = int.Parse(getConsultant("���ծa�x", ddlYear.SelectedValue.ToString(), "�j��"));
            Yvalue[5] = int.Parse(getConsultant("�j�N�a�x", ddlYear.SelectedValue.ToString(), "�j��"));
            Yvalue[6] = int.Parse(getConsultant("�q�B�a�x", ddlYear.SelectedValue.ToString(), "�j��"));
            Yvalue[7] = int.Parse(getConsultant("�����a�x", ddlYear.SelectedValue.ToString(), "�j��"));
            Yvalue[8] = int.Parse(getConsultant("���~���D", ddlYear.SelectedValue.ToString(), "�j��"));
            Yvalue[9] = int.Parse(getConsultant("�Ѧ~���D", ddlYear.SelectedValue.ToString(), "�j��"));
            //Yvalue[10] = int.Parse(getConsultant("�g��", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[11] = int.Parse(getConsultant("����", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[12] = int.Parse(getConsultant("�H��", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[13] = int.Parse(getConsultant("�H��", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[14] = int.Parse(getConsultant("�C�@", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[15] = int.Parse(getConsultant("�o�[", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[16] = int.Parse(getConsultant("�⨬", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[17] = int.Parse(getConsultant("¾��", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[18] = int.Parse(getConsultant("�з|", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[19] = int.Parse(getConsultant("���q", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[20] = int.Parse(getConsultant("�ʥͬ�", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[21] = int.Parse(getConsultant("�믫�e�f", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[22] = int.Parse(getConsultant("����e�f", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[23] = int.Parse(getConsultant("�ө�", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[24] = int.Parse(getConsultant("�~�J", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[25] = int.Parse(getConsultant("�ɤO", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[26] = int.Parse(getConsultant("�Ш|", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[27] = int.Parse(getConsultant("�Q��", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[28] = int.Parse(getConsultant("�Ƿ~", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[29] = int.Parse(getConsultant("���g", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[30] = int.Parse(getConsultant("�ʧO���D", ddlYear.SelectedValue.ToString(), "����"));
            Chart1.Series["Default"].Points.DataBindXY(xValues, Yvalue);
            Chart1.Series["Default"].ChartType = SeriesChartType.Pie;
            Chart1.Series["Default"]["PieLabelStyle"] = "Outside";

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
            string time = "";
            string Year1 = "";
            string Querter = "";
            string Data = "";
            if (txtBegCreateDate.Text != "" && txtEndCreateDate.Text != "")
            {
                time = txtBegCreateDate.Text + "~" + txtEndCreateDate.Text;
            }
            if (ddlYear1.SelectedValue != "")
            {
                Year1 = ddlYear1.SelectedValue;
            }
            strTemp =  "���h�M�u�ӹq" + ddlYear.SelectedValue + Year1  + "�~�ײέp" + time ;
            if (ddlQuerter.SelectedValue != "")
            {
                Querter = ddlQuerter.SelectedValue;
                strTemp =  "���h�M�u�ӹq" + ddlYear.SelectedValue + Year1 + "��" + Querter + "�u�~�ײέp" + time ;
            }
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
                      "�B�e",
                      "�B��",
                      "���",
                      "�ˤl",
                      "���ծa�x",
                      "�j�N�a�x",
                      "�q�B�a�x",
                      "�����a�x",
                      "���~���D",
                      "�Ѧ~���D"         
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
            

            //-------------------------------------------------------------
            Yvalue[0] = int.Parse(getConsultant("�B�e", ddlYear.SelectedValue.ToString(), "�j��"));
            Yvalue[1] = int.Parse(getConsultant("�B��", ddlYear.SelectedValue.ToString(), "�j��"));
            Yvalue[2] = int.Parse(getConsultant("���", ddlYear.SelectedValue.ToString(), "�j��"));
            Yvalue[3] = int.Parse(getConsultant("�ˤl", ddlYear.SelectedValue.ToString(), "�j��"));
            Yvalue[4] = int.Parse(getConsultant("���ծa�x", ddlYear.SelectedValue.ToString(), "�j��"));
            Yvalue[5] = int.Parse(getConsultant("�j�N�a�x", ddlYear.SelectedValue.ToString(), "�j��"));
            Yvalue[6] = int.Parse(getConsultant("�q�B�a�x", ddlYear.SelectedValue.ToString(), "�j��"));
            Yvalue[7] = int.Parse(getConsultant("�����a�x", ddlYear.SelectedValue.ToString(), "�j��"));
            Yvalue[8] = int.Parse(getConsultant("���~���D", ddlYear.SelectedValue.ToString(), "�j��"));
            Yvalue[9] = int.Parse(getConsultant("�Ѧ~���D", ddlYear.SelectedValue.ToString(), "�j��"));
            //Yvalue[10] = int.Parse(getConsultant("�g��", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[11] = int.Parse(getConsultant("����", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[12] = int.Parse(getConsultant("�H��", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[13] = int.Parse(getConsultant("�H��", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[14] = int.Parse(getConsultant("�C�@", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[15] = int.Parse(getConsultant("�o�[", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[16] = int.Parse(getConsultant("�⨬", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[17] = int.Parse(getConsultant("¾��", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[18] = int.Parse(getConsultant("�з|", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[19] = int.Parse(getConsultant("���q", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[20] = int.Parse(getConsultant("�ʥͬ�", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[21] = int.Parse(getConsultant("�믫�e�f", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[22] = int.Parse(getConsultant("����e�f", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[23] = int.Parse(getConsultant("�ө�", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[24] = int.Parse(getConsultant("�~�J", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[25] = int.Parse(getConsultant("�ɤO", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[26] = int.Parse(getConsultant("�Ш|", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[27] = int.Parse(getConsultant("�Q��", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[28] = int.Parse(getConsultant("�Ƿ~", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[29] = int.Parse(getConsultant("���g", ddlYear.SelectedValue.ToString(), "����"));
            //Yvalue[30] = int.Parse(getConsultant("�ʧO���D", ddlYear.SelectedValue.ToString(), "����"));
            //---------------------------------------------------------------
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 1234);
            cell.InnerHtml = Yvalue[0].ToString();
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 234);
            cell.InnerHtml = Yvalue[1].ToString();
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 234);
            cell.InnerHtml = Yvalue[2].ToString();
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            css.Add("mso-number-format", "\\@");
            Util.AddTDLine(css, 234);
            cell.InnerHtml = Yvalue[3].ToString();
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 234);
            cell.InnerHtml = Yvalue[4].ToString();
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            css.Add("mso-number-format", "\\@");
            Util.AddTDLine(css, 234);
            cell.InnerHtml = Yvalue[5].ToString();
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            css.Add("mso-number-format", "\\@");
            Util.AddTDLine(css, 234);
            cell.InnerHtml = Yvalue[6].ToString();
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            css.Add("mso-number-format", "\\@");
            Util.AddTDLine(css, 234);
            cell.InnerHtml = Yvalue[7].ToString();
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            css.Add("mso-number-format", "\\@");
            Util.AddTDLine(css, 234);
            cell.InnerHtml = Yvalue[8].ToString();
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            css.Add("mso-number-format", "\\@");
            Util.AddTDLine(css, 234);
            cell.InnerHtml = Yvalue[9].ToString();
            row.Cells.Add(cell);
            table.Rows.Add(row);
            //--------------------------------------------
            //��3��,������
            row = new HtmlTableRow();
            cell = new HtmlTableCell();


            //�έp
            int sum = Yvalue[0] + Yvalue[1] + Yvalue[2] + Yvalue[3] + Yvalue[4]+
                      Yvalue[5] + Yvalue[6] + Yvalue[7] + Yvalue[8] + Yvalue[9];
            //---------------------------------------------------------------
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 134);
            if (sum != 0)
            {
                cell.InnerHtml = Math.Round(((float.Parse(Yvalue[0].ToString()) / sum) * 100), 2, MidpointRounding.AwayFromZero).ToString() + "%";
            }
            else
            {
                cell.InnerHtml = "0%";
            }
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 34);
            if (sum != 0)
            {
                cell.InnerHtml = Math.Round(((float.Parse(Yvalue[1].ToString()) / sum) * 100), 2, MidpointRounding.AwayFromZero).ToString() + "%";
            }
            else
            {
                cell.InnerHtml = "0%";
            }
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            css.Add("mso-number-format", "\\@");
            Util.AddTDLine(css, 34);
            if (sum != 0)
            {
                cell.InnerHtml = Math.Round(((float.Parse(Yvalue[2].ToString()) / sum) * 100), 2, MidpointRounding.AwayFromZero).ToString() + "%";
            }
            else
            {
                cell.InnerHtml = "0%";
            }
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 34);
            if (sum != 0)
            {
                cell.InnerHtml = Math.Round(((float.Parse(Yvalue[3].ToString()) / sum) * 100), 2, MidpointRounding.AwayFromZero).ToString() + "%";
            }
            else
            {
                cell.InnerHtml = "0%";
            }
            row.Cells.Add(cell);
            table.Rows.Add(row);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 34);
            if (sum != 0)
            {
                cell.InnerHtml = Math.Round(((float.Parse(Yvalue[4].ToString()) / sum) * 100), 2, MidpointRounding.AwayFromZero).ToString() + "%";
            }
            else
            {
                cell.InnerHtml = "0%";
            }
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 34);
            if (sum != 0)
            {
                cell.InnerHtml = Math.Round(((float.Parse(Yvalue[5].ToString()) / sum) * 100), 2, MidpointRounding.AwayFromZero).ToString() + "%";
            }
            else
            {
                cell.InnerHtml = "0%";
            }
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 34);
            if (sum != 0)
            {
                cell.InnerHtml = Math.Round(((float.Parse(Yvalue[6].ToString()) / sum) * 100), 2, MidpointRounding.AwayFromZero).ToString() + "%";
            }
            else
            {
                cell.InnerHtml = "0%";
            }
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 34);
            if (sum != 0)
            {
                cell.InnerHtml = Math.Round(((float.Parse(Yvalue[7].ToString()) / sum) * 100), 2, MidpointRounding.AwayFromZero).ToString() + "%";
            }
            else
            {
                cell.InnerHtml = "0%";
            }
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 34);
            if (sum != 0)
            {
                cell.InnerHtml = Math.Round(((float.Parse(Yvalue[8].ToString()) / sum) * 100), 2, MidpointRounding.AwayFromZero).ToString() + "%";
            }
            else
            {
                cell.InnerHtml = "0%";
            }
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 34);
            if (sum != 0)
            {
                cell.InnerHtml = Math.Round(((float.Parse(Yvalue[9].ToString()) / sum) * 100), 2, MidpointRounding.AwayFromZero).ToString() + "%";
            }
            else
            {
                cell.InnerHtml = "0%";
            }
            row.Cells.Add(cell);


            //�ন html �X
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            table.RenderControl(htw);
            return htw.InnerWriter.ToString();

        }
        //DownList���~��
        private void LoadDownListYear(DropDownList ddlTarget, int iStartYear, string strOrder)
        {
            int iEndYear = Util.GetDBDateTime().Year;

            if (strOrder == "Desc")
            {
                for (int i = iEndYear; i >= iStartYear; i--)
                {
                    ddlTarget.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }
            }
            else
            {
                for (int i = iStartYear; i <= iEndYear; i++)
                {
                    ddlTarget.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }
            }
        }
        protected void ddlQuerter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlYear1.SelectedValue != "")
            {
                ddlYear1.SelectedValue = "";
            }
            if (txtBegCreateDate.Text != "" && txtEndCreateDate.Text != "")
            {
                txtBegCreateDate.Text = "";
                txtEndCreateDate.Text = "";
            }
        }
}
