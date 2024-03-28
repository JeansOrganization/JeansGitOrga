<template>
  <input type="text" v-model="state.userMessage" @keypress="keyPress">
  <ul>
    <li v-for="(msg,index) in state.allMessage" :key="index">{{msg}}</li>
  </ul>
</template>

<script>
import { onMounted, reactive } from 'vue'
import * as signalR from '@microsoft/signalr'
let connection
export default {
  name: 'HelloWorld',
  setup(){
    let state = reactive({userMessage:'',allMessage:[]})

    async function keyPress(e){
      if(e.keyCode != 13) return;
      await connection.invoke('SendPublicMessage',state.userMessage)
      state.userMessage = ''
    }

    onMounted(async function(){
      connection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7069/Hub/CharHub')
      .withAutomaticReconnect().build();
      console.log(connection)
      await connection.start();
      connection.on('ReceiptPublicMessage',msg=>{
        state.allMessage.push(msg)
      });
    })


    return {
      state,
      keyPress
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
  display: inline-block;
  margin: 0 10px;
}
a {
  color: #42b983;
}
</style>
