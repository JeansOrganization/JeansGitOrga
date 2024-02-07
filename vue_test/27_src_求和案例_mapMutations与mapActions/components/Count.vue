<template>
    <div>
        <h2>当前求和为：{{sum}}</h2>
        <h4>当前求和放大10倍为：{{bigSum}}</h4>
        <h4>我在{{school}}，学习{{subject}}</h4>
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
        //借助mapState生成计算属性，从state中读取数据。(对象写法)
        // ...mapState({sum:'sum',school:'school',subject:'subject'})
        //借助mapState生成计算属性，从state中读取数据。(数组写法)
        ...mapState(['sum','school','subject']),

        /* ******************************************************************** */

        //借助mapGetters生成计算属性，从getters中读取数据。(对象写法)
        // ...mapGetters({bigSum:'bigSum'})
        //借助mapGetters生成计算属性，从getters中读取数据。(数组写法)
        ...mapGetters(['bigSum'])

    },
    methods:{
        //靠程序员自己写方法
        // Increment(){
        //     this.$store.commit('Increment',this.n)
        // },
        // Decrement(){
        //     this.$store.commit('Decrement',this.n)
        // },

        //借助mapMutations生成对应的方法，方法中会调用commit去联系mutations(对象写法)
        // ...mapMutations({Increment:'Increment',Decrement:'Decrement'}),
        //借助mapMutations生成对应的方法，方法中会调用commit去联系mutations(数组写法)
        ...mapMutations(['Increment','Decrement']),

        /* ************************************************* */
        
        //程序员亲自写方法
        // incrementOdd(){
        //     this.$store.dispatch('incrementOdd',this.n)
        // },
        // incrementWait(){
        //     this.$store.dispatch('incrementWait',this.n)
        // },

        //借助mapActions生成对应的方法，方法中会调用dispatch去联系actions(对象写法)
        // ...mapActions({incrementOdd:'incrementOdd',incrementWait:'incrementWait'})
        //借助mapActions生成对应的方法，方法中会调用dispatch去联系actions(数组写法)
        ...mapActions(['incrementOdd','incrementWait'])

    },
    mounted(){
        const x = mapState({sum:'sum',school:'school',subject:'subject'})
        console.log(x)
    }
};
</script>
<style>
button{
    margin-left: 5px;
}
</style>