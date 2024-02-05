<template>
    <div>
        <Student name="Jean" sex="男" :age="17" />
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