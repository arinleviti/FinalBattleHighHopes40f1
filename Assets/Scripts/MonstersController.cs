using System.Collections;
using System.Collections.Generic;
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


	private GameObject rangeIndicatorGO;
	public RangeIndicator rangeIndicatorScript;
	private Vector3 pointA;
	private float radius;
	

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
	}

	// Update is called once per frame
	void Update()
	{
		marker.transform.position = new Vector3(monster.transform.position.x, monster.transform.position.y + 2, monster.transform.position.z);
		ApproachPlayer();
		//if (movesLeft < 1) combatManagerRef.monsterTurnCompleted = true;
	}
	private void ApproachPlayer()
	{
		if (!reachedTarget)
		{
			Vector3 directionToPlayer = playerTarget.transform.position - monster.transform.position;
			directionToPlayer.Normalize();

			// Check if the player is within range and attack without moving
			if (rangeIndicatorScript.targetsInRange.Contains(playerTarget))
			{
				// Attack if within range
				//rangeIndicatorGO.transform.position = new Vector3(monster.transform.position.x, 0.07f, monster.transform.position.z);
				
				reachedTarget = true;
				UpdateUIManager();
			}
			else
			{
				// Move towards the player
				monster.transform.position += directionToPlayer * movespeed * Time.deltaTime;
				float distanceToPlayer = Vector3.Distance(playerTarget.transform.position, monster.transform.position);
				float distanceTravelled = Vector3.Distance(monster.transform.position, pointA);

				if (distanceToPlayer < 0.5f)
				{
					// Stop when within 2 units from the player
					//rangeIndicatorGO.transform.position = new Vector3(monster.transform.position.x, 0.07f, monster.transform.position.z);
					reachedTarget = true;
					combatManagerRef.monsterTurnCompleted = true;
				}
				else if (distanceTravelled >= radius)
				{
					// Stop when the monster has moved as far as it can in one move
					//rangeIndicatorGO.transform.position = new Vector3(monster.transform.position.x, 0.07f, monster.transform.position.z);
					reachedTarget = true;
					combatManagerRef.monsterTurnCompleted = true;
				}
			}
		}
			//if (!reachedTarget) //*(monster.transform.position != Vector3.zero)
			//{
			//	Vector3 directionToPlayer = playerTarget.transform.position - monster.transform.position;
			//	directionToPlayer.Normalize();
			//	monster.transform.position += directionToPlayer * movespeed * Time.deltaTime;
			//	float distanceTravelled = Vector3.Distance(monster.transform.position, pointA);

			//	if (!rangeIndicatorScript.targetsInRange.Contains(playerTarget) && Vector3.Distance(playerTarget.transform.position, monster.transform.position) < 2)
			//	{
			//		rangeIndicatorGO.transform.position = new Vector3(monster.transform.position.x, 0.07f, monster.transform.position.z);
			//		reachedTarget = true;
			//		combatManagerRef.monsterTurnCompleted = true;
			//	}

			//	if (!rangeIndicatorScript.targetsInRange.Contains(playerTarget) && distanceTravelled >= radius)
			//	{
			//		rangeIndicatorGO.transform.position = new Vector3(monster.transform.position.x, 0.07f, monster.transform.position.z);
			//		reachedTarget = true;
			//		combatManagerRef.monsterTurnCompleted = true;
			//	}

			//	if (rangeIndicatorScript.targetsInRange.Contains(playerTarget))
			//	{
			//		rangeIndicatorGO.transform.position = new Vector3(monster.transform.position.x, 0.07f, monster.transform.position.z);

			//		reachedTarget = true;
			//		UpdateUIManager();
			//	}
			//}


		
	}

	//private IEnumerator UpdateUIManager()
	//{
	//	if (!GameObject.Find("UIManagerPrefab(Clone)"))
	//	{
	//		UIManagerPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/UIManagerPrefab"));
	//		yield return null; // Wait for the next frame to ensure the UIManager is instantiated
	//	}

	//	// Your UI update logic...


	//}
	private void UpdateUIManager()
	{
		
			if (!GameObject.Find("UIManagerPrefab(Clone)"))
			{

				UIManagerPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/UIManagerPrefab"));
				
				Debug.Log("Is UIManagerPrefab for the monster instantiated?" + UIManagerPrefab.name);
			}

		

	}
	//private void UpdateUIManager()
	//{
	//	StartCoroutine(InstantiateAndProceed());

	//	IEnumerator InstantiateAndProceed()
	//	{
	//		if (!GameObject.Find("UIManagerPrefab(Clone)"))
	//		{

	//			UIManagerPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/UIManagerPrefab"));
	//			yield return null;
	//			Debug.Log("Is UIManagerPrefab for the monster instantiated?" + UIManagerPrefab.name);
	//		}

	//	}

	//}
	// Sometimes the monster gets stuck in limbo if the player is too close to the range indicator and the monster can't complete its move.
	public void StopThere()
	{
		combatManagerRef.monsterTurnCompleted = true;
	}

	
}
