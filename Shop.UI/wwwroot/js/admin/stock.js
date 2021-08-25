var app = new Vue({
    el: '#app',
    data: {
        editing: false,
        loading: false,
        objectIndex: 0,
        stockModel: {
            id: 0,
            description: "Stock Description",
            qty: 0,
            productId: 0,
            product: {}
        },
        stocks: []
    },
    mounted() {  /*On first run (automatically)*/
        this.getStocks();
    },
    methods: {
        //getStock(id) {
        //    this.loading = true;
        //    axios.get('/Admin/stock/' + id)
        //        .then(res => {
        //            console.log(res);
        //            var stock = res.data;
        //            this.stockModel = {
        //                id: stock.id,
        //                description: stock.description,
        //                qty: product.qty
        //            };
        //        })
        //        .catch(err => {
        //            console.log(err);
        //        })
        //        .then(() => {
        //            this.loading = false;
        //        });
        //},
        //getProducts() {
        //    this.loading = true;
        //    axios.get('/Admin/stock')
        //        .then(res => {
        //            console.log(res);
        //            this.stock = res.data;
        //        })
        //        .catch(err => {
        //            console.log(err);
        //        })
        //        .then(() => {
        //            this.loading = false;
        //        });
        //},
        createStock() {
            this.loading = true;
            axios.post('/Admin/stock', this.stockModel)
                .then(res => {
                    console.log(res.data);
                    this.stocks.push(res.data);
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                    this.editing = false;
                });
        },
        //updateProduct() {
        //    this.loading = true;
        //    axios.put('/Admin/stock', this.productModel)
        //        .then(res => {
        //            console.log(res.data);
        //            this.products.splice(this.objectIndex, 1, res.data);
        //        })
        //        .catch(err => {
        //            console.log(err);
        //        })
        //        .then(() => {
        //            this.loading = false;
        //            this.editing = false;
        //        });
        //},
        //deleteProduct(id, index) {
        //    this.loading = true;
        //    axios.delete('/Admin/stock/' + id)
        //        .then(res => {
        //            console.log(res);
        //            this.products.splice(index, 1);
        //        })
        //        .catch(err => {
        //            console.log(err);
        //        })
        //        .then(() => {
        //            this.loading = false;
        //        });
        //},
        newStock() {
            this.editing = true;
            this.productModel.id = 0;
        },
        //editProduct(id, index) {
        //    this.objectIndex = index;
        //    this.getProduct(id);
        //    this.editing = true;
        //},
        cancel() {
            this.editing = false;
        }
    },
    computed: {
    }
})