using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTip : DialogMonoBehaviour
{
    public UIInput input, input2, input3, input4;
    public GameObject but, zhucebut,shuomingbut;
    void Start()
    {
        UIEventListener.Get(but).onClick = closeDialog;
        UIEventListener.Get(zhucebut).onClick = onClick16;
        UIEventListener.Get(shuomingbut).onClick = onClickshuoming;
    }

    private void onClickshuoming(GameObject go)
    {
        string str = "\n"
+ "根据国家新闻出版署发布《关于防止未成年人沉迷网络游戏的通知》和《关于进一步严格管理 切实防止未成年人沉迷网络游戏的通知》，请您如实填写身份信息，本游戏对未成年人有以下限制:\n\n"

        + "游戏登录\n"
+ "1.每周五、周六、周日和法定节假日每20时至 21时提供1小时网络游戏服务。\n"
+ "游戏充值\n"
+ "1.本游戏不为未满12周岁的用户提供游戏服务。\n"
+ "2.未满8周岁的用户不能付费。\n"
+ "3.8周岁以上未满16周岁的未成年人用户，单次充值金额不得超过50元人民币，每月充值金额累计不得超过200元人民币；16周岁以上的未成年人用户，单次充值金额不得超过100元人民币，每月充值金额累计不得超过400元人民币。\n";


        UIManager.showTip(str, null, null, null, null, "未成年人防沉迷政策说明");
    }

    private void onClick16(GameObject go)
    {
        if (input.value.Length < 6)
        {
            UIManager.showToast("账号长度最小为6位。");
            return;
        }else if (input.value.Length >64)
        {
            UIManager.showToast("账号长度最大64位。");
            return;
        }
        if (input2.value.Length < 6)
        {
            UIManager.showToast("账号长度最小为6位。");
            return;
        }
        else if (input2.value.Length > 16)
        {
            UIManager.showToast("密码长度最大16位。");
            return;
        }

        if (input4.value.Length <= 1)
        {
            UIManager.showToast("请输入正确的姓名。");
            return;
        }

        if (IsValidIdNumber(input3.value))
        {
            UIManager.showToast("请输入正确的身份证号。");
            return;
        }
        HttpManager.instance.sendLogin1(input.value, input2.value, input3.value, (code0) =>
        {

            if (code0 == Callback.SUCCESS)
            {
                UIManager.showToast("账号注册成功。");
                CoverDlg.instance.input.value = input.value;
                CoverDlg.instance.input2.value = input2.value;
                CoverDlg.instance.onClick(null);
                closeDialog(null);

            }

        });
    }

    public bool IsValidIdNumber(string idNumber)
    {
        if (idNumber == null || idNumber.Length != 18)
            return false; // 长度不是18位

        // 提取身份证号码的前17位
        string idNumber17 = idNumber.Substring(0, 17);

        // 计算前17位的加权和
        int[] weights = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
        int sum = 0;
        for (int i = 0; i < idNumber17.Length; i++)
        {
            sum += (idNumber17[i] - '0') * weights[i];
        }

        // 计算校验码
        int checkCode = (12 - sum % 11) % 11;
        char[] checkCodes = { '1', '0', 'X', '9', '8', '7', '6', '5', '4', '3', '2' };
        char expectedCheckCode = checkCodes[checkCode];

        // 比较计算出的校验码和最后一位
        char actualCheckCode = idNumber[17];
        return char.ToUpper(actualCheckCode) == char.ToUpper(expectedCheckCode);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
