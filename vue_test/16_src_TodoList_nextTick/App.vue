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
            this.todos.unshift(todoObj)
        },
        changeTodoState(_,todoId){
            this.todos.forEach(r=>r.id == todoId ? (r.done = !r.done) : '')
        },
        deleteTodo(_,todoId){
            let i = 0
            this.todos.forEach(r=>r.id == todoId ? (this.todos.splice(i,1)) : i++)
        },
        allCheck(isAllCheck){
            if(isAllCheck){
                this.todos.forEach(r=>r.done = true)
            }
            else{
                this.todos.forEach(r=>r.done = false)
            }
        },
        clearAllTodo(){
            this.todos = this.todos.filter(r=>!r.done)
        },
        modifyTodoIsEdit(_,todoId){
            this.todos.forEach(r=>r.id == todoId ? (r.isEdit = true) : '')
        },
        modifyTodoTitle(_,obj){
            this.todos.forEach(r=>{
                if(r.id == obj.todoId){
                    r.title = obj.title
                    r.isEdit = false
                }
            })
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
        this.modifyTodoIsEditId = pubsub.subscribe('modifyTodoIsEdit',this.modifyTodoIsEdit)
        this.modifyTodoTitleId = pubsub.subscribe('modifyTodoTitle',this.modifyTodoTitle)
    },
    beforeDestroy(){
        // this.$bus.$off('changeTodoState')
        // this.$bus.$off('deleteTodo')
        pubsub.unsubscribe(this.changeTodoStateId)
        pubsub.unsubscribe(this.deleteTodoId)
        pubsub.unsubscribe(this.modifyTodoIsEditId)
        pubsub.unsubscribe(this.modifyTodoTitleId)
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

    .btn-edit {
        color: #fff;
        background-color: skyblue;
        border: 1px solid rgb(94, 194, 234);
        margin-right: 10px;
    }

    .btn-edit:hover {
        color: #fff;
        background-color: rgb(94, 194, 234);
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