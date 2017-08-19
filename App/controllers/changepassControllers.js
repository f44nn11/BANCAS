'use strict';

function changepassControllers($scope, $location, authService) {
    $scope.authentication = authService.authentication;
    if (!$scope.authentication.isAuth) {
        $location.path('/login');
    };
    console.log("disini");
    $scope.logOut = function () {
        authService.logOut();
        $location.path('/login');
    };

}


angular
    .module('WebApp')
    .controller('changepassControllers', changepassControllers)

