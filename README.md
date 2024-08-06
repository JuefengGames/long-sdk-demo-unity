## demo 简介

本工程由unity开发平台生成，主要是为了帮助unity开发者在unity代码中接入 LongSDK 安卓端SDK。

## 项目结构

```
Assest
├── Plugins			           // LongSDK 文件存放和SDK接入代码目录通常不需要修改	  编写自己的工程时候请把本目录直接拷贝过去
|          ├──Android
|          |         ├──haiwai-5.4.3.aar   //LongSDK文件存放目录  如果要接入自己的工程需要修改包中的 AndroidManifest.xml相关配置
|          |          
|          ├──script                       //unity 对接 SDK相关代码 本目录下代码一般不需要修改
|          |         ├──JFSDK.cs
|          |         ├──JFSDKImp.cs
|          |         ├──JFSDKListener.cs
├── JFSDK_DEMO_UI.cs			// demo 工程调用sdk接口代码   如需在自己工程中调用LongSDK请参考此代码
└── ...
```

## 开发环境


Unity Hub Version：3.3.1-c3  
Unity Editor Version：2022.3.15f1c1



## 项目调试


1、将unity项目打包成apk文件  
2、将apk文件安装到模拟器或者安卓手机启动后进行调试



## 接入项目

### 一、 接入说明

#### 1.1：谁来阅读此文档

接入厂商的产品，技术人员，平台技术人员。

#### 1.2：注意事项

接口参数大小写敏感

request和response的编码格式为UTF-8。

#### 1.3：名词定义

名词	        说明  
CP	        游戏合作商  
APP	        接入平台的游戏  
JF_APPID	接入时由平台分配的游戏ID。  
JF_APPKEY	接入时由平台分配的游戏/应用密钥。

### 二、 拷贝配置文件

#### 2.1：SDK文件的拷贝

1、将demo工程Assest目录下面的Plugins文件夹整体拷贝到目标项目的Assest目录下面  
2、JFSDK.cs JFSDKImp.cs JFSDKListener.cs添加到工程中

#### 2.2：AndroidManifest.xml文件的配制

需要修改 aar包中的 AndroidManifest.xml 文件中的 JF_APPID和JF_APPKEY为绝锋游戏平台所提供的JF_APPID和JF_APPKEY

###三、 sdk业务接口代码说明

#### 3.1：sdk初始化，登录，注册，支付，退出等事件监听接口的实现及参数说明

##### 3.1.1：api说明

SDK的大部分API调用，使用了事件通知的方式回调给游戏,游戏通过创建一个Listener对象（需要在代码中生成一个类继承JFSDKListener类并实现回调方法，请参考demo），实现对SDK的每个事件结果的监听以下是代码部分（包含对每个回调接口的解释，及参数说明）。

##### 3.1.2：代码部分：

生成一个类并继承JFSDKListener类
```
public class CallBackListener : JFSDKListener
{
    public override void onCancleExitCallback(string desc)
    {
        Debug.Log("取消退出游戏");
    }
    ...
}
```

##### 3.1.3：接口及参数说明

| 方法  | 说明 |
|-------------|-------------|
|onInitSuccessCallback(String desc,boolean isAutoLogin)|初始化成功；参数说明：desc(字符串“初始化成功”)IsAutoLogin(是否可调取自动登录)|
|onInitFaildCallback(String desc)|初始化失败；参数说明：desc(字符串“初始化失败原因”)|
|onLoginSuccessCallback(LogincallBack logincallBack)|登录成功参数说明：logincallBack(登录成功后返回的用户信息对像)|
|onLoginFailedCallback(LoginErrorMsgerrorMsg)|登录失败参数说明：errorMsg (登录失败后返回的错误码，及失败原因)|
|onPaySuccess (PaySuccessInfopaySuccessInfo)|支付成功参数说明：paySuccessInfo(支付成功后返回的订单信息，含有订单号，此处还需要去跟游戏服务器确认支付信息，才可确定支付成功)|
|onPayFaild(PayFaildInfopayFaildInfo)|支付失败参数说明：payFaildInfo(支付失败返回的信息)|
|onExitCallback(String desc)|退出成功参数说明：desc(字符串“成功退出”)|
|onCancleExitCallback(String desc)|取消退出，选择继续游戏参数说明：desc(字符串“取消退出”)|
|onCreatedOrderCallback(CreateOrderInfocreatOrderInfo)|SDK服务器已经成功创建此订单creatOrderInfo订单信息|
|onLogoutLoginCallback()|注销账号登录（需在此回调中退出游戏，返回登录页面）|
|onSwitchAccountSuccessCallback|此回调接口是在当SDK内部有切换帐号的功能，且切换成功时会调用，游 戏方需要在这个回调接口中注销原来的角色数据，然后根据新的 (参数 login中可以获取到)来重新加载角色数据；|
|onGameSwitchAccountCallback|此接口是在游戏内有账号切换功能点击 调用 JFSDK.getInstance().switchAccount(MainActivity.this);后回调 游戏方账号切换逻辑需要在此回调中执行|