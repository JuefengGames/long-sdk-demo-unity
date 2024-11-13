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


    //�ַ���תunicode �˷������ܲ����������ַ� �������Ŀ��������޸�
    public static string stringToUnicode(string text)
    {
        byte[] unicodeBytes = System.Text.Encoding.UTF8.GetBytes(text);
        StringBuilder unicodeText = new StringBuilder();

        foreach (byte b in unicodeBytes)
        {
            // ����������ַ������ֽ�ת��Ϊʮ�����Ʊ�ʾ�� Unicode ���룬��׷�ӵ� unicodeText ��
            if (b >= 0x80)
            {
                unicodeText.Append("\\u" + ((int)b).ToString("X4"));
            }
            // ������������ַ���ֱ�ӽ��ֽ�ת��Ϊ�ַ�����׷�ӵ� unicodeText ��
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
        //Debug.Log("��Ŀ����·���� " + runpath);
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
            Debug.LogError("SDK��ʼ���쳣��"+ex);
        }

    }
    public void Login()
    {
        try
        {
            if (!isInit)
            {
                Debug.LogError("���ȳ�ʼ��������");
                return;
            }
            JFSDK.getInstance().doLogin();
        }
        catch (Exception ex)
        {
            Debug.LogError("�����¼�쳣��"+ex);
        }

    }
    public void Pay()
    {
        try
        {
            if (token.Equals(""))
            {
                Debug.LogError("���ȵ�¼");
                return;
            }
            string strParam = stringToUnicode(payArgs.text);
            if (strParam.Equals(""))
            {
                Debug.Log("֧������Ϊ�գ�������Ĭ�ϲ���������");
                //return;
                strParam = "11,2024080173827,testuser,2342,testserver,1001,5,1,testgoodsname,testgoodsdes,12231,2024080173827";
                payArgs.text = strParam;
            }
            string[] result = strParam.Split(',');
            if(result.Length < 12)
            {
                Debug.LogError("֧��������������,��������д������");
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
            Debug.Log("���֧���쳣��"+ex);
        }
    }

    //��ʾ������
    public void showFloatView()
    {
        JFSDK.getInstance().showFloatView();
    }
    //�ر�������
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
                Debug.LogError("���ȵ�¼");
                return;
            }
            /*            string strParam = antiAddictionArgs.text;
                        if (strParam.Equals(""))
                        {
                            Debug.Log("��������֤����Ϊjson��ʽ����Ϊ�գ�����");
                            return;
                        }*/
            JFSDK.getInstance().logoutLogin();
        }
        catch
        {
            Debug.LogError("�û��ǳ��쳣");
        }
    }
    long ConvertToUnixTimestamp(string dateString)
    {
        // ���Խ��������ַ���
        if (DateTime.TryParse(dateString, out DateTime dateTime))
        {
            // ������ʱ��ת��Ϊ Universal Time
            DateTimeOffset dateTimeOffset = new DateTimeOffset(dateTime.ToUniversalTime());
            // ��ȡ Unix ʱ���������Ϊ��λ��
            long unixTimestamp = dateTimeOffset.ToUnixTimeSeconds();
            return unixTimestamp;
        }
        else
        {
            Debug.LogError("�޷����������ַ���");
            return -1;
        }
    }
    public void SyncRoleInfo()
    {
        try
        {
            if (token.Equals(""))
            {
                Debug.LogError("���ȵ�¼");
                return;
            }
            string roleInfo = stringToUnicode(syncRoleInfoArgs.text);
            if (roleInfo.Equals(""))
            {
                Debug.Log("��ɫ��Ϣ����Ϊ�գ�������Ĭ�ϲ���������");
                //return;
                roleInfo = "12323,3,testservername,testuser,2342,1002,1001,testpartyname,12,testatttach,2024-08-02,4,383908";
                syncRoleInfoArgs.text = roleInfo;
            }
            string[] result = roleInfo.Split(',');
            if (result.Length < 13)
            {
                Debug.LogError("֧��������������,��������д������");
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
            Debug.LogError("���ɫ��Ϣͬ���쳣��"+ ex);
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
            Debug.LogError("����ر��쳣");
        }
    }

    //��Ϸ���˺��˳���¼
    public void logoutLogin()
    {
        JFSDK.getInstance().logoutLogin();
    }



}

//************************************************************��������Ҫʵ�ֵĻص��ӿ�*************************************************************************************************************************
//callback

public class CallBackListener : JFSDKListener
{
    public override void onCancleExitCallback(string desc)
    {
        Debug.Log("ȡ���˳���Ϸ");
    }

    public override void onCreatedOrderCallback(CreatOrderInfo infoBean)
    {
        Debug.Log("���������ɹ��ˣ�����˶����ţ�" + infoBean.orderId) ;
    }

    public override void onExitCallback(string desc)
    {
        Debug.Log("SDK����ر���Ϸ�����ڴ˷���ʵ��");
        JFSDK_DEMO_UI.isInit = false ;
        JFSDK_DEMO_UI.token = "";
        Application.Quit();
    }

    public override void onGameSwitchAccountCallback()
    {
        Debug.Log("�յ�SDK�л��˻��������ڴ˷��������¼�");
    }

    public override void onInitFaildCallback(string desc)
    {
        try
        {
            Debug.LogError("��ʼ��ʧ���ˣ�"+ desc);
        }
        catch
        {
            Debug.LogError("��ʼ���쳣��");
        }
    }

    public override void onInitSuccessCallback(string desc, bool isAutoLogin)
    {
        try
        {
            JFSDK_DEMO_UI.isInit = true;
            Debug.Log("��ʼ���ɹ���");
        }
        catch
        {
            Debug.LogError("��ʼ���쳣��");
        }
    }

    public override void onLoginFailedCallback(LoginErrorMsg loginErrorMsg)
    {
        try
        {
            Debug.Log("��¼ʧ���ˣ�"+ loginErrorMsg.getErrorMsg());
        }
        catch
        {
            Debug.Log("�����¼ʧ���쳣��");
        }
    }

    public override void onLoginSuccessCallback(LogincallBack logincallBack)
    {
        try
        {
            Debug.Log("��¼�ɹ��ˣ�����");
            Debug.Log("�û�id��" + logincallBack.getJfUserId());
            Debug.Log("��¼token��" + logincallBack.getToken());
            JFSDK_DEMO_UI.token = logincallBack.getToken();
        }
        catch
        {
            Debug.LogError("�����¼�ص��쳣�ˣ�����");
        }
    }

    public override void onLogoutLoginCallback()
    {
        try
        {
            Debug.Log("SDK�����˳���¼�����ڴ˷���ʵ�ַ�����Ϸ��¼���棡����");
            JFSDK_DEMO_UI.isInit = false;
            JFSDK_DEMO_UI.token = "";
        }
        catch
        {
            Debug.Log("����ǳ��쳣��");
        }
    }

    public override void onPayFaild(PayFaildInfo payFaildInfo)
    {
        Debug.LogError("֧��ʧ����,ʧ��ԭ��"+ payFaildInfo.getMsg());
    }

    public override void onPaySuccess(PaySuccessInfo paySuccessInfo)
    {
        Debug.Log("֧���ɹ���");
    }

    public override void onSwitchAccountSuccessCallback(LogincallBack login)
    {
        Debug.Log("�л��˺ųɹ���");
    }
    public override void onSyncSuccessCallback()
    {
        Debug.Log("�ص�ͬ���ɹ���");
    }
}