<%@ Page Language="VB" AutoEventWireup="false" CodeFile="content.aspx.vb" Inherits="App_Views_common_content" %>

<!-- Wrapper-->
<div id="wrapper">

    <!-- Navigation -->
    <div ng-include="'./App/views/common/navigation.aspx'"></div>

    <!-- Page wraper -->
    <!-- ng-class with current state name give you the ability to extended customization your view -->
    <div id="page-wrapper" class="gray-bg {{$state.current.name}}">

        <!-- Page wrapper -->
        <div ng-include="'./App/views/common/topnavbar.aspx'"></div>
        <div class="row wrapper border-bottom white-bg page-heading" ng-controller="appmenuControllers">
            <div class="col-lg-10">
                <h2>{{SiteMenu[0].NAME}}</h2>
                <ui-breadcrumbs displayname-property="data.displayName" abstract-proxy-property="data.proxy" template-url="./App/views/common/uiBreadcrumbs.tpl.html"></ui-breadcrumbs>
            </div>
        </div>
        <!-- Main view  -->
        <div ui-view></div>
        
        <!-- Footer -->
        <div ng-include="'./App/views/common/footer.aspx'"></div>
        
    </div>
    <!-- End page wrapper-->

    <!-- Right Sidebar -->
    <%--<div ng-include="'./App/views/common/right_sidebar.aspx'"></div>--%>
    

</div>
<!-- End wrapper-->
