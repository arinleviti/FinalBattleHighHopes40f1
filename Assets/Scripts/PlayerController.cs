using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

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

	private float threshold = 0.1f;


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

		if (clickPosition != Vector3.zero && Hit.collider.CompareTag("Monster") && rangeIndicatorScript.targetsInRange.Contains(Hit.transform.gameObject))
		{
			Vector3 direction = clickPosition - attacker.transform.position;
			direction.Normalize();
			direction.y = 0;

			attacker.transform.rotation = Quaternion.LookRotation(direction);
			clickPosition = Vector3.zero;
			
				ActivateUIManager();


		}
		if (clickPosition != Vector3.zero && Hit.collider.CompareTag("RangeIndicator"))
		{
			Vector3 direction = new Vector3(clickPosition.x, attacker.transform.position.y, clickPosition.z) - attacker.transform.position;
			direction.Normalize();
			attacker.transform.position += direction * movespeed * Time.deltaTime;
			if (Vector3.Distance(attacker.transform.position, new Vector3(clickPosition.x, attacker.transform.position.y, clickPosition.z)) < threshold)
			{
				// Snaps the attacker to the target position
				attacker.transform.position = new Vector3(clickPosition.x, attacker.transform.position.y, clickPosition.z);

				rangeIndicatorGO.transform.position = new Vector3(attacker.transform.position.x, 0.07f, attacker.transform.position.z);
				
				CombatManagerScript.playerTurnCompleted = true;
				clickPosition = Vector3.zero;
				Destroy(gameObject);
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
}
