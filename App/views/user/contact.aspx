<%@ Page Language="VB" AutoEventWireup="false" CodeFile="contact.aspx.vb" Inherits="App_views_user_contact" %>

<div class="wrapper wrapper-content animated fadeIn">
    <div class="row">
        <div class="col-lg-12">
            <div class="ibox float-e-margins">
                <div class="ibox-title">
                    <h5>Dropzone Area</h5>

                    <div ibox-tools></div>
                </div>
                <div class="ibox-content">
                    <%--<js-xlsx onread="read" onerror="error"></js-xlsx>--%>
                    <form class="form-horizontal">
                        <div class="form-group">
                            <label for="title" class="col-md-2 control-label">Param1</label>
                            <div class="col-md-10">
                                <input type="text" ng-model="tutorial.param1" name="param1" class="form-control" />
                            </div>
                            <label for="title" class="col-md-2 control-label">Param2</label>
                            <div class="col-md-10">
                                <input type="text" ng-model="tutorial.param2" name="param2" class="form-control" />
                            </div>
                        </div> 
                        <div class="form-group">
                            <label for="attachment" class="col-md-2 control-label">Attachment</label>
                            <div class="col-md-10">
                                <input type="file" name="attachment" accept=".csv,application/vnd.ms-excel.sheet.macroenabled.12, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel" class="form-control" file-model="tutorial.attachment" multiple="multiple" />
                            </div>
                            <label for="attachment" class="col-md-2 control-label">Attachment</label>
                            <div class="col-md-10">
                                <input type="file" name="read" accept=".csv,application/vnd.ms-excel.sheet.macroenabled.12, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel" class="form-control" file-reader="fileContent" />
                            </div>
                            <%--<div id="divoutput"></div>--%>
                            <%--<label id="divoutput" onchange="dataupd"></label>  --%>
                            <div ng-app="WebApp" ng-controller="contactControllers"  ng-init="getMember(memId)">
                              Your output: {{memId}}
                            </div>  
                            <div ng-repeat="file in datafileupd">
                            {{file.FileName  + '  ' + file.FileLength + ' bytes'  }} 
<%--                            <div ng-controller="contactControllers">
                                <js-xls onread="read" onerror="error"></js-xls>
                            </div>--%>
<%--                            <div class="col-md-10">
                                <input type="file" name="reader" js-xls onread="read" onerror="error" accept=".csv,application/vnd.ms-excel.sheet.macroenabled.12, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel" class="form-control" />
                            </div>    --%>     

                        </div>
                        </div>
                        
                          <div class="form-group">
                                <div class="col-md-offset-2 col-md-10">
                                    <input type="button" class="btn btn-primary" 

                                    value="Save" ng-click="saveTutorial(tutorial)" />
                                </div>
                            </div>
                    </form>
                </div>
            </div>
        </div>
    </div>

</div>
