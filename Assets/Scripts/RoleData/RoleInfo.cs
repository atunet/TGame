using System.Collections;

public class LoginInfo
{
    internal string s_openId = "123456";
    internal string s_openKey = "openkey_openkey_";
    internal string s_token = null;
    internal ulong s_accountId = 0;
    internal ulong s_roleId = 0;
};

public class RoleInfo
{
	public int createTime;				// 帐号创建时间
	public int currentExp;   			// 当前经验值			
	public int currentGold;				// 当前金币数量
	public int currentDiamond;			// 当前钻石数量
	public int currentPower;			// 当前体力值
	public int powerCountdownTime;		// 体力恢复倒计时(上次开始倒计时时刻)
	//public EAnimal currentAnimal;		// 当前所带的宠物(宠物代表技能)
}
