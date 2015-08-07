//左邊 menu 隱藏顯示控制
function InitMenu()
{
    var Switch_Status = parent.document.getElementsByTagName('frameset')[1].cols;
    if (Switch_Status == "162,*") {
        $('#menu_show').addClass('hidden');
    } else if (Switch_Status == "0,*") {
        $('#menu_hide').addClass('hidden');
    }

    $('#menu_hide').click(function () {
        $('#menu_show').removeClass('hidden');
        $('#menu_hide').addClass('hidden');
    });
    $('#menu_show').click(function () {
        $('#menu_hide').removeClass('hidden');
        $('#menu_show').addClass('hidden');
    });

    $.swapImage(".swapImage");
    $.swapImage(".swapImageClick", true, true, "click");
    $.swapImage(".swapImageDoubleClick", true, true, "dblclick");
    $.swapImage(".swapImageSingleClick", true, false, "click");
    $.swapImage(".swapImageDisjoint");
}

function SwitchMenu() {
    var Switch_Status = parent.document.getElementsByTagName('frameset')[1].cols;
    if (Switch_Status == "162,*") {
        parent.document.getElementsByTagName('frameset')[1].cols = '0,*';
    } else if (Switch_Status == "0,*") {
        parent.document.getElementsByTagName('frameset')[1].cols = '162,*';
    }
}
//----------------------------------------------------------------------------
//檢查日期
function CheckDateFormat(ControlID, FieldDesc)
{
    var alertStr = CheckDate(ControlID, FieldDesc, false);

    if (alertStr != "") {
        document.getElementById(ControlID).value = "";
        alert(alertStr);
    }
}
//----------------------------------------------------------------------------
//檢測中英文夾雜字串實際長度
function CheckLen(ControlID, Length, FieldDesc, blnRequest) {
    var control = GetControl(ControlID);
    var value = GetControlValue(ControlID);
    var strAlert = "";
    if (blnRequest == true) {
        strAlert = CheckControl("Request", ControlID, FieldDesc);
        if (strAlert != "")
            return strAlert;
    }
    else if (value == "") return "";


    if (blen(value) > Length) {
        return FieldDesc + " 欄位中文字不可超過" + (Math.floor(Length / 2)) + "個字或英文字不可超過" + Length + "個字!\r\n";
    }
    return "";
}
//----------------------------------------------------------------------------
//檢測中英文夾雜字串實際長度
function blen(str) {
    //參考網址
    //http://www.360doc.com/content/081222/15/16915_2177384.html
    var len = str.match(/[^ -~]/g) == null ? str.length : str.length + str.match(/[^ -~]/g).length;
    return len;
}
//----------------------------------------------------------------------------
//判斷是否為日期格式
function CheckDate(ControlID, FieldDesc, blnRequest) {
    var control = GetControl(ControlID);
    var value = GetControlValue(ControlID);
    if (blnRequest == true)
    {
        strAlert = CheckControl("Request", ControlID, FieldDesc);
        if (strAlert != "")
            return strAlert;
    }
    else if (value == "") return "";
    //規則 yyyy/mm/dd
    var obj, strValue, flag = false;
    strValue = GetControlValue(ControlID);
    if (strValue == "")
        return "";
    var objRegExp = /^\d{4}\/\d{1,2}\/\d{1,2}$/;

    //check to see if in correct format
    if (!objRegExp.test(strValue))
    {
        return FieldDesc + " 欄位必須為西元日期格式 ! (yyyy/mm/dd)\r\n";
        //doesn't match pattern, bad date
    }
    else
    {
        var strSeparator = '/';
        var arrayDate = strValue.split(strSeparator);
        //create a lookup for months not equal to Feb.
        var arrayLookup = { '01': 31, '03': 31,
            '04': 30, '05': 31,
            '06': 30, '07': 31,
            '08': 31, '09': 30,
            '10': 31, '11': 30, '12': 31
        }
        var intDay = parseInt(arrayDate[2], 10);

        //check if month value and day value agree
        if (arrayLookup[arrayDate[1]] != null)
        {
            if (intDay <= arrayLookup[arrayDate[1]] && intDay != 0)
                flag = true; //found in lookup table, good date
        }

        //check for February (bugfix 20050322)
        //bugfix  for parseInt kevin
        //bugfix  biss year  O.Jp Voutat
        var intMonth = parseInt(arrayDate[1], 10);
        if (intMonth == 2)
        {
            var intYear = parseInt(arrayDate[0]);
            if (intDay > 0 && intDay < 29)
            {
                flag = true;
            }
            else if (intDay == 29)
            {
                if ((intYear % 4 == 0) && (intYear % 100 != 0) ||
                    (intYear % 400 == 0))
                {
                    // year div by 4 and ((not div by 100) or div by 400) ->ok
                    flag = true;
                }
            }
        }
    }

    if (flag == false) {
        return FieldDesc + " 欄位日期資料有誤!\r\n";
    }
    return "";
}
//----------------------------------------------------------------------------
//取得控制項
function GetControlValue(ControlID)
{
    var control;
    var value;
    control = GetControl(ControlID);
    if (control.tagName.toLowerCase() == "select")
    {
        if (control.options.length == 0)
            value = "";
        else
            value = control.options[control.selectedIndex].value;
    }
    else
        value = control.value;
    return value
}
//----------------------------------------------------------------------------
//取得控制項的值
function GetControl(ControlID)
{
    return document.getElementById(ControlID);
}
//----------------------------------------------------------------------------
function CheckDateFormat(Control, FieldDesc)
{
    var alertStr = CheckDate(Control.id, FieldDesc, false);
    if (alertStr != "")
    {
        document.getElementById(Control.id).value = "";
        alert(alertStr);
    }
}
//----------------------------------------------------------------------------
