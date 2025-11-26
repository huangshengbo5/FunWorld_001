package util

import (
	"sync"
	"time"

	"github.com/panjf2000/ants/v2"
	"go.uber.org/zap"
)

const goPoolSize = 10240

var (
	p        *ants.Pool
	oncePool sync.Once
)

func GoPool() *ants.Pool {
	oncePool.Do(func() {
		var err error
		p, err = ants.NewPool(goPoolSize, ants.WithPanicHandler(func(err interface{}) {
			GetLogger().Error("GoPool.Panic", zap.Any("panic", err))
		}))
		if err == nil {
			go func() {
				defer func() {
					if x := recover(); x != nil {

					}
				}()
				t := time.NewTicker(time.Second * time.Duration(3600))
				defer t.Stop()

				for {
					select {
					case <-t.C:
						GetLogger().Info("GoPool.Monitor", zap.Int("ants_cap", p.Cap()), zap.Int("ants_free",
							p.Free()), zap.Int("ants_running", p.Running()))
					}
				}
			}()
		}
	})
	return p
}

func ReleaseGoPool() {
	if p != nil {
		p.Release()
	}
}
