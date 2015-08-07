using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

public partial class Common_UploadAll : BasePage 
{
      private string folderPath;
    protected void Page_Load(object sender, EventArgs e)
    {
        //HFD_SubFolder.Value = Util.GetQueryString("SubFolder");

        HFD_UploadPath.Value = Util.GetAppSetting("UploadPath");
        //folderPath = Session["UploadPath"].ToString() + HFD_SubFolder.Value;

        if (!IsPostBack)
        {
            HFD_ObjectID.Value = Util.GetQueryString("ObjectID");
            HFD_ApName.Value = Util.GetQueryString("ApName");
            HFD_AllowType.Value = Util.GetQueryString("AllowType");
        }
    }
    //---------------------------------------------------------------------------------
    /*
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!FileUpload.HasFile)
        {
            RegisterStartupScript("js", @"<script language='javascript'>alert('請選擇檔案');</script>");
            return;
        }
        UpLoadObject objUpload = new UpLoadObject(FileUpload);
        objUpload.DenyMbSize = 10;
        //檔名通過
        if (objUpload.FileNameValid() == false)
        {
            RegisterStartupScript("js", @"<script language='javascript' >alert('檔案名稱不能有空白');</script>");
            return;
        }
        //檔案大小或型態不允許上傳
        if (!objUpload.AllowSave())
        {
            RegisterStartupScript("js", @"<script language='javascript' >alert('" + objUpload.ErrorMssage + "');</script>");
            return;
        }
        //判斷是否為可上傳的檔型
        //if (HFD_AllowType.Value == "img" && objUpload.IsPicture() == false )
        //{
        //    RegisterStartupScript("js", @"<script language='javascript' >alert('必須為圖檔格式');</script>");
        //    return;
        //}
        string AttachType = ""; ;
        string ObjectID = HFD_ObjectID.Value;
        string ApName = HFD_ApName.Value;
        string UploadDate = txtFileDesc.Text;
        if (objUpload.IsPicture())
        {
            AttachType = "img";
        }
        else
        {
            AttachType = "doc";
        }
        string UploadFileURL;
        Dictionary<string,object> dict = new Dictionary<string,object>();
        objUpload.UploadFileFolderPath = Server.MapPath("~" + folderPath);//上傳路徑
        objUpload.fileName = ApName + objUpload.fileName;  //上傳檔案名稱 
        objUpload.FileUpLoad();

        UploadFileURL = folderPath + objUpload.fileName;
        string strSql = @" Insert into upload
        ( ObjectID, ApName, AttachType, FileDesc, UploadFileURL, UploadDate) values
        (@ObjectID,@ApName,@AttachType,@FileDesc,@UploadFileURL,@UploadDate)
                       ";

        dict.Add("ObjectID", ObjectID);
        dict.Add("ApName", ApName);
        dict.Add("AttachType", AttachType);
        dict.Add("FileDesc", txtFileDesc.Text);
        dict.Add("UploadFileURL", UploadFileURL);
        dict.Add("UploadDate", Util.GetToday(DateType.yyyyMMddHHmmss));

        NpoDB.ExecuteSQLS(strSql, dict);
        Session["Msg"] = "圖檔上傳成功!";
        RegisterStartupScript("js", "<script language='javascript'>opener.window.location.reload();window.close();</script>");
    }
    */
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

        strSql = " Insert into upload\n";
        strSql += "( ObjectID, ApName, AttachType, FileDesc, UploadFileURL, UploadDate) values \n";
        strSql += "(@ObjectID,@ApName,@AttachType,@FileDesc,@UploadFileURL,@UploadDate) \n";

        dict.Add("ObjectID", HFD_ObjectID.Value);
        dict.Add("ApName", HFD_ApName.Value);
        dict.Add("AttachType", AttachType);
        dict.Add("FileDesc", txtFileDesc.Text);
        dict.Add("UploadFileURL", UploadFileURL);
        dict.Add("UploaDdate", UploadDate);

        int effect = NpoDB.ExecuteSQLS(strSql, dict);
        if (effect == 1)
        {
            Session["Msg"] = "圖檔上傳成功!";
            RegisterStartupScript("js", "<script language='javascript'>opener.window.location.reload();window.close();</script>");
        }
        else
        {
            ShowSysMsg("圖檔上傳失敗!");
        }
    }
    //------------------------------------------------------------------------------
}
