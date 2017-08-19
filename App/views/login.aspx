<%@ Page Language="VB" AutoEventWireup="false" CodeFile="login.aspx.vb" Inherits="app_views_login" %>

    <div class="middle-box text-center loginscreen  animated fadeInDown">
        <div>
            <div>
                <h1 class="logo-name">BaS</h1>
            </div>
            <h3>Welcome to BAS APP</h3>
            <p>Web Application Bancassurance
                <!--Continually expanded and constantly improved Inspinia Admin Them (IN+)-->
            </p>
            <p>Login in</p>
            <form class="m-t" role="form">
                <div class="form-group">
                    <input type="text" ng-model="loginData.userName" id="txtuname" class="form-control" placeholder="Username" required autofocus/>
                </div>
                <div class="form-group">
                    <input type="password" ng-model="loginData.password" id="txtpassword" class="form-control" placeholder="Password" required/>
                </div>
                <button type="submit" ng-click="login()" runat="server"  class="btn btn-primary block full-width m-b">Login</button>
                <%--<div ng-class="{ 'alert': flash, 'alert-success': flash.type === 'success', 'alert-danger': flash.type === 'error' }" ng-if="flash" ng-bind="flash.message"></div>--%>
                <div ng-hide="message == ''" class="alert alert-danger">
                    {{message}}
                </div>
                <a href="#"><small>Forgot password?</small></a>
<%--                <p class="text-muted text-center"><small>Do not have an account?</small></p>
                <a class="btn btn-sm btn-white btn-block" href="register.html">Create an account</a>--%>
            </form>
            <p class="m-t"> <small>Hanwhalife Bancassurance web app framework base on Bootstrap 3 &copy; 2017</small> </p>
        </div>
    </div>

