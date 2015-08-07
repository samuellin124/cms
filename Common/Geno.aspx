<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Geno.aspx.cs" Inherits="Common_Geno" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=Big5" />
    <title>圖形編輯工具</title>
    <link href="../include/main.css" rel="stylesheet" type="text/css" />
    <link href="../include/table.css" rel="stylesheet" type="text/css" />
</head>
<body class="body">
    <form name="genoPanel" id="genoPanel" runat="server">
        <asp:HiddenField runat="server" ID="HFD_XML" />
        <asp:HiddenField runat="server" ID="HFD_Uid" />
        <asp:HiddenField runat="server" ID="HFD_TableName" />
        <asp:HiddenField runat="server" ID="HFD_FieldName" />
        <table style="font-size:10pt;margin:0px;border:0px;Padding:0px">
            <tr valign="top">
                <td valign="top" align="center">
                    <applet archive="Geno.jar" code="com.esound.geno.Geno" name="JGeno"  width="600" height="400" ></applet>
                </td>
                <td valign="top">
                    <input name="button" type="button" style="width:44px" onclick="addGuy();" value="男性" />
                    <input name="button2" type="button" style="width:44px" onclick="addGirl();" value="女性" />
                    <input type="button" name="interactb" style="width:90px" value="互動關係" onclick="addRelationship()" />
                    <br />
                    <input type="button" name="ecob" style="width:92px" value="居住關係" onclick="addEco(1)" />
                    <input type="button" name="ecob" style="width:90px" value="生態圈" onclick="addEco(0)" />
                    <br />
                    <input type="button" name="memo" style="width:186px;" value="注解文字" onclick="addFloatText();" />
                    
                    <div id="GenoMenu1" style="display:none;">
                        <table border="1" style="font-size:10pt;margin:1px;border:1px;Padding:1px;width:180px;">
        	                 <tr>
        	                     <td align="center">
        	                         <b>成員屬性設定</b>    
        	                     </td>
        	                 </tr>
        	                 <tr>
        	                     <td>
        	                         性別：<input type="radio" name="jrbg" id="jrbg1" value="1" onclick="setCurrentSex(this.value);" /> 男<input type="radio" name="jrbg" id="jrbg2" value="2" onclick="setCurrentSex(this.value);" /> 女 <input type="radio" name="jrbg" id="jrbg3" value="0" onclick="setCurrentSex(this.value);" /> 不詳 <br />
        	                         年齡： <input type="text" name="jtf1" style="margin:1px;border-width:1px;width:66px" maxlength="3" onkeyup="setCurrentAge(this.value);" /> 歲<br />              
        	                         狀態：<input type="checkbox" name="jcb2" id="jcb2" onclick="setCurrentMaster(this.checked);" /> 案主 <input type="checkbox" name="jcb1" id="jcb1" onclick="setCurrentAlive(this.checked);" /> 死亡
        	                         <br />
        	                         生養：<input type="radio" name="jrbp" id="jrbp0" value="0" onclick="setIsAdopted(0);" checked="checked" /> 親生 <input type="radio" name="jrbp" id="jrbp4" value="4" onclick="setIsAdopted(1);" /> 收養
        	                         <br />
        	                     　　　<input type="radio" name="jrbp" id="jrbp1" value="1" onclick="setCurrentPregnancy(this.value);" /> 胎兒 <input type="radio" name="jrbp" id="jrbp2" value="2" onclick="setCurrentPregnancy(this.value);" /> 墮胎
        	                         <br />
        	                     　　　<input type="radio" name="jrbp" id="jrbp3" value="3" onclick="setCurrentPregnancy(this.value);" /> 流產
        	                         <br />
        	                         備註：
        	                         <br />
        	                         <textarea cols="9" rows="3" style="width:170px;overflow:hidden;" name="jta1" onkeyup="setCurrentMemo(this.value);"></textarea>
        	                     </td>
        	                 </tr>
                        </table>
                    </div>
            
                    <div id='GenoMenu2' style="display:none;"> 
                        <table border="1" style="font-size:10pt;margin:1px;border:1px;Padding:1px;width:180px;">
                            <tr>
                                <td align="center">
                                    <b>配偶關係種類</b>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="radio" name="marriage" id="marriage1" value="0" onclick="setLineType(this.value);" /> 結婚
                                    <br />
                                    <input type="radio" name="marriage" id="marriage3" value="2" onclick="setLineType(this.value);" /> 離婚
                                    <br />
                                    <input type="radio" name="marriage" id="marriage4" value="3" onclick="setLineType(this.value);" /> 同居
                                    <br />
                                    <input type="radio" name="marriage" id="marriage2" value="1" onclick="setLineType(this.value);" /> 分居
                                    <br />
                                    <input type="radio" name="marriage" id="marriage5" value="4" onclick="setLineType(this.value);" /> 曾經同居
                                </td>
                            </tr>
                        </table>
                    </div>	
            	
                    <div id="GenoMenu3" style="display:none;">
	                    <table border="1" style="font-size:10pt;margin:1px;border:1px;Padding:1px;width:180px;">
	                        <tr>
	                            <td align="center">
	                                <b>注解文字</b>    
	                            </td>
	                        </tr>
	                        <tr>
	                            <td>
  			                         <textarea cols="10" rows="5" style="width:170px;overflow:hidden;" name="floatText" id="ftMemo" onkeyup="setFTMemo(this.value);"></textarea>
	                            </td>
	                        </tr>
                        </table>
                    </div>
                    
                    <div id="GenoMenu4" style="display:none;"> 
                        <table border="1" style="font-size:10pt;margin:1px;border:1px;Padding:1px;width:180px;">
                            <tr>
                                <td align="center">
                                    <b>兩造互動關係</b>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="radio" name="rel" id="rel1" value="0" onclick="setLineType(this.value);" />和諧
                                    <input type="radio" name="rel" id="rel2" value="1" onclick="setLineType(this.value);" />親密
                                    <input type="radio" name="rel" id="rel3" value="2" onclick="setLineType(this.value);" />非常親密
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="radio" name="rel" id="rel4" value="3" onclick="setLineType(this.value);" />疏離
                                    <input type="radio" name="rel" id="rel5" value="4" onclick="setLineType(this.value);" />不和
                                    <input type="radio" name="rel" id="rel6" value="5" onclick="setLineType(this.value);" />仇恨
                                    <br />
                                    <input type="radio" name="rel" id="rel7" value="6" onclick="setLineType(this.value);" />互不來往
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="radio" name="rel" id="rel8" value="7" onclick="setLineType(this.value);" />衝突
                                    <input type="radio" name="rel" id="rel9" value="8" onclick="setLineType(this.value);" />又愛又恨
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="radio" name="rel" id="rel10" value="9" onclick="setLineType(this.value);" />暴力 
                                    <input type="radio" name="rel" id="rel11" value="10" onclick="setLineType(this.value);" />親密又暴力
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="radio" name="rel" id="rel12" value="11" onclick="setLineType(this.value);" />愛慕 
                                    <input type="radio" name="rel" id="rel13" value="12" onclick="setLineType(this.value);" />戀愛
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="radio" name="rel" id="rel14" value="13" onclick="setLineType(this.value);" />性侵害
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <b>生態圈間依屬關係</b>    
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="radio" name="rel" id="rel90" value="90" onclick="setLineType(this.value);" />輕微
                                    <input type="radio" name="rel" id="rel91" value="91" onclick="setLineType(this.value);" />普通
                                    <input type="radio" name="rel" id="rel92" value="92" onclick="setLineType(this.value);" />強烈
                                    <br />
                                    <input type="radio" name="rel" id="rel93" value="93" onclick="setLineType(this.value);" />排斥
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <b>關係線備註</b>    
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="textfield" rows="4" style="width:170px;" name="reltxt" id="reltext" onkeyup="setLMemo(this.value);" />
                                </td>
                            </tr>
                        </table>
                    </div>
            
                    <div id="GenoMenu5" style="display: none;">  
                        <table border="1" style="font-size:10pt;margin:1px;border:1px;Padding:1px;width:180px;">
                            <tr>
                                <td align="center">
                                    <b>生態圖標</b>    
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <textarea cols="10" rows="3"   style="width:170px;overflow:hidden" name="ecoM" id="ecoMemo" onkeyup="setEMemo(this.value);"></textarea>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <input type="button" name="allClear" class="button1 button1_delete" value="清除重畫" onclick="clearPanel();" />
                    <input type="button" name="reOrg" class="button1 button1_setup" value="圖面重整" onclick="alignPanel();" />
                    <input type="button" name="save" class="button1 button1_add" value="另存圖檔" onclick="writeImage();" />
                    <asp:Button runat="server" ID="btn_SaveXML" Text="存檔" CssClass="button1 button1_save" OnClientClick="saveXML();" onclick="btn_SaveXML_Click" />
                    <input type="button" name="exit" class="button1 button1_exit" value="離開" onclick="window.close();" />
                </td>
                <td align="right" valign="bottom">
                    <span style="cursor: hand; font-size:smaller;" onclick="alert('本家系圖及生態圖繪製軟體－JGeno為采聲科技(股)公司所提供，該電腦程式著作受著作權法及國際公約之保護。\n本次授權為提供關懷ｅ起來專案之範圍使用，未經授權擅自複製本電腦程式著作之全部或部份，都不是法令所允許的行為。\n如您希望在其他應用系統也可以享受到本家系圖及生態圖繪製軟體的便利，您可洽采聲科技（股）公司購買使用，連絡電話為(02)2531-1938');return false;">版權說明 </span>
                </td>
            </tr>
        </table>
    </form>
    <script type="text/javascript">
        //頁面Load完成後，把預設的圖檔載入
        JGeno.setXML(document.getElementById('HFD_XML').value);
    </script>
    <script language="JavaScript" type="text/javascript">
	    var currentPersonObjectId = 0;
	    var currentLineObjectId = 0;
	    var currentEcoObjectId = 0;
	    var currentFloatTexttId = 0;
	    function nothing()
	    {
		    ClearMenu();
		    document.genoPanel.ecob.disabled = 0;
		    currentPersonObjectId = 0;
		    currentLineObjectId = 0;
		    currentEcoObjectId = 0;
		    currentFloatTexttId = 0;
	    }

	    function addGuy()
	    {
		    document.JGeno.AddPerson(1);		
		    document.JGeno.selectNone();
		    nothing();
	    }

	    function addGirl()
	    {
		    document.JGeno.AddPerson(2);		
		    document.JGeno.selectNone();		
		    nothing();
	    }

	    function addSpouse()
	    {
		    document.JGeno.AddSpouse(0);
		}
	    
	    function addChild()
	    {
		    if (currentPersonObjectId != 0) document.JGeno.AddChild(0,1);
		    else document.JGeno.AddChild(0,2);
		}
	    
	    function addSibling()
	    {
		    document.JGeno.AddSibling(0);
		}
	    
	    function addTwins()
	    {
		    document.JGeno.AddTwins(0);
		}
	    
	    function addParents()
	    {
		    document.JGeno.AddParents(0);
	    }

	    function addRelationship()
	    {
		    document.JGeno.selectNone();
		    document.JGeno.AddRelationship(0);
	    }

	    function addEco(id)
	    {
		    document.JGeno.selectNone();
		    document.JGeno.AddEco(id);
	    }

	    function writeImage()
	    {
		    document.JGeno.writeImage();
	    }

	    function importXML()
	    {
		    document.JGeno.importXML();
	    }

	    function exportXML()
	    {
		    document.JGeno.exportXML();
	    }

	    function switchToGirl()
	    {
		    document.JGeno.setCurrentGenderMode(2);	
		    document.getElementById('jrbp1').disabled = 0;
		    document.getElementById('jrbp2').disabled = 0;
		    document.getElementById('jrbp3').disabled = 0;
		    document.getElementById('jrbp4').disabled = 0;			
	    }

	    function switchToUnknown()
	    {
		    document.JGeno.setCurrentGenderMode(0);	
		    document.getElementById('jrbp1').disabled = 0;
		    document.getElementById('jrbp2').disabled = 0;
		    document.getElementById('jrbp3').disabled = 0;
		    document.getElementById('jrbp4').disabled = 0;		
	    }

	    function ClearMenu()
	    {
		    GenoMenu1.style.display = 'none';
		    GenoMenu2.style.display = 'none';
		    GenoMenu3.style.display = 'none';
		    GenoMenu4.style.display = 'none';
		    GenoMenu5.style.display = 'none';
	    }

	    function setCurrentSex(value)
	    {
		    document.JGeno.setSex(currentPersonObjectId, value);
	    }

	    function setCurrentAlive(checked)
	    {
		    if ( checked == true)
		    {
			    document.JGeno.setIsAlive(currentPersonObjectId, false);
		    }
		    else
		    {
			    document.JGeno.setIsAlive(currentPersonObjectId, true);
		    }
	    }

	    function setCurrentMaster(checked)
	    {
		    if ( checked == true)
		    {
			    document.JGeno.setIsMaster(currentPersonObjectId, true);
		    }
		    else
		    {
			    document.JGeno.setIsMaster(currentPersonObjectId, false);
		    }
	    }

	    function setCurrentAge(value)
	    {
		    document.JGeno.setAge(currentPersonObjectId, value);			
	    }

	    function setCurrentPregnancy(value)
	    {
		    document.JGeno.setPregnancy(currentPersonObjectId, value);
		    if (value ==2 || value==3) 
		    {
			    document.getElementById('jcb1').checked = 1;
			    document.getElementById('jcb1').disabled = 1;
		    }
		    else
		    {								
			    document.getElementById('jcb1').checked = 0;
			    document.getElementById('jcb1').disabled = 0;
		    }
	    }

	    function setIsAdopted(value)
	    {
		    if ( value == 0) document.JGeno.setIsAdopted(currentPersonObjectId, false);
		    else document.JGeno.setIsAdopted(currentPersonObjectId, true);
		    document.getElementById('jcb1').disabled = 0;
	    }

	    function setCurrentMemo(value)
	    {
		    document.JGeno.setPMemo(currentPersonObjectId, value);
	    }

	    function showPersonDetails(id)
	    {	
		    currentPersonObjectId = id;
		    if ( id != 0)
		    {
			    ClearMenu();
			    GenoMenu1.style.display ='block';

			    document.genoPanel.jtf1.value = document.JGeno.getAge(id);
			    document.getElementById('jcb1').checked = !document.JGeno.getIsAlive(id);
			    document.getElementById('jcb2').checked = document.JGeno.getIsMaster(id);
    						
			    if (document.JGeno.getIsAdopted(id) == true) document.getElementById('jrbp4').checked = 1;
			    else if (document.JGeno.getPregnancy(id) == 1) document.getElementById('jrbp1').checked = 1;
			    else if (document.JGeno.getPregnancy(id) == 2) document.getElementById('jrbp2').checked = 1;
			    else if (document.JGeno.getPregnancy(id) == 3) document.getElementById('jrbp3').checked = 1;			
                else if (document.JGeno.getIsAdopted(id) == false) document.getElementById('jrbp0').checked = 1;

			    if (document.JGeno.getPregnancy(id) >= 2) document.getElementById('jcb1').disabled = 1;
			    else document.getElementById('jcb1').disabled = 0;					

			    document.genoPanel.jta1.value = document.JGeno.getPMemo(id);

			    if (document.JGeno.getSex(id) == 0) document.getElementById('jrbg3').checked = 1;
			    else if (document.JGeno.getSex(id) == 1)  document.getElementById('jrbg1').checked = 1;
			    else if (document.JGeno.getSex(id) == 2) document.getElementById('jrbg2').checked = 1;
    		
			    document.genoPanel.interactb.disabled = 0;
			    document.genoPanel.ecob.disabled = 0;
		    }
		    else
		    {
			    nothing();
		    }
		}
	    
	    function setLineType(value)
	    {
		    document.JGeno.setLineType(currentLineObjectId, value);
		}
	    
	    function setLMemo(value)
	    {
		    document.JGeno.setLMemo(currentLineObjectId, value);
	    }

	    function addFloatText()
	    {
	        document.JGeno.addFloatText();
		}
	    
      function saveXML()
      {
  	    JGeno.setXML(JGeno.getXML());
  	    document.getElementById('HFD_XML').value = JGeno.getXML();
      }

	    function showFloatTextDetails(ftid)
	    {
		    currentFloatTexttId = ftid;
		    if(ftid <= 0) 
		    {
		    ClearMenu();
		    }
		    else
		    {
                document.genoPanel.floatText.value = document.JGeno.getFTMemo(currentFloatTexttId);
		    ClearMenu();
		    GenoMenu3.style.display ='block';
		    }

	    }

	    function clearPanel()
	    {
		    document.JGeno.clearPanel();
	    }

	    function alignPanel()
	    {
		    document.JGeno.alignPanel();
	    }

	    function showLineDetails(lineId)
	    {	
			    currentLineObjectId = lineId;
			    if(document.JGeno.getLineCategory(lineId) == 0)
			    {
				    document.genoPanel.interactb.disabled = 0;
				    document.genoPanel.ecob.disabled = 1;
				    ClearMenu();
				    GenoMenu2.style.display ='block';
				     b = document.JGeno.getLineType(lineId);
				    if(b == 0) document.getElementById('marriage1').checked = 1;
				    else if(b == 1) document.getElementById('marriage2').checked = 1;
				    else if(b == 2) document.getElementById('marriage3').checked = 1;
				    else if(b == 3) document.getElementById('marriage4').checked = 1;
				    else if(b == 4) document.getElementById('marriage5').checked = 1;
			    }
			    else if(document.JGeno.getLineCategory(lineId) == 2)
			    {
				    b = document.JGeno.getLineType(lineId);
				    if(b == 0) document.getElementById('rel1').checked = 1;
				    else if(b == 1) document.getElementById('rel2').checked = 1;
				    else if(b == 2) document.getElementById('rel3').checked = 1;
				    else if(b == 3) document.getElementById('rel4').checked = 1;
				    else if(b == 4) document.getElementById('rel5').checked = 1;
				    else if(b == 5) document.getElementById('rel6').checked = 1;
				    else if(b == 6) document.getElementById('rel7').checked = 1;
				    else if(b == 7) document.getElementById('rel8').checked = 1;
				    else if(b == 8) document.getElementById('rel9').checked = 1;
				    else if(b == 9) document.getElementById('rel10').checked = 1;
				    else if(b == 10) document.getElementById('rel11').checked = 1;
				    else if(b == 11) document.getElementById('rel12').checked = 1;
				    else if(b == 12) document.getElementById('rel13').checked = 1;
				    else if(b == 13) document.getElementById('rel14').checked = 1;
				    else if(b == 90) document.getElementById('rel90').checked = 1;
				    else if(b == 91) document.getElementById('rel91').checked = 1;
				    else if(b == 92) document.getElementById('rel92').checked = 1;
				    else if(b == 93) document.getElementById('rel93').checked = 1;

				    document.genoPanel.reltext.value = document.JGeno.getLMemo(lineId);
				    document.genoPanel.interactb.disabled = 0;
				    document.genoPanel.ecob.disabled = 1;
				    ClearMenu();
				    GenoMenu4.style.display ='block';
			    }
	    }

	    function showEcoDetails(ecoId)
	    {
		    currentEcoObjectId = ecoId;
            b = document.JGeno.getEType(ecoId);
		    if(b == 0)
		    {
			    ClearMenu();
			    GenoMenu5.style.display ='block';
			    document.genoPanel.ecoM.value = document.JGeno.getEMemo(currentEcoObjectId);
		    }
		    else
		    {
			    ClearMenu();
		    }
	    }
    	
	    function setEMemo(memo)
	    {
		    document.JGeno.setEMemo(currentEcoObjectId, memo);
	    }
    	
	    function setFTMemo(memo)
	    {
		    document.JGeno.setFTMemo(currentFloatTexttId, memo);
	    }

	    function setSize()
	    {
		    if ((document.genoPanel.widthtf.value >= 0) && (document.genoPanel.heighttf.value >=0))
			    document.JGeno.setPanelSize(document.genoPanel.widthtf.value,document.genoPanel.heighttf.value);
	    }
    </script>
</body>
</html>
