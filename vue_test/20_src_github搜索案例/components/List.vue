<template>
    <div class="row">
        <div v-show="info.users.length" v-for="user in info.users" class="card" :key="user.login">
            <a :href="user.html_url" target="_blank">
                <img :src="user.avatar_url" style='width: 100px'/>
            </a>
            <p class="card-text">{{user.login}}</p>
        </div>
        <h2 v-show="info.isFirst">初次见面，你好</h2>
        <h2 v-show="info.isLoading">加载中</h2>
        <h2 v-show="info.errMsg">{{info.errMsg}}</h2>
    </div>
</template>

<script>
export default {
    name: 'List',
    data() {
        return {
            info:{
                isFirst:true,
                isLoading:false,
                errMsg:'',
                users: [],
            }
        };
    },
    methods:{
        updateListInfo(infoObj){
            this.info = {...this.info,...infoObj}
            console.log('List收到数据',infoObj)
            console.log('info',this)
        }
    },
    mounted(){
        this.$bus.$on('updateListInfo',this.updateListInfo)
    }
};
</script>

<style scoped>

.album {
  min-height: 50rem; /* Can be removed; just added for demo purposes */
  padding-top: 3rem;
  padding-bottom: 3rem;
  background-color: #f7f7f7;
}

.card {
  float: left;
  width: 33.333%;
  padding: .75rem;
  margin-bottom: 2rem;
  border: 1px solid #efefef;
  text-align: center;
}

.card > img {
  margin-bottom: .75rem;
  border-radius: 100px;
}

.card-text {
  font-size: 85%;
} 

</style>