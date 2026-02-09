<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useUserStore } from '@/stores/userStore'
import { User, Lock, Coin } from '@element-plus/icons-vue'
import { ElMessage } from 'element-plus'

const router = useRouter()
const userStore = useUserStore()

const form = ref({
  username: '',
  password: ''
})

const loading = ref(false)

async function handleLogin() {
  if (!form.value.username || !form.value.password) {
    ElMessage.warning('请输入用户名和密码')
    return
  }

  loading.value = true
  try {
    const success = await userStore.login(form.value.username, form.value.password)
    if (success) {
      ElMessage.success('登录成功')
      router.push('/')
    } else {
      ElMessage.error('登录失败，请检查用户名和密码')
    }
  } catch (error) {
    console.error('登录错误:', error)
    ElMessage.error('登录失败')
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <div class="min-h-screen flex items-center justify-center relative overflow-hidden bg-[#0a1628]">
    <!-- 背景装饰 -->
    <div class="absolute top-0 left-0 w-full h-full overflow-hidden z-0">
      <div class="absolute top-[-10%] left-[-10%] w-[40%] h-[40%] rounded-full bg-blue-500/10 blur-[100px]"></div>
      <div class="absolute bottom-[-10%] right-[-10%] w-[40%] h-[40%] rounded-full bg-cyan-500/10 blur-[100px]"></div>
    </div>

    <!-- 登录卡片 -->
    <div class="relative z-10 w-full max-w-md p-8 glass rounded-2xl animate-fade-in-up">
      <div class="flex flex-col items-center mb-8">
        <div class="w-16 h-16 rounded-2xl bg-gradient-to-br from-cyan-400 to-blue-500 flex items-center justify-center shadow-lg shadow-cyan-500/30 mb-4">
          <el-icon :size="32" class="text-white"><Coin /></el-icon>
        </div>
        <h1 class="text-2xl font-bold text-white tracking-wide">表结构管理工具</h1>
        <p class="text-[#6b8db5] mt-2 text-sm">Database Structure Manager</p>
      </div>

      <form @submit.prevent="handleLogin" class="space-y-6">
        <div class="space-y-4">
          <el-input
            v-model="form.username"
            placeholder="用户名"
            size="large"
            :prefix-icon="User"
            class="custom-input"
          />
          <el-input
            v-model="form.password"
            type="password"
            placeholder="密码"
            size="large"
            :prefix-icon="Lock"
            show-password
            class="custom-input"
          />
        </div>

        <el-button
          type="primary"
          size="large"
          class="w-full btn-gradient !h-12 !text-lg !rounded-xl"
          :loading="loading"
          native-type="submit"
          @click="handleLogin"
        >
          {{ loading ? '登录中...' : '登 录' }}
        </el-button>
      </form>

      <div class="mt-8 text-center">
        <p class="text-[#6b8db5] text-xs">
          默认账号: admin / 任意密码
        </p>
      </div>
    </div>
  </div>
</template>

<style scoped>
.glass {
  background: linear-gradient(135deg, rgba(18, 42, 74, 0.8) 0%, rgba(15, 40, 71, 0.9) 100%);
  backdrop-filter: blur(20px);
  border: 1px solid rgba(56, 189, 248, 0.2);
  box-shadow: 0 20px 60px rgba(0, 0, 0, 0.4);
}

.animate-fade-in-up {
  animation: fadeInUp 0.6s ease-out;
}

@keyframes fadeInUp {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

:deep(.el-input__wrapper) {
  background: rgba(10, 22, 40, 0.6) !important;
  border-color: rgba(42, 80, 128, 0.5) !important;
  box-shadow: none !important;
  padding: 8px 16px;
}

:deep(.el-input__wrapper:hover) {
  border-color: rgba(56, 189, 248, 0.5) !important;
}

:deep(.el-input__wrapper.is-focus) {
  border-color: #38bdf8 !important;
  background: rgba(10, 22, 40, 0.8) !important;
  box-shadow: 0 0 0 1px rgba(56, 189, 248, 0.2) !important;
}

:deep(.el-input__inner) {
  color: #fff !important;
  height: auto !important;
}
</style>
