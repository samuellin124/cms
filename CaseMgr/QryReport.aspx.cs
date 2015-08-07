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

public partial class CaseMgr_QryReport :  BasePage 
{
    GroupAuthrity ga = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        Session["ProgID"] = "Consult";
        HFD_MemberID.Value = Util.GetQueryString("uid");
        lblReport.Text = GetRptHead() + GetReport();
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
                  select ISNULL((SELECT [Name] FROM CodeCity WHERE CodeCity.Zipcode = City),'')  + ISNULL((SELECT Name FROM CodeCity WHERE CodeCity.zipcode = b.ZipCode),'') + [address] as FullAddress ,b.Memo as M_Memo,*
                        from Consulting a  
                        inner join Member b on a.MemberUID = b.uid
                        --left join CodeCity c on b.City = c.ZipCode 
                        --left join CodeCity d on b.ZipCode = d.ZipCode     
                  where 1=1 and isnull(a.IsDelete, '') != 'Y' ";
        strSql += "  And a.MemberUID =  " + HFD_MemberID.Value.ToString() + "";


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

        strSql += " Order by  CreateDate Desc";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("MemberUID", HFD_MemberID.Value.ToString());
        //dict.Add("Event", "%" + txtEvent.Text + "%");
        //dict.Add("CName", "%" + txtName.Text + "%");
        //dict.Add("Marry", "%" + HFD_Marry.Value + "%");
        //dict.Add("Phone", '%' + txtPhone.Text + '%');
        //dict.Add("BegCreateDate", txtBegCreateDate.Text);
        //dict.Add("EndCreateDate", txtEndCreateDate.Text);
        //dict.Add("ServiceUser", txtServiceUser.Text);
        //dict.Add("EditServiceUser", SessionInfo.UserID.ToString());
        //dict.Add("ZipCode", txtZipCode.Text);
        //dict.Add("City", ddlCity.SelectedValue);
        //dict.Add("Address", "%" + txtAddress.Text + "%");
        //dict.Add("ConsultantItem", "%" + HFD_ConsultantItem.Value.ToString().TrimEnd(',') + "%");
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
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        //table.Border = 0;
        //table.BorderColor = "black";
        //table.CellPadding = 0;
        //table.CellSpacing = 0;
        //table.Align = "center";
        //css = table.Style;
        //css.Add("font-size", "32pt");
        //css.Add("font-family", "標楷體");
        //css.Add("width", "900px");
        //--------------------------------------------
        DataTable dt = GetDataTable();

        if (dt.Rows.Count == 0)
        {
            lblReport.Text = "";
            ShowSysMsg("查無資料");
            return "";
        }

        foreach (DataRow dr in dt.Rows)
        {
            string strFontSize = "16px";
         
            table = new HtmlTable();
            table.Border = 1;
            table.BorderColor = "black";
            table.CellPadding = 0;
            table.CellSpacing = 0;
            table.Align = "center";
            css = table.Style;
            css.Add("font-size", strFontSize);
            css.Add("font-family", "標楷體");
            css.Add("width", "900px");

            string strColor = "lightgrey";  //字的顏色
            string strBackGround = "darkseagreen"; //cell 顏色
            string strMarkColor = "'background-color:'''"; // 字的底色
            string strMemberColor = "color:crimson"; //聯絡人的字顏色
            string strWeight = "bold"; //字體
            string strHeight = "20px";//欄高

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("font-weight", strWeight);
            css.Add("height", "20px");
            Util.AddTDLine(css, 123, strColor);
            cell.InnerHtml = "1.電話：<span style= " + strMemberColor + " >" + dr["Phone"].ToString() + "</span>";// font-weight:bold;
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("font-weight", strWeight);
            Util.AddTDLine(css, 23, strColor);
            cell.InnerHtml = "4.年齡約：<span style=" + strMemberColor + "> " + dr["age"].ToString() + "</span>";
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("font-weight", strWeight);
            Util.AddTDLine(css, 23, strColor);
            cell.InnerHtml = "7.轉介教會：<span style=" + strMemberColor + ">" + dr["ChurchYN"].ToString() + "</span>";
            row.Cells.Add(cell);
            table.Rows.Add(row);


            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("font-weight", strWeight);
            css.Add("height", "25px");
            Util.AddTDLine(css, 123, strColor);
            cell.InnerHtml = "2.姓名：<span style=" + strMemberColor + " >" + dr["Cname"].ToString() + "</span> ";
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("font-weight", strWeight);
            Util.AddTDLine(css, 23, strColor);
            cell.InnerHtml = "5.婚姻：<span style=" + strMemberColor + " >" + Util.TrimLastChar(dr["Marry"].ToString(), ',') + "</span>&nbsp;";
            row.Cells.Add(cell);


            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("font-weight", strWeight);
            Util.AddTDLine(css, 23, strColor);
            cell.RowSpan = 2;
            //cell.ColSpan = 2;
            if (dr["FullAddress"].ToString() != "")
            {
                cell.InnerHtml = "8.地址：<span style=" + strMemberColor + " >" + dr["FullAddress"].ToString() + "</span> ";
            }
            else
            {
                cell.InnerHtml = "8.地址：<span style=" + strMemberColor + " >" + dr["Overseas"].ToString().TrimEnd(',') + "</span> ";
            }
            row.Cells.Add(cell);
            table.Rows.Add(row);



            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("font-weight", strWeight);
            css.Add("height", "30px");
            Util.AddTDLine(css, 123, strColor);
            cell.InnerHtml = "3.性別：<span style=" + strMemberColor + " >" + dr["Sex"].ToString() + "</span>";
            row.Cells.Add(cell);


            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("font-weight", strWeight);
            Util.AddTDLine(css, 23, strColor);
            cell.InnerHtml = "6.基督徒：<span style=" + strMemberColor + " >" + dr["Christian"].ToString() + "</span>";
            row.Cells.Add(cell);
            table.Rows.Add(row);
            //table.RenderControl(htw);

            //table = new HtmlTable();
            //table.Border = 0;
            //table.BorderColor = "black";
            //table.CellPadding = 0;
            //table.CellSpacing = 0;
            //table.Align = "center";
            //css = table.Style;
            //css.Add("font-size", strFontSize);
            //css.Add("font-family", "標楷體");
            //css.Add("width", "900px");

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("font-weight", strWeight);
            Util.AddTDLine(css, 123, strColor);
            cell.ColSpan = 3;
            cell.InnerHtml = "備註：<span style=" + strMemberColor + " >" + dr["M_Memo"].ToString() + "</span>";
            row.Cells.Add(cell);
            table.Rows.Add(row);
            table.RenderControl(htw);

            table = new HtmlTable();
            table.Border = 0;
            table.BorderColor = "black";
            table.CellPadding = 0;
            table.CellSpacing = 0;
            table.Align = "center";
            css = table.Style;
            css.Add("font-size", strFontSize);
            css.Add("font-family", "標楷體");
            css.Add("width", "900px");



            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("font-weight", strWeight);
            css.Add("height", "30px");
            Util.AddTDLine(css, 123, strColor);
            cell.ColSpan = 1;
            cell.InnerHtml = "志工：<span style=color:darkseagreen >" + dr["ServiceUser"].ToString() + "</span>";
            row.Cells.Add(cell);
            string TotalTalkTime = "";
            DateTime t1, t2;
            if (dr["CreateDate"].ToString() != "" && dr["EndDate"].ToString() != "")
            {
                t1 = DateTime.Parse(Util.DateTime2String(dr["CreateDate"].ToString(), DateType.yyyyMMddHHmmss, EmptyType.ReturnNull));
                t2 = DateTime.Parse(Util.DateTime2String(dr["EndDate"].ToString(), DateType.yyyyMMddHHmmss, EmptyType.ReturnNull));

                TotalTalkTime = DateDiff(t1, t2);
            }


            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("font-weight", strWeight);
            Util.AddTDLine(css, 23, strColor);
            cell.ColSpan = 3;
            cell.InnerHtml = "建檔時間：<span style=" + strMarkColor + " >" + Util.DateTime2String(dr["CreateDate"].ToString(), DateType.yyyyMMddHHmmss, EmptyType.ReturnNull) + " " + "</span> &nbsp;結束時間：<span style=" + strMarkColor + " >" + Util.DateTime2String(dr["EndDate"].ToString(), DateType.yyyyMMddHHmmss, EmptyType.ReturnNull) + "</span>&nbsp;" + TotalTalkTime;
            row.Cells.Add(cell);
            table.Rows.Add(row);

            table.RenderControl(htw);

            table = new HtmlTable();
            table.Border = 0;
            table.BorderColor = "black";
            table.CellPadding = 0;
            table.CellSpacing = 0;
            table.Align = "center";
            css = table.Style;
            css.Add("font-size", strFontSize);
            css.Add("font-family", "標楷體");
            css.Add("width", "900px");

          

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "26px");
            css.Add("background", strBackGround);
            Util.AddTDLine(css, 123, strColor);
            cell.Width = "88mm";
            cell.RowSpan = 6;
            cell.InnerHtml = "S";
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            css.Add("background", strBackGround);
            Util.AddTDLine(css, 23, strColor);
            cell.InnerHtml = "<span style=font-weight:bold>求助者的主訴(用第一人稱 '我' 敘述)</span>";
            cell.ColSpan = 3;
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            //css.Add("font-weight", strWeight);
            css.Add("height", strHeight);
            Util.AddTDLine(css, 23, strColor);
            cell.ColSpan = 3;
            cell.InnerHtml = "<span style=font-weight:bold>A(事件)：</span>" + dr["Event"].ToString() + "&nbsp;";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("height", strHeight);
            // css.Add("font-weight", strWeight);
            Util.AddTDLine(css, 23, strColor);
            cell.InnerHtml = "<span style=font-weight:bold>B(想法)：</span>" + dr["Think"].ToString() + "&nbsp;";
            cell.ColSpan = 3;
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("height", strHeight);
          //  css.Add("font-weight", strWeight);
            Util.AddTDLine(css, 23, strColor);
            cell.InnerHtml = "<span style=font-weight:bold>C(感受)：</span>" + dr["Feel"].ToString() + "&nbsp;";
            cell.ColSpan = 3;
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("height", strHeight);
            //css.Add("font-weight", strWeight);
            Util.AddTDLine(css, 23, strColor);
            cell.ColSpan = 3;
            cell.InnerHtml = "<span style=font-weight:bold>補充說明：</span>" + dr["Comment"].ToString() + "&nbsp;";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 23, strColor);
            cell.ColSpan = 3;
            cell.InnerHtml = "&nbsp;";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "26px");
            css.Add("background", strBackGround);
            Util.AddTDLine(css, 123, strColor);
            cell.RowSpan = 6;
            cell.InnerHtml = "O";
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            css.Add("background", strBackGround);
            //css.Add("font-weight", strWeight);
            Util.AddTDLine(css, 23, strColor);
            cell.ColSpan = 3;
            cell.InnerHtml = "<span style=font-weight:bold>客觀分析(諮商類別)</span>";
            row.Cells.Add(cell);
            table.Rows.Add(row);


            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            // css.Add("font-weight", strWeight)
            css.Add("height", strHeight); ;
            Util.AddTDLine(css, 23, strColor);
            cell.ColSpan = 3;
            cell.InnerHtml = "<span style=font-weight:bold>來電諮詢別(大類)：</span>" + Util.TrimLastChar(dr["ConsultantMain"].ToString(), ',') + "&nbsp;";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            // css.Add("font-weight", strWeight);
            css.Add("height", strHeight);
            Util.AddTDLine(css, 23, strColor);
            cell.ColSpan = 3;
            cell.InnerHtml = "<span style=font-weight:bold >來電諮詢別(分項)：</span>" + Util.TrimLastChar(dr["ConsultantItem"].ToString(), ',') + "&nbsp;";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("height", strHeight);
            // css.Add("font-weight", strWeight);
            Util.AddTDLine(css, 23, strColor);
            cell.ColSpan = 3;
            if (dr["HarassOther"].ToString() != "")
            {
                cell.InnerHtml = "<span style=font-weight:bold >騷擾電話：</span>" + Util.TrimLastChar(dr["HarassPhone"].ToString().Replace("其他", ""), ',').TrimEnd(',') + "<span style=font-weight:bold >其他： </span>" + dr["HarassOther"].ToString() + "&nbsp;";
                //cell.InnerHtml = "<span style=font-weight:bold >騷擾電話：</span>" + Util.TrimLastChar(dr["HarassPhone"].ToString(), ',') + "<span style=font-weight:bold >其他： </span>" + dr["HarassOther"].ToString() + "&nbsp;";
            }
            else
            {
                cell.InnerHtml = "<span style=font-weight:bold>騷擾電話：</span>" + Util.TrimLastChar(dr["HarassPhone"].ToString(), ',') + "&nbsp;";
            }

            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 23, strColor);
            css.Add("height", strHeight);
            // css.Add("font-weight", strWeight);
            cell.ColSpan = 3;
            cell.InnerHtml = "<span style= font-weight:bold>轉介單位(告知電話)：</span>" + Util.TrimLastChar(dr["IntroductionUnit"].ToString(), ',') + "&nbsp;";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            //   css.Add("font-weight", strWeight);
            css.Add("height", strHeight);
            Util.AddTDLine(css, 23, strColor);
            cell.ColSpan = 3;
            cell.InnerHtml = "<span style= font-weight:bold>緊急轉介(撥打電話)：</span>" + Util.TrimLastChar(dr["CrashIntroduction"].ToString(), ',') + "&nbsp;";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "26px");
            css.Add("background", strBackGround);
            css.Add("font-weight", strWeight);
            Util.AddTDLine(css, 123, strColor);
            cell.RowSpan = 2;
            cell.InnerHtml = "A";
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            css.Add("background", strBackGround);
            // css.Add("font-weight", strWeight);
            Util.AddTDLine(css, 23, strColor);
            cell.ColSpan = 3;
            cell.InnerHtml = "<span style=font-weight:bold >問題評估</span>&nbsp;";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("height", strHeight);
            Util.AddTDLine(css, 23, strColor);
            cell.ColSpan = 1;
            cell.InnerHtml = "<span style=" + strMarkColor + " > " + dr["Problem"].ToString() + " </span>&nbsp;";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            //row = new HtmlTableRow();
            //cell = new HtmlTableCell();
            //css = cell.Style;
            //css.Add("text-align", "left");
            //css.Add("font-size", strFontSize);
            //Util.AddTDLine(css, 23);
            //cell.ColSpan = 3;
            //cell.InnerHtml = "2.因" + dr["Reason2"].ToString() + "造成" + dr["Trouble2"].ToString() + "問題";
            //row.Cells.Add(cell);
            //table.Rows.Add(row);

            //row = new HtmlTableRow();
            //cell = new HtmlTableCell();
            //css = cell.Style;
            //css.Add("text-align", "left");
            //css.Add("font-size", strFontSize);
            //Util.AddTDLine(css, 23);
            //cell.ColSpan = 3;
            //cell.InnerHtml = "3.因" + dr["Reason3"].ToString() + "造成" + dr["Trouble3"].ToString() + "問題";
            //row.Cells.Add(cell);
            //table.Rows.Add(row);


            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", "26px");
            css.Add("background", strBackGround);
            css.Add("height", strHeight);
            //css.Add("font-weight", strWeight);
            Util.AddTDLine(css, 1234, strColor);
            cell.RowSpan = 8;
            cell.InnerHtml = "P";
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            css.Add("background", strBackGround);                 
            //css.Add("font-weight", strWeight);
            Util.AddTDLine(css, 23, strColor);
            cell.ColSpan = 3;
            cell.InnerHtml = "<span style= font-weight:bold>計畫</span>";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            // css.Add("font-weight", strWeight);
            css.Add("height", strHeight);
            Util.AddTDLine(css, 23, strColor);
            cell.ColSpan = 3;
            cell.InnerHtml = "<span style=font-weight:bold >1.找出案主心中的假設：</span>" + dr["FindAssume"].ToString() + "</span>&nbsp;";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            //css.Add("font-weight", strWeight);
            css.Add("height", strHeight);
            Util.AddTDLine(css, 23, strColor);
            cell.ColSpan = 3;
            cell.InnerHtml = "<span style=font-weight:bold >2.與案主討論與解釋假設： </span>" + dr["Discuss"].ToString() + "</span>&nbsp;";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("height", strHeight);
            // css.Add("font-weight", strWeight);
            Util.AddTDLine(css, 23, strColor);
            cell.ColSpan = 3;
            cell.InnerHtml = "<span style=font-weight:bold >3.加入新的元素：</span>" + dr["Element"].ToString() + "&nbsp;";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("height", strHeight);
            //css.Add("font-weight", strWeight);
            Util.AddTDLine(css, 23, strColor);
            cell.ColSpan = 3;
            cell.InnerHtml = " <span style=font-weight:bold >4.改變案主原先的期待：</span>" + dr["Expect"].ToString() + "&nbsp;";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("height", strHeight);
            // css.Add("font-weight", strWeight);
            Util.AddTDLine(css, 23, strColor);
            cell.InnerHtml = " <span style=font-weight:bold >5.給予案主祝福與盼望：</span>" + dr["Blessing"].ToString() + "&nbsp;";
            cell.ColSpan = 3;
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("height", strHeight);
            // css.Add("font-weight", strWeight);
            Util.AddTDLine(css, 23, strColor);
            cell.ColSpan = 3;
            cell.InnerHtml = "<span style=font-weight:bold >6.帶領決志禱告：</span>" + dr["IntroductionChurch"].ToString() + "&nbsp;";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("height", strHeight);
            //  css.Add("font-weight", strWeight);
            Util.AddTDLine(css, 234, strColor);
            cell.ColSpan = 3;
            cell.InnerHtml = "<span style=font-weight:bold >7.我對個案的幫助程度：</span>" + dr["HelpLvMark"].ToString() + "&nbsp;";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("height", strHeight);
            css.Add("font-size", strFontSize);
            //css.Add("font-weight", strWeight);
            Util.AddTDLine(css, 134, strColor);
            cell.ColSpan = 6;
            cell.InnerHtml = "<span style=font-weight:bold >小叮嚀：</span>" + dr["Memo"].ToString() + "&nbsp;";
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
            table.RenderControl(htw);
        }
        //轉成 html 碼========================================================================
       
        return htw.InnerWriter.ToString();
    }
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect(Util.RedirectByTime("ConsultQry.aspx"));
    }

    private string DateDiff(DateTime DateTime1, DateTime DateTime2)
    {
        string dateDiff = null;
        TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
        TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
        TimeSpan ts = ts1.Subtract(ts2).Duration();
        dateDiff = "通話時間共：<span style=color:red>" + (int.Parse(ts.Days.ToString()) * 1440 + int.Parse(ts.Hours.ToString()) * 60 + int.Parse(ts.Minutes.ToString())).ToString() + "分</span>";
        return dateDiff;
    }

}