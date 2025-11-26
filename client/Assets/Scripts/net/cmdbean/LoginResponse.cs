using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LoginResponse:Response
{
    public string refreshToken;
    public int refreshTokenExpireTime;
    public string token;
    public int tokenExpireTime;
    public int uid;
}
