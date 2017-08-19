
function contactControllers($scope,$http, $location, $timeout,$window, localStorageService, authService, uploadfileService) {
    $scope.authentication = authService.authentication;
    
    if (!$scope.authentication.isAuth) {
        $location.path('/login');
    }
    //$scope.read = function (workbook) {
    //    /* DO SOMETHING WITH workbook HERE */
    //    for (var sheetName in workbook.Sheets) {
    //        var jsonData = XLSX.utils.sheet_to_json(workbook.Sheets[sheetName]);
    //        console.log(jsonData);
    //    }
    //}

    //$scope.error = function (e) {
    //    /* DO SOMETHING WHEN ERROR IS THROWN */
    //    console.log(e);
    //}
    $scope.memId = $window.memId;
    $scope.getMember = function (id) {
        console.log(id);
    };
    //$scope.dataupd = {};

    //console.log($scope.dataupd);

    $scope.datafileupd = [];


    $scope.saveTutorial = function (data) {
        //var file = $scope.myFile;
        //var file = $scope.myFile;

        //console.log('file is ');
        //console.log(JSON.stringify(data));

        var uploadUrl = "./uploadfile/upload";
        //uploadfileService.updfile(data, uploadUrl, "12345");
        var datas = new FormData();
        
        //data.append('file', $scope.tutorial.attachment);
        
        angular.forEach(data, function (value, key) {
            if (key == "attachment") {
                for (var i = 0; i < value.length; i++) {
                    
                    datas.append(value[i].name, value[i]);
                    //$scope.datafileupd.push({ FileName: value[i].name, FileLength: value[i].size });
                }
            } else {
                datas.append(key, value);
            }
        });
        
        $http.post(uploadUrl, datas, {
            transformRequest: angular.identity,
            headers: {
                'Content-Type': undefined
            }
        }).then(function (response) {
            conlog.log(response);
        }).catch(function (e) {
            conlog.log(e);
        }).finally(function () {
            //console.log('ini buat final');
        });
        //return deferred.promise;
    }


    $scope.logOut = function () {
        authService.logOut();
        $location.path('/login');
    };

}

angular
    .module('WebApp')
    .controller('MainCtrl', MainCtrl)
    .controller('contactControllers', contactControllers)