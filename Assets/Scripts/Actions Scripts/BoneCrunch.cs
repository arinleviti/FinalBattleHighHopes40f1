using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneCrunch : MonoBehaviour, IAction
{
	public string Name { get; } = "BoneCrunch";
	public int HPInflicted { get; set; }
	
	public int Hit(ICharacter characterHit, ICharacter attacker)
	{
		HPInflicted = UnityEngine.Random.Range(5, 15);
		int newHP = characterHit.HP - HPInflicted;
		return newHP;
	}
}
