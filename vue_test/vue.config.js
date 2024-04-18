module.exports = {
  pages: {
    index: {
      // page 的入口
      entry: 'src/main.js',
    }
  },
  lintOnSave:false, //关闭语法检查
  //开启代理服务器-vue-cli官方配置
  // 方式一
  // devServer: {
  //   proxy: 'http://localhost:5000'
  // }

  // 方式二
  devServer: {
    proxy: {
      '/jean': {
        target: 'http://localhost:5000',
        pathRewrite: {'^/jean':''},
        ws: true, //用于支持websocket
        changeOrigin: false
      },
      '/demo': {
        target: 'http://localhost:5001',
        pathRewrite: {'^/demo':''},
        ws: true, //用于支持websocket
        changeOrigin: true
      }
    }
  }
}