<script setup lang="ts">
import { ref, watch } from 'vue'
import { CopyDocument } from '@element-plus/icons-vue'
import { ElMessage } from 'element-plus'
import { SysTables, SysDatabaseTypes } from '@/api/project'
import * as projectApi from '@/api/project'

const props = defineProps<{
  visible: boolean
  table: SysTables | null
  databaseTypes: SysDatabaseTypes[]
  defaultDatabaseTypeId?: string
}>()

const emit = defineEmits<{
  'update:visible': [value: boolean]
}>()

const selectedDatabaseType = ref('')
const generatedSQL = ref('')

watch(() => props.visible, (newVal) => {
  if (newVal && props.table) {
    selectedDatabaseType.value = props.defaultDatabaseTypeId || ''
    regenerateSQL()
  }
})

watch(selectedDatabaseType, () => {
  if (props.table && props.visible) {
    regenerateSQL()
  }
})

async function regenerateSQL() {
  if (props.table) {
    const data = await projectApi.sqlPreview(selectedDatabaseType.value, props.table.id);
    if (!data.createScripts) {
      generatedSQL.value = `数据库「${data.databaseName}」还未配置建表脚本！！！！`;
      return;
    }

    try {
      // 1. 获取用户在文本框输入的代码字符串
      const funcBody = `return  ` + data.createScripts;
      const dynamicFn = new Function('tableName', 'tableDes', 'fields', 'keys', 'identitys', 'uniques', funcBody);

      // 3. 准备实参数据
      const arg1 = data.tableName;
      const arg2 = data.tableDescription;
      const arg3 = data.fields;
      const arg4 = data.primaryKeys; // 暂时用 items 充当 keys
      const arg5 = data.identitys; // 暂时用 items 充当 keys
      const arg6 = data.uniques; // 暂时用 items 充当 keys

      // 4. 执行函数 (注意：不要传数组，而是按顺序传参数)
      const result = dynamicFn(arg1, arg2, arg3, arg4, arg5, arg6);

      generatedSQL.value = result;
      return;
    }
    catch (e: any) {
      generatedSQL.value = e.message;
    }

  }
}


function copySQL() {
  navigator.clipboard.writeText(generatedSQL.value)
  ElMessage.success('SQL 已复制到剪贴板')
}

function handleClose() {
  emit('update:visible', false)
}
</script>

<template>
  <el-dialog :model-value="visible" :title="`生成 SQL - ${table?.name}`" width="800px"
    @update:model-value="$emit('update:visible', $event)">
    <div class="space-y-4">
      <div class="flex items-center gap-4">
        <span class="text-dark-400">数据库类型：</span>
        <el-select v-model="selectedDatabaseType" class="w-48">
          <el-option v-for="db in databaseTypes" :key="db.id" :label="db.name" :value="db.id" />
        </el-select>
        <div class="flex-1"></div>
        <el-button :icon="CopyDocument" @click="copySQL">复制 SQL</el-button>
      </div>

      <div class="glass rounded-lg p-4 font-mono text-sm overflow-auto max-h-96">
        <pre class="text-emerald-400 ">{{ generatedSQL }}</pre>
      </div>
    </div>
    <template #footer>
      <el-button @click="handleClose">关闭</el-button>
    </template>
  </el-dialog>
</template>
