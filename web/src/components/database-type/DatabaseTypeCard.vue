<script setup lang="ts">
import { Coin, Edit, Delete, Setting } from '@element-plus/icons-vue'
import { SysDatabaseTypesRes} from '@/api/databaseType'

defineProps<{
  dbType: SysDatabaseTypesRes
}>()

defineEmits<{
  edit: [dbType: SysDatabaseTypesRes]
  delete: [dbType: SysDatabaseTypesRes]
  mapping: [dbType: SysDatabaseTypesRes]
}>()
</script>

<template>
  <div class="glass rounded-xl p-6 card-hover group">
    <div class="flex items-start justify-between mb-4">
      <div class="w-12 h-12 rounded-xl bg-gradient-to-br from-cyan-500/20 to-blue-500/20 flex items-center justify-center border border-cyan-500/30">
        <el-icon :size="24" class="text-cyan-400"><Coin /></el-icon>
      </div>
      <div class="flex gap-2 opacity-0 group-hover:opacity-100 transition-opacity">
        <el-tooltip content="编辑字段类型映射">
          <el-button :icon="Setting" size="small" circle text @click="$emit('mapping', dbType)" />
        </el-tooltip>
        <el-button :icon="Edit" size="small" circle text @click="$emit('edit', dbType)" />
        <el-button :icon="Delete" size="small" circle text type="danger" @click="$emit('delete', dbType)" />
      </div>
    </div>

    <div class="flex items-center gap-2 mb-2">
      <h3 class="text-lg font-semibold text-white">{{ dbType.name }}</h3>
      <span v-if="dbType.version" class="text-xs text-dark-500 bg-dark-700 px-2 py-0.5 rounded">
        {{ dbType.version }}
      </span>
    </div>
    <p class="text-dark-400 text-sm mb-4 line-clamp-2">{{ dbType.description || '暂无描述' }}</p>

    <div class="pt-4 border-t border-dark-700">
      <div class="flex items-center justify-between">
        <span class="text-xs text-dark-500">已配置字段类型映射</span>
        <span class="text-sm text-blue-400">{{ dbType.fieldCount }} 个</span>
      </div>
    </div>
  </div>
</template>

<style scoped>
.line-clamp-2 {
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}
</style>
