<template>
    <div>
        <h1>人员列表</h1>
        <h2 style="color: red">Count组件求和为：{{sum}}</h2>
        <h2>列表中第一个人员姓名：{{firstPersonName}}</h2>
        <input type="text" placeholder="请输入人员姓名" v-model="name">
        <button @click="addPerson">添加</button>
        <button @click="addPersonWang">添加一个姓王的人</button>
        <button @click="addPersonServer">添加一个随机人员</button>
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
            this.$store.commit('personAbout/AddPerson',personObj)
            this.name = ''
        },
        addPersonWang(){
            if(this.name.trim()=='')return;
            const personObj = {id:nanoid(),name:this.name}
            this.$store.dispatch('personAbout/addPersonWang',personObj)
            this.name = ''
        },
        addPersonServer(){
            this.$store.dispatch('personAbout/addPersonServer')
            this.name = ''
        }
    },
    computed:{
        personList(){
            return this.$store.state.personAbout.personList
        },
        sum(){
            return this.$store.state.countAbout.sum
        },
        firstPersonName(){
            return this.$store.getters['personAbout/firstPersonName']
        }
    }
};
</script>