using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeIndicator : MonoBehaviour
{
	public bool isTargetInRange = false;
	public List<GameObject> targetsInRange = new List<GameObject>();

	public Material originalMaterial;
	public Material newMaterial;

	public CombatManager combatManagerScript;
	//public PlayerController playerControllerScript;
	//public MonstersController monstersControllerScript;
	private int index = 1;

	// Start is called before the first frame update
	void Start()
	{
		originalMaterial = GetComponent<Material>();
		combatManagerScript = GameObject.Find("CombatManager").GetComponent<CombatManager>();

	}

	// Update is called once per frame
	void Update()
	{
		//if (GameObject.Find("PlayerControllerPrefab(Clone)"))
		//{
			//playerControllerScript = GameObject.Find("PlayerControllerPrefab(Clone)").GetComponent<PlayerController>();
			if (combatManagerScript.currentTurn.CompareTag("Player") && combatManagerScript.movesLeft < 2)
			{
				
				GetComponent<Renderer>().material = newMaterial;
			}
		//}
		//if (GameObject.Find("MonstersControllerPrefab(Clone)"))
		//{
			//monstersControllerScript = GameObject.Find("MonstersControllerPrefab(Clone)").GetComponent<MonstersController>();
			if (combatManagerScript.currentTurn.CompareTag("Monster") && combatManagerScript.movesLeft < 2)
			{
				
				GetComponent<Renderer>().material = newMaterial;
			}
		//}

	}

	//public void CheckRange()
	//{
	//	targetsInRange.Clear();

	//	Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);
	//	foreach (Collider collider in colliders)
	//	{
	//		GameObject target = collider.gameObject;
	//		targetsInRange.Add(target);
	//	}
	//}


	private void OnTriggerEnter(Collider other)
	{
		
		//isTargetInRange = true;
		if (!targetsInRange.Contains(other.gameObject))
		{
			targetsInRange.Add(other.gameObject);
		}
		index++;
		foreach (GameObject target in targetsInRange)
		{
			
			
			Debug.Log("Range Indicator contains: " +  target.name + index);
		}
	}

	//private void OnTriggerExit(Collider other)
	//{
	//	targetsInRange.Remove(other.gameObject);
	//}

	public List<GameObject> GetTargetsInRange()
	{
		return targetsInRange;
	}
}
