'use strict';


function loginControllers($scope, $location, $state, $timeout, authService, ngAuthSettings) {
    $scope.loginData = {
        userName: "",
        password: "",
        useRefreshTokens: false
    };

    $scope.message = "";
    
    $scope.login = function () {
        authService.login($scope.loginData).then(function (response) {
            //console.log(response);
            var status = response.data.status;
            if (status = '1') {
                
                $state.go('appmain');
            }
            //$location.path("/appmain"); // if status OK
            //
        },
         function (err) {
             //console.log(err.data.message);
             $scope.message = err.data.message;
         });
    };
}

angular
    .module('WebApp')
    .controller('loginControllers', loginControllers)
