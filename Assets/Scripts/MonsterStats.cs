using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStats : CharacterClass
{

	public CharacterType CharacterType { get; set; }
	// Expose the fields in the Inspector
	[SerializeField]
	public int hp = 10;
	[SerializeField]
	private int maxHP = 10;
	[SerializeField]
	private string characterName = "MonsterX";
	[SerializeField]
	private List<AttackType> attackT = new List<AttackType>();
	[SerializeField]
	private Category characterCategory = Category.Skeleton;
	[SerializeField]
	private bool isDead = false;
	[SerializeField]
	private int potionsAvailable = 1;
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

	//private void OnCollisionEnter(Collision collision)
	//{
	//	if (collision.gameObject.CompareTag("Player"))
	//	{
	//		// Find the MonstersController and call StopThere
	//		MonstersController monstersController = GameObject.Find("MonstersControllerPrefab(Clone)").GetComponent<MonstersController>();
	//		monstersController.StopThere();
	//	}
	//}

	
}

