using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class CaseMgr_AdvisoryDataQry: BasePage
{
    #region NpoGridView 處理換頁相關程式碼
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
    #endregion NpoGridView 處理換頁相關程式碼
    //---------------------------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["ProgID"] = "AdvisoryCase";

        //有 npoGridView 時才需要
        Util.ShowSysMsgWithScript("$('#btnPreviousPage').hide();$('#btnNextPage').hide();$('#btnGoPage').hide();");

        if (!IsPostBack)
        {
            //HFD_DonorName.Value = Util.GetQueryString("DonorName");
            //HFD_TelHome.Value = Util.GetQueryString("TelHome");
            //HFD_TelOffice.Value = Util.GetQueryString("TelOffice");
            //HFD_QryType.Value = Util.GetQueryString("QueryType"); 
           // LoadFormData();
           // LoadFirstFormData();
            txtName.Text = Util.GetQueryString("Name");
            HFD_MOD.Value = Util.GetQueryString("MOD");
            if (HFD_MOD.Value == "Qry")
            {
                btnCreate.Visible = true;
                Label1.Visible = true;
            }
            LoadDropDownListData();
            if (txtName.Text != "")
            {
                LoadFormData();
            }
        }
       
        LoadHiddenData();
    }
    //---------------------------------------------------------------------------
    public void LoadDropDownListData()
    {
        Util.FillCityData(ddlCity);
        Util.FillAreaData(ddlArea, ddlCity.SelectedValue);
    }
    //-------------------------------------------------------------


    public void LoadFormData()
    {
        string strSql = @"
                            select distinct a.uid , a.CName AS 姓名 ,a.Memo as 備註,REPLACE(a.phone,',','<BR>') AS 電話
                            from Member a  
                            Inner join Consulting b on  a.uid = b.MemberUID
                            where 1=1 and isnull(a.IsDelete, '') != 'Y'  and isnull(b.IsDelete, '') != 'Y' 
                          ";


        if (txtServiceUser.Text.Trim() != "")
        {
            strSql += " and ServiceUser = @ServiceUser\n";
        }

        if (HFD_Marry.Value != "")
        {
            strSql += " and Marry like @Marry\n";
        }

        if (txtEvent.Text.Trim() != "")
        {
            strSql += " and Event like @Event\n";
        }

        if (txtPhone.Text.Trim() != "")
        {
            strSql += " and Phone like @Phone\n";
        }

        if (txtName.Text.Trim() != "")
        {
            strSql += " and CName like @CName\n";
        }

        if (txtBegCreateDate.Text.Trim() != "" && txtEndCreateDate.Text.Trim() != "")
        {
            strSql += " and CONVERT(varchar(100), CreateDate, 111) Between @BegCreateDate And @EndCreateDate\n";
        }

        if (txtZipCode.Text.Trim() != "")
        {
            strSql += " And ZipCode=@ZipCode\n";
        }

        if (ddlCity.SelectedValue != "")
        {
            strSql += " And City=@City\n";
        }

        if (txtAddress.Text.Trim() != "")
        {
            strSql += " and Address like @Address\n";
        }

        if (HFD_ConsultantItem.Value != "")
        {
            strSql += " And ConsultantItem like @ConsultantItem\n";
        }

        strSql += " Order by CName desc";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("Event", "%" + txtEvent.Text.Trim() + "%");
        dict.Add("CName", "%" + txtName.Text.Trim() + "%");
        dict.Add("Marry", "%" + HFD_Marry.Value + "%");
        dict.Add("Phone", '%' + txtPhone.Text.Trim() + '%');
        dict.Add("BegCreateDate", txtBegCreateDate.Text.Trim());
        dict.Add("EndCreateDate", txtEndCreateDate.Text.Trim());
        dict.Add("ServiceUser", txtServiceUser.Text.Trim());
        //dict.Add("EditServiceUser", SessionInfo.UserID.ToString());
        dict.Add("ZipCode", txtZipCode.Text.Trim());
        dict.Add("City", ddlCity.SelectedValue);
        dict.Add("Address", "%" + txtAddress.Text.Trim() + "%");
        dict.Add("ConsultantItem", "%" + HFD_ConsultantItem.Value + "%");
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        
        int count = dt.Rows.Count;

        if (dt.Rows.Count == 0)
        {
          
            ShowSysMsg("查無資料");
            gdvDonorQry.Visible = false;
            return;

        }
        gdvDonorQry.Visible = true;
        gdvDonorQry.DataSource = dt;
        
        //gdvDonorQry.DataSource = dtAddCaseIDCol(dt);
        gdvDonorQry.DataBind();

    }

    public void LoadFirstFormData()
    {
        string strSql = @"
                            select distinct a.uid , a.CName AS 姓名 ,REPLACE(a.phone,',','<BR>' ) AS 電話
                            from Member a  
                            inner join Consulting b on  a.uid = b.MemberUID
                            where 1=1 and isnull(a.IsDelete, '') != 'Y' 
                           And a.uid in ('1005','1008','1013','1017','1026','1053','1070','909','933','936','965','974','978','982','995')
                          ";
                          
        strSql += " Order by uid ";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        int count = dt.Rows.Count;
        gdvDonorQry.DataSource = dt;
        gdvDonorQry.DataBind();

    }
    //---------------------------------------------------------------------------
    private DataTable dtAddCaseIDCol(DataTable dtSrc)
    {
        DataTable dtRet = dtSrc.Clone();
        dtRet.Columns.Add("來電案號");

        string strDonorID = "";
        for (int i = 0; i < dtSrc.Rows.Count; i++)
        {
            strDonorID = dtSrc.Rows[i]["Donor_Id"].ToString();

            string strSql = @"
                                select distinct donor_id, CaseID
                                from
                                (
					                select Donor_Id as [Donor_Id]
	                                        ,CaseID as [CaseID]  
                                    from AdvisoryCase A
                                    left join DONOR DP
                                    on A.Patient_Id =DP.Donor_Id 
                                    where 1=1
                                    and A.DeleteDate is null 

                                    union all
					                select Donor_Id as [Donor_Id]
	                                        ,CaseID as [CaseID]  
                                    from AdvisoryCase A
                                    left join DONOR DC
                                    on A.Contactor_Id =DC.Donor_Id 
                                    where 1=1
                                    and A.DeleteDate is null 
                                ) a 
                                where donor_Id=@DonorID
                                order by CaseID desc
                            ";
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("DonorID", strDonorID);

            DataTable dt = NpoDB.GetDataTableS(strSql, dict);

            string strCaseID = "";
            //取最新3案號
            for (int k = 0; k < dt.Rows.Count; k++)
            {
                if (k > 2)
                {
                    break;
                }
                strCaseID += dt.Rows[k]["CaseID"].ToString() + ",";
            }

            DataRow drNew = dtRet.NewRow();
            for (int j = 0; j < dtSrc.Columns.Count; j++)
            {
                    drNew[j] = dtSrc.Rows[i][j];
            }
            drNew["來電案號"] = strCaseID;
            dtRet.Rows.Add(drNew);
        }

        return dtRet;
    }
    //---------------------------------------------------------------------------
    protected void gdvDonorQry_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "btnSelect")
        {
            
            int index = Convert.ToInt32(e.CommandArgument);
            string uids = "";
            uids = gdvDonorQry.Rows[index].Cells[4].Text;
            //Label Label1 = (Label)e.Row.FindControl("Label1");
            //uids = Label1.Text;
            //this.Response.Write(@"<script>window.opener.$('#divData').show();self.opener.Form1.submit();window.close();</script>");
            Response.Write(@"<script>self.opener.document.getElementById('HFD_MemberID').value='" + uids + "';self.opener.document.getElementById('btnReload').click();window.close();</script>");
        }
    }
    private void LoadHiddenData()
    {
        List<ControlData> list = new List<ControlData>();
        list = new List<ControlData>();
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem1", "%經濟%", "經濟", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem2", "%情緒%", "情緒", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem3", "%信仰%", "信仰", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem4", "%人際%", "人際", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem5", "%婆媳%", "婆媳", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem6", "%翁媳%", "翁媳", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem7", "%妯娌%", "妯娌", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem8", "%手足%", "手足", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem9", "%職場%", "職場", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem10", "%教會%", "教會", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem11", "%溝通%", "溝通", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem12", "%性生活%", "性生活", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem13", "%精神疾病%", "精神疾病", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem14", "%身體疾病%", "身體疾病", true, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem15", "%個性%", "個性", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem16", "%外遇%", "外遇", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem17", "%暴力%", "暴力", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem18", "%教育%", "教育", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem19", "%霸凌%", "霸凌", false, HFD_ConsultantItem.Value));
        list.Add(new ControlData("Checkbox", "ConsultantItem", "CHK_ConsultantItem20", "%學業%", "學業", false, HFD_ConsultantItem.Value));
        lblConsultantItem.Text = HtmlUtil.RenderControl(list);
        //婚姻狀況
        list = new List<ControlData>();
        list.Add(new ControlData("Checkbox", "Marry", "CHK_Marry1", "%未婚%", "未婚", false, HFD_Marry.Value));
        list.Add(new ControlData("Checkbox", "Marry", "CHK_Marry2", "%已婚%", "已婚", false, HFD_Marry.Value));
        list.Add(new ControlData("Checkbox", "Marry", "CHK_Marry3", "%離婚%", "離婚", false, HFD_Marry.Value));
        list.Add(new ControlData("Checkbox", "Marry", "CHK_Marry4", "%喪偶%", "喪偶", false, HFD_Marry.Value));
        lblMarry.Text = HtmlUtil.RenderControl(list);
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        LoadFormData();
    }
    protected void btnCreate_Click(object sender, EventArgs e)
    {
        Response.Write(@"<script>self.opener.document.getElementById('HFD_Create').value='N';window.close();</script>");
    }
}
