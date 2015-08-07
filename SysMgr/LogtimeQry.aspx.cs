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

public partial class CaseMgr_LogtimeQry : BasePage
{
    GroupAuthrity ga = null;
    #region NpoGridView 處理換頁相關程式碼
    Button btnNextPage, btnPreviousPage, btnGoPage;
    HiddenField HFD_CurrentPage;
    HiddenField HFD_CurrentPage2;
    Literal lit1;
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
        if (Session["查詢當日多重登入名單"] == "True")
        {
            btnNextPage.Click += new System.EventHandler(btnNextPage_Click2);
        }
        else {
            btnNextPage.Click += new System.EventHandler(btnNextPage_Click);
        }
               

        btnPreviousPage = new Button();
        btnPreviousPage.ID = "btnPreviousPage";
        Form1.Controls.Add(btnPreviousPage);
        if (Session["查詢當日多重登入名單"] == "True")
        {
            btnPreviousPage.Click += new System.EventHandler(btnPreviousPage_Click2);
        } else {
            btnPreviousPage.Click += new System.EventHandler(btnPreviousPage_Click);
        }

        btnGoPage = new Button();
        btnGoPage.ID = "btnGoPage";
        Form1.Controls.Add(btnGoPage);
        if (Session["查詢當日多重登入名單"] == "True")
        {
            btnGoPage.Click += new System.EventHandler(btnGoPage_Click2);
        }else{
            btnGoPage.Click += new System.EventHandler(btnGoPage_Click);
        }

        HFD_CurrentPage = new HiddenField();
        HFD_CurrentPage.Value = "1";
        HFD_CurrentPage.ID = "HFD_CurrentPage";
        Form1.Controls.Add(HFD_CurrentPage);

        //單日多重登入名單的頁
        HFD_CurrentPage2 = new HiddenField();
        HFD_CurrentPage2.Value = "1";
        HFD_CurrentPage2.ID = "HFD_CurrentPage2";
        Form1.Controls.Add(HFD_CurrentPage2);
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

    protected void btnPreviousPage_Click2(object sender, EventArgs e)
    {
        HFD_CurrentPage2.Value = Util.MinusStringNumber(HFD_CurrentPage2.Value);
        LoadFormData2();
    }
    protected void btnNextPage_Click2(object sender, EventArgs e)
    {
        HFD_CurrentPage2.Value = Util.AddStringNumber(HFD_CurrentPage2.Value);
        LoadFormData2();
    }
    protected void btnGoPage_Click2(object sender, EventArgs e)
    {
        try{
            HFD_CurrentPage2.Value = Request.Form["Gopage"].ToString();
        }catch (Exception){}
        
        LoadFormData2();
    }
    #endregion NpoGridView 處理換頁相關程式碼
    //---------------------------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {

        Session["ProgID"] = "Logtime";
        //權控處理
        AuthrityControl();

        //有 npoGridView 時才需要
        Util.ShowSysMsgWithScript("$('#btnPreviousPage').hide();$('#btnNextPage').hide();$('#btnGoPage').hide();");

        if (!IsPostBack)
        {
            Session["查詢當日多重登入名單"] = "";
            LoadFormData();
        }
    }
    private void AuthrityControl()
    {
        if (Authrity.PageRight("_Focus") == false)
        {
            return;
        }
        Authrity.CheckButtonRight("_AddNew", btnAdd);
        Authrity.CheckButtonRight("_Query", btnQuery);
    }
    //-------------------------------------------------------------------------------------------------------------
    private void LoadFormData()
    {
        //呼叫資料庫GetDataTable
        DataTable dt = GetDataTable();
        //Grid initial
        NPOGridView npoGridView = new NPOGridView();
        npoGridView.Source = NPOGridViewDataSource.fromDataTable;
        npoGridView.dataTable = dt;
        npoGridView.Keys.Add("Uid");
        npoGridView.DisableColumn.Add("Uid");

        npoGridView.CurrentPage = Util.String2Number(HFD_CurrentPage.Value);
        npoGridView.EditLinkTarget = "','target=_blank,help=no,status=yes,resizable=yes,scrollbars=yes,scrolling=yes,scroll=yes,center=yes,width=900px,height=500px";
        npoGridView.EditLink = Util.RedirectByTime("LogtimeEdit.aspx", "Uid=");

        lblGridList.Text = npoGridView.Render();
        lblScript.Text = "";
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        //Response.Redirect(Util.RedirectByTime("LogtimeEdit.aspx?Mode=ADD&"));
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        Session["查詢當日多重登入名單"] = "";
        LoadFormData();
    }

    private void LoadFormData2()
    {
        //呼叫資料庫GetDataTable
        DataTable dt = 取得當日多重登入資料();

        //Grid initial
        NPOGridView npoGridView = new NPOGridView();
        npoGridView.Source = NPOGridViewDataSource.fromDataTable;
        npoGridView.dataTable = dt;
        npoGridView.Keys.Add("Uid");
        npoGridView.DisableColumn.Add("Uid");

        npoGridView.CurrentPage = Util.String2Number(HFD_CurrentPage2.Value);
        npoGridView.EditLinkTarget = "','target=_blank,help=no,status=yes,resizable=yes,scrollbars=yes,scrolling=yes,scroll=yes,center=yes,width=900px,height=500px";
        npoGridView.EditLink = Util.RedirectByTime("LogtimeEdit.aspx", "Uid=");

        lblGridList.Text = npoGridView.Render();

        string fixStr = "";
        fixStr += "<script language=\"javascript\" type=\"text/javascript\">\n";
        fixStr += " function makeLink(){\n";
        fixStr += "     //kuofly2015 這個邪惡的寫法是透過抓取畫面所有的TAG為TABLE者，其中CLASSNAME=TABLE_H的\n";
        fixStr += "     //將每一列強迫放上script作用\n";
        fixStr += " var myTable = document.getElementsByTagName(\"TABLE\");\n";
        fixStr += "     for (x = 0; x < myTable.length; x++) {\n";
        fixStr += "         if (myTable[x].className == \"table_h\") {\n";
        fixStr += "             var myTableRight = myTable[x];\n";
        fixStr += "             var myRows = myTableRight.rows.length;\n";
        fixStr += "             for (i = 0; i < myRows; i++) {\n";
        fixStr += "                 if (i > 0) {\n";
        fixStr += "                     var objRow = myTableRight.rows[i];\n";
        fixStr += "                     objRow.onclick = function () {\n";
        fixStr += "                         document.getElementById('txtUserName').value = this.getElementsByTagName(\"TD\")[1].getElementsByTagName(\"SPAN\")[0].outerText;\n";
        fixStr += "                         document.getElementById('txtlogtDate').value = this.getElementsByTagName(\"TD\")[0].getElementsByTagName(\"SPAN\")[0].outerText;\n";
        fixStr += "                     };\n";
        fixStr += "                 }\n";
        fixStr += "             }\n";
        fixStr += "         }\n";
        fixStr += "     }\n";
        fixStr += " }\n";
        fixStr += " makeLink();\n";
        fixStr += "</script>\n";
        lblScript.Text = fixStr;
    }

    private DataTable 取得當日多重登入資料()
    {
        //讀取資料庫
        string strSql = @"
            select '' uid,logtime 上班日期,userName 志工名稱,count(*) 單日登入次數 from 
	            (
	            select uid,
	            (select top 1 UserName from adminuser where adminuser.UserID=log.UserID and IsUse='1') userName,
	            CONVERT(varchar,logtime,111) logtime from log
	            where ISNULL(LogTime,'')<>'' and ISNULL(logouttime,'')<>''
	            ) talbe1
            where isnull(UserName,'')<>''
            group by logtime,userName
            having count(logtime)>1
            order by logtime desc
            ";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        
        return dt;
    }
    protected void btnQueryEx_Click(object sender, EventArgs e)
    {
        Session["查詢當日多重登入名單"] = "True";
        HFD_CurrentPage2.Value = "1";
        LoadFormData2();
    }
      //----------------------------------------------------------------------------------------------------------
    private DataTable GetDataTable()
    {
        //讀取資料庫
        string strSql = @"
                        
                        select l.uid as uid ,l.UserID as UserID,a.UserName as 志工名稱,LogTime as 上班時間, LogoutTime as 下班時間
						from log l
						inner join AdminUser a on a.UserID =l.UserID
                        where 1=1   
                        ";
        if (txtUserName.Text != "")
        {
            strSql += " and a.UserName like @UserName\n";
        }
        if (txtlogtDate.Text != "")
        {
            strSql += " and CONVERT(varchar(10), LogTime, 111) = '" + txtlogtDate.Text + "'";
        }
        //Response.Write(strSql);
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("UserName", "%" + txtUserName.Text + "%");
        //dict.Add("LogTime", txtlogtDate.Text);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        return dt;
    }

}

