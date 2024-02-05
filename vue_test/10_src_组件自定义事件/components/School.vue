<template>
    <div class="school">
        <h2>学校名：{{name}}</h2>
        <h2>学校地址：{{address}}</h2>
        <button @click="sendSchoolName">发送学校名给APP</button>
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
        },
        methods:{
            sendSchoolName(){
                this.getSchoolName(this.name)
            }
        },
        props:['getSchoolName'],
    }
</script>

<style scoped >
    .school{
        background-color: orange;
        padding: 10px;
        margin-top: 10px;
    }
</style>