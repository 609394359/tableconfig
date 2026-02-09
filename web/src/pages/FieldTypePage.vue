<script setup lang="ts">
import { ref,reactive, computed, onMounted } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { Plus, Setting } from '@element-plus/icons-vue'


import * as fieldTypeApi from '@/api/fieldType'
import {SysFieldTypesRes,SysDatabaseFieldRelationsVM,SysFieldTypesSaveReq,SysFieldDatabasesSaveReq} from '@/api/fieldType'


// 组件导入
import PageHeader from '@/components/common/PageHeader.vue'
import SearchBar from '@/components/common/SearchBar.vue'
import EmptyState from '@/components/common/EmptyState.vue'
import FieldTypeCard from '@/components/field-type/FieldTypeCard.vue'
import FieldTypeForm from '@/components/field-type/FieldTypeForm.vue'
import DatabaseMappingDialog from '@/components/field-type/DatabaseMappingDialog.vue'


const state = reactive({
    fieldList: [] as SysFieldTypesRes[],
    editingType: null as SysFieldTypesRes | null,
    editingDatabaseTypes: [] as SysDatabaseFieldRelationsVM[],
})

// 对话框状态
const dialogVisible = ref(false)
const dialogMode = ref<'create' | 'edit'>('create')

const mappingDialogVisible = ref(false)

const searchQuery = ref('')
const viewMode = ref<'grid' | 'list'>('grid')

// 页面加载时从 API 获取数据
onMounted(async () => {
  await Promise.all([
    await loadDatabaseTypes()
  ])
})



async function loadDatabaseTypes() {
  const list = await fieldTypeApi.getList();
  state.fieldList = list;
}
async function loadDatabaseList(id:string) {
  return await fieldTypeApi.getDatabaseList(id);
}


// 过滤字段类型
const filteredFieldTypes = computed(() => {
  if (!searchQuery.value) return state.fieldList
  const query = searchQuery.value.toLowerCase()
  return state.fieldList.filter(db =>
    db.name.toLowerCase().includes(query) || db.description.toLowerCase().includes(query) 
  )
})


// 打开创建对话框
function openCreateDialog() {
  dialogMode.value = 'create'
  state.editingType = null
  dialogVisible.value = true
}

// 打开编辑对话框
function openEditDialog(fieldType: SysFieldTypesRes) {
  dialogMode.value = 'edit'
  state.editingType = fieldType
  dialogVisible.value = true
}

// 保存字段类型
async function saveFieldType(formData: Partial<SysFieldTypesRes>) {
  if (!formData.name?.trim()) {
    ElMessage.warning('请填写字段类型名称')
    return
  }

  if (dialogMode.value === 'create') {
    const newType: SysFieldTypesSaveReq = {
      name: formData.name!,
      description: formData.description||'',
    }
    await fieldTypeApi.save(newType)
    ElMessage.success('字段类型创建成功')
    await loadDatabaseTypes();

  } else if (state.editingType) {
    const newType: SysFieldTypesSaveReq = {
      id: state.editingType.id,
      name: formData.name!,
      description: formData.description||'',
    }
    await fieldTypeApi.save(newType)
    ElMessage.success('字段类型更新成功')
    await loadDatabaseTypes();
  }

  dialogVisible.value = false
}

// 删除字段类型
async function deleteFieldType(fieldType: SysFieldTypesRes) {
  try {
    await ElMessageBox.confirm(
      `确定要删除字段类型「${fieldType.name}」吗？`,
      '删除确认',
      { type: 'warning', confirmButtonText: '删除', cancelButtonText: '取消' }
    )
    await fieldTypeApi.del(fieldType.id)
    ElMessage.success('字段类型已删除')
    await loadDatabaseTypes();
  } catch {
    // 用户取消
  }
}

// 打开数据库映射编辑对话框
async function openMappingDialog(fieldType: SysFieldTypesRes) {
  
  const fieldList = await loadDatabaseList(fieldType.id)
  state.editingType = fieldType
  state.editingDatabaseTypes = fieldList
  mappingDialogVisible.value = true
}

// 保存字段类型映射
async function saveMappings(dbType: SysFieldTypesRes,mappings: SysDatabaseFieldRelationsVM[]) {
  const newType: SysFieldDatabasesSaveReq = {
      id: dbType.id,
      databases: mappings.map(f => {
        return {
          databaseId: f.databaseId,
          dataType: f.dataType,
          length: f.length,
          precision: f.precision,
          defaultValue: f.defaultValue
        }
      })

    }
  await fieldTypeApi.saveDatabaseRelations(newType)
  ElMessage.success('字段类型映射已保存')
  mappingDialogVisible.value = false
  await loadDatabaseTypes();
}

</script>

<template>
  <div class="space-y-6">
    <!-- 页面标题和操作栏 -->
    <PageHeader
      title="字段类型管理"
      subtitle="定义业务字段类型及其对应的数据库类型映射"
      button-text="新建字段类型"
      :button-icon="Plus"
      @action="openCreateDialog"
    />

    <!-- 筛选栏 -->
    <SearchBar
      v-model:search-query="searchQuery"
      v-model:view-mode="viewMode"
      placeholder="搜索字段类型..."
      :count="filteredFieldTypes.length"
      count-label="个字段类型"
      :show-view-toggle="true"
    />

    <!-- 字段类型卡片列表 (网格视图) -->
    <div v-if="viewMode === 'grid'" class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-5">
      <FieldTypeCard
        v-for="fieldType in filteredFieldTypes"
        :key="fieldType.id"
        :field-type="fieldType"
        @edit="openEditDialog"
        @delete="deleteFieldType"
        @mapping="openMappingDialog"
      />

      <!-- 空状态 -->
      <EmptyState
        v-if="filteredFieldTypes.length === 0"
        :icon="Setting"
        :message="searchQuery ? '未找到匹配的字段类型' : '暂无字段类型'"
        :hint="searchQuery ? '请尝试其他搜索关键词' : '点击下方按钮创建第一个字段类型'"
        :action-text="!searchQuery ? '创建字段类型' : undefined"
        @action="openCreateDialog"
      />
    </div>

    <!-- 字段类型列表 (列表视图) -->
    <div v-else class="glass rounded-xl overflow-hidden">
      <el-table :data="filteredFieldTypes" stripe>
        <el-table-column prop="name" label="字段类型" width="180">
          <template #default="{ row }">
            <span class="font-medium text-white">{{ row.name }}</span>
          </template>
        </el-table-column>
        <el-table-column prop="description" label="描述" min-width="200">
          <template #default="{ row }">
            <span class="text-[#6b8db5]">{{ row.description || '-' }}</span>
          </template>
        </el-table-column>
        <el-table-column label="数据库映射示例" min-width="200">
          <template #default="{ row }">
            <div class="flex flex-wrap gap-1.5">
              <el-tag 
              v-for="db in row.relations?.slice(0, 2)" 
              :key="db.databaseId"
              size="small" 
              class="!bg-[#1a3a5c] !border-[#2a5080] !text-[#a8c5e2]"
            >
              {{ db.databaseName }}: {{ db.dataTypeString }}
            </el-tag>
            <el-tag 
              v-if="row.relations && row.relations.length > 2" 
              size="small"
              class="!bg-[#1a3a5c] !border-[#2a5080] !text-[#6b8db5]"
            >
              +{{ row.relations.length - 2 }}
            </el-tag>
            </div>
          </template>
        </el-table-column>
        <el-table-column label="操作" width="180" fixed="right">
          <template #default="{ row }">
            <el-tooltip content="数据库映射">
              <el-button :icon="Setting" size="small" text @click="openMappingDialog(row)" />
            </el-tooltip>
            <el-button size="small" text @click="openEditDialog(row)">编辑</el-button>
            <el-button size="small" text type="danger" @click="deleteFieldType(row)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>
    </div>

    <!-- 创建/编辑字段类型对话框 -->
    <FieldTypeForm
      v-model:visible="dialogVisible"
      :mode="dialogMode"
      :field-type="state.editingType"
      @save="saveFieldType"
    />

    <!-- 数据库映射编辑对话框 -->
    <DatabaseMappingDialog
      v-model:visible="mappingDialogVisible"
      :field-type="state.editingType"
      :database-types="state.editingDatabaseTypes"
      @save="saveMappings"
    />
  </div>
</template>
