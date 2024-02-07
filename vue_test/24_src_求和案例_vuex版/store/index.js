//该文件用于创建Vuex中最为核心的store
import Vue from 'vue'
//引入Vuex
import Vuex from 'vuex'

Vue.use(Vuex)

//准备actions——用来编写业务逻辑的位置
const actions = {
    incrementOdd(context,value){
        if(context.state.sum%2==1){
            context.commit('Increment',value)
        }
    },
    incrementWait(context,value){
        setTimeout(() => {
            context.commit('Increment',value)
        }, 500);
    },
}
//准备mutations——用于操作数据的位置（state）
const mutations = {
    Increment(state,value){
        state.sum += value;
    },
    Decrement(state,value){
        state.sum -= value;
    }
}
const state = {
    sum:0,
    y:10
}

export default new Vuex.Store({
    actions,
    mutations,
    state
})

