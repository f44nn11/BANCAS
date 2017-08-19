﻿<%@ Page Language="VB" AutoEventWireup="false" CodeFile="layout.aspx.vb" Inherits="App_views_layout" %>

<!-- Wrapper-->
<div id="wrapper">

    <!-- Navigation -->
    <div ng-include="'./App/views/common/navigation.aspx'"></div>

    <!-- Page wraper -->
    <!-- ng-class with current state name give you the ability to extended customization your view -->
    <div id="page-wrapper" class="gray-bg {{$state.current.name}}">

        <!-- Page wrapper -->
        <div ng-include="'./App/views/common/topnavbar.aspx'"></div>

        <!-- Main view  -->
        <div class="row wrapper border-bottom white-bg page-heading">
            <div class="col-lg-10">
                <h2>Layouts</h2>
                <ol class="breadcrumb">
                    <li>
                        <a ui-sref="appmain">Home</a>
                    </li>
                    <li class="active">
                        <strong>Layouts</strong>
                    </li>
                </ol>
            </div>
            <div class="col-lg-2">

            </div>
        </div>
        <div class="wrapper wrapper-content animated fadeInRight">
            <div class="row">
                <div class="col-lg-12">
                    <div class="ibox float-e-margins">
                        <div class="ibox-content text-center p-md">

                            <h2><span class="text-navy">INSPINIA - Responsive Admin Theme</span>
                                is provided with two main layouts <br/>three skins and separate configure options.</h2>

                            <p>
                                All config options you can trun on/off from the theme box configuration (green icon on the left side of page).
                            </p>


                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-6">
                    <div class="ibox float-e-margins">
                        <div class="ibox-content text-center p-md">

                            <h4 class="m-b-xxs">Top navigation, centered content layout <span class="label label-primary">NEW</span></h4>
                            <small>(optional layout)</small>
                            <p>Avalible configure options</p>
                            <input type="hidden" ng-model="rptData.Uname" id="txtuname" ng-init="rptData.Uname = '4'"/>
                            <input type="hidden" ng-model="rptData.Uid" id="txtuid" ng-init="rptData.Uid = '1'"/>
                            <input type="hidden" ng-model="rptData.Opt" id="txtopt" ng-init="rptData.Opt = 'A'"/>
                            <span class="simple_tag"><a href="" ng-click="report()">Report1</a></span>
                            <span class="simple_tag"><a href="" ng-click="report2()">Report2</a></span>
                            <span class="simple_tag">Boxed layout</span>
                            <span class="simple_tag">Scroll footer</span>
                            <span class="simple_tag">Fixed footer</span>
                            <div class="m-t-md">
                                <p>Check the Dashboard v.4 with top navigation layout</p>
                                <div class="p-lg ">
                                    <a ui-sref="dashboards_top.dashboard_4"><img class="img-responsive img-shadow" src="img/dashbard4_2.jpg" alt=""></a>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>

                <div class="col-lg-6">
                    <div class="ibox float-e-margins">
                        <div class="ibox-content text-center p-md">

                            <h4 class="m-b-xxs">Basci left side nabigation layout </h4><small>(main layout)</small>
                            <p>Avalible configure options</p>
                            <span class="simple_tag">Collapse menu</span>
                            <span class="simple_tag">Fixed sidebar</span>
                            <span class="simple_tag">Scroll navbar</span>
                            <span class="simple_tag">Top fixed navbar</span>
                            <span class="simple_tag">Boxed layout</span>
                            <span class="simple_tag">Scroll footer</span>
                            <span class="simple_tag">Fixed footer</span>
                            <div class="m-t-md">
                                <p>Check the Dashboard v.4 with basic layout</p>
                                <div class="p-lg">
                                    <a ui-sref="dashboards.dashboard_4_1"><img class="img-responsive img-shadow" src="img/dashbard4_1.jpg" alt=""></a>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>


            </div>
            <div class="row">
                <div class="col-lg-6">
                    <div class="ibox float-e-margins">
                        <div class="ibox-content text-center p-md">

                            <h4 class="m-b-xxs">Full height - Outlook view <span class="label label-primary">NEW</span></h4>
                            <small>(optional layout)</small>
                            <p>Avalible configure options</p>
                            <span class="simple_tag">Scroll navbar</span>
                            <span class="simple_tag">Boxed layout</span>
                            <span class="simple_tag">Scroll footer</span>
                            <span class="simple_tag">Fixed footer</span>
                            <div class="m-t-md">
                                <p>Check the Outlook view in in full height page</p>
                                <div class="p-lg ">
                                    <a ui-sref="outlook"><img class="img-responsive img-shadow" src="img/full_height.jpg" alt=""></a>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>

                <div class="col-lg-6">
                    <div class="ibox float-e-margins">
                        <div class="ibox-content text-center p-md">

                            <h4 class="m-b-xxs">Off canvas menu <span class="label label-primary">NEW</span></h4>
                            <small>(optional layout)</small>
                            <p>Avalible configure options</p>
                            <span class="simple_tag">Collapse menu</span>
                            <span class="simple_tag">Fixed sidebar</span>
                            <span class="simple_tag">Top fixed navbar</span>
                            <span class="simple_tag">Boxed layout</span>
                            <span class="simple_tag">Scroll footer</span>
                            <span class="simple_tag">Fixed footer</span>
                            <div class="m-t-md">
                                <p>Check the off canvas menu on example article page</p>
                                <div class="p-lg">
                                    <a ui-sref="off_canvas"><img class="img-responsive img-shadow" src="img/off_canvas.jpg" alt=""></a>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>

        </div>

        <!-- Footer -->
        <div ng-include="'./App/views/common/footer.aspx'"></div>

    </div>
    <!-- End page wrapper-->

</div>
<!-- End wrapper-->