﻿<%@ Page Language="VB" AutoEventWireup="false" CodeFile="navigation.aspx.vb" Inherits="App_Views_common_navigation" %>

<nav class="navbar-default navbar-static-side" role="navigation" ng-controller="appmenuControllers">
    <div class="sidebar-collapse">
        <ul side-navigation class="nav metismenu" id="side-menu">
            <li class="nav-header">
                <div class="dropdown profile-element" dropdown>
                    <img alt="image" class="img-circle" src="./Content/img/profile_small.png"/>
                    <a class="dropdown-toggle" dropdown-toggle href>
                            <span class="clear">
                                <span class="block m-t-xs">
                                    <strong class="font-bold">{{authentication.userName}}</strong>
                                </span>
                                <span class="text-muted text-xs block">{{authentication.groupid}} <b class="caret"></b></span>
                            </span>
                    </a>
                    <ul class="dropdown-menu animated fadeInRight m-t-xs">
                        <li><a ui-sref="profile">Profile</a></li>
                        <li><a ui-sref="contacts">Contacts</a></li>
                        <li><a ui-sref="inbox">Mailbox</a></li>
                        <li class="divider"></li>
                        <li><a href="../login.html">Logout</a></li>
                    </ul>
                </div>
                <div class="logo-element">
                    BaS
                </div>
            </li>
            
            <li ng-class="{active: $state.includes('{{menu.URL}}')}" ng-repeat="menu in SiteMenu  | filter:{PARENTID : 0}" ng-if="(SiteMenu | filter:{PARENTID : menu.ID}).length > 0">
                <a ui-sref="{{menu.URL}}"><i class="{{menu.CLASS}}"></i> <span class="nav-label" >{{ menu.NAME }} </span> <span class="fa arrow"></span></a>
                <ul ng-class="{in: $state.includes('{{menu.URL}}')}" class="nav nav-second-level collapse" data-ng-if="(SiteMenu | filter:{PARENTID : menu.ID}).length > 0">
                        <li ui-sref-active="active" ng-repeat="menu in SiteMenu  | filter:{PARENTID : menu.ID}" >
                            <a ui-sref="{{menu.URL}}"> {{ menu.NAME }}<span class="fa arrow" ng-if="(SiteMenu | filter:{PARENTID : menu.ID}).length > 1"></span></a>
                            <ul ng-class="{in: $state.includes('{{menu.URL}}')}" class="nav nav-third-level collapse" ng-if="(SiteMenu | filter:{PARENTID : menu.ID}).length > 1">
                                <li ng-class="{active: $state.includes('{{menu.URL}}')}" ui-sref-active="active" ng-repeat="menu in SiteMenu  | filter:{PARENTID : menu.ID}">
                                    <a ui-sref="{{menu.URL}}">{{menu.NAME}}<span class="fa arrow" ng-if="(SiteMenu | filter:{PARENTID : menu.ID}).length > 0"></span></a>
                                        <ul  class="nav nav-four-level collapse" ng-if="(SiteMenu | filter:{PARENTID : menu.ID}).length > 0">
                                            <li ui-sref-active="active" ng-repeat="menu in SiteMenu  | filter:{PARENTID : menu.ID}">
                                                <a ui-sref="{{menu.URL}}">{{menu.NAME}}</a>
                                            </li>
                                        </ul>
                                </li>
                            </ul>
                        </li>
                </ul>
            </li>

            <%--<li ng-class="{active: $state.includes('dashboards')}">
                <a href="index.html"><i class="fa fa-th-large"></i> <span class="nav-label">{{ 'DASHBOARD' }}</span> <span class="fa arrow"></span></a>
                <ul class="nav nav-second-level collapse" ng-class="{in: $state.includes('dashboards')}">
                    <li ui-sref-active="active"><a ui-sref="dashboards.dashboard_1">{{ 'DASHBOARD' }} v.1</a></li>
                    <li ui-sref-active="active"><a ui-sref="dashboards.dashboard_2">{{ 'DASHBOARD' }} v.2</a></li>
                    <li ui-sref-active="active"><a ui-sref="dashboards.dashboard_3">{{ 'DASHBOARD' }} v.3</a></li>
                    <li ui-sref-active="active"><a ui-sref="dashboards.dashboard_4_1">{{ 'DASHBOARD' }} v.4</a></li>
                    <li ui-sref-active="active"><a ui-sref="dashboards.dashboard_5">{{ 'DASHBOARD' }} v.5 <span class="label label-primary pull-right">NEW</span></a></li>
                </ul>
            </li>
            <li ui-sref-active="active">
                <a ui-sref="layouts"><i class="fa fa-diamond"></i> <span class="nav-label">{{ 'LAYOUTS' }}</span> </a>
            </li>

            <li ng-class="{active: $state.includes('charts')}">
                <a href=""><i class="fa fa-bar-chart-o"></i> <span class="nav-label">{{ 'GRAPHS' }}</span><span class="fa arrow"></span></a>
                <ul class="nav nav-second-level collapse" ng-class="{in: $state.includes('charts')}">
                    <li ui-sref-active="active"><a ui-sref="charts.flot_chart">Flot Charts</a></li>
                    <li ui-sref-active="active"><a ui-sref="charts.rickshaw_chart">Rickshaw Charts</a></li>
                    <li ui-sref-active="active"><a ui-sref="charts.chartjs_chart">Chart.js</a></li>
                    <li ui-sref-active="active"><a ui-sref="charts.chartist_chart">Chartist</a></li>
                    <li ui-sref-active="active"><a ui-sref="charts.peity_chart">Peity Charts</a></li>
                    <li ui-sref-active="active"><a ui-sref="charts.sparkline_chart">Sparkline Charts</a></li>
                </ul>
            </li>
            <li ng-class="{active: $state.includes('mailbox')}">
                <a ui-sref="inbox"><i class="fa fa-envelope"></i> <span class="nav-label">{{ 'MAILBOX' }} </span><span class="label label-warning pull-right">16/24</span></a>
                <ul class="nav nav-second-level collapse" ng-class="{in: $state.includes('mailbox')}">
                    <li ui-sref-active="active"><a ui-sref="mailbox.inbox">Inbox</a></li>
                    <li ui-sref-active="active"><a ui-sref="mailbox.email_view">Email view</a></li>
                    <li ui-sref-active="active"><a ui-sref="mailbox.email_compose">Compose email</a></li>
                    <li ui-sref-active="active"><a ui-sref="mailbox.email_template">Email template</a></li>
                </ul>
            </li>
            <li ui-sref-active="active">
                <a ui-sref="metrics"><i class="fa fa-pie-chart"></i> <span class="nav-label">{{ 'METRICS' }}</span> </a>
            </li>
            <li ui-sref-active="active">
                <a ui-sref="widgets"><i class="fa fa-flask"></i> <span class="nav-label">{{ 'WIDGETS' }}</span></a>
            </li>
            <li ng-class="{active: $state.includes('forms')}">
                <a href=""><i class="fa fa-edit"></i> <span class="nav-label">{{ 'FORMS' }}</span><span class="fa arrow"></span></a>
                <ul class="nav nav-second-level collapse" ng-class="{in: $state.includes('forms')}">
                    <li ui-sref-active="active"><a ui-sref="forms.basic_form">Basic form</a></li>
                    <li ui-sref-active="active"><a ui-sref="forms.advanced_plugins">Advanced Plugins</a></li>
                    <li ui-sref-active="active"><a ui-sref="forms.wizard.step_one">Wizard</a></li>
                    <li ui-sref-active="active"><a ui-sref="forms.file_upload">File Upload</a></li>
                    <li ui-sref-active="active"><a ui-sref="forms.text_editor">Text Editor</a></li>
                </ul>
            </li>
            <li ng-class="{active: $state.includes('app')}">
                <a href=""><i class="fa fa-desktop"></i> <span class="nav-label">{{ 'APPVIEWS' }}</span>  <span class="pull-right label label-primary">SPECIAL</span></a>
                <ul class="nav nav-second-level collapse" ng-class="{in: $state.includes('app')}">
                    <li ui-sref-active="active"><a ui-sref="app.contacts">Contacts</a></li>
                    <li ui-sref-active="active"><a ui-sref="app.profile">Profile</a></li>
                    <li ui-sref-active="active"><a ui-sref="app.profile_2">Profile v.2</a></li>
                    <li ui-sref-active="active"><a ui-sref="app.contacts_2">Contacts v.2</a></li>
                    <li ui-sref-active="active"><a ui-sref="app.projects">Projects</a></li>
                    <li ui-sref-active="active"><a ui-sref="app.project_detail">Project detail</a></li>
                    <li ui-sref-active="active"><a ui-sref="app.teams_board">Teams board</a></li>
                    <li ui-sref-active="active"><a ui-sref="app.social_feed">Social feed</a></li>
                    <li ui-sref-active="active"><a ui-sref="app.clients">Clients</a></li>
                    <li ui-sref-active="active"><a ui-sref="app.vote_list">Vote list</a></li>
                    <li ui-sref-active="active"><a ui-sref="app.file_manager">File manager</a></li>
                    <li ui-sref-active="active"><a ui-sref="app.calendar">Calendar</a></li>
                    <li ui-sref-active="active"><a ui-sref="app.issue_tracker">Issue tracker</a></li>
                    <li ui-sref-active="active"><a ui-sref="app.blog">Blog</a></li>
                    <li ui-sref-active="active"><a ui-sref="app.article">Article</a></li>
                    <li ui-sref-active="active"><a ui-sref="app.faq">FAQ</a></li>
                    <li ui-sref-active="active"><a ui-sref="app.timeline">Timeline</a></li>
                    <li ui-sref-active="active"><a ui-sref="app.pin_board">Pin board</a></li>
                    <li ui-sref-active="active"><a ui-sref="app.invoice">Invoice</a></li>
                </ul>
            </li>
            <li ng-class="{active: $state.includes('pages')}">
                <a href=""><i class="fa fa-files-o"></i> <span class="nav-label">{{ 'OTHERPAGES' }}</span><span class="fa arrow"></span></a>
                <ul class="nav nav-second-level collapse" ng-class="{in: $state.includes('pages')}">
                    <li ui-sref-active="active"><a ui-sref="pages.search_results">Search results</a></li>
                    <li><a ui-sref="lockscreen">Lockscreen</a></li>
                    <li><a ui-sref="errorOne">404 Page</a></li>
                    <li><a ui-sref="errorTwo">500 Page</a></li>
                    <li><a ui-sref="login">Login</a></li>
                    <li><a ui-sref="login_two_columns">Login v.2</a></li>
                    <li><a ui-sref="forgot_password">Forgot password</a></li>
                    <li><a ui-sref="register">Register</a></li>
                    <li ui-sref-active="active"><a ui-sref="pages.empy_page">Empty page</a></li>
                </ul>
            </li>
            <li ng-class="{active: $state.includes('miscellaneous')}">
                <a href=""><i class="fa fa-globe"></i> <span class="nav-label">{{ 'MISCELLANEOUS' }}</span> <span class="label label-info pull-right">NEW</span></a>
                <ul class="nav nav-second-level collapse" ng-class="{in: $state.includes('miscellaneous')}">
                    <li ui-sref-active="active"><a ui-sref="miscellaneous.notify">Notification</a></li>
                    <li ui-sref-active="active"><a ui-sref="miscellaneous.nestable_list">Nestable list</a></li>
                    <li ui-sref-active="active"><a ui-sref="miscellaneous.agile_board">Agile board</a></li>
                    <li ui-sref-active="active"><a ui-sref="miscellaneous.timeline_2">Timeline v.2</a></li>
                    <li ui-sref-active="active"><a ui-sref="miscellaneous.diff">Diff</a></li>
                    <li ui-sref-active="active"><a ui-sref="miscellaneous.sweet_alert">Sweet alert</a></li>
                    <li ui-sref-active="active"><a ui-sref="miscellaneous.idle_timer">Idle timer</a></li>
                    <li ui-sref-active="active"><a ui-sref="miscellaneous.spinners">Spinners</a></li>
                    <li ui-sref-active="active"><a ui-sref="miscellaneous.live_favicon">Live favicon</a></li>
                    <li ui-sref-active="active"><a ui-sref="miscellaneous.google_maps">Google maps</a></li>
                    <li ui-sref-active="active"><a ui-sref="miscellaneous.code_editor">Code editor</a></li>
                    <li ui-sref-active="active"><a ui-sref="miscellaneous.forum_view">Forum view</a></li>
                    <li ui-sref-active="active"><a ui-sref="miscellaneous.validation">Validation</a></li>
                    <li ui-sref-active="active"><a ui-sref="miscellaneous.modal_window">Modal window</a></li>
                    <li ui-sref-active="active"><a ui-sref="miscellaneous.chat_view">Chat view</a></li>
                    <li ui-sref-active="active"><a ui-sref="miscellaneous.masonry">Masonry</a></li>
                    <li ui-sref-active="active"><a ui-sref="miscellaneous.toastr">Toastr notification</a></li>
                </ul>
            </li>
            <li ng-class="{active: $state.includes('ui')}">
                <a href=""><i class="fa fa-flask"></i> <span class="nav-label">{{ 'UIELEMENTS' }}</span><span class="fa arrow"></span></a>
                <ul class="nav nav-second-level collapse" ng-class="{in: $state.includes('ui')}">
                    <li ui-sref-active="active"><a ui-sref="ui.typography">Typography</a></li>
                    <li ui-sref-active="active"><a ui-sref="ui.icons">Icons</a></li>
                    <li ui-sref-active="active"><a ui-sref="ui.draggable">Draggable Panels</a></li>
                    <li ui-sref-active="active"><a ui-sref="ui.buttons">Buttons</a></li>
                    <li ui-sref-active="active"><a ui-sref="ui.video">Video</a></li>
                    <li ui-sref-active="active"><a ui-sref="ui.tabs_panels">Panels</a></li>
                    <li ui-sref-active="active"><a ui-sref="ui.tabs">Tabs</a></li>
                    <li ui-sref-active="active"><a ui-sref="ui.notifications_tooltips">Notifications & Tooltips</a></li>
                    <li ui-sref-active="active"><a ui-sref="ui.badges_labels">Badges, Labels, Progress</a></li>
                </ul>
            </li>
            <li ui-sref-active="active">
                <a ui-sref="grid_options"><i class="fa fa-laptop"></i> <span class="nav-label">{{ 'GRIDOPTIONS' }}</span></a>
            </li>
            <li ng-class="{active: $state.includes('tables')}">
                <a href=""><i class="fa fa-table"></i> <span class="nav-label">{{ 'TABLES' }}</span><span class="fa arrow"></span></a>
                <ul class="nav nav-second-level collapse" ng-class="{in: $state.includes('tables')}">
                    <li ui-sref-active="active"><a ui-sref="tables.static_table">Static Tables</a></li>
                    <li ui-sref-active="active"><a ui-sref="tables.data_tables">Data Tables</a></li>
                    <li ui-sref-active="active"><a ui-sref="tables.foo_table">Foo Table</a></li>
                    <li ui-sref-active="active"><a ui-sref="tables.nggrid">NG Grid</a></li>
                </ul>
            </li>
            <li ng-class="{active: $state.includes('commerce')}">
                <a href="#"><i class="fa fa-shopping-cart"></i> <span class="nav-label">{{ 'COMMERCE' }}</span><span class="fa arrow"></span></a>
                <ul class="nav nav-second-level collapse" ng-class="{in: $state.includes('commerce')}">
                    <li ui-sref-active="active"><a ui-sref="commerce.products_grid">Products grid</a></li>
                    <li ui-sref-active="active"><a ui-sref="commerce.product_list">Products list</a></li>
                    <li ui-sref-active="active"><a ui-sref="commerce.product">Product edit</a></li>
                    <li ui-sref-active="active"><a ui-sref="commerce.product_details">Product detail</a></li>
                    <li ui-sref-active="active"><a ui-sref="commerce.orders">Orders</a></li>
                    <li ui-sref-active="active"><a ui-sref="commerce.payments">Credit Card form</a></li>
                </ul>
            </li>
            <li ng-class="{active: $state.includes('gallery')}">
                <a href=""><i class="fa fa-picture-o"></i> <span class="nav-label">{{ 'GALLERY' }}</span><span class="fa arrow"></span></a>
                <ul class="nav nav-second-level collapse" ng-class="{in: $state.includes('gallery')}">
                    <li ui-sref-active="active"><a ui-sref="gallery.basic_gallery">Lightbox Gallery</a></li>
                    <li ui-sref-active="active"><a ui-sref="gallery.slick_gallery">Slick Carousel</a></li>
                    <li ui-sref-active="active"><a ui-sref="gallery.bootstrap_carousel">Bootstrap Carousel</a></li>

                </ul>
            </li>
            <li>
                <a href=""><i class="fa fa-sitemap"></i> <span class="nav-label">{{ 'MENULEVELS' }}</span><span class="fa arrow"></span></a>
                <ul class="nav nav-second-level collapse">
                    <li>
                        <a href="">Third Level <span class="fa arrow"></span></a>
                        <ul class="nav nav-third-level">
                            <li>
                                <a href="">Third Level Item</a>
                            </li>
                            <li>
                                <a href="">Third Level Item</a>
                            </li>
                            <li>
                                <a href="">Third Level Item</a>
                            </li>

                        </ul>
                    </li>
                    <li><a href="">Second Level Item</a></li>
                    <li>
                        <a href="">Second Level Item</a></li>
                    <li>
                        <a href="">Second Level Item</a></li>
                </ul>
            </li>
            <li ui-sref-active="active">
                <a ui-sref="css_animations"><i class="fa fa-magic"></i> <span class="nav-label">{{ 'ANIMATIONS' }}</span><span class="label label-info pull-right">62</span></a>
            </li>
            <li class="landing_link">
                <a ui-sref="landing"><i class="fa fa-star"></i> <span class="nav-label">{{ 'LANDING' }}</span> <span class="label label-warning pull-right">NEW</span></a>
            </li>--%>
        </ul>

    </div>
</nav>