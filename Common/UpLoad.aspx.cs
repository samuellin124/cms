using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

//�{����HEADSCRIPT�n��o�@�q
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
//�{������l�X�̭��n��o�@�q
//ObjectID �� ���x�s�ɮ׸�ƪ�uid
//allowtype �����\�W�Ǯ榡�Gdoc img all
//<asp:Button ID="btnUploadPic" runat="server" class="npoButton npoButton_Upload" Text="�W��"
//     OnClientClick="openUploadWindow('YourAPPName');return false;" />

public partial class Common_UpLoad : BasePage 
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
            Util.ShowMsg("�п���ɮ�!");
            return;
        }
        UpLoadObject objUpload = new UpLoadObject(this.FileUpload);
        objUpload.DenyMbSize = 10;
        //�ɦW�q�L
        if (objUpload.FileNameValid() == false)
        {
            Util.ShowMsg("�ɮצW�٤��঳�ť�!");
            return;
        }
        //�ɮפj�p�Ϋ��A�����\�W��
        if (!objUpload.AllowSave())
        {
            Util.ShowMsg(objUpload.ErrorMssage);
            return;
        }
        //�P�_�O�_���i�W�Ǫ��ɫ�
        if (this.HFD_AllowType.Value == "img" && objUpload.IsPicture() == false)
        {
            Util.ShowMsg("���������ɮ榡!");
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
        objUpload.UploadFileFolderPath = Server.MapPath("~" + HFD_UploadPath.Value);//�W�Ǹ��|
        objUpload.fileName = HFD_ApName.Value + objUpload.fileName;  //�W���ɮצW�� 
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
            Session["Msg"] = "���ɤW�Ǧ��\!";
            RegisterStartupScript("js", "<script language='javascript'>opener.window.location.reload();window.close();</script>");
        }
        else
        {
            ShowSysMsg("���ɤW�ǥ���!");
        }
    }
    //------------------------------------------------------------------------------
}
