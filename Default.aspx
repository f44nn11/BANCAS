<%@ Page Language="VB" AutoEventWireup="true" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html>
<html ng-app ="WebApp" xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <title>BAS | WEBAPP</title>

    <link href="Content/css/bootstrap.min.css?v=1" rel="stylesheet" />
    <link href="Content/font-awesome/css/font-awesome.css?v=1" rel="stylesheet"/>

    <link href="Content/css/animate.css?v=1" rel="stylesheet"/>
    <link href="Content/css/loading-bar.css?v=1" rel="stylesheet"/>

    <link id="loadBefore" href="Content/css/style.css?v=1" rel="stylesheet"/>
    <link rel="shortcut icon" href="favicon.ico"/>


 </head>
<body class="gray-bg" ng-controller="MainCtrl as main" landing-scrollspy id="page-top">

    <div ui-view=""></div>


    <!-- jQuery and Bootstrap -->
    <script src="Scripts/jquery/jquery-2.1.1.js?t=<%= DateTime.Now.Ticks %>"></script>
    <script src="Scripts/plugins/jquery-ui/jquery-ui.js?t=<%= DateTime.Now.Ticks %>"></script>
    <script src="Scripts/bootstrap/bootstrap.min.js?t=<%= DateTime.Now.Ticks %>"></script>   
    

    <!-- MetsiMenu -->
    <script src="Scripts/plugins/metisMenu/jquery.metisMenu.js?t=<%= DateTime.Now.Ticks %>"></script>

    <!-- SlimScroll -->
    <script src="Scripts/plugins/slimscroll/jquery.slimscroll.min.js?t=<%= DateTime.Now.Ticks %>"></script>

    <!-- Peace JS -->
    <script src="Scripts/plugins/pace/pace.min.js?t=<%= DateTime.Now.Ticks %>"></script>

    <!-- Custom and plugin javascript -->
    <script src="Scripts/inspinia.js?t=<%= DateTime.Now.Ticks %>"></script>

    <!-- Angularjs 1.6.4 scripts -->
    <script src="Scripts/angular/angular.min.js"></script>
    <script src="Scripts/angular/angular-route.js"></script>
    <%--<script src="Scripts/angular-translate/angular-translate.min.js"></script>--%>
    <script src="Scripts/angular/angular-ui-router.min.js"></script>
    <script src="Scripts/angular/angular-sanitize.js"></script>
    <script src="Scripts/angular/angular-cookies.min.js"></script>
    <script src="scripts/angular/angular-local-storage.min.js"></script>
    <%--Read Excel--%>
    <script src="Scripts/excel/cpexcel.js"></script>
    <script src="Scripts/excel/xlsx.js"></script>
    <script src="Scripts/utility.js"></script>

    <script src="Scripts/bootstrap/ui-bootstrap-tpls-0.12.0.min.js?t=<%= DateTime.Now.Ticks %>"></script>
    <script src="Scripts/loading-bar.min.js"></script>
    <script src="Scripts/plugins/oclazyload/dist/ocLazyLoad.min.js"></script>
    <script src="Scripts/plugins/angular-idle/angular-idle.js"></script>
    <script src="Scripts/dirPagination.js"></script>
    <!-- uiBreadcrumbs javascript -->
    <script src="Scripts/uiBreadcrumbs.js"></script>

    <!-- Load app main script -->
    <script src="App/app.js?t=<%= DateTime.Now.Ticks %>"></script>
    <script src="App/config.js?t=<%= DateTime.Now.Ticks %>"></script>
    <script src="App/directives.js?t=<%= DateTime.Now.Ticks %>"></script>
    <%--<script src="App/translations.js"></script>--%>

    <!-- Load services -->
    <script src="App/services/authInterceptorService.js?t=<%= DateTime.Now.Ticks %>"></script>
    <script src="App/services/authService.js?t=<%= DateTime.Now.Ticks %>"></script>
    <script src="App/services/tokensManagerService.js?t=<%= DateTime.Now.Ticks %>"></script>
    <script src="App/services/appmenuService.js?t=<%= DateTime.Now.Ticks %>"></script>
    <script src="App/services/userService.js?t=<%= DateTime.Now.Ticks %>"></script>
    <script src="App/services/ugroupIDService.js?t=<%= DateTime.Now.Ticks %>"></script>
    <script src="App/services/uploadfileService.js?t=<%= DateTime.Now.Ticks %>"></script>
    <!-- Load controllers -->
    <script src="App/controllers/loginControllers.js?t=<%= DateTime.Now.Ticks %>"></script>
    <script src="App/controllers/appmainControllers.js?t=<%= DateTime.Now.Ticks %>"></script>
    <script src="App/controllers/layoutControllers.js?t=<%= DateTime.Now.Ticks %>"></script>
    <script src="App/controllers/appmenuControllers.js?t=<%= DateTime.Now.Ticks %>"></script>
    <script src="App/controllers/usermanageControllers.js?t=<%= DateTime.Now.Ticks %>"></script>
    <script src="App/controllers/changepassControllers.js?t=<%= DateTime.Now.Ticks %>"></script>
    <script src="App/controllers/contactControllers.js?t=<%= DateTime.Now.Ticks %>"></script>
</body>
</html>
