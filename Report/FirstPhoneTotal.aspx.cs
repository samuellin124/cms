/**
* kuofly 20150205
* ��¦�d��SQL�ҫ��A�D�n�N�Ĥ@���ӹq���~���ɤ����ഫ����ȡA�ҥH��������Ƨk�X��Ȫ���ơA�N�O�Ĥ@���ӹq
* select * from Consulting a 
*where convert(varchar,CreateDate,111) between '2014/02/08' and '2014/02/10'
*and a.MemberUID + '_' + (select replace(convert(varchar(8), a.CreateDate, 112)+convert(varchar(8), CreateDate, 114), ':',''))
*in
*	(
*		select �ϥΪ���� from 
*		(
*		select Memberuid+ '_' + min(replace(convert(varchar(8), CreateDate, 112)+convert(varchar(8), CreateDate, 114), ':','')) �ϥΪ����,convert(varchar,MIN(CreateDate),111) �Ĥ@���ӹq��� 
*		from Consulting where isnull(MemberUID,'')<>'' and isnull(IsDelete, '') != 'Y' 
*		group by Memberuid 
*		) table1
*		where �Ĥ@���ӹq��� between '2014/02/08' and '2014/02/10'
*		and �ϥΪ���� is not null
*	)
*	order by 1
**/
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Text;

public partial class FirstPhone : BasePage
{
    GroupAuthrity ga = null;
    string FirstPhoneNumber = "";
    #region NpoGridView �B�z���������{���X
    Button btnNextPage, btnPreviousPage, btnGoPage;
    HiddenField HFD_CurrentPage;
    //�Ƨǥ�
    HiddenField HFD_CurrentSortField;
    HiddenField HFD_CurrentSortDir;

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
        btnNextPage.Height = 1;
        btnNextPage.Width = 1;
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

        HFD_CurrentSortField = new HiddenField();
        HFD_CurrentSortField.Value = "";
        HFD_CurrentSortField.ID = "HFD_CurrentSortField";
        Form1.Controls.Add(HFD_CurrentSortField);

        HFD_CurrentSortDir = new HiddenField();
        HFD_CurrentSortDir.Value = "asc";
        HFD_CurrentSortDir.ID = "HFD_CurrentSortDir";
        Form1.Controls.Add(HFD_CurrentSortDir);
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
        Session["ProgID"] = "FirstPhoneTotal";
        HFD_EditServiceUser.Value = SessionInfo.UserID.ToString();

        litTitle.Text = "�Ĥ@���ӹq�`�� ";


        Util.ShowSysMsgWithScript("$('#btnPreviousPage').hide();$('#btnNextPage').hide();$('#btnGoPage').hide();");
    }
    //-------------------------------------------------------------------------------------------------------------
    private void LoadFormData()
    {
        //�I�s��ƮwGetDataTable
        DataTable dt = GetDataTable();
        DataRow dr = null;
        if (dt.Rows.Count == 0)
        {
            //lblGridList.Text = "";
            lblCnt.Text = "";
            ShowSysMsg("�d�L���");
            return;
        }

        dr = dt.Rows[0];

            lblCnt.Text = "�@"+dr["Cnt"].ToString()+"��";
 //�I�s��ƮwGetDataTable1
            DataTable dt1 = GetDataTable1();
            DataRow dr1 = null;
            if (dt1.Rows.Count == 0)
            {
                //lblGridList.Text = "";
                lblGridList.Text = "";
                ShowSysMsg("�d�L���");
                return;
            }
            dr1 = dt1.Rows[0];
            lblGridList.Text = (dr1["firstphone"].ToString() == "") ? "0����" : dr1["firstphone"].ToString() + "����";
            //********************************************************************************************
            //�ۭq���
            NPOGridView npoGridView = new NPOGridView();
            //�ӷ�����
            npoGridView.Source = NPOGridViewDataSource.fromDataTable;
            //�ϥΪ� DataTable
            npoGridView.dataTable = dt;
            //���n��ܪ����(�i�H�h��)

            npoGridView.DisableColumn.Add("uid");
            npoGridView.EditLinkTarget = "','target=_blank,help=no,status=yes,resizable=yes,scrollbars=yes,scrolling=yes,scroll=yes,center=yes,width=900px,height=500px";
            //npoGridView.EditLink = Util.RedirectByTime("ConsultEdit2.aspx", "ConsultingUID=");
             
            //�ϥΪ� key ���,�Φb�զ� QueryString �� key ��(EditLink �|�Ψ�)
            npoGridView.Keys.Add("uid");


            //�p�G GridView �n½��, �n�]�w CurrentPage ����
            if (npoGridView.ShowPage == true)
            {
                npoGridView.CurrentPage = Util.String2Number(HFD_CurrentPage.Value);
            }


            //NPOGridViewColumn col;
            //col = new NPOGridViewColumn("�m�W"); //���ͩһ����γ]�w Title
            //col.ColumnType = NPOColumnType.NormalText; //�]�w������ܤ�r
            //col.ColumnName.Add("�m�W"); //�ϥ����W��
            //col.CellStyle = "width:100px";
            //npoGridView.Columns.Add(col);
        
            //col = new NPOGridViewColumn("�Ƶ�"); //���ͩһ����γ]�w Title
            //col.ColumnType = NPOColumnType.NormalText; //�]�w������ܤ�r
            //col.ColumnName.Add("�Ƶ�"); //�ϥ����W��
            //col.Link = Util.RedirectByTime("ConsultEdit2.aspx", "ConsultingUID=");
            //col.CellStyle = "width:200px";
            //npoGridView.Columns.Add(col);
            ////----------------------------------------------
            //col = new NPOGridViewColumn("�q��"); //���ͩһ����γ]�w Title
            //col.ColumnType = NPOColumnType.NormalText; //�]�w������ܤ�r
            //col.ColumnName.Add("�q��"); //�ϥ����W��
            //col.Link = Util.RedirectByTime("ConsultEdit2.aspx", "ConsultingUID=");
            //col.CellStyle = "width:150px";
            //npoGridView.Columns.Add(col);
            //// lblGridList.Text = npoGridView.Render();
            ////----------------------------------------------------------
            //col = new NPOGridViewColumn("���ɮɶ�"); //���ͩһ����γ]�w Title
            //col.ColumnType = NPOColumnType.NormalText; //�]�w������ܤ�r
            //col.ColumnName.Add("���ɮɶ�"); //�ϥ����W��
            //col.Link = Util.RedirectByTime("ConsultEdit2.aspx", "ConsultingUID=");
            //col.CellStyle = "width:150px";
            //npoGridView.Columns.Add(col);
            //    //----------------------------------------------------------
            //col = new NPOGridViewColumn("�ӹq�ԸߧO(�j��)"); //���ͩһ����γ]�w Title
            //col.ColumnType = NPOColumnType.NormalText; //�]�w������ܤ�r
            //col.ColumnName.Add("�ӹq�ԸߧO(�j��)"); //�ϥ����W��
            //col.Link = Util.RedirectByTime("ConsultEdit2.aspx", "ConsultingUID=");
            //col.CellStyle = "width:200px";
            //npoGridView.Columns.Add(col);
            ////----------------------------------------------------------
            //col = new NPOGridViewColumn("�ӹq�ԸߧO(����)"); //���ͩһ����γ]�w Title
            //col.ColumnType = NPOColumnType.NormalText; //�]�w������ܤ�r
            //col.ColumnName.Add("�ӹq�ԸߧO(����)"); //�ϥ����W��
            //col.Link = Util.RedirectByTime("ConsultEdit2.aspx", "ConsultingUID=");
            //col.CellStyle = "width:200px";
            //npoGridView.Columns.Add(col);
            //lblGridList.Text = npoGridView.Render();
        //----------------------------------------------------------
        //********************************************************************************************
    }
   //-------------------------------------------------------------------------------------------------------------
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        LoadFormData();
    }
    //-------------------------------------------------------------------------------------------------------------
    private string GetRptHead()
    {
        string strTemp = "";
        HtmlTable table = new HtmlTable();
        HtmlTableRow row;
        HtmlTableCell cell;
        CssStyleCollection css;

        string FontSize = "14px";
        table.Border = 0;
        table.CellPadding = 0;
        table.CellSpacing = 0;
        table.Align = "center";
        css = table.Style;
        css.Add("font-size", "12px");
        css.Add("font-family", "�з���");
        css.Add("width", "100%");
        //****************************************
        row = new HtmlTableRow();
        cell = new HtmlTableCell();
        //�d�߫����N�X�W��
        DataTable dt = GetDataTable();
        foreach (DataRow dr in dt.Rows)
        {
        string city = dr["����"].ToString();
        strTemp =  city+ "������";
        }

        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        //�з|���m��
        cell.ColSpan = 7;
        css.Add("text-align", "center");
        css.Add("font-size", "24px");
        css.Add("font-family", "�з���");
        css.Add("border-style", "none");
        css.Add("font-weight", "bold");
        row.Cells.Add(cell);
        table.Rows.Add(row);
        //*************************************
        //�ন html �X
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        table.RenderControl(htw);

        return htw.InnerWriter.ToString();
    }
   
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        lblRpt.Text = GetRptHead();
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
        Util.OutputTxt(sb + lblRpt.Text, "1", DateTime.Now.ToString("yyyyMMddHHmmss"));
    }
    // ���Ƹ�Ʈw
    private DataTable GetDataTable()
    {
//        string strSql = @"
//                        ;with sum_phone
//                        as
//                        (
//                        select  count(*) as cnt from Consulting where 1=1 and isnull(IsDelete, '') != 'Y' 
//                        ";
//        if (txtBegCreateDate.Text != "" && txtEndCreateDate.Text != "")
//        {
//            strSql += " and CONVERT(varchar(100), CreateDate, 111) Between @BegCreateDate And @EndCreateDate";
//        }
//         strSql += @" group by memberuid  having  count(memberuid)=1 
//                        )
//                        select sum(cnt) as Cnt
//                        from sum_phone;
//  ";
        string strSql = @"select COUNT(*) cnt from Consulting a 
                        where 1=1";
        if (txtBegCreateDate.Text != "" && txtEndCreateDate.Text != "")
        {
            strSql += @" and convert(varchar,CreateDate,111) between @BegCreateDate and @EndCreateDate ";
        }
                        strSql += @"and a.MemberUID + '_' + (select replace(convert(varchar(8), a.CreateDate, 112)+convert(varchar(8), CreateDate, 114), ':',''))
                        in
	                        (
		                        select �ϥΪ���� from 
		                        (
		                        select Memberuid+ '_' + min(replace(convert(varchar(8), CreateDate, 112)+convert(varchar(8), CreateDate, 114), ':','')) �ϥΪ����,convert(varchar,MIN(CreateDate),111) �Ĥ@���ӹq��� 
		                        from Consulting where isnull(MemberUID,'')<>'' and isnull(IsDelete, '') != 'Y' 
		                        group by Memberuid 
		                        ) table1
		                        where 1=1 ";
                        if (txtBegCreateDate.Text != "" && txtEndCreateDate.Text != "")
                        {
                            strSql += @" and �Ĥ@���ӹq��� between  @BegCreateDate and @EndCreateDate ";
                        }
		                 strSql += @" and �ϥΪ���� is not null
	                        )";

        if (SessionInfo.GroupName != "�t�κ޲z��")
        {
            strSql += " and ServiceUser = @ServiceUser\n";
        }
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("BegCreateDate", txtBegCreateDate.Text.Trim());
        dict.Add("EndCreateDate", txtEndCreateDate.Text.Trim());
        dict.Add("ServiceUser", SessionInfo.UserID.ToString());
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        return dt;
    }


//�p��ɤ����ҫ�
//select (SUM(DATEPART(hour,�ɤ�) * 60) +SUM(DATEPART(minute,�ɤ�)))as firstphone
//from 
//(
//select CONVERT(varchar, DATEADD(s, DATEDIFF(s,CreateDate,EndDate), 0), 108) �ɤ�
//from Consulting
//where ServiceUser ='Nash' 
//) table1
    private DataTable GetDataTable1()
    {
//        string strSql = @"
//                            ;with cte_raw
//                            as
//                            (
//                            select  distinct  CONVERT(varchar, DATEADD(s, DATEDIFF(s,CreateDate,EndDate), 0), 108) as number                                
//                            from Consulting a  
//                            inner join Member b on a.MemberUID = b.uid 
//                            where 1=1 ";
//        if (txtBegCreateDate.Text != "" && txtEndCreateDate.Text != "")
//        {
//            strSql += " and CONVERT(varchar(100), CreateDate, 111) Between @BegCreateDate And @EndCreateDate\n";
//        }
//        strSql += @"
//                            and b.uid in
//                            (
//                            select  memberuid from Consulting where 1=1 and isnull(IsDelete, '') != 'Y' 
//                            group by memberuid  having  count(memberuid)=1 
//                            )
//                            and isnull(a.IsDelete, '') != 'Y'
//                            )
//                            select (SUM(DATEPART(hour,number) * 60) +SUM(DATEPART(minute,number)))as firstphone
//                            from cte_raw;
//                    ";
        string strSql = @" select (SUM(DATEPART(hour,�ɤ�) * 60) +SUM(DATEPART(minute,�ɤ�))) as firstphone from ";

        strSql += @" (select CONVERT(varchar, DATEADD(s, DATEDIFF(s,CreateDate,EndDate), 0), 108) �ɤ� from Consulting a 
                        where 1=1";
        if (txtBegCreateDate.Text != "" && txtEndCreateDate.Text != "")
        {
            strSql += @" and convert(varchar,CreateDate,111) between @BegCreateDate and @EndCreateDate ";
        }
        strSql += @"and a.MemberUID + '_' + (select replace(convert(varchar(8), a.CreateDate, 112)+convert(varchar(8), CreateDate, 114), ':',''))
                        in
	                        (
		                        select �ϥΪ���� from 
		                        (
		                        select Memberuid+ '_' + min(replace(convert(varchar(8), CreateDate, 112)+convert(varchar(8), CreateDate, 114), ':','')) �ϥΪ����,convert(varchar,MIN(CreateDate),111) �Ĥ@���ӹq��� 
		                        from Consulting where isnull(MemberUID,'')<>'' and isnull(IsDelete, '') != 'Y' 
		                        group by Memberuid 
		                        ) table1
		                        where 1=1 ";
        if (txtBegCreateDate.Text != "" && txtEndCreateDate.Text != "")
        {
            strSql += @" and �Ĥ@���ӹq��� between  @BegCreateDate and @EndCreateDate ";
        }
        strSql += @" and �ϥΪ���� is not null
	                        )";
        if (SessionInfo.GroupName != "�t�κ޲z��"){
            strSql += " and ServiceUser = @ServiceUser\n";
        }
        strSql += @" ) table1 ";
        //Response.Write(strSql);
        //Response.End();
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("BegCreateDate", txtBegCreateDate.Text.Trim());
        dict.Add("EndCreateDate", txtEndCreateDate.Text.Trim());
        dict.Add("ServiceUser", SessionInfo.UserID.ToString());
        DataTable dt1 = NpoDB.GetDataTableS(strSql, dict);
        return dt1;
    }
}
