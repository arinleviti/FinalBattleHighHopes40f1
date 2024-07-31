using System.Collections;
using System.Collections.Generic;
using System.Net;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Timeline;
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]

public class NewPlayerController : MonoBehaviour
{
	private GameObject attacker;
	public Canvas canvas;
	private GameObject marker;
	private Vector3 clickPosition;
	public RaycastHit Hit;
	private bool isUIManagerActivated = false;
	private GameObject rangeIndicatorGO;
	public RangeIndicator rangeIndicatorScript;
	public CombatManager CombatManagerScript;
	private float radius;
	private Animator animator;
	private AnimScript animScriptS;
	public GameObject midpoint;
	private NavMeshAgent navMeshAgentPlayer;
	private GameObject UIManagerPrefab;
	private GameObject animatorObj;
	enum Mode { AgentControlsPosition, AnimatorControlsPosition } //
	[SerializeField] Mode mode = Mode.AnimatorControlsPosition; //
	[SerializeField] string isMovingParameterName = "Moving"; //
	[SerializeField] string sidewaysSpeedParameterName = "X Speed"; //
	[SerializeField] string forwardSpeedParameterName = "Z Speed"; //
	private Vector2 SmoothDeltaPosition = Vector2.zero; //

	private Rigidbody rb;
	private bool flag1 = false;
	private bool flag2 = false;
	private bool flag3 = false;
	private float maxDistanceFromMonster = 1.2f;

	// Start is called before the first frame update
	void Start()
    {
		attacker = GameObject.Find("Player");
		marker = GameObject.Find("MarkerPrefab");
		//UIManagerRef = GameObject.FindGameObjectWithTag("UIManager");
		canvas = GameObject.Find("Canvas1").GetComponent<Canvas>();
		marker.transform.position = new Vector3(attacker.transform.position.x, attacker.transform.position.y + 2, attacker.transform.position.z);
		canvas.enabled = false;
		rangeIndicatorGO = Instantiate(Resources.Load<GameObject>("Prefabs/RangeIndicatorPrefab"));
		rangeIndicatorGO.transform.position = new Vector3(attacker.transform.position.x, 0.5f, attacker.transform.position.z);
		rangeIndicatorScript = rangeIndicatorGO.GetComponent<RangeIndicator>();
		radius = rangeIndicatorGO.transform.localScale.x / 2;
		navMeshAgentPlayer = attacker.GetComponent<NavMeshAgent>();
		CombatManagerScript = GameObject.Find("CombatManager").GetComponent<CombatManager>();
		animator = GameObject.Find("OrkAssasin").GetComponent<Animator>();
		//animator.applyRootMotion = true; //
		midpoint = GameObject.Find("Midpoint");
		animatorObj = GameObject.Find("AnimatorObj");
		animScriptS = animatorObj.GetComponent<AnimScript>();
		ConfigureNavMeshAgent(navMeshAgentPlayer);
		if (attacker != null)
		{
			rb = attacker.GetComponent<Rigidbody>();
			rb.isKinematic = true;
		}
		navMeshAgentPlayer.updatePosition = false;
	}

	// Update is called once per frame
	void Update()
    {
		if (Input.GetMouseButtonDown(0))
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

		if (navMeshAgentPlayer.hasPath)
		{
			Vector3 worldDeltaPosition = navMeshAgentPlayer.nextPosition - attacker.transform.position; //
			float xMovement = Vector3.Dot(attacker.transform.right, worldDeltaPosition); //
			float zMovement = Vector3.Dot(attacker.transform.forward, worldDeltaPosition); //
			Vector2 localDeltaPosition = new Vector2(xMovement, zMovement); //
			float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f); //
			SmoothDeltaPosition = Vector2.Lerp(SmoothDeltaPosition, localDeltaPosition, smooth); //
			var velocity = SmoothDeltaPosition / Time.deltaTime; //
			bool shouldMove = velocity.magnitude > 0.5f && navMeshAgentPlayer.remainingDistance > navMeshAgentPlayer.radius; //

			animator.SetBool(isMovingParameterName, shouldMove); //
			animator.SetFloat(sidewaysSpeedParameterName, velocity.x); //
			animator.SetFloat(forwardSpeedParameterName, velocity.y); //

			if (mode == Mode.AnimatorControlsPosition)  //
			{
				if (worldDeltaPosition.magnitude > navMeshAgentPlayer.radius)
				{
					attacker.transform.position = Vector3.Lerp(attacker.transform.position, navMeshAgentPlayer.nextPosition, Time.deltaTime / 0.15f);
				}
			}
			
		}
		
		if (clickPosition != Vector3.zero && Hit.collider.CompareTag("Monster") && rangeIndicatorScript.targetsInRange.Contains(Hit.transform.gameObject)
				&& Vector3.Distance(attacker.transform.position, Hit.transform.position) > maxDistanceFromMonster)
		{
			if (!flag1)
			{				
				rb.isKinematic = false;			
				navMeshAgentPlayer.isStopped = false;
				navMeshAgentPlayer.SetDestination(Hit.transform.position);
				flag1 = true;
			}
		}
		if (clickPosition != Vector3.zero && Hit.collider.CompareTag("Monster") && rangeIndicatorScript.targetsInRange.Contains(Hit.transform.gameObject) &&
				 Vector3.Distance(attacker.transform.position, Hit.transform.position) <= maxDistanceFromMonster)
		
		{
			
			animScriptS.SetUpIdle(attacker, animator);
			rb.isKinematic = true;
			clickPosition = Vector3.zero;
			StopMoving();
			TurnToTarget(40, Hit.transform.gameObject);
			navMeshAgentPlayer.isStopped = true;
			//reachedTarget = true;
			ActivateUIManager();
			
		}

		if (clickPosition != Vector3.zero && Hit.collider.CompareTag("RangeIndicator"))
		{


			navMeshAgentPlayer.isStopped = false;
			if (!flag2)
			{
				rb.isKinematic = false;
				navMeshAgentPlayer.SetDestination(clickPosition);
				//animScriptS.SetupWalking(attacker, animator);

				flag2 = true;
			}

			if (!navMeshAgentPlayer.pathPending && navMeshAgentPlayer.remainingDistance <= navMeshAgentPlayer.stoppingDistance && !flag3)
			{
				
				rb.isKinematic = true;
				clickPosition = Vector3.zero;
				//reachedTarget = true;
				animScriptS.SetUpIdle(attacker, animator);
				navMeshAgentPlayer.isStopped = true;
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
	}
	private void OnAnimatorMove()
	{
		switch(mode)
		{
			case Mode.AgentControlsPosition:
				attacker.transform.position = navMeshAgentPlayer.nextPosition;
				break;
			case Mode.AnimatorControlsPosition:
				Vector3 position = animator.rootPosition;
				position.y = navMeshAgentPlayer.nextPosition.y;
				attacker.transform.position = position;
				break;
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
	private void TurnToTarget(float speed, GameObject target)
	{
		Vector3 direction = (target.transform.position - attacker.transform.position).normalized;
		Quaternion targetRotation = Quaternion.LookRotation(direction);
		attacker.transform.rotation = Quaternion.Lerp(attacker.transform.rotation, targetRotation, Time.deltaTime * speed);
	}
	private void ConfigureNavMeshAgent(NavMeshAgent agent)
	{
		agent.avoidancePriority = 50;
		/*agent.updatePosition = false;*///
		agent.updateRotation = true; //
		agent.radius = 0.2f;
		agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
		agent.enabled = true;
		//agent.speed = movespeed;
		agent.stoppingDistance = 0.1f;

	}
}
