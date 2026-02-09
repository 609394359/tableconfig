<script setup lang="ts">
import { ref, watch } from 'vue'
import { Coin } from '@element-plus/icons-vue'
import {SysProjectsRes,SysDatabaseTypes} from '@/api/project'

const props = defineProps<{
  visible: boolean
  mode: 'create' | 'edit'
  project?: SysProjectsRes | null
  databases?: SysDatabaseTypes[]
}>()

const emit = defineEmits<{
  'update:visible': [value: boolean]
  save: [data: { id: string; name: string; description: string; databaseId: string }]
}>()

const formData = ref({
  id: '',
  name: '',
  description: '',
  databaseId: ''
})

watch(() => props.visible, (newVal) => {
  if (newVal) {
    if (props.mode === 'edit' && props.project) {
      formData.value.id = props.project.id;
      formData.value.name = props.project.name;
      formData.value.description = props.project.description;
      formData.value.databaseId = props.project.databaseId;
    } else {
      formData.value.id = "";
      formData.value.name = "";
      formData.value.description = "";
      formData.value.databaseId = "";
    }
  }
})

function handleClose() {
  emit('update:visible', false)
}

function handleSave() {
  emit('save', formData.value)
}
</script>

<template>
  <el-dialog
    :model-value="visible"
    :title="mode === 'create' ? '新建项目' : '编辑项目'"
    width="500px"
    class="dark-dialog"
    @update:model-value="$emit('update:visible', $event)"
  >
    <el-form :model="formData" label-position="top" class="space-y-4">
      <el-form-item label="项目名称" required>
        <el-input v-model="formData.name" placeholder="请输入项目名称" />
      </el-form-item>
      <el-form-item label="项目描述" required>
        <el-input
          v-model="formData.description"
          type="textarea"
          :rows="3"
          placeholder="请输入项目描述"
        />
      </el-form-item>
      <el-form-item label="数据库类型" required>
        <el-select v-model="formData.databaseId" placeholder="选择数据库类型" class="w-full">
          <el-option
            v-for="db in databases"
            :key="db.id"
            :label="db.name"
            :value="db.id"
          >
            <div class="flex items-center gap-2">
              <el-icon><Coin /></el-icon>
              <span>{{ db.name }}</span>
              <span class="text-dark-500 text-xs">{{ db.version }}</span>
            </div>
          </el-option>
        </el-select>
      </el-form-item>
    </el-form>
    <template #footer>
      <el-button @click="handleClose">取消</el-button>
      <el-button type="primary" @click="handleSave">
        {{ mode === 'create' ? '创建' : '保存' }}
      </el-button>
    </template>
  </el-dialog>
</template>

<style scoped>
:deep(.el-dialog) {
  --el-dialog-bg-color: #1e293b;
  --el-dialog-border-color: #334155;
}

:deep(.el-dialog__header) {
  border-bottom: 1px solid #334155;
}

:deep(.el-dialog__footer) {
  border-top: 1px solid #334155;
}
</style>
