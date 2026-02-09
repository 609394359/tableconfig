<script setup lang="ts">
import { ref, watch } from 'vue'
import { QuestionFilled } from '@element-plus/icons-vue'
import { SysDatabaseTypesRes} from '@/api/databaseType'

const props = defineProps<{
  visible: boolean
  mode: 'create' | 'edit'
  databaseType?: SysDatabaseTypesRes | null
}>()

const emit = defineEmits<{
  'update:visible': [value: boolean]
  save: [data: Partial<SysDatabaseTypesRes>]
}>()

const formData = ref<Partial<SysDatabaseTypesRes>>({
  name: '',
  version: '',
  description: '',
  createScripts: ''
})

watch(() => props.visible, (newVal) => {
  if (newVal) {
    if (props.mode === 'edit' && props.databaseType) {
      formData.value = { ...props.databaseType }
    } else {
      formData.value = {
        name: '',
        version: '',
        description: '',
        createScripts: ''
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
</script>

<template>
  <el-dialog
    :model-value="visible"
    :title="mode === 'create' ? '新建数据库类型' : '编辑数据库类型'"
    width="800px"
    @update:model-value="$emit('update:visible', $event)"
  >
    <el-form :model="formData" label-position="top" class="space-y-4">
      <el-form-item label="数据库名称" required>
        <el-input v-model="formData.name" placeholder="如 MySQL" />
      </el-form-item>
      <el-form-item label="版本">
        <el-input v-model="formData.version" placeholder="如 8.0+" />
      </el-form-item>
      <el-form-item label="描述">
        <el-input v-model="formData.description" type="textarea" :rows="2" placeholder="数据库的描述说明" />
      </el-form-item>
      <el-form-item label="建表脚本模板 (JavaScript)" class="!mb-0">
        <template #label>
          <div class="flex items-center gap-1">
            <span>建表脚本模板 (JavaScript)</span>
            <el-tooltip placement="top" width="300">
              <template #content>
                <div class="max-w-xs">
                  <p class="mb-2">自定义生成 CREATE TABLE SQL 的逻辑。可用变量：</p>
                  <ul class="list-disc pl-4 mb-2">
                    <li>tableNam: 表名称</li>
                    <li>tableDes: 表描述</li>
                    <li>fields: 字段列表 </li>
                    <li>field.name: 字段名称</li>
                    <li>field.dbTypeValue: 数据库类型</li>
                    <li>field.isPrimaryKey: 主键</li>
                    <li>field.isIdentity: 自增</li>
                    <li>field.isUnique: 唯一</li>
                    <li>field.isNull: 可空</li>
                    <li>field.defaultValue: 默认值</li>
                    <li>field.des: 字段描述</li>
                    <li>keys: 主键列表 </li>
                    <li>identitys: 自增列表 </li>
                    <li>uniques: 唯一列列表 </li>
                  </ul>
                  <p>应返回生成的 SQL 字符串。</p>
                </div>
              </template>
              <el-icon class="text-dark-400 cursor-help"><QuestionFilled /></el-icon>
            </el-tooltip>
          </div>
        </template>
        <el-input 
          v-model="formData.createScripts" 
          type="textarea" 
          :rows="6" 
          class="font-mono text-sm"
          placeholder="// 示例：
return `CREATE TABLE ${table.name} (
${fields.map(f => `  ${f.name} ${f.des}`).join(',\n')}
);`" 
        />
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
