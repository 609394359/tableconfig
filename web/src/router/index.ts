import { createRouter, createWebHashHistory } from 'vue-router'
import type { RouteRecordRaw } from 'vue-router'
import { useUserStore } from '@/stores/userStore'

const routes: RouteRecordRaw[] = [
    {
        path: '/login',
        name: 'Login',
        component: () => import('@/pages/LoginPage.vue'),
        meta: { title: '登录' }
    },
    {
        path: '/',
        component: () => import('@/components/layout/MainLayout.vue'),
        children: [
            {
                path: '',
                redirect: '/projects'
            },
            {
                path: 'projects',
                name: 'Projects',
                component: () => import('@/pages/ProjectPage.vue'),
                meta: { title: '项目管理' }
            },
            {
                path: 'projects/:projectId/tables',
                name: 'Tables',
                component: () => import('@/pages/TablePage.vue'),
                meta: { title: '表管理' }
            },
            {
                path: 'field-types',
                name: 'FieldTypes',
                component: () => import('@/pages/FieldTypePage.vue'),
                meta: { title: '字段类型' }
            },
            {
                path: 'database-types',
                name: 'DatabaseTypes',
                component: () => import('@/pages/DatabaseTypePage.vue'),
                meta: { title: '数据库类型' }
            },
            {
                path: 'users',
                name: 'users',
                component: () => import('@/pages/UserPage.vue'),
                meta: { title: '账号管理' }
            }
        ]
    }
]

const router = createRouter({
    history: createWebHashHistory(),
    routes
})

// 路由守卫
router.beforeEach((to, _from, next) => {
    // 设置标题
    const title = to.meta.title as string
    document.title = title ? `${title} - 表结构管理工具` : '表结构管理工具'

    // 鉴权
    const userStore = useUserStore()
    const isAuthenticated = !!userStore.token

    if (to.path !== '/login' && !isAuthenticated) {
        next('/login')
    } else if (to.path === '/login' && isAuthenticated) {
        next('/')
    } else {
        next()
    }
})

export default router
