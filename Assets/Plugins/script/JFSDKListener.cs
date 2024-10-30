using UnityEngine;
using System;

namespace jfsdk
{
    // JFSDKListener
    public abstract class JFSDKListener : AndroidJavaProxy
    {
        protected JFSDKListener() : base("com.juefeng.sdk.juefengsdk.interf.SDKEventListener")
        {
        }

        //callback

        public abstract void onLoginSuccessCallback(LogincallBack logincallBack);
        public abstract void onLoginFailedCallback(LoginErrorMsg loginErrorMsg);
        public abstract void onInitSuccessCallback(String desc, Boolean isAutoLogin);
        public abstract void onInitFaildCallback(String desc);
        public abstract void onPaySuccess(PaySuccessInfo paySuccessInfo);
        public abstract void onPayFaild(PayFaildInfo payFaildInfo);
        public abstract void onExitCallback(String desc);
        public abstract void onCancleExitCallback(String desc);
        public abstract void onCreatedOrderCallback(CreatOrderInfo infoBean);
        public abstract void onLogoutLoginCallback();
        public abstract void onSwitchAccountSuccessCallback(LogincallBack login);
        public abstract void onGameSwitchAccountCallback();
        public abstract void onSyncSuccessCallback();

        //callback end


        public void onLoginSuccess(AndroidJavaObject logincallBack)
        {
            LogincallBack uLogincallBack = new LogincallBack();
            uLogincallBack.setToken(logincallBack.Call<String>("getToken"));
            uLogincallBack.setChannelUserId(logincallBack.Call<String>("getChannelUserId"));
            uLogincallBack.setJfUserId(logincallBack.Call<String>("getJfUserId"));
            uLogincallBack.setUserName(logincallBack.Call<String>("getUserName"));
            onLoginSuccessCallback(uLogincallBack);
        }
        public void onLoginFailed(AndroidJavaObject loginErrorMsg)
        {
            LoginErrorMsg uLoginErrorMsg = new LoginErrorMsg(loginErrorMsg.Call<int>("getCode"), loginErrorMsg.Call<String>("getErrorMsg"));
            onLoginFailedCallback(uLoginErrorMsg);
        }
        public void onInitSuccess(String desc, Boolean isAutoLogin)
        {
            onInitSuccessCallback(desc,isAutoLogin);
        }
        public void onInitFaild(String desc)
        {
            onInitFaildCallback(desc);
        }
        public void onPaySuccessCallback(AndroidJavaObject paySuccessInfo)
        {
            PaySuccessInfo uPaySuccessInfo = new PaySuccessInfo(
                paySuccessInfo.Get<String>("orderId"),
                paySuccessInfo.Get<String>("gameRole"),
                paySuccessInfo.Get<String>("gameArea"),
                paySuccessInfo.Get<String>("productName"),
                paySuccessInfo.Get<String>("productDesc"),
                paySuccessInfo.Get<String>("remark")
                );
            onPaySuccess(uPaySuccessInfo);
        }
        public void onPayFaildCallback(AndroidJavaObject payFaildInfo)
        {
            PayFaildInfo uPayFaildInfo = new PayFaildInfo(
                payFaildInfo.Call<String>("getCode"),
                payFaildInfo.Call<String>("getMsg")
                );
            onPayFaild(uPayFaildInfo);
        }
        public void onExit(String desc)
        {
            onExitCallback(desc);
        }
        public void onCancleExit(String desc)
        {
            onCancleExitCallback(desc);
        }
        public void onCreatedOrder(AndroidJavaObject infoBean)
        {
            CreatOrderInfo uCreatOrderInfo = new(
                infoBean.Get<String>("orderId"),
                infoBean.Get<String>("gameRole"),
                infoBean.Get<String>("gameArea"),
                infoBean.Get<String>("productName"),
                infoBean.Get<String>("productDesc"),
                infoBean.Get<String>("remark")
                );
            onCreatedOrderCallback(uCreatOrderInfo);
        }
        public void onLogoutLogin()
        {
            onLogoutLoginCallback();
        }
        public void onSwitchAccountSuccess(AndroidJavaObject logincallBack)
        {
            LogincallBack uLogincallBack = new LogincallBack();
            uLogincallBack.setToken(logincallBack.Call<String>("getToken"));
            uLogincallBack.setChannelUserId(logincallBack.Call<String>("getChannelUserId"));
            uLogincallBack.setJfUserId(logincallBack.Call<String>("getJfUserId"));
            uLogincallBack.setUserName(logincallBack.Call<String>("getUserName"));
            onSwitchAccountSuccessCallback(uLogincallBack);
        }
        public void onGameSwitchAccount()
        {
            onGameSwitchAccountCallback();
        }
        public void onSyncSuccess()
        {
            onSyncSuccessCallback();
        }
    }
}
