import request from './request'

// 数据库类型
export interface SysDatabaseTypes {
    id: string
    name: string
    version?: string
    description: string
    createScripts?: string
    createTime?: string
    updateTime?: string
}

// 保存请求
export interface SysDatabaseTypesSaveReq {
    id?: string
    name: string
    version?: string
    description: string
    createScripts?: string
}

// 列表响应
export interface SysDatabaseTypesRes extends SysDatabaseTypes {
    // 额外字段
    fieldCount?: number
}

// 字段关系
export interface SysDatabaseFieldRelationsVM {
    fieldId: string
    fieldName: string
    fieldDescription: string
    dataType: string
    length?: number
    precision?: number
    defaultValue?: string
}

// 保存字段关系请求
export interface SysDatabaseFieldsSaveReq {
    id: string
    fields: Array<{
        fieldId: string
        dataType: string
        length?: number
        precision?: number
        defaultValue?: string
    }>
}

// 获取列表
export function getList(): Promise<SysDatabaseTypesRes[]> {
    return request.get('/api/DatabaseType/List')
}

// 获取详情
export function getById(id: string): Promise<SysDatabaseTypes> {
    return request.get('/api/DatabaseType/Get', { params: { id } })
}

// 保存（新增/更新）
export function save(data: SysDatabaseTypesSaveReq): Promise<SysDatabaseTypes> {
    return request.post('/api/DatabaseType/Save', data)
}

// 删除
export function del(id: string): Promise<void> {
    return request.post('/api/DatabaseType/Del', { id })
}

// 获取字段关系列表
export function getFieldList(id: string): Promise<SysDatabaseFieldRelationsVM[]> {
    return request.get('/api/DatabaseType/Field/List', { params: { id } })
}

// 保存字段关系
export function saveFieldRelations(data: SysDatabaseFieldsSaveReq): Promise<void> {
    return request.post('/api/DatabaseType/Field/Save', data)
}
