<template>
    <div>
        <h2>学校名：{{name | mySlice}}</h2>
        <h2>学校地址：{{address}}</h2>
        <button @click="Hello">点我打招呼</button>
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