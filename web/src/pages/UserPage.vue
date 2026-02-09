<script setup lang="ts">
import { ref,reactive, computed, onMounted } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { Plus } from '@element-plus/icons-vue'

import * as userApi from '@/api/user.ts'
import { SysUsers,SysUsersSaveReq} from '@/api/user'

// 组件导入
import PageHeader from '@/components/common/PageHeader.vue'
import SearchBar from '@/components/common/SearchBar.vue'
import UserForm from '@/components/user/UserForm.vue'


const searchQuery = ref('')
const dialogVisible = ref(false)
const dialogMode = ref<'create' | 'edit'>('create')


const state = reactive({
    userList: [] as SysUsers[],
    editingUser: null as SysUsers | null,
})



// 页面加载时从 API 获取数据
onMounted(async () => {
  await Promise.all([
    loadUserList(),
  ])
})


async function loadUserList() {
  const list = await userApi.getList();
  state.userList = list;
}



// 过滤数据库类型
const filteredUserList = computed(() => {
  if (!searchQuery.value) return state.userList
  const query = searchQuery.value.toLowerCase()
  return state.userList.filter(db =>
    db.userCode.toLowerCase().includes(query) || db.userName.toLowerCase().includes(query) 
  )
})

// 打开创建对话框
function openCreateDialog() {
  dialogMode.value = 'create'
  state.editingUser = null
  dialogVisible.value = true
}

// 打开编辑对话框
function openEditDialog(dbType: SysUsers) {
  dialogMode.value = 'edit'
  state.editingUser = dbType
  dialogVisible.value = true
}

// 保存数据库类型
async function saveDatabaseType(formData: Partial<SysUsers>) {
  if (!formData.userCode?.trim() || !formData.userName?.trim()) {
    ElMessage.warning('请填写必要字段')
    return
  }

  if (dialogMode.value === 'create') {
    const newType: SysUsersSaveReq = {
      userCode: formData.userCode!,
      userName: formData.userName!,
      password: formData.password!,
    }
    await userApi.save(newType)
    ElMessage.success('账号创建成功')
  } else if (state.editingUser) {
    const newType: SysUsersSaveReq = {
      id: state.editingUser.id,
      userCode: formData.userCode!,
      userName: formData.userName!,
      password: formData.password,
    }
    await userApi.save(newType)
    ElMessage.success('账号更新成功')
  }

  dialogVisible.value = false
  await loadUserList();
}

// 删除
async function deleteUser(dbType: SysUsers) {
  try {
    await ElMessageBox.confirm(
      `确定要删除账号「${dbType.userCode}」吗？`,
      '删除确认',
      { type: 'warning', confirmButtonText: '删除', cancelButtonText: '取消' }
    )
    await userApi.del(dbType.id)
    ElMessage.success('账号已删除')
    
    await loadUserList();
  } catch {
    // 用户取消
  }
}


</script>

<template>
  <div class="space-y-6">
    <!-- 页面标题和操作栏 -->
    <PageHeader
      title="账号管理"
      subtitle="管理系统登录账号"
      button-text="新建账号"
      :button-icon="Plus"
      @action="openCreateDialog"
    />

    <!-- 搜索栏 -->
    <SearchBar
      v-model:search-query="searchQuery"
      placeholder="搜索账号..."
      :count="filteredUserList.length"
      count-label="个账号"
    />

    <!-- 字段类型列表 (列表视图) -->
    <div class="glass rounded-xl overflow-hidden">
      <el-table :data="filteredUserList" stripe>
        <el-table-column prop="userCode" label="账号" width="180">
          <template #default="{ row }">
            <span class="font-medium text-white">{{ row.userCode }}</span>
          </template>
        </el-table-column>
        <el-table-column prop="userName" label="名称" min-width="200">
          <template #default="{ row }">
            <span class="text-[#6b8db5]">{{ row.userName || '-' }}</span>
          </template>
        </el-table-column>
        <el-table-column label="操作" width="180" fixed="right">
          <template #default="{ row }">
            <el-button size="small" text @click="openEditDialog(row)">编辑</el-button>
            <el-button size="small" text type="danger" @click="deleteUser(row)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>
    </div>

    <!-- 创建/编辑对话框 -->
    <UserForm
      v-model:visible="dialogVisible"
      :mode="dialogMode"
      :user="state.editingUser"
      @save="saveDatabaseType"
    />

  </div>
</template>
