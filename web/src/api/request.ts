import axios, { type AxiosInstance, type AxiosResponse, type InternalAxiosRequestConfig } from 'axios'
import { ElMessage } from 'element-plus'

// API 响应基础结构
export interface ApiResult<T = unknown> {
    code: number
    message: string
    data: T
}

// 创建 axios 实例
const request: AxiosInstance = axios.create({
    baseURL: import.meta.env.VITE_APP_BASE_API,
    timeout: 30000,
    headers: {
        'Content-Type': 'application/json'
    }
})

// 请求拦截器 - 添加 Token
request.interceptors.request.use(
    (config: InternalAxiosRequestConfig) => {
        const token = localStorage.getItem('token')
        if (token) {
            config.headers.Token = token
        }
        return config
    },
    (error) => {
        return Promise.reject(error)
    }
)

// 响应拦截器 - 统一处理响应
request.interceptors.response.use(
    (response: AxiosResponse<ApiResult>) => {
        const res = response.data

        // 根据业务 code 判断请求是否成功
        if (res.code === 0 || res.code === 200) {
            return res.data as unknown as AxiosResponse
        }

        // 业务错误
        ElMessage.error(res.message || '请求失败')

        // Token 失效
        if (res.code === 401 || res.code === 403) {
            localStorage.removeItem('token')
            localStorage.removeItem('userName')
            window.location.href = '/login'
        }

        return Promise.reject(new Error(res.message || '请求失败'))
    },
    (error) => {
        // 网络错误
        let message = '网络错误，请稍后重试'
        if (error.response) {
            switch (error.response.status) {
                case 401:
                    message = '未授权，请重新登录'
                    localStorage.removeItem('token')
                    localStorage.removeItem('userName')
                    window.location.href = '/login'
                    break
                case 403:
                    message = '拒绝访问'
                    break
                case 404:
                    message = '请求地址不存在'
                    break
                case 500:
                    message = '服务器内部错误'
                    break
                default:
                    message = error.response.data?.message || '请求失败'
            }
        }
        ElMessage.error(message)
        return Promise.reject(error)
    }
)

export default request
