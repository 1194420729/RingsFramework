angular.module('myfilter', []);

angular.module('myfilter').filter('cost', function () {
    return function (input, show) {
        return show ? input : '****';
    };
});

angular.module('myfilter').filter('comment', function () {
    return function (input, maxlength) {
        if (!input) return '';
        if (input.length <= maxlength) return input;

        var sub = input.substring(0, maxlength) + '...';
        return sub;
    };
});