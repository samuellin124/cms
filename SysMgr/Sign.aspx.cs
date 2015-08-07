using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Threading;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Net.Mail;

public partial class SysMgr_Sign :  BasePage 
{
    GroupAuthrity ga = null;
    string Name = "";
    #region NpoGridView 處理換頁相關程式碼
    Button btnNextPage, btnPreviousPage, btnGoPage;
    HiddenField HFD_CurrentPage;
    //排序用
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
    #endregion NpoGridView 處理換頁相關程式碼
    protected void Page_Load(object sender, EventArgs e)
    {
        lblHeaderText.Text = "報名";
        Util.ShowSysMsgWithScript("$('#btnPreviousPage').hide();$('#btnNextPage').hide();$('#btnGoPage').hide();");
        if (!IsPostBack)
        {
            HFD_UID.Value = Util.GetQueryString("uid");
            
            getUserUid();

            //載入下拉式選單資料
            LoadDropDownListData();
            //載入資料
            LoadFormData();
        }
    }
    //-------------------------------------------------------------------------
    private void SetupDefaultValue()
    {
    }
    //-------------------------------------------------------------------------
    private void SetButton()
    {
    }
    //-------------------------------------------------------------------------
    private void AuthrityControl()
    {
    }
    //-------------------------------------------------------------------------
    public void LoadDropDownListData()
    {
        //Util.FillCityData(ddlCity);
        //Util.FillAreaData(ddlArea, ddlCity.SelectedValue);
    }
    //-------------------------------------------------------------------------------------------------------------
    private void getUserUid()
    {
        string strSql = @"
                        SELECT uid  FROM AdminUser
                        where 1=1
                        and UserID=@UserID
                    ";
        
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("UserID", SessionInfo.UserID);
        DataRow dr = null;
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);

        if (dt.Rows.Count == 0)
        {

            ShowSysMsg("查無資料");
            return;
        }
        else
        {
            dr = dt.Rows[0];
            HFD_UserUid.Value = dr["uid"].ToString();
        }
    }                                             
    //-------------------------------------------------------------------------------------------------------------
    private void LoadFormData()
    {
        string strSql = @"
                        SELECT DISTINCT a.uid AS uid,  ClassName as 類別 ,  (itemname +'('+ Remark + ')') AS 項目,CONVERT(NVARCHAR(12),EndDate,111) as 結束時間 ,Places 
                        ,(SELECT COUNT(*) FROM  ActMap WHERE ActUid =a.uid and  ISNULL(IsDelete ,'') != 'Y' ) AS 人數
                        , CAST('TRUE' as bit)  as selected
                        FROM Act a
                        left JOIN ActMap a2 ON a2.ActUid = a.uid  
                        INNER JOIN ServiceItem s ON a.ItemUid = s.uid 
                        INNER JOIN ServiceClass s2 ON s2.uid = s.ClassUid 
                        WHERE ISNULL(a2.IsDelete ,'') != 'Y' AND ISNULL(a.IsDelete ,'') != 'Y'  AND  a.uid IN (SELECT a.uid FROM Act a INNER JOIN ActMap b ON a.uid = b.ActUid WHERE  ISNULL(b.IsDelete ,'') != 'Y'  AND b.UserID =@UserID)
                        UNION 
                        SELECT  DISTINCT a.uid AS uid,  ClassName as 類別 , (itemname +'('+ Remark + ')') AS 項目,CONVERT(NVARCHAR(12),EndDate,111) as 結束時間  ,Places 
                        ,(SELECT COUNT(*) FROM  ActMap WHERE ActUid =a.uid and  ISNULL(IsDelete ,'') != 'Y' ) AS 人數
                        , CAST('FALSE' as bit)   as selected
                        FROM Act a
                        left JOIN ActMap a2 ON a2.ActUid = a.uid 
                        INNER JOIN ServiceItem s ON a.ItemUid = s.uid 
                        INNER JOIN ServiceClass s2 ON s2.uid = s.ClassUid 
                        WHERE    ISNULL(a.IsDelete ,'') != 'Y' And  a.uid not IN (SELECT a.uid FROM Act a INNER JOIN ActMap b ON a.uid = b.ActUid WHERE  ISNULL(b.IsDelete ,'') != 'Y'  AND b.UserID =@UserID)
                        ";
        Dictionary<string, object> dict = new Dictionary<string, object>();

        dict.Add("UserID", HFD_UserUid.Value);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        gdvDonorQry.DataSource = dt;
        gdvDonorQry.DataBind();


       

        for (int i = 0; i <= gdvDonorQry.Rows.Count - 1; i++)
        {
            CheckBox cbox = (CheckBox)gdvDonorQry.Rows[i].FindControl("CheckBox1");
            Label label1 = (Label)gdvDonorQry.Rows[i].FindControl("Label1");


            //***********************************************************************
            DateTime time1 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            DateTime time2 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")); ;
          //  DateTime time2 = Convert.ToDateTime(dt.Rows[i]["結束時間"]);
            if (dt.Rows[i]["結束時間"].ToString() != "")
            {

                time2 = Convert.ToDateTime(Util.DateTime2String(dt.Rows[i]["結束時間"].ToString(), DateType.yyyyMMdd, EmptyType.ReturnEmpty));
            }
            
             if (int.Parse(dt.Rows[i]["人數"].ToString())   >= int.Parse(dt.Rows[i]["Places"].ToString()))
             {
                  cbox.Text = "已額滿";
                  cbox.Enabled = false;
             }
                
            if (DateTime.Compare(time1, time2) > 0) //判断日期大小
            {
                cbox.Text = "截止報名";
                cbox.Enabled = false;
            }
            else
            {
            }
            //**************************************************************

        }
        //GridView1.DataSource = dt;
        //GridView1.DataBind();
    }
    //-------------------------------------------------------------------------------------------------------------
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        List<ColumnData> list = new List<ColumnData>();
        
        string[] strArray;
        string uids = "";
        string strSql = "";
        foreach (string str in Request.Form.Keys)
        {
            if (str.StartsWith("chkSelectLand_"))
            {
               
                //strArray = str.Split('_');
                ////uids += strArray[1] + ",";
                //HFD_ActUid.Value = strArray[1].ToString();

                SetColumnData(list);
                strSql = Util.CreateInsertCommand("ActMap", list, dict);
                NpoDB.ExecuteSQLS(strSql, dict);
            }
        }
        //uids = uids.TrimEnd(',');
        //HFD_ChurchUid.Value = uids;
        int iUid = uids.IndexOf(",");
        //if (iUid != -1)
        //{
        //    ShowSysMsg("此處為單選，請確認!");
        //    return;
        //}

         strSql = @"
                            update  ActMap
                            set IsDelete='Y'
                            Where 1=1
                            And uid =@uid
                            ";
        dict.Add("UserID", HFD_UserUid .Value);

        if (HFD_UserUid.Value != "")
        {
            NpoDB.ExecuteSQLS(strSql, dict);
        }


      

        
        if (uids.ToString() != "")
        {
       //     
        }
        ShowSysMsg("儲存資料成功!");
        //this.Response.Write(@"<script>self.opener.LoadChurchMember('LoadGird');window.close();</script>");
    }
    //-------------------------------------------------------------------------------------------------------------
    protected void btnExit_Click(object sender, EventArgs e)
    {
       // Response.Redirect("ChangeChurchQry.aspx");
    }
    //-------------------------------------------------------------------------------------------------------------
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        LoadFormData();
    }

    //-------------------------------------------------------------------------------------------------------------
 
 
    private void SetColumnData(List<ColumnData> list)
    {
        ////Key
        //list.Add(new ColumnData("Uid", HFD_UID.Value, false, false, true));
        //活動Uid
        list.Add(new ColumnData("ActUid",HFD_ActUid.Value,true, true, false ));

        //志工UID            //SessionInfo.UserID
        list.Add(new ColumnData("UserID", HFD_UserUid.Value , true, true, false));
    }
    //--------------------------------------------------------------------------
 
  
    protected void btnGridView_Click(object sender, EventArgs e)
    {
        string strSql = @"
                        SELECT DISTINCT a.uid AS uid,  ClassName as 類別 , itemname AS 項目,EndDate 結束時間 
                        ,(SELECT COUNT(*) FROM  ActMap WHERE ActUid =a.uid ) AS 人數
                        , CAST('TRUE' as bit)  as selected
                        FROM Act a
                        left JOIN ActMap a2 ON a2.ActUid = a.uid  
                        INNER JOIN ServiceItem s ON a.ItemUid = s.uid 
                        INNER JOIN ServiceClass s2 ON s2.uid = s.ClassUid 
                        WHERE ISNULL(a2.IsDelete ,'') != 'Y'  AND  a.uid IN (SELECT a.uid FROM Act a INNER JOIN ActMap b ON a.uid = b.ActUid WHERE  ISNULL(b.IsDelete ,'') != 'Y'  AND b.UserID =@UserID)
                        UNION 
                        SELECT  DISTINCT a.uid AS uid,  ClassName as 類別 , itemname AS 項目,EndDate 結束時間 
                        ,(SELECT COUNT(*) FROM  ActMap WHERE ActUid =a.uid ) AS 人數
                        , CAST('FALSE' as bit)   as selected
                        FROM Act a
                        left JOIN ActMap a2 ON a2.ActUid = a.uid 
                        INNER JOIN ServiceItem s ON a.ItemUid = s.uid 
                        INNER JOIN ServiceClass s2 ON s2.uid = s.ClassUid 
                        WHERE    a.uid not IN (SELECT a.uid FROM Act a INNER JOIN ActMap b ON a.uid = b.ActUid WHERE  ISNULL(b.IsDelete ,'') != 'Y'  AND b.UserID =@UserID)
                        ";
        Dictionary<string, object> dict = new Dictionary<string, object>();

        dict.Add("UserID", HFD_UserUid.Value);
        //dict.Add("City", ddlCity.SelectedValue);
        //dict.Add("ConsultantItem", "%" + HFD_ConsultantItem.Value + "%");
        //Response.Write(strSql);
       // Response.End();
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        gdvDonorQry.DataSource = dt;
        gdvDonorQry.DataBind();

        for (int i = 0; i <= gdvDonorQry.Rows.Count - 1; i++)
        {
            CheckBox cbox = (CheckBox)gdvDonorQry.Rows[i].FindControl("CheckBox1");
            Label label1 = (Label)gdvDonorQry.Rows[i].FindControl("Label1");
          
            if (cbox.Checked == true)
            {
                //cbox.Text = "";
               // label1.Text = dt.Rows[i]["人數"].ToString();
            }
        }
        //GridView1.DataSource = dt;
        //GridView1.DataBind();
        
    }

    protected void gdvDonorQry_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "btnSelect")
        {

            int index = Convert.ToInt32(e.CommandArgument);
            string uids = "";
            uids = gdvDonorQry.Rows[index].Cells[4].Text;

            CheckBox cbox = (CheckBox)gdvDonorQry.Rows[index].FindControl("CheckBox1");
            Label label1 = (Label)gdvDonorQry.Rows[index].FindControl("Label1");
         

         
                //for (int i = 0; i <= gdvDonorQry.Rows.Count - 1; i++)
                //{
                //    //CheckBox cbox = (CheckBox)gdvDonorQry.Rows[i].FindControl("CheckBox1");
                //    //Label label1 = (Label)gdvDonorQry.Rows[i].FindControl("Label1");
                //    if (cbox.Checked == true)
                //    {
                //        label1.Text = "已選";
                //    }
                //    else
                //    {

                //    }
                //}
           
           // Label Label1 = (Label)e.Row.FindControl("Label1");
           // uids = Label1.Text;
           // this.Response.Write(@"<script>window.opener.$('#divData').show();self.opener.Form1.submit();window.close();</script>");
           // Response.Write(@"<script>self.opener.document.getElementById('HFD_MemberID').value='" + uids + "';self.opener.document.getElementById('btnReload').click();window.close();</script>");
        }
    }
 
    protected void btnGvSelect_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        Dictionary<string, object> dict2 = new Dictionary<string, object>();
        List<ColumnData> list = new List<ColumnData>();
        string strSql = "";


          strSql = @"
                            update  ActMap
                            set IsDelete='Y'
                            Where 1=1
                            And UserID =@UserID
                            ";   
               dict.Add("UserID", HFD_UserUid.Value);
              

                if (HFD_UserUid.Value != "")
                {
                    NpoDB.ExecuteSQLS(strSql, dict);
                }


        for (int i = 0; i <= gdvDonorQry.Rows.Count - 1; i++)
        {
            CheckBox cbox = (CheckBox)gdvDonorQry.Rows[i].FindControl("CheckBox1");
            Label label1 = (Label)gdvDonorQry.Rows[i].FindControl("Label1");
            HFD_ActUid.Value = gdvDonorQry.Rows[i].Cells[5].Text;


            if (cbox.Checked == true)
            {
                //label1.Text = "已選";
               // string uids = "";
              
                //txtName.Text = txtName.Text + uids + ",";
              
                if (HFD_UserUid.Value != "")
                {
                    //SetColumnData(list);
                  //  strSql = Util.CreateInsertCommand("ActMap", list, dict2);
                    strSql = "INSERT INTO ActMap (ActUid ,UserID) VALUES ('"+ HFD_ActUid.Value  +"','"+ HFD_UserUid.Value +"')";
                   // dict.Add("ActUid", HFD_ActUid.Value);
                    NpoDB.ExecuteSQLS(strSql, dict);

                }
            }
            else
            {

            }
        }
        ShowSysMsg("儲存資料成功!");
    }


}
