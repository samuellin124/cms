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
    //folderPath Ū��Global.asax ���� Session["FolderPath"]�A�H���}����
    private string folderPath;//�O���ɮצs�ɥؿ�
    private string fileName;//�O���ɮצW��
    private int fileSize;//�O���ɮפj�p

    protected void Page_Load(object sender, EventArgs e)
    {
        FD_ShowTip.Text = "�`�N�G�W���ɮ������u��O�H�U�X��<br>����ɡBOffice����PDF�G\".DOC\", \".PPT\", \".XLS\", \".TXT\", \".PDF\"<br>�ϧ��ɡG\".JPG\", \".JPEG\", \".BMP\", \".PNG\", \".GIF\"";

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
        //��Ʋ��`
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
        //�S���W���ɮסA�h�u�קﻡ��
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
        //���o���ɸ�T
        DataRow dr = dt.Rows[0];
        filecat_id = dr["AppObject_ID"].ToString();
        string old_upload_filename = dr["Upload_FileName"].ToString();
        //�R�����ɮ�
        DeleteFile(old_upload_filename);
        //�B�z�W�Ǹ�� 
        FileUpLoad();

        strSql = " update uploads set Upload_FileName=@upload_filename,Upload_FileSize=@upload_filesize,Upload_By=@upload_by,Upload_Note=@upload_note where Upload_ID=@upload_id;";
        strSql += " update FileCats set FileCat_UpdateDate=getdate(),FileCat_UpdateBy=@upload_by where FileCat_ID=@filecat_id ";
        dict.Add("upload_filename", upload_filename);
        dict.Add("upload_filesize", upload_filesize);
        dict.Add("upload_by", upload_by);
        dict.Add("upload_note", upload_note);
        dict.Add("filecat_id", filecat_id);

        NpoDB.ExecuteSQLS(strSql, dict);
        Session["Msg"] = "�ɮפW�ǧ�s���\ !";
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
        //���o���ɸ�T
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
        Session["Msg"] = "�ɮ׻�����s���\ !";
        string LocationHref = "FileCats.aspx?FileCat_id=" + filecat_id;
        RegisterStartupScript("js", @"<script language='javascript'>opener.window.location.href='" + LocationHref + @"';window.close();</script>");
    }
    //---------------------------------------------------------------------------
    //�ˬd�έ��w�W���ɮ������P�j�p
    public bool AllowSave()
    {
        int DenyMbSize = 10;//����j�p3MB
        bool flag = false;
        //�P�_���ɦW
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
                ShowSysMsg("���ɮ����������\�W��");
                return false;
                break;
        }
        if (GetFileSizeMB(FileUpload1.FileBytes.Length) > DenyMbSize)
        {
            ShowSysMsg("�ɮפj�p���o�W�L" + DenyMbSize + "MB");
            return false;
        }
        return true;
    }
    //---------------------------------------------------------------------------
    //���o��쬰KB���ɮ�Size
    public double GetFileSizeKB(int intLength)
    {
        int intKbSize = 1024;
        return intLength / intKbSize;
    }
    //---------------------------------------------------------------------------
    //���o��쬰MB���ɮ�Size
    public double GetFileSizeMB(int intLength)
    {
        int intMbSize = 1048576;
        return intLength / intMbSize;
    }
    //---------------------------------------------------------------------------
    //�B�z�W���ɮ�
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
    //�R�����ɮ�
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
        if (strfileName.Contains(" ") || strfileName.Contains("�@"))
        {
            ShowSysMsg("�ɮצW�٤��঳�ť�");
            return false;
        }
        return true;
    }
    //---------------------------------------------------------------------------
}
