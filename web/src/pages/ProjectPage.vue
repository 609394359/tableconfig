<script setup lang="ts">
import { ref,reactive, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { ElMessage, ElMessageBox } from 'element-plus'
import { Plus, FolderOpened } from '@element-plus/icons-vue'

import * as projectApi from '@/api/project'
import {SysProjectsRes,SysDatabaseTypes} from '@/api/project'

// 组件导入
import PageHeader from '@/components/common/PageHeader.vue'
import SearchBar from '@/components/common/SearchBar.vue'
import EmptyState from '@/components/common/EmptyState.vue'
import ProjectCard from '@/components/project/ProjectCard.vue'
import ProjectForm from '@/components/project/ProjectForm.vue'

const router = useRouter()

const state = reactive({
    projectList: [] as SysProjectsRes[],
    databaseList: [] as SysDatabaseTypes[],
    editingType: null as SysProjectsRes | null,
})


// 页面加载时从 API 获取数据
onMounted(async () => {
  await Promise.all([
    await loadList(),
    await loadDatabaseTypes(),
  ])
})

async function loadList() {
  const list = await projectApi.getList();
  state.projectList = list;
}
async function loadDatabaseTypes() {
  const list = await projectApi.getDatabaseList();
  state.databaseList = list;
}

const searchQuery = ref('')
const dialogVisible = ref(false)
const dialogMode = ref<'create' | 'edit'>('create')

// 过滤项目
const filteredProjects = computed(() => {
  if (!searchQuery.value) return state.projectList
  const query = searchQuery.value.toLowerCase()
  return state.projectList.filter(db =>
    db.name.toLowerCase().includes(query) || db.description.toLowerCase().includes(query) 
  )
})


// 打开创建对话框
function openCreateDialog() {
  dialogMode.value = 'create'
  state.editingType = null
  dialogVisible.value = true
}

// 打开编辑对话框
function openEditDialog(project: SysProjectsRes) {
  dialogMode.value = 'edit'
  state.editingType = project
  dialogVisible.value = true
}

// 保存项目
async function saveProject(data: {  id: string;name: string; description: string; databaseId: string }) {
  if (!data.name.trim()) {
    ElMessage.warning('请输入项目名称')
    return
  }

  if (dialogMode.value === 'create') {
    await projectApi.save(data)
    ElMessage.success('项目创建成功')
  } else if (state.editingType) {
    await projectApi.save({...data, id: state.editingType.id})
    ElMessage.success('项目更新成功')
  }

  dialogVisible.value = false
  await loadList();
}

// 删除项目
async function deleteProject(project: SysProjectsRes) {
  try {
    await ElMessageBox.confirm(
      `确定要删除项目「${project.name}」吗？此操作将删除项目下的所有表结构，且不可恢复。`,
      '删除确认',
      { type: 'warning', confirmButtonText: '删除', cancelButtonText: '取消' }
    )
    await projectApi.del(project.id)
    ElMessage.success('项目已删除')
    await loadList();
  } catch {
    // 用户取消
  }
}

// 进入项目
function enterProject(project: SysProjectsRes) {
  router.push(`/projects/${project.id}/tables`)
}
</script>

<template>
  <div class="space-y-6">
    <!-- 页面标题和操作栏 -->
    <PageHeader
      title="项目管理"
      subtitle="管理您的数据库设计项目"
      button-text="新建项目"
      :button-icon="Plus"
      @action="openCreateDialog"
    />

    <!-- 搜索栏 -->
    <SearchBar
      v-model:search-query="searchQuery"
      placeholder="搜索项目名称或描述..."
    />

    <!-- 项目列表 -->
    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      <ProjectCard
        v-for="project in filteredProjects"
        :key="project.id"
        :project="project"
        @click="enterProject"
        @edit="openEditDialog"
        @delete="deleteProject"
      />

      <!-- 空状态 -->
      <EmptyState
        v-if="filteredProjects.length === 0"
        :icon="FolderOpened"
        :message="searchQuery ? '未找到匹配的项目' : '暂无项目'"
        :action-text="!searchQuery ? '创建第一个项目' : undefined"
        @action="openCreateDialog"
      />
    </div>

    <!-- 创建/编辑对话框 -->
    <ProjectForm
      v-model:visible="dialogVisible"
      :mode="dialogMode"
      :project="state.editingType"
      :databases="state.databaseList"
      @save="saveProject"
    />
  </div>
</template>
