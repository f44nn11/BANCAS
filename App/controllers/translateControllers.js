function translateCtrls($translate, $scope) {
    $scope.changeLanguage = function (langKey) {
        $translate.use(langKey);
    };
}

angular
    .module('WebApp')
    .controller('translateControllers', translateCtrls)