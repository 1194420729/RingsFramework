angular.module("ngTableBindHtml", []);
angular.module("ngTableBindHtml").directive("myBindCompiledHtml", function() {
    var directive = {
        restrict: "A",
        controller: function($scope, $element, $attrs, $compile) {
            $scope.$watch($attrs.myBindCompiledHtml, compileHtml);

            function compileHtml(html) {
                //debugger;
                var compiledElements = $compile(html)($scope);
                $element.append(compiledElements);
            }
        }
    };
    return directive;
});
