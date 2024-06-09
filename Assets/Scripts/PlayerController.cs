using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

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
	private GameObject rangeIndicatorGO;
	public RangeIndicator rangeIndicatorScript;
	public CombatManager CombatManagerScript;
	private float radius;
	private Animator animator;
	private AnimScript animScript;

	private float threshold = 0.1f;

	public bool isWalking = false;
	public bool isIdle = false;
	private Vector3 direction = new Vector3();
	private bool isAnimatorSetup = false;

	private GameObject midpoint;

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
		animScript = GameObject.Find("AnimatorObj").GetComponent<AnimScript>();
		
	}

	// Update is called once per frame
	void Update()
	{
		marker.transform.position = new Vector3(attacker.transform.position.x, attacker.transform.position.y + 2, attacker.transform.position.z);
		ClickOnMonster();
		//if (movesLeft < 1) CombatManagerScript.playerTurnCompleted = true;

	}

	void ClickOnMonster()
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

		if (clickPosition != Vector3.zero && Hit.collider.CompareTag("Monster") && rangeIndicatorScript.targetsInRange.Contains(Hit.transform.gameObject))
		{
			//isWalking = false;
			//animator.SetBool("IsWalking", isWalking);

			direction = clickPosition - attacker.transform.position;
			direction.Normalize();
			direction.y = 0;

			attacker.transform.rotation = Quaternion.LookRotation(direction);
			clickPosition = Vector3.zero;

			ActivateUIManager();


		}
		if (clickPosition != Vector3.zero && Hit.collider.CompareTag("RangeIndicator"))
		{
			//isIdle = false;
			//animator.SetBool("IsIdle", isIdle);
			//Vector3 direction = new Vector3(clickPosition.x, attacker.transform.position.y, clickPosition.z) - attacker.transform.position;
			//direction.Normalize();

			if (!isAnimatorSetup)
			{
				direction = animScript.SetupWalking(attacker, clickPosition );/*SetUpWalking();*/
				isAnimatorSetup = true;
			}

			attacker.transform.position += direction * movespeed * Time.deltaTime;
			//isWalking = true;
			//animator.SetBool("IsWalking", isWalking);
			if (Vector3.Distance(attacker.transform.position, new Vector3(clickPosition.x, attacker.transform.position.y, clickPosition.z)) < threshold)
			{
				animScript.SetUpIdle(attacker, midpoint);
				// Snaps the attacker to the target position
				attacker.transform.position = new Vector3(clickPosition.x, attacker.transform.position.y, clickPosition.z);

				rangeIndicatorGO.transform.position = new Vector3(attacker.transform.position.x, 0.07f, attacker.transform.position.z);
				isWalking = false;
			
				
				CombatManagerScript.playerTurnCompleted = true;
				clickPosition = Vector3.zero;
				//Destroy(gameObject);
			}

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

	//void SetUpWalking()
	//{
	//	direction = new Vector3(clickPosition.x, attacker.transform.position.y, clickPosition.z) - attacker.transform.position;
	//	direction.Normalize();
	//	attacker.transform.rotation = Quaternion.LookRotation(direction);
	//	isWalking = true;
	//	animator.SetBool("IsWalking", isWalking);
	//	isAnimatorSetup = true;
	//}

	//void SetUpIdle()
	//{
		
	//	isWalking = false;
	//	animator.SetBool("IsWalking", isWalking);

	//	isIdle = true;
	//	animator.SetBool("IsIdle", isIdle);
	//	direction = new Vector3(midpoint.transform.position.x, attacker.transform.position.y, midpoint.transform.position.z) - attacker.transform.position;
	//	direction.Normalize();
	//	attacker.transform.rotation = Quaternion.LookRotation(direction);
	//}

}
