using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MonstersController : MonoBehaviour
{
	public GameObject monster;
	public GameObject marker;
	public float movespeed = 3f;
	private GameObject UIManagerPrefab;
	public GameObject playerTarget;
	private CombatManager combatManagerRef;
	public bool reachedTarget = false;
	private bool isWalkSetup1 = false;
	private bool isWalkSetup2 = false;
	private bool isIdleSetup1 = false;
	private bool isIdleSetup2 = false;
	private AnimScript animScript;
	private Animator animatorZ1;
	private Animator animatorZ2;
	private GameObject rangeIndicatorGO;
	public RangeIndicator rangeIndicatorScript;
	private Vector3 pointA;
	private float radius;
	private GameObject currentTurn;
	private GameObject midpoint;
	private float threshold = 2f;
	private Vector3 directionToPlayer;
	private List<GameObject> rangeIndicatorList = new List<GameObject>();
	private bool rangeIndicatorListTransferred = false;

	// Start is called before the first frame update
	void Start()
	{
		playerTarget = GameObject.Find("Player");
		marker = GameObject.Find("Marker");
		combatManagerRef = GameObject.Find("CombatManager").GetComponent<CombatManager>();
		monster = combatManagerRef.currentTurn;
		marker = GameObject.Find("MarkerPrefab");
		marker.transform.position = new Vector3(monster.transform.position.x, monster.transform.position.y + 2, monster.transform.position.z);
		rangeIndicatorGO = Instantiate(Resources.Load<GameObject>("Prefabs/RangeIndicatorPrefab"));
		rangeIndicatorGO.transform.position = new Vector3(monster.transform.position.x, 0.5f, monster.transform.position.z);
		rangeIndicatorScript = rangeIndicatorGO.GetComponent<RangeIndicator>();
		radius = rangeIndicatorGO.transform.localScale.x / 2;
		pointA = monster.transform.position;
		animScript = GameObject.Find("AnimatorObj").GetComponent<AnimScript>();
		animatorZ1 = GameObject.Find("Zombie 1").GetComponentInChildren<Animator>();
		animatorZ2 = GameObject.Find("Zombie 2").GetComponentInChildren<Animator>();
		midpoint = GameObject.Find("Midpoint");
		
	}

	// Update is called once per frame
	void Update()
	{
		
		//rangeIndicatorList = rangeIndicatorScript.targetsInRange;
		marker.transform.position = new Vector3(monster.transform.position.x, monster.transform.position.y + 2, monster.transform.position.z);
		midpoint = GameObject.Find("Midpoint");
		//rangeIndicatorList = rangeIndicatorScript.targetsInRange;
		
		if (!rangeIndicatorListTransferred)
		{
			rangeIndicatorList = rangeIndicatorScript.targetsInRange;
			rangeIndicatorListTransferred = true;
		}
		ApproachPlayer();
		//if (movesLeft < 1) combatManagerRef.monsterTurnCompleted = true;
	}
	private void ApproachPlayer()
	{
		if (!reachedTarget)
		{
			//Vector3 directionToPlayer = playerTarget.transform.position - monster.transform.position;
			//directionToPlayer.Normalize();

			Debug.Log($"ApproachPlayer - Checking if playerTarget {playerTarget.name} is in range. InstanceID: {playerTarget.GetInstanceID()}");
			// Check if the player is within range and attack without moving
			//bool playerInRangeByName = rangeIndicatorList.Any(obj => obj.name == "Player");
			//bool playerInRange = rangeIndicatorList.Contains(playerTarget);
			if (!rangeIndicatorList.Contains(playerTarget))
			{
				Debug.Log($"Player target {playerTarget.name} (InstanceID: {playerTarget.GetInstanceID()}) is not in targetsInRange.");
				GameObject zombie1 = GameObject.Find("Zombie 1");
				GameObject zombie2 = GameObject.Find("Zombie 2");
				// Move towards the player
				if (!isWalkSetup1)
				{
					if (monster != null && monster == zombie1)
					{
						animScript.SetupWalking(monster, animatorZ1, playerTarget.transform.position);
						isWalkSetup1 = true;
					}
					else if (monster != null && monster == zombie2)
					{
						animScript.SetupWalking(monster, animatorZ2, playerTarget.transform.position);
						isWalkSetup1 = true;
					}
				}
				directionToPlayer = playerTarget.transform.position - monster.transform.position;
				monster.transform.position += directionToPlayer * movespeed * Time.deltaTime;
				float distanceToPlayer = Vector3.Distance(playerTarget.transform.position, monster.transform.position);
				float distanceTravelled = Vector3.Distance(monster.transform.position, pointA);

				if (distanceToPlayer < threshold || distanceTravelled >= radius)
				{
					// Stop when within 2 units from the player
					//rangeIndicatorGO.transform.position = new Vector3(monster.transform.position.x, 0.07f, monster.transform.position.z);
					if (!isIdleSetup1)
					{
						if (monster != null && monster == zombie1)
						{
							animScript.SetUpIdle(monster, animatorZ1, midpoint);
							isIdleSetup1 = true;
						}
						else if (monster != null && monster == zombie2)
						{
							animScript.SetUpIdle(monster, animatorZ2, midpoint);
							isIdleSetup1 = true;
						}
					}
					reachedTarget = true;
					combatManagerRef.monsterTurnCompleted = true;
				}
				//else if (distanceTravelled >= radius)
				//{
				//	// Stop when the monster has moved as far as it can in one move
				//	//rangeIndicatorGO.transform.position = new Vector3(monster.transform.position.x, 0.07f, monster.transform.position.z);
				//	if (!isIdleSetup)
				//	{
				//		if (monster == GameObject.Find("Zombie 1") && monster != null)
				//		{
				//			animScript.SetUpIdle(monster, animatorZ1, midpoint);
				//			isIdleSetup = true;
				//		}
				//		else if (monster == GameObject.Find("Zombie 2") && monster != null)
				//		{
				//			animScript.SetUpIdle(monster, animatorZ2, midpoint);
				//			isIdleSetup = true;
				//		}
				//	}
				//	reachedTarget = true;
				//	combatManagerRef.monsterTurnCompleted = true;
				//}
			}

			else if (rangeIndicatorList.Contains(playerTarget) && Vector3.Distance(monster.transform.position, playerTarget.transform.position) >= threshold)
			{
				// Attack if within range
				//rangeIndicatorGO.transform.position = new Vector3(monster.transform.position.x, 0.07f, monster.transform.position.z);
				directionToPlayer = playerTarget.transform.position - monster.transform.position;
				directionToPlayer.Normalize();

				if (!isWalkSetup2)
				{
					if (monster == GameObject.Find("Zombie 1") && monster != null)
					{
						animScript.SetupWalking(monster, animatorZ1, playerTarget.transform.position);
						isWalkSetup2 = true;
					}
					else if (monster == GameObject.Find("Zombie 2") && monster != null)
					{
						animScript.SetupWalking(monster, animatorZ2, playerTarget.transform.position);
						isWalkSetup2 = true;
					}
				}

				monster.transform.position += directionToPlayer * movespeed * Time.deltaTime;
			}
			else if (rangeIndicatorList.Contains(playerTarget) && Vector3.Distance(monster.transform.position, playerTarget.transform.position) < threshold)
			{
				if (!isIdleSetup2)
				{
					if (monster == GameObject.Find("Zombie 1") && monster != null)
					{
						animScript.SetUpIdle(monster, animatorZ1, midpoint);
						isIdleSetup2 = true;

					}
					else if (monster == GameObject.Find("Zombie 2") && monster != null)
					{
						animScript.SetUpIdle(monster, animatorZ2, midpoint);
						isIdleSetup2 = true;

					}
					//UpdateUIManager();
					reachedTarget = true;
				}

				UpdateUIManager();
			}


			
		}




	}


	private void UpdateUIManager()
	{

		if (!GameObject.Find("UIManagerPrefab(Clone)"))
		{

			UIManagerPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/UIManagerPrefab"));

			Debug.Log("Is UIManagerPrefab for the monster instantiated?" + UIManagerPrefab.name);
		}



	}

	// Sometimes the monster gets stuck in limbo if the player is too close to the range indicator and the monster can't complete its move.
	public void StopThere()
	{
		combatManagerRef.monsterTurnCompleted = true;
	}


}
