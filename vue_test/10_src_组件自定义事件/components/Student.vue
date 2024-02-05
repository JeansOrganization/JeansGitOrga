<template>
    <div class="student">
        <h2>学生名：{{name}}</h2>
        <h2>学生性别：{{sex}}</h2>
        <h2>{{number}}</h2>
        <button @click="add">点我+1</button>
        <button @click="sendStudentName">发送学生名给APP</button>
        <button @click="cancelBind">解除绑定事件</button>
        <button @click="death">销毁vc实例</button>
    </div>
</template>

<script>
    export default {
        name:'Student',
        data() {
            return {
                name:'张三',
                sex:'男',
                number:0
            };
        },
        methods:{
            add(){
                console.log('add被触发啦')
                this.number++
            },
            sendStudentName(){
                this.$emit('jean', this.name,555,666,777,888);
                // this.$emit('demo');
            },
            cancelBind(){
                this.$off('jean') //解绑单个事件
                // this.$off(['jean','demo']) //解绑多个事件
                // this.$off() //解绑全部事件
            },
            death(){
                this.$destroy() //销毁了当前student组件的实例，销毁后所有student实例的自定义事件全都不奏效
            }
        }
    }
</script>

<style lang="less" >
    .student{
        background-color: pink;
        padding: 10px;
        margin-top: 10px;
    }
</style>