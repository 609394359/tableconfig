<script setup lang="ts">
import { ref, watch } from 'vue'
import {SysTableFields,SysFieldTypesRes} from '@/api/project'

const props = defineProps<{
  visible: boolean
  mode: 'create' | 'edit'
  field?: SysTableFields | null
  fieldTypes: SysFieldTypesRes[]
  defaultSortOrder?: number
}>()

const emit = defineEmits<{
  'update:visible': [value: boolean]
  save: [data: Partial<SysTableFields>]
}>()

const formData = ref<Partial<SysTableFields>>({
    id: "",
    tableId: "",
    fieldTypeId: "",
    name: "",
    description: "",
    defaultValue: undefined,
    isPrimaryKey: false,
    isIdentity: false,
    isUnique: false,
    isNull: false,
})

watch(() => props.visible, (newVal) => {
  if (newVal) {
    if (props.mode === 'edit' && props.field) {
      formData.value = { ...props.field }
    } else {
      formData.value = {
        tableId: "",
        fieldTypeId: "",
        name: "",
        description: "",
        defaultValue: undefined,
        isPrimaryKey: false,
        isIdentity: false,
        isUnique: false,
        isNull: false,
      }
    }
  }
})
function changeFieldType(e:any) {
  props.fieldTypes.map(ft => {
    if (ft.id === e) {
      formData.value.defaultValue = ft.defaultValue
    }
  })
}
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
    :title="mode === 'create' ? '添加字段' : '编辑字段'" 
    width="600px"
    @update:model-value="$emit('update:visible', $event)"
  >
    <el-form :model="formData" label-position="top">
      <el-form-item label="字段名" required>
          <el-input v-model="formData.name" placeholder="如：username" />
        </el-form-item>
      <el-form-item label="字段类型" required>
          <el-select v-model="formData.fieldTypeId" filterable class="w-full" @change = "changeFieldType">
            <el-option
              v-for="ft in fieldTypes"
              :key="ft.id"
              :label="ft.name"
              :value="ft.id"
            >
              <div class="flex items-center gap-2">
                <span>{{ ft.name }}</span>
                <span class="text-dark-500 text-xs">{{ ft.description }}</span>
              </div>
            </el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="默认值">
          <el-input v-model="formData.defaultValue" placeholder="覆盖字段类型默认值" />
        </el-form-item>
      <el-form-item label="注释">
        <el-input v-model="formData.description" placeholder="字段说明" />
      </el-form-item>
      <el-form-item label="约束">
        <div class="flex flex-wrap gap-4">
          <el-checkbox v-model="formData.isPrimaryKey">主键</el-checkbox>
          <el-checkbox v-model="formData.isIdentity">自增</el-checkbox>
          <el-checkbox v-model="formData.isUnique">唯一</el-checkbox>
          <el-checkbox v-model="formData.isNull">允许空值</el-checkbox>
        </div>
      </el-form-item>
    </el-form>
    <template #footer>
      <el-button @click="handleClose">取消</el-button>
      <el-button type="primary" @click="handleSave">保存</el-button>
    </template>
  </el-dialog>
</template>
