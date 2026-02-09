import request from './request'

// 用户类型
export interface SysUsers {
    id: string
    userCode: string
    userName: string
    password?: string
    createTime?: string
    updateTime?: string
}

// 保存请求
export interface SysUsersSaveReq {
    id?: string
    userCode: string
    userName: string
    password?: string
}

// 获取列表
export function getList(): Promise<SysUsers[]> {
    return request.get('/api/User/List')
}

// 获取详情
export function getById(id: string): Promise<SysUsers> {
    return request.get('/api/User/Get', { params: { id } })
}

// 保存（新增/更新）
export function save(data: SysUsersSaveReq): Promise<SysUsers> {
    return request.post('/api/User/Save', data)
}

// 删除
export function del(id: string): Promise<void> {
    return request.post('/api/User/Del', { id })
}

// 重置
export function reset(): Promise<void> {
    return request.post('/api/User/Reset')
}
