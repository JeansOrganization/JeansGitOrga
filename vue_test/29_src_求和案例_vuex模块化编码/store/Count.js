//count配置
export default {
    namespaced:true,
    actions:{
        incrementOdd(context,value){
            if(context.state.sum%2==1){
                context.commit('Increment',value)
            }
        },
        incrementWait(context,value){
            setTimeout(() => {
                context.commit('Increment',value)
            }, 500);
        }
    },
    mutations:{
        Increment(state,value){
            state.sum += value;
        },
        Decrement(state,value){
            state.sum -= value;
        }
    },
    state:{
        sum:0,
        school:'尚硅谷',
        subject:'前端',
    },
    getters:{
        bigSum(state,getters){
            return state.sum * 10
        }
    }
}