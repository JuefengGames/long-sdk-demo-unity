using UnityEngine;
using System;

namespace jfsdk
{
    public class JFSDK
    {
        private static JFSDK _instance;

        public static JFSDK getInstance()
        {
            if (null == _instance)
            {
                _instance = new JFSDK();
            }
            return _instance;
        }

        public void init(JFSDKListener listener)
        {
            JFSDKImp.getInstance().init(listener);
        }

        public void doLogin()
        {
            JFSDKImp.getInstance().doLogin();
        }

        public void showPay(JfOrderInfo orderInfo)
        {
            JFSDKImp.getInstance().showPay(orderInfo);
        }

        public void exitLogin()
        {
            JFSDKImp.getInstance().exitLogin();
        }

        public void showFloatView()
        {
            JFSDKImp.getInstance().showFloatView();
        }
        public void removeFloatView()
        {
            JFSDKImp.getInstance().removeFloatView();
        }
        public void logoutLogin()
        {
            JFSDKImp.getInstance().logoutLogin();
        }
        public void syncInfo(JfRoleInfo roleInfo)
        {
            JFSDKImp.getInstance().syncInfo(roleInfo);
        }
        public void OnCreate(AndroidJavaObject act)
        {
            JFSDKImp.getInstance().onCreate(act);
        }
        public void SwitchAccount()
        {
            JFSDKImp.getInstance().switchAccount();
        }
        public void syncUserId(string userId, string token)
        {
            JFSDKImp.getInstance().syncUserId(userId, token);
        }
        public string getChannelType()
        {
            return JFSDKImp.getInstance().getChannelType();
        }
        public void OnResume(AndroidJavaObject act)
        {
            JFSDKImp.getInstance().onResume(act);
        }

        public void OnPause(AndroidJavaObject act)
        {
            JFSDKImp.getInstance().onPause(act);
        }

        public void OnStart(AndroidJavaObject act)
        {
            JFSDKImp.getInstance().onStart(act);
        }

        public void OnRestart(AndroidJavaObject act)
        {
            JFSDKImp.getInstance().onRestart(act);
        }

        public void OnStop(AndroidJavaObject act)
        {
            JFSDKImp.getInstance().onStop(act);
        }

        public void OnDestroy(AndroidJavaObject act)
        {
            JFSDKImp.getInstance().onDestroy(act);
        }

        public void OnNewIntent(AndroidJavaObject act, AndroidJavaObject intent)
        {
            JFSDKImp.getInstance().onNewIntent(act, intent);
        }

        public void OnActivityResult(AndroidJavaObject act, int requestCode, int resultCode, AndroidJavaObject intent)
        {
            JFSDKImp.getInstance().onActivityResult(act, requestCode, resultCode, intent);
        }

        public void OnWindowFocusChanged(bool hasFocus)
        {
            JFSDKImp.getInstance().onWindowFocusChanged(hasFocus);
        }

        public void OnBackPressed()
        {
            JFSDKImp.getInstance().onBackPressed();
        }

        public void OnRequestPermissionsResult(AndroidJavaObject activity, int requestCode, string[] permissions, int[] grantResults)
        {
            JFSDKImp.getInstance().onRequestPermissionsResult(activity, requestCode, permissions, grantResults);
        }
    }

    public class LogincallBack
    {
        public String jfUserId;
        public String userName;
        public String token;
        public String channelUserId;
        public LogincallBack()
        {
        }


        public String getChannelUserId()
        {
            return channelUserId == null ? "" : channelUserId;
        }

        public void setChannelUserId(String channelUserId)
        {
            this.channelUserId = channelUserId;
        }

        public String getJfUserId()
        {
            return jfUserId;
        }

        public void setJfUserId(String jfUserId)
        {
            this.jfUserId = jfUserId;
        }

        public String getUserName()
        {
            return userName;
        }

        public void setUserName(String userName)
        {
            this.userName = userName;
        }

        public String getToken()
        {
            return token;
        }

        public void setToken(String token)
        {
            this.token = token;
        }

    }

    public class LoginErrorMsg
    {
        private int code;
        private String errorMsg;

        public LoginErrorMsg(int code, String errorMsg)
        {
            this.code = code;
            this.errorMsg = errorMsg;
        }


        public String getErrorMsg()
        {
            return errorMsg;
        }

        public void setErrorMsg(String errorMsg)
        {
            this.errorMsg = errorMsg;
        }

        public int getCode()
        {
            return code;
        }

        public void setCode(int code)
        {
            this.code = code;
        }
    }

    public class JfOrderInfo
    {
        private String level;
        private String cpOrderId;
        private String roleName;
        private String roleId;
        private String serverName;
        private String serverId;
        private String vip;
        private String price;
        private String goodsName;
        private String goodsDes;
        private String goodsId;
        private String remark;


        public String getPrice()
        {
            return price == null ? "" : price;
        }

        public void setPrice(String price)
        {
            this.price = price;
        }

        public String getLevel()
        {
            return level == null ? "" : level;
        }

        public void setLevel(String level)
        {
            this.level = level;
        }

        public String getCpOrderId()
        {
            return cpOrderId == null ? "" : cpOrderId;
        }

        public void setCpOrderId(String cpOrderId)
        {
            this.cpOrderId = cpOrderId;
        }

        public String getRoleName()
        {
            return roleName == null ? "" : roleName;
        }

        public void setRoleName(String roleName)
        {
            this.roleName = roleName;
        }

        public String getRoleId()
        {
            return roleId == null ? "" : roleId;
        }

        public void setRoleId(String roleId)
        {
            this.roleId = roleId;
        }

        public String getServerName()
        {
            return serverName == null ? "" : serverName;
        }

        public void setServerName(String serverName)
        {
            this.serverName = serverName;
        }

        public String getServerId()
        {
            return serverId == null ? "" : serverId;
        }

        public void setServerId(String serverId)
        {
            this.serverId = serverId;
        }

        public String getVip()
        {
            return vip == null ? "" : vip;
        }

        public void setVip(String vip)
        {
            this.vip = vip;
        }


        public String getGoodsName()
        {
            return goodsName == null ? "" : goodsName;
        }

        public void setGoodsName(String goodsName)
        {
            this.goodsName = goodsName;
        }

        public String getGoodsDes()
        {
            return goodsDes == null ? "" : goodsDes;
        }

        public void setGoodsDes(String goodsDes)
        {
            this.goodsDes = goodsDes;
        }

        public String getGoodsId()
        {
            return goodsId == null ? "" : goodsId;
        }

        public void setGoodsId(String goodsId)
        {
            this.goodsId = goodsId;
        }

        public String getRemark()
        {
            return remark == null ? "" : remark;
        }

        public void setRemark(String remark)
        {
            this.remark = remark;
        }
        public String toString()
        {
            return "JfOrderInfo{" +
                    "level='" + level + '\'' +
                    ", cpOrderId='" + cpOrderId + '\'' +
                    ", roleName='" + roleName + '\'' +
                    ", roleId='" + roleId + '\'' +
                    ", serverName='" + serverName + '\'' +
                    ", serverId='" + serverId + '\'' +
                    ", vip='" + vip + '\'' +
                    ", price=" + price +
                    ", goodsName='" + goodsName + '\'' +
                    ", goodsDes='" + goodsDes + '\'' +
                    ", goodsId='" + goodsId + '\'' +
                    ", remark='" + remark + '\'' +
                    '}';
        }
    }

    public class PaySuccessInfo
    {
        public String orderId;
        public String gameRole;
        public String gameArea;
        public String productName;
        public String productDesc;
        public String remark;


        public PaySuccessInfo(String orderId, String gameRole, String gameArea, String productName, String productDesc, String remark)
        {
            this.orderId = orderId;
            this.gameRole = gameRole;
            this.gameArea = gameArea;
            this.productName = productName;
            this.productDesc = productDesc;
            this.remark = remark;
        }
    }

    public class PayFaildInfo
    {
        private String code;
        private String msg;

        public PayFaildInfo(String resultStatus, String result)
        {
            this.msg = result;
            this.code = resultStatus;
        }

        public String getMsg()
        {
            return msg;
        }

        public void setMsg(String msg)
        {
            this.msg = msg;
        }

        public String getCode()
        {
            return code;
        }

        public void setCode(String code)
        {
            this.code = code;
        }
    }
    public class CreatOrderInfo
    {
        public String orderId;
        public String gameRole;
        public String gameArea;
        public String productName;
        public String productDesc;
        public String remark;

        public CreatOrderInfo(String orderId, String gameRole, String gameArea, String productName, String productDesc, String remark)
        {
            this.orderId = orderId;
            this.gameRole = gameRole;
            this.gameArea = gameArea;
            this.productName = productName;
            this.productDesc = productDesc;
            this.remark = remark;
        }

    }

    public class JfRoleInfo
    {
        private String serverName;
        private String serverId;
        private String roleName;
        private String roleId;
        private String partyId;
        private String partyName;
        private String gameRoleLevel;
        private String attach;
        private String type;
        private String experience;
        private long roleCreateTime;
        private int vipLevel;
        private int gameRolePower;

        public String getExperience()
        {
            return experience == null ? "" : experience;
        }

        public void setExperience(String experience)
        {
            this.experience = experience;
        }

        public String getType()
        {
            return type == null ? "" : type;
        }

        public void setType(String type)
        {
            this.type = type;
        }

        public String getServerName()
        {
            return serverName == null ? "" : serverName;
        }

        public void setServerName(String serverName)
        {
            this.serverName = serverName;
        }

        public String getServerId()
        {
            return serverId == null ? "" : serverId;
        }

        public void setServerId(String serverId)
        {
            this.serverId = serverId;
        }

        public String getRoleName()
        {
            return roleName == null ? "" : roleName;
        }

        public void setRoleName(String roleName)
        {
            this.roleName = roleName;
        }

        public String getRoleId()
        {
            return roleId == null ? "" : roleId;
        }

        public void setRoleId(String roleId)
        {
            this.roleId = roleId;
        }

        public String getPartyId()
        {
            return partyId == null ? "" : partyId;
        }

        public void setPartyId(String partyId)
        {
            this.partyId = partyId;
        }

        public String getPartyName()
        {
            return partyName == null ? "" : partyName;
        }

        public void setPartyName(String partyName)
        {
            this.partyName = partyName;
        }

        public String getGameRoleLevel()
        {
            return gameRoleLevel == null ? "" : gameRoleLevel;
        }

        public void setGameRoleLevel(String gameRoleLevel)
        {
            this.gameRoleLevel = gameRoleLevel;
        }

        public String getAttach()
        {
            return attach == null ? "" : attach;
        }

        public void setAttach(String attach)
        {
            this.attach = attach;
        }

        public long getRoleCreateTime()
        {
            return roleCreateTime;
        }

        public void setRoleCreateTime(long roleCreateTime)
        {
            this.roleCreateTime = roleCreateTime;
        }


        public int getVipLevel()
        {
            return vipLevel;
        }

        public void setVipLevel(int vipLevel)
        {
            this.vipLevel = vipLevel;
        }

        public int getGameRolePower()
        {
            return gameRolePower;
        }

        public void setGameRolePower(int gameRolePower)
        {
            this.gameRolePower = gameRolePower;
        }

        public String toString()
        {
            return "JfRoleInfo{" +
                    "serverName='" + serverName + '\'' +
                    ", serverId='" + serverId + '\'' +
                    ", roleName='" + roleName + '\'' +
                    ", roleId='" + roleId + '\'' +
                    ", partyId='" + partyId + '\'' +
                    ", partyName='" + partyName + '\'' +
                    ", gameRoleLevel='" + gameRoleLevel + '\'' +
                    ", attach='" + attach + '\'' +
                    ", roleCreateTime=" + roleCreateTime +
                    ", experience=" + experience +
                    ", type=" + type +
                    ", vipLevel=" + vipLevel +
                    ", gameRolePower=" + gameRolePower +
                    '}';
        }
    }
}
