// 该文件专门用于创建整个应用的路由器
import VueRouter from 'vue-router'
//引入组件
import About from '../pages/About'
import Home from '../pages/Home'
import Message from '../pages/Message'
import News from '../pages/News'
import Detail from '../pages/Detail'

//创建并暴露一个路由器
const router = new VueRouter({
    routes:[
        {
            name:'guanyu',
            path:'/about',
            meta:{title:'关于',isAuth:true},
            component:About
        },
        {
            name:'zhuye',
            path:'/home',
            meta:{title:'主页'},
            component:Home,
            children:[
                {
                    name:'xiaoxi',
                    path:'message',
                    meta:{title:'消息',isAuth:true},
                    component:Message,
                    children:[
                        {
                            name:'xiangqing',
                            path:'detail',
                            // path:'detail/:id/:title',
                            meta:{title:'详情',isAuth:true},
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
                    name:'xinwen',
                    path:'news',
                    meta:{title:'新闻',isAuth:true},
                    component:News,
                    //专属前置路由守卫
                    // beforeEnter:(to,from,next)=>{
                    //     console.log('专属前置路由守卫',to,from)
                    //     if(to.meta.isAuth){
                    //         if(localStorage.getItem('school') === 'yingxin'){
                    //             next()
                    //         }
                    //         else{
                    //             alert('您无权访问' + to.meta.title + '模块')
                    //         }
                    //     }
                    //     else{
                    //         next()
                    //     }
                    // }
                }
            ]
        }
    ]
})

//全局前置路由守卫————初始化的时候被调用、每次路由切换之前被调用
// router.beforeEach((to,from,next)=>{
//     console.log('全局前置路由守卫',to,from)
//     if(to.meta.isAuth){
//         if(localStorage.getItem('school') === 'yingxin'){
//             next()
//         }
//         else{
//             alert('您无权访问' + to.meta.title + '模块')
//         }
//     }
//     else{
//         next()
//     }
// })

//全局后置路由守卫————初始化的时候被调用、每次路由切换之后被调用
router.afterEach((to,from)=>{
    console.log('全局后置路由守卫',to,from)
    document.title = to.meta.title || 'vue_test'
})

export default router