<template>
    <div>
        <h1 class="title">111</h1>
        <School/>
        <Student/>
    </div>
</template>

<script>
import Student from './components/Student.vue';
import School from './components/School.vue';
export default {
    name: 'App',
    components:{
        Student,
        School
    }
};
</script>

<style scoped >
    .title{
        color:red;
    }
</style>