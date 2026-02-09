<script setup lang="ts">
import { ref, watch } from 'vue'
import {SysTables,SysTableGroups} from '@/api/project'

const props = defineProps<{
  visible: boolean
  mode: 'create' | 'edit' | 'copy'
  table?: SysTables | null
  groups: SysTableGroups[]
}>()

const emit = defineEmits<{
  'update:visible': [value: boolean]
  save: [data: {
    name: string
    description: string
    groupId: string
  }]
}>()

const formData = ref({
  name: '',
  description: '',
  groupId: '',
})

watch(() => props.visible, (newVal) => {
  if (newVal) {
    if (props.mode === 'edit' && props.table) {
      formData.value = {
        name: props.table.name,
        description: props.table.description,
        groupId: props.table.groupId,
      }
    } else {
      formData.value = {
        name: '',
        description: '',
        groupId: props.groups[0]?.id || '',
      }
    }
  }
})

function handleClose() {
  emit('update:visible', false)
}

function handleSave() {
  emit('save', formData.value)
}

const dialogTitle = () => {
  switch (props.mode) {
    case 'create': return '新建表'
    case 'edit': return '编辑表'
    default: return '表'
  }
}
</script>

<template>
  <el-dialog
    :model-value="visible"
    :title="dialogTitle()"
    width="550px"
    @update:model-value="$emit('update:visible', $event)"
  >
    <el-form :model="formData" label-position="top">
      <el-form-item label="表名（英文）" required>
        <el-input v-model="formData.name" placeholder="如：users" />
      </el-form-item>
      <el-form-item label="所属分组" required>
        <el-select v-model="formData.groupId" placeholder="选择分组" class="w-full">
          <el-option
            v-for="group in groups"
            :key="group.id"
            :label="group.name"
            :value="group.id"
          />
        </el-select>
      </el-form-item>
      <el-form-item label="描述">
        <el-input v-model="formData.description" type="textarea" :rows="2" placeholder="表的用途说明" />
      </el-form-item>
    </el-form>
    <template #footer>
      <el-button @click="handleClose">取消</el-button>
      <el-button type="primary" @click="handleSave">
        {{ mode === 'copy' ? '复制' : '保存' }}
      </el-button>
    </template>
  </el-dialog>
</template>
