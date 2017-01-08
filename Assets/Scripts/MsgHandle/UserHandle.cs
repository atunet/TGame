using System;
using System.IO;
using System.Text;
using ProtoBuf;

public static class UserHandle
{
  //  private static MemoryStream s_sendStream = new MemoryStream(64*1024);
  //  private static MemoryStream s_recvStream = new MemoryStream(64*1024);

    public static bool ParseUserList(byte[] msg_, int msgLen_)
    {
        MemoryStream ms = new MemoryStream(msg_, 0, msgLen_);
        Cmd.UserList rcv = Serializer.Deserialize<Cmd.UserList>(ms);

        if (rcv.userbase.Count > 0)      
        {
            Cmd.SelectUserOnline req = new Cmd.SelectUserOnline();
            req.userid = rcv.userbase[0].userid;

            MemoryStream ms2 = new MemoryStream();
            Serializer.Serialize<Cmd.SelectUserOnline>(ms2, req);
            NetController.Instance.SendMsgToGate(req.id, ms2.ToArray());

            Utility.Log("Select user online:" + req.userid);
        }
        else
        {
            Cmd.CreateUserReq req = new Cmd.CreateUserReq();
            Random nameRandom = new Random();
            req.username = Encoding.UTF8.GetBytes("abc_" + nameRandom.Next(1, 999999));
            req.usertype = 1212121;

            MemoryStream ms2 = new MemoryStream();
            Serializer.Serialize<Cmd.CreateUserReq>(ms2, req);
            NetController.Instance.SendMsgToGate(req.id, ms2.ToArray());

            Utility.Log("Create user:" + req.username + ",type:" + req.usertype);
        }

        return true;
    }

    public static bool ParseCreateUserRet(byte[] msg_, int msgLen_)
    {
        MemoryStream ms = new MemoryStream(msg_, 0, msgLen_);
        Cmd.CreateUserRet rcv = Serializer.Deserialize<Cmd.CreateUserRet>(ms);

        Cmd.SelectUserOnline req = new Cmd.SelectUserOnline();
        req.userid = rcv.userbase.userid;
        MemoryStream ms2 = new MemoryStream();
        Serializer.Serialize<Cmd.SelectUserOnline>(ms2, req);
        NetController.Instance.SendMsgToGate(req.id, ms2.ToArray());

        Utility.Log("Select user online:" + req.userid);
        return true;
    }

    public static bool ParseUserBaseData(byte[] msg_, int msgLen_)
    {
        MemoryStream ms = new MemoryStream(msg_, 0, msgLen_);
        Cmd.SendUserBaseData rcv = Serializer.Deserialize<Cmd.SendUserBaseData>(ms);

        Utility.Log("recv user base data,user:" + rcv.info.userid + ",name:" + rcv.info.username);
        return true;
    }

    public static bool ParseLoadOk(byte[] msg_, int msgLen_)
    {
        Utility.Log("user online load data ok");

        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene", UnityEngine.SceneManagement.LoadSceneMode.Single);

        return true;
    }
}
