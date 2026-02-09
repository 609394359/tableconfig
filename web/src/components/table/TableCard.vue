<script setup lang="ts">
import { Grid, Edit, Delete, CopyDocument } from '@element-plus/icons-vue'
import {SysTables} from '@/api/project'

defineProps<{
  table: SysTables
}>()

defineEmits<{
  click: [table: SysTables]
  edit: [table: SysTables]
  delete: [table: SysTables]
  copy: [table: SysTables]
}>()
</script>

<template>
  <div
    class="glass rounded-xl p-6 card-hover group cursor-pointer"
    @click="$emit('click', table)"
  >
    <div class="flex items-start justify-between mb-4">
      <div class="w-10 h-10 rounded-lg bg-gradient-to-br from-emerald-500/20 to-teal-500/20 flex items-center justify-center border border-emerald-500/30">
        <el-icon :size="20" class="text-emerald-400"><Grid /></el-icon>
      </div>
      <div class="flex gap-1 opacity-0 group-hover:opacity-100 transition-opacity" @click.stop>
        <el-tooltip content="复制表">
          <el-button :icon="CopyDocument" size="small" circle text @click="$emit('copy', table)" />
        </el-tooltip>
        <el-button :icon="Edit" size="small" circle text @click="$emit('edit', table)" />
        <el-button :icon="Delete" size="small" circle text type="danger" @click="$emit('delete', table)" />
      </div>
    </div>

    <div class="mb-2">
      <code class="text-blue-400 text-sm bg-blue-500/10 px-2 py-0.5 rounded">{{ table.name }}</code>
    </div>
    <h3 class="text-lg font-semibold text-white mb-1">{{ table.description }}</h3>
    <p class="text-dark-500 text-xs mb-3">{{ table.groupName }}</p>

    <div class="flex items-center gap-3 text-sm text-dark-400">
      <span>{{ table.fieldCount }} 个字段</span>
    </div>
  </div>
</template>
