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


public partial class CaseMgr_ConsultAdd : BasePage
{
    GroupAuthrity ga = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["ProgID"] = "Consult";
        //權限控制
        AuthrityControl();

        ShowSysMsg();

        if (!IsPostBack)
        {
            HFD_Mode.Value = Util.GetQueryString("Mode");
            HFD_UID.Value = Util.GetQueryString("UID");
            HFD_ConsultingUID.Value = Util.GetQueryString("ConsultingUID");
       
           LoadConsulting();
           
            //載入下拉式選單資料
            LoadDropDownListData();
            //載入資料
            LoadFormData();

            if (txtCreateDate.Text.Trim() == "")
            {
                txtCreateDate.Text = Util.GetToday(DateType.yyyyMMdd);
            }
            if (txtCreateTime.Text.Trim() == "")
            {
                txtCreateTime.Text = Util.GetToday(DateType.HHmmss);
            }

            SetButton(); //要擺在讀資料之後
        }
        LoadHiddenData(); //來電項目
    }

    private void SetupDefaultValue()
    {
    }
    //-------------------------------------------------------------------------
    private void SetButton()
    {
        btnDelete.Visible = (SessionInfo.GroupName == "系統管理員") ? true : false;
       // btnUpdate.Visible = (SessionInfo.GroupName == "系統管理員") ? true : false;
        if (HFD_Mode.Value == "NEW")
        {
            //新增時, 修改及刪除鍵沒動作
            //btnUpdate.Visible = false;
            btnAdd.Visible = false;
            //btnSelectMember.Visible  = true;
            txtServiceUser.Text = SessionInfo.UserID.ToString();
        }
        else
        {
            //修改時, 新增鍵沒動作
            btnAdd.Visible = false;
        }

        //if (txtServiceUser.Text == SessionInfo.UserID.ToString())
        //{
        //    btnUpdate.Visible = true;
        //  //  btnDelete.Visible = true;
        //}
        //else
        //{
        //    btnUpdate.Visible = false;
        //   // btnDelete.Visible = false;
        //}
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
    }
    //-------------------------------------------------------------------------------------------------------------
   private void LoadFormData()
    {
        string strSql;
        DataRow dr = null;
        DataTable dt = null;
        Dictionary<string, object> dict = new Dictionary<string, object>();

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
                return;
            }
            dr = dt.Rows[0];
        }

        //電話
        txtPhone.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Phone"].ToString();
        //第一次
       // chkFirst.Checked = Boolean.Parse((HFD_Mode.Value == "ADD") ? "false" : dr["First"].ToString());
        //年齡
        txtAge.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Age"].ToString();
        ddlAge.SelectedValue = (HFD_Mode.Value == "ADD") ? "" : dr["Age"].ToString();
        //轉介教會
        rdoChurchYN.SelectedValue = (HFD_Mode.Value == "ADD") ? "" : dr["ChurchYN"].ToString();
        //姓名
        txtCName.Text = (HFD_Mode.Value == "ADD") ? "" : dr["CName"].ToString();
        //婚姻
        HFD_Marry.Value = (HFD_Mode.Value == "ADD") ? "" : dr["Marry"].ToString();
        //rdoMarry.SelectedValue = (HFD_Mode.Value == "ADD") ? "" : dr["Marry"].ToString();
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
       // txtFileName.Text = (HFD_Mode.Value == "ADD") ? "" : dr["FileName"].ToString();

        if (txtCreateDate.Text == "")
        {
            txtCreateDate.Text = Util.GetToday(DateType.yyyyMMdd);
        }
        if (txtCreateTime.Text == "")
        {
            txtCreateTime.Text = Util.GetToday(DateType.HHmmss);
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
        if (strChkField != "")
        {
            ShowSysMsg(strChkField);
        }
        CreateConsultingData();
        SetColumnData(list);
        
        if (HFD_MemberID.Value != "")      //若有UID 表示有會友資料
        {
            strSql = Util.CreateUpdateCommand("Member", list, dict);
            NpoDB.ExecuteSQLS(strSql, dict);
        }
        else  //若沒有UID 表示沒有會友資料 要Insert
        {
            strSql = Util.CreateInsertCommand("Member", list, dict);
            strSql += "select @@IDENTITY";
            HFD_UID.Value = NpoDB.GetScalarS(strSql, dict);
            HFD_MemberID.Value = HFD_UID.Value;
        }

        List<ColumnData> list2 = new List<ColumnData>();
        SetConsultingColumnData(list2);
        strSql = Util.CreateUpdateCommand("Consulting", list2, dict);
        NpoDB.ExecuteSQLS(strSql, dict);
        //SetSysMsg("儲存資料成功");
        //Response.Write(@"<script>window.close();</script>");
        //this.Response.Write(@"<script>self.opener.LoadConsultQry('LoadGird');window.close();</script>");
        this.Response.Write(@"<script>self.opener.LoadConsultQry('LoadGird');window.close();</script>");
        //this.Response.Write(@"<script>window.close();</script>");
        //Response.Redirect(Util.RedirectByTime("ConsultEdit2.aspx", "UID=" + HFD_UID.Value + "&ConsultingUID=" + HFD_ConsultingUID.Value + ""));
    }
    //-------------------------------------------------------------------------------------------------------------
    //基本資料
    private void SetColumnData(List<ColumnData> list)
    {
        //第一個欄位給 update 用
        list.Add(new ColumnData("uid", HFD_MemberID.Value, false, false, true));
        //電話
        list.Add(new ColumnData("Phone", txtPhone.Text, true, true, false));
        //第一次
        //list.Add(new ColumnData("First", chkFirst.Checked.ToString(), true, true, false));
        //年齡
        //list.Add(new ColumnData("Age", txtAge.Text, true, true, false));
        list.Add(new ColumnData("Age", ddlAge.SelectedValue, true, true, false));
        //轉介教會
        list.Add(new ColumnData("ChurchYN", rdoChurchYN.SelectedValue, true, true, false));
        //姓名
        list.Add(new ColumnData("CName", txtCName.Text, true, true, false));
        //婚姻
        list.Add(new ColumnData("Marry", HFD_Marry.Value.ToString().TrimEnd(',') , true, true, false));
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
        list.Add(new ColumnData("Overseas", HFD_Overseas.Value, true, true, false));
        //檔案名稱	FileName
        //list.Add(new ColumnData("FileName", txtFileName.Text, true, true, false));

        if (HFD_Mode.Value == "NEW")
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
      //  Response.Redirect(Util.RedirectByTime("ConsultQry.aspx"));
        //Response.Write(@"<script>window.close();</script>");
        this.Response.Write(@"<script>self.opener.LoadConsultQry('LoadGird');window.close();</script>");
    }
    //-------------------------------------------------------------------------------------------------------------
    protected void btnExit_Click(object sender, EventArgs e)
    {
        //Response.Redirect(Util.RedirectByTime("ConsultQry.aspx"));
        Response.Write(@"<script>window.close();</script>");

    }
    //-------------------------------------------------------------------------------------------------------------
    //諮詢資料
    private void SetConsultingColumnData(List<ColumnData> list)
    {
        //第一個欄位給 update 用
        list.Add(new ColumnData("uid", HFD_ConsultingUID.Value, false, false, true));
        //會員編號
        list.Add(new ColumnData("MemberUID", HFD_MemberID.Value, true, true, false));
        //服務志工          
        list.Add(new ColumnData("ServiceUser", txtServiceUser.Text , true, true, false));
        //建檔時間          
        list.Add(new ColumnData("CreateDate", txtCreateDate.Text + " " + txtCreateTime.Text, true, true, false));
        if (txtEndTime.Text.ToString().Trim() != "")
        {
            list.Add(new ColumnData("EndDate", Util.DateTime2String(txtCreateDate.Text + " " + txtEndTime.Text, DateType.yyyyMMddHHmmss, EmptyType.ReturnNull), true, true, false));
        }
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
        if (HFD_HarassPhone.Value == "" && HFD_Special.Value =="")
        {
            //來電諮詢別 (大類)
            list.Add(new ColumnData("ConsultantMain", HFD_ConsultantMain.Value, true, true, false));
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

        if (HFD_Mode.Value == "NEW")
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

    protected void btnReload_Click(object sender, EventArgs e)
    {
        LoadFormData();
        
    }

    protected void btnUpdateConsulting_Click(object sender, EventArgs e)
    {
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
        list.Add(new ControlData("Checkbox", "ConsultantMain", "CHK_ConsultantMain1", "婚前", "婚前", false, HFD_ConsultantMain.Value));
        list.Add(new ControlData("Checkbox", "ConsultantMain", "CHK_ConsultantMain2", "婚姻", "婚姻", false, HFD_ConsultantMain.Value));
        list.Add(new ControlData("Checkbox", "ConsultantMain", "CHK_ConsultantMain3", "單親", "單親", false, HFD_ConsultantMain.Value));
        list.Add(new ControlData("Checkbox", "ConsultantMain", "CHK_ConsultantMain4", "親子", "親子", false, HFD_ConsultantMain.Value));
        list.Add(new ControlData("Checkbox", "ConsultantMain", "CHK_ConsultantMain5", "重組家庭", "重組家庭", false, HFD_ConsultantMain.Value));
        list.Add(new ControlData("Checkbox", "ConsultantMain", "CHK_ConsultantMain6", "遠距家庭", "遠距家庭", false, HFD_ConsultantMain.Value));
        list.Add(new ControlData("Checkbox", "ConsultantMain", "CHK_ConsultantMain7", "隔代家庭", "隔代家庭", false, HFD_ConsultantMain.Value));
        list.Add(new ControlData("Checkbox", "ConsultantMain", "CHK_ConsultantMain8", "通婚家庭", "通婚家庭", false, HFD_ConsultantMain.Value));
        list.Add(new ControlData("Checkbox", "ConsultantMain", "CHK_ConsultantMain9", "受難家庭", "受難家庭", false, HFD_ConsultantMain.Value));
        list.Add(new ControlData("Checkbox", "ConsultantMain", "CHK_ConsultantMain10", "中年問題", "中年問題", false, HFD_ConsultantMain.Value));
        list.Add(new ControlData("Checkbox", "ConsultantMain", "CHK_ConsultantMain11", "老年問題", "老年問題", false, HFD_ConsultantMain.Value));
        lblConsultantMain.Text = HtmlUtil.RenderControl(list);
        //=================================================================================================================
        //來電諮詢別(分項) 
        list = new List<ControlData>();
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem1", "經濟", "經濟", false, HFD_ConsultantItem.Value));
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
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem12", "性生活", "性生活", false, HFD_ConsultantItem.Value));
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
        //特殊諮詢 
        list.Add(new ControlData("Checkbox", "Special", "CHK_Special1", "感恩祝福", "感恩祝福", false, HFD_Special.Value));
        list.Add(new ControlData("Checkbox", "Special", "CHK_Special2", "宗教問題", "宗教問題", false, HFD_Special.Value));
        list.Add(new ControlData("Checkbox", "Special", "CHK_Special2", "代禱", "代禱", false, HFD_Special.Value));
        lblSpecial.Text  = HtmlUtil.RenderControl(list);
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


    private void CreateConsultingData()
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        List<ColumnData> list = new List<ColumnData>();
        string strSql = "";
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
    //諮詢內容
    private void LoadConsulting()
    {
        string strSql;
        DataRow dr = null;
        DataTable dt = null;
        Dictionary<string, object> dict = new Dictionary<string, object>();

        strSql = @"
                   select *
                   from Consulting
                   where uid=@uid
                   and isnull(IsDelete, '') != 'Y'
                  ";    
        dict.Add("uid", HFD_ConsultingUID.Value);
        dt = NpoDB.GetDataTableS(strSql, dict);
        //資料異常
        if (dt.Rows.Count == 0)
        {
            return;
        }
        dr = dt.Rows[0];

        HFD_MemberID.Value  = (HFD_Mode.Value == "ADD") ? "" : dr["MemberUID"].ToString();
        //建檔時間
        txtCreateDate.Text = (HFD_Mode.Value == "ADD") ? Util.GetToday(DateType.yyyyMMdd) : Util.DateTime2String(dr["CreateDate"].ToString(),DateType.yyyyMMdd ,EmptyType.ReturnEmpty );
        txtCreateTime.Text = (HFD_Mode.Value == "ADD") ? Util.GetToday(DateType.HHmmss) : Util.DateTime2String(dr["CreateDate"].ToString(), DateType.HHmmss, EmptyType.ReturnEmpty);
        //txtEndDate.Text = (HFD_Mode.Value == "ADD") ? Util.GetToday(DateType.yyyyMMdd) : Util.DateTime2String(dr["EndDate"].ToString(), DateType.yyyyMMdd, EmptyType.ReturnEmpty);
        txtEndTime.Text = (HFD_Mode.Value == "ADD") ? Util.GetToday(DateType.HHmmss) : Util.DateTime2String(dr["EndDate"].ToString(), DateType.HHmmss, EmptyType.ReturnEmpty);
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
       // txtReason1.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Reason1"].ToString();
        //原因2
        // txtReason2.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Reason2"].ToString();
        //原因3
        //txtReason3.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Reason3"].ToString();
        //問題1
        // txtTrouble1.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Trouble1"].ToString();
        //問題2
        //txtTrouble2.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Trouble2"].ToString();
        //問題3
        // txtTrouble3.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Trouble3"].ToString();
        //因...造成問題
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

    private string  chkField()
    {
        if (isDate(txtCreateDate.Text) == false)
        {
            return "日期格式錯誤，格式為 yyyy/mm/dd";
        }

        if (isTime(txtCreateTime.Text) == false)
        {
            txtCreateTime.Text = "";
            return "時間格式錯誤，格式為 hh:mm:ss";
        }
        if (isTime(txtEndTime.Text) == false)
        {
            txtEndTime.Text = "";
            return "時間格式錯誤，格式為 hh:mm:ss";
        }

        return "";
    }
    protected void btnEndDate_Click(object sender, EventArgs e)
    {
        DateTime t1, t2;
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
        dateDiff ="通話時間共：" + (int.Parse(ts.Days.ToString()) * 1440 + int.Parse(ts.Hours.ToString()) * 60 + int.Parse(ts.Minutes.ToString())).ToString() + "分" + ts.Seconds.ToString() + "秒";
        return dateDiff;
    }
    
}