//引入Vue
import Vue from 'vue'
//引入APP组件，它是所有组件的父组件
import App from './App.vue'

//关闭vue的生产提示
Vue.config.productionTip = false
//创建Vue实例对象
new Vue({
  el:'#app',
  render: h => h(App)
})
