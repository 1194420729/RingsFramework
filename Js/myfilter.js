angular.module('myfilter', []);

angular.module('myfilter').filter('cost', function () {
    return function (input, show) {
        return show ? input : '****';
    };
});