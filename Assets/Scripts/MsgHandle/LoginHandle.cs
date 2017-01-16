using System.IO;
using System.Text;
using System.Collections.Generic;
using ProtoBuf;

public static class LoginHandle
{
    public static bool ParseLoginLoginRet(byte[] msg_, int msgLen_)
    {
        MemoryStream ms = new MemoryStream(msg_, 0, msgLen_);
        Cmd.LoginRet rcv = Serializer.Deserialize<Cmd.LoginRet>(ms);
        Utility.Log("Recv gateway server addr:[" + rcv.gatewayip + ":" + rcv.gatewayport + "]"); 

        NetController.Instance.LoginToGateServer(rcv.gatewayip, (int)rcv.gatewayport,  Encoding.UTF8.GetString(rcv.account), rcv.tempid);
        NetController.Instance.DestroyLoginClient();
        return true;
    }

    public static bool ParseLoginGatewayRet(byte[] msg_, int msgLen_)
    {
        Utility.Log("Login gateway server return ok!");
        return true;
    }

    public static bool ParseLoginCrossRet(byte[] msg_, int msgLen_)
    {
        return true;
    }
}
