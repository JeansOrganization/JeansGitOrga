<template>
    <div class="demo">
        <h2 class="title">学校名：{{name}}</h2>
        <h2>学校地址：{{address}}</h2>
        <hr>
    </div>
</template>

<script>
    export default {
        name:'School',
        data() {
            return {
                name: '尚硅谷atguigu',
                address:'北京'
            };
        }
    }
</script>

<style scoped >
    .demo{
        background-color: orange;
    }
</style>