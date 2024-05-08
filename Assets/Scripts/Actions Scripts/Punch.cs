using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour, IAction
{
	public string Name { get; } = "Punch";
	public int HPInflicted { get; set; } /*= 5;*/
	//public int RequiredBattleNumber { get; set; }
	public int Hit(ICharacter characterHit, ICharacter attacker)
	{
		HPInflicted = UnityEngine.Random.Range(7, 11);
		int newHP = characterHit.HP - HPInflicted;
		Console.ForegroundColor = ConsoleColor.DarkRed;
		Console.WriteLine($"{attacker.Name} has landed a Punch, dealing {HPInflicted} points of damage to {characterHit.Name}'s health.");
		Console.ResetColor();
		return newHP;
		//int newHP =characterHit.HP - HPInflicted;
		//Console.ForegroundColor = ConsoleColor.DarkRed;
		//Console.WriteLine($"{attacker.Name} has landed a Punch, dealing {HPInflicted} points of damage to {characterHit.Name}'s health.");
		//Console.ResetColor();
		//return newHP;
	}

}
