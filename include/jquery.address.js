(function ($) {
    $.fn.CityAreaZip = function (ddlCity, ddlArea, txtZip, HFD_Area) {
        $('#' + ddlCity).bind('change', function (e) {
            var CityID = $(e.target).val();
            //alert(CityID);
            ChangeArea(e, ddlArea);
        });
        //----------------------------------------------------------------------
        $('#' + ddlArea).bind("change", function (e) {
            var ZipCode = $(this).val();
            if (ZipCode != '0') {
                $('#' + txtZip).val(ZipCode);
                $('#' + HFD_Area).val(ZipCode);
            }
        });
        //----------------------------------------------------------------------
        //以 CityID 取得其鄉鎮列表
        function ChangeArea(e, ddlArea) {
            var CityID = $(e.target).val();
            $.ajax({
                type: 'post',
                url: "../common/ajax.aspx",
                data: 'Type=1' + '&CityID=' + CityID,
                async: false,
                success: function (result) {
                    $('select[id$=' + ddlArea + ']').html(result);
                    //
                    $('#' + HFD_Area).val('');
                    $('#' + txtZip).val('');
                },
                error: function () { alert('ajax failed'); }
            })
        } //end of ChangeArea()
        //----------------------------------------------------------------------
        //alert("City=" + City + " Area=" + Area + " Zip=" + Zip);
    } //end of $.fn.CityAreaZip
    //--------------------------------------------------------------------------
    //同戶籍地址
    $.fn.SetSameAddress = function (ddlDestCity, ddlDestArea, txtDestZip, destHFD_Area, txtDestAddress,
                                    ddlSrcCity, ddlSrcArea, txtSrcZip, txtSrcAddress) {
        var CityID = $('#' + ddlSrcCity).val();
        var AreaID = $('#' + ddlSrcArea).val();
        var ZipCode = $('#' + txtSrcZip).val();
        var Address = $('#' + txtSrcAddress).val();
        $('#' + ddlDestCity).val(CityID);
        //要先 load 鄉鎮資料
        ChangeArea2(CityID, AreaID, ddlDestArea)
        $('#' + txtDestZip).val(ZipCode);
        $('#' + txtDestAddress).val(Address);
        $('#' + destHFD_Area).val(AreaID);
    } //end of $.fn.SetSameAddress
    //--------------------------------------------------------------------------
    //設定地址
    $.fn.SetAddress = function (ddlDestCity, ddlDestArea, txtDestZip, destHFD_Area, txtDestAddress,
                    CityID, AreaID, ZipCode, Address) {
        $('#' + ddlDestCity).val(CityID);
        //要先 load 鄉鎮資料
        ChangeArea2(CityID, AreaID, ddlDestArea)
        $('#' + txtDestZip).val(ZipCode);
        $('#' + txtDestAddress).val(Address);
        $('#' + destHFD_Area).val(AreaID);
    } //end of $.fn.SetSameAddress
    //--------------------------------------------------------------------------
    //設定地址
    $.fn.SetAddressNew = function (ddlDestCity, ddlDestArea, ddlDestStreet, txtDestZip, destHFD_Area, destHFD_Street, txtDestAddress,
                    CityID, AreaID, StreetID, ZipCode, Address) {
        $('#' + ddlDestCity).val(CityID);
        //要先 load 鄉鎮資料
        ChangeArea2(CityID, AreaID, ddlDestArea)
        //要先 load 街名資料
        ChangeStreet2(CityID, AreaID, StreetID, ddlDestStreet)
        $('#' + txtDestZip).val(ZipCode);
        $('#' + txtDestAddress).val(Address);
        $('#' + destHFD_Area).val(AreaID);
        $('#' + destHFD_Street).val(StreetID);
    } //end of $.fn.SetSameAddress
    //--------------------------------------------------------------------------
    function ChangeArea2(CityID, AreaID, destAreaID) {
        $.ajax({
            type: 'post',
            url: "../common/ajax.aspx",
            data: 'Type=1' + '&CityID=' + CityID,
            async: false,
            success: function (result) {
                $('select[id$=' + destAreaID + ']').html(result);
                $('#' + destAreaID).val(AreaID);
            },
            error: function () { alert('ajax failed'); }
        })
    }
    //--------------------------------------------------------------------------
    function ChangeStreet2(CityID, AreaID, StreetID, destStreetID) {
        $.ajax({
            type: 'post',
            url: "../common/ajax.aspx",
            data: 'Type=102' + '&CityID=' + CityID + '&AreaID=' + AreaID,
            async: false,
            success: function (result) {
                $('select[id$=' + destStreetID + ']').html(result);
                $('#' + destStreetID).val(StreetID);
            },
            error: function () { alert('ajax failed'); }
        })
    } //end of ChangeStreet2()
    //--------------------------------------------------------------------------
    //新加街名選擇
    $.fn.CityAreaStreetZip = function (ddlCity, ddlArea, ddlStreet, txtZip, HFD_Area, HFD_Street) {
        $('#' + ddlCity).bind('change', function (e) {
            var CityID = $(e.target).val();
            ChangeArea(e, ddlArea);
        });
        //----------------------------------------------------------------------
        $('#' + ddlArea).bind("change", function (e) {
            var ZipCode = $(this).val();
            if (ZipCode != '0') {
                $('#' + txtZip).val(ZipCode);
                $('#' + HFD_Area).val(ZipCode);
            }
            ChangeStreet(e, ddlCity, ddlArea);
        });
        //----------------------------------------------------------------------
        $('#' + ddlStreet).bind("change", function (e) {
            var StreetUID = $(this).val();
            $('#' + HFD_Street).val(StreetUID);
        });
        //----------------------------------------------------------------------
        //以 CityID 取得其鄉鎮列表
        function ChangeArea(e, ddlArea) {
            var CityID = $(e.target).val();
            $.ajax({
                type: 'post',
                url: "../common/ajax.aspx",
                data: 'Type=1' + '&CityID=' + CityID,
                async: false,
                success: function (result) {
                    $('select[id$=' + ddlArea + ']').html(result);
                    //
                    $('#' + HFD_Area).val('');
                    $('#' + txtZip).val('');
                },
                error: function () { alert('ajax failed'); }
            })
        } //end of ChangeArea()
        //----------------------------------------------------------------------
        //以 AreaID 取得其街名列表
        function ChangeStreet(e, ddlCity, ddlArea) {
            var CityName = $('#' + ddlCity + ' option:selected').text();
            var AreaName = $('#' + ddlArea + ' option:selected').text();
            $.ajax({
                type: 'post',
                url: "../common/ajax.aspx",
                data: 'Type=101' + '&CityName=' + CityName + '&AreaName=' + AreaName,
                async: false,
                success: function (result) {
                    $('select[id$=' + ddlStreet + ']').html(result);
                    $('#' + HFD_Street).val('');
                },
                error: function () { alert('ajax failed'); }
            })
        } //end of ChangeStreet()
        //----------------------------------------------------------------------
        
        
        
        //alert("City=" + City + " Area=" + Area + " Zip=" + Zip);
    } //end of $.fn.CityAreaZip
    //--------------------------------------------------------------------------
})(jQuery);

