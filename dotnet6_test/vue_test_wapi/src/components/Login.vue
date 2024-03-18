<template>
    账号:<input type="text" v-model="state.LoginRequest.userName">
    密码:<input type="text" v-model="state.LoginRequest.password">
    <input type="button" value="获取" @click="LoginRequestFunc">
    <ul>
        <li v-for="p in state.processes" :key="p.id">{{p.id}} {{p.processName}}  {{p.workingSet64}}</li>
    </ul>
</template>

<script>
import {reactive} from 'vue'
import axios from 'axios'

export default ({
    name:'Login',
    setup() {
        const state=reactive({LoginRequest:{},processes:[]});
        
        function LoginRequestFunc(){
            axios.post('https://localhost:7210/Login/Login',state.LoginRequest).then(
                Response=>{
                    if(Response.data.isOk){
                        state.processes = Response.data.processes
                    }
                    else{
                        state.processes = null
                        alert('错误错误！！')
                    }
                }
            )
        }
        return {
            state,
            LoginRequestFunc,
        };
  },
});
</script>
