
using System;

// 关卡配置
public struct LevelConf
{
	public UInt16 id;
	public byte passType;
	public UInt16 passValue;
	public UInt16 starScore1;
	public UInt16 starScore2;
	public UInt16 starScore3;
	/*
	public byte aimChessNum1;
	public byte aimChessNum2;
	public byte aimChessNum3;
	public byte aimChessNum4;
	public byte aimChessNum5;
	*/
};

// 道具配置
public struct ItemConf
{
	//public EItemId id;
	public byte priceType;
	public UInt16 price1;
	public UInt16 price2;
	public UInt16 price3;
	public UInt16 defaultPrice;
	public byte levelLimit;
	public byte generateRate;
	public byte maxNum;
};

// 等级经验配置
public struct ExpConf
{
	public byte lvId;
	public int exp;
	public byte addition;
};

// buy conf
public struct BuyConf
{
	public ushort id;
	public string fromNum;
	public int toNum;
};