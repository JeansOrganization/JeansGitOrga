// 该文件专门用于创建整个应用的路由器
import VueRouter from 'vue-router'
//引入组件
import About from '../pages/About'
import Home from '../pages/Home'
import Message from '../pages/Message'
import News from '../pages/News'
import Detail from '../pages/Detail'

//创建并暴露一个路由器
export default new VueRouter({
    routes:[
        {
            path:'/about',
            component:About
        },
        {
            path:'/home',
            component:Home,
            children:[
                {
                    path:'message',
                    component:Message,
                    children:[
                        {
                            name:'xiangqing',
                            path:'detail',
                            // path:'detail/:id/:title',
                            component:Detail,

                            //props的第一种写法，值为对象，该对象中的所有key-value都会以props的形式传给Detail组件
                            // props:{id:'002',title:'xxxx'}

                            //props的第二种写法，值为布尔值，若布尔值为真，就会把该路由组件收到的所有params参数，以props的形式传给Detail组件(写法方便)
                            // props:true

                            //props的第三种写法，值为函数(灵活度高)
                            props($route){
                                return{
                                    id:$route.query.id,
                                    title:$route.query.title,
                                    a:1,
                                    b:9
                                }
                            }

                            //只要使用params参数，就需要在path里加上对应的/:id/:title,不使用就不需要加
                        }
                    ]
                },
                {
                    path:'news',
                    component:News
                }
            ]
        }
    ]
})