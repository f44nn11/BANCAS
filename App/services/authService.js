'use strict';
//app.factory('authService', ['$http', '$q', 'localStorageService', 'ngAuthSettings', function ($http, $q, localStorageService, ngAuthSettings) {

//    var serviceBase = ngAuthSettings.apiServiceBaseUri;
//    var authServiceFactory = {};

//    var _authentication = {
//        isAuth: false,
//        userName: "",
//        useRefreshTokens: false
//    };

//    var _externalAuthData = {
//        provider: "",
//        userName: "",
//        externalAccessToken: ""
//    };

//    var _saveRegistration = function (registration) {

//        _logOut();

//        return $http.post(serviceBase + 'api/account/register', registration).then(function (response) {
//            return response;
//        });

//    };

//    var _login = function (loginData) {

//        //var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;
//        var data = { uname: loginData.userName, pass: loginData.password };
//        if (loginData.useRefreshTokens) {
//            data = data + "&client_id=" + ngAuthSettings.clientId;
//        }

//        var deferred = $q.defer();

//        $http.post(serviceBase, data, { headers: { 'Content-Type': 'application/json' } }).then(function (response) {

//            if (loginData.useRefreshTokens) {
//                localStorageService.set('authorizationData', { token: response.access_token, userName: loginData.userName, refreshToken: response.refresh_token, useRefreshTokens: true });
//            }
//            else { //if status OK
//                localStorageService.set('authorizationData', { token: response.access_token, userName: loginData.userName, refreshToken: "", useRefreshTokens: false });
//            }
//            _authentication.isAuth = true;
//            _authentication.userName = loginData.userName;
//            _authentication.useRefreshTokens = loginData.useRefreshTokens;

//            deferred.resolve(response);
        
//        }).catch(function (e) {
//            _logOut();
//            deferred.reject(e);
//        }).finally(function () {
//            //console.log('ini buat final block');
//        });

//        return deferred.promise;

//    };

//    var _logOut = function () {

//        localStorageService.remove('authorizationData');

//        _authentication.isAuth = false;
//        _authentication.userName = "";
//        _authentication.useRefreshTokens = false;

//    };

//    var _fillAuthData = function () {
        
//        var authData = localStorageService.get('authorizationData');
//        //console.log(authData);
//        if (authData) {
//            _authentication.isAuth = true;
//            _authentication.userName = authData.userName;
//            _authentication.useRefreshTokens = authData.useRefreshTokens;
//        }
//    };

//    var _refreshToken = function () {
//        var deferred = $q.defer();

//        var authData = localStorageService.get('authorizationData');

//        if (authData) {

//            if (authData.useRefreshTokens) {

//                var data = "grant_type=refresh_token&refresh_token=" + authData.refreshToken + "&client_id=" + ngAuthSettings.clientId;

//                localStorageService.remove('authorizationData');

//                $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

//                    localStorageService.set('authorizationData', { token: response.access_token, userName: response.userName, refreshToken: response.refresh_token, useRefreshTokens: true });

//                    deferred.resolve(response);

//                }).error(function (err, status) {
//                    _logOut();
//                    deferred.reject(err);
//                });
//            }
//        }

//        return deferred.promise;
//    };

//    var _obtainAccessToken = function (externalData) {

//        var deferred = $q.defer();

//        $http.get(serviceBase + 'api/account/ObtainLocalAccessToken', { params: { provider: externalData.provider, externalAccessToken: externalData.externalAccessToken } }).success(function (response) {

//            localStorageService.set('authorizationData', { token: response.access_token, userName: response.userName, refreshToken: "", useRefreshTokens: false });

//            _authentication.isAuth = true;
//            _authentication.userName = response.userName;
//            _authentication.useRefreshTokens = false;

//            deferred.resolve(response);

//        }).error(function (err, status) {
//            _logOut();
//            deferred.reject(err);
//        });

//        return deferred.promise;

//    };

//    var _registerExternal = function (registerExternalData) {

//        var deferred = $q.defer();

//        $http.post(serviceBase + 'api/account/registerexternal', registerExternalData).success(function (response) {

//            localStorageService.set('authorizationData', { token: response.access_token, userName: response.userName, refreshToken: "", useRefreshTokens: false });

//            _authentication.isAuth = true;
//            _authentication.userName = response.userName;
//            _authentication.useRefreshTokens = false;

//            deferred.resolve(response);

//        }).error(function (err, status) {
//            _logOut();
//            deferred.reject(err);
//        });

//        return deferred.promise;

//    };

//    authServiceFactory.saveRegistration = _saveRegistration;
//    authServiceFactory.login = _login;
//    authServiceFactory.logOut = _logOut;
//    authServiceFactory.fillAuthData = _fillAuthData;
//    authServiceFactory.authentication = _authentication;
//    authServiceFactory.refreshToken = _refreshToken;

//    authServiceFactory.obtainAccessToken = _obtainAccessToken;
//    authServiceFactory.externalAuthData = _externalAuthData;
//    authServiceFactory.registerExternal = _registerExternal;

//    return authServiceFactory;
//}]);

function authService($http, $q, localStorageService, ngAuthSettings) {
    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var authServiceFactory = {};
    
    var _authentication = {
        isAuth: false,
        uid:"",
        userName: "",
        groupnm:"",
        useRefreshTokens: false
    };
    var _authenticationappmenu = {
        isAuthmenu: false,
    };

    var _externalAuthData = {
        provider: "",
        userName: "",
        externalAccessToken: ""
    };

    var _saveRegistration = function (registration) {

        _logOut();

        return $http.post(serviceBase + 'api/account/register', registration).then(function (response) {
            return response;
        });

    };
    
    var _login = function (loginData) {
        
        //var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;
        var data = { uname: loginData.userName, pass: loginData.password };
        if (loginData.useRefreshTokens) {
            data = data + "&client_id=" + ngAuthSettings.clientId;
        }

        var deferred = $q.defer();

        $http.post(serviceBase, data, { headers: { 'Content-Type': 'application/json' } }).then(function (response) {
            deferred.resolve(response);
            //var uid = response.data.uid;
            //var groupnm = response.data.ugroupnm;
            localStorageService.set('authorizationData', { token: response.access_token,uid:response.data.uid,groupnm:response.data.ugroupnm,userName: loginData.userName, refreshToken: "", useRefreshTokens: false });

            _authentication.isAuth = true;
            _authentication.uid = response.data.uid;
            _authentication.userName = loginData.userName;
            _authentication.groupnm = response.data.ugroupnm;
            _authentication.useRefreshTokens = loginData.useRefreshTokens;
            
            console.log(response.data);
        }).catch(function (e) {
            _logOut();
            deferred.reject(e);
        }).finally(function () {
            //console.log('ini buat final block');
        });

        return deferred.promise;

    };

    var _logOut = function () {

        localStorageService.remove('authorizationData');
        localStorageService.remove('authorizationMenu');
        //console.log("disini");
        _authentication.isAuth = false;
        _authentication.userName = "";
        _authentication.uid = "";
        _authentication.groupnm = "";
        _authentication.useRefreshTokens = false;

    };

    var _fillAuthData = function () {

        var authData = localStorageService.get('authorizationData');
        //console.log(authData);
        if (authData) {
            _authentication.isAuth = true;
            _authentication.userName = authData.userName;
            _authentication.uid = authData.uid;
            _authentication.groupnm = authData.groupnm;
            _authentication.useRefreshTokens = authData.useRefreshTokens;
        }
    };





    var _refreshToken = function () {
        var deferred = $q.defer();

        var authData = localStorageService.get('authorizationData');

        if (authData) {

            if (authData.useRefreshTokens) {

                var data = "grant_type=refresh_token&refresh_token=" + authData.refreshToken + "&client_id=" + ngAuthSettings.clientId;

                localStorageService.remove('authorizationData');

                $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

                    localStorageService.set('authorizationData', { token: response.access_token, userName: response.userName, refreshToken: response.refresh_token, useRefreshTokens: true });

                    deferred.resolve(response);

                }).error(function (err, status) {
                    _logOut();
                    deferred.reject(err);
                });
            }
        }

        return deferred.promise;
    };

    var _obtainAccessToken = function (externalData) {

        var deferred = $q.defer();

        $http.get(serviceBase + 'api/account/ObtainLocalAccessToken', { params: { provider: externalData.provider, externalAccessToken: externalData.externalAccessToken } }).success(function (response) {

            localStorageService.set('authorizationData', { token: response.access_token, userName: response.userName, refreshToken: "", useRefreshTokens: false });

            _authentication.isAuth = true;
            _authentication.userName = response.userName;
            _authentication.useRefreshTokens = false;

            deferred.resolve(response);

        }).error(function (err, status) {
            _logOut();
            deferred.reject(err);
        });

        return deferred.promise;

    };

    var _registerExternal = function (registerExternalData) {

        var deferred = $q.defer();

        $http.post(serviceBase + 'api/account/registerexternal', registerExternalData).success(function (response) {

            localStorageService.set('authorizationData', { token: response.access_token, userName: response.userName, refreshToken: "", useRefreshTokens: false });

            _authentication.isAuth = true;
            _authentication.userName = response.userName;
            _authentication.useRefreshTokens = false;

            deferred.resolve(response);

        }).error(function (err, status) {
            _logOut();
            deferred.reject(err);
        });

        return deferred.promise;

    };

    authServiceFactory.saveRegistration = _saveRegistration;
    authServiceFactory.login = _login;
    authServiceFactory.logOut = _logOut;
    authServiceFactory.fillAuthData = _fillAuthData;
    authServiceFactory.authentication = _authentication;
    authServiceFactory.refreshToken = _refreshToken;

    authServiceFactory.obtainAccessToken = _obtainAccessToken;
    authServiceFactory.externalAuthData = _externalAuthData;
    authServiceFactory.registerExternal = _registerExternal;

    return authServiceFactory;
}

angular
    .module('WebApp')
    .factory('authService', authService)