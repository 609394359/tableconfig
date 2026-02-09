import { defineStore } from 'pinia'
import { ref } from 'vue'
import * as authApi from '@/api/auth'
import type { UserTokenVM } from '@/api/auth'

export const useUserStore = defineStore('user', () => {
    // 状态
    const token = ref<string>(localStorage.getItem('token') || '')
    const userInfo = ref<{
        id?: string
        code?: string
        name: string
        avatar: string
    }>({
        id: undefined,
        code: localStorage.getItem('userCode') || '',
        name: localStorage.getItem('userName') || '',
        avatar: 'https://cube.elemecdn.com/0/88/03b0d39583f48206768a7534e55bcpng.png'
    })
    const loading = ref(false)

    // 登录
    async function login(userCode: string, password: string): Promise<boolean> {
        loading.value = true
        try {
            const tokenResult = await authApi.login({ userCode, password })
            token.value = tokenResult

            // 持久化
            localStorage.setItem('token', tokenResult)

            // 获取用户信息
            await fetchCurrentUser()

            return true
        } catch (error) {
            console.error('登录失败:', error)
            return false
        } finally {
            loading.value = false
        }
    }

    // 获取当前用户信息
    async function fetchCurrentUser(): Promise<UserTokenVM | null> {
        if (!token.value) return null

        try {
            const user = await authApi.getCurrentUser()
            userInfo.value = {
                id: user.id,
                code: user.code,
                name: user.name,
                avatar: 'https://cube.elemecdn.com/0/88/03b0d39583f48206768a7534e55bcpng.png'
            }
            localStorage.setItem('userName', user.name)
            localStorage.setItem('userCode', user.code)
            return user
        } catch (error) {
            console.error('获取用户信息失败:', error)
            return null
        }
    }

    // 退出
    async function logout(): Promise<void> {
        try {
            await authApi.logout()
        } catch (error) {
            console.error('登出请求失败:', error)
        } finally {
            // 无论请求是否成功，都清除本地状态
            token.value = ''
            userInfo.value = {
                id: undefined,
                code: '',
                name: '',
                avatar: 'https://cube.elemecdn.com/0/88/03b0d39583f48206768a7534e55bcpng.png'
            }
            localStorage.removeItem('token')
            localStorage.removeItem('userName')
            localStorage.removeItem('userCode')
        }
    }

    // 检查是否已登录
    function isLoggedIn(): boolean {
        return !!token.value
    }

    return {
        token,
        userInfo,
        loading,
        login,
        logout,
        fetchCurrentUser,
        isLoggedIn
    }
})
