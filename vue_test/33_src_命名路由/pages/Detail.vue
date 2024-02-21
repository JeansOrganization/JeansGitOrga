<template>
    <ul>
        <li>消息ID：{{$route.query.id}}</li>
        <li>消息标题：{{$route.query.title}}</li>
    </ul>
</template>

<script>
    export default {
        name: 'Detail',
        updated(){
            console.log(this.$route)
        }
    };
</script>