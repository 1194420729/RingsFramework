angular.module("ngDateScope", []);
angular.module("ngDateScope").directive("ngDateScope", function ($parse) {
    var template = '<a href="javascript:;" ng-click="datescopeClick(\'today\')">今天</a>' +
            '<a style="margin-left:10px;" href="javascript:;" ng-click="datescopeClick(\'month\')">本月</a>' +
            '<a style="margin-left:10px;" href="javascript:;" ng-click="datescopeClick(\'lastmonth\')">上月</a>' +
            '<a style="margin-left:10px;" href="javascript:;" ng-click="datescopeClick(\'lastthreemonth\')">前3个月</a>' +
            '<a style="margin-left:10px;" href="javascript:;" ng-click="datescopeClick(\'lastyear\')">上一年</a>';
    var directive = {
        restrict: 'A',
        template: template,
        link: function (scope, element, attrs) {
            scope.datescopeClick = function (name) {
                if (name == 'today') {
                    $parse(attrs.startmodel).assign(scope, moment().format('YYYY-MM-DD'));
                    $parse(attrs.endmodel).assign(scope, moment().format('YYYY-MM-DD'));
                } else if (name == 'month') {
                    $parse(attrs.startmodel).assign(scope, moment().startOf('month').format('YYYY-MM-DD'));
                    $parse(attrs.endmodel).assign(scope, moment().endOf('month').format('YYYY-MM-DD'));
                } else if (name == 'lastmonth') {
                    var lastmonth = moment().subtract(1,'months');
                    $parse(attrs.startmodel).assign(scope, lastmonth.startOf('month').format('YYYY-MM-DD'));
                    $parse(attrs.endmodel).assign(scope, lastmonth.endOf('month').format('YYYY-MM-DD'));
                } else if (name == 'lastthreemonth') {
                    var startmonth = moment().subtract(3, 'months');
                    var endmonth = moment().subtract(1, 'months');
                    $parse(attrs.startmodel).assign(scope, startmonth.startOf('month').format('YYYY-MM-DD'));
                    $parse(attrs.endmodel).assign(scope, endmonth.endOf('month').format('YYYY-MM-DD'));
                } else if (name == 'lastyear') {
                    var lastyear = moment().subtract(1, 'years');
                    $parse(attrs.startmodel).assign(scope, lastyear.startOf('year').format('YYYY-MM-DD'));
                    $parse(attrs.endmodel).assign(scope, lastyear.endOf('year').format('YYYY-MM-DD'));
                }
            };
        }
    };
    return directive;
});
