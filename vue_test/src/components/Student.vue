<template>
    <div>
        <h2>学生名：{{name}}</h2>
        <h2>学生年龄：{{age}}</h2>
    </div>
</template>

<script>
    export default {
        name:'Student',
        data() {
            return {
                name: 'Jean',
                age:17
            };
        },
    }
</script>