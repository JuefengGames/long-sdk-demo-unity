## LANGUAGE

[reading in english | 阅读英文文档](https://github.com/JuefengGames/long-sdk-demo-unity/blob/main/README.md)  

## demo 简介

本工程由unity开发平台生成，主要是为了帮助unity开发者在unity代码中接入 LongSDK 安卓端SDK。

## 项目结构

```
Assest
├── Plugins			           // LongSDK 文件存放和SDK接入代码目录通常不需要修改	  编写自己的工程时候请把本目录直接拷贝过去
|          ├──Android
|          |         ├──JFSDK5.6.0.aar   //LongSDK文件存放在此处  
|          |         ├──AndroidManifest.xml   //  项目的清单文件放在此处，unity打包安卓apk文件就会用这个清单文件打包，最好保证aar包中的AndroidManifest.xml文件和这个位置的文件内容保持一致
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

|名词|说明|
|-------------|-------------|
|CP|游戏合作商|
|APP|接入平台的游戏|
|JF_APPID|接入时由平台分配的游戏ID|
|JF_APPKEY|接入时由平台分配的游戏/应用密钥|

### 二、 拷贝配置文件

#### 2.1：SDK文件的拷贝

1、将demo工程Assest目录下面的Plugins文件夹整体拷贝到目标项目的Assest目录下面  
2、JFSDK.cs JFSDKImp.cs JFSDKListener.cs添加到工程中

#### 2.2：AndroidManifest.xml文件的配制

需要修改 aar包中和aar包同目录下的 AndroidManifest.xml 文件中的 JF_APPID和JF_APPKEY为绝锋游戏平台所提供的JF_APPID和JF_APPKEY

### 三、 sdk业务接口代码说明

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

#### 3.2：sdk的初始化

##### 3.2.1：代码部分

主类中生成对象
```
using jfsdk;
private static CallBackListener JFListener;
```

初始化方法中调用
```
JFListener = new CallBackListener();
JFSDK.getInstance().init(JFListener);
```

##### 3.2.2：参数说明

1、初始化sdk资源，注册sdk事件监听器。  

2、CallBackListener 为3.1.2中生成的类

##### 3.2.3：回调说明

初始化的成功失败会回调 3.1中的onInitSuccessCallback 和 onInitFaildCallback cp可根据回调结果做出相应处理

#### 3.3：登录

##### 3.3.1：登录拉起说明

方法需要在UI线程中调用，

代码调用：
```
JFSDK.getInstance().doLogin();
```

##### 3.3.2：登录回调说明

回调说明:登录和注册的失败会回调到3.1中的onLoginSuccessCallback和onLoginFailedCallback cp可根据回调结果做出相应处理

##### 3.3.3：返回参数说明：

登录或注册成功回调方法：onLoginSuccessCallback
回调参数说明：
|参数名|类型|参数说明|
|-------------|-------------|-------------|
|userId|string|登录成功后，用户的]()userId（唯一）|
|token|string|用户此次登录平台分配的唯一token（唯一）|
|userName|string|用户名|
|isAuthenticated|boolean|是否已经实名认证,true(是),false(否)|
|pi|String|(防沉迷预留字段)|
|age|int|年龄|


登录失败回调方法：onLoginFaild
该方法在产生业务逻辑错误时调用。
回调参数说明：
|参数名|类型|参数说明|
|-------------|-------------|-------------|
|code|string|登录失败错误码|
|errorMsg|string|登录失败的消息提示|

#### 3.4：支付

##### 3.4.1 支付接口

```
JFSDK.getInstance().showPay(jfOrderInfo);
```

参数说明
|字段|说明|
|-------------|-------------|
|level	角色等级|
|goodsId|商品Id (商品编号) 没有传 “1”|
|goodsName|商品名称(String)不可为null不可为空串|
|goodsDes|商品描述(String)不可为null不可为空串|
|price|商品价格，不可为null、""， 默认为单位为美金（如果是默认为其他货币单位，可以和绝峰运营提前沟通），支持两位小数。|
|serverId|区服ID 不可为null不可为空串|
|serverName|区服名称（必传）不可为null不可为空串|
|roleId|角色游戏内唯一标示（必传）(不可为null不可为空串)|
|roleName|角色名称（必传）|
|vip|用户Vip等级 没有传 “1”|
|remark|透传字段（无特殊情况 请传入订单号（cpOrderId））|
|cpOrderId|Cp生成的订单号（必传）|

##### 3.4.2：支付的回调

支付涉及三个回调，创建订单成功，支付成功，支付失败。分别会回调到3.1中的

onCreatedOrderCallback，onPaySuccess，onPayFaild

##### 3.4.3：回调返回对象字段解释

onPaySuccess，onCreatedOrderCallback，onPayFaild
```
PaySuccessInfo｛
public String orderId;  //服务端生成的唯一订单号

public String gameRole;// 游戏端传入的角色Id

public String gameArea;  //游戏端传入的区服信息

public String productName;  //游戏端传入的商品名称

public String productDesc;  ///游戏端传入的商品描述

public String remark;  //游戏端传入的自定义参数

public String cpOrderId;  //cp传入的订单号

｝

CreateOrderInfo｛

public String orderId;  //sdk服务端生成的唯一订单号

public String gameRole;//游戏端传入的角色Id

public String gameArea;  //游戏端传入的区服信息

public String productName;  //游戏端传入的商品名称

public String productDesc;  //游戏端传入的商品描述

public String cpOrderId;  //cp传入的订单号

public String remark;  //游戏端传入的自定义参数｝

PayFaildInfo｛

private String code; //错误码

private String msg;	//失败说明	｝

```

#### 3.5：平台浮点，用户中心（必须调用）

##### 3.5.1：功能说明

登录完成后，可以通过悬浮按钮进入个人中心  

 查看平台帐号信息  

 设置帐号密码保护  
 
 帐号绑定手机  

 修改帐号密码  

 管理支付密码，查看充值和消费记录  

##### 3.5.2：API调用

1、浮点的显示  

代码：
```
JFSDK.getInstance().showFloatView();
```

2、浮点的隐藏
代码：
```
JFSDK.getInstance().removeFloatView(){}//隐藏悬浮窗口
```
##### 3.5.3：API调用位置

1：浮点的显示

登陆成功回调内调用


#### 3.6: 游戏数据同步（必须调用）

##### 3.6.1:说明

首次登入游戏或角色信息发生变化时sdk需要同步角色信息，角色信息分为四种类型

|类型|Type值|调用方法|
|-------------|-------------|-------------|
|角色创建|1|JFSDK.getInstance().syncInfo(roleInfo);|
|角色登陆|2|JFSDK.getInstance().syncInfo(roleInfo);|
|角色升级|3|JFSDK.getInstance().syncInfo(roleInfo);|
|角色退出|4|JFSDK.getInstance().syncInfo(roleInfo);|


代码如下3.6.2所示，不同类型传入不同type值

##### 3.6.2：调用示例(角色创建)

|字段|说明|
|-------------|-------------|
|serverName|服务器名称(必传)|
|serverId|服务器ID(必传)|
|roleName|角色名称(必传)|
|roleId|角色ID(必传)|
|partyId|公会id（尽量传）|
|partyName|公会名称（尽量传）|
|gameRoleLevel|角色等级(必传)|
|attach|额外字段|
|type|1:创建，2：登录，3：升级 4：退出(必传)|
|experience|当前经验值（尽量传）|
|roleCreateTime|角色创建时间 long（type为1传入）|
|vipLevel|Vip等级 int（尽量传）|
|gameRolePower|战力值 int（尽量传）|

调用代码：
```
JfRoleInfo roleInfo = new JfRoleInfo();

roleInfo.setGameRoleLevel("3");

roleInfo.setRoleId("角色id");

roleInfo.setGameRolePower(55555);

roleInfo.setServerId("服务器ID");

roleInfo.setServerName("服务器名称");

roleInfo.setRoleName("柳鸿振");

roleInfo.setExperience("135355446");

roleInfo.setPartyId("公会ID");

roleInfo.setPartyName("公会名称");

roleInfo.setRoleCreateTime(System.currentTimeMillis());

roleInfo.setVipLevel(5);

roleInfo.setType("1");

JFSDK.getInstance().syncInfo(roleInfo);
```

#### 3.8：游戏退出

##### 3.8.1：API的使用

调用JFSDK.getInstance().exitLogin();

##### 3.8.2：回调说明

点击返回键调用 JFSDK.getInstance().exitLogin();

选择退出是 会回调 （3.1）SdkEventListener 内的 onExitCallback()方法 在此方法里面执行游戏界面的关闭



##### 3.8.3：游戏内退出

游戏内出现顶号或者主动注销账号的时候调用

JFSDK.getInstance().logoutLogin();

#### 3.9：注销登录（必须实现）

点击个人中心的退出当前账户按钮，会回调**（3.1）中的onLogoutLoginCallback**接口 此接口内需实现返回游戏登录界面的代码 游戏方执行

#### 3.10:SDK生命周期（SDK已经自动实现 需要在AndroidManifest.xml文件中加入如下配置，参考demo）

```
    <application  android:name="com.juefeng.sdk.juefengsdk.JfApplication">
       <activity android:name="com.juefeng.sdk.juefengsdk.ui.activity.JfUnityActivity"
            android:theme="@android:style/Theme.Light.NoTitleBar.Fullscreen"
            android:screenOrientation="landscape"
            android:launchMode="singleTask"
			android:exported="true"
            android:configChanges="orientation|navigation|screenSize|keyboard|keyboardHidden">
            <intent-filter>
                <action android:name="android.intent.action.MAIN" />
                <category android:name="android.intent.category.LAUNCHER" />
            </intent-filter>
        </activity>
	......
     </application>
```

#### 生命周期手动调用接口

```
public void onCreate(AndroidJavaObject act);（必须先于 SDK 的 init 前调用）

public void onResume(AndroidJavaObject act);

public void onPause(AndroidJavaObject act);

public void onStart(AndroidJavaObject act);

public void onRestart(AndroidJavaObject act);

public void onStop(AndroidJavaObject act);

public void onDestroy(AndroidJavaObject act);

public void onNewIntent(AndroidJavaObject act, AndroidJavaObject intent);

public void onActivityResult(AndroidJavaObject act,int requestCode, int resultCode, AndroidJavaObject intent);

public void onWindowFocusChanged(boolean hasFocus)

public void onBackPressed()

public void onRequestPermissionsResult(Activity AndroidJavaObject, int requestCode, String[] permissions, int[] grantResults)
```

#### 3.11:Application的添加（必接）
若 App本身无自定义Application， 请在AndroidManifest.xml 中接入  


若 App本身有自定义Application,请继承或者调用 com.juefeng.sdk.juefengsdk. JfApplication  

本sdk自带的的初始化方法中已经调用，如果碰到问题可以去掉自己实现


#### 3.12：游戏内账号切换

游戏内有账号切换按钮时点击按钮需要调用一下接口

JFSDK.getInstance().switchAccount();

接口会回调Listener 中的onGameSwitchAccountCallback接口参考3.1.3在接口内做相关处理

#### 4 混淆

JFSDK包是以jar提供给用户的，已经半混淆状态，您在混淆自己APK包的时候请不要将jar包一起混淆，因为里面有自定义UI控件，若被混淆后会因为无法找到相关类而抛异常

## 联系我们

If you have any questions about this program, please contact us!  

Email: ouyangjie@juefeng.com