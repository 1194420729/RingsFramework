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

angular.module('myfilter').filter('billname', function () {
    return function (input) {
        if (!input) return '';
		if(input==='purchaseorder') return '采购订单';
		else if(input==='purchasebill') return '采购入库单';
		else if(input==='purchasebackbill') return '采购退货单';
		else if(input==='saleorder') return '销售订单';
		else if(input==='salebill') return '销售出库单';
		else if(input==='salebackbill') return '销售退货单';
		else return input;
    };
});