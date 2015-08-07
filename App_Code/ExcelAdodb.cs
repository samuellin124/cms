using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.OleDb;

/// <summary>
/// Summary description for ExcelAdodb
/// </summary>
public class ExcelAdodb
{
    public string ExcelFilePath;
    private OleDbConnection OleDbConn;
    private string ConnectionString;

    //�غc�l�A���o�ɮצ�m�A�N�i�H�}�l���ާ@�F�C
    public ExcelAdodb(string filepath)
    {
        this.ExcelFilePath = filepath;
        this.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                                "Data Source=" + this.ExcelFilePath + ";" +
                                "Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
        this.OleDbConn = new OleDbConnection(this.ConnectionString);
    }
    //���o��ơA�ˤJ�y�k�A�N�o��۹諸��ƪ�
    //string strSQL = "SELECT * FROM [Sheet1$]";�C
    public DataTable GetTable(string strSQL)
    {       
        DataSet myDataset = new DataSet();
        OleDbDataAdapter myData = new OleDbDataAdapter(strSQL, this.OleDbConn);
        myData.TableMappings.Add("Table", "ExcelTest");
        myData.Fill(myDataset);
        return myDataset.Tables[0];
    }
    public void CloseConnection()
    {
        this.OleDbConn.Close();
        this.OleDbConn.Dispose();
    }

}

