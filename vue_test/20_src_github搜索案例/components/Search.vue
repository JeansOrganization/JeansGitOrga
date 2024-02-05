<template>
    <section class="jumbotron">
        <h3 class="jumbotron-heading">Search Github Users</h3>
        <div>
            <input 
                type="text" 
                placeholder="enter the name you search" 
                v-model="keyWord" 
                @keyup.enter="searchUsers"
            />&nbsp;
            <button @click="searchUsers">Search</button>
        </div>
    </section>
</template>

<script>
import axios from 'axios'
export default {
    name: 'Search',
    data(){
        return{
            keyWord:''
        }
    },
    methods:{
        searchUsers(){
            this.$bus.$emit('updateListInfo',{isFirst:false,isLoading:true,users:[]})
            axios.get(`https://api.github.com/search/users?q=${this.keyWord}`).then(
                response => {
                    this.$bus.$emit('updateListInfo',{isLoading:false,users:response.data.items})
                },
                error => {
                    this.$bus.$emit('updateListInfo',{isLoading:false,errMsg:error.message,users:[]})
                }
            )
        }
    }
};
</script>

<style scope>

</style>