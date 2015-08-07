using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic ;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;

public partial class FileMgr_FileUpLoad_Edit :BasePage 
{
    //folderPath 讀取Global.asax 中的 Session["FolderPath"]，以網址型式
    private string folderPath;//記錄檔案存檔目錄
    private string fileName;//記錄檔案名稱
    private int fileSize;//記錄檔案大小

    protected void Page_Load(object sender, EventArgs e)
    {
        FD_ShowTip.Text = "注意：上傳檔案類型只能是以下幾種<br>文件檔、Office文件或PDF：\".DOC\", \".PPT\", \".XLS\", \".TXT\", \".PDF\"<br>圖形檔：\".JPG\", \".JPEG\", \".BMP\", \".PNG\", \".GIF\"";

        folderPath = Util.GetAppSetting("DocUploadPath");
        if (!IsPostBack)
        {
            HFD_UPLOAD_ID.Value = Util.GetQueryString("upload_id");
            if (HFD_UPLOAD_ID.Value == "")
            {
                Response.Redirect("FileCatas.aspx");
            }
            Form_DataBind();
        }

    }
    //---------------------------------------------------------------------------
    public void Form_DataBind()
    {
        string strSql, upload_id;
        DataTable dt;
        Dictionary<string, object> dict = new Dictionary<string, object>();

        upload_id = HFD_UPLOAD_ID.Value;

        strSql = " select * from uploads where upload_id=@upload_id";
        dict.Add("upload_id", upload_id);
        dt = NpoDB.GetDataTableS(strSql, dict);
        //資料異常
        if (dt.Rows.Count == 0)
        {
            Response.Redirect("uploads.aspx");
        }
        DataRow dr = dt.Rows[0];
        FD_Upload_Desc.Text = dr["upload_note"].ToString();
    }
    //---------------------------------------------------------------------------
    protected void btnSave_Click(object sender, EventArgs e)
    {
        //沒有上傳檔案，則只修改說明
        if (!FileUpload1.HasFile)
        {
            SaveDesc();
            return;
        }
        if (FileNameValid() == false)
            return;
        if (!AllowSave())
            return;

        string strSql, upload_id;
        string filecat_id, upload_filename, upload_by;
        string upload_note;
        int upload_filesize;
        DataTable dt;
        Dictionary<string, object> dict = new Dictionary<string, object>();

        string filePath;
        filePath = FileUpload1.FileName;
        fileName = DateTime.Now.Ticks.ToString() + "_" + filePath.Substring(filePath.LastIndexOf(@"\") + 1);
        fileSize = FileUpload1.FileBytes.Length;
        upload_id = HFD_UPLOAD_ID.Value ;
        upload_note = FD_Upload_Desc.Text ;
        upload_by = SessionInfo.UserName;
        upload_filename = fileName;
        upload_filesize = fileSize;
        if (upload_note == "")
        {
            upload_note = filePath.Substring(filePath.LastIndexOf(@"\") + 1);
        }

        strSql = "select * from uploads where upload_id=@upload_id";

        dict.Add("upload_id", upload_id);

        dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count == 0)
        {
            Response.Redirect("FileCats.aspx");
        }
        //取得舊檔資訊
        DataRow dr = dt.Rows[0];
        filecat_id = dr["AppObject_ID"].ToString();
        string old_upload_filename = dr["Upload_FileName"].ToString();
        //刪除舊檔案
        DeleteFile(old_upload_filename);
        //處理上傳資料 
        FileUpLoad();

        strSql = " update uploads set Upload_FileName=@upload_filename,Upload_FileSize=@upload_filesize,Upload_By=@upload_by,Upload_Note=@upload_note where Upload_ID=@upload_id;";
        strSql += " update FileCats set FileCat_UpdateDate=getdate(),FileCat_UpdateBy=@upload_by where FileCat_ID=@filecat_id ";
        dict.Add("upload_filename", upload_filename);
        dict.Add("upload_filesize", upload_filesize);
        dict.Add("upload_by", upload_by);
        dict.Add("upload_note", upload_note);
        dict.Add("filecat_id", filecat_id);

        NpoDB.ExecuteSQLS(strSql, dict);
        Session["Msg"] = "檔案上傳更新成功 !";
        string LocationHref = "FileCats.aspx?FileCat_id=" + filecat_id;
        RegisterStartupScript("js", @"<script language='javascript'>opener.window.location.href='" + LocationHref + @"';window.close();</script>");
    }
    //---------------------------------------------------------------------------
    public void SaveDesc()
    {
        string strSql, upload_id;
        string filecat_id,  upload_by;
        string upload_note;
        DataTable dt;
        Dictionary<string, object> dict = new Dictionary<string, object>();

        upload_id = HFD_UPLOAD_ID.Value;
        upload_note = FD_Upload_Desc.Text;
        upload_by = SessionInfo.UserName;

        strSql = "select * from uploads where upload_id=@upload_id";
        dict.Add("upload_id", upload_id);
        dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count == 0)
        {
            Response.Redirect("FileCats.aspx");
        }
        //取得舊檔資訊
        DataRow dr = dt.Rows[0];
        filecat_id = dr["AppObject_ID"].ToString();
        string old_upload_note = dr["Upload_Note"].ToString();

        if (upload_note == "")
        {
            upload_note = old_upload_note;
        }
        strSql = " update uploads set Upload_By=@upload_by,Upload_Note=@upload_note where Upload_ID=@upload_id;";
        strSql += " update FileCats set FileCat_UpdateDate=getdate(),FileCat_UpdateBy=@upload_by where FileCat_ID=@filecat_id ";
        dict.Add("upload_by", upload_by);
        dict.Add("upload_note", upload_note);
        dict.Add("filecat_id", filecat_id);

        NpoDB.ExecuteSQLS(strSql, dict);
        Session["Msg"] = "檔案說明更新成功 !";
        string LocationHref = "FileCats.aspx?FileCat_id=" + filecat_id;
        RegisterStartupScript("js", @"<script language='javascript'>opener.window.location.href='" + LocationHref + @"';window.close();</script>");
    }
    //---------------------------------------------------------------------------
    //檢查及限定上傳檔案類型與大小
    public bool AllowSave()
    {
        int DenyMbSize = 10;//限制大小3MB
        bool flag = false;
        //判斷副檔名
        switch (System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName).ToUpper())
        {
            case ".DOC":
            case ".PPT":
            case ".XLS":
            case ".TXT":
            case ".JPG":
            case ".JPEG":
            case ".BMP":
            case ".PNG":
            case ".PDF":
            case ".GIF":
                flag = true;
                break;
            default:
                ShowSysMsg("此檔案類型不允許上傳");
                return false;
                break;
        }
        if (GetFileSizeMB(FileUpload1.FileBytes.Length) > DenyMbSize)
        {
            ShowSysMsg("檔案大小不得超過" + DenyMbSize + "MB");
            return false;
        }
        return true;
    }
    //---------------------------------------------------------------------------
    //取得單位為KB的檔案Size
    public double GetFileSizeKB(int intLength)
    {
        int intKbSize = 1024;
        return intLength / intKbSize;
    }
    //---------------------------------------------------------------------------
    //取得單位為MB的檔案Size
    public double GetFileSizeMB(int intLength)
    {
        int intMbSize = 1048576;
        return intLength / intMbSize;
    }
    //---------------------------------------------------------------------------
    //處理上傳檔案
    public void FileUpLoad()
    {
        string UploadFileFolderPath, UploadFilePath;
        string filePath;
        filePath = FileUpload1.FileName;

        UploadFileFolderPath = Server.MapPath("~" + folderPath);
        fileSize = FileUpload1.FileBytes.Length;
        UploadFilePath = UploadFileFolderPath + fileName;

        FileUpload1.SaveAs(UploadFilePath);        
    }
    //---------------------------------------------------------------------------
    //刪除舊檔案
    public void DeleteFile(string OldFileName)
    {
        string UploadFileFolderPath, UploadFilePath;
        UploadFileFolderPath = Server.MapPath("~" + folderPath);
        UploadFilePath = UploadFileFolderPath + OldFileName ;

        File.Delete(UploadFilePath);
    }
    //---------------------------------------------------------------------------
    public bool FileNameValid()
    {
        string strfilePath = FileUpload1.FileName;
        string strfileName = strfilePath.Substring(strfilePath.LastIndexOf(@"\") + 1);
        if (strfileName.Contains(" ") || strfileName.Contains("　"))
        {
            ShowSysMsg("檔案名稱不能有空白");
            return false;
        }
        return true;
    }
    //---------------------------------------------------------------------------
}
