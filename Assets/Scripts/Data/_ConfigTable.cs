#pragma warning disable 114
using System;
using System.Collections;
using System.Collections.Generic;
public class _ConfigTable {
private static readonly Lazy<_ConfigTable> _instance = new Lazy<_ConfigTable>(() => new _ConfigTable());
public static _ConfigTable Instance { get { return _instance.Value; } }

	public int MaxSeed;
	public int MaxGold;
	public int RequestWait;
	public int RequestMax;
	public int StorageMaxProduct;
	public int StorageMaxCollect;
	public int StorageMaxLetter;
	public int StorageMaxEquipUpgrade;
	public int StorageMaxFavorExp;
	public int StorageMaxVillagerExp;
	public int StorageMaxTimecut;
	public int StorageMaxGacha;
	public int StorageMaxRandombox;
	public int TimecutSeed;
	public int SpecialMentTouch;
	public int FriendVillagerSlot;
	public int FriendMax;
	public int RecommendDay;
	public int RecommendLevelUp;
	public int RecommendLevelDown;
	public int RecommendHighLevel;
	public int FriendRequestGetMax;
	public int FriendRequestSendMax;
	public int GuestbookWriteMax;
	public int IgnoreMax;
	public int FriendCoinGet;
	public int FriendCoinGetCount;
	public int FriendCoinMax;
	public int FavorExpLike;
	public int FavorExp;
	public int FavorExpDislike;
	public int DeremodeDuration;
	public int GloomymodeOccurLv;
	public int GloomymodeOccurTime;
	public int StarUpgarde1;
	public int StarUpgarde2;
	public int StarUpgarde3;
	public int StarUpgarde4;
	public int EquipUpgradeIndex;
	public int ChangeNickPrice;
	public int LimitGreetingWord;
	public int NicknameMax;
	public int NicknameMin;
	public int RankRewardTime;
	public string LoginResetTime;
	public int LoginBonusCount;
	public int LoginBonusSEED;
	public int PromiseResetPrice;
	public int PostboxMailMax;
	public int PostboxLogMax;
	public int PostboxLogExpired;
	public int PostboxAdCooltime;
	public int SetTouchDelay;
	public int PostboxAdIcon;
	public int NormalMissionSlot;
	public int SpecialMissionSlot;
	public int CloseMsgBox;
	public int NoticeRollingTime;
	public int PromiseTouchDelay;
	public int UseRandomboxMax;
	public int CouponApplyTime;
	public int RetrySSBoxID;
	public int RetrySBoxID;
	public int RetryABoxID;
	public int RetryBBoxID;
	public int PackageCouponIcon;
	public int CouponPostboxTime;
	public int PackageRewardIcon;
	public int VillagerBMaxBonus;
	public int VillagerAMaxBonus;
	public int VillagerSMaxBonus;
	public int VillagerSSMaxBonus;
	public int CouponOnOff;
	public int OpenStoryUseKey;
	public void LoadConfig(Dictionary<string, Dictionary<string, object>> rowList)
	{
		foreach (var rowItem in rowList)
		{
			var field = typeof(_ConfigTable).GetField(rowItem.Key, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			field.SetValue(this, rowItem.Value["value"]);
		}
	}
};
