using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MonstersController : MonoBehaviour
{
	public GameObject monster;
	public GameObject marker;
	public float movespeed = 5f;
	private GameObject UIManagerPrefab;
	public GameObject playerTarget;
	private CombatManager combatManagerRef;
	private bool reachedTarget;

	// Start is called before the first frame update
	void Start()
	{
		playerTarget = GameObject.Find("Player");
		marker = GameObject.Find("Marker");
		combatManagerRef = GameObject.Find("CombatManager").GetComponent<CombatManager>();
		monster = combatManagerRef.currentTurn;
		marker = GameObject.Find("MarkerPrefab");
		marker.transform.position = new Vector3(monster.transform.position.x, monster.transform.position.y + 2, monster.transform.position.z);
	}

	// Update is called once per frame
	void Update()
	{
		marker.transform.position = new Vector3(monster.transform.position.x, monster.transform.position.y + 2, monster.transform.position.z);
		ApproachPlayer();
	}
	private void ApproachPlayer()
	{
		while(!reachedTarget)/*(monster.transform.position != Vector3.zero)*/
		{
			Vector3 directionToPlayer = playerTarget.transform.position - monster.transform.position;
			directionToPlayer.Normalize();
			monster.transform.position += directionToPlayer * movespeed * Time.deltaTime;

			if (Vector3.Distance(monster.transform.position, playerTarget.transform.position) < 1f)
			{
				reachedTarget = true;
				//monster.transform.position = Vector3.zero;
				UpdateUIManager();
			}
		}

	}

	private void UpdateUIManager()
	{
		StartCoroutine(InstantiateAndProceed());

		IEnumerator InstantiateAndProceed ()
		{
			if (!GameObject.Find("UIManagerPrefab(Clone)"))
			{
				UIManagerPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/UIManagerPrefab"));
				yield return null;
				Debug.Log("Is UIManagerPrefab for the monster instantiated?" + UIManagerPrefab.name);
			}
			
		}
		
	}

}
