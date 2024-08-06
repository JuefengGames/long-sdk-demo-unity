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

### 二、 拷贝部分

#### 2.1：文件的拷贝

将demo工程Assest目录下面的Plugins文件夹整体拷贝到目标项目的Assest目录下面

#### 2.2：AndroidManifest.xml文件的配制

需要修改 aar包中的 AndroidManifest.xml 文件中的 JF_APPID和JF_APPKEY为绝锋游戏平台所提供的JF_APPID和JF_APPKEY