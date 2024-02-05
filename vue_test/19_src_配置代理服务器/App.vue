<template>
    <div id="root">
        <button @click="getStudentInfo">获取学生信息</button>
        <button @click="getCarInfo">获取汽车信息</button>
    </div>
</template>

<script>
import axios from 'axios'
export default {
    name: 'App',
    methods:{
        getStudentInfo(){
            axios.get('http://localhost:8080/jean/students').then(
                Response => {
                    console.log('成功获取到学生数据:',Response.data)
                },
                error => {
                    console.log('oh,报错了',error.message)
                }
            )
        },
        getCarInfo(){
            axios.get('http://localhost:8080/demo/cars').then(
                Response => {
                    console.log('成功获取到汽车数据:',Response.data)
                },
                error => {
                    console.log('oh,报错了',error.message)
                }
            )
        }
    }
};
</script>