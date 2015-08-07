using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Threading;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Net.Mail;

public partial class sysMgr_Footer_Mail : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        HFD_UID.Value = Util.GetQueryString("uid");
        string strSql;
        string strEmail, strContact, context;

        DataRow dr = null;
        DataTable dt = null;
        Dictionary<string, object> dict = new Dictionary<string, object>();


        strSql = @"
                   select *
                   from Church
                   where uid=@uid
                   and isnull(IsDelete, '') != 'Y'
                  ";
        dict.Add("uid", HFD_UID.Value);
        dt = NpoDB.GetDataTableS(strSql, dict);
        //資料異常
        if (dt.Rows.Count == 0)
        {
            return;
        }
        dr = dt.Rows[0];

        strEmail = dr["Email"].ToString();
        strContact = dr["Contact"].ToString() + "   平安：<BR>";
        strContact += " 感謝貴教會加入夥伴教會，<BR>";
        strContact += " 未來有非基督徒來電者，<BR>";
        strContact += " 居住於貴教會附近，<BR>";
        strContact += " 並有意願進入教會時，<BR>";
        strContact += " 我們會以 email方式寄給您，<BR>";
        strContact += " 並請派員關懷聯繫，<BR>";
        strContact += " 謝謝您！<BR>";
        strContact += " 家庭事工部副主任 孔令峯敬上";



        //轉出word
        context = GetRptHead() + GetReport();
        //畫面顯示
        if (!IsPostBack)
        {
            txtEmailTo.Text = strEmail;
            txtSubject.Text = "轉介教會";
            txtBody.Text = strContact;
          //  Literal1.Text =  context;
            
        }
        
    }

    protected void btnSend_Click(object sender, EventArgs e)
    {

        //---------------------------------------------------------------
        SendEMailObject mailObj = new SendEMailObject();
        string strfile = "";

        //MailFrom寄件人要與SmtpServer對應
        mailObj.MailFrom = System.Web.Configuration.WebConfigurationManager.AppSettings["SmtpFrom"];// "edm@mail.goodtv.tv";
        mailObj.SmtpServer = System.Web.Configuration.WebConfigurationManager.AppSettings["SmtpHost"];//"smtp1.goodtv.tv";
        mailObj.SmtpAccount = System.Web.Configuration.WebConfigurationManager.AppSettings["SmtpAccount"];//"edm@mail.goodtv.tv";
        mailObj.SmtpPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["SmtpPassword"];//"12341234";
        //strfile = FileUPAttachPath.PostedFile.FileName;
        //  strfile = FileUPAttachPath.FileName;

        //若需附加檔案，將檔案路徑及檔名加入List<string>
        List<string> strAttFile = new List<string>();
        string tmpfile= FileUPAttachPath.PostedFile.FileName;
        strAttFile.Add(tmpfile);
        //strAttFile.Add(strfile);
        mailObj.lstAttachPath = strAttFile;

        mailObj.MailTo =  txtEmailTo.Text;
        mailObj.Subject = "轉介教會";
        mailObj.Body = txtBody.Text;


        int ErrorCode = mailObj.SendMail();
        if (ErrorCode == 0)
        {
            Session["Msg"] = "發送Mail成功!!!";
        }
        else
        {
            Session["Msg"] = "發送Mail失敗 [" + mailObj.ErrorMessage + "]";
        }
        ShowSysMsg();

        //---------------------------------------------------------------
        //ClientScript.RegisterStartupScript(this.GetType(), "mailto", "<script type = 'text/javascript'>parent.location='mailto:" + strEmail + "&subject=這是主旨&body=" + lblEmail.Text  + "'</script>");    
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
        strTemp = "好消息家庭關懷專線個案轉介&nbsp;";// "" + ddlYear.SelectedValue + "年熱線志工班表  第 " + ddlQuerter.SelectedValue + " 季 ";
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
                  select top 1 ISNULL((SELECT [Name] FROM CodeCity WHERE CodeCity.Zipcode = City),'')  + ISNULL((SELECT Name FROM CodeCity WHERE CodeCity.zipcode = b.ZipCode),'') + [address] as FullAddress ,b.Memo as M_Memo,*
                        from Consulting a  
                        inner join Member b on a.MemberUID = b.uid
                        --left join CodeCity c on b.City = c.ZipCode 
                        --left join CodeCity d on b.ZipCode = d.ZipCode     
                  where 1=1 and isnull(a.IsDelete, '') != 'Y' ";
        strSql += "  And a.MemberUID = " + HFD_UID.Value + "";



        strSql += " Order by  CreateDate Desc";

        Dictionary<string, object> dict = new Dictionary<string, object>();

        //dict.Add("MemberUID", HFD_MemberID.Value.ToString());

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

        DataTable dt = GetDataTable();

        if (dt.Rows.Count == 0)
        {
            //lblReport.Text = "";
            ShowSysMsg("查無資料");
            return "";
        }

        foreach (DataRow dr in dt.Rows)
        {
            string strFontSize = "14px";

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
            string strMemberColor = "color:red"; //聯絡人的字顏色
            string strWeight = ""; //字體
            string strHeight = "20px";//欄高


            //------------------------------------------------
            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("font-weight", strWeight);
            css.Add("height", "20px");
            Util.AddTDLine(css, 123, strColor);
            cell.InnerHtml = "<span style= " + strMemberColor + " >103 年 &nbsp; 月 &nbsp; 日</span>";
            row.Cells.Add(cell);
            //---------------------------------------------------
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("font-weight", strWeight);
            css.Add("height", "20px");
            Util.AddTDLine(css, 123, strColor);
            cell.InnerHtml = "<span style= " + strMemberColor + " >轉介教會：&nbsp</span>";
            row.Cells.Add(cell);
            table.Rows.Add(row);
            //----------------------------------------------------
            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("font-weight", strWeight);
            css.Add("height", "20px");
            Util.AddTDLine(css, 123, strColor);
            cell.InnerHtml = "<span style= " + strMemberColor + " >電話：" + dr["Phone"].ToString() + "</span>";// font-weight:bold;
            row.Cells.Add(cell);
            //---------------------------------------------------
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            // css.Add("font-weight", strWeight)
            css.Add("height", strHeight); ;
            Util.AddTDLine(css, 23, strColor);
            cell.InnerHtml = "<span style=" + strMemberColor + ">來電諮詢別(大類)：" + Util.TrimLastChar(dr["ConsultantMain"].ToString(), ',') + "&nbsp;</span>";
            row.Cells.Add(cell);
            table.Rows.Add(row);
            //------------------------------------------------------------------

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("font-weight", strWeight);
            css.Add("height", "25px");
            Util.AddTDLine(css, 123, strColor);
            cell.InnerHtml = "<span style=" + strMemberColor + " >姓名：" + dr["Cname"].ToString() + "</span> ";
            row.Cells.Add(cell);
            //table.Rows.Add(row);

            //------------------------------------------------------------------

            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            // css.Add("font-weight", strWeight);
            css.Add("height", strHeight);
            Util.AddTDLine(css, 23, strColor);
            cell.RowSpan = 3;
            cell.InnerHtml = "<span style=" + strMemberColor + " >來電諮詢別(分項)：" + Util.TrimLastChar(dr["ConsultantItem"].ToString(), ',') + "&nbsp;</span>";
            row.Cells.Add(cell);
            table.Rows.Add(row);
            //-------------------------------------------------

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("font-weight", strWeight);
            css.Add("height", "30px");
            Util.AddTDLine(css, 123, strColor);
            cell.InnerHtml = "<span style=" + strMemberColor + " >性別：" + dr["Sex"].ToString() + "</span>";
            row.Cells.Add(cell);
            table.Rows.Add(row);
            //------------------------------------------------------------------
            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("font-weight", strWeight);
            Util.AddTDLine(css, 23, strColor);
            cell.InnerHtml = "<span style=" + strMemberColor + " >婚姻：" + Util.TrimLastChar(dr["Marry"].ToString(), ',') + "</span>&nbsp;";
            row.Cells.Add(cell);
            table.Rows.Add(row);
            //------------------------------------------------------------------

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("font-weight", strWeight);
            Util.AddTDLine(css, 23, strColor);
            //cell.RowSpan = 2;
            cell.ColSpan = 2;
            if (dr["FullAddress"].ToString() != "")
            {
                cell.InnerHtml = "<span style=" + strMemberColor + " >8.地址：" + dr["FullAddress"].ToString() + "</span> ";
            }
            else
            {
                cell.InnerHtml = "<span style=" + strMemberColor + " >8.地址：" + dr["Overseas"].ToString().TrimEnd(',') + "</span> ";
            }
            row.Cells.Add(cell);
            table.Rows.Add(row);

            table.RenderControl(htw);


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



            row = new HtmlTableRow();
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
            table.RenderControl(htw);


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

            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 23, strColor);
            cell.ColSpan = 3;
            cell.InnerHtml = "  由轉介之教會填寫 (敬請一個月內回覆告知) &nbsp;";
            row.Cells.Add(cell);
            table.Rows.Add(row);



            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            cell.InnerHtml = " <span style=font-weight:bold >A、問題摘要：&nbsp;<P>";
            cell.InnerHtml += " <span style=font-weight:bold >關懷簡述：  ：&nbsp;<P>";
            cell.InnerHtml += " <span style=font-weight:bold >B、後續跟進：&nbsp;<br>";
            cell.InnerHtml += " <span style=font-weight:bold >1.□受洗：&nbsp;<br>";
            cell.InnerHtml += " <span style=font-weight:bold >2.□穩定聚會(有參加主日或其他小組聚會)：&nbsp;<br>";
            cell.InnerHtml += " <span style=font-weight:bold >3.□偶而參加聚會：&nbsp;<br>";
            cell.InnerHtml += " <span style=font-weight:bold >4.□不肯進入教會   (原因：           ) ：&nbsp;<br>";
            cell.InnerHtml += " <span style=font-weight:bold >5.□其他：  &nbsp;<P>";

            cell.InnerHtml += " <span style=font-weight:bold >C、關懷方式：&nbsp;<BR>";
            cell.InnerHtml += " <span style=font-weight:bold >1.□進入教會的關懷系統(小組/團契等) ：&nbsp;<P>";
            cell.InnerHtml += " <span style=font-weight:bold >2.□繼續保持聯繫(電話或探訪)：&nbsp;<br>";
            cell.InnerHtml += " <span style=font-weight:bold >3.□曾經聯繫過但目前中斷，原因：&nbsp;<br>";
            cell.InnerHtml += " <span style=font-weight:bold >4.□其他：&nbsp;<P>";
            cell.InnerHtml += " <span style=font-weight:bold >D、建 議：&nbsp;<br>";
            cell.InnerHtml += " <span style=font-weight:bold >&nbsp;<br>&nbsp;<br>&nbsp;<br>";


            row.Cells.Add(cell);
            table.Rows.Add(row);
            table.RenderControl(htw);
        }
        //轉成 html 碼========================================================================

        return htw.InnerWriter.ToString();
    }


    protected void backcancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ChangeChurchEdit.aspx");
    
    }
}
