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
using System.IO ;
/******************************************************
程式的HEADSCRIPT要放這一段
 * <script type="text/javascript"  language="javascript">
     function openUploadWindow()
     {
        window.open("FileUpLoad.aspx?Ap_Name=UpLoad","upload","scrollbars=no,status=no,toolbar=no,top=100,left=120,width=580,height=200");
     }
</script>
程式的原始碼裡面要放這一段
<asp:Button runat="server" Text=" 上傳新檔案 " ID="btnNewFile" CssClass="cbutton" OnClientClick="openUploadWindow();return false;" />
******************************************************/

public partial class FileMgr_FileUpLoad : BasePage 
{
    //folderPath 讀取Global.asax 中的 Session["FolderPath"]，以網址型式
    private string folderPath;//記錄檔案存檔目錄
    private string fileName;//記錄檔案名稱
    private int fileSize;//記錄檔案大小

    protected void Page_Load(object sender, EventArgs e)
    {
        folderPath = Util.GetAppSetting("DocUploadPath");
        if (!IsPostBack)
        {
            Inital();
        }
    }
    //---------------------------------------------------------------------------
    public void Inital()
    {
        HFD_Ap_Name.Value = Util.GetQueryString("Ap_Name");
        HFD_dept_id.Value = Util.GetQueryString("dept_id");
        HFD_filecat_id.Value = Util.GetQueryString("filecat_id");
    }
    //---------------------------------------------------------------------------
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!FileUpload1.HasFile)
        {            
            //this.RegisterStartupScript("js", @"<script language='javascript'>alert('請選擇檔案');</script>");
            this.ClientScript.RegisterStartupScript(this.GetType(), "js", @"<script language='javascript'>alert('請選擇檔案');</script>");
            return;
        }
        if (FileNameValid() == false)
            return;
        if (!AllowSave())
            return;
        //****上傳資料處理****//
         FileUpLoad();


        //***********************************
        // Insert To DataBase
        //***********************************
        //****變數宣告****//
        string strSql;
        DataTable dt;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        string dept_id, app_code, filecat_id, upload_filename, upload_by;
        string upload_note;
        int upload_filesize ; 
        //****變數設定****//
        string filePath;
        filePath = FileUpload1.FileName;
        dept_id = this.HFD_dept_id.Value;
        app_code = "File";
        filecat_id = this.HFD_filecat_id.Value;
        upload_filename = this.fileName ;
        upload_filesize = this.fileSize;
        upload_by = SessionInfo.UserName;
        upload_note = this.FD_Upload_Desc.Text;
        if (upload_note == "")
        {
            upload_note = filePath.Substring(filePath.LastIndexOf(@"\") + 1);  
        }
        strSql = " insert into uploads(dept_id,app_code, appobject_id, upload_filename,upload_filesize,upload_by, upload_date, upload_note,download_count) ";
        strSql += " values (@dept_id,@app_code,@filecat_id,@upload_filename,@upload_filesize,@upload_by,getdate(),@upload_note,0) ;"; 
        strSql += " update FileCats set FileCat_UpdateDate=getdate(),FileCat_UpdateBy=@upload_by where FileCat_ID=@filecat_id ";

        dict.Add("dept_id", dept_id);
        dict.Add("app_code", app_code);
        dict.Add("filecat_id", filecat_id);
        dict.Add("upload_filename", upload_filename);
        dict.Add("upload_filesize", upload_filesize);
        dict.Add("upload_by", upload_by);
        dict.Add("upload_note", upload_note);

        NpoDB.ExecuteSQLS(strSql, dict);

        Session["Msg"] = "檔案上傳成功 !";
        string LocationHref = "FileCats.aspx?rand=" + DateTime.Now.Ticks.ToString() + "&dept_id="+dept_id+"&FileCat_id="+filecat_id;

        //this.RegisterStartupScript("js", @"<script language='javascript'>opener.window.location.href='" + LocationHref + @"';window.close();</script>");
        this.ClientScript.RegisterStartupScript(this.GetType(), "js", @"<script language='javascript'>opener.window.location.href='" + LocationHref + @"';window.close();</script>");
    }
    //---------------------------------------------------------------------------
    //檢查及限定上傳檔案類型與大小
    public bool AllowSave()
    {
        int DenyMbSize =10;//限制大小3MB
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
            default :
                //this.RegisterStartupScript("js", @"<script language='javascript' >alert('此檔案類型不允許上傳');</script>"); 
                this.ClientScript.RegisterStartupScript(this.GetType(), "js", @"<script language='javascript' >alert('此檔案類型不允許上傳');</script>"); 
                flag= false;
                break;
        }
        if (GetFileSizeMB(FileUpload1.FileBytes.Length ) > DenyMbSize)
        {
            //this.RegisterStartupScript("js", @"<script language='javascript' >alert('檔案大小不得超過" + DenyMbSize + @"MB');</script>");
            this.ClientScript.RegisterStartupScript(this.GetType(), "js", @"<script language='javascript' >alert('檔案大小不得超過" + DenyMbSize + @"MB');</script>");
            flag = false;
        }
        return flag;
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
        this.fileName = DateTime.Now.Ticks.ToString() + "_" + filePath.Substring(filePath.LastIndexOf(@"\") + 1);
        this.fileSize = FileUpload1.FileBytes.Length;
        UploadFilePath = UploadFileFolderPath + this.fileName;

        FileUpload1.SaveAs(UploadFilePath);        
    }
    //---------------------------------------------------------------------------
    public bool FileNameValid()
    {
        string strfilePath = FileUpload1.FileName;
        string strfileName =  strfilePath.Substring(strfilePath.LastIndexOf(@"\") + 1);
        if (strfileName.Contains(" ") || strfileName.Contains("　"))
        {
            //this.RegisterStartupScript("js", @"<script language='javascript' >alert('檔案名稱不能有空白');</script>");
            this.ClientScript.RegisterStartupScript(this.GetType(), "js", @"<script language='javascript' >alert('檔案名稱不能有空白');</script>");
            return false;
        }
        return true;
    }
    //---------------------------------------------------------------------------
}
