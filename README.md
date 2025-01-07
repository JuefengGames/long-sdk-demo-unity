Sure, here is the translation of the README file into English:

---

## LANGUAGE

reading in English | 阅读英文文档

## Demo Introduction

This project is created using the Unity development platform and aims to assist Unity developers in integrating the LongSDK Android SDK within Unity code.

## Project Structure

```
Assets
├── Plugins                        // Directory for LongSDK files and SDK integration code, usually does not require modification. Please copy this entire directory into your own project.
|    ├── Android
|    |    ├── JFSDK5.7.2.aar      // LongSDK file located here
|    |    ├── AndroidManifest.xml // The project's manifest file is located here; Unity uses this file to build Android APKs. It is recommended to keep this file consistent with the AndroidManifest.xml in the AAR file.
|    |
|    ├── script                   // Unity SDK integration related code; this code typically does not require modification
|    |    ├── JFSDK.cs
|    |    ├── JFSDKImp.cs
|    |    ├── JFSDKListener.cs
├── JFSDK_DEMO_UI.cs                // Code on how to call SDK interfaces in the demo. Refer to this when calling LongSDK in your own project.
└── ...
```

## Development Environment

Unity Hub Version: 3.3.1-c3  
Unity Editor Version: 2022.3.15f1c1

## Project Debugging

1. Package the Unity project into an APK file.
2. Install the APK file onto an emulator or Android phone, then start it for debugging.

## Integration into Your Project

### 1. Integration Instructions

#### 1.1: Who Should Read This Document

Technical staff of partners and platform developers.

#### 1.2: Precautions

API parameters are case-sensitive.

Both request and response encoding formats are UTF-8.

#### 1.3: Definitions

| Term | Description |
|------|-------------|
| CP   | Game partner |
| APP  | Game integrated with the platform |
| JF_APPID | Game ID assigned by the platform during integration |
| JF_APPKEY | Application key assigned by the platform during integration |

### 2. Copy Configuration Files

#### 2.1: Copy SDK Files

1. Copy the entire Plugins folder from the demo project to the Assets directory of the target project.
2. Add JFSDK.cs, JFSDKImp.cs, and JFSDKListener.cs to your project.

#### 2.2: Configure AndroidManifest.xml

Modify the JF_APPID and JF_APPKEY in both the AAR package's AndroidManifest.xml and the one located alongside it according to the values provided by the Juefeng platform.

### 3. SDK Business Interface Code Explanation

#### 3.1: Implementing and Defining Parameters for SDK Initialization, Login, Registration, Payment, Exit, and Event Listener Interfaces

##### 3.1.1: API Explanation

Most SDK API calls use an event notification method to callback to the game. The game creates a Listener object (by creating a class that inherits JFSDKListener and implementing the callback methods—refer to the demo for guidance), to listen for results of SDK events. Below is the code part (including explanations and parameter definitions for each callback interface).

##### 3.1.2: Code Example

Create a class that inherits JFSDKListener:

```csharp
public class CallBackListener : JFSDKListener
{
    public override void onCancleExitCallback(string desc)
    {
        Debug.Log("Cancel Exit Game");
    }
    ...
}
```

##### 3.1.3: Interface and Parameter Explanation

| Method | Explanation |
|--------|-------------|
| onInitSuccessCallback(string desc, bool isAutoLogin) | Initialization success. Parameters: desc (string "Initialization Successful"), isAutoLogin (for automatic login) |
| onInitFaildCallback(string desc) | Initialization failure. Parameter: desc (string "Initialization Failure Reason") |
| onLoginSuccessCallback(LogincallBack loginCallback) | Login success. Parameter: loginCallback (user information object after successful login) |
| onLoginFailedCallback(LoginErrorMsg errorMsg) | Login failure. Parameter: errorMsg (error code and reason for login failure) |
| onPaySuccess(PaySuccessInfo paySuccessInfo) | Payment success. Parameter: paySuccessInfo (order information after successful payment, needs server confirmation) |
| onPayFaild(PayFaildInfo payFaildInfo) | Payment failure. Parameter: payFaildInfo (information on payment failure) |
| onExitCallback(string desc) | Exit success. Parameter: desc (string "Successful Exit") |
| onCancleExitCallback(string desc) | Cancel exit, continue game. Parameter: desc (string "Cancel Exit") |
| onCreatedOrder(CreateOrderInfo creatOrderInfo) | SDK server has successfully created the order [deprecated, do not use] |
| onLogoutLoginCallback() | Account logout notification (exit game and return to login screen) |
| onSwitchAccountSuccessCallback | This callback is used when there is an account switch feature within the SDK, and it is successful. Clear current role data on game side and reload with new data. |
| onGameSwitchAccountCallback | This is when the game has an account switch feature; call JFSDK.getInstance().switchAccount() on click and perform game-side account switch logic on callback. |
| onSyncSuccess | This callback is when your login is used and sync to the platform's userId is successful. |
| onSyncFailure(string msg) | This callback indicates a sync failure when using your login. Please do not proceed and treat it as a login failure. |

#### 3.2: SDK Initialization

##### 3.2.1: Code Example

In the main class, create an object:

```csharp
using jfsdk;
private static CallBackListener JFListener;
```

Call the initialization method:

```csharp
JFListener = new CallBackListener();
JFSDK.getInstance().init(JFListener);
```

##### 3.2.2: Parameter Explanation

1. Initialize SDK resources and register SDK event listeners.

2. `CallBackListener` refers to the class created in 3.1.2.

##### 3.2.3: Callback Explanation

Initialization success or failure will trigger callbacks: `onInitSuccessCallback` and `onInitFaildCallback` in section 3.1, allowing CP to manage the process accordingly.

#### 3.3: Login

To adapt to downstream channels and obtain more user flow, by default, the original game must include both "its own login" and "Juefeng login" in the bundle. Specific channel distribution will be carried out by us to ensure compliant login methods for each channel. (Both methods will not appear in the same channel bundle.)

##### 3.3.1 Determine Whether to Use Juefeng Login (Recommended Integration)

Call the method:

```csharp
LoginType loginType = JFSDK.getInstance().getLoginType();

public enum LoginType
{
    JuefengLogin,  // Juefeng Login
    GameLogin      // Developer's Own Login
}

/**
* The following configuration should be applied in the AndroidManifest.xml file for mother package switching testing. The value will affect the getLoginType() method's return value;
* This logic is only for use in mother package testing. Subsequent channel packages will replace it as per Juefeng's control.
*
* value=1: JUEFENG_LOGIN
* value=0: GAME_LOGIN
*/

<meta-data android:name="JF_LOGIN_TYPE" android:value="1"/>
```

##### 3.3.2 Juefeng Login

##### 3.3.2.1: Launch Juefeng Login

Ensure method calls occur in the UI thread.

Call:

```csharp
JFSDK.getInstance().doLogin();
```

##### 3.3.2.2: Login Callback Explanation

Login or registration failures trigger callbacks: `onLoginSuccessCallback` and `onLoginFailedCallback` as per section 3.1, for CP to manage process accordingly.

##### 3.3.2.3: Return Parameter Explanation:

Successful login or registration triggers the `onLoginSuccessCallback` method, returning these parameters:

| Parameter | Type  | Description |
|-----------|-------|-------------|
| userId    | string| Unique user ID upon successful login |
| token     | string| Unique token granted to user for this login session |
| userName  | string| Username |
| isAuthenticated | boolean| Whether user is authenticated, true/false |
| pi        | string | (Anti-addiction reserve field) |
| age       | int    | Age |

Failed login triggers the `onLoginFaild` method, which uses these parameters:

| Parameter | Type   | Description |
|-----------|--------|-------------|
| code      | string | Error code upon login failure |
| errorMsg  | string | Message indicating reason for login failure |

##### 3.3.3 Use Developer's Own Login

Invoke as follows:

```csharp
JFSDK.getInstance().syncUserId(String userId, String token);
```

If using your own account system (i.e., integrated login/registration), use this method to access post-login unique user identifier userId and token credentials sent through this method. Post-integration, the `onSyncSuccess` callback will trigger.

Note: To ensure user safety and legality, it's suggested developers provide a back-end verification interface as required by us. Parameters userId and token will be used for verification. Refer to the server documentation.

#### 3.4: Payment

##### 3.4.1 Payment Interface

```csharp
JFSDK.getInstance().showPay(jfOrderInfo);
```

Parameter Explanation
| Field    | Description |
|----------|-------------|
| level    | Player Level |
| goodsId  | Product ID; defaults to “1” if absent |
| goodsName| Product Name (String); can't be null/empty |
| goodsDes | Product Description (String); can't be null/empty and max 64 chars |
| price    | Product price, required (non-null/empty) in USD by default (communicate if otherwise) and supports two decimals |
| serverId | Server ID, must match role sync info |
| serverName| Server Name (required), must match role sync info |
| roleId   | In-game unique role identifier (required, not null/empty), must match role sync info |
| roleName | Role Name (required), must match role sync info |
| vip      | User VIP Level; default “1” if absent |
| remark   | Extra field; usually order number (cpOrderId), max 64 chars |
| cpOrderId| Developer's order number; must be unique |

Note: All fields must not be null; if absent, use default "1"

##### 3.4.2: Payment Callback

Payment involves callbacks for three events: order creation success, payment success, and payment failure. Each event triggers callbacks described in section 3.1:

-onCreatedOrderCallback, onPaySuccess, onPayFaild

##### 3.4.3: Callback Return Object Field Explanation

`onPaySuccess`, `onCreatedOrderCallback`, `onPayFaild` return the following:

```csharp
PaySuccessInfo
{
    public String orderId;    // Unique order ID generated by server
    public String gameRole;   // Role ID provided by game
    public String gameArea;   // Game server info provided
    public String productName;// Product name provided
    public String productDesc;// Product description provided
    public String remark;     // Custom parameter provided
    public String cpOrderId;  // cp's order number provided
}

CreateOrderInfo
{
    public String orderId;    // SDK server-generated unique order ID
    public String gameRole;   // Role ID provided by game
    public String gameArea;   // Game server info provided
    public String productName;// Product name provided
    public String productDesc;// Product description provided
    public String cpOrderId;  // cp's order number provided
    public String remark;     // Custom parameter provided
}

PayFaildInfo
{
    private String code;  // Error code
    private String msg;   // Failure message
}
```

#### 3.5: Platform Floating Button, User Center (Automatically Handled by SDK)

##### 3.5.1: Feature Description

Post-login, you can access personal center via floating button 

 View platform account information 

 Account password protection settings 

 Account phone binding 

 Password modification 

 Payment password management, view charge and expense records 

##### 3.5.2: API Calls

1. Show Floating Button  

```csharp
JFSDK.getInstance().showFloatView();
```

2. Hide Floating Button

```csharp
JFSDK.getInstance().removeFloatView(); //Hide floating view
```

#### 3.6: Game Data Synchronization (Compulsory Call)

##### 3.6.1: Explanation

When logging into the game for the first time or upon role information change, the SDK needs to sync role info, categorized as four types:

| Type      | Type Value | Call Method |
|-----------|------------|-------------|
| Role Creation | 1        | JFSDK.getInstance().syncInfo(roleInfo); |
| Role Login   | 2        | JFSDK.getInstance().syncInfo(roleInfo); |
| Role Upgrade | 3        | JFSDK.getInstance().syncInfo(roleInfo); |
| Role Exit    | 4        | JFSDK.getInstance().syncInfo(roleInfo); |

Use the code in section 3.6.2, each type uses a different type value.

##### 3.6.2: Call Example (Role Creation)

| Field       | Description |
|-------------|-------------|
| serverName  | Server name (required) |
| serverId    | Server ID (required) |
| roleName    | Role Name (required) |
| roleId      | Role ID (required) |
| partyId     | Guild ID (transmit if possible) |
| partyName   | Guild name (transmit if possible) |
| gameRoleLevel| Role level (required) |
| attach      | Extra field |
| type        | 1: create, 2: login, 3: upgrade, 4: exit (required) |
| experience  | Current experience value (transmit if possible) |
| roleCreateTime | Required if type = 1; Role creation datetime (required) |
| vipLevel    | VIP Level (int) (transmit if possible) |
| gameRolePower| Battle Power (int) (transmit if possible) |

Note: All fields must not be null; if absent, use default "1"

Call as follows:

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

##### 3.8.1: API Usage

Invoke JFSDK.getInstance().exitLogin();

##### 3.8.2: Callback Explanation

Upon clicking the back button, call JFSDK.getInstance().exitLogin(); 

Upon selecting exit, callbacks execute in script `(3.1)SdkEventListener`'s `onExitCallback()` method—handle game screen exit here.

##### 3.8.3: In-Game Exit

For cases like forced logout or proactive account logout, call:

JFSDK.getInstance().logoutLogin();

This will trigger `onLogoutLogin` (see callback interfaces in section 3.1). Exit game to login screen and implement relogic logic post-login.

#### 3.9: Logout (Compulsory to Implement)

Clicking 'Exit current account' in personal center triggers `(3.1)`'s `onLogoutLoginCallback` callback. Implement logic for returning to game login screen here.

#### 3.10: SDK Lifecycle (Automatically Handled by SDK; integrate within AndroidManifest.xml file)

```xml
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

#### Manual Lifecycle Callbacks

```csharp
public void onCreate(AndroidJavaObject act); // Must be called before SDK init

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

#### 3.11: Application Addition (Compulsory)
If there is no custom Application in your app, integrate it in the AndroidManifest.xml:

```xml
<application  android:name="com.juefeng.sdk.juefengsdk.JfApplication" />
```

If your app does have its own Application, make sure it either extends or invokes `com.juefeng.sdk.juefengsdk.JfApplication`. 

The SDK's internal initialization method already calls this. If issues arise, you might implement it yourself.

#### 3.12: In-Game Account Switch

For account switch button inside the game, invoke the following interface upon button click:

```csharp
JFSDK.getInstance().switchAccount();
```

This will trigger the `onGameSwitchAccountCallback` method in Listener (refer to section 3.1.3) for relevant processing.

#### 3.13: Get Downstream Channel Identifier

```csharp
String channelType = JFSDK.getInstance().getChannelType();
```

Purpose: Integrate this API to distinguish and compile channel data by dimensions like child channel ID, supporting market operations planning.

Current Overseas Channel Codes (case-sensitive):

| Channel Name | Code       |
|--------------|------------|
| Juefeng Main | jfgame     |
| Xiaomi Global| xiaomiglobal |
| Xiaomi Test  | mitest    |
| TapTap       | taptap    |
| RuStore      | RuStore   |
| QooApp       | QooAPP    |
| OneStore     | onestore  |
| LD Emulator  | leidianglobal |
| Honor Mobile | rongyao   |
| Huawei Mobile| huawei    |
| Android Emulator | nowgg |
| Aptoide      | Aptoide   |
| Cafe Bazaar  | AppBazar  |
| Amazon       | Amazon    |
| Overseas Xiao7| xiao7    |
| xsolla       | aikesuola |
| DMM          | dmm       |
| Samsung Mobile | sanxingglobal |

#### 3.13: Retrieving Product Information [Optional]

After establishing a connection with JFSDK, you can query and display available products to the user. Querying product details before display is crucial, as it provides locality-specific product data.

To query in-app product details, call `queryProductDetailsAsync`. Supports individual and batch queries; when final list is empty, returns all product info.

Sample code:

```csharp
// Query product details
List<String> finalList = new List<String>();
// Adding no ID returns all product info, adding specific ID returns that info
finalList.Add("charge_2.99");
PdListener = new CallBackPdListener();
JFSDK.getInstance().queryProductDetailsAsync(finalList, PdListener);
```

Product information fields' access methods are elaborated in demo code:

```csharp
"currency": "USD",// Currency unit
"describe": "You can get the goods after purchase.",// Product description
"currencySymbol": "₽",// Currency unit symbol
"price": "0.99",// Price
"sku": "charge_0.99",// Product ID
"title": "Pack I"// Product name
```

#### 4: Best Practices

##### 4.1: Obfuscation

JFSDK is provided as a partially obfuscated JAR package. When obfuscating your APK, do not obfuscate this JAR, since it contains custom UI controls—obfuscating it may lead to class search exceptions.

##### 4.2: APK Package Name

If convenient, please use ".jf" as a suffix for easier package identification, and allow Juefeng to modify the game package name. This might be necessary when distributing packages, as it helps satisfy channel requirements.

Should your server-side conduct package name verification, Juefeng can compile a whitelist in advance containing the necessary package names.

The primary reason here is certain channels have specific package name requirements, e.g., Huawei, Samsung.

##### 4.3: Mother Package Testing [Important]

To assess SDK integration status and streamline integration, complete mother package testing post-integration.

Self-test: Click Floating Window --> Personal Center --> Lifecycle Test to view unintegrated interfaces. Complete necessary integration steps or communicate with our developers if unable to do so independently.

##### 4.4: In-Game Product Price Unit

During SDK integration, the currency unit visible in the mother package is RMB. Rest assured, upon distribution, channels display designated local currency.

## Contact Us

If you have any questions about this program, please contact us!

Email: ouyangjie@juefeng.com