using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Threading;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Net.Mail;


public partial class Church_CaseChangeChurchEdit : BasePage 
{
    GroupAuthrity ga = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        lblHeaderText.Text = "個案轉介";

        if (!IsPostBack)
        {
            HFD_UID.Value = Util.GetQueryString("uid");
            //載入下拉式選單資料
            //LoadDropDownListData();
            //載入資料
            LoadFormData();
        }    
    }
    //-------------------------------------------------------------------------
    private void SetupDefaultValue()
    {
    }
    //-------------------------------------------------------------------------
    private void SetButton()
    {

    }
    //-------------------------------------------------------------------------
    private void AuthrityControl()
    {
    }
    //-------------------------------------------------------------------------
    public void LoadDropDownListData()
    {
    }
    //-------------------------------------------------------------------------------------------------------------
    private void LoadFormData()
    {

        //string strSql = "";
        DataTable dt = null;
        DataRow dr = null;
        if (HFD_UID.Value != "ADD")
        {
            string strSql = @"
                            Select  a.uid as uid ,CName as 姓名,a.changeChurchDate as  轉介日期,Problem,Brief,FollowUp,Caring,Proposal,
                            FileName As 檔案名稱,
                            b.Name As 教會 
                            From Member a 
                            left outer join Church b on a.ChurchUid=b.uid 
                            Where 1=1 
                            And ChurchYN = '是'
                            And isnull(a.IsDelete, '') != 'Y'
                            and ChangeChurchSta='已處理'
                            and a.uid =@uid
                        ";
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("uid", HFD_UID.Value);
            dt = NpoDB.GetDataTableS(strSql, dict);
            if (dt.Rows.Count == 0)
            {
                //資料異常
                SetSysMsg("資料異常!");
                Response.Redirect("Church.aspx");
                return;
            }
            dr = dt.Rows[0];

        }
        txtCName.Text = dr["姓名"].ToString();
        txtProblem.Text = (HFD_UID.Value == "ADD") ? "" : dr["Problem"].ToString();
        txtProposal.Text = (HFD_UID.Value == "ADD") ? "" : dr["Proposal"].ToString();
        txtBrief.Text = (HFD_UID.Value == "ADD") ? "" : dr["Brief"].ToString();
        txtChurch.Text = (HFD_UID.Value == "ADD") ? "" : dr["教會"].ToString();
        txt_changeChurchDate.Text = Util.DateTime2String(dr["轉介日期"].ToString(), DateType.yyyyMMdd, EmptyType.ReturnNull);

        if (HFD_UID.Value != "ADD" && dr["FollowUp"].ToString() != "")
        {
            string tmp = dr["FollowUp"].ToString();
            string[] split = tmp.Split(new Char[] { ',' });
            split[0].ToString();
            split[1].ToString();
            split[2].ToString();
            split[3].ToString();
            split[4].ToString();
            split[5].ToString();
            split[6].ToString();
            if (HFD_UID.Value != "ADD")
            {
                CKB_FollowUp1.Checked = split[0].ToString() != "" ? true : false;
            }
            if (HFD_UID.Value != "ADD")
            {
                CKB_FollowUp2.Checked = split[1].ToString() != "" ? true : false;
            }
            if (HFD_UID.Value != "ADD")
            {
                CKB_FollowUp3.Checked = split[2].ToString() != "" ? true : false;
            }
            if (HFD_UID.Value != "ADD")
            {
                CKB_FollowUp4.Checked = split[3].ToString() != "" ? true : false;
                txt_FollowUp4.Text = split[4].ToString().TrimEnd(':');
            }
            if (HFD_UID.Value != "ADD")
            {
                CKB_FollowUp5.Checked = split[5].ToString() != "" ? true : false;
                txt_FollowUp5.Text = split[6].ToString().TrimEnd(':');
            }
        }

        //////
        if (HFD_UID.Value != "ADD" && dr["Caring"].ToString() != "")
        {
            string tmp = dr["Caring"].ToString();
            string[] split = tmp.Split(new Char[] { ',' });
            split[0].ToString();
            split[1].ToString();
            split[2].ToString();
            split[3].ToString();
            split[4].ToString();
            //split[5].ToString();
            if (HFD_UID.Value != "ADD")
            {
                CKB_Caring1.Checked = split[0].ToString() == "1" ? true : false;
            }
            if (HFD_UID.Value != "ADD")
            {
                CKB_Caring2.Checked = split[1].ToString() == "1" ? true : false;
            }
            if (HFD_UID.Value != "ADD")
            {
                CKB_Caring3.Checked = split[2].ToString() == "1" ? true : false;
            }
            if (HFD_UID.Value != "ADD")
            {
               // CKB_Caring4.Checked = split[3].ToString() == "1" ? true : false;
                txt_Caring4.Text = split[4].ToString();
            }
        }

        NPOGridViewColumn col;
        //-------------------------------------------------------------------------
        //col = new NPOGridViewColumn("選擇");
        //col.ColumnType = NPOColumnType.Checkbox;
        //col.ControlKeyColumn.Add("uid");
        //col.CaptionWithControl = false;
        //col.ControlName.Add("chkSelectLand");
        //col.ControlId.Add("chkSelectLand");
        //col.ControlValue.Add("0");
        //col.ControlText.Add("");
        //col.ColumnName.Add("selected");
        //npoGridView.Columns.Add(col);
        ////-------------------------------------------------------------------------
        //col = new NPOGridViewColumn("教會名稱");
        //col.ColumnType = NPOColumnType.NormalText;
        //col.ColumnName.Add("教會名稱");
        //npoGridView.Columns.Add(col);
        ////-------------------------------------------------------------------------
        //col = new NPOGridViewColumn("區域");
        //col.ColumnType = NPOColumnType.NormalText;
        //col.ColumnName.Add("區域");
        //npoGridView.Columns.Add(col);
        ////-------------------------------------------------------------------------
        //col = new NPOGridViewColumn("地址");
        //col.ColumnType = NPOColumnType.NormalText;
        //col.ColumnName.Add("地址");
        //npoGridView.Columns.Add(col);
        //col = new NPOGridViewColumn("主任牧師");
        //col.ColumnType = NPOColumnType.NormalText;
        //col.ColumnName.Add("主任牧師");
        //npoGridView.Columns.Add(col);
        ////-------------------------------------------------------------------------
        //col = new NPOGridViewColumn("聯絡窗口");
        //col.ColumnType = NPOColumnType.NormalText;
        //col.ColumnName.Add("聯絡窗口");
        //npoGridView.Columns.Add(col);

        //col = new NPOGridViewColumn("Email");
        //col.ColumnType = NPOColumnType.NormalText;
        //col.ColumnName.Add("Email");
        //npoGridView.Columns.Add(col);
        ////-------------------------------------------------------------------------
        //lblGridList.Text = npoGridView.Render();
    }
    //-------------------------------------------------------------------------------------------------------------
    protected void btnClear_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        List<ColumnData> list = new List<ColumnData>();
        list.Add(new ColumnData("Uid", HFD_UID.Value, false, false, true));
        list.Add(new ColumnData("ChurchUid", "", true, true, false));
        string strSql = Util.CreateUpdateCommand("Member", list, dict);
        NpoDB.ExecuteSQLS(strSql, dict);
        Response.Redirect("CaseChangeChurchEdit.aspx?uid=" + HFD_UID.Value);
    }
    //-------------------------------------------------------------------------------------------------------------
    protected void btnQueryChurch_Click(object sender, EventArgs e)
    {
        Response.Redirect("CaseChangeChurch_QueryChurch.aspx?uid=" + HFD_UID.Value);
    }
    //-------------------------------------------------------------------------------------------------------------
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("CaseChangeChurch.aspx");
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        //把該筆刪除的資料 IsDelete改成Y
        Dictionary<string, object> dict = new Dictionary<string, object>();
        string strSql = @"
                           update Member set
                           IsDelete='Y',
                           DeleteID=@DeleteID,
                           DeleteDate=GetDate()
                           where uid = @uid
                         ";
        dict.Add("uid", HFD_UID.Value);
        dict.Add("DeleteID", SessionInfo.UserID);
        NpoDB.ExecuteSQLS(strSql, dict);
        Session["Msg"] = "刪除資料成功";
        Response.Redirect(Util.RedirectByTime("CaseChangeChurch.aspx"));
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
        
    protected void btnsave_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        List<ColumnData> list = new List<ColumnData>();
        SetColumnData(list);
        //string strSql = Util.CreateInsertCommand("Member", list, dict);
        string strSql = Util.CreateUpdateCommand("Member", list, dict);
        NpoDB.ExecuteSQLS(strSql, dict);
        Session["Msg"] = "新增資料成功";
        Response.Redirect(Util.RedirectByTime("CaseChangeChurch.aspx"));
    }
    private void SetColumnData(List<ColumnData> list)
    {
        list.Add(new ColumnData("Uid", HFD_UID.Value, false, false, true));
        list.Add(new ColumnData("changeChurchDate", Util.DateTime2String(txt_changeChurchDate.Text, DateType.yyyyMMddHHmmss, EmptyType.ReturnNull), true, true, false));
        list.Add(new ColumnData("Problem", txtProblem.Text, true, true, false));
        list.Add(new ColumnData("Brief", txtBrief.Text, true, true, false));
        //list.Add(new ColumnData("Brief", txtBrief.Text, true, true, false));
        list.Add(new ColumnData("Proposal", txtProposal.Text, true, true, false));
        ///B、後續跟進
        string B1 = "";
        if (CKB_FollowUp1.Checked == true)
        {
            B1 = "受洗,";
        }
        else
            B1 = ",";
        if (CKB_FollowUp2.Checked == true)
        {
            B1 += "穩定聚會,";
        }
        else
        {
            B1 += ",";
        }
        if (CKB_FollowUp3.Checked == true)
        {
            B1 += "偶而聚會,";
        }
        else
        {
            B1 += ",";
        }
        if (CKB_FollowUp4.Checked == true)
        {
            B1 += "不肯進入教會,";
            B1 += txt_FollowUp4.Text  + ":,";
        }
        else
        {
          
            B1 += ",:,";
            
        }
        if (CKB_FollowUp5.Checked == true)
        {
            B1 += "關懷中,";
            B1 += txt_FollowUp5.Text +":";
        }
        else
        {
            B1 += ",:";
        }
        list.Add(new ColumnData("FollowUp", B1, true, true, false));
        /// C、關懷方式
        string C1 = "";
        if (CKB_Caring1.Checked == true)
        {
            C1 = "1,";
        }
        else
            C1 = "0,";
        if (CKB_Caring2.Checked == true)
        {
            C1 += "1,";
        }
        else
        {
            C1 += "0,";
        }
        if (CKB_Caring3.Checked == true)
        {
            C1 += "1,";
        }
        else
        {
            C1 += "0,";
        }
        //if (CKB_Caring4.Checked == true)
        //{
            C1 += "1,";
            C1 += txt_Caring4.Text;
        //}
        //else
        //{
        //C1 += "0,";

        //}
        list.Add(new ColumnData("Caring", C1, true, true, false));

        //if (HFD_Mode.Value == "ADD")
        //{
        //    //新增日期
        //    list.Add(new ColumnData("InsertDate", Util.GetToday(DateType.yyyyMMddHHmmss), true, true, false));
        //    list.Add(new ColumnData("InsertID", SessionInfo.UserID, true, true, false));
        //}
        //else
        //{
        //    //修改日期
        //    list.Add(new ColumnData("UpdateDate", Util.GetToday(DateType.yyyyMMddHHmmss), true, true, false));
        //    list.Add(new ColumnData("UpdateID", SessionInfo.UserID, true, true, false));
        //}

    }
    protected void CKB_FollowUp5_CheckedChanged(object sender, EventArgs e)
    {
        if (CKB_FollowUp5.Checked == false)
            txt_FollowUp5.Text = "";

    }

    protected void CKB_FollowUp4_CheckedChanged(object sender, EventArgs e)
    {
        //if (CKB_FollowUp4.Checked == false)
        //    txt_FollowUp4.Text = "";
    }
    //protected void CKB_Caring4_CheckedChanged(object sender, EventArgs e)
    //{
        //if (CKB_Caring4.Checked == false)
        //    txt_Caring4.Text = "";
    //}
}
