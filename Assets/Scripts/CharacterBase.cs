using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
	public int HP { get; set; }
	public int MaxHP { get; set; }
	public string Name { get; set; }
	//public int BattlesWon { get; set; }
	public List<AttackType> AttackT { get; set; }
	public Category CharacterCategory { get; set; }
	public bool IsDead { get; set; }
	//public bool Turn { get; set; }
	public int PotionsAvailable { get; set; }
	//public AttackModifier AttackModifier { get; set; }
	//public int HitsTakenPerBattle { get; set; }
}


public class CharacterClass : MonoBehaviour, ICharacter 
{
	public int HP { get; set; }
	public int MaxHP { get; set; }
	public string Name { get; set; }
	public int BattlesWon { get; set; }
	public List<AttackType> AttackT { get; set; }
	public Category CharacterCategory { get; set; }
	public bool IsDead { get; set; }
	public bool Turn { get; set; }
	public int PotionsAvailable { get; set; }
	//public AttackModifier AttackModifier { get; set; }
	public int HitsTakenPerBattle { get; set; } = 0;
	
}



public enum AttackType { Punch, BoneCrunch, Claw, MistyFist, ThunderBlast, Annihilator }
public enum CharacterType { VinFletcher, Tog }
public enum Category { Hero, Skeleton, Werewolf, TheUncodedOne }
public enum AttackModifierEnum { GoldenShield, SilverShield, NoShield }