using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

//程式的HEADSCRIPT要放這一段
//    <script type="text/javascript"  language="javascript">
//        function openUploadWindow(ApName) {
//            var ServiceAdvisoryUID = document.getElementById('HFD_ServiceAdvisoryUID').value;
//            window.open('../Common/UpLoad.aspx?ObjectID=' + ServiceAdvisoryUID +
//                                              '&ApName=' + ApName + 
//                                              '&AllowType=img',
//                        'upload', 
//                        'scrollbars=no,status=no,toolbar=no,top=100,left=120,width=560,height=120');
//        }
//    </script>
//程式的原始碼裡面要放這一段
//ObjectID 為 有儲存檔案資料表的uid
//allowtype 為允許上傳格式：doc img all
//<asp:Button ID="btnUploadPic" runat="server" class="npoButton npoButton_Upload" Text="上傳"
//     OnClientClick="openUploadWindow('YourAPPName');return false;" />

public partial class Common_UpLoadCaseID : BasePage 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HFD_UploadPath.Value = Util.GetAppSetting("UploadPath");

        if (!IsPostBack)
        {
            KeyWordSet();
        }
    }
    //------------------------------------------------------------------------------
    public void KeyWordSet()
    {
        HFD_ObjectID.Value = Util.GetQueryString("ObjectID");
        HFD_ApName.Value = Util.GetQueryString("ApName");
        HFD_AllowType.Value = Util.GetQueryString("AllowType");
    }
    //------------------------------------------------------------------------------
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!FileUpload.HasFile)
        {
            Util.ShowMsg("請選擇檔案!");
            return;
        }
        UpLoadObject objUpload = new UpLoadObject(this.FileUpload);
        objUpload.DenyMbSize = 10;
        //檔名通過
        if (objUpload.FileNameValid() == false)
        {
            Util.ShowMsg("檔案名稱不能有空白!");
            return;
        }
        //檔案大小或型態不允許上傳
        if (!objUpload.AllowSave())
        {
            Util.ShowMsg(objUpload.ErrorMssage);
            return;
        }
        //判斷是否為可上傳的檔型
        if (this.HFD_AllowType.Value == "img" && objUpload.IsPicture() == false)
        {
            Util.ShowMsg("必須為圖檔格式!");
            return;
        }

        string AttachType;
        if (objUpload.IsPicture())
        {
            AttachType = "img";
        }
        else
        {
            AttachType = "doc";
        }
        string strSql;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        objUpload.UploadFileFolderPath = Server.MapPath("~" + HFD_UploadPath.Value);//上傳路徑
        objUpload.fileName = HFD_ApName.Value + objUpload.fileName;  //上傳檔案名稱 
        objUpload.FileUpLoad();

        string UploadFileURL = HFD_UploadPath.Value + objUpload.fileName;
        string UploadDate = Util.DateTime2String(DateTime.Now, DateType.yyyyMMddHHmmss, EmptyType.ReturnNull);

        strSql = " Insert into uploadCaseID\n";
        strSql += "( ObjectID, ApName, AttachType, FileDesc, UploadFileURL, UploadDate) values \n";
        strSql += "(@ObjectID,@ApName,@AttachType,@FileDesc,@UploadFileURL,@UploadDate) \n";

        dict.Add("ObjectID", HFD_ObjectID.Value);
        dict.Add("ApName", HFD_ApName.Value);
        dict.Add("AttachType", AttachType);
        dict.Add("FileDesc", txtFileDesc.Text);
        dict.Add("UploadFileURL", UploadFileURL);
        dict.Add("UploaDdate", UploadDate);

        int effect = NpoDB.ExecuteSQLS(strSql, dict);

        //若是開案及年度訪視表則須更新基本資料之圖檔
        if (HFD_ApName.Value == "CaseVisit" || HFD_ApName.Value == "CaseVisitYear")
        {
            string strRet = Util.GetDBValue("uploadCaseID", "Uid", "ObjectID"
                , HFD_ObjectID.Value.Substring(0, 8), "ApName", "FamilyTree");
            //若已上傳家系圖，則更新，反之則新增
            if (strRet != "")
            {
                strSql = @"update uploadCaseID set
                        UploadFileURL=@UploadFileURL
                        ,UploadDate=@UploadDate 
                        where ObjectID=@ObjectID 
                        and ApName=@ApName 
                        ";
                dict.Clear();
                dict.Add("ObjectID", HFD_ObjectID.Value.Substring(0, 8));
                dict.Add("ApName", "FamilyTree");
                dict.Add("UploadFileURL", UploadFileURL);
                dict.Add("UploaDdate", UploadDate);
                int iRet = NpoDB.ExecuteSQLS(strSql, dict);
            }
            else
            {
                strSql = " Insert into uploadCaseID\n";
                strSql += "( ObjectID, ApName, AttachType, FileDesc, UploadFileURL, UploadDate) values \n";
                strSql += "(@ObjectID,@ApName,@AttachType,@FileDesc,@UploadFileURL,@UploadDate) \n";

                dict.Clear();
                dict.Add("ObjectID", HFD_ObjectID.Value.Substring(0, 8));
                dict.Add("ApName", "FamilyTree");
                dict.Add("AttachType", AttachType);
                dict.Add("FileDesc", txtFileDesc.Text);
                dict.Add("UploadFileURL", UploadFileURL);
                dict.Add("UploaDdate", UploadDate);

                int iRet = NpoDB.ExecuteSQLS(strSql, dict);

            }
        }

        if (effect == 1)
        {
            Session["Msg"] = "圖檔上傳成功!";
            this.ClientScript.RegisterStartupScript(this.GetType(), "js", "<script language='javascript'>opener.window.location.reload();window.close();</script>");
        }
        else
        {
            ShowSysMsg("圖檔上傳失敗!");
        }
    }
    //------------------------------------------------------------------------------
}
