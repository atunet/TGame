using System;

public enum EDirection
{
    DIR_EAST    =   0,
    DIR_SOUTH   =   1,
    DIR_WEST    =   2,
    DIR_NORTH   =   3,
}

public enum EFlyState
{
    FLY_READY           =   0,     //  readay for fly
    FLY_FLYING          =   1,     //  flying
    FLY_OVER            =   2,     //  fly over
}

public enum EItemType
{
    ITEM_CLOUD          =   0,
    ITEM_ENVELOPE       =   1,
    ITEM_FUEL           =   2,
    ITEM_GIFT           =   3,
    ITEM_GOLD1          =   4,
    ITEM_GOLD2          =   5,
    ITEM_KITE           =   6,
    ITEM_PLANE          =   7,
    ITEM_REPAIR         =   8,
    ITEM_SAUCER         =   9,
    ITEM_TICKET         =   10,
    ITEM_MAX            =   11,
}

// 背景音乐枚举
public enum EBGMusic : byte
{
	BG_MUSIC_MAIN,
	BG_MUSIC_FIGHT,
}

// sound enumation
public enum ESound : byte
{
	SOUND_OK,
	SOUND_CANCEL,
	SOUND_SELECT_ITEM,
	SOUND_POWER_USED,
	SOUND_READY_GO,
	SOUND_TIME_UP,
	SOUND_BONUS_TIME,
	SOUND_LAST_BONUS,
	SOUND_CAT,
	SOUND_SKILL,
	SOUND_MISSION,
	SOUND_FIRE,
	SOUND_WOW,
	SOUND_YEAH,
	//SOUND_PERFECT,
	//SOUND_AMAZING,
	//SOUND_UNBELIEVEABLE,
	SOUND_COUNT_DOWN,
	SOUND_AUTO_BOMB,
	SOUND_STAR_01,
	SOUND_STAR_02,
	SOUND_STAR_03,
	SOUND_ADD_SCORE,
	SOUND_ADD_MONEY,
	SOUND_BEST_SCORE,
	SOUND_LEVELUP,
    SOUND_ITEM_COLLISION,
    SOUND_ROCKET_ENGINE,
}

// 
public enum EBuyType : byte
{
	BUY_DIAMOND,
	BUY_GOLD,
	BUY_POWER
}
