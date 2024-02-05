<template>
    <div>
        <h2 @click="showName">学校名：{{name}}</h2>
        <h2>学校地址：{{address}}</h2>
        <hr>
    </div>
</template>

<script>
    // import {hunhe,hunhe2} from '../mixin'
    export default {
        name:'School',
        data() {
            return {
                name: '尚硅谷',
                address:'北京'
            };
        }
        // mixins:[hunhe,hunhe2]
    }
</script>