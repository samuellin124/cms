/// <summary>
/// Summary 使用者的所有相關資料
/// </summary>
public class CSessionInfo
{
    public string OrgID;                //機關代號
    public string OrgName;              //機關名稱
    public string UserID;               //使用者代號
    public string UserName;             //使用者名稱
    public string DeptID;               //單位ID
    public string DeptName;             //單位中文名稱
    public string GroupID;              //使用者權限群組ID
    public string GroupName;            //使用者權限群組
    public string GroupArea;            //使用者權限範圍

    public CSessionInfo()
	{
	}    

    public void Clear()
	{
        OrgID = "";
        OrgName = "";
        UserID = "";
        UserName = "";
        DeptID = "";
        DeptName = "";
        GroupID = "";
        GroupName = "";
        GroupArea= "";
	}    
}
