
namespace M2Server
{
    public class EncryptUnit
    {
        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="str">密文</param>
        /// <param name="chinese">是否返回中文</param>
        /// <returns></returns>
        public static unsafe string DeCodeString(string str, bool chinese)
        {
            var result = string.Empty;
            var EncBuf = new byte[grobal2.BUFFERSIZE];
            var bSrc = HUtil32.StringToByteAry(str);
            var nLen = EDcode.Decode6BitBuf(bSrc, EncBuf, bSrc.Length, grobal2.BUFFERSIZE);
            fixed (byte* pb = EncBuf)
            {
                if (chinese)
                {
                    result = HUtil32.SBytePtrToString((sbyte*)pb, nLen);
                }
                else
                {
                    result = HUtil32.SBytePtrToString((sbyte*)pb, 0, nLen);
                }
            }
            return result;
        }

        public void GetIdeSerialNumber_ChangeByteOrder(ref object Data, int Size)
        {
            //string ptr;
            //int i;
            //char c;
            //ptr = Data;
            //for (i = 0; i < (Size >> 1); i ++ )
            //{
            //    c = ptr;
            //    ptr = (ptr + 1);
            //    (ptr + 1) = c;
            //    ptr += 2;
            //}
        }

        public static string GetIdeSerialNumber()
        {
            //string result;
            //const int IDENTIFY_BUFFER_SIZE = 512;
            //int hDevice;
            //double cbBytesReturned;
            //string ptr;
            //TSendCmdInParams SCIP;
            //byte[] aIdOutCmd = new byte[sizeof(TSendCmdOutParams) + IDENTIFY_BUFFER_SIZE - 1 - 1 + 1];
            //TSendCmdOutParams IdOutCmd;
            //result = "";
            //if (Win32Platform == VER_PLATFORM_WIN32_NT)
            //{
            //    hDevice = CreateFile("\\\\.\\PhysicalDrive0", GENERIC_READ || GENERIC_WRITE, FILE_SHARE_READ || FILE_SHARE_WRITE, null, OPEN_EXISTING, 0, 0);
            //}
            //else
            //{
            //    hDevice = CreateFile("\\\\.\\SMARTVSD", 0, 0, null, CREATE_NEW, 0, 0);
            //}
            
            //if (hDevice == INVALID_HANDLE_VALUE)
            //{
            //    return result;
            //}
            //try {
            //    FillChar(SCIP, sizeof(TSendCmdInParams) - 1, '\0');
            //    FillChar(aIdOutCmd, sizeof(aIdOutCmd), '\0');
            //    cbBytesReturned = 0;
            //    // Set up data structures for IDENTIFY command.
            //    SCIP.cBufferSize = IDENTIFY_BUFFER_SIZE;
            //    // bDriveNumber := 0;
            //    TIDERegs _wvar1 = SCIP.irDriveRegs;
            //    _wvar1.bSectorCountReg = 1;
            //    _wvar1.bSectorNumberReg = 1;

            //    _wvar1.bDriveHeadReg = 0xA0;
            //    _wvar1.bCommandReg = 0xEC;
                
            //    if (!DeviceIoControl(hDevice, 0x0007c088, SCIP, sizeof(TSendCmdInParams) - 1, aIdOutCmd, sizeof(aIdOutCmd), cbBytesReturned, null))
            //    {
            //        return result;
            //    }
            //} finally {
            //    CloseHandle(hDevice);
            //}
            //TIdSector _wvar2 = ((IdOutCmd.bBuffer) as TIdSector);
            //GetIdeSerialNumber_ChangeByteOrder(ref _wvar2.sSerialNumber, sizeof(_wvar2.sSerialNumber));
            //((_wvar2.sSerialNumber as string) + sizeof(_wvar2.sSerialNumber)) = '\0';
            //result = (_wvar2.sSerialNumber as string);
            return string.Empty;
        }
    }
}

