/*
Copyright (c) 2003-2010, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function( config )
{
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
	// config.uiColor = '#AADC6E';
	// config.height = 350
	config.font_names = '新細明體;標楷體;Arial;Comic Sans MS;Courier New;Tahoma;Times New Roman;Verdana';
	// config.filebrowserBrowseUrl = '/ckfinder/ckfinder.html';
    // config.filebrowserImageBrowseUrl = '/ckfinder/ckfinder.html?Type=Images';
    // config.filebrowserFlashBrowseUrl = '/ckfinder/ckfinder.html?Type=Flash';
    // config.filebrowserUploadUrl = '/ckfinder/core/connector/asp/connector.asp?command=QuickUpload&type=Files';
    // config.filebrowserImageUploadUrl = '/ckfinder/core/connector/asp/connector.asp?command=QuickUpload&type=Images';
    // config.filebrowserFlashUploadUrl = '/ckfinder/core/connector/asp/connector.asp?command=QuickUpload&type=Flash';
  config.toolbar = 'MyToolbar';

  //config.toolbar_MyToolbar =
    //[
        //['Source','NewPage','Preview'],
        //['Cut','Copy','Paste','PasteText','PasteFromWord'],
        //['Undo','Redo','-','Find','Replace','-','SelectAll','RemoveFormat'],
        //['Image','Flash','Table','HorizontalRule','Smiley','SpecialChar'],
	//['JustifyLeft','JustifyCenter','JustifyRight'],
        //'/',
        //['Format','Font','FontSize'],
	//['TextColor','BGColor'],
        //['Bold','Italic','Underline','Strike'],
        //['NumberedList','BulletedList','-','Outdent','Indent'],
        //['Link','Unlink','Maximize']
    //];
    
  config.toolbar_MyToolbar =
    [
        ['Font', 'FontSize'],
        ['TextColor', 'BGColor'],
        ['Bold', 'Italic', 'Underline'],
        ['JustifyLeft', 'JustifyCenter', 'JustifyRight'],
        '/',
        ['Table', 'HorizontalRule'],
        ['NumberedList', 'BulletedList'],
        ['Link','Unlink']
    ];
};
