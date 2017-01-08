using System;
using System.Collections;
using System.Threading;
using System.Text;
using System.IO;

internal class NetThread
{
    private Thread m_thread = null;

    private TCPClient m_loginClient = null;
    private TCPClient m_gateClient = null;
    private UDPClient m_crossClient = null;

    private byte[] m_loginRcvBuf = null;
    private byte[] m_gateRcvBuf = null;
    private byte[] m_crossRcvBuf = null;

    private bool m_terminate = true;

    public NetThread()
    {
        m_thread = new Thread(new ThreadStart(Run));

        m_loginRcvBuf = new byte[64*1024];
        m_gateRcvBuf = new byte[64*1024];
        m_crossRcvBuf = new byte[64*1024];
    }

    public void Start() 
    { 
        m_thread.Start(); 
        m_terminate = false; 
    }

    public void Terminate()
    { 
        if (!m_terminate)
        {
            m_terminate = true; 
            if (null != m_loginClient)
            {
                m_loginClient.Close();
                m_loginClient = null;
            }
            if (null != m_gateClient)
            {
                m_gateClient.Close();
                m_gateClient = null;
            }
            if (null != m_crossClient)
            {
                m_crossClient.Close();
                m_crossClient = null;
            }
            m_thread.Join();
            m_thread = null;
        }
    }

    public bool InitLoginClient(string ip_, int port_)
    {
        if (null != m_loginClient)
        {
            Console.WriteLine("tcp login client is not null, init ignored!!!");
            return false;
        }

        m_loginClient = new TCPClient(ip_, port_);
        if (!m_loginClient.Connect())
        {
            Console.WriteLine("tcp login client connect failed [{0}:{1}]", ip_, port_);
            m_loginClient = null;
            return false;
        }

        Console.WriteLine("tcp login client init connect ok [{0}:{1}]", ip_, port_);
        return true;
    }


    public bool InitGateClient(string ip_, int port_)
    {
        if (null != m_gateClient)
        {
            Console.WriteLine("tcp gate client is not null, init ignored!!!");
            return false;
        }

        m_gateClient = new TCPClient(ip_, port_);
        if (!m_gateClient.Connect())
        {
            Console.WriteLine("tcp gate client connect failed [{0}:{1}]", ip_, port_);
            m_gateClient = null;
            return false;
        }

        Console.WriteLine("tcp gate client init connect ok [{0}:{1}]", ip_, port_);
        return true;
    }


    public bool InitCrossClient(string ip_, int port_)
    {
        if (null != m_crossClient)
        {
            Console.WriteLine("udp cross client is not null, init ignored!!!");
            return false;
        }

        m_crossClient = new UDPClient(ip_, port_);
        return true;
    }


    public void DestroyLoginClient()
    {
        if (null != m_loginClient)
        {
            m_loginClient.Close();
            m_loginClient = null;

            Console.WriteLine("tcp login client destroy ok");
        }
        else
        {
            Console.WriteLine("tcp login client is null,destroy ignored!!!");
        }
    }

    public void DestroyGateClient()
    {
        if (null != m_gateClient)
        {
            m_gateClient.Close();
            m_gateClient = null;

            Console.WriteLine("tcp gate client destroy ok");
        }
        else
        {
            Console.WriteLine("tcp gate client is null,destroy ignored!!!");
        }
    }

    public bool SendMsgToLogin(UInt16 msgId_, byte[] msg_)
    {
        return (null != m_loginClient) ? m_loginClient.SendMsg(msgId_, msg_) : false;
    }

    public bool SendMsgToGate(UInt16 msgId_, byte[] msg_)
    {
        return (null != m_gateClient) ? m_gateClient.SendMsg(msgId_, msg_) : false;
    }

    public bool SendMsgToCross(UInt16 msgId_, byte[] msg_)
    {
        return (null != m_crossClient) ? m_crossClient.SendMsg(msgId_, msg_) : false;
    }
	
    public bool CheckGateConnected()
    {
        return (null != m_gateClient && m_gateClient.Connected());
    }

	private void Run()
	{
        while(!m_terminate)
		{	
			Thread.Sleep(10);
			
            if (null != m_loginClient)
            {
                TCPClient.EReceiveCode retCode = m_loginClient.Receive();
                if (retCode == TCPClient.EReceiveCode.RCV_CODE_OK)
                {
                    while (true)
                    {
                        int msgLen = m_loginClient.bufToMsg(ref m_loginRcvBuf);
                        if (msgLen <= 0) 
                            break;                        
                        NetController.Instance.AddMsg(m_loginRcvBuf, msgLen);
                    }
                }
                else if (retCode == TCPClient.EReceiveCode.RCV_CODE_CLOSE)
                {
                }
                else if (retCode == TCPClient.EReceiveCode.RCV_CODE_ERR)
                {
                }
            }
            if (null != m_gateClient)
            {
                TCPClient.EReceiveCode retCode = m_gateClient.Receive();
                if (retCode == TCPClient.EReceiveCode.RCV_CODE_OK)
                {
                    while (true)
                    {
                        int msgLen = m_gateClient.bufToMsg(ref m_gateRcvBuf);
                        if(msgLen <= 0) 
                            break;
                        NetController.Instance.AddMsg(m_gateRcvBuf, msgLen);
                    }
                }
                else if (retCode == TCPClient.EReceiveCode.RCV_CODE_CLOSE)
                {
                }
                else if (retCode == TCPClient.EReceiveCode.RCV_CODE_ERR)
                {
                }

            }
            if (null != m_crossClient)
            {
                int retCode = m_crossClient.Receive();
                if (retCode >= 0)
                {
                    while (true)
                    {
                        int msgLen = m_crossClient.bufToMsg(ref m_crossRcvBuf);
                        if(msgLen <= 0) 
                            break;
                        NetController.Instance.AddMsg(m_crossRcvBuf, msgLen);
                    }
                }
            }
		}
	}
};
