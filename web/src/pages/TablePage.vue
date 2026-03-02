<script setup lang="ts">
import { ref,reactive, computed, onMounted,watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { ElMessage, ElMessageBox } from 'element-plus'
import { Plus, Back, FolderAdd, Folder, Grid, List, Search } from '@element-plus/icons-vue'


import * as projectApi from '@/api/project'
import {SysProjectsRes,SysTableGroups,SysTables,SysTableFields,SysFieldTypesRes,SysDatabaseTypes,SysTableFieldsSaveReq} from '@/api/project'


// 组件导入
import EmptyState from '@/components/common/EmptyState.vue'
import TableCard from '@/components/table/TableCard.vue'
import TableForm from '@/components/table/TableForm.vue'
import GroupManager from '@/components/table/GroupManager.vue'
import GroupForm from '@/components/table/GroupForm.vue'
import FieldEditor from '@/components/table/FieldEditor.vue'
import FieldForm from '@/components/table/FieldForm.vue'
import SQLPreviewDialog from '@/components/table/SQLPreviewDialog.vue'
import InheritFieldsDialog from '@/components/table/InheritFieldsDialog.vue'

const route = useRoute()
const router = useRouter()

const projectId = computed(() => route.params.projectId as string)
const searchQuery = ref('')
const selectedGroupId = ref<string>('')
const viewMode = ref<'grid' | 'list'>('grid')


const state = reactive({
    project: {} as SysProjectsRes,
    groupList: [] as SysTableGroups[],
    tableList: [] as SysTables[],
    fieldList: [] as SysTableFields[],
    fieldTypeList: [] as SysFieldTypesRes[],
    databaseTypeist: [] as SysDatabaseTypes[],

    editingTable: null as SysTables | null,
    editingGroup: null as SysTableGroups | null,
    editingField: null as SysTableFields | null,
})

// 对话框状态
const tableDialogVisible = ref(false)
const tableDialogMode = ref<'create' | 'edit' | 'copy'>('create')


const groupDialogVisible = ref(false)
const groupDialogMode = ref<'create' | 'edit'>('create')


const fieldDialogVisible = ref(false)
const fieldDialogMode = ref<'create' | 'edit'>('create')

const selectedTableId = ref<string | null>(null)
const selectedTableIdShow = ref<boolean | null>(false)
const sqlDialogVisible = ref(false)
const inheritDialogVisible = ref(false)


// 初始化
onMounted(async () => {
  await Promise.all([
    await loadList(),
    await loadFieldTypeList(),
    await loadDatabaseList(),
  ])
})

watch(() => selectedTableIdShow.value, (newVal) => {
  if (newVal == false) {
    loadList();
  }
})

async function loadList() {
  const data = await projectApi.getTableList(projectId.value);
  state.project = data.project;
  state.groupList = data.groups;
  state.tableList = data.tables
}
async function loadFieldTypeList() {
  const data = await projectApi.getFieldTypeList(state.project.databaseId);
  state.fieldTypeList = data
}
async function loadDatabaseList() {
  const data = await projectApi.getDatabaseList();
  state.databaseTypeist = data
}


// 过滤后的表
const filteredTables = computed(() => {
  if (!searchQuery.value && !selectedGroupId.value) return state.tableList
  const query = searchQuery.value?.toLowerCase()
  return state.tableList.filter(db =>
    (selectedGroupId.value ? db.groupId === selectedGroupId.value : true) && (query ? (db.name.toLowerCase().includes(query) || db.description.toLowerCase().includes(query)) : true)
  )
})



// 返回项目列表
function goBack() {
  router.push('/projects')
}

// ===== 分组操作 =====
function openCreateGroupDialog() {
  groupDialogMode.value = 'create'
  state.editingGroup = null
  groupDialogVisible.value = true
}

function openEditGroupDialog(group: SysTableGroups) {
  groupDialogMode.value = 'edit'
  state.editingGroup = group
  groupDialogVisible.value = true
}

async function saveGroup(data: { name: string; description: string; sortIndex: number }) {
  if (!data.name.trim()) {
    ElMessage.warning('请输入分组名称')
    return
  }

  if (groupDialogMode.value === 'create') {
    await projectApi.saveGroup({projectId: projectId.value, ...data})
    ElMessage.success('分组创建成功')
  } else if (state.editingGroup) {
    await projectApi.saveGroup({projectId: projectId.value,id: state.editingGroup.id, ...data})
    ElMessage.success('分组更新成功')
  }

  groupDialogVisible.value = false;
    await loadList();
}

async function deleteGroup(group: SysTableGroups) {
  try {
    await ElMessageBox.confirm(`确定要删除分组「${group.name}」吗？`, '删除确认', { type: 'warning' })
    await projectApi.delGroup(group.id)
    if (selectedGroupId.value === group.id) {
      selectedGroupId.value = ''
    }
    ElMessage.success('分组已删除')
    await loadList();
  } catch {
    // 用户取消
  }
}

// ===== 表操作 =====
function openCreateTableDialog() {
  tableDialogMode.value = 'create'
  state.editingTable = null
  tableDialogVisible.value = true
}

function openEditTableDialog(table: SysTables) {
  tableDialogMode.value = 'edit'
  state.editingTable = table
  tableDialogVisible.value = true
}

async function openCopyTableDialog(table: SysTables) {
  try {
    await ElMessageBox.confirm(
      `确定要复制表「${table.name}」吗？所有字段将一并复制。`,
      '删除复制',
      { type: 'warning' }
    )
    await projectApi.copyTable(table.id);
    ElMessage.success('复制成功')
    await loadList();
  } catch {
    // 用户取消
  }
}

async function saveTable(data: { name: string; description: string; groupId: string; }) {
  if (!data.name.trim() || !data.description.trim()) {
    ElMessage.warning('请填写表名和显示名称')
    return
  }
  if (!data.groupId) {
    ElMessage.warning('请选择所属分组')
    return
  }

  if (tableDialogMode.value === 'create') {
    await projectApi.saveTable({projectId: projectId.value, ...data})
    ElMessage.success('表创建成功')
  } else if (tableDialogMode.value === 'copy' && state.editingTable) {
    await projectApi.copyTable(state.editingTable.id);
    ElMessage.success('表复制成功')
  } else if (state.editingTable) {
    await projectApi.saveTable({projectId: projectId.value,id: state.editingTable.id, ...data}) 
    ElMessage.success('表更新成功')
  }

  tableDialogVisible.value = false;
  await loadList();
}

async function deleteTable(table: SysTables) {
  try {
    await ElMessageBox.confirm(
      `确定要删除表「${table.name}」吗？所有字段将一并删除。`,
      '删除确认',
      { type: 'warning' }
    )
    await projectApi.delTable(table.id)
    ElMessage.success('表已删除')
    await loadList();
  } catch {
    // 用户取消
  }
}

// ===== 字段操作 =====
async function openFieldEditor(table: SysTables) {
  const fieldList = await projectApi.getFieldList(state.project.databaseId, table.id)
  state.fieldList = fieldList
  selectedTableId.value = table.id
  selectedTableIdShow.value = true
  state.editingTable = table
}

function openCreateFieldDialog() {
  fieldDialogMode.value = 'create'
  state.editingField = null
  fieldDialogVisible.value = true
}

function openEditFieldDialog(field: SysTableFields) {
  fieldDialogMode.value = 'edit'
  state.editingField = field
  fieldDialogVisible.value = true
}

async function saveField(data: Partial<SysTableFields>) {
  if (!selectedTableId.value) return
  if (!data.name?.trim()) {
    ElMessage.warning('请填写字段名和显示名称')
    return
  }

  data.projectId = projectId.value;
  data.tableId = selectedTableId.value ?? '';
  if (fieldDialogMode.value === 'create') {
    await projectApi.saveField({...data as SysTableFieldsSaveReq})
    ElMessage.success('字段添加成功')
  } else if (state.editingField) {
    await projectApi.saveField({...data as SysTableFieldsSaveReq})
    ElMessage.success('字段更新成功')
  }

  const fieldList = await projectApi.getFieldList(state.project.databaseId, selectedTableId.value)
  state.fieldList = fieldList
  fieldDialogVisible.value = false
}

async function deleteField(field: SysTableFields) {
  if (!selectedTableId.value) return
  try {
    await ElMessageBox.confirm(`确定要删除字段「${field.name}」吗？`, '删除确认', { type: 'warning' })
    await projectApi.delField( field.id)
    ElMessage.success('字段已删除')
  } catch {
    // 用户取消
  }
}

function moveFieldUp(_field: SysTableFields,index: number) {
  if(index <= 0) return;
  const temp = state.fieldList[index];
  state.fieldList[index] = state.fieldList[index - 1];
  state.fieldList[index - 1] = temp;
  projectApi.sortFields({tableId:selectedTableId.value || '',ids:state.fieldList.map(f => f.id)});

}

function moveFieldDown(_field: SysTableFields,index: number) {
    if(!state.fieldList) return;
    if(index >= state.fieldList.length-1) return;
    const temp = state.fieldList[index];
    state.fieldList[index] = state.fieldList[index + 1];
    state.fieldList[index + 1] = temp;

    projectApi.sortFields({tableId:selectedTableId.value || '',ids:state.fieldList.map(f => f.id)});
  
}

// ===== 继承字段 =====
async function handleInherit(fieldIds: string[]) {
  if (!selectedTableId.value) return
  await projectApi.copyFields({tableId:state.editingTable?.id || '', ids:fieldIds})
  inheritDialogVisible.value = false;
  
  const fieldList = await projectApi.getFieldList(state.project.databaseId, selectedTableId.value)
  state.fieldList = fieldList
}

</script>

<template>
  <div class="space-y-6">
    <!-- 返回按钮和标题 -->
    <div class="flex items-center gap-4">
      <el-button :icon="Back" circle @click="goBack" />
      <div class="flex-1">
        <h1 class="text-2xl font-bold text-white">{{ state.project?.name || '项目' }}</h1>
        <p class="text-dark-400 mt-1">{{ state.project?.description || '管理表结构' }}</p>
      </div>
      <div class="flex gap-2">
        <el-button :icon="FolderAdd" @click="openCreateGroupDialog">新建分组</el-button>
        <el-button type="primary" :icon="Plus" class="btn-gradient" @click="openCreateTableDialog">
          新建表
        </el-button>
      </div>
    </div>

    <!-- 分组选择和筛选 -->
    <div class="glass rounded-xl p-4 flex flex-wrap items-center gap-4">
      <el-select v-model="selectedGroupId" placeholder="全部分组" clearable class="w-48">
        <template #prefix><el-icon><Folder /></el-icon></template>
        <el-option
          v-for="group in state.groupList"
          :key="group.id"
          :label="`${group.name} (${group.tableCount})`"
          :value="group.id"
        >
          <div class="flex items-center justify-between w-full">
            <span>{{ group.name }}</span>
            <span class="text-dark-500 text-xs">{{ group.tableCount }} 张表</span>
          </div>
        </el-option>
      </el-select>
      
      <el-input
        v-model="searchQuery"
        placeholder="搜索表名..."
        :prefix-icon="Search"
        clearable
        class="w-64"
      />

      <div class="flex-1"></div>

      <div class="flex items-center gap-2">
        <span class="text-dark-500 text-sm">{{ filteredTables.length }} 张表</span>
        <el-button-group>
          <el-button :icon="Grid" :type="viewMode === 'grid' ? 'primary' : 'default'" @click="viewMode = 'grid'" />
          <el-button :icon="List" :type="viewMode === 'list' ? 'primary' : 'default'" @click="viewMode = 'list'" />
        </el-button-group>
      </div>
    </div>

    <!-- 分组管理标签 -->
    <GroupManager
      v-if="state.groupList.length > 0"
      :groups="state.groupList"
      :selected-group-id="selectedGroupId"
      @update:selected-group-id="selectedGroupId = $event"
      @edit="openEditGroupDialog"
      @delete="deleteGroup"
    />

    <!-- 表列表 - 网格视图 -->
    <div v-if="viewMode === 'grid'" class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      <TableCard
        v-for="table in filteredTables"
        :key="table.id"
        :table="table"
        @click="openFieldEditor"
        @edit="openEditTableDialog"
        @delete="deleteTable"
        @copy="openCopyTableDialog"
      />

      <!-- 空状态 -->
      <EmptyState
        v-if="filteredTables.length === 0"
        :icon="Grid"
        :message="searchQuery ? '未找到匹配的表' : '暂无数据表'"
        :action-text="!searchQuery ? '创建第一张表' : undefined"
        @action="openCreateTableDialog"
      />
    </div>

    <!-- 表列表 - 列表视图 -->
    <div v-else class="glass rounded-xl overflow-hidden">
      <el-table :data="filteredTables" stripe>
        <el-table-column prop="name" label="表名" width="180">
          <template #default="{ row }">
            <code class="text-blue-400 text-sm">{{ row.name }}</code>
          </template>
        </el-table-column>
        <el-table-column prop="displayName" label="显示名称" width="150" />
        <el-table-column label="分组" width="120">
          <template #default="{ row }">
            <span class="text-dark-400">{{ row.groupName }}</span>
          </template>
        </el-table-column>
        <el-table-column prop="description" label="描述" min-width="200">
          <template #default="{ row }">
            <span class="text-dark-400">{{ row.description || '-' }}</span>
          </template>
        </el-table-column>
        <el-table-column label="字段数" width="100" align="center">
          <template #default="{ row }">
            {{ row.fieldCount }}
          </template>
        </el-table-column>
        <el-table-column label="操作" width="180" fixed="right">
          <template #default="{ row }">
            <el-button size="small" text @click="openFieldEditor(row)">编辑字段</el-button>
            <el-button size="small" text @click="openCopyTableDialog(row)">复制</el-button>
            <el-button size="small" text @click="openEditTableDialog(row)">编辑</el-button>
            <el-button size="small" text type="danger" @click="deleteTable(row)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>
    </div>

    <!-- 分组表单对话框 -->
    <GroupForm
      v-model:visible="groupDialogVisible"
      :mode="groupDialogMode"
      :group="state.editingGroup"
      @save="saveGroup"
    />

    <!-- 表表单对话框 -->
    <TableForm
      v-model:visible="tableDialogVisible"
      :mode="tableDialogMode"
      :table="state.editingTable"
      :groups="state.groupList || []"
      @save="saveTable"
    />

    <!-- 字段编辑器抽屉 -->
    <el-drawer v-model="selectedTableIdShow" :title="state.editingTable?.name" size="70%" :with-header="true">
      <template #header>
        <div class="flex items-center justify-between w-full pr-4">
          <div>
            <h3 class="text-lg font-semibold">{{ state.editingTable?.name }}</h3>
            <code class="text-sm text-blue-400">{{ state.editingTable?.description }}</code>
          </div>
        </div>
      </template>

      <FieldEditor
        v-if="state.editingTable"
        :table="state.editingTable"
        :fields="state.fieldList"
        @add-field="openCreateFieldDialog"
        @edit-field="openEditFieldDialog"
        @delete-field="deleteField"
        @move-up="moveFieldUp"
        @move-down="moveFieldDown"
        @inherit="inheritDialogVisible = true"
        @generate-s-q-l="sqlDialogVisible = true"
      />
    </el-drawer>

    <!-- 字段表单对话框 -->
    <FieldForm
      v-model:visible="fieldDialogVisible"
      :mode="fieldDialogMode"
      :field="state.editingField"
      :field-types="state.fieldTypeList"
      @save="saveField"
    />

    <!-- SQL预览对话框 -->
    <SQLPreviewDialog
      v-model:visible="sqlDialogVisible"
      :table="state.editingTable"
      :database-types="state.databaseTypeist"
      :default-database-type-id="state.project.databaseId"
    />

    <!-- 继承字段对话框 -->
    <InheritFieldsDialog
      v-model:visible="inheritDialogVisible"
      :current-table-id="selectedTableId"
      :all-tables="state.tableList"
      :database-id="state.project.databaseId|| ''"
      @inherit="handleInherit"
    />
  </div>
</template>

<style scoped>
:deep(.el-drawer__body) {
  padding: 20px;
  background: #1e293b;
}

:deep(.el-drawer__header) {
  padding: 16px 20px;
  margin-bottom: 0;
  border-bottom: 1px solid #334155;
  background: #1e293b;
}
</style>
