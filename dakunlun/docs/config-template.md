# 配置文件模板

## 配置文件说明

项目使用TOML格式的配置文件，支持配置继承。主要配置文件包括：

- `configs/base.toml` - 基础配置模板
- `configs/dev.toml` - 开发环境配置
- `configs/test.toml` - 测试环境配置

## 完整配置模板

### base.toml 完整配置

```toml
# 环境配置
env = "dev" # 可选值: "dev"/"test"/"sandbox"/"prod"
platform = "native" # 可选值: "native"/"facebook"/"facebook_canvas"

# API文档配置
swagger_json = "http://127.0.0.1:8080/swagger/doc.json"
http_port = 8080
reqcheck_ttl = 10000  # 请求检查TTL，1000=1秒

# Redis配置
redis_uri = '127.0.0.1:6379'
redis_password = 'YOUR_REDIS_PASSWORD'  # 替换为实际Redis密码
redis_db = '1'
redis_cluster_addrs = []  # Redis集群地址，如果使用集群模式

# MySQL数据库配置
mysql_host = '127.0.0.1'
mysql_port = 3306
mysql_user = 'dakunlun'
mysql_password = 'YOUR_MYSQL_PASSWORD'  # 替换为实际MySQL密码
mysql_db = 'dakunlun'

# 日志配置
log_file = "server.log"
log_max_size = 128      # 单个文件大小(MB)
log_max_backups = 10    # 保留的日志文件数量
log_max_age = 3         # 保存天数
log_compress = false    # 是否压缩旧日志
log_localtime = true    # 是否使用本地时区
```

### 环境特定配置

#### 开发环境 (dev.toml)

```toml
inherit_files=["base.toml"]

# 覆盖基础配置
swagger_json = "http://127.0.0.1:8080/swagger/doc.json"
env = "dev"

# 开发环境Redis配置
redis_uri = '127.0.0.1:6379'  # 本地Redis

# 开发环境数据库配置
mysql_host = '127.0.0.1'      # 本地数据库
mysql_port = 3306
mysql_user = 'dakunlun'
mysql_password = 'YOUR_DEV_MYSQL_PASSWORD'  # 开发环境数据库密码
```

#### 测试环境 (test.toml)

```toml
inherit_files=["base.toml"]

# 测试环境配置
swagger_json = "http://test-server:8080/swagger/doc.json"
env = "test"

# 测试环境Redis配置
redis_uri = '127.0.0.1:6379'

# 测试环境数据库配置
mysql_host = '127.0.0.1'
mysql_port = 3306
mysql_user = 'dakunlun'
mysql_password = 'YOUR_TEST_MYSQL_PASSWORD'  # 测试环境数据库密码
```

#### 生产环境配置建议

```toml
inherit_files=["base.toml"]

# 生产环境配置
env = "prod"
http_port = 8080

# 生产环境Redis配置（建议使用集群）
redis_uri = 'prod-redis-master:6379'
redis_password = 'YOUR_PROD_REDIS_PASSWORD'  # 生产环境Redis密码
redis_db = '0'
# 如果使用Redis集群
# redis_cluster_addrs = ['redis-node1:6379', 'redis-node2:6379', 'redis-node3:6379']

# 生产环境数据库配置
mysql_host = 'prod-mysql-master'
mysql_port = 3306
mysql_user = 'dakunlun_prod'
mysql_password = 'YOUR_PROD_MYSQL_PASSWORD'  # 生产环境数据库密码
mysql_db = 'dakunlun_prod'

# 生产环境日志配置
log_file = "/var/log/dakunlun/server.log"
log_max_size = 256      # 生产环境可以设置更大的日志文件
log_max_backups = 30    # 保留更多的日志文件
log_max_age = 7         # 保存更长时间
log_compress = true     # 生产环境建议压缩日志
```

## 敏感信息配置指南

### 1. 数据库密码

- **开发环境**: 使用简单密码，如 `dev123456`
- **测试环境**: 使用中等强度密码
- **生产环境**: 使用强密码，包含大小写字母、数字和特殊字符，长度至少16位

### 2. Redis密码

- 建议使用随机生成的强密码
- 可以使用命令生成: `openssl rand -base64 32`

### 3. API Token

- 从对应的第三方服务平台获取
- 定期轮换token以提高安全性
- 不同环境使用不同的token

### 4. 钉钉机器人Token

- 在钉钉开发者平台创建机器人获取AccessToken
- 配置IP白名单和关键词过滤

## 配置文件安全建议

1. **权限控制**: 配置文件应设置适当的文件权限 (600 或 640)
2. **版本控制**: 不要将包含真实密码的配置文件提交到版本控制系统
3. **环境变量**: 生产环境建议使用环境变量覆盖敏感配置
4. **配置加密**: 考虑使用配置加密工具保护敏感信息
5. **定期轮换**: 定期更换密码和token

## 环境变量支持

项目支持通过环境变量覆盖配置文件中的设置：

```bash
# 数据库配置
export MYSQL_PASSWORD="your_mysql_password"
export REDIS_PASSWORD="your_redis_password"

# API配置
export DINGTALK_ACCESS_TOKEN="your_dingtalk_token"
```