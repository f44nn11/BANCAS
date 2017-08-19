
//-------------------------------------------------------------------
var X = XLSX;
//var policyno = 	$('#select_pol').val();
var XW = {
	/* worker message */
	msg: 'xlsx',
	/* worker scripts */
	rABS: 'scripts/excel/xlsxworker2.js',
	norABS: 'scripts/excel/xlsxworker1.js',
	noxfer: 'scripts/excel/xlsxworker.js'
};
var rABS = typeof FileReader !== "undefined" && typeof FileReader.prototype !== "undefined" && typeof FileReader.prototype.readAsBinaryString !== "undefined";
if(!rABS) {
	//document.getElementsByName("userabs")[0].disabled = true;
	//document.getElementsByName("userabs")[0].checked = false;
}

var use_worker = typeof Worker !== 'undefined';
if(!use_worker) {
	//document.getElementsByName("useworker")[0].disabled = true;
	//document.getElementsByName("useworker")[0].checked = false;
}

var transferable = use_worker;
if(!transferable) {
	//document.getElementsByName("xferable")[0].disabled = true;
	//document.getElementsByName("xferable")[0].checked = false;
}

var wtf_mode = false;

function fixdata(data) {
	var o = "", l = 0, w = 10240;
	for(; l<data.byteLength/w; ++l) o+=String.fromCharCode.apply(null,new Uint8Array(data.slice(l*w,l*w+w)));
	o+=String.fromCharCode.apply(null, new Uint8Array(data.slice(l*w)));
	return o;
}

function ab2str(data) {
	var o = "", l = 0, w = 10240;
	for(; l<data.byteLength/w; ++l) o+=String.fromCharCode.apply(null,new Uint16Array(data.slice(l*w,l*w+w)));
	o+=String.fromCharCode.apply(null, new Uint16Array(data.slice(l*w)));
	return o;
}

function s2ab(s) {
	var b = new ArrayBuffer(s.length*2), v = new Uint16Array(b);
	for (var i=0; i != s.length; ++i) v[i] = s.charCodeAt(i);
	return [v, b];
}

function xw_noxfer(data, cb) {
	var worker = new Worker(XW.noxfer);
	worker.onmessage = function(e) {
		switch(e.data.t) {
			case 'ready': break;
			case 'e': console.error(e.data.d); break;
			case XW.msg: cb(JSON.parse(e.data.d)); break;
		}
	};
	var arr = rABS ? data : btoa(fixdata(data));
	worker.postMessage({d:arr,b:rABS});
}

function xw_xfer(data, cb) {
	var worker = new Worker(rABS ? XW.rABS : XW.norABS);
	worker.onmessage = function(e) {
		switch(e.data.t) {
			case 'ready': break;
			case 'e': console.error(e.data.d); break;
			default: var xx=ab2str(e.data).replace(/\n/g,"\\n").replace(/\r/g,"\\r"); console.log("done"); cb(JSON.parse(xx)); break;
		}
	};
	if(rABS) {
		var val = s2ab(data);
		worker.postMessage(val[1], [val[1]]);
	} else {
		worker.postMessage(data, [data]);
	}
}

function xw(data, cb) {
    xw_xfer(data, cb);
	//transferable = document.getElementsByName("xferable")[0].checked;
	//if(transferable) xw_xfer(data, cb);
	//else xw_noxfer(data, cb);
}
	
function to_json(workbook) {
	var result = {};
	var sheetName="";
	var validate = true;
	var typesub ="";
	workbook.SheetNames.forEach(function(sheetName,index) {
		var roa = X.utils.sheet_to_json(workbook.Sheets[sheetName]); //,{range:1}
		
		if(roa.length > 0){
			result[sheetName] = roa;
		}else{
			return false;
		}
	});
	
	return result;
}

var global_wb;
var output = "";
var memId = "";
function process_wb(wb) {
    global_wb = wb;
    output = JSON.stringify(to_json(wb), 2, 2);
    //$('#divoutput').text(output);
    //console.log(outPut);
    memId = output;
}


//function process_wb(wb) {
//	global_wb = wb;
	
//	output = JSON.stringify(to_json(wb), 2, 2);
//	console.log(output);
//	//to_html(wb);
//	var policyno = 	$('#select_pol').val();
//	if (output.length > 2 ){
//		var dataPost = "";
//		var typesubmission = $("#select_type").val();
//		var typesubmissionNm=$("#select_type option:selected").text();
//		var namefile = $('#filenm').val().replace(/.*(\/|\\)/, '');	
//		var jsonAll = $.parseJSON(output);
//		var codes = "valid";
//		var error = "";
//		var err = "";
//		var error1,error2,error3,error4,error5,error6,error7,error8,error9="";
//		var str3;
//		var product = policyno.substring(0, 3);
//		var recno;
		
//		if (typesubmission === "1"){ //ADDITION
//			var add = jsonAll["PolicyMember"];
//			var addition = jsonAll["PolicyMember"][2]["data"];
			
//			if (addition.length > 0){
//				var version = add[0]["version"]["E"];
//				var process = add[1]["processtype"][0]["B"];
//				var type = add[1]["processtype"][1]["B"];
//				dataPost = {"actiontype":"version","typesubmission":typesubmission,"fields":"","value":""};
//				var response = validateDatatemp(JSON.stringify(dataPost));
//				var versiondb = response["info"]["version"];
				
//				if (version != versiondb ){
//					console.log("Template Excel old version");
//					$('#button').hide();
//					$('#export').hide();
//					$('#loading-img').hide();
//					$("#output").html("Template "+ namefile +" old version");
//					$("#data").val("");
//					$("#data_support").val("");
//					return false;
//				}
//				$(addition).each(function(i,val){
//					err = "";
//					$.each(val,function(sheets,cells){
//						//var jsondata = $.parseJSON(sheets);
//						recno = i+1;
//						var dataCell = "";
//						var status;

//						//if(sheets == "B"){
//							//console.log(sheets + ":" + cells);
//							/*dataCell = {"actiontype":"check","typesubmission":typesubmission,"fields":sheets,"value":cells};
//							var response = validateDatatemp(JSON.stringify(dataCell));
//							status = response["info"][0]["status"];
//							descp = response["info"][0]["descp"];
//							if (status == "2"){
//								codes = "invalid";
//								error1 =  ";sub group cant empty";
//								err = "err";
//							}else{
//								codes;
//								err;
//								error1 = "";
//							}*/
//							//codes;
//							//err;
//							//error1 = "";
//						//}
//						if(sheets == "C"){
//							if (cells == ""){
//								codes = "invalid";
//								error1 = ";fullname " + errCode(1001);
//								err = "err";
//								//console.log(error);
//							}else{
//								codes;
//								err;
//								error1 = "";
//							}
//						}
//						if(sheets == "D"){
//							if (cells == "M" || cells == "F" ) {
//								codes;
//								err;
//								error2 = "";
//							}else{
//								//console.log(sheets + ":" + cells);
//								codes = "invalid";
//								error2 	= ";sex " + errCode(1003);
//								err = "err";
//							}

//						}
//						if(sheets == "E"){
//							var date = new Date(cells);
//							/*var year = d.getFullYear();
//							var month =d.getMonth() + 1;
//							var day = d.getDate();
//							var result = month + "/" + day + "/" + year;
//							console.log(result);*/
//							if (isNaN(date) != false){
//								codes = "invalid";
//								error3 = ";dob " + errCode(1002);
//								err = "err";
//							}else{
//								codes;
//								err;
//								error3 = "";
//							}
//						}
//						if(sheets == "Y"){
//							var date = new Date(cells);
//							/*var year = d.getFullYear();
//							var month =d.getMonth() + 1;
//							var day = d.getDate();
//							var result = month + "/" + day + "/" + year;
//							console.log(result);*/
//							if (isNaN(date) != false){
//								codes = "invalid";
//								error4 = ";eff.dt " + errCode(1002);
//								err = "err";
//							}else{
//								codes;
//								err;
//								error4 = "";
//							}
//						}
//						if(sheets == "Z"){
//							var date = new Date(cells);
//							/*var year = d.getFullYear();
//							var month =d.getMonth() + 1;
//							var day = d.getDate();
//							var result = month + "/" + day + "/" + year;
//							console.log(result);*/
//							if (product == "001" || product == "002" || product == "003" || product == "004"){
//								if (isNaN(date) != false){
//									codes = "invalid";
//									error5 = ";exp.dt " + errCode(1002);
//									err = "err";
//								}else{
//									codes;
//									err;
//									error5 = "";
//								}
//							}else{
//								codes;
//								err;
//								error5 = "";
//							}
//						}
//						if(sheets == "AK"){
//							var regex  = /\B(?=(\d{3})+(?!\d))/g;
//							cells = parseNumber(cells);
//							if (regex.test(cells)){
//								codes;
//								err;
//								error6 = "";
//							}else{
//								codes = "invalid";
//								error6 = ";up " + errCode(1004);
//								err = "err";
//							}
//						}

//						if (codes == "invalid"){
//							error = error1 + error2 + error3 + error4 + error5 + error6;
//							error = error.indexOf(';') == 0 ? error.substring(1) : error;
//							err = err;
//							str3 = {"error" : error};
//							str4 = {"flagerr":err};
//						}else{
//							error="";
//							err = "";
//							str3 = {"error" : error};
//							str4 = {"flagerr":err};
//						}

//					});
//					str1 = val;
//					str2 = {"policyno":policyno};
//					str5 = {"tipe submission":typesubmissionNm};
//					str6 = {"actiontype":"insert"};
//					//str7 = {"recno":recno};

//					result = $.extend(true,str1,str2,str3,str4,str5,str6);
//				});
//				var datajson = JSON.stringify(addition)
//				var errdata = find_in_object(JSON.parse(datajson), {flagerr:'err'});
//				errdata = removeJsonAttrs(errdata,["F","G","H","I"]);
//				errdata = removeJsonAttrs(errdata,["M","N","O","P","Q","R","S"]);
//				errdata = removeJsonAttrs(errdata,["X"]);
//				errdata = removeJsonAttrs(errdata,["AB"]);
//				errdata = removeJsonAttrs(errdata,["AD","AE","AF","AG","AH","AI","AJ"]);
//				errdata = removeJsonAttrs(errdata,["AL","AM","AN","AO","AP","AQ","AR","AS","AT","AU","AV","AW","AX","AY"]);
//				errdata = removeJsonAttrs(errdata,["undefined"]);
//				errdata = removeJsonAttrs(errdata,["flagerr"]);
//				errdata = removeJsonAttrs(errdata,["actiontype"]);
//				//console.log(JSON.stringify(errdata));
//				//console.log(JSON.stringify(addition));
//				var str="";
//				var arrHeaderAddition = [{
//											"SeqNo": "",
//											"Member Id": "",
//											"Full Name": "",
//											"Sex": "",
//											"DOB (mm/dd/yyyy)": "",
//											"Sub Group": "",
//											"Loan No": "",
//											"Id No": "",
//											"Bank Code": "",
//											"Account No": "",
//											"Bank Branch": "",
//											"Account Name": "",
//											"Eff. DT (mm/dd/yyyy)": "",
//											"Exp. DT  (mm/dd/yyyy)": "",
//											"NOHP [USERFIELD5]": "",
//											"EMAIL [USERFIELD7]": "",
//											"UP [USERFIELD15]": "",
//											"TGR": "",										
//											"policyno": "",
//											"Error": "",
//											"tipe submission": "",
//										}];
//				if (errdata.length > 0){
//					str = "with "+ errdata.length + " error";
//					$('#select_type').val("Choose type of submission");
//					$("#data").val("");
//					$("#data_support").val("");
//					$('#button').hide();
//					$('#export').show();
//					var $btnDLtoExcel = $('#export');
//					$btnDLtoExcel.on('click', function () {
						
//						var currentDate = new Date()
//						var day = currentDate.getDate()
//						var month = currentDate.getMonth() + 1
//						var year = currentDate.getFullYear()
//						var hour = currentDate.getHours();
//						var minutes = currentDate.getMinutes();
//						var secconds = currentDate.getSeconds()
//						var tgl = month + "" + day + "" + year +"_"+hour+""+minutes+""+secconds;

//						JSONToCSVConvertor(errdata, tgl, true,arrHeaderAddition,namefile);

//		//				$("#dvjson").excelexportjs({
//		//							containerid: "dvjson"
//		//							   , datatype: 'json'
//		//							   , dataset: errdata
//		//							   , columns: getColumns(errdata) 
//		//							   //, columns : [
//		////								   				{ headertext: 'Recno', datafield: 'recno'},
//		////								   				{ headertext: 'Policyno', datafield: 'policyno', width: 130,cellsformat: 'd2'},
//		////											  	{ headertext: 'No Sertifikat AJK', datafield: 'A', width: 130},
//		////											  	{ headertext: 'SubGroup', datafield: 'B', width: 150 },
//		////											  	{ headertext: 'No Referensi', datafield: 'C', width: 130,align: 'left',cellsalign: 'right' },
//		////											  	{ headertext: 'No Pinjaman', datafield: 'D', width: 130, cellsalign: 'center', align: 'center' },
//		////											  	{ headertext: 'Nama Peserta', datafield: 'E'},
//		////											  	{ headertext: 'Jenis Kelamin', datafield: 'F', align: 'center', cellsalign: 'center' },
//		////											  	{ headertext: 'Tgl Lahir', datafield: 'G', cellsalign: 'center', align: 'center'},
//		////								   			  	{ headertext: 'TGL_EFEKTIF', datafield: 'H', cellsalign: 'center', align: 'center' },
//		////								   				{ headertext: 'TGL_EXPIRED', datafield: 'I', cellsalign: 'center', align: 'center' },
//		////								   				{ headertext: 'Uang Pertanggungan', datafield: 'J', width: 130, cellsalign: 'right', align: 'right'},
//		////								   				{ headertext: 'Keterangan', datafield: 'K'},
//		////								   				{ headertext: 'Error', datafield: 'error'},
//		////										  ]
//		//						});
//					});
//				}else{
//					str = "no error ";
//					$('#button').show();
//					$('#export').hide();
//					$('#button').on('click', function () {
//						//postDatatemp(JSON.stringify(addition)); //insert ke tempDB 		

//					});

//				}
//				//console.log(detectIE())

//				$('#loading-img').hide();
//				$("#output").html("finish validation " + str); 
//			}else{
//					$("#output").html("No record data in Sheet PolicyMember");
//					$("#data").val("");
//					$("#data_support").val("");
//					$('#button').hide();
//					$('#export').hide();
//					$('#loading-img').hide();
//					return false;
//			}	
			
//		} //end typesubmission ADDITION		
//		else if (typesubmission == "2"){ //CHANGE
//			var ch = jsonAll["Alteration"];
//			var changes = jsonAll["Alteration"][2]["data"];
			
//			if (changes.length > 0){
//				var version = ch[0]["version"]["E"];
//				var process = ch[1]["processtype"][0]["B"];

//				dataPost = {"actiontype":"version","typesubmission":typesubmission,"fields":"","value":""};
//				var response = validateDatatemp(JSON.stringify(dataPost));
//				var versiondb = response["info"]["version"];
					
//				if (version != versiondb ){
//					$('#button').hide();
//					$('#export').hide();
//					$('#loading-img').hide();
//					$("#output").html("Template "+ namefile +" old version");
//					$("#data").val("");
//					$("#data_support").val("");
//					return false;
//				}
//				$(changes).each(function(i,val){
//					err = "";
//					var cellsD = "";
//					$.each(val,function(sheets,cells){
//						recno = i+1;
//						if (sheets == "C"){
//							if (cells == ""){
//								codes = "invalid";
//								error1 = ";fullname " + errCode(1001);
//								err = "err";
//								//console.log(error);
//							}else{
//								codes;
//								err;
//								error1 = "";
//							}	
//						}
						
//						if (sheets == "D"){
//							if (cells == ""){
//								codes = "invalid";
//								error2 = ";newdata " + errCode(1001);
//								err = "err";
//							}else{
//								cellsD = cells;
//								codes;
//								err;
//								error2 = "";
//							}
//						}
//						if (sheets == "E"){
//							var regex  = /^(?:[1-9]\d*|\d)$/;
//							if (!regex.test(cells)){
//								codes = "invalid";
//								error3 = ";type " + errCode(1004);
//								err = "err";
//								//console.log(cells + " : " + error2);
//							}else{
//								codes;
//								err;
//								error3 = "";
//								if (cells == "1"){
//									if (cellsD == ""){
//										codes = "invalid";
//										error2 = ";newdata name " + errCode(1001);
//										err = "err";
//										console.log(error2);
//									}else{
//										codes;
//										err;
//										error2 = "";
//									}
//								}
//								if (cells == "2"){
//									if (cells == "M" || cells == "F"){
//										codes;
//										err;
//										error2 = "";
//									}else{
//										codes = "invalid";
//										error2 = ";newdata sex "  + errCode(1003);
//										err = "err";
//										console.log(error2);
//									}
//								}
//								if (cells == "3"){
//									var date = new Date(cells);
//									if (isNaN(date) != false){
//										codes = "invalid";
//										error2 = ";newdata dob " + errCode(1002);
//										err = "err";
//										console.log(error2);
//									}else{
//										codes;
//										err;
//										error2 = "";
										
//									}
//								}
//								if (cells == "5"){
//									if (cellsD == ""){
//										codes = "invalid";
//										error2 = ";newdata loanno " + errCode(1001);
//										err = "err";
//										console.log(cells);
//									}else{
//										codes;
//										err;
//										error2 = "";
//									}
//								}
//								if (cells == "6"){
//									if (cellsD == ""){
//										codes = "invalid";
//										error2 = ";newdata alternate card number " + errCode(1001);
//										err = "err";
//										console.log(error2);
//									}else{
//										codes;
//										err;
//										error2 = "";
//									}
//								}
//								if (cells == "7"){
//									if (cellsD == ""){
//										codes = "invalid";
//										error2 = ";newdata updateno hp " + errCode(1001);
//										err = "err";
//										console.log(error2);
//									}else{
//										codes;
//										err;
//										error2 = "";
//									}
//								}
//								if (cells == "9"){
//									var regex = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
//									if(regex.test(cellsD)){
//										codes = "invalid";
//										error2 = ";newdata email " + errCode(1004);
//										err = "err";
//										console.log(error2);
//									}else{
//										codes;
//										err;
//										error2 = "";
//									}
//								}
//								if (cells == "10"){
//									var regex = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
//									if (regex.test(cellsD)){
//										codes = "invalid";
//										error2 = ";newdata email " + errCode(1004);
//										err = "err";
//										console.log(error2);
//									}else{
//										codes;
//										err;
//										error2 = "";
//									}
//								}
//								if (cells == "11"){
//									var regex  = /\B(?=(\d{3})+(?!\d))/g;
//									cellsD = parseNumber(cellsD);
//									if (regex.test(cellsD)){
//										codes;
//										err;
//										error2 = "";
//									}else{
//										codes = "invalid";
//										error2 = ";newdata up " + errCode(1004);
//										err = "err";
//										console.log(error2);
										
//									}
//								}
//								if (cells == "15"){
//									if (cellsD == ""){
//										codes = "invalid";
//										error2 = ";newdata tenor " + errCode(1001);
//										err = "err";
//										console.log(error2);
//									}else{
//										codes;
//										err;
//										error2 = "";
//									}
//								}
//						   	}
//						}
//						if (sheets == "F"){
//							var date = new Date(cells);
//							//console.log(date);
//							if (isNaN(date) != false){
//								codes = "invalid";
//								error4 = ";changedate " + errCode(1002);
//								err = "err";
//							}else{
//								codes;
//								err;
//								error4 = "";
//							}
//						}
//						if (sheets == "G"){
//							if (cells == ""){
//								codes = "invalid";
//								error5 = ";certificate " + errCode(1001);
//								err = "err";
//							}else{
//								codes;
//								err;
//								error5 = "";
//							}
//						}
//						if (sheets == "H"){
//							var date = new Date(cells);
//							if (isNaN(date) != false){
//								codes = "invalid";
//								error6 = ";dob " + errCode(1002);
//								err = "err";
//							}else{
//								codes;
//								err;
//								error6 = "";
//							}
//						}
//						if (sheets == "I"){
//							var regex  = /\B(?=(\d{3})+(?!\d))/g;
//							cells = parseNumber(cells);
//							if (regex.test(cells)){
//								codes;
//								err;
//								error7 = "";
//							}else{
//								codes = "invalid";
//								error7 = ";up " + errCode(1004);
//								err = "err";

//							}
//						}
//						if (sheets == "J"){
//							if (cells == ""){
//								codes = "invalid";
//								error8 = ";remark " + errCode(1004);
//								err = "err";
//							}else{
//								codes;
//								err;
//								error8 = "";
//							}
//						}
						
//						if (codes == "invalid"){
//							error = error1 + error2 + error3 + error4 + error5 + error6 + error7 + error8;
//							error = error.indexOf(';') == 0 ? error.substring(1) : error;
//							err = err;
//							str3 = {"error" : error};
//							str4 = {"flagerr":err};
//						}else{
//							error="";
//							err = "";
//							str3 = {"error" : error};
//							str4 = {"flagerr":err};
//						}
						
//					});
//					str1 = val;
//					str2 = {"policyno":policyno};
//					str5 = {"tipe submission":typesubmissionNm};
//					str6 = {"actiontype":"insert"};
//					//str7 = {"recno":recno};

//					result = $.extend(true,str1,str2,str3,str4,str5,str6);
//				});
//				var datajson = JSON.stringify(changes)
//				var errdata = find_in_object(JSON.parse(datajson), {flagerr:'err'});
//				//errdata = removeJsonAttrs(errdata,["K",]);
//				errdata = removeJsonAttrs(errdata,["undefined"]);
//				errdata = removeJsonAttrs(errdata,["flagerr"]);
//				errdata = removeJsonAttrs(errdata,["actiontype"]);
//				console.log(JSON.stringify(errdata));

//				var arrHeaderChanges = [{
//											"SEQNO": "",
//											"MEMBERID": "",
//											"FULLNAME": "",
//											"NEW DATA": "",
//											"TYPE": "",
//											"CHANGE DATE": "",
//											"CERTIFICATE NO [Field1]": "",
//											"DOB [Field2]": "",
//											"UP [Field3]": "",
//											"REMARK [Field4]": "",
//											"FIELD5": "",
//											"FIELD6": "",
//											"policyno": "",
//											"Error": "",
//											"tipe submission": "",
//										}];
//				if (errdata.length > 0){
//					str = "with "+ errdata.length + " error";
//					$('#select_type').val("Choose type of submission");
//					$("#data").val("");
//					$("#data_support").val("");
//					$('#button').hide();
//					$('#export').show();
//					var $btnDLtoExcel = $('#export');
//					$btnDLtoExcel.on('click', function () {
//						var currentDate = new Date()
//						var day = currentDate.getDate()
//						var month = currentDate.getMonth() + 1
//						var year = currentDate.getFullYear()
//						var hour = currentDate.getHours();
//						var minutes = currentDate.getMinutes();
//						var secconds = currentDate.getSeconds();
//						var tgl = month + "" + day + "" + year +"_"+hour+""+minutes+""+secconds;

//						JSONToCSVConvertor(errdata, tgl, true,arrHeaderChanges,namefile);

//					});
//				}else{
//					str = "no error ";
//					$('#button').show();
//					$('#export').hide();
//					$('#button').on('click', function () {
//						//postDatatemp(JSON.stringify(changes)); //insert ke tempDB		

//					});

//				}

//				$('#loading-img').hide();
//				$("#output").html("finish validation " + str); 
//			}else{
//				$("#output").html("No record data in Sheet Alteration");
//				$("#data").val("");
//				$("#data_support").val("");
//				$('#button').hide();
//				$('#export').hide();
//				$('#loading-img').hide();
//				return false;
//			}	

//		} //end typesubmission CHANGES	
//		//else if (typesubmission == "TERMINATE" || typesubmission == "SURRENDER" || typesubmission == "CANCEL"){
//		else if (typesubmission == "3" || typesubmission == "4" || typesubmission == "5"){
//			var cts = jsonAll["ListMember"];
//			var cantermsurr = jsonAll["ListMember"][2]["data"];
			
//			if (cantermsurr.length > 0){
//				var version = cts[0]["version"]["F"];
//				var process = cts[1]["processtype"][0]["B"];
//				var type = cts[1]["processtype"][1]["B"];
//				dataPost = {"actiontype":"version","typesubmission":typesubmission,"fields":"","value":""};
//				var response = validateDatatemp(JSON.stringify(dataPost));
//				var versiondb = response["info"]["version"];
//				if (version != versiondb ){
//					$('#button').hide();
//					$('#export').hide();
//					$('#loading-img').hide();
//					$("#output").html("Template "+ namefile +" old version");
//					return false;
//				}
				
//				if (typesubmission == "3"){
//					if (type != "T"){
//						$("#output").html("wrong type in excelfile");
//						$("#data").val("");
//						$("#data_support").val("");
//						$('#button').hide();
//						$('#export').hide();
//						$('#loading-img').hide();
//						return false;
//					}
//				}
//				if (typesubmission == "4"){
//					if (type != "S"){
//						$("#output").html("wrong type in excelfile");
//						$("#data").val("");
//						$("#data_support").val("");
//						$('#button').hide();
//						$('#export').hide();
//						$('#loading-img').hide();
//						return false;
//					}
//				}
//				if (typesubmission == "5"){
//					if (type != "C"){
//						$("#output").html("wrong type in excelfile");
//						$("#data").val("");
//						$("#data_support").val("");
//						$('#button').hide();
//						$('#export').hide();
//						$('#loading-img').hide();
//						return false;
//					}
//				}
				
//				$(cantermsurr).each(function(i,val){
//					err = "";
//					$.each(val,function(sheets,cells){
//						recno = i+1;
//						if (sheets == "C"){
//							if (cells == ""){
//								codes = "invalid";
//								error1 = ";fullname " + errCode(1001);
//								err = "err";
//								//console.log(error);
//							}else{
//								codes;
//								err;
//								error1 = "";
//							}	
//						}
//						if (sheets == "D"){
//							var date = new Date(cells);
//							if (isNaN(date) != false){
//								codes = "invalid";
//								error2 = ";date " + errCode(1002);
//								err = "err";
//							}else{
//								codes;
//								err;
//								error2 = "";
//							}
//						}
//						if (sheets == "F"){
//							if (cells == ""){
//								codes = "invalid";
//								error3 = ";certificate " + errCode(1001);
//								err = "err";
//							}else{
//								codes;
//								err;
//								error3 = "";
//							}
//						}
//						if (sheets == "G"){
//							var date = new Date(cells);
//							if (isNaN(date) != false){
//								codes = "invalid";
//								error4 = ";date " + errCode(1002);
//								err = "err";
//							}else{
//								codes;
//								err;
//								error4 = "";
//							}
//						}
//						if (sheets == "H"){
//							var regex  = /\B(?=(\d{3})+(?!\d))/g;
//							cells = parseNumber(cells);
//							if (regex.test(cells)){
//								codes;
//								err;
//								error5 = "";
//							}else{
//								codes = "invalid";
//								error5 = ";up " + errCode(1004);
//								err = "err";

//							}
//						}
//						if (sheets == "I"){
//							if (cells == ""){
//								codes = "invalid";
//								error6 = ";remark " + errCode(1001);
//								err = "err";
//							}else{
//								codes;
//								err;
//								error6 = "";
//							}
//						}
//						if (codes == "invalid"){
//							error = error1 + error2 + error3 + error4 + error5 + error6;
//							error = error.indexOf(';') == 0 ? error.substring(1) : error;
//							err = err;
//							str3 = {"error" : error};
//							str4 = {"flagerr":err};
//						}else{
//							error="";
//							err = "";
//							str3 = {"error" : error};
//							str4 = {"flagerr":err};
//						}
					
//					});
//					str1 = val;
//					str2 = {"policyno":policyno};
//					str5 = {"tipe submission":typesubmissionNm};
//					str6 = {"actiontype":"insert"};
//					//str7 = {"recno":recno};

//					result = $.extend(true,str1,str2,str3,str4,str5,str6);
//				});
//				var datajson = JSON.stringify(cantermsurr)
//				var errdata = find_in_object(JSON.parse(datajson), {flagerr:'err'});
//				errdata = removeJsonAttrs(errdata,["E","J"]);
//				errdata = removeJsonAttrs(errdata,["undefined"]);
//				errdata = removeJsonAttrs(errdata,["flagerr"]);
//				errdata = removeJsonAttrs(errdata,["actiontype"]);
//				console.log(JSON.stringify(errdata));

//				var arrHeaderCantermsurr = [{
//											"SEQNO": "",
//											"MEMBER ID": "",
//											"NAMA": "",
//											"TRANSACTION DATE": "",
//											"CERTIFICATE NO [Field1]": "",
//											"DOB [Field2]": "",
//											"UP [Field3]": "",
//											"REMARK [Field4]": "",
//											"TGR": "",
//											"policyno": "",
//											"Error": "",
//											"tipe submission": "",
//										}];
//				if (errdata.length > 0){
//					str = "with "+ errdata.length + " error";
//					$('#select_type').val("Choose type of submission");
//					$("#data").val("");
//					$("#data_support").val("");
//					$('#button').hide();
//					$('#export').show();
//					var $btnDLtoExcel = $('#export');
//					$btnDLtoExcel.on('click', function () {
//						var currentDate = new Date()
//						var day = currentDate.getDate()
//						var month = currentDate.getMonth() + 1
//						var year = currentDate.getFullYear()
//						var hour = currentDate.getHours();
//						var minutes = currentDate.getMinutes();
//						var secconds = currentDate.getSeconds();
//						var tgl = month + "" + day + "" + year +"_"+hour+""+minutes+""+secconds;

//						JSONToCSVConvertor(errdata, tgl, true,arrHeaderCantermsurr,namefile);

//					});
//				}else{
//					str = "no error ";
//					$('#button').show();
//					$('#export').hide();
//					$('#button').on('click', function () {
//						//postDatatemp(JSON.stringify(cantermsurr)); //insert ke tempDB		

//					});

//				}

//				$('#loading-img').hide();
//				$("#output").html("finish validation " + str);
				
//			}else{
//				$("#output").html("No record data in Sheet TerminateMember");
//				$("#data").val("");
//				$("#data_support").val("");
//				$('#button').hide();
//				$('#export').hide();
//				$('#loading-img').hide();
//				return false;
//			}
			
//		} //end typesubmission
		
		
//	}// end output
//	else{
//		$("#output").html("No record data in ExcelFile");
//		$("#data").val("");
//		$("#data_support").val("");
//		$('#button').hide();
//		$('#export').hide();
//		$('#loading-img').hide();
//		return false;
//	}
//}
	
function errCode (code){
	var result = "";
	if (code == "1001"){
		//result = "tidak boleh kosong";
		result = "not be empty";
	}
	if (code == "1002"){
		//result = "format tgl salah (mm/dd/yyyy)";
		result = "format date must be (mm/dd/yyyy)";
	}
	if (code == "1003"){
		//result = "nilai salah (F/M)";	
		result = "value must be (M/F)";
	}
	if (code == "1004"){
		result = "wrong format";
	}
	return result;
}	
	
function pad( num ) {
    num = "0" + num;
    return num.slice( -2 );
}	
function validatedate(string) {  
  var dateformat = /^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$/; //dd/mm/yyyy or (24/05/2017)  
  //var dateformat = /^(0?[1-9]|[12][0-9]|3[01])[\/\-](\b\d{1,2}\D{0,3})?\b(?:Jan(?:uary)?|Feb(?:ruary)?|Mar(?:ch)?|Apr(?:il)?|May|Jun(?:e)?|Jul(?:y)?|Aug(?:ust)?|Sep(?:tember)?|Oct(?:ober)?|(Nov|Dec)(?:ember)?)[\/\-]\d{4}$/; //dd/MM/yyyy(24/May/2017)
	// Match the date format through regular expression  
  if(string.match(dateformat)){  
	  //document.form1.text1.focus();  
	  //Test which seperator is used '/' or '-'  
	  var opera1 = string.split('/');  
	  var opera2 = string.split('-');  
	  lopera1 = opera1.length;  
	  lopera2 = opera2.length;  
	  // Extract the string into month, date and year  
	  if (lopera1>1){  
	  	var pdate = string.split('/');
		return pdate;  
	  }else if (lopera2>1){  
	  	var pdate = string.split('-');  
	  }
	  
	  var dd = parseInt(pdate[0]);  
	  var mm  = parseInt(pdate[1]);  
	  var yy = parseInt(pdate[2]);  
	  // Create list of days of a month [assume there is no leap year by default]  
	  var ListofDays = [31,28,31,30,31,30,31,31,30,31,30,31];  
	  if (mm==1 || mm>2){  
		  if (dd>ListofDays[mm-1]){
			  //console.log("disini1 : " + mm);
			  return false;  
		  }else{
			  return true;
		  }  
	  }  
	  if (mm==2){  
		  var lyear = false;  
		  if ( (!(yy % 4) && yy % 100) || !(yy % 400)){  
			  lyear = true;  
		  }  
		  if ((lyear==false) && (dd>=29)){
			  //console.log("disini2 : " + dd);
			  return false;  
		  }else{
			  return true;
		  }  
		  if ((lyear==true) && (dd>29)){
			  //console.log("disini3 : " + dd);
			  return false;  
		  }else{
			  return true;
		  }  
	  }  
  }else { 
	  //console.log("disini4 : " + string);
	  return false;  
  }  
} 	
function JSONToCSVConvertor(JSONData, ReportTitle, ShowLabel,JSONhdr,filenm) {
    //If JSONData is not an object then JSON.parse will parse the JSON string in an Object
    var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;
	var arrDatahdr = typeof JSONhdr != 'object' ? JSON.parse(JSONhdr) : JSONhdr;
    var CSV = '';   
    //CSV += ReportTitle + '\r\n\n';
	CSV += "";
    //This condition will generate the Label/Header
    if (ShowLabel) {
        var row = "";
        
        //This loop will extract the label from 1st index of on array
        for (var index in arrDatahdr[0]) {
            row += index + ',';
        }
        row = row.slice(0, -1);
        CSV += row + '\r\n';
    }
    //1st loop is to extract each row
    for (var i = 0; i < arrData.length; i++) {
        var row = "";
        //2nd loop will extract each column and convert it in string comma-seprated
        for (var index in arrData[i]) {
            row += '"' + arrData[i][index] + '",';
        }
        row.slice(0, row.length - 1);
        //add a line break after each row
        CSV += row + '\r\n';
    }
    if (CSV == '') {        
        alert("Invalid data");
        return;
    }   
    
    var fileName = filenm + "_Error_";
    fileName += ReportTitle;//.replace(/ /g,"_");   
    //Initialize file format you want csv or xls
    if(detectIE()){
        /*var IEwindow = window.open();
        IEwindow.document.write('sep=,\r\n' + CSV);
        IEwindow.document.close();
        IEwindow.document.execCommand('SaveAs', true, fileName + ".csv");
        IEwindow.close();*/
		window.txtArea1.document.open("text/html", "replace");
    	window.txtArea1.document.write('sep=,\r\n' + CSV);
    	window.txtArea1.document.close();
    	window.txtArea1.focus();
    	window.txtArea1.document.execCommand('SaveAs', true, fileName + ".csv");
    } else {
        var uri = 'data:application/csv;charset=utf-8,' + escape(CSV);
        var link = document.createElement("a");
        link.href = uri;
        link.style = "visibility:hidden";
        link.download = fileName + ".csv";
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    }
}
	
function detectIE() {
    var ua = window.navigator.userAgent;

    var msie = ua.indexOf('MSIE ');
    if (msie > 0) {
        // IE 10 or older => return version number
        return parseInt(ua.substring(msie + 5, ua.indexOf('.', msie)), 10);
    }

    var trident = ua.indexOf('Trident/');
    if (trident > 0) {
        // IE 11 => return version number
        var rv = ua.indexOf('rv:');
        return parseInt(ua.substring(rv + 3, ua.indexOf('.', rv)), 10);
    }

    var edge = ua.indexOf('Edge/');
    if (edge > 0) {
       // Edge (IE 12+) => return version number
       return parseInt(ua.substring(edge + 5, ua.indexOf('.', edge)), 10);
    }

    // other browser
    return false;
}	
function removeJsonAttrs(json,attrs){
    return JSON.parse(JSON.stringify(json,function(k,v){
        return attrs.indexOf(k)!==-1 ? undefined: v;
}));}	
	
function find_in_object(my_object, my_criteria){

  return my_object.filter(function(obj) {
    return Object.keys(my_criteria).every(function(c) {
      return obj[c] == my_criteria[c];
    });
  });

}	
function postDatatemp(datatemp){
	$.ajax({
    type: 'POST',
    url: '../apirest/datatemplate',
    dataType: 'json',
    data: datatemp,
    success: function(msg) {
		var status = msg["info"][0]["status"];
		var descp = msg["info"][0]["descp"];	
			if (status == "1"){
				console.log(descp);
			}else{
				console.log(descp);
			} 
		}
  	});
}
//validateDatatemp
function validateDatatemp (datacell){
	var response = null;
	$.ajax({
			type: 'POST',
			url: '../apirest/validasitemplate',
			dataType: 'json',
			data: datacell,
			async: false,
			success: function(msg) {
				response = msg;
				/*status = msg["info"][0]["status"];
				descp = msg["info"][0]["descp"];	
					if (status == "1"){
						console.log(descp);
					}else{
						console.log(descp);
					} */
			}
	});
	return response;	
}
//check version Excelfile
function versionExcel (datacell){
	var response = null;
	$.ajax({
			type: 'POST',
			url: '../apirest/validasitemplate',
			dataType: 'json',
			data: datacell,
			async: false,
			success: function(msg) {
				response = msg;
			}
	});
	return response;	
}
//check value type submission
function doctype (data){
	var response = null;
	$.ajax({
			type: 'POST',
			url: '../apirest/getdatadoc',
			dataType: 'json',
			data: data,
			async: false,
			success: function(msg) {
				response = msg;
			}
	});
	return response;	
}	

function parseNumber(item) {
  isField=false
  Value = (isField) ? parseFloat( $(item).val().replace(/,/g,'') ).toFixed(2) : parseFloat( item.replace(/,/g,'') ).toFixed(2)
  return +Value;
}
	
function setfmt() {if(global_wb) process_wb(global_wb); }
window.setfmt = setfmt;
function bytesToSize(bytes) {
    var sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
    if (bytes == 0) return 'n/a';
    var i = parseInt(Math.floor(Math.log(bytes) / Math.log(1024)));
    if (i == 0) return bytes + ' ' + sizes[i];
    return (bytes / Math.pow(1024, i)).toFixed(1) + ' ' + sizes[i];
};
