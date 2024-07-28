using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	public Canvas canvas;
	private GameObject marker;
	public float movespeed = 5f;
	private Vector3 clickPosition;
	public RaycastHit Hit;
	private bool isUIManagerActivated = false;
	//private GameObject UIManagerRef;
	public GameObject attacker;
	private GameObject UIManagerPrefab;
	private UIManager UIManagerScript;
	private GameObject rangeIndicatorGO;
	public RangeIndicator rangeIndicatorScript;
	public CombatManager CombatManagerScript;
	private float radius;
	private Animator animator;
	private AnimScript animScriptS;

	private float threshold = 0.1f;
	private float maxDistanceFromMonster = 1.2f;

	public bool isWalking = false;
	public bool isIdle = false;
	private Vector3 direction = new Vector3();
	private bool isAnimatorSetup = false;
	private GameObject animatorObj;

	public GameObject midpoint;
	private NavMeshAgent navMeshAgentPlayer;
	private bool flag1 = false;
	private bool flag2 = false;
	private bool flag3 = false;
	private bool flag4 = false;
	private bool reachedTarget = false;
	private Vector3 pointA;
	private Rigidbody rb;

	private Vector2 Velocity;
	

	private bool attackerMustTurn = false;

	public float turnSpeed = 50;

	private Quaternion targetRotation;

	public Canvas canvasPotion;
	private GameObject canvasPotionGO;
	private GameObject actionChoicesGO;
	private ActionChoices actionChoicesScript;

	// Start is called before the first frame update
	void Start()
	{
		//isPlayerTurn = true;
		canvas = GameObject.Find("Canvas1").GetComponent<Canvas>();
		Debug.Log(canvas);
		marker = GameObject.Find("MarkerPrefab");
		//UIManagerRef = GameObject.FindGameObjectWithTag("UIManager");
		attacker = GameObject.Find("Player");
		marker.transform.position = new Vector3(attacker.transform.position.x, attacker.transform.position.y + 2, attacker.transform.position.z);
		canvas.enabled = false;

		rangeIndicatorGO = Instantiate(Resources.Load<GameObject>("Prefabs/RangeIndicatorPrefab"));
		rangeIndicatorGO.transform.position = new Vector3(attacker.transform.position.x, 0.5f, attacker.transform.position.z);
		rangeIndicatorScript = rangeIndicatorGO.GetComponent<RangeIndicator>();
		radius = rangeIndicatorGO.transform.localScale.x / 2;

		CombatManagerScript = GameObject.Find("CombatManager").GetComponent<CombatManager>();
		animator = GameObject.Find("OrkAssasin").GetComponent<Animator>();
		
		midpoint = GameObject.Find("Midpoint");
		animatorObj = GameObject.Find("AnimatorObj");
		animScriptS = animatorObj.GetComponent<AnimScript>();
		navMeshAgentPlayer = attacker.GetComponent<NavMeshAgent>();
		pointA = attacker.transform.position;
		ConfigureNavMeshAgent(navMeshAgentPlayer);
		if (attacker != null)
		{
			rb = attacker.GetComponent<Rigidbody>();
			rb.isKinematic = true;
		}
		CreatePotionButton();
		//UIManagerPrefab = GameObject.Find("Prefabs/UIManager");
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		
		marker.transform.position = new Vector3(attacker.transform.position.x, attacker.transform.position.y + 2, attacker.transform.position.z);
		ClickOnMonster();
		//if (movesLeft < 1) CombatManagerScript.playerTurnCompleted = true;
		

	}
	
	void ClickOnMonster()
	{
		if (!reachedTarget)
		{

			if (Input.GetMouseButtonDown(0) && !IsPointerOverUIElement())
			{

				clickPosition = new Vector3();
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;

				if (Physics.Raycast(ray, out hit))
				{
					clickPosition = hit.point;
					Hit = hit;
				}
			}
			if(navMeshAgentPlayer.hasPath)
			{
				Vector3 dir = (navMeshAgentPlayer.steeringTarget  -  attacker.transform.position).normalized;
				Vector3 animDir = attacker.transform.InverseTransformDirection(dir);
				attacker.transform.rotation = Quaternion.RotateTowards(attacker.transform.rotation, Quaternion.LookRotation(dir), 360 * Time.deltaTime);
			}
			
			

			if (clickPosition != Vector3.zero && Hit.collider.CompareTag("Monster") && rangeIndicatorScript.targetsInRange.Contains(Hit.transform.gameObject)
				&& Vector3.Distance(attacker.transform.position, Hit.transform.position) > maxDistanceFromMonster)
			/*Vector3.Distance(attacker.transform.position, new Vector3(clickPosition.x, attacker.transform.position.y, clickPosition.z)) > threshold*/
			{
				//isWalking = false;
				//animator.SetBool("IsWalking", isWalking);

				if (!flag1)
				{
					//direction = clickPosition - attacker.transform.position;
					//direction.Normalize();
					//direction.y = 0;
					//float distanceFromMonster1 = Vector3.Distance(attacker.transform.position, Hit.transform.position);
					//Debug.Log(distanceFromMonster1);
					//Debug.Log(maxDistanceFromMonster);
					rb.isKinematic = false;
					animScriptS.SetupWalking(attacker, animator);
					navMeshAgentPlayer.isStopped = false;
					navMeshAgentPlayer.SetDestination(Hit.transform.position);
					flag1 = true;
				}
			}
			//float distanceFromMonster = Vector3.Distance(attacker.transform.position, Hit.transform.position);
			//Debug.Log("This should be the max distance the player can ge tto the monster: "+distanceFromMonster);
			//attacker.transform.rotation = Quaternion.LookRotation(direction);

			//if (Vector3.Distance(attacker.transform.position, new Vector3(clickPosition.x, attacker.transform.position.y, clickPosition.z)) > threshold)
			//{
			//	//if (!isAnimatorSetup)
			//	//{
			//	direction = animScriptS.SetupWalking(attacker, animator, clickPosition);
			//	isAnimatorSetup = true;
			//	//}				
			//	navMeshAgentPlayer.SetDestination(direction);
			//	flag1 = true;


			//	//attacker.transform.position += direction * movespeed * Time.deltaTime;
			//}
			//else
			//{
			if (clickPosition != Vector3.zero && Hit.collider.CompareTag("Monster") && rangeIndicatorScript.targetsInRange.Contains(Hit.transform.gameObject) &&
				 Vector3.Distance(attacker.transform.position, Hit.transform.position) <= maxDistanceFromMonster)
			/*Vector3.Distance(attacker.transform.position, new Vector3(clickPosition.x, attacker.transform.position.y, clickPosition.z)) <= threshold*/
			{
				//animScriptS.SetupIdle();
				animScriptS.SetUpIdle(attacker, animator);
				rb.isKinematic = true;
				clickPosition = Vector3.zero;
				StopMoving();
				TurnToTarget(40, Hit.transform.gameObject);
				navMeshAgentPlayer.isStopped = true;
				reachedTarget = true;
				ActivateUIManager();
				//CombatManagerScript.playerTurnCompleted = true;
			}

			//}

			if (clickPosition != Vector3.zero && Hit.collider.CompareTag("RangeIndicator"))
			{


				navMeshAgentPlayer.isStopped = false;
				if (!flag2)
				{
					rb.isKinematic = false;
					navMeshAgentPlayer.SetDestination(clickPosition);
					animScriptS.SetupWalking(attacker, animator);

					flag2 = true;
				}





				//Vector3 direction = clickPosition - attacker.transform.position;
				/*direction.y = 0;*/ // Ensure rotation is only on the Y axis
									 //targetRotation = Quaternion.LookRotation(direction);
									 //attacker.transform.rotation = Quaternion.Lerp(attacker.transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
									 //}
									 //	flag2 = true; 
									 //}
									 //Debug.Log("Checkpoint");
									 //float distance = Vector3.Distance(attacker.transform.position, pointA);

				if (!navMeshAgentPlayer.pathPending && navMeshAgentPlayer.remainingDistance <= navMeshAgentPlayer.stoppingDistance && !flag3)
				{
					//StopMoving();
					rb.isKinematic = true;
					clickPosition = Vector3.zero;
					reachedTarget = true;
					animScriptS.SetUpIdle(attacker, animator);
					navMeshAgentPlayer.isStopped = true;
					//animScriptS.flag1 = true;
					//animScriptS.turnToTarget = true;
					//TurnToTarget(10f, midpoint.gameObject);
					flag3 = true;
					animScriptS.isRotating = true;
					CombatManagerScript.playerTurnCompleted = true;


					//StopMoving();
				}
				//if(flag4)
				//{

				//	Vector3 turnDirection = midpoint.transform.position - attacker.transform.position;
				//	Quaternion turnRotation = Quaternion.LookRotation(turnDirection);
				//	attacker.transform.rotation = Quaternion.Lerp(attacker.transform.rotation, turnRotation, Time.deltaTime * turnSpeed);

				//}

			}


			//attacker.transform.position += direction * movespeed * Time.deltaTime;

			//if (Vector3.Distance(attacker.transform.position, new Vector3(clickPosition.x, attacker.transform.position.y, clickPosition.z)) < thresholdRI)
			//{
			//	animScriptS.SetUpIdle(attacker, animator, midpoint);
			//	// Snaps the attacker to the target position
			//	//attacker.transform.position = new Vector3(clickPosition.x, attacker.transform.position.y, clickPosition.z);

			//	rangeIndicatorGO.transform.position = new Vector3(attacker.transform.position.x, 0.07f, attacker.transform.position.z);
			//	isWalking = false;


			//	CombatManagerScript.playerTurnCompleted = true;
			//	clickPosition = Vector3.zero;
			//	//Destroy(gameObject);
			//}





		}

	}

	void ActivateUIManager()
	{
		if (!isUIManagerActivated)
		{
			UIManagerPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/UIManagerPrefab"));
			isUIManagerActivated = true;
		}
	}

	private bool IsPointerOverUIElement()
	{
		return EventSystem.current.IsPointerOverGameObject();
	}
	private void TurnToTarget(float speed, GameObject target)
	{
		Vector3 direction = (target.transform.position - attacker.transform.position).normalized;
		Quaternion targetRotation = Quaternion.LookRotation(direction);
		attacker.transform.rotation = Quaternion.Lerp(attacker.transform.rotation, targetRotation, Time.deltaTime * speed);
	}

	private void StopMoving()
	{

		if (attacker != null)
		{
			if (navMeshAgentPlayer.isOnNavMesh)
			{
				navMeshAgentPlayer.isStopped = true; // Stop the agent from moving further
				navMeshAgentPlayer.velocity = Vector3.zero; // Ensure the velocity is zero to prevent sliding
			}
		}

	}

	private void ConfigureNavMeshAgent(NavMeshAgent agent)
	{
		agent.avoidancePriority = 50;
		/*agent.updatePosition = false;*///
		agent.updateRotation = true; //
		//agent.radius = 0.2f; //
		agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
		agent.enabled = true;
		//agent.speed = movespeed;
		agent.stoppingDistance = 0.1f;

	}
	//THis overrides the default RootMotion behaviour
	//private void OnAnimatorMove() //
	//{
	//	Vector3 rootPosition = animator.rootPosition;
	//	rootPosition.y = navMeshAgentPlayer.nextPosition.y;
	//	attacker.transform.position = rootPosition;
	//	//attacker.transform.rotation = animator.rootRotation;
	//	navMeshAgentPlayer.nextPosition = rootPosition;
	//}
	private void CreatePotionButton()
	{
		if (CombatManagerScript.currentTurn != null && CombatManagerScript.currentTurn.CompareTag("Player") && canvasPotionGO == null)
		{
			canvasPotionGO = Instantiate(Resources.Load<GameObject>("Prefabs/PotionCanvasPrefab"));
			canvasPotion = canvasPotionGO.GetComponent<Canvas>();
			canvasPotion.enabled = true;
			Button potionButton = canvasPotion.GetComponentInChildren<Button>();
			
			potionButton.onClick.AddListener(() => OnPotionButtonClick());
			
		}
		
	}
	private void OnPotionButtonClick()
	{
		//GameObject UIManagerGO = Instantiate(Resources.Load<GameObject>("Prefabs/UIManagerPrefab"));
		//UIManager UIManagerPrefabScript = UIManagerGO.GetComponent<UIManager>();
		//UIManagerPrefabScript.attackTypeChosen = AttackType.Potion;
		//ActivateUIManager();
		if (!GameObject.Find("ActionChoicesPrefab(Clone)"))
		{
			actionChoicesGO = Instantiate(Resources.Load<GameObject>("Prefabs/ActionChoicesPrefab"));
		}
		
		actionChoicesScript = actionChoicesGO.GetComponent<ActionChoices>();
		
		actionChoicesScript.HandleAttackChoice(AttackType.Potion);

	}
}
