using System.Collections.Generic;
using Cmd;

public static class HandleMgr
{
    public delegate bool MsgHandle(byte[] msg_, int msgLen_);
    private static Dictionary<EMessageID, MsgHandle> s_msgDict = new Dictionary<EMessageID, MsgHandle>();
	
    public static MsgHandle getHandle(EMessageID msgId_)
    {
        MsgHandle handle = null;
        s_msgDict.TryGetValue(msgId_, out handle);
        return handle;
    }


    public static bool Init() 
    {
        s_msgDict[EMessageID.LOGIN_LOGIN_SC]        =   LoginHandle.ParseLoginLoginRet;
        s_msgDict[EMessageID.LOGIN_GATEW_SC]        =   LoginHandle.ParseLoginGatewayRet;
        s_msgDict[EMessageID.LOGIN_CROSS_SC]        =   LoginHandle.ParseLoginCrossRet;
        s_msgDict[EMessageID.USER_LIST_S]           =   UserHandle.ParseUserList;
        s_msgDict[EMessageID.USER_BASE_DATA_SC]     =   UserHandle.ParseUserBaseData;
        s_msgDict[EMessageID.CREATE_USER_SC]        =   UserHandle.ParseCreateUserRet;
        s_msgDict[EMessageID.DATA_LOAD_OK_S]        =   UserHandle.ParseLoadOk;

        return true;
	}
}
