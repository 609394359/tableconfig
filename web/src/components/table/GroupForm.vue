<script setup lang="ts">
import { ref, watch } from 'vue'
import {SysTableGroups} from '@/api/project'

const props = defineProps<{
  visible: boolean
  mode: 'create' | 'edit'
  group?: SysTableGroups | null
}>()

const emit = defineEmits<{
  'update:visible': [value: boolean]
  save: [data: { name: string; description: string; sortIndex: number }]
}>()

const formData = ref({
  name: '',
  description: '',
  sortIndex: 0
})

watch(() => props.visible, (newVal) => {
  if (newVal) {
    if (props.mode === 'edit' && props.group) {
      formData.value = {
        name: props.group.name,
        description: props.group.description,
        sortIndex: props.group.sortIndex
      }
    } else {
      formData.value = { name: '', description: '', sortIndex: 0 }
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
    :title="mode === 'create' ? '新建分组' : '编辑分组'" 
    width="450px"
    @update:model-value="$emit('update:visible', $event)"
  >
    <el-form :model="formData" label-position="top">
      <el-form-item label="分组名称" required>
        <el-input v-model="formData.name" placeholder="如：用户模块" />
      </el-form-item>
      <el-form-item label="描述">
        <el-input v-model="formData.description" type="textarea" :rows="2" placeholder="分组描述" />
      </el-form-item>
      <el-form-item label="排序">
        <el-input-number v-model="formData.sortIndex" :min="0" :controls="false" align="left" class="w-full" />
      </el-form-item>
    </el-form>
    <template #footer>
      <el-button @click="handleClose">取消</el-button>
      <el-button type="primary" @click="handleSave">保存</el-button>
    </template>
  </el-dialog>
</template>
