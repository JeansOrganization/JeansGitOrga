<template>
  <br/><br/>
  用户名:<input type="text" v-model="loginRequest.username"><br/><br/>
    密码:<input type="text" v-model="loginRequest.password"><br/><br/>
  <button @click="Login">登录</button><br/><br/>
  <span>用户名:{{state.username}}</span><br/><br/>
  <span>token:{{state.token}}</span><br/><br/>
  好友:<input type="text" v-model="state.toUserName"><input type="text" v-model="state.toUserMessage" @keypress.enter="SendPrivateMessage">
  <button @click="SendPrivateMessage">发送</button><br/><br/>
  <ul>
    <li v-for="(msg,index) in state.fromUserMessage" :key="index">{{msg}}<br/></li>
  </ul>
  <br/><br/>
  群组通话:<input type="text" v-model="state.toAllMessage" @keypress.enter="SendPublicMessage"><br/><br/>
  <ul>
    <li v-for="(msg,index) in state.fromAllMessage" :key="index">{{msg}}</li>
  </ul>
</template>

<script>
import { reactive } from 'vue'
import * as signalR from '@microsoft/signalr'
import axios from 'axios'
let connection
export default {
  name: 'HelloWorld',
  setup(){
    const state = reactive({username:'',token:'',toAllMessage:'',fromAllMessage:[],toUserName:'',toUserMessage:'',fromUserMessage:[]})
    const loginRequest = reactive({username:'',password:''})

    function Login(){
      if(loginRequest.username == '' || loginRequest.password == '')return;
      axios.post('https://localhost:7055/Demo/Login',loginRequest).then(
        Response=>{
          console.log(Response)
          if(Response.status != 200){
            console.log(1,Response.statusText)
            return;
          }
          if(Response.data.code != '0'){
            console.log(2,Response.data.error)
            return;
          }
          alert('登录成功')
          state.username = Response.data.obj.userName
          state.token = Response.data.token
          OpenConnection()
        },
        Error=>{
          console.log(Error)
        }
      )
    }

    async function SendPrivateMessage(){
      if(connection == null) return
      await connection.invoke('SendPrivateMessage',state.toUserName,state.toUserMessage)
      state.toUserMessage = ''
    }

    async function SendPublicMessage(){
      if(connection == null) return
      await connection.invoke('SendPublicMessage',state.toAllMessage)
      state.toAllMessage = ''
    }

    async function OpenConnection(){
      const options = {skipNegotiation:true,transport: signalR.HttpTransportType.WebSockets}
      options.accessTokenFactory = () => state.token;
      connection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7055/Hub/ChatHub',options)
      .withAutomaticReconnect().build();
      await connection.start();
      connection.on('ReceivePublicMessage',msg=>{
        state.fromAllMessage.push(msg)
      });
      connection.on('ReceivePrivateMessage',(msg,id)=>{
        state.fromUserMessage.push(msg)
        console.log('获取到id啦:' + id)
      });
    }

    // onMounted(async function(){
    //   const options = {skipNegotiation:true,transport: signalR.HttpTransportType.WebSockets}
    //   options.accessTokenFactory = () => state.token;
    //   connection = new signalR.HubConnectionBuilder()
    //   .withUrl('https://localhost:7055/Hub/ChatHub',options)
    //   .withAutomaticReconnect().build();
    //   await connection.start();
    //   connection.on('ReceivePublicMessage',msg=>{
    //     state.fromAllMessage.push(msg)
    //   });
    // })

    return {
      state,
      loginRequest,
      Login,
      SendPrivateMessage,
      SendPublicMessage,
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
