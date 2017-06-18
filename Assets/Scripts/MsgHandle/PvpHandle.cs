using System.IO;
using ProtoBuf;
using UnityEngine;

public static class PvpHandle
{
    public static bool ParseMatchRet(byte[] msg_, int msgLen_)
    {
        MemoryStream ms = new MemoryStream(msg_, 0, msgLen_);
        Cmd.PvpMatchRet rcv = Serializer.Deserialize<Cmd.PvpMatchRet>(ms);

        Utility.Log("join pvp match ok"); 
        return true;
    }

    public static bool ParseMatchCancelRet(byte[] msg_, int msgLen_)
    {
        MemoryStream ms = new MemoryStream(msg_, 0, msgLen_);
        Cmd.PvpMatchCancelRet rcv = Serializer.Deserialize<Cmd.PvpMatchCancelRet>(ms);
       
        Utility.Log("cancel pvp match ok"); 
        return true;
    }

    public static bool ParseBattleInitInfoNtf(byte[] msg_, int msgLen_)
    {
        MemoryStream ms = new MemoryStream(msg_, 0, msgLen_);
        Cmd.BattleInitInfo rcv = Serializer.Deserialize<Cmd.BattleInitInfo>(ms);

        Utility.Log("pvp battle init info [row:" + rcv.row + ",col:" + rcv.column + "],(" +
            rcv.uid1 + "," + rcv.name1 + ":" + rcv.uid2 + "," + rcv.name2 + ")"); 

        InitBattleController.s_mineRow = (byte)rcv.row;
        InitBattleController.s_mineColumn = (byte)rcv.column;
        InitBattleController.s_mines = rcv.mines;
        InitBattleController.s_allReady = false;

        Utility.LoadingScene("BattleScene");
        return true;
    }

    public static bool ParseBattleAllReadyNtf(byte[] msg_, int msgLen_)
    {
        MemoryStream ms = new MemoryStream(msg_, 0, msgLen_);
        Cmd.BattleAllReadyNtf rcv = Serializer.Deserialize<Cmd.BattleAllReadyNtf>(ms);
        InitBattleController.s_allReady = true;

        Utility.Log("battle all ready recv"); 
        return true;
    }

    public static bool ParseBattleOpenMineRet(byte[] msg_, int msgLen_)
    {
        MemoryStream ms = new MemoryStream(msg_, 0, msgLen_);
        Cmd.BattleOpenMineRet rcv = Serializer.Deserialize<Cmd.BattleOpenMineRet>(ms);

        Utility.Log("battle open mine ret"); 
        return true;
    }

    public static bool ParseBattleOpenMineNtf(byte[] msg_, int msgLen_)
    {
        MemoryStream ms = new MemoryStream(msg_, 0, msgLen_);
        Cmd.BattleOpenMineNtf rcv = Serializer.Deserialize<Cmd.BattleOpenMineNtf>(ms);

        string mineName = "mine_" + rcv.row + "_" + rcv.column;
        Transform mineTrans = GlobalRef.SceneRoot.FindChild(mineName);
        if (null == mineTrans)
        {
            Utility.LogWarning("open mine not found (" + rcv.row + ":" + rcv.column + ")");
            return true;
        }

        MineController mController = mineTrans.gameObject.GetComponent<MineController>();
        mController.m_cover.gameObject.SetActive(false);
        mController.Opened = true;

        Utility.Log("battle open mine ntf (" + rcv.row + ":" + rcv.column + ")"); 
        return true;
    }

    public static bool ParseBattleResult(byte[] msg_, int msgLen_)
    {
        MemoryStream ms = new MemoryStream(msg_, 0, msgLen_);
        Cmd.BattleResult rcv = Serializer.Deserialize<Cmd.BattleResult>(ms);

        Utility.Log("battle result recv"); 
        return true;
    }
}
