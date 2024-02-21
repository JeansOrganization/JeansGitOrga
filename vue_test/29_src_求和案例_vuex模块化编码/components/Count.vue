<template>
    <div>
        <h2>当前求和为：{{sum}}</h2>
        <h4>当前求和放大10倍为：{{bigSum}}</h4>
        <h4>我在{{school}}，学习{{subject}}</h4>
        <h4 style="color: red;">Person组件的总人数是：{{personList.length}}</h4>
        <select v-model.number="n">
            <option value="1">1</option>
            <option value="2">2</option>
            <option value="3">3</option>
        </select>
        <button @click="Increment(n)">+</button>
        <button @click="Decrement(n)">-</button>
        <button @click="incrementOdd(n)">当前和为奇数再加</button>
        <button @click="incrementWait(n)">等一等再加</button>
    </div>
</template>

<script>
import { mapState,mapGetters,mapMutations,mapActions } from 'vuex';
export default {
    name: 'Count',
    data(){
        return{
            n:1,
        }
    },
    computed:{
        //借助mapState生成计算属性，从state中读取数据。(数组写法)
        ...mapState('countAbout',['sum','school','subject']),
        ...mapState('personAbout',['personList']),

        //借助mapGetters生成计算属性，从getters中读取数据。(数组写法)
        ...mapGetters('countAbout',['bigSum']),

    },
    methods:{
        //借助mapMutations生成对应的方法，方法中会调用commit去联系mutations(数组写法)
        ...mapMutations('countAbout',['Increment','Decrement']),

        //借助mapActions生成对应的方法，方法中会调用dispatch去联系actions(数组写法)
        ...mapActions('countAbout',['incrementOdd','incrementWait'])

    },
    mounted(){
        const x = mapState({sum:'sum',school:'school',subject:'subject'})
        console.log(this.$store)
    }
};
</script>
<style>
button{
    margin-left: 5px;
}
</style>