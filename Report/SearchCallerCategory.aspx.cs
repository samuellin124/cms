//kuofly 20150211
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Text;

public partial class SearchCallerCategory : BasePage
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
        //LoadFormData();
    }
    protected void btnNextPage_Click(object sender, EventArgs e)
    {
        HFD_CurrentPage.Value = Util.AddStringNumber(HFD_CurrentPage.Value);
        //LoadFormData();
    }
    protected void btnGoPage_Click(object sender, EventArgs e)
    {
        //LoadFormData();
    }
    #endregion NpoGridView 處理換頁相關程式碼
    //---------------------------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["ProgID"] = "Consult";
        HFD_EditServiceUser.Value = SessionInfo.UserID.ToString();
        Util.ShowSysMsgWithScript("$('#btnPreviousPage').hide();$('#btnNextPage').hide();$('#btnGoPage').hide();");


    }
    //-------------------------------------------------------------------------------------------------------------
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        lblRpt.Text = GetRptHead1() + GetRptHead2() + GetRptHead3() + GetRptHead4() + GetRptHead5() + GetRptHead6();
    }
    //----------------------------------------------------------------------------------
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        string 列印模式新增表頭 = "";

        列印模式新增表頭 += "<table><tr><td colspan=\"8\" style=\"font-size:24px;font-family:標楷體;width:100%;\">";
        列印模式新增表頭 += "關懷專線";
        if (txtBegCreateDate.Text.Trim() != "" && txtEndCreateDate.Text.Trim() != "") {
            列印模式新增表頭 += "(" + txtBegCreateDate.Text.Trim() + "～" + txtEndCreateDate.Text.Trim() + ")"; 
        }
        列印模式新增表頭 += "統計";
        列印模式新增表頭 += "</td></tr></table>\n";
        lblRpt.Text = 列印模式新增表頭 + GetRptHead1() + GetRptHead2() + GetRptHead3() + GetRptHead4() + GetRptHead5() + GetRptHead6();
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
    //查詢SLQ來電諮詢別(大類)-----------------------------------------------------------
    private string getCnt1(string ConsultantMain)
    {

        //讀取資料庫
        string strSql = @" 
                            select count(*) as sum
                            from consulting
                            where 1=1
                            and isnull(IsDelete, '') != 'Y'
                            and ConsultantMain like @ConsultantMain
                          ";
        if (txtBegCreateDate.Text.Trim() != "" && txtEndCreateDate.Text.Trim() != "")
        {
            strSql += " and CONVERT(varchar, CreateDate, 111) Between @BegCreateDate And @EndCreateDate\n";
        }
        DataRow dr = null;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("ConsultantMain", "%" + ConsultantMain + "%");
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
    //顯示來電諮詢別(大類)  筆數--------------------------------------------------------
    private string GetRptHead1()
    {
        //----------------------------------------------------------------------
        string[] O1 = new string[11];
        O1[0] = getCnt1("婚前");
        O1[1] = getCnt1("婚姻");
        O1[2] = getCnt1("單親");
        O1[3] = getCnt1("親子");
        O1[4] = getCnt1("重組家庭");
        O1[5] = getCnt1("遠距家庭");
        O1[6] = getCnt1("隔代家庭");
        O1[7] = getCnt1("通婚家庭");
        O1[8] = getCnt1("受難家庭");
        O1[9] = getCnt1("中年問題");
        O1[10] = getCnt1("老年問題");

        int sum = int.Parse(O1[0]) + int.Parse(O1[1]) + int.Parse(O1[2]) + int.Parse(O1[3]) + int.Parse(O1[4]);
        sum += int.Parse(O1[5]) + int.Parse(O1[6]) + int.Parse(O1[7]) + int.Parse(O1[8]) + int.Parse(O1[9]) + int.Parse(O1[10]);
        //設定框架----------------------------------------------------------------------
        string strTemp = "";
        string 產出表格的Str="";  
        //寫這種寫法的人怎麼不快點離職....
        //HtmlTable table = new HtmlTable();
        //HtmlTableRow row;
        //HtmlTableCell cell;
        //CssStyleCollection css;
        //string strColor = "BLACK";
        //string FontSize = "14px";
        //table.Border = 0;
        //table.CellPadding = 0;
        //table.CellSpacing = 0;
        //table.Align = "center";
        //css = table.Style;
        //css.Add("font-size", "12px");
        //css.Add("font-family", "標楷體");
        //css.Add("width", "100%");
        產出表格的Str += "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" align=\"center\" style=\"font-size:12px;font-family:標楷體;width:100%;\">\n";
        //----------------------------------------------------------
        if (sum != 0)
        {
            //row = new HtmlTableRow();
            //cell = new HtmlTableCell();
            //strTemp = "來電諮詢別(大類)";
            //cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;   //<<--你神經病，上一行都自己說有值了這裡判斷甚麼小朋友
            //css = cell.Style;
            //css.Add("text-align", "center");
            //css.Add("font-size", "28px");
            //css.Add("font-family", "標楷體");
            //css.Add("border-style", "none");
            //cell.ColSpan = 3;
            //row.Cells.Add(cell);
            //table.Rows.Add(row);
            
            //有資料的時候有表頭
            產出表格的Str += "\t<tr>\n";
		    產出表格的Str += "\t\t<td style=\"text-align:center;font-size:28px;font-family:標楷體;border-style:none;\" colspan=\"3\">來電諮詢別(大類)</td>\n";
            產出表格的Str += "\t</tr>\n";

        }
/////////////////////////////////////////   第一行 by apple  //////////////////////////////////////////////////////////////////
        //婚前O1[0]----------------------------------------------------------
        //row = new HtmlTableRow();
        產出表格的Str += "\t<tr>\n";
        if (int.Parse(O1[0]) != 0)
        {
            //cell = new HtmlTableCell();
            //strTemp = "婚前";
            //cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            //css = cell.Style;
            //css.Add("text-align", "center");
            //css.Add("font-size", "18px");
            //css.Add("font-family", "標楷體");
            //Util.AddTDLine(css, 1234, strColor);
            //css.Add("width","130px");
            //row.Cells.Add(cell);
            產出表格的Str += "\t\t<td style=\"text-align:center;font-size:18px;font-family:標楷體;border-left:1px solid  BLACK ;border-top:1px solid BLACK ;border-right:1px solid BLACK ;border-bottom:1px solid BLACK ;width:130px;\">";
            產出表格的Str += "婚前";
            產出表格的Str += "</td>\n";
        }
        //婚姻O1[1]----------------------------------------------------------------
        if (int.Parse(O1[1]) != 0)
        {
            //cell = new HtmlTableCell();
            //strTemp = "婚姻";
            //cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            //css = cell.Style;
            //css.Add("text-align", "center");
            //css.Add("font-size", "18px");
            //css.Add("font-family", "標楷體");
            //Util.AddTDLine(css, 234, strColor);
            //css.Add("width", "130px");
            //row.Cells.Add(cell);
            產出表格的Str += "\t\t<td style=\"text-align:center;font-size:18px;font-family:標楷體;border-left:1px solid  BLACK ;border-top:1px solid BLACK ;border-right:1px solid BLACK ;border-bottom:1px solid BLACK ;width:130px;\">";
            產出表格的Str += "婚姻";
            產出表格的Str += "</td>\n";

        }
        //單親O1[2]----------------------------------------------------------------
        if (int.Parse(O1[2]) != 0)
        {
            //cell = new HtmlTableCell();
            //strTemp = "單親";
            //cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            //css = cell.Style;
            //css.Add("text-align", "center");
            //css.Add("font-size", "18px");
            //css.Add("font-family", "標楷體");
            //Util.AddTDLine(css, 234, strColor);
            //css.Add("width", "130px");
            //row.Cells.Add(cell);
            產出表格的Str += "\t\t<td style=\"text-align:center;font-size:18px;font-family:標楷體;border-left:1px solid  BLACK ;border-top:1px solid BLACK ;border-right:1px solid BLACK ;border-bottom:1px solid BLACK ;width:130px;\">";
            產出表格的Str += "單親";
            產出表格的Str += "</td>\n";
        }
        //親子O1[3]----------------------------------------------------------------
        if (int.Parse(O1[3]) != 0)
        {
            //cell = new HtmlTableCell();
            //strTemp = "親子";
            //cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            //css = cell.Style;
            //css.Add("text-align", "center");
            //css.Add("font-size", "18px");
            //css.Add("font-family", "標楷體");
            //Util.AddTDLine(css, 234, strColor);
            //css.Add("width", "130px");
            //row.Cells.Add(cell);
            產出表格的Str += "\t\t<td style=\"text-align:center;font-size:18px;font-family:標楷體;border-left:1px solid  BLACK ;border-top:1px solid BLACK ;border-right:1px solid BLACK ;border-bottom:1px solid BLACK ;width:130px;\">";
            產出表格的Str += "親子";
            產出表格的Str += "</td>\n";

        }
        //重組家庭O1[4]----------------------------------------------------------------
        if (int.Parse(O1[4]) != 0)
        {
            //cell = new HtmlTableCell();
            //strTemp = "重組家庭";
            //cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            //css = cell.Style;
            //css.Add("text-align", "center");
            //css.Add("font-size", "18px");
            //css.Add("font-family", "標楷體");
            //Util.AddTDLine(css, 234, strColor);
            //css.Add("width", "130px");
            //row.Cells.Add(cell);
            產出表格的Str += "\t\t<td style=\"text-align:center;font-size:18px;font-family:標楷體;border-left:1px solid  BLACK ;border-top:1px solid BLACK ;border-right:1px solid BLACK ;border-bottom:1px solid BLACK ;width:130px;\">";
            產出表格的Str += "重組家庭";
            產出表格的Str += "</td>\n";
        }
        //遠距家庭O1[5]----------------------------------------------------------------
        if (int.Parse(O1[5]) != 0)
        {
            //cell = new HtmlTableCell();
            //strTemp = "遠距家庭";
            //cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            //css = cell.Style;
            //css.Add("text-align", "center");
            //css.Add("font-size", "18px");
            //css.Add("font-family", "標楷體");
            //Util.AddTDLine(css, 234, strColor);
            //css.Add("width", "130px");
            //row.Cells.Add(cell);
            產出表格的Str += "\t\t<td style=\"text-align:center;font-size:18px;font-family:標楷體;border-left:1px solid  BLACK ;border-top:1px solid BLACK ;border-right:1px solid BLACK ;border-bottom:1px solid BLACK ;width:130px;\">";
            產出表格的Str += "遠距家庭";
            產出表格的Str += "</td>\n";
        }
        //隔代家庭O1[6]----------------------------------------------------------------
        if (int.Parse(O1[6]) != 0)
        {
            //cell = new HtmlTableCell();
            //strTemp = "隔代家庭";
            //cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            //css = cell.Style;
            //css.Add("text-align", "center");
            //css.Add("font-size", "18px");
            //css.Add("font-family", "標楷體");
            //Util.AddTDLine(css, 234, strColor);
            //css.Add("width", "130px");
            //row.Cells.Add(cell);
            產出表格的Str += "\t\t<td style=\"text-align:center;font-size:18px;font-family:標楷體;border-left:1px solid  BLACK ;border-top:1px solid BLACK ;border-right:1px solid BLACK ;border-bottom:1px solid BLACK ;width:130px;\">";
            產出表格的Str += "隔代家庭";
            產出表格的Str += "</td>\n";

        }
        //通婚家庭O1[7]----------------------------------------------------------------
        if (int.Parse(O1[7]) != 0)
        {
            //cell = new HtmlTableCell();
            //strTemp = "通婚家庭";
            //cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            //css = cell.Style;
            //css.Add("text-align", "center");
            //css.Add("font-size", "18px");
            //css.Add("font-family", "標楷體");
            //Util.AddTDLine(css, 234, strColor);
            //css.Add("width", "130px");
            //row.Cells.Add(cell);
            產出表格的Str += "\t\t<td style=\"text-align:center;font-size:18px;font-family:標楷體;border-left:1px solid  BLACK ;border-top:1px solid BLACK ;border-right:1px solid BLACK ;border-bottom:1px solid BLACK ;width:130px;\">";
            產出表格的Str += "通婚家庭";
            產出表格的Str += "</td>\n";
        }
        //受難家庭O1[8]----------------------------------------------------------------
        if (int.Parse(O1[8]) != 0)
        {
            //cell = new HtmlTableCell();
            //strTemp = "受難家庭";
            //cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            //css = cell.Style;
            //css.Add("text-align", "center");
            //css.Add("font-size", "18px");
            //css.Add("font-family", "標楷體");
            //Util.AddTDLine(css, 234, strColor);
            //css.Add("width", "130px");
            //row.Cells.Add(cell);
            產出表格的Str += "\t\t<td style=\"text-align:center;font-size:18px;font-family:標楷體;border-left:1px solid  BLACK ;border-top:1px solid BLACK ;border-right:1px solid BLACK ;border-bottom:1px solid BLACK ;width:130px;\">";
            產出表格的Str += "受難家庭";
            產出表格的Str += "</td>\n";
        }
        //中年問題O1[9]----------------------------------------------------------------
        if (int.Parse(O1[9]) != 0)
        {
            //cell = new HtmlTableCell();
            //strTemp = "中年問題";
            //cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            //css = cell.Style;
            //css.Add("text-align", "center");
            //css.Add("font-size", "18px");
            //css.Add("font-family", "標楷體");
            //Util.AddTDLine(css, 234, strColor);
            //css.Add("width", "130px");
            //row.Cells.Add(cell);
            產出表格的Str += "\t\t<td style=\"text-align:center;font-size:18px;font-family:標楷體;border-left:1px solid  BLACK ;border-top:1px solid BLACK ;border-right:1px solid BLACK ;border-bottom:1px solid BLACK ;width:130px;\">";
            產出表格的Str += "中年問題";
            產出表格的Str += "</td>\n";
        }
        //老年問題O1[10]----------------------------------------------------------------
        if (int.Parse(O1[10]) != 0)
        {
            //cell = new HtmlTableCell();
            //strTemp = "老年問題";
            //cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            //css = cell.Style;
            //css.Add("text-align", "center");
            //css.Add("font-size", "18px");
            //css.Add("font-family", "標楷體");
            //Util.AddTDLine(css, 234, strColor);
            //css.Add("width", "130px");
            //row.Cells.Add(cell);
            產出表格的Str += "\t\t<td style=\"text-align:center;font-size:18px;font-family:標楷體;border-left:1px solid  BLACK ;border-top:1px solid BLACK ;border-right:1px solid BLACK ;border-bottom:1px solid BLACK ;width:130px;\">";
            產出表格的Str += "老年問題";
            產出表格的Str += "</td>\n";
        }
        //----------------
        產出表格的Str += "\t</tr>\n";
        /////////////////////////////////////////   第二行by apple  //////////////////////////////////////////////////////////////////
        //婚前O1[0]----------------------------------------------------------
        //row = new HtmlTableRow();
        產出表格的Str += "\t<tr>\n";
        if (int.Parse(O1[0]) != 0)
        {
            //cell = new HtmlTableCell();
            //strTemp = O1[0];
            //cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            //css = cell.Style;
            //css.Add("text-align", "center");
            //css.Add("font-size", "18px");
            //css.Add("font-family", "標楷體");
            //Util.AddTDLine(css, 134, strColor);
            //css.Add("width", "130px");
            //row.Cells.Add(cell);
            strTemp = (O1[0] == "" ? "&nbsp" : O1[0]); 
            產出表格的Str += "\t\t<td style=\"text-align:center;font-size:18px;font-family:標楷體;border-left:1px solid  BLACK;border-right:1px solid  BLACK;border-bottom:1px solid  BLACK;width:130px;\">";
            產出表格的Str += strTemp;
            產出表格的Str += "</td>\n";
        }
        //婚姻O1[1]----------------------------------------------------------------
        if (int.Parse(O1[1]) != 0)
        {
            //cell = new HtmlTableCell();
            //strTemp = O1[1];
            //cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            //css = cell.Style;
            //css.Add("text-align", "center");
            //css.Add("font-size", "18px");
            //css.Add("font-family", "標楷體");
            //Util.AddTDLine(css, 34, strColor);
            //css.Add("width", "130px");
            //row.Cells.Add(cell);
            strTemp = (O1[1] == "" ? "&nbsp" : O1[1]);
            產出表格的Str += "\t\t<td style=\"text-align:center;font-size:18px;font-family:標楷體;border-left:1px solid  BLACK;border-right:1px solid  BLACK;border-bottom:1px solid  BLACK;width:130px;\">";
            產出表格的Str += strTemp;
            產出表格的Str += "</td>\n";

        }
        //單親O1[2]----------------------------------------------------------------
        if (int.Parse(O1[2]) != 0)
        {
            //cell = new HtmlTableCell();
            //strTemp = O1[2];
            //cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            //css = cell.Style;
            //css.Add("text-align", "center");
            //css.Add("font-size", "18px");
            //css.Add("font-family", "標楷體");
            //Util.AddTDLine(css, 34, strColor);
            //css.Add("width", "150px");
            //row.Cells.Add(cell);
            strTemp = (O1[2] == "" ? "&nbsp" : O1[2]);
            產出表格的Str += "\t\t<td style=\"text-align:center;font-size:18px;font-family:標楷體;border-left:1px solid  BLACK;border-right:1px solid  BLACK;border-bottom:1px solid  BLACK;width:130px;\">";
            產出表格的Str += strTemp;
            產出表格的Str += "</td>\n";
        }
        //親子O1[3]----------------------------------------------------------------
        if (int.Parse(O1[3]) != 0)
        {
            //cell = new HtmlTableCell();
            //strTemp = O1[3];
            //cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            //css = cell.Style;
            //css.Add("text-align", "center");
            //css.Add("font-size", "18px");
            //css.Add("font-family", "標楷體");
            //Util.AddTDLine(css, 34, strColor);
            //css.Add("width", "130px");
            //row.Cells.Add(cell);
            strTemp = (O1[3] == "" ? "&nbsp" : O1[3]);
            產出表格的Str += "\t\t<td style=\"text-align:center;font-size:18px;font-family:標楷體;border-left:1px solid  BLACK;border-right:1px solid  BLACK;border-bottom:1px solid  BLACK;width:130px;\">";
            產出表格的Str += strTemp;
            產出表格的Str += "</td>\n";
        }
        //重組家庭O1[4]----------------------------------------------------------------
        if (int.Parse(O1[4]) != 0)
        {
            //cell = new HtmlTableCell();
            //strTemp = O1[4];
            //cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            //css = cell.Style;
            //css.Add("text-align", "center");
            //css.Add("font-size", "18px");
            //css.Add("font-family", "標楷體");
            //Util.AddTDLine(css, 34, strColor);
            //css.Add("width", "130px");
            //row.Cells.Add(cell);
            strTemp = (O1[4] == "" ? "&nbsp" : O1[4]);
            產出表格的Str += "\t\t<td style=\"text-align:center;font-size:18px;font-family:標楷體;border-left:1px solid  BLACK;border-right:1px solid  BLACK;border-bottom:1px solid  BLACK;width:130px;\">";
            產出表格的Str += strTemp;
            產出表格的Str += "</td>\n";
        }
        //遠距家庭O1[5]----------------------------------------------------------------
        if (int.Parse(O1[5]) != 0)
        {
            //cell = new HtmlTableCell();
            //strTemp = O1[5];
            //cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            //css = cell.Style;
            //css.Add("text-align", "center");
            //css.Add("font-size", "18px");
            //css.Add("font-family", "標楷體");
            //Util.AddTDLine(css, 34, strColor);
            //css.Add("width", "130px");
            //row.Cells.Add(cell);
            strTemp = (O1[5] == "" ? "&nbsp" : O1[5]);
            產出表格的Str += "\t\t<td style=\"text-align:center;font-size:18px;font-family:標楷體;border-left:1px solid  BLACK;border-right:1px solid  BLACK;border-bottom:1px solid  BLACK;width:130px;\">";
            產出表格的Str += strTemp;
            產出表格的Str += "</td>\n";
        }
        //隔代家庭O1[6]----------------------------------------------------------------
        if (int.Parse(O1[6]) != 0)
        {
            //cell = new HtmlTableCell();
            //strTemp = O1[6];
            //cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            //css = cell.Style;
            //css.Add("text-align", "center");
            //css.Add("font-size", "18px");
            //css.Add("font-family", "標楷體");
            //Util.AddTDLine(css, 34, strColor);
            //css.Add("width", "130px");
            //row.Cells.Add(cell);
            strTemp = (O1[6] == "" ? "&nbsp" : O1[6]);
            產出表格的Str += "\t\t<td style=\"text-align:center;font-size:18px;font-family:標楷體;border-left:1px solid  BLACK;border-right:1px solid  BLACK;border-bottom:1px solid  BLACK;width:130px;\">";
            產出表格的Str += strTemp;
            產出表格的Str += "</td>\n";

        }
        //通婚家庭O1[7]----------------------------------------------------------------
        if (int.Parse(O1[7]) != 0)
        {
            //cell = new HtmlTableCell();
            //strTemp = O1[7];
            //cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            //css = cell.Style;
            //css.Add("text-align", "center");
            //css.Add("font-size", "18px");
            //css.Add("font-family", "標楷體");
            //Util.AddTDLine(css, 34, strColor);
            //css.Add("width", "130px");
            //row.Cells.Add(cell);
            strTemp = (O1[7] == "" ? "&nbsp" : O1[7]);
            產出表格的Str += "\t\t<td style=\"text-align:center;font-size:18px;font-family:標楷體;border-left:1px solid  BLACK;border-right:1px solid  BLACK;border-bottom:1px solid  BLACK;width:130px;\">";
            產出表格的Str += strTemp;
            產出表格的Str += "</td>\n";
        }
        //受難家庭O1[8]----------------------------------------------------------------
        if (int.Parse(O1[8]) != 0)
        {
            //cell = new HtmlTableCell();
            //strTemp = O1[8];
            //cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            //css = cell.Style;
            //css.Add("text-align", "center");
            //css.Add("font-size", "18px");
            //css.Add("font-family", "標楷體");
            //Util.AddTDLine(css, 34, strColor);
            //css.Add("width", "130px");
            //row.Cells.Add(cell);
            strTemp = (O1[8] == "" ? "&nbsp" : O1[8]);
            產出表格的Str += "\t\t<td style=\"text-align:center;font-size:18px;font-family:標楷體;border-left:1px solid  BLACK;border-right:1px solid  BLACK;border-bottom:1px solid  BLACK;width:130px;\">";
            產出表格的Str += strTemp;
            產出表格的Str += "</td>\n";
        }
        //中年問題O1[9]----------------------------------------------------------------
        if (int.Parse(O1[9]) != 0)
        {
            //cell = new HtmlTableCell();
            //strTemp = O1[9];
            //cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            //css = cell.Style;
            //css.Add("text-align", "center");
            //css.Add("font-size", "18px");
            //css.Add("font-family", "標楷體");
            //Util.AddTDLine(css, 34, strColor);
            //css.Add("width", "130px");
            //row.Cells.Add(cell);
            strTemp = (O1[9] == "" ? "&nbsp" : O1[9]);
            產出表格的Str += "\t\t<td style=\"text-align:center;font-size:18px;font-family:標楷體;border-left:1px solid  BLACK;border-right:1px solid  BLACK;border-bottom:1px solid  BLACK;width:130px;\">";
            產出表格的Str += strTemp;
            產出表格的Str += "</td>\n";
        }
        //老年問題O1[10]----------------------------------------------------------------
        if (int.Parse(O1[10]) != 0)
        {
            //cell = new HtmlTableCell();
            //strTemp = O1[10];
            //cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            //css = cell.Style;
            //css.Add("text-align", "center");
            //css.Add("font-size", "18px");
            //css.Add("font-family", "標楷體");
            //Util.AddTDLine(css, 34, strColor);
            //css.Add("width", "130px");
            //row.Cells.Add(cell);
            strTemp = (O1[10] == "" ? "&nbsp" : O1[10]);
            產出表格的Str += "\t\t<td style=\"text-align:center;font-size:18px;font-family:標楷體;border-left:1px solid  BLACK;border-right:1px solid  BLACK;border-bottom:1px solid  BLACK;width:130px;\">";
            產出表格的Str += strTemp;
            產出表格的Str += "</td>\n";
        }
        //----------------
        //table.Rows.Add(row);
        產出表格的Str += "\t<tr>\n";
        產出表格的Str += "</table>\n";
        //*************************************
        //轉成 html 碼
        //StringWriter sw = new StringWriter();
        //HtmlTextWriter htw = new HtmlTextWriter(sw);
        //table.RenderControl(htw);

        return 產出表格的Str;
    }
    //查詢SLQ來電諮詢別(分項)-----------------------------------------------------------
    private string getCnt2(string ConsultantItem)
    {

        //讀取資料庫
        string strSql = @" 
                            select count(*) as sum
                            from consulting
                            where ConsultantItem like @ConsultantItem
                          ";
        if (txtBegCreateDate.Text.Trim() != "" && txtEndCreateDate.Text.Trim() != "")
        {
            strSql += " and CONVERT(varchar(100), CreateDate, 111) Between @BegCreateDate And @EndCreateDate\n";
        }
        DataRow dr = null;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("ConsultantItem", "%" + ConsultantItem + "%");
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
    //顯示來來電諮詢別(分項)  筆數------------------------------------------------------
    private string GetRptHead2()
    {
        //----------------------------------------------------------------------
        string[] O2 = new string[22];
        O2[0] = getCnt2("經濟");
        O2[1] = getCnt2("情緒");
        O2[2] = getCnt2("信仰");
        O2[3] = getCnt2("人際");
        O2[4] = getCnt2("婆媳");
        O2[5] = getCnt2("翁媳");
        O2[6] = getCnt2("妯娌");
        O2[7] = getCnt2("手足");
        O2[8] = getCnt2("職場");
        O2[9] = getCnt2("教會");
        O2[10] = getCnt2("溝通");
        O2[11] = getCnt2("性生活");
        O2[12] = getCnt2("精神疾病");
        O2[13] = getCnt2("身體疾病");
        O2[14] = getCnt2("個性");
        O2[15] = getCnt2("外遇");
        O2[16] = getCnt2("暴力");
        O2[17] = getCnt2("教育");
        O2[18] = getCnt2("霸凌");
        O2[19] = getCnt2("學業");
        O2[20] = getCnt2("隱症");
        O2[21] = getCnt2("性別問題");

        int sum = int.Parse(O2[0]) + int.Parse(O2[1]) + int.Parse(O2[2]) + int.Parse(O2[3]) + int.Parse(O2[4]);
        sum += int.Parse(O2[5]) + int.Parse(O2[6]) + int.Parse(O2[7]) + int.Parse(O2[8]) + int.Parse(O2[9]);
        sum += int.Parse(O2[10]) + int.Parse(O2[11]) + int.Parse(O2[12]) + int.Parse(O2[13]) + int.Parse(O2[14]);
        sum += int.Parse(O2[15]) + int.Parse(O2[16]) + int.Parse(O2[17]) + int.Parse(O2[18]) + int.Parse(O2[19]);
        sum += int.Parse(O2[20]) + int.Parse(O2[21]);
        //設定框架----------------------------------------------------------------------
        string strTemp = "";
        HtmlTable table = new HtmlTable();
        HtmlTableRow row;
        HtmlTableCell cell;
        CssStyleCollection css;
        string strColor = "BLACK";
        string FontSize = "14px";
        table.Border = 0;
        table.CellPadding = 0;
        table.CellSpacing = 0;
        table.Align = "center";
        css = table.Style;
        css.Add("font-size", "12px");
        css.Add("font-family", "標楷體");
        css.Add("width", "100%");
        //----------------------------------------------------------
        if (sum != 0)
        {
            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            strTemp = "來電諮詢別(分項)";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "28px");
            css.Add("font-family", "標楷體");
            css.Add("border-style", "none");
            cell.ColSpan = 6;
            row.Cells.Add(cell);
            table.Rows.Add(row);
        }
        /////////////////////////////////////////   第一行 by apple  //////////////////////////////////////////////////////////////////
        //經濟O2[0]----------------------------------------------------------
        row = new HtmlTableRow();
        if (int.Parse(O2[0]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "經濟";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 1234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //情緒 O2[1]----------------------------------------------------------------
        if (int.Parse(O2[1]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "情緒";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //信仰 O2[2]----------------------------------------------------------------
        if (int.Parse(O2[2]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "信仰";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //人際O2[3]----------------------------------------------------------------
        if (int.Parse(O2[3]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "人際";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //婆媳O2[4]----------------------------------------------------------------
        if (int.Parse(O2[4]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "婆媳";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //翁媳O2[5]----------------------------------------------------------------
        if (int.Parse(O2[5]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "翁媳";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //妯娌O2[6]----------------------------------------------------------------
        if (int.Parse(O2[6]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "妯娌";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //手足O2[7]----------------------------------------------------------------
        if (int.Parse(O2[7]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "手足";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //職場O2[8]----------------------------------------------------------------
        if (int.Parse(O2[8]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "職場";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //教會O2[9]----------------------------------------------------------------
        if (int.Parse(O2[9]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "教會";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //溝通O2[10]----------------------------------------------------------------
        if (int.Parse(O2[10]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "溝通";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //性生活O2[11]----------------------------------------------------------------
        if (int.Parse(O2[11]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "性生活";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //精神疾病O2[12]----------------------------------------------------------------
        if (int.Parse(O2[12]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "精神疾病";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //身體疾病O2[13]----------------------------------------------------------------
        if (int.Parse(O2[13]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "身體疾病";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //個性O2[14]----------------------------------------------------------------
        if (int.Parse(O2[14]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "個性";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //外遇O2[15]----------------------------------------------------------------
        if (int.Parse(O2[15]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "外遇";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //暴力O2[16]----------------------------------------------------------------
        if (int.Parse(O2[16]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "暴力";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //教育O2[17]----------------------------------------------------------------
        if (int.Parse(O2[17]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "教育";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //霸凌O2[18]----------------------------------------------------------------
        if (int.Parse(O2[18]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "霸凌";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //學業O2[19]----------------------------------------------------------------
        if (int.Parse(O2[19]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "學業";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //隱症O2[20]----------------------------------------------------------------
        if (int.Parse(O2[20]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "隱症";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //性別問題O2[21]----------------------------------------------------------------
        if (int.Parse(O2[21]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "性別問題";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //----------------
        table.Rows.Add(row);
        /////////////////////////////////////////   第二行by apple  //////////////////////////////////////////////////////////////////
        //經濟O2[0]----------------------------------------------------------
        row = new HtmlTableRow();
        if (int.Parse(O2[0]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O2[0];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 134, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //情緒O2[1]----------------------------------------------------------------
        if (int.Parse(O2[1]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O2[1];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //信仰O2[2]----------------------------------------------------------------
        if (int.Parse(O2[2]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O2[2];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //人際O2[3]----------------------------------------------------------------
        if (int.Parse(O2[3]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O2[3];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //婆媳O2[4]----------------------------------------------------------------
        if (int.Parse(O2[4]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O2[4];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //翁媳O2[5]----------------------------------------------------------------
        if (int.Parse(O2[5]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O2[5];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //妯娌O2[6]----------------------------------------------------------------
        if (int.Parse(O2[6]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O2[6];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //手足O2[7]----------------------------------------------------------------
        if (int.Parse(O2[7]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O2[7];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //職場O2[8]----------------------------------------------------------------
        if (int.Parse(O2[8]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O2[8];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //教會O2[9]----------------------------------------------------------------
        if (int.Parse(O2[9]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O2[9];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //溝通O2[10]----------------------------------------------------------------
        if (int.Parse(O2[10]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O2[10];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //性生活O2[11]----------------------------------------------------------------
        if (int.Parse(O2[11]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O2[11];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //精神疾病O2[12]----------------------------------------------------------------
        if (int.Parse(O2[12]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O2[12];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //身體疾病O2[13]----------------------------------------------------------------
        if (int.Parse(O2[13]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O2[13];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //個性O2[14]----------------------------------------------------------------
        if (int.Parse(O2[14]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O2[14];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //外遇O2[15]----------------------------------------------------------------
        if (int.Parse(O2[15]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O2[15];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //暴力O2[16]----------------------------------------------------------------
        if (int.Parse(O2[16]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O2[16];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //教育O2[17]----------------------------------------------------------------
        if (int.Parse(O2[17]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O2[17];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //霸凌O2[18]----------------------------------------------------------------
        if (int.Parse(O2[18]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O2[18];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //學業O2[19]----------------------------------------------------------------
        if (int.Parse(O2[19]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O2[19];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //隱症O2[20]----------------------------------------------------------------
        if (int.Parse(O2[20]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O2[20];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //性別問題O2[21]----------------------------------------------------------------
        if (int.Parse(O2[21]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O2[21];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //----------------
        table.Rows.Add(row);
        //*************************************
        //轉成 html 碼
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        table.RenderControl(htw);

        return htw.InnerWriter.ToString();
    }
    //查詢SLQ騷擾電話-------------------------------------------------------------------
    private string getCnt3(string HarassPhone)
    {

        //讀取資料庫
        string strSql = @" 
                            select count(*) as sum
                            from consulting
                            where HarassPhone like @HarassPhone
                          ";
        if (txtBegCreateDate.Text.Trim() != "" && txtEndCreateDate.Text.Trim() != "")
        {
            strSql += " and CONVERT(varchar(100), CreateDate, 111) Between @BegCreateDate And @EndCreateDate\n";
        }
        DataRow dr = null;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("HarassPhone", "%" + HarassPhone + "%");
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
    //顯示騷擾電話  筆數----------------------------------------------------------------
    private string GetRptHead3()
    {
        //----------------------------------------------------------------------
        string[] O3 = new string[5];
        O3[0] = getCnt3("猥褻");
        O3[1] = getCnt3("攻擊");
        O3[2] = getCnt3("沉默");
        O3[3] = getCnt3("精神錯亂");
        O3[4] = getCnt3("其他");

        int sum = int.Parse(O3[0]) + int.Parse(O3[1]) + int.Parse(O3[2]) + int.Parse(O3[3]) + int.Parse(O3[4]);
        //設定框架----------------------------------------------------------------------
        string strTemp = "";
        HtmlTable table = new HtmlTable();
        HtmlTableRow row;
        HtmlTableCell cell;
        CssStyleCollection css;
        string strColor = "BLACK";
        string FontSize = "14px";
        table.Border = 0;
        table.CellPadding = 0;
        table.CellSpacing = 0;
        table.Align = "center";
        css = table.Style;
        css.Add("font-size", "12px");
        css.Add("font-family", "標楷體");
        css.Add("width", "100%");
        //----------------------------------------------------------
        if (sum != 0)
        {
            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            strTemp = "騷擾電話";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "28px");
            css.Add("font-family", "標楷體");
            css.Add("border-style", "none");
            cell.ColSpan = 2;
            row.Cells.Add(cell);
            table.Rows.Add(row);
        }
        /////////////////////////////////////////   第一行 by apple  //////////////////////////////////////////////////////////////////
        //猥褻O3[0]----------------------------------------------------------
        row = new HtmlTableRow();
        if (int.Parse(O3[0]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "猥褻";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 1234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //攻擊O3[1]----------------------------------------------------------------
        if (int.Parse(O3[1]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "攻擊";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //沉默O3[2]----------------------------------------------------------------
        if (int.Parse(O3[2]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "沉默";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //精神錯亂O3[3]----------------------------------------------------------------
        if (int.Parse(O3[3]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "精神錯亂";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //其他O3[4]----------------------------------------------------------------
        if (int.Parse(O3[4]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "其他";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
         //----------------
        table.Rows.Add(row);
        /////////////////////////////////////////   第二行by apple  //////////////////////////////////////////////////////////////////
        //猥褻O3[0]----------------------------------------------------------
        row = new HtmlTableRow();
        if (int.Parse(O3[0]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O3[0];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 134, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //攻擊O3[1]----------------------------------------------------------------
        if (int.Parse(O3[1]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O3[1];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //沉默O3[2]----------------------------------------------------------------
        if (int.Parse(O3[2]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O3[2];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "150px");
            row.Cells.Add(cell);
        }
        //精神錯亂O3[3]----------------------------------------------------------------
        if (int.Parse(O3[3]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O3[3];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //其他O3[4]----------------------------------------------------------------
        if (int.Parse(O3[4]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O3[4];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //----------------
        table.Rows.Add(row);
        //*************************************
        //轉成 html 碼
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        table.RenderControl(htw);

        return htw.InnerWriter.ToString();
    }
    //查詢SLQ特殊諮詢-------------------------------------------------------------------
    private string getCnt4(string Special)
    {

        //讀取資料庫
        string strSql = @" 
                            select count(*) as sum
                            from consulting
                            where Special like @Special
                          ";
        if (txtBegCreateDate.Text.Trim() != "" && txtEndCreateDate.Text.Trim() != "")
        {
            strSql += " and CONVERT(varchar(100), CreateDate, 111) Between @BegCreateDate And @EndCreateDate\n";
        }
        DataRow dr = null;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("Special", "%" + Special + "%");
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
    //顯示特殊諮詢  筆數----------------------------------------------------------------
    private string GetRptHead4()
    {
        //----------------------------------------------------------------------
        string[] O4 = new string[3];
        O4[0] = getCnt4("感恩祝福");
        O4[1] = getCnt4("宗教問題");
        O4[2] = getCnt4("代禱");


        int sum = int.Parse(O4[0]) + int.Parse(O4[1]) + int.Parse(O4[2]);
        //設定框架----------------------------------------------------------------------
        string strTemp = "";
        HtmlTable table = new HtmlTable();
        HtmlTableRow row;
        HtmlTableCell cell;
        CssStyleCollection css;
        string strColor = "BLACK";
        string FontSize = "14px";
        table.Border = 0;
        table.CellPadding = 0;
        table.CellSpacing = 0;
        table.Align = "center";
        css = table.Style;
        css.Add("font-size", "12px");
        css.Add("font-family", "標楷體");
        css.Add("width", "100%");
        //----------------------------------------------------------
        if (sum != 0)
        {
            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            strTemp = "特殊諮詢";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "28px");
            css.Add("font-family", "標楷體");
            css.Add("border-style", "none");
            cell.ColSpan = 1;
            row.Cells.Add(cell);
            table.Rows.Add(row);
        }
        /////////////////////////////////////////   第一行 by apple  //////////////////////////////////////////////////////////////////
        //感恩祝福O4[0]----------------------------------------------------------
        row = new HtmlTableRow();
        if (int.Parse(O4[0]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "感恩祝福";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 1234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //宗教問題O4[1]----------------------------------------------------------------
        if (int.Parse(O4[1]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "宗教問題";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //代禱O4[2]----------------------------------------------------------------
        if (int.Parse(O4[2]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "代禱";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //----------------
        table.Rows.Add(row);
        /////////////////////////////////////////   第二行by apple  //////////////////////////////////////////////////////////////////
        //感恩祝福O4[0]----------------------------------------------------------
        row = new HtmlTableRow();
        if (int.Parse(O4[0]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O4[0];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 134, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //宗教問題O4[1]----------------------------------------------------------------
        if (int.Parse(O4[1]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O4[1];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //代禱O4[2]----------------------------------------------------------------
        if (int.Parse(O4[2]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O4[2];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "150px");
            row.Cells.Add(cell);
        }
        //----------------
        table.Rows.Add(row);
        //*************************************
        //轉成 html 碼
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        table.RenderControl(htw);

        return htw.InnerWriter.ToString();
    }
    //查詢SLQ轉介單位(告知電話)---------------------------------------------------------
    private string getCnt5(string IntroductionUnit)
    {

        //讀取資料庫
        string strSql = @" 
                            select count(*) as sum
                            from consulting
                            where IntroductionUnit like @IntroductionUnit
                          ";
        if (txtBegCreateDate.Text.Trim() != "" && txtEndCreateDate.Text.Trim() != "")
        {
            strSql += " and CONVERT(varchar(100), CreateDate, 111) Between @BegCreateDate And @EndCreateDate\n";
        }
        DataRow dr = null;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("IntroductionUnit", "%" + IntroductionUnit + "%");
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
    //顯示轉介單位(告知電話)  筆數--------------------------------------------------------
    private string GetRptHead5()
    {
        //----------------------------------------------------------------------
        string[] O5 = new string[7];
        O5[0] = getCnt5("諮商協談");
        O5[1] = getCnt5("未婚懷孕");
        O5[2] = getCnt5("各癮症");
        O5[3] = getCnt5("社會福利");
        O5[4] = getCnt5("法律諮商");
        O5[5] = getCnt5("精神醫療");
        O5[6] = getCnt5("性問題");
        int sum = int.Parse(O5[0]) + int.Parse(O5[1]) + int.Parse(O5[2]) + int.Parse(O5[3]) + int.Parse(O5[4]) + int.Parse(O5[5]) + int.Parse(O5[6]);
        //設定框架----------------------------------------------------------------------
        string strTemp = "";
        HtmlTable table = new HtmlTable();
        HtmlTableRow row;
        HtmlTableCell cell;
        CssStyleCollection css;
        string strColor = "BLACK";
        string FontSize = "14px";
        table.Border = 0;
        table.CellPadding = 0;
        table.CellSpacing = 0;
        table.Align = "center";
        css = table.Style;
        css.Add("font-size", "12px");
        css.Add("font-family", "標楷體");
        css.Add("width", "100%");
        //----------------------------------------------------------
        if (sum != 0)
        {
            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            strTemp = "轉介單位(告知電話)";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "28px");
            css.Add("font-family", "標楷體");
            css.Add("border-style", "none");
            cell.ColSpan = 2;
            row.Cells.Add(cell);
            table.Rows.Add(row);
        }
        /////////////////////////////////////////   第一行 by apple  //////////////////////////////////////////////////////////////////
        //諮商協談O5[0]----------------------------------------------------------
        row = new HtmlTableRow();
        if (int.Parse(O5[0]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "諮商協談";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 1234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //未婚懷孕O5[1]----------------------------------------------------------------
        if (int.Parse(O5[1]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "未婚懷孕";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //各癮症O5[2]----------------------------------------------------------------
        if (int.Parse(O5[2]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "各癮症";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //社會福利O5[3]----------------------------------------------------------------
        if (int.Parse(O5[3]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "社會福利";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //法律諮商O5[4]----------------------------------------------------------------
        if (int.Parse(O5[4]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "法律諮商";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //精神醫療O5[5]----------------------------------------------------------------
        if (int.Parse(O5[5]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "精神醫療";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //性問題O5[6]----------------------------------------------------------------
        if (int.Parse(O5[6]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "性問題";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //----------------
        table.Rows.Add(row);
        /////////////////////////////////////////   第二行by apple  //////////////////////////////////////////////////////////////////
        //諮商協談O5[0]----------------------------------------------------------
        row = new HtmlTableRow();
        if (int.Parse(O5[0]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O5[0];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 134, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //未婚懷孕O5[1]----------------------------------------------------------------
        if (int.Parse(O5[1]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O5[1];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //各癮症O5[2]----------------------------------------------------------------
        if (int.Parse(O5[2]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O5[2];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "150px");
            row.Cells.Add(cell);
        }
        //社會福利O5[3]----------------------------------------------------------------
        if (int.Parse(O5[3]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O5[3];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //法律諮商O5[4]----------------------------------------------------------------
        if (int.Parse(O5[4]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O5[4];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //精神醫療O5[5]----------------------------------------------------------------
        if (int.Parse(O5[5]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O5[5];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //性問題O5[6]----------------------------------------------------------------
        if (int.Parse(O5[6]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O5[6];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //----------------
        table.Rows.Add(row);
        //*************************************
        //轉成 html 碼
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        table.RenderControl(htw);

        return htw.InnerWriter.ToString();
    }
    //查詢SLQ緊急單位(撥打電話)---------------------------------------------------------
    private string getCnt6(string crashintroduction)
    {

        //讀取資料庫
        string strSql = @" 
                            select count(*) as sum
                            from consulting
                            where crashintroduction like @crashintroduction
                          ";
        if (txtBegCreateDate.Text.Trim() != "" && txtEndCreateDate.Text.Trim() != "")
        {
            strSql += " and CONVERT(varchar(100), CreateDate, 111) Between @BegCreateDate And @EndCreateDate\n";
        }
        DataRow dr = null;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("crashintroduction","%"+ crashintroduction + "%");
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
    //顯示緊急單位(撥打電話)  筆數--------------------------------------------------------
    private string GetRptHead6()
    {
        //----------------------------------------------------------------------
        string[] O6 = new string[8];
        O6[0] = getCnt6("自殺");
        O6[1] = getCnt6("企圖自殺");
        O6[2] = getCnt6("緊急求救");
        O6[3] = getCnt6("暴力");
        O6[4] = getCnt6("隱症");
        O6[5] = getCnt6("性侵");
        O6[6] = getCnt6("身心疾病");
        O6[7] = getCnt6("求助");
        int sum = int.Parse(O6[0]) + int.Parse(O6[1]) + int.Parse(O6[2]) + int.Parse(O6[3]) + int.Parse(O6[4]) + int.Parse(O6[5]) + int.Parse(O6[6]) + int.Parse(O6[7]);
        //設定框架----------------------------------------------------------------------
        string strTemp = "";
        HtmlTable table = new HtmlTable();
        HtmlTableRow row;
        HtmlTableCell cell;
        CssStyleCollection css;
        string strColor = "BLACK";
        string FontSize = "14px";
        table.Border = 0;
        table.CellPadding = 0;
        table.CellSpacing = 0;
        table.Align = "center";
        css = table.Style;
        css.Add("font-size", "12px");
        css.Add("font-family", "標楷體");
        css.Add("width", "100%");
        //----------------------------------------------------------
        if (sum != 0)
        {
            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            strTemp = "緊急單位(撥打電話)";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "28px");
            css.Add("font-family", "標楷體");
            css.Add("border-style", "none");
            cell.ColSpan = 2;
            row.Cells.Add(cell);
            table.Rows.Add(row);
        }
        /////////////////////////////////////////   第一行 by apple  //////////////////////////////////////////////////////////////////
        //自殺O6[0]----------------------------------------------------------
        row = new HtmlTableRow();
        if (int.Parse(O6[0]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "自殺";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 1234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //企圖自殺O6[1]----------------------------------------------------------------
        if (int.Parse(O6[1]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "企圖自殺";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //緊急求救O6[2]----------------------------------------------------------------
        if (int.Parse(O6[2]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "緊急求救";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //暴力O6[3]----------------------------------------------------------------
        if (int.Parse(O6[3]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "暴力";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //隱症O6[4]----------------------------------------------------------------
        if (int.Parse(O6[4]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "隱症";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //性侵O6[5]----------------------------------------------------------------
        if (int.Parse(O6[5]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "性侵";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //身心疾病O6[6]----------------------------------------------------------------
        if (int.Parse(O6[6]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "身心疾病";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //求助O6[7]----------------------------------------------------------------
        if (int.Parse(O6[7]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = "求助";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 234, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //----------------
        table.Rows.Add(row);
        /////////////////////////////////////////   第二行by apple  //////////////////////////////////////////////////////////////////
        //自殺O6[0]----------------------------------------------------------
        row = new HtmlTableRow();
        if (int.Parse(O6[0]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O6[0];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 134, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //企圖自殺O6[1]----------------------------------------------------------------
        if (int.Parse(O6[1]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O6[1];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //緊急求助O6[2]----------------------------------------------------------------
        if (int.Parse(O6[2]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O6[2];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "150px");
            row.Cells.Add(cell);
        }
        //暴力O6[3]----------------------------------------------------------------
        if (int.Parse(O6[3]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O6[3];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //隱症O6[4]----------------------------------------------------------------
        if (int.Parse(O6[4]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O6[4];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //性侵O6[5]----------------------------------------------------------------
        if (int.Parse(O6[5]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O6[5];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //身心疾病O6[6]----------------------------------------------------------------
        if (int.Parse(O6[6]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O6[6];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //求助O6[7]----------------------------------------------------------------
        if (int.Parse(O6[7]) != 0)
        {
            cell = new HtmlTableCell();
            strTemp = O6[7];
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "18px");
            css.Add("font-family", "標楷體");
            Util.AddTDLine(css, 34, strColor);
            css.Add("width", "130px");
            row.Cells.Add(cell);
        }
        //----------------
        table.Rows.Add(row);
        //*************************************
        //轉成 html 碼
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        table.RenderControl(htw);

        return htw.InnerWriter.ToString();
    }
}
