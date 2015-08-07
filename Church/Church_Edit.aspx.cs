using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic ;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.IO;

public partial class SysMgr_Church_Edit : BasePage
{
    //程式載入時候的處理
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["ProID"] = "Church";
        //權限控制
        AuthrityControl();

        //取得模式, 新增("ADD")或修改("")
        HFD_Mode.Value = Util.GetQueryString("Mode");

        //設定一些按鍵的 Visible 屬性
        //注意:要在 AuthrityControl() 及取得 HFD_Mode.Value 呼叫
        SetButton();

        if (!IsPostBack)
        {
            HFD_ChurchUID.Value = Util.GetQueryString("ChurchUID");

            //載入下拉式選單資料
            LoadDropDownListData();

            LoadFormData();
          
        }
    }
    //------------------------------------------------------------------------------
    private void AuthrityControl()
    {
        if (Authrity.PageRight("_Focus") == false)
        {
            return;
        }
        Authrity.CheckButtonRight("_Update", btnUpdate);
        Authrity.CheckButtonRight("_Delete", btnDelete);
    }
    //------------------------------------------------------------------------------
    private void SetButton()
    {
        //如果是新增,要做一些初始畫的動作
        if (HFD_Mode.Value == "ADD")
        {
            //新增時, 修改及刪除鍵沒動作
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
        }
        else
        {
            //修改時, 新增鍵沒動作
            btnAdd.Visible = false;
        }
    }
    //-------------------------------------------------------------------------

    public void LoadDropDownListData()
    {
        Util.FillCityData(ddlCity);
        Util.FillAreaData(ddlArea, ddlCity.SelectedValue);
       
    }
    public void LoadFormData()
    {
        string strSql = "";
        DataTable dt = null;
        DataRow dr = null;
        if (HFD_Mode.Value != "ADD")
        {

             strSql = @"
                        select uid , Name  , ZipCode,City,Area  ,Address , ChurchPhone  ,
                        ChurchEmail2  , Priest  ,Priestphone  , PriestEmail  ,
                        Contact  , ChurchPhone  ,ChurchEmail,ChurchMphone,Churchministry ,ministryOther,InsertDate
                        from Church where uid=@uid
                      ";


            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("uid", HFD_ChurchUID.Value);
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
    
        txtName.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Name"].ToString();
        //txtArea.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Area"].ToString();

        //郵遞區號(H)	ZipCode
        txtZipCode.Text = (HFD_Mode.Value == "ADD") ? "" : dr["ZipCode"].ToString();
        //縣市(H)	City
        Util.SetDdlIndex(ddlCity, (HFD_Mode.Value == "ADD") ? "" : dr["City"].ToString());
        //鄉鎮市區(H)	Area
        Util.FillAreaData(ddlArea, ddlCity.SelectedValue);
        Util.SetDdlIndex(ddlArea, (HFD_Mode.Value == "ADD") ? "" : dr["Area"].ToString());
        HFD_Area.Value = (HFD_Mode.Value == "ADD") ? "" : dr["Area"].ToString();


        txtAddress.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Address"].ToString();
        txtChurchPhone.Text = (HFD_Mode.Value == "ADD") ? "" : dr["ChurchPhone"].ToString();
        txtChurchEmail2.Text = (HFD_Mode.Value == "ADD") ? "" : dr["ChurchEmail2"].ToString();
        txtPriest.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Priest"].ToString();
        txtPriestPhone.Text = (HFD_Mode.Value == "ADD") ? "" : dr["PriestPhone"].ToString();
        txtPriestEmail.Text = (HFD_Mode.Value == "ADD") ? "" : dr["PriestEmail"].ToString();
        txtContact.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Contact"].ToString();
        txtChurchMphone.Text = (HFD_Mode.Value == "ADD") ? "" : dr["ChurchMphone"].ToString();
        txtChurchEmail.Text = (HFD_Mode.Value == "ADD") ? "" : dr["ChurchEmail"].ToString();
        txt_Mministry7.Text = (HFD_Mode.Value == "ADD") ? "" : dr["ministryOther"].ToString();
        txtInsertDate.Text = (HFD_Mode.Value == "ADD") ? "" : dr["InsertDate"].ToString();
        //判斷[教會事工]的資料庫是否空值在讀取資料
        if (HFD_Mode.Value != "ADD" && dr["Churchministry"].ToString() != "")
        {
        string tmp = dr["Churchministry"].ToString();
        string[] split = tmp.Split(new Char[] { ',' });
        split[0].ToString();
        split[1].ToString();
        split[2].ToString();
        split[3].ToString();
        split[4].ToString();
        split[5].ToString();
        split[6].ToString();
        if (HFD_Mode.Value != "ADD")
            CHK_Mministry1.Checked = split[0].ToString() == "婚前" ? true : false;        
        if (HFD_Mode.Value != "ADD")
            CHK_Mministry2.Checked = split[1].ToString() == "婚姻" ? true : false;
        if (HFD_Mode.Value != "ADD")
            CHK_Mministry3.Checked = split[2].ToString() == "青少" ? true : false;
        if (HFD_Mode.Value != "ADD")
            CHK_Mministry4.Checked = split[3].ToString() == "老人" ? true : false;
        if (HFD_Mode.Value != "ADD")
            CHK_Mministry5.Checked = split[4].ToString() == "家庭關懷" ? true : false;
        if (HFD_Mode.Value != "ADD")
            CHK_Mministry6.Checked = split[5].ToString() == "協談輔導" ? true : false;
        if (HFD_Mode.Value != "ADD")
            CHK_Mministry7.Checked = split[6].ToString() == "其他" ? true : false;
        }
       


    }
    //------------------------------------------------------------------------------
    protected void btnAdd_Click(object sender, EventArgs e)
    {
       /* //判斷欄位是否必填
        if (txtChurchName.Text.Trim() == "" || txtChurchArea.Text.Trim() == "")
        {
            ShowSysMsg("教會名稱及區域不可空白！請重新輸入。");
            return;
        }*/
        Dictionary<string, object> dict = new Dictionary<string, object>();
        List<ColumnData> list = new List<ColumnData>();
        SetColumnData(list);
        string strSql = Util.CreateInsertCommand("Church", list, dict);
        NpoDB.ExecuteSQLS(strSql, dict);
        Session["Msg"] = "新增資料成功";
        Response.Redirect(Util.RedirectByTime("Church.aspx"));
    }
    //------------------------------------------------------------------------------
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
         /* //判斷欄位是否必填
        if (txtChurchName.Text.Trim() == "" || txtChurchArea.Text.Trim() == "")
        {
            ShowSysMsg("教會名稱及區域不可空白！請重新輸入。");
            return;
        }*/
        Dictionary<string, object> dict = new Dictionary<string, object>();
        List<ColumnData> list = new List<ColumnData>();
        SetColumnData(list);
        string strSql = Util.CreateUpdateCommand("Church", list, dict);
        NpoDB.ExecuteSQLS(strSql, dict);
        Session["Msg"] = "修改資料成功";
        Response.Redirect(Util.RedirectByTime("Church.aspx"));
    }

    //------------------------------------------------------------------------------
    private void SetColumnData(List<ColumnData> list)
    {
        list.Add(new ColumnData("Uid", HFD_ChurchUID.Value, false, false, true));

        list.Add(new ColumnData("Name", txtName.Text, true, true, false));

        //郵遞區號(H)	ZipCode
        list.Add(new ColumnData("ZipCode", txtZipCode.Text, true, true, false));
        //縣市(H)	City
        list.Add(new ColumnData("City", ddlCity.SelectedValue, true, true, false));
        //鄉鎮市區(H)	Area
        list.Add(new ColumnData("Area", HFD_Area.Value, true, true, false));


       // list.Add(new ColumnData("Area", txtArea.Text, true, true, false));
        list.Add(new ColumnData("Address", txtAddress.Text, true, true, false));
        list.Add(new ColumnData("ChurchPhone", txtChurchPhone.Text, true, true, false));
        list.Add(new ColumnData("ChurchEmail2", txtChurchEmail2.Text, true, true, false));
        list.Add(new ColumnData("Priest", txtPriest.Text, true, true, false));
        list.Add(new ColumnData("PriestPhone", txtPriestPhone.Text, true, true, false));
        list.Add(new ColumnData("PriestEmail", txtPriestEmail.Text, true, true, false));
        list.Add(new ColumnData("Contact", txtContact.Text, true, true, false));
        list.Add(new ColumnData("ChurchMphone", txtChurchMphone.Text, true, true, false));
        list.Add(new ColumnData("ChurchEmail", txtChurchEmail.Text, true, true, false));
        string m1 = "";
        if (CHK_Mministry1.Checked == true)
        {
            m1 = "婚前,";
        }
        else
            m1 = ",";
        if (CHK_Mministry2.Checked == true)
        {
            m1 += "婚姻,";
        }
        else
        {
            m1 += ",";
        }
        if (CHK_Mministry3.Checked == true)
        {
            m1 += "青少,";
        }
        else
        {
            m1 += ",";
        }
        if (CHK_Mministry4.Checked == true)
        {
            m1 += "老人,";
        }
        else
        {
            m1 += ",";
        }
        if (CHK_Mministry5.Checked == true)
        {
            m1 += "家庭關懷,";
        }
        else
        {
            m1 += ",";
        }
        if (CHK_Mministry6.Checked == true)
        {
            m1 += "協談輔導,";
        }
        else
        {
            m1 += ",";
        }
        if (CHK_Mministry7.Checked == true)
        {
            m1 += "其他,";
            list.Add(new ColumnData("ministryOther", txt_Mministry7.Text, true, true, false));
        }
        else
        {
            m1 += ",";
        }
        list.Add(new ColumnData("Churchministry", m1 , true, true, false));

        if (HFD_Mode.Value == "ADD")
        {
            //新增日期
            list.Add(new ColumnData("InsertDate", Util.GetToday(DateType.yyyyMMddHHmmss), true, true, false));
            list.Add(new ColumnData("InsertID", SessionInfo.UserID, true, true, false));
        }
        else
        {
            //修改日期
            list.Add(new ColumnData("UpdateDate", Util.GetToday(DateType.yyyyMMddHHmmss), true, true, false));
            list.Add(new ColumnData("UpdateID", SessionInfo.UserID, true, true, false));
        }

    }
    //-------------------------------------------------------------------------------------------------------------
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        //把該筆刪除的資料 IsDelete改成Y
        Dictionary<string, object> dict = new Dictionary<string, object>();
        string strSql = @"
                           update Church set
                           IsDelete='Y',
                           DeleteID=@DeleteID,
                           DeleteDate=GetDate()
                           where uid = @uid
                         ";
        dict.Add("uid", HFD_ChurchUID.Value);
        dict.Add("DeleteID", SessionInfo.UserID);
        NpoDB.ExecuteSQLS(strSql, dict);
        Session["Msg"] = "刪除資料成功";
        Response.Redirect(Util.RedirectByTime("Church.aspx"));
    }
    //------------------------------------------------------------------------------
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("Church.aspx");
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
                    ";//邊界-上右下左
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(s);
        Util.OutputTxt(sb + lblRpt.Text, "1", DateTime.Now.ToString("yyyyMMddHHmmss"));
    }
    //-------------------------------------------------------------------------------------
    private string GetReport()
    {
        string[] Items = {
                        "教會名稱",
                        "教會地址",
                        "教會電話",
                        "教會Email",
                        "主任牧師",
                        "主任牧師電話",
                        "主任牧師Email",
                        "聯絡窗口",
                        "窗口手機",
                        "窗口Email",
                        "教會事工",
                        
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
        Util.AddTDLine(css, 234);
        cell.InnerHtml = txtName.Text;
        row.Cells.Add(cell);

        string Address = "";
        //呼叫資料庫GetDataTable
        DataTable dt = GetDataTable();
        foreach (DataRow dr in dt.Rows)
        {
             Address = dr["City"].ToString() + dr["Area"].ToString() ;
        }
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            Util.AddTDLine(css, 234);
            cell.InnerHtml = txtZipCode.Text + Address + txtAddress.Text;
            row.Cells.Add(cell);


        cell = new HtmlTableCell();
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        css.Add("mso-number-format", "\\@");
        Util.AddTDLine(css, 234);
        cell.InnerHtml = txtChurchPhone.Text;
        row.Cells.Add(cell);

        cell = new HtmlTableCell();
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 234);
        cell.InnerHtml = txtChurchEmail2.Text;
        row.Cells.Add(cell);

        cell = new HtmlTableCell();
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 234);
        cell.InnerHtml = txtPriest.Text;
        row.Cells.Add(cell);

        cell = new HtmlTableCell();
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        css.Add("mso-number-format", "\\@");
        Util.AddTDLine(css, 234);
        cell.InnerHtml = txtPriestPhone.Text;
        row.Cells.Add(cell);
        table.Rows.Add(row);

        cell = new HtmlTableCell();
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        css.Add("mso-number-format", "\\@");
        Util.AddTDLine(css, 234);
        cell.InnerHtml = txtPriestEmail.Text;
        row.Cells.Add(cell);
        table.Rows.Add(row);

        cell = new HtmlTableCell();
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 234);
        cell.InnerHtml = txtContact.Text;
        row.Cells.Add(cell);
        table.Rows.Add(row);

        cell = new HtmlTableCell();
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        css.Add("mso-number-format", "\\@");
        Util.AddTDLine(css, 234);
        cell.InnerHtml = txtChurchMphone.Text;
        row.Cells.Add(cell);

        cell = new HtmlTableCell();
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 234);
        cell.InnerHtml = txtChurchEmail.Text;
        row.Cells.Add(cell);

        string Mministry = "";
        if (CHK_Mministry1.Checked == true)
            Mministry = CHK_Mministry1.Text + ",";
        if (CHK_Mministry2.Checked == true)
            Mministry += CHK_Mministry2.Text + ",";
        if (CHK_Mministry3.Checked == true)
            Mministry += CHK_Mministry3.Text + ",";
        if (CHK_Mministry4.Checked == true)
            Mministry += CHK_Mministry4.Text + ",";
        if (CHK_Mministry5.Checked == true)
            Mministry += CHK_Mministry5.Text + ",";
        if (CHK_Mministry6.Checked == true)
            Mministry += CHK_Mministry6.Text + ",";
        if (CHK_Mministry7.Checked == true)
            Mministry += "其他:("+txt_Mministry7.Text+")";
        cell = new HtmlTableCell();
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        Util.AddTDLine(css, 234);
        cell.InnerHtml = Mministry;
        row.Cells.Add(cell);

        table.Rows.Add(row);

        //轉成 html 碼
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        table.RenderControl(htw);
        return htw.InnerWriter.ToString();
    
    }
    //----------------------------------------------------------------------------------------------------------
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
        strTemp = "GOOD TV 夥伴教會資料";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "24px");
        css.Add("font-family", "標楷體");
        css.Add("border-style", "none");
        css.Add("font-weight", "bold");
        //ColSpan跨行置中幾格
        cell.ColSpan = 11;
        row.Cells.Add(cell);
        table.Rows.Add(row);
        //*************************************
        //轉成 html 碼
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        table.RenderControl(htw);

        return htw.InnerWriter.ToString();
    }

    protected void CHK_Mministry7_CheckedChanged(object sender, EventArgs e)
    {
        if (CHK_Mministry7.Checked == false)
        {
            txt_Mministry7.Text = "";
        }
    }
    protected void txt_Mministry7_TextChanged(object sender, EventArgs e)
    {
        if (txt_Mministry7.Text != "")
        {
            CHK_Mministry7.Checked = true;
        }
        if (txt_Mministry7.Text == "")
        {
            CHK_Mministry7.Checked = false;
        }
    }
    private DataTable GetDataTable()
    {

        //讀取資料庫
        string strSql = @"
                        select c.uid , c.Name  , c.ZipCode,CC.Name as  City , CC1.Name as Area  ,Address , ChurchPhone  ,
                        ChurchEmail2  , Priest  ,Priestphone  , PriestEmail  ,
                        Contact  , ChurchPhone  ,ChurchEmail,ChurchMphone,Churchministry ,ministryOther
                        from Church as C inner join CodeCity CC on CC.ZipCode =City 
                        inner join CodeCity CC1 on CC1.ZipCode =Area 
                        where 1=1 
                        and City = @City 
                        and Area = @Area
                    ";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("City", ddlCity.SelectedValue);
        dict.Add("Area", HFD_Area.Value);
        //dict.Add("Area", HFD_Area.Value);
        dict.Add("ServiceUser", SessionInfo.UserID.ToString());
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        return dt;
    }
}
