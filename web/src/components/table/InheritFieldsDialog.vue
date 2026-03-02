<script setup lang="ts">
import { ref, watch, computed } from 'vue'
import {SysTables,SysTableFieldsRes} from '@/api/project'
import * as projectApi from '@/api/project'

const props = defineProps<{
  visible: boolean
  databaseId:string | ''
  currentTableId: string | null
  allTables: SysTables[]
}>()

const emit = defineEmits<{
  'update:visible': [value: boolean]
  inherit: [fieldIds: string[]]
}>()

const sourceTableId = ref('')
const selectedFields = ref<string[]>([])
const fieldList = ref<SysTableFieldsRes[]>([])

// 可用的源表（排除当前表）
const availableTables = computed(() => {
  return props.allTables.filter(t => t.id !== props.currentTableId)
})

// 源表对象
const sourceTable = computed(() => {
  if (!sourceTableId.value) return null
  return props.allTables.find(t => t.id === sourceTableId.value)
})

watch(() => props.visible, (newVal) => {
  if (newVal) {
    sourceTableId.value = ''
    selectedFields.value = []
  }
})

// 当源表变化时，重置选中的字段
watch(sourceTableId, () => {
  selectedFields.value = []
})

async function getFieldList() {
  if (!sourceTableId.value) return
  const tablefieldList = await projectApi.getFieldList(props.databaseId, sourceTableId.value);
  fieldList.value = tablefieldList;
}

function handleClose() {
  emit('update:visible', false)
}

function handleInherit() {
  if (sourceTableId.value && selectedFields.value.length > 0) {
    emit('inherit', selectedFields.value)
  }
}
</script>

<template>
  <el-dialog
    :model-value="visible"
    title="继承字段"
    width="650px"
    @update:model-value="$emit('update:visible', $event)"
  >
    <div class="space-y-4">
      <el-alert type="info" :closable="false" show-icon>
        从其他表继承字段定义，继承的字段可以在当前表中独立修改
      </el-alert>
      
      <el-form-item label="选择源表">
        <el-select v-model="sourceTableId" placeholder="选择要继承的表" class="w-full" filterable @change="getFieldList">
          <el-option
            v-for="table in availableTables"
            :key="table.id"
            :label="`${table.description} (${table.name})`"
            :value="table.id"
          >
            <div class="flex items-center justify-between w-full">
              <span>{{ table.name }}</span>
              <span class="text-dark-500 text-xs">{{ table.fieldCount }} 个字段</span>
            </div>
          </el-option>
        </el-select>
      </el-form-item>
      
      <div v-if="sourceTable" class="space-y-2">
        <div class="flex items-center justify-between">
          <span class="text-dark-400">选择要继承的字段：</span>
          <el-checkbox
            :model-value="selectedFields.length === fieldList.length"
            :indeterminate="selectedFields.length > 0 && selectedFields.length < fieldList.length"
            @change="selectedFields = $event ? fieldList.map(f => f.id) : []"
          >
            全选
          </el-checkbox>
        </div>
         <el-checkbox-group v-model="selectedFields" class="grid grid-cols-2 gap-2">
            <el-checkbox
              v-for="field in fieldList"
              :key="field.id"
              :label="field.id"
              class="!m-0 p-3 rounded border border-dark-700 hover:border-dark-500"
            >
              <div class="flex items-center gap-2">
                <code class="text-blue-400 text-xs">{{ field.name }}</code>
                <span class="text-dark-300">{{ field.description }}</span>
                <code class="text-green-400 text-xs">{{ field.fieldTypeName }}</code>
              </div>
            </el-checkbox>
          </el-checkbox-group>
      </div>
      
      <div v-else-if="availableTables.length === 0" class="text-center py-8 text-dark-500">
        暂无其他表可供继承
      </div>
    </div>
    <template #footer>
      <el-button @click="handleClose">取消</el-button>
      <el-button 
        type="primary" 
        :disabled="!sourceTableId || selectedFields.length === 0"
        @click="handleInherit"
      >
        继承 {{ selectedFields.length || '' }} 个字段
      </el-button>
    </template>
  </el-dialog>
</template>
