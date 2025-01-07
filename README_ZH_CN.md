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
|onCreatedOrder(CreateOrderInfo creatOrderInfo)|SDK服务器已经成功创建此订单creatOrderInfo订单信息[废弃，请勿使用]|
|onLogoutLoginCallback()|注销账号登录（需在此回调中退出游戏，返回登录页面）|
|onSwitchAccountSuccessCallback|此回调接口是在当SDK内部有切换帐号的功能，且切换成功时会调用，游 戏方需要在这个回调接口中注销原来的角色数据，然后根据新的 (参数 login中可以获取到)来重新加载角色数据；|
|onGameSwitchAccountCallback|此接口是在游戏内有账号切换功能点击 调用 JFSDK.getInstance().switchAccount(MainActivity.this);后回调 游戏方账号切换逻辑需要在此回调中执行|
|onSyncSuccess|研发使用自己登录时，同步给渠道userId，成功时回调|
|onSyncFailure(String msg)|研发使用自己登录时，同步给渠道userId，失败时回调，当收到此回调时，请勿进行下一步操作，同登录失败处理|

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

为了适配下游渠道，获取更多用户流量，默认情况下游戏母包内需要同时包含“游戏原有的登录”，“绝峰的登录”。上架具体渠道的时候，绝峰这边会分包，同时设置符合渠道要求的登录方式。（两种登录方式不会出现在同一个渠道包体内）

##### 3.3.1 判断是否使用绝峰登录 (推荐接入)

调用方式
```
LoginType loginType = JFSDK.getInstance().getLoginType();

public enum LoginType
{
    JuefengLogin,  // 绝峰登录
    GameLogin      // 研发使用自己的登录
}


/**
* 以下配置请在AndroidManifest.xml文件中配置，用于母包切换的测试，value值会影响getLoginType()方法的返回值；
* 此逻辑仅在母包中测试使用，后续渠道包会替换此逻辑，改由绝峰控制
*
* value=1：JUEFENG_LOGIN
* value=0: GAME_LOGIN
*/

<meta-data android:name="JF_LOGIN_TYPE" android:value="1"/>
```

##### 3.3.2 绝峰登录

##### 3.3.2.1：绝峰登录拉起说明

方法需要在UI线程中调用，

代码调用：
```
JFSDK.getInstance().doLogin();
```

##### 3.3.2.2：登录回调说明

回调说明:登录和注册的失败会回调到3.1中的onLoginSuccessCallback和onLoginFailedCallback cp可根据回调结果做出相应处理

##### 3.3.2.3：返回参数说明：

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

##### 3.3.3 研发方使用自己的登录

代码调用：
```
JFSDK.getInstance().syncUserId(String userId, String token);
```

若使用游戏自己的账号体系（使用游戏自带的登录注册功能），则需要接入此方法。请将登录后的用户唯一标识userId及用户token凭证使用本方法接入，接入后会回调onSyncSuccess方法
注：为了保证用户安全及合法性，建议研发根据我方要求提供一个服务端验证接口，userId,token 将作为验证参数传入 具体参考服务端文档

参数说明：
|参数名|类型|参数说明|
|-------------|-------------|-------------|
|userId|string|当前游戏唯一的用户ID，最大长度32位以内。|
|token|string|用户登录成功的凭证，建议每次传唯一值。|

注：为了保证用户安全及合法性，建议研发根据我方要求提供一个服务端验证接口，userId,token 将作为验证参数传入 具体参考服务端文档

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
|goodsDes|商品描述(String)不可为null不可为空串，最大长度：64字符|
|price|商品价格，不可为null、""， 默认为单位为美金（如果是默认为其他货币单位，可以和绝峰运营提前沟通），支持两位小数。|
|serverId|区服ID 不可为null不可为空串，需和角色信息同步中的值相同|
|serverName|区服名称（必传）不可为null不可为空串，需和角色信息同步中的值相同|
|roleId|角色游戏内唯一标示（必传）(不可为null不可为空串)，需和角色信息同步中的值相同|
|roleName|角色名称（必传），需和角色信息同步中的值相同|
|vip|用户Vip等级 没有传 “1”|
|remark|透传字段（无特殊情况 请传入订单号（cpOrderId）），最大长度：64字符|
|cpOrderId|游戏开发者的订单号，请务必保证数据唯一。|

注：所有字段不得为空，没有默认传值“1”

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

#### 3.5：平台浮点，用户中心（SDK已经自动调用实现无需手动调用）

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
|roleCreateTime|type = 1时必传，角色创建时间 （必传|
|vipLevel|Vip等级 int（尽量传）|
|gameRolePower|战力值 int（尽量传）|

注：所有字段不得为空，如果无法提供数据，默认传值“1”

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
调用此接口后会回调onLogoutLogin（回调接口见3.1api说明），请在此回调中退出游戏至登录见面，然后重新调用登录接口

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

```
<application  android:name="com.juefeng.sdk.juefengsdk.JfApplication" />
```

若 App本身有自定义Application,请继承或者调用 com.juefeng.sdk.juefengsdk. JfApplication  

本sdk自带的的初始化方法中已经调用，如果碰到问题可以去掉自己实现


#### 3.12：游戏内账号切换

游戏内有账号切换按钮时点击按钮需要调用一下接口

JFSDK.getInstance().switchAccount();

接口会回调Listener 中的onGameSwitchAccountCallback接口参考3.1.3在接口内做相关处理

#### 3.13：获取下游渠道标识

```
String channelType = JFSDK.getInstance().getChannelType()；
```

用途说明： 集成这个API , 您可以按子渠道等维度统计各个渠道的数据，更好的支撑市场运营计划。

目前海外渠道编号如下(区分大小写)

|渠道名称|编号|
|-------------|-------------|
|绝峰母包|jfgame|
|小米|xiaomiglobal|
|小米测试|mitest|
|TapTap|taptap|
|RuStore|RuStore|
|QooApp|QooAPP|
|OneStore|onestore|
|雷电模拟器|leidianglobal|
|荣耀手机|rongyao|
|华为手机|huawei|
|蓝叠模拟器|nowgg|
|Aptoide|Aptoide|
|AppBazar|AppBazar|
|亚马逊|Amazon|
|小七海外版|xiao7|
|xsolla|aikesuola|
|DMM|dmm|
|三星手机|sanxingglobal|

#### 3.13：获取商品信息【可选】

与 JFSDK 建立连接后，您就可以查询可售的商品并将其展示给用户了。
在将商品展示给用户之前，查询商品详情是非常重要的一步，因为查询会返回本地化的商品信息。
如需查询应用内商品详情，请调用 queryProductDetailsAsync。
支持单个和批量查询，当 finalList 为空时 会返回所有商品信息。

示例代码：
```

//查询商品详情
List<String> finalList = new List<String>();
//不添加id会返回所有商品信息，添加id会返回特定商品信息
finalList.Add("charge_2.99");
PdListener = new CallBackPdListener();
JFSDK.getInstance().queryProductDetailsAsync(finalList, PdListener);

```

返回商品字段信息具体获取方法请看demo代码
```

    "currency": "USD",//货币单位
    "describe": "You can get the goods after purchase.",//商品描述
    "currencySymbol": "₽",//货币单位符号
    "price": "0.99",//价格
    "sku": "charge_0.99",//商品id
    "title": "Pack I"//商品名称

```

#### 4 最佳实践

##### 4.1 混淆

JFSDK包是以jar提供给用户的，已经半混淆状态，您在混淆自己APK包的时候请不要将jar包一起混淆，因为里面有自定义UI控件，若被混淆后会因为无法找到相关类而抛异常

##### 4.2：APK 包名

如果方便，请使用.jf 结尾即可，并请允许绝峰修改游戏包名称。分包的时候需要这么做。
如果您的服务器端会校验包名，绝峰这边也可以提前收集所需要的包名列表，提交白名单。
这样做的主要原因是我们的渠道（华为、三星）等对游戏的包名有特殊的要求。

##### 4.3：母包测试【重要】

为方便了解SDK集成情况，提升对接效率，对接完成后，请务必进行母包测试。
自测：点击悬浮窗 --> 个人中心 --> 生命周期测试  查看是否有未完成接入接口，未完成的需要对接完成，无法完成对接群联系我方开发人员沟通. 

##### 4.4： 游戏内商品价格单位

SDK集成的时候，母包内显示的货币单位是人民币，但请放心，分包的时候，渠道那边会展示当地的货币。


## 联系我们

If you have any questions about this program, please contact us!  

Email: ouyangjie@juefeng.com