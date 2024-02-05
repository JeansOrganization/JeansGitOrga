<template>
    <div>
        <button @click="isShow = !isShow">显示/隐藏</button>
        <transitionGroup name="hello" appear>
            <h2 v-show="!isShow" class="test" key="1">这是一条数据</h2>
            <h2 v-show="isShow" class="test" key="2">这是二条数据</h2>
        </transitionGroup>
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

.hello-enter,.hello-leave-to{
    transform: translateX(-100%);
}
.hello-enter-active,.hello-leave-active{
    transition: 0.5s;
}
.hello-enter-to,.hello-leave{
    transform: translateX(0);
}

</style>