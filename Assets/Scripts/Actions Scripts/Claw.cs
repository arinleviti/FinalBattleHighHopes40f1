using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claw : MonoBehaviour, IAction
{
	public string Name { get; } = "Claw";
	public int HPInflicted { get; set; }
	//public int RequiredBattleNumber { get; set; }
	public int Hit(ICharacter characterHit, ICharacter attacker)
	{
		HPInflicted = UnityEngine.Random.Range(5, 10);
		int newHP = characterHit.HP - HPInflicted;		
		return newHP;
	}
}
