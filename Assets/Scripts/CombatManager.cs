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

	//public PlayerController playerControllerRef;
	public PlayerController playerControllerRef;

	public List<GameObject> TurnList = new List<GameObject>();

	private GameObject PlayerControllerPrefab;
	private GameObject monsterControllerPrefab;
	public GameObject UIManagerPrefabGO;

	public GameObject currentTurn;
	public int movesLeft; //Each character has 2 moves for each turn

	private List<CharacterClass> charactersIOList;

	private bool isDead = false;

	private PlayerStats playerStats;
	private MonsterStats monsterStats;
	//private Animator playerAnimator;
	public AnimScript animatorScript;
	private int playerHP;

	private Animator animator;
	private Animator animatorZ1;
	private Animator animatorZ2;

	// Start is called before the first frame update
	void Start()
	{
		//TurnList.Add(GameObject.FindGameObjectWithTag("Player"));
		//foreach (var gameObject in GameObject.FindGameObjectsWithTag("Monster"))
		//{
		//	TurnList.Add(gameObject);
		//}
		//playerControllerRef = gameObject.GetComponent<PlayerController>();

		CreateTurnList();
		string debug = string.Join(",", TurnList.Select(x => x.ToString()).ToList());
		Debug.Log("Characters in the Turn List: " + debug);
		TurnManager();
		//playerAnimator = GameObject.Find("OrkAssasin").GetComponent<Animator>();
		animatorScript = GameObject.Find("AnimatorObj").GetComponent<AnimScript>();
		animator = GameObject.Find("OrkAssasin").GetComponent<Animator>();
		animatorZ1 = GameObject.Find("Zombie 1").GetComponentInChildren<Animator>();
		animatorZ2 = GameObject.Find("Zombie 2").GetComponentInChildren<Animator>();
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
		//may be redundant, delete.
		while (TurnList.Count > 0)
		{
			//bool characterDied;
			//do
			//{
			//	characterDied = false;

			// Creates a copy of the TurnList to iterate over
			List<GameObject> currentTurnList = new List<GameObject>(TurnList);
			for (int i = 0; i < currentTurnList.Count; i++)
			{
				GameObject gameObject = currentTurnList[i];

				if (gameObject == null)
				{
					continue; // Skip null gameObjects
				}

				int hp = gameObject.GetComponent<CharacterClass>().HP;
				if (hp <= 0)
					continue;
				movesLeft = 2;
				while (movesLeft > 0)
				{
					currentTurn = gameObject;

					if (gameObject != null && gameObject.CompareTag("Player"))
					{

						 
						CleanUpTurn();
						PlayerControllerPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/PlayerControllerPrefab"));
						//playerControllerRef = PlayerControllerPrefab.GetComponent<NewPlayerController>();
						playerControllerRef = PlayerControllerPrefab.GetComponent<PlayerController>();

						yield return new WaitUntil(() => playerTurnCompleted);
						yield return StartCoroutine(IsCharacterDead());
						

						Debug.Log("Is the turn completed in combatManager? " + playerTurnCompleted);
						playerTurnCompleted = false;
						//UIManagerPrefabGO = GameObject.Find("UIManagerPrefab(Clone)");
						
						//PlayerAnimator.Rebind();
						movesLeft--;
						if (movesLeft < 1)
						{
							yield return StartCoroutine(WaitAndContinue2());
						}
						
					}

					if (gameObject != null && gameObject.CompareTag("Monster"))
					{
						
						CleanUpTurn();
						monsterControllerPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/MonstersControllerPrefab"));

						yield return new WaitUntil(() => monsterTurnCompleted);
						yield return StartCoroutine(IsCharacterDead());
						
						monsterTurnCompleted = false;
						//UIManagerPrefabGO = GameObject.Find("UIManagerPrefab(Clone)");
						//CleanUpTurn();
						movesLeft--;
						if (movesLeft < 1)
						{
							yield return StartCoroutine(WaitAndContinue2());
						}
						else yield return StartCoroutine(WaitAndContinue1());

					}
				}
				
			}
		 foreach (GameObject GO in currentTurnList)
			{
				if (GO != null)
				{
					Debug.Log("Elem in CURRENT TURN LIST:" + GO.name);
				}
				
			}
		}

	}
	IEnumerator WaitAndContinue2()
	{
		yield return new WaitForSeconds(2);	
	}

	IEnumerator WaitAndContinue1()
	{		
		yield return new WaitForSeconds(1);	
	}
	void CleanUpTurn()
	{
		Debug.Log("Cleaning up turn artifacts.");
		Destroy(GameObject.Find("UIManagerPrefab(Clone)"));
		Destroy(GameObject.Find("ButtonPrefab(Clone)"));
		Destroy(GameObject.Find("ButtonPrefab(Clone)(Clone)"));
		Destroy(GameObject.Find("RangeIndicatorPrefab(Clone)"));
		Destroy(GameObject.Find("MonstersControllerPrefab(Clone)"));
		Destroy(GameObject.Find("PlayerControllerPrefab(Clone)"));
		Destroy(GameObject.Find("ActionChoicesPrefab(Clone)"));

		GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Action");
		foreach (GameObject obj in objectsWithTag)
		{
			Destroy(obj);
		}
		
	}
	// Creates a list with the scripts attached to the GO. Used in IsCharacterDead().
	private void CreateIOList()
	{
		
		if (charactersIOList == null)
			charactersIOList = new List<CharacterClass>();
			charactersIOList.Clear();
		foreach (GameObject characterGO in TurnList)
		{
			if (characterGO == null)
			{
				continue; // Skip this character if it's null or already destroyed
			}

			CharacterClass character = null;

			CharacterClass playerstats = characterGO.GetComponent<PlayerStats>();
			if (playerstats != null)
			{
				character = playerstats;
				Debug.Log("Character's hp :" + character + character.HP);
				playerHP = character.HP;
				
			}
			else
			{
				CharacterClass monsterStats = characterGO.GetComponent<MonsterStats>();
				if (monsterStats != null)
				{
					character = monsterStats;
					Debug.Log("Character's hp :" + character + character.HP);
				}
			}
			if (character != null)
			{
				charactersIOList.Add(character);
			}
		}
		foreach (CharacterClass character in charactersIOList)
		{
			if (character !=null)
			{
				Debug.Log("char in CHARACTERSIOList:" + character.name);
			}
			
		}
	}
	//Checks if any character's HP < 0, places them in a new list, removes them from the TurnList and destroys the corresponding GO. 
	private IEnumerator IsCharacterDead()
	{
		CreateIOList();
		List<GameObject> charactersToRemove = new List<GameObject>();
		foreach (CharacterClass character in charactersIOList)
		{
			if (character == null || (character as MonoBehaviour) == null)
			{
				continue; // Skip this character if it's null or already destroyed
			}
			if (character.HP <= 0)
			{
				// Get the GameObject associated with the character
				GameObject characterGO = character.gameObject;
				if (characterGO != null)
				{
					charactersToRemove.Add(characterGO);
					//Destroy(characterGO);
					character.IsDead = true;
				}
				//isDead = true;

			}

		}
		foreach (GameObject character in charactersToRemove)
		{
			Debug.Log("char in CHARACTERSTOREMOVE list:" + character.name);
		}
		foreach (GameObject characterToRemove in charactersToRemove)
		{
			if (characterToRemove.CompareTag("Player"))
			{
				animatorScript.PlayDeathAnim(characterToRemove, animator);
			}
			else
			{
				if (characterToRemove != null && characterToRemove.name == "Zombie 1")
					animatorScript.PlayDeathAnim(characterToRemove, animatorZ1);
				else if (characterToRemove != null && characterToRemove.name == "Zombie 2")
					animatorScript.PlayDeathAnim(characterToRemove, animatorZ2);
			}
			TurnList.Remove(characterToRemove);
			//TurnList.Remove(characterToRemove);
			//Destroy(characterToRemove);
		}
		yield break;
	}

	public void CreateTurnList()
	{
		TurnList.Add(GameObject.FindGameObjectWithTag("Player"));
		foreach (var gameObject in GameObject.FindGameObjectsWithTag("Monster"))
		{
			TurnList.Add(gameObject);
		}
	}

	
}
