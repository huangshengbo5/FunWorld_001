#!/bin/bash

WORK_DIR=$(cd $(dirname $0); pwd)


swag init -g ${WORK_DIR}/cmd/server/main.go

