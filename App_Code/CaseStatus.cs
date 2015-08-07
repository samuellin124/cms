using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// CaseStatus 的摘要描述
/// </summary>
public class CaseClass
{
    //個案表單處理 
    //1.開案訪視表
    //2.年度訪視表
    //3.個案轉介表
    //4.心輔轉介表
    //5.內部轉介表
    //6.個案結案表
    //7.服務紀錄表
    //8.個案申請表
    //9.個案不開案表
    public CaseClass()
    {
        //
        // TODO: 在此加入建構函式的程式碼
        //
    }
    //---------------------------------------------------------------------------
    public static string CaseVisit
    {
        get { return "開案訪視表"; }
    }
    public static string CaseYear
    {
        get { return "年度訪視表"; }
    }
    public static string CaseReferral
    {
        get { return "個案轉介表"; }
    }
    public static string CaseReferralPsychol
    {
        get { return "心輔轉介表"; }
    }
    public static string CaseReferralInternal
    {
        get { return "內部轉介表"; }
    }
    public static string CaseClose
    {
        get { return "個案結案表"; }
    }
    public static string CaseRecord
    {
        get { return "服務紀錄表"; }
    }
    public static string CaseApply
    {
        get { return "個案申請表"; }
    }
    public static string CaseUnopen
    {
        get { return "個案不開案表"; }
    }
}
/// <summary>
/// CaseStatus 的摘要描述
/// </summary>
public class CaseStatus
{
	public CaseStatus()
	{
		//
		// TODO: 在此加入建構函式的程式碼
		//
	}
    //---------------------------------------------------------------------------
    public static string CaseApply
    {
        get { return "已申請"; }
    }
    public static string CaseDispatch
    {
        get { return "已派案"; }
    }
    //---------------------------------------------------------------------------
    public static string CaseUnopenSubmit
    {
        get { return "不開案待審核"; }
    }
    public static string CaseUnopenReturn
    {
        get { return "不開案已退回"; }
    }
    public static string CaseUnopenVerify
    {
        get { return "不開案已審核"; }
    }
    //---------------------------------------------------------------------------
    public static string CaseVisitSubmit
    {
        get { return "開案訪視待審核"; }
    }
    public static string CaseVisitReturn
    {
        get { return "開案訪視已退回"; }
    }
    public static string CaseVisitVerify
    {
        get { return "開案訪視已審核"; }
    }
    //---------------------------------------------------------------------------
    public static string CaseYearSubmit
    {
        get { return "年度訪視待審核"; }
    }
    public static string CaseYearReturn
    {
        get { return "年度訪視已退回"; }
    }
    public static string CaseYearVerify
    {
        get { return "年度訪視已審核"; }
    }
    //---------------------------------------------------------------------------
    public static string CaseReferralSubmit
    {
        get { return "轉介待審核"; }
    }
    public static string CaseReferralReturn
    {
        get { return "轉介已退回"; }
    }
    public static string CaseReferralVerify
    {
        get { return "轉介已審核"; }
    }
    //---------------------------------------------------------------------------
    public static string CaseCloseSubmit
    {
        get { return "結案待審核"; }
    }
    public static string CaseCloseReturn
    {
        get { return "結案已退回"; }
    }
    public static string CaseCloseVerify
    {
        get { return "結案已審核"; }
    }
    //---------------------------------------------------------------------------
    public static string CaseRecordSubmit
    {
        get { return "服務紀錄待審核"; }
    }
    public static string CaseRecordReturn
    {
        get { return "服務紀錄已退回"; }
    }
    public static string CaseRecordVerify
    {
        get { return "服務紀錄已審核"; }
    }

}