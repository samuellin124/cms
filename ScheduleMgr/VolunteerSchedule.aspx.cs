using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Data;

public partial class VolunteerSchedule : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["ProgID"] = "VolunteerSchedule";
        //權限控制
        AuthrityControl();

        if (!IsPostBack)
        {
            btnAdd.Visible = false;
            LoadDropDownListData();
        }
    }
    //---------------------------------------------------------------------------
    public void setButton()
    {
    }
    //---------------------------------------------------------------------------
    private void AuthrityControl()
    {
        if (Authrity.PageRight("_Focus") == false)
        {
            return;
        }
        Authrity.CheckButtonRight("_AddNew", btnAdd);
        Authrity.CheckButtonRight("_AddNew", btnUpdate);
        Authrity.CheckButtonRight("_Query", btnQuery);
    }
    //-------------------------------------------------------------------------------------------------------------
    public void LoadDropDownListData()
    {
        //志工列表
        DataTable dt = Util.GetDataTable("AdminUser", "IsUse", "1", "UserID", "");
        Util.FillDropDownList(ddlVoluntee, dt, "UserName", "UserID", true);
        //年月選擇
        lblYearMonth.Text = HtmlUtil.GetDropDownList("ddlYear", DateTime.Now.Year - 1, DateTime.Now.Year + 1, false, DateTime.Now.Year.ToString()) +
                            HtmlUtil.Space(1) + "年" + HtmlUtil.Space(1) +
                            HtmlUtil.GetDropDownList("ddlMonth", 1, 12, false, DateTime.Now.Month.ToString()) +
                            HtmlUtil.Space(1) + "月";
        HFD_Year.Value = DateTime.Now.Year.ToString();
        HFD_Month.Value = DateTime.Now.Month.ToString();
    }
    //----------------------------------------------------------------------------
    private string GetYearMonth()
    {
        string Year = Util.GetControlValue("ddlYear").PadLeft(4, '0');
        string Month = Util.GetControlValue("ddlMonth").PadLeft(2, '0');
        return Year + "/" + Month;
    }
    //----------------------------------------------------------------------------
    public void LoadFormData()
    {
        DataTable dt = CreateTable();

        string UserID = ddlVoluntee.SelectedValue;
        string YearMonth = GetYearMonth();
        DataTable dtSchedule = Util.GetDataTable("ScheduleDetail", "UserID", UserID, "CONVERT(char(7), SchDate, 111)", YearMonth, "SchDate", "");

        foreach (DataRow dr in dtSchedule.Rows)
        {
            DateTime d = (DateTime)dr["SchDate"];
            dt.Rows[d.Day - 1]["上午班"] = dr["periodA"].ToString();
            dt.Rows[d.Day - 1]["下午班"] = dr["periodB"].ToString();
            dt.Rows[d.Day - 1]["晚上班"] = dr["periodC"].ToString();
        }
        //需自訂欄位時
        NPOGridView npoGridView = new NPOGridView();
        npoGridView.Source = NPOGridViewDataSource.fromDataTable;
        npoGridView.dataTable = dt;
        npoGridView.Keys.Add("uid");
        npoGridView.DisableColumn.Add("uid");
        npoGridView.ShowPage = false;
        //-----------------------------------------------------------------------
        //欄位為 checkbox
        NPOGridViewColumn col = new NPOGridViewColumn("日期");
        col.ColumnType = NPOColumnType.Textbox;
        col.ControlKeyColumn.Add("uid");
        col.Readonly = true;

        col.ControlName.Add("txtScheduleDate");
        col.ControlId.Add("txtScheduleDate");
        //col.ControlValue.Add("1");
        col.ColumnName.Add("日期");
        npoGridView.Columns.Add(col);
        //-------------------------------------------------------------------------
       
        col = new NPOGridViewColumn("星期");
        col.ColumnType = NPOColumnType.Textbox;
        col.ControlKeyColumn.Add("uid");
        col.Readonly = true;

        col.ControlName.Add("txtDayWeek");
        col.ControlId.Add("txtDayWeek");
        col.ColumnName.Add("星期");
        npoGridView.Columns.Add(col);
        //-------------------------------------------------------------------------
        
        col = new NPOGridViewColumn("上午班");
        col.ColumnType = NPOColumnType.Dropdownlist;
        col.ControlKeyColumn.Add("uid");
        col.ControlName.Add("chkperiodA");
        col.ControlId.Add("chkperiodA");
        col.ColumnName.Add("上午班");
       col.ControlOptionText.Add("");
       col.ControlOptionText.Add("正常班");
       col.ControlOptionText.Add("加班");
       col.ControlOptionText.Add("補班");
       col.ControlOptionText.Add("代班");
       col.ControlOptionValue.Add("");
       col.ControlOptionValue.Add("正常班");
       col.ControlOptionValue.Add("加班");
       col.ControlOptionValue.Add("補班");
       col.ControlOptionValue.Add("代班");
       npoGridView.Columns.Add(col);

       
        //-------------------------------------------------------------------------
       col = new NPOGridViewColumn("下午班");
       col.ColumnType = NPOColumnType.Dropdownlist;
       col.ControlKeyColumn.Add("uid");
       col.ControlName.Add("chkperiodB");
       col.ControlId.Add("chkperiodB");
       col.ColumnName.Add("下午班");
       col.ControlOptionText.Add("");
       col.ControlOptionText.Add("正常班");
       col.ControlOptionText.Add("加班");
       col.ControlOptionText.Add("補班");
       col.ControlOptionText.Add("代班");
       col.ControlOptionValue.Add("");
       col.ControlOptionValue.Add("正常班");
       col.ControlOptionValue.Add("加班");
       col.ControlOptionValue.Add("補班");
       col.ControlOptionValue.Add("代班");
       npoGridView.Columns.Add(col);
        //-------------------------------------------------------------------------
        //-------------------------------------------------------------------------
       col = new NPOGridViewColumn("晚上班");
       col.ColumnType = NPOColumnType.Dropdownlist;
       col.ControlKeyColumn.Add("uid");
       col.ControlName.Add("chkperiodC");
       col.ControlId.Add("chkperiodC");
       col.ColumnName.Add("晚上班");
       col.ControlOptionText.Add("");
       col.ControlOptionText.Add("正常班");
       col.ControlOptionText.Add("加班");
       col.ControlOptionText.Add("補班");
       col.ControlOptionText.Add("代班");
       col.ControlOptionValue.Add("");
       col.ControlOptionValue.Add("正常班");
       col.ControlOptionValue.Add("加班");
       col.ControlOptionValue.Add("補班");
       col.ControlOptionValue.Add("代班");
       npoGridView.Columns.Add(col);
        //-------------------------------------------------------------------------
        lblGridList.Text = npoGridView.Render();
    }

    //------------------------------------------------------------------
    public DataTable CreateTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("uid"));
        dt.Columns.Add(new DataColumn("姓名"));
        dt.Columns.Add(new DataColumn("日期"));
        dt.Columns.Add(new DataColumn("星期"));
        dt.Columns.Add(new DataColumn("上午班"));
        dt.Columns.Add(new DataColumn("下午班"));
        dt.Columns.Add(new DataColumn("晚上班"));

        DataRow dr;

        int Year = Util.String2Number(Util.GetControlValue("ddlYear"));
        int Month = Util.String2Number(Util.GetControlValue("ddlMonth"));
        int numberOfDays = DateTime.DaysInMonth(Year, Month);
        
        for (int i = 1; i <= numberOfDays; i++)
        {
            DateTime d = new DateTime(Year, Month, i);
            dr = dt.NewRow();
            dr["uid"] = i.ToString();
            dr["姓名"] = "姓名";
            dr["日期"] = Util.DateTime2String(d, DateType.yyyyMMdd, EmptyType.ReturnNull);
            string WeekDay = Util.GetWeekString(d, WeekType.Long);
            dr["星期"] = WeekDay;
            //if (WeekDay == "星期六" || WeekDay == "星期日")
            //{
            //    //dr["星期"] = "<span class='necessary'>" + WeekDay + "</span>";
            //    dr["星期"] = WeekDay;
            //    dr["上午班"] = "-1";
            //    dr["下午班"] = "-1";
            //    dr["晚上班"] = "-1";
            //}
            //else
            //{
            //    dr["星期"] = WeekDay;
            //    dr["上午班"] = "0";
            //    dr["下午班"] = "0";
            //    dr["晚上班"] = "0";
            //}
            dt.Rows.Add(dr);
        }
        return dt;
    }
    //------------------------------------------------------------------
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        List<ColumnData> list = new List<ColumnData>();
        int[] InsertFlag = new int[32];

        DeleteScheduleData();

        foreach (string str in Request.Form.Keys)
        {
            string SchDate = "";
            string periodA = "0";
            string periodB = "0";
            string periodC = "0";
            string DayWeek = "0";
            if (str.StartsWith("chkperiodA_") || str.StartsWith("chkperiodB_"))
            {
                string[] strArray = str.Split('_');
                string uid = strArray[1];
                //因 checkbox 欄位有兩個,為避免重複新增
                if (InsertFlag[Util.String2Number(uid)] == 1)
                {
                    continue;
                }
                SchDate = Util.GetControlValue("txtScheduleDate_" + uid);
                periodA = Util.GetControlValue("chkperiodA_" + uid);
                periodB = Util.GetControlValue("chkperiodB_" + uid);
                periodC = Util.GetControlValue("chkperiodC_" + uid); //Nash
                DayWeek = Util.GetControlValue("txtDayWeek_" + uid); //Nash
                DayWeek = GetDayOfWeek(DayWeek);
                InsertData(uid, SchDate, periodA, periodB, periodC, DayWeek);
                InsertFlag[Util.String2Number(uid)] = 1;
            }
        }
        LoadFormData();
        ShowSysMsg("資料儲存完成!");
    }
    //---------------------------------------------------------------------------
    private void DeleteScheduleData()
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        string strSql = @"
                          delete from ScheduleDetail
                          where convert(char(7), SchDate, 111)=@SchDate
                          and UserID=@UserID
                         ";
        dict.Add("UserID", ddlVoluntee.SelectedValue);
        dict.Add("SchDate", GetYearMonth());
        NpoDB.ExecuteSQLS(strSql, dict);
    }
    //---------------------------------------------------------------------------
    private void InsertData(string uid, string ScheduleDate, string ScheduleAM, string SchedulePM, string ScheduleNM,string DayOfWeek)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        List<ColumnData> list = new List<ColumnData>();
        list.Add(new ColumnData("uid", uid, false, false, true));
        //值班日期 
        list.Add(new ColumnData("UserID", ddlVoluntee.SelectedValue, true, true, false));
        list.Add(new ColumnData("SchDate", ScheduleDate, true, true, false));
        list.Add(new ColumnData("periodA", ScheduleAM, true, true, false));
        list.Add(new ColumnData("periodB", SchedulePM, true, true, false));
        list.Add(new ColumnData("periodC", ScheduleNM, true, true, false));
        string strSql = Util.CreateInsertCommand("ScheduleDetail", list, dict);
        NpoDB.ExecuteSQLS(strSql, dict);
    }
    //---------------------------------------------------------------------------
    private void SetColumnData(List<ColumnData> list, string uid)
    {
    }
    //---------------------------------------------------------------------------
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (ddlVoluntee.SelectedValue == "")
        {
            ShowSysMsg("請先選擇志工!");
            return;
        }
        btnAdd.Visible = true;
        LoadFormData();
    }
    //---------------------------------------------------------------------------
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        btnUpdate.Enabled = GetQuarterDateCount("", ""); //該季若個人班表裡沒有資料就不能作編輯(是只該日期區間有沒有資料只要日期區間不用UserID)
        btnAdd.Visible = false;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        string strSql = @"
                          select convert(char(10), SchDate, 111) as SchDate, 
                          periodA,periodB,periodC ,UserName,
                          Case when periodA <> '' and periodA is not null then UserName +'('+ periodA +')' end as 上午班,
                          Case when periodB <> '' and periodB is not null then UserName +'('+ periodB +')' end as 下午班,
                          Case when periodC <> '' and periodC is not null then UserName +'('+ periodC +')' end as 晚上班
                          from ScheduleDetail s
                          inner join AdminUser u on s.UserID=u.UserID
                          where 1=1
                         ";
        strSql += "and CONVERT(char(7), SchDate, 111)=@SchDate\n";
        if (ddlVoluntee.SelectedValue != "")
        {
            strSql += "and s.UserID=@UserID\n";
        }
        strSql += "order by SchDate\n";
        dict.Add("UserID", ddlVoluntee.SelectedValue);
        dict.Add("SchDate", GetYearMonth());
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);

        Dictionary<string, ScheduleUser> UserList = new Dictionary<string, ScheduleUser>();
        foreach (DataRow dr in dt.Rows)
        {
            string ScheduleDate = dr["SchDate"].ToString();
            ScheduleUser User;
            if (UserList.ContainsKey(ScheduleDate))
            {
                User = UserList[ScheduleDate];
            }
            else
            {
                User = new ScheduleUser();
                UserList.Add(ScheduleDate, User);
            }
            if (dr["上午班"].ToString() != "")
            {
                User.ScheduleAM += dr["上午班"].ToString() + ",";
            }
            if (dr["下午班"].ToString() != "")
            {
                User.SchedulePM += dr["下午班"].ToString() + ",";
            }
            if (dr["晚上班"].ToString() != "")
            {
                User.ScheduleNM += dr["晚上班"].ToString() + ",";
            }
        }
        dt = CreateQueryTable(UserList);
        //需自訂欄位時
        NPOGridView npoGridView = new NPOGridView();
        npoGridView.Source = NPOGridViewDataSource.fromDataTable;
        npoGridView.dataTable = dt;
        npoGridView.Keys.Add("uid");
        npoGridView.DisableColumn.Add("uid");
        npoGridView.ShowPage = false;
        //-----------------------------------------------------------------------
        //欄位為 checkbox
        NPOGridViewColumn col = new NPOGridViewColumn("日期");
        col.ColumnType = NPOColumnType.NormalText;
        col.ColumnName.Add("日期");
        npoGridView.Columns.Add(col);
        //-------------------------------------------------------------------------
        col = new NPOGridViewColumn("星期");
        col.ColumnType = NPOColumnType.NormalText;
        col.ColumnName.Add("星期");
        npoGridView.Columns.Add(col);
        //-------------------------------------------------------------------------
        col = new NPOGridViewColumn("上午班");
        col.ColumnType = NPOColumnType.NormalText;
        col.ColumnName.Add("上午班");
        npoGridView.Columns.Add(col);
        //-------------------------------------------------------------------------
        col = new NPOGridViewColumn("下午班");
        col.ColumnType = NPOColumnType.NormalText;
        col.ColumnName.Add("下午班");
        npoGridView.Columns.Add(col);
        //-------------------------------------------------------------------------
        col = new NPOGridViewColumn("晚上班");
        col.ColumnType = NPOColumnType.NormalText;
        col.ColumnName.Add("晚上班");
        npoGridView.Columns.Add(col);
        //-------------------------------------------------------------------------
        lblGridList.Text = npoGridView.Render();
    }
    //---------------------------------------------------------------------------
    public DataTable CreateQueryTable(Dictionary<string, ScheduleUser> UserList)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("uid"));
        dt.Columns.Add(new DataColumn("日期"));
        dt.Columns.Add(new DataColumn("星期"));
        dt.Columns.Add(new DataColumn("上午班"));
        dt.Columns.Add(new DataColumn("下午班"));
        dt.Columns.Add(new DataColumn("晚上班"));

        DataRow dr;

        int Year = Util.String2Number(Util.GetControlValue("ddlYear"));
        int Month = Util.String2Number(Util.GetControlValue("ddlMonth"));
        int numberOfDays = DateTime.DaysInMonth(Year, Month);

        for (int i = 1; i <= numberOfDays; i++)
        {
            DateTime d = new DateTime(Year, Month, i);
            string ScheduleDate = Util.DateTime2String(d, DateType.yyyyMMdd, EmptyType.ReturnNull);
            dr = dt.NewRow();
            dr["uid"] = i.ToString();
            dr["日期"] = Util.DateTime2String(d, DateType.yyyyMMdd, EmptyType.ReturnNull);
            string WeekDay = Util.GetWeekString(d, WeekType.Long);
            dr["星期"] = WeekDay;
            //if (WeekDay == "星期六" || WeekDay == "星期日")
            //{
            //    dr["星期"] = "<span class='necessary'>" + WeekDay + "</span>";
            //}
            //else
            //{
            //    dr["星期"] = WeekDay;
            //}
            if (UserList.ContainsKey(ScheduleDate))
            {
                dr["上午班"] = Util.TrimLastChar(UserList[ScheduleDate].ScheduleAM, ',');
                dr["下午班"] = Util.TrimLastChar(UserList[ScheduleDate].SchedulePM, ',');
                dr["晚上班"] = Util.TrimLastChar(UserList[ScheduleDate].ScheduleNM, ',');
            }
            dt.Rows.Add(dr);
        }
        return dt;
    }
    //------------------------------------------------------------------

    public string GetDayOfWeek(string strDayOfWeek)
    {
        switch (strDayOfWeek)
        {
            case "星期一":
                return "Mon";
                break;
            case "星期二":
                return "Tue";
                break;
            case "星期三":
                return "Wed";
                break;
            case "星期四":
                return "Thu";
                break;
            case "星期五":
                return "Fri";
                break;
            case "星期六":
                return "Sat";
                break;
            case "星期日":
                return "Sun";
                break;
            default :
                return "";
                break;
        }
        return "";
    }

    //傳入日期查詢判斷第幾季  查詢 個人班表裡有沒有資料
    public bool  GetQuarterDateCount(string strYear, string strQuarter)
    {
        string icount = "";
        int Year = Util.String2Number(Util.GetControlValue("ddlYear"));
        int Month = Util.String2Number(Util.GetControlValue("ddlMonth"));
       
        switch (Month)
        {
            case 1:
            case 2:
            case 3:
                strQuarter ="1";
                break;
            case 4:
            case 5:
            case 6:
                strQuarter="2";
                break;
            case 7:
            case 8:
            case 9:
                strQuarter="3";
                break;
            case 10:
            case 11:
            case 12:
                strQuarter="4";
                break ;
        }
        switch (strQuarter)
        {
            case "1":
                icount = objNpoDB.QueryGetScalar("select Count(*) As Cnt from scheduledetail where schdate  between '" + Year + "/01/01' and '" + Year + "/03/31' ");
                break;
            case "2":
                icount= objNpoDB.QueryGetScalar("select Count(*) As Cnt from scheduledetail where schdate  between '" + Year + "/04/01' and '" + Year + "/06/30' ");
                break;
            case "3":
                icount= objNpoDB.QueryGetScalar("select Count(*) As Cnt from scheduledetail where schdate  between '" + Year + "/07/01' and '" + Year + "/09/30' ");
                break;
            case "4":
                icount= objNpoDB.QueryGetScalar("select Count(*) As Cnt from scheduledetail where schdate  between '" + Year + "/10/01' and '" + Year + "/12/31' ");
                break;
        }
        if (icount != "0")
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }


}

public class ScheduleUser
{
    public string ScheduleAM;
    public string SchedulePM;
    public string ScheduleNM;
}