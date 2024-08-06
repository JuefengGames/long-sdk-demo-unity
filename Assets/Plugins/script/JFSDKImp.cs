using UnityEngine;
using System;


namespace jfsdk
{

    public class JFSDKImp
    {
        private static JFSDKImp _instance;

        public static JFSDKImp getInstance()
        {
            if (null == _instance)
            {
                _instance = new JFSDKImp();
            }
            return _instance;
        }

        public void init(JFSDKListener listener)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
			JFUnitySupportAndroid androidSupport = JFUnitySupportAndroid.getInstance();
			androidSupport.init(listener);
#else
            Debug.LogWarning("请在安卓平台上运行！！！");
#endif
        }

        public void exitLogin()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
			JFUnitySupportAndroid androidSupport = JFUnitySupportAndroid.getInstance();
			androidSupport.exitLogin();
#endif
        }
        public void doLogin()
        {
#if UNITY_IOS && !UNITY_EDITOR
			JFSDK_nativeLogin();
#elif UNITY_ANDROID && !UNITY_EDITOR
			JFUnitySupportAndroid androidSupport = JFUnitySupportAndroid.getInstance();
			androidSupport.doLogin();
#endif
        }

        public void showPay(JfOrderInfo orderInfo)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
			JFUnitySupportAndroid androidSupport = JFUnitySupportAndroid.getInstance();
			androidSupport.showPay(orderInfo);
#endif
        }
        public void showFloatView()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
			JFUnitySupportAndroid androidSupport = JFUnitySupportAndroid.getInstance();
			androidSupport.showFloatView();
#endif
        }
        public void removeFloatView()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
			JFUnitySupportAndroid androidSupport = JFUnitySupportAndroid.getInstance();
			androidSupport.removeFloatView();
#endif
        }
        public void logoutLogin()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
			JFUnitySupportAndroid androidSupport = JFUnitySupportAndroid.getInstance();
			androidSupport.logoutLogin();
#endif
        }
        public void syncInfo(JfRoleInfo roleInfo)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
			JFUnitySupportAndroid androidSupport = JFUnitySupportAndroid.getInstance();
			androidSupport.syncInfo(roleInfo);
#endif
        }
        public void switchAccount()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
			JFUnitySupportAndroid androidSupport = JFUnitySupportAndroid.getInstance();
			androidSupport.switchAccount();
#endif
        }
        public void onCreate(AndroidJavaObject act)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        JFUnitySupportAndroid androidSupport = JFUnitySupportAndroid.getInstance();
        androidSupport.onCreate(act);
#endif
        }

        public void onResume(AndroidJavaObject act)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        JFUnitySupportAndroid androidSupport = JFUnitySupportAndroid.getInstance();
        androidSupport.onResume(act);
#endif
        }

        public void onPause(AndroidJavaObject act)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        JFUnitySupportAndroid androidSupport = JFUnitySupportAndroid.getInstance();
        androidSupport.onPause(act);
#endif
        }

        public void onStart(AndroidJavaObject act)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        JFUnitySupportAndroid androidSupport = JFUnitySupportAndroid.getInstance();
        androidSupport.onStart(act);
#endif
        }

        public void onRestart(AndroidJavaObject act)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        JFUnitySupportAndroid androidSupport = JFUnitySupportAndroid.getInstance();
        androidSupport.onRestart(act);
#endif
        }

        public void onStop(AndroidJavaObject act)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        JFUnitySupportAndroid androidSupport = JFUnitySupportAndroid.getInstance();
        androidSupport.onStop(act);
#endif
        }

        public void onDestroy(AndroidJavaObject act)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        JFUnitySupportAndroid androidSupport = JFUnitySupportAndroid.getInstance();
        androidSupport.onDestroy(act);
#endif
        }

        public void onNewIntent(AndroidJavaObject act, AndroidJavaObject intent)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        JFUnitySupportAndroid androidSupport = JFUnitySupportAndroid.getInstance();
        androidSupport.onNewIntent(act, intent);
#endif
        }

        public void onActivityResult(AndroidJavaObject act, int requestCode, int resultCode, AndroidJavaObject intent)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        JFUnitySupportAndroid androidSupport = JFUnitySupportAndroid.getInstance();
        androidSupport.onActivityResult(act, requestCode, resultCode, intent);
#endif
        }

        public void onWindowFocusChanged(Boolean hasFocus)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        JFUnitySupportAndroid androidSupport = JFUnitySupportAndroid.getInstance();
        androidSupport.onWindowFocusChanged(hasFocus);
#endif
        }

        public void onBackPressed()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        JFUnitySupportAndroid androidSupport = JFUnitySupportAndroid.getInstance();
        androidSupport.onBackPressed();
#endif
        }

        public void onRequestPermissionsResult(AndroidJavaObject activity, int requestCode, String[] permissions, int[] grantResults)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        JFUnitySupportAndroid androidSupport = JFUnitySupportAndroid.getInstance();
        androidSupport.onRequestPermissionsResult(activity, requestCode, permissions, grantResults);
#endif
        }
    }


    public class JFUnitySupportAndroid
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaObject ao;
        AndroidJavaObject unityActivity;
        private static JFUnitySupportAndroid instance;

        private JFUnitySupportAndroid() {
            AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            unityActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass ac = new AndroidJavaClass("com.juefeng.sdk.juefengsdk.JFSDK");
            ao = ac.CallStatic<AndroidJavaObject>("getInstance");
        }

        public static JFUnitySupportAndroid getInstance()
        {
            if (instance == null)
            {
                instance = new JFUnitySupportAndroid();
            }

            return instance;
        }

		public void init(JFSDKListener listener)
        {
            if (listener == null)
            {
                Debug.LogError("set JFSDKListener error, listener is null");
                return;
            }
            if (ao == null)
            {
                Debug.LogError("setListener error, current activity is null");
            }
            else
            {
                AndroidJavaObject initApp = new AndroidJavaObject("com.juefeng.sdk.juefengsdk.JfApplication");
                initApp.Call("onCreate");
                Debug.Log("开始初始化SDK");
                ao.Call("onCreate", unityActivity);
                ao.Call("init", unityActivity, listener);
            }
        }

        public void doLogin()
        {
            Debug.Log("开始调用登录");
            ao.Call("doLogin", unityActivity);
        }



        public void showPay(JfOrderInfo orderInfo)
        {
            if (orderInfo == null)
            {
                Debug.LogError("call pay error, orderInfo is null");
                return;
            }
            AndroidJavaObject orderInfoJavaObj = new AndroidJavaObject("com.juefeng.sdk.juefengsdk.services.bean.JfOrderInfo");
            orderInfoJavaObj.Call("setLevel", orderInfo.getLevel());
            orderInfoJavaObj.Call("setCpOrderId", orderInfo.getCpOrderId());
            orderInfoJavaObj.Call("setRoleName", orderInfo.getRoleName());
            orderInfoJavaObj.Call("setRoleId", orderInfo.getRoleId());
            orderInfoJavaObj.Call("setServerName", orderInfo.getServerName());
            orderInfoJavaObj.Call("setServerId", orderInfo.getServerId());
            orderInfoJavaObj.Call("setVip", orderInfo.getVip());
            orderInfoJavaObj.Call("setPrice", orderInfo.getPrice());
            orderInfoJavaObj.Call("setGoodsName", orderInfo.getGoodsName());
            orderInfoJavaObj.Call("setGoodsDes", orderInfo.getGoodsDes());
            orderInfoJavaObj.Call("setGoodsId", orderInfo.getGoodsId());
            orderInfoJavaObj.Call("setRemark", orderInfo.getRemark());
            Debug.Log("开始调用SDK支付");
            ao.Call("showPay", unityActivity, orderInfoJavaObj);
        }
        public void showFloatView() {
            Debug.Log("开始显示悬浮球");
            ao.Call("showFloatView", unityActivity);
        }

        public void removeFloatView()
        {
            Debug.Log("开始显示悬浮球");
            ao.Call("removeFloatView");
        }

        public void exitLogin()
        {
            Debug.Log("开始调用SDK退出游戏");
            ao.Call("exitLogin", unityActivity);
        }
        public void logoutLogin()
        {
            Debug.Log("开始调用SDK退出当前账号登录");
            ao.Call("logoutLogin", unityActivity);
        }
        public void syncInfo(JfRoleInfo roleInfo)
        {
            if (roleInfo == null)
            {
                Debug.LogError("call pay error, roleInfo is null");
                return;
            }
            AndroidJavaObject roleInfoJavaObj = new AndroidJavaObject("com.juefeng.sdk.juefengsdk.services.bean.JfRoleInfo");
            roleInfoJavaObj.Call("setExperience", roleInfo.getExperience());
            roleInfoJavaObj.Call("setType", roleInfo.getType());
            roleInfoJavaObj.Call("setServerName", roleInfo.getServerName());
            roleInfoJavaObj.Call("setRoleName", roleInfo.getRoleName());
            roleInfoJavaObj.Call("setRoleId", roleInfo.getRoleId());
            roleInfoJavaObj.Call("setServerId", roleInfo.getServerId());
            roleInfoJavaObj.Call("setPartyId", roleInfo.getPartyId());
            roleInfoJavaObj.Call("setPartyName", roleInfo.getPartyName());
            roleInfoJavaObj.Call("setGameRoleLevel", roleInfo.getGameRoleLevel());
            roleInfoJavaObj.Call("setAttach", roleInfo.getAttach());
            roleInfoJavaObj.Call("setRoleCreateTime", roleInfo.getRoleCreateTime());
            roleInfoJavaObj.Call("setVipLevel", roleInfo.getVipLevel());
            roleInfoJavaObj.Call("setGameRolePower", roleInfo.getGameRolePower());
            Debug.Log("开始调用SDK同步游戏数据");
            ao.Call("syncInfo", roleInfoJavaObj);
        }
        public void switchAccount()
        {
            Debug.Log("开始调用SDK切换账户方法");
            ao.Call("switchAccount", unityActivity);
        }
        public void onCreate(AndroidJavaObject act)
        {
            Debug.Log("开始调用SDK onCreate 方法");
            ao.Call("onCreate", act);
        }

        public void onResume(AndroidJavaObject act)
        {
            Debug.Log("开始调用SDK onResume 方法");
            ao.Call("onResume", act);
        }

        public void onPause(AndroidJavaObject act)
        {
            Debug.Log("开始调用SDK onPause 方法");
            ao.Call("onPause", act);
        }

        public void onStart(AndroidJavaObject act)
        {
            Debug.Log("开始调用SDK onStart 方法");
            ao.Call("onStart", act);
        }

        public void onRestart(AndroidJavaObject act)
        {
            Debug.Log("开始调用SDK onRestart 方法");
            ao.Call("onRestart", act);
        }

        public void onStop(AndroidJavaObject act)
        {
            Debug.Log("开始调用SDK onStop 方法");
            ao.Call("onStop", act);
        }

        public void onDestroy(AndroidJavaObject act)
        {
            Debug.Log("开始调用SDK onDestroy 方法");
            ao.Call("onDestroy", act);
        }

        public void onNewIntent(AndroidJavaObject act, AndroidJavaObject intent)
        {
            Debug.Log("开始调用SDK onNewIntent 方法");
            ao.Call("onNewIntent", act, intent);
        }

        public void onActivityResult(AndroidJavaObject act, int requestCode, int resultCode, AndroidJavaObject intent)
        {
            Debug.Log("开始调用SDK onActivityResult 方法");
            ao.Call("onActivityResult", act, requestCode, resultCode, intent);
        }

        public void onWindowFocusChanged(Boolean hasFocus)
        {
            Debug.Log("开始调用SDK onWindowFocusChanged 方法");
            ao.Call("onWindowFocusChanged", hasFocus);
        }

        public void onBackPressed()
        {
            Debug.Log("开始调用SDK onBackPressed 方法");
            ao.Call("onBackPressed");
        }

        public void onRequestPermissionsResult(AndroidJavaObject activity, int requestCode, String[] permissions, int[] grantResults)
        {
            Debug.Log("开始调用SDK onRequestPermissionsResult 方法");
            ao.Call("onRequestPermissionsResult", activity, requestCode, permissions, grantResults);
        }
#endif
    }

}