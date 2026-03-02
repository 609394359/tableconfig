<script setup lang="ts">
import { Edit, Delete, Setting } from '@element-plus/icons-vue'

import {SysFieldTypesRes} from '@/api/fieldType'

defineProps<{
  fieldType: SysFieldTypesRes
}>()

defineEmits<{
  edit: [fieldType: SysFieldTypesRes]
  delete: [fieldType: SysFieldTypesRes]
  mapping: [fieldType: SysFieldTypesRes]
}>()
</script>

<template>
  <div class="glass rounded-xl p-5 card-hover group">
    <div class="flex items-center justify-between mb-4">
      <div class="w-12 h-12 rounded-xl bg-gradient-to-br from-cyan-500/20 to-blue-500/20 flex items-center justify-center border border-cyan-500/30">
        <el-icon :size="24" class="text-cyan-400"><Memo /></el-icon>
      </div>
      <div class="flex gap-1 opacity-0 group-hover:opacity-100 transition-opacity">
        <el-tooltip content="数据库映射">
          <el-button :icon="Setting" size="small" circle text @click="$emit('mapping', fieldType)" />
        </el-tooltip>
        <el-button :icon="Edit" size="small" circle text @click="$emit('edit', fieldType)" />
        <el-button :icon="Delete" size="small" circle text type="danger" @click="$emit('delete', fieldType)" />
      </div>
    </div>

    <!-- 名称和描述 -->
    <h3 class="text-lg font-semibold text-white mb-2">{{ fieldType.name }}</h3>
    <p class="text-[#6b8db5] text-sm mb-4 line-clamp-2">{{ fieldType.description || '暂无描述' }}</p>

    <!-- 映射预览 -->
    <div class="pt-4 border-t border-[#2a5080]">
      <div class="text-xs text-[#6b8db5] mb-2">数据库映射示例：</div>
      <div class="flex flex-wrap gap-1.5">
        <el-tag 
              v-for="db in fieldType.relations?.slice(0, 2)" 
              :key="db.databaseId"
              size="small" 
              class="!bg-[#1a3a5c] !border-[#2a5080] !text-[#a8c5e2]"
            >
              {{ db.databaseName }}: {{ db.dataTypeString }}
            </el-tag>
            <el-tag 
              v-if="fieldType.relations && fieldType.relations.length > 2" 
              size="small"
              class="!bg-[#1a3a5c] !border-[#2a5080] !text-[#6b8db5]"
            >
              +{{ fieldType.relations.length - 2 }}
            </el-tag>

      </div>
    </div>
  </div>
</template>

<style scoped>
.line-clamp-2 {
  display: -webkit-box;
  -webkit-line-clamp: 2;
  line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}
</style>
