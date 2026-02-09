<script setup lang="ts">
import { Search, Grid, List } from '@element-plus/icons-vue'

const searchQuery = defineModel<string>('searchQuery', { default: '' })
const viewMode = defineModel<'grid' | 'list'>('viewMode')

defineProps<{
  placeholder?: string
  count?: number
  countLabel?: string
  showViewToggle?: boolean
}>()
</script>

<template>
  <div class="glass rounded-xl p-4 flex flex-wrap items-center gap-4">
    <el-input
      v-model="searchQuery"
      :placeholder="placeholder || '搜索...'"
      :prefix-icon="Search"
      clearable
      class="w-64"
    />
    
    <slot name="filters" />
    
    <div class="flex-1"></div>
    
    <div class="flex items-center gap-2">
      <span v-if="count !== undefined" class="text-dark-500 text-sm">
        共 {{ count }} {{ countLabel || '个' }}
      </span>
      <el-button-group v-if="showViewToggle && viewMode !== undefined">
        <el-button :icon="Grid" :type="viewMode === 'grid' ? 'primary' : 'default'" @click="viewMode = 'grid'" />
        <el-button :icon="List" :type="viewMode === 'list' ? 'primary' : 'default'" @click="viewMode = 'list'" />
      </el-button-group>
      <slot name="extra" />
    </div>
  </div>
</template>
