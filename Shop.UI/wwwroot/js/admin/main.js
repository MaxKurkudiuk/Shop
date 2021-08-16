var app = new Vue({
    el: '#app',
    data: {
        loading: false,
        products: []
    },
    methods: {
        getProducts() {
            this.loading = true;
            axios.get(url = '/Admin/products', {
                url: url,
                method: 'get',
            })
                .then(res => {
                    console.log(res);
                    this.products = res.data;
                })
                .catch(err => {
                    console.log(url);
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                });
        }
    },
    computed: {
    }
})