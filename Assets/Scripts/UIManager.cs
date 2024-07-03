using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public Canvas canvas;
	public Button buttonPrefab;
	public List<AttackType> attackTypes;


	public GameObject targetCharacterGO;
	public GameObject attackerGO;

	public ICharacter targetCharacterIO;
	public ICharacter attackerIO;

	public AttackType attackTypeChosen;

	public GameObject actionPrefab;

	public bool isCharacterTurnOver = false;

	public CombatManager combatManagerRef;

	public GameObject playerControllerGO;
	public GameObject monstersControllerGO;
	public PlayerController playerControllerRef;
	public MonstersController monstersControllerRef;

	private GameObject actionChoicesGO;
	private ActionChoices actionChoicesRef;

	private bool flagForCoroutine = false;
	public bool flagForAttackChoice = false;

	// Start is called before the first frame update
	void Start()
	{
		combatManagerRef = GameObject.Find("CombatManager").GetComponent<CombatManager>();
		flagForCoroutine = true;
		
		//RetrievePlayerGO();
		//RetrieveMonsterGO();
		//RetrieveTargetIO();
		//RetrieveAttackerIO();
		//CreateButtonForPlayer();
		//MonsterAttack();

	}



	// The Update method checks if the current turn is over and destroys the character's game object.
	void Update()
	{
		
		if (flagForCoroutine)
		{
			TriggerSequence();
		}

		if (isCharacterTurnOver)
		{
			isCharacterTurnOver = false;
			//combatManagerRef.movesLeft--;
			if (combatManagerRef == null)
			{
				combatManagerRef = GameObject.Find("CombatManager").GetComponent<CombatManager>();
			}

			if (combatManagerRef.currentTurn != null && combatManagerRef.currentTurn.CompareTag("Player"))
			{
				Destroy(playerControllerGO);
				if (GameObject.Find("PlayerControllerPrefab"))
				{
					Debug.Log("ERROR: Player Controller Prefab still alive");
				}
				combatManagerRef.playerTurnCompleted = true;
			}
			if (combatManagerRef.currentTurn != null && combatManagerRef.currentTurn.CompareTag("Monster"))
			{
				Destroy(monstersControllerGO);
				if (GameObject.Find("MonstersControllerPrefab(Clone)"))
				{
					Debug.Log("ERROR: Monster Controller Prefab still alive");
				}
				combatManagerRef.monsterTurnCompleted = true;
			}

		}

	}

	IEnumerator ExecuteMethodsInSequence()
	{
		yield return StartCoroutine(RetrievePlayerGO());
		yield return StartCoroutine(RetrieveMonsterGO());	
		yield return StartCoroutine(RetrieveTargetIO());		
		yield return StartCoroutine(RetrieveAttackerIO());
		yield return StartCoroutine(InstantiateActionChoices());
		yield return StartCoroutine(CreateButtonForPlayer());	
		yield return StartCoroutine(MonsterAttack());		
		//yield return StartCoroutine(DestroyUIManager());
		
	}

	void TriggerSequence()
	{
		StartCoroutine(ExecuteMethodsInSequence());
		flagForCoroutine = false;
	}
	private IEnumerator RetrievePlayerGO()
	{
		if (combatManagerRef.currentTurn != null && combatManagerRef.currentTurn.CompareTag("Player"))
		{
			playerControllerGO = GameObject.Find("PlayerControllerPrefab(Clone)");
			playerControllerRef = playerControllerGO.GetComponent<PlayerController>();
			canvas = GameObject.Find("Canvas1").GetComponent<Canvas>();
			buttonPrefab = Instantiate(Resources.Load<Button>("Prefabs/ButtonPrefab"));
			attackerGO = playerControllerRef.attacker.gameObject;
			targetCharacterGO = playerControllerRef.Hit.collider.gameObject;
			canvas.enabled = true;
			yield return null;
		}
	}

	private IEnumerator RetrieveMonsterGO()
	{
		if (combatManagerRef.currentTurn != null && combatManagerRef.currentTurn.CompareTag("Monster"))
		{
			monstersControllerGO = GameObject.Find("MonstersControllerPrefab(Clone)");
			monstersControllerRef = monstersControllerGO.GetComponent<MonstersController>();
			attackerGO = monstersControllerRef.monster.gameObject;
			targetCharacterGO = monstersControllerRef.playerTarget.gameObject;
			yield return null;
		}
	}

	private IEnumerator RetrieveTargetIO()
	{
		if (targetCharacterGO != null && targetCharacterGO.CompareTag("Monster"))
		{
			ICharacter target = targetCharacterGO.GetComponent<MonsterStats>();
			targetCharacterIO = target;


		}
		else if (targetCharacterGO != null && targetCharacterGO.CompareTag("Player"))
		{
			ICharacter target = targetCharacterGO.GetComponent<PlayerStats>();
			targetCharacterIO = target;
		}
		yield return null;
	}

	private IEnumerator RetrieveAttackerIO()
	{
		if (attackerGO != null && attackerGO.CompareTag("Monster"))
		{
			ICharacter attacker = attackerGO.GetComponent<MonsterStats>();
			attackerIO = attacker;

			if (attackerIO.CharacterCategory == Category.Skeleton)
			{
				attackerIO.AttackT.Add(AttackType.BoneCrunch);
				string debug = string.Join(", ", attackerIO.AttackT.Select(x => x.ToString()).ToArray());
				Debug.Log("Monster's attack :" + debug);
			}

		}
		else if (attackerGO != null && attackerGO.CompareTag("Player"))
		{
			ICharacter attacker = attackerGO.GetComponent<PlayerStats>();
			Debug.Log(attacker);
			attackerIO = attacker;
			Debug.Log(attackerIO);
		}
		yield return null;
	}

	private IEnumerator InstantiateActionChoices()
	{
		actionChoicesGO = Instantiate(Resources.Load<GameObject>("Prefabs/ActionChoicesPrefab"));
		actionChoicesRef = actionChoicesGO.GetComponent<ActionChoices>();
		yield return null;
	}
	private IEnumerator CreateButtonForPlayer()
	{
		if (combatManagerRef.currentTurn != null && combatManagerRef.currentTurn.CompareTag("Player"))
		{
			attackTypes = new List<AttackType>();
			attackTypes = attackerIO.AttackT;

			foreach (AttackType attackType in attackTypes)
			{
				Button newButton = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity);
				newButton.transform.SetParent(canvas.transform, false);
				Debug.Log("Attack type: " + attackType);
				newButton.GetComponentInChildren<TextMeshProUGUI>().text = attackType.ToString();
				attackTypeChosen = attackType;
				if (GameObject.Find("ActionChoicesPrefab(Clone)"))
				{
					//actionChoicesRef = Instantiate(Resources.Load<GameObject>("Prefabs/ActionChoicesPrefab")).GetComponent<ActionChoices>();
					Debug.Log("ActionChoicesRef instantiated: " + (actionChoicesRef != null));
					newButton.onClick.AddListener(() => actionChoicesRef.HandleAttackChoice());
				}


			}
		}
		yield return null;
		
	}
	private IEnumerator MonsterAttack()
	{
		if (combatManagerRef.currentTurn != null && combatManagerRef.currentTurn.CompareTag("Monster"))
		{
			int lastIndex = attackerIO.AttackT.Count - 1;
			Debug.Log("AttackerIO: " + attackerIO);
			attackTypeChosen = attackerIO.AttackT[lastIndex];
			Debug.Log("ATTACK TYPE CHOSEN monster" + attackTypeChosen);
			foreach (AttackType attackType in attackerIO.AttackT)
			{
				Debug.Log("Attack type in ATTACKT for the MONSTER" + attackType.ToString());
			}

			if (!actionChoicesGO)
			{
				//StartCoroutine(
				//InstantiateAndProceed());
				InstantiateAndProceed();
			}
			if (actionChoicesGO)
			{
				Destroy(actionChoicesGO);
				InstantiateAndProceed();

			}

			//IEnumerator InstantiateAndProceed()
			//{
			//	GameObject actionChoicesPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/ActionChoicesPrefab"));
			//	yield return null;
			//	actionChoicesRef = actionChoicesPrefab.GetComponent<ActionChoices>();
			//	actionChoicesRef.HandleAttackChoice();
			//	isCharacterTurnOver = true;
			//	Debug.Log("Is CharacterTurnOver true?" + isCharacterTurnOver);
			//}
		}
		yield return null;
	}
	void InstantiateAndProceed()
	{
		//if (!GameObject.Find("ActionChoicesPrefab(Clone)"))
		//{
			//actionChoicesRef = Instantiate(Resources.Load<GameObject>("Prefabs/ActionChoicesPrefab")).GetComponent<ActionChoices>();
			
		//yield return null;
			flagForAttackChoice = false;
			actionChoicesRef.HandleAttackChoice();
		//yield return new WaitUntil(() => flagForAttackChoice);
		//Destroy(actionChoicesRef);
		isCharacterTurnOver = true;
		//isCharacterTurnOver = true;
		//Debug.Log("Is CharacterTurnOver true?" + isCharacterTurnOver);
		//}
		//Destroy(actionChoicesGO);
		
	}
	public void SetAttackChoiceHandled(bool value)
	{
		flagForAttackChoice = value;
	}
	private IEnumerator DestroyUIManager()
	{
		if (gameObject!= null)
		{
			Destroy(gameObject);
		}
		yield return null;
	}
}
