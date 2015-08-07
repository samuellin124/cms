using System;
using System.Collections.Generic;

public partial class Common_GenoCaseID : BasePage 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HFD_CaseID.Value = Util.GetQueryString("CaseID");
            HFD_EvaluateID.Value = Util.GetQueryString("EvaluateID");
            HFD_TableName.Value = Util.GetQueryString("TableName");
            HFD_FieldName.Value = Util.GetQueryString("FieldName");

            string strSQL = "select " + HFD_FieldName.Value + " from " + HFD_TableName.Value;
            strSQL += " where CaseID = '" + HFD_CaseID.Value + "' ";
            if (HFD_EvaluateID.Value != "")
            {
                strSQL += " and EvaluateID='" + HFD_EvaluateID.Value + "'";
            }

            string XML = NpoDB.GetScalarS(strSQL, null);

            HFD_XML.Value = XML;
        }
    }

    protected void btn_SaveXML_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        string strSql;

        strSql = " update " + HFD_TableName.Value + " set ";
        strSql += " " + HFD_FieldName.Value + " = @xml";
        strSql += " where CaseID = @CaseID";
        if (HFD_EvaluateID.Value != "")
        {
            strSql += " and EvaluateID=@EvaluateID";
        }

        dict.Add("CaseID", HFD_CaseID.Value);
        dict.Add("EvaluateID", HFD_EvaluateID.Value);
        dict.Add("xml", HFD_XML.Value);

        NpoDB.ExecuteSQLS(strSql, dict);
        Session["Msg"] = "¶s¿…¶®•\";
        ShowSysMsg();
    }
}