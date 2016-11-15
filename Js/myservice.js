angular.module('myservice', []);

angular.module('myservice').service('mytools', function () {
    this.toFixedNumber = function (n, d) {
        return parseFloat(n.toFixed(d));
    };

    this.qtyChange = function (obj, digit) {
        if (obj.qty && obj.price !== undefined) {
            obj.total = this.toFixedNumber(obj.qty * obj.price, digit);
            obj.taxtotal = this.toFixedNumber(obj.total * (100 + obj.taxrate) / 100, digit);
            obj.taxprice = this.toFixedNumber(obj.price * (100 + obj.taxrate) / 100, digit);
            obj.discounttotal = this.toFixedNumber(obj.taxtotal * obj.discountrate / 100, digit);
            obj.discountprice = this.toFixedNumber(obj.taxprice * obj.discountrate / 100, digit);
        }
    };
     
    this.totalChange = function () {
        if (obj.qty && obj.total !== undefined) {
            obj.price = this.toFixedNumber(obj.total / obj.qty, digit);
            obj.taxtotal = this.toFixedNumber(obj.total * (100 + obj.taxrate) / 100, digit);
            obj.taxprice = this.toFixedNumber(obj.price * (100 + obj.taxrate) / 100, digit);
            obj.discounttotal = this.toFixedNumber(obj.taxtotal * obj.discountrate / 100, digit);
            obj.discountprice = this.toFixedNumber(obj.taxprice * obj.discountrate / 100, digit);
        }
    };

    this.taxpriceChange = function () {
        if (obj.qty && obj.taxprice !== undefined) {
            obj.price = this.toFixedNumber(obj.taxprice / (1 + obj.taxrate / 100), digit);
            obj.total = this.toFixedNumber(obj.qty * obj.price, digit);
            obj.taxtotal = this.toFixedNumber(obj.qty * obj.taxprice, digit);
            obj.discounttotal = this.toFixedNumber(obj.taxtotal * obj.discountrate / 100, digit);
            obj.discountprice = this.toFixedNumber(obj.taxprice * obj.discountrate / 100, digit);
        }
    };

    this.taxtotalChange = function () {
        if (obj.qty && obj.taxtotal !== undefined) {
            obj.taxprice = this.toFixedNumber(obj.taxtotal / obj.qty, digit);
            obj.price = this.toFixedNumber(obj.taxprice / (1 + obj.taxrate / 100), digit);
            obj.total = this.toFixedNumber(obj.qty * obj.price, digit);
            obj.discounttotal = this.toFixedNumber(obj.taxtotal * obj.discountrate / 100, digit);
            obj.discountprice = this.toFixedNumber(obj.taxprice * obj.discountrate / 100, digit);
        }
    };

    this.discountpriceChange = function () {
        if (obj.qty && obj.discountprice !== undefined) {
            obj.discounttotal = this.toFixedNumber(obj.discountprice * obj.qty, digit);
            obj.taxprice = this.toFixedNumber(obj.discountprice / obj.discountrate * 100, digit);
            obj.taxtotal = this.toFixedNumber(obj.taxprice * obj.qty, digit);
            obj.price = this.toFixedNumber(obj.taxprice / (1 + obj.taxrate / 100), digit);
            obj.total = this.toFixedNumber(obj.qty * obj.price, digit);
        }
    };

    this.discounttotalChange = function () {
        if (obj.qty && obj.discounttotal !== undefined) {
            obj.discountprice = this.toFixedNumber(obj.discounttotal / obj.qty, digit);
            obj.taxprice = this.toFixedNumber(obj.discountprice / obj.discountrate * 100, digit);
            obj.taxtotal = this.toFixedNumber(obj.taxprice * obj.qty, digit);
            obj.price = this.toFixedNumber(obj.taxprice / (1 + obj.taxrate / 100), digit);
            obj.total = this.toFixedNumber(obj.qty * obj.price, digit);
        }
    };

});