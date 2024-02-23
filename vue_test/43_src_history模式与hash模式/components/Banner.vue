<template>
    <div class="col-xs-offset-2 col-xs-8">
        <div class="page-header"><h2>Vue Router Demo</h2></div>
        <button @click="back">后退</button>
        <button @click="forward">前进</button>
        <button @click="go(2)">前进2</button>
    </div>
</template>

<script>
    export default {
        name: 'Banner',
        methods:{
            back(){
                this.$router.back()
            },
            forward(){
                this.$router.forward()
            },
            go(n){
                //根据输入数字表示前进几次或者后退几次 -2后退两次  2前进两次
                this.$router.go(n)
            }
        }
    };
</script>