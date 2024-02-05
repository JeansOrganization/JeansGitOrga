<template>
    <div>
        <h2>学生名：{{name}}</h2>
        <h2>学生性别：{{sex}}</h2>
        <input type="text" v-fbind:value="age">
        <hr>
    </div>
</template>

<script>
    export default {
        name:'Student',
        data() {
            return {
                name:'张三',
                sex:'男',
                age:10
            };
        }
    }
</script>