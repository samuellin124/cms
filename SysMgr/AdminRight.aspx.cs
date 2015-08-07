using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using System.IO;

public partial class SysMgr_AdminRight : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["ProgID"] = "AdminRight";
        //權控處理
        AuthrityControl();

        base.ShowSysMsg();
        if (!IsPostBack)
        {
            Session["Prog_id"] = "AdminRight";
            LoadDropDownListData();
            LoadFormData();
        }
    }
    //----------------------------------------------------------------------
    public void AuthrityControl()
    {
        if (Authrity.PageRight("_Focus") == false)
        {
            return;
        }

        Authrity.CheckButtonRight("_Query", btnQuery);
        HFD_UpdateRight.Value = Authrity.RightCheck("_Update").ToString();
    }
    //----------------------------------------------------------------------------
    public void LoadDropDownListData()
    {
        //使用者群組
        string strSql = "select uid, GroupName from AdminGroup order by uid";
        Util.FillDropDownList(ddlGroup, strSql, "GroupName", "uid", true);
        Util.SetDdlIndex(ddlGroup, SessionInfo.GroupID);
    }
    //----------------------------------------------------------------------------
    private string GetTitle()
    {
        string strTemp = "";
        HtmlTable table = new HtmlTable();
        HtmlTableRow row;
        HtmlTableCell cell;
        CssStyleCollection css;
        table.Border = 1;
        table.CellPadding = 3;
        table.CellSpacing = 0;
        css = table.Style;
        css.Add("font-size", "24px");
        css.Add("font-family", "標楷體");
        string strFontSize = "14px";
        string strWidth = "45px";

        strTemp = ddlGroup.SelectedItem.ToString() + "使用權限設定";
        row = new HtmlTableRow();
        cell = new HtmlTableCell();
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", "24px");
        cell.ColSpan = 20;
        row.Cells.Add(cell);

        table.Rows.Add(row);
        //------------------------------------------------------------------------
        string strDefaultValue = "";
        List<ControlData> list = new List<ControlData>();
        list.Clear();
        list.Add(new ControlData("Checkbox", "GN_SelectAll", "CB_SelectAll", "", "全選", false, null));
        list.Add(new ControlData("Checkbox", "GN_USelectAll", "CB_UnSelectAll", "", "全不選", false, null));
        row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = HtmlUtil.RenderControl(list);
        strTemp += "<input type=button id='btnSave' runat=server value='存檔'  onclick='SaveAllValue()' style='height:20px;' />";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "right");
        css.Add("font-size", strFontSize);
        cell.ColSpan = 40;
        row.Cells.Add(cell);

        table.Rows.Add(row);
        //------------------------------------------------------------------------
        //顯示抬頭
        row = new HtmlTableRow();
        cell = new HtmlTableCell();
        strTemp = "項目";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        css.Add("width", "14px");
        row.Cells.Add(cell);

        cell = new HtmlTableCell();
        strTemp = "第一層";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        css.Add("width", "60px");
        row.Cells.Add(cell);

        cell = new HtmlTableCell();
        strTemp = "第二層";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        css.Add("width", "60px");
        row.Cells.Add(cell);

        cell = new HtmlTableCell();
        strTemp = "第三層";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        css.Add("width", "60px");
        row.Cells.Add(cell);

        cell = new HtmlTableCell();
        strTemp = "第四層";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        css.Add("width", "60px");
        row.Cells.Add(cell);
        //----------------------------------------------------------------------------
        //新增
        cell = new HtmlTableCell();
        list.Clear();
        list.Add(new ControlData("Checkbox", "GN_AddColAll", "CB_AddColAll", "AddColAll", "", true, strDefaultValue));
        list.Add(new ControlData("Label", "新增"));
        strTemp = HtmlUtil.RenderControl(list);
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        css.Add("width", strWidth);
        row.Cells.Add(cell);
        //修改
        cell = new HtmlTableCell();
        list.Clear();
        list.Add(new ControlData("Checkbox", "GN_UpdateColAll", "CB_UpdateColAll", "UpdateColAll", "", true, strDefaultValue));
        list.Add(new ControlData("Label", "修改"));
        strTemp = HtmlUtil.RenderControl(list);
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        css.Add("width", strWidth);
        row.Cells.Add(cell);
        //刪除
        cell = new HtmlTableCell();
        list.Clear();
        list.Add(new ControlData("Checkbox", "GN_DeleteColAll", "CB_DeleteColAll", "DeleteColAll", "", true, strDefaultValue));
        list.Add(new ControlData("Label", "刪除"));
        strTemp = HtmlUtil.RenderControl(list);
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        css.Add("width", strWidth);
        row.Cells.Add(cell);
        //查詢
        cell = new HtmlTableCell();
        list.Clear();
        list.Add(new ControlData("Checkbox", "GN_QueryColAll", "CB_QueryColAll", "QueryColAll", "", true, strDefaultValue));
        list.Add(new ControlData("Label", "查詢"));
        strTemp = HtmlUtil.RenderControl(list);
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        css.Add("width", strWidth);
        row.Cells.Add(cell);
        //輸出
        cell = new HtmlTableCell();
        list.Clear();
        list.Add(new ControlData("Checkbox", "GN_PrintColAll", "CB_PrintColAll", "PrintColAll", "", true, strDefaultValue));
        list.Add(new ControlData("Label", "輸出"));
        strTemp = HtmlUtil.RenderControl(list);
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        css.Add("width", strWidth);
        row.Cells.Add(cell);
        //進入
        cell = new HtmlTableCell();
        list.Clear();
        list.Add(new ControlData("Checkbox", "GN_PrintColAll", "CB_FocusColAll", "FocusColAll", "", true, strDefaultValue));
        list.Add(new ControlData("Label", "進入"));
        strTemp = HtmlUtil.RenderControl(list);
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        css.Add("width", strWidth);
        row.Cells.Add(cell);
        //審核
        cell = new HtmlTableCell();
        list.Clear();
        list.Add(new ControlData("Checkbox", "GN_Examine1ColAll", "CB_Examine1ColAll", "Examine1ColAll", "", true, strDefaultValue));
        list.Add(new ControlData("Label", "審核"));
        strTemp = HtmlUtil.RenderControl(list);
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        css.Add("width", strWidth);
        row.Cells.Add(cell);
        //複審
        cell = new HtmlTableCell();
        list.Clear();
        list.Add(new ControlData("Checkbox", "GN_Examine2ColAll", "CB_Examine2ColAll", "Examine2ColAll", "", true, strDefaultValue));
        list.Add(new ControlData("Label", "複審"));
        strTemp = HtmlUtil.RenderControl(list);
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        css.Add("width", strWidth);
        row.Cells.Add(cell);
        //核示
        cell = new HtmlTableCell();
        list.Clear();
        list.Add(new ControlData("Checkbox", "GN_Examine3ColAll", "CB_Examine3ColAll", "Examine3ColAll", "", true, strDefaultValue));
        list.Add(new ControlData("Label", "核示"));
        strTemp = HtmlUtil.RenderControl(list);
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        css.Add("width", strWidth);
        row.Cells.Add(cell);
        //全選
        cell = new HtmlTableCell();
        strTemp = "全 選";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        css.Add("width", strWidth);
        row.Cells.Add(cell);
        //全不選
        cell = new HtmlTableCell();
        strTemp = "全不選";
        cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
        css = cell.Style;
        css.Add("text-align", "center");
        css.Add("font-size", strFontSize);
        css.Add("width", strWidth);
        row.Cells.Add(cell);
        
        table.Rows.Add(row);
        //-----------------------------------------------------------------------
        //轉成 html 碼
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        table.RenderControl(htw);
        return htw.InnerWriter.ToString();
    }
    //----------------------------------------------------------------------------
    private void LoadFormData()
    {
        LB_Title.Text = GetTitle();
        LB_Grid.Text = GetBody();
    }
    //----------------------------------------------------------------------------
    private string GetBody()
    {
        string strTemp = "";
        HtmlTable table = new HtmlTable();
        HtmlTableRow row;
        HtmlTableCell cell;
        CssStyleCollection css;
        table.Border = 1;
        table.CellPadding = 3;
        table.CellSpacing = 0;
        css = table.Style;
        css.Add("font-size", "24px");
        css.Add("font-family", "標楷體");
        css.Add("background", "#fff");
        string strFontSize = "14px";

        List<ControlData> list = new List<ControlData>();
        list.Clear();
        //------------------------------------------------------------------------
        //先取得所有的 Menu List, menuList 有 Level 資訊
        List<clsMenuList> menuList = new List<clsMenuList>();
        GetMenuList("0", 1, menuList);
        HFD_TotalMenuItem.Value = menuList.Count.ToString();
        //顯示內容
        for (int i = 0; i < menuList.Count; i++)
        {
            string[] LevelName = { "", "", "", "" };

            clsMenuList item = menuList[i];
            LevelName[item.Level - 1] = item.Name;
            row = new HtmlTableRow();

            cell = new HtmlTableCell();
            strTemp = (i + 1).ToString();
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            css.Add("width", "14px");
            cell.Attributes.Add("id", "MenuID" + i.ToString());
            cell.Attributes.Add("value", item.menuID);
            row.Cells.Add(cell);

            //共4 層
            for (int j = 0; j < 4; j++)
            {
                cell = new HtmlTableCell();
                strTemp = LevelName[j];
                cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
                css = cell.Style;
                css.Add("text-align", "center");
                css.Add("font-size", strFontSize);
                css.Add("width", "60px");
                row.Cells.Add(cell);
            }
            //取得 Menu 權限
            string strSql = " select * from AdminRight where MenuID=@MenuID and GroupID=@GroupID";
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("MenuID", item.menuID);
            dict.Add("GroupID", ddlGroup.SelectedValue);
            DataTable dt = NpoDB.GetDataTableS(strSql, dict);

            bool bAdd = false, bUpdate = false, bDelete = false, bQuery = false;
            bool bExamine1 = false, bExamine2 = false, bExamine3 = false;
            bool bPrint = false, bFocus = false;
            if (dt.Rows.Count > 0)
            {
                bAdd = dt.Rows[0]["_AddNew"].ToString() == "True" ? true : false;
                bUpdate = dt.Rows[0]["_Update"].ToString() == "True" ? true : false;
                bDelete = dt.Rows[0]["_Delete"].ToString() == "True" ? true : false;
                bQuery = dt.Rows[0]["_Query"].ToString() == "True" ? true : false;
                bPrint = dt.Rows[0]["_Print"].ToString() == "True" ? true : false;
                bFocus = dt.Rows[0]["_Focus"].ToString() == "True" ? true : false;
                bExamine1 = dt.Rows[0]["_Examine"].ToString() == "True" ? true : false;
                bExamine2 = dt.Rows[0]["_ReExamine"].ToString() == "True" ? true : false;
                bExamine3 = dt.Rows[0]["_Approve"].ToString() == "True" ? true : false;
            }

            //----------------------------------------------------------------------------
            string strWidth = "46px";
            //新 增
            cell = new HtmlTableCell();
            list.Clear();
            list.Add(new ControlData("Checkbox", "GN_Add", "CB_ColAdd" + i.ToString(), i.ToString(), "", false, null));
            list[0].Checked = bAdd;
            strTemp = HtmlUtil.RenderControl(list);
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            css.Add("width", strWidth);
            row.Cells.Add(cell);
            //修 改 
            cell = new HtmlTableCell();
            list.Clear();
            list.Add(new ControlData("Checkbox", "GN_Update", "CB_ColUpdate" + i.ToString(), i.ToString(), "", false, null));
            list[0].Checked = bUpdate;
            strTemp = HtmlUtil.RenderControl(list);
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            css.Add("width", strWidth);
            row.Cells.Add(cell);
            //刪 除 Delete
            cell = new HtmlTableCell();
            list.Clear();
            list.Add(new ControlData("Checkbox", "GN_Delete", "CB_ColDelete" + i.ToString(), i.ToString(), "", false, null));
            list[0].Checked = bDelete;
            strTemp = HtmlUtil.RenderControl(list);
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            css.Add("width", strWidth);
            row.Cells.Add(cell);
            //查 詢 
            cell = new HtmlTableCell();
            list.Clear();
            list.Add(new ControlData("Checkbox", "GN_Query", "CB_ColQuery" + i.ToString(), i.ToString(), "", false, null));
            list[0].Checked = bQuery;
            strTemp = HtmlUtil.RenderControl(list);
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            css.Add("width", strWidth);
            row.Cells.Add(cell);
            //輸 出 
            cell = new HtmlTableCell();
            list.Clear();
            list.Add(new ControlData("Checkbox", "GN_Print", "CB_ColPrint" + i.ToString(), i.ToString(), "", false, null));
            list[0].Checked = bPrint;
            strTemp = HtmlUtil.RenderControl(list);
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            css.Add("width", strWidth);
            row.Cells.Add(cell);
            //進入
            cell = new HtmlTableCell();
            list.Clear();
            list.Add(new ControlData("Checkbox", "GN_Focus", "CB_ColFocus" + i.ToString(), i.ToString(), "", false, null));
            list[0].Checked = bFocus;
            strTemp = HtmlUtil.RenderControl(list);
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            css.Add("width", strWidth);
            row.Cells.Add(cell);
            //審 核 
            cell = new HtmlTableCell();
            list.Clear();
            list.Add(new ControlData("Checkbox", "GN_Examine1", "CB_ColExamine1" + i.ToString(), i.ToString(), "", false, null));
            list[0].Checked = bExamine1;
            strTemp = HtmlUtil.RenderControl(list);
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            css.Add("width", strWidth);
            row.Cells.Add(cell);
            //複 審 Examine2
            cell = new HtmlTableCell();
            list.Clear();
            list.Add(new ControlData("Checkbox", "GN_Examine2", "CB_ColExamine2" + i.ToString(), i.ToString(), "", false, null));
            list[0].Checked = bExamine2;
            strTemp = HtmlUtil.RenderControl(list);
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            css.Add("width", strWidth);
            row.Cells.Add(cell);
            //核 示 
            cell = new HtmlTableCell();
            list.Clear();
            list.Add(new ControlData("Checkbox", "GN_Examine3", "CB_ColExamine3" + i.ToString(), i.ToString(), "", false, null));
            list[0].Checked = bExamine3;
            strTemp = HtmlUtil.RenderControl(list);
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            css.Add("width", strWidth);
            row.Cells.Add(cell);
            //----------------------------------------------------------------------------
            //全 選 
            cell = new HtmlTableCell();
            list.Clear();
            list.Add(new ControlData("Checkbox", "GN_RowSelectAll", "CB_RowSelectAll" + i.ToString(), i.ToString(), "", false, null));
            list[0].Checked = false;
            strTemp = HtmlUtil.RenderControl(list);
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            css.Add("width", "45px");
            row.Cells.Add(cell);
            //全不選 
            cell = new HtmlTableCell();
            list.Clear();
            list.Add(new ControlData("Checkbox", "GN_RowUnSelectAll", "CB_RowUnSelectAll" + i.ToString(), i.ToString(), "", false, null));
            list[0].Checked = false;
            strTemp = HtmlUtil.RenderControl(list);
            cell.InnerHtml = strTemp == "" ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", "center");
            css.Add("font-size", strFontSize);
            css.Add("width", "45px");
            row.Cells.Add(cell);

            table.Rows.Add(row);
        }
        //轉成 html 碼
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        table.RenderControl(htw);
        return htw.InnerWriter.ToString();
    }
    //----------------------------------------------------------------------------
    private void GetMenuList(string ParentID, int Level, List<clsMenuList> menuList)
    {
        string strSql = " select * from AdminMenu\n";
        strSql += "where ParentID=@ParentID\n";
        strSql += "and IsUse=1  order by Sort\n";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("ParentID", ParentID);
        DataTable dtTopLevel = NpoDB.GetDataTableS(strSql, dict);
        if (dtTopLevel.Rows.Count != 0)
        {
            foreach (DataRow dr in dtTopLevel.Rows)
            {
                clsMenuList item = new clsMenuList();
                item.menuID = dr["MenuID"].ToString();
                item.Name = dr["MenuName"].ToString();
                item.Level = Level;
                menuList.Add(item);
                string strParentID = dr["MenuID"].ToString();
                GetMenuList(strParentID, Level + 1, menuList);
            }
        }
    }
    //----------------------------------------------------------------------------
    protected void BindData()
    {
        LoadFormData();
    }
    //----------------------------------------------------------------------------
    protected void btnQuery_Click(object sender, ImageClickEventArgs e)
    {
        BindData();
    }
    //----------------------------------------------------------------------------
    protected void KeyFocus_OnClick(object sender, EventArgs e)
    {
        SaveData();
        ShowSysMsg("資料更新成功");
        LoadFormData();
    }
    //----------------------------------------------------------------------------
    private void SaveData()
    {
        string[] SetupValues = HFD_SetupValue.Value.Split('|');
        string strSql = "";
        for (int i = 0; i < SetupValues.Length; i++)
        {
            string[] SetupValue = SetupValues[i].Split(',');
            string MenuID = SetupValue[0];
            Dictionary<string, object> dict = new Dictionary<string, object>();
            strSql = "select * from  AdminRight\n";
            strSql += " where MenuID=@MenuID and GroupID=@GroupID";
            dict.Add("MenuID", MenuID);
            dict.Add("GroupID", ddlGroup.SelectedValue);

            if (NpoDB.GetDataTableS(strSql, dict).Rows.Count == 0)
            {
                strSql = " insert into AdminRight ";
                strSql += "( MenuID, GroupID, _AddNew, _Update, _Delete, _Query, _Print, _Focus, _Examine, _ReExamine, _Approve,  UpdateDate) values";
                strSql += "(@MenuID,@GroupID,@_AddNew,@_Update,@_Delete,@_Query,@_Print,@_Focus,@_Examine,@_ReExamine,@_Approve, getdate()) ";
            }
            else
            {
                strSql = "update AdminRight set ";
                strSql += "  _AddNew=@_AddNew";
                strSql += ", _Update=@_Update";
                strSql += ", _Delete=@_Delete";
                strSql += ", _Query=@_Query";
                strSql += ", _Print=@_Print";
                strSql += ", _Focus=@_Focus";
                strSql += ", _Examine=@_Examine";
                strSql += ", _ReExamine=@_ReExamine";
                strSql += ", _Approve=@_Approve";
                strSql += " where MenuID=@MenuID and GroupID=@GroupID";
            }
            dict.Clear();
            dict.Add("MenuID", MenuID);
            dict.Add("GroupID", ddlGroup.SelectedValue);
            dict.Add("_AddNew", SetupValue[1]);
            dict.Add("_Update", SetupValue[2]);
            dict.Add("_Delete", SetupValue[3]);
            dict.Add("_Query", SetupValue[4]);
            dict.Add("_Print", SetupValue[5]);
            dict.Add("_Focus", SetupValue[6]);
            dict.Add("_Examine", SetupValue[7]);
            dict.Add("_ReExamine", SetupValue[8]);
            dict.Add("_Approve", SetupValue[9]);
            NpoDB.ExecuteSQLS(strSql, dict);
        }
    }
    //----------------------------------------------------------------------------
    protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
    }
    //----------------------------------------------------------------------------
}

//----------------------------------------------------------------------------
public class clsMenuList
{
    public string menuID;
    public string Name;
    public int Level;
    }

