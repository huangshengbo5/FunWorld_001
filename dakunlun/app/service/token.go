package service

import (
	"context"
	"dakunlun/app/util"
	"fmt"
	"time"

	"github.com/go-redis/redis/v8"
)

const AccessTokenExpireTime = 24 * time.Hour
const RefreshTokenExpireTime = 72 * time.Hour

const (
	TokenTypeAceessToken = iota
	TokenTypeRefreshToken
)

// 鉴权信息
type Auth struct {
	AccessToken          string
	AccessTokenExpireIn  int64
	RefreshToken         string
	RefreshTokenExpireIn int64
}

type tokenService struct {
}

var TokenService = new(tokenService)

func (srv *tokenService) AuthToken(uid uint32, inputToken string, tokenType int) (ok bool, err error) {
	var rightToken string
	rightToken, err = srv.getToken(uid, tokenType)

	if err == redis.Nil {
		return false, util.NewAppError(util.ErrorCodeTokenExpire)
	}

	if err != nil {
		return
	}

	return rightToken == inputToken, nil
}

func (*tokenService) GenerateToken(uid uint32) (auth *Auth, err error) {
	var (
		accessToken, refreshToken                 string
		accessTokenExpireIn, refreshTokenExpireIn int64
	)
	//生成TOKEN
	accessToken = util.GenerateToken()
	refreshToken = util.GenerateToken()
	//生成到期时间
	accessTokenExpireIn = time.Now().Add(AccessTokenExpireTime).Unix()
	refreshTokenExpireIn = time.Now().Add(RefreshTokenExpireTime).Unix()

	//token持久化
	pipe := util.GetRedisClient().Pipeline()
	ctx := context.Background()
	pipe.Set(ctx, tokenKey(uid), accessToken, AccessTokenExpireTime)

	pipe.Set(ctx, refreshTokenKey(uid), refreshToken, RefreshTokenExpireTime)

	_, err = pipe.Exec(ctx)
	if err != nil {
		return
	}

	auth = &Auth{
		AccessToken:          accessToken,
		AccessTokenExpireIn:  accessTokenExpireIn,
		RefreshToken:         refreshToken,
		RefreshTokenExpireIn: refreshTokenExpireIn,
	}

	return
}

func tokenKey(uid uint32) string {
	return fmt.Sprintf("%d", uid)
}

func refreshTokenKey(uid uint32) string {
	return fmt.Sprintf("%d_ref", uid)
}

func (*tokenService) getToken(uid uint32, tokenType int) (token string, err error) {
	switch tokenType {
	case TokenTypeAceessToken:
		token, err = util.GetRedisClient().Get(context.Background(), tokenKey(uid)).Result()
	case TokenTypeRefreshToken:
		token, err = util.GetRedisClient().Get(context.Background(), refreshTokenKey(uid)).Result()
	}

	if err != nil {
		return
	}

	return
}
