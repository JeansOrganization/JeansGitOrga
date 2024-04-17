<template>
    <div>
        <h1 v-text="msg" ref="title"></h1>
        <button @click="showDOM" ref="btn">点我输出上方的DOM元素</button>
        <School ref="sch"/>
    </div>
</template>

<script>
import Student from './components/Student'
import School from './components/School'

export default ({
    name:'App',
    components:{
        Student,
        School
    },
    data(){
        return {
            msg:'欢迎来尚硅谷学习Vue!'
        }
    },
    methods:{
        showDOM:function(){
            console.log(this.$refs)
            console.log(this.$refs.title)
            console.log(this.$refs.btn)
            console.log(this.$refs.sch)
        }
    }
})
</script>