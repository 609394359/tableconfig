<script setup lang="ts">
import { FolderOpened, Coin, Edit, Delete } from '@element-plus/icons-vue'
import {SysProjectsRes} from '@/api/project'

defineProps<{
  project: SysProjectsRes
}>()

defineEmits<{
  click: [project: SysProjectsRes]
  edit: [project: SysProjectsRes]
  delete: [project: SysProjectsRes]
}>()
</script>

<template>
  <div
    class="glass rounded-xl p-6 card-hover cursor-pointer group"
    @click="$emit('click', project)"
  >
    <div class="flex items-start justify-between mb-4">
      <div class="w-12 h-12 rounded-xl bg-gradient-to-br from-blue-500/20 to-purple-500/20 flex items-center justify-center border border-blue-500/30">
        <el-icon :size="24" class="text-blue-400"><FolderOpened /></el-icon>
      </div>
      <div class="flex gap-2 opacity-0 group-hover:opacity-100 transition-opacity" @click.stop>
        <el-button :icon="Edit" size="small" circle text @click="$emit('edit', project)" />
        <el-button :icon="Delete" size="small" circle text type="danger" @click="$emit('delete', project)" />
      </div>
    </div>

    <h3 class="text-lg font-semibold text-white mb-2">{{ project.name }}</h3>
    <p class="text-dark-400 text-sm mb-4 line-clamp-2">{{ project.description || '暂无描述' }}</p>

    <div class="flex items-center gap-4 text-sm">
      <div class="flex items-center gap-1.5 text-dark-400">
        <el-icon><Coin /></el-icon>
        <span>{{ project.databaseName }}</span>
      </div>
      <div class="text-dark-500">
        {{ project.groupCount }} 个分组 · {{ project.tableCount }} 张表
      </div>
    </div>

    <div class="mt-4 pt-4 border-t border-dark-700 text-xs text-dark-500">
      更新于 {{ project.updateTime }}
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
