﻿<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ibox_tools_full_screen.aspx.vb" Inherits="App_Views_common_ibox_tools_full_screen" %>

<div class="ibox-tools dropdown" dropdown>
    <a ng-click="showhide()"> <i class="fa fa-chevron-up"></i></a>
    <a ng-click="fullscreen()">
        <i class="fa fa-expand"></i>
    </a>
    <a class="dropdown-toggle" href dropdown-toggle>
        <i class="fa fa-wrench"></i>
    </a>
    <ul class="dropdown-menu dropdown-user">
        <li><a href>Config option 1</a>
        </li>
        <li><a href>Config option 2</a>
        </li>
    </ul>
    <a ng-click="closebox()"><i class="fa fa-times"></i></a>
</div>
