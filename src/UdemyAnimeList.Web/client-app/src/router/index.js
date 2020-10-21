import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '../views/Home.vue'

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/about',
    name: 'About',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/About.vue')
  },
  {
    path: '/anime/:id',
    name: 'view-anime',
    component: () => import('../views/Anime/View.vue')
  },
  {
    path: '/anime/current-season',
    name: 'current-season',
    component: () => import('../views/Anime/CurrentSeason.vue')
  },
  {
    path: '/anime/:id/edit',
    name: 'edit-anime',
    component: () => import('../views/Anime/CreateEdit.vue')
  },
  {
    path: '/anime/create',
    name: 'create-anime',
    component: () => import('../views/Anime/CreateEdit.vue')
  }
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router
