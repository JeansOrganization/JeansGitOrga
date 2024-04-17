import Vue from 'vue'
import App from './App'

Vue.config.productionTip = false

new Vue({
  el:'#app',
  render:function(h){
    console.log(this)
    return h(App)
  }
})