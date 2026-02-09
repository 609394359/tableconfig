import request from './request'

// 请求类型
export interface LoginReq {
    userCode: string
    password: string
}

// 响应类型
export interface UserTokenVM {
    id: string
    code: string
    name: string
    token: string
}

// 登录
export function login(data: LoginReq): Promise<string> {
    return request.post('/api/Auth/Login', data)
}

// 登出
export function logout(): Promise<void> {
    return request.post('/api/Auth/LoginOut')
}

// 获取当前用户信息
export function getCurrentUser(): Promise<UserTokenVM> {
    return request.get('/api/Auth/Get')
}
