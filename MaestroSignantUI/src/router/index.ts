import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router';
import LandingPage from '@/views/LandingPage.vue';
import Routes from '@/types/routers';

const routes: any = [
  {
    path: Routes.root.path,
    name: Routes.root.name,
    component: LandingPage,
  },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

export default router;
