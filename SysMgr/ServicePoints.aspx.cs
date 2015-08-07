using System;
using System.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
public partial class SysMgr_ServicePoints : BasePage
{
    #region NpoGridView 處理換頁相關程式碼
    Button btnNextPage, btnPreviousPage, btnGoPage;
    HiddenField HFD_CurrentPage;

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
    }
    protected void btnPreviousPage_Click(object sender, EventArgs e)
    {
        HFD_CurrentPage.Value = Util.MinusStringNumber(HFD_CurrentPage.Value);
        LoadFormData();
    }
    protected void btnNextPage_Click(object sender, EventArgs e)
    {
        HFD_CurrentPage.Value = Util.AddStringNumber(HFD_CurrentPage.Value);
        LoadFormData();
    }
    protected void btnGoPage_Click(object sender, EventArgs e)
    {
        LoadFormData();
    }
    #endregion NpoGridView 處理換頁相關程式碼
    //---------------------------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["ProgID"] = "ServicePoints";
        //權控處理
        AuthrityControl();

        //有 npoGridView 時才需要
        Util.ShowSysMsgWithScript("$('#btnPreviousPage').hide();$('#btnNextPage').hide();$('#btnGoPage').hide();");

        if (!IsPostBack)
        {
            LoadFormData(); 
        }
    }
    //------------------------------------------------------------------------------
    public void AuthrityControl()
    {
        if (Authrity.PageRight("_Focus") == false)
        {
            return;
        }
        //Authrity.CheckButtonRight("_AddNew", btnAdd);
        //Authrity.CheckButtonRight("_Query", btnQuery);
    }
    //----------------------------------------------------------------------
    public void LoadFormData()
    {

        //呼叫資料庫GetDataTable
        DataTable dt = GetDataTable(txtsDate.Text,txteDate.Text);
        //Grid initial
        NPOGridView npoGridView = new NPOGridView();
        npoGridView.Source = NPOGridViewDataSource.fromDataTable;
        npoGridView.dataTable = dt;
        npoGridView.Keys.Add("uid");
        npoGridView.DisableColumn.Add("uid");

        npoGridView.CurrentPage = Util.String2Number(HFD_CurrentPage.Value);
        //npoGridView.EditLink = Util.RedirectByTime("Church_Edit.aspx", "ChurchUID=");
        lblGridList.Text = npoGridView.Render();

        string strSql = "select UserID,UserName from AdminUser";
        Util.FillDropDownList(ddlUserName, strSql, "UserName", "UserID", true);

    }
    //------------------------------------------------------------------------------
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        string keep = ddlUserName.SelectedValue.ToString();
        LoadFormData();
        ddlUserName.SelectedValue = keep;
    }
    //------------------------------------------------------------------------------
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        string 列印模式新增表頭 = "";

        列印模式新增表頭 += "<table><tr><td colspan=\"8\" style=\"font-size:24px;font-family:標楷體;width:100%;\">";
        列印模式新增表頭 += "志工服務紀錄表";
        if (txtsDate.Text.Trim() != "" && txteDate.Text.Trim() != "") {
            列印模式新增表頭 += "(" + txtsDate.Text.Trim() + "～" + txteDate.Text.Trim() + ")"; 
        }
        列印模式新增表頭 += "統計";
        列印模式新增表頭 += "</td></tr></table>\n";

        //原始的寫法,其實看不懂在寫甚麼
        ExportDataTable Export = new ExportDataTable();
        DataTable dt = GetDataTable("", "");
        Export.dataTable = dt;
        Export.DisableColumn.Add("ColName");
        string ExportData = Title + Export.Render();
        
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
        Util.OutputTxt(sb + 列印模式新增表頭 + Export.Render(), "1", "" + DateTime.Now.ToString("yyyyMMddHHmmss"));

    }
    //----------------------------------------------------------------------------------------------------------
    
    private DataTable GetDataTable(string 開始日期YYYYMMDD,string 結束日期YYYYMMDD)
    {
        //讀取資料庫
        string 使用者代號 = "";
        if (this.ddlUserName.SelectedValue.ToString() != "")
        {
            使用者代號 = ddlUserName.SelectedValue.ToString();
        }
        string strSql = 取得計算SQL(開始日期YYYYMMDD, 結束日期YYYYMMDD,使用者代號);

        Dictionary<string, object> dict = new Dictionary<string, object>();
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);

        if (dt.Rows.Count > 0)
        {
            dt = 重新計算資料(dt);
        }
        return dt;
    }


    #region 基礎SQL範例
//    select 
//姓名,志工號,時數,總計點數,
//isnull([接聽真情之夜],0) [接聽真情之夜],
//isnull([讀書會],0) [讀書會],
//isnull([在職教育],0) [在職教育],
//isnull([參與活動],0) [參與活動],
//isnull([擔任組長],0) [擔任組長],
//isnull([讀書會帶領人],0) [讀書會帶領人],
//isnull([擔任夜間掌門人],0) [擔任夜間掌門人],
//isnull([2/14志工望年會],0) [2/14志工望年會],
//isnull([GOODTV望年會],0) [GOODTV望年會]
//from
//(
//select 
//Uid,
//UserName 姓名,
//UserID 志工號,
//(select sum(dateDiff(minute,LogTime,LogoutTime)) 
//    From log where 1=1 and log.UserID=AdminUser.Userid and (LogTime is not null and LogoutTime is not null) 
//    and convert(varchar,LogTime,111) between '2014/01/01' and '2015/12/31'
//    group by userid 
//) 時數,
//'' [總計點數],
//(select COUNT(*) from ServiceRecord 
//where 1=1 and ItemUid='1' and ServiceRecord.UserID=AdminUser.UserID
//and convert(varchar,serviceData,111) between '2014/01/01' and '2015/12/31' 
//group by itemuid
//) [接聽真情之夜],
//(select COUNT(*) from ServiceRecord 
//where 1=1 and ItemUid='3' and ServiceRecord.UserID=AdminUser.UserID
//and convert(varchar,serviceData,111) between '2014/01/01' and '2015/12/31' 
//group by itemuid
//) [讀書會],
//(select COUNT(*) from ServiceRecord 
//where 1=1 and ItemUid='4' and ServiceRecord.UserID=AdminUser.UserID
//and convert(varchar,serviceData,111) between '2014/01/01' and '2015/12/31' 
//group by itemuid
//) [在職教育],
//(select COUNT(*) from ServiceRecord 
//where 1=1 and ItemUid='6' and ServiceRecord.UserID=AdminUser.UserID
//and convert(varchar,serviceData,111) between '2014/01/01' and '2015/12/31' 
//group by itemuid
//) [參與活動],
//(select COUNT(*) from ServiceRecord 
//where 1=1 and ItemUid='7' and ServiceRecord.UserID=AdminUser.UserID
//and convert(varchar,serviceData,111) between '2014/01/01' and '2015/12/31' 
//group by itemuid
//) [擔任組長],
//(select COUNT(*) from ServiceRecord 
//where 1=1 and ItemUid='8' and ServiceRecord.UserID=AdminUser.UserID
//and convert(varchar,serviceData,111) between '2014/01/01' and '2015/12/31' 
//group by itemuid
//) [讀書會帶領人],
//(select COUNT(*) from ServiceRecord 
//where 1=1 and ItemUid='9' and ServiceRecord.UserID=AdminUser.UserID
//and convert(varchar,serviceData,111) between '2014/01/01' and '2015/12/31' 
//group by itemuid
//) [擔任夜間掌門人],
//(select COUNT(*) from ServiceRecord 
//where 1=1 and ItemUid='10' and ServiceRecord.UserID=AdminUser.UserID
//and convert(varchar,serviceData,111) between '2014/01/01' and '2015/12/31' 
//group by itemuid
//) [2/14志工望年會],
//(select COUNT(*) from ServiceRecord 
//where 1=1 and ItemUid='11' and ServiceRecord.UserID=AdminUser.UserID
//and convert(varchar,serviceData,111) between '2014/01/01' and '2015/12/31' 
//group by itemuid
//) [GOODTV望年會]
//from AdminUser
//where 1=1
//and IsUse=1 
//and UserID in 
//(
//    select UserID from Log where  1=1 and (LogTime is not null and LogoutTime is not null) and convert(varchar,LogTime,111) between '2014/01/01' and '2015/12/31'
//    union
//    select UserID from ServiceRecord where convert(varchar,serviceData,111) between '2014/01/01' and '2015/12/31'
//)
//) table1 
//order by uid
    #endregion 基礎SQL範例
    //概念:改為一個內容比較長但是結構很簡單的重複SQL
    private string 取得計算SQL(string 開始日期YYYYMMDD ,string 結束日期YYYYMMDD,string 使用者代號)
    {
        string strSQL = @"
            select 
            姓名,志工號,時數,0 總計點數"
            + 取得第一段SQL() +
            @" from
            (
            select 
            Uid,
            UserName 姓名,
            UserID 志工號
            ,(select sum(dateDiff(minute,LogTime,LogoutTime)) 
	            From log where 1=1 and log.UserID=AdminUser.Userid and (LogTime is not null and LogoutTime is not null) 
            ";
            if (開始日期YYYYMMDD!="" && 結束日期YYYYMMDD!="" ){
                strSQL += "    and convert(varchar,LogTime,111) between '" + 開始日期YYYYMMDD + "' and '" + 結束日期YYYYMMDD + "' ";
            }
            strSQL +=
	        @"  group by userid 
            ) 時數
            ,'' [總計點數]"
            + 取得第二段SQL(開始日期YYYYMMDD, 結束日期YYYYMMDD) +
            @" from AdminUser
            where 1=1
            and IsUse=1 
            and UserID in 
            (";
            strSQL += " select UserID from Log where  1=1 and (LogTime is not null and LogoutTime is not null) \n";
            if (開始日期YYYYMMDD!="" && 結束日期YYYYMMDD!="" ){
                strSQL += " and convert(varchar,LogTime,111) between '" + 開始日期YYYYMMDD + "' and '" + 結束日期YYYYMMDD + "' \n";
            }
	        if (使用者代號!=""){
                strSQL += " and UserID='" + 使用者代號 + "' \n";
            }
            strSQL += " Union \n";
	        strSQL += " select UserID from ServiceRecord where 1=1 ";
            if (開始日期YYYYMMDD!="" && 結束日期YYYYMMDD!="" ){
                strSQL += " and convert(varchar,serviceData,111) between '" + 開始日期YYYYMMDD + "' and '" + 結束日期YYYYMMDD + "' \n";
            }
            if (使用者代號 != "")
            {
                strSQL += " and UserID='" + 使用者代號 + "' \n";
            }
            strSQL += @")
            ) table1 
            order by uid 
            ";
        return strSQL;
    }

    //這段用程式產生 
    private string 取得第一段SQL() {
        string getSql = " SELECT Uid,'[' + ItemName + ']' itemName,isnull(Score,0) score  FROM ServiceItem where isnull(IsDelete ,'') != 'Y' ORDER BY ClassUid,uid ";
        DataTable tmpDT = NpoDB.GetDataTableS(getSql, new Dictionary<string, object>());
        string retStr = "";
        for (int i = 0; i < tmpDT.Rows.Count; i++) {
            retStr += ",isnull(" + tmpDT.Rows[i]["itemName"].ToString() + ",0) " + tmpDT.Rows[i]["itemName"].ToString() + "\n";
        }
        return retStr;
    }

    private string 取得第二段SQL(string 開始日期YYYYMMDD, string 結束日期YYYYMMDD)
    {
        string getSql = " SELECT Uid,'[' + ItemName + ']' itemName,isnull(Score,0) score  FROM ServiceItem where isnull(IsDelete ,'') != 'Y' ORDER BY ClassUid,uid ";
        DataTable tmpDT = NpoDB.GetDataTableS(getSql, new Dictionary<string, object>());
        string retStr = "";
        for (int i = 0; i < tmpDT.Rows.Count; i++)
        {
            //,(select COUNT(*) from ServiceRecord 
            //        where 1=1 and ItemUid='1' and ServiceRecord.UserID=AdminUser.UserID
            //        and convert(varchar,serviceData,111) between '2014/01/01' and '2015/12/31' 
            //        group by itemuid
            //        ) [接聽真情之夜]
            retStr += ",(select COUNT(*) from ServiceRecord \n";
            retStr += " where 1=1 and  isnull(IsDelete, '') != 'Y' AND ItemUid='" + tmpDT.Rows[i]["Uid"].ToString() + "' and ServiceRecord.UserID=AdminUser.UserID \n";
            if (開始日期YYYYMMDD != "" && 結束日期YYYYMMDD != "")
            {
                retStr += " and convert(varchar,serviceData,111) between '" + 開始日期YYYYMMDD + "' and '" + 結束日期YYYYMMDD + "' \n";
            }
            retStr += " group by itemuid ) " + tmpDT.Rows[i]["itemName"].ToString() + " \n";
        }
        return retStr;
    }

    //計算DataTable內所有資料傳回
    private DataTable 重新計算資料(DataTable sourceDataTable) {
        //整體構思:
        //將資料載入一個陣列，再將陣列重算，再將重算的結果放回DataTable，然後把DataTable當作GridView的Source
        //把資料塞進去ArrayList

        //#2 分數表
        Dictionary<string, int> 成績表 = new Dictionary<string, int>();
        string getSql = " SELECT Uid,'[' + ItemName + ']' itemName,isnull(Score,0) score  FROM ServiceItem where isnull(IsDelete ,'') != 'Y' ORDER BY ClassUid,uid ";
        DataTable tmpDT = NpoDB.GetDataTableS(getSql, new Dictionary<string, object>());
        for (int n = 0; n < tmpDT.Rows.Count; n++)
        {
            成績表.Add(tmpDT.Rows[n]["ItemName"].ToString(), int.Parse(tmpDT.Rows[n]["score"].ToString()));
        }

        ArrayList ArrDataRows = new ArrayList();

        int i = 0;
        foreach (DataRow dataRow in sourceDataTable.Rows)
        {
            string tmpStr = "";
            for (int i2 = 0; i2 < dataRow.ItemArray.Length; i2++) {
                tmpStr += dataRow.ItemArray[i2].ToString() + ";";
            }
            if (tmpStr.EndsWith(";") == true) {
                tmpStr=tmpStr.Remove(tmpStr.Length-1, 1);
            }
            ArrDataRows.Add(string.Join(";",tmpStr.Split(';')));
            i++;
        }
        
        //讀取ArrayList的方法
        //for (int j = 0; j < ArrDataRows.Count; j++)
        //{
        //    for (int j1 = 0; j1 < ArrDataRows[j].ToString().Split(';').Length; j1++)
        //        Response.Write(ArrDataRows[j].ToString().Split(';')[j1].ToString() + ";");
        //    Response.Write("<br>");
        //}


        //重新計算整個ArrayList
        

        //將ArrayList塞回去dt
        DataTable dt2 = new DataTable();

        //建立資料結構
        for (int j = 0; j < sourceDataTable.Columns.Count; j++){
            dt2.Columns.Add(sourceDataTable.Columns[j].ColumnName);
        }

        int RowDataColumnsSize = ArrDataRows[0].ToString().Split(';').Length;

        for (int k = 0; k < ArrDataRows.Count; k++) {
            DataRow row = dt2.NewRow();
            decimal tmp時數 = 0;
            for (int k1 = 0; k1 < RowDataColumnsSize;k1++){
                if (dt2.Columns[k1].ToString() == "姓名" || dt2.Columns[k1].ToString() == "志工號"){ 
                    //不統計
                    row[k1] = ArrDataRows[k].ToString().Split(';')[k1].ToString(); 
                }
                else if (dt2.Columns[k1].ToString() == "時數")
                {

                    try
                    {
                        tmp時數 = Math.Round((decimal.Parse(ArrDataRows[k].ToString().Split(';')[k1].ToString()) / 60), 0);
                        row[k1] = tmp時數.ToString();
                    }
                    catch (Exception e)
                    {
                        row[k1] = "0";
                    }
                }
                else if (dt2.Columns[k1].ToString() == "總計點數")
                {
                    //這段把除以四並且四捨五入的上班時數加入總計點數
                    row[k1] = Math.Round((decimal.Parse(ArrDataRows[k].ToString().Split(';')[k1].ToString()) + tmp時數 / 4), 0).ToString();
                }
                else
                {
                    //計算分數
                    string 欄位值=ArrDataRows[k].ToString().Split(';')[k1].ToString();
                    int 欄位數據 = 0;
                    try
                    {
                        欄位數據 = int.Parse(欄位值);
                        if (欄位數據 > 0)
                        {
                            int 乘上計分 = 0;
                            乘上計分 = 成績表["[" + dt2.Columns[k1].ToString() + "]"];
                            row["總計點數"] = (int.Parse(row["總計點數"].ToString()) + 欄位數據 * 乘上計分).ToString();
                        }
                    }
                    catch (Exception){ }
                    row[k1] = 欄位值;
                }
            }
            dt2.Rows.Add(row);
        }

        //重新整理整個DT，如果是0，寫為空白
        for (int k = 0; k < dt2.Rows.Count; k++)
        {
            for (int j = 0; j < dt2.Columns.Count; j++) {
                if (dt2.Rows[k][j].ToString() == "0") {
                    dt2.Rows[k][j] = "";
                }
            }
        }
        return dt2;
    }

}