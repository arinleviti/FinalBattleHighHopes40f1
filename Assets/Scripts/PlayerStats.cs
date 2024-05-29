using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerStats : CharacterClass
{

	public CharacterType CharacterType { get; set; }
	// Expose the fields in the Inspector
	[SerializeField]
	public int hp = 5;
	[SerializeField]
	private int maxHP = 50;
	[SerializeField]
	private string characterName = "X";
	[SerializeField]
	private List<AttackType> attackT = new List<AttackType>();
	[SerializeField]
	private Category characterCategory = Category.Hero;
	[SerializeField]
	private bool isDead = false;
	[SerializeField]
	private int potionsAvailable = 5;
	public void Awake()
	{
		// Initialize the properties
		HP = hp;
		MaxHP = maxHP;
		Name = characterName;
		AttackT = attackT;
		CharacterCategory = characterCategory;
		IsDead = isDead;
		PotionsAvailable = potionsAvailable;

	}
}
