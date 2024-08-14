using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour, IAction
{
	public string Name { get; } = "Punch";
	public int HPInflicted { get; set; }

	public int Hit(ICharacter characterHit, ICharacter attacker)
	{
		HPInflicted = UnityEngine.Random.Range(10, 20);
		int newHP = characterHit.HP - HPInflicted;
		Console.ForegroundColor = ConsoleColor.DarkRed;
		Console.WriteLine($"{attacker.Name} has landed a Punch, dealing {HPInflicted} points of damage to {characterHit.Name}'s health.");
		Console.ResetColor();
		return newHP;
	}

}
