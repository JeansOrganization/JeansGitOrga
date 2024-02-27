<template>
    <div>
        <button>原生按钮</button>
        <input type="text">
        <hr>
        <jean-row>
            <jean-button>默认按钮</jean-button>
            <jean-button type="primary">主要按钮</jean-button>
            <jean-button type="success">成功按钮</jean-button>
            <jean-button type="info">信息按钮</jean-button>
            <jean-button type="warning">警告按钮</jean-button>
            <jean-button type="danger">危险按钮</jean-button>
        </jean-row>
        <hr>
        <jean-row>
            <jean-button icon="el-icon-search" circle></jean-button>
            <jean-button type="primary" icon="el-icon-edit" circle></jean-button>
            <jean-button type="success" icon="el-icon-check" circle></jean-button>
            <jean-button type="info" icon="el-icon-message" circle></jean-button>
            <jean-button type="warning" icon="el-icon-star-off" circle></jean-button>
            <jean-button type="danger" icon="el-icon-delete" circle></jean-button>
        </jean-row>
        <hr>
        <jean-date-picker
            v-model="value1"
            type="date"
            placeholder="选择日期">
        </jean-date-picker>
    </div>
</template>
<script>
    export default {
        name: 'App',
    }
</script>