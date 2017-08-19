'use strict';

function usermanageControllers($rootScope, $scope, $state, $location,$timeout, authService, userService,ugroupIDService,toaster) {
    var urlstate;
    var id;
    var name;
    var ugroupid;
    var sessionuser;
    var appregno;
    var mdlname;
    var entryfrom;

    $scope.authentication = authService.authentication;
    $scope.Alluser = [];
    $scope.Allmodule = [{
        APPREGNO : "",
        APPDESC: "",
        ENTRYFORM:""
    }];   
    $scope.model = {};
    $scope.Allmodulenew = [{
        APPREGNO: "",
        APPDESC: ""
    }];
    $scope.AuthUser = [];
    $scope.Groupid = [];
    $scope.fromname = ""; //select option copy user

    if (!$scope.authentication.isAuth) {
        $location.path('/login');
    };

    urlstate = $state.current.url;

    if (urlstate == '/manage') {
        $scope.data = {
            id: "",
            name: "",
            ugroupid: "",
            sessionuser: "",
            modulenm: "",
            appregno: ""
        };
        $scope.datanewuser = {
            id: "",
            name: "",
            ugroupid: "",
            sessionuser: $scope.authentication.userName,
            password: "",
            cpassword: ""
        };
        $scope.datacopyuser = {
            uid:"",
            name: "",
            fromname: "",
            rbopr:""
        }
        $scope.data.datachk = {
            chkview: true,
            chkadd: false,
            chkedit: false,
            chkdelete: false,
            chkprint: false
        };
        $rootScope.showhide = {
            topmodule: true,
            alluser: true,
            addnewuser: false,
            edit: false,
            module: false,
            editmodul: false,
            addnewmodule: false,
            copymodule:false
        }
        $rootScope.showchk = {
            chk : true
        }
        
        $scope.showhide = $rootScope.showhide;
        $scope.showchk = $rootScope.showchk;
        
        $scope.edituser = $rootScope.edituser;
        getAlluser();
        ugroupIDService.ugroupID().then(function (result) {
            //console.log(JSON.stringify(result));
            $scope.Groupid = result.data;
        });
        
        $scope.selectedRow = null;  // initialize our variable to null
        $scope.setClickedRow = function (index) {  //function that sets the value of selectedRow to current index            
            $scope.selectedRow = index;
        }
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        $scope.sort2 = function (keyname) {
            $scope.sortKey2 = keyname;   //set the sortKey to the param passed
            $scope.reverse2 = !$scope.reverse2; //if true make it false and vice versa
        }
        $scope.sort3 = function (keyname) {
            $scope.sortKey3 = keyname;   //set the sortKey to the param passed
            $scope.reverse3 = !$scope.reverse3; //if true make it false and vice versa
        }
        $scope.currentPage = 1;
        $scope.currentPage2 = 1;
        $scope.currentPage3 = 1;

        //Begin AddNew User
        $scope.addnewuser = function () {
            $rootScope.showhide.addnewuser = true;
            $rootScope.showhide.edit = false;
            $rootScope.showhide.alluser = false;
            $rootScope.showhide.module = false;
            $rootScope.showhide.editmodul = false;
            $rootScope.showhide.addnewmodule = false;
            $rootScope.showhide.copymodule = false;
            $scope.title = " / Addnew User";
            //console.log("Addnew User");
        }
        $scope.saveAddnewuser = function () {
            
            var datauser = {
                "uid": "",
                "uname": $scope.datanewuser.name,
                "pass": $scope.datanewuser.password,
                "ugroupid": $scope.datanewuser.ugroupid,
                "sessionuser": $scope.authentication.userName,
                "actiontype": "addnewuser"
            }
            if ($scope.frm_addnewuser.$valid) {
                //console.log(datauser);
                saveAddnewuser(datauser);
            } else {
                $scope.frm_addnewuser.submitted = true;
            }
        };
        $scope.cancelAddnewuser = function () {
            $rootScope.showhide.addnewuser = false;
            $rootScope.showhide.edit = false;
            $rootScope.showhide.alluser = true;
            $rootScope.showhide.module = false;
            $rootScope.showhide.editmodul = false;
            $rootScope.showhide.addnewmodule = false;
            $rootScope.showhide.copymodule = false;
            $scope.datanewuser = {};            
            $scope.title = "";
        };
        //End AddNew User


        //Begin Edit User
        $scope.cancel = function () {
            $rootScope.showhide.edit = false;
            $rootScope.showhide.alluser = true;
            $rootScope.showhide.module = false;
            $rootScope.showhide.editmodul = false;
            $rootScope.showhide.addnewmodule = false;
            $rootScope.showhide.copymodule = false;
            $scope.selectedRow = null;
        }
        $scope.edit = function (value) {
            //console.log(value);
            var array = value.split(',');
            id = array[0];
            name = array[1];
            ugroupid = array[2];


            $scope.data.id = id;
            $scope.data.name = name;
            $scope.data.ugroupid = ugroupid;
            $rootScope.showhide.edit = true;
            $rootScope.showhide.alluser = false;
            $rootScope.showhide.module = false;
            $rootScope.showhide.editmodul = false;
            $rootScope.showhide.addnewmodule = false;
            $rootScope.showhide.copymodule = false;



        };
        $scope.saveedit = function () {
            //$rootScope.edituser.showedit = false;
            //$rootScope.alluser.showalluser = true;
            //$scope.selectedRow = null;
            id = $scope.data.id;
            name = $scope.data.name;
            ugroupid = $scope.data.ugroupid;
            sessionuser = $scope.authentication.userName;
            var datauser = {
                "uid" : id,
                "uname": name,
                "pass":"",
                "ugroupid": ugroupid,
                "sessionuser": sessionuser,
                "actiontype":"update"
            }
           userService.saveedituser(datauser).then(function (result) {
               //console.log(JSON.stringify(result));
               var status = result.data.status;
               if (status = '1'){
                   $rootScope.showhide.edit = false;
                   $rootScope.showhide.alluser = true;
                   $rootScope.showhide.module = false;
                   $rootScope.showhide.editmodul = false;
                   $rootScope.showhide.addnewmodule = false;
                   getAlluser();
                   toaster.success({
                       type: 'info',
                       title: 'Update Success',
                       body: 'You are update in Application of User Authorization',
                       showCloseButton: true

                   });
               }
               
            },
             function (err) {
                 console.log(err.data.message);
                 //$scope.message = err.data.message;
             });
        }
        $scope.delete = function (index) {
            $scope.Alluser.splice(index, 1);
        };
        //End Edit User


        //Begin Edit Module Application of User Authorization
        $scope.addnewmodule = function () {
            //$rootScope.showhide.edit = false;
            //$rootScope.showhide.alluser = false;
            //$rootScope.showhide.module = false;
            //$rootScope.showhide.editmodul = false;
            $rootScope.showhide.addnewmodule = true;
            $rootScope.showhide.topmodule = false;
            getModuleNew(id, name);
        };        
        $scope.copymodule = function () {
            //$scope.userAll = $scope.Alluser;
            $scope.datacopyuser.uid = id;
            $scope.datacopyuser.name = name;
            $rootScope.showhide.edit = false;
            $rootScope.showhide.alluser = false;
            $rootScope.showhide.module = false;
            $rootScope.showhide.editmodul = false;
            $rootScope.showhide.addnewmodule = false;
            $rootScope.showhide.copymodule = true;
        };
        $scope.undomodule = function () {
            $rootScope.showhide.edit = false;
            $rootScope.showhide.alluser = true;
            $rootScope.showhide.module = false;
            $rootScope.showhide.editmodul = false;
            $rootScope.showhide.addnewmodule = false;
            $rootScope.showhide.copymodule = false;
            $scope.selectedRow = null;
            $scope.title = "";
        }
        $scope.savecopyuser = function () {
            var uid = $scope.fromname;
            var opr = $scope.datacopyuser.rbopr;
            var actiontype = "";

            if (opr == "copy"){
                actiontype = "copyuser";
            } else {
                actiontype = "appenduser";
            }

            var datauser = {
                "uid": $scope.datacopyuser.uid,
                "uname": uid.UID,
                "pass": "",
                "ugroupid": "",
                "sessionuser": $scope.authentication.userName,
                "actiontype": actiontype
            }
            if ($scope.frm_copyuser.$valid) {
                if (datauser.uname != "") {
                    //console.log(JSON.stringify(datauser));
                    
                    saveCopyuser(datauser);
                } else {
                    toaster.pop({
                        type: 'error',
                        title: 'Save Failed',
                        body: 'Copy From User is empty',
                        showCloseButton: true
                    });
                }
            } else {
                $scope.frm_copyuser.submitted = true;
            }
        }
        $scope.cancelcopyuser = function () {
            $rootScope.showhide.edit = false;
            $rootScope.showhide.alluser = false;
            $rootScope.showhide.module = true;
            $rootScope.showhide.editmodul = false;
            $rootScope.showhide.addnewmodule = false;
            $rootScope.showhide.copymodule = false;
            $scope.fromname = "";
            $scope.datacopyuser.rbopr = "";
        }
        $scope.module = function (value) {
            $scope.title = " / Module User";
            var array = value.split(',');
            id = array[0];
            name = array[1];

            $scope.data.name = name;
            $rootScope.showhide.edit = false;
            $rootScope.showhide.alluser = false;
            $rootScope.showhide.module = true;
            $rootScope.showhide.editmodul = false;
            $rootScope.showhide.addnewmodule = false;
            $rootScope.showhide.copymodule = false;
            getModule(id, name);

        }
        $scope.editmodule = function (value) {

            var array = value.split(',');
            appregno = array[0];
            mdlname = array[1];
            entryfrom = array[2];

            $scope.data.uid = id;
            $scope.data.name = name;
            $scope.data.modulenm = mdlname;
            $scope.data.appregno = appregno;

            $scope.data.datachk.chkview = true;
            getModuleAppstatus(id, appregno, entryfrom);

            $rootScope.showhide.edit = false;
            $rootScope.showhide.alluser = false;
            $rootScope.showhide.module = false;
            $rootScope.showhide.editmodul = true;
            $rootScope.showhide.addnewmodule = false;
            $rootScope.showhide.copymodule = false;

        }
        $scope.canceleditmodule = function () {
            $rootScope.showhide.edit = false;
            $rootScope.showhide.alluser = false;
            $rootScope.showhide.module = true;
            $rootScope.showhide.editmodul = false;
            $rootScope.showhide.addnewmodule = false;
            $rootScope.showhide.copymodule = false;
            $scope.selectedRow = null;
        };
        $scope.saveeditmodule = function (datachk) {
            var uid = datachk.uid;
            var appregno = datachk.appregno;
            var add = datachk.datachk.chkadd;
            var edit = datachk.datachk.chkedit;
            var del = datachk.datachk.chkdelete;
            var print = datachk.datachk.chkprint;

            $scope.dataarray = [];

            if (add) {
                $scope.dataarray.push({ "status": "ADD" });
            }
            if (edit) {
                $scope.dataarray.push({ "status": "EDIT" });
            }
            if (del) {
                $scope.dataarray.push({ "status": "DELETE" });
            }
            if (print) {
                $scope.dataarray.push({ "status": "PRINT" });
            }
            var datasend = [{
                "uid": uid,
                "appregno": appregno,
                "sessionuser": $scope.authentication.userName,
                "statuss": $scope.dataarray,
                "actiontype": "modulestatusupd"
            }];
            //console.log(JSON.stringify(datasend));
            StatusAppUpd(JSON.stringify(datasend));
            //angular.forEach($scope.dataarray, function (item) {
            //    var datasend = {
            //        "uid" : uid,
            //        "appregno": appregno,
            //        "status": item
            //    }
            //    console.log(JSON.stringify(datasend));
            //});

        };
        //End Edit Module Application of User Authorization

        //Begin scope Add new Module
        $scope.showchkuth = {
            chk : true
        }
        $scope.selectEntity = function (item) {
            var index = $scope.Allmodulenew.indexOf(item.APPREGNO);
            var entryform = item.ENTRYFORM;
            if (item.isChecked) {                
                if (index === -1) {                    
                    $scope.AuthUser.push(item); //add row table
                    
                }
            } else {
                for (var j = 0; j < $scope.AuthUser.length; j++) {
                    if ($scope.AuthUser[j].APPREGNO == item.APPREGNO) {
                        $scope.AuthUser.splice(j, 1); //remove row table
                        break;
                    }
                }                
            }
            for (var i = 0; i < $scope.Allmodulenew.length; i++) {
                if (!$scope.Allmodulenew[i].isChecked) {
                    $scope.model.allItemsSelected = false;
                    return;
                } 
                
            }

            $scope.model.allItemsSelected = true;
        };
        $scope.selectAll = function () {
            angular.forEach($scope.Allmodulenew, function (item) {
                var index = $scope.Allmodulenew.indexOf(item.APPREGNO);
                if (!item.isChecked | undefined) {
                    item.isChecked = $scope.model.allItemsSelected;
                    if (item.isChecked) {
                        if (index === -1) {
                            $scope.AuthUser.push(item);
                        }
                    } 
                } else {
                    item.isChecked = $scope.model.allItemsSelected;
                    for (var j = 0; j < $scope.AuthUser.length; j++) {
                        if ($scope.AuthUser[j].APPREGNO == item.APPREGNO) {
                            if (!item.isChecked) {
                                $scope.AuthUser.splice(j, 1);
                                break;
                            }
                            
                        }
                    }
                }

            });
            
        };
        $scope.saveadd = function () {
            if ($scope.AuthUser.length > 0) {
                var datasend = [];
                var datasendArr = [];
                $scope.datasendArr = [];
                angular.forEach($scope.AuthUser, function (item) {
                    $scope.dataarray = [];
                    var appregno = item.APPREGNO;
                    var add = item.chkAdd;
                    var edit = item.chkEdit;
                    var del = item.chkDel;
                    var print = item.chkPrint;
                    if (add) {
                        $scope.dataarray.push({ "status": "ADD" });
                    }
                    if (edit) {
                        $scope.dataarray.push({ "status": "EDIT" });
                    }
                    if (del) {
                        $scope.dataarray.push({ "status": "DELETE" });
                    }
                    if (print) {
                        $scope.dataarray.push({ "status": "PRINT" });
                    }
                    datasend = {
                        "uid": id,
                        "appregno": appregno,
                        "sessionuser": $scope.authentication.userName,
                        "statuss": $scope.dataarray,
                        "actiontype": "modulesinsert"
                    }
                    $scope.datasendArr.push(datasend);
                });
                //console.log(JSON.stringify($scope.datasendArr));
                SaveAddnewModule(JSON.stringify($scope.datasendArr))
            } else {
                toaster.pop({
                    type: 'error',
                    title: 'Data records',
                    body: 'No record data in table.',
                    showCloseButton: true
                    //,timeout: 800
                });
            }
            
        };
        $scope.undosaveadd = function(){
            $rootScope.showhide.addnewmodule = false;
            $rootScope.showhide.topmodule = true;
            $scope.model.allItemsSelected = false;
            for (var j = 0; j < $scope.AuthUser.length; j++) {
                $scope.AuthUser.splice(j, $scope.AuthUser.length);
            }
        }
        //End scope Add new Module

        //hide karena belum dibutuhkan
        Object.defineProperty($scope, "queryFilter", {
            get: function () {
                var out = {};
                out[$scope.queryBy || "$"] = $scope.query;
                return out;
            },
        })
        

    }

    $rootScope.logOut = function () {
        authService.logOut();
        $location.path('/login');
    };

    function getAlluser() {
        userService.user().then(function (result) {
            //console.log(JSON.stringify(result));
            $scope.Alluser = result.data;
            $scope.Alluser.Refresh();
        },
         function (err) {
             //console.log(err.data.message);
             //$scope.message = err.data.message;
         });
    }
    function saveAddnewuser(datauser) {
        userService.saveaddnewuser(datauser).then(function (result) {
            //console.log(JSON.stringify(result));
            var status = result.data.status;
            if (status == '1') {
                $rootScope.showhide.edit = false;
                $rootScope.showhide.alluser = true;
                $rootScope.showhide.addnewuser = false;
                $rootScope.showhide.module = false;
                $rootScope.showhide.editmodul = false;
                $rootScope.showhide.addnewmodule = false;
                $rootScope.showhide.copymodule = false;
                getAlluser();
                toaster.success({
                    type: 'info',
                    title: 'Create Addnew User Success',
                    body: 'You are Create Addnew in Application of User Authorization',
                    showCloseButton: true

                });
            } else {
                toaster.pop({
                    type: 'error',
                    title: 'Create Addnew User Failed',
                    body: 'You are Create Addnew in Application of User Authorization',
                    showCloseButton: true
                });
            }

        },
          function (err) {
              //console.log(err.data.message);
              //$scope.message = err.data.message;
              toaster.pop({
                  type: 'error',
                  title: 'Insert Addnew Error',
                  body: err.data.message,
                  showCloseButton: true

              });
          });
    }
    function saveCopyuser(datauser) {
        userService.savecopyuser(datauser).then(function (result) {
            //console.log(JSON.stringify(result));
            var status = result.data.status;
            if (status == '1') {
                $rootScope.showhide.edit = false;
                $rootScope.showhide.alluser = false;
                $rootScope.showhide.addnewuser = false;
                $rootScope.showhide.module = true;
                $rootScope.showhide.editmodul = false;
                $rootScope.showhide.addnewmodule = false;
                $rootScope.showhide.copymodule = false;
                
                getModule(id, "");

                //console.log("ini id :" + id);
                toaster.success({
                    type: 'info',
                    title: 'Create module User Success',
                    body: 'You are Create module copy in Application of User Authorization',
                    showCloseButton: true

                });
            } else {
                toaster.pop({
                    type: 'error',
                    title: 'Create Module User Failed',
                    body: result.data.message,
                    showCloseButton: true
                });
            }

        },
          function (err) {
              //console.log(err.data.message);
              //$scope.message = err.data.message;
              toaster.pop({
                  type: 'error',
                  title: 'Create Module User Error',
                  body: err.data.message,
                  showCloseButton: true

              });
          });
    }
    function getModule(uid, name) {
        var datausermodule = {
            "uid": uid,
            "ugroupid": "",
            "actiontype":"moduleuser"
        }        
        userService.getmodule(datausermodule).then(function (result) {
            //console.log(JSON.stringify(result.data));
            $scope.Allmodule = result.data;
            //$scope.Allmodule.Refresh();
        },
         function (err) {
             //console.log(err.data.message);
             //$scope.message = err.data.message;
         });
    }
    function getModuleAppstatus(uid, appregno,entryfrom) {
        var datausermodule = [{
            "uid": uid,
            "appregno": appregno,
            "sessionuser": "",
            "actiontype": "select",
        }]
        userService.getmoduleopr(datausermodule).then(function (result) {
            //console.log(JSON.stringify(result.data));
            var lenghts = JSON.stringify(result.data).length;
            if (entryfrom == 1) {
                $rootScope.showchk.chk = true;
                angular.forEach(result.data, function (item) {
                    //console.log(item.STATUS);
                    if (item.STATUS == "EDIT") {
                        $scope.data.datachk.chkedit = true;
                    }
                    if (item.STATUS == "ADD") {
                        $scope.data.datachk.chkadd = true;
                    }
                    if (item.STATUS == "DELETE") {
                        $scope.data.datachk.chkdelete = true;
                    }
                    if (item.STATUS == "PRINT") {
                        $scope.data.datachk.chkprint = true;
                    }
                });
            } else {
                $scope.data.datachk = {
                    chkview: true,
                    chkadd: false,
                    chkedit: false,
                    chkdelete: false,
                    chkprint: false
                }
                $rootScope.showchk.chk = false;
            }
        },
         function (err) {
             //console.log(err.data.message);
             //$scope.message = err.data.message;
         });
    }
    function StatusAppUpd(datauser) {
        userService.getmoduleoprupdinstdel(datauser).then(function (result) {
            //console.log(JSON.stringify(result.data));
            var status = result.data.status;
            if (status = '1') {
                $rootScope.showhide.edit = false;
                $rootScope.showhide.alluser = false;
                $rootScope.showhide.module = true;
                $rootScope.showhide.editmodul = false;
                $rootScope.showhide.addnewmodule = false;
                $rootScope.showhide.copymodule = false;
                getAlluser();
                toaster.success({
                    type: 'info',
                    title: 'Update StatusAPP Success',
                    body: 'You are update in Application of User Authorization',
                    showCloseButton: true
                });
            }
        },
         function (err) {
             //console.log(err.data.message);
             //$scope.message = err.data.message;
         });
    }
    function SaveAddnewModule(datauser) {
        userService.getmoduleoprupdinstdel(datauser).then(function (result) {
            //console.log(JSON.stringify(result.data));
            var status = result.data.status;
            var uid = result.data.uid;
            if (status == '1') {
                getModule(uid, name);
                toaster.success({
                    type: 'info',
                    title: 'Insert New Module Success',
                    body: 'You are Insert in Application of User Authorization',
                    showCloseButton: true
                });
                $rootScope.showhide.addnewmodule = false;
                $rootScope.showhide.topmodule = true;
                $scope.model.allItemsSelected = false;
                for (var j = 0; j < $scope.AuthUser.length; j++) {
                    $scope.AuthUser.splice(j, $scope.AuthUser.length);
                }

            }
        },
         function (err) {
             //console.log(err.data.message);
             //$scope.message = err.data.message;
         });
    }
    function getModuleNew(uid, name) {
        var datausermodule = {
            "uid": uid,
            "ugroupid": "",
            "actiontype": "modulenew"
        }
        userService.getmodulenew(datausermodule).then(function (result) {
            //console.log(JSON.stringify(result.data));
            //console.log(result.data);
            $scope.Allmodulenew = result.data;
            //$scope.Allmodule.Refresh();
        },
         function (err) {
             //console.log(err.data.message);
             //$scope.message = err.data.message;
         });
    }
}

angular
    .module('WebApp')
    .controller('usermanageControllers', usermanageControllers);

