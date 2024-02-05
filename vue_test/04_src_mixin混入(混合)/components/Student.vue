<template>
    <div>
        <h2 @click="showName">学生名：{{name}}</h2>
        <h2>学生性别：{{sex}}</h2>
        <hr>
    </div>
</template>

<script>
    // import {hunhe,hunhe2} from '../mixin'
    export default {
        name:'Student',
        data() {
            return {
                name:'张三',
                sex:'男',
                x:300
            };
        }
        // mixins:[hunhe,hunhe2],
    }
</script>