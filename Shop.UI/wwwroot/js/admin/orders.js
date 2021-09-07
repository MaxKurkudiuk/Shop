﻿var app = new Vue({
    el: '#app',
    data: {
        status: 0,
        loading: false,
        orders: [],
        selectedOrder: null
    },
    mounted() {
        this.getOrders();
    },
    watch: {
        status: function () {
            this.getOrders();
        }
    },
    methods: {
        getOrders() {
            this.loading = true;
            axios.get('/Admin/orders?status=' + this.status)
                .then(result => {
                    console.log(result.data);
                    this.orders = result.data;
                    this.loading = false;
                });
        },
        selectOrder(id) {
            this.loading = true;
            axios.get('/Admin/orders/' + id)
                .then(result => {
                    this.selectedOrder = result.data;
                    this.loading = false;
                });
        },
        updateOrder() {
            this.loading = true;
            axios.put('/Admin/orders/' + this.selectedOrder.id, null)
                .then(result => {
                    this.loading = false;
                    this.exitOrder();
                    this.getOrders();
                });
        },
        exitOrder() {
            this.selectedOrder = null;
        }
    },
    computed: {

    }
})