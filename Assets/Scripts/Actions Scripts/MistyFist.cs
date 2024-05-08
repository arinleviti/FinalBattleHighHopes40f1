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
		Console.ForegroundColor = ConsoleColor.DarkRed;
		Console.WriteLine($"{attacker.Name} has landed a Misty Fist, dealing {HPInflicted} points of damage to {characterHit.Name}'s health.");
		Console.ResetColor();
		return newHP;
	}
}
