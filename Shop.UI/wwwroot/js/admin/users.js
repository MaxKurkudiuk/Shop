var app = new Vue({
    el: '#app',
    data: {
        username: ""
    },
    mounted() {  /*On first run (automatically)*/
        //this.getAllUsers();
    },
    methods: {
        createUser() {
            axios.post('/users', { UserName: this.username })
                .then(res => {
                    console.log(res);
                })
                .catch(err => {
                    console.log(err);
                });
        }
    }
})