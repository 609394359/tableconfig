// 字段类型 - 自定义业务字段类型
export interface FieldType {
    id: string
    name: string           // 字段类型名称，如"用户名"、"手机号"、"金额"
    description: string    // 描述
    // 数据库类型映射：key 是数据库类型ID，value 是对应的原生SQL类型
    databaseTypeMappings: Record<string, DatabaseTypeMapping>
}

// 数据库类型映射详情
export interface DatabaseTypeMapping {
    dataType: string        // SQL类型，如 VARCHAR(50), BIGINT(20)
    length?: number        // 长度
    precision?: number     // 精度（小数位数）
    defaultValue?: string  // 默认值
}


// 数据库类型映射详情
export interface DatabaseTypeField {
    fieldId: string        // 字段类型id
    fieldName: string        // 字段类型名称
    dataType: string        // SQL类型，如 VARCHAR(50), BIGINT(20)
    length?: number        // 长度
    precision?: number     // 精度（小数位数）
    defaultValue?: string  // 默认值
}

// 数据库类型
export interface DatabaseType {
    id: string
    name: string          // 如 MySQL, PostgreSQL
    version?: string
    description: string
    createTableScript?: string // 自定义建表脚本模板 (JavaScript)
}

// 项目
export interface Project {
    id: string
    name: string
    description: string
    databaseTypeId: string
    createdAt: string
    updatedAt: string
    tableGroups: TableGroup[]
}

// 表分组
export interface TableGroup {
    id: string
    projectId: string
    name: string
    description: string
    sortOrder: number
    tables: Table[]
}

// 表
export interface Table {
    id: string
    groupId: string
    name: string           // 表名（英文）
    displayName: string    // 显示名称（中文）
    description: string
    engine?: string        // 存储引擎
    charset?: string       // 字符集
    collation?: string     // 排序规则
    fields: TableField[]
    createdAt: string
    updatedAt: string
    parentTableId?: string // 继承自哪个表
}

// 表字段
export interface TableField {
    id: string
    tableId: string
    name: string           // 字段名
    displayName: string    // 显示名称
    fieldTypeId: string    // 字段类型 ID（引用 FieldType）
    nullable: boolean      // 是否可空
    defaultValue?: string  // 默认值（覆盖字段类型的默认值）
    isPrimaryKey: boolean  // 是否主键
    isAutoIncrement: boolean // 是否自增
    isUnique: boolean      // 是否唯一
    isIndex: boolean       // 是否索引
    comment: string        // 注释
    sortOrder: number      // 排序
    inherited: boolean     // 是否继承的字段
    inheritedFrom?: string // 继承自哪个表
}

// 表单对话框状态
export interface DialogState {
    visible: boolean
    mode: 'create' | 'edit' | 'copy'
    data: Record<string, unknown>
}

// 通用列表响应
export interface ListResponse<T> {
    items: T[]
    total: number
}
