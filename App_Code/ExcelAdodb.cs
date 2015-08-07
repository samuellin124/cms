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

    //建構子，取得檔案位置，就可以開始做操作了。
    public ExcelAdodb(string filepath)
    {
        this.ExcelFilePath = filepath;
        this.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                                "Data Source=" + this.ExcelFilePath + ";" +
                                "Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
        this.OleDbConn = new OleDbConnection(this.ConnectionString);
    }
    //取得資料，裝入語法，就得到相對的資料表
    //string strSQL = "SELECT * FROM [Sheet1$]";。
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

