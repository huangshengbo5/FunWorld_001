package msg

type LoginRequest struct {
	Name     string `form:"name" json:"name" binding:"required,min=6,max=64"`
	Password string `form:"password" json:"password" binding:"required,min=6,max=16"`
	Clear    bool   `json:"clear"`
}

type LoginResponse struct {
	Uid                    uint32 `json:"uid"`                    //用户ID
	Token                  string `json:"token"`                  //TOKEN
	RefreshToken           string `json:"refreshToken"`           //REFRESHTOKEN（TOKEN快过期时延期用）
	TokenExpireTime        int64  `json:"tokenExpireTime"`        //TOKEN过期时间
	RefreshTokenExpireTime int64  `json:"refreshTokenExpireTime"` //REFRESHTOKEN过期时间
}

type RegistRequest struct {
	Name     string `form:"name" json:"name" binding:"required,min=6,max=64"`
	Password string `form:"password" json:"password" binding:"required,min=6,max=16"`
	//身份证
	IDCard string `form:"idcard" json:"idcard"`
}

type RegistResponse struct {
	Name string `json:"name"`
}
