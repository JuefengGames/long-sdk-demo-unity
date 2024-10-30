## LANGUAGE

[reading in chinese | 阅读中文文档](https://github.com/JuefengGames/long-sdk-demo-unity/blob/main/README_ZH_CN.md)  

## Demo Introduction

This project, generated using the Unity development platform, primarily aims to assist Unity developers in integrating LongSDK (Android) encapsulated with Unity SDK into their Unity code.

## Project Structure

```
Assets
├── Plugins                                   // LongSDK files and SDK integration code directory, usually do not need modification. Please copy this directory to your own project during development.
|          ├──Android
|          |         ├──JFSDK5.6.0.aar     // LongSDK file is placed here.
|          |         ├──AndroidManifest.xml   //The project's manifest file is placed here. When Unity builds the Android APK file, it will use this manifest file for packaging. It's best to ensure that the content of the AndroidManifest.xml file in the AAR package remains consistent with the file in this location.
|          |          
|          ├──script                         // Unity encapsulated LongSDK (Android) related code, usually do not need modification.
|          |         ├──JFSDK.cs
|          |         ├──JFSDKImp.cs
|          |         ├──JFSDKListener.cs
├── JFSDK_DEMO_UI.cs                          // SDK interface invocation code in the demo project. Refer to this code when invoking LongSDK in your own project.
└── ...
```

## Development Environment

Unity Hub Version：3.3.1-c3  
Unity Editor Version：2022.3.15f1c1

## Project Debugging

1. Package the Unity project into an APK file.
2. Install the APK file on an emulator or an Android device and start the debugging process.

## Integration Instructions

### 1. Integration Guide

#### 1.1: Intended Audience

Technical personnel of integration partners, product technical staff, and platform technical staff.

#### 1.2: Points to Note

- Interface parameters are case-sensitive.
- The encoding format for request and response is UTF-8.

#### 1.3: Terminology Definitions

| Term              | Description |
|-------------------|-------------|
| CP                | Game Partner |
| APP               | Game integrated into the platform |
| JF_APPID         | Game ID assigned by the platform during integration. |
| JF_APPKEY        | Game/application key assigned by the platform during integration. |

### 2. Copy Configuration File

#### 2.1: Copy SDK Files

1. Copy the entire Plugins folder from the demo project's Assets directory to the target project's Assets directory.
2. Add JFSDK.cs, JFSDKImp.cs, and JFSDKListener.cs to your project.

#### 2.2: Configuration of AndroidManifest.xml

Modify the AndroidManifest.xml within the aar package to set JF_APPID and JF_APPKEY to the values provided by the JF platform.

### 3. SDK Business Interface Code Explanation

#### 3.1: Implementation and Parameter Explanation of SDK Initialization, Login, Registration, Payment, and Exit Event Listener Interfaces

##### 3.1.1: API Description

Most of the SDK API calls use the event notification method to call back to the game. Create a Listener object in the code (generate a class that inherits the JFSDKListener class and implements callback methods, refer to the demo). This monitors each event result of the SDK. Below is the code section, including explanations of each callback interface and parameter descriptions.

##### 3.1.2: Code Section

Generate a class and inherit the JFSDKListener class:
```
public class CallBackListener : JFSDKListener
{
    public override void onCancleExitCallback(string desc)
    {
        Debug.Log("Cancel exit game");
    }
    ...
}
```

##### 3.1.3: Interface and Parameter Description

| Method | Description |
|-------------|-------------|
|onInitSuccessCallback(String desc, boolean isAutoLogin)|Initialization successful; parameter description: desc (string "Initialization successful"), isAutoLogin (boolean indicating if auto-login can be called)|
|onInitFaildCallback(String desc)|Initialization failed; parameter description: desc (string "Reason for initialization failure")|
|onLoginSuccessCallback(LogincallBack logincallBack)|Login successful; parameter description: logincallBack (user information object returned after successful login)|
|onLoginFailedCallback(LoginErrorMsgerrorMsg)|Login failed; parameter description: errorMsg (error code and reason for failure returned after login failure)|
|onPaySuccess (PaySuccessInfopaySuccessInfo)|Payment successful; parameter description: paySuccessInfo (order information returned after successful payment, including order number, further confirmation with the game server is required to confirm payment success)|
|onPayFaild(PayFaildInfopayFaildInfo)|Payment failed; parameter description: payFaildInfo (information returned after payment failure)|
|onExitCallback(String desc)|Exit successful; parameter description: desc (string "Successfully exited")|
|onCancleExitCallback(String desc)|Cancel exit, choose to continue the game; parameter description: desc (string "Cancelled exit")|
|onCreatedOrderCallback(CreateOrderInfocreatOrderInfo)|SDK server has successfully created this order; creatOrderInfo contains order information|
|onLogoutLoginCallback()|Logout and login (need to exit the game and return to the login page in this callback)|
|onSwitchAccountSuccessCallback|This callback is invoked when there is a switch account function within the SDK and the switch is successful. The game needs to unload the original role data and reload the role data based on the new (parameters can be obtained from login).|
|onGameSwitchAccountCallback|This callback is invoked when the switch account function within the game is clicked (call JFSDK.getInstance().switchAccount(MainActivity.this)). The game's account switch logic needs to be implemented within this callback.|

#### 3.2: SDK Initialization

##### 3.2.1: Code Section

In the main class, generate the object:
```
using jfsdk;
private static CallBackListener JFListener;
```

Call the initialization method:
```
JFListener = new CallBackListener();
JFSDK.getInstance().init(JFListener);
```

##### 3.2.2: Parameter Description

1. Initialize SDK resources and register SDK event listeners.
2. CallBackListener is the class generated in 3.1.2.

##### 3.2.3: Callback Description

The success and failure of initialization will callback to the onInitSuccessCallback and onInitFaildCallback in 3.1. The CP can handle accordingly based on the callback results.

#### 3.3: Login

##### 3.3.1: Login Trigger Description

The method needs to be called in the UI thread.

Code call:
```
JFSDK.getInstance().doLogin();
```

##### 3.3.2: Login Callback Description

The success and failure of login and registration will callback to the onLoginSuccessCallback and onLoginFailedCallback in 3.1. The CP can handle accordingly based on the callback results.

##### 3.3.3: Return Parameter Description

Login or registration success callback method: onLoginSuccessCallback

Callback Parameter Description:

| Parameter Name | Type | Parameter Description |
|-------------|-------------|-------------|
|userId|string|User ID after successful login (unique)|
|token|string|Unique token assigned to the user by the platform during this login (unique)|
|userName|string|User name|
|isAuthenticated|boolean|Whether the user is authenticated, true (yes), false (no)|
|pi|String|(Reserved field for addiction prevention)|
|age|int|Age|

Login failure callback method: onLoginFaild

This method is called when a business logic error occurs.

Callback Parameter Description:

| Parameter Name | Type | Parameter Description |
|-------------|-------------|-------------|
|code|string|Login failure error code|
|errorMsg|string|Login failure message prompt|

#### 3.4: Payment

##### 3.4.1 Payment Interface

```
JFSDK.getInstance().showPay(jfOrderInfo);
```

Parameter Description

| Field | Description |
|-------------|-------------|
|level | Character level |
|goodsId | Product ID (product code), if not available, pass "1" |
|goodsName | Product name (string), cannot be null or empty |
|goodsDes | Product description (string), cannot be null or empty |
|price | Amount (int, string type), cannot be null or empty (unit yuan) |
|serverId | Server ID, cannot be null or empty |
|serverName | Server name (required), cannot be null or empty |
|roleId | Unique identifier for the character in the game (required), cannot be null or empty |
|roleName | Character name (required) |
|vip | User VIP level, if not available, pass "1" |
|remark | Pass-through field (if no special case, please pass the order number (cpOrderId)) |
|cpOrderId | CP generated order number (required) |

##### 3.4.2: Payment Callback Description

Payment involves three callbacks: creating an order successfully, payment success, and payment failure. They will respectively callback to the onCreatedOrderCallback, onPaySuccess, and onPayFaild in 3.1.

##### 3.4.3: Callback Return Object Field Explanation

onPaySuccess, onCreatedOrderCallback, onPayFaild

```
PaySuccessInfo{
public String orderId;  // Unique order number generated by the server

public String gameRole; // Game role ID passed by the game

public String gameArea;  // Server information passed by the game

public String productName;  // Product name passed by the game

public String productDesc;  // Product description passed by the game

public String remark;  // Custom parameters passed by the game

public String cpOrderId;  // Order number passed by the CP
}

CreateOrderInfo{
public String orderId;  // Unique order number generated by the SDK server

public String gameRole; // Game role ID passed by the game

public String gameArea;  // Server information passed by the game

public String productName;  // Product name passed by the game

public String productDesc;  // Product description passed by the game

public String cpOrderId;  // Order number passed by the CP

public String remark;  // Custom parameters passed by the game
}

PayFaildInfo{
private String code; // Error code

private String msg;  // Failure description
}
```

#### 3.5: Platform Floating Point and User Center (must be called)

##### 3.5.1: Function Description

After logging in, users can access the personal center through the floating button to:

- View platform account information
- Set account password protection
- Bind account to phone
- Change account password
- Manage payment passwords and view recharge and consumption records

##### 3.5.2: API Call

1. Display Floating Point

Code:
```
JFSDK.getInstance().showFloatView();
```

2. Hide Floating Point
Code:
```
JFSDK.getInstance().removeFloatView();// Hide floating window
```
##### 3.5.3: API Call Position

1. Display Floating Point

Call within the login success callback

#### 3.6: Game Data Synchronization (must be called)

##### 3.6.1: Explanation

During the first login or when role information changes, the SDK needs to synchronize role information. There are four types of role information:

| Type | Type Value | Call Method |
|-------------|-------------|-------------|
|Role Creation|1|JFSDK.getInstance().syncInfo(roleInfo);|
|Role Login|2|JFSDK.getInstance().syncInfo(roleInfo);|
|Role Upgrade|3|JFSDK.getInstance().syncInfo(roleInfo);|
|Role Exit|4|JFSDK.getInstance().syncInfo(roleInfo);|

The code is as shown in 3.6.2. Different types pass different type values.

##### 3.6.2: Call Example (Role Creation)

| Field | Description |
|-------------|-------------|
|serverName | Server name (required) |
|serverId | Server ID (required) |
|roleName | Role name (required) |
|roleId | Role ID (required) |
|partyId | Guild ID (if available, pass) |
|partyName | Guild name (if available, pass) |
|gameRoleLevel | Role level (required) |
|attach | Extra field |
|type | 1: Creation, 2: Login, 3: Upgrade, 4: Exit (required) |
|experience | Current experience value (if available, pass) |
|roleCreateTime | Role creation time long (pass when type is 1) |
|vipLevel | VIP level int (if available, pass) |
|gameRolePower | Combat power int (if available, pass) |

Call code:
```
JfRoleInfo roleInfo = new JfRoleInfo();

roleInfo.setGameRoleLevel("3");

roleInfo.setRoleId("Role ID");

roleInfo.setGameRolePower(55555);

roleInfo.setServerId("Server ID");

roleInfo.setServerName("Server Name");

roleInfo.setRoleName("Liu Hongzhen");

roleInfo.setExperience("135355446");

roleInfo.setPartyId("Guild ID");

roleInfo.setPartyName("Guild Name");

roleInfo.setRoleCreateTime(System.currentTimeMillis());

roleInfo.setVipLevel(5);

roleInfo.setType("1");

JFSDK.getInstance().syncInfo(roleInfo);
```

#### 3.8: Game Exit

##### 3.8.1: API Usage

Call JFSDK.getInstance().exitLogin();

##### 3.8.2: Callback Description

When the back button is pressed, call JFSDK.getInstance().exitLogin();

Choosing to exit will callback to the onExitCallback() method in SdkEventListener (3.1). The game interface closure should be executed inside this method.

##### 3.8.3: In-Game Exit

When kicked off or voluntarily logging out in the game, call:

JFSDK.getInstance().logoutLogin();

#### 3.9: Logout Login (must implement)

Clicking the exit current account button in the personal center will callback to the onLogoutLoginCallback interface in 3.1. The code for returning to the game login page should be implemented inside this interface.

#### 3.10: SDK Lifecycle Interface (must implement)

```
public void onCreate(AndroidJavaObject act); (Must be called before SDK's init call)

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

public void onRequestPermissionsResult(AndroidJavaObject act, int requestCode, String[] permissions, int[] grantResults);
```

#### 3.11: Application Addition (must integrate)

If the app itself does not have a custom Application, please integrate in the AndroidManifest.xml.

If the app itself has a custom Application, please inherit or call com.juefeng.sdk.juefengsdk.JfApplication.

This SDK’s own initialization method contains the call, if issues arise, it can be removed and implemented manually.

#### 3.12: Account Switch in Game

When there is an account switch button in the game, you need to call:

JFSDK.getInstance().switchAccount();

The interface will callback to the onGameSwitchAccountCallback interface in the Listener (see 3.1.3) for related handling.

#### 4: Obfuscation

The JFSDK package is provided to users as a jar, which is already partially obfuscated. When obfuscating your own APK package, please do not obfuscate the jar package together, as it may contain custom UI controls that can cause exceptions if the relevant classes are not found after obfuscation.

## Contact Us

If you have any questions about this program, please contact us!

Email: [ouyangjie@juefeng.com](mailto:ouyangjie@juefeng.com)