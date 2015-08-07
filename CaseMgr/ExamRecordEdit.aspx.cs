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

public partial class CaseMgr_ExamRecordEdit : BasePage
{
    GroupAuthrity ga = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["ProID"] = "ExamRecord";
        //權限控制
        AuthrityControl();

      
        //取得模式, 新增("ADD")或修改("")
        HFD_Mode.Value = Util.GetQueryString("Mode");

     
        //設定一些按鍵的 Visible 屬性
        //注意:要在 AuthrityControl() 及取得 HFD_Mode.Value 呼叫
        SetButton();

        if (!IsPostBack)
        {
            HFD_AdminUserUID.Value = Util.GetQueryString("AdminUserUID");
            HFD_UID.Value = Util.GetQueryString("UID"); //會員UID
           // functionName();

            //if (functioncnt(HFD_AdminUserUID.Value) == "0")
            //{
            //    HFD_Mode.Value = "ADD";
            //}
            
            LoadFormData();

        }
     }

    private string functioncnt(string AdminUserUID)
    {
        string strSql = "";
        strSql = @" 
                       select count(*) as Cnt from ExamRecord where userid = @AdminUserUID
                          ";
        DataRow dr = null;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("AdminUserUID", AdminUserUID);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        dt = NpoDB.GetDataTableS(strSql, dict);
        dr = dt.Rows[0];
        if (dt.Rows.Count == 0)
        {
            return "0";
        }
        //if (dr["Cnt"].ToString() == "0")
        //{
        //    //新增時, 修改及刪除鍵沒動作
        //    btnUpdate.Visible = false;
        //    //btnDelete.Visible = false;
        //    btnAdd.Visible = true;
        //}
        //else
        //{
        //    //修改時, 新增鍵沒動作
        //    btnAdd.Visible = false;
        //    btnUpdate.Visible = true;
        //    //btnDelete.Visible = true;
        //}
        //dr = dt.Rows[0];
        return dr["Cnt"].ToString();

    }
    private string functionName()
    {
        string strSql = "";
        strSql = @" 
                      select * from AdminUser
                        where uid =@uid
                          ";
        DataRow dr = null;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("uid", HFD_AdminUserUID.Value);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count == 0)
        {
            return "0";
        }
        dr = dt.Rows[0];
        //志工編號
        txtUserID.Text = dr["UserID"].ToString();
        //志工姓名
        txtUserName.Text = dr["UserName"].ToString();
        //到職年
        txtStartData.Text = Util.DateTime2String(dr["StartData"], DateType.yyyyMMdd, EmptyType.ReturnNull);
        return "";

    }
    private void SetupDefaultValue()
    {
    }
    //-------------------------------------------------------------------------
    private void SetButton()
    {

      //  btnDelete.Visible = (SessionInfo.GroupName == "系統管理員") ? true : false;
        if (HFD_Mode.Value == "ADD")
        {
            //新增時, 修改及刪除鍵沒動作
            btnUpdate.Visible = false;
           // btnDelete.Visible = false;
        }
        else
        {
            //修改時, 新增鍵沒動作
            btnAdd.Visible = false;
        }
    }
    //-------------------------------------------------------------------------
    private void AuthrityControl()
    {
        ga = Authrity.GetGroupRight();
        if (Authrity.CheckPageRight(ga.Focus) == false)
        {
            return;
        }
        btnUpdate.Visible = ga.Update;
    }
    //-------------------------------------------------------------------------------------------------------------
   private void LoadFormData()
    {
        string strSql = "";
        DataTable dt = null;
        DataRow dr = null;

        //志工
        strSql = "select uid,UserName from AdminUser";
        Util.FillDropDownList(ddlUserName, strSql, "UserName", "uid", true);

        if (HFD_Mode.Value != "ADD")
        {

            strSql = @"
                        select u.uid as UserID,ExamSData,ExamFData,FillData,UserName,StartData,AttendanceScore,
                        AttendanceText,ProfessionalScore,ProfessionalText,ServiceScore,ServiceText,EthicsScore,EthicsText,
                        CrisisScore,CrisisText,TeamScore,TeamText,ResourcesScore,ResourcesText,other,otherScore,Rating,CounselorText,
                        DirectorText
                         from ExamRecord e
                         right join AdminUser u on u.uid =e.UserID
                        where e.uid=@uid
                      ";


            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("uid", HFD_UID.Value );
            dt = NpoDB.GetDataTableS(strSql, dict);
           // Response.Write(strSql);
            if (dt.Rows.Count == 0)
            {
                //資料異常
                //SetSysMsg("資料異常!");
                Response.Redirect("ExamRecordQry.aspx");
                return;
            }
            dr = dt.Rows[0];


            //評核期間(起)
            txtBegCreateDate.Text = (HFD_Mode.Value == "ADD") ? "" : Util.DateTime2String(dr["ExamSData"], DateType.yyyyMMdd, EmptyType.ReturnNull);
            //評核期間(終)
            txtEndCreateDate.Text = (HFD_Mode.Value == "ADD") ? "" : Util.DateTime2String(dr["ExamFData"], DateType.yyyyMMdd, EmptyType.ReturnNull);
            //填寫日期
            txtFillData.Text = (HFD_Mode.Value == "ADD") ? "" : Util.DateTime2String(dr["FillData"], DateType.yyyyMMdd, EmptyType.ReturnNull);
            //志工編號
            //txtUserID.Text =  dr["UserID"].ToString();
            ddlUserName.SelectedValue = dr["UserID"].ToString();
            //志工姓名

            //txtUserName.Text =  dr["UserName"].ToString();
            //到職年
            //txtStartData.Text = Util.DateTime2String(dr["StartData"], DateType.yyyyMMdd, EmptyType.ReturnEmpty);
            //A評核標準
            ddlAttendance.SelectedValue = (HFD_Mode.Value == "ADD") ? "" : dr["AttendanceScore"].ToString();
            //A輔導員評核
            txtAttendance.Text = (HFD_Mode.Value == "ADD") ? "" : dr["AttendanceText"].ToString();
            //B評核標準
            ddlProfessional.SelectedValue = (HFD_Mode.Value == "ADD") ? "" : dr["ProfessionalScore"].ToString();
            //B輔導員評核
            txtProfessional.Text = (HFD_Mode.Value == "ADD") ? "" : dr["ProfessionalText"].ToString();
            //C評核標準
            ddlService.SelectedValue = (HFD_Mode.Value == "ADD") ? "" : dr["ServiceScore"].ToString();
            //C輔導員評核
            txtService.Text = (HFD_Mode.Value == "ADD") ? "" : dr["ServiceText"].ToString();
            //D評核標準
            ddlEthics.SelectedValue = (HFD_Mode.Value == "ADD") ? "" : dr["EthicsScore"].ToString();
            //D輔導員評核
            txtEthics.Text = (HFD_Mode.Value == "ADD") ? "" : dr["EthicsText"].ToString();
            //E評核標準
            ddlCrisis.SelectedValue = (HFD_Mode.Value == "ADD") ? "" : dr["CrisisScore"].ToString();
            //E輔導員評核
            txtCrisis.Text = (HFD_Mode.Value == "ADD") ? "" : dr["CrisisText"].ToString();
            //F評核標準
            ddlTeam.SelectedValue = (HFD_Mode.Value == "ADD") ? "" : dr["TeamScore"].ToString();
            //F輔導員評核
            txtTeam.Text = (HFD_Mode.Value == "ADD") ? "" : dr["TeamText"].ToString();
            //G評核標準
            ddlResources.SelectedValue = (HFD_Mode.Value == "ADD") ? "" : dr["ResourcesScore"].ToString();
            //G輔導員評核
            txtResources.Text = (HFD_Mode.Value == "ADD") ? "" : dr["ResourcesText"].ToString();
            //H實際表現說明
            txtOther.Text = (HFD_Mode.Value == "ADD") ? "" : dr["other"].ToString();
            //H評核分數
            txtOtherSore.Text = (HFD_Mode.Value == "ADD") ? "" : dr["otherScore"].ToString();
            //評核等第
            RBL_Rating.SelectedValue = (HFD_Mode.Value == "ADD") ? "" : dr["Rating"].ToString();
            //輔導員總評
            txtCounselor.Text = (HFD_Mode.Value == "ADD") ? "" : dr["CounselorText"].ToString();
            //主任總評
            txtDirector.Text = (HFD_Mode.Value == "ADD") ? "" : dr["DirectorText"].ToString();
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {

        Dictionary<string, object> dict = new Dictionary<string, object>();
        List<ColumnData> list = new List<ColumnData>();
        SetColumnData(list);
        string strSql = Util.CreateInsertCommand("ExamRecord", list, dict);
        NpoDB.ExecuteSQLS(strSql, dict);
        Session["Msg"] = "新增資料成功";
        Response.Redirect(Util.RedirectByTime("ExamRecordQry.aspx"));
    }
    //-------------------------------------------------------------------------------------------------------------
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        List<ColumnData> list = new List<ColumnData>();
        SetColumnData(list);
        string strSql = Util.CreateUpdateCommand("ExamRecord", list, dict);
        NpoDB.ExecuteSQLS(strSql, dict);
        Session["Msg"] = "修改資料成功";
        Response.Redirect(Util.RedirectByTime("ExamRecordQry.aspx"));
    }
    //-------------------------------------------------------------------------------------------------------------
    //基本資料
    private void SetColumnData(List<ColumnData> list)
    {

        list.Add(new ColumnData("uid", HFD_UID.Value, false, false, true));
       
        //評核期間(起)
        list.Add(new ColumnData("ExamSData",Util.DateTime2String(txtBegCreateDate.Text, DateType.yyyyMMdd, EmptyType.ReturnEmpty) , true, true, false));
        //評核期間(終)
        list.Add(new ColumnData("ExamFData", Util.DateTime2String(txtEndCreateDate.Text, DateType.yyyyMMdd, EmptyType.ReturnEmpty), true, true, false));
        //填寫日期
        list.Add(new ColumnData("FillData", Util.DateTime2String(txtFillData.Text, DateType.yyyyMMdd, EmptyType.ReturnEmpty), true, true, false));

        //A 評核標準
        list.Add(new ColumnData("AttendanceScore", ddlAttendance.SelectedValue, true, true, false));
        //A 輔導員評核
        list.Add(new ColumnData("AttendanceText", txtAttendance.Text, true, true, false));
        //B 評核標準
        list.Add(new ColumnData("ProfessionalScore", ddlProfessional.SelectedValue, true, true, false));
        //B 輔導員評核
        list.Add(new ColumnData("ProfessionalText", txtProfessional.Text, true, true, false));
        //C 評核標準
        list.Add(new ColumnData("ServiceScore", ddlService.SelectedValue, true, true, false));
        //C 輔導員評核
        list.Add(new ColumnData("ServiceText", txtService.Text, true, true, false));
        //D 評核標準
        list.Add(new ColumnData("EthicsScore", ddlEthics.SelectedValue, true, true, false));
        //D 輔導員評核
        list.Add(new ColumnData("EthicsText", txtEthics.Text, true, true, false));
        //E 評核標準
        list.Add(new ColumnData("CrisisScore", ddlCrisis.SelectedValue, true, true, false));
        //E 輔導員評核
        list.Add(new ColumnData("CrisisText", txtCrisis.Text, true, true, false));
        //F 評核標準
        list.Add(new ColumnData("TeamScore", ddlTeam.SelectedValue, true, true, false));
        //F 輔導員評核
        list.Add(new ColumnData("TeamText", txtTeam.Text, true, true, false));
        //G 評核標準
        list.Add(new ColumnData("ResourcesScore", ddlResources.SelectedValue, true, true, false));
        //G 輔導員評核
        list.Add(new ColumnData("ResourcesText", txtResources.Text, true, true, false));
        //H 其他表現說明
        list.Add(new ColumnData("other", txtOther.Text, true, true, false));
        //H 評核分數
        list.Add(new ColumnData("otherScore", txtOtherSore.Text, true, true, false));
        //評核等第
        list.Add(new ColumnData("Rating", RBL_Rating.SelectedValue, true, true, false));
        //輔導員總評
        list.Add(new ColumnData("CounselorText", txtCounselor.Text, true, true, false));
        //主任總評
        list.Add(new ColumnData("DirectorText", txtDirector.Text, true, true, false));

        //UserID
        list.Add(new ColumnData("userid", ddlUserName .SelectedValue , true, true, false));
     
    }
    //-------------------------------------------------------------------------------------------------------------
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        //把該筆刪除的資料 IsDelete改成Y
        Dictionary<string, object> dict = new Dictionary<string, object>();
        string strSql = @"
                           update ExamRecord set
                           IsDelete='Y',
                           DeleteID=@DeleteID,
                           DeleteDate=GetDate()
                           where uid = @uid
                         ";
        dict.Add("uid", HFD_UID.Value );
        dict.Add("DeleteID", SessionInfo.UserID);
        NpoDB.ExecuteSQLS(strSql, dict);
        Session["Msg"] = "刪除資料成功";
        Response.Redirect(Util.RedirectByTime("ExamRecordQry.aspx"));
    }
    //-------------------------------------------------------------------------------------------------------------
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("ExamRecordQry.aspx");
    }
    //-------------------------------------------------------------------------------------------------------------
    //諮詢資料
    private void SetConsultingColumnData(List<ColumnData> list)
    {
        
     }

    protected void btnUpdateConsulting_Click(object sender, EventArgs e)
    {
       
//        lblRpt.Text = GetRptHead() + GetReport();
//        string s = @"<head>
//                        <style>
//                        @page Section2
//                        { size:841.7pt 595.45pt;
//                          mso-page-orientation:landscape;
//                        margin:1.5cm 0.5cm 0.5cm 0.5cm;}
//                        div.Section2
//                        {page:Section2;}
//                        </style>
//                    </head>
//                    <body>
//                        <div class=Section2>
//                    ";//邊界-上右下左
//        StringBuilder sb = new StringBuilder();
//        //sb.AppendLine(s);
//       // Util.OutputTxt(sb + lblRpt.Text, "1", DateTime.Now.ToString("yyyyMMddHHmmss"));
    }

    //諮商類別
    private void LoadHiddenData()
    {
        
    }
    // 報表************************************************************************************************************************
    private string GetReport()
    {
        return "";
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
//        string strSql = @"
//                  select Top 5 ROW_NUMBER() OVER(ORDER BY CreateDate) AS 筆數, (c.Name + d.Name + b.Address) FullAddress, 
//                         REVERSE(SUBSTRING(REVERSE(ConsultantMain),CHARINDEX(',',REVERSE(ConsultantMain))+1,LEN(ConsultantMain))) ConsultantMainALL,
//                         REVERSE(SUBSTRING(REVERSE(ConsultantItem),CHARINDEX(',',REVERSE(ConsultantItem))+1,LEN(ConsultantItem))) ConsultantItemALL
//                        ,*
//                        from Consulting a  
//                        inner join Member b on a.MemberUID = b.uid
//                        left join CodeCity c on b.City = c.ZipCode 
//                        left join CodeCity d on b.ZipCode = d.ZipCode     
//                  where 1=1 
//                  And a.MemberUID=@MemberUID
//                  and isnull(a.IsDelete, '') != 'Y'
//                  Order by CreateDate desc
//                        "; 
//        Dictionary<string, object> dict = new Dictionary<string, object>();
//        dict.Add("MemberUID", HFD_MemberID.Value);
//        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
//        return dt;
//    }

//    private void CreateConsultingData()
//    {
//        Dictionary<string, object> dict = new Dictionary<string, object>();
//        List<ColumnData> list = new List<ColumnData>();
//        string strSql = "";
//        string NewUID = "";
//        SetColumnData(list);

//        List<ColumnData> list2 = new List<ColumnData>();
//        if (HFD_ConsultingUID.Value == "")
//        {
//            SetConsultingColumnData(list2);
//            strSql = Util.CreateInsertCommand("Consulting", list2, dict);
//            strSql += "select @@IDENTITY";
//            HFD_ConsultingUID.Value = NpoDB.GetScalarS(strSql, dict);
//        }
        return null;
     }

    private void LoadConsulting()
    {
//        string strSql;
//        DataRow dr = null;
//        DataTable dt = null;
//        Dictionary<string, object> dict = new Dictionary<string, object>();
       
//         strSql = @"
//                   select *
//                   from Consulting
//                   where uid=@uid
//                   and isnull(IsDelete, '') != 'Y'
//                  ";          // 
//         dict.Add("uid", HFD_ConsultingUID.Value );
//        dt = NpoDB.GetDataTableS(strSql, dict);
//        //資料異常
//        if (dt.Rows.Count == 0)
//        {
//            return;
//        }
//        dr = dt.Rows[0];
//        //建檔時間
//        txtCreateDate.Text = (HFD_Mode.Value == "ADD") ? Util.GetToday(DateType.yyyyMMdd) : Util.DateTime2String(dr["CreateDate"].ToString(), DateType.yyyyMMdd, EmptyType.ReturnEmpty);
//        txtCreateTime.Text = (HFD_Mode.Value == "ADD") ? Util.GetToday(DateType.HHmmss) : Util.DateTime2String(dr["CreateDate"].ToString(), DateType.HHmmss, EmptyType.ReturnEmpty);
//        txtEndTime.Text = (HFD_Mode.Value == "ADD") ? Util.GetToday(DateType.HHmmss) : Util.DateTime2String(dr["EndDate"].ToString(), DateType.HHmmss, EmptyType.ReturnEmpty);
//        //服務志工
//        txtServiceUser.Text = (HFD_Mode.Value == "ADD") ? "" : dr["ServiceUser"].ToString();
//        //事件
//        txtEvent.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Event"].ToString();
//        //想法
//        txtThink.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Think"].ToString();
//        //感受
//        txtFeel.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Feel"].ToString();
//        //補充說明
//        txtComment.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Comment"].ToString();
//        //來電諮詢別(大類)
//        HFD_ConsultantMain.Value = (HFD_Mode.Value == "ADD") ? "" : dr["ConsultantMain"].ToString();
//        //來電諮詢別 (分項)
//        HFD_ConsultantItem.Value = (HFD_Mode.Value == "ADD") ? "" : dr["ConsultantItem"].ToString();
//        //騷擾電話
//        HFD_HarassPhone.Value = (HFD_Mode.Value == "ADD") ? "" : dr["HarassPhone"].ToString();
//        //騷擾電話(其他)
//        txtHarassOther.Text = (HFD_Mode.Value == "ADD") ? "" : dr["HarassOther"].ToString();
//        //轉介單位
//        HFD_IntroductionUnit.Value = (HFD_Mode.Value == "ADD") ? "" : dr["IntroductionUnit"].ToString();
//        //緊急轉介
//        HFD_CrashIntroduction.Value = (HFD_Mode.Value == "ADD") ? "" : dr["CrashIntroduction"].ToString();
//        //原因1
//        //txtReason1.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Reason1"].ToString();
//        //原因2
//        //txtReason2.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Reason2"].ToString();
//        //原因3
//        //txtReason3.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Reason3"].ToString();
//        //問題1
//        //txtTrouble1.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Trouble1"].ToString();
//        //問題2
//        //txtTrouble2.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Trouble2"].ToString();
//        //問題3
//        //txtTrouble3.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Trouble3"].ToString();
//        ////因...造成問題
//        txtProblem.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Problem"].ToString();
//        //找出案主心中的假設
//        txtFindAssume.Text = (HFD_Mode.Value == "ADD") ? "" : dr["FindAssume"].ToString();
//        //與案主討論解釋與假設
//        txtDiscuss.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Discuss"].ToString();
//        //加入新的元素
//        txtElement.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Element"].ToString();
//        //改變案主原先的期待
//        txtExpect.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Expect"].ToString();
//        //給予案主祝福與盼望
//        txtBlessing.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Blessing"].ToString();
//        //轉介教會
//        txtIntroductionChurch.Text = (HFD_Mode.Value == "ADD") ? "" : dr["IntroductionChurch"].ToString();
//        //我對個案的幫助程度
//        rdoHelpLvMark.SelectedValue = (HFD_Mode.Value == "ADD") ? "" : dr["HelpLvMark"].ToString();
//        //小叮嚀          
//        txtMemo.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Memo"].ToString();

    }
    //日期檢查
    public bool isDate(string dateStr)
    {
        bool _isDate = false;
        string matchStr = "";
        matchStr += @"^((?!0000)[0-9]{4}/((0?[1-9]|1[0-2])/(0?[1-9]|1[0-9]|2[0-8])|(0[13-9]|1[0-2])-(29|30)|(0[13578]|1[02])-31)|([0-9]{2}(0[48]|[2468][048]|[13579][26])|(0[48]|[2468][048]|[13579][26])00)-02-29)$  ";
        RegexOptions option = (RegexOptions.IgnoreCase | (RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace));

        if (Regex.IsMatch(dateStr, matchStr, option))
            _isDate = true;
        else
            _isDate = false;
        return _isDate;
    }
    public bool isTime(string dateStr)
    {
        bool _isTime = false;
        string matchStr = "";
        matchStr += @"^((20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$";
        RegexOptions option = (RegexOptions.IgnoreCase | (RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace));

        if (Regex.IsMatch(dateStr, matchStr, option))
            _isTime = true;
        else
            _isTime = false;

        return _isTime;
     }

    private string chkField()
    {
        //if (isDate(txtCreateDate.Text) == false)
        //{
        //    return "日期格式錯誤，格式為 yyyy/mm/dd";
        //}

        //if (isTime(txtCreateTime.Text) == false)
        //{
        //    return "時間格式錯誤，格式為 hh:mm:ss";
        //}

        return "";
    }
    protected void btnEndDate_Click(object sender, EventArgs e)
    {
        //DateTime t1, t2;
        //t1 = DateTime.Parse(txtCreateDate.Text + " " + txtCreateTime.Text);
        //if (txtEndTime.Text == "")
        //{
        //    btnEndDate.Text = "計算時間";
        //    txtEndTime.Text = Util.GetToday(DateType.HHmmss);
        //    t2 = DateTime.Parse(txtCreateDate.Text + " " + txtEndTime.Text);
        //    lblTime.Text = DateDiff(t1, t2);
        //}
        //else
        //{
        //    t2 = DateTime.Parse(txtCreateDate.Text + " " + txtEndTime.Text);
        //    lblTime.Text = DateDiff(t1, t2);
        //}
    }

    private string DateDiff(DateTime DateTime1, DateTime DateTime2)
    {
        string dateDiff = null;
        TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
        TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
        TimeSpan ts = ts1.Subtract(ts2).Duration();
        //dateDiff = "通話時間共：" + (int.Parse(ts.Days.ToString()) * 1440 + int.Parse(ts.Hours.ToString()) * 60 + int.Parse(ts.Minutes.ToString())).ToString() + "分" + ts.Seconds.ToString() + "秒";
        dateDiff = "通話時間共：<span style=color:red>" + (int.Parse(ts.Days.ToString()) * 1440 + int.Parse(ts.Hours.ToString()) * 60 + int.Parse(ts.Minutes.ToString())).ToString() + "分" + ts.Seconds.ToString() + "秒</span>";
        return dateDiff;
    }

    private void GetPhoneToUid()
    {
//         string strSql;
//        DataRow dr = null;
//        DataTable dt = null;
//        Dictionary<string, object> dict = new Dictionary<string, object>();
//        Dictionary<string, object> dict2 = new Dictionary<string, object>();
//        Dictionary<string, object> dict3 = new Dictionary<string, object>();

//        //由電話系統取得電話**************************************************************
//        strSql = @"
//                   select top 1 phone
//                   from InBound
//                   where IP=@IP
//                   and isnull(IsProcess, '') != 'Y'
//                  ";

//        HttpCookie CookieAgentID = Request.Cookies["AgentID"];  
//        dict.Add("IP",  Server.UrlDecode(CookieAgentID.Value));//Request.ServerVariables["REMOTE_ADDR"]
//        dt = NpoDB.GetDataTableS(strSql, dict);
//        //資料異常
//        if (dt.Rows.Count == 0)
//        {
//           // ShowSysMsg("無新來電");
//            return;
//        }
//        dr = dt.Rows[0];

//        //將此client IP 的是否已接聽得值 都update 為 已接聽***************************************************************

//        strSql = @"
//                   Update InBound 
//                   Set IsProcess='Y',
//                       UpdateID=@UpdateID,
//                       UpdateDate=@UpdateDate
//                   where 1=1
//                   And IP=@IP
//                   and isnull(IsProcess, '') != 'Y'
//                  ";
//        dict2.Add("IP", Server.UrlDecode(CookieAgentID.Value));//Request.ServerVariables["REMOTE_ADDR"]
//        dict2.Add("UpdateID", SessionInfo.UserID);
//        dict2.Add("UpdateDate", Util.GetToday(DateType.yyyyMMddHHmmss));
//        NpoDB.ExecuteSQLS(strSql, dict2);

//        //從電話號碼找會員資料*******************************************
//        strSql = @"
//                   select *
//                   from Member
//                   where phone=@phone
//                   and isnull(IsDelete, '') != 'Y'
//                  ";
//        dict3.Add("phone", HFD_Phone.Value);
//        dt = NpoDB.GetDataTableS(strSql, dict3);
//        //資料異常
//        if (dt.Rows.Count == 0)
//        {
//            HFD_UID.Value = "";
//        }
//        else
//        {
//            dr = dt.Rows[0];
//            HFD_UID.Value = dr["uid"].ToString();
//            HFD_MemberID.Value = dr["uid"].ToString();
//        }
     
    }
    protected void btnSum_Click(object sender, EventArgs e)
    {
        int sum = 0;
        int   Other=0;
        if (txtOtherSore.Text != "")
        Other =int.Parse(txtOtherSore.Text) ;
       sum = int.Parse(ddlAttendance.SelectedValue) + int.Parse(ddlProfessional.SelectedValue);
          sum +=  int.Parse(ddlService.SelectedValue) + int.Parse(ddlEthics.SelectedValue);
          sum += int.Parse(ddlCrisis.SelectedValue) + int.Parse(ddlTeam.SelectedValue);
          sum += int.Parse(ddlResources.SelectedValue) + Other;
          lblSum.Text = sum.ToString() ;

    }





    protected void btnPrint_Click(object sender, EventArgs e)
    {
        //讀取本案相對資料
        string strSql = "";
        DataTable dt = null;
        DataRow dr = null;

        //志工
        strSql = @"
            select u.uid as UserID,ExamSData,ExamFData,FillData,UserName,StartData,AttendanceScore,
            AttendanceText,ProfessionalScore,ProfessionalText,ServiceScore,ServiceText,EthicsScore,EthicsText,
            CrisisScore,CrisisText,TeamScore,TeamText,ResourcesScore,ResourcesText,other,otherScore,Rating,CounselorText,
            DirectorText
            from ExamRecord e
            right join AdminUser u on u.uid =e.UserID
            where e.uid=@uid
            ";
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("uid", HFD_UID.Value);
            dt = NpoDB.GetDataTableS(strSql, dict);
            if (dt.Rows.Count == 0)
            {
                //資料異常
                //SetSysMsg("資料異常!");
                Response.Redirect("ExamRecordQry.aspx");
                return;
            }
            dr = dt.Rows[0];
            

        //這是桃園社工師案設計的郭氏報表
        Dictionary<string, object> myReport = new Dictionary<string, object>();
        myReport.Add("評核期間起", Util.DateTime2String(dr["ExamSData"], DateType.yyyyMMdd, EmptyType.ReturnNull));
        myReport.Add("評核期間迄", Util.DateTime2String(dr["ExamFData"], DateType.yyyyMMdd, EmptyType.ReturnNull));
        myReport.Add("填寫日期", Util.DateTime2String(dr["FillData"], DateType.yyyyMMdd, EmptyType.ReturnNull));
        myReport.Add("志工編號", dr["UserID"].ToString());
        myReport.Add("志工姓名", dr["UserName"].ToString());
        myReport.Add("到職服務日期", Util.DateTime2String(dr["StartData"], DateType.yyyyMMdd, EmptyType.ReturnEmpty));

        //A
        string 出勤狀況成績 = dr["AttendanceScore"].ToString();
        string 出勤狀況成績組合文字 = "";
        出勤狀況成績組合文字 += (出勤狀況成績 == "5" ? "●" : "○") + "優21-25<br>";
        出勤狀況成績組合文字 += (出勤狀況成績 == "4" ? "●" : "○") + "佳16-20<br>";
        出勤狀況成績組合文字 += (出勤狀況成績 == "3" ? "●" : "○") + "良11-15<br>";
        出勤狀況成績組合文字 += (出勤狀況成績 == "2" ? "●" : "○") + "可6-10<br>";
        出勤狀況成績組合文字 += (出勤狀況成績 == "1" ? "●" : "○") + "待改善0-5";
        myReport.Add("出勤狀況成績", 出勤狀況成績組合文字);
        string 出勤狀況 = dr["AttendanceText"].ToString();
        myReport.Add("出勤狀況", 出勤狀況 == "" ? "&nbsp;<br/><br/><br/>" : 出勤狀況);

        //B
        string 專業助人能力成績 = dr["ProfessionalScore"].ToString();
        string 專業助人能力成績組合文字 = "";
        專業助人能力成績組合文字 += (專業助人能力成績 == "5" ? "●" : "○") + "優21-25<br>";
        專業助人能力成績組合文字 += (專業助人能力成績 == "4" ? "●" : "○") + "佳16-20<br>";
        專業助人能力成績組合文字 += (專業助人能力成績 == "3" ? "●" : "○") + "良11-15<br>";
        專業助人能力成績組合文字 += (專業助人能力成績 == "2" ? "●" : "○") + "可6-10<br>";
        專業助人能力成績組合文字 += (專業助人能力成績 == "1" ? "●" : "○") + "待改善0-5";
        myReport.Add("專業助人能力成績",專業助人能力成績組合文字);
        string 專業助人能力 = dr["ProfessionalText"].ToString();
        myReport.Add("專業助人能力", 專業助人能力 == "" ? "&nbsp;<br/><br/><br/>" : 專業助人能力);
        
        //C
        string 服務與學習意願成績 = dr["ServiceScore"].ToString();
        string 服務與學習意願成績組合文字 = "";
        服務與學習意願成績組合文字 += (服務與學習意願成績 == "5" ? "●" : "○") + "優9-10<br>";
        服務與學習意願成績組合文字 += (服務與學習意願成績 == "4" ? "●" : "○") + "佳7-8<br>";
        服務與學習意願成績組合文字 += (服務與學習意願成績 == "3" ? "●" : "○") + "良5-6<br>";
        服務與學習意願成績組合文字 += (服務與學習意願成績 == "2" ? "●" : "○") + "可3-4<br>";
        服務與學習意願成績組合文字 += (服務與學習意願成績 == "1" ? "●" : "○") + "待改善1-2";
        myReport.Add("服務與學習意願成績", 服務與學習意願成績組合文字);
        string 服務與學習意願 = dr["ServiceText"].ToString();
        myReport.Add("服務與學習意願", 服務與學習意願 == "" ? "&nbsp;<br/><br/><br/>" : 服務與學習意願);
        
        //D
        string 專業助人倫理成績 = dr["EthicsScore"].ToString();
        string 專業助人倫理成績組合文字 = "";
        專業助人倫理成績組合文字 += (專業助人倫理成績 == "5" ? "●" : "○") + "優9-10<br>";
        專業助人倫理成績組合文字 += (專業助人倫理成績 == "4" ? "●" : "○") + "佳7-8<br>";
        專業助人倫理成績組合文字 += (專業助人倫理成績 == "3" ? "●" : "○") + "良5-6<br>";
        專業助人倫理成績組合文字 += (專業助人倫理成績 == "2" ? "●" : "○") + "可3-4<br>";
        專業助人倫理成績組合文字 += (專業助人倫理成績 == "1" ? "●" : "○") + "待改善1-2";
        myReport.Add("專業助人倫理成績", 專業助人倫理成績組合文字);
        string 專業助人倫理 = dr["EthicsText"].ToString();
        myReport.Add("專業助人倫理", 專業助人倫理 == "" ? "&nbsp;<br/><br/><br/>" : 專業助人倫理);
        
        //E
        string 危機處理能力成績 = dr["CrisisScore"].ToString();
        string 危機處理能力成績組合文字 = "";
        危機處理能力成績組合文字 += (危機處理能力成績 == "5" ? "●" : "○") + "優9-10<br>";
        危機處理能力成績組合文字 += (危機處理能力成績 == "4" ? "●" : "○") + "佳7-8<br>";
        危機處理能力成績組合文字 += (危機處理能力成績 == "3" ? "●" : "○") + "良5-6<br>";
        危機處理能力成績組合文字 += (危機處理能力成績 == "2" ? "●" : "○") + "可3-4<br>";
        危機處理能力成績組合文字 += (危機處理能力成績 == "1" ? "●" : "○") + "待改善1-2";
        myReport.Add("危機處理能力成績", 危機處理能力成績組合文字);
        string 危機處理能力 = dr["CrisisText"].ToString();
        myReport.Add("危機處理能力", 危機處理能力 == "" ? "&nbsp;<br/><br/><br/>" : 危機處理能力);

        //F
        string 團隊關係成績 = dr["TeamScore"].ToString();
        string 團隊關係成績組合文字 = "";
        團隊關係成績組合文字 += (團隊關係成績 == "5" ? "●" : "○") + "優9-10<br>";
        團隊關係成績組合文字 += (團隊關係成績 == "4" ? "●" : "○") + "佳7-8<br>";
        團隊關係成績組合文字 += (團隊關係成績 == "3" ? "●" : "○") + "良5-6<br>";
        團隊關係成績組合文字 += (團隊關係成績 == "2" ? "●" : "○") + "可3-4<br>";
        團隊關係成績組合文字 += (團隊關係成績 == "1" ? "●" : "○") + "待改善1-2";
        myReport.Add("團隊關係成績", 團隊關係成績組合文字);
        string 團隊關係 = dr["TeamText"].ToString();
        myReport.Add("團隊關係", 團隊關係 == "" ? "&nbsp;<br/><br/><br/>" : 團隊關係);
        
        //G
        string 資源運用成績 = dr["ResourcesScore"].ToString();
        string 資源運用成績組合文字 = "";
        資源運用成績組合文字 += (資源運用成績 == "5" ? "●" : "○") + "優9-10<br>";
        資源運用成績組合文字 += (資源運用成績 == "4" ? "●" : "○") + "佳7-8<br>";
        資源運用成績組合文字 += (資源運用成績 == "3" ? "●" : "○") + "良5-6<br>";
        資源運用成績組合文字 += (資源運用成績 == "2" ? "●" : "○") + "可3-4<br>";
        資源運用成績組合文字 += (資源運用成績 == "1" ? "●" : "○") + "待改善1-2";
        myReport.Add("資源運用成績", 資源運用成績組合文字);
        string 資源運用 = dr["ResourcesText"].ToString();
        myReport.Add("資源運用", 資源運用 == "" ? "&nbsp;<br/><br/><br/>" : 資源運用);
        
        //H
        string 實際表現說明 = dr["other"].ToString().Replace("\n","<br/>");
        myReport.Add("實際表現說明", 實際表現說明 == "" ? "&nbsp;<br/><br/><br/>" : 實際表現說明);
        myReport.Add("評核分數", "&nbsp;" + dr["otherScore"].ToString() + "&nbsp;");

        //評核等第 dr["Rating"].ToString()
        string 評核等第=dr["Rating"].ToString();
        myReport.Add("評核分數A正", (評核等第=="A+" ? "●":"○"));
        myReport.Add("評核分數A", (評核等第 == "B" ? "●" : "○"));
        myReport.Add("評核分數B", (評核等第 == "C" ? "●" : "○"));
        myReport.Add("評核分數C", (評核等第 == "D" ? "●" : "○"));
        myReport.Add("評核分數D", (評核等第 == "E" ? "●" : "○"));

        string 輔導員總評 = dr["CounselorText"].ToString().Replace("\n","<br/>");
        myReport.Add("輔導員總評", 輔導員總評 == "" ? "&nbsp;<br/><br/><br/><br/>" : 輔導員總評);
        string 主任總評 = dr["DirectorText"].ToString().Replace("\n","<br/>");
        myReport.Add("主任總評", 主任總評 == "" ? "&nbsp;<br/><br/><br/><br/>" : 主任總評);

        //第二大階段，讀入及組合報表
        
        //放置讀取結果容器
        StringBuilder sb = new StringBuilder();
        //讀取檔案
        StreamReader sr = new StreamReader(Server.MapPath(".\\ExamRecordPrn.htm").ToString());

        sb.Append(sr.ReadToEnd().ToString());
        //關閉讀取動作
        sr.Close();

        //替換關鍵字
        foreach (KeyValuePair<string, object> myKey in myReport)
        {
            sb.Replace("<!--" + myKey.Key + "-->", myKey.Value.ToString());
        }

        Util.OutputTxt(sb.ToString() , "2", "" + DateTime.Now.ToString("yyyyMMddHHmmss"));
    }


}

 