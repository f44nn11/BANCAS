'use strict';

function userService($http, $q, $timeout, localStorageService, ngAuthSettings) {
    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var userServiceFactory = {};
    var deferred = $q.defer();
    var _user = function () {

        var datajson = { id: "", name: "", url: "", parentid: "" };
        //if (loginData.useRefreshTokens) {
        //    data = data + "&client_id=" + ngAuthSettings.clientId;
        //}

        var deferred = $q.defer();

        $http.get(serviceBase).then(function (response) {

            //localStorageService.set('authorizationMenu', { token: response.access_token, userName: loginData.userName, refreshToken: "", useRefreshTokens: false });
            //_authenticationappmenu.isAuthmenu = true;
            //_authentication.userName = loginData.userName;
            //_authentication.useRefreshTokens = loginData.useRefreshTokens;
            //deferred.resolve(response);
            //console.log(response);
            deferred.resolve(response);

        }).catch(function (e) {
            _logOut();
            deferred.reject(e);
        }).finally(function () {
            //console.log('ini buat final block');
        });
        return deferred.promise;

    };
    var _checknames = function (datauser) {

        var deferred = $q.defer();

        $http.post("./datausersmanage/usermanage", datauser).then(function (response) {
            //console.log(response);
            deferred.resolve(response);

        }).catch(function (e) {
            _logOut();
            deferred.reject(e);
        }).finally(function () {
            //console.log('ini buat final block');
        });
        return deferred.promise;

    };
    var _saveaddnewuser = function (datauser) {
        var deferred = $q.defer();
        $http.post("./datausersmanage/usermanage", datauser).then(function (response) {
            deferred.resolve(response);
        }).catch(function (e) {
            _logOut();
            deferred.reject(e);
        }).finally(function () {
            //console.log('ini buat final block');
        });
        return deferred.promise;

    };
    var _savecopyuser = function (datauser) {
        var deferred = $q.defer();
        $http.post("./datausersmanage/usermanage", datauser).then(function (response) {
            deferred.resolve(response);
        }).catch(function (e) {
            _logOut();
            deferred.reject(e);
        }).finally(function () {
            //console.log('ini buat final block');
        });
        return deferred.promise;

    };
    var _saveedituser = function (datauser) {

        var datajson = { id: "", name: "", url: "", parentid: "" };
        var deferred = $q.defer();

        $http.post("./datausersmanage/usermanage", datauser).then(function (response) {
            deferred.resolve(response);
        }).catch(function (e) {
            _logOut();
            deferred.reject(e);
        }).finally(function () {
            //console.log('ini buat final block');
        });
        return deferred.promise;

    };
    var _getmodule = function (datauser) {

        deferred = $q.defer();

        $http.post('./getdatamodule/module', datauser).then(function (response) {
            //$timeout(function () {
                deferred.resolve(response);
            //}, 1000);
           
        }).catch(function (e) {
            _logOut();
            deferred.reject(e);
        }).finally(function () {
            //console.log('ini buat final block');
        });
        return deferred.promise;

    };
    var _getmoduleopr = function (datauser) {

        var deferred = $q.defer();

        $http.post('./getdatamodule/moduleopr', datauser).then(function (response) {
            deferred.resolve(response);
        }).catch(function (e) {
            _logOut();
            deferred.reject(e);
        }).finally(function () {
            //console.log('ini buat final block');
        });
        return deferred.promise;

    };
    var _getmoduleoprupdinstdel = function (datauser) {

        var deferred = $q.defer();

        $http.post('./getdatamodule/moduleopr', datauser).then(function (response) {
            deferred.resolve(response);
        }).catch(function (e) {
            _logOut();
            deferred.reject(e);
        }).finally(function () {
            //console.log('ini buat final block');
        });
        return deferred.promise;

    };
    var _getmodulenew = function (datauser) {

        var deferred = $q.defer();

        $http.post('./getdatamodule/module', datauser).then(function (response) {
            deferred.resolve(response);
        }).catch(function (e) {
            _logOut();
            deferred.reject(e);
        }).finally(function () {
            //console.log('ini buat final block');
        });
        return deferred.promise;

    };


    userServiceFactory.user = _user;
    userServiceFactory.checknames = _checknames;
    userServiceFactory.saveaddnewuser = _saveaddnewuser;
    userServiceFactory.savecopyuser = _savecopyuser;
    userServiceFactory.saveedituser = _saveedituser;
    userServiceFactory.getmodule = _getmodule;
    userServiceFactory.getmoduleopr = _getmoduleopr;
    userServiceFactory.getmoduleoprupdinstdel = _getmoduleoprupdinstdel;
    userServiceFactory.getmodulenew = _getmodulenew;
    return userServiceFactory;
}


angular
    .module('WebApp')
    .factory('userService', userService)