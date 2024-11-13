## LANGUAGE

[reading in chinese | 阅读中文文档](https://github.com/JuefengGames/long-sdk-demo-unity/blob/main/README_ZH_CN.md)  

## Demo Introduction

This project is generated by the Unity development platform and is primarily aimed at assisting Unity developers in integrating the LongSDK Android SDK into Unity code.

## Project Structure

```
Assets
├── Plugins                        // LongSDK files and SDK integration codes are usually placed here. Normally, no modifications are needed. Please copy this directory directly to your own project.
|          ├── Android
|          |         ├── JFSDK5.6.0.aar   // LongSDK files are stored here
|          |         ├── AndroidManifest.xml   // The manifest file of the project is placed here. When Unity packages an Android APK file, it uses this manifest file for packaging. It's best to ensure that the AndroidManifest.xml file content in the aar package and this location are consistent.
|          |
|          ├── script                       // Unity SDK integration related code. The code in this directory usually doesn't require modification.
|          |         ├── JFSDK.cs
|          |         ├── JFSDKImp.cs
|          |         ├── JFSDKListener.cs
├── JFSDK_DEMO_UI.cs             // This is the code for calling the SDK interface in the demo project. Refer to this code when calling LongSDK in your project.
└── ...
```

## Development Environment

Unity Hub Version: 3.3.1-c3  
Unity Editor Version: 2022.3.15f1c1

## Project Debugging

1. Package the Unity project into an APK file.
2. Install the APK file on an emulator or Android phone and start debugging.

## SDK Integration

### 1. Introduction

#### 1.1: Who Should Read This Document

Technical personnel from partner companies and platform technical personnel.

#### 1.2: Important Notices

- Interface parameters are case-sensitive.
- The encoding format for requests and responses is UTF-8.

#### 1.3: Definitions

| Term       | Description                                     |
|------------|-------------------------------------------------|
| CP         | Game partner                                    |
| APP        | Game integrated into the platform               |
| JF_APPID   | Game ID assigned by the platform upon integration|
| JF_APPKEY  | Game/application key assigned by the platform   |

### 2. Copy Configuration Files

#### 2.1: Copy SDK Files

1. Copy the entire Plugins folder from the demo project's Assets directory to your target project's Assets directory.
2. Add JFSDK.cs, JFSDKImp.cs, and JFSDKListener.cs to your project.

#### 2.2: Configure AndroidManifest.xml

Modify the JF_APPID and JF_APPKEY in the AndroidManifest.xml file located in the same directory as the aar package and the AndroidManifest.xml file in your project directory to the values provided by the Juefeng Gaming Platform.

### 3. SDK Business Interface Code Explanation

#### 3.1: Implementation of SDK Initialization, Login, Registration, Payment, Exit, and Other Event Listener Interfaces

##### 3.1.1: API Description

Most of the SDK API calls utilize event notification for callbacks to the game. Games need to create a Listener object by defining a class that extends the JFSDKListener class and implements callback methods (refer to the demo). Below is part of the code with explanations and parameter descriptions for each callback interface.

##### 3.1.2: Code Example

Create a class that extends the JFSDKListener class:

```csharp
public class CallBackListener : JFSDKListener
{
    public override void onCancleExitCallback(string desc)
    {
        Debug.Log("Game exit canceled");
    }
    ...
}
```

##### 3.1.3: Interface and Parameter Descriptions

| Method                                   | Description                                                                                                      |
|------------------------------------------|------------------------------------------------------------------------------------------------------------------|
| onInitSuccessCallback(String desc, boolean isAutoLogin) | Initialization success; Parameters: desc (string "Initialization successful"), isAutoLogin (boolean, can auto-login)| 
| onInitFaildCallback(String desc)         | Initialization failure; Parameters: desc (string "Reason for initialization failure")                           |
| onLoginSuccessCallback(LogincallBack logincallBack) | Login success; Parameters: logincallBack (object containing user info upon successful login)                     |
| onLoginFailedCallback(LoginErrorMsgerrorMsg) | Login failure; Parameters: errorMsg (error message with a code and reason for failure)                             |
| onPaySuccess(PaySuccessInfo paySuccessInfo) | Payment success; Parameters: paySuccessInfo (information on successful order, verify on server for final confirmation)|
| onPayFaild(PayFaildInfo payFaildInfo)    | Payment failure; Parameters: payFaildInfo (error message and explanation)                                        |
| onExitCallback(String desc)              | Exit success; Parameters: desc (string "Successfully exited")                                                   |
| onCancleExitCallback(String desc)        | Cancel exit, choose to continue the game; Parameters: desc (string "Cancel exit")                               |
| onCreatedOrderCallback(CreateOrderInfo creatOrderInfo) | Successful order creation; creatOrderInfo (order information)                                                   |
| onLogoutLoginCallback()                  | Logout account (exit game, return to login screen on this callback)                                             |
| onSwitchAccountSuccessCallback           | Triggered when an internal SDK account switch succeeds. Clear old role data and load new data accordingly.          |
| onGameSwitchAccountCallback              | Invoked when the game has an account switch feature. Implement switch account logic within this callback.           |
| onSyncSuccess                            | Called when using custom login, sync userId successfully                                                        |

#### 3.2: SDK Initialization

##### 3.2.1: Code

Define an object in the main class:
```csharp
using jfsdk;
private static CallBackListener JFListener;
```

Invoke in the initialization method:
```csharp
JFListener = new CallBackListener();
JFSDK.getInstance().init(JFListener);
```

##### 3.2.2: Parameter Description

1. Initializes SDK resources and registers SDK event listeners.

2. CallBackListener is the class generated in 3.1.2.

##### 3.2.3: Callback Description

Initialization success or failure will trigger callbacks onInitSuccessCallback and onInitFaildCallback in 3.1. CP should handle these accordingly.

#### 3.3: Log In (Choose one option)

##### 3.3.1 Juefeng Login

##### 3.3.1.1: Juefeng Login Initiation

This method should be invoked in the UI thread.

Code invocation:
```csharp
JFSDK.getInstance().doLogin();
```

##### 3.3.1.2: Login Callback Description

Login and registration failures will trigger callbacks onLoginSuccessCallback and onLoginFailedCallback in 3.1. CP should handle these accordingly.

##### 3.3.1.3: Return Parameters Description:

Successful login or registration callback method: onLoginSuccessCallback. Explanation for callback parameters:
| Parameter        | Type   | Description                                                       |
|------------------|--------|-------------------------------------------------------------------|
| userId           | string | UserId after a successful login (unique)                          |
| token            | string | Unique login token assigned to the user by the platform (unique)  |
| userName         | string | Username                                                         |
| isAuthenticated  | boolean| Whether the user is authenticated, true(yes) or false(no)         |
| pi               | String | (Anti-addiction reserved field)                                  |
| age              | int    | User's age                                                       |

Login failure callback method: onLoginFaild. This method is called in case of business logic errors.
Explanation for callback parameters:
| Parameter  | Type   | Description                      |
|------------|--------|----------------------------------|
| code       | string | Error code for login failure     |
| errorMsg   | string | Error message for login failure  |

##### 3.3.2 Using Your Own Login

Code invocation:
```csharp
JFSDK.getInstance().syncUserId(String userId, String token);
```

If using the game's own account system, integrate using this method. Pass the unique userId and token obtained after login. Integration will trigger the onSyncSuccess method. Note: To ensure user security and legality, it's recommended to provide a server-side verification interface according to our guidelines. The userId and token will be verification parameters (refer to the server documentation for more details).

#### 3.4: Payments

##### 3.4.1 Payment Interface

Invoke this interface:
```
JFSDK.getInstance().showPay(jfOrderInfo);
```

Parameter Description:
| Field          | Description                                                |
|----------------|------------------------------------------------------------|
| level          | Role level                                                |
| goodsId        | Product ID. If not available, pass "1".                    |
| goodsName      | Product name (String). Cannot be null or empty.            |
| goodsDes       | Product description (String). Cannot be null or empty. Maximum length: 64 characters. |
| price          | Product price. Cannot be null or empty. Default currency is USD (can discuss other currencies with Juefeng operations in advance). Supports two decimal places. |
| serverId       | Server ID. Must match the value passed in role info synchronization. |
| serverName     | Server name. Must match the value passed in role info synchronization. |
| roleId         | Unique role identifier within the game. Must match the value passed in role info synchronization. |
| roleName       | Role name. Must match role info synchronization value.    |
| vip            | User's VIP level. If not available, pass "1".            |
| remark         | Custom field. Preferably pass the order number (cpOrderId). Maximum length: 64 characters. |
| cpOrderId      | Developer's order number, must be unique.                |

Note: Fields cannot be empty. If default values aren't available, use "1".

##### 3.4.2 Payment Callbacks

Payments involve three callbacks: order creation success, payment success, and payment failure. They trigger callbacks in 3.1:
- onCreatedOrderCallback
- onPaySuccess
- onPayFaild

##### 3.4.3 Callback Object Field Explanation

onPaySuccess, onCreatedOrderCallback, onPayFaild:
```csharp
PaySuccessInfo {
    public String orderId;      // Unique order number created by the server
    public String gameRole;     // Game role ID as passed by the client
    public String gameArea;     // Game area information as passed by the client
    public String productName;  // Product name as passed by the client
    public String productDesc;  // Product description as passed by the client
    public String remark;       // Custom user parameter
    public String cpOrderId;    // Order ID as passed by the client
}

CreateOrderInfo {
    public String orderId;      // SDK server's unique order number
    public String gameRole;     // Game role ID as passed by the client
    public String gameArea;     // Game area passed by the client
    public String productName;  // Product name passed by the client
    public String productDesc;  // Product description passed by the client
    public String cpOrderId;    // Client's order number
    public String remark;       // Custom user parameter
}

PayFaildInfo {
    public String code;         // Error code
    public String msg;          // Error message description
}
```

#### 3.5: Platform Floating, User Center (Required)

##### 3.5.1 Functionality Description

Once logged in, the personal center can be accessed via a floating button to:
- View platform account information.
- Set account password protection.
- Bind account with phone.
- Change account password.
- Manage payment passwords and view recharge/expenditure records.

##### 3.5.2 API Calls

1. Show floating widget:  
```csharp
JFSDK.getInstance().showFloatView();
```

2. Hide floating widget:
```csharp
JFSDK.getInstance().removeFloatView(); // Hide floating window
```

##### 3.5.3 API Call Location

- Display the floating widget after login success callback.

#### 3.6: Game Data Synchronization (Required)

##### 3.6.1 Explanation

The SDK requires role information synchronization when logging in for the first time or when role information changes. There are four types of role info:

| Type       | Type Value | Method Invocation                           |
|------------|------------|----------------------------------------------|
| Role Creation | 1          | `JFSDK.getInstance().syncInfo(roleInfo);`    |
| Role Login  | 2          | `JFSDK.getInstance().syncInfo(roleInfo);`    |
| Role Upgrade| 3          | `JFSDK.getInstance().syncInfo(roleInfo);`    |
| Role Exit   | 4          | `JFSDK.getInstance().syncInfo(roleInfo);`    |

Please use the correct type value when invoking the method as shown in 3.6.2.

##### 3.6.2 Invocation Example (Role Creation)

| Field           | Description                                      |
|-----------------|--------------------------------------------------|
| serverName      | Server name (required)                           |
| serverId        | Server ID (required)                             |
| roleName        | Role name (required)                             |
| roleId          | Role ID (required)                               |
| partyId         | Guild ID (preferred)                             |
| partyName       | Guild name (preferred)                           |
| gameRoleLevel   | Role level (required)                             |
| attach          | Additional field                                 |
| type            | 1: Creation, 2: Login, 3: Upgrade, 4: Exit (required) |
| experience      | Current experience (preferred)                   |
| roleCreateTime  | Required when type=1, role creation time (required) |
| vipLevel        | VIP level (int, preferred)                       |
| gameRolePower   | Combat power (int, preferred)                    |

Note: Fields cannot be empty. If default values aren't available, use "1".

Code invocation:
```csharp
JfRoleInfo roleInfo = new JfRoleInfo();

roleInfo.setGameRoleLevel("3");
roleInfo.setRoleId("Role ID");
roleInfo.setGameRolePower(55555);
roleInfo.setServerId("Server ID");
roleInfo.setServerName("Server Name");
roleInfo.setRoleName("Player Name");
roleInfo.setExperience("135355446");
roleInfo.setPartyId("Guild ID");
roleInfo.setPartyName("Guild Name");
roleInfo.setRoleCreateTime(System.currentTimeMillis());
roleInfo.setVipLevel(5);
roleInfo.setType("1");

JFSDK.getInstance().syncInfo(roleInfo);
```

#### 3.8: Game Exit

##### 3.8.1 API Usage

Call `JFSDK.getInstance().exitLogin();`

##### 3.8.2 Callback Description

Clicking the back button will invoke `JFSDK.getInstance().exitLogin();`. Upon selecting to exit, it will callback onExitCallback() method in SDKEventListener (3.1). Implement the game's interface close actions here.

##### 3.8.3 In-Game Exit

Invoke `JFSDK.getInstance().logoutLogin();` during forced logout or user-initiated account logout. This interface will callback onLogoutLogin (refer to 3.1.3). Return to the login screen, then re-call the login interface.

#### 3.9: Logout (Mandatory)

Clicking the logout button in the personal center will callback the onLogoutLoginCallback interface in 3.1. Implement game-specific logout actions to return to the login screen here.

#### 3.10: SDK Lifecycle (SDK automatically handles this, add following in AndroidManifest.xml, refer to the demo)

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

#### Manual Lifecycle Call Interface

```
public void onCreate(AndroidJavaObject act); // Must be called before SDK's init

public void onResume(AndroidJavaObject act);

public void onPause(AndroidJavaObject act);

public void onStart(AndroidJavaObject act);

public void onRestart(AndroidJavaObject act);

public void onStop(AndroidJavaObject act);

public void onDestroy(AndroidJavaObject act);

public void onNewIntent(AndroidJavaObject act, AndroidJavaObject intent);

public void onActivityResult(AndroidJavaObject act, int requestCode, int resultCode, AndroidJavaObject intent);

public void onWindowFocusChanged(boolean hasFocus);

public void onBackPressed();

public void onRequestPermissionsResult(Activity AndroidJavaObject, int requestCode, String[] permissions, int[] grantResults);
```

#### 3.11: Application Configuration (Required)

If the app does not have a custom Application, integrate it in AndroidManifest.xml using:
```
<application  android:name="com.juefeng.sdk.juefengsdk.JfApplication" />
```

If the app has a custom Application, extend or call com.juefeng.sdk.juefengsdk.JfApplication. Note that the initialization method in this SDK has already made this call. Remove if causing problems and implement it yourself.

#### 3.12: In-Game Account Switching

When there's an account switch button in the game, clicking it should invoke the following interface:
`JFSDK.getInstance().switchAccount();`
The interface will callback onGameSwitchAccountCallback in the Listener (refer to 3.1.3). Implement related actions here.

#### 3.13: Obtain Sub-channel Identifier

Use the following to obtain the current sub-channel type:
```csharp
String channelType = JFSDK.getInstance().getChannelType();
```

Current overseas channel identifiers are as follows (case-sensitive):

| Channel Name | Identifier       |
|--------------|------------------|
| Juefeng Base | ""               |
| Xiaomi       | xiaomiglobal     |
| Xiaomi Test  | mitest           |
| TapTap       | taptap           |
| RuStore      | RuStore          |
| QooApp       | QooAPP           |
| OneStore     | onestore         |
| LeiDian      | leidianglobal    |
| Honor        | rongyao          |
| Huawei       | huawei           |
| BlueStacks   | nowgg            |
| Aptoide      | Aptoide          |
| AppBazar     | AppBazar         |
| Amazon       | Amazon           |
| Xiao7 Overseas | xiao7             |
| Xsolla      | aikesuola        |
| DMM         | dmm              |
| Samsung     | sanxingglobal    |

#### 4 Obfuscation

The JFSDK package is provided as a jar and is already partially obfuscated. When obfuscating your APK, do not include the jar package in obfuscation, as there are custom UI controls. If obfuscated, exceptions may occur due to missing classes.

## Contact Us

If you have any questions about this program, please contact us!

Email: ouyangjie@juefeng.com