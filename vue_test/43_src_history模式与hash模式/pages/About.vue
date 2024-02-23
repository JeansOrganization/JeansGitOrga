<template>
    <h2>我是About的内容</h2>
</template>

<script>
    export default {
        name: 'About',
		/* beforeDestroy() {
			console.log('About组件即将被销毁了')
		},*/
		/* mounted() {
			console.log('About组件挂载完毕了',this)
			window.aboutRoute = this.$route
			window.aboutRouter = this.$router
		},  */
		beforeRouteEnter(to,from,next){
			console.log('About-beforeRouteEnter',to,from)
			if(to.meta.isAuth){
				if(localStorage.getItem('school') === 'yingxin'){
					next()
				}
				else{
					alert('您无权访问' + to.meta.title + '模块')
				}
			}
			else{
				next()
			}
		},
		beforeRouteLeave(to,from,next){
			console.log('About-beforeRouteLeave',to,from)
			next();
		}
    };
</script>