using System;
using System.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
public partial class SysMgr_ServicePoints : BasePage
{
    #region NpoGridView �B�z���������{���X
    Button btnNextPage, btnPreviousPage, btnGoPage;
    HiddenField HFD_CurrentPage;

    override protected void OnInit(EventArgs e)
    {
        CreatePageControl();
        base.OnInit(e);
    }
    private void CreatePageControl()
    {
        // Create dynamic controls here.
        btnNextPage = new Button();
        btnNextPage.ID = "btnNextPage";
        Form1.Controls.Add(btnNextPage);
        btnNextPage.Click += new System.EventHandler(btnNextPage_Click);

        btnPreviousPage = new Button();
        btnPreviousPage.ID = "btnPreviousPage";
        Form1.Controls.Add(btnPreviousPage);
        btnPreviousPage.Click += new System.EventHandler(btnPreviousPage_Click);

        btnGoPage = new Button();
        btnGoPage.ID = "btnGoPage";
        Form1.Controls.Add(btnGoPage);
        btnGoPage.Click += new System.EventHandler(btnGoPage_Click);

        HFD_CurrentPage = new HiddenField();
        HFD_CurrentPage.Value = "1";
        HFD_CurrentPage.ID = "HFD_CurrentPage";
        Form1.Controls.Add(HFD_CurrentPage);
    }
    protected void btnPreviousPage_Click(object sender, EventArgs e)
    {
        HFD_CurrentPage.Value = Util.MinusStringNumber(HFD_CurrentPage.Value);
        LoadFormData();
    }
    protected void btnNextPage_Click(object sender, EventArgs e)
    {
        HFD_CurrentPage.Value = Util.AddStringNumber(HFD_CurrentPage.Value);
        LoadFormData();
    }
    protected void btnGoPage_Click(object sender, EventArgs e)
    {
        LoadFormData();
    }
    #endregion NpoGridView �B�z���������{���X
    //---------------------------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["ProgID"] = "ServicePoints";
        //�v���B�z
        AuthrityControl();

        //�� npoGridView �ɤ~�ݭn
        Util.ShowSysMsgWithScript("$('#btnPreviousPage').hide();$('#btnNextPage').hide();$('#btnGoPage').hide();");

        if (!IsPostBack)
        {
            LoadFormData(); 
        }
    }
    //------------------------------------------------------------------------------
    public void AuthrityControl()
    {
        if (Authrity.PageRight("_Focus") == false)
        {
            return;
        }
        //Authrity.CheckButtonRight("_AddNew", btnAdd);
        //Authrity.CheckButtonRight("_Query", btnQuery);
    }
    //----------------------------------------------------------------------
    public void LoadFormData()
    {

        //�I�s��ƮwGetDataTable
        DataTable dt = GetDataTable(txtsDate.Text,txteDate.Text);
        //Grid initial
        NPOGridView npoGridView = new NPOGridView();
        npoGridView.Source = NPOGridViewDataSource.fromDataTable;
        npoGridView.dataTable = dt;
        npoGridView.Keys.Add("uid");
        npoGridView.DisableColumn.Add("uid");

        npoGridView.CurrentPage = Util.String2Number(HFD_CurrentPage.Value);
        //npoGridView.EditLink = Util.RedirectByTime("Church_Edit.aspx", "ChurchUID=");
        lblGridList.Text = npoGridView.Render();

        string strSql = "select UserID,UserName from AdminUser";
        Util.FillDropDownList(ddlUserName, strSql, "UserName", "UserID", true);

    }
    //------------------------------------------------------------------------------
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        string keep = ddlUserName.SelectedValue.ToString();
        LoadFormData();
        ddlUserName.SelectedValue = keep;
    }
    //------------------------------------------------------------------------------
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        string �C�L�Ҧ��s�W���Y = "";

        �C�L�Ҧ��s�W���Y += "<table><tr><td colspan=\"8\" style=\"font-size:24px;font-family:�з���;width:100%;\">";
        �C�L�Ҧ��s�W���Y += "�Ӥu�A�Ȭ�����";
        if (txtsDate.Text.Trim() != "" && txteDate.Text.Trim() != "") {
            �C�L�Ҧ��s�W���Y += "(" + txtsDate.Text.Trim() + "��" + txteDate.Text.Trim() + ")"; 
        }
        �C�L�Ҧ��s�W���Y += "�έp";
        �C�L�Ҧ��s�W���Y += "</td></tr></table>\n";

        //��l���g�k,���ݤ����b�g�ƻ�
        ExportDataTable Export = new ExportDataTable();
        DataTable dt = GetDataTable("", "");
        Export.dataTable = dt;
        Export.DisableColumn.Add("ColName");
        string ExportData = Title + Export.Render();
        
        string s = @"<head>
                        <style>
                        @page Section2
                        { size:841.7pt 595.45pt;
                          mso-page-orientation:landscape;
                        margin:1.5cm 0.5cm 0.5cm 0.5cm;}
                        div.Section2
                        {page:Section2;}
                        </style>
                    </head>
                    <body>
                        <div class=Section2>
                    ";//���-�W�k�U��
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(s);
        Util.OutputTxt(sb + �C�L�Ҧ��s�W���Y + Export.Render(), "1", "" + DateTime.Now.ToString("yyyyMMddHHmmss"));

    }
    //----------------------------------------------------------------------------------------------------------
    
    private DataTable GetDataTable(string �}�l���YYYYMMDD,string �������YYYYMMDD)
    {
        //Ū����Ʈw
        string �ϥΪ̥N�� = "";
        if (this.ddlUserName.SelectedValue.ToString() != "")
        {
            �ϥΪ̥N�� = ddlUserName.SelectedValue.ToString();
        }
        string strSql = ���o�p��SQL(�}�l���YYYYMMDD, �������YYYYMMDD,�ϥΪ̥N��);

        Dictionary<string, object> dict = new Dictionary<string, object>();
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);

        if (dt.Rows.Count > 0)
        {
            dt = ���s�p����(dt);
        }
        return dt;
    }


    #region ��¦SQL�d��
//    select 
//�m�W,�Ӥu��,�ɼ�,�`�p�I��,
//isnull([��ť�u�����]],0) [��ť�u�����]],
//isnull([Ū�ѷ|],0) [Ū�ѷ|],
//isnull([�b¾�Ш|],0) [�b¾�Ш|],
//isnull([�ѻP����],0) [�ѻP����],
//isnull([����ժ�],0) [����ժ�],
//isnull([Ū�ѷ|�a��H],0) [Ū�ѷ|�a��H],
//isnull([����]���x���H],0) [����]���x���H],
//isnull([2/14�Ӥu��~�|],0) [2/14�Ӥu��~�|],
//isnull([GOODTV��~�|],0) [GOODTV��~�|]
//from
//(
//select 
//Uid,
//UserName �m�W,
//UserID �Ӥu��,
//(select sum(dateDiff(minute,LogTime,LogoutTime)) 
//    From log where 1=1 and log.UserID=AdminUser.Userid and (LogTime is not null and LogoutTime is not null) 
//    and convert(varchar,LogTime,111) between '2014/01/01' and '2015/12/31'
//    group by userid 
//) �ɼ�,
//'' [�`�p�I��],
//(select COUNT(*) from ServiceRecord 
//where 1=1 and ItemUid='1' and ServiceRecord.UserID=AdminUser.UserID
//and convert(varchar,serviceData,111) between '2014/01/01' and '2015/12/31' 
//group by itemuid
//) [��ť�u�����]],
//(select COUNT(*) from ServiceRecord 
//where 1=1 and ItemUid='3' and ServiceRecord.UserID=AdminUser.UserID
//and convert(varchar,serviceData,111) between '2014/01/01' and '2015/12/31' 
//group by itemuid
//) [Ū�ѷ|],
//(select COUNT(*) from ServiceRecord 
//where 1=1 and ItemUid='4' and ServiceRecord.UserID=AdminUser.UserID
//and convert(varchar,serviceData,111) between '2014/01/01' and '2015/12/31' 
//group by itemuid
//) [�b¾�Ш|],
//(select COUNT(*) from ServiceRecord 
//where 1=1 and ItemUid='6' and ServiceRecord.UserID=AdminUser.UserID
//and convert(varchar,serviceData,111) between '2014/01/01' and '2015/12/31' 
//group by itemuid
//) [�ѻP����],
//(select COUNT(*) from ServiceRecord 
//where 1=1 and ItemUid='7' and ServiceRecord.UserID=AdminUser.UserID
//and convert(varchar,serviceData,111) between '2014/01/01' and '2015/12/31' 
//group by itemuid
//) [����ժ�],
//(select COUNT(*) from ServiceRecord 
//where 1=1 and ItemUid='8' and ServiceRecord.UserID=AdminUser.UserID
//and convert(varchar,serviceData,111) between '2014/01/01' and '2015/12/31' 
//group by itemuid
//) [Ū�ѷ|�a��H],
//(select COUNT(*) from ServiceRecord 
//where 1=1 and ItemUid='9' and ServiceRecord.UserID=AdminUser.UserID
//and convert(varchar,serviceData,111) between '2014/01/01' and '2015/12/31' 
//group by itemuid
//) [����]���x���H],
//(select COUNT(*) from ServiceRecord 
//where 1=1 and ItemUid='10' and ServiceRecord.UserID=AdminUser.UserID
//and convert(varchar,serviceData,111) between '2014/01/01' and '2015/12/31' 
//group by itemuid
//) [2/14�Ӥu��~�|],
//(select COUNT(*) from ServiceRecord 
//where 1=1 and ItemUid='11' and ServiceRecord.UserID=AdminUser.UserID
//and convert(varchar,serviceData,111) between '2014/01/01' and '2015/12/31' 
//group by itemuid
//) [GOODTV��~�|]
//from AdminUser
//where 1=1
//and IsUse=1 
//and UserID in 
//(
//    select UserID from Log where  1=1 and (LogTime is not null and LogoutTime is not null) and convert(varchar,LogTime,111) between '2014/01/01' and '2015/12/31'
//    union
//    select UserID from ServiceRecord where convert(varchar,serviceData,111) between '2014/01/01' and '2015/12/31'
//)
//) table1 
//order by uid
    #endregion ��¦SQL�d��
    //����:�אּ�@�Ӥ��e��������O���c��²�檺����SQL
    private string ���o�p��SQL(string �}�l���YYYYMMDD ,string �������YYYYMMDD,string �ϥΪ̥N��)
    {
        string strSQL = @"
            select 
            �m�W,�Ӥu��,�ɼ�,0 �`�p�I��"
            + ���o�Ĥ@�qSQL() +
            @" from
            (
            select 
            Uid,
            UserName �m�W,
            UserID �Ӥu��
            ,(select sum(dateDiff(minute,LogTime,LogoutTime)) 
	            From log where 1=1 and log.UserID=AdminUser.Userid and (LogTime is not null and LogoutTime is not null) 
            ";
            if (�}�l���YYYYMMDD!="" && �������YYYYMMDD!="" ){
                strSQL += "    and convert(varchar,LogTime,111) between '" + �}�l���YYYYMMDD + "' and '" + �������YYYYMMDD + "' ";
            }
            strSQL +=
	        @"  group by userid 
            ) �ɼ�
            ,'' [�`�p�I��]"
            + ���o�ĤG�qSQL(�}�l���YYYYMMDD, �������YYYYMMDD) +
            @" from AdminUser
            where 1=1
            and IsUse=1 
            and UserID in 
            (";
            strSQL += " select UserID from Log where  1=1 and (LogTime is not null and LogoutTime is not null) \n";
            if (�}�l���YYYYMMDD!="" && �������YYYYMMDD!="" ){
                strSQL += " and convert(varchar,LogTime,111) between '" + �}�l���YYYYMMDD + "' and '" + �������YYYYMMDD + "' \n";
            }
	        if (�ϥΪ̥N��!=""){
                strSQL += " and UserID='" + �ϥΪ̥N�� + "' \n";
            }
            strSQL += " Union \n";
	        strSQL += " select UserID from ServiceRecord where 1=1 ";
            if (�}�l���YYYYMMDD!="" && �������YYYYMMDD!="" ){
                strSQL += " and convert(varchar,serviceData,111) between '" + �}�l���YYYYMMDD + "' and '" + �������YYYYMMDD + "' \n";
            }
            if (�ϥΪ̥N�� != "")
            {
                strSQL += " and UserID='" + �ϥΪ̥N�� + "' \n";
            }
            strSQL += @")
            ) table1 
            order by uid 
            ";
        return strSQL;
    }

    //�o�q�ε{������ 
    private string ���o�Ĥ@�qSQL() {
        string getSql = " SELECT Uid,'[' + ItemName + ']' itemName,isnull(Score,0) score  FROM ServiceItem where isnull(IsDelete ,'') != 'Y' ORDER BY ClassUid,uid ";
        DataTable tmpDT = NpoDB.GetDataTableS(getSql, new Dictionary<string, object>());
        string retStr = "";
        for (int i = 0; i < tmpDT.Rows.Count; i++) {
            retStr += ",isnull(" + tmpDT.Rows[i]["itemName"].ToString() + ",0) " + tmpDT.Rows[i]["itemName"].ToString() + "\n";
        }
        return retStr;
    }

    private string ���o�ĤG�qSQL(string �}�l���YYYYMMDD, string �������YYYYMMDD)
    {
        string getSql = " SELECT Uid,'[' + ItemName + ']' itemName,isnull(Score,0) score  FROM ServiceItem where isnull(IsDelete ,'') != 'Y' ORDER BY ClassUid,uid ";
        DataTable tmpDT = NpoDB.GetDataTableS(getSql, new Dictionary<string, object>());
        string retStr = "";
        for (int i = 0; i < tmpDT.Rows.Count; i++)
        {
            //,(select COUNT(*) from ServiceRecord 
            //        where 1=1 and ItemUid='1' and ServiceRecord.UserID=AdminUser.UserID
            //        and convert(varchar,serviceData,111) between '2014/01/01' and '2015/12/31' 
            //        group by itemuid
            //        ) [��ť�u�����]]
            retStr += ",(select COUNT(*) from ServiceRecord \n";
            retStr += " where 1=1 and  isnull(IsDelete, '') != 'Y' AND ItemUid='" + tmpDT.Rows[i]["Uid"].ToString() + "' and ServiceRecord.UserID=AdminUser.UserID \n";
            if (�}�l���YYYYMMDD != "" && �������YYYYMMDD != "")
            {
                retStr += " and convert(varchar,serviceData,111) between '" + �}�l���YYYYMMDD + "' and '" + �������YYYYMMDD + "' \n";
            }
            retStr += " group by itemuid ) " + tmpDT.Rows[i]["itemName"].ToString() + " \n";
        }
        return retStr;
    }

    //�p��DataTable���Ҧ���ƶǦ^
    private DataTable ���s�p����(DataTable sourceDataTable) {
        //����c��:
        //�N��Ƹ��J�@�Ӱ}�C�A�A�N�}�C����A�A�N���⪺���G��^DataTable�A�M���DataTable��@GridView��Source
        //���ƶ�i�hArrayList

        //#2 ���ƪ�
        Dictionary<string, int> ���Z�� = new Dictionary<string, int>();
        string getSql = " SELECT Uid,'[' + ItemName + ']' itemName,isnull(Score,0) score  FROM ServiceItem where isnull(IsDelete ,'') != 'Y' ORDER BY ClassUid,uid ";
        DataTable tmpDT = NpoDB.GetDataTableS(getSql, new Dictionary<string, object>());
        for (int n = 0; n < tmpDT.Rows.Count; n++)
        {
            ���Z��.Add(tmpDT.Rows[n]["ItemName"].ToString(), int.Parse(tmpDT.Rows[n]["score"].ToString()));
        }

        ArrayList ArrDataRows = new ArrayList();

        int i = 0;
        foreach (DataRow dataRow in sourceDataTable.Rows)
        {
            string tmpStr = "";
            for (int i2 = 0; i2 < dataRow.ItemArray.Length; i2++) {
                tmpStr += dataRow.ItemArray[i2].ToString() + ";";
            }
            if (tmpStr.EndsWith(";") == true) {
                tmpStr=tmpStr.Remove(tmpStr.Length-1, 1);
            }
            ArrDataRows.Add(string.Join(";",tmpStr.Split(';')));
            i++;
        }
        
        //Ū��ArrayList����k
        //for (int j = 0; j < ArrDataRows.Count; j++)
        //{
        //    for (int j1 = 0; j1 < ArrDataRows[j].ToString().Split(';').Length; j1++)
        //        Response.Write(ArrDataRows[j].ToString().Split(';')[j1].ToString() + ";");
        //    Response.Write("<br>");
        //}


        //���s�p����ArrayList
        

        //�NArrayList��^�hdt
        DataTable dt2 = new DataTable();

        //�إ߸�Ƶ��c
        for (int j = 0; j < sourceDataTable.Columns.Count; j++){
            dt2.Columns.Add(sourceDataTable.Columns[j].ColumnName);
        }

        int RowDataColumnsSize = ArrDataRows[0].ToString().Split(';').Length;

        for (int k = 0; k < ArrDataRows.Count; k++) {
            DataRow row = dt2.NewRow();
            decimal tmp�ɼ� = 0;
            for (int k1 = 0; k1 < RowDataColumnsSize;k1++){
                if (dt2.Columns[k1].ToString() == "�m�W" || dt2.Columns[k1].ToString() == "�Ӥu��"){ 
                    //���έp
                    row[k1] = ArrDataRows[k].ToString().Split(';')[k1].ToString(); 
                }
                else if (dt2.Columns[k1].ToString() == "�ɼ�")
                {

                    try
                    {
                        tmp�ɼ� = Math.Round((decimal.Parse(ArrDataRows[k].ToString().Split(';')[k1].ToString()) / 60), 0);
                        row[k1] = tmp�ɼ�.ToString();
                    }
                    catch (Exception e)
                    {
                        row[k1] = "0";
                    }
                }
                else if (dt2.Columns[k1].ToString() == "�`�p�I��")
                {
                    //�o�q�Ⱓ�H�|�åB�|�ˤ��J���W�Z�ɼƥ[�J�`�p�I��
                    row[k1] = Math.Round((decimal.Parse(ArrDataRows[k].ToString().Split(';')[k1].ToString()) + tmp�ɼ� / 4), 0).ToString();
                }
                else
                {
                    //�p�����
                    string ����=ArrDataRows[k].ToString().Split(';')[k1].ToString();
                    int ���ƾ� = 0;
                    try
                    {
                        ���ƾ� = int.Parse(����);
                        if (���ƾ� > 0)
                        {
                            int ���W�p�� = 0;
                            ���W�p�� = ���Z��["[" + dt2.Columns[k1].ToString() + "]"];
                            row["�`�p�I��"] = (int.Parse(row["�`�p�I��"].ToString()) + ���ƾ� * ���W�p��).ToString();
                        }
                    }
                    catch (Exception){ }
                    row[k1] = ����;
                }
            }
            dt2.Rows.Add(row);
        }

        //���s��z���DT�A�p�G�O0�A�g���ť�
        for (int k = 0; k < dt2.Rows.Count; k++)
        {
            for (int j = 0; j < dt2.Columns.Count; j++) {
                if (dt2.Rows[k][j].ToString() == "0") {
                    dt2.Rows[k][j] = "";
                }
            }
        }
        return dt2;
    }

}