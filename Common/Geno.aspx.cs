using System;
using System.Collections.Generic;

public partial class Common_Geno : BasePage 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HFD_Uid.Value = Util.GetQueryString("Uid");
            HFD_TableName.Value = Util.GetQueryString("TableName");
            HFD_FieldName.Value = Util.GetQueryString("FieldName");

            string XML = NpoDB.GetScalarS("select " + HFD_FieldName.Value + " from " + HFD_TableName.Value + " where uid = '" + HFD_Uid.Value + "' ", null);

            HFD_XML.Value = XML;
        }
    }

    protected void btn_SaveXML_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        string strSql;

        strSql = " update " + HFD_TableName.Value + " set ";
        strSql += " " + HFD_FieldName.Value + " = @xml";
        strSql += " where uid = @uid";

        dict.Add("uid", HFD_Uid.Value);
        dict.Add("xml", HFD_XML.Value);

        NpoDB.ExecuteSQLS(strSql, dict);
        Session["Msg"] = "¶s¿…¶®•\";
        ShowSysMsg();
    }
}