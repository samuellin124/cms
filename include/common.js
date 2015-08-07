//左邊 menu 隱藏顯示控制
function InitMenu() {
    var Switch_Status;
    try {
        Switch_Status = parent.document.getElementsByTagName('frameset')[1].cols;
    }
    catch (err) {
        $('#menu_hide').hide();
        $('#menu_show').hide();
        return;
    }
    
    if (Switch_Status == "162,*") {
        $('#menu_show').hide();
    } else if (Switch_Status == "0,*") {
        $('#menu_hide').hide();
    }

    $('#menu_hide').click(function () {
        $('#menu_show').show();
        $('#menu_hide').hide();
    });
    $('#menu_show').click(function () {
        $('#menu_hide').show();
        $('#menu_show').hide();
    });

    //2013/05/03 rocky, 解決 swapImage 只能有一個 image 的問題
    $('#menu_hide').hover(function () {
        var $th = $(this);
        var newSrc = '../images/menu_hide_over.gif';
        $th.attr('src', newSrc)
    },
    function () {
        var $th = $(this);
        var newSrc = '../images/menu_hide.gif';
        $th.attr('src', newSrc)
    });

    $('#menu_show').hover(function () {
        var $th = $(this);
        var newSrc = '../images/menu_show_over.gif';
        $th.attr('src', newSrc)

        $('#menu_show').click(); //自動展開
    },
    function () {
        var $th = $(this);
        var newSrc = '../images/menu_show.gif';
        $th.attr('src', newSrc)
    });

    //$('#menu_hide').click();
    //parent.document.getElementsByTagName('frameset')[1].cols = '0,*';
}

function SwitchMenu() {
    var Switch_Status = parent.document.getElementsByTagName('frameset')[1].cols;
    var a = parent.document.getElementsByTagName('frameset')[1];
    var b = $('frameset').eq(2).attr('rocky');
    if (Switch_Status == "162,*") {
        parent.document.getElementsByTagName('frameset')[1].cols = '0,*';
    } else if (Switch_Status == "0,*") {
        parent.document.getElementsByTagName('frameset')[1].cols = '162,*';
    }
}
//----------------------------------------------------------------------------
//檢查日期
function CheckDateFormat(Control, FieldDesc) {
    var alertStr = CheckDate(Control.id, FieldDesc, false);
    if (alertStr != "") {
        document.getElementById(Control.id).value = "";
        alert(alertStr);
    }
}
//---------------------------------------------------------------------------
//判斷是否為日期格式
function CheckDate(ControlID, FieldDesc, blnRequest) {
    var control = GetControl(ControlID);
    var value = GetControlValue(ControlID);
    if (blnRequest == true) {
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
    if (!objRegExp.test(strValue)) {
        return FieldDesc + " 欄位必須為西元日期格式 ! (yyyy/mm/dd)\r\n";
        //doesn't match pattern, bad date
    }
    else {
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
        if (arrayLookup[arrayDate[1]] != null) {
            if (intDay <= arrayLookup[arrayDate[1]] && intDay != 0)
                flag = true; //found in lookup table, good date
        }

        //check for February (bugfix 20050322)
        //bugfix  for parseInt kevin
        //bugfix  biss year  O.Jp Voutat
        var intMonth = parseInt(arrayDate[1], 10);
        if (intMonth == 2) {
            var intYear = parseInt(arrayDate[0]);
            if (intDay > 0 && intDay < 29) {
                flag = true;
            }
            else if (intDay == 29) {
                if ((intYear % 4 == 0) && (intYear % 100 != 0) ||
                    (intYear % 400 == 0)) {
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
//---------------------------------------------------------------------------
function IsValidDate(Year, Month, Day) {
    var dayLookup = { 1: 31, 3: 31, 4: 30, 5: 31, 6: 30,
        7: 31, 8: 31, 9: 30, 10: 31, 11: 30, 12: 31
    }

    if (Month == 2) {
        var days = daysInFebruary(Year);
        if (Day > days) {
            return false;
        }
    }

    if (Day > dayLookup[Month]) {
        return false;
    }
    return true;
}
//---------------------------------------------------------------------------
function daysInFebruary(year) {
    // February has 29 days in any year evenly divisible by four,
    // EXCEPT for centurial years which are not also divisible by 400.
    return (((year % 4 == 0) && ((!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28);
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
//function CheckDateFormat(Control, FieldDesc)
//{
//    var alertStr = CheckDate(Control.id, FieldDesc, false);
//    if (alertStr != "")
//    {
//        document.getElementById(Control.id).value = "";
//        alert(alertStr);
//    }
//}
////----------------------------------------------------------------------------
function SetHiddenValue(form, HiddenValue, array) {
    $(form).submit(function () {
        var val = '';
        val = 'hiddenValueHeader|';
        $.each(array, function (index, value) {
            if (value.type == 'text') {
                val += value.id + '|' + value.type + '|' + $('#' + value.id).getValue() + "|";
            }
            else if (value.type == 'checkbox') {
                val += value.id + '|' + value.type + '|' + $('input:checkbox[name=' + value.id + ']').getValue() + "|";
            }
            else if (value.type == 'radio') {
                val += value.id + '|' + value.type + '|' + $('input:radio[name=' + value.id + ']').getValue() + "|";
            }
            //alert(value);
        })

        //$('#HFD_Value').val(val);
        $('#' + HiddenValue).val(val);
    })
}
//----------------------------------------------------------------------------
function GetHiddenValue(HiddenValue) {
    var hiddenValueArray = $('#' + HiddenValue).val().split('|');
    var Len = hiddenValueArray.length;
    if (Len > 0) {
        if (hiddenValueArray[0] == 'hiddenValueHeader') {
            for (var i = 1; i <= Len - 2; i += 3) {
                if (hiddenValueArray[i + 1] == 'text') {
                    $('#' + hiddenValueArray[i]).setValue(hiddenValueArray[i + 2]);
                }
                if (hiddenValueArray[i + 1] == 'checkbox') {
                    $('input:checkbox[name=' + hiddenValueArray[i] + ']').setValue(hiddenValueArray[i + 2]);
                }
                else if (hiddenValueArray[i + 1] == 'radio') {
                    $('input:radio[name=' + hiddenValueArray[i] + ']').setValue(hiddenValueArray[i + 2]);
                }
            }
        }
    }
}
//----------------------------------------------------------------------------
function initCalendar(array) {
    $.each(array, function (index, value) {
        Calendar.setup({
            inputField: "txt" + value,   // id of the input field
            button: "img" + value        // 與觸發動作的物件ID相同
        });
    })
}
//----------------------------------------------------------------------------
//此函數只需有 ../images/loading.gif 的圖檔即可
function InitAjaxLoading(ID) {
    if (ID == undefined) {
        ID = '#spinner';
        //兩種寫法都可以
        $("<div id='spinner' class='spinner' style='display:none;'><img id='img-spinner' src='../images/loading.gif' alt='Loading'/></div>").insertAfter('body');
        //$('body').after("<div id='spinner' class='spinner' style='display:none;'><img id='img-spinner' src='../images/loading.gif' alt='Loading'/></div>");
    }

    $(ID).css({ 'position': 'fixed', 'top': '50%', 'left': '50%',
                'margin-left': '-50px', 'margin-top': '-50px',
                'text-align': 'center', 'z-index': '1234', 'overflow': 'auto', 
                'width':'100px', 'height':'102px'}).
    bind("ajaxSend", function() {
        $(this).show();
    }).bind("ajaxStop", function() {
        $(this).hide();
    }).bind("ajaxError", function() {
        $(this).hide();
    });
}
//----------------------------------------------------------------------------
function String2Number(data) {
    if (data == '')
        return 0;
    try {
        var Num = parseInt(data, 10);
        if (isNaN(Num)) {
            return 0;
        }
        return Num;
    }
    catch (Error) {
        return 0;
    }
}
//-------------------------------------------------------------------------
function BindNumeric(selector) {
    if (selector == undefined) {
        //class 為 Numeric 的欄位只能輸入數字
        $('.Numeric').bind("keyup", function(e) {
            var val = $(this).val();
            if (isNaN(val)) {
                val = val.replace(/[^0-9\.]/g, '');
                if (val.split('.').length > 2) {
                    val = val.replace(/\.+$/, '');
                }
                $(this).val(val);
            }
        });
    }
    else { //有輸入 selector, 則以 selector 為對象
        $(selector).bind("keyup", function(e) {
            var val = $(this).val();
            if (isNaN(val)) {
                val = val.replace(/[^0-9\.]/g, '');
                if (val.split('.').length > 2) {
                    val = val.replace(/\.+$/, '');
                }
                $(this).val(val);
            }
        });
    }
}
//-------------------------------------------------------------------------
// Test for letters (only good up to char 127)
function isAlpha(aChar) {
    myCharCode = aChar.charCodeAt(0);

    if (((myCharCode > 64) && (myCharCode < 91)) ||
        ((myCharCode > 96) && (myCharCode < 123))) {
        return true;
    }

    return false;
}
//-------------------------------------------------------------------------
// Test for digits
function isDigit(aChar) {
    myCharCode = aChar.charCodeAt(0);

    if ((myCharCode > 47) && (myCharCode < 58)) {
        return true;
    }

    return false;
}
//-------------------------------------------------------------------------
function ShowAjaxFailMsg(ModuleName) {
    var Msg = 'Ajax error:讀取後端資料時發生錯誤!';
    if (ModuleName != undefined)
    {
        Msg += '(' + ModuleName + ')';
    }
    alert(Msg);
}
