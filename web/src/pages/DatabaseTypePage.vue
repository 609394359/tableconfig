<script setup lang="ts">
import { ref,reactive, computed, onMounted } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { Plus, Coin } from '@element-plus/icons-vue'

import * as databaseTypeApi from '@/api/databaseType'
import { SysDatabaseTypesRes,SysDatabaseFieldRelationsVM ,SysDatabaseTypesSaveReq,SysDatabaseFieldsSaveReq} from '@/api/databaseType'

// 组件导入
import PageHeader from '@/components/common/PageHeader.vue'
import SearchBar from '@/components/common/SearchBar.vue'
import EmptyState from '@/components/common/EmptyState.vue'
import DatabaseTypeCard from '@/components/database-type/DatabaseTypeCard.vue'
import DatabaseTypeForm from '@/components/database-type/DatabaseTypeForm.vue'
import FieldTypeMappingDialog from '@/components/database-type/FieldTypeMappingDialog.vue'


const searchQuery = ref('')
const dialogVisible = ref(false)
const dialogMode = ref<'create' | 'edit'>('create')
// 字段类型映射编辑对话框
const mappingDialogVisible = ref(false)

const state = reactive({
    databaseList: [] as SysDatabaseTypesRes[],
    editingType: null as SysDatabaseTypesRes | null,
    editingDatabaseTypes: [] as SysDatabaseFieldRelationsVM[],
})



// 页面加载时从 API 获取数据
onMounted(async () => {
  await Promise.all([
    loadDatabaseTypes(),
  ])
})


async function loadDatabaseTypes() {
  const list = await databaseTypeApi.getList();
  state.databaseList = list;
}
async function loadFieldList(id:string) {
  return await databaseTypeApi.getFieldList(id);
}


// 过滤数据库类型
const filteredDatabaseList = computed(() => {
  if (!searchQuery.value) return state.databaseList
  const query = searchQuery.value.toLowerCase()
  return state.databaseList.filter(db =>
    db.name.toLowerCase().includes(query) 
  )
})

// 打开创建对话框
function openCreateDialog() {
  dialogMode.value = 'create'
  state.editingType = null
  dialogVisible.value = true
}

// 打开编辑对话框
function openEditDialog(dbType: SysDatabaseTypesRes) {
  dialogMode.value = 'edit'
  state.editingType = dbType
  dialogVisible.value = true
}

// 保存数据库类型
async function saveDatabaseType(formData: Partial<SysDatabaseTypesRes>) {
  if (!formData.name?.trim()) {
    ElMessage.warning('请填写必要字段')
    return
  }

  if (dialogMode.value === 'create') {
    const newType: SysDatabaseTypesSaveReq = {
      id: formData.id,
      name: formData.name!,
      version: formData.version || '',
      description: formData.description || '',
      createScripts: formData.createScripts || ''
    }
    await databaseTypeApi.save(newType)
    ElMessage.success('数据库类型创建成功')
  } else if (state.editingType) {
    const newType: SysDatabaseTypesSaveReq = {
      id: state.editingType.id,
      name: formData.name!,
      version: formData.version || '',
      description: formData.description || '',
      createScripts: formData.createScripts || ''
    }
    await databaseTypeApi.save(newType)
    ElMessage.success('数据库类型更新成功')
  }

  dialogVisible.value = false
  await loadDatabaseTypes();
}

// 删除数据库类型
async function deleteDatabaseType(dbType: SysDatabaseTypesRes) {
  try {
    await ElMessageBox.confirm(
      `确定要删除数据库类型「${dbType.name}」吗？`,
      '删除确认',
      { type: 'warning', confirmButtonText: '删除', cancelButtonText: '取消' }
    )
    await databaseTypeApi.del(dbType.id)
    ElMessage.success('数据库类型已删除')
    
    await loadDatabaseTypes();
  } catch {
    // 用户取消
  }
}

// 打开字段类型映射编辑对话框
async function openMappingDialog(dbType: SysDatabaseTypesRes) {
  const fieldList = await loadFieldList(dbType.id)
  state.editingType = dbType
  state.editingDatabaseTypes = fieldList
  mappingDialogVisible.value = true
}

// 保存字段类型映射
async function saveMappings(dbType: SysDatabaseTypesRes,mappings: SysDatabaseFieldRelationsVM[]) {
  const newType: SysDatabaseFieldsSaveReq = {
      id: dbType.id,
      fields: mappings.map(f => {
        return {
          fieldId: f.fieldId,
          dataType: f.dataType,
          length: f.length,
          precision: f.precision,
          defaultValue: f.defaultValue
        }
      })

    }
  await databaseTypeApi.saveFieldRelations(newType)
  ElMessage.success('字段类型映射已保存')
  mappingDialogVisible.value = false
  await loadDatabaseTypes();
}


</script>

<template>
  <div class="space-y-6">
    <!-- 页面标题和操作栏 -->
    <PageHeader
      title="数据库类型管理"
      subtitle="配置支持的数据库类型，管理字段类型映射"
      button-text="新建数据库类型"
      :button-icon="Plus"
      @action="openCreateDialog"
    />

    <!-- 搜索栏 -->
    <SearchBar
      v-model:search-query="searchQuery"
      placeholder="搜索数据库类型..."
      :count="filteredDatabaseList.length"
      count-label="个数据库类型"
    />

    <!-- 数据库类型卡片列表 -->
    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      <DatabaseTypeCard
        v-for="dbType in filteredDatabaseList"
        :key="dbType.id"
        :db-type="dbType"
        @edit="openEditDialog"
        @delete="deleteDatabaseType"
        @mapping="openMappingDialog"
      />

      <!-- 空状态 -->
      <EmptyState
        v-if="filteredDatabaseList.length === 0"
        :icon="Coin"
        :message="searchQuery ? '未找到匹配的数据库类型' : '暂无数据库类型'"
        :action-text="!searchQuery ? '添加数据库类型' : undefined"
        @action="openCreateDialog"
      />
    </div>

    <!-- 创建/编辑对话框 -->
    <DatabaseTypeForm
      v-model:visible="dialogVisible"
      :mode="dialogMode"
      :database-type="state.editingType"
      @save="saveDatabaseType"
    />

    <!-- 字段类型映射编辑对话框 -->
    <FieldTypeMappingDialog
      v-model:visible="mappingDialogVisible"
      :database-type="state.editingType"
      :field-types="state.editingDatabaseTypes"
      @save="saveMappings"
    />
  </div>
</template>
