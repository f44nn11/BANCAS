
var app = angular.module('WebApp', ['ui.router', 'LocalStorageModule', 'angular-loading-bar', 'oc.lazyLoad', 'ui.bootstrap', 'ngIdle', 'ngSanitize', 'angularUtils.directives.dirPagination', 'uiBreadcrumbs']);

var serviceBase = './getdatausers/users';
var serviceMenu = './getappmenu/appmenu';
var serviceGroupid = './getdatagroupid/groupid';
app.constant('ngAuthSettings', {
    apiServiceBaseUri: serviceBase,
    apiServiceAppmenu: serviceMenu,
    apiServiceGroupid: serviceGroupid,
    clientId: 'ngAuthApp'
});

app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
    
});
app.config(['$qProvider', function ($qProvider) {
    $qProvider.errorOnUnhandledRejections(false);
}]);

//app.run(function ($rootScope, $state) {
//    $rootScope.$state = $state;
//});

app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);



