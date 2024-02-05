<template>
    <div id="root">
        <div class="todo-container">
            <div class="todo-wrap">
                <MyHeader @addTodo="addTodo"></MyHeader>
                <MyList :todos="todos"></MyList>
                <MyFooter :todos="todos" @allCheck="allCheck" @clearAllTodo="clearAllTodo"></MyFooter>
            </div>
        </div>
    </div>
</template>

<script>
import pubsub from 'pubsub-js'
import MyHeader from './components/MyHeader'
import MyFooter from './components/MyFooter'
import MyList from './components/MyList'
export default {
    name: 'App',
    components:{MyHeader,MyFooter,MyList},
    data(){
        return{
            todos:JSON.parse(localStorage.getItem('todos')) || []
        }
    },
    methods:{
        addTodo(todoObj){
            console.log('app组件收到数据')
            this.todos.unshift(todoObj)
        },
        changeTodoState(msgName,todoId){
            this.todos.forEach(r=>r.id == todoId ? (r.done = !r.done) : '')
        },
        deleteTodo(msgName,todoId){
            let i = 0
            this.todos.forEach(r=>r.id == todoId ? (this.todos.splice(i,1)) : i++)
        },
        allCheck(isAllCheck){
            
            console.log(isAllCheck)
            if(isAllCheck){
                this.todos.forEach(r=>r.done = true)
            }
            else{
                this.todos.forEach(r=>r.done = false)
            }
        },
        clearAllTodo(){
            this.todos = this.todos.filter(r=>!r.done)
        }
    },
    watch:{
        todos:{
            deep:true,
            handler(value){
                localStorage.setItem('todos',JSON.stringify(value))
            }
        }
    },
    mounted(){
        // this.$bus.$on('changeTodoState',this.changeTodoState)
        // this.$bus.$on('deleteTodo',this.deleteTodo)
        this.changeTodoStateId = pubsub.subscribe('changeTodoState',this.changeTodoState)
        this.deleteTodoId = pubsub.subscribe('deleteTodo',this.deleteTodo)
    },
    beforeDestroy(){
        // this.$bus.$off('changeTodoState')
        // this.$bus.$off('deleteTodo')
        pubsub.unsubscribe(this.changeTodoStateId)
        pubsub.unsubscribe(this.deleteTodoId)
    }
};
</script>

<style>
    /*base*/
    body {
        background: #fff;
    }

    .btn {
        display: inline-block;
        padding: 4px 12px;
        margin-bottom: 0;
        font-size: 14px;
        line-height: 20px;
        text-align: center;
        vertical-align: middle;
        cursor: pointer;
        box-shadow: inset 0 1px 0 rgba(255, 255, 255, 0.2), 0 1px 2px rgba(0, 0, 0, 0.05);
        border-radius: 4px;
    }

    .btn-danger {
        color: #fff;
        background-color: #da4f49;
        border: 1px solid #bd362f;
    }

    .btn-danger:hover {
        color: #fff;
        background-color: #bd362f;
    }

    .btn:focus {
        outline: none;
    }

    .todo-container {
        width: 600px;
        margin: 0 auto;
    }

    .todo-container .todo-wrap {
        padding: 10px;
        border: 1px solid #ddd;
        border-radius: 5px;
    }
</style>