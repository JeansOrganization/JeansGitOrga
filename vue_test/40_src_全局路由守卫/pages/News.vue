<template>
    <ul>
        <li :style="{opacity}">欢迎大家</li>
        <li>news001 <input type="text"></li>
        <li>news002 <input type="text"></li>
        <li>news003 <input type="text"></li>
    </ul>
</template>

<script>
    export default {
        name: 'News',
        data(){
            return {
                opacity:1
            }
        },
        // mounted(){
        //     console.log('news被挂载')
        // },
        // beforeDestroy(){
        //     console.log('news即将被销毁')
        // },

        activated(){
            console.log('News组件被激活了')
            this.timer = setInterval(() => {
                console.log('@')
                this.opacity -= 0.01
                if(this.opacity <= 0) this.opacity = 1
            }, 16);
        },
        deactivated(){
            console.log('News组件失活了')
            clearInterval(this.timer)
            // console.log(window)
        }
    };
</script>