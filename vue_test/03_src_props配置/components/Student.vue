<template>
    <div>
        <h1>{{msg}}</h1>
        <h2>学生名：{{name}}</h2>
        <h2>学生年龄：{{myAge}}</h2>
        <h2>学生性别：{{sex}}</h2>
        <button @click="addAge">点我年龄+1</button>
        <hr>
    </div>
</template>

<script>
    export default {
        name:'Student',
        data() {
            return {
                msg:'我是一个尚硅谷的学生',
                myAge:this.age
            };
        },
        methods: {
            addAge(){
                this.myAge++
            }
        },

        // props:['name','age','sex'], //简单声明接收

        //接收的同时对数据进行类型限制
        /* props:{
            name:String,
            age:Number,
            sex:String
        } */

        //接收的同时对数据进行类型限制、默认值的指定、必要性的限制
        props:{
            name:{
                type:String,
                required:true
            },
            age:{
                type:Number,
                default:99
            },
            sex:{
                type:String,
                required:true
            }
        }
    }
</script>