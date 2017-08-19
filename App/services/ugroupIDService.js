'use strict';


function ugroupIDService($http, $q, localStorageService, ngAuthSettings) {
    var serviceBase = ngAuthSettings.apiServiceGroupid;

    var ugroupIDServiceFactory = {};
    var deferred = $q.defer();
    var _ugroupID = function () {

        $http.get(serviceBase).then(function (response) {
            deferred.resolve(response);

        }).catch(function (e) {
            _logOut();
            deferred.reject(e);
        }).finally(function () {
            //console.log('ini buat final block');
        });
        return deferred.promise;

    };

    ugroupIDServiceFactory.ugroupID = _ugroupID;

    return ugroupIDServiceFactory;
}

angular
    .module('WebApp')
    .factory('ugroupIDService', ugroupIDService)