using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
	//private string tag1 = "Player";
	//private string tag2 = "Monster";
	public bool playerTurnCompleted = false;
	public bool monsterTurnCompleted = false;
	public bool isEnemyTurn = false;

	public PlayerController playerControllerRef;

	public List<GameObject> TurnList = new List<GameObject>();

	private GameObject PlayerControllerPrefab;
	private GameObject monsterControllerPrefab;
	public GameObject UIManagerPrefabGO;

	public GameObject currentTurn;

	// Start is called before the first frame update
	void Start()
	{
		TurnList.Add(GameObject.FindGameObjectWithTag("Player"));
		foreach (var gameObject in GameObject.FindGameObjectsWithTag("Monster"))
		{
			TurnList.Add(gameObject);
		}
		//playerControllerRef = gameObject.GetComponent<PlayerController>();
		string debug = string.Join(",", TurnList.Select(x=>x.ToString()).ToList());
		Debug.Log("Characters in the Turn List: " + debug);
		TurnManager();
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void TurnManager()
	{
		StartCoroutine(ExecuteTurns());
	}

	private IEnumerator ExecuteTurns()
	{
		while (TurnList.Count > 0)
		{
			foreach (GameObject gameObject in TurnList)
			{
				currentTurn = gameObject;
				if (gameObject != null && gameObject.CompareTag("Player"))
				{
					PlayerControllerPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/PlayerControllerPrefab"));
					

					yield return new WaitUntil(() => playerTurnCompleted);
					Debug.Log("Is the turn completed in combatManager? " + playerTurnCompleted);
					playerTurnCompleted = false;
					UIManagerPrefabGO = GameObject.Find("UIManagerPrefab(Clone)");
					Destroy(GameObject.Find("UIManagerPrefab(Clone)"));
					Destroy(GameObject.Find("ButtonPrefab(Clone)"));
					GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Action");
					foreach (GameObject obj in objectsWithTag)
					{
						Destroy(obj);
					}

				}
				if (gameObject != null && gameObject.CompareTag("Monster"))
				{	
					monsterControllerPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/MonstersControllerPrefab"));
					yield return new WaitUntil(() => monsterTurnCompleted);
					monsterTurnCompleted = false;
					UIManagerPrefabGO = GameObject.Find("UIManagerPrefab(Clone)");
					Destroy(GameObject.Find("UIManagerPrefab(Clone)"));
					GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Action");
					foreach (GameObject obj in objectsWithTag)
					{
						Destroy(obj);
					}
				}
			}
		}

	}
}
