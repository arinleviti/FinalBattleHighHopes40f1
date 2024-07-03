using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class MonstersController : MonoBehaviour
{
	public GameObject monster;
	private GameObject zombie1GO;
	private GameObject zombie2GO;
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
	private float threshold = 1.5f;
	private Vector3 directionToPlayer;
	private List<GameObject> rangeIndicatorList = new List<GameObject>();
	private bool rangeIndicatorListTransferred = false;
	//private Rigidbody rb;
	private NavMeshAgent navMeshAgentZ1;
	private NavMeshAgent navMeshAgentZ2;
	private NavMeshObstacle navMeshObstacleZ2;
	private NavMeshObstacle navMeshObstacleZ1;
	private bool navMeshFlag1 = false;
	private bool navMeshFlag2 = false;
	private bool flag0 = false;
	private bool flag1 = false;
	private bool flag2 = false;
	private float distanceToPlayer;
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
		zombie1GO = GameObject.Find("Zombie 1");
		zombie2GO = GameObject.Find("Zombie 2");
		currentTurn = combatManagerRef.currentTurn;
		if (currentTurn != null && currentTurn == zombie1GO)
		{
			navMeshAgentZ1 = zombie1GO.GetComponent<NavMeshAgent>();
			navMeshAgentZ1.enabled = true;
			navMeshAgentZ2 = zombie2GO.GetComponent<NavMeshAgent>();
			navMeshAgentZ2.enabled = false;
			navMeshObstacleZ2 = zombie2GO.GetComponent<NavMeshObstacle>();
			navMeshObstacleZ2.enabled = true;
			navMeshObstacleZ1 = zombie1GO.GetComponent<NavMeshObstacle>();
			navMeshObstacleZ1.enabled = false;
			navMeshAgentZ1.speed = movespeed;
			navMeshAgentZ1.stoppingDistance = threshold;
			navMeshAgentZ1.isStopped = false;
			navMeshAgentZ1.stoppingDistance = 1f;
			ConfigureNavMeshAgent(navMeshAgentZ1);
			ConfigureNavMeshObstacle(navMeshObstacleZ1);
		}
		else if (currentTurn != null && currentTurn == zombie2GO)
		{
			navMeshAgentZ2 = zombie2GO.GetComponent<NavMeshAgent>();
			navMeshAgentZ2.enabled = true;
			navMeshAgentZ1 = zombie1GO.GetComponent<NavMeshAgent>();
			navMeshAgentZ1.enabled = false;
			navMeshObstacleZ1 = zombie1GO.GetComponent<NavMeshObstacle>();
			navMeshObstacleZ1.enabled = true;
			navMeshObstacleZ2 = zombie2GO.GetComponent <NavMeshObstacle>();
			navMeshObstacleZ2.enabled = false;
			navMeshAgentZ2.speed = movespeed;
			navMeshAgentZ2.stoppingDistance = threshold;
			navMeshAgentZ2.isStopped = false;
			navMeshAgentZ2.stoppingDistance = 1f;
			ConfigureNavMeshAgent(navMeshAgentZ2);
			ConfigureNavMeshObstacle(navMeshObstacleZ2);
		}
		//// Set collision detection mode to Continuous to prevent tunneling
		//rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
		//navMeshAgent = monster.GetComponent<NavMeshAgent>();
		//if (navMeshAgent == null)
		//{
		//	navMeshAgent = monster.AddComponent<NavMeshAgent>();
		//}
		
		//navMeshAgent.speed = movespeed;
		//navMeshAgent.stoppingDistance = threshold; 
		// Ensure the stopping distance is set
												   //var rb = monster.GetComponent<Rigidbody>();
												   //if (rb != null )
												   //{
												   //	Destroy(rb);
												   //}

		// Reset the NavMeshAgent properties
		/*navMeshAgent.isStopped = false;*/  // Ensure the agent is not stopped

		//ConfigureNavMeshAgent(navMeshAgent);
		//ConfigureNavMeshObstacle(navMeshObstacle);
		//navMeshObstacle.enabled = false;
		//navMeshAgent.enabled = true;

	}

	// Update is called once per frame
	void FixedUpdate()
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
				//GameObject zombie1 = GameObject.Find("Zombie 1");
				//GameObject zombie2 = GameObject.Find("Zombie 2");
				// Move towards the player
				if (!isWalkSetup1)
				{
					if (monster != null && monster == zombie1GO)
					{
						animScript.SetupWalkingMonster(monster, animatorZ1, playerTarget.transform.position);
						isWalkSetup1 = true;
					}
					else if (monster != null && monster == zombie2GO)
					{
						animScript.SetupWalkingMonster(monster, animatorZ2, playerTarget.transform.position);
						isWalkSetup1 = true;
					}
				}
				//navMeshAgent.enabled = true;
				//navMeshObstacle.enabled = false;
				if (monster != null && monster == zombie1GO && !navMeshFlag1)
				{
					navMeshAgentZ1.SetDestination(playerTarget.transform.position);
					navMeshFlag1 = true;
				}
				else if (monster != null && monster == zombie2GO && !navMeshFlag1)
				{
					navMeshAgentZ2.SetDestination(playerTarget.transform.position);
					navMeshFlag1 = true;
				}

				//directionToPlayer = playerTarget.transform.position - monster.transform.position;
				//directionToPlayer.Normalize();

				////added:
				//Vector3 newPosition = rb.position + directionToPlayer * movespeed * Time.deltaTime;
				//rb.MovePosition(newPosition);

				//deleted:
				//monster.transform.position += directionToPlayer * movespeed * Time.deltaTime;
				distanceToPlayer = Vector3.Distance(playerTarget.transform.position, monster.transform.position);
				float distanceTravelled = Vector3.Distance(monster.transform.position, pointA);

				if (distanceToPlayer < threshold || distanceTravelled >= radius)
				{
					// Stop when within 2 units from the player
					//rangeIndicatorGO.transform.position = new Vector3(monster.transform.position.x, 0.07f, monster.transform.position.z);


					if (!isIdleSetup1)
					{
						if (monster != null && monster == zombie1GO)
						{
							animScript.SetUpIdle(monster, animatorZ1, midpoint);
							isIdleSetup1 = true;
						}
						else if (monster != null && monster == zombie2GO)
						{
							animScript.SetUpIdle(monster, animatorZ2, midpoint);
							isIdleSetup1 = true;
						}
					}
					reachedTarget = true;
					StopMoving();
					ResetNavMesh();
					combatManagerRef.monsterTurnCompleted = true;

				}

			}

			if (rangeIndicatorList.Contains(playerTarget) /*&& Vector3.Distance(monster.transform.position, playerTarget.transform.position) >= threshold*/ /*&& !flag0*/)
			{
				
				
				//directionToPlayer = playerTarget.transform.position - monster.transform.position;
				//directionToPlayer.Normalize();

				if (!isWalkSetup2)
				{
					if (monster == GameObject.Find("Zombie 1") && monster != null)
					{
						animScript.SetupWalkingMonster(monster, animatorZ1, playerTarget.transform.position);
						isWalkSetup2 = true;
					}
					else if (monster == GameObject.Find("Zombie 2") && monster != null)
					{
						animScript.SetupWalkingMonster(monster, animatorZ2, playerTarget.transform.position);
						isWalkSetup2 = true;
					}
				}
				//navMeshAgent.isStopped = false;
				//navMeshAgent.enabled = true;
				//navMeshObstacle.enabled = false;
				if (monster != null && monster == zombie1GO && !navMeshFlag2)
				{
					navMeshAgentZ1.SetDestination(playerTarget.transform.position);
					navMeshFlag2 = true;
				}
				else if (monster != null && monster == zombie2GO && !navMeshFlag2)
				{
					navMeshAgentZ2.SetDestination(playerTarget.transform.position);
					navMeshFlag2 = true;
				}
				
				Debug.Log("It's blocking here 1");

				
				// deleted:
				//monster.transform.position += directionToPlayer * movespeed * Time.deltaTime;

				//added:
				//Vector3 newPosition = rb.position + directionToPlayer * movespeed * Time.deltaTime;
				//rb.MovePosition(newPosition);
			}
			if ((monster != null && monster == zombie1GO && !navMeshAgentZ1.pathPending && navMeshAgentZ1.remainingDistance <= 1f) ||
				(monster != null && monster == zombie2GO && !navMeshAgentZ2.pathPending && navMeshAgentZ2.remainingDistance <= 1f))
				/*(rangeIndicatorList.Contains(playerTarget) && Vector3.Distance(monster.transform.position, playerTarget.transform.position) < threshold)*/
			{
				//if (!isIdleSetup2)
				//{
					//if (monster == GameObject.Find("Zombie 1") && monster != null)
					//{
					//	animScript.SetUpIdle(monster, animatorZ1, midpoint);
					//	isIdleSetup2 = true;

					//}
					//if (monster == GameObject.Find("Zombie 2") && monster != null)
					//{
					//	animScript.SetUpIdle(monster, animatorZ2, midpoint);
					//	isIdleSetup2 = true;

					//}
					//UpdateUIManager();
					//reachedTarget = true;
					//StopMoving();
					//ResetNavMesh();
				flag0 = true;

				
				reachedTarget = true;
				StopMoving();
				ResetNavMesh();
				UpdateUIManager();
				Debug.Log("It's blocking here 2");
				
			}
			//else Debug.Log("None of the conditions can be met");
		}
	}
	private void StopMoving()
	{
		if (monster != null && monster == zombie1GO)
		{
			if (navMeshAgentZ1.isOnNavMesh)
			{
				navMeshAgentZ1.isStopped = true; // Stop the agent from moving further
				navMeshAgentZ1.velocity = Vector3.zero; // Ensure the velocity is zero to prevent sliding
			}
		}
		else if (monster != null && monster == zombie2GO)
		{
			if (navMeshAgentZ2.isOnNavMesh)
			{
				navMeshAgentZ2.isStopped = true; // Stop the agent from moving further
				navMeshAgentZ2.velocity = Vector3.zero; // Ensure the velocity is zero to prevent sliding
			}
		}
	}
	private void ResetNavMesh()
	{
		navMeshAgentZ1.enabled = false;
		navMeshAgentZ2.enabled = false;
		navMeshObstacleZ1.enabled = false;
		navMeshObstacleZ2.enabled = false;
	}
	private void UpdateUIManager()
	{

		if (!GameObject.Find("UIManagerPrefab(Clone)"))
		{

			UIManagerPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/UIManagerPrefab"));

			Debug.Log("Is UIManagerPrefab for the monster instantiated?" + UIManagerPrefab.name);
		}



	}

	private void ConfigureNavMeshAgent(NavMeshAgent agent)
	{
		agent.avoidancePriority = 50;
		agent.radius = 0.5f;
		agent.height = 2.0f;
		agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
		agent.autoBraking = true;
	}

	private void ConfigureNavMeshObstacle(NavMeshObstacle obstacle)
	{
		obstacle.carving = true;
		obstacle.shape = NavMeshObstacleShape.Capsule;
		obstacle.center = Vector3.zero;
		obstacle.size = new Vector3(1.0f, 2.0f, 1.0f);
	}
	// Sometimes the monster gets stuck in limbo if the player is too close to the range indicator and the monster can't complete its move.
	//public void StopThere()
	//{
	//	combatManagerRef.monsterTurnCompleted = true;
	//}


}
