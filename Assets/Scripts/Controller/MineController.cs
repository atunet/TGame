using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ProtoBuf;
using System.IO;

public class MineController : MonoBehaviour {


    public Image m_bomb;
    public Image m_cover;
    public Text m_number;

    private byte m_row;
    private byte m_column;
    private bool m_isMine;
    private bool m_opened;

	// Use this for initialization
	void Start () {
        m_isMine = false;
        m_opened = false;
	}

    public byte Row
    {
        get { return m_row; }
        set { m_row = value; }
    }
	
    public byte Column
    {
        get { return Column; }
        set { Column = value; }
    }

    public bool IsMine
    {
        get { return m_isMine; }
        set { m_isMine = value; }
    }

    public bool Opened
    {
        get { return m_opened; }
        set { m_opened = value; }
    }

    public void OnMineClick()
    {
        if (!InitBattleController.s_allReady)
        {
            Utility.Log("Not all ready,please wait ...");
            return;
        }
        if (m_opened)
            return;

        Cmd.BattleOpenMineReq req = new Cmd.BattleOpenMineReq();
        req.row = m_row;
        req.column = m_column;

        MemoryStream ms2 = new MemoryStream();
        Serializer.Serialize<Cmd.BattleOpenMineReq>(ms2, req);
        NetController.Instance.SendMsgToGate(req.id, ms2.ToArray());
    }
}
