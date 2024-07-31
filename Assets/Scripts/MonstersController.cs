using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
//using UnityEditor.Experimental.GraphView;
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
	private AnimScript animScriptS;
	private Animator animatorZ1;
	private Animator animatorZ2;
	private GameObject rangeIndicatorGO;
	public RangeIndicator rangeIndicatorScript;
	private Vector3 pointA;
	private float radius;
	private GameObject currentTurn;
	private GameObject midpoint;

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
	private Rigidbody zombie1RB;
	private Rigidbody zombie2RB;
	
	

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
		zombie1GO = GameObject.Find("Zombie 1");
		zombie2GO = GameObject.Find("Zombie 2");
		
		if (zombie1GO != null)
		{
			animatorZ1 = zombie1GO.GetComponentInChildren<Animator>();
			zombie1RB = zombie1GO.GetComponent<Rigidbody>();
		}
		if (zombie2GO != null)
		{
			animatorZ2 = zombie2GO.GetComponentInChildren<Animator>();
			zombie2RB = zombie2GO.GetComponent<Rigidbody>();
		}
		animScriptS = GameObject.Find("AnimatorObj").GetComponent<AnimScript>();
		midpoint = GameObject.Find("Midpoint");
		currentTurn = combatManagerRef.currentTurn;
		//
		if (currentTurn != null && currentTurn == zombie1GO && zombie2GO != null)
		{
			SetupNavMeshAgent(zombie1GO, true, out navMeshAgentZ1);
			SetupNavMeshObst(zombie1GO, false, out navMeshObstacleZ1);
			SetupNavMeshAgent(zombie2GO, false, out navMeshAgentZ2); //RESUME
			SetupNavMeshObst(zombie2GO, true, out navMeshObstacleZ2);
			zombie1RB.isKinematic = false;
			zombie2RB.isKinematic = true;
		}
		else if (currentTurn != null && currentTurn == zombie2GO & zombie1GO != null)
		{
			SetupNavMeshAgent(zombie2GO, true, out navMeshAgentZ2);
			SetupNavMeshObst(zombie2GO, false, out navMeshObstacleZ2);
			SetupNavMeshAgent(zombie1GO, false , out navMeshAgentZ1);
			SetupNavMeshObst(zombie1GO, true, out navMeshObstacleZ2);
			zombie2RB.isKinematic = false;
			zombie1RB.isKinematic = true;
		}
		else if (currentTurn != null && currentTurn == zombie1GO && zombie2GO == null)
		{
			SetupNavMeshAgent(zombie1GO, true, out navMeshAgentZ1);
			SetupNavMeshObst(zombie1GO, false, out navMeshObstacleZ1);
			zombie1RB.isKinematic = false;
		}
		else if (currentTurn != null && currentTurn == zombie2GO && zombie1GO == null)
		{
			SetupNavMeshAgent(zombie2GO, true, out navMeshAgentZ2);
			SetupNavMeshObst(zombie2GO, false, out navMeshObstacleZ2);
			zombie2RB.isKinematic = false;
		}
		
		GameObject PotionCanvas =GameObject.Find("PotionCanvasPrefab(Clone)");
		if (PotionCanvas != null)
		{
			Destroy(PotionCanvas);
		}
		


	}
	private void SetupNavMeshAgent(GameObject zombieGO, bool isActive, out NavMeshAgent navMeshAgent)
	{
		NavMeshAgent agent = zombieGO.GetComponent<NavMeshAgent>();
		agent.enabled = isActive;
		agent.speed = movespeed;
		
		agent.isStopped = false;
		//agent.stoppingDistance = 1.2f;
		navMeshAgent = agent;
		ConfigureNavMeshAgent(navMeshAgent);
		
	}
	private void SetupNavMeshObst(GameObject zombie, bool isActive, out NavMeshObstacle navMeshObstacle)
	{
		NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
		obstacle = zombie.GetComponent<NavMeshObstacle>();
		obstacle.enabled = isActive;
		navMeshObstacle = obstacle;
		ConfigureNavMeshObstacle(navMeshObstacle);
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
			 distanceToPlayer = Vector3.Distance(monster.transform.position, playerTarget.transform.position);
			if (!isWalkSetup1 && distanceToPlayer > 1.5f)
			{
				if (monster != null && monster == zombie1GO)
				{
					animScriptS.SetupWalkingMonster(monster, animatorZ1);
					isWalkSetup1 = true;
				}
				else if (monster != null && monster == zombie2GO)
				{
					animScriptS.SetupWalkingMonster(monster, animatorZ2);
					isWalkSetup1 = true;
				}
			}

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

				
				distanceToPlayer = Vector3.Distance(playerTarget.transform.position, monster.transform.position);
				float distanceTravelled = Vector3.Distance(monster.transform.position, pointA);

				if (/*distanceToPlayer < threshold || */distanceTravelled >= radius)
				{					

					if (!isIdleSetup1)
					{
						if (monster != null && monster == zombie1GO)
						{
							animScriptS.SetUpIdle(monster, animatorZ1);
							isIdleSetup1 = true;
						}
						else if (monster != null && monster == zombie2GO)
						{
							animScriptS.SetUpIdle(monster, animatorZ2);
							isIdleSetup1 = true;
						}
					}
					reachedTarget = true;

					TurnToPlayer(50);
				
					ResetNavMesh();
					StopMoving();
					combatManagerRef.monsterTurnCompleted = true;

				}

			

			if (rangeIndicatorList.Contains(playerTarget) && Vector3.Distance(monster.transform.position, playerTarget.transform.position) >= 1.5f /*&& !flag0*/)
			{
				
				if (!isWalkSetup2)
				{
					if (monster == GameObject.Find("Zombie 1") && monster != null)
					{
						animScriptS.SetupWalkingMonster(monster, animatorZ1);
						isWalkSetup2 = true;
					}
					else if (monster == GameObject.Find("Zombie 2") && monster != null)
					{
						animScriptS.SetupWalkingMonster(monster, animatorZ2);
						isWalkSetup2 = true;
					}
				}
				
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

				
				
			}
			if (monster != null && monster == zombie1GO && navMeshAgentZ1 != null)
			{
				if (!navMeshAgentZ1.pathPending && navMeshAgentZ1.isActiveAndEnabled && navMeshAgentZ1.isOnNavMesh && navMeshAgentZ1.remainingDistance <= 1.2f)
				{
					flag0 = true;


					reachedTarget = true;
					
					ResetNavMesh();

					TurnToPlayer(50);
					//animScriptS.isRotating = true;
					UpdateUIManager();
					StopMoving();
					Debug.Log("It's blocking here 2");
				}
			}
			else if (monster != null && monster == zombie2GO && navMeshAgentZ2 != null)
			{
				if (!navMeshAgentZ2.pathPending && navMeshAgentZ2.isActiveAndEnabled && navMeshAgentZ2.isOnNavMesh && navMeshAgentZ2.remainingDistance <= 1.2f)
				{
					flag0 = true;


					reachedTarget = true;
					
					ResetNavMesh();
					
					TurnToPlayer(50);
					UpdateUIManager();
					StopMoving();
					Debug.Log("It's blocking here 2");

				}
			}
			
			
		}
	}
	private void TurnToPlayer(float speed)
	{
		Vector3 direction = (playerTarget.transform.position - monster.transform.position).normalized;
		Quaternion targetRotation = Quaternion.LookRotation(direction);
		monster.transform.rotation = Quaternion.Lerp(monster.transform.rotation, targetRotation, Time.deltaTime * speed);
	}
	private void StopMoving()
	{
		monster.GetComponent<Rigidbody>().isKinematic = true;
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
		NavMeshAgent[] navMeshAgents = { navMeshAgentZ1, navMeshAgentZ2 };
		NavMeshObstacle[] navMeshObstacles = { navMeshObstacleZ1, navMeshObstacleZ2 };

		foreach (var agent in navMeshAgents)
		{
			if (agent != null)
			{
				agent.enabled = false;
			}
		}

		foreach (var obstacle in navMeshObstacles)
		{
			if (obstacle != null)
			{
				obstacle.enabled = false;
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

	private void ConfigureNavMeshAgent(NavMeshAgent agent)
	{
		agent.avoidancePriority = 50;
		//agent.radius = 0.2f;  //
		//agent.height = 2.0f;  //
		//agent.angularSpeed = 10000f;  //
		agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
		agent.autoBraking = true;
	}

	private void ConfigureNavMeshObstacle(NavMeshObstacle obstacle)
	{
		obstacle.carving = true;
		obstacle.shape = NavMeshObstacleShape.Capsule;
		obstacle.center = Vector3.zero;
		//obstacle.size = new Vector3(1.0f, 2.0f, 1.0f);
	}
	


}
