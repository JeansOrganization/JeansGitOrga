<template>
    <li>
        <label>
            <input type="checkbox" :checked="todo.done" @click="handleChangeTodoState(todo.id)">
            <!-- 如下代码也能实现功能，但是不太推荐，因为有点违反原则，因为修改了props -->
            <!-- <input type="checkbox" v-model="todo.done"> -->
            <span>{{todo.title}}</span>
        </label>
        <button class="btn btn-danger" @click="handleDeleteTodo(todo.id)">删除</button>
    </li>
</template>

<script>
export default {
    name:'MyItem',
    props:['todo','changeTodoState','deleteTodo'],
    methods:{
        //修改状态
        handleChangeTodoState(todoId){
            this.changeTodoState(todoId)
        },
        handleDeleteTodo(todoId){
            if(confirm('确认删除该项吗？')){
                this.deleteTodo(todoId)
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