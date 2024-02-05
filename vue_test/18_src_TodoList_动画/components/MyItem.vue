<template>
    <li>
        <label>
            <input type="checkbox" :checked="todo.done" @click="handleChangeTodoState(todo.id)">
            <!-- 如下代码也能实现功能，但是不太推荐，因为有点违反原则，因为修改了props -->
            <!-- <input type="checkbox" v-model="todo.done"> -->
            <input 
                v-show="todo.isEdit" 
                type="text" 
                ref="inputEdit"
                :value="todo.title" 
                @blur="handleTitleModify" 
                @keyup.enter="handleTitleModify">
            <span v-show="!todo.isEdit" >{{todo.title}}</span>
        </label>
        <button class="btn btn-danger" @click="handleDeleteTodo(todo.id)">删除</button>
        <button v-show="!todo.isEdit" class="btn btn-edit" @click="handleEdit($event,todo)">编辑</button>
    </li>
</template>

<script>
import pubsub from 'pubsub-js'
export default {
    name:'MyItem',
    props:['todo'],
    methods:{
        //修改状态
        handleChangeTodoState(todoId){
            // this.$bus.$emit('changeTodoState',todoId)
            pubsub.publish('changeTodoState',todoId)
        },
        handleDeleteTodo(todoId){
            if(confirm('确认删除该项吗？')){
                // this.$bus.$emit('deleteTodo',todoId)
                pubsub.publish('deleteTodo',todoId)
            }
        },
        handleEdit(e,todo){
            // pubsub.publish('modifyTodoIsEdit',todo.id) //订阅方式

            //直接改todo属性
            todo.isEdit = true
            // this.$refs.inputEdit.focus() //因为页面渲染会在此函数结束后再进行，所以此处获取焦点无效
            //$nextTick会在当前函数变更数据执行完页面重新渲染后调用
            this.$nextTick(function(){
                this.$refs.inputEdit.focus()
            })

            // setTimeout(() => {
            //     this.$refs.inputEdit.focus()
            // }, 200);
            

        },
        handleTitleModify(e){
            if(e.target.value.trim() === ''){
                e.target.value = this.todo.title
            }
            pubsub.publish('modifyTodoTitle',{todoId:this.todo.id,title:e.target.value})
        }
    },
    directives:{
        ffocus:{
            //自定义指令，给定bool值，true的话会在页面放入时给与焦点
            inserted(element,bind){
                if(bind.value){
                    element.focus();
                }
            }
        }
    }
}
</script>

<style scoped>
    /*item*/
    li {
        list-style: none;
        height: 36px;
        line-height: 36px;
        padding: 0 5px;
        border-bottom: 1px solid #ddd;
    }

    li label {
        float: left;
        cursor: pointer;
    }

    li label li input {
        vertical-align: middle;
        margin-right: 6px;
        position: relative;
        top: -1px;
    }

    li button {
        float: right;
        display: none;
        margin-top: 3px;
    }

    li:before {
        content: initial;
    }

    li:last-child {
        border-bottom: none;
    }

    li:hover{
        background-color: #ddd;
    }

    li:hover button{
        display: block;
    }
</style>