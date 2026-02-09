<script setup lang="ts">
import { ref, watch } from 'vue'
import { SysDatabaseTypesRes,SysDatabaseFieldRelationsVM} from '@/api/databaseType'

const props = defineProps<{
  visible: boolean
  databaseType: SysDatabaseTypesRes|null
  fieldTypes: SysDatabaseFieldRelationsVM[]
}>()

const emit = defineEmits<{
  'update:visible': [value: boolean]
  save: [dbType: SysDatabaseTypesRes,mappings: SysDatabaseFieldRelationsVM[]]
}>()

const mappingFormData = ref<SysDatabaseFieldRelationsVM[]>([])

watch(() => props.visible, (newVal) => {
  if (newVal && props.databaseType) {
    mappingFormData.value = []

    mappingFormData.value = JSON.parse(JSON.stringify(props.fieldTypes));
  }
})

function handleClose() {
  emit('update:visible', false)
}

function handleSave() {
  emit('save', props.databaseType || {} as SysDatabaseTypesRes,mappingFormData.value)
}
</script>

<template>
  <el-dialog
    :model-value="visible"
    :title="`编辑字段类型映射 - ${databaseType?.name}`"
    width="800px"
    @update:model-value="$emit('update:visible', $event)"
  >
    <div class="space-y-4">
      <el-alert type="info" :closable="false" show-icon>
        设置各字段类型在「{{ databaseType?.name }}」数据库中对应的SQL类型
      </el-alert>
      
      <div class="max-h-96 overflow-y-auto">
        <el-table :data="mappingFormData" stripe size="small">
          <el-table-column prop="fieldName" label="字段类型" width="150" fixed />
          <el-table-column label="SQL类型" width="130">
            <template #default="{ row }">
              <el-input 
                v-model="row.dataType" 
                placeholder="如 VARCHAR"
                size="small"
              />
            </template>
          </el-table-column>
          <el-table-column label="长度" width="100">
            <template #default="{ row }">
              <el-input-number 
                v-model="row.length" 
                :min="0"
                size="small"
                :controls="false"
                controls-position="right"
                class="w-full"
              />
            </template>
          </el-table-column>
          <el-table-column label="精度" width="100">
            <template #default="{ row }">
              <el-input-number 
                v-model="row.precision" 
                :min="0"
                size="small"
                :controls="false"
                controls-position="right"
                class="w-full"
              />
            </template>
          </el-table-column>
          <el-table-column label="默认值" min-width="150">
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
