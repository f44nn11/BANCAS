'use strict';

function layoutControllers($scope, $location, authService) {
    $scope.authentication = authService.authentication;
    $scope.rptData = {
        Uname: "",
        Uid: "",
        Opt:""
    };
    if (!$scope.authentication.isAuth) {
        $location.path('/login');
    };
    //$scope.uri = "http://localhost/ReportServer?" + "/ReportWeb/Report1&rs:Command=Render&rs:Format=PDF";
    $scope.report = function () {
        window.open('./app/pdfviewer/web/viewer.aspx?'+btoa('file:' + 'Report1&'));
        //window.open('http://localhost/ReportServer?/ReportWeb/Report1&rs:Command=Render&rs:ClearSession=true&rc:Toolbar=false&rs:Format=PDF');
        //window.open('./pdfjs/index.html');
    };
    var date = new Date();
    var pUName;
    var pUserId;
    var pOpt;
    var api;

    $scope.report2 = function () {
        pUName = $scope.rptData.Uname;
        pUserId = $scope.rptData.Uid;
        pOpt = $scope.rptData.Opt;
        window.open('./app/pdfviewer/web/viewer.aspx?'+btoa('file:' + 'Report2&pUName='+pUName+'&pUserId='+pUserId+'&pOpt='+pOpt+'&'));
    };

    $scope.logOut = function () {
        authService.logOut();
        $location.path('/login');
    };

}



angular
    .module('WebApp')
    .controller('layoutControllers', layoutControllers)