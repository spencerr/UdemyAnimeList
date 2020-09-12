import Vue from 'vue'
import App from './App.vue'
import router from './router'
import axios from 'axios'

import { BootstrapVue, IconsPlugin } from 'bootstrap-vue'
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'
import './css/site.css'

const axiosConfig = {
  baseURL: '/api'
}

Vue.config.productionTip = false
Vue.prototype.axios = axios.create(axiosConfig)

Vue.use(BootstrapVue)
Vue.use(IconsPlugin)

router.afterEach((to, from) => {
  if (document.cookie.indexOf('XSRF-TOKEN') > -1) {
    const value = `; ${document.cookie}`
    const parts = value.split('; XSRF-TOKEN=')
    const token = parts.pop().split(';').shift()
    axios.defaults.headers.common['X-XSRF-TOKEN'] = token
  }
})

new Vue({
  router,
  render: h => h(App)
}).$mount('#app')
