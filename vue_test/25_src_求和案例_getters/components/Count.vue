<template>
    <div>
        <h1>当前求和为：{{$store.state.sum}}</h1>
        <h1>当前求和放大10倍为：{{$store.getters.bigSum}}</h1>
        <select v-model.number="n">
            <option value="1">1</option>
            <option value="2">2</option>
            <option value="3">3</option>
        </select>
        <button @click="increment">+</button>
        <button @click="decrement">-</button>
        <button @click="incrementOdd">当前和为奇数再加</button>
        <button @click="incrementWait">等一等再加</button>
    </div>
</template>

<script>
export default {
    name: 'Count',
    data(){
        return{
            n:1,
        }
    },
    methods:{
        increment(){
            this.$store.commit('Increment',this.n)
        },
        decrement(){
            this.$store.commit('Decrement',this.n)
        },
        incrementOdd(){
            this.$store.dispatch('incrementOdd',this.n)
        },
        incrementWait(){
            this.$store.dispatch('incrementWait',this.n)
        },
    },
    mounted(){
        console.log(this.$store)
    }
};
</script>
<style>
button{
    margin-left: 5px;
}
</style>