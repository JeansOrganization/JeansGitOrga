<template>
    <div>
        <button @click="isShow = !isShow">显示/隐藏</button>
        <transitionGroup 
            appear
            name="animate__animated animate__bounce"
            enter-active-class="animate__rubberBand"
            leave-active-class="animate__backOutRight"
        >
            <h2 v-show="!isShow" class="test" key="1">这是一条数据</h2>
            <h2 v-show="isShow" class="test" key="2">这是二条数据</h2>
        </transitionGroup>
    </div>
</template>

<script>
import 'animate.css'
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

</style>