using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MistyFist : MonoBehaviour, IAction
{
	public string Name { get; } = "MistyFist";
	public int HPInflicted { get; set; }
	//public int RequiredBattleNumber { get; set; }
	public int Hit(ICharacter characterHit, ICharacter attacker)
	{
		HPInflicted = UnityEngine.Random.Range(7, 18);
		int newHP = characterHit.HP - HPInflicted;	
		return newHP;
	}
}
