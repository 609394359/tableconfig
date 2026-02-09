<script setup lang="ts">
import { type Component } from 'vue'
import { FolderOpened } from '@element-plus/icons-vue'

defineProps<{
  icon?: Component
  message: string
  hint?: string
  actionText?: string
}>()

defineEmits<{
  action: []
}>()
</script>

<template>
  <div class="col-span-full flex flex-col items-center justify-center py-16 text-dark-500">
    <el-icon :size="48" class="mb-4">
      <component :is="icon || FolderOpened" />
    </el-icon>
    <p class="text-lg">{{ message }}</p>
    <p v-if="hint" class="text-sm mt-1">{{ hint }}</p>
    <el-button v-if="actionText" type="primary" class="mt-4" @click="$emit('action')">
      {{ actionText }}
    </el-button>
  </div>
</template>
