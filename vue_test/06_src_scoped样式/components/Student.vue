<template>
    <div class="demo">
        <h2 class="title">学生名：{{name}}</h2>
        <h2 class="jean">学生性别：{{sex}}</h2>
        <hr>
    </div>
</template>

<script>
    export default {
        name:'Student',
        data() {
            return {
                name:'张三',
                sex:'男'
            };
        }
    }
</script>

<style lang="less" >
    .demo{
        background-color: pink;
        .jean{
            font-size: 40px;
        }
    }
</style>