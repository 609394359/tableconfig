import request from './request'

// 字段类型
export interface SysFieldTypes {
    id: string
    name: string
    description: string
    createTime?: string
    updateTime?: string
}

// 保存请求
export interface SysFieldTypesSaveReq {
    id?: string
    name: string
    description: string
}

// 列表响应
export interface SysFieldTypesRes extends SysFieldTypes {
    // 额外字段
    databaseCount?: number
    relations?: SysDatabaseFieldRelationsVM[]
}

// 数据库关系
export interface SysDatabaseFieldRelationsVM {
    databaseId: string
    databaseName: string
    databaseDescription: string
    dataType: string
    dataTypeString: string
    length?: number
    precision?: number
    defaultValue?: string
}

// 保存数据库关系请求
export interface SysFieldDatabasesSaveReq {
    id: string
    databases: Array<{
        databaseId: string
        dataType: string
        length?: number
        precision?: number
        defaultValue?: string
    }>
}

// 获取列表
export function getList(): Promise<SysFieldTypesRes[]> {
    return request.get('/api/FieldType/List')
}

// 获取详情
export function getById(id: string): Promise<SysFieldTypes> {
    return request.get('/api/FieldType/Get', { params: { id } })
}

// 保存（新增/更新）
export function save(data: SysFieldTypesSaveReq): Promise<SysFieldTypes> {
    return request.post('/api/FieldType/Save', data)
}

// 删除
export function del(id: string): Promise<void> {
    return request.post('/api/FieldType/Del', { id })
}

// 获取数据库关系列表
export function getDatabaseList(id: string): Promise<SysDatabaseFieldRelationsVM[]> {
    return request.get('/api/FieldType/Database/List', { params: { id } })
}

// 保存数据库关系
export function saveDatabaseRelations(data: SysFieldDatabasesSaveReq): Promise<void> {
    return request.post('/api/FieldType/Database/Save', data)
}
