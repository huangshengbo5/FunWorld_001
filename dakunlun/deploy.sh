#!/bin/bash

# 切换到目标目录
cd /data/web_root/dakunlun

# 拉取最新的代码
git pull

# 构建服务器
go build -ldflags "-w -s" -v -o server ./cmd/server

# 查找旧的服务器进程ID
PID=$(ps -ef | grep "./server -config test" | grep -v grep | awk '{print $2}')

# 终止旧的服务器进程
if [ -n "$PID" ]; then
  kill $PID
  echo "Terminated old server process: $PID"
else
  echo "No old server process found"
fi

# 启动新的服务器进程
nohup ./server -config test > server.log 2>&1 &

# 取消进程组与父进程的关系
disown -h %1

echo "Server started and disowned"