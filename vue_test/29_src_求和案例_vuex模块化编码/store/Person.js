//引入axios
import axios from 'axios'
//引入nanoid
import { nanoid } from 'nanoid'


//person配置
export default {
    namespaced:true,
    actions:{
        addPersonWang(context,value){
            if(value.name.indexOf('王') === 0){
                context.commit('AddPerson',value)
            }
            else{
                alert('输入的名字必须姓王');
            }
        },
        addPersonServer(context){
            axios.get('https://api.xygeng.cn/one').then(
                Response=>{
                    if(Response.data.data.content.trim() == ''){
                        alert('获取随机名称为空')
                    }
                    const personObj = {id:nanoid(),name:Response.data.data.content}
                    context.commit('AddPerson',personObj)
                },
                error=>{
                    console.log(error.message)
                }
            )
        }
    },
    mutations:{
        AddPerson(state,value){
            state.personList.push(value)
        }
    },
    state:{
        personList:[
            {id:'001',name:'张三'}
        ]
    },
    getters:{
        firstPersonName(state,getters){
            if(state.personList.length > 0){
                return state.personList[0].name;
            }
            return "";
        }
    }
}