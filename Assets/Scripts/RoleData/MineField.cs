using System.Collections;
using System.Collections.Generic;

public class MineField
{
    public MineField(byte row, byte column) 
    {
        m_row = row;
        m_column = column;
        m_mines = new byte[m_row * m_column];
        m_states = new byte[m_row * m_column];
    }


    public bool Init(byte[] mineList)
    {
        if (null == mineList || mineList.Length != m_row * m_column)
        {
            return false;
        }

        for (byte i = 0; i < mineList.Length; ++i)
        {
            m_mines[i] = (byte)(mineList[i] > 0 ? 1 : 0);
            m_states[i] = 1;
        }

        return true;
    }

    private byte m_row = 20;
    private byte m_column = 20;
    private byte[] m_mines = null;  // 0:not have mine; 1:have mine
    private byte[] m_states = null; // 0:already open; 1:not open
}
