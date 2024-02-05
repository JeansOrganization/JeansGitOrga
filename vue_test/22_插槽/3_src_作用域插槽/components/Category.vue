<template>
    <div class="category">
        <h2>{{title}}</h2>
		<slot :games="games">默认值</slot>
    </div>
</template>

<script>
export default {
    name: 'Category',
    props:['title'],
    data() {
        return {
            games:['红色警戒','穿越火线','劲舞团','超级玛丽'],
        }
    },
};
</script>
<style>
.category{
    background-color: pink;
    width: 200px;
    height: 300px;
}
h2{
    text-align: center;
    background-color: orange;
}
img{
    width: 100%;
}
video{
    width: 100%;
}

</style>