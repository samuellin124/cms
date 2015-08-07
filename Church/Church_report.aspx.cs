using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class Report_ScheduleRpt : System.Web.UI.Page
{

    string FontSize = "16px";
    string LineHeight = "24px";
    string TableWidth = "800px";
    string strFontSize = "12px";
    int intRow = 4;
    DataTable dtDept;

    string[] Items = {
                        "星期",
                        "星期一",
                        "星期二",
                        "星期三",
                        "星期四",
                        "星期五",
                        "星期六",
                        "星期日",
                        
                   };
    protected void Page_Load(object sender, EventArgs e)
    {
        HFD_Uid.Value = Util.GetQueryString("Uid");
       // DataTable dt = GetTableTitle();
       // lblGridList.Text  = PrintReportByDataTable(dt);
        PrintReport();


    }
  
    //抬頭
    private string GetRptHead()
    {
        string strTemp = "";
        HtmlTable table = new HtmlTable();
        HtmlTableRow row;
        HtmlTableCell cell;
        CssStyleCollection css;

        FontSize = "14px";
        table.Border = 0;
        table.CellPadding = 0;
        table.CellSpacing = 0;
        table.Align = "center";
        css = table.Style;
        css.Add("font-size", "12px");
        css.Add("font-family", "標楷體");
        css.Add("width", "100%");

        //--------------------------------------------
        row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "103年熱線志工班表  第一季 ";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "24px");
        css.Add("font-family", "標楷體");
        css.Add("border-style", "none");
        css.Add("font-weight", "bold");
        //cell.ColSpan = 11;
        row.Cells.Add(cell);
        table.Rows.Add(row);
        //--------------------------------------------
        //轉成 html 碼
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        table.RenderControl(htw);

        return htw.InnerWriter.ToString();
       }


    private string GetTable(string strTitle)
    {

        DataTable dt = GetDataTable();
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

       // css.Add("line-height", TableWidth);


        //intRow = intRow + dt.Rows.Count;


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


            row.Cells.Add(cell);

        }

        cell = new HtmlTableCell();
        string strTemp1 = "";// "" + strTitle + "";
        cell.InnerHtml = strTemp1;
        css = cell.Style;
        css.Add("text-align", "left");
        css.Add("font-size", "12px");
        //cell.RowSpan = intRow;
        cell.Width = "8";
        row.Cells.Add(cell);
        table.Rows.Add(row);

        //--------------------------------------------
        //第2行,資料顯示

        row = new HtmlTableRow();
        cell = new HtmlTableCell();
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 123);
        cell.InnerHtml = "morning";

        row = new HtmlTableRow();
        cell = new HtmlTableCell();
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 123);
        cell.InnerHtml = "";

        foreach (DataRow dr in dt.Rows)
        {
            row = new HtmlTableRow();

            for (int i = 0; i < Items.Length; i++)
            {
                string Align = "left";

                cell = new HtmlTableCell();
                strTemp = dr["UserName"].ToString();
                cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
                css = cell.Style;
                css.Add("text-align", Align);
                css.Add("font-size", strFontSize);
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
                //if (i == 8)
                //{
                //    Util.AddTDLine(css, 23);
                //}
                //if (i == 9)
                //{
                //    Util.AddTDLine(css, 23);
                //}
               
                row.Cells.Add(cell);
            }
            //cell.InnerHtml = "A";
            //row.Cells.Add(cell);
            table.Rows.Add(row);
        }

               //table.Rows.Add(row);

        //轉成 html 碼
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        table.RenderControl(htw);
        return htw.InnerWriter.ToString();
    }

    private string GetTableA()
    {

        DataTable dt = GetDataTable();
        //組 table
        string strTemp = "";
        HtmlTable table = new HtmlTable();
        HtmlTableRow row;
        HtmlTableCell cell;
        CssStyleCollection css;

        table.Border = 0;
        //table.BorderColor = "black";
        table.CellPadding = 0;
        table.CellSpacing = 0;
        css = table.Style;
        css.Add("font-size", "12px");
        css.Add("font-family", "標楷體");
        // css.Add("width", "0");

        css.Add("line-height", LineHeight);


        intRow = intRow + dt.Rows.Count;


        //第1行
        row = new HtmlTableRow();

        for (int i = 0; i < Items.Length; i++)
        {
            cell = new HtmlTableCell();
            strTemp = Items[i].ToString();
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;


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

            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            row.Cells.Add(cell);

        }

        cell = new HtmlTableCell();
        string strTemp1 = "第<br>二<br>聯<br>&nbsp<br> 會<br>計<br>聯<br>︵<br>此<br>聯<br>由<br>會<br>計<br>單<br>位<br>存<br>查<br>︶";
        cell.InnerHtml = strTemp1;

        //cell.Width = "500px";

        css = cell.Style;

        css.Add("text-align", "left");
        css.Add("font-size", "12px");

        //css.Add("line-height", "20px");
        //css.Add("width", "10px");
        //cell.Width = "5px";
        cell.RowSpan = intRow;
        cell.Width = "8";
        row.Cells.Add(cell);
        table.Rows.Add(row);

        //--------------------------------------------
        //第2行,資料顯示
        foreach (DataRow dr in dt.Rows)
        {
            row = new HtmlTableRow();

            for (int i = 0; i < Items.Length; i++)
            {
                string Align = "left";

                cell = new HtmlTableCell();
                strTemp = dr[Items[i]].ToString();
                cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
                css = cell.Style;
                css.Add("text-align", Align);
                css.Add("font-size", strFontSize);
                //cell.Width  = "10";
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
                row.Cells.Add(cell);
            }

            table.Rows.Add(row);
        }


        //--------------------------------------------
        //table2
        //組 table

        table.Border = 0;
        table.CellPadding = 3;
        table.CellSpacing = 0;
        css = table.Style;
        css.Add("font-size", "12px");
        css.Add("font-family", "標楷體");
        css.Add("width", TableWidth);
        // css.Add("line-height", LineHeight);


        row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "經辦單位：";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "left");
        css.Add("font-size", strFontSize);
        cell.ColSpan = 2;
        Util.AddTDLine(css, 1234);
        //cell.Height = "20";


        row.Cells.Add(cell);


        cell = new HtmlTableCell();
        strTemp = "使用單位：";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "left");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 234);
        cell.ColSpan = 2;
        row.Cells.Add(cell);


        cell = new HtmlTableCell();
        strTemp = "會 計 單 位" + "&nbsp &nbsp &nbsp &nbsp &nbsp &nbsp";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        cell.ColSpan = 6;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 234);
        row.Cells.Add(cell);
        table.Rows.Add(row);





        table.Border = 0;
        table.CellPadding = 0;
        table.CellSpacing = 0;
        css = table.Style;
        css.Add("font-size", "12px");
        css.Add("font-family", "標楷體");
        css.Add("width", TableWidth);
        css.Add("line-height", LineHeight);
        //Util.AddTDLine(css, 1234);

        row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "主管";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "left");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 134);
        row.Cells.Add(cell);
        // table.Rows.Add(row);

        cell = new HtmlTableCell();
        strTemp = "經辦";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "left");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 34);
        row.Cells.Add(cell);
        //table.Rows.Add(row);

        cell = new HtmlTableCell();
        strTemp = "主管";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "left");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 34);
        row.Cells.Add(cell);
        //table.Rows.Add(row);

        cell = new HtmlTableCell();
        strTemp = "經辦";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "left");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 34);
        // cell.ColSpan = 3;
        row.Cells.Add(cell);
        //table.Rows.Add(row);

        cell = new HtmlTableCell();
        strTemp = "主管";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "left");
        css.Add("font-size", strFontSize);
        row.Cells.Add(cell);
        Util.AddTDLine(css, 34);
        // table.Rows.Add(row);

        cell = new HtmlTableCell();
        strTemp = "財產登記";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "left");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 34);
        row.Cells.Add(cell);
        // table.Rows.Add(row);

        cell = new HtmlTableCell();
        strTemp = "審核";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "left");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 34);
        row.Cells.Add(cell);
        //table.Rows.Add(row);

        cell = new HtmlTableCell();
        strTemp = "費用編號";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "left");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 34);
        row.Cells.Add(cell);
        //table.Rows.Add(row);

        cell = new HtmlTableCell();
        strTemp = "&nbsp&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "left");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 34);
        cell.ColSpan = 2;

        row.Cells.Add(cell);
        table.Rows.Add(row);



        row = new HtmlTableRow();

        for (int i = 0; i < 9; i++)
        {
            cell = new HtmlTableCell();
            if (i == 7)
            {
                strTemp = "預算編號";
            }
            else
            {
                strTemp = "&nbsp";
            }
            //strTemp = "&nbsp;";
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "left");

            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 34);
            if (i == 0)
            {
                Util.AddTDLine(css, 13);
            }
            if (i == 8)
            {
                cell.ColSpan = 2;
            }
            cell.Height = "20";
            row.Cells.Add(cell);
        }

        table.Rows.Add(row);

        //轉成 html 碼
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        table.RenderControl(htw);
        return htw.InnerWriter.ToString();
    }




    private void PrintReport()
    {
        lblGridList.Text = GetRptHead() + GetTable("");
        //GridList2.Text = GetTable1() + GetTable("第二聯　會計聯"); //+ GetTable2() + GetTable3();
        //if (HFD_PrintType.Value == "Excel")
        //{
        //    Util.OutputTxt(GridList.Text, "1", DateTime.Now.ToString("yyyyMMddHHmmss"));
        //}
        //size:841.7pt 595.45pt;mso-page-orientation:landscape
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
        //sb.AppendLine(MakeReport());
        //Util.OutputTxt(sb, "2", DateTime.Now.ToString("yyyyMMddHHmmss"));



        //Util.OutputTxt(sb + GridList.Text + GridList2.Text , "2", DateTime.Now.ToString("yyyyMMddHHmmss"));
        //Util.OutputTxt(sb + GridList.Text + GridList2.Text, "2", DateTime.Now.ToString("yyyyMMddHHmmss"));

    }

    private DataTable GetDataTable()
    {
        string strSql = @"
                        select top 3 UserName,DayOfWeek+periodID As DayClass from Schedule sh 
                        inner join ScheduleMap sm on sh.uid = sm.ScheduleID 
                        inner join AdminUser AU on au.UserID=sm.UserID 
                        where [Year] = '2014' And Quarter ='1' 
                        order by DayOfWeek,periodID
                        ";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        //dict.Add("MoveMainUid", HFD_Uid.Value);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        return dt;
    }
    private string PrintReportByDataTable(DataTable dt)
    {
        //組 table
        string strTemp = "";
        HtmlTable table = new HtmlTable();
        HtmlTableRow row;
        HtmlTableCell cell;
        CssStyleCollection css;

        table.Border = 1;
        table.CellPadding = 3;
        table.CellSpacing = 0;
        css = table.Style;
        css.Add("font-size", "12px");
        css.Add("font-family", "標楷體");
        css.Add("width", "100%");
        css.Add("line-height", "24px");

        string strFontSize = "16px";
        //--------------------------------------------
        //第1行

        row = new HtmlTableRow();
        for (int i = 0; i < dt.Columns.Count; i++)
        {
            cell = new HtmlTableCell();
            strTemp = dt.Columns[i].ColumnName;
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            css.Add("font-weight", "bold");
            css.Add("vertical-align", "middle");
            row.Cells.Add(cell);
        }
        table.Rows.Add(row);
        //--------------------------------------------
        //第2行,資料顯示
        foreach (DataRow dr in dt.Rows)
        {
            row = new HtmlTableRow();

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                string Align = "center";
                cell = new HtmlTableCell();
                strTemp = dr[dt.Columns[i].ColumnName].ToString();

                if (strTemp.Contains("H"))
                {
                    continue;
                }

                cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
                css = cell.Style;

                css.Add("text-align", Align);
                css.Add("font-size", strFontSize);
                css.Add("line-height", "24px");
                if (dt.Columns[i].ColumnName == "年度")
                {
                    if (strTemp == "合計")
                    {
                        cell.RowSpan = 2;
                    }
                    else
                    {
                        cell.RowSpan = 8;
                    }
                }
                if (dt.Columns[i].ColumnName == "出租對象"
                    || dt.Columns[i].ColumnName == "租金金額")
                {
                    cell.RowSpan = 2;
                }
                row.Cells.Add(cell);
            }
            table.Rows.Add(row);
        }
        //--------------------------------------------
        //轉成 html 碼
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        table.RenderControl(htw);

        return htw.InnerWriter.ToString();
    }

    private DataTable GetTableTitle()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("星期");
        dt.Columns.Add("星期一");
        dt.Columns.Add("星期二");
        dt.Columns.Add("星期三");
        dt.Columns.Add("星期四");
        dt.Columns.Add("星期五");
        dt.Columns.Add("星期六");
        dt.Columns.Add("星期日");
        return dt;
    }
    //---------------------------------------------------------------------------------------
}