
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Chip amount, number of intances, and total amount.
/// </summary>
[System.Serializable]
public class Bet
{
	public int chipAmount = 0;
	public int chipNumber = 0;
	public int chipTotal { get { return chipAmount * chipNumber; } }

	public Bet(int amount, int number) { chipAmount = amount; chipNumber = number; }
}

/// <summary>
/// Results.
/// </summary>
[System.Serializable]
public class Bets
{
	public List<Bet> list = new List<Bet>();

	public int getBetsTotal
	{
		get
		{
			int totalBet = 0;
			foreach(Bet bet in list)
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
public class PlayResult
{
	public int[] result = new int[6];
	public List<Bets> colorBets = new List<Bets>(6);

	public PlayResult()
	{
		colorBets = new List<Bets>(6);
		colorBets.Add ( new Bets() );
		colorBets.Add ( new Bets() );
		colorBets.Add ( new Bets() );
		colorBets.Add ( new Bets() );
		colorBets.Add ( new Bets() );
		colorBets.Add ( new Bets() );
	}

	public int getTotalPlayBet
	{
		get
		{
			int totalPlayBet = 0;
			foreach(Bets result in colorBets)
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
		MachineInstance mIntance = CustomReference.Access.machineGroups.OnFocusedMachine;

		if(mIntance.playResult.colorBets[colorIndex].list.Exists( x => x.chipAmount == chipAmount ))
		{
			mIntance.playResult.colorBets [colorIndex].list.Find ( x => x.chipAmount == chipAmount ).chipNumber += 1;
		}

		else
		{
			mIntance.playResult.colorBets [colorIndex].list.Add ( new Bet (chipAmount, 1) );
		}
	}
}

[System.Serializable]
public class UserDetails
{
	public string userToken = "";
	public string userName = "";
	public Texture2D userPhoto = null;

	public int currentCash = 100;
	public int userReturnAward = 100;
}

[System.Serializable]
public class UserRecords
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
public class MachinePrefabs
{
	public string name = string.Empty;
	public MachineInstance machineInstance = null;
	public List<BillValue> billPrefab = new List<BillValue>();
	public List<Sprite> billTexture = new List<Sprite>();
}