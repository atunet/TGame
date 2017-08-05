using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using ProtoBuf;
using System.Collections.Generic;

public class InitBattleController : MonoBehaviour 
{
    public const byte MINE_WIDE = 64;   // piexl
    public const byte MINE_HIGH = 64;

    public static byte s_mineRow = 0;
    public static byte s_mineColumn = 0;
    public static List<uint> s_mines = null;
    public static bool s_allReady = false;


    public GameObject m_panelResult = null;


    void Start () 
    {
        if (!GlobalRef.Init())
            return;

        CreateCommonUI();

        // string[] abList = new string[]{"sprite_battle", AppConst.AB_BATTLE };
        string[] abList = { "sprite_battle", AppConst.AB_BATTLE };
        StartCoroutine (GlobalRef.AssetMgr.GetABList (abList, CreateBattleUI));
    }

    public void CreateCommonUI()
    {
        AssetBundle commonAB = GlobalRef.AssetMgr.TryGetAB(AppConst.AB_COMMON);
        GameObject reconnectPrefab = commonAB.LoadAsset("panel_reconnecting") as GameObject;
        if (null == reconnectPrefab)
        {
            Utility.LogError("reconnectprefab not found");
            return;
        }
        GameObject reconnectGo = GameObject.Instantiate(reconnectPrefab);
        reconnectGo.transform.SetParent(GlobalRef.UIRoot, false);
        reconnectGo.name = "panel_reconnecting";
        reconnectGo.SetActive(false);
    }

    public void CreateBattleUI(AssetBundle ab_)
    {
        GameObject battleuiPrefab = ab_.LoadAsset("panel_battle_ui") as GameObject;
        if (null == battleuiPrefab)
        {
            Utility.LogError("battleuiprefab not found");
            return;
        }
        GameObject battleuiGo = GameObject.Instantiate(battleuiPrefab);
        battleuiGo.transform.SetParent(GlobalRef.UIRoot, false);
        battleuiGo.name = "panel_battleui";
        //battleuiGo.SetActive(true);

        GameObject quitBtnGo = battleuiGo.transform.FindChild ("btn_pause").gameObject;
        Button quitBtn = quitBtnGo.GetComponent<Button> ();
        quitBtn.onClick.AddListener (OnQuitBattleClick);
    }


    public void OnQuitBattleClick()
    {
        Utility.Log("OnQuitBattleClicked");
        Utility.LoadingScene("MainScene");
    }


    public void InitMineLand()
    {
        int initX = 0 - (MINE_WIDE * s_mineRow / 2);
        int initY = 0 - (MINE_HIGH * s_mineColumn / 2);

        GameObject minePrefab = Resources.Load<GameObject>("MinePrefab");

        for (byte r = 0; r < s_mineRow; ++r)
        {
            for (byte c = 0; c < s_mineColumn; ++c)
            {
                GameObject mineGo = GameObject.Instantiate<GameObject>(minePrefab);
                mineGo.name = "mine_" + r + "_" + c;
                mineGo.transform.SetParent(GlobalRef.SceneRoot, false);
                mineGo.transform.localPosition = new Vector3(
                    initX + r * MINE_WIDE + MINE_WIDE / 2, 
                    initY + c * MINE_HIGH + MINE_HIGH / 2, 
                    0);
                MineController mController = mineGo.GetComponent<MineController>();
                mController.Row = r;
                mController.Column = c;
                if (s_mines[r * s_mineColumn + c] > 8)
                {
                    mController.IsMine = true;
                    mController.m_bomb.gameObject.SetActive(true);
                    mController.m_number.gameObject.SetActive(false);
                    mController.m_cover.gameObject.SetActive(true);
                }
                else
                {
                    mController.IsMine = false;
                    mController.m_bomb.gameObject.SetActive(false);
                    mController.m_number.gameObject.SetActive(true);
                    mController.m_cover.gameObject.SetActive(true);
                }
                mController.Opened = false;
            }
        }


        Cmd.BattleClientReady req = new Cmd.BattleClientReady();
        MemoryStream ms2 = new MemoryStream();
        Serializer.Serialize<Cmd.BattleClientReady>(ms2, req);
        NetMgr.Instance.SendMsgToGate(req.id, ms2.ToArray());
    }
}
