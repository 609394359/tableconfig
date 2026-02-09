<script setup lang="ts">
import { ref, watch } from 'vue'
import {SysFieldTypesRes} from '@/api/fieldType'

const props = defineProps<{
  visible: boolean
  mode: 'create' | 'edit'
  fieldType?: SysFieldTypesRes | null
}>()

const emit = defineEmits<{
  'update:visible': [value: boolean]
  save: [data: { id:string;name: string; description: string; }]
}>()

const formData = ref({
  id: '',
  name: '',
  description: '',
  icon: 'Document'
})

watch(() => props.visible, (newVal) => {
  if (newVal) {
    if (props.mode === 'edit' && props.fieldType) {
      formData.value.id = props.fieldType.id;
      formData.value.name = props.fieldType.name;
      formData.value.description = props.fieldType.description;
    } else {
      formData.value.id = "";
      formData.value.name = "";
      formData.value.description = "";
    }
  }
})

function handleClose() {
  emit('update:visible', false)
}

function handleSave() {
  emit('save', {
    id: formData.value.id,
    name: formData.value.name,
    description: formData.value.description
  })
}
</script>

<template>
  <el-dialog
    :model-value="visible"
    :title="mode === 'create' ? '新建字段类型' : '编辑字段类型'"
    width="500px"
    @update:model-value="$emit('update:visible', $event)"
  >
    <el-form :model="formData" label-position="top" class="space-y-4">
      <el-form-item label="字段类型名称" required>
        <el-input v-model="formData.name" placeholder="如：用户名、手机号、金额" />
      </el-form-item>

      <el-form-item label="描述">
        <el-input v-model="formData.description" type="textarea" :rows="3" placeholder="字段类型的用途说明" />
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
