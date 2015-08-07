using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Collections;
using System.Web.UI.HtmlControls;

public partial class ScheduleMgr_selectVolunteer : BasePage 
{
    GroupAuthrity ga = null;

    protected void Page_Load(object sender, EventArgs e)
    {
         Session["ProgID"] = "MoveMgr";
        //權控處理
        AuthrityControl();

        //取得模式, 新增("ADD")或修改("")
        //HFD_Mode.Value = Util.GetQueryString("Mode");

        ShowSysMsg();
     
        if (!IsPostBack)
        {
            HFD_Uid.Value = Util.GetQueryString("Uid");
            HFD_lblName.Value  = Util.GetQueryString("lblName"); //排班表 Schedule.aspx 的 label。 放的是志工名字
            HFD_ScheduleID.Value = Util.GetQueryString("ScheduleID"); // table Schedule 的 Uid
            HFD_Year.Value = Util.GetQueryString("Year").ToString();
            HFD_Querter.Value = Util.GetQueryString("Querter");   //第幾季
            HFD_DayOfWeek.Value = Util.GetQueryString("DayOfWeek");//星期
            HFD_periodID.Value = Util.GetQueryString("periodID");  //班別
            HFD_ScheduleID.Value = GetScheduleID();  //   撈出  table Schedule 的 Uid
            LoadScheduleUser();  // 撈出 在班表上該班別的 的志工
            LoadUser();       // 撈出 尚未排在該班別的志工UserID   也就是 抓AdmimUser 的 Table
         }

        SetButton();
    }
    //----------------------------------------------------------------------
    private void SetButton()
    {
    }
    //-------------------------------------------------------------------------
    public void AuthrityControl()
    {
    }
    //----------------------------------------------------------------------
    public void LoadDropDownListData()
    {
     
    }
    //----------------------------------------------------------------------
      
    public void LoadFormData()
    {
    }
    //----------------------------------------------------------------------
    //加入 的Button
   //從AdminUser 加入  班表UserID
    protected void btnSelect_Click(object sender, EventArgs e)
    {
        foreach (ListItem item in lstUser.Items)
        {
            if (item.Selected == true)
            {
                AddUser(item.Value);
            }
        }
        LoadScheduleUser();
        LoadUser();
    }
    //----------------------------------------------------------------------
    private void AddUser(string UserID)
    {
        string strSql = "Insert into  ScheduleMap";
        strSql += "( ScheduleID, UserID ) values";
        strSql += "(@ScheduleID,@UserID);";
        strSql += "\n";
        strSql += "select @@IDENTITY";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("ScheduleID", HFD_ScheduleID.Value);
        dict.Add("UserID", UserID );
        HFD_Uid.Value = NpoDB.GetScalarS(strSql, dict);
     }
    //----------------------------------------------------------------------
    //移出 的 Button 
    protected void btnRemove_Click(object sender, EventArgs e)
    {
        foreach (ListItem item in lstScheduleUser.Items)
        {
            if (item.Selected == true)
            {
                RemoveUser(item.Value);
            }
        }
       
        LoadScheduleUser();
        LoadUser();
        
    }
    //----------------------------------------------------------------------
    private void RemoveUser(string UserID)
    {
        string strSql = @"
                        Delete From ScheduleMap 
                        Where UserID =@UserID And ScheduleID =@ScheduleID
                        ";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("UserID", UserID);
        dict.Add("ScheduleID", HFD_ScheduleID.Value);
        NpoDB.ExecuteSQLS(strSql, dict);
    }
    //----------------------------------------------------------------------
    protected void btnSearch_Click(object sender, EventArgs e)
    {
    
    }
    //----------------------------------------------------------------------
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string strAssetNo = lstScheduleUser.Text;
        for (int i = 0; i <= lstScheduleUser.Items.Count - 1; i++)
        {
            strAssetNo = strAssetNo + lstScheduleUser.Items[i].Text + "<BR>";
        }
        Response.Write(@"<script>self.opener.document.getElementById('" + HFD_lblName.Value + "').innerHTML  ='" + strAssetNo + "'"
           + ";window.close();</script>");
    }

     //取得排班表上的志工
    private void   LoadScheduleUser()
    {
        DataRow dr = null;
        DataTable dt = null;
            string strSql = @"
                            Select b.UserID As UserID,b.uid As uid ,
                            UserName = (select UserName from  AdminUser Where UserID = b.UserID )
                            From Schedule a 
                            inner join ScheduleMap b on a.uid = b.ScheduleID 
                            Where   1=1
                            And ScheduleID=@ScheduleID
                            And isnull(a.IsDelete, '') != 'Y'
                        ";
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("ScheduleID", HFD_ScheduleID.Value );
            dt = NpoDB.GetDataTableS(strSql, dict);
            Util.FillListBox(lstScheduleUser, dt, "UserName", "UserID", false);
       }

    //取得志工資料
    private void LoadUser()
    {
        DataTable dt = null;
        string strSql = @"
                            select * from AdminUser 
                            Where UserID  Not in (select UserID From ScheduleMap where ScheduleID =@ScheduleID)
                        ";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("ScheduleID",HFD_ScheduleID.Value );
        dt = NpoDB.GetDataTableS(strSql, dict);
        Util.FillListBox(lstUser, dt, "UserName", "UserID", false);
    }

    //取得ScheduleID
    public string GetScheduleID()
    {
        DataRow dr = null;
        DataTable dt = null;
        string strSql = @"
                            Select * From Schedule
                            Where   1=1
                            And SchYear=@SchYear
                            And Quarter=@Quarter  
                            And DayOfWeek=@DayOfWeek
                            And periodID=@periodID
                            And isnull(IsDelete, '') != 'Y'
                        ";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("SchYear", HFD_Year.Value);
        dict.Add("Quarter", HFD_Querter.Value);
        dict.Add("DayOfWeek", HFD_DayOfWeek.Value);
        dict.Add("periodID", HFD_periodID.Value);
        dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count == 0)
        {
            string NewUID=InsertScheduleID();
            return NewUID;
        }
        dr = dt.Rows[0];
        return dr["uid"].ToString();
     }


    //點編輯早午晚班沒有志工資料 就先塞入一筆 table Schedule 才能 join 到 table scheduleMap 才能新增志工資料
    private string InsertScheduleID()
    {
        string strSql = "Insert into Schedule";
        strSql += "(SchYear,Quarter,DayOfWeek,periodID) values";
        strSql += "(@SchYear,@Quarter,@DayOfWeek,@periodID);";
        strSql += "\n";
        strSql += "select @@IDENTITY";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("SchYear", HFD_Year.Value);
        dict.Add("Quarter", HFD_Querter.Value);
        dict.Add("DayOfWeek", HFD_DayOfWeek.Value);
        dict.Add("periodID", HFD_periodID.Value);
        string NewUID = objNpoDB.QueryGetScalar(strSql, dict);
        return NewUID;
      
    }

}
