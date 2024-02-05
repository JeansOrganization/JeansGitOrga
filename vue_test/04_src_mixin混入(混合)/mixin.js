export const hunhe = {
    methods:{
        showName(){
            alert(this.name)
        }
    },
    mounted() {
        console.log('挂载')
    },
}

export const hunhe2 = {
    data(){
        return{
            x:100,
            y:200
        }
    }
}