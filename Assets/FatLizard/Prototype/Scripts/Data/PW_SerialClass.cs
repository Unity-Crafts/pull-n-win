
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class PW_Bet
{
	public int chipAmount = 0;
	public int chipNumber = 0;
	public int chipTotal
	{
		get
		{
			return chipAmount * chipNumber;
		}
	}

	public PW_Bet(int amount, int number) 
	{
		chipAmount = amount;
		chipNumber = number;
	}
}

[System.Serializable]
public class PW_Bets
{
	public List<PW_Bet> list = new List<PW_Bet>();

	public int getBetsTotal
	{
		get
		{
			int totalBet = 0;
			foreach(PW_Bet bet in list)
			{
				totalBet += bet.chipTotal;
			}
			return totalBet;
		}
	}

	public int calcBetsWin(int colorFrequency)
	{
		return getBetsTotal * colorFrequency;
	}
}

[System.Serializable]
public class PW_PlayResult
{
	public int[] result = new int[6];
	public List<PW_Bets> colorBets = new List<PW_Bets>(6);

	public PW_PlayResult()
	{
		colorBets = new List<PW_Bets>(6);
		colorBets.Add ( new PW_Bets() );
		colorBets.Add ( new PW_Bets() );
		colorBets.Add ( new PW_Bets() );
		colorBets.Add ( new PW_Bets() );
		colorBets.Add ( new PW_Bets() );
		colorBets.Add ( new PW_Bets() );
	}

	public int getTotalPlayBet
	{
		get
		{
			int totalPlayBet = 0;
			foreach(PW_Bets result in colorBets)
			{
				totalPlayBet += result.getBetsTotal;
			}
			return totalPlayBet;
		}
	}

	public int getTotalPlayWin
	{
		get
		{
			int totalPlayBet = 0;
			for(int i = 0; i < colorBets.Count; i++)
			{
				totalPlayBet += colorBets[i].calcBetsWin( result[i] * 2 );
			}
			return totalPlayBet;
		}
	}

	public void UpdateResult(int colorIndex, int chipAmount)
	{
		PW_MInstance mIntance = PW_References.Access.machineGroups.OnFocusedMachine;

		if(mIntance.playResult.colorBets[colorIndex].list.Exists( x => x.chipAmount == chipAmount ))
		{
			mIntance.playResult.colorBets [colorIndex].list.Find ( x => x.chipAmount == chipAmount ).chipNumber += 1;
		}

		else
		{
			mIntance.playResult.colorBets [colorIndex].list.Add ( new PW_Bet (chipAmount, 1) );
		}
	}
}

[System.Serializable]
public class PW_UserDetails
{
	public int currentChips = 100000;
	public int currentGems = 10;
}

[System.Serializable]
public class PW_UserRecords
{
	public int totalPlays = 0;

	public int numberBet = 0;
	public int numberWin = 0;
	public int numberLoss = 0;

	public int totalBet = 0;
	public int totalWin = 0;
	public int totalLoss = 0;

	public int maxBet = 0;
	public int maxWin = 0;
	public int maxLoss = 0;
}

[System.Serializable]
public class PW_MPrefabs
{
	public string name = string.Empty;
	public PW_MInstance machineInstance = null;
	public List<PW_BillValue> billPrefab = new List<PW_BillValue>();
	public List<Sprite> billTexture = new List<Sprite>();
}