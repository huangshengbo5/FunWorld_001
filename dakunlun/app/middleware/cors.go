package middleware

import (
	"fmt"
	"net/http"
	"strconv"
	"strings"

	"github.com/gin-gonic/gin"
)

const (
	HeaderOrigin                        = "Origin"
	HeaderVary                          = "Vary"
	HeaderAccessControlRequestMethod    = "Access-Control-Request-Method"
	HeaderAccessControlRequestHeaders   = "Access-Control-Request-Headers"
	HeaderAccessControlAllowOrigin      = "Access-Control-Allow-Origin"
	HeaderAccessControlAllowMethods     = "Access-Control-Allow-Methods"
	HeaderAccessControlAllowHeaders     = "Access-Control-Allow-Headers"
	HeaderAccessControlAllowCredentials = "Access-Control-Allow-Credentials"
	HeaderAccessControlExposeHeaders    = "Access-Control-Expose-Headers"
	HeaderAccessControlMaxAge           = "Access-Control-Max-Age"
)

type CorsConf struct {
	AllowOrigins     []string
	AllowMethods     []string
	AllowHeaders     []string
	AllowCredentials bool
	ExposeHeaders    []string
	MaxAge           int
}

var defaultConfOptions = [...]ConfOption{
	WithAllowOrigins([]string{"*"}),
	WithAllowMethods([]string{http.MethodGet, http.MethodHead, http.MethodPut, http.MethodPatch, http.MethodPost, http.MethodDelete}),
	WithAllowHeaders(nil),
	WithAllowCredentials(false),
	WithExposeHeaders(nil),
	WithMaxAge(0),
}

type ConfOption func(cc *CorsConf) ConfOption

func WithAllowOrigins(v []string) ConfOption {
	return func(cc *CorsConf) ConfOption {
		previous := cc.AllowOrigins
		cc.AllowOrigins = v
		return WithAllowOrigins(previous)
	}
}
func WithAllowMethods(v []string) ConfOption {
	return func(cc *CorsConf) ConfOption {
		previous := cc.AllowMethods
		cc.AllowMethods = v
		return WithAllowMethods(previous)
	}
}
func WithAllowHeaders(v []string) ConfOption {
	return func(cc *CorsConf) ConfOption {
		previous := cc.AllowHeaders
		cc.AllowHeaders = v
		return WithAllowHeaders(previous)
	}
}
func WithAllowCredentials(v bool) ConfOption {
	return func(cc *CorsConf) ConfOption {
		previous := cc.AllowCredentials
		cc.AllowCredentials = v
		return WithAllowCredentials(previous)
	}
}
func WithExposeHeaders(v []string) ConfOption {
	return func(cc *CorsConf) ConfOption {
		previous := cc.ExposeHeaders
		cc.ExposeHeaders = v
		return WithExposeHeaders(previous)
	}
}
func WithMaxAge(v int) ConfOption {
	return func(cc *CorsConf) ConfOption {
		previous := cc.MaxAge
		cc.MaxAge = v
		return WithMaxAge(previous)
	}
}

func newDefaultConf() *CorsConf {
	cc := &CorsConf{}

	for _, opt := range defaultConfOptions {
		_ = opt(cc)
	}

	return cc
}

func NewCorsConf(opts ...ConfOption) *CorsConf {
	cc := newDefaultConf()
	for _, opt := range opts {
		_ = opt(cc)
	}
	return cc
}

// 处理跨域请求,支持options访问
func Cors(ops ...ConfOption) gin.HandlerFunc {
	conf := NewCorsConf(ops...)
	allowMethods := strings.Join(conf.AllowMethods, ",")
	allowHeaders := strings.Join(conf.AllowHeaders, ",")
	exposeHeaders := strings.Join(conf.ExposeHeaders, ",")

	return func(c *gin.Context) {
		origin := c.GetHeader(HeaderOrigin)
		allowOrigin := ""

		// Check allowed origins
		for _, o := range conf.AllowOrigins {
			if o == "*" && conf.AllowCredentials {
				allowOrigin = origin
				break
			}
			if o == "*" || o == origin {
				allowOrigin = o
				break
			}
			if matchSubdomain(origin, o) {
				allowOrigin = origin
				break
			}
		}

		// Simple request
		if c.Request.Method != http.MethodOptions {
			c.Header(HeaderVary, HeaderOrigin)
			c.Header(HeaderAccessControlAllowOrigin, allowOrigin)
			if conf.AllowCredentials {
				c.Header(HeaderAccessControlAllowCredentials, "true")
			}
			if exposeHeaders != "" {
				c.Header(HeaderAccessControlExposeHeaders, exposeHeaders)
			}
			// 处理请求
			c.Next()
		} else {
			// Preflight request
			c.Header(HeaderVary, fmt.Sprintf("%s, %s, %s", HeaderOrigin, HeaderAccessControlRequestMethod, HeaderAccessControlRequestHeaders))
			c.Header(HeaderAccessControlAllowOrigin, allowOrigin)
			c.Header(HeaderAccessControlAllowMethods, allowMethods)

			if conf.AllowCredentials {
				c.Header(HeaderAccessControlAllowCredentials, "true")
			}
			if allowHeaders != "" {
				c.Header(HeaderAccessControlAllowHeaders, allowHeaders)
			} else {
				h := c.GetHeader(HeaderAccessControlRequestHeaders)
				if h != "" {
					c.Header(HeaderAccessControlAllowHeaders, h)
				}
			}
			if conf.MaxAge > 0 {
				c.Header(HeaderAccessControlMaxAge, strconv.Itoa(conf.MaxAge))
			}

			c.AbortWithStatus(http.StatusNoContent)
		}
	}
}

func matchScheme(domain, pattern string) bool {
	didx := strings.Index(domain, ":")
	pidx := strings.Index(pattern, ":")
	return didx != -1 && pidx != -1 && domain[:didx] == pattern[:pidx]
}

// matchSubdomain compares authority with wildcard
func matchSubdomain(domain, pattern string) bool {
	if !matchScheme(domain, pattern) {
		return false
	}
	didx := strings.Index(domain, "://")
	pidx := strings.Index(pattern, "://")
	if didx == -1 || pidx == -1 {
		return false
	}
	domAuth := domain[didx+3:]
	// to avoid long loop by invalid long domain
	if len(domAuth) > 253 {
		return false
	}
	patAuth := pattern[pidx+3:]

	domComp := strings.Split(domAuth, ".")
	patComp := strings.Split(patAuth, ".")
	for i := len(domComp)/2 - 1; i >= 0; i-- {
		opp := len(domComp) - 1 - i
		domComp[i], domComp[opp] = domComp[opp], domComp[i]
	}
	for i := len(patComp)/2 - 1; i >= 0; i-- {
		opp := len(patComp) - 1 - i
		patComp[i], patComp[opp] = patComp[opp], patComp[i]
	}

	for i, v := range domComp {
		if len(patComp) <= i {
			return false
		}
		p := patComp[i]
		if p == "*" {
			return true
		}
		if p != v {
			return false
		}
	}
	return false
}
