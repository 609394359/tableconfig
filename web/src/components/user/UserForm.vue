<script setup lang="ts">
import { ref, watch } from 'vue'
import { SysUsers} from '@/api/user'

const props = defineProps<{
  visible: boolean
  mode: 'create' | 'edit'
  user?: SysUsers | null
}>()

const emit = defineEmits<{
  'update:visible': [value: boolean]
  save: [data: Partial<SysUsers>]
}>()

const formData = ref<Partial<SysUsers>>({
  userCode: '',
  userName: '',
  password: undefined,
})

watch(() => props.visible, (newVal) => {
  if (newVal) {
    if (props.mode === 'edit' && props.user) {
      formData.value = { ...props.user }
    } else {
      formData.value = {
        userCode: '',
        userName: '',
        password: '',
      }
    }
  }
})

function handleClose() {
  emit('update:visible', false)
}

function handleSave() {
  emit('save', formData.value)
}
</script>

<template>
  <el-dialog
    :model-value="visible"
    :title="mode === 'create' ? '新建账号' : '编辑账号'"
    width="800px"
    @update:model-value="$emit('update:visible', $event)"
  >
    <el-form :model="formData" label-position="top" class="space-y-4">
      <el-form-item label="账号" required>
        <el-input v-model="formData.userCode" placeholder="请输入登录账号" />
      </el-form-item>
      <el-form-item label="名称" required>
        <el-input v-model="formData.userName" placeholder="请输入用户名称" />
      </el-form-item>
      <el-form-item label="登录密码">
        <el-input v-model="formData.password" placeholder="请输入登录密码" type="password" />
      </el-form-item>
    </el-form>
    <template #footer>
      <el-button @click="handleClose">取消</el-button>
      <el-button type="primary" @click="handleSave">
        {{ mode === 'create' ? '创建' : '保存' }}
      </el-button>
    </template>
  </el-dialog>
</template>
