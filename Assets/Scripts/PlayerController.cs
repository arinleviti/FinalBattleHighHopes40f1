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
	private GameObject UIManagerRef;
	public GameObject attacker;
	private GameObject UIManagerPrefab;

	//public bool isPlayerTurn = false;
	//public bool isEnemyTurn = false;

	
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
	}

	// Update is called once per frame
	void Update()
	{
		marker.transform.position = new Vector3(attacker.transform.position.x, attacker.transform.position.y + 2, attacker.transform.position.z);
		ClickOnMonster();	
	}

	void ActivateUIManager()
	{
		if (!isUIManagerActivated)
		{
			UIManagerPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/UIManagerPrefab"));
			isUIManagerActivated = true;
		}
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
		if (clickPosition != Vector3.zero && Hit.collider.CompareTag("Monster"))
		{

			Vector3 direction = clickPosition - attacker.transform.position;
			direction.Normalize();
			attacker.transform.position += direction * movespeed * Time.deltaTime;

			if (Vector3.Distance(attacker.transform.position, clickPosition) < 1f)
			{
				clickPosition = Vector3.zero;
				ActivateUIManager();
			}
		}
	}
}
