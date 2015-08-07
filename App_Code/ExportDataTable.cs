using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.IO;

public class ExportDataTable
{
    public ExportDataTable()
    {
        _disableColumn = new List<string>();
        _textAlign = new List<string>();
        _forceText = new List<string>();
    }

    private DataTable _dataTable;
    public DataTable dataTable
    {
        set
        {
            _dataTable = value;
            for (int i = 0; i < _dataTable.Columns.Count; i++)
            {
                _textAlign.Add("center");
            }
        }
    }

    private string _titleAlign = "center";
    public string TitleAlign
    {
        set
        {
            _titleAlign = value;
        }
    }


    private List<string> _disableColumn; //不顯示的欄位
    public List<string> DisableColumn
    {
        get
        {
            return _disableColumn;
        }
    }

    private List<string> _forceText; //純文字的欄位
    public List<string> ForceText
    {
        get
        {
            return _forceText;
        }
    }

    private string _fontSize = "12pt";
    public string FontSize
    {
        set
        {
            _fontSize = value;
        }
    }

    private string _fontFamily = "標楷體";
    public string FontFamily
    {
        set
        {
            _fontFamily = value;
        }
    }

    private string _titleFontSize = "14pt";
    public string TitleFontSize
    {
        set
        {
            _titleFontSize = value;
        }
    }

    private string _columnFontSize = "14pt";
    public string ColumnFontSize
    {
        set
        {
            _columnFontSize = value;
        }
    }

    private string _columnAlign = "center";
    public string ColumnAlign
    {
        set
        {
            _columnAlign = value;
        }
    }

    private string _titleFontWeight = "bold";
    public string TitleFontWeight
    {
        set
        {
            _titleFontWeight = value;
        }
    }

    public string _titleFontFamily = "標楷體";
    public string TitleFontFamily
    {
        set
        {
            _titleFontFamily = value;
        }
    }

    private List<string> _textAlign;

    public void SetTextAlign(string FieldName, string Align)
    {
        for (int i = 0; i < _dataTable.Columns.Count; i++)
        {
            string ColumnName = _dataTable.Columns[i].ColumnName;
            if (FieldName == ColumnName)
            {
                _textAlign[i] = Align;
            }
        }
    }
    //-------------------------------------------------------------------------
    public string Render()
    {
        //組 table
        string strTemp = "";
        HtmlTable table = new HtmlTable();
        HtmlTableRow row;
        HtmlTableCell cell;
        CssStyleCollection css;

        table.Border = 1;
        table.CellPadding = 3;
        table.CellSpacing = 0;
        css = table.Style;
        css.Add("font-size", _fontSize);
        css.Add("font-family", _fontFamily);
        css.Add("width", "100%");
        //--------------------------------------------
        //第1行
        row = new HtmlTableRow();
        for (int i = 0; i < _dataTable.Columns.Count; i++)
        {
            string ColumnName = _dataTable.Columns[i].ColumnName;
            if (_disableColumn.Contains(ColumnName))
            {
                continue;
            }

            cell = new HtmlTableCell();
            strTemp = _dataTable.Columns[i].ColumnName;
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", _columnAlign);
            css.Add("font-size", _columnFontSize);
            row.Cells.Add(cell);
        }
        table.Rows.Add(row);
        //--------------------------------------------
        //第2行,資料顯示
        foreach (DataRow dr in _dataTable.Rows)
        {
            row = new HtmlTableRow();

            for (int i = 0; i < _dataTable.Columns.Count; i++)
            {
                string ColumnName = _dataTable.Columns[i].ColumnName;
                if (_disableColumn.Contains(ColumnName))
                {
                    continue;
                }

                cell = new HtmlTableCell();
                strTemp = dr[_dataTable.Columns[i].ColumnName].ToString();

                cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
                css = cell.Style;

                if (_forceText.Contains(ColumnName))
                {
                    css.Add("mso-number-format", "\\@");
                }
                css.Add("text-align", _textAlign[i]);
                css.Add("font-size", _fontSize);
                row.Cells.Add(cell);
            }
            table.Rows.Add(row);
        }
        //--------------------------------------------
        //轉成 html 碼
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        table.RenderControl(htw);

        return htw.InnerWriter.ToString();
    }
    //---------------------------------------------------------------------------
    public string GetTitle(string Title, int ColSpan)
    {
        //組 table
        string strTemp = "";
        HtmlTable table = new HtmlTable();
        HtmlTableRow row;
        HtmlTableCell cell;
        CssStyleCollection css;

        table.Border = 0;
        table.CellPadding = 3;
        table.CellSpacing = 0;
        css = table.Style;
        css.Add("font-size", _titleFontSize);
        css.Add("font-family", _titleFontFamily);
        css.Add("width", "100%");
        css.Add("font-weight", _titleFontWeight);

        //--------------------------------------------
        row = new HtmlTableRow();

        cell = new HtmlTableCell();
        strTemp = Title;
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", _titleAlign);
        css.Add("font-size", _titleFontSize);
        cell.ColSpan = ColSpan;
        row.Cells.Add(cell);

        table.Rows.Add(row);
        //--------------------------------------------
        //轉成 html 碼
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        table.RenderControl(htw);

        return htw.InnerWriter.ToString();
    }
    //---------------------------------------------------------------------------
} //end of class DocxHelper
//---------------------------------------------------------------------------

