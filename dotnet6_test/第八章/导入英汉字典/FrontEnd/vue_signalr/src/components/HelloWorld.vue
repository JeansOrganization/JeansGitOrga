<template>
  <br/><br/>
  <button @click="Import">开始导入</button><br/><br/>
  <progress v-show="state.currentCount != state.totalCount" :value="state.currentCount" :max="state.totalCount"></progress>
</template>

<script>
import * as signalR from '@microsoft/signalr'
import { onMounted, reactive } from 'vue'
// import axios from 'axios'
let connection
export default {
  name: 'HelloWorld',
  setup(){
    const state = reactive({currentCount:0,totalCount:0})
    async function Import(){
      if(connection == null)return;
      await connection.invoke('Import')
      alert('导入完成')
    }

    async function OpenConnection(){
      connection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7175/Hub/ImportDictHub')
      .withAutomaticReconnect().build();
      await connection.start();
      connection.on('ImportProgress',(cur,total)=>{
        console.log('ImportProgress接受到了',cur,total)
        state.currentCount = cur
        state.totalCount = total
      });
    }

    onMounted(OpenConnection)



    return {
      state,
      Import,
      OpenConnection
    }
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
h3 {
  margin: 40px 0 0;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  margin: 0 10px;
}
a {
  color: #42b983;
}
</style>
