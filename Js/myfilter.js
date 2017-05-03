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
		else if(input==='gatheringbill') return '收款单';
		else if(input==='paybill') return '付款单';
		else if(input==='receivablebill') return '应收调整单';
		else if(input==='payablebill') return '应付调整单';
		else if(input==='stockinbill') return '入库单';
		else if(input==='stockoutbill') return '出库单';
		else if(input==='stockmovebill') return '调拨单';
		else if(input==='stockinventorybill') return '盘点单';
		else if(input==='feebill') return '费用单';
		else if(input==='earningbill') return '收入单';
		else if(input==='transferbill') return '转款单';
		else if(input==='saleinvoicebill') return '销售发票';
		else if(input==='purchaseinvoicebill') return '采购发票';
		else return input;
    };
});