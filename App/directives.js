function pageTitle($rootScope, $timeout) {
    return {
        link: function (scope, element) {
            var listener = function (event, toState, toParams, fromState, fromParams) {
                // Default title - load on Dashboard 1
                var title = 'AJK | WEBAPP';
                // Create your own title pattern
                if (toState.data && toState.data.pageTitle) title = 'AJK | ' + toState.data.pageTitle;
                $timeout(function () {
                    element.text(title);
                });
            };
            $rootScope.$on('$stateChangeStart', listener);
        }
    }
};
function sideNavigation($timeout) {
    return {
        restrict: 'A',
        link: function (scope, element) {
            // Call the metsiMenu plugin and plug it to sidebar navigation
            $timeout(function () {
                element.metisMenu();
            },450);
        }
    };
};

function responsiveVideo() {
    return {
        restrict: 'A',
        link: function (scope, element) {
            var figure = element;
            var video = element.children();
            video
                .attr('data-aspectRatio', video.height() / video.width())
                .removeAttr('height')
                .removeAttr('width')

            //We can use $watch on $window.innerWidth also.
            $(window).resize(function () {
                var newWidth = figure.width();
                video
                    .width(newWidth)
                    .height(newWidth * video.attr('data-aspectRatio'));
            }).resize();
        }
    }
}

/**
 * iboxTools - Directive for iBox tools elements in right corner of ibox
 */
function iboxTools($timeout) {
    return {
        restrict: 'A',
        scope: true,
        templateUrl: './App/views/common/ibox_tools.aspx',
        controller: function ($scope, $element) {
            // Function for collapse ibox
            $scope.showhide = function () {
                var ibox = $element.closest('div.ibox');
                var icon = $element.find('i:first');
                var content = ibox.find('div.ibox-content');
                content.slideToggle(200);
                // Toggle icon from up to down
                icon.toggleClass('fa-chevron-up').toggleClass('fa-chevron-down');
                ibox.toggleClass('').toggleClass('border-bottom');
                $timeout(function () {
                    ibox.resize();
                    ibox.find('[id^=map-]').resize();
                }, 50);
            };
            // Function for close ibox
            $scope.closebox = function () {
                var ibox = $element.closest('div.ibox');
                ibox.remove();
            }
        }
    };
}

/**
 * iboxTools with full screen - Directive for iBox tools elements in right corner of ibox with full screen option
 */
function iboxToolsFullScreen($timeout) {
    return {
        restrict: 'A',
        scope: true,
        templateUrl: './App/views/common/ibox_tools_full_screen.aspx',
        controller: function ($scope, $element) {
            // Function for collapse ibox
            $scope.showhide = function () {
                var ibox = $element.closest('div.ibox');
                var icon = $element.find('i:first');
                var content = ibox.find('div.ibox-content');
                content.slideToggle(200);
                // Toggle icon from up to down
                icon.toggleClass('fa-chevron-up').toggleClass('fa-chevron-down');
                ibox.toggleClass('').toggleClass('border-bottom');
                $timeout(function () {
                    ibox.resize();
                    ibox.find('[id^=map-]').resize();
                }, 50);
            };
            // Function for close ibox
            $scope.closebox = function () {
                var ibox = $element.closest('div.ibox');
                ibox.remove();
            };
            // Function for full screen
            $scope.fullscreen = function () {
                var ibox = $element.closest('div.ibox');
                var button = $element.find('i.fa-expand');
                $('body').toggleClass('fullscreen-ibox-mode');
                button.toggleClass('fa-expand').toggleClass('fa-compress');
                ibox.toggleClass('fullscreen');
                setTimeout(function () {
                    $(window).trigger('resize');
                }, 100);
            }
        }
    };
}

/**
 * minimalizaSidebar - Directive for minimalize sidebar
*/
function minimalizaSidebar($timeout) {
    return {
        restrict: 'A',
        template: '<a class="navbar-minimalize minimalize-styl-2 btn btn-primary " href="" ng-click="minimalize()"><i class="fa fa-bars"></i></a>',
        controller: function ($scope, $element) {
            $scope.minimalize = function () {
                $("body").toggleClass("mini-navbar");
                if (!$('body').hasClass('mini-navbar') || $('body').hasClass('body-small')) {
                    // Hide menu in order to smoothly turn on when maximize menu
                    $('#side-menu').hide();
                    // For smoothly turn on menu
                    setTimeout(
                        function () {
                            $('#side-menu').fadeIn(500);
                        }, 100);
                } else if ($('body').hasClass('fixed-sidebar')) {
                    $('#side-menu').hide();
                    setTimeout(
                        function () {
                            $('#side-menu').fadeIn(500);
                        }, 300);
                } else {
                    // Remove all inline style from jquery fadeIn function to reset menu state
                    $('#side-menu').removeAttr('style');
                }
            }
        }
    };
};


function closeOffCanvas() {
    return {
        restrict: 'A',
        template: '<a class="close-canvas-menu" ng-click="closeOffCanvas()"><i class="fa fa-times"></i></a>',
        controller: function ($scope, $element) {
            $scope.closeOffCanvas = function () {
                $("body").toggleClass("mini-navbar");
            }
        }
    };
}

/**
 * vectorMap - Directive for Vector map plugin
 */
function vectorMap() {
    return {
        restrict: 'A',
        scope: {
            myMapData: '=',
        },
        link: function (scope, element, attrs) {
            element.vectorMap({
                map: 'world_mill_en',
                backgroundColor: "transparent",
                regionStyle: {
                    initial: {
                        fill: '#e4e4e4',
                        "fill-opacity": 0.9,
                        stroke: 'none',
                        "stroke-width": 0,
                        "stroke-opacity": 0
                    }
                },
                series: {
                    regions: [
                        {
                            values: scope.myMapData,
                            scale: ["#1ab394", "#22d6b1"],
                            normalizeFunction: 'polynomial'
                        }
                    ]
                },
            });
        }
    }
}


/**
 * sparkline - Directive for Sparkline chart
 */
function sparkline() {
    return {
        restrict: 'A',
        scope: {
            sparkData: '=',
            sparkOptions: '=',
        },
        link: function (scope, element, attrs) {
            scope.$watch(scope.sparkData, function () {
                render();
            });
            scope.$watch(scope.sparkOptions, function () {
                render();
            });
            var render = function () {
                $(element).sparkline(scope.sparkData, scope.sparkOptions);
            };
        }
    }
};

/**
 * icheck - Directive for custom checkbox icheck
 */
function icheck($timeout) {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function ($scope, element, $attrs, ngModel) {
            return $timeout(function () {
                var value;
                value = $attrs['value'];

                $scope.$watch($attrs['ngModel'], function (newValue) {
                    $(element).iCheck('update');                    
                })

                return $(element).iCheck({
                    checkboxClass: 'icheckbox_square-green',
                    radioClass: 'iradio_square-green'

                }).on('ifChanged', function (event) {
                    if ($(element).attr('type') === 'checkbox' && $attrs['ngModel']) {
                        $scope.$apply(function () {
                            
                            return ngModel.$setViewValue(event.target.checked);
                        });
                    }
                    if ($(element).attr('type') === 'radio' && $attrs['ngModel']) {
                       
                        return $scope.$apply(function () {
                            return ngModel.$setViewValue(value);
                        });
                    }
                });
            });
        }
    };
}

/**
 * ionRangeSlider - Directive for Ion Range Slider
 */
function ionRangeSlider() {
    return {
        restrict: 'A',
        scope: {
            rangeOptions: '='
        },
        link: function (scope, elem, attrs) {
            elem.ionRangeSlider(scope.rangeOptions);
        }
    }
}

/**
 * dropZone - Directive for Drag and drop zone file upload plugin
 */
function dropZone() {
    return function (scope, element, attrs) {
        element.dropzone({
            url: "/upload",
            maxFilesize: 100,
            paramName: "uploadfile",
            maxThumbnailFilesize: 5,
            init: function () {
                scope.files.push({ file: 'added' });
                this.on('success', function (file, json) {

                });
                this.on('addedfile', function (file) {
                    scope.$apply(function () {
                        alert(file);
                        scope.files.push({ file: 'added' });
                    });
                });
                this.on('drop', function (file) {
                    alert('file');
                });
            }
        });
    }
}

/**
 * chatSlimScroll - Directive for slim scroll for small chat
 */
function chatSlimScroll($timeout) {
    return {
        restrict: 'A',
        link: function (scope, element) {
            $timeout(function () {
                element.slimscroll({
                    height: '234px',
                    railOpacity: 0.4
                });

            });
        }
    };
}

/**
 * customValid - Directive for custom validation example
 */
function customValid() {
    return {
        require: 'ngModel',
        link: function (scope, ele, attrs, c) {
            scope.$watch(attrs.ngModel, function () {

                // You can call a $http method here
                // Or create custom validation

                var validText = "Inspinia";

                if (scope.extras == validText) {
                    c.$setValidity('cvalid', true);
                } else {
                    c.$setValidity('cvalid', false);
                }

            });
        }
    }
}

/**
 * Password Complex - Directive for AddnewUSer
 */
function complexPassword() {
    return {
        require: 'ngModel',
        link: function (scope, ele, attrs, c) {
            scope.$watch(attrs.ngModel, function () {
                var hasUpperCase = /[A-Z]/.test(scope.datanewuser.password);
                var hasLowerCase = /[a-z]/.test(scope.datanewuser.password);
                var hasNumbers = /\d/.test(scope.datanewuser.password);
                var hasNonalphas = /\W/.test(scope.datanewuser.password);
                var characterGroupCount = hasUpperCase + hasLowerCase + hasNumbers + hasNonalphas;

                if (characterGroupCount >= 3) {
                    c.$setValidity('pcomplex', true);
                } else {
                    c.$setValidity('pcomplex', false);
                }

            });
        }
    }
}


/**
 * Password Match - Directive for AddnewUSer
 */
function passMatch() {
    return {
        require: 'ngModel',
        link: function (scope, ele, attrs, c) {
            scope.$watch(attrs.ngModel, function () {

                // You can call a $http method here
                // Or create custom validation

                var validText = "Inspinia";

                if (scope.datanewuser.password == scope.datanewuser.cpassword) {
                    c.$setValidity('cvalid', true);
                } else {
                    c.$setValidity('cvalid', false);
                }

            });
        }
    }
}

/**
 * Check Username - Directive for AddnewUSer
 */
function checkName($http) {
    return {
        require: 'ngModel',
        link: function (scope, ele, attrs, c) {
            scope.$watch(attrs.ngModel, function () {

                var datauser = {
                    "uname": scope.datanewuser.name,                    
                    "actiontype": "checkname"
                }
                $http.post("./datausersmanage/usermanage", datauser).then(function (response) {
                    if (response.data.status == "1") {                        
                        c.$setValidity('checknmvalid', true);
                    } else {
                        c.$setValidity('checknmvalid', false);
                    }
                    //deferred.resolve(response);

                }).catch(function (e) {
                    //_logOut();
                    //deferred.reject(e);
                }).finally(function () {
                    //console.log('ini buat final block');
                });

            });
        }
    }
}


//FileUpload

function fileModel($parse) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var model = $parse(attrs.fileModel);
            var modelSetter = model.assign;
            element.bind('change', function () {
                scope.$apply(function () {
                    //modelSetter(scope, element[0].files[0]); //singgle file
                    scope.datafileupd = [];
                    angular.forEach(element[0].files, function (value, key) {
                        scope.datafileupd.push({ FileName: value.name, FileLength: value.size });
                    })
                    modelSetter(scope, element[0].files); //multiple file
                });
            });
        }
    };
}
function fileReader($parse) {
    return {
        restrict: 'A',
        scope: {
            fileReader: "="
        },
        link: function (scope, element) {
            $(element).on('change', function (changeEvent) {
                var files = changeEvent.target.files;
                if (files.length) {
                    var r = new FileReader();
                    r.onload = function (e) {
                        var contents = e.target.result;
                        scope.$apply(function () {
                            scope.fileReader = contents;
                        });
                        xw(contents, process_wb);
                    };

                    r.readAsBinaryString(files[0]);
                }
            });
        }
    };
}


/**
 * fullScroll - Directive for slimScroll with 100%
 */
function fullScroll($timeout) {
    return {
        restrict: 'A',
        link: function (scope, element) {
            $timeout(function () {
                element.slimscroll({
                    height: '100%',
                    railOpacity: 0.9
                });

            });
        }
    };
}

/**
 * slimScroll - Directive for slimScroll with custom height
 */
function slimScroll($timeout) {
    return {
        restrict: 'A',
        scope: {
            boxHeight: '@'
        },
        link: function (scope, element) {
            $timeout(function () {
                element.slimscroll({
                    height: scope.boxHeight,
                    railOpacity: 0.9
                });

            });
        }
    };
}

/**
 * clockPicker - Directive for clock picker plugin
 */
function clockPicker() {
    return {
        restrict: 'A',
        link: function (scope, element) {
            element.clockpicker();
        }
    };
};


/**
 * landingScrollspy - Directive for scrollspy in landing page
 */
function landingScrollspy() {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            element.scrollspy({
                target: '.navbar-fixed-top',
                offset: 80
            });
        }
    }
}

function datatableSetup($timeout) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            $timeout(function () {
                element.dataTable();
            });
        }
    };
}

/**
 * fitHeight - Directive for set height fit to window height
 */
function fitHeight() {
    return {
        restrict: 'A',
        link: function (scope, element) {
            element.css("height", $(window).height() + "px");
            element.css("min-height", $(window).height() + "px");
        }
    };
}

angular
    .module('WebApp')
    .directive('pageTitle', pageTitle)
    .directive('sideNavigation', sideNavigation)
    .directive('iboxTools', iboxTools)
    .directive('minimalizaSidebar', minimalizaSidebar)
    .directive('vectorMap', vectorMap)
    .directive('sparkline', sparkline)
    .directive('icheck', icheck)
    .directive('ionRangeSlider', ionRangeSlider)
    .directive('dropZone', dropZone)
    .directive('responsiveVideo', responsiveVideo)
    .directive('chatSlimScroll', chatSlimScroll)
    .directive('customValid', customValid)
    .directive('fullScroll', fullScroll)
    .directive('closeOffCanvas', closeOffCanvas)
    .directive('clockPicker', clockPicker)
    .directive('landingScrollspy', landingScrollspy)
    .directive('fitHeight', fitHeight)
    .directive('iboxToolsFullScreen', iboxToolsFullScreen)
    .directive('checkName', checkName)
    .directive('complexPassword', complexPassword)
    .directive('passMatch', passMatch)
    .directive('datatableSetup', datatableSetup)
    .directive('fileReader', fileReader)
    .directive('fileModel', fileModel)
    .directive('slimScroll', slimScroll);