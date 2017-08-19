'use strict';

function appmenuControllers($scope, $state, appmenuService, authService) {
    $scope.authentication = authService.authentication;
    var uid = $scope.authentication.uid;
    //var groupnm = $scope.authentication.groupnm;
    console.log(uid);

    $scope.SiteMenu = [{
        ID: "",
        NAME: "",
        URL: "",
        PARENTID:""
    }];

    getSiteMenu();
    
    function getSiteMenu() {
        var datajson = {
            id: "",
            name: "",
            url: "",
            parentid:""
        }
        appmenuService.appmenu().then(function (result) {
            //console.log(JSON.stringify(result.data));
             $scope.SiteMenu = result.data;
         },
         function (err) {
             //console.log(err.data.message);
             //$scope.message = err.data.message;
         });
    }


}

angular
    .module('WebApp')
    .controller('appmenuControllers', appmenuControllers)