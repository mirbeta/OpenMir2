using System;
using SystemModule;
using SystemModule.Packages;

namespace RunGate
{
    /// <summary>
    /// 用户会话封包处理
    /// </summary>
    public class UserClientSession
    {
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
                                            if (!string.IsNullOrEmpty(sDataMsg))
                                            {
                                                switch (DefMsg.Ident)
                                                {
                                                    case Grobal2.CM_SPELL://使用技能
                                                        //检查技能是否超速
                                                        
                                                        break;
                                                    case Grobal2.CM_EAT: //使用物品
                                                        // var dwTime = HUtil32.GetTickCount();
                                                        // if (dwTime - LastEat > dwEatTime)
                                                        // {
                                                        //     LastEat = dwTime;
                                                        // }
                                                        // else
                                                        // {
                                                        //    GateShare.AddMainLogMsg(string.Format("超速封包(药品):{0}",[dwTime - LastEat]), 1);
                                                        // }
                                                        break;
                                                    case Grobal2.CM_SAY: // 控制发言间隔时间
                                                    {
                                                        sDataText = EDcode.DeCodeString(sDataMsg);
                                                        if (sDataText != "")
                                                        {
                                                            if (sDataText[0] == '/')
                                                            {
                                                                sDataText = HUtil32.GetValidStr3(sDataText, ref sHumName, new string[] { " " }); // 限制最长可发字符长度
                                                                FilterSayMsg(ref sDataText);
                                                                sDataText = sHumName + " " + sDataText;
                                                            }
                                                            else
                                                            {
                                                                if (sDataText[0] != '@')
                                                                {
                                                                    FilterSayMsg(ref sDataText);// 限制最长可发字符长度
                                                                }
                                                            }
                                                        }
                                                        sDataMsg = EDcode.EncodeString(sDataText);
                                                        break;
                                                    }
                                                }

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

        /// <summary>
        /// 文字消息处理(过滤，已经发言间隔)
        /// </summary>
        /// <param name="sMsg"></param>
        private void FilterSayMsg(ref string sMsg)
        {
            
        }

        private void Send(ushort nIdent, int wSocketIndex, int nSocket, int nUserListIndex, int nLen, byte[] dataBuff)
        {
            GateShare.ForwardMsgList.Writer.TryWrite(new ForwardMessage()
            {
                nIdent = Grobal2.GM_DATA,
                wSocketIndex = wSocketIndex,
                nSocket = nSocket,
                nUserListIndex =   nUserListIndex,
                nLen = nLen,
                Data = dataBuff
            });
        }
    }
}