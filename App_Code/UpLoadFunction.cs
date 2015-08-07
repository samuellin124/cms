using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for UpLoadObject
/// </summary>
public class UpLoadObject
{
    public FileUpload file;
    public int DenyMbSize;//單位MB
    public string UploadFileFolderPath;
    public string fileName;
    public int fileSize;    
    /// <summary>
    /// 使用者檔案完整路徑
    /// </summary>
    public string filePath;//使用者檔案完整路徑 
    public string ErrorMssage;
    public int ErrorNo=0;
    //****ErrorNo*****
    /* Error = 1 沒有檔案
     * Error = 2 檔案為不允許上傳檔案類型
     * Error = 3 檔案大小不得超過 + DenyMbSize + MB
     * Error = 4 檔案名稱不能有空白 
     * Error = 100 Exception Error
     * *************/
    public bool HasFile()
    {
        return file.HasFile;
    }
	public UpLoadObject(FileUpload objfile)
	{
        DenyMbSize = 10;//預設10MB
        file = objfile;
        if (!HasFile())
        {
            filePath = "";
            fileName = "";
            fileSize = 0;
            ErrorMssage = "沒有檔案";
            ErrorNo = 1; 
        }
        else
        {
            filePath = file.FileName;
            fileName = DateTime.Now.Ticks.ToString() + "_" + filePath.Substring(filePath.LastIndexOf(@"\") + 1);
            fileSize = file.FileBytes.Length;
        }
	}
    //取得副檔名
    public string GetExtension()
    {
        return System.IO.Path.GetExtension(file.PostedFile.FileName).ToUpper();
    }
    //檢查及限定上傳檔案類型與大小
    public bool AllowSave()
    {
        bool flag = false;
        //判斷副檔名
        switch (GetExtension())
        {
            case ".DOC":
            case ".DOCX":
            case ".XLS":
            case ".XLSX":
            case ".PPT":
            case ".PPTX":
            case ".TXT":
            case ".JPG":
            case ".JPEG":
            case ".BMP":
            case ".PNG":
            case ".PDF":
            case ".GIF":
            case ".ZIP":
            case ".RAR":
                flag = true;
                break;
            default:
                flag = false;
                ErrorMssage = "檔案為不允許上傳檔案類型";
                ErrorNo = 2;
                break;
        }
        if (GetFileSizeMB() > DenyMbSize)
        {
            ErrorMssage = "檔案大小不得超過" + DenyMbSize + "MB";
            ErrorNo = 3;
            flag = false;
        }
        if (FileNameValid()==false)
        {
            ErrorMssage = "檔案名稱不能有空白";
            ErrorNo = 4;
            flag = false;
        }
        return flag;
    }
    //檢查是否為可接受的圖檔類型
    public bool IsPicture()
    {
        bool flag = false;
        //判斷副檔名
        switch (GetExtension())
        {
            case ".JPG":
            case ".JPEG":
            case ".BMP":
            case ".PNG":
            case ".GIF":
                flag = true;
                break;
            default:
                flag = false;
                break;
        }
        return flag;
    }
    //是否為文件類型（非圖檔）
    public bool IsDocFile()
    {
        return !IsPicture();
    }
    //取得單位為KB的檔案Size
    private double GetFileSizeKB()
    {
        return fileSize / 1024;
    }
    //取得單位為MB的檔案Size
    private double GetFileSizeMB()
    {
        return fileSize / 1048576;
    }
    //處理上傳檔案
    public bool FileUpLoad()
    {
        string UploadFilePath;

        if (!AllowSave())
            return false ;
        
        UploadFilePath = UploadFileFolderPath + fileName;
        try
        {
            file.SaveAs(UploadFilePath);
            return true;
        }
        catch (Exception ex)
        {
            ErrorNo = 100;
            ErrorMssage = ex.Message;
            return false;
        }
    }
    //判斷檔名是否有空白
    public bool FileNameValid()
    {
        string strfilePath = file.FileName;
        string strfileName = strfilePath.Substring(strfilePath.LastIndexOf(@"\") + 1);
        if (strfileName.Contains(" ") || strfileName.Contains("　"))
        {
            return false;
        }
        return true;
    }
}
