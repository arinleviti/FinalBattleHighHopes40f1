using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction
{
	public string Name { get; }
	public int HPInflicted { get; }
	public int Hit(ICharacter characterHit, ICharacter attacker);
}




public interface IExtraActions
{
	public string Name { get; }
	public int HPInflicted { get; }
	public int MAXHPSelfInflicted { get; }
	public int Cost { get; }
	public int RequiredBattleNumber { get; }
	public int ChanceOfDying { get; set; }
}
public class ThunderBlast : IExtraActions
{
	public string Name { get; } = "ThunderBlast";
	public int HPInflicted { get; } = 12;            //Points inflicted to victim
	public int MAXHPSelfInflicted { get; set; }     //Points inflicted to attacker. Add 1 to the value when creating an instance of the object
	public int Cost { get; } = 2;
	public int RequiredBattleNumber { get; } = 2;
	public int ChanceOfDying { set; get; }
	public int SelfInflicted { set; get; }
	public ThunderBlast(int maxHPSelfInflicted)
	{
		MAXHPSelfInflicted = maxHPSelfInflicted;
	}
	public int Hit(ICharacter characterHit, ICharacter attacker)
	{
		SelfInflicted = UnityEngine.Random.Range(0, MAXHPSelfInflicted);
		int newHP = characterHit.HP - HPInflicted;
		attacker.HP -= SelfInflicted;
		Console.ForegroundColor = ConsoleColor.DarkRed;
		Console.WriteLine($"{attacker.Name} has landed a ThunderBlast, dealing {HPInflicted} points of damage to {characterHit.Name}'s health.");
		Console.WriteLine($"{attacker.Name} has suffered {SelfInflicted} HP. Their new HP is {attacker.HP}");
		Console.ResetColor();
		return newHP;
	}
}

public class Annihilator : IExtraActions
{
	public string Name { get; } = "Annihilator";
	public int HPInflicted { get; } = 15;
	public int MAXHPSelfInflicted { get; set; }
	public int Cost { get; } = 3;
	public int RequiredBattleNumber { get; set; } = 3;
	public int ChanceOfDying { get; set; }
	public int SelfInflicted { get; set; }
	public Annihilator(int maxHPSelfInflicted, int chanceOfDying)
	{
		MAXHPSelfInflicted = maxHPSelfInflicted;
		ChanceOfDying = chanceOfDying;
	}
	public int Hit(ICharacter characterHit, ICharacter attacker)
	{
		int willIDie = UnityEngine.Random.Range(1, ChanceOfDying);
		if (willIDie == 1)
		{
			characterHit.HP -= HPInflicted;
			attacker.HP = 0;
			Console.ForegroundColor = ConsoleColor.DarkRed;
			Console.WriteLine($"{attacker.Name} has landed an Annihilator, dealing {HPInflicted} points of damage to {characterHit.Name}'s health.");
			Console.WriteLine($"{Name} was too powerful for {attacker.Name}.");
			Console.ResetColor();
		}
		else
		{
			SelfInflicted = UnityEngine.Random.Range(1, MAXHPSelfInflicted);
			attacker.HP -= SelfInflicted;
			characterHit.HP -= HPInflicted;
			Console.ForegroundColor = ConsoleColor.DarkRed;
			Console.WriteLine($"{attacker.Name} has landed an Annihilator, dealing {HPInflicted} points of damage to {characterHit.Name}'s health.");
			Console.WriteLine($"{attacker.Name} has suffered {SelfInflicted} HP. Their new HP is {attacker.HP}");
			Console.ResetColor();
		}
		return characterHit.HP;
	}
}

