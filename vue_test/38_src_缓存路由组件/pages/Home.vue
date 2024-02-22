<template>
    <div>
      <h2>Home组件内容</h2>
      <div>
        <ul class="nav nav-tabs">
          <li>
            <router-link replace class="list-group-item" active-class="active" to="/home/news">News</router-link>
          </li>
          <li>
            <router-link replace class="list-group-item" active-class="active" to="/home/message">Message</router-link>
          </li>
        </ul>
        <hr>

        <!-- 缓存多个路由组件(include里面填写组件名称) -->
        <keep-alive :include="['News','Message']">

        <!-- 缓存一个路由组件(只缓存一个组件时，不需要v-bind，直接填写字符串) -->
        <!-- <keep-alive include="News"> -->
          <router-view></router-view>
        </keep-alive>
      </div>
    </div>
</template>

<script>
    export default {
        name: 'Home',
        /* beforeDestroy() {
          console.log('Home组件即将被销毁了')
        }, */
        /* mounted() {
          console.log('Home组件挂载完毕了',this)
          window.homeRoute = this.$route
          window.homeRouter = this.$router
        },  */
    };
</script>