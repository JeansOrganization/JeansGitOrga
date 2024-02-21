//该文件用于创建Vuex中最为核心的store
import Vue from 'vue'
//引入Vuex
import Vuex from 'vuex'
//引入Count配置
import countAbout from './Count'
//引入Person配置
import personAbout from './Person'

Vue.use(Vuex)

export default new Vuex.Store({
    modules:{
        personAbout,
        countAbout
    }
})

