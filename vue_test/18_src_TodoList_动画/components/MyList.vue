<template>
    <ul class="todo-main">
        <transitionGroup name="todo" appear>
            <MyItem v-for="todoObj in todos" :key="todoObj.id" :todo="todoObj"></MyItem>
        </transitionGroup>
    </ul>
</template>

<script>
import MyItem from './MyItem'
export default {
    name:'MyList',
    components:{MyItem},
    props:['todos']
}
</script>

<style scoped>
    /*main*/
    .todo-main {
        margin-left: 0px;
        border: 1px solid #ddd;
        border-radius: 2px;
        padding: 0px;
    }

    .todo-empty {
        height: 40px;
        line-height: 40px;
        border: 1px solid #ddd;
        border-radius: 2px;
        padding-left: 5px;
        margin-top: 10px;
    }

    

.todo-enter-active{
    animation: jean 0.5s;
}

.todo-leave-active{
    animation: jean 0.5s reverse;
}

@keyframes jean{
    from{
        transform: translateX(100%);
    }
    to{
        transform: translateX(0px);
    }
}
</style>