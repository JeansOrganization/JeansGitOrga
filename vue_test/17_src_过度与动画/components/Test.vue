<template>
    <div>
        <button @click="isShow = !isShow">显示/隐藏</button>
        <transition name="hello" appear>
            <h2 v-show="isShow" class="test">这是一条数据</h2>
        </transition>
    </div>
</template>

<script>
export default {
    name: 'VueTestTest',

    data() {
        return {
            isShow:true
        };
    }
};
</script>

<style scoped>
.test{
    background-color: orange;
}

.hello-enter-active{
    animation: jean 0.5s;
}

.hello-leave-active{
    animation: jean 0.5s reverse;
}


@keyframes jean{
    from{
        transform: translateX(-100%);
    }
    to{
        transform: translateX(0px);
    }
}
</style>