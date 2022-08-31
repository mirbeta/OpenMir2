[EN](https://github.com/dwbeta/OpenMir2/blob/master/README.md)  | [中文](https://github.com/dwbeta/OpenMir2/blob/master/README.cn.md)  



热血传奇完整服务端，支持经典热血传奇客户端（1.10-1.76），支持联机和多人游戏.  

##  
项目基于网络上泄露Delphi原始代码完成，仅供一些有兴趣的本机测试,研究之用.目的是为了研究程序的结构,运行原理,作为学习的例子.  
如果你有兴趣和疑问，欢迎提交issues或pr

## 正在开发 
1. 客户端(目前进度 登陆、角色场景基本完成)  
2. 微端服务(完成基础Demo)   

### 如何使用    
## 客户端
>  (https://github.com/mirbeta/MirClient) 在这里，需要自行安装Delphi XE 10.4以上版本环境编译.

## 服务端
> (https://github.com/mirbeta/MirServer) 在这里，基础的游戏服务端.

## 项目说明. 
1. DBSvr (数据库服务，读取和保存玩家数据). 
2. LoginSvr (账号服务，读取和保存玩家账号数据). 
3. GameSvr (游戏数据服务,处理玩家数据 走路 攻击 施法等操作). 
4. GameGate (游戏网关,玩家的操作数据并转发到GameSvr处理). 
5. SelGate (角色网关,新建角色、删除、恢复等操作并转发到DBSvr处理). 
6. LoginGate (登陆网关,客户端登陆操作 账号注册、找回密码等并转发到LoginSvr处理). 

### 截图
![](./Images/1632561445962.jpg)
![](./Images/1632561467819.jpg)
![](./Images/1632561488323.jpg)
![](./Images/1632561522104.jpg)
