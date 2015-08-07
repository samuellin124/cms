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
using System.Text  ; 

//�ثe�ɮצW�٦p�G���Ʒ|�л\,�^�Y�n�ץ������Ͱߤ@�ʦW�٦b�ɮ׸̭����g�k
//�PFileCats��Uploads Table ����

#region Table Schema
/*
CREATE TABLE [dbo].[UPLOADS](
    [Upload_ID] [int] IDENTITY(1,1) NOT NULL,
    [dept_id] [char](4) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
    [App_Code] [varchar](40) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
    [AppObject_ID] [int] NULL,
    [Upload_FileName] [varchar](100) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
    [Upload_FileSize] [int] NULL,
    [Upload_By] [varchar](20) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
    [Upload_Date] [datetime] NULL,
    [Upload_Note] [varchar](200) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
    [Download_Count] [int] NULL,
 CONSTRAINT [PK_UPLOADS] PRIMARY KEY CLUSTERED 
(
    [Upload_ID] ASC
)WITH FILLFACTOR = 90 ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[FILECATS](
    [FileCat_ID] [int] IDENTITY(1,1) NOT NULL,
    [Dept_id] [char](4) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
    [FileCat_Name] [varchar](60) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
    [FileCat_ParentID] [int] NULL,
    [FileCat_AllAccess] [nvarchar](1) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
    [FileCat_SortID] [int] NULL,
    [FileCat_CreatedBy] [varchar](20) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
    [FileCat_CreatedDate] [datetime] NULL,
    [FileCat_UpdateBy] [varchar](20) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
    [FileCat_UpdateDate] [datetime] NULL,
 CONSTRAINT [PK_FILECATS] PRIMARY KEY CLUSTERED 
(
    [FileCat_ID] ASC
)WITH FILLFACTOR = 90 ON [PRIMARY]
) ON [PRIMARY]
*/
#endregion 

public partial class FileMgr_FileCats :BasePage 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ShowSysMsg();
        if (!IsPostBack)
        {
            Inital();
            DirectoryList_DataBind();
            FileList_DataBind();
        }
    }
    //-------------------------------------------------------------------------------
    public void Inital()
    {
        //�]�w��l��
        HFD_dept_id.Value = SessionInfo.DeptID;
        HFD_filecat_id.Value = Util.GetQueryString("filecat_id");
        if (HFD_filecat_id.Value == "")
        {
            HFD_filecat_id.Value = "0";
        }

        LINK_FileCats.NavigateUrl = "FileCats.aspx?dept_id=" + HFD_dept_id.Value;

        string strSql = "select FileCat_Name,FileCat_ParentID from FileCats where FileCat_ID=@filecat_id ";
        Dictionary<string,object> dict = new Dictionary<string,object>();
        DataTable dt ; 
        string filecat_id = HFD_filecat_id.Value  ;      
        dict.Add("filecat_id",filecat_id);
        dt = NpoDB.GetDataTableS(strSql, dict);

        if (dt.Rows.Count > 0)
        {
            lbl_FileCat_Parent.Text = @"<img border='0' src='../images/menu-sob.gif' width='20' height='20' align='absmiddle'><span style='font-size: 9pt; font-family: �s�ө���'>" + dt.Rows[0]["FileCat_Name"] + @"�E<img border='0' src='../images/Icon_Upfolder.gif' width='16' height='16' align='absmiddle'> <a href=""FileCats.aspx?dept_id=" + HFD_dept_id + "&FileCat_ID=" + dt.Rows[0]["FileCat_ParentID"] + @""">�^�W�@�h</a></span>";
        }
        else
        {
            lbl_FileCat_Parent.Text = "";
        }
    }
    //---------------------------------------------------------------------
    public void DirectoryList_DataBind()
    {
        string HLink, LinkParam, LinkTarget, DataLink, DataParam, DataTarget, strSql = "";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        DataTable dt;

        strSql = @"select FileCat_ID,FileCat_ParentID,��Ƨ��W��=FileCat_Name,�إߤH=FileCat_CreatedBy,��s���=FileCat_UpdateDate,��Ƨ���=(select count(*) from filecats where filecat_ParentID=b.filecat_id),";
        strSql += "    �ɮ׼�=(select count(*) from uploads where uploads.AppObject_ID=b.filecat_id),�Ƨ�=Filecat_SortID from FileCats as b where ";
        strSql += "   (@FileCat_id='' and FileCat_ParentID=0 or FileCat_ParentID=@FileCat_id) order by FileCat_SortID ";
        dict.Add("FileCat_id", HFD_filecat_id.Value);
        dt = NpoDB.GetDataTableS(strSql, dict);

        HLink = "FileCats.aspx?filecat_id=";
        LinkParam = "filecat_id";
        LinkTarget="_self";
        DataLink = "FileCats_Delete.aspx?filecat_id=";
        DataParam = "filecat_id";
        DataTarget="_self";

        Grid_List1.Text = DataLinkGrids(dt, HLink, LinkParam, LinkTarget, DataLink, DataParam, DataTarget); 
    }
    //---------------------------------------------------------------------
    public void FileList_DataBind()
    {
        string HLink, LinkParam, LinkTarget, DataLink, DataParam, DataTarget, strSql = "";
        string keyword;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        DataTable dt;

        keyword = FD_Keyword.Text ;
        strSql = @"select Upload_ID,Upload_FileName,�ɮצW��=(case when upload_note<>'' then Upload_Note else Upload_FileName end),�ɮפj�p=Convert(varchar,Upload_filesize/1024)+'KB',���G�H=Upload_By,���G���=Upload_Date,�I�\��=download_Count from uploads ";
        strSql+= @"    where    (AppObject_ID=@filecat_id and @keyword='') or (@keyword<>'' and (Upload_Note like @keyword_like or Upload_FileName like @keyword_like)) order by Upload_ID desc ";
        dict.Add("FileCat_id", HFD_filecat_id.Value);
        dict.Add("keyword",keyword);
        dict.Add("keyword_like", "%" + keyword + "%");
        dt = NpoDB.GetDataTableS(strSql, dict);

        HLink="File_Show.aspx?upload_id=";
        LinkParam="upload_id";
        LinkTarget="_self";
        DataLink = "FileUpload_Delete.aspx?upload_id=";
        DataParam = "upload_id";
        DataTarget = "_self";

        Grid_List2.Text = DataLinkGrids2(dt, HLink, LinkParam, LinkTarget, DataLink, DataParam, DataTarget); 
    }
    //---------------------------------------------------------------------
    //��Ƨ��C��M��
    public string DataLinkGrids(DataTable dt, string HLink , string LinkParam,string LinkTarget,string DataLink ,string DataParam , string DataTarget)
    {
        int i ; 
        StringBuilder  sb = new StringBuilder(); 
        sb.AppendLine(@"<table width='100%' align='center' class='table_h'>");
        sb.AppendLine(@"<tr>");
        i = 0 ; 
        foreach(DataColumn dc in dt.Columns )
        {
            if(i<2)
            {
                i++;
                continue ;
                
            }
            sb.AppendLine(@"<th>" + dc.ColumnName + "</th>");
        }
        sb.AppendLine(@"<th>�ﶵ</th>");
        sb.AppendLine("</tr>");

        foreach(DataRow  dr in dt.Rows)
        {
            i = 0 ;
            sb.AppendLine(@"<tr>");
            foreach(DataColumn dc in dt.Columns)
            {
                if(i<2)
                {
                    i++;
                    continue ;                    
                }
                if (i == 2)
                {
                    sb.AppendLine(@"<td><img border='0' src='../images/folder_min.gif' width='16' height='16' align='absmiddle'><a href='" + HLink + dr[LinkParam] + @"' target='" + LinkTarget + @"'>" + dr[dc.ColumnName] + "</a></td>");
                }
                else
                {
                    //��ܸ��
                    sb.AppendLine("<td>" + dr[dc.ColumnName] + "</td>");
                }
                i++;
            }
            sb.AppendLine(@"<td><a href onclick=""window.open('FileCats_Edit.aspx?FileCat_ParentID="+dr[1]+"&FileCat_ID="+dr[0]+@"','','scrollbars=yes,menubar=yes,top=100,left=120,width=550,height=180,resizable=yes')""><img src='../images/save.gif' border=0 align='absmiddle' alt='�ק�'>�ק�</a> <a href='JavaScript:if(confirm(""�O�_�T�w�n�R�� ?"")){window.location.href=""" + DataLink + dr[DataParam] + @""";}' target='" + DataTarget +@"'><img src='../images/delete2.gif' align='absmiddle' border=0 alt='�R��'></a></td>");
            sb.AppendLine("</tr>");
        }
        sb.AppendLine("</table>");
        return sb.ToString();
    }
    //---------------------------------------------------------------------
    //�ɮצC��M��
    public string DataLinkGrids2(DataTable dt, string HLink , string LinkParam,string LinkTarget,string DataLink ,string DataParam , string DataTarget)
    {
        if (dt.Rows.Count == 0)
        {
            return "";
        }

        int i ; 
        StringBuilder  sb = new StringBuilder(); 
        sb.AppendLine(@"<table width='100%' align='center' class='table_h'>");
        sb.AppendLine(@"<tr>");
        i = 0 ; 
        foreach(DataColumn dc in dt.Columns )
        {
            if(i<2)
            {
                i++;
                continue ;               
            }
            sb.AppendLine(@"<th nowrap>" + dc.ColumnName + "</th>");
        }
        sb.AppendLine(@"<th nowrap></th>");
        sb.AppendLine("</tr>");

        string icon,filetype;
        foreach(DataRow dr in dt.Rows )
        {
            i=0;
            string Upload_fileName  = dr["Upload_FileName"].ToString();
            string Extension = System.IO.Path.GetExtension(Upload_fileName).ToUpper();
            switch(Extension)
            {
                case ".JPG":
                case ".GIF":
                    icon="icon_pic.gif";
                    filetype="�Ӥ��ι���";
                    break;
                case ".XLS":
                    icon="icon_xls.gif";
                    filetype="Excel�ɮ�";
                    break;
                case ".DOC":
                    icon="icon_doc.gif"  ;
                    filetype="Word�ɮ�";
                    break;
                case ".PPT":
                    icon="icon_ppt.gif" ;
                    filetype="PowerPoint�ɮ�";
                    break;
                case ".HTM":
                    icon="icon_htm.gif";
                    filetype="�����ɮ�";
                    break;
                case ".PDF":
                    icon="icon_pdf.gif";
                    filetype="PDF�ɮ�";
                    break;
                case ".SWF":
                    icon="icon_swf.gif";
                    filetype="Flash�ʵe�ɮ�";
                    break;
                case ".TXT":
                    icon="icon_txt.gif";
                    filetype="�¤�r�ɮ�";
                    break;
                default:
                    icon="icon_txt.gif";
                    filetype="�ɮץ���";
                    break;
            }
            sb.AppendLine(@"<tr>");
            foreach(DataColumn dc in dt.Columns )
            {
                if(i<2)
                {
                    i++;
                    continue ;                    
                }
                if (i == 2)
                {
                    sb.AppendLine(@"<td><img border='0' src='../images/" + icon + @"' align='absmiddle' alt='" + filetype + @"'> <a href onclick=""window.open('" + HLink + dr[LinkParam] + @"','','scrollbars=yes,menubar=yes,top=0,left=0,width=780,height=600,resizable=yes')"" title='" + dr[1] + @"'>" + dr[dc.ColumnName] + "</a></td>");
                }
                else
                {
                    sb.AppendLine(@"<td>" + dr[dc.ColumnName] + "</td>");
                }
                i++;
            }
            sb.AppendLine(@"<td><a href onclick=""window.open('FileUpload_Edit.aspx?Upload_ID="+dr[LinkParam]+@"','','scrollbars=yes,menubar=yes,top=100,left=120,width=550,height=200,resizable=yes')""><img src='../images/save.gif' border=0 align='absmiddle' alt='��s'>��s</a> <a href='JavaScript:if(confirm(""�O�_�T�w�n�R�� ?"")){window.location.href=""" + DataLink + dr[DataParam] + @""";}' target='" + DataTarget+@"'><img src='../images/delete2.gif' align='absmiddle' border=0 alt='�R��'></a></td>");
            sb.AppendLine("</tr>");
        }
        sb.AppendLine("</table>");
        return sb.ToString();
    }
    //---------------------------------------------------------------------
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        if (FD_Keyword.Text != "")
        {
            Grid_List1.Text = "";
        }
        else
        {
            DirectoryList_DataBind();
        }
        FileList_DataBind();
    }
    //---------------------------------------------------------------------
}
