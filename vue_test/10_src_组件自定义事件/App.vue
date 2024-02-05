<template>
    <div class="app">
        <h1>{{msg}}，学生名是：{{studentName}}</h1>

        <!-- 通过父组件给子组件传递函数类型的props实现：子组件给父组件传递数据 -->
        <School :getSchoolName="getSchoolName" />

        <!-- 通过父组件给子组件绑定一个自定义事件实现：子组件给父组件传递数据(第一种写法，使用@或v-on) -->
        <!-- <Student @jean="getStudentName" @demo="m1"/> -->

        <!-- 通过父组件给子组件绑定一个自定义事件实现：子组件给父组件传递数据(第二种写法，使用ref) -->
        <Student ref="student" @click.native="show"/>
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
    },
    data(){
        return{
            msg:'你好呀',
            studentName:''
        }
    },
    methods:{
        getSchoolName(name){
            console.log('接收到了学校名：' + name)
        },
        getStudentName(name,...params){
            console.log('接收到了学生名：' + name,params)
            this.studentName = name
        },
        m1(){
            console.log('demo事件被触发了')
        },
        show(){
            console.log(123)
        }
    },
    mounted(){
        this.$refs.student.$on('jean',this.getStudentName) //绑定自定义事件
        // this.$refs.student.$on('jean',(name,...params) => {
        //     console.log('接收到了学生名：' + name,params)
        //     this.studentName = name
        //     console.log(this)
        // }) //将函数总体作为入参放入的话this所指向的是绑定这个事件的组件student，这时需要用箭头函数往外找外部的app组件

        // this.$refs.student.$once('jean',this.getStudentName) //绑定自定义事件
        // setTimeout(() => {
        //     this.$destroy()
        // }, 3000);
    }
};
</script>

<style scoped >
    .app{
        background-color:gray;
        padding: 10px;
        margin-top: 10px;
    }
</style>