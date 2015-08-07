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
    public int DenyMbSize;//���MB
    public string UploadFileFolderPath;
    public string fileName;
    public int fileSize;    
    /// <summary>
    /// �ϥΪ��ɮק�����|
    /// </summary>
    public string filePath;//�ϥΪ��ɮק�����| 
    public string ErrorMssage;
    public int ErrorNo=0;
    //****ErrorNo*****
    /* Error = 1 �S���ɮ�
     * Error = 2 �ɮ׬������\�W���ɮ�����
     * Error = 3 �ɮפj�p���o�W�L + DenyMbSize + MB
     * Error = 4 �ɮצW�٤��঳�ť� 
     * Error = 100 Exception Error
     * *************/
    public bool HasFile()
    {
        return file.HasFile;
    }
	public UpLoadObject(FileUpload objfile)
	{
        DenyMbSize = 10;//�w�]10MB
        file = objfile;
        if (!HasFile())
        {
            filePath = "";
            fileName = "";
            fileSize = 0;
            ErrorMssage = "�S���ɮ�";
            ErrorNo = 1; 
        }
        else
        {
            filePath = file.FileName;
            fileName = DateTime.Now.Ticks.ToString() + "_" + filePath.Substring(filePath.LastIndexOf(@"\") + 1);
            fileSize = file.FileBytes.Length;
        }
	}
    //���o���ɦW
    public string GetExtension()
    {
        return System.IO.Path.GetExtension(file.PostedFile.FileName).ToUpper();
    }
    //�ˬd�έ��w�W���ɮ������P�j�p
    public bool AllowSave()
    {
        bool flag = false;
        //�P�_���ɦW
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
                ErrorMssage = "�ɮ׬������\�W���ɮ�����";
                ErrorNo = 2;
                break;
        }
        if (GetFileSizeMB() > DenyMbSize)
        {
            ErrorMssage = "�ɮפj�p���o�W�L" + DenyMbSize + "MB";
            ErrorNo = 3;
            flag = false;
        }
        if (FileNameValid()==false)
        {
            ErrorMssage = "�ɮצW�٤��঳�ť�";
            ErrorNo = 4;
            flag = false;
        }
        return flag;
    }
    //�ˬd�O�_���i��������������
    public bool IsPicture()
    {
        bool flag = false;
        //�P�_���ɦW
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
    //�O�_����������]�D���ɡ^
    public bool IsDocFile()
    {
        return !IsPicture();
    }
    //���o��쬰KB���ɮ�Size
    private double GetFileSizeKB()
    {
        return fileSize / 1024;
    }
    //���o��쬰MB���ɮ�Size
    private double GetFileSizeMB()
    {
        return fileSize / 1048576;
    }
    //�B�z�W���ɮ�
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
    //�P�_�ɦW�O�_���ť�
    public bool FileNameValid()
    {
        string strfilePath = file.FileName;
        string strfileName = strfilePath.Substring(strfilePath.LastIndexOf(@"\") + 1);
        if (strfileName.Contains(" ") || strfileName.Contains("�@"))
        {
            return false;
        }
        return true;
    }
}
