using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour, IAction
{
	public string Name { get; } = "Potion";
	public int HPInflicted { get; } = 25;
	
	
	public int Hit(ICharacter potionOwner, ICharacter attacker)
	{
		int addHP = potionOwner.HP + HPInflicted;
		if (addHP <= potionOwner.MaxHP && potionOwner.PotionsAvailable > 0)
		{
			int nwHP = potionOwner.HP + HPInflicted;			
			potionOwner.PotionsAvailable--;
			return nwHP;
		}
		else if (addHP > potionOwner.MaxHP && potionOwner.PotionsAvailable > 0)
		{
			int pointsToMax = potionOwner.MaxHP - potionOwner.HP;			
			potionOwner.HP = potionOwner.MaxHP;
			potionOwner.PotionsAvailable--;		
			return potionOwner.HP;
		}
		else
		{
			return potionOwner.HP;
		}
	}
}
