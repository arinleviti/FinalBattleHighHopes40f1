using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MonsterStats : CharacterClass
{

	public CharacterType CharacterType { get; set; }
	// Expose the fields in the Inspector
	[SerializeField]
	public int hp ;
	[SerializeField]
	public int maxHP;
	[SerializeField]
	private string characterName = "MonsterX";
	[SerializeField]
	private List<AttackType> attackT = new List<AttackType>();
	[SerializeField]
	private Category characterCategory = Category.Skeleton;
	
	[SerializeField]
	private int potionsAvailable;
	
	public void Awake()
	{
		// Initialize the properties
		HP = hp;
		MaxHP = maxHP;
		Name = characterName;
		AttackT = attackT;
		CharacterCategory = characterCategory;
	}

	

	public void Update()
	{
		// Keep the monster upright
		transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
	}
}

