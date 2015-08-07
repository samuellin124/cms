<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Geno.aspx.cs" Inherits="Common_Geno" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=Big5" />
    <title>�ϧνs��u��</title>
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
                    <input name="button" type="button" style="width:44px" onclick="addGuy();" value="�k��" />
                    <input name="button2" type="button" style="width:44px" onclick="addGirl();" value="�k��" />
                    <input type="button" name="interactb" style="width:90px" value="�������Y" onclick="addRelationship()" />
                    <br />
                    <input type="button" name="ecob" style="width:92px" value="�~�����Y" onclick="addEco(1)" />
                    <input type="button" name="ecob" style="width:90px" value="�ͺA��" onclick="addEco(0)" />
                    <br />
                    <input type="button" name="memo" style="width:186px;" value="�`�Ѥ�r" onclick="addFloatText();" />
                    
                    <div id="GenoMenu1" style="display:none;">
                        <table border="1" style="font-size:10pt;margin:1px;border:1px;Padding:1px;width:180px;">
        	                 <tr>
        	                     <td align="center">
        	                         <b>�����ݩʳ]�w</b>    
        	                     </td>
        	                 </tr>
        	                 <tr>
        	                     <td>
        	                         �ʧO�G<input type="radio" name="jrbg" id="jrbg1" value="1" onclick="setCurrentSex(this.value);" /> �k<input type="radio" name="jrbg" id="jrbg2" value="2" onclick="setCurrentSex(this.value);" /> �k <input type="radio" name="jrbg" id="jrbg3" value="0" onclick="setCurrentSex(this.value);" /> ���� <br />
        	                         �~�֡G <input type="text" name="jtf1" style="margin:1px;border-width:1px;width:66px" maxlength="3" onkeyup="setCurrentAge(this.value);" /> ��<br />              
        	                         ���A�G<input type="checkbox" name="jcb2" id="jcb2" onclick="setCurrentMaster(this.checked);" /> �ץD <input type="checkbox" name="jcb1" id="jcb1" onclick="setCurrentAlive(this.checked);" /> ���`
        	                         <br />
        	                         �;i�G<input type="radio" name="jrbp" id="jrbp0" value="0" onclick="setIsAdopted(0);" checked="checked" /> �˥� <input type="radio" name="jrbp" id="jrbp4" value="4" onclick="setIsAdopted(1);" /> ���i
        	                         <br />
        	                     �@�@�@<input type="radio" name="jrbp" id="jrbp1" value="1" onclick="setCurrentPregnancy(this.value);" /> �L�� <input type="radio" name="jrbp" id="jrbp2" value="2" onclick="setCurrentPregnancy(this.value);" /> �Z�L
        	                         <br />
        	                     �@�@�@<input type="radio" name="jrbp" id="jrbp3" value="3" onclick="setCurrentPregnancy(this.value);" /> �y��
        	                         <br />
        	                         �Ƶ��G
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
                                    <b>�t�����Y����</b>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="radio" name="marriage" id="marriage1" value="0" onclick="setLineType(this.value);" /> ���B
                                    <br />
                                    <input type="radio" name="marriage" id="marriage3" value="2" onclick="setLineType(this.value);" /> ���B
                                    <br />
                                    <input type="radio" name="marriage" id="marriage4" value="3" onclick="setLineType(this.value);" /> �P�~
                                    <br />
                                    <input type="radio" name="marriage" id="marriage2" value="1" onclick="setLineType(this.value);" /> ���~
                                    <br />
                                    <input type="radio" name="marriage" id="marriage5" value="4" onclick="setLineType(this.value);" /> ���g�P�~
                                </td>
                            </tr>
                        </table>
                    </div>	
            	
                    <div id="GenoMenu3" style="display:none;">
	                    <table border="1" style="font-size:10pt;margin:1px;border:1px;Padding:1px;width:180px;">
	                        <tr>
	                            <td align="center">
	                                <b>�`�Ѥ�r</b>    
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
                                    <b>��y�������Y</b>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="radio" name="rel" id="rel1" value="0" onclick="setLineType(this.value);" />�M��
                                    <input type="radio" name="rel" id="rel2" value="1" onclick="setLineType(this.value);" />�˱K
                                    <input type="radio" name="rel" id="rel3" value="2" onclick="setLineType(this.value);" />�D�`�˱K
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="radio" name="rel" id="rel4" value="3" onclick="setLineType(this.value);" />����
                                    <input type="radio" name="rel" id="rel5" value="4" onclick="setLineType(this.value);" />���M
                                    <input type="radio" name="rel" id="rel6" value="5" onclick="setLineType(this.value);" />����
                                    <br />
                                    <input type="radio" name="rel" id="rel7" value="6" onclick="setLineType(this.value);" />�����ө�
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="radio" name="rel" id="rel8" value="7" onclick="setLineType(this.value);" />�Ĭ�
                                    <input type="radio" name="rel" id="rel9" value="8" onclick="setLineType(this.value);" />�S�R�S��
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="radio" name="rel" id="rel10" value="9" onclick="setLineType(this.value);" />�ɤO 
                                    <input type="radio" name="rel" id="rel11" value="10" onclick="setLineType(this.value);" />�˱K�S�ɤO
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="radio" name="rel" id="rel12" value="11" onclick="setLineType(this.value);" />�R�} 
                                    <input type="radio" name="rel" id="rel13" value="12" onclick="setLineType(this.value);" />�ʷR
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="radio" name="rel" id="rel14" value="13" onclick="setLineType(this.value);" />�ʫI�`
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <b>�ͺA�鶡�������Y</b>    
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="radio" name="rel" id="rel90" value="90" onclick="setLineType(this.value);" />���L
                                    <input type="radio" name="rel" id="rel91" value="91" onclick="setLineType(this.value);" />���q
                                    <input type="radio" name="rel" id="rel92" value="92" onclick="setLineType(this.value);" />�j�P
                                    <br />
                                    <input type="radio" name="rel" id="rel93" value="93" onclick="setLineType(this.value);" />�ƥ�
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <b>���Y�u�Ƶ�</b>    
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
                                    <b>�ͺA�ϼ�</b>    
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
                    <input type="button" name="allClear" class="button1 button1_delete" value="�M�����e" onclick="clearPanel();" />
                    <input type="button" name="reOrg" class="button1 button1_setup" value="�ϭ�����" onclick="alignPanel();" />
                    <input type="button" name="save" class="button1 button1_add" value="�t�s����" onclick="writeImage();" />
                    <asp:Button runat="server" ID="btn_SaveXML" Text="�s��" CssClass="button1 button1_save" OnClientClick="saveXML();" onclick="btn_SaveXML_Click" />
                    <input type="button" name="exit" class="button1 button1_exit" value="���}" onclick="window.close();" />
                </td>
                <td align="right" valign="bottom">
                    <span style="cursor: hand; font-size:smaller;" onclick="alert('���a�t�ϤΥͺA��ø�s�n���JGeno�����n���(��)���q�Ҵ��ѡA�ӹq���{���ۧ@���ۧ@�v�k�ΰ�ڤ������O�@�C\n�������v���������h��_�ӱM�פ��d��ϥΡA���g���v�զ۽ƻs���q���{���ۧ@�������γ����A�����O�k�O�Ҥ��\���欰�C\n�p�z�Ʊ�b��L���Ψt�Τ]�i�H�ɨ��쥻�a�t�ϤΥͺA��ø�s�n�骺�K�Q�A�z�i�����n��ޡ]�ѡ^���q�ʶR�ϥΡA�s���q�ܬ�(02)2531-1938');return false;">���v���� </span>
                </td>
            </tr>
        </table>
    </form>
    <script type="text/javascript">
        //����Load������A��w�]�����ɸ��J
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
