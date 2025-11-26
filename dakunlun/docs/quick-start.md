# 快速开始指南

## 前置要求

在开始之前，请确保您的系统已安装以下软件：

- Go 1.19 或更高版本
- MySQL 5.7 或更高版本
- Redis 6.0 或更高版本
- Git

## 快速配置步骤

### 1. 克隆项目

```bash
git clone <your-repository-url>
cd dakunlun
```

### 2. 安装依赖

```bash
go mod download
```

### 3. 配置数据库

#### 创建数据库

```sql
-- 连接到MySQL
mysql -u root -p

-- 创建数据库
CREATE DATABASE dakunlun CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- 创建用户（可选，推荐）
CREATE USER 'dakunlun'@'localhost' IDENTIFIED BY 'your_password_here';
GRANT ALL PRIVILEGES ON dakunlun.* TO 'dakunlun'@'localhost';
FLUSH PRIVILEGES;
```

#### 导入数据库结构

```bash
mysql -u dakunlun -p dakunlun < cmd/dakunlun.sql
```

### 4. 配置Redis

确保Redis服务正在运行：

```bash
# Ubuntu/Debian
sudo systemctl start redis-server
sudo systemctl enable redis-server

# CentOS/RHEL
sudo systemctl start redis
sudo systemctl enable redis

# macOS (使用Homebrew)
brew services start redis

# Windows
# 下载并安装Redis，然后启动服务
```

### 5. 配置应用

#### 方法一：修改配置文件（推荐用于开发环境）

编辑 `configs/dev.toml`：

```toml
inherit_files=["base.toml"]

env = "dev"

# 数据库配置
mysql_host = '127.0.0.1'
mysql_port = 3306
mysql_user = 'dakunlun'
mysql_password = 'your_actual_mysql_password'  # 替换为实际密码

# Redis配置
redis_uri = '127.0.0.1:6379'
# 如果Redis设置了密码，请在base.toml中配置
```

编辑 `configs/base.toml`：

```toml
# 如果Redis设置了密码
redis_password = 'your_redis_password'  # 替换为实际密码，如果没有密码可以留空

#### 方法二：使用环境变量（推荐用于生产环境）

创建 `.env` 文件：

```bash
# 数据库配置
MYSQL_PASSWORD=your_actual_mysql_password

# Redis配置
REDIS_PASSWORD=your_redis_password

### 6. 配置第三方服务（可选）

#### 钉钉机器人配置

如果需要使用钉钉通知功能，请：

1. 在钉钉群中创建自定义机器人
2. 获取AccessToken
3. 修改 `app/util/alert.go` 文件：

```go
ding := dinghook.Ding{AccessToken: "your_actual_dingtalk_token"}
```

### 7. 启动应用

#### 开发模式

```bash
# 使用dev配置启动
go run cmd/server/main.go
```

#### 生产模式

```bash
# 编译
go build -o server cmd/server/main.go

# 运行
./server
```

### 8. 验证安装

应用启动后，您应该看到类似以下的输出：

```
[GIN-debug] [WARNING] Creating an Engine instance with the Logger and Recovery middleware already attached.
[GIN-debug] [WARNING] Running in "debug" mode. Switch to "release" mode in production.
[GIN-debug] Listening and serving HTTP on :8080
```

访问以下URL验证服务是否正常：

- API文档: http://localhost:8080/swagger/index.html
- 健康检查: http://localhost:8080/health（如果有的话）

## 常见问题解决

### 数据库连接失败

**错误信息**: `Error connecting to database`

**解决方案**:
1. 检查MySQL服务是否启动
2. 验证用户名和密码是否正确
3. 确认数据库名称是否存在
4. 检查网络连接

```bash
# 测试数据库连接
mysql -h 127.0.0.1 -u dakunlun -p dakunlun
```

### Redis连接失败

**错误信息**: `Error connecting to Redis`

**解决方案**:
1. 检查Redis服务是否启动
2. 验证Redis密码（如果设置了）
3. 检查端口是否正确

```bash
# 测试Redis连接
redis-cli ping
# 如果设置了密码
redis-cli -a your_password ping
```

### 端口被占用

**错误信息**: `bind: address already in use`

**解决方案**:
1. 更改配置文件中的端口号
2. 或者停止占用端口的进程

```bash
# 查找占用端口的进程
lsof -i :8080
# 或者
netstat -tulpn | grep 8080

# 杀死进程
kill -9 <PID>
```

### 权限问题

**错误信息**: `permission denied`

**解决方案**:
1. 检查文件权限
2. 确保以正确的用户运行

```bash
# 设置正确的权限
chmod +x server
chmod 600 configs/*.toml
```

## 开发环境设置

### IDE配置

推荐使用以下IDE：
- GoLand
- VS Code + Go扩展
- Vim/Neovim + vim-go

### 调试配置

#### VS Code调试配置 (.vscode/launch.json)

```json
{
    "version": "0.2.0",
    "configurations": [
        {
            "name": "Launch Server",
            "type": "go",
            "request": "launch",
            "mode": "auto",
            "program": "${workspaceFolder}/cmd/server/main.go",
            "env": {
                "GIN_MODE": "debug"
            },
            "args": []
        }
    ]
}
```

### 代码格式化

```bash
# 格式化代码
go fmt ./...

# 代码检查
go vet ./...

# 运行测试
go test ./...
```

## 下一步

现在您的项目已经成功启动，您可以：

1. 查看 [API文档](http://localhost:8080/swagger/index.html)
2. 阅读 [配置模板文档](config-template.md) 了解详细配置
3. 查看 [安全指南](security-guide.md) 了解生产环境部署
4. 开始开发您的功能

## 获取帮助

如果遇到问题，请：

1. 检查日志文件 `server.log`
2. 查看 [常见问题文档](README.md#常见问题)
3. 联系项目维护者

祝您使用愉快！