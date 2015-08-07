using System;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.Script.Serialization;
using System.Collections.Generic;

public partial class Ajax : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string Type = Request.Form["Type"];
        string result = "";
        result = ProcessCommonRequest(Type);

        if (result != "ERROR")
        {
            Response.Write(result);
            return;
        }
        result = ProcessRequest1(Type);
        if (result != "ERROR")
        {
            Response.Write(result);
            return;
        }
        result = ProcessRequest2(Type);
        if (result != "ERROR")
        {
            Response.Write(result);
            return;
        }
        result = ProcessRequest3(Type);
        if (result != "ERROR")
        {
            Response.Write(result);
            return;
        }

        Response.Write("");   //firewall
    }
    //-------------------------------------------------------------------------------------------------------------
    private string ProcessCommonRequest(string Type)
    {
        string result = "";

        switch (Type)
        {
            //Common
            case "GetAreaList":  //以 CityID 取得其鄉鎮列表
                result = GetAreaList();
                break;
            case "GetStreetList":  //以 CityName & AreaName 取得其街名列表
                result = GetStreetList();
                break;
            case "GetStreetListByID":  //以 CityID & AreaID 取得其街名列表
                result = GetStreetListByID();
                break;
            case "LoadChurchMember":
                result = LoadChurchMember();
                break;
            case "LoadConsultQry":
                result = LoadConsultQry();
                break;
            case "LoadMainDefault":
                result = LoadMainDefault();
                break;
            default:
                result = "ERROR";
                break;
        }
        return result;
    }
    //-------------------------------------------------------------------------------------------------------------
    private string ProcessRequest1(string Type)
    {
        string result = "";
        
        switch (Type)
        {
            case "LoadRentalDetail":  //載入收租明細
                result = LoadRentalDetail();
                break;
            case "LoadPaymentDetail":  //載入付租明細
                result = LoadPaymentDetail();
                break;
            case "LoadRentUnit":  //載入收租租賃單位 
                result = LoadRentUnit();
                break;
            default:
                result = "ERROR";
                break;
        }
        return result;
    }
    //-------------------------------------------------------------------------------------------------------------
    private string ProcessRequest2(string Type)
    {
        string result = "";

        switch (Type)
        {
            case "GetHouseList":  //取得土地上的房屋列表
                result = CaseUtil.GetHouseList(Request.Form["LandUID"]);
                break;
            case "GetLandList":  //取得房屋所屬的土地列表
                result = CaseUtil.GetLandList(Request.Form["HouseUID"]);
                break;
            default:
                result = "ERROR";
                break;
        }
        return result;
    }
    //-------------------------------------------------------------------------------------------------------------
    private string ProcessRequest3(string Type)
    {
        string result = "";

        switch (Type)
        {
            case "GetHouseList":  //取得土地上的房屋列表
                result = CaseUtil.GetHouseList(Request.Form["LandUID"]);
                break;
            case "GetLandList":  //取得房屋所屬的土地列表
                result = CaseUtil.GetLandList(Request.Form["HouseUID"]);
                break;
            case "LoadRentUnit":
                result = LoadRentUnit();
                break;
            case "LoadGird":
                result = LoadGird();
                break;
            //case "LoadVolunteer":
            //    result =LoadVolunteer();
            //    break ;
            default:
                result = "ERROR";
                break;
        }
        return result;
    }
    //-------------------------------------------------------------------------------------------------------------
    //以 CityID 取得其鄉鎮列表
    private string GetAreaList()
    {
        string CityID = Request.Form["CityID"];

        string strSql = "select ZipCode, Name from CodeCity\n";
        strSql += "where ParentCityID=@CityID\n";
        strSql += "order by Sort\n";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("CityID", CityID);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        string retStr = "";
        retStr += "<option value=''></option>";
        foreach (DataRow dr in dt.Rows)
        {
            retStr += "<option value='" + dr["ZipCode"].ToString() + "'>" + dr["Name"].ToString() + "</option>";
        }
        return retStr;
    }
    //-------------------------------------------------------------------------
    //以 CityName & AreaName 取得其街名列表
    private string GetStreetList()
    {
        string CityName = Request.Form["CityName"];
        string AreaName = Request.Form["AreaName"];

        string strSql = @"
                            select uid, StreetName
                            from CodeStreet
                            where CityName=@CityName and AreaName=@AreaName
                            order by StreetName
                        ";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("CityName", CityName);
        dict.Add("AreaName", AreaName);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        string retStr = "";
        retStr += "<option value=''></option>";
        foreach (DataRow dr in dt.Rows)
        {
            retStr += "<option value='" + dr["uid"].ToString() + "'>" + dr["StreetName"].ToString() + "</option>";
        }
        return retStr;
    }
    //-------------------------------------------------------------------------
    //以 CityID & AreaID 取得其街名列表
    private string GetStreetListByID()
    {
        string CityID = Request.Form["CityID"];
        string AreaID = Request.Form["AreaID"];

        string strSql = @"
                            select cs.uid, StreetName
                            from CodeStreet cs
                            inner join CodeCity cc1 on cc1.ZipCode=@CityID
                            inner join CodeCity cc2 on cc2.ZipCode=@AreaID
                            where CityName=cc1.Name and AreaName=cc2.Name
                            order by StreetName
                        ";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("CityID", CityID);
        dict.Add("AreaID", AreaID);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        string retStr = "";
        retStr += "<option value=''></option>";
        foreach (DataRow dr in dt.Rows)
        {
            retStr += "<option value='" + dr["uid"].ToString() + "'>" + dr["StreetName"].ToString() + "</option>";
        }
        return retStr;
    }
    //-------------------------------------------------------------------------
    protected string LoadRentalDetail()
    {
        string strSql = @"
                            select  
                                Uid as Uid
                                ,RenterUid
                                ,convert(varchar(10), PaymentDate,111) as [實收租金日]
                                ,PaymentType as [付租方式]
                                ,RenterPayment as [實收租金]
                                ,FinePayment as [違約金]
                                ,OtherPayment as [其他費用]
                                ,Supplement as [備註]
                            from RentalDetail
                            where RenterUid=@RenterUid
                            and DeleteDate is null
                            order by PaymentDate desc
                        ";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("RenterUid", Request.Form["Uid"]);

        DataTable dt = NpoDB.GetDataTableS(strSql, dict);

        if (dt.Rows.Count == 0)
        {
            return "";
        }

        //Grid initial
        //沒有需要特別處理的欄位時
        NPOGridView npoGridView = new NPOGridView();
        npoGridView.Source = NPOGridViewDataSource.fromDataTable;
        npoGridView.dataTable = dt;
        npoGridView.ShowPage = false;
        npoGridView.Keys.Add("Uid");
        npoGridView.DisableColumn.Add("Uid");
        npoGridView.QuerySyting.Add("Uid");
        npoGridView.Keys.Add("RenterUid");
        npoGridView.DisableColumn.Add("RenterUid");
        npoGridView.QuerySyting.Add("RenterUid");

        npoGridView.EditLinkTarget = "租金資料編輯','help=no,status=no,resizable=no,scroll=no,center=yes,width=700px,height=500px";
        npoGridView.EditLink = Util.RedirectByTime("RentalDetail_Edit.aspx", "Mode=MOD");
        return npoGridView.Render();
    }
    //-------------------------------------------------------------------------
    protected string LoadPaymentDetail()
    {
        string strSql = @"
                            select  
                                Uid as Uid
                                ,LandlordUid
                                ,convert(varchar(10), PaymentDate,111) as [實付租金日]
                                ,RentPayment as [實付租金]
                                ,FinePayment as [違約金]
                                ,OtherPayment as [其他費用]
                                ,Supplement as [備註]
                            from PaymentDetail
                            where LandlordUid=@LandlordUid
                            and DeleteDate is null
                            order by PaymentDate desc
                        ";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("LandlordUid", Request.Form["Uid"]);

        DataTable dt = NpoDB.GetDataTableS(strSql, dict);

        if (dt.Rows.Count == 0)
        {
            return "";
        }

        //Grid initial
        //沒有需要特別處理的欄位時
        NPOGridView npoGridView = new NPOGridView();
        npoGridView.Source = NPOGridViewDataSource.fromDataTable;
        npoGridView.dataTable = dt;
        npoGridView.ShowPage = false;
        npoGridView.Keys.Add("Uid");
        npoGridView.DisableColumn.Add("Uid");
        npoGridView.QuerySyting.Add("Uid");
        npoGridView.Keys.Add("LandlordUid");
        npoGridView.DisableColumn.Add("LandlordUid");
        npoGridView.QuerySyting.Add("LandlordUid");

        npoGridView.EditLinkTarget = "租金資料編輯','help=no,status=no,resizable=no,scroll=no,center=yes,width=700px,height=500px";
        npoGridView.EditLink = Util.RedirectByTime("PaymentDetail_Edit.aspx", "Mode=MOD");
        return npoGridView.Render();
    }
    //-------------------------------------------------------------------------
    protected string LoadRentUnit()
    {
        string strSql = @"
                            select  
                                R.Uid as Uid
                                ,R.RenterUid as RenterUid
                                ,H.UnitAddress as [租賃單位地址]
                                --,R.Supplement as [備註]
                            from RentUnitData R
                            left join HouseUnitData H
                            on R.HouseUnitUid=H.Uid
                            where R.RenterUid=@RenterUid
                            and R.DeleteDate is null
                            order by R.Uid 
                        ";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("RenterUid", Request.Form["Uid"]);

        DataTable dt = NpoDB.GetDataTableS(strSql, dict);

        if (dt.Rows.Count == 0)
        {
            return "";
        }

        //Grid initial
        //沒有需要特別處理的欄位時
        NPOGridView npoGridView = new NPOGridView();
        npoGridView.Source = NPOGridViewDataSource.fromDataTable;
        npoGridView.dataTable = dt;
        npoGridView.ShowPage = false;
        npoGridView.Keys.Add("Uid");
        npoGridView.DisableColumn.Add("Uid");
        npoGridView.QuerySyting.Add("Uid");
        npoGridView.Keys.Add("RenterUid");
        npoGridView.DisableColumn.Add("RenterUid");
        npoGridView.QuerySyting.Add("RenterUid");

        npoGridView.EditLinkTarget = "租賃單位編輯','help=no,status=no,resizable=no,scroll=no,center=yes,width=700px,height=500px";
        npoGridView.EditLink = Util.RedirectByTime("SelectRentUnit.aspx", "Mode=MOD");
        return npoGridView.Render();
    }
    //-------------------------------------------------------------------------
    protected string LoadGird()
    {

        string strSql = @"
                   
                    Select Uid as [Uid],
                            AssetsNum as [財產編號] ,
                            Scope as  面積,
                            Stock as [持份] ,
                            Parting as 變動狀態,
                            TotalPrice as 總價 ,
                            LandNum as 座落地號
                            ,AssetsName  as 財產名稱 ,
                            convert(varchar(10), IssueDate,111) as 發狀日期 ,
                            Mark as  備註
                              
                        from MoveDetail 
                        where DeleteDate is null And MoveMainUid=@Uid
                        ";

        Dictionary<string, object> dict = new Dictionary<string, object>();

        dict.Add("Uid", Request.Form["Uid"]);

        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        string Uid = Request.Form["Uid"];
        //Grid initial
        //沒有需要特別處理的欄位時
        NPOGridView npoGridView = new NPOGridView();
        npoGridView.Source = NPOGridViewDataSource.fromDataTable;
        npoGridView.dataTable = dt;
        npoGridView.ShowPage = false;
        npoGridView.Keys.Add("Uid");
        npoGridView.DisableColumn.Add("Uid");
        npoGridView.EditLinkTarget = "異動資料編輯','help=no,status=yes,resizable=yes,scroll=yes,center=yes,width=900px,height=300px";
        npoGridView.EditLink = Util.RedirectByTime("MoveMgrList.aspx", "Mode=MOD&MoveMainUid=" + Uid + "&Uid=");
        return npoGridView.Render();

    

    }

    //人員轉介教會   Church/ChangeChurchQry.aspx
    private string LoadChurchMember()
    {
     string strSql = @"
                        Select  a.uid as uid ,CName as 姓名,
	                    CASE When ChangeChurchSta  IS NULL THEN '未處理' ELSE '已處理' END AS 處理狀態,
	                    b.Name As 教會 
                        From Member a 
	                    left outer join Church b on a.ChurchUid=b.uid 
                        Where 1=1 
                        And ChurchYN = '是'
                        And isnull(a.IsDelete, '') != 'Y'
                        ";
     string Name = Request.Form["Name"];
     string ChangeChurchSta = Request.Form["ChangeChurchSta"];
     string Church = Request.Form["Church"];
     string Event = Request.Form["Event"];
     string ServiceUser = Request.Form["ServiceUser"];
     string Phone = Request.Form["Phone"];
     string BegCreateDate = Request.Form["BegCreateDate"];
     string EndCreateDate = Request.Form["EndCreateDate"];
     string ZipCode = Request.Form["ZipCode"];
     string City = Request.Form["City"];
     string Address = Request.Form["Address"];


     //if (Church != "")
     //{
     //    strSql += " and b.Name like @ChurchName\n";
     //}

     //if (Event != "")
     //{
     //    strSql += " and Event like @Event\n";
     //}

     //if (Name != "")
     //{
     //    strSql += " and CName like @CName\n";
     //}
  
     //if (Phone != "")
     //{
     //    strSql += " and Phone = @Phone\n";
     //}

     //if (BegCreateDate != "" && EndCreateDate != "")
     //{
     //    strSql += " and CONVERT(varchar(100), CreateDate, 111) Between @BegCreateDate And @EndCreateDate\n";
     //}

     //if (ZipCode != "")
     //{
     //    strSql += " And ZipCode=@ZipCode\n";
     //}

     //if (City != "")
     //{
     //    strSql += " And City=@City\n";
     //}

     //if (Address != "")
     //{
     //    strSql += " and a.Address like @Address\n";
     //}

     //if (Name != "")
     //{
     //    strSql += " and CName like @CName\n";
     //}
     if (ChangeChurchSta == "已處理")
     {
         strSql += " and ChangeChurchSta = @ChangeChurchSta\n";
     }
     else
     {
         strSql += " and isnull(changechurchsta,'') != @ChangeChurchSta\n";
     }
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("CName", "%" + Name + "%");
        dict.Add("Event", "%" + Event + "%");
        dict.Add("ServiceUser", "%" + ServiceUser + "%");
        dict.Add("Phone", "%" + Phone + "%");
        dict.Add("BegCreateDate", BegCreateDate);
        dict.Add("EndCreateDate", EndCreateDate);
        dict.Add("ZipCode", ZipCode);
        dict.Add("City", City);
        dict.Add("Address", "%" + Address + "%");
        dict.Add("ChurchName", "%" + Church + "%");
        dict.Add("ChangeChurchSta", "已處理");

        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        string Uid = Request.Form["Uid"];
        //Grid initial
        //沒有需要特別處理的欄位時
        NPOGridView npoGridView = new NPOGridView();
        npoGridView.Source = NPOGridViewDataSource.fromDataTable;
        npoGridView.dataTable = dt;
        npoGridView.ShowPage = false;
        npoGridView.Keys.Add("uid");
        npoGridView.DisableColumn.Add("uid");
        npoGridView.EditLinkTarget = "轉介教會資料編輯','help=no,status=yes,resizable=yes,scrolling=yes,scroll=yes,center=yes,width=900px,height=500px";
        npoGridView.EditLink = Util.RedirectByTime("ChangeChurchEdit.aspx", "Mode=MOD&MoveMainUid=" + Uid + "&uid=");
        return npoGridView.Render();
    }

    private string LoadConsultQry()
    {
        string strSql = @"
                        select  a.uid as uid, CName As  姓名,b.Memo as [備註] , Phone As 電話, convert(varchar(19), CreateDate,120) as 建檔時間, ConsultantMain as [來電諮詢別(大類)], ConsultantItem  as [來電諮詢別(分項)] 
                        from Consulting a  inner join Member b on a.MemberUID = b.uid
                        where 1=1 
                        and isnull(a.IsDelete, '') != 'Y'
                        ";
     string Phone = Request.Form["Phone"];
     string Event = Request.Form["Event"];
     string CName = Request.Form["CName"];
     string Marry = Request.Form["Marry"];
     string BegCreateDate = Request.Form["BegCreateDate"];
     string EndCreateDate = Request.Form["EndCreateDate"];
     string City = Request.Form["City"];
     string Address = Request.Form["Address"];
     string ConsultantItem = Request.Form["ConsultantItem"];
     string ZipCode = Request.Form["ZipCode"];
 	 string ServiceUser = Request.Form["ServiceUser"];
	 string EditServiceUser = Request.Form["EditServiceUser"];



     if (Phone != "")
     {
         strSql += " and Phone like @Phone\n";
     }

     if (Event != "")
     {
         strSql += " and Event like @Event\n";
     }

     if (CName != "")
     {
         strSql += " and CName like @CName\n";
     }

     if (Marry != "")
     {
         strSql += " and Marry like @Marry\n";
     }

     if (BegCreateDate != "" && EndCreateDate != "")
     {
         strSql += " and CONVERT(varchar(100), CreateDate, 111) Between @BegCreateDate And @EndCreateDate\n";
     }

     if (City != "")
     {
         strSql += " and City = @City\n";
     }
     if (ZipCode != "")
     {
         strSql += " And ZipCode=@ZipCode\n";
     }

   
     if (Address != "")
     {
         strSql += " and a.Address like @Address\n";
     }
     if (ConsultantItem != "")
     {
         strSql += " And ConsultantItem like @ConsultantItem\n";
     }
     //if (ServiceUser != "")
     //{
     //    strSql += " and ServiceUser like @ServiceUser\n";
     //}
     if (EditServiceUser != "sys")
     {
         if (EditServiceUser != "")
         {
             strSql += " and ServiceUser=@ServiceUser\n";
         }
     }
     strSql += " Order by CreateDate desc";
      Dictionary<string, object> dict = new Dictionary<string, object>();
      dict.Add("CName", "%" + CName + "%");
      dict.Add("Phone", "%" + Phone + "%");
      dict.Add("Event", "%" + Event + "%");
      dict.Add("Marry", "%" + Marry + "%");
      dict.Add("BegCreateDate", "%" + BegCreateDate + "%");
      dict.Add("EndCreateDate", EndCreateDate);
      dict.Add("City", City);
      dict.Add("ZipCode", ZipCode);
      dict.Add("Address", "%" + Address + "%");
      dict.Add("ConsultantItem", "%" + ConsultantItem + "%");
        //dict.Add("ServiceUser",  ServiceUser );
      dict.Add("ServiceUser",  EditServiceUser);

      DataTable dt = NpoDB.GetDataTableS(strSql, dict);
      string Uid = Request.Form["Uid"];
      //********************************************************************************************
      //自訂欄位
      NPOGridView npoGridView = new NPOGridView();
      //來源種類
      npoGridView.Source = NPOGridViewDataSource.fromDataTable;
      //使用的 DataTable
      npoGridView.dataTable = dt;
      //不要顯示的欄位(可以多個)

      npoGridView.TableID = "tblDemo";

      npoGridView.DisableColumn.Add("uid");
      npoGridView.EditLinkTarget = "','target=_blank,help=no,status=yes,resizable=yes,scrollbars=yes,scrolling=yes,scroll=yes,center=yes,width=900px,height=500px";
      npoGridView.EditLink = Util.RedirectByTime("ConsultEdit2.aspx", "ConsultingUID=");

      //使用的 key 欄位,用在組成 QueryString 的 key 值(EditLink 會用到)
      npoGridView.Keys.Add("uid");
      NPOGridViewColumn col;
      col = new NPOGridViewColumn("姓名"); //產生所需欄位及設定 Title
      col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
      col.ColumnName.Add("姓名"); //使用欄位名稱
      col.CellStyle = "width:100px";
      // col.Link = Util.RedirectByTime("ConsultEdit2.aspx", "ConsultingUID=");
      npoGridView.Columns.Add(col);
      //----------------------------------------
      //  npoGridView.Keys.Add("uid");

      col = new NPOGridViewColumn("備註"); //產生所需欄位及設定 Title
      col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
      col.ColumnName.Add("備註"); //使用欄位名稱
      col.Link = Util.RedirectByTime("ConsultEdit2.aspx", "ConsultingUID=");
      col.CellStyle = "width:200px";
      npoGridView.Columns.Add(col);
      //----------------------------------------------
      col = new NPOGridViewColumn("電話"); //產生所需欄位及設定 Title
      col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
      col.ColumnName.Add("電話"); //使用欄位名稱
      col.Link = Util.RedirectByTime("ConsultEdit2.aspx", "ConsultingUID=");
      col.CellStyle = "width:150px";
      npoGridView.Columns.Add(col);
      // lblGridList.Text = npoGridView.Render();
      //----------------------------------------------------------
      col = new NPOGridViewColumn("建檔時間"); //產生所需欄位及設定 Title
      col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
      col.ColumnName.Add("建檔時間"); //使用欄位名稱
      col.Link = Util.RedirectByTime("ConsultEdit2.aspx", "ConsultingUID=");
      col.CellStyle = "width:150px";
      npoGridView.Columns.Add(col);
      //  lblGridList.Text = npoGridView.Render();
      //----------------------------------------------------------
      col = new NPOGridViewColumn("來電諮詢別(大類)"); //產生所需欄位及設定 Title
      col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
      col.ColumnName.Add("來電諮詢別(大類)"); //使用欄位名稱
      col.Link = Util.RedirectByTime("ConsultEdit2.aspx", "ConsultingUID=");
      col.CellStyle = "width:200px";
      npoGridView.Columns.Add(col);
      //lblGridList.Text = npoGridView.Render();
      ////----------------------------------------------------------
      col = new NPOGridViewColumn("來電諮詢別(分項)"); //產生所需欄位及設定 Title
      col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
      col.ColumnName.Add("來電諮詢別(分項)"); //使用欄位名稱
      col.Link = Util.RedirectByTime("ConsultEdit2.aspx", "ConsultingUID=");
      col.CellStyle = "width:200px";
      npoGridView.Columns.Add(col);
      //********************************************************************************************
      return npoGridView.Render();
    
    }

    private string LoadMainDefault()
    {
        string ServiceUser = Request.Form["ServiceUser"].ToString().TrimEnd(',');
        string GroupId = Request.Form["GroupID"].ToString();
        string strSql = @"
                        select  a.uid as uid, CName As  姓名,b.Memo as 備註,Phone As 電話, convert(varchar(19), CreateDate,120) as 建檔時間, ConsultantMain as [來電諮詢別(大類)], ConsultantItem  as [來電諮詢別(分項)] 
                        from Consulting a  inner join Member b on a.MemberUID = b.uid
                        where 1=1 
                        and isnull(a.IsDelete, '') != 'Y'  and isnull(b.IsDelete, '') != 'Y'
                         And
                        (((ISNULL(Overseas,'')='' AND ISNULL(city,'')=''))
                        or ((ISNULL(HarassPhone,'') <> '' AND   ISNULL(Special,'') <>'')  AND  (ISNULL(ConsultantMain,'')='' or isnull(ConsultantItem,'')=''))
                        OR ((ISNULL(HarassPhone,'') = '' AND ISNULL(Special,'')='')  AND  (ISNULL(ConsultantMain,'')='' or isnull(ConsultantItem,'')=''))
                        OR( ISNULL(CName,'')='' OR ISNULL(sex,'')='' OR ISNULL(Marry,'')='' OR ISNULL(Christian,'')='' OR ISNULL(Event,'')='' OR  ISNULL(CreateDate,'')='' OR  ISNULL(EndDate,'')='') )
                        ";
        if (GroupId != "1")
        {
            strSql += " And ServiceUser=@ServiceUser ";
        }
             strSql += " Order by CreateDate desc";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("ServiceUser", ServiceUser);
       
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
       
        int count = dt.Rows.Count;
        string Uid = Request.Form["Uid"];
        //Grid initial
        //沒有需要特別處理的欄位時
        NPOGridView npoGridView = new NPOGridView();
        //npoGridView.Source = NPOGridViewDataSource.fromDataTable;
        //npoGridView.dataTable = dt;
        //npoGridView.Keys.Add("uid");
        //npoGridView.DisableColumn.Add("uid");
        ////npoGridView.CurrentPage = Util.String2Number("21");//HFD_CurrentPage.Value
        ////Response.Write(@"<script>window.parent.location.href='ConsultEdit2.aspx';</script>");
        //npoGridView.EditLinkTarget = "','target=_blank,help=no,status=yes,resizable=yes,scrollbars=yes,scrolling=yes,scroll=yes,center=yes,width=900px,height=500px";
        //npoGridView.EditLink = Util.RedirectByTime("../CaseMgr/ConsultEdit2.aspx", "ConsultingUID=");
        //********************************************************************************************
        //自訂欄位
        //NPOGridView npoGridView = new NPOGridView();
        //來源種類
        npoGridView.Source = NPOGridViewDataSource.fromDataTable;
        //使用的 DataTable
        npoGridView.dataTable = dt;
        //不要顯示的欄位(可以多個)

        npoGridView.DisableColumn.Add("uid");
        npoGridView.EditLinkTarget = "','target=_blank,help=no,status=yes,resizable=yes,scrollbars=yes,scrolling=yes,scroll=yes,center=yes,width=900px,height=500px";
        npoGridView.EditLink = Util.RedirectByTime("../CaseMgr/ConsultEdit2.aspx", "ConsultingUID=");

        //使用的 key 欄位,用在組成 QueryString 的 key 值(EditLink 會用到)
        npoGridView.Keys.Add("uid");
        NPOGridViewColumn col;
        col = new NPOGridViewColumn("姓名"); //產生所需欄位及設定 Title
        col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
        col.ColumnName.Add("姓名"); //使用欄位名稱
        col.CellStyle = "width:100px";
        npoGridView.Columns.Add(col);

        col = new NPOGridViewColumn("備註"); //產生所需欄位及設定 Title
        col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
        col.ColumnName.Add("備註"); //使用欄位名稱
        col.Link = Util.RedirectByTime("../CaseMgr/ConsultEdit2.aspx", "ConsultingUID=");
        col.CellStyle = "width:200px";
        npoGridView.Columns.Add(col);
        //----------------------------------------------
        col = new NPOGridViewColumn("電話"); //產生所需欄位及設定 Title
        col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
        col.ColumnName.Add("電話"); //使用欄位名稱
        col.Link = Util.RedirectByTime("../CaseMgr/ConsultEdit2.aspx", "ConsultingUID=");
        col.CellStyle = "width:150px";
        npoGridView.Columns.Add(col);
        // lblGridList.Text = npoGridView.Render();
        //----------------------------------------------------------
        col = new NPOGridViewColumn("建檔時間"); //產生所需欄位及設定 Title
        col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
        col.ColumnName.Add("建檔時間"); //使用欄位名稱
        col.Link = Util.RedirectByTime("../CaseMgr/ConsultEdit2.aspx", "ConsultingUID=");
        col.CellStyle = "width:150px";
        npoGridView.Columns.Add(col);
        //----------------------------------------------------------
        col = new NPOGridViewColumn("來電諮詢別(大類)"); //產生所需欄位及設定 Title
        col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
        col.ColumnName.Add("來電諮詢別(大類)"); //使用欄位名稱
        col.Link = Util.RedirectByTime("../CaseMgr/ConsultEdit2.aspx", "ConsultingUID=");
        col.CellStyle = "width:200px";
        npoGridView.Columns.Add(col);
        //----------------------------------------------------------
        col = new NPOGridViewColumn("來電諮詢別(分項)"); //產生所需欄位及設定 Title
        col.ColumnType = NPOColumnType.NormalText; //設定為純顯示文字
        col.ColumnName.Add("來電諮詢別(分項)"); //使用欄位名稱
        col.Link = Util.RedirectByTime("../CaseMgr/ConsultEdit2.aspx", "ConsultingUID=");
        col.CellStyle = "width:200px";
        npoGridView.Columns.Add(col);
              
        //********************************************************************************************
        return npoGridView.Render();
    }
}

