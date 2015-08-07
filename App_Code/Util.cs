using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web;
using System.Web.SessionState;


/// <summary>
/// Summary description for util
/// </summary>

public enum DateType
{
    yyyyMMdd,
    yyyyMMddHHmmss,
    CyyyMMdd,
    CyyyMMddHHmmss,
    HHmmss
}
public enum EmptyType
{
    ReturnEmpty,
    ReturnNull
}
public enum WeekType
{
    Long, //星期三
    Short //三
}

public class Util
{
    public static HttpSessionState Session { get { return HttpContext.Current.Session; } }
    public static HttpRequest Request { get { return HttpContext.Current.Request; } }
    public static HttpResponse Response { get { return HttpContext.Current.Response; } }
    public static HttpServerUtility Server { get { return HttpContext.Current.Server; } }
    public static Page page { get { return HttpContext.Current.CurrentHandler as Page; } }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 字串轉數字(int), 如果無法轉數字, 則轉為 0
    /// </summary>
    public static int String2Number(string strNumber)
    {
        if (strNumber == "")
        {
            return 0;
        }
        strNumber = strNumber.Replace(",", "");

        int retValue = 0;
        try
        {
            retValue = Convert.ToInt32(strNumber);
        }
        catch (Exception)
        {
            retValue = 0;
        }
        return retValue;
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 字串轉數字(double), 如果無法轉數字, 則轉為 0
    /// </summary>
    public static double String2Double(string strNumber)
    {
        if (strNumber == "")
        {
            return 0;
        }
        strNumber = strNumber.Replace(",", "");

        double retValue = 0;
        strNumber = strNumber.Replace(",", "");
        try
        {
            retValue = Convert.ToDouble(strNumber);
        }
        catch (Exception)
        {
            retValue = 0;
        }
        return retValue;
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 數字字串加 1, 如果無法轉數字, 則轉為 0
    /// </summary>
    public static string AddStringNumber(string strNumber)
    {
        int retValue = 0;
        try
        {
            retValue = Convert.ToInt32(strNumber);
            retValue++;
        }
        catch (Exception)
        {
            retValue = 0;
        }
        return retValue.ToString();
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 數字字串加1個指定的數值, 如果無法轉數字, 則轉為 0
    /// </summary>
    public static string AddStringNumber(string strNumber, int AddNumber)
    {
        int retValue = 0;
        try
        {
            retValue = Convert.ToInt32(strNumber);
            retValue += AddNumber;
        }
        catch (Exception)
        {
            retValue = 0;
        }
        return retValue.ToString();
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 數字字串減 1, 如果無法轉數字, 則轉為 0
    /// </summary>
    public static string MinusStringNumber(string strNumber)
    {
        int retValue = 0;
        try
        {
            retValue = Convert.ToInt32(strNumber);
            retValue--;
        }
        catch (Exception)
        {
            retValue = 0;
        }
        return retValue.ToString();
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 數字字串減1個指定的數值, 如果無法轉數字, 則轉為 0
    /// </summary>
    public static string MinusStringNumber(string strNumber, int MinusNumber)
    {
        int retValue = 0;
        try
        {
            retValue = Convert.ToInt32(strNumber);
            retValue -= MinusNumber;
        }
        catch (Exception)
        {
            retValue = 0;
        }
        return retValue.ToString();
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 得到字串長度,英文字算 1,中文字算 2
    /// </summary>
    public static int GetStrLength(string strData)
    {
        int nLength = 0;
        for (int i = 0; i < strData.Length; i++)
        {
            if (strData[i] >= 0x3000 && strData[i] <= 0x9FFF)
            {
                nLength += 2;
            }
            else
            {
                nLength++;
            }
        }
        return nLength;
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 得到固定長度字串, 右邊未滿部分以空白代替
    /// </summary>
    public static string GetRightPaddedString(string strData, int iPadLen)
    {
        int iDiffLen; //為了計算要 pad 的空格,中英文長度不一樣,所以要先計算
        iDiffLen = GetStrLength(strData) - strData.Length;
        if (iPadLen - iDiffLen < 0)
        {
            return strData;
        }
        return strData.PadRight(iPadLen - iDiffLen, ' ');
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 得到固定長度字串, 左邊未滿部分以空白代替
    /// </summary>
    public static string GetLeftPaddedString(string strData, int iPadLen)
    {
        int iDiffLen; //為了計算要 pad 的空格,中英文長度不一樣,所以要先計算
        iDiffLen = GetStrLength(strData) - strData.Length;
        if (iPadLen - iDiffLen < 0)
        {
            return strData;
        }
        return strData.PadLeft(iPadLen - iDiffLen, ' ');
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 以 start 與 end 為界,產生連續數值的 DropDownList
    /// </summary>
    public static void FillDropDownList(DropDownList ddl, int iStart, int iEnd, bool addEmpty, int iDefault)
    {
        ddl.Items.Clear();
        if (addEmpty == true)
        {
            ListItem newitem = new ListItem();
            newitem.Text = "";
            newitem.Value = "";
            ddl.Items.Add(newitem);
        }
        for (int i = iStart; i <= iEnd; i++)
        {
            ListItem newitem = new ListItem();
            newitem.Text = i.ToString();
            newitem.Value = i.ToString();
            ddl.Items.Add(newitem);
        }
        if (iDefault != -1)
        {
            ddl.SelectedValue = iDefault.ToString();
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    public static void FillDropDownList(DropDownList ddl, int iStart, int iEnd, int iDefault, bool PadLeft, int Width, char PadChar)
    {
        ddl.Items.Clear();
        for (int i = iStart; i <= iEnd; i++)
        {
            System.Web.UI.WebControls.ListItem newitem = new System.Web.UI.WebControls.ListItem();
            if (PadLeft == true)
            {
                newitem.Text = i.ToString().PadLeft(Width, PadChar);
                newitem.Value = i.ToString();
            }
            else
            {
                newitem.Text = i.ToString();
                newitem.Value = i.ToString();
            }
            ddl.Items.Add(newitem);
        }
        if (iDefault != -1)
        {
            ddl.SelectedValue = iDefault.ToString();
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    public static void FillDropDownList(DropDownList ddl, int iStart, int iEnd, bool addEmpty, int iDefault, bool PadLeft = false, int Width = 0, char PadChar = '0')
    {
        ddl.Items.Clear();
        if (addEmpty == true)
        {
            ListItem newitem = new ListItem();
            newitem.Text = "";
            newitem.Value = "";
            ddl.Items.Add(newitem);
        }
        for (int i = iStart; i <= iEnd; i++)
        {
            ListItem newitem = new ListItem();
            if (PadLeft == true)
            {
                newitem.Text = i.ToString().PadLeft(Width, PadChar);
                newitem.Value = i.ToString();
            }
            else
            {
                newitem.Text = i.ToString();
                newitem.Value = i.ToString();
            }
            ddl.Items.Add(newitem);
        }
        if (iDefault != -1)
        {
            ddl.SelectedValue = iDefault.ToString();
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 以傳入的 array 為內容產生 DropDownList
    /// </summary>
    public static void FillDropDownList(DropDownList ddl, string[] arText, string[] arValue)
    {
        ddl.Items.Clear();
        for (int i = 0; i < arText.Length; i++)
        {
            ListItem newitem = new ListItem();
            newitem.Text = arText[i];
            newitem.Value = arValue[i];
            ddl.Items.Add(newitem);
        }
        ddl.SelectedIndex = 0;
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 以傳入的 SQL command 為內容產生 DropDownList
    /// </summary>
    public static void FillDropDownList(DropDownList ddl, string strSql, string TextField, string ValueField, bool AddEmpty)
    {
        DataTable dt = NpoDB.GetDataTableS(strSql, null);

        ddl.Items.Clear();
        ListItem newitem;
        if (AddEmpty == true)
        {
            newitem = new ListItem();
            newitem.Text = "";
            newitem.Value = "";
            ddl.Items.Add(newitem);
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            newitem = new ListItem();
            newitem.Text = dt.Rows[i][TextField].ToString();
            newitem.Value = dt.Rows[i][ValueField].ToString();
            ddl.Items.Add(newitem);
        }
        ddl.SelectedIndex = 0;
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 以傳入的 SQL command 為內容產生 DropDownList,  有 EmptyValue, EmptyText 參數
    /// </summary>
    public static void FillDropDownList(DropDownList ddl, string strSql, string TextField, string ValueField, bool AddEmpty, string EmptyValue, string EmptyText)
    {
        DataTable dt = NpoDB.GetDataTableS(strSql, null);

        ddl.Items.Clear();
        ListItem newitem;
        if (AddEmpty == true)
        {
            newitem = new ListItem();
            newitem.Text = EmptyText;
            newitem.Value = EmptyValue;
            ddl.Items.Add(newitem);
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            newitem = new ListItem();
            newitem.Text = dt.Rows[i][TextField].ToString();
            newitem.Value = dt.Rows[i][ValueField].ToString();
            ddl.Items.Add(newitem);
        }
        ddl.SelectedIndex = 0;
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 以傳入的 DataTable 為內容產生 DropDownList
    /// </summary>
    public static void FillDropDownList(DropDownList ddl, DataTable dt, string TextField, string ValueField, bool AddEmpty)
    {
        ddl.Items.Clear();
        ListItem newitem;
        if (AddEmpty == true)
        {
            newitem = new ListItem();
            newitem.Text = "";
            newitem.Value = "";
            ddl.Items.Add(newitem);
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            newitem = new ListItem();
            newitem.Text = dt.Rows[i][TextField].ToString();
            newitem.Value = dt.Rows[i][ValueField].ToString();
            ddl.Items.Add(newitem);
        }
        if (ddl.Items.Count > 0)
        {
            ddl.SelectedIndex = 0;
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 以傳入的 DataTable 為內容產生 DropDownList, 有 EmptyValue, EmptyText 參數
    /// </summary>
    public static void FillDropDownList(DropDownList ddl, DataTable dt, string TextField, string ValueField, bool AddEmpty, string EmptyValue, string EmptyText)
    {
        ddl.Items.Clear();
        ListItem newitem;
        if (AddEmpty == true)
        {
            newitem = new ListItem();
            newitem.Text = EmptyText;
            newitem.Value = EmptyValue;
            ddl.Items.Add(newitem);
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            newitem = new ListItem();
            newitem.Text = dt.Rows[i][TextField].ToString();
            newitem.Value = dt.Rows[i][ValueField].ToString();
            ddl.Items.Add(newitem);
        }
        if (ddl.Items.Count > 0)
        {
            ddl.SelectedIndex = 0;
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 以傳入的 DataTable 為內容產生 ListBox
    /// </summary>
    public static void FillListBox(ListBox lst, DataTable dt, string TextField, string ValueField, bool AddEmpty)
    {
        lst.Items.Clear();
        ListItem newitem;
        if (AddEmpty == true)
        {
            newitem = new ListItem();
            newitem.Text = "";
            newitem.Value = "";
            lst.Items.Add(newitem);
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            newitem = new ListItem();
            newitem.Text = dt.Rows[i][TextField].ToString();
            newitem.Value = dt.Rows[i][ValueField].ToString();
            lst.Items.Add(newitem);
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 以傳入的值(Text 或 Value)定位DropDownList
    /// </summary>
    public static int SetDdlIndex(DropDownList ddl, string value)
    {
        int iRet = GetDdlIndex(ddl, value);
        ddl.SelectedIndex = iRet;
        return iRet;
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 以傳入的值找 DropDownList 的 index
    /// </summary>
    public static int GetDdlIndex(DropDownList ddl, string sItem)
    {
        for (int i = 0; i < ddl.Items.Count; i++)
        {
            if (sItem.Trim() == ddl.Items[i].Text.Trim() || sItem.Trim() == ddl.Items[i].Value.Trim())
            {
                return i;
            }
        }
        return -1;
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 計算 HtmlTable 某個 Col 的加總
    /// </summary>
    public static string CalcuCols(HtmlTableRow row, int ColStart, int ColEnd)
    {
        int iTotal = 0;
        for (int i = ColStart; i <= ColEnd; i++)
        {
            iTotal += String2Number(row.Cells[i].InnerText);
        }
        return iTotal.ToString();
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 計算 HtmlTable 某個 Col 的加總, 有 Col step 參數
    /// </summary>
    public static string CalcuCols(HtmlTableRow row, int ColStart, int ColEnd, int Step)
    {
        int iTotal = 0;
        for (int i = ColStart; i <= ColEnd; i += Step)
        {
            iTotal += String2Number(row.Cells[i].InnerText);
        }
        return iTotal.ToString();
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 計算 HtmlTable 某個 Row 的加總
    /// </summary>
    public static string CalcuRows(HtmlTable table, int Col, int RowStart, int RowEnd)
    {
        int iTotal = 0;
        for (int i = RowStart; i <= RowEnd; i++)
        {
            iTotal += String2Number(table.Rows[i].Cells[Col].InnerText);
        }
        return iTotal.ToString();
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 取得 Form 上 Control 的值, 沒有則傳回空字串
    /// </summary>
    public static string GetControlValue(string ItemName)
    {
        string retString = "";
        try
        {
            retString = page.Request.Form[ItemName].ToString();
        }
        catch
        {
            retString = "";
        }
        return retString;
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 取得 QueryString 的值, 沒有則傳回空字串
    /// </summary>
    public static string GetQueryString(string ItemName)
    {
        string retString = "";
        try
        {
            retString = page.Request.QueryString[ItemName].ToString();
        }
        catch
        {
            retString = "";

        }
        return retString;
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 產生 Insert 的 SQL 指令
    /// </summary>
    public static string CreateInsertCommand(string tableName, List<ColumnData> list, Dictionary<string, object> dict)
    {
        string ReturnStr = "";
        //欄位寫入值
        WriteDict(list, dict);

        string str1 = "";
        string str2 = "";
        ReturnStr += "insert into " + tableName + "(";
        int iCount = list.Count;
        for (int i = 0; i < iCount; i++)
        {
            ColumnData item = list[i];
            if (item.AddFlag == false)
            {
                continue;
            }
            str1 += item.ColumnName;
            str2 += "@" + item.ColumnName;
            //if (i != iCount - 1)
            //{
                str1 += ",";
                str2 += ",";
            //}
        }
        str1 = TrimLastChar(str1, ',');
        str2 = TrimLastChar(str2, ',');

        return ReturnStr + str1 + ") values (" + str2 + ")";
    } //end of CreateInsertCommand()
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 產生 Update 的 SQL 指令
    /// </summary>
    public static string CreateUpdateCommand(string tableName, List<ColumnData> list, Dictionary<string, object> dict)
    {
        string ReturnStr = "";
        //欄位寫入值
        WriteDict(list, dict);

        ReturnStr += "update " + tableName + " set \n";
        int iCount = list.Count;
        for (int i = 0; i < iCount; i++)
        {
            ColumnData item = list[i];
            if (item.UpdateFlag == false)
            {
                continue;
            }
            ReturnStr += item.ColumnName + "=@" + item.ColumnName;
            if (i != iCount - 1)
            {
                ReturnStr += ",";
            }
        }
        ReturnStr = TrimLastChar(ReturnStr, ',');
        ReturnStr += " where 1=1 ";
        bool hasError = true;
        for (int i = 0; i < iCount; i++)
        {
            ColumnData item = list[i];
            if (item.ConditionFlag == true)
            {
                ReturnStr += " and " + item.ColumnName + "=@" + item.ColumnName;
                hasError = false;
            }
        }
        if (hasError == true) //2012/09/19,如果沒有條件設定,不能往下做,否則資料庫會全部更新
        {
            ShowMsg("UpdateCommand 沒有條件設定!");
            return "沒有條件設定";
        }
        return ReturnStr;
    } //end of CreateUpdateCommand()
    //-------------------------------------------------------------------------------------------------------------
    //欄位寫入值
    private static void WriteDict(List<ColumnData> list, Dictionary<string, object> dict)
    {
        int iCount = list.Count;
        dict.Clear();
        for (int i = 0; i < iCount; i++)
        {
            ColumnData item = list[i];
            dict.Add(item.ColumnName, item.ColumnValue);
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 刪除字串最後一個字
    /// </summary>
    public static string TrimLastChar(string str)
    {
        if (str != null && str.Length > 0)
        {
            str = str.Substring(0, str.Length - 1);
        }
        return str;
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 刪除字串最後一個字, 有指定字元
    /// </summary>
    public static string TrimLastChar(string str, char ch)
    {
        if (str != null && str.Length > 0)
        {
            if (str.EndsWith(ch.ToString()))
            {
                str = str.Substring(0, str.Length - 1);
            }
        }
        return str;
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 檢查字串內容是否為數字
    /// </summary>
    public static bool IsNumeric(string number)
    {
        try
        {
            Single.Parse(number);
            return true;
        }
        catch
        {
            return false;
        }
    }
    //-------------------------------------------------------------------------
    /// <summary>
    /// 移除 array 中重複的項目
    /// </summary>
    public static string[] RemoveDuplicates(string[] myStringArray)
    {
        List<string> myStringList = new List<string>();
        foreach (string s in myStringArray)
        {
            if (s == "")
            {
                continue;
            }
            if (!myStringList.Contains(s))
            {
                myStringList.Add(s);
            }
        }
        return myStringList.ToArray();
    }
    //-------------------------------------------------------------------------
    /// <summary>
    /// 移除以逗點為分格的 string 中重複的項目
    /// </summary>
    public static string[] RemoveDuplicates(string myString)
    {
        string[] myStringArray = myString.Split(',');
        string retStr = "";

        List<string> myStringList = new List<string>();
        foreach (string s in myStringArray)
        {
            if (s == "")
            {
                continue;
            }
            if (!myStringList.Contains(s))
            {
                myStringList.Add(s);
            }
        }
        return myStringList.ToArray();
    }
    //-------------------------------------------------------------------------
    /// <summary>
    ///取得某個日期當月的第一天
    /// </summary>
    public static DateTime GetStartDateOfMonth(DateTime d)
    {
        int Year = d.Year;
        int numberOfDays = DateTime.DaysInMonth(d.Year, d.Month);
        return new DateTime(d.Year, d.Month, 1);
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///取得某個日期當月的最後一天
    /// </summary>
    public static DateTime GetEndDateOfMonth(DateTime d)
    {
        int numberOfDays = DateTime.DaysInMonth(d.Year, d.Month);
        return new DateTime(d.Year, d.Month, numberOfDays, 23, 59, 59);
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///取得某個日期當天的最後時間
    /// </summary>
    public static string GetEndDateOfDay(string strDate)
    {
        try
        {
            DateTime d = Convert.ToDateTime(strDate);
            DateTime d2 = new DateTime(d.Year, d.Month, d.Day, 23, 59, 59);
            return DateTime2String(d2, DateType.yyyyMMddHHmmss, EmptyType.ReturnEmpty);
        }
        catch (Exception ex)
        {
            return strDate;
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///將月日的值一律轉為 2 位數
    /// </summary>
    public static string NormalizeDate(string strDate)
    {
        if (strDate.Trim() == "")
        {
            return "";
        }
        string NormalDate = "";
        try
        {
            DateTime d = Convert.ToDateTime(strDate);
            NormalDate = d.Year.ToString().PadLeft(4, '0') + "/" +
                         d.Month.ToString().PadLeft(2, '0') + "/" +
                         d.Day.ToString().PadLeft(2, '0');
        }
        catch (Exception ex)
        {
        }
        return NormalDate;
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///取得某個月有幾天
    /// </summary>
    public static int GetNumberOfDays(DateTime d)
    {
        return DateTime.DaysInMonth(d.Year, d.Month);
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 以 SQL command 為來源, 取前兩個欄位組成 List of list
    /// </summary>
    public static List<List<string>> GetListFromDB(string strSQL)
    {
        List<List<string>> list = new List<List<string>>();
        List<string> list1 = new List<string>();
        List<string> list2 = new List<string>();
        DataTable dt = NpoDB.GetDataTableS(strSQL, null);
        foreach (DataRow dr in dt.Rows)
        {
            list1.Add(dr[0].ToString());
            list2.Add(dr[1].ToString());
        }
        list.Add(list1);
        list.Add(list2);
        return list;
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 輸出 Excel, Word 及 PDF
    /// </summary>
    public static void OutputTxt(string strHtml, string Type, string filName)
    {
        Response.Clear();//從緩衝區資料流清除所有內容輸出
        Response.Buffer = true;//使PWS/IIS要輸出的資料先寫到緩衝區
        Response.ContentType = "text/html";//將檔案顯示在網頁的類型 
        Response.Charset = "utf8";//設定文件編碼改成大五碼字集
        string filename = "attachment;filename=";
        if (Request.Browser.Browser == "IE")
        {
            filName = Server.UrlEncode(filName);
        }

        switch (Type)
        {
            case "1":   //Excel
                Response.ContentType = "application/ms-excel";
                filename += filName + ".xls";
                break;
            case "2":  //Word
                Response.ContentType = "application/msword";
                filename += filName + ".doc";
                break;
            case "3":  //PDF
                Response.ContentType = "application/OCTET-STREAM";
                filename += filName + ".pdf";
                break;
        }

        Response.AppendHeader("Content-Disposition", filename); //將指定的文件直接寫入HTTP 內容輸出流
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");//理論上輸出應該是utf-8才正常
        System.IO.StringWriter objStringWriter = new System.IO.StringWriter();//定義將資料寫入字串
        System.Web.UI.HtmlTextWriter objHtmlTextWriter = new System.Web.UI.HtmlTextWriter(objStringWriter);
        //2013/07/23, utf8 要加加 BOM, Excel 開檔才不會亂碼 
        Response.Write("\uFEFF" + strHtml.Trim());//發送objStringWriter到瀏覽器
        Response.Flush();
        Response.End();// 結束資料輸出
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 輸入生日,得到年紀
    /// </summary>
    public static int CalculateAge(string strBirthDate)
    {
        int age = 0;
        try
        {
            DateTime birthDate = Convert.ToDateTime(strBirthDate);
            DateTime now = DateTime.Now;
            age = now.Year - birthDate.Year;
            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
            {
                age--;
            }
        }
        catch (Exception ex)
        {
        }
        return age;

    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 輸入生日,得到年紀
    /// </summary>
    public static int CalculateAge(DateTime birthDate)
    {
        DateTime now = DateTime.Now;
        int age = now.Year - birthDate.Year;
        if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
        {
            age--;
        }
        return age;
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 顯示訊息,連同動態產生的 javascript 碼
    /// </summary>
    public static void ShowSysMsgWithScript(string AnotherScript)
    {
        String Script = "";
        if (Session["Msg"] != null && Session["Msg"].ToString() != "")
        {
            Script = AnotherScript + ";\nalert('" + Session["Msg"].ToString() + "');\n";
            CreateJavaScript(Script);
            Session["Msg"] = "";
        }
        else
        {
            CreateJavaScript(AnotherScript);
        }
    }
    //----------------------------------------------------------------------
    /// <summary>
    /// 嵌入 JavaScript 碼
    /// </summary>
    public static void CreateJavaScript(string JavaScript)
    {
        //加亂數就可保證一定執行,不會因為同名而有問題
        String csname = "PageScript" + DateTime.Now.Ticks.ToString();
        Type cstype = page.GetType();
        ClientScriptManager cs = page.ClientScript;
        cs.RegisterStartupScript(cstype, csname, JavaScript, true);
    }
    //----------------------------------------------------------------------
    /// <summary>
    /// 日期資料轉字串, 輸入型態可為 DateTime, string 或資料庫欄位
    /// </summary>
    public static string DateTime2String(object objDate, DateType dateType, EmptyType emptyType)
    {
        if (objDate is DateTime)
        {
            DateTime d = (DateTime)objDate;
            if (dateType == DateType.yyyyMMdd)
            {
                return d.ToString("yyyy/MM/dd");
            }
            else if (dateType == DateType.yyyyMMddHHmmss)
            {
                return d.ToString("yyyy/MM/dd HH:mm:ss");
            }
            else if (dateType == DateType.CyyyMMdd)
            {
                return String.Format("{0:000}/{1:00}/{2:00}", d.Year-1911, d.Month, d.Day);
            }
            else if (dateType == DateType.CyyyMMddHHmmss)
            {
                return String.Format("{0:0000}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}", d.Year - 1911, d.Month, d.Day, d.Hour, d.Minute, d.Second);
            }
            else if (dateType == DateType.HHmmss)
            {
                return d.ToString("HH:mm:ss");
            }
        }
        else if (objDate is string)
        {
            string strDate = objDate as string;
            if (strDate == "")
            {
                if (emptyType == EmptyType.ReturnEmpty)
                {
                    return "";
                }
                else if (emptyType == EmptyType.ReturnNull)
                {
                    return null;
                }
            }
            else
            {
                //如果字串長度小於 10,則視為民國年
                strDate = FixDateTime(strDate);
                if (strDate.IndexOf("/") != 4)
                {
                    string[] strArr = strDate.Split('/');
                    if (strArr.Length == 3)
                    {
                        strDate = (Util.String2Number(strArr[0]) + 1911).ToString() +
                                  "/" + strArr[1] + "/" + strArr[2];
                    }
                    else
                    {
                        strDate = "";
                    }
                }
                //如果字串不能轉成日期,則傳回 "" 或 null
                try
                {
                    DateTime d = Convert.ToDateTime(strDate);
                    if (dateType == DateType.yyyyMMdd)
                    {
                        return d.ToString("yyyy/MM/dd");
                    }
                    else if (dateType == DateType.yyyyMMddHHmmss)
                    {
                        return d.ToString("yyyy/MM/dd HH:mm:ss");
                    }
                    else if (dateType == DateType.CyyyMMdd)
                    {
                        return String.Format("{0}/{1}/{2}", d.Year - 1911, d.Month, d.Day);
                    }
                    else if (dateType == DateType.CyyyMMddHHmmss)
                    {
                        return String.Format("{0}/{1}/{2} {3}:{4}:{5}", d.Year - 1911, d.Month, d.Day, d.Hour, d.Minute, d.Second);
                    }
                    else if (dateType == DateType.HHmmss)
                    {
                        return d.ToString("HH:mm:ss");
                    }
                }
                catch (Exception ex)
                {
                    if (emptyType == EmptyType.ReturnEmpty)
                    {
                        return "";
                    }
                    else if (emptyType == EmptyType.ReturnNull)
                    {
                        return null;
                    }
                }
            }
        }
        else if (objDate == DBNull.Value)
        {
            if (emptyType == EmptyType.ReturnEmpty)
            {
                return "";
            }
            else if (emptyType == EmptyType.ReturnNull)
            {
                return null;
            }
        }
        return null;
    }
    //--------------------------------------------------------------------------
    /// <summary>
    /// 處理時間日期出現上下午以至於無法寫入資料庫問題
    /// </summary>
    public static string FixDateTime(string retdatetime)
    {
        if (retdatetime.IndexOf(" 上午") >= 0)
        {
            retdatetime = retdatetime.Replace(" 上午", "") + " AM";
        }

        if (retdatetime.IndexOf(" 下午") >= 0)
        {
            retdatetime = retdatetime.Replace(" 下午", "") + " PM";
        }
        if (retdatetime == "")
        {
            return null;
        }
        return retdatetime;
    }
    //----------------------------------------------------------------------
    /// <summary>
    /// 為了不Cache而在連結後面加入亂數
    /// </summary>
    /// <param name="Link">連結</param>
    /// <param name="QueryString">自訂Querystring</param>
    /// <param name="forEdit">是否使用於Edit相關連結使用</param>
    /// <returns></returns>
    public static string RedirectByTime(string Link, string QueryString, bool forEdit)
    {
        if (forEdit == true)
            return Link + "?rand=" + DateTime.Now.Ticks.ToString() + "&" + QueryString + "=";
        else
            return Link + "?rand=" + DateTime.Now.Ticks.ToString() + "&" + QueryString;
    }
    //----------------------------------------------------------------------
    /// <summary>
    /// 為了不Cache而在連結後面加入亂數
    /// </summary>
    /// <param name="Link">連結</param>
    /// <param name="QueryString">自訂Querystring</param>
    /// <returns></returns>
    public static string RedirectByTime(string Link, string QueryString)
    {
        return Link + "?rand=" + DateTime.Now.Ticks.ToString() + "&" + QueryString;
    }
    //----------------------------------------------------------------------
    /// <summary>
    /// Link後面加，亂數時間
    /// </summary>
    /// <param name="Link"></param>
    /// <returns></returns>
    public static string RedirectByTime(string Link)
    {
        return Link + "?rand=" + DateTime.Now.Ticks.ToString();
    }
    //----------------------------------------------------------------------
    /// <summary>
    /// 填入縣市名稱
    /// </summary>
    public static void FillCityData(DropDownList ddl)
    {
        string strSql = @"
                        select ZipCode as CityID,
                        Name as CityName
                        from CodeCity
                        where ParentCityID='0'
                        order by Sort
                        ";
        FillDropDownList(ddl, strSql, "CityName", "CityID", true);
    }
    //----------------------------------------------------------------------
    ///<summary>
    ///填入鄉鎮市資料
    ///</summary>
    public static void FillAreaData(DropDownList ddl, string CityID)
    {
        string strSql = @"
                        select ZipCode as AreaID,
                        Name as AreaName
                        from CodeCity
                        where ParentCityID=@CityID
                        order by Sort
                        ";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("CityID", CityID);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        Util.FillDropDownList(ddl, dt, "AreaName", "AreaID", true);
    }
    //-------------------------------------------------------------------------------------------------------------
    ///<summary>
    ///填入街道資料
    ///</summary>
    public static void FillStreetData(DropDownList ddl, string AreaID)
    {
        string strSql = @"
                            select cs.uid, StreetName
                            from CodeStreet cs
                            inner join CodeCity cc2 on cc2.ZipCode=@AreaID
                            where AreaName=cc2.Name
                            order by StreetName
                        ";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("AreaID", AreaID);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);

        Util.FillDropDownList(ddl, dt, "StreetName", "uid", true);
    }
    //-------------------------------------------------------------------------------------------------------------
    ///<summary>
    ///取得 GetSessionValue 值,如果失效,導至 default 頁面
    ///</summary>
    public static string GetSessionValue(string SessionName)
    {
        if (Session[SessionName] == null)
        {
            Response.Write(@"<script>window.parent.location.href='../Default.aspx';</script>");
            return "";
        }
        else
        {
            return page.Session[SessionName].ToString();
        }
    }
    //----------------------------------------------------------------------
    ///<summary>
    ///取得 GetSessionValue 值,如果失效,以 RedirectPage 參數決定是否 Redirect
    ///</summary>
    public static string GetSessionValue(string SessionName, bool RedirectPage)
    {
        if (Session[SessionName] == null)
        {
            if (RedirectPage == true)
            {
                Response.Write(@"<script>window.parent.location.href='../Default.aspx';</script>");
            }
            return "";
        }
        else
        {
            return page.Session[SessionName].ToString();
        }
    }
    //----------------------------------------------------------------------
    /// <summary>
    /// 顯示訊息
    /// </summary>
    public static void ShowMsg(string Msg)
    {
        String cstext = "alert('" + Msg + "');";
        CreateJavaScript(cstext);
    }
    //---------------------------------------------------------------------------
    /// <summary>
    /// 取得 appSettings 的 value
    /// </summary>
    public static string GetAppSetting(string key)
    {
        return System.Configuration.ConfigurationManager.AppSettings[key].ToString();
    }
    //--------------------------------------------------------------------------
    ///<summary>
    ///取得縣市代碼之函式
    ///</summary>
    public static string GetCityCode(string strCityName)
    {
        string strRet = "";

        string strSQL = "select ZipCode from CodeCity where Name=@CityName";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("CityName", strCityName);
        DataTable dt = NpoDB.GetDataTableS(strSQL, dict);

        if (dt.Rows.Count > 0)
        {
            strRet = dt.Rows[0]["ZipCode"].ToString();
        }
        return strRet;
    }
    //--------------------------------------------------------------------------
    ///<summary>
    ///取得縣市名稱之函式
    ///</summary>
    public static string GetCityName(string strCityCode)
    {
        string strRet = "";

        string strSQL = "select Name from CodeCity where ZipCode=@CityCode";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("CityCode", strCityCode);
        DataTable dt = NpoDB.GetDataTableS(strSQL, dict);

        if (dt.Rows.Count > 0)
        {
            strRet = dt.Rows[0]["Name"].ToString();
        }
        return strRet;
    }
    //--------------------------------------------------------------------------
    ///<summary>
    ///取得鄉鎮市區代碼之函式
    ///</summary>
    public static string GetAreaCode(string strAreaName)
    {
        string strRet = "";

        string strSQL = "select ZipCode from CodeCity where Name=@AreaName";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("AreaName", strAreaName);
        DataTable dt = NpoDB.GetDataTableS(strSQL, dict);

        if (dt.Rows.Count > 0)
        {
            strRet = dt.Rows[0]["ZipCode"].ToString();
        }
        return strRet;
    }
    //--------------------------------------------------------------------------
    ///<summary>
    ///取得鄉鎮市區名稱之函式
    ///</summary>
    public static string GetAreaName(string strAreaCode)
    {
        string strRet = "";

        string strSQL = "select Name from CodeCity where ZipCode=@AreaCode";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("AreaCode", strAreaCode);
        DataTable dt = NpoDB.GetDataTableS(strSQL, dict);

        if (dt.Rows.Count > 0)
        {
            strRet = dt.Rows[0]["Name"].ToString();
        }
        return strRet;
    }
    //--------------------------------------------------------------------------
    ///<summary>
    ///取得郵遞區號之函式
    ///</summary>
    public static string GetZipCode(string strAreaName)
    {
        string strRet = "";

        string strSQL = "select ZipCode from CodeCity where Name=@AreaName";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("AreaName", strAreaName);
        DataTable dt = NpoDB.GetDataTableS(strSQL, dict);

        if (dt.Rows.Count > 0)
        {
            strRet = dt.Rows[0]["ZipCode"].ToString();
        }
        return strRet;
    }
    //---------------------------------------------------------------------------
    ///<summary>
    ///一般的 GridView, 有 Key list 參數
    ///</summary>
    //---------------------------------------------------------------------------
    public static string ShowGrid(DataTable DataSource, string CssClass, List<string> Keys)
    {
        NPOGridView npoGridView = new NPOGridView();
        npoGridView.Source = NPOGridViewDataSource.fromDataTable;
        npoGridView.dataTable = DataSource;
        npoGridView.Keys = Keys;
        npoGridView.DisableColumn = Keys;
        npoGridView.ShowPage = false;
        if (CssClass != null)
        {
            npoGridView.CssClass = CssClass;
        }
        return npoGridView.Render();
    }
    //---------------------------------------------------------------------------
    ///<summary>
    ///一般的GridView
    ///</summary>
    public static string ShowGrid(DataTable DataSource, string CssClass)
    {
        List<string> list = new List<string>();
        list.Add("uid");
        return ShowGrid(DataSource, CssClass, list);
    }
    //---------------------------------------------------------------------------
    ///<summary>
    ///檢查欄位值是否存在
    ///</summary>
    public static bool Exist(string TableName, string FieldName, string FieldValue)
    {
        string strSql = "select " + FieldName + "\n";
        strSql += "from " + TableName + "\n";
        strSql += "where " + FieldName + "=@FieldValue\n";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("FieldValue", FieldValue);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        return dt.Rows.Count > 0;
    }
    //---------------------------------------------------------------------------
    ///<summary>
    ///檢查欄位值是否存在
    ///</summary>
    public static bool Exist(string TableName, string FieldName, string FieldValue, string ExtraCondition)
    {
        string strSql = "select " + FieldName + "\n";
        strSql += "from " + TableName + "\n";
        strSql += "where " + FieldName + "=@FieldValue\n";
        if (ExtraCondition != "")
        {
            strSql += ExtraCondition;
        }

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("FieldValue", FieldValue);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        return dt.Rows.Count > 0;
    }
    //---------------------------------------------------------------------------
    ///<summary>
    ///檢查欄位值是否存在
    ///</summary>
    public static bool Exist(string TableName, string FieldName1, string FieldValue1, string FieldName2, string FieldValue2)
    {
        string strSql = "select " + FieldName1 + "\n";
        strSql += "from " + TableName + "\n";
        strSql += "where " + FieldName1 + "=@FieldValue1\n";
        strSql += "and " + FieldName2 + "=@FieldValue2\n";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("FieldValue1", FieldValue1);
        dict.Add("FieldValue2", FieldValue2);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        return dt.Rows.Count > 0;
    }
    //---------------------------------------------------------------------------
    ///<summary>
    ///返回今天的字串
    ///</summary>
    public static string GetToday(DateType dateType)
    {
        return DateTime2String(DateTime.Now, dateType, EmptyType.ReturnNull);
    }
    //-------------------------------------------------------------------------------------------------------------
    ///<summary>
    ///輸入欄位及值,返回合乎條件的值(一個欄位)
    ///</summary>
    public static string GetDBValue(string TableName, string ReturnField, string FieldName, string FieldValue)
    {
        string strSql = "select " + ReturnField + "\n";
        strSql += "from " + TableName + "\n";
        strSql += "where " + FieldName + "=@FieldValue\n";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("FieldValue", FieldValue);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count > 0)
        {
            return dt.Rows[0][0].ToString();
        }
        return "";
    }
    //-------------------------------------------------------------------------------------------------------------
    ///<summary>
    ///輸入欄位及值,返回合乎條件的值(兩個欄位)
    ///</summary>
    public static string GetDBValue(string TableName, string ReturnField, string FieldName1, string FieldValue1, string FieldName2, string FieldValue2)
    {
        string strSql = "select " + ReturnField + "\n";
        strSql += "from " + TableName + "\n";
        strSql += "where " + FieldName1 + "=@FieldValue1\n";
        strSql += "and " + FieldName2 + "=@FieldValue2\n";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("FieldValue1", FieldValue1);
        dict.Add("FieldValue2", FieldValue2);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count > 0)
        {
            return dt.Rows[0][0].ToString();
        }
        return "";
    }
    //-------------------------------------------------------------------------------------------------------------
    ///<summary>
    ///輸入欄位及值,返回合乎條件的 DataTable(一個欄位)
    ///</summary>
    public static DataTable GetDataTable(string TableName, string FieldName, string FieldValue, string OrderBy, string AscDesc)
    {
        string strSql = "select *\n";
        strSql += "from " + TableName + "\n";
        strSql += "where " + FieldName + "=@FieldValue1\n";
        if (OrderBy != "" && OrderBy != null)
        {
            strSql += "order by " + OrderBy + "\n";
            strSql += AscDesc;
        }

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("FieldValue1", FieldValue);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        return dt;
    }
    //-------------------------------------------------------------------------------------------------------------
    ///<summary>
    ///輸入欄位及值,返回合乎條件的 DataTable(兩個欄位)
    ///</summary>
    public static DataTable GetDataTable(string TableName, string FieldName1, string FieldValue1, string FieldName2, string FieldValue2, string OrderBy, string AscDesc)
    {
        string strSql = "select *\n";
        strSql += "from " + TableName + "\n";
        strSql += "where " + FieldName1 + "=@FieldValue1\n";
        strSql += "and " + FieldName2 + "=@FieldValue2\n";
        if (OrderBy != "" && OrderBy != null)
        {
            strSql += "order by " + OrderBy + "\n";
            strSql += AscDesc;
        }

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("FieldValue1", FieldValue1);
        dict.Add("FieldValue2", FieldValue2);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        return dt;
    }
    //-------------------------------------------------------------------------------------------------------------
    public static string GetWeekString(DateTime d, WeekType weekType)
    {
        string strTemp = "";
        switch (d.DayOfWeek)
        {
            case DayOfWeek.Monday:
                strTemp = "星期一";
                break;
            case DayOfWeek.Tuesday:
                strTemp = "星期二";
                break;
            case DayOfWeek.Wednesday:
                strTemp = "星期三";
                break;
            case DayOfWeek.Thursday:
                strTemp = "星期四";
                break;
            case DayOfWeek.Friday:
                strTemp = "星期五";
                break;
            case DayOfWeek.Saturday:
                strTemp = "星期六";
                break;
            case DayOfWeek.Sunday:
                strTemp = "星期日";
                break;
        }
        if (weekType == WeekType.Short)
        {
            return strTemp.Substring(2);
        }
        else if (weekType == WeekType.Long)
        {
            return strTemp;
        }
        return "參數錯誤";
    }
    //-------------------------------------------------------------------------------------------------------------
    //可以輸入日期或是單純的數字
    public static string GetMonthString(object obj)
    {
        int Month = 0;
        DateTime d;
        if (obj is DateTime)
        {
            d = (DateTime)obj;
            Month = d.Month;
        }
        else if (obj is int)
        {
            Month = (int)obj;
        }

        string strTemp = "";
        switch (Month)
        {
            case 1:
                strTemp = "一";
                break;
            case 2:
                strTemp = "二";
                break;
            case 3:
                strTemp = "三";
                break;
            case 4:
                strTemp = "四";
                break;
            case 5:
                strTemp = "五";
                break;
            case 6:
                strTemp = "六";
                break;
            case 7:
                strTemp = "七";
                break;
            case 8:
                strTemp = "八";
                break;
            case 9:
                strTemp = "九";
                break;
            case 10:
                strTemp = "十";
                break;
            case 11:
                strTemp = "十一";
                break;
            case 12:
                strTemp = "十二";
                break;
        }
        return strTemp;
    }
    //-------------------------------------------------------------------------------------------------------------
    public static void AddTDLine(CssStyleCollection css, int type)
    {
        switch (type)
        {
            case 1234:
                css.Add("border-left", "1px solid windowtext");
                css.Add("border-top", "1px solid windowtext");
                css.Add("border-right", "1px solid windowtext");
                css.Add("border-bottom", "1px solid windowtext");
                break;
            case 123:
                css.Add("border-left", "1px solid windowtext");
                css.Add("border-top", "1px solid windowtext");
                css.Add("border-right", "1px solid windowtext");
                break;
            case 234:
                css.Add("border-top", "1px solid windowtext");
                css.Add("border-right", "1px solid windowtext");
                css.Add("border-bottom", "1px solid windowtext");
                break;
            case 124:
                css.Add("border-left", "1px solid windowtext");
                css.Add("border-top", "1px solid windowtext");
                css.Add("border-bottom", "1px solid windowtext");
                break;
            case 134:
                css.Add("border-left", "1px solid windowtext");
                css.Add("border-right", "1px solid windowtext");
                css.Add("border-bottom", "1px solid windowtext");
                break;
            case 12:
                css.Add("border-left", "1px solid windowtext");
                css.Add("border-top", "1px solid windowtext");
                break;
            case 13:
                css.Add("border-left", "1px solid windowtext");
                css.Add("border-right", "1px solid windowtext");
                break;
            case 23:
                css.Add("border-top", "1px solid windowtext");
                css.Add("border-right", "1px solid windowtext");
                break;
            case 34:
                css.Add("border-right", "1px solid windowtext");
                css.Add("border-bottom", "1px solid windowtext");
                break;
            case 1:
                css.Add("border-left", "1px solid windowtext");
                break;
            case 2:
                css.Add("border-top", "1px solid windowtext");
                break;
            case 3:
                css.Add("border-right", "1px solid windowtext");
                break;
            case 4:
                css.Add("border-bottom", "1px solid windowtext");
                break;
            default:
                break;
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    public static void AddTDLine(CssStyleCollection css, int type, string strColor)
    {          
        //css.Add("border-color", "red");
        switch (type)
        {
            case 1234:
                css.Add("border-left", "1px solid  " + strColor + " ");
                css.Add("border-top", "1px solid " + strColor + " ");
                css.Add("border-right", "1px solid " + strColor + " ");
                css.Add("border-bottom", "1px solid " + strColor + " ");
                break;
            case 123:
                css.Add("border-left", "1px solid " + strColor + "");
                css.Add("border-top", " 1px solid " + strColor + "");
                css.Add("border-right","1px solid " + strColor + "");
                break;
            case 234:
                css.Add("border-top", "1px solid  " + strColor + " ");
                css.Add("border-right", "1px solid  " + strColor + " ");
                css.Add("border-bottom", "1px solid  " + strColor + " ");
                break;
            case 124:
                css.Add("border-left", "1px solid  " + strColor + " ");
                css.Add("border-top", "1px solid  " + strColor + " ");
                css.Add("border-bottom", "1px solid " + strColor + "");
                break;
            case 134:
                css.Add("border-left", "1px solid  " + strColor + "");
                css.Add("border-right", "1px solid  " + strColor + "");
                css.Add("border-bottom", "1px solid  " + strColor + "");
                break;
            case 12:
                css.Add("border-left", "1px solid  " + strColor + "");
                css.Add("border-top", "1px solid  " + strColor + "");
                break;
            case 13:
                css.Add("border-left", "1px solid  " + strColor + "");
                css.Add("border-right", "1px solid  " + strColor + "");
                break;
            case 23:
                css.Add("border-top", "1px solid  " + strColor + "");
                css.Add("border-right", "1px solid  " + strColor + "");
                break;
            case 34:
                css.Add("border-right", "1px solid  " + strColor + "");
                css.Add("border-bottom", "1px solid " + strColor + " ");
                break;
            case 1:
                css.Add("border-left", "1px solid  " + strColor + "");
                break;
            case 2:
                css.Add("border-top", "1px solid  " + strColor + "");
                break;
            case 3:
                css.Add("border-right", "1px solid  " + strColor + "");
                break;
            case 4:
                css.Add("border-bottom", "1px solid  " + strColor + "");
                break;
            default:
                break;
        }
    }
    //---------------------------------------------------------------------------
    //DataTable 行列交換：ColumnHeader轉為第一欄之Data，第一欄之Data轉為ColumnHeader
    public static DataTable ConvertRowColumn(DataTable dtTarget, string strNewColName0, string strNewColNameFromRow)
    {
        DataTable dtNew = new DataTable();
        dtNew.Columns.Add(strNewColName0, typeof(string));
        for (int i = 0; i < dtTarget.Rows.Count; i++)
        {
            dtNew.Columns.Add(dtTarget.Rows[i][strNewColNameFromRow].ToString(), typeof(string));
        }
        foreach (DataColumn dc in dtTarget.Columns)
        {
            DataRow drNew = dtNew.NewRow();
            drNew[strNewColName0] = dc.ColumnName;
            for (int i = 0; i < dtTarget.Rows.Count; i++)
            {
                drNew[i + 1] = dtTarget.Rows[i][dc].ToString();
            }
            dtNew.Rows.Add(drNew);
        }
        //移除第一筆資料，因已加入到Header
        dtNew.Rows.RemoveAt(0);
        return dtNew;
    }
    //-------------------------------------------------------------------------------------------------------------
    public static DateTime GetDBDateTime()
    {
        string strSql = "select GetDate()";
        DataTable dt = NpoDB.GetDataTableS(strSql, null);

        DateTime dtRet = Convert.ToDateTime(dt.Rows[0][0].ToString());
        return dtRet;
    }
    //-------------------------------------------------------------------------------------------
    public static string NpoEncrypt(string val, string key_val)
    {
        string encrypted = val;
        byte[] key = new System.Text.ASCIIEncoding().GetBytes(key_val);
        for (int i = 0; i < 2; i++)
        {
            val = encrypted;
            try
            {
                byte[] inputBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(val);
                byte[] pwdhash = null;

                System.Security.Cryptography.MD5CryptoServiceProvider hashmd5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                pwdhash = hashmd5.ComputeHash(key);
                hashmd5 = null;
                System.Security.Cryptography.TripleDESCryptoServiceProvider tdesProvider = new System.Security.Cryptography.TripleDESCryptoServiceProvider();
                tdesProvider.Key = pwdhash;
                tdesProvider.Mode = System.Security.Cryptography.CipherMode.ECB;
                encrypted = Convert.ToBase64String(tdesProvider.CreateEncryptor().TransformFinalBlock(inputBytes, 0, inputBytes.Length));
            }
            catch (Exception e)
            {
                string err = e.Message;
                throw;
            }
        }
        return encrypted;
    }
    //---------------------------------------------------------------------------
    public static string NpoDecrypt(string val, string key_val)
    {
        if (val == "")
        {
            return "";
        }
        string decyprted = val;
        byte[] inputBytes = null;
        byte[] key = new System.Text.ASCIIEncoding().GetBytes(key_val);
        for (int i = 0; i < 2; i++)
        {
            val = decyprted;

            try
            {
                inputBytes = Convert.FromBase64String(val);
                byte[] pwdhash = null;
                System.Security.Cryptography.MD5CryptoServiceProvider hashmd5;
                hashmd5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                pwdhash = hashmd5.ComputeHash(key);
                hashmd5 = null;
                System.Security.Cryptography.TripleDESCryptoServiceProvider tdesProvider = new System.Security.Cryptography.TripleDESCryptoServiceProvider();
                tdesProvider.Key = pwdhash;
                tdesProvider.Mode = System.Security.Cryptography.CipherMode.ECB;
                decyprted = System.Text.ASCIIEncoding.ASCII.GetString(tdesProvider.CreateDecryptor().TransformFinalBlock(inputBytes, 0, inputBytes.Length));
            }
            catch (Exception e)
            {
                string str = e.Message;
                throw;
            }
        }
        return decyprted;
    }
    //---------------------------------------------------------------------------
    public static string ChineseMoney(long money)
    {
        if (money == 0.0)
        {
            return "零元整";
        }
        string strDallor = money.ToString();
        string[] chDallor = new string[] { "", "拾", "佰", "仟", "萬", "拾", "佰", "仟", "億", "拾", "佰", "仟", "兆" };
        string[] chB = new string[] { "", "萬", "億", "兆" };
        int i;
        string result = "";
        int strDallorLen = strDallor.Length;
        for (i = 0; i < strDallorLen; i++)
        {
            if (strDallor[i] != '0')
            {
                result = result + strDallor[i] + chDallor[strDallorLen - i - 1];
            }
            else
            {
                if (result[result.Length - 1] != '零')
                {
                    if ((strDallorLen - i - 1) % 4 == 0)
                        result = result + chB[(strDallorLen - i - 1) / 4];
                    else
                        result = result + "零";
                }
                else
                {
                    if ((strDallorLen - i - 1) % 4 == 0)
                        result = result.Substring(0, result.Length - 1) + chB[(strDallorLen - i - 1) / 4];

                }
            }
        }

        if (result[result.Length - 1] == '零')
        {
            result.Substring(0, result.Length - 1);
        }

        result = result.Replace("1", "壹");
        result = result.Replace("2", "貳");
        result = result.Replace("3", "參");
        result = result.Replace("4", "肆");
        result = result.Replace("5", "伍");
        result = result.Replace("6", "陸");
        result = result.Replace("7", "柒");
        result = result.Replace("8", "捌");
        result = result.Replace("9", "玖");

        return result + "元整";
    }
    //---------------------------------------------------------------------------
} //end of class Util
