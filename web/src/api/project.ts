import request from './request'

// 数据库类型
export interface SysDatabaseTypes {
    id: string
    name: string
    version?: string
    description: string
    createTableScript?: string
    createTime?: string
    updateTime?: string
}

// ===== 项目相关类型 =====
export interface SysProjects {
    id: string
    name: string
    description: string
    databaseId: string
    databaseName?: string
    createTime?: string
    updateTime?: string
}

export interface SysProjectsSaveReq {
    id?: string
    name: string
    description: string
    databaseId?: string
}

export interface SysProjectsRes extends SysProjects {
    tableCount?: number
    groupCount?: number
}

// ===== 分组相关类型 =====
export interface SysTableGroups {
    id: string
    projectId: string
    name: string
    description: string
    sortIndex: number
    createTime?: string
    updateTime?: string
    tableCount?: number
}

export interface SysTableGroupsSaveReq {
    id?: string
    projectId: string
    name: string
    description: string
    sortIndex?: number
}

// ===== 表相关类型 =====
export interface SysTables {
    id: string
    projectId: string
    groupId: string
    name: string
    description: string
    groupName?: string
    fieldCount?: number
    createTime?: string
    updateTime?: string
}

export interface SysTablesSaveReq {
    id?: string
    projectId: string
    groupId: string
    name: string
    description: string
}

// ===== 字段相关类型 =====
export interface SysTableFields {
    id: string
    projectId: string
    tableId: string
    fieldTypeId: string
    name: string
    description: string
    defaultValue?: string
    isPrimaryKey: boolean
    isIdentity: boolean
    isUnique: boolean
    isNull: boolean
    sortIndex: number
    dataType: string
    databaseName: string
}

export interface SysTableFieldsSaveReq {
    id?: string
    tableId: string
    fieldTypeId: string
    name: string
    description: string
    length?: number
    precision?: number
    defaultValue?: string
    isPrimaryKey: boolean
    isIdentity: boolean
    isUnique: boolean
    isNull: boolean
}

export interface SysTableFieldsSortReq {
    tableId: string
    ids: string[]
}

export interface SysTableFieldsCopyReq {
    tableId: string
    ids: string[]
}

export interface SysTableFieldsRes extends SysTableFields {
    fieldTypeName?: string
    databaseName: string
    relation?: SysDatabaseFieldRelationsVM
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

// ===== 项目表列表响应（含分组和表） =====
export interface SysProjectTablesRes {
    project: SysProjects
    groups: SysTableGroups[]
    tables: SysTables[]
}

// 列表响应
export interface SysFieldTypesRes {
    id: string
    name: string
    description: string
    createTime?: string
    updateTime?: string
    databaseCount?: number
    dataType?: string
    length?: number
    precision?: number
    defaultValue?: string
}

// 
export interface SqlPreviewRes {
    tableName: string
    tableDescription: string
    databaseName:string
    createScripts: string
    fields: DataFieldRes[]
    primaryKeys: DataFieldRes[]
    identitys: DataFieldRes[]
    uniques: DataFieldRes[]
}

// 
export interface DataFieldRes {
    name: string
    des: string
    isPrimaryKey: boolean
    isIdentity: boolean
    isUnique: boolean
    isNull: boolean
    defaultValue: string
    length?: number
    precision?: number
    dbType: string
    dbTypeValue: string
}

// ===== API 方法 =====

// 项目列表
export function getList(): Promise<SysProjectsRes[]> {
    return request.get('/api/Project/List')
}

// 数据库列表
export function getDatabaseList(): Promise<SysDatabaseTypes[]> {
    return request.get('/api/Project/Database/List')
}


// 项目详情
export function getById(id: string): Promise<SysProjects> {
    return request.get('/api/Project/Get', { params: { id } })
}

// 保存项目
export function save(data: SysProjectsSaveReq): Promise<SysProjects> {
    return request.post('/api/Project/Save', data)
}

// 删除项目
export function del(id: string): Promise<void> {
    return request.post('/api/Project/Del', { id })
}

// 获取项目的表列表（含分组）
export function getTableList(projectId: string): Promise<SysProjectTablesRes> {
    return request.get('/api/Project/Table/List', { params: { projectId } })
}

// 保存表
export function saveTable(data: SysTablesSaveReq): Promise<SysTables> {
    return request.post('/api/Project/Table/Save', data)
}

// 表复制
export function copyTable(id: string): Promise<void> {
    return request.post('/api/Project/Table/Copy', { id })
}

// 删除表
export function delTable(id: string): Promise<void> {
    return request.post('/api/Project/Table/Del', { id })
}

// 保存分组
export function saveGroup(data: SysTableGroupsSaveReq): Promise<SysTableGroups> {
    return request.post('/api/Project/Group/Save', data)
}

// 删除分组
export function delGroup(id: string): Promise<void> {
    return request.post('/api/Project/Group/Del', { id })
}

// 获取字段列表
export function getFieldTypeList(databaseId: string): Promise<SysFieldTypesRes[]> {
    return request.get('/api/Project/FieldType/List', { params: { databaseId } })
}
// 获取字段列表
export function getFieldList(databaseId: string, tableId: string): Promise<SysTableFieldsRes[]> {
    return request.get('/api/Project/Field/List', { params: { databaseId, tableId } })
}

// 保存字段
export function saveField(data: SysTableFieldsSaveReq): Promise<SysTableFields> {
    return request.post('/api/Project/Field/Save', data)
}

// 删除字段
export function delField(id: string): Promise<void> {
    return request.post('/api/Project/Field/Del', { id })
}

// 字段排序
export function sortFields(data: SysTableFieldsSortReq): Promise<void> {
    return request.post('/api/Project/Field/Sort', data)
}

// 复制字段
export function copyFields(data: SysTableFieldsCopyReq): Promise<void> {
    return request.post('/api/Project/Field/Copy', data)
}


export function sqlPreview(databaseId: string,tableId:string): Promise<SqlPreviewRes> {
    return request.get('/api/Project/Sql/Preview', { params: { databaseId, tableId } })
}
