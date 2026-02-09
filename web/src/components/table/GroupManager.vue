<script setup lang="ts">
import { ref } from 'vue'
import { Folder, Edit } from '@element-plus/icons-vue'
import {SysTableGroups} from '@/api/project'

const props = defineProps<{
  groups: SysTableGroups[]
  selectedGroupId: string
}>()

const emit = defineEmits<{
  'update:selectedGroupId': [value: string]
  create: []
  edit: [group: SysTableGroups]
  delete: [group: SysTableGroups]
}>()

// 分组表单对话框
const dialogVisible = ref(false)
const dialogMode = ref<'create' | 'edit'>('create')
const editingGroup = ref<SysTableGroups | null>(null)
const formData = ref({
  name: '',
  description: '',
  sortIndex: 0
})

function openCreateDialog() {
  dialogMode.value = 'create'
  editingGroup.value = null
  formData.value = { name: '', description: '', sortIndex: 0 }
  dialogVisible.value = true
}

function openEditDialog(group: SysTableGroups) {
  dialogMode.value = 'edit'
  editingGroup.value = group
  formData.value = {
    name: group.name,
    description: group.description,
    sortIndex: group.sortIndex
  }
  dialogVisible.value = true
}

defineExpose({
  openCreateDialog,
  openEditDialog
})
</script>

<template>
  <div class="flex flex-wrap gap-2">
    <div
      v-for="group in props.groups"
      :key="group.id"
      :class="[
        'px-4 py-2 rounded-lg flex items-center gap-2 cursor-pointer transition-all',
        selectedGroupId === group.id
          ? 'bg-blue-500/20 text-blue-400 border border-blue-500/30'
          : 'bg-dark-800 text-dark-300 border border-dark-700 hover:border-dark-500'
      ]"
      @click="$emit('update:selectedGroupId', selectedGroupId === group.id ? '' : group.id)"
    >
      <el-icon><Folder /></el-icon>
      <span>{{ group.name }}</span>
      <span class="text-xs opacity-60">({{ group.tableCount }})</span>
      <el-dropdown trigger="click" @click.stop>
        <el-button size="small" text class="!ml-2"><el-icon><Edit /></el-icon></el-button>
        <template #dropdown>
          <el-dropdown-menu>
            <el-dropdown-item @click="$emit('edit', group)">编辑分组</el-dropdown-item>
            <el-dropdown-item @click="$emit('delete', group)" divided>
              <span class="text-red-400">删除分组</span>
            </el-dropdown-item>
          </el-dropdown-menu>
        </template>
      </el-dropdown>
    </div>
  </div>
</template>
