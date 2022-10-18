[EN](https://github.com/dwbeta/OpenMir2/blob/master/README.md)  | [中文](https://github.com/dwbeta/OpenMir2/blob/master/README.cn.md)  

# OpenMir2

热血传奇服务端，支持联机和多人游戏.  

>项目基于网络上泄露Delphi代码翻译，仅供一些有兴趣测试,研究之用.目的是为了研究程序的结构,运行原理,作为学习的例子.  
如果你有兴趣完善或遇到问题，欢迎提交issues或pr
 
## 客户端
>  你可用使用网上泄露的Delphi源码自行编译客户端，如（BLUE 小火炬 刺客二代）等 

## 服务端
> 能够使用网络上大部分传奇私服服务端版本，会存在部分脚本命令不支持的问题，可提交issues或pr来支持

## 项目说明
1. DBSvr (数据库服务，读取和保存玩家数据). 
2. LoginSvr (账号服务，读取和保存玩家账号数据). 
3. GameSvr (游戏数据服务,处理玩家数据 走路 攻击 施法等操作). 
4. GameGate (游戏网关,玩家的操作数据并转发到GameSvr处理). 
5. SelGate (角色网关,新建角色、删除、恢复等操作并转发到DBSvr处理). 
6. LoginGate (登陆网关,客户端登陆操作 账号注册、找回密码等并转发到LoginSvr处理). 

## 游戏截图
![](./Images/1632561445962.jpg)
![](./Images/1632561467819.jpg)
![](./Images/1632561488323.jpg)
![](./Images/1632561522104.jpg)
