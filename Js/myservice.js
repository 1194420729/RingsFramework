angular.module('myservice', []);

angular.module('myservice').service('mytools', function () {
    this.toFixedNumber = function (n, d) {
        return parseFloat(n.toFixed(d));
    };

    this.qtyChange = function (obj, digit) {
        if (obj.qty && obj.price !== undefined && obj.price !== null) {
            obj.total = this.toFixedNumber(obj.qty * obj.price, digit);
            obj.taxprice = this.toFixedNumber(obj.price * (100 + obj.taxrate) / 100, digit);
            obj.taxtotal = this.toFixedNumber(obj.taxprice * obj.qty, digit);
            obj.discounttotal = this.toFixedNumber(obj.taxtotal * obj.discountrate / 100, digit);
            obj.discountprice = this.toFixedNumber(obj.taxprice * obj.discountrate / 100, digit);
        }
    };

    this.totalChange = function (obj, digit) {
        if (obj.total === undefined || obj.total === null) return;

        obj.taxtotal = this.toFixedNumber(obj.total * (100 + obj.taxrate) / 100, digit);
        obj.discounttotal = this.toFixedNumber(obj.taxtotal * obj.discountrate / 100, digit);
        if (obj.qty) {
            obj.price = this.toFixedNumber(obj.total / obj.qty, digit);
            obj.taxprice = this.toFixedNumber(obj.price * (100 + obj.taxrate) / 100, digit);
            obj.discountprice = this.toFixedNumber(obj.taxprice * obj.discountrate / 100, digit);
        }
    };

    this.taxpriceChange = function (obj, digit) {
        if (obj.taxprice === undefined || obj.taxprice === null) return;

        obj.price = this.toFixedNumber(obj.taxprice / (1 + obj.taxrate / 100), digit);
        obj.discountprice = this.toFixedNumber(obj.taxprice * obj.discountrate / 100, digit);
        if (obj.qty) {
            obj.total = this.toFixedNumber(obj.qty * obj.price, digit);
            obj.taxtotal = this.toFixedNumber(obj.qty * obj.taxprice, digit);
            obj.discounttotal = this.toFixedNumber(obj.taxtotal * obj.discountrate / 100, digit);
        }
    };

    this.taxtotalChange = function (obj, digit) {
        if (obj.taxtotal === undefined || obj.taxtotal === null) return;

        obj.total = this.toFixedNumber(obj.taxtotal / (1 + obj.taxrate / 100), digit);
        obj.discounttotal = this.toFixedNumber(obj.taxtotal * obj.discountrate / 100, digit);
        if (obj.qty) {
            obj.taxprice = this.toFixedNumber(obj.taxtotal / obj.qty, digit);
            obj.price = this.toFixedNumber(obj.taxprice / (1 + obj.taxrate / 100), digit);
            obj.discountprice = this.toFixedNumber(obj.taxprice * obj.discountrate / 100, digit);
        }
    };

    this.discountpriceChange = function (obj, digit) {
        if (obj.discountprice === undefined || obj.discountprice === null) return;

        obj.taxprice = this.toFixedNumber(obj.discountprice / obj.discountrate * 100, digit);
        obj.price = this.toFixedNumber(obj.taxprice / (1 + obj.taxrate / 100), digit);
        if (obj.qty) {
            obj.discounttotal = this.toFixedNumber(obj.discountprice * obj.qty, digit);
            obj.taxtotal = this.toFixedNumber(obj.taxprice * obj.qty, digit);
            obj.total = this.toFixedNumber(obj.qty * obj.price, digit);
        }
    };

    this.discounttotalChange = function (obj, digit) {
        if (obj.discounttotal === undefined || obj.discounttotal === null) return;

        obj.taxtotal = this.toFixedNumber(obj.discounttotal / obj.discountrate * 100, digit);
        obj.total = this.toFixedNumber(obj.taxtotal / (1 + obj.taxrate / 100), digit);
        if (obj.qty) {
            obj.discountprice = this.toFixedNumber(obj.discounttotal / obj.qty, digit);
            obj.taxprice = this.toFixedNumber(obj.discountprice / obj.discountrate * 100, digit);
            obj.price = this.toFixedNumber(obj.taxprice / (1 + obj.taxrate / 100), digit);
        }
    };

    this.checkpositivenumeric = function (num) {
        if (num === undefined || num === null || num < 0) {
            return false;
        }

        return true;
    };

});