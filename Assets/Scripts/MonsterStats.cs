using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Experimental.GraphView;
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
	private Rigidbody rb;
	private Vector3 initialPosition;
	private float distanceMoved;
	//public float pushForce = 10f;
	public float maxDistanceToMove = 2f;
	public float moveSpeed = 5f;
	public float pushForce =1000f; // Adjust this to control how far aside the zombie moves
	public float freezeDuration = 5.0f;
	private Vector3 originalDirection;
	private bool isHandlingCollision = false;
	private GameObject player;
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
		player = GameObject.Find("Player");
		rb = GetComponent<Rigidbody>();
		initialPosition = transform.position;
		distanceMoved = 0f;
		originalDirection = (GameObject.Find("Player").transform.position - transform.position).normalized;

	}

	

	public void Update()
	{
		// Keep the monster upright
		transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
	}
	public void FixedUpdate()
	{


		//if (player != null)
		//{
		//	transform.LookAt(player.transform);
		//}

		//Vector3 directionToPlayer = (GameObject.Find("Player").transform.position - transform.position).normalized;


		////Track the distance moved since the collision
		//distanceMoved = Vector3.Distance(transform.position, initialPosition);

		//// Stop moving if the distance moved exceeds the maximum allowed distance
		//if (distanceMoved >= maxDistanceToMove)
		//{
		//	rb.velocity = Vector3.zero;
		//	//rotation speed.
		//	rb.angularVelocity = Vector3.zero;
		//}


	}
	

}

