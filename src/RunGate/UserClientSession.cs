using System;
using SystemModule;
using SystemModule.Packages;
using SystemModule.Sockets;

namespace RunGate
{
    /// <summary>
    /// 用户会话封包处理
    /// </summary>
    public class UserClientSession
    {
        private GameSpeed _gameSpeed;
        private int nSpeedCount = 0;
        private int mSpeedCount = 0;

        public UserClientSession()
        {
            _gameSpeed = new GameSpeed();
            _gameSpeed.nErrorCount = 0; // 加速的累计值
            _gameSpeed.m_nHitSpeed = 0; // 装备加速
            _gameSpeed.dwSayMsgTick = HUtil32.GetTickCount(); // 发言时间
            _gameSpeed.dwHitTick = HUtil32.GetTickCount(); // 攻击时间
            _gameSpeed.dwSpellTick = HUtil32.GetTickCount(); // 魔法时间
            _gameSpeed.dwWalkTick = HUtil32.GetTickCount(); // 走路时间
            _gameSpeed.dwRunTick = HUtil32.GetTickCount(); // 跑步时间
            _gameSpeed.dwTurnTick = HUtil32.GetTickCount(); // 转身时间
            _gameSpeed.dwButchTick = HUtil32.GetTickCount(); // 挖肉时间
            _gameSpeed.dwEatTick = HUtil32.GetTickCount(); // 吃药时间
            _gameSpeed.dwPickupTick = HUtil32.GetTickCount(); // 捡起时间
            _gameSpeed.dwRunWalkTick = HUtil32.GetTickCount(); // 移动时间
            _gameSpeed.dwFeiDnItemsTick = HUtil32.GetTickCount() - 10000; // 传送时间  {-15000 刚上来15秒内不能拣东西}
            _gameSpeed.dwSupSpeederTick = HUtil32.GetTickCount(); // 变速齿轮时间
            _gameSpeed.dwSupSpeederCount = 0; // 变速齿轮累计
            _gameSpeed.dwSuperNeverTick = HUtil32.GetTickCount(); // 超级加速时间
            _gameSpeed.dwSuperNeverCount = 0; // 超级加速累计
            _gameSpeed.dwUserDoTick = 0; // 记录上一次操作
            _gameSpeed.dwContinueTick = 0; // 保存停顿操作时间
            _gameSpeed.dwConHitMaxCount = 0; // 带有攻击并发累计
            _gameSpeed.dwConSpellMaxCount = 0; // 带有魔法并发累计
            _gameSpeed.dwCombinationTick = 0; // 记录上一次移动方向
            _gameSpeed.dwCombinationCount = 0; // 智能攻击累计
            _gameSpeed.dwGameTick = HUtil32.GetTickCount(); // 在线时间
        }

        public GameSpeed GetGameSpeed()
        {
            return _gameSpeed;
        }

        /// <summary>
        /// 处理客户端发送过来的封包
        /// </summary>
        /// <param name="UserData"></param>
        public void HangdleUserPacket(TSendUserData UserData)
        {
            string sMsg = string.Empty;
            string sData = string.Empty;
            string sDefMsg = string.Empty;
            string sDataMsg = string.Empty;
            string sDataText = string.Empty;
            string sHumName = string.Empty;
            byte[] DataBuffer = null;
            int nOPacketIdx;
            int nPacketIdx;
            int nDataLen;
            int n14;
            TDefaultMessage DefMsg;
            TDefaultMessage Msg;
            try
            {
                n14 = 0;
                //nProcessMsgSize += UserData.sMsg.Length;
                if (UserData.nSocketIdx >= 0 && UserData.nSocketIdx < UserData.UserClient.GetMaxSession())
                {
                    if (UserData.nSocketHandle == UserData.UserClient.SessionArray[UserData.nSocketIdx].nSckHandle && UserData.UserClient.SessionArray[UserData.nSocketIdx].nPacketErrCount < 10)
                    {
                        if (UserData.UserClient.SessionArray[UserData.nSocketIdx].sSocData.Length > GateShare.MSGMAXLENGTH)
                        {
                            UserData.UserClient.SessionArray[UserData.nSocketIdx].sSocData = "";
                            UserData.UserClient.SessionArray[UserData.nSocketIdx].nPacketErrCount = 99;
                            UserData.sMsg = "";
                        }
                        sMsg = UserData.UserClient.SessionArray[UserData.nSocketIdx].sSocData + UserData.sMsg;
                        while (true)
                        {
                            sData = "";
                            sMsg = HUtil32.ArrestStringEx(sMsg, "#", "!", ref sData);
                            if (sData.Length > 2)
                            {
                                nPacketIdx = HUtil32.Str_ToInt(sData[0].ToString(), 99); // 将数据名第一位的序号取出
                                if (UserData.UserClient.SessionArray[UserData.nSocketIdx].nPacketIdx == nPacketIdx)
                                {
                                    // 如果序号重复则增加错误计数
                                    UserData.UserClient.SessionArray[UserData.nSocketIdx].nPacketErrCount++;
                                }
                                else
                                {
                                    nOPacketIdx = UserData.UserClient.SessionArray[UserData.nSocketIdx].nPacketIdx;
                                    UserData.UserClient.SessionArray[UserData.nSocketIdx].nPacketIdx = nPacketIdx;
                                    sData = sData.Substring(1, sData.Length - 1);
                                    nDataLen = sData.Length;
                                    if (nDataLen >= Grobal2.DEFBLOCKSIZE)
                                    {
                                        if (UserData.UserClient.SessionArray[UserData.nSocketIdx].boStartLogon)// 第一个人物登录数据包
                                        {
                                            // 第一个人物登录数据包   **1111/小小/6/120040918/0
                                            sDataText = EDcode.DeCodeString(sData,true);
                                            if ((sDataText[0] != '*') || (sDataText[1] != '*'))// 非法登陆
                                            {
                                                UserData.UserClient.SessionArray[UserData.nSocketIdx].nSckHandle =  -1;
                                                UserData.UserClient.SessionArray[UserData.nSocketIdx].sSocData = "";
                                                UserData.UserClient.SessionArray[UserData.nSocketIdx].sSendData = "";
                                                UserData.UserClient.SessionArray[UserData.nSocketIdx].Socket.Close();
                                                UserData.UserClient.SessionArray[UserData.nSocketIdx].Socket = null;
                                                return;
                                            }
                                            sDataText = HUtil32.GetValidStr3(sDataText, ref sHumName, new string[] {"/"});
                                            sDataText = HUtil32.GetValidStr3(sDataText, ref UserData.UserClient.SessionArray[UserData.nSocketIdx].sUserName, new string[] {"/"}); // 取角色名
                                            sDataText = "";
                                            sHumName = "";
                                            
                                            //nHumLogonMsgSize += sData.Length;
                                            UserData.UserClient.SessionArray[UserData.nSocketIdx].boStartLogon = false;
                                            sData = "#" + nPacketIdx + sData + "!";
                                            var sendBuff = HUtil32.GetBytes(sData);
                                            Send(Grobal2.GM_DATA, UserData.nSocketIdx, (int)UserData.UserClient.SessionArray[UserData.nSocketIdx].Socket.Handle,
                                                   UserData.UserClient.SessionArray[UserData.nSocketIdx].nUserListIndex, sendBuff.Length, sendBuff);
                                        }
                                        else
                                        {
                                            // 普通数据包
                                            //nHumPlayMsgSize += sData.Length;
                                            if (nDataLen == Grobal2.DEFBLOCKSIZE)
                                            {
                                                sDefMsg = sData;
                                                sDataMsg = "";
                                            }
                                            else
                                            {
                                                sDefMsg = sData.Substring(0, Grobal2.DEFBLOCKSIZE);
                                                sDataMsg = sData.Substring(Grobal2.DEFBLOCKSIZE, sData.Length - Grobal2.DEFBLOCKSIZE);
                                            }
                                            DefMsg = EDcode.DecodeMessage(sDefMsg); // 检查数据
                                            if (GateShare.boStartSpeedCheck) //游戏速度控制
                                            {
                                                if (!UserData.UserClient.SessionArray[UserData.nSocketIdx].boSendAvailable)
                                                {
                                                    break;
                                                }
                                                var btSpeedControlMode = CheckDefMsg(DefMsg, UserData.UserClient.SessionArray[UserData.nSocketIdx], ref sDataMsg);
                                                switch (btSpeedControlMode)
                                                {
                                                    case 0:// 0停顿操作
                                                        UserData.UserClient.SessionArray[UserData.nSocketIdx].sSocData = "";// 清空所有当前动作
                                                        _gameSpeed.dwContinueTick = HUtil32.GetTickCount();// 保存停顿操作时间
                                                        SendWarnMsg(UserData.UserClient.SessionArray[UserData.nSocketIdx], GateShare.jWarningMsg, GateShare.btMsgFColorJ, GateShare.btMsgBColorJ);// 提示文字警告
                                                        continue;
                                                    case 1:// 1延迟处理
                                                        UserData.UserClient.SessionArray[UserData.nSocketIdx].bosendAvailableStart = true;
                                                        SendWarnMsg(UserData.UserClient.SessionArray[UserData.nSocketIdx], GateShare.yWarningMsg, GateShare.btMsgFColorY, GateShare.btMsgBColorY);// 提示文字警告
                                                        break;
                                                    case 2:// 2游戏掉线
                                                        mSpeedCount++;// 统计防御
                                                        sHumName = UserData.UserClient.SessionArray[UserData.nSocketIdx].sUserName;
                                                        if (!GateShare.GameSpeedList.ContainsKey(sHumName))
                                                        {
                                                            GateShare.GameSpeedList.TryAdd(sHumName, sHumName);
                                                        }
                                                        if (GateShare.boCheckBoxShowData)
                                                        {
                                                            GateShare.AddMainLogMsg("[超速提示]:" + sHumName + " 使用非法加速，已被T下线。", 3);
                                                        }
                                                        SendWarnMsg(UserData.UserClient.SessionArray[UserData.nSocketIdx], GateShare.sWarningMsg, GateShare.btMsgFColorS, GateShare.btMsgBColorS);// 提示文字警告
                                                        Send(Grobal2.GM_CLOSE, 0, (int)UserData.UserClient.SessionArray[UserData.nSocketIdx].Socket.Handle, 0, 0, null);// 发送给M2，通知T人
                                                        UserData.UserClient.SessionArray[UserData.nSocketIdx].nSckHandle = -1;
                                                        UserData.UserClient.SessionArray[UserData.nSocketIdx].sSocData = "";
                                                        UserData.UserClient.SessionArray[UserData.nSocketIdx].sSendData = "";
                                                        _gameSpeed.nErrorCount = 0;// 清理累计
                                                        //UserData.UserClient.SessionCount -= 1;
                                                        UserData.UserClient.SessionArray[UserData.nSocketIdx].Socket.Close();
                                                        UserData.UserClient.SessionArray[UserData.nSocketIdx].Socket = null;
                                                        return;
                                                    case 3:// 3 执行脚本
                                                        nSpeedCount++;// 统计防御
                                                        sHumName = UserData.UserClient.SessionArray[UserData.nSocketIdx].sUserName;
                                                        if (!GateShare.GameSpeedList.ContainsKey(sHumName))
                                                        {
                                                            GateShare.GameSpeedList.TryAdd(sHumName, sHumName);
                                                        }
                                                        if (GateShare.boCheckBoxShowData)
                                                        {
                                                            GateShare.AddMainLogMsg("[超速提示]:" + sHumName + " 使用非法加速，已脚本处理。", 3);
                                                        }
                                                        SendWarnMsg(UserData.UserClient.SessionArray[UserData.nSocketIdx], GateShare.sWarningMsg, GateShare.btMsgFColorS, GateShare.btMsgBColorS);// 提示文字警告
                                                        _gameSpeed.nErrorCount = 0;// 清理累计
                                                        Msg = Grobal2.MakeDefaultMsg(Grobal2.CM_SAY, 0, 0, 0, 0);
                                                        sMsg = "#" + 0 + EDcode.EncodeMessage(Msg) + EDcode.EncodeString("@加速处理") + "!" + sMsg;
                                                        break;
                                                    case 4:// 4 抛出封包
                                                        continue;
                                                    case 10:// 0 + 10 抛出封包      转身、挖肉、拣取  说话过滤
                                                        continue;
                                                    case 11:// 1 + 10 延迟处理      喝药
                                                        UserData.UserClient.SessionArray[UserData.nSocketIdx].bosendAvailableStart = true;
                                                        break;
                                                }
                                            }
                                            if (!string.IsNullOrEmpty(sDataMsg))
                                            {
                                                DataBuffer = new byte[sDataMsg.Length + 12 + 1]; //GetMem(Buffer, sDataMsg.Length + 12 + 1);
                                                Buffer.BlockCopy(DefMsg.ToByte(), 0, DataBuffer, 0, 12);//Move(DefMsg, Buffer, 12);
                                                var msgBuff = HUtil32.GetBytes(sDataMsg);
                                                Buffer.BlockCopy(msgBuff, 0, DataBuffer, 12, msgBuff.Length); //Move(sDataMsg[1], Buffer[12], sDataMsg.Length + 1);
                                                Send(Grobal2.GM_DATA, UserData.nSocketIdx, (int)UserData.UserClient.SessionArray[UserData.nSocketIdx].Socket.Handle,
                                                         UserData.UserClient.SessionArray[UserData.nSocketIdx].nUserListIndex, DataBuffer.Length, DataBuffer);
                                            }
                                            else
                                            {
                                                DataBuffer = DefMsg.ToByte();
                                                Send(Grobal2.GM_DATA, UserData.nSocketIdx, (int)UserData.UserClient.SessionArray[UserData.nSocketIdx].Socket.Handle,
                                                    UserData.UserClient.SessionArray[UserData.nSocketIdx].nUserListIndex, 12, DataBuffer);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (n14 >= 1)
                                {
                                    sMsg = "";
                                }
                                else
                                {
                                    n14++;
                                }
                            }
                            if (HUtil32.TagCount(sMsg, '!') < 1)
                            {
                                break;
                            }
                        }
                        UserData.UserClient.SessionArray[UserData.nSocketIdx].sSocData = sMsg;
                    }
                    else
                    {
                        UserData.UserClient.SessionArray[UserData.nSocketIdx].sSocData = "";
                    }
                }
            }
            catch
            {
                if (UserData.nSocketIdx >= 0 && UserData.nSocketIdx < UserData.UserClient.GetMaxSession())
                {
                    sData = "[" + UserData.UserClient.SessionArray[UserData.nSocketIdx].sRemoteAddr + "]";
                }
                GateShare.AddMainLogMsg("[Exception] ProcessUserPacket" + sData, 1);
            }
        }

        private int CheckDefMsg(TDefaultMessage DefMsg, TSessionInfo SessionInfo, ref string sMsg)
        {
            int result= -1;
            int NextHitTime;
            int LevelFastTime;
            int nMsgCount;
            string sDataText;
            string sHumName = string.Empty;
            try
            {
                TDefaultMessage? message = DefMsg;
                if (message == null)
                {
                    result = 2;
                    return result;
                }
                if ((SessionInfo == null))
                {
                    result = 2;
                    return result;
                }
                if ((SessionInfo.Socket == null))
                {
                    result = 2;
                    return result;
                }
                switch (DefMsg.Ident)
                {
                    case Grobal2.CM_WALK:
                        // 走路
                        // ------------------------------------
                        // 原理:外挂利用挖地.自身不显示下蹲动作,而达到比正常玩家快1步.
                        // 每次保存上一次动作,判断挖地操作后,少下蹲动作,作为挖地暗杀处理.
                        // ------------------------------------
                        if (GateShare.boDarkHitCheck)
                        {
                            // 封挖地暗杀
                            if (_gameSpeed.dwUserDoTick == Grobal2.CM_BUTCH)
                            {
                                result = 2;// 返回掉线处理
                                return result;
                            }
                        }
                        // ------------------------------------
                        if ((HUtil32.GetTickCount() - _gameSpeed.dwContinueTick) < 3000) // 停顿操作后3秒内数据全部抛出
                        {
                            result = 4;// 返回抛出数据
                            return result;
                        }
                        // ------------------------------------
                        // 原理：智能攻击，十字走位等，来回自动跑位0、2、4、6方向
                        // 只要判断重复以上移动动作超过10次以上，并判断为智能攻击。
                        // 重复1个动作只累加1次，如一直一个方向移动需要排除掉。
                        // ------------------------------------
                        if (GateShare.boCombinationCheck)
                        {
                            // 封掉组合攻击
                            if ((_gameSpeed.dwCombinationTick != DefMsg.Tag))
                            {
                                if ((DefMsg.Tag == 1) || (DefMsg.Tag == 3) || (DefMsg.Tag == 5) || (DefMsg.Tag == 7))
                                {
                                    if (_gameSpeed.dwCombinationCount > 10)
                                    {
                                        result = 2;// 返回掉线处理
                                        return result;
                                    }
                                    _gameSpeed.dwCombinationCount++;// 智能攻击累计
                                }
                                else
                                {
                                    _gameSpeed.dwCombinationCount = 0;// 清零
                                }
                            }
                            else
                            {
                                if (_gameSpeed.dwCombinationCount >= 1)
                                {
                                    _gameSpeed.dwCombinationCount -= 1; // 一个方向走减少累计
                                }
                            }
                            _gameSpeed.dwCombinationTick = DefMsg.Tag;// 记录移动方向
                        }
                        // ------------------------------------
                        if (GateShare.boStartWalkCheck)
                        {
                            if ((HUtil32.GetTickCount() - _gameSpeed.dwWalkTick) < GateShare.dwSpinEditWalkTime)
                            {
                                _gameSpeed.nErrorCount += GateShare.nIncErrorCount;// 每次加速的累加值
                                if (_gameSpeed.nErrorCount >= GateShare.nSpinEditWalkCount) // 50
                                {
                                    if (SessionInfo.boSendAvailable)
                                    {
                                        SessionInfo.dwClientCheckTimeOut = 500; // 延迟间隔
                                    }
                                    else
                                    {
                                        SessionInfo.dwClientCheckTimeOut += GateShare.dwSpinEditWalkTime; // 延迟间隔
                                    }
                                    _gameSpeed.dwSuperNeverTick = HUtil32.GetTickCount();// 保存超级加速时间
                                    _gameSpeed.nErrorCount = 0;// 清除累计值
                                    result = GateShare.dwComboBoxWalkCheck;// 返回走路加速处理
                                }
                            }
                            else
                            {
                                if (_gameSpeed.nErrorCount >= GateShare.nDecErrorCount)
                                {
                                    _gameSpeed.nErrorCount -= GateShare.nDecErrorCount;// 正常动作的减少值
                                }
                            }
                            if (SessionInfo.sUserName == GateShare.boMsgUserName)
                            {
                                GateShare.AddMainLogMsg("[走路间隔]：" + (HUtil32.GetTickCount() - _gameSpeed.dwWalkTick) + " 毫秒(" + (_gameSpeed.nErrorCount) + "/50)", 3);
                            }
                            // 封包显示
                            _gameSpeed.dwWalkTick = HUtil32.GetTickCount();// 保存走路时间
                        }
                        _gameSpeed.dwRunWalkTick = HUtil32.GetTickCount();// 保存移动时间
                        _gameSpeed.dwGameTick = HUtil32.GetTickCount();
                        break;
                    case Grobal2.CM_RUN:// 在线时间
                        // 跑步
                        // ------------------------------------
                        // 原理:外挂利用挖地.自身不显示下蹲动作,而达到比正常玩家快1步.
                        // 每次保存上一次动作,判断挖地操作后,少下蹲动作,作为挖地暗杀处理.
                        // ------------------------------------
                        if (GateShare.boDarkHitCheck) // 封挖地暗杀
                        {
                            if (_gameSpeed.dwUserDoTick == Grobal2.CM_BUTCH)
                            {
                                result = 2;// 返回掉线处理
                                return result;
                            }
                        }
                        // ------------------------------------
                        // 原理：返回移动加速处理 或 转向加速处理 后
                        // 再指定一段时间内 再次接收2次或2次以上跑步动作
                        // 将判断为外挂强行破解客户端程序，所谓超级不卡。
                        // ------------------------------------
                        if (GateShare.boSuperNeverCheck) // 封超级加速
                        {
                            if ((HUtil32.GetTickCount() - _gameSpeed.dwSuperNeverTick) < HUtil32._MAX(500, GateShare.dwSpinEditRunTime))
                            {
                                if (_gameSpeed.dwSuperNeverCount >= 2)
                                {
                                    _gameSpeed.dwSuperNeverCount = 0;// 清零
                                    result = 2;// 返回掉线处理
                                    return result;
                                }
                                else
                                {
                                    _gameSpeed.dwSuperNeverCount++;// 超级加速累计
                                }
                            }
                        }
                        // ------------------------------------
                        if ((HUtil32.GetTickCount() - _gameSpeed.dwContinueTick) < 3000)// 停顿操作后3秒内数据全部抛出
                        {
                            result = 4;// 返回抛出数据
                            return result;
                        }
                        // ------------------------------------
                        // 原理：智能攻击，十字走位等，来回自动跑位0、2、4、6方向
                        // 只要判断重复以上移动动作超过10次以上，并判断为智能攻击。
                        // 重复1个动作只累加1次，如一直一个方向移动需要排除掉。
                        // ------------------------------------
                        if (GateShare.boCombinationCheck)
                        {
                            // 封掉组合攻击
                            if ((_gameSpeed.dwCombinationTick != DefMsg.Tag))
                            {
                                if ((DefMsg.Tag == 0) || (DefMsg.Tag == 2) || (DefMsg.Tag == 4) || (DefMsg.Tag == 6))
                                {
                                    if (_gameSpeed.dwCombinationCount > 10)
                                    {
                                        result = 2;// 返回掉线处理
                                        return result;
                                    }
                                    _gameSpeed.dwCombinationCount++;// 智能攻击累计
                                }
                                else
                                {
                                    _gameSpeed.dwCombinationCount = 0;// 清零
                                }
                            }
                            else
                            {
                                if (_gameSpeed.dwCombinationCount >= 1)
                                {
                                    _gameSpeed.dwCombinationCount -= 1;// 一个方向走减少累计
                                }
                            }
                            _gameSpeed.dwCombinationTick = DefMsg.Tag;// 记录移动方向
                        }
                        // ------------------------------------
                        if (GateShare.boStartRunCheck)
                        {
                            if ((HUtil32.GetTickCount() - _gameSpeed.dwRunTick) < GateShare.dwSpinEditRunTime)
                            {
                                _gameSpeed.nErrorCount += GateShare.nIncErrorCount;// 每次加速的累加值
                                // 50
                                if (_gameSpeed.nErrorCount >= GateShare.nSpinEditRunCount)
                                {
                                    if (SessionInfo.boSendAvailable)
                                    {
                                        SessionInfo.dwClientCheckTimeOut = 500;// 延迟间隔
                                    }
                                    else
                                    {
                                        SessionInfo.dwClientCheckTimeOut += GateShare.dwSpinEditRunTime;// 延迟间隔
                                    }
                                    _gameSpeed.dwSuperNeverTick = HUtil32.GetTickCount();// 保存超级加速时间
                                    _gameSpeed.nErrorCount = 0;// 清除累计值
                                    result = GateShare.dwComboBoxRunCheck;// 返回跑步加速处理
                                }
                            }
                            else
                            {
                                if (_gameSpeed.nErrorCount >= GateShare.nDecErrorCount)
                                {
                                    _gameSpeed.nErrorCount -= GateShare.nDecErrorCount; // 正常动作的减少值
                                }
                            }
                            if (SessionInfo.sUserName == GateShare.boMsgUserName)
                            {
                                GateShare.AddMainLogMsg("[跑步间隔]：" + (HUtil32.GetTickCount() - _gameSpeed.dwRunTick) + " 毫秒(" + (_gameSpeed.nErrorCount) + "/50)", 3);// 封包显示
                            }
                            _gameSpeed.dwRunTick = HUtil32.GetTickCount();// 保存跑步时间
                        }
                        _gameSpeed.dwRunWalkTick = HUtil32.GetTickCount();// 保存移动时间
                        _gameSpeed.dwGameTick = HUtil32.GetTickCount();
                        break;
                    //case Grobal2.CM_3035:// 在线时间
                    //    if (GateShare.boWalk3caseCheck) // 一步三格
                    //    {
                    //        result = 2;// 返回掉线处理
                    //        return result;
                    //    }
                    //    break;
                    //case Grobal2.CM_1099:// 过非法移动
                    //    if (GateShare.boSuperNeverCheck) // 封掉超级加速
                    //    {
                    //        result = 2;// 返回掉线处理
                    //        return result;
                    //    }
                    //    break;
                    case Grobal2.CM_HIT:
                    case Grobal2.CM_HEAVYHIT:
                    case Grobal2.CM_BIGHIT:
                    case Grobal2.CM_POWERHIT:
                    case Grobal2.CM_LONGHIT:
                    case Grobal2.CM_WIDEHIT:
                    case Grobal2.CM_FIREHIT:
                        // 霜月
                        // 攻击
                        // ------------------------------------
                        // 原理:外挂利用挖地.自身不显示下蹲动作,而达到比正常玩家快1步.
                        // 每次保存上一次动作,判断挖地操作后,少下蹲动作,作为挖地暗杀处理.
                        // ------------------------------------
                        if (GateShare.boDarkHitCheck)
                        {
                            // 封挖地暗杀
                            if (_gameSpeed.dwUserDoTick == Grobal2.CM_BUTCH)
                            {
                                result = 2;// 返回掉线处理
                                return result;
                            }
                        }
                        // ------------------------------------
                        if ((HUtil32.GetTickCount() - _gameSpeed.dwContinueTick) < 3000)
                        {
                            // 停顿操作后3秒内数据全部抛出
                            result = 4;
                            // 返回抛出数据
                            return result;
                        }
                        // ------------------------------------
                        // 原理:判断大于10条封包时，含有攻击操作，连续触犯2次，当秒杀处理。
                        // ------------------------------------
                        if (GateShare.boStartConHitMaxCheck)
                        {
                            // 带有攻击并发控制
                            nMsgCount = HUtil32.TagCount(SessionInfo.sSocData, '!');
                            if (nMsgCount > GateShare.dwSpinEditConHitMaxTime)
                            {
                                if (_gameSpeed.dwConHitMaxCount > 1)
                                {
                                    _gameSpeed.dwConHitMaxCount = 0;// 清零
                                    result = GateShare.dwComboBoxConHitMaxCheck;// 返回带有攻击并发处理
                                    return result;
                                }
                                else
                                {
                                    _gameSpeed.dwConHitMaxCount++;// 带有攻击并发累计
                                }
                            }
                            else
                            {
                                _gameSpeed.dwConHitMaxCount = 0;// 清零
                            }
                        }
                        // ------------------------------------
                        // 原理:保存每次走路和跑步时间,判断移动后攻击之间间隔.
                        // ------------------------------------
                        if (GateShare.boStartRunhitCheck)
                        {
                            if ((HUtil32.GetTickCount() - _gameSpeed.dwRunWalkTick) < GateShare.dwSpinEditRunhitTime) // 移动攻击
                            {
                                SessionInfo.dwClientCheckTimeOut = 5000;// 延迟间隔
                                result = GateShare.dwComboBoxRunhitCheck;// 返回移动攻击加速处理
                                return result;// 到这里停止下面攻击检测
                            }
                        }
                        // ------------------------------------
                        if (GateShare.boStartHitCheck)
                        {
                            LevelFastTime = HUtil32._MIN(430, _gameSpeed.m_nHitSpeed * GateShare.nItemSpeedCount);// 60
                            NextHitTime = HUtil32._MAX(200, GateShare.dwSpinEditHitTime - LevelFastTime);
                            if ((HUtil32.GetTickCount() - _gameSpeed.dwHitTick) < NextHitTime)
                            {
                                _gameSpeed.nErrorCount += GateShare.nIncErrorCount;// 每次加速的累加值
                                if (_gameSpeed.nErrorCount >= GateShare.nSpinEditHitCount)// 50
                                {
                                    if (SessionInfo.boSendAvailable)// 延迟间隔
                                    {
                                        SessionInfo.dwClientCheckTimeOut = 3000;
                                    }
                                    else
                                    {
                                        SessionInfo.dwClientCheckTimeOut += GateShare.dwSpinEditHitTime; // 延迟间隔
                                    }
                                    _gameSpeed.nErrorCount = 0;// 清除累计值
                                    result = GateShare.dwComboBoxHitCheck;// 返回攻击加速处理
                                }
                            }
                            else
                            {
                                if (_gameSpeed.nErrorCount >= GateShare.nDecErrorCount)
                                {
                                    _gameSpeed.nErrorCount -= GateShare.nDecErrorCount;// 正常动作的减少值
                                }
                            }
                            if (SessionInfo.sUserName == GateShare.boMsgUserName)
                            {
                                GateShare.AddMainLogMsg("[攻击间隔]：" + (HUtil32.GetTickCount() - _gameSpeed.dwHitTick) + " 毫秒(" + (_gameSpeed.nErrorCount) + "/50)", 3);// 封包显示
                            }
                            _gameSpeed.dwHitTick = HUtil32.GetTickCount();// 保存攻击时间
                            _gameSpeed.dwGameTick = HUtil32.GetTickCount();// 在线时间
                        }
                        break;
                    case Grobal2.CM_3037:
                        // ------------------------------------
                        // 原理:外挂利用挖地.自身不显示下蹲动作,而达到比正常玩家快1步.
                        // 每次保存上一次动作,判断挖地操作后,少下蹲动作,作为挖地暗杀处理.
                        // ------------------------------------
                        if (GateShare.boDarkHitCheck) // 封挖地暗杀
                        {
                            if (_gameSpeed.dwUserDoTick == Grobal2.CM_BUTCH)
                            {
                                result = 2;// 返回掉线处理
                                return result;
                            }
                        }
                        // ------------------------------------
                        if ((HUtil32.GetTickCount() - _gameSpeed.dwContinueTick) < 3000) // 停顿操作后3秒内数据全部抛出
                        {
                            result = 4;// 返回抛出数据
                            return result;
                        }
                        // ------------------------------------
                        // 原理:判断大于10条封包时，含有攻击操作，连续触犯2次，当秒杀处理。
                        // ------------------------------------
                        if (GateShare.boStartConHitMaxCheck)
                        {
                            // 带有攻击并发控制
                            nMsgCount = HUtil32.TagCount(SessionInfo.sSocData, '!');
                            if (nMsgCount > GateShare.dwSpinEditConHitMaxTime)
                            {
                                if (_gameSpeed.dwConHitMaxCount > 1)
                                {
                                    _gameSpeed.dwConHitMaxCount = 0;// 清零
                                    result = GateShare.dwComboBoxConHitMaxCheck;// 返回带有攻击并发处理
                                    return result;
                                }
                                else
                                {
                                    _gameSpeed.dwConHitMaxCount++;// 带有攻击并发累计
                                }
                            }
                            else
                            {
                                _gameSpeed.dwConHitMaxCount = 0;// 清零
                            }
                        }
                        // ------------------------------------
                        // 原理:保存每次走路和跑步时间,判断移动后攻击之间间隔.
                        // ------------------------------------
                        if (GateShare.boStartRunhitCheck) // 移动攻击
                        {
                            if ((HUtil32.GetTickCount() - _gameSpeed.dwRunWalkTick) < GateShare.dwSpinEditRunhitTime)
                            {
                                SessionInfo.dwClientCheckTimeOut = 5000;// 延迟间隔
                                result = GateShare.dwComboBoxRunhitCheck;// 返回移动攻击加速处理
                                return result;// 到这里停止下面攻击检测
                            }
                        }
                        // ------------------------------------
                        if (GateShare.boAfterHitCheck)
                        {
                            LevelFastTime = HUtil32._MIN(430, _gameSpeed.m_nHitSpeed * GateShare.nItemSpeedCount); // 60
                            NextHitTime = HUtil32._MAX(200, GateShare.dwSpinEditHitTime - LevelFastTime);
                            if ((HUtil32.GetTickCount() - _gameSpeed.dwHitTick) < NextHitTime)
                            {
                                _gameSpeed.nErrorCount += GateShare.nIncErrorCount;// 每次加速的累加值
                                if (_gameSpeed.nErrorCount >= GateShare.nSpinEditHitCount) // 50
                                {
                                    if (SessionInfo.boSendAvailable)
                                    {
                                        SessionInfo.dwClientCheckTimeOut = 3000; // 延迟间隔
                                    }
                                    else
                                    {
                                        SessionInfo.dwClientCheckTimeOut += GateShare.dwSpinEditHitTime; // 延迟间隔
                                    }
                                    _gameSpeed.nErrorCount = 0;// 清除累计值
                                    result = GateShare.dwComboBoxHitCheck;// 返回攻击加速处理
                                }
                            }
                            else
                            {
                                if (_gameSpeed.nErrorCount >= GateShare.nDecErrorCount)
                                {
                                    _gameSpeed.nErrorCount -= GateShare.nDecErrorCount; // 正常动作的减少值
                                }
                            }
                            if (SessionInfo.sUserName == GateShare.boMsgUserName)
                            {
                                GateShare.AddMainLogMsg("[攻击间隔]：" + (HUtil32.GetTickCount() - _gameSpeed.dwHitTick) + " 毫秒(" + (_gameSpeed.nErrorCount) + "/50)", 3);// 封包显示
                            }
                            _gameSpeed.dwHitTick = HUtil32.GetTickCount();// 保存攻击时间
                            _gameSpeed.dwGameTick = HUtil32.GetTickCount();// 在线时间
                        }
                        break;
                    case Grobal2.CM_SPELL: // 魔法
                        // ------------------------------------
                        // 原理:外挂利用挖地.自身不显示下蹲动作,而达到比正常玩家快1步.
                        // 每次保存上一次动作,判断挖地操作后,少下蹲动作,作为挖地暗杀处理.
                        // ------------------------------------
                        if (GateShare.boDarkHitCheck)
                        {
                            // 封挖地暗杀
                            if (_gameSpeed.dwUserDoTick == Grobal2.CM_BUTCH)
                            {
                                result = 2;// 返回掉线处理
                                return result;
                            }
                        }
                        // ------------------------------------
                        if ((HUtil32.GetTickCount() - _gameSpeed.dwContinueTick) < 3000) // 停顿操作后3秒内数据全部抛出
                        {
                            result = 4;// 返回抛出数据
                            return result;
                        }
                        // ------------------------------------
                        // 原理:判断大于10条封包时，含有魔法操作，连续触犯2次，当秒杀处理。
                        // ------------------------------------
                        if (GateShare.boStartConSpellMaxCheck)
                        {
                            // 带有魔法并发控制
                            nMsgCount = HUtil32.TagCount(SessionInfo.sSocData, '!');
                            if (nMsgCount > GateShare.dwSpinEditConSpellMaxTime)
                            {
                                if (_gameSpeed.dwConSpellMaxCount > 1)
                                {
                                    _gameSpeed.dwConSpellMaxCount = 0;// 清零
                                    result = GateShare.dwComboBoxConSpellMaxCheck;// 返回带有攻击并发处理
                                    return result;
                                }
                                else
                                {
                                    _gameSpeed.dwConSpellMaxCount++;// 带有魔法并发累计
                                }
                            }
                            else
                            {
                                _gameSpeed.dwConSpellMaxCount = 0;// 清零
                            }
                        }
                        // ------------------------------------
                        // 原理:保存每次走路和跑步时间,判断移动后魔法之间间隔.
                        // ------------------------------------
                        if (GateShare.boStartRunhitCheck)
                        {
                            // 移动魔法
                            if ((HUtil32.GetTickCount() - _gameSpeed.dwRunWalkTick) < GateShare.dwSpinEditRunspellTime)
                            {
                                SessionInfo.dwClientCheckTimeOut = 5000;// 延迟间隔
                                result = GateShare.dwComboBoxRunspellCheck;// 返回移动魔法加速处理
                                return result;// 到这里停止下面攻击检测
                            }
                        }
                        switch (DefMsg.Tag)
                        {
                            case Grobal2.SKILL_FIREBALL:
                            case Grobal2.SKILL_HEALLING:
                            case Grobal2.SKILL_FIREBALL2:
                            case Grobal2.SKILL_AMYOUNSUL:
                            case Grobal2.SKILL_FIREWIND:
                            case Grobal2.SKILL_FIRE:
                            case Grobal2.SKILL_SHOOTLIGHTEN:
                            case Grobal2.SKILL_LIGHTENING:
                            case Grobal2.SKILL_FIRECHARM:
                            case Grobal2.SKILL_HANGMAJINBUB:
                            case Grobal2.SKILL_DEJIWONHO:
                            case Grobal2.SKILL_HOLYSHIELD:
                            case Grobal2.SKILL_SKELLETON:
                            case Grobal2.SKILL_CLOAK:
                            case Grobal2.SKILL_BIGCLOAK:
                            case Grobal2.SKILL_TAMMING:
                            case Grobal2.SKILL_SPACEMOVE:
                            case Grobal2.SKILL_EARTHFIRE:
                            case Grobal2.SKILL_FIREBOOM:
                            case Grobal2.SKILL_LIGHTFLOWER:
                            case Grobal2.SKILL_MOOTEBO:
                            case Grobal2.SKILL_SHOWHP:
                            case Grobal2.SKILL_BIGHEALLING:
                            case Grobal2.SKILL_SINSU:
                            case Grobal2.SKILL_SHIELD:
                            case Grobal2.SKILL_KILLUNDEAD:
                            case Grobal2.SKILL_SNOWWIND:
                            case Grobal2.SKILL_UNAMYOUNSUL:
                            case Grobal2.SKILL_WINDTEBO:
                            case Grobal2.SKILL_MABE:
                            case Grobal2.SKILL_GROUPLIGHTENING:
                            case Grobal2.SKILL_GROUPAMYOUNSUL:
                            case Grobal2.SKILL_GROUPDEDING:
                            case Grobal2.SKILL_44:
                            case Grobal2.SKILL_45:
                            case Grobal2.SKILL_46:
                            case Grobal2.SKILL_47:
                            case Grobal2.SKILL_49:
                            case Grobal2.SKILL_51:
                            case Grobal2.SKILL_52:
                            case Grobal2.SKILL_53:
                            case Grobal2.SKILL_54:
                            case Grobal2.SKILL_55:
                            case Grobal2.SKILL_57:
                            case Grobal2.SKILL_58:
                            case Grobal2.SKILL_59:
                                if (GateShare.boStartSpellCheck)
                                {
                                    if ((HUtil32.GetTickCount() - _gameSpeed.dwSpellTick) < GateShare.dwSpinEditSpellTime)
                                    {
                                        _gameSpeed.nErrorCount += GateShare.nIncErrorCount; // 每次加速的累加值
                                        if (_gameSpeed.nErrorCount >= GateShare.nSpinEditSpellCount)// 50
                                        {
                                            if (SessionInfo.boSendAvailable)
                                            {
                                                SessionInfo.dwClientCheckTimeOut = 3000;// 延迟间隔
                                            }
                                            else
                                            {
                                                SessionInfo.dwClientCheckTimeOut += GateShare.dwSpinEditSpellTime;// 延迟间隔
                                            }
                                            _gameSpeed.nErrorCount = 0; // 清除累计值
                                            result = GateShare.dwComboBoxSpellCheck; // 返回魔法加速处理
                                        }
                                    }
                                    else
                                    {
                                        if (_gameSpeed.nErrorCount >= GateShare.nDecErrorCount)
                                        {
                                            _gameSpeed.nErrorCount -= GateShare.nDecErrorCount;// 正常动作的减少值
                                        }
                                    }
                                    if (SessionInfo.sUserName == GateShare.boMsgUserName)
                                    {
                                        GateShare.AddMainLogMsg("[魔法间隔]：" + (HUtil32.GetTickCount() - _gameSpeed.dwSpellTick) + " 毫秒(" + (_gameSpeed.nErrorCount) + "/50)", 3); // 封包显示
                                    }
                                    _gameSpeed.dwSpellTick = HUtil32.GetTickCount(); // 保存魔法时间
                                    _gameSpeed.dwGameTick = HUtil32.GetTickCount(); // 在线时间
                                }
                                break;
                        }
                        break;
                    case Grobal2.CM_TURN:
                        // 转身     （只有停顿和延迟处理）
                        // ------------------------------------
                        // 原理:外挂利用挖地.自身不显示下蹲动作,而达到比正常玩家快1步.
                        // 每次保存上一次动作,判断挖地操作后,少下蹲动作,作为挖地暗杀处理.
                        // ------------------------------------
                        if (GateShare.boDarkHitCheck) // 封挖地暗杀
                        {
                            if (_gameSpeed.dwUserDoTick == Grobal2.CM_BUTCH)
                            {
                                result = 2;// 返回掉线处理
                                return result;
                            }
                        }
                        // ------------------------------------
                        if ((HUtil32.GetTickCount() - _gameSpeed.dwContinueTick) < 3000) // 停顿操作后3秒内数据全部抛出
                        {
                            result = 4;// 返回抛出数据
                            return result;
                        }
                        // ------------------------------------
                        if (GateShare.boStartTurnCheck)
                        {
                            if ((HUtil32.GetTickCount() - _gameSpeed.dwTurnTick) < GateShare.dwSpinEditTurnTime)
                            {
                                if (SessionInfo.boSendAvailable)
                                {
                                    SessionInfo.dwClientCheckTimeOut = 200;// 延迟间隔
                                }
                                else
                                {
                                    SessionInfo.dwClientCheckTimeOut += GateShare.dwSpinEditTurnTime;// 延迟间隔
                                }
                                _gameSpeed.dwSuperNeverTick = HUtil32.GetTickCount() - 300;// 保存超级加速时间
                                result = GateShare.dwComboBoxTurnCheck + 10;// 返回转身加速处理
                            }
                            if (SessionInfo.sUserName == GateShare.boMsgUserName)
                            {
                                GateShare.AddMainLogMsg("[转身间隔]：" + (HUtil32.GetTickCount() - _gameSpeed.dwTurnTick) + " 毫秒(" + (_gameSpeed.nErrorCount) + "/50)", 3);// 封包显示
                            }
                            _gameSpeed.dwTurnTick = HUtil32.GetTickCount();// 保存转身时间
                            _gameSpeed.dwGameTick = HUtil32.GetTickCount();// 在线时间
                        }
                        break;
                    case Grobal2.CM_DROPITEM:// 扔东西
                        break;
                    case Grobal2.CM_PICKUP: // 捡东西   （与转身控制相连）
                        if (GateShare.boStartTurnCheck)
                        {
                            if ((HUtil32.GetTickCount() - _gameSpeed.dwPickupTick) < GateShare.dwSpinEditPickupTime)
                            {
                                if (SessionInfo.boSendAvailable)
                                {
                                    SessionInfo.dwClientCheckTimeOut = 100;// 延迟间隔
                                }
                                else
                                {
                                    SessionInfo.dwClientCheckTimeOut += GateShare.dwSpinEditPickupTime;// 延迟间隔
                                }
                                result = GateShare.dwComboBoxTurnCheck + 10; // 返回转身加速处理
                            }
                            if (SessionInfo.sUserName == GateShare.boMsgUserName)
                            {
                                GateShare.AddMainLogMsg("[捡起间隔]：" + (HUtil32.GetTickCount() - _gameSpeed.dwPickupTick) + " 毫秒(" + (_gameSpeed.nErrorCount) + "/50)", 3); // 封包显示
                            }
                            _gameSpeed.dwPickupTick = HUtil32.GetTickCount(); // 保存捡起时间
                            _gameSpeed.dwGameTick = HUtil32.GetTickCount(); // 在线时间
                        }
                        break;
                    case Grobal2.CM_BUTCH:// 挖肉    （只有停顿和延迟处理）
                        if (GateShare.boStartButchCheck)
                        {
                            if ((HUtil32.GetTickCount() - _gameSpeed.dwButchTick) < GateShare.dwSpinEditButchTime)
                            {
                                if (SessionInfo.boSendAvailable)
                                {
                                    SessionInfo.dwClientCheckTimeOut = 200;// 延迟间隔
                                }
                                else
                                {
                                    SessionInfo.dwClientCheckTimeOut += GateShare.dwSpinEditButchTime;// 延迟间隔
                                }
                                result = GateShare.dwComboBoxButchCheck + 10; // 返回挖肉加速处理
                            }
                            if (SessionInfo.sUserName == GateShare.boMsgUserName)
                            {
                                GateShare.AddMainLogMsg("[挖肉间隔]：" + (HUtil32.GetTickCount() - _gameSpeed.dwButchTick) + " 毫秒(" + (_gameSpeed.nErrorCount) + "/50)", 3);// 封包显示
                            }
                            _gameSpeed.dwButchTick = HUtil32.GetTickCount();// 保存挖肉时间
                            _gameSpeed.dwGameTick = HUtil32.GetTickCount();// 在线时间
                        }
                        _gameSpeed.dwUserDoTick = 1007;
                        break;
                    case Grobal2.CM_SITDOWN:// 记录本次操作
                        _gameSpeed.dwUserDoTick = 3012;// 挖(蹲下)
                        _gameSpeed.dwGameTick = HUtil32.GetTickCount();// 记录本次操作
                        break;
                    case Grobal2.CM_EAT:
                        // 在线时间
                        // 吃药     （只有停顿和延迟处理）
                        // ------------------------------------
                        // 原理：所有可双击物品，全部视为吃药动作，
                        // 判断两次间隔超速后，可采取抛出数据包或延迟数据包执行。
                        // 使用抛出数据包会将导致，回程卷等物品也无法使用，和出现卡药现象。
                        // 使用延迟处理，未做测试，封顶药挂效果。 新添加的方案~
                        // ------------------------------------
                        if ((HUtil32.GetTickCount() - _gameSpeed.dwContinueTick) < 3000)// 停顿操作后3秒内数据全部抛出
                        {
                            result = 4;// 返回抛出数据
                            return result;
                        }
                        // ------------------------------------
                        if (GateShare.boStartEatCheck)
                        {
                            if ((HUtil32.GetTickCount() - _gameSpeed.dwEatTick) < GateShare.dwSpinEditEatTime)
                            {
                                if (SessionInfo.boSendAvailable)
                                {
                                    // 延迟间隔
                                    SessionInfo.dwClientCheckTimeOut = 100;
                                }
                                else
                                {
                                    SessionInfo.dwClientCheckTimeOut += GateShare.dwSpinEditEatTime;
                                }
                                // 延迟间隔
                                result = GateShare.dwComboBoxEatCheck + 10;
                                // 返回吃药加速处理
                            }
                            if (SessionInfo.sUserName == GateShare.boMsgUserName)
                            {
                                GateShare.AddMainLogMsg("[吃药间隔]：" + (HUtil32.GetTickCount() - _gameSpeed.dwEatTick) + " 毫秒(" + (_gameSpeed.nErrorCount) + "/50)", 3);
                            }
                            // 封包显示
                            _gameSpeed.dwEatTick = HUtil32.GetTickCount();// 保存吃药时间
                            _gameSpeed.dwGameTick = HUtil32.GetTickCount();// 在线时间
                        }
                        break;
                    //case Grobal2.CM_15999:
                    //    // 变速齿轮
                    //    // ------------------------------------
                    //    // 原理：SKY客户端登陆游戏后，每1分钟向游戏网关发送一次CM_15999进行验证
                    //    // 在这里只需要判断两次间隔小于50秒，作为客户端已被加速处理。
                    //    // 为了减少误封，判断加速连续2次以上，执行掉线处理。
                    //    // ------------------------------------
                    //    if (GateShare.boSupSpeederCheck)
                    //    {
                    //        if ((HUtil32.GetTickCount() - GameSpeed.dwSupSpeederTick) < 50000)
                    //        {
                    //            if (GameSpeed.dwSupSpeederCount > 1)
                    //            {
                    //                GameSpeed.dwSupSpeederCount = 0; // 清零
                    //                result = 2; // 返回掉线处理
                    //                return result;
                    //            }
                    //            else
                    //            {
                    //                GameSpeed.dwSupSpeederCount++; // 变速齿轮累计
                    //            }
                    //        }
                    //        GameSpeed.dwSupSpeederTick = HUtil32.GetTickCount(); // 保存变速齿轮时间
                    //        GameSpeed.dwGameTick = HUtil32.GetTickCount(); // 在线时间
                    //    }
                    //    result = 4; // 抛出处理，解决SKY登陆器 1分钟一卡问题。
                    //    return result;
                    case Grobal2.CM_SAY:// 说话
                        sDataText = "";
                        if ((HUtil32.GetTickCount() - SessionInfo.dwSayMsgTick) < GateShare.dwSayMsgTime) // 控制发言间隔时间
                        {
                            SendWarnMsg(SessionInfo, GateShare.gWarningMsg, GateShare.btRedMsgFColor, GateShare.btRedMsgBColor);// 提示文字警告
                            result = 4;
                            return result;
                        }
                        sDataText = EDcode.DeCodeString(sMsg);// 解密
                        if (sDataText != "")
                        {
                            if (sDataText[0] == '/')
                            {
                                sDataText = HUtil32.GetValidStr3(sDataText, ref sHumName, new string[] { " " });// 限制最长可发字符长度
                                if (sDataText.Length > GateShare.nSayMsgMaxLen)
                                {
                                    sDataText = sDataText.Substring(1 - 1, GateShare.nSayMsgMaxLen);
                                }
                                FilterSayMsg(ref sDataText);// 过滤文字
                                if (sDataText == "掉线处理")
                                {
                                    result = 2;
                                    return result;
                                }
                                if (sDataText == "丢包处理")
                                {
                                    result = 4;
                                    return result;
                                }
                                sDataText = sHumName + " " + sDataText;
                            }
                            else
                            {
                                if(sDataText[0]=='@') //对游戏命令放行
                                {
                                    result = -1;
                                    return result;
                                }
                                if (sDataText[0] != '@')
                                {
                                    // 限制最长可发字符长度
                                    if (sDataText.Length > GateShare.nSayMsgMaxLen)
                                    {
                                        sDataText = sDataText.Substring(1 - 1, GateShare.nSayMsgMaxLen);
                                    }
                                    FilterSayMsg(ref sDataText);// 过滤文字
                                    if (sDataText == "掉线处理")
                                    {
                                        result = 2;
                                        return result;
                                    }
                                    if (sDataText == "丢包处理")
                                    {
                                        result = 4;
                                        return result;
                                    }
                                }
                                else
                                {
                                    if (sDataText.IndexOf(GateShare.sModeFilter, StringComparison.OrdinalIgnoreCase) != 0)
                                    {
                                        if (GateShare.boFeiDnItemsCheck)// 封掉飞装备
                                        {
                                            if ((HUtil32.GetTickCount() - _gameSpeed.dwFeiDnItemsTick) < GateShare.dwSayMoveTime)
                                            {
                                                SendWarnMsg(SessionInfo, GateShare.fWarningMsg, GateShare.btYYMsgFColor, GateShare.btYYMsgBColor); // 提示文字警告
                                                result = 4; // 返回抛出数据处理
                                                return result;
                                            }
                                        }
                                        _gameSpeed.dwFeiDnItemsTick = HUtil32.GetTickCount(); // 保存传送时间
                                    }
                                    else
                                    {
                                        if (sDataText.IndexOf(GateShare.sMsgFilter, StringComparison.OrdinalIgnoreCase) != 0)// 控制发言间隔时间
                                        {
                                            if ((HUtil32.GetTickCount() - SessionInfo.dwSayMsgTick) < GateShare.TrinidadMsgTime)
                                            {
                                                SendWarnMsg(SessionInfo, GateShare.gWarningMsg, GateShare.btRedMsgFColor, GateShare.btRedMsgBColor); // 提示文字警告
                                                result = 4;
                                                return result;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        sMsg = EDcode.EncodeString(sDataText);
                        _gameSpeed.dwGameTick = HUtil32.GetTickCount(); // 在线时间
                        SessionInfo.dwSayMsgTick = HUtil32.GetTickCount();// 保存发言时间
                        break;
                }
            }
            catch
            {
                GateShare.AddMainLogMsg("[异常] TFrmMain.CheckDefMsg", 1);
            }
            return result;
        }

        /// <summary>
        /// 发送警告文字
        /// </summary>
        private void SendWarnMsg(TSessionInfo SessionInfo, string sMsg, byte FColor, byte BColor)
        {
            TDefaultMessage DefMsg;
            string sSendText;
            try
            {
                if ((SessionInfo == null))
                {
                    return;
                }
                if ((SessionInfo.Socket == null))
                {
                    return;
                }
                if (SessionInfo.Socket.Connected)
                {
                    DefMsg = Grobal2.MakeDefaultMsg(103, (int)(SessionInfo.Socket.Handle), HUtil32.MakeWord(FColor, BColor), 0, 1);
                    sSendText = "#" + EDcode.EncodeMessage(DefMsg) + EDcode.EncodeString(sMsg) + "!";
                    SessionInfo.Socket.SendText(sSendText);
                }
            }
            catch
            {
                GateShare.AddMainLogMsg("[异常] TFrmMain.SendWarnMsg", 1);
            }
        }

        /// <summary>
        /// 文字消息处理(过滤，已经发言间隔)
        /// </summary>
        /// <param name="sMsg"></param>
        private void FilterSayMsg(ref string sMsg)
        {
            int nLen;
            string sReplaceText;
            string sFilterText;
            try
            {
                HUtil32.EnterCriticalSection(GateShare.CS_FilterMsg);
                for (var i = 0; i < GateShare.AbuseList.Count; i++)
                {
                    sFilterText = GateShare.AbuseList[i];
                    sReplaceText = "";
                    if (sMsg.IndexOf(sFilterText, StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        for (nLen = 0; nLen <= sFilterText.Length; nLen++)
                        {
                            sReplaceText = sReplaceText + GateShare.sReplaceWord;
                        }
                        sMsg = sMsg.Replace(sFilterText, sReplaceText);
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(GateShare.CS_FilterMsg);
            }
        }

        private void Send(ushort nIdent, int wSocketIndex, int nSocket, int nUserListIndex, int nLen, byte[] dataBuff)
        {
            GateShare.ForwardMsgList.Writer.TryWrite(new ForwardMessage()
            {
                nIdent = nIdent,
                wSocketIndex = wSocketIndex,
                nSocket = nSocket,
                nUserListIndex = nUserListIndex,
                nLen = nLen,
                Data = dataBuff
            });
        }

        // private int CheckDefMsg(TDefaultMessage defMsg, out string sMsg)
        // {
        //     switch (defMsg.Ident)
        //     {
        //         //原理:外挂利用挖地.自身不显示下蹲动作,而达到比正常玩家快1步.
        //         //每次保存上一次动作,判断挖地操作后,少下蹲动作,作为挖地暗杀处理.
        //         case Grobal2.CM_WALK:
        //         case Grobal2.CM_RUN:
        //             if (GateShare.g_pConfig.m_fMoveInterval)
        //             {
        //                 fPacketOverSpeed = false;
        //                 dwCurrentTick = HUtil32.GetTickCount();
        //                 if (m_fSpeedLimit)
        //                 {
        //                     nMoveInterval = GateShare.g_pConfig.m_nMoveInterval +
        //                                     GateShare.g_pConfig.m_nPunishMoveInterval;
        //                 }
        //                 else
        //                 {
        //                     nMoveInterval = GateShare.g_pConfig.m_nMoveInterval;
        //                 }
        //
        //                 nInterval = dwCurrentTick - m_dwMoveTick;
        //                 if (nInterval >= nMoveInterval)
        //                 {
        //                     m_dwMoveTick = dwCurrentTick;
        //                     m_dwSpellTick = dwCurrentTick - GateShare.g_pConfig.m_nMoveNextSpellCompensate;
        //                     if (m_dwAttackTick < dwCurrentTick - GateShare.g_pConfig.m_nMoveNextAttackCompensate)
        //                     {
        //                         m_dwAttackTick = dwCurrentTick - GateShare.g_pConfig.m_nMoveNextAttackCompensate;
        //                     }
        //
        //                     m_dwLastDirection = CltCmd.Dir;
        //                 }
        //                 else
        //                 {
        //                     fPacketOverSpeed = true;
        //                     if (GateShare.g_pConfig.m_tOverSpeedPunishMethod == TPunishMethod.ptDelaySend)
        //                     {
        //                         nMsgCount = GetDelayMsgCount();
        //                         if (nMsgCount == 0)
        //                         {
        //                             dwDelay = GateShare.g_pConfig.m_nPunishBaseInterval +
        //                                       (int)Math.Round((nMoveInterval - nInterval) *
        //                                                       GateShare.g_pConfig.m_nPunishIntervalRate);
        //                             m_dwMoveTick = dwCurrentTick + dwDelay;
        //                         }
        //                         else
        //                         {
        //                             m_dwMoveTick = dwCurrentTick + (nMoveInterval - nInterval);
        //                             if (nMsgCount >= 2)
        //                             {
        //                                 SendKickMsg(0);
        //                             }
        //
        //                             break;
        //                         }
        //                     }
        //                 }
        //             }
        //
        //             break;
        //
        //         case Grobal2.CM_HIT: //攻击
        //         case Grobal2.CM_HEAVYHIT:
        //         case Grobal2.CM_BIGHIT:
        //         case Grobal2.CM_POWERHIT:
        //         case Grobal2.CM_LONGHIT:
        //         case Grobal2.CM_WIDEHIT:
        //         case Grobal2.CM_CRSHIT:
        //         case Grobal2.CM_FIREHIT:
        //             if (GateShare.g_pConfig.m_fAttackInterval)
        //             {
        //                 fPacketOverSpeed = false;
        //                 dwCurrentTick = HUtil32.GetTickCount();
        //                 if (m_fSpeedLimit)
        //                 {
        //                     nAttackInterval = GateShare.g_pConfig.m_nAttackInterval +
        //                                       GateShare.g_pConfig.m_nPunishAttackInterval;
        //                 }
        //                 else
        //                 {
        //                     nAttackInterval = GateShare.g_pConfig.m_nAttackInterval;
        //                 }
        //
        //                 nAttackFixInterval = HUtil32._MAX(0,
        //                     (nAttackInterval - GateShare.g_pConfig.m_nMaxItemSpeedRate * m_nItemSpeed));
        //                 nInterval = dwCurrentTick - m_dwAttackTick;
        //                 if (nInterval >= nAttackFixInterval)
        //                 {
        //                     m_dwAttackTick = dwCurrentTick;
        //                     if (GateShare.g_pConfig.m_fItemSpeedCompensate)
        //                     {
        //                         m_dwMoveTick = dwCurrentTick - (GateShare.g_pConfig.m_nAttackNextMoveCompensate +
        //                                                         GateShare.g_pConfig.m_nMaxItemSpeedRate *
        //                                                         m_nItemSpeed); //550
        //                         m_dwSpellTick = dwCurrentTick - (GateShare.g_pConfig.m_nAttackNextSpellCompensate +
        //                                                          GateShare.g_pConfig.m_nMaxItemSpeedRate *
        //                                                          m_nItemSpeed); //1150
        //                     }
        //                     else
        //                     {
        //                         m_dwMoveTick = dwCurrentTick - GateShare.g_pConfig.m_nAttackNextMoveCompensate; //550
        //                         m_dwSpellTick = dwCurrentTick - GateShare.g_pConfig.m_nAttackNextSpellCompensate; //1150
        //                     }
        //                 }
        //                 else
        //                 {
        //                     fPacketOverSpeed = true;
        //                     if (GateShare.g_pConfig.m_tOverSpeedPunishMethod == TPunishMethod.ptDelaySend)
        //                     {
        //                         nMsgCount = GetDelayMsgCount();
        //                         if (nMsgCount == 0)
        //                         {
        //                             dwDelay = GateShare.g_pConfig.m_nPunishBaseInterval +
        //                                       (int)Math.Round((nAttackFixInterval - nInterval) *
        //                                                       GateShare.g_pConfig.m_nPunishIntervalRate);
        //                             m_dwAttackTick = dwCurrentTick + dwDelay;
        //                         }
        //                         else
        //                         {
        //                             m_dwAttackTick = dwCurrentTick + (nAttackFixInterval - nInterval);
        //                             if (nMsgCount >= 2)
        //                             {
        //                                 SendKickMsg(0);
        //                             }
        //
        //                             break;
        //                         }
        //                     }
        //                 }
        //             }
        //
        //             break;
        //         case Grobal2.CM_SPELL: //使用技能
        //             if (GateShare.g_pConfig.m_fSpellInterval) //1380
        //             {
        //                 dwCurrentTick = HUtil32.GetTickCount();
        //             }
        //
        //             break;
        //         case Grobal2.CM_EAT: //使用物品
        //
        //             break;
        //         case Grobal2.CM_SAY: // 控制发言间隔时间
        //         {
        //             sDataText = EDcode.DeCodeString(sDataMsg);
        //             if (sDataText != "")
        //             {
        //                 if (sDataText[0] == '/')
        //                 {
        //                     sDataText = HUtil32.GetValidStr3(sDataText, ref sHumName,
        //                         new string[] { " " }); // 限制最长可发字符长度
        //                     FilterSayMsg(ref sDataText);
        //                     sDataText = sHumName + " " + sDataText;
        //                 }
        //                 else
        //                 {
        //                     if (sDataText[0] != '@')
        //                     {
        //                         FilterSayMsg(ref sDataText); // 限制最长可发字符长度
        //                     }
        //                 }
        //             }
        //
        //             sDataMsg = EDcode.EncodeString(sDataText);
        //             break;
        //         }
        //     }
    }


    public enum TCheckStep
    {
        csCheckLogin,
        csSendCheck,
        csSendSmu,
        csSendFinsh,
        csCheckTick
    }

    public class GameSpeed
    {
        public int nErrorCount;// 加速的累计值
        public int m_nHitSpeed;// 装备加速
        public long dwSayMsgTick;// 发言时间
        public long dwHitTick;// 攻击时间
        public long dwSpellTick;// 魔法时间
        public long dwWalkTick;// 走路时间
        public long dwRunTick;// 跑步时间
        public long dwTurnTick;// 转身时间
        public long dwButchTick;// 挖肉时间
        public long dwEatTick;// 吃药时间
        public long dwPickupTick;// 捡起时间
        public long dwRunWalkTick;// 移动时间
        public long dwFeiDnItemsTick;// 传送时间
        public long dwSupSpeederTick;// 变速齿轮时间
        public int dwSupSpeederCount;// 变速齿轮累计
        public long dwSuperNeverTick;// 超级加速时间
        public int dwSuperNeverCount;// 超级加速累计
        public int dwUserDoTick;// 记录上一次操作
        public long dwContinueTick;// 保存停顿操作时间
        public int dwConHitMaxCount;// 带有攻击并发累计
        public int dwConSpellMaxCount;// 带有魔法并发累计
        public int dwCombinationTick;// 记录上一次移动方向
        public int dwCombinationCount;// 智能攻击累计
        public long dwGameTick;

        public GameSpeed()
        {
            
        }
    }
}