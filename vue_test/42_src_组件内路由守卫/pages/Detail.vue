<template>
    <ul>
        <li>消息ID：{{id}}</li>
        <li>消息标题：{{title}}</li>
    </ul>
</template>

<script>
    export default {
        name: 'Detail',
        props:['id','title','a','b','c'],
        computed:{
            // id(){
            //     return this.$route.params.id
            // },
            // title(){
            //     return this.$route.params.title
            // }
        },
        updated(){
            console.log(this.$route,this)
        }
    };
</script>