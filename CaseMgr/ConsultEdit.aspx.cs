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

public partial class CaseMgr_ConsultEdit : BasePage
{
    GroupAuthrity ga = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        
        Session["ProgID"] = "Consult";
        //權限控制
        AuthrityControl();

        SetButton();

        ShowSysMsg();
                                                    
        if (!IsPostBack)
        {
            if (Util.GetQueryString("InBound") == "N")
            {
                return;
            }
            HFD_MemberID.Value = Util.GetQueryString("UID"); //會員UID
            HFD_ConsultingUID.Value = Util.GetQueryString("ConsultingUID"); //諮詢紀錄UID
            HFD_Phone.Value = Util.GetQueryString("phone");   //來電號碼
            HFD_Create.Value = Util.GetQueryString("Create");  //從查詢鈕傳來的值 若為空白代表要直接新增一筆新的個資
            txtServiceUser.Text = SessionInfo.UserID.ToString();
            if (HFD_Phone.Value != "0" && HFD_Phone.Value != "unknown" && HFD_Phone.Value != "")  //若是有孝電話號碼的話就將電話號碼欄位鎖住
            {
                //txtPhone.Enabled = false;
            }
            if (HFD_Phone.Value == "0")   //若來電顯示為無號碼 則 個資uid 為空白
            {
                HFD_MemberID.Value = "";
            }
            if (HFD_MemberID.Value == "" && HFD_ConsultingUID.Value == "" && HFD_UID.Value == "" || txtCName.Text == "第一次來電")
            {
                txtCName.Enabled = true;
            }
            if (txtCreateDate.Text == "") //開始時間
            {
                txtCreateDate.Text = Util.GetToday(DateType.yyyyMMdd);
            }
            if (txtCreateTime.Text == "")
            {
                txtCreateTime.Text = Util.GetToday(DateType.HHmmss);
            }
            if (HFD_Phone.Value != "")
            {
                GetPhoneToUid();
            }
            //載入下拉式選單資料
            LoadDropDownListData();
            //載入資料
            if (HFD_Phone.Value != "0")
            {
                LoadFormData();
            }
            else
            {
                txtPhone.Text = HFD_Phone.Value;
                //btnQry_Click(btnQry, null);
            }
            if ((HFD_Phone.Value != "" && HFD_MemberID.Value == "") || ( HFD_Phone.Value == "0"))
            {
                txtCName.Enabled = true;
               // txtCName.Text = "第一次來電";
                
            }
            else
            {
                lblSta.Visible = false;
            }


            if (txtCName.Text == "" || txtCName.Text == "第一次來電")
            {
                txtCName.Enabled = true;
            }
       
       if (txtCName.Text != "" || txtPhone.Text != "")
        {
            btnUpdate.Enabled = true;
            Button1.Enabled = true;
        }
        }
        if (txtCName.Text != "" || txtPhone.Text != "")
        {
            btnUpdate.Enabled = true;
            Button1.Enabled = true;
        }
            LoadHiddenData();
     }

    private void SetupDefaultValue()
    {
    }
    //-------------------------------------------------------------------------
    private void SetButton()
    {
        txtServiceUser.Enabled = (SessionInfo.GroupName == "系統管理員") ? true : false;
        txtCName.Enabled = (SessionInfo.GroupName == "系統管理員") ? true : false;
        btnDelete.Visible = (SessionInfo.GroupName == "系統管理員") ? true : false;
        if (HFD_MemberID.Value != "")
        {
            txtCName.Enabled = true;
        }
        if (HFD_Mode.Value == "ADD")
        {
            //新增時, 修改及刪除鍵沒動作
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
    //-------------------------------------------------------------------------
    public void LoadDropDownListData()
    {
        Util.FillCityData(ddlCity);
        Util.FillAreaData(ddlArea, ddlCity.SelectedValue);
        Util.FillDropDownList(ddlstrHour, 0, 23, false, 1, true, 2, '0');
        Util.FillDropDownList(ddlstrMin, 0, 59, false, 1, true, 2, '0');
        ddlstrHour.SelectedValue = DateTime.Now.Hour.ToString();
        ddlstrMin.SelectedValue = DateTime.Now.Minute.ToString();
        Util.FillDropDownList(ddlEndHour, 0, 23, true, -1, true, 2, '0');
        Util.FillDropDownList(ddlEndMin, 0, 59, true, -1, true, 2, '0');
    }
    //-------------------------------------------------------------------------------------------------------------
   private void LoadFormData()
    {
        string strSql;
        DataRow dr = null;
        DataTable dt = null;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        //Response.Write("MemberUID2-->" + HFD_MemberID.Value + "<BR>");
        if (HFD_Mode.Value != "ADD")
        {
            strSql = @"
                   select *
                   from Member
                   where uid=@uid
                   and isnull(IsDelete, '') != 'Y'
                  ";
            dict.Add("uid", HFD_MemberID.Value);
            dt = NpoDB.GetDataTableS(strSql, dict);
            //資料異常
            if (dt.Rows.Count == 0)
            {
                txtPhone.Text = HFD_Phone.Value;
                return;
            }
            dr = dt.Rows[0];
        }
        lblRpt.Text = GetRptHead() + GetReport();   //報表資料
        //電話
        txtPhone.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Phone"].ToString();
        //第一次
        //chkFirst.Checked = Boolean.Parse((HFD_Mode.Value == "ADD") ? "false" : dr["First"].ToString()); 
        //年齡
        txtAge.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Age"].ToString();
        ddlAge.SelectedValue = (HFD_Mode.Value == "ADD") ? "" : dr["Age"].ToString();
        //轉介教會
        rdoChurchYN.SelectedValue = (HFD_Mode.Value == "ADD") ? "" : dr["ChurchYN"].ToString();
        //姓名
        txtCName.Text = (HFD_Mode.Value == "ADD") ? "" : dr["CName"].ToString();
        //婚姻
        //rdoMarry.SelectedValue = (HFD_Mode.Value == "ADD") ? "" : dr["Marry"].ToString();
        HFD_Marry.Value = (HFD_Mode.Value == "ADD") ? "" : dr["Marry"].ToString();
        //性別
        rdoSex.SelectedValue = (HFD_Mode.Value == "ADD") ? "" : dr["sex"].ToString();
        //基督徒
        rdoChristianYN.SelectedValue = (HFD_Mode.Value == "ADD") ? "" : dr["Christian"].ToString();
        //郵遞區號(H)	ZipCode
        txtZipCode.Text = (HFD_Mode.Value == "ADD") ? "" : dr["ZipCode"].ToString();
        //縣市(H)	City
        Util.SetDdlIndex(ddlCity, (HFD_Mode.Value == "ADD") ? "" : dr["City"].ToString());
        //鄉鎮市區(H)	Area
        Util.FillAreaData(ddlArea, ddlCity.SelectedValue);
        Util.SetDdlIndex(ddlArea, (HFD_Mode.Value == "ADD") ? "" : dr["Area"].ToString());
        HFD_Area.Value = (HFD_Mode.Value == "ADD") ? "" : dr["Area"].ToString();
        //地址(H)	Address
        txtAddress.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Address"].ToString();
        //海外地區
        HFD_Overseas.Value = (HFD_Mode.Value == "ADD") ? "" : dr["Overseas"].ToString();
        //檔案名稱
        //txtFileName.Text = (HFD_Mode.Value == "ADD") ? "" : dr["FileName"].ToString();
        //備註
        txtMemberMemo.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Memo"].ToString();
        if (txtCreateDate.Text == "")
        {
            txtCreateDate.Text = Util.GetToday(DateType.yyyyMMddHHmmss);
        }
        //*******************************************************************************************
        //----諮詢內容
        if (HFD_ConsultingUID.Value != "")
        {
            LoadConsulting();
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        List<ColumnData> list = new List<ColumnData>();
        string strSql = "";
        string NewUID = "";
        SetColumnData(list);
        chkField();
        if (HFD_UID.Value != "")      //若有UID 表示有會友資料
        {
            strSql = Util.CreateUpdateCommand("Member", list, dict);
            NpoDB.ExecuteSQLS(strSql, dict);
        }
        else  //若沒有UID 表示沒有會友資料 要Insert
        {
            strSql = Util.CreateInsertCommand("Member", list, dict);
            strSql += "select @@IDENTITY";
            HFD_UID.Value = NpoDB.GetScalarS(strSql, dict);
        }
        List<ColumnData> list2 = new List<ColumnData>();
        SetConsultingColumnData(list2);
        strSql = Util.CreateInsertCommand("Consulting", list2, dict);
        strSql += "select @@IDENTITY";
        NewUID = NpoDB.GetScalarS(strSql, dict);

        SetSysMsg("新增資料成功");
        Response.Redirect(Util.RedirectByTime("ConsultEdit.aspx"));
    }
    //-------------------------------------------------------------------------------------------------------------
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        List<ColumnData> list = new List<ColumnData>();
        string strSql = "";
        string strChkField = "";
        strChkField = chkField();//欄位檢查

        if (HFD_Create.Value == "")    // 若為空白代表要直接新增一筆新的個資
        {
            if (HFD_MemberID.Value == "")
            {
                if (IsHaveName(txtCName.Text))
                {
                    txtCName.Enabled = true;
                    Response.Write("<script language='javascript'>window.open('AdvisoryDataQry.aspx?Name=" + txtCName.Text + "&MOD=Qry', 'window','HEIGHT=600,WIDTH=820,top=50,left=50,toolbar=yes,scrollbars=yes,resizable=yes');</script>");
                    return;
                }
            }
        }

       
        if (strChkField != "")
        {
         //   ShowSysMsg(strChkField);
        }
        CreateConsultingData();
        SetColumnData(list);
        //Response.Write("MemberUID3-->" + HFD_MemberID.Value + "<BR>");
        if (HFD_MemberID.Value != "")      //若有UID 表示有會友資料
        {
            strSql = Util.CreateUpdateCommand("Member", list, dict);
            NpoDB.ExecuteSQLS(strSql, dict);
        }
        else  //若沒有UID 表示沒有會友資料 要Insert
        {
        
            strSql = Util.CreateInsertCommand("Member", list, dict);
            strSql += "select @@IDENTITY";
            HFD_MemberID.Value = NpoDB.GetScalarS(strSql, dict);
            //Response.Write("MemberUID4-->" + HFD_MemberID.Value + "<BR>");
        }

        List<ColumnData> list2 = new List<ColumnData>();
        SetConsultingColumnData(list2);
        strSql = Util.CreateUpdateCommand("Consulting", list2, dict);
        NpoDB.ExecuteSQLS(strSql, dict);
        string redirectURL = "";
        //Response.Redirect("", "_blank", "menubar=0,scrollbars=1,width=780,height=900,top=10");

        ScriptManager.RegisterStartupScript(this, typeof(string), "New_Window", "window.open( '../SaveOK.htm', null, 'resizable=no,scrollbars=no,height=150,width=250,status=no,toolbar=no,menubar=no,location=no,top=300,left=500px');", true);
        //ShowSysMsg("資料已儲存");
        return;
      
    }
    //-------------------------------------------------------------------------------------------------------------
    //基本資料
    private void SetColumnData(List<ColumnData> list)
    {
        //Response.Write("MemberUID5-->" + HFD_MemberID.Value + "<BR>");
        //第一個欄位給 update 用
        list.Add(new ColumnData("uid", HFD_MemberID.Value, false, false, true));
        //電話
        list.Add(new ColumnData("Phone", txtPhone.Text, true, true, false));
        //第一次
       // list.Add(new ColumnData("First", chkFirst.Checked.ToString(), true, true, false));
        //年齡
        //list.Add(new ColumnData("Age", txtAge.Text, true, true, false));
        list.Add(new ColumnData("Age", ddlAge.SelectedValue , true, true, false));
        //轉介教會
        list.Add(new ColumnData("ChurchYN", rdoChurchYN.SelectedValue , true, true, false));
        //姓名
        list.Add(new ColumnData("CName", txtCName.Text.ToString().Trim(), true, true, false));
        //婚姻
        list.Add(new ColumnData("Marry", HFD_Marry.Value.ToString().TrimEnd(','), true, true, false));
        //性別
        list.Add(new ColumnData("Sex", rdoSex.SelectedValue, true, true, false));
        //基督徒
        list.Add(new ColumnData("Christian", rdoChristianYN.SelectedValue, true, true, false));
        //郵遞區號(H)	ZipCode
        list.Add(new ColumnData("ZipCode", txtZipCode.Text, true, true, false));
        //縣市(H)	City
        list.Add(new ColumnData("City", ddlCity.SelectedValue, true, true, false));
        //鄉鎮市區(H)	Area
        list.Add(new ColumnData("Area", HFD_Area.Value, true, true, false));
        //地址(H)	Address
        list.Add(new ColumnData("Address", txtAddress.Text, true, true, false));
        //海外地區
        list.Add(new ColumnData("Overseas", HFD_Overseas.Value , true, true, false));
        //檔案名稱	FileName
        //list.Add(new ColumnData("FileName", txtFileName.Text, true, true, false));
        //備註
        list.Add(new ColumnData("Memo", txtMemberMemo.Text, true, true, false));
        if (HFD_MemberID.Value == "")
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
        Dictionary<string, object> dict = new Dictionary<string, object>();
        string strSql = @"
                           update Consulting set
                           IsDelete='Y',
                           DeleteID=@DeleteID,
                           DeleteDate=GetDate()
                           where uid = @uid
                         ";
        dict.Add("uid", HFD_ConsultingUID.Value);
        dict.Add("DeleteID", SessionInfo.UserID);
        NpoDB.ExecuteSQLS(strSql, dict);
        Session["Msg"] = "刪除資料成功";
        Response.Redirect(Util.RedirectByTime("ConsultEdit.aspx"));
    }
    //-------------------------------------------------------------------------------------------------------------
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect(Util.RedirectByTime("ConsultEdit.aspx"));
    }
    //-------------------------------------------------------------------------------------------------------------
    //諮詢資料
    private void SetConsultingColumnData(List<ColumnData> list)
    {
        //Response.Write("MemberUID6-->" + HFD_MemberID.Value + "<BR>");
        //第一個欄位給 update 用
        list.Add(new ColumnData("uid", HFD_ConsultingUID.Value, false, false, true));
        //會員編號
        list.Add(new ColumnData("MemberUID", HFD_MemberID.Value, true, true, false));
        //服務志工          
        list.Add(new ColumnData("ServiceUser", txtServiceUser.Text , true, true, false));
        //建檔時間          
        //list.Add(new ColumnData("CreateDate", txtCreateDate.Text + " " + txtCreateTime.Text, true, true, false));
        //if (txtEndTime.Text.Trim() != "")
        //{
        //    list.Add(new ColumnData("EndDate", Util.DateTime2String(txtCreateDate.Text + " " + txtEndTime.Text, DateType.yyyyMMddHHmmss, EmptyType.ReturnNull), true, true, false));
        //}
        //else
        //{
        //    list.Add(new ColumnData("EndDate", Util.DateTime2String(txtEndTime.Text, DateType.yyyyMMddHHmmss, EmptyType.ReturnNull), true, true, false));
        //}
        list.Add(new ColumnData("CreateDate", Util.DateTime2String(txtCreateDate.Text + " " + ddlstrHour.SelectedValue + ":" + ddlstrMin.SelectedValue + ":00", DateType.yyyyMMddHHmmss, EmptyType.ReturnNull), true, true, false));
        list.Add(new ColumnData("EndDate", Util.DateTime2String(txtCreateDate.Text + " " + ddlEndHour.SelectedValue + ":" + ddlEndMin.SelectedValue + ":00", DateType.yyyyMMddHHmmss, EmptyType.ReturnNull), true, true, false));

        //事件
        list.Add(new ColumnData("Event", txtEvent.Text, true, true, false));
        //想法
        list.Add(new ColumnData("think", txtThink.Text, true, true, false));
        //感受
        list.Add(new ColumnData("feel", txtFeel.Text, true, true, false));
        //補充說明
        list.Add(new ColumnData("Comment", txtComment.Text, true, true, false));
        //特殊諮詢
        list.Add(new ColumnData("Special", HFD_Special.Value, true, true, false));
        //騷擾電話
        list.Add(new ColumnData("HarassPhone", HFD_HarassPhone.Value, true, true, false));
        //騷擾電話(其他)
        list.Add(new ColumnData("HarassOther", txtHarassOther.Text, true, true, false));
        if (HFD_HarassPhone.Value == "" && HFD_Special.Value == "")
        {
            //來電諮詢別 (大類)
            list.Add(new ColumnData("consultantMain", HFD_ConsultantMain.Value, true, true, false));
            //來電諮詢別 (分項)
            list.Add(new ColumnData("ConsultantItem", HFD_ConsultantItem.Value, true, true, false));
            //轉介單位
            list.Add(new ColumnData("IntroductionUnit", HFD_IntroductionUnit.Value, true, true, false));
            //緊急轉介
            list.Add(new ColumnData("CrashIntroduction", HFD_CrashIntroduction.Value, true, true, false));
        }
        //原因1
        //list.Add(new ColumnData("Reason1", txtReason1.Text, true, true, false));
        //原因2
        //list.Add(new ColumnData("Reason2", txtReason2.Text, true, true, false));
        //原因3
        //list.Add(new ColumnData("Reason3", txtReason3.Text, true, true, false));
        //問題1
        //list.Add(new ColumnData("Trouble1", txtTrouble1.Text, true, true, false));
        //問題2
        //list.Add(new ColumnData("Trouble2", txtTrouble2.Text, true, true, false));
        //問題3
        //list.Add(new ColumnData("Trouble3", txtTrouble3.Text, true, true, false));
        //因...造成問題
        list.Add(new ColumnData("Problem", txtProblem.Text, true, true, false));
        //找出案主心中的假設
        list.Add(new ColumnData("FindAssume", txtFindAssume.Text, true, true, false));
        //與案主討論解釋與假設
        list.Add(new ColumnData("Discuss", txtDiscuss.Text, true, true, false));
        //加入新的元素
        list.Add(new ColumnData("Element", txtElement.Text, true, true, false));
        //改變案主原先的期待
        list.Add(new ColumnData("Expect", txtExpect.Text, true, true, false));
        //給予案主祝福與盼望
        list.Add(new ColumnData("Blessing", txtBlessing.Text, true, true, false));
        //轉介教會
        list.Add(new ColumnData("IntroductionChurch", txtIntroductionChurch.Text, true, true, false));
        //我對個案的幫助程度
        list.Add(new ColumnData("HelpLvMark", rdoHelpLvMark.SelectedValue, true, true, false));
        //小叮嚀          
        list.Add(new ColumnData("Memo", txtMemo.Text , true, true, false));
        //新增日期
        list.Add(new ColumnData("InsertDate", Util.GetToday(DateType.yyyyMMddHHmmss), true, true, false));
        list.Add(new ColumnData("InsertID", SessionInfo.UserID, true, true, false));
     }

    protected void btnUpdateConsulting_Click(object sender, EventArgs e)
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
        //sb.AppendLine(s);
       // Util.OutputTxt(sb + lblRpt.Text, "1", DateTime.Now.ToString("yyyyMMddHHmmss"));
    }

    //諮商類別
    private void LoadHiddenData()
    {
        List<ControlData> list = new List<ControlData>();
        list = new List<ControlData>();
        //婚姻狀況
        list.Add(new ControlData("Checkbox", "Marry", "CHK_Marry1", "未婚", "未婚", false, HFD_Marry.Value));
        list.Add(new ControlData("Checkbox", "Marry", "CHK_Marry2", "已婚", "已婚", false, HFD_Marry.Value));
        list.Add(new ControlData("Checkbox", "Marry", "CHK_Marry3", "離婚", "離婚", false, HFD_Marry.Value));
        list.Add(new ControlData("Checkbox", "Marry", "CHK_Marry4", "喪偶", "喪偶", false, HFD_Marry.Value));
        lblMarry.Text = HtmlUtil.RenderControl(list);
        //海外地區
        list = new List<ControlData>();
        list.Add(new ControlData("Checkbox", "Overseas", "CHK_Overseas1", "海外地區", "海外地區", false, HFD_Overseas.Value));
        lblOverseas.Text = HtmlUtil.RenderControl(list);
        //來電諮詢別(大類) 
        list = new List<ControlData>();
        list.Add(new ControlData("Checkbox", "ConsultantMain","CHK_ConsultantMain1", "婚前", "婚前", false, HFD_ConsultantMain.Value ));
        list.Add(new ControlData("Checkbox", "ConsultantMain","CHK_ConsultantMain2", "婚姻", "婚姻", false, HFD_ConsultantMain.Value));
        list.Add(new ControlData("Checkbox", "ConsultantMain","CHK_ConsultantMain3", "單親", "單親", false, HFD_ConsultantMain.Value));
        list.Add(new ControlData("Checkbox", "ConsultantMain","CHK_ConsultantMain4", "親子", "親子", false, HFD_ConsultantMain.Value));
        list.Add(new ControlData("Checkbox", "ConsultantMain","CHK_ConsultantMain5", "重組家庭", "重組家庭", false, HFD_ConsultantMain.Value));
        list.Add(new ControlData("Checkbox", "ConsultantMain","CHK_ConsultantMain6", "遠距家庭", "遠距家庭", false, HFD_ConsultantMain.Value));
        list.Add(new ControlData("Checkbox", "ConsultantMain","CHK_ConsultantMain7", "隔代家庭", "隔代家庭", false, HFD_ConsultantMain.Value));
        list.Add(new ControlData("Checkbox", "ConsultantMain","CHK_ConsultantMain8", "通婚家庭", "通婚家庭", false, HFD_ConsultantMain.Value));
        list.Add(new ControlData("Checkbox", "ConsultantMain", "CHK_ConsultantMain9", "受難家庭", "受難家庭", false, HFD_ConsultantMain.Value));
        list.Add(new ControlData("Checkbox", "ConsultantMain","CHK_ConsultantMain10", "中年問題", "中年問題", false, HFD_ConsultantMain.Value));
        list.Add(new ControlData("Checkbox", "ConsultantMain","CHK_ConsultantMain11", "老年問題", "老年問題", false, HFD_ConsultantMain.Value));
        lblConsultantMain.Text = HtmlUtil.RenderControl(list);
        //=================================================================================================================
        //來電諮詢別(分項) 
        list = new List<ControlData>();
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem1", "經濟", "經濟", false, HFD_ConsultantItem.Value ));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem2", "情緒", "情緒", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem3", "信仰", "信仰", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem4", "人際", "人際", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem5", "婆媳", "婆媳", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem6", "翁媳", "翁媳", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem7", "妯娌", "妯娌", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem8", "手足", "手足", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem9", "職場", "職場", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem10", "教會", "教會", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem11", "溝通", "溝通", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem12", "性生活", "性生活", true, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem13", "精神疾病", "精神疾病", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem14", "身體疾病", "身體疾病", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem15", "個性", "個性", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem16", "外遇", "外遇", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem17", "暴力", "暴力", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem18", "教育", "教育", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem19", "霸凌", "霸凌", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem20", "學業", "學業", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem21", "癮症", "癮症", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem22", "性別問題", "性別問題", false, HFD_ConsultantItem.Value));
        lblConsultantItem.Text = HtmlUtil.RenderControl(list);
        //=======================================================================================================
        list = new List<ControlData>();
        //騷擾電話 
        list.Add(new ControlData("Checkbox", "HarassPhone", "CHK_HarassPhone1", "猥褻", "猥褻", false, HFD_HarassPhone.Value));
        list.Add(new ControlData("Checkbox", "HarassPhone", "CHK_HarassPhone2", "攻擊", "攻擊", false, HFD_HarassPhone.Value));
        list.Add(new ControlData("Checkbox", "HarassPhone", "CHK_HarassPhone3", "沉默", "沉默", false, HFD_HarassPhone.Value));
        list.Add(new ControlData("Checkbox", "HarassPhone", "CHK_HarassPhone4", "精神錯亂", "精神錯亂", false, HFD_HarassPhone.Value));
        list.Add(new ControlData("Checkbox", "HarassPhone", "CHK_HarassPhone5", "其他", "其他", false, HFD_HarassPhone.Value));
        lblHarassPhone.Text = HtmlUtil.RenderControl(list);
        //=========================================================================================================
        list = new List<ControlData>();
        //特殊諮詢
        list.Add(new ControlData("Checkbox", "Special", "CHK_Special1", "感恩祝福", "感恩祝福", false, HFD_Special.Value));
        list.Add(new ControlData("Checkbox", "Special", "CHK_Special2", "宗教問題", "宗教問題", false, HFD_Special.Value));
        list.Add(new ControlData("Checkbox", "Special", "CHK_Special2", "代禱", "代禱", false, HFD_Special.Value));
        lblSpecial.Text = HtmlUtil.RenderControl(list);
        //=======================================================================================================
        list = new List<ControlData>();
        //轉介單位(告知電話) 
        list.Add(new ControlData("Checkbox", "IntroductionUnit", "CHK_IntroductionUnit1", "諮商協談", "諮商協談", false, HFD_IntroductionUnit.Value));
        list.Add(new ControlData("Checkbox", "IntroductionUnit", "CHK_IntroductionUnit2", "未婚懷孕", "未婚懷孕", false, HFD_IntroductionUnit.Value));
        list.Add(new ControlData("Checkbox", "IntroductionUnit", "CHK_IntroductionUnit3", "各癮症", "各癮症", false, HFD_IntroductionUnit.Value));
        list.Add(new ControlData("Checkbox", "IntroductionUnit", "CHK_IntroductionUnit4", "社會福利", "社會福利", false, HFD_IntroductionUnit.Value));
        list.Add(new ControlData("Checkbox", "IntroductionUnit", "CHK_IntroductionUnit5", "法律諮商", "法律諮商", false, HFD_IntroductionUnit.Value));
        list.Add(new ControlData("Checkbox", "IntroductionUnit", "CHK_IntroductionUnit6", "精神醫療", "精神醫療", false, HFD_IntroductionUnit.Value));
        list.Add(new ControlData("Checkbox", "IntroductionUnit", "CHK_IntroductionUnit7", "性問題", "性問題", false, HFD_IntroductionUnit.Value));
        lblIntroductionUnit.Text = HtmlUtil.RenderControl(list);
        //========================================================================================================
        list = new List<ControlData>();
        //緊急單位(撥打電話) 
        list.Add(new ControlData("Checkbox", "CrashIntroduction", "CHK_CrashIntroduction1", "自殺", "自殺", false, HFD_CrashIntroduction.Value));
        list.Add(new ControlData("Checkbox", "CrashIntroduction", "CHK_CrashIntroduction2", "企圖自殺", "企圖自殺", false, HFD_CrashIntroduction.Value));
        list.Add(new ControlData("Checkbox", "CrashIntroduction", "CHK_CrashIntroduction3", "緊急求救", "緊急求救", false, HFD_CrashIntroduction.Value));
        list.Add(new ControlData("Checkbox", "CrashIntroduction", "CHK_CrashIntroduction4", "暴力", "暴力", false, HFD_CrashIntroduction.Value));
        list.Add(new ControlData("Checkbox", "CrashIntroduction", "CHK_CrashIntroduction5", "癮症", "癮症", false, HFD_CrashIntroduction.Value));
        list.Add(new ControlData("Checkbox", "CrashIntroduction", "CHK_CrashIntroduction6", "性侵", "性侵", false, HFD_CrashIntroduction.Value));
        list.Add(new ControlData("Checkbox", "CrashIntroduction", "CHK_CrashIntroduction7", "身心疾病", "身心疾病", false, HFD_CrashIntroduction.Value));
        list.Add(new ControlData("Checkbox", "CrashIntroduction", "CHK_CrashIntroduction8", "求助", "求助", false, HFD_CrashIntroduction.Value));
        lblCrashIntroduction.Text = HtmlUtil.RenderControl(list);

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
            
            lblRpt.Text = "";
          //  ShowSysMsg("查無資料");
            return "";
        }

        foreach (DataRow dr in dt.Rows)
        {
            string strFontSize = "16px";

            table = new HtmlTable();
            table.Border = 0;
            table.BorderColor = "grey";
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

            //row = new HtmlTableRow();
            cell = new HtmlTableCell();
            css = cell.Style;
            css.Add("text-align", "left");
            css.Add("font-size", strFontSize);
            css.Add("font-weight", strWeight);
            Util.AddTDLine(css, 23, strColor);
            cell.InnerHtml = "6.基督徒：<span style=" + strMemberColor + " >" + dr["Christian"].ToString() + "</span>";
            row.Cells.Add(cell);
            table.Rows.Add(row);
           // table.RenderControl(htw);

           

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
        string strSql = @"
                  select Top 5 ROW_NUMBER() OVER(ORDER BY CreateDate) AS 筆數,
                         ISNULL((SELECT [Name] FROM CodeCity WHERE CodeCity.Zipcode = City),'')  + ISNULL((SELECT Name FROM CodeCity WHERE CodeCity.zipcode = b.ZipCode),'') + [address] as FullAddress, 
                         REVERSE(SUBSTRING(REVERSE(ConsultantMain),CHARINDEX(',',REVERSE(ConsultantMain))+1,LEN(ConsultantMain))) ConsultantMainALL,
                         REVERSE(SUBSTRING(REVERSE(ConsultantItem),CHARINDEX(',',REVERSE(ConsultantItem))+1,LEN(ConsultantItem))) ConsultantItemALL
                        ,b.Memo as M_Memo,*
                        from Consulting a  
                        inner join Member b on a.MemberUID = b.uid
                        left join CodeCity c on b.City = c.ZipCode 
                        left join CodeCity d on b.ZipCode = d.ZipCode     
                  where 1=1 
                  And a.MemberUID=@MemberUID
                  and isnull(a.IsDelete, '') != 'Y'
                  Order by CreateDate desc
                        "; 
        Dictionary<string, object> dict = new Dictionary<string, object>();
       // Response.Write("MemberUID7-->" + HFD_MemberID.Value + "<BR>");
        dict.Add("MemberUID", HFD_MemberID.Value);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        return dt;
    }

    private void CreateConsultingData()
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        List<ColumnData> list = new List<ColumnData>();
        string strSql = "";
        string NewUID = "";
        SetColumnData(list);

        List<ColumnData> list2 = new List<ColumnData>();
        if (HFD_ConsultingUID.Value == "")
        {
            SetConsultingColumnData(list2);
            strSql = Util.CreateInsertCommand("Consulting", list2, dict);
            strSql += "select @@IDENTITY";
            HFD_ConsultingUID.Value = NpoDB.GetScalarS(strSql, dict);
        }
     }

    private void LoadConsulting()
    {
        string strSql;
        DataRow dr = null;
        DataTable dt = null;
        Dictionary<string, object> dict = new Dictionary<string, object>();
       
         strSql = @"
                   select * ,
                   DATEPART(hour, CreateDate) AS strH,DATEPART(minute, CreateDate) AS strM 
                   ,DATEPART(hour, EndDate) AS EndH,DATEPART(minute, EndDate) AS EndM
                   from Consulting
                   where uid=@uid
                   and isnull(IsDelete, '') != 'Y'
                  ";          // 
         dict.Add("uid", HFD_ConsultingUID.Value );
        dt = NpoDB.GetDataTableS(strSql, dict);
        //資料異常
        if (dt.Rows.Count == 0)
        {
            return;
        }
        dr = dt.Rows[0];
        //建檔時間
        txtCreateDate.Text = (HFD_Mode.Value == "ADD") ? Util.GetToday(DateType.yyyyMMdd) : Util.DateTime2String(dr["CreateDate"].ToString(), DateType.yyyyMMdd, EmptyType.ReturnEmpty);
        //txtCreateTime.Text = (HFD_Mode.Value == "ADD") ? Util.GetToday(DateType.HHmmss) : Util.DateTime2String(dr["CreateDate"].ToString(), DateType.HHmmss, EmptyType.ReturnEmpty);
        //txtEndTime.Text = (HFD_Mode.Value == "ADD") ? Util.GetToday(DateType.HHmmss) : Util.DateTime2String(dr["EndDate"].ToString(), DateType.HHmmss, EmptyType.ReturnEmpty);

        //服務志工
        txtServiceUser.Text = (HFD_Mode.Value == "ADD") ? "" : dr["ServiceUser"].ToString();
        //事件
        txtEvent.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Event"].ToString();
        //想法
        txtThink.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Think"].ToString();
        //感受
        txtFeel.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Feel"].ToString();
        //補充說明
        txtComment.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Comment"].ToString();
        //來電諮詢別(大類)
        HFD_ConsultantMain.Value = (HFD_Mode.Value == "ADD") ? "" : dr["ConsultantMain"].ToString();
        //來電諮詢別 (分項)
        HFD_ConsultantItem.Value = (HFD_Mode.Value == "ADD") ? "" : dr["ConsultantItem"].ToString();
        //特殊諮詢
        HFD_Special.Value = (HFD_Mode.Value == "ADD") ? "" : dr["Special"].ToString();
        //騷擾電話
        HFD_HarassPhone.Value = (HFD_Mode.Value == "ADD") ? "" : dr["HarassPhone"].ToString();
        //騷擾電話(其他)
        txtHarassOther.Text = (HFD_Mode.Value == "ADD") ? "" : dr["HarassOther"].ToString();
        //轉介單位
        HFD_IntroductionUnit.Value = (HFD_Mode.Value == "ADD") ? "" : dr["IntroductionUnit"].ToString();
        //緊急轉介
        HFD_CrashIntroduction.Value = (HFD_Mode.Value == "ADD") ? "" : dr["CrashIntroduction"].ToString();
        //原因1
        //txtReason1.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Reason1"].ToString();
        //原因2
        //txtReason2.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Reason2"].ToString();
        //原因3
        //txtReason3.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Reason3"].ToString();
        //問題1
        //txtTrouble1.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Trouble1"].ToString();
        //問題2
        //txtTrouble2.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Trouble2"].ToString();
        //問題3
        //txtTrouble3.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Trouble3"].ToString();
        ////因...造成問題
        txtProblem.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Problem"].ToString();
        //找出案主心中的假設
        txtFindAssume.Text = (HFD_Mode.Value == "ADD") ? "" : dr["FindAssume"].ToString();
        //與案主討論解釋與假設
        txtDiscuss.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Discuss"].ToString();
        //加入新的元素
        txtElement.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Element"].ToString();
        //改變案主原先的期待
        txtExpect.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Expect"].ToString();
        //給予案主祝福與盼望
        txtBlessing.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Blessing"].ToString();
        //轉介教會
        txtIntroductionChurch.Text = (HFD_Mode.Value == "ADD") ? "" : dr["IntroductionChurch"].ToString();
        //我對個案的幫助程度
        rdoHelpLvMark.SelectedValue = (HFD_Mode.Value == "ADD") ? "" : dr["HelpLvMark"].ToString();
        //小叮嚀          
        txtMemo.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Memo"].ToString();
       
    }
    //日期檢查
    public bool isDate(string dateStr)
    {
        bool _isDate = false;
        //string matchStr = "";
        //matchStr += @"^((?!0000)[0-9]{4}/((0?[1-9]|1[0-2])/(0?[1-9]|1[0-9]|2[0-8])|(0[13-9]|1[0-2])-(29|30)|(0[13578]|1[02])-31)|([0-9]{2}(0[48]|[2468][048]|[13579][26])|(0[48]|[2468][048]|[13579][26])00)-02-29)$  ";
        //RegexOptions option = (RegexOptions.IgnoreCase | (RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace));

        //if (Regex.IsMatch(dateStr, matchStr, option))
        try
        {
            DateTime.Parse(dateStr);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
        //if  (isDate(dateStr))
        //    _isDate = true;
        //else
        //    _isDate = false;
      //  return _isDate;
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

    public bool IsHaveName(string strName)
    {
        string strSql;
        DataTable dt = null;
        Dictionary<string, object> dict = new Dictionary<string, object>();
            strSql = @"
                   select *
                   from Member
                   where 1=1 
                   And CName like @CName
                   And isnull(IsDelete, '') != 'Y'
                   And CName NOT LIKE  '%弟兄%'  AND  CName NOT LIKE  '%姐妹%' AND  CName NOT LIKE  '%姊妹%'  AND  CName NOT LIKE  '%先生%' 
                   And CName NOT LIKE  '%太太%'  AND  CName NOT LIKE  '%小姐%' AND  CName NOT LIKE  '%媽媽%'  AND  CName NOT LIKE  '%爸爸%' 
                   And CName NOT LIKE  '%女士%' 
                  ";
            dict.Add("CName", "%" + strName + "%");
            dt = NpoDB.GetDataTableS(strSql, dict);
            if (dt.Rows.Count >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
    }

    private string chkField()
    {
        //if (isDate(txtCreateDate.Text) == false)
        //{   
        //    txtCreateDate.Text = "";
        //    return "日期格式錯誤，格式為 yyyy/mm/dd";
        //}

        //if (isTime(txtCreateTime.Text) == false)
        //{
        //    txtCreateTime.Text = "";
        //    return "時間格式錯誤，格式為 hh:mm:ss";
        //}
        if (isTime(txtEndTime.Text) == false)
        {
            txtEndTime.Text = "";
            return "時間格式錯誤，格式為 hh:mm:ss";
        }
        return "";
    }
    protected void btnReload_Click(object sender, EventArgs e)
    {
        Button1.Enabled = true;
        btnUpdate.Enabled = true;
        LoadFormData();
        LoadHiddenData();
    }
    protected void btnEndDate_Click(object sender, EventArgs e)
    {

        DateTime t1, t2 ;
        t1 = DateTime.Parse(txtCreateDate.Text + " " + txtCreateTime.Text);
        if (isTime(txtCreateTime.Text) == false)
        {
            txtCreateTime.Text = "";
            ShowSysMsg("時間格式錯誤，格式為 hh:mm:ss");
        }
      
        if (txtEndTime.Text == "")
        {
         
            btnEndDate.Text = "計算時間";
            txtEndTime.Text = Util.GetToday(DateType.HHmmss);
            if (isTime(txtEndTime.Text) == false)
            {
                txtEndTime.Text = "";
                ShowSysMsg("時間格式錯誤，格式為 hh:mm:ss");
            }
            t2 = DateTime.Parse(txtCreateDate.Text + " " + txtEndTime.Text);
            lblTime.Text = DateDiff(t1, t2);
        }
        else
        {
            if (isTime(txtEndTime.Text) == false)
            {
                txtEndTime.Text = "";
                ShowSysMsg("時間格式錯誤，格式為 hh:mm:ss");
            }
            t2 = DateTime.Parse(txtCreateDate.Text + " " + txtEndTime.Text);
            lblTime.Text = DateDiff(t1, t2);
        }
    }

    private string DateDiff(DateTime DateTime1, DateTime DateTime2)
    {
        string dateDiff = null;
        TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
        TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
        TimeSpan ts = ts1.Subtract(ts2).Duration();
        //dateDiff = "通話時間共：" + (int.Parse(ts.Days.ToString()) * 1440 + int.Parse(ts.Hours.ToString()) * 60 + int.Parse(ts.Minutes.ToString())).ToString() + "分" + ts.Seconds.ToString() + "秒";
        dateDiff = "通話時間共：<span style=color:red>" + (int.Parse(ts.Days.ToString()) * 1440 + int.Parse(ts.Hours.ToString()) * 60 + int.Parse(ts.Minutes.ToString())).ToString() + "分</span>";
        return dateDiff;
    }

    private void GetPhoneToUid()
    {
         string strSql;
        DataRow dr = null;
        DataTable dt = null;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        Dictionary<string, object> dict2 = new Dictionary<string, object>();
        Dictionary<string, object> dict3 = new Dictionary<string, object>();

        //由電話系統取得電話**************************************************************
        strSql = @"
                   select top 1 phone
                   from InBound
                   where IP=@IP
                   and isnull(IsProcess, '') != 'Y'
                  ";

        HttpCookie CookieAgentID = Request.Cookies["AgentID"];  
        dict.Add("IP",  Server.UrlDecode(CookieAgentID.Value));//Request.ServerVariables["REMOTE_ADDR"]
        dt = NpoDB.GetDataTableS(strSql, dict);
        //資料異常
        if (dt.Rows.Count == 0)
        {
           // ShowSysMsg("無新來電");
            return;
        }
        dr = dt.Rows[0];

        //將此client IP 的是否已接聽得值 都update 為 已接聽***************************************************************

        strSql = @"
                   Update InBound 
                   Set IsProcess='Y',
                       UpdateID=@UpdateID,
                       UpdateDate=@UpdateDate
                   where 1=1
                   And IP=@IP
                   and isnull(IsProcess, '') != 'Y'
                  ";
        dict2.Add("IP", Server.UrlDecode(CookieAgentID.Value));//Request.ServerVariables["REMOTE_ADDR"]
        dict2.Add("UpdateID", SessionInfo.UserID);
        dict2.Add("UpdateDate", Util.GetToday(DateType.yyyyMMddHHmmss));
        NpoDB.ExecuteSQLS(strSql, dict2);

        //從電話號碼找會員資料*******************************************
        strSql = @"
                   select *
                   from Member
                   where phone like @phone
                   and isnull(IsDelete, '') != 'Y'
                  ";
        if ((HFD_Phone.Value.ToString().Length) >= 4)
        {
            dict3.Add("phone", "%" + HFD_Phone.Value + "%");
        }
        else
        {
            dict3.Add("phone", "" + HFD_Phone.Value + "");
        }
        dt = NpoDB.GetDataTableS(strSql, dict3);
        //資料異常
        if (dt.Rows.Count == 0)
        {
            HFD_UID.Value = "";
        }
        else
        {
            dr = dt.Rows[0];
            HFD_UID.Value = dr["uid"].ToString();
            HFD_MemberID.Value = dr["uid"].ToString();
            //Response.Write("MemberUID8-->" + HFD_MemberID.Value + "<BR>");
        }
     
    }







    protected void btnSerch_Click(object sender, EventArgs e)
    {


        //if (txtCName.Text != "" && txtCName.Text != "第一次來電" && txtCName.Text.ToString().Length >= 3)
        //{
        //    Dictionary<string, object> dict = new Dictionary<string, object>();
        //    List<ColumnData> list = new List<ColumnData>();
        //    string strSql = "";

        //    CreateConsultingData();
        //    SetColumnData(list);

        //    if (HFD_MemberID.Value != "")      //若有UID 表示有會友資料
        //    {
        //        strSql = Util.CreateUpdateCommand("Member", list, dict);
        //        NpoDB.ExecuteSQLS(strSql, dict);
        //    }
        //    else  //若沒有UID 表示沒有會友資料 要Insert
        //    {
        //        strSql = Util.CreateInsertCommand("Member", list, dict);
        //        strSql += "select @@IDENTITY";
        //        HFD_MemberID.Value = NpoDB.GetScalarS(strSql, dict);
        //    }
        //    List<ColumnData> list2 = new List<ColumnData>();
        //    SetConsultingColumnData(list2);
        //    strSql = Util.CreateUpdateCommand("Consulting", list2, dict);
        //    NpoDB.ExecuteSQLS(strSql, dict);
        //    Response.Redirect(Util.RedirectByTime("ConsultEdit.aspx"));
        //}
        //else
        //{
        //txtCName.Enabled = true;
        //}
        
    }

    protected void btnQry_Click(object sender, EventArgs e)
    {
        Response.Write("<script language='javascript'> window.open('AdvisoryDataQry.aspx', 'window','HEIGHT=600,WIDTH=820,top=50,left=50,toolbar=yes,scrollbars=yes,resizable=yes');</script>");
    }
    protected void btnNothing_Click(object sender, EventArgs e)
    {
        return;
    }
}

 