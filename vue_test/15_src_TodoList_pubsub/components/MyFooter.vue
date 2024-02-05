<template>
    <div class="todo-footer" v-show="total">
        <label>
            <!-- 使用:checked单向绑定，再使用点击事件进行数据传递 -->
            <!-- <input type="checkbox" :checked="isAllCheck" @click="handleAllCheck"> -->
            <!-- 直接使用v-model双向绑定更方便 -->
            <input type="checkbox" v-model="isAllCheck">
        </label>
        <span>
            <span>已完成{{totalDone}}</span> / 全部{{total}}
        </span>
        <button class="btn btn-danger" @click="handleClearAllTodo">清除已完成任务</button>
    </div>
</template>

<script>
export default {
    name:'MyFooter',
    props:['todos'],
    methods:{
        // handleAllCheck(e){
        //     this.allCheck(e.target.checked)
        // }
    },
    computed:{
        total(){
            return this.todos.length
        },
        totalDone(){
            //pre指统计值，初始值为reduce第二个参数控制，todo是每次遍历的对象，每次遍历返回值会赋值给pre，最后一次返回值会直接返回给外部
            return this.todos.reduce((pre,todo) => pre + (todo.done ? 1 : 0),0)
        },
        isAllCheck:{
            get(){
                return this.total > 0 && this.totalDone == this.total
            },
            set(value){
                this.$emit('allCheck',value)
            }
        }
    },
    methods:{
        handleClearAllTodo(){
            this.$emit('clearAllTodo')
        }
    }
}
</script>

<style scoped>
    /*footer*/
    .todo-footer {
        height: 40px;
        line-height: 40px;
        padding-left: 6px;
        margin-top: 5px;
    }

    .todo-footer label {
        display: inline-block;
        margin-right: 5px;
        cursor: pointer;
    }

    .todo-footer label input {
        position: relative;
        top: -1px;
        vertical-align: middle;
        margin-right: 5px;
    }

    .todo-footer button {
        float: right;
        margin-top: 5px;
    }
</style>