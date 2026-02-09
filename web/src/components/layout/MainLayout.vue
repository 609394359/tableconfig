<script setup lang="ts">
import { ref, computed } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import {
  Setting,
  Coin,
  Folder,
  Histogram,
  ArrowLeft,
  ArrowRight,
  UserFilled,
  SwitchButton
} from '@element-plus/icons-vue'
import { useUserStore } from '@/stores/userStore'

const userStore = useUserStore()

const router = useRouter()
const route = useRoute()
const isCollapse = ref(false)

const menuItems = [
  {
    path: '/projects',
    title: '项目管理',
    icon: Folder
  },
  {
    path: '/field-types',
    title: '字段类型',
    icon: Histogram
  },
  {
    path: '/database-types',
    title: '数据库类型',
    icon: Coin
  },
  {
    path: '/users',
    title: '账号管理',
    icon: Coin
  }
]

const activeMenu = computed(() => {
  if (route.path.startsWith('/projects') && route.path !== '/projects') {
    return '/projects'
  }
  return route.path
})

function navigateTo(path: string) {
  router.push(path)
}

function handleLogout() {
  userStore.logout()
  router.push('/login')
}

function toggleCollapse() {
  isCollapse.value = !isCollapse.value
}
</script>

<template>
  <div class="min-h-screen flex">
    <!-- 侧边栏 -->
    <aside 
      :class="[
        'h-screen sticky top-0 flex flex-col transition-all duration-300',
        isCollapse ? 'w-20' : 'w-72'
      ]"
      class="sidebar-container"
    >
      <!-- Logo -->
      <div class="h-20 flex items-center justify-center border-b border-[#2a5080] shrink-0">
        <div v-if="!isCollapse" class="flex items-center gap-4">
          <div class="w-10 h-10 rounded-xl bg-gradient-to-br from-cyan-400 to-blue-500 flex items-center justify-center shadow-lg shadow-cyan-500/30">
            <el-icon :size="22" class="text-white"><Coin /></el-icon>
          </div>
          <div>
            <span class="text-xl font-bold text-white tracking-tight">表结构管理</span>
            <p class="text-xs text-[#6b8db5]">Database Designer</p>
          </div>
        </div>
        <div v-else class="w-10 h-10 rounded-xl bg-gradient-to-br from-cyan-400 to-blue-500 flex items-center justify-center shadow-lg shadow-cyan-500/30">
          <el-icon :size="22" class="text-white"><Coin /></el-icon>
        </div>
      </div>

      <!-- 菜单 -->
      <nav class="flex-1 py-6 overflow-y-auto">
        <div v-if="!isCollapse" class="px-6 mb-4">
          <span class="text-xs font-semibold text-[#6b8db5] uppercase tracking-wider">导航菜单</span>
        </div>
        <ul class="space-y-2 px-4">
          <li v-for="item in menuItems" :key="item.path">
            <button
              :class="[
                'w-full flex items-center gap-4 px-4 py-3.5 rounded-xl transition-all duration-200',
                activeMenu === item.path 
                  ? 'bg-gradient-to-r from-cyan-500/20 to-blue-500/20 text-cyan-400 border-l-4 border-cyan-400 shadow-lg shadow-cyan-500/10' 
                  : 'text-[#a8c5e2] hover:bg-[#1a3a5c] hover:text-white border-l-4 border-transparent'
              ]"
              @click="navigateTo(item.path)"
            >
              <el-icon :size="22"><component :is="item.icon" /></el-icon>
              <span v-if="!isCollapse" class="text-sm font-medium">{{ item.title }}</span>
            </button>
          </li>
        </ul>
      </nav>

      <!-- 折叠按钮 -->
      <div class="p-4 border-t border-[#2a5080] shrink-0">
        <button
          class="w-full flex items-center justify-center gap-3 px-4 py-3 rounded-xl text-[#6b8db5] hover:bg-[#1a3a5c] hover:text-white transition-all"
          @click="toggleCollapse"
        >
          <el-icon :size="18">
            <ArrowLeft v-if="!isCollapse" />
            <ArrowRight v-else />
          </el-icon>
          <span v-if="!isCollapse" class="text-sm">收起菜单</span>
        </button>
      </div>
    </aside>

    <!-- 主内容区 -->
    <main class="flex-1 flex flex-col min-h-screen overflow-hidden">
      <!-- 顶部栏 -->
      <header class="h-20 flex items-center justify-between px-8 shrink-0 header-container">
        <div class="flex items-center gap-4">
          <el-breadcrumb separator="/">
            <el-breadcrumb-item :to="{ path: '/' }">
              <span class="text-[#6b8db5]">首页</span>
            </el-breadcrumb-item>
            <el-breadcrumb-item>
              <span class="text-white font-medium">{{ route.meta.title || '页面' }}</span>
            </el-breadcrumb-item>
          </el-breadcrumb>
        </div>
        <div class="flex items-center gap-4">
          <!-- 用户信息 -->
          <el-dropdown trigger="click">
            <div class="flex items-center gap-2 cursor-pointer hover:bg-[#1a3a5c] py-1 px-2 rounded-lg transition-colors">
              <el-avatar :size="32" :src="userStore.userInfo.avatar" class="border border-[#38bdf8]/30">
                <el-icon><UserFilled /></el-icon>
              </el-avatar>
              <span class="text-white text-sm font-medium">{{ userStore.userInfo.name }}</span>
            </div>
            <template #dropdown>
              <el-dropdown-menu class="!bg-[#122a4a] !border-[#2a5080]">
                <el-dropdown-item class="!text-[#a8c5e2] hover:!text-white hover:!bg-[#1a3a5c]">
                  <el-icon><UserFilled /></el-icon>个人中心
                </el-dropdown-item>
                <el-dropdown-item divided @click="handleLogout" class="!text-red-400 hover:!bg-red-500/10">
                  <el-icon><SwitchButton /></el-icon>退出登录
                </el-dropdown-item>
              </el-dropdown-menu>
            </template>
          </el-dropdown>

          <el-tooltip content="设置" placement="bottom">
            <el-button :icon="Setting" circle class="!border-[#2a5080] !bg-[#122a4a] hover:!bg-[#1a3a5c] hover:!border-cyan-400" />
          </el-tooltip>
        </div>
      </header>

      <!-- 页面内容 -->
      <div class="flex-1 p-8 overflow-auto content-container">
        <RouterView />
      </div>
    </main>
  </div>
</template>

<style scoped>
.sidebar-container {
  background: linear-gradient(180deg, #0f2847 0%, #0a1628 100%);
  border-right: 1px solid #2a5080;
  box-shadow: 4px 0 20px rgba(0, 0, 0, 0.3);
}

.header-container {
  background: linear-gradient(90deg, rgba(18, 42, 74, 0.95) 0%, rgba(15, 40, 71, 0.9) 100%);
  backdrop-filter: blur(12px);
  border-bottom: 1px solid #2a5080;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.2);
}

.content-container {
  background: transparent;
}
</style>
