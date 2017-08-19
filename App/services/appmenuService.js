'use strict';


function appmenuService($http, $q, $timeout, localStorageService, ngAuthSettings) {    
    var serviceBase = ngAuthSettings.apiServiceAppmenu; //'./getappmenu/appmenu'
    
    var appmenuServiceFactory = {};
    var deferred = $q.defer();
    var _appmenu = function () {

        var datajson = { id: "1", name: "", url: "", parentid: "" };
        //if (loginData.useRefreshTokens) {
        //    data = data + "&client_id=" + ngAuthSettings.clientId;
        //}
        //console.log(datajson);
        $http.post("./getappmenu/appmenu", datajson).then(function (response) {
            //localStorageService.set('authorizationMenu', { data: response.data })
            deferred.resolve(response);
        }).catch(function (e) {
            //_logOut();
            deferred.reject(e);
        }).finally(function () {
            //console.log('ini buat final block');
        });
        return deferred.promise;

    };
    
    appmenuServiceFactory.appmenu = _appmenu;

    return appmenuServiceFactory;
}



angular
    .module('WebApp')
    .factory('appmenuService', appmenuService)