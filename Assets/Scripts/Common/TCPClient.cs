using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class TCPClient
{
    internal static Byte HEAD_LEN = 4;
    internal static Byte MSG_ID_LEN = 2;
    internal static Byte SEQUENCE_LEN = 4;
    internal static int RCV_BUF_LEN = 256 * 1024;

    public enum EReceiveCode
    {
        RCV_CODE_OK,
        RCV_CODE_ERR,
        RCV_CODE_CLOSE,
    };

    private String m_serverIP = null;
    private int m_serverPort = 0;
    private Socket m_socket = null;
    private UInt32 m_seqNO = 0;

    private byte[] m_rcvBuf = null;
    private int m_bufReadOffset = 0;
    private int m_bufWriteOffset = 0;

    public TCPClient (String ip_, int port_)
    {	
        m_serverIP = ip_;
        m_serverPort = port_;
        m_rcvBuf = new byte[RCV_BUF_LEN];
    }


	public Boolean Connect ()
	{
        if(null != m_socket)
        {
            Console.WriteLine("Socket is not null,connect ignored!!!");
            return true;
        }

        IPEndPoint serverAddr = new IPEndPoint(IPAddress.Parse(m_serverIP), m_serverPort);
        m_socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);	
		try
		{
			m_socket.Connect (serverAddr);
		}
		catch(SocketException e_)
		{
            Console.WriteLine("TCPClient Connect exception:{0}", e_.ToString());
            m_socket = null;
			return false;
		}

        Console.WriteLine("TCPClient connect server ok [{0}:{1}]", m_serverIP, m_serverPort);
		return true;
	}


	public void Close ()
	{
		if (null != m_socket) 
		{
            if (m_socket.Connected)
            {
                m_socket.Shutdown(SocketShutdown.Both);
                m_socket.Disconnect(true);
            }
            m_socket = null;
		}
        Console.WriteLine("TCPClient closed!");
	}
	

    public Boolean SendMsg (UInt16 msgId_, byte[] msg_)
	{	        
        if (null != m_socket && m_socket.Connected)
        {
            UInt32 packetLen = (UInt32)(MSG_ID_LEN + SEQUENCE_LEN + msg_.Length);
            byte[] sendBytes = new byte[HEAD_LEN + packetLen];

            BitConverter.GetBytes(packetLen).CopyTo(sendBytes, 0);
            BitConverter.GetBytes(msgId_).CopyTo(sendBytes, HEAD_LEN);
            BitConverter.GetBytes(++m_seqNO).CopyTo(sendBytes, HEAD_LEN + MSG_ID_LEN);
            msg_.CopyTo(sendBytes, HEAD_LEN + MSG_ID_LEN + SEQUENCE_LEN);		
    		
            int offset = 0;
            while (offset < sendBytes.Length)
            {
                try
                {
                    offset += m_socket.Send(sendBytes, offset, sendBytes.Length - offset, SocketFlags.None);
                }
                catch(Exception e_)
                {
                    Console.WriteLine("tcpclient sendmsg error:" + e_.ToString());
                    //Close();
                    return false;
                }
            }

            Console.WriteLine("TCPClient send msgid:{0} ok,msg len:{1},total len:{2}", msgId_.ToString("X"), msg_.Length, packetLen);
            return true;
        }
        else
        {
            Console.WriteLine("TCPClient send msg failed,socket is disconnected,msgid:{0},len:{1}", msgId_.ToString("X"), msg_.Length);
            return false;
        }
	}

    public TCPClient.EReceiveCode Receive ()
	{	
        if(null != m_socket && m_socket.Connected)
        {
            try
            {
                int rcvCount = m_socket.Receive(m_rcvBuf, m_bufWriteOffset, RCV_BUF_LEN-m_bufWriteOffset, SocketFlags.None);
                m_bufWriteOffset += rcvCount;
                return TCPClient.EReceiveCode.RCV_CODE_OK;
            }
            catch(ObjectDisposedException e_)
            {
                Console.WriteLine("remote server close the connection:" + e_.ToString());
                return TCPClient.EReceiveCode.RCV_CODE_CLOSE;
            }
            catch(Exception e_)
            {
                Console.WriteLine("tcp client receive error:" + e_.ToString());
                return TCPClient.EReceiveCode.RCV_CODE_ERR;
            }
		}

        return TCPClient.EReceiveCode.RCV_CODE_ERR;
	}
	
    public int bufToMsg(ref byte[] msgBuf_)
    {
        if (m_bufWriteOffset - m_bufReadOffset < (TCPClient.HEAD_LEN + TCPClient.MSG_ID_LEN))
            return 0;
        
        int msgLen = BitConverter.ToInt32(m_rcvBuf, m_bufReadOffset);                          
        if (m_bufWriteOffset - m_bufReadOffset < msgLen + TCPClient.HEAD_LEN)
            return 0;
        
        Array.Copy(m_rcvBuf, m_bufReadOffset + TCPClient.HEAD_LEN, msgBuf_, 0, msgLen);
        m_bufReadOffset += (msgLen + TCPClient.HEAD_LEN);

        if (m_bufReadOffset > TCPClient.RCV_BUF_LEN / 2)
        {
            if (m_bufWriteOffset > m_bufReadOffset)
            {
                Array.Copy(m_rcvBuf, m_bufReadOffset, m_rcvBuf, 0, m_bufWriteOffset - m_bufReadOffset);
                m_bufWriteOffset -= m_bufReadOffset;
                m_bufReadOffset = 0;
            }
        }

        return msgLen;
    }

	public bool Connected ()
	{
        return (null != m_socket && m_socket.Connected);
	}
}
