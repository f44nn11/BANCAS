'use strict';

function uploadfileService($http, $q, localStorageService, ngAuthSettings) {
    

    var uploadfileServiceFactory = {};
    var _updfile = function (file, uploadUrl,param1) {
        var deferred = $q.defer();
        //var data = new FormData();
        //data.append('file', file);
        //data.append('param1', param1);

        //console.log(data);

        $http.post(uploadUrl,file, {
            transformRequest: angular.identity,
            //headers: {
            //    'Content-Type': undefined
            //}
        }).then(function (response) {
            conlog.log(response);
        }).catch(function (e) {
            conlog.log(e);
        }).finally(function () {
            //console.log('ini buat final');
        });
        return deferred.promise;
        //.success(function () {
        //    conlog.log("success upload");
        //})
        //.error(function () {
        //    conlog.log("Error upload");
        //});

        

    };

    uploadfileServiceFactory.updfile = _updfile;

    return uploadfileServiceFactory;
}


angular
    .module('WebApp')
    .factory('uploadfileService', uploadfileService)