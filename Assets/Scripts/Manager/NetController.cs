using UnityEngine;

using System;
using System.Text;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ProtoBuf;

public class NetController : MonoBehaviour 
{ 
    private static NetController s_instance = null;
    public static NetController Instance
    {
        get
        {
            if (s_instance == null)
            {
                GameObject obj = new GameObject("NetController");
                s_instance = obj.AddComponent<NetController>(); 
                GameObject.DontDestroyOnLoad(obj);
            }            
            return s_instance;
        }
    }
		
    private string m_loginIP;
    private int m_loginPort;

    private string m_gateIP;
    private int m_gatePort;
    private string m_account;
    private uint m_zoneId;
    private UInt32 m_tempId;
    private DateTime m_lastSendGateTime = DateTime.Now;

    private string m_crossIP;
    private int m_crossPort;

	private NetThread m_thread = null;
    private ArrayList m_cmdList = new ArrayList();
    private MemoryStream m_pbStream = new MemoryStream();

    private GameObject m_reconnectingPanel = null;
    public GameObject ReconnectingUI
    {
        get { return m_reconnectingPanel; }
        set { m_reconnectingPanel = value; }
    }

    private NetController()
    {
        m_loginIP = "";
        m_loginPort = 0;
        m_gateIP = "";
        m_gatePort = 0;
        m_crossIP = "";
        m_crossPort = 0;

        m_account = "";
        m_zoneId = 0;
        m_tempId = 0;
    }

    public bool Init()
    {
        if (null != m_thread)
        {
            Utility.LogWarning("NetController already inited,repeated init ignored!!!");
            return false;
        }

        m_thread = new NetThread();
        m_thread.Start();

		m_reconnectingPanel = GlobalRef.UIRoot.FindChild("panel_reconnecting").gameObject;
        if (null == m_reconnectingPanel)
        {
            Utility.Log("NetController init,reconnectingpanel not found");
        }

        Utility.Log("NetController init ok,netthread start working!!!");
        return true;
    }
        
    public void LoginToLoginServer(string ip_, int port_, string account_, uint zoneId_)
    {
        m_loginIP = ip_;
        m_loginPort = port_;
        m_account = account_;
        m_zoneId = zoneId_;

        if (m_thread.InitLoginClient(ip_, port_))
        {           
            Cmd.VerifyVersion verify = new Cmd.VerifyVersion();
            verify.clientversion = 2017;
            Serializer.Serialize<Cmd.VerifyVersion>(m_pbStream, verify);
            SendMsgToLogin(verify.id, m_pbStream.ToArray());

            Cmd.LoginReq login = new Cmd.LoginReq();
            login.account = Encoding.UTF8.GetBytes(m_account);
            login.verifier = "this is verifier code";
            login.zoneid = m_zoneId;
            Serializer.Serialize<Cmd.LoginReq>(m_pbStream, login);
            SendMsgToLogin(login.id, m_pbStream.ToArray());
        }
        else
            Utility.LogError("login to login server failed (ip:" + ip_ + ":" + port_ + ")");
    }

    public void LoginToGateServer(string ip_, int port_, string account_, UInt32 tempId_)
    { 
        m_gateIP = ip_;
        m_gatePort = port_;
        m_account = account_;
        m_tempId = tempId_;

        if (m_thread.InitGateClient(ip_, port_))
        {
            Cmd.LoginGatewayReq login = new Cmd.LoginGatewayReq();
            login.account = Encoding.UTF8.GetBytes(m_account);
            login.tempid = m_tempId;
            Serializer.Serialize<Cmd.LoginGatewayReq>(m_pbStream, login);
            SendMsgToGate(login.id, m_pbStream.ToArray());          
        }
        else
            Utility.LogError("login to gate server failed (ip:" + ip_ + ":" + port_ + ")");
    }

    public void LoginToCrossServer(string ip_, int port_, UInt64 uid_, UInt32 tempId_)
    {        
        if (m_thread.InitCrossClient(ip_, port_))
        {
            m_crossIP = ip_;
            m_crossPort = port_;

            Cmd.LoginCrossReq login = new Cmd.LoginCrossReq();
            login.userid = uid_;
            login.tempid = tempId_;
            Serializer.Serialize<Cmd.LoginCrossReq>(m_pbStream, login);
            SendMsgToCross(login.id, m_pbStream.ToArray());
        }
        else
            Utility.LogError("login to cross server failed (ip:" + ip_ + ":" + port_ + ")");
    }

    public void DestroyLoginClient()
    {
        m_thread.DestroyLoginClient();
    }

    public bool SendMsgToLogin(Cmd.EMessageID protoId_, byte[] buf_)
    {
        return m_thread.SendMsgToLogin((UInt16)protoId_, buf_);
    }

    public bool SendMsgToGate(Cmd.EMessageID protoId_, byte[] buf_)
    {
        return m_thread.SendMsgToGate((UInt16)protoId_, buf_);
    }

    public bool SendMsgToCross(Cmd.EMessageID protoId_, byte[] buf_)
    {
        return m_thread.SendMsgToCross((UInt16)protoId_, buf_);
    }


    public void AddMsg(byte[] cmd_, int len_)
    {
        Monitor.Enter(m_cmdList);  

        byte[] msgBytes = new byte[len_];
        Array.Copy(cmd_, msgBytes, len_);
        m_cmdList.Add(msgBytes);       

        Monitor.Exit(m_cmdList);
    }

    void LateUpdate()
    {
        Monitor.Enter(m_cmdList);        
        if(m_cmdList.Count > 0)
        {
            foreach (byte[] recvCmd in m_cmdList)
			{	
				UInt16 msgId = BitConverter.ToUInt16(recvCmd, 0);
				byte[] realCmd = new byte[recvCmd.Length-TCPClient.MSG_ID_LEN];
				Array.Copy(recvCmd, TCPClient.MSG_ID_LEN, realCmd, 0, realCmd.Length);

				if (0 == msgId)
                {
                    Utility.Log("recv tickcmd,just send back");
					SendMsgToGate((Cmd.EMessageID)msgId, realCmd);
                }
                else
                {
					HandleMgr.MsgHandle handle = HandleMgr.getHandle((Cmd.EMessageID)msgId);
                    if (null != handle)
                    {
                        handle(realCmd, realCmd.Length);
                    }
                    else
                    {
                        Utility.LogError("recv unknown proto msg id:0x0" + msgId.ToString("X"));
                    }
                }
                    /*
                else if (Cmd.EMessageID.LOGIN_LOGIN_SC == (Cmd.EMessageID)CSBridge.s_recvProtoId)
                {  
                    MemoryStream ms = new MemoryStream(realCmd, 0, realCmd.Length);
                    Cmd.LoginRet ret = Serializer.Deserialize<Cmd.LoginRet>(ms);

                    LoginToGateServer(ret.gatewayip, (int)ret.gatewayport, ret.accountid, ret.tempid);
                    m_thread.DestroyLoginClient();
                }
                else if (Cmd.EMessageID.LOGIN_GATEW_SC == (Cmd.EMessageID)CSBridge.s_recvProtoId)
                {
                    Utility.Log("Login gateway server return ok!");
                }
                else if (Cmd.EMessageID.PVP_CMD == (Cmd.EMessageID)(CSBridge.s_recvProtoId | 0xff00))
                {
                    // recv battle msg, do fighting logic in c#
                }
                else
				{		// default do other logic in lua 			
					CSBridge.s_recvBytes = new LuaInterface.LuaByteBuffer(realCmd);
    	            MsgHandler.Instance.MsgParse();
            	}*/
            }
            
            m_cmdList.Clear();
        }        
        Monitor.Exit(m_cmdList);

        if (DateTime.Now.Second - m_lastSendGateTime.Second > 4)
        {
            if (m_gateIP.Length > 0)
            {
                if (!m_thread.CheckGateConnected())
                {
                    Utility.LogWarning("tcp gate client is disconnected!!!");
                    m_reconnectingPanel.SetActive(true);
                    m_thread.DestroyGateClient();
                    LoginToGateServer(m_gateIP, m_gatePort, m_account, m_tempId);
                }
                else
                {
                    Utility.Log("tcp gate client is connected");
                    if (m_reconnectingPanel && m_reconnectingPanel.activeSelf)
                    {
                        m_reconnectingPanel.SetActive(false);
                    }
                }
            }
            m_lastSendGateTime = DateTime.Now;
        }
    }   
      

    public void DestroyThread()
    {
        if(null != m_thread)
        {
            m_thread.Terminate();
            m_thread = null;
        }
        Utility.Log("DestroyThread called");
    }

	void OnDestroy()
    {
        DestroyThread();
    }

	void OnApplicationQuit()
	{
        DestroyThread ();
	}
        
    void OnApplicationFocus(bool focus_)
    {
        if (focus_)
        {
            Utility.Log("application get focus");

            if (m_gateIP.Length > 0)
            {
                if (!m_thread.CheckGateConnected())
                {
                    Utility.LogWarning("tcp gate client is disconnected!!!");
                    m_reconnectingPanel.SetActive(true);
                    m_thread.DestroyGateClient();
                    LoginToGateServer(m_gateIP, m_gatePort, m_account, m_tempId);
                }
            }
        }
        else
            Utility.Log("application get been background");
    }
}
