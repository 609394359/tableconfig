<script setup lang="ts">
import { ref, watch } from 'vue'
import {SysFieldTypesRes,SysDatabaseFieldRelationsVM} from '@/api/fieldType'

const props = defineProps<{
  visible: boolean
  fieldType: SysFieldTypesRes | null
  databaseTypes: SysDatabaseFieldRelationsVM[]
}>()

const emit = defineEmits<{
  'update:visible': [value: boolean]
  save: [fieldType: SysFieldTypesRes,mappings: SysDatabaseFieldRelationsVM[]]
}>()

const mappingFormData = ref<SysDatabaseFieldRelationsVM[]>([])

watch(() => props.visible, (newVal) => {
  
  if (newVal && props.databaseTypes) {
    mappingFormData.value = []

    mappingFormData.value = JSON.parse(JSON.stringify(props.databaseTypes));
  }
})

function handleClose() {
  emit('update:visible', false)
}

function handleSave() {
  emit('save', props.fieldType || {} as SysFieldTypesRes,mappingFormData.value)
}
</script>

<template>
  <el-dialog
    :model-value="visible"
    :title="`编辑数据库映射 - ${fieldType?.name}`"
    width="750px"
    @update:model-value="$emit('update:visible', $event)"
  >
    <div class="space-y-4">
      <el-alert type="info" :closable="false" show-icon>
        为「{{ fieldType?.name }}」设置在各数据库中对应的SQL类型
      </el-alert>
      
      <div class="glass rounded-lg overflow-hidden">
        <el-table :data="mappingFormData" stripe>
          <el-table-column prop="databaseName" label="数据库" width="160">
            <template #default="{ row }">
              <span class="font-medium text-white">{{ row.databaseName }}</span>
            </template>
          </el-table-column>
          <el-table-column label="SQL类型" width="140">
            <template #default="{ row }">
              <el-input 
                v-model="row.dataType" 
                placeholder="如 VARCHAR"
                size="small"
              />
            </template>
          </el-table-column>
          <el-table-column label="长度" width="110">
            <template #default="{ row }">
              <el-input-number 
                v-model="row.length" 
                :min="0"
                size="small"
                controls-position="right"
                class="w-full"
              />
            </template>
          </el-table-column>
          <el-table-column label="精度" width="110">
            <template #default="{ row }">
              <el-input-number 
                v-model="row.precision" 
                :min="0"
                size="small"
                controls-position="right"
                class="w-full"
              />
            </template>
          </el-table-column>
          <el-table-column label="默认值" min-width="140">
            <template #default="{ row }">
              <el-input 
                v-model="row.defaultValue" 
                placeholder="默认值"
                size="small"
              />
            </template>
          </el-table-column>
        </el-table>
      </div>
    </div>
    <template #footer>
      <el-button @click="handleClose">取消</el-button>
      <el-button type="primary" @click="handleSave">保存映射</el-button>
    </template>
  </el-dialog>
</template>
