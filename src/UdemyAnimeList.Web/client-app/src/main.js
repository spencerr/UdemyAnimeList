import Vue from 'vue'
import App from './App.vue'
import router from './router'
import axios from 'axios'

import { BootstrapVue, IconsPlugin } from 'bootstrap-vue'
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'
import './css/site.css'

import { ValidationObserver, ValidationProvider, extend, localize } from 'vee-validate'
import en from 'vee-validate/dist/locale/en.json'
import * as rules from 'vee-validate/dist/rules'

const axiosConfig = {
  baseURL: '/api'
}

extend('required_if_not', {
  ...rules.required_if,
  validate (value, { target }) {
    return Boolean(target || value)
  }
})

Object.keys(rules).forEach(rule => {
  extend(rule, rules[rule])
})

localize('en', en)

Vue.config.productionTip = false
Vue.prototype.axios = axios.create(axiosConfig)

Vue.component('ValidationObserver', ValidationObserver)
Vue.component('ValidationProvider', ValidationProvider)

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
