<%@ Page Language="VB" AutoEventWireup="false" CodeFile="usermanagement.aspx.vb" Inherits="App_views_usermanagement" %>
<toaster-container></toaster-container>
<%--<div class="row wrapper border-bottom white-bg page-heading" ng-controller="appmenuControllers">
    <div class="col-lg-10">
        <h2>{{SiteMenu[0].NAME}}</h2>
        <ui-breadcrumbs displayname-property="data.displayName" abstract-proxy-property="data.proxy" template-url="./App/views/common/uiBreadcrumbs.tpl.html"></ui-breadcrumbs>
        <%--<ol class="breadcrumb">
            <%--<li>
                <a ui-sref="appmain">{{active}}</a>
            </li>
            <li>
                <a>Tables</a>
            </li>
            <li class="active">
                <strong>Data Tables</strong>
            </li>
            <li ng-repeat="menu in SiteMenu" ng-if="menu.NAME === true" ng-class="{active:active}">
                <a ui-sref="{{menu.URL}}"> <span class="nav-label">{{ menu.NAME }}</span></a>
            </li>
        </ol>
    </div>
    <div class="col-lg-2">
        <%--<button class="btn btn-primary" ng-click="demo1()">Success</button>
    </div>
</div>--%>

<div class="wrapper wrapper-content animated fadeInRight" ng-show="showhide.topmodule">
<div class="row">
<div class="col-lg-12">
<div class="ibox float-e-margins">
<div class="ibox-title">
    <h5><strong>Application of User Authorization</strong> <small> {{title}} </small></h5>
    
    <div ibox-tools></div>
</div>
<%--<div class="ibox-title" >
    <div class="btn-group" style="margin-top:-10px">
    <button type="button" class="btn btn-default btn" tooltip-placement="top" tooltip="Edit" ng-click="editmodule();"><i class="glyphicon glyphicon-pencil"></i></button>  
    <button type="button" class="btn btn-default btn" tooltip-placement="top" tooltip="Delete" ng-click="deletemodule();"><i class="glyphicon glyphicon-trash"></i></button> 
    </div>
</div>--%>  
<div class="ibox-content" ng-show="showhide.alluser">
    <div class="btn-group" style="margin-top:-10px">
        <button type="button" class="btn btn-default btn" tooltip-placement="top" tooltip="AddNew User" ng-click="addnewuser();"><i class="fa fa-plus-square"></i></button>  
    </div>
    <table id="tableuser"  datatable="ng" class="table table-striped table-bordered table-hover dataTables-example">
        <thead>
<%--        <tr>
            <td align="right">Search :</td>
            <td><input ng-model="query" /></td>
        </tr>           
        <tr>
            <td align="right">Search By :</td>
            <td>
                <select ng-model="queryBy">
                    <option value="" selected>ALL</option>
                    <option value="UID">ID</option>
                    <option value="UNAME">NAME</option>
                    <option value="UGROUPID">GROUPID</option>
                </select>   
            </td>
        </tr>--%>
        <tr>
            <%--<th>SEQ</th>--%>
            <th>UID</th>
            <th>UNAME</th>
            <th>UGROUPID</th>
            <th>ACTION</th>
        </tr>
        </thead>
        <tbody>
        <tr style="cursor:pointer;" ng-repeat="user in Alluser | filter:queryFilter track by $index" ng-class="{'selected':$index == selectedRow}" ng-click="setClickedRow($index)">
            <%--<td>{{$index + 1}}</td>--%>
            <td>{{ user.UID }}</td>
            <td>{{ user.UNAME }}</td>
            <td>{{ user.UGROUPID }}</td>
            <td>
                <div class="btn-group">
                    <button type="button" class="btn btn-default btn" tooltip-placement="top" tooltip="Module" ng-click="module(user.UID+','+user.UNAME);"><i class="fa fa-list-alt"></i></button> 
                    <button type="button" class="btn btn-default btn" tooltip-placement="top" tooltip="Edit" ng-click="edit(user.UID+','+user.UNAME+','+user.UGROUPID);"><i class="glyphicon glyphicon-pencil"></i></button>  
                    <button type="button" class="btn btn-default btn" tooltip-placement="top" tooltip="Delete" ng-click="delete($index);"><i class="glyphicon glyphicon-trash"></i></button> 
                </div>
            </td>
        </tr>
        </tbody>
    </table>


</div>
<div class="ibox-content" ng-show="showhide.addnewuser">
    <form role="form" name="frm_addnewuser" novalidate class="form-horizontal" ng-submit="saveAddnewuser()">
        <div class="form-group">
            <label class="col-sm-2 control-label">Id</label>
            <div class="col-sm-10"><input type="text" class="form-control" ng-model="datanewuser.id" disabled="disabled"/></div>
        </div>
        <div class="form-group">
            <label class="col-sm-2 control-label">Name</label>
            <div class="col-sm-10">
                <input type="text" class="form-control" name="name" ng-model="datanewuser.name" ng-minlength="4" ng-maxlength="20" required="" check-Name/>
                <div class="m-t-xs" ng-show="frm_addnewuser.name.$invalid && frm_addnewuser.submitted">
                      <small class="text-danger" ng-show="frm_addnewuser.name.$error.required">Please input a name</small>
                      <small class="text-danger" ng-show="frm_addnewuser.name.$error.minlength">Your name is required to be at least 4 characters</small>
                      <small class="text-danger" ng-show="frm_addnewuser.name.$error.maxlength">Your name cannot be longer than 20 characters</small>
                      <small class="text-danger" ng-show="frm_addnewuser.name.$error.checknmvalid">Name already exist</small>
                </div>
            </div>
        </div>
        <div class="form-group">
            <label class="col-sm-2 control-label">Group Name</label>
            <div class="col-sm-10">
                <select class="form-control" name="ugroupid" ng-model="datanewuser.ugroupid" required="">
                     <option ng-repeat="row in Groupid" value="{{row.UGroupId}}">{{row.UGroupNm}} </option>
                </select>
                <div class="m-t-xs" ng-show="frm_addnewuser.ugroupid.$invalid && frm_addnewuser.submitted">
                      <small class="text-danger" ng-show="frm_addnewuser.ugroupid.$error.required">Please choose group name</small>
                </div>
            </div>
        </div>
        <div class="form-group">
            <label class="col-sm-2 control-label">Password</label>
            <div class="col-sm-10">
                <input type="{{showPassword?'text':'password'}}" class="form-control" name="password" ng-model="datanewuser.password" ng-minlength="6" ng-maxlength="20" required="" complex-Password/>
                <div class="m-t-xs" ng-show="frm_addnewuser.password.$invalid && frm_addnewuser.submitted">
                      <small class="text-danger" ng-show="frm_addnewuser.password.$error.required">Please input a password</small>
                      <small class="text-danger" ng-show="frm_addnewuser.password.$error.minlength">Your name is required to be at least 6 characters</small>
                      <small class="text-danger" ng-show="frm_addnewuser.password.$error.maxlength">Your name cannot be longer than 20 characters</small>
                      <small class="text-danger" ng-show="frm_addnewuser.password.$error.pcomplex">Password has Upper(A),Lower(a),Number(1),Alphas(@)</small>
                </div>
            </div>
        </div>
        <div class="form-group">
            <label class="col-sm-2 control-label">Verify</label>
            <div class="col-sm-10">
                <input type="password" class="form-control" name="cpassword" ng-model="datanewuser.cpassword" required="" pass-Match/>
                <div class="m-t-xs" ng-show="frm_addnewuser.cpassword.$invalid && frm_addnewuser.submitted">
                      <small class="text-danger" ng-show="frm_addnewuser.cpassword.$error.cvalid">Passwords do not match</small>
                </div>
            </div>
        </div>
        <div class="form-group">
            <label class="col-sm-2 control-label"></label>
            <div class="col-sm-10"><input icheck type="checkbox" title="Show Password" ng-model="showPassword" ng-checked="showPassword"/><label class="checkbox-inline">Show Password</label></div>
        </div>
        <div class="hr-line-dashed"></div>
        <div class="form-group">
            <div class="col-sm-4 col-sm-offset-2">
                  <button class="btn btn-white" type="submit" ng-click="cancelAddnewuser()">Cancel</button>              
                  <button class="btn btn-primary" type="submit" >Save changes</button>              
            </div>
        </div>                                
    </form>
</div>
<div class="ibox-content" ng-show="showhide.edit">
    <form method="get" class="form-horizontal">
        <div class="form-group">
            <label class="col-sm-2 control-label">Id</label>
            <div class="col-sm-10"><input type="text" class="form-control" ng-model="data.id" disabled></div>
        </div>
        <div class="form-group">
            <label class="col-sm-2 control-label">Name</label>
            <div class="col-sm-10"><input type="text" class="form-control" ng-model="data.name"></div>
        </div>
        <div class="form-group">
            <label class="col-sm-2 control-label">Group Name</label>
            <div class="col-sm-10">
                <select class="form-control" ng-model="data.ugroupid">
                     <option ng-repeat="row in Groupid" value="{{row.UGroupId}}">{{row.UGroupNm}}</option>
                </select>
            </div>
        </div>
        <div class="hr-line-dashed"></div>
        <div class="form-group">
            <div class="col-sm-4 col-sm-offset-2">
                  <button class="btn btn-white" type="submit" ng-click="cancel()">Cancel</button>              
                  <button class="btn btn-primary" type="submit" ng-click="saveedit()">Save changes</button>              
            </div>
        </div>                                
    </form>
</div>
<div class="ibox-content" ng-show="showhide.module">
    <div class="btn-group" style="margin-top:-10px">
        <button type="button" class="btn btn-default btn" tooltip-placement="top" tooltip="Add New Module" ng-click="addnewmodule();"><i class="fa fa-plus-square"></i></button>  
        <button type="button" class="btn btn-default btn" tooltip-placement="top" tooltip="Copy Authorization from User" ng-click="copymodule();"><i class="fa fa-user"></i></button>
        <button type="button" class="btn btn-default btn" tooltip-placement="top" tooltip="Undo Process" ng-click="undomodule();"><i class="fa fa-undo"></i></button>        
    </div>    
    <form method="get" class="form-horizontal">
        <div class="form-group">
            <label class="col-sm-2 control-label">User Name</label>
            <div class="col-sm-10"><input type="text" class="form-control" ng-model="data.name" disabled="disabled"></div>
        </div>
        <div class="hr-line-dashed"></div>
        <div class="form-group">
            <div class="col-sm-12"><input type="text" style="width:50%" ng-model="search" class="form-control" placeholder="Search"/></div>
            <div class="col-sm-12"><label style="text-align:left" class="col-sm-3 control-label">Page : {{ currentPage }}</label></div>
        </div>                           
    </form>
    <table class="table table-striped table-bordered table-hover">
        <thead>
        <tr>
            <th ng-click="sort3('APPREGNO')">APPREGNO
                <span style="margin-left:5px" class="fa sort-icon" ng-show="sortKey=='APPREGNO'" ng-class="{'fa-angle-up':reverse,'fa-angle-down':!reverse}"></span>
            </th>
            <th ng-click="sort3('APPDESC')">MODULENAME
                <span style="margin-left:5px" class="fa sort-icon" ng-show="sortKey=='APPDESC'" ng-class="{'fa-angle-up':reverse,'fa-angle-down':!reverse}"></span>
            </th>
            <th>ACTION</th>
        </tr>
        </thead>
        <tbody>
        <tr style="cursor:pointer;" pagination-id="moduleusers" dir-paginate="modules in Allmodule|orderBy:sortKey3:reverse3|filter:search|itemsPerPage:10" current-page="currentPage3">
            <td>{{ modules.APPREGNO }}</td>
            <td>{{ modules.APPDESC }}</td>
            <td>
                <div class="btn-group" ><%--ng-show="show"--%>
                    <button type="button" class="btn btn-default btn" tooltip-placement="top" tooltip="Edit" ng-click="editmodule(modules.APPREGNO+','+modules.APPDESC+','+modules.ENTRYFORM);"><i class="glyphicon glyphicon-pencil"></i></button>  
                    <button type="button" class="btn btn-default btn" tooltip-placement="top" tooltip="Delete" ng-click="deletemodule();"><i class="glyphicon glyphicon-trash"></i></button> 
                </div>
            </td>
        </tr>
        </tbody>
    </table>
    <div class="form-group" ng-show="Allmodule.length > 0">
        <label class="col-sm-3 control-label" style="margin-top:-5px">Total : {{Allmodule.length}}</label>
        <div class="col-sm-9" style="text-align:right;margin-top:-30px">
             <dir-pagination-controls
                pagination-id="moduleusers"
                max-size="6"
                direction-links="true"
                boundary-links="true" >
             </dir-pagination-controls>
        </div>
     </div>
</div>

<div class="ibox-content" ng-show="showhide.copymodule">
    <form role="form" name="frm_copyuser" novalidate class="form-horizontal" ng-submit="savecopyuser()">
        <div class="form-group">
            <label class="col-sm-2 control-label">Username</label>
            <div class="col-sm-10"><input type="text" class="form-control" ng-model="datacopyuser.name" disabled="disabled"/></div>
        </div>
        <div class="form-group">
            <label class="col-sm-2 control-label">Copy from user</label>
            <div class="col-sm-10">
                <div class="input-group">
                    <select chosen="{'placeholder_text_single': 'Chose username'}" class="chosen-select" name="fromname" style="width:350px;" ng-model="fromname" ng-options="user.UNAME for user in Alluser" required="">
                        <%--<option value="" selected="selected">Choose user</option>--%>
                    </select>
                    <div class="m-t-xs" ng-show="frm_copyuser.fromname.$invalid && frm_copyuser.submitted">
                      <small class="text-danger" ng-show="frm_copyuser.fromname.$error.required">Please input a name</small>
                    </div>
                </div>
            </div>
        </div>
        <div class="form-group">
            <label class="col-sm-2 control-label">Operation</label>
            <div class="col-sm-10">
                <div class="radio radio-info radio-inline">
                    <input type="radio" id="operationrb1" value="copy" ng-model="datacopyuser.rbopr" name="rboperation" required=""/>
                    <label for="operationrb1"> Some copy </label>
                </div>
                <div class="radio radio-inline">
                    <input type="radio" id="operationrb2" value="append" ng-model="datacopyuser.rbopr" name="rboperation" required=""/>
                    <label for="operationrb2"> Append </label>
                </div>
                <div class="m-t-xs" ng-show="frm_copyuser.rboperation.$invalid && frm_copyuser.submitted">
                      <small class="text-danger" ng-show="frm_copyuser.rboperation.$error.required">Please choose operation</small>
                </div>
            </div>
        </div>
        <div class="hr-line-dashed"></div>
        <div class="form-group">
            <div class="col-sm-4 col-sm-offset-2">
                  <button class="btn btn-white" type="button" ng-click="cancelcopyuser()">Cancel</button>              
                  <button class="btn btn-primary" type="submit" >Save</button>              
            </div>
        </div>                                
    </form>
</div>
<div class="ibox-content" ng-show="showhide.editmodul">
    <form method="get" class="form-horizontal">
        <div class="form-group">
            <label class="col-sm-2 control-label">User Name</label>
            <div class="col-sm-10">
                <input type="text" class="form-control" ng-model="data.name"/>
                <input type="text" class="form-control" ng-model="data.uid" style="display:none"/></div>
        </div>
        <div class="form-group">
            <label class="col-sm-2 control-label">Module Name</label>
            <div class="col-sm-10">
                <input type="text" class="form-control" ng-model="data.modulenm"/>
                <input type="text" class="form-control" ng-model="data.appregno" style="display:none"/>
            </div>
        </div>
        <div class="form-group">
            <label class="col-sm-2 control-label">Operation</label>
            <div class="col-sm-10">
                <label class="checkbox-inline"> <input icheck type="checkbox" ng-model="data.datachk.chkview" disabled/>View</label>
                <label class="checkbox-inline" ng-show="showchk.chk"> <input icheck type="checkbox"  ng-model="data.datachk.chkadd"/>Add</label>
                <label class="checkbox-inline" ng-show="showchk.chk"> <input icheck type="checkbox"  ng-model="data.datachk.chkedit"/>Edit</label>
                <label class="checkbox-inline" ng-show="showchk.chk"> <input icheck type="checkbox"  ng-model="data.datachk.chkdelete"/>Delete</label>
                <label class="checkbox-inline" ng-show="showchk.chk"> <input icheck type="checkbox"  ng-model="data.datachk.chkprint"/>Print</label>   
            </div>
        </div>
        <div class="hr-line-dashed"></div>
        <div class="form-group">
            <div class="col-sm-4 col-sm-offset-2">
                  <button class="btn btn-white" type="submit" ng-click="canceleditmodule()">Cancel</button>              
                  <button class="btn btn-primary" type="submit" ng-click="saveeditmodule(data)">Save changes</button>              
            </div>
        </div>                                
    </form>
</div>
</div>
</div>
</div>
</div>
<div class="wrapper wrapper-content animated fadeInRight" ng-show="showhide.addnewmodule">
<div class="row">
    <div class="col-lg-6">
        <div class="ibox float-e-margins">
            <div class="ibox-title">
                <h5>Add New Module</h5>
                <div ibox-tools></div>
            </div>
            <div class="ibox-content">
                <div class="btn-group" style="margin-top:-10px">
                    <button type="button" class="btn btn-default btn" tooltip-placement="top" tooltip="Save Data" ng-click="saveadd();"><i class="fa fa-save"></i></button>  
                    <button type="button" class="btn btn-default btn" tooltip-placement="top" tooltip="Undo Process" ng-click="undosaveadd();"><i class="fa fa-undo"></i></button> 
                </div>
                <form method="get" class="form-horizontal">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">User Name</label>
                        <div class="col-sm-9"><input type="text" class="form-control" ng-model="data.name" disabled/></div>
                    </div>
                    <div class="hr-line-dashed"></div>
                    <div class="form-group">
                        <%--<label class="col-sm-2 control-label">Search</label>--%>
                        <div class="col-sm-12"><input type="text" style="width:50%" ng-model="search" class="form-control" placeholder="Search"/></div>
                        <div class="col-sm-12"><label style="text-align:left" class="col-sm-3 control-label">Page : {{ currentPage }}</label></div>
                    </div>                           
                </form>
                <%--<input type="text" class="form-control input-sm m-b-xs" id="filter"
                           placeholder="Search in table"/>--%>
                <table class="table table-striped table-bordered table-hover" >
                    <thead style="cursor:pointer;">
                    <tr> <%--ng-click="sort('isChecked')"--%>
                        <th ><input icheck type="checkbox" ng-model="model.allItemsSelected" ng-change="selectAll()"/>
                            <span style="margin-left:2px" class="fa sort-icon" ng-show="sortKey=='isChecked'" ng-class="{'fa-angle-up':reverse,'fa-angle-down':!reverse}"></span>
                        </th>
                        <th ng-click="sort('APPREGNO')">APPREGNO
                            <span style="margin-left:5px" class="fa sort-icon" ng-show="sortKey=='APPREGNO'" ng-class="{'fa-angle-up':reverse,'fa-angle-down':!reverse}"></span>
                        </th>
                        <th ng-click="sort('APPDESC')">MODULENAME
                            <span style="margin-left:5px" class="fa sort-icon" ng-show="sortKey=='APPDESC'" ng-class="{'fa-angle-up':reverse,'fa-angle-down':!reverse}"></span>
                        </th>
                    </tr>
                    </thead>
                    <tbody>
                    <tr style="cursor:pointer;" pagination-id="moduleuser" dir-paginate="modules in Allmodulenew|orderBy:sortKey:reverse|filter:search|itemsPerPage:15" current-page="currentPage">
                        <td><input icheck type="checkbox" ng-model="modules.isChecked"  ng-change="selectEntity(modules)"/></td><%--ng-change="toggleChange"--%>
                        <td>{{ modules.APPREGNO }}</td>
                        <td>{{ modules.APPDESC }}</td>
                    </tr>
                    </tbody>
                </table>
                <div class="form-group"  ng-show="Allmodulenew.length > 0">
                    <label class="col-sm-3 control-label" style="margin-top:-5px">Total : {{Allmodulenew.length}}</label>
                    <div class="col-sm-9" style="text-align:right;margin-top:-30px">
                        <dir-pagination-controls
                             pagination-id="moduleuser"
                             max-size="6"
                             direction-links="true"
                             boundary-links="true" >
                        </dir-pagination-controls>
                    </div>
                </div>                                
            </div>
        </div>
    </div>
    <div class="col-lg-6">
        <div class="ibox float-e-margins">
            <div class="ibox-title">
                <h5>Authorization User</h5>
                <div ibox-tools></div>
            </div>
            <div class="ibox-content">
                <table class="table table-striped table-bordered table-hover checkbox checkbox-primary">
                    <thead style="cursor:pointer;">
                    <tr>
                        <th ng-click="sort2('APPREGNO')">APPREGNO</th>
                        <th ng-click="sort2('APPDESC')">MODULENAME</th>
                        <th>ADD</th>
                        <th>EDIT</th>
                        <th>DEL</th>
                        <th>PRINT</th>
                    </tr>
                    </thead>
                    <tbody>
                    <tr pagination-id="authuser" dir-paginate="auth in AuthUser|orderBy:sortKey2:reverse2|itemsPerPage:15" current-page="currentPage2">
                        <td>{{auth.APPREGNO}}</td>
                        <td>{{auth.APPDESC}}</td>
                        <td><input id="chkadd{{$index}}" type="checkbox" class="checkbox checkbox-warning" ng-show="showchkuth.chk"  ng-model="auth.chkAdd"/><label style="margin-left:25px" for="chkadd{{$index}}"></label></td>
                        <td><input id="chkedit{{$index}}"  type="checkbox" class="checkbox checkbox-warning" ng-show="showchkuth.chk"  ng-model="auth.chkEdit" /><label style="margin-left:25px" for="chkedit{{$index}}"></label></td>
                        <td><input id="chkdel{{$index}}"  type="checkbox" class="checkbox checkbox-warning" ng-show="showchkuth.chk"  ng-model="auth.chkDel" /><label style="margin-left:25px" for="chkdel{{$index}}"></label></td>
                        <td><input id="chkprint{{$index}}"  type="checkbox" class="checkbox checkbox-warning" ng-show="showchkuth.chk"  ng-model="auth.chkPrint" /><label style="margin-left:25px" for="chkprint{{$index}}"></label></td>                        
                    </tr>
                    </tbody>
                </table>
                <div class="form-group"  ng-show="AuthUser.length > 0">
                    <label class="col-sm-3 control-label" style="margin-top:-5px">Total : {{AuthUser.length}}</label>
                    <div class="col-sm-9" style="text-align:right;margin-top:-30px">
                        <dir-pagination-controls
                             pagination-id="authuser"
                             max-size="6"
                             direction-links="true"
                             boundary-links="true" >
                        </dir-pagination-controls>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
</div>

