(function (angular) {
    'use strict';

    if (!angular) {
        throw 'Missing something? Please add angular.js to your project or move this script below the angular.js reference';
    }

    function ngNextDirective() {
        var obj = {
            restrict: 'A',
            link: function (scope, element, attr) {
                if (attr.ngNext==='true') {
                    var focusables = angular.element('input');
                    element.bind('keydown', function (e) {
                        var code = e.keyCode || e.which; 
                        if (code === 13) { 
                            e.preventDefault();
                            var current = focusables.index(this);
                            var nextinput = focusables.eq(current + 1).length ? focusables.eq(current + 1) : focusables.eq(0);
                            //console.log(angular.toJson(nextinput));
                            nextinput.focus();
                        }
                    });
                }
            }
        };

        return obj;
    }

    //// Angular Code ////
    var mi = angular.module('ngNext', []);
    mi.directive('ngNext', ngNextDirective);

})(angular);
