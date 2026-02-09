<script setup lang="ts">
import { Plus, Edit, Delete, Top, Bottom, Connection, DocumentCopy } from '@element-plus/icons-vue'
import {SysTables,SysTableFields} from '@/api/project'

defineProps<{
  table: SysTables | null
  fields: SysTableFields[]
}>()

defineEmits<{
  addField: []
  editField: [field: SysTableFields]
  deleteField: [field: SysTableFields]
  moveUp: [field: SysTableFields,index: number]
  moveDown: [field: SysTableFields,index: number]
  inherit: []
  generateSQL: []
}>()
</script>

<template>
  <div class="space-y-4">
    <!-- 操作按钮 -->
    <div class="flex justify-end gap-2">
      <el-button :icon="DocumentCopy" @click="$emit('generateSQL')">生成SQL</el-button>
      <el-button :icon="Connection" @click="$emit('inherit')">继承字段</el-button>
      <el-button type="primary" :icon="Plus" @click="$emit('addField')">添加字段</el-button>
    </div>

    <!-- 字段表格 -->
    <el-table v-if="table" :data="fields" stripe row-key="id" border style="width: 100%">
      <el-table-column type="index" width="50" label="#" align="center" />
      <el-table-column prop="name" label="字段名" min-width="130">
        <template #default="{ row }">
          <div class="flex items-center gap-2">
            <code class="text-cyan-400 font-medium">{{ row.name }}</code>
          </div>
        </template>
      </el-table-column>
      <el-table-column label="类型" min-width="120">
        <template #default="{ row }">
          <code class="text-emerald-400 text-xs bg-emerald-500/15 px-2 py-1 rounded font-medium">
           {{ row.fieldTypeName }}
          </code>
        </template>
      </el-table-column>
      <el-table-column label="默认数据库" min-width="120">
        <template #default="{ row }">
          <code class="text-emerald-400 text-xs bg-emerald-500/15 px-2 py-1 rounded font-medium">
           {{ row.databaseName }}
          </code>
        </template>
      </el-table-column>
      <el-table-column label="默认类型" min-width="120">
        <template #default="{ row }">
          <code class="text-emerald-400 text-xs bg-emerald-500/15 px-2 py-1 rounded font-medium">
           {{ row.relation.dataTypeString }}
          </code>
        </template>
      </el-table-column>
      <el-table-column label="约束" min-width="200">
        <template #default="{ row }">
          <div class="flex items-center gap-1 whitespace-nowrap">
            <el-tag v-if="row.isPrimaryKey" size="small" type="warning">主键</el-tag>
            <el-tag v-if="row.isIdentity" size="small" type="info">自增</el-tag>
            <el-tag v-if="row.isUnique" size="small">唯一</el-tag>
            <el-tag v-if="!row.isNull" size="small" type="danger">非空</el-tag>
            <span v-if="!row.isPrimaryKey && !row.isIdentity && !row.isUnique && row.isNull" class="text-[#6b8db5]">-</span>
          </div>
        </template>
      </el-table-column>
      <el-table-column prop="defaultValue" label="默认值" min-width="80">
        <template #default="{ row }">
          <span class="text-[#a8c5e2]">{{ row.defaultValue || '-' }}</span>
        </template>
      </el-table-column>
      <el-table-column prop="description" label="注释" min-width="140">
        <template #default="{ row }">
          <span class="text-[#6b8db5]">{{ row.description || '-' }}</span>
        </template>
      </el-table-column>
      <el-table-column label="操作" width="200" align="center">
        <template #default="{ row, $index }">
          <el-button :icon="Top" size="small" text :disabled="$index === 0" @click="$emit('moveUp', row, $index)" />
          <el-button :icon="Bottom" size="small" text :disabled="$index === fields.length - 1" @click="$emit('moveDown', row, $index)" />
          <el-button :icon="Edit" size="small" text @click="$emit('editField', row)" />
          <el-button :icon="Delete" size="small" text type="danger" @click="$emit('deleteField', row)" />
        </template>
      </el-table-column>
    </el-table>
  </div>
</template>
