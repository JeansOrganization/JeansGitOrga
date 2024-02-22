<template>
    <ul>
        <li>news001</li>
        <li>news002</li>
        <li>news003</li>
    </ul>
</template>

<script>
    export default {
        name: 'News',
        mounted(){
            console.log('news被挂载')
        },
        beforeDestroy(){
            console.log('news即将被销毁')
        }
    };
</script>