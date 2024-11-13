using UnityEngine;
using System;
using UnityEngine.UI;
using System.Text;
using jfsdk;
using System.Collections.Concurrent;

public class JFSDK_DEMO_UI : MonoBehaviour
{
    public GameObject console_window;
    public Text logList;
    public InputField initArgs;
    public InputField loginArgs;
    public InputField payArgs;
    public InputField antiAddictionArgs;
    public InputField syncRoleInfoArgs;
    public InputField closeArgs;

    public static string token = "";
    public static string userId = "";
    public static bool isInit = false;

    private ConcurrentQueue<string> logQueue = new ConcurrentQueue<string>();
    private object lockObject = new object();

    private static CallBackListener JFListener;


    //字符串转unicode 此方法可能不适用所有字符 请根据项目自身情况修改
    public static string stringToUnicode(string text)
    {
        byte[] unicodeBytes = System.Text.Encoding.UTF8.GetBytes(text);
        StringBuilder unicodeText = new StringBuilder();

        foreach (byte b in unicodeBytes)
        {
            // 如果是中文字符，则将字节转换为十六进制表示的 Unicode 编码，并追加到 unicodeText 中
            if (b >= 0x80)
            {
                unicodeText.Append("\\u" + ((int)b).ToString("X4"));
            }
            // 如果不是中文字符，直接将字节转换为字符，并追加到 unicodeText 中
            else
            {
                unicodeText.Append((char)b);
            }
        }
        return unicodeText.ToString();
    }
    // Start is called before the first frame update
    void Start()
    {
        //string runpath = System.Environment.CurrentDirectory;
        //Debug.Log("项目运行路径： " + runpath);
        Init();
    }
    void Awake()
    {
        Application.logMessageReceivedThreaded += HandleLog;
    }
    // Update is called once per frame
    void Update()
    {
        while (logQueue.TryDequeue(out string message))
        {
            logList.text += message + "\n";
        }
    }
    void HandleLog(string condition, string stackTrace, LogType type)
    {
        string message = "==================================\n";
        if (type == LogType.Log)
        {
            message = condition;
        }
        else
        {
            message = "<color=red>" + condition + "</color>";
        }

        lock (lockObject)
        {
            logQueue.Enqueue(message);
        }
    }
    public void console_window_btn()
    {
        console_window.SetActive(!console_window.activeSelf);
    }

    public void Init() {
        try
        {
            JFListener = new CallBackListener();
            JFSDK.getInstance().init(JFListener);
        }
        catch (Exception ex)
        {
            Debug.LogError("SDK初始化异常："+ex);
        }

    }
    public void Login()
    {
        try
        {
            if (!isInit)
            {
                Debug.LogError("请先初始化！！！");
                return;
            }
            JFSDK.getInstance().doLogin();
        }
        catch (Exception ex)
        {
            Debug.LogError("点击登录异常："+ex);
        }

    }
    public void Pay()
    {
        try
        {
            if (token.Equals(""))
            {
                Debug.LogError("请先登录");
                return;
            }
            string strParam = stringToUnicode(payArgs.text);
            if (strParam.Equals(""))
            {
                Debug.Log("支付参数为空，将采用默认参数！！！");
                //return;
                strParam = "11,2024080173827,testuser,2342,testserver,1001,5,1,testgoodsname,testgoodsdes,12231,2024080173827";
                payArgs.text = strParam;
            }
            string[] result = strParam.Split(',');
            if(result.Length < 12)
            {
                Debug.LogError("支付参数个数不对,请重新填写！！！");
                return;
            }
            JfOrderInfo jfOrderInfo = new JfOrderInfo();
            jfOrderInfo.setLevel(result[0]);
            jfOrderInfo.setCpOrderId(result[1]);
            jfOrderInfo.setRoleName(result[2]);
            jfOrderInfo.setRoleId(result[3]);
            jfOrderInfo.setServerName(result[4]);
            jfOrderInfo.setServerId(result[5]);
            jfOrderInfo.setVip(result[6]);
            jfOrderInfo.setPrice(result[7]);
            jfOrderInfo.setGoodsName(result[8]);
            jfOrderInfo.setGoodsDes(result[9]);
            jfOrderInfo.setGoodsId(result[10]);
            jfOrderInfo.setRemark(result[11]);
            JFSDK.getInstance().showPay(jfOrderInfo);
        }
        catch (Exception ex)
        {
            Debug.Log("点击支付异常："+ex);
        }
    }

    //显示悬浮球
    public void showFloatView()
    {
        JFSDK.getInstance().showFloatView();
    }
    //关闭悬浮球
    public void removeFloatView()
    {
        JFSDK.getInstance().removeFloatView();
    }

    public void accountLogoutLogin()
    {
        try
        {
            if (token.Equals(""))
            {
                Debug.LogError("请先登录");
                return;
            }
            /*            string strParam = antiAddictionArgs.text;
                        if (strParam.Equals(""))
                        {
                            Debug.Log("防沉迷验证参数为json格式不能为空！！！");
                            return;
                        }*/
            JFSDK.getInstance().logoutLogin();
        }
        catch
        {
            Debug.LogError("用户登出异常");
        }
    }
    long ConvertToUnixTimestamp(string dateString)
    {
        // 尝试解析日期字符串
        if (DateTime.TryParse(dateString, out DateTime dateTime))
        {
            // 将日期时间转换为 Universal Time
            DateTimeOffset dateTimeOffset = new DateTimeOffset(dateTime.ToUniversalTime());
            // 获取 Unix 时间戳（以秒为单位）
            long unixTimestamp = dateTimeOffset.ToUnixTimeSeconds();
            return unixTimestamp;
        }
        else
        {
            Debug.LogError("无法解析日期字符串");
            return -1;
        }
    }
    public void SyncRoleInfo()
    {
        try
        {
            if (token.Equals(""))
            {
                Debug.LogError("请先登录");
                return;
            }
            string roleInfo = stringToUnicode(syncRoleInfoArgs.text);
            if (roleInfo.Equals(""))
            {
                Debug.Log("角色信息参数为空，将采用默认参数！！！");
                //return;
                roleInfo = "12323,3,testservername,testuser,2342,1002,1001,testpartyname,12,testatttach,2024-08-02,4,383908";
                syncRoleInfoArgs.text = roleInfo;
            }
            string[] result = roleInfo.Split(',');
            if (result.Length < 13)
            {
                Debug.LogError("支付参数个数不对,请重新填写！！！");
                return;
            }
            JfRoleInfo jfRoleInfo = new JfRoleInfo();
            jfRoleInfo.setExperience(result[0]);
            jfRoleInfo.setType(result[1]);
            jfRoleInfo.setServerName(result[2]);
            jfRoleInfo.setRoleName(result[3]);
            jfRoleInfo.setRoleId(result[4]);
            jfRoleInfo.setServerId(result[5]);
            jfRoleInfo.setPartyId(result[6]);
            jfRoleInfo.setPartyName(result[7]);
            jfRoleInfo.setGameRoleLevel(result[8]);
            jfRoleInfo.setAttach(result[9]);
            jfRoleInfo.setRoleCreateTime(ConvertToUnixTimestamp(result[10]));
            jfRoleInfo.setVipLevel(int.Parse(result[11]));
            jfRoleInfo.setGameRolePower(int.Parse(result[12]));
            JFSDK.getInstance().syncInfo(jfRoleInfo);
        }
        catch (Exception ex)
        {
            Debug.LogError("点角色信息同步异常："+ ex);
        }
    }
    public void Close()
    {
        try
        {
            JFSDK.getInstance().exitLogin();
        }
        catch
        {
            Debug.LogError("点击关闭异常");
        }
    }

    //游戏内账号退出登录
    public void logoutLogin()
    {
        JFSDK.getInstance().logoutLogin();
    }



}

//************************************************************以下是需要实现的回调接口*************************************************************************************************************************
//callback

public class CallBackListener : JFSDKListener
{
    public override void onCancleExitCallback(string desc)
    {
        Debug.Log("取消退出游戏");
    }

    public override void onCreatedOrderCallback(CreatOrderInfo infoBean)
    {
        Debug.Log("订单创建成功了，服务端订单号：" + infoBean.orderId) ;
    }

    public override void onExitCallback(string desc)
    {
        Debug.Log("SDK请求关闭游戏，请在此方法实现");
        JFSDK_DEMO_UI.isInit = false ;
        JFSDK_DEMO_UI.token = "";
        Application.Quit();
    }

    public override void onGameSwitchAccountCallback()
    {
        Debug.Log("收到SDK切换账户请求，请在此方法处理事件");
    }

    public override void onInitFaildCallback(string desc)
    {
        try
        {
            Debug.LogError("初始化失败了："+ desc);
        }
        catch
        {
            Debug.LogError("初始化异常了");
        }
    }

    public override void onInitSuccessCallback(string desc, bool isAutoLogin)
    {
        try
        {
            JFSDK_DEMO_UI.isInit = true;
            Debug.Log("初始化成功了");
        }
        catch
        {
            Debug.LogError("初始化异常了");
        }
    }

    public override void onLoginFailedCallback(LoginErrorMsg loginErrorMsg)
    {
        try
        {
            Debug.Log("登录失败了："+ loginErrorMsg.getErrorMsg());
        }
        catch
        {
            Debug.Log("处理登录失败异常了");
        }
    }

    public override void onLoginSuccessCallback(LogincallBack logincallBack)
    {
        try
        {
            Debug.Log("登录成功了！！！");
            Debug.Log("用户id：" + logincallBack.getJfUserId());
            Debug.Log("登录token：" + logincallBack.getToken());
            JFSDK_DEMO_UI.token = logincallBack.getToken();
        }
        catch
        {
            Debug.LogError("处理登录回调异常了！！！");
        }
    }

    public override void onLogoutLoginCallback()
    {
        try
        {
            Debug.Log("SDK请求退出登录，需在此方法实现返回游戏登录界面！！！");
            JFSDK_DEMO_UI.isInit = false;
            JFSDK_DEMO_UI.token = "";
        }
        catch
        {
            Debug.Log("处理登出异常了");
        }
    }

    public override void onPayFaild(PayFaildInfo payFaildInfo)
    {
        Debug.LogError("支付失败了,失败原因："+ payFaildInfo.getMsg());
    }

    public override void onPaySuccess(PaySuccessInfo paySuccessInfo)
    {
        Debug.Log("支付成功了");
    }

    public override void onSwitchAccountSuccessCallback(LogincallBack login)
    {
        Debug.Log("切换账号成功了");
    }
    public override void onSyncSuccessCallback()
    {
        Debug.Log("回调同步成功了");
    }
}