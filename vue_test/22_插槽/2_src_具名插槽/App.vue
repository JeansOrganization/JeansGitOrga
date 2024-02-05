<template>
    <div class="container">
        <Category title="美食">
			<img slot="center" src="https://s3.ax1x.com/2021/01/16/srJlq0.jpg" alt="">
            <a slot="footer" href="https://www.baidu.com">更多美食</a>
        </Category>
        <Category title="游戏">
            <ul slot="center">
                <li v-for="(g,index) in games" :key="index">{{g}}</li>
            </ul>
            <div class="footer" slot="footer">
                <a href="https://www.baidu.com">单机游戏</a>
                <a href="https://www.baidu.com">网络游戏</a>
            </div>
        </Category>
        <Category title="电影">
			<video slot="center" controls src="https://www.runoob.com/try/demo_source/mov_bbb.mp4"></video>
            <template v-slot:footer>
                <div class="footer">
                    <a href="https://www.baidu.com">经典</a>
                    <a href="https://www.baidu.com">热门</a>
                    <a href="https://www.baidu.com">推荐</a>
                </div>
                <h3>欢迎前来观看</h3>
            </template>
        </Category>
    </div>
</template>
<script>
import Category from './components/Category'
export default {
    name: 'App',
    components:{Category},
		data() {
			return {
				foods:['火锅','烧烤','小龙虾','牛排'],
				games:['红色警戒','穿越火线','劲舞团','超级玛丽'],
				films:['《教父》','《拆弹专家》','《你好，李焕英》','《尚硅谷》']
			}
		},
}
</script>
<style>
.container,.footer{
    display: flex;
    justify-content: space-around;
}
h3{
    text-align: center;
}
</style>