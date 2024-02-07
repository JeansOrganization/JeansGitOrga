<template>
    <div>
        <h1>人员列表</h1>
        <h2 style="color: red">Count组件求和为：{{sum}}</h2>
        <input type="text" placeholder="请输入人员姓名" v-model="name">
        <button @click="addPerson">添加</button>
        <ul>
            <li v-for="p in personList" :key="p.id">{{p.name}}</li>
        </ul>
    </div>
</template>

<script>
import {nanoid} from 'nanoid'
export default {
    name: 'Person',
    data(){
        return {
            name:''
        }
    },
    methods:{
        addPerson(){
            if(this.name.trim()=='')return;
            const personObj = {id:nanoid(),name:this.name}
            this.$store.commit('AddPerson',personObj)
            this.name = ''
        }
    },
    computed:{
        personList(){
            return this.$store.state.personList
        },
        sum(){
            return this.$store.state.sum
        }
    }
};
</script>