<template>
    <ul>
        <li>消息ID：{{$route.params.id}}</li>
        <li>消息标题：{{$route.params.title}}</li>
    </ul>
</template>

<script>
    export default {
        name: 'Detail',
        updated(){
            console.log(this.$route)
        },
        mounted(){
            console.log(this.$route)
        }
    };
</script>