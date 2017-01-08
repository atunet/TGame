using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;

public class UDPClient
{
    private String m_serverIP = null;
    private int m_serverPort = 0;
    private EndPoint m_serverAddr = null;
    private Socket m_socket = null;
    private UInt32 m_seqNO = 0;

    private byte[] m_rcvBuf = null;
    private int m_bufReadOffset = 0;
    private int m_bufWriteOffset = 0;

    public UDPClient (string ip_, int port_)
    {   
        m_serverIP = ip_;
        m_serverPort = port_;

        m_serverAddr = (EndPoint)new IPEndPoint(IPAddress.Parse(m_serverIP), m_serverPort);
        m_socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Udp); 

        m_rcvBuf = new byte[TCPClient.RCV_BUF_LEN];

    }

    public Boolean SendMsg (UInt16 msgId_, byte[] msg_)
    {  
        UInt32 packetLen = (UInt32)(TCPClient.MSG_ID_LEN + TCPClient.SEQUENCE_LEN + msg_.Length);
        byte[] sendBytes = new byte[TCPClient.HEAD_LEN + packetLen];

        BitConverter.GetBytes(packetLen).CopyTo(sendBytes, 0);
        BitConverter.GetBytes(msgId_).CopyTo(sendBytes, TCPClient.HEAD_LEN);
        BitConverter.GetBytes(++m_seqNO).CopyTo(sendBytes, TCPClient.HEAD_LEN + TCPClient.MSG_ID_LEN);
        msg_.CopyTo(sendBytes, TCPClient.HEAD_LEN + TCPClient.MSG_ID_LEN + TCPClient.SEQUENCE_LEN);       

        int offset = 0;
        while (offset < sendBytes.Length)
        {
            offset += m_socket.SendTo(sendBytes, offset, sendBytes.Length - offset, SocketFlags.None, m_serverAddr);
        }

        Console.WriteLine("UDPClient send msgid:{0} ok,msg len:{1},total len:{2}", msgId_.ToString("X"), msg_.Length, packetLen);
        return true;
    }

    public int Receive()
    {
        return m_socket.ReceiveFrom(m_rcvBuf, m_bufWriteOffset, TCPClient.RCV_BUF_LEN-m_bufWriteOffset, SocketFlags.None, ref m_serverAddr);
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

        if (m_bufReadOffset > TCPClient.RCV_BUF_LEN/2)
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


    public void Close()
    {
    }
}
