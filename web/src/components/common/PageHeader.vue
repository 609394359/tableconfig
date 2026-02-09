<script setup lang="ts">
import { Plus } from '@element-plus/icons-vue'

defineProps<{
  title: string
  subtitle?: string
  buttonText?: string
  buttonIcon?: typeof Plus
}>()

defineEmits<{
  action: []
}>()
</script>

<template>
  <div class="flex items-center justify-between">
    <div>
      <h1 class="text-2xl font-bold text-white">{{ title }}</h1>
      <p v-if="subtitle" class="text-dark-400 mt-1">{{ subtitle }}</p>
    </div>
    <slot name="actions">
      <el-button 
        v-if="buttonText" 
        type="primary" 
        :icon="buttonIcon" 
        class="btn-gradient" 
        @click="$emit('action')"
      >
        {{ buttonText }}
      </el-button>
    </slot>
  </div>
</template>
