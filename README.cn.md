[EN](https://github.com/dwbeta/OpenMir2/blob/master/README.md)  | [中文](https://github.com/dwbeta/OpenMir2/blob/master/README.cn.md)  

# OpenMir2
 
《热血传奇》是韩国Wemade公司推出的一款大型多人在线角色扮演游戏（MMORPG），由盛大游戏2001年引进到中国并一度火爆全国、风靡一时，至今的经典游戏玩法还在延续。  
该游戏具有战士、魔法师和道士三种职业，所有情节的发生、经验值取得以及各种打猎、采矿等活动都是在网络上即时发生。  
游戏系统包括白天、黑夜、贸易、物品等观念，玩家可以通过采矿、打猎等来获得货币，利用货币进行贸易。整个游戏充满了魔力，具有东方色彩。

>项目基于网络上泄露Delphi代码翻译，仅供一些有兴趣测试,研究之用.目的是为了研究程序的结构,运行原理,作为学习的例子.  
如果您有兴趣完善或遇到问题，欢迎提交issues或pr
 
## 游戏客户端

> 游戏客户端代码(https://github.com/mirbeta/MirClient)     
> 需要自行安装Delphi XE 10.4以上版本环境编译或错误调试.

## 游戏服务端
> 1.基础游戏服务端(https://github.com/mirbeta/MirServer)  
> 2.自行使用搜索引擎搜索其他特色游戏版本文件


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
