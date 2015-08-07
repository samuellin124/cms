using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Text;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

public partial class SysMgr_ActJoinReport :BasePage 
{
    GroupAuthrity ga = null;
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

        Session["ProgID"] = "ActQry";
        //權控處理
        AuthrityControl();

        //有 npoGridView 時才需要
        Util.ShowSysMsgWithScript("$('#btnPreviousPage').hide();$('#btnNextPage').hide();$('#btnGoPage').hide();");

        if (!IsPostBack)
        {
            LoadDropDownListData();
            LoadFormData();
        }
    }
    private void AuthrityControl()
    {
        if (Authrity.PageRight("_Focus") == false)
        {
            return;
        }
        //Authrity.CheckButtonRight("_AddNew", btnAdd);
        Authrity.CheckButtonRight("_Query", btnQuery);
    }
    //-------------------------------------------------------------------------------------------------------------
    public void LoadDropDownListData()
    {
        //服務類別
        string strSql = "select uid, classname from serviceclass";
        Util.FillDropDownList(ddlclassname, strSql, "classname", "uid", true);

        //服務項目
        strSql = "select uid, itemname from ServiceItem where isnull(IsDelete, '') != 'Y'";
        Util.FillDropDownList(ddlitem, strSql, "itemname", "uid", true);
    }
    //-------------------------------------------------------------------------------------------------------------

    //----------------------------------------------------------------------------------------------------------
    private DataTable GetDataTable()
    {
        //讀取資料庫
        /*string strSql = @"
                   SELECT  a.uid as uid ,ClassName As 類別 , itemname As 項目,CONVERT( NVARCHAR(12),BegDate,111 ) AS 活動日期,
                    (SELECT COUNT(*) FROM  ActMap WHERE ActUid =a.uid and ISNULL(IsDelete ,'') != 'Y' ) AS 人數
                    ,(SELECT (CONVERT(NVARCHAR(10),UserName )  + ',') FROM Act a1 
                    INNER JOIN ActMap b ON a1.uid = b.ActUid 
                    INNER JOIN AdminUser c ON b.UserID = c.uid 
                    WHERE a1.uid = a.uid  and  ISNULL(b.IsDelete ,'') != 'Y'
                    GROUP BY UserName FOR XML PATH('') ) AS 參加者
                    FROM Act a 
                    INNER JOIN ServiceItem s ON a.ItemUid = s.uid 
                    INNER JOIN ServiceClass s2 ON s2.uid = s.ClassUid 
                    INNER JOIN ActMap a2 ON a2.ActUid = a.uid 
                      WHERE ISNULL(a.IsDelete ,'')!='Y' and  ISNULL(a2.IsDelete ,'') != 'Y'
                     ";*/
        string strSql = @"
                   SELECT  a.uid as uid ,ClassName As 類別 , itemname As 項目,CONVERT( NVARCHAR(12),BegDate,111 ) AS 活動日期,
                    (SELECT COUNT(*) FROM  ActMap WHERE ActUid =a.uid and ISNULL(IsDelete ,'') != 'Y' ) AS 人數
                    ,rtrim(c.UserName)+'('+c.UserID+')' as 參加者
                    FROM Act a 
                    INNER JOIN ServiceItem s ON a.ItemUid = s.uid 
                    INNER JOIN ServiceClass s2 ON s2.uid = s.ClassUid 
                    INNER JOIN ActMap a2 ON a2.ActUid = a.uid 
                    INNER JOIN AdminUser c ON a2.UserID = c.uid 
                      WHERE ISNULL(a.IsDelete ,'')!='Y' and  ISNULL(a2.IsDelete ,'') != 'Y'
                     ";
        if (ddlclassname.SelectedValue != "")
        {
            strSql += " and a.classuid like @classuid\n";
        }

        if (ddlitem.SelectedValue != "")
        {
            strSql += " and itemuid like @itemuid\n";
        }
        //strSql += " GROUP BY a.uid,ClassName,itemname,BegDate ORDER BY a.uid ";
        strSql += " order by uid,itemname,BegDate,c.UserID ";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("classuid", ddlclassname.SelectedValue);
        dict.Add("itemuid", ddlitem.SelectedValue);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        return dt;
    }

    private void LoadFormData()
    {
        //呼叫資料庫GetDataTable
        DataTable dt = GetDataTable();
        //Grid initial
        NPOGridView npoGridView = new NPOGridView();
        npoGridView.Source = NPOGridViewDataSource.fromDataTable;
        npoGridView.dataTable = dt;
        npoGridView.Keys.Add("uid");
        npoGridView.DisableColumn.Add("uid");
        npoGridView.DisableColumn.Add("itemuid");
        npoGridView.DisableColumn.Add("classuid");

        npoGridView.CurrentPage = Util.String2Number(HFD_CurrentPage.Value);
       // npoGridView.EditLink = Util.RedirectByTime("Act_Edit.aspx", "Mode=Edit&Uid=");

        //kuofly20150325調整畫面的cell
        NPOGridViewColumn col;
        col = new NPOGridViewColumn("類別"); //產生所需欄位及設定 Title
        col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
        col.ColumnName.Add("類別"); //使用欄位名稱
        col.CellStyle = "width:200px";
        npoGridView.Columns.Add(col);

        col = new NPOGridViewColumn("項目"); //產生所需欄位及設定 Title
        col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
        col.ColumnName.Add("項目"); //使用欄位名稱
        col.CellStyle = "width:200px";
        npoGridView.Columns.Add(col);

        col = new NPOGridViewColumn("活動日期"); //產生所需欄位及設定 Title
        col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
        col.ColumnName.Add("活動日期"); //使用欄位名稱
        col.CellStyle = "width:50px";
        npoGridView.Columns.Add(col);

        col = new NPOGridViewColumn("人數"); //產生所需欄位及設定 Title
        col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
        col.ColumnName.Add("人數"); //使用欄位名稱
        col.CellStyle = "width:50px";
        npoGridView.Columns.Add(col);

        col = new NPOGridViewColumn("參加者"); //產生所需欄位及設定 Title
        col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
        col.ColumnName.Add("參加者"); //使用欄位名稱
        col.CellStyle = "width:100px;word-wrap: break-word;";
        npoGridView.Columns.Add(col);

        //

        lblGridList.Text = npoGridView.Render();
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
     //   Response.Redirect(Util.RedirectByTime("Act_Edit.aspx?Mode=ADD&"));
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        LoadFormData();

    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {

        string RptText = GetReport();
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
        Util.OutputTxt(sb + RptText, "1", DateTime.Now.ToString("yyyyMMddHHmmss"));
    }
    // 報表************************************************************************************************************************
    private string GetReport()
    {
        string retStr = "";
        retStr += "<table x:str border=\"0\" bordercolor=\"black\" cellpadding=\"0\" cellspacing=\"0\" align=\"center\" style=\"font-size:12pt;font-family:標楷體;width:800px;\">\n";
        retStr += "\t<tr>\n";
        retStr += "\t\t<td style=\"text-align:center;font-size:25px;border-left:1px solid windowtext;border-top:1px solid windowtext;border-right:1px solid windowtext;\">";
        retStr += "類別</td>\n";
        retStr += "\t\t<td style=\"text-align:center;font-size:25px;border-left:1px solid windowtext;border-top:1px solid windowtext;border-right:1px solid windowtext;\">";
        retStr += "項目</td>\n";
        retStr += "\t\t<td style=\"text-align:center;font-size:25px;border-top:1px solid windowtext;border-right:1px solid windowtext;\">";
        retStr += "活動日期</td>\n";
        retStr += "\t\t<td style=\"text-align:center;font-size:25px;border-top:1px solid windowtext;border-right:1px solid windowtext;\">";
        retStr += "人數</td>\n";
        retStr += "\t\t<td style=\"text-align:center;font-size:25px;border-top:1px solid windowtext;border-right:1px solid windowtext;\">";
        retStr += "參加者</td>\n";
        retStr += "</tr>\n";

        DataTable dt = GetDataTable();
        if (dt.Rows.Count == 0)
        {
            ShowSysMsg("查無資料");
            return "";
        }
        foreach (DataRow dr in dt.Rows)
        {
            retStr += "<tr>\n";
            retStr += "\t<td style=\"text-align:center;font-size:18px;border-left:1px solid windowtext;border-top:1px solid windowtext;border-right:1px solid windowtext;border-bottom:1px solid windowtext;\">";
            retStr += dr["類別"].ToString();
            retStr += "</td>";
            retStr += "\t<td style=\"text-align:center;font-size:18px;border-left:1px solid windowtext;border-top:1px solid windowtext;border-right:1px solid windowtext;border-bottom:1px solid windowtext;\">";
            retStr += dr["項目"].ToString();
            retStr += "</td>";
            retStr += "\t<td style=\"text-align:center;font-size:18px;border-left:1px solid windowtext;border-top:1px solid windowtext;border-right:1px solid windowtext;border-bottom:1px solid windowtext;\">";
            retStr += dr["活動日期"].ToString();
            retStr += "</td>";
            retStr += "\t<td style=\"text-align:center;font-size:18px;border-left:1px solid windowtext;border-top:1px solid windowtext;border-right:1px solid windowtext;border-bottom:1px solid windowtext;\">";
            retStr += dr["人數"].ToString();
            retStr += "</td>";
            retStr += "\t<td style=\"text-align:left;font-size:18px;border-left:1px solid windowtext;border-top:1px solid windowtext;border-right:1px solid windowtext;border-bottom:1px solid windowtext;word-wrap: break-word;\">";
            retStr += dr["參加者"].ToString();
            retStr += "</td>";
            retStr += "</tr>";
        }
        retStr += "</table>";
        return retStr;
    }

    protected void ddlclassname_SelectedIndexChanged(object sender, EventArgs e)
    {
        string myValue = ddlclassname.SelectedValue;
        string strSql;
        //strSql = "select uid, itemname from ServiceItem ";
        strSql = "select uid, itemname from serviceitem where 1=1";
        if (myValue != "")
        {
            strSql += " and ClassUid=cast('" + myValue + "' as varchar)";
        }
        strSql += " and isnull(IsDelete, '') != 'Y'";
        Util.FillDropDownList(ddlitem, strSql, "itemname", "uid", true);
    }
}

