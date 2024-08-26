using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
//using UnityEditor.Experimental.GraphView;
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
    private Animator animator;
    private GameObject zombie1;
    private GameObject zombie2;
    private GameObject player;
    private GameObject animatorObj;
    private ScoresManager scoresManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        combatManagerRef = GameObject.Find("CombatManager").GetComponent<CombatManager>();
        flagForCoroutine = true;

        if (GameObject.Find("AnimatorObj"))
        {
            animatorObj = GameObject.Find("AnimatorObj");
        }
        if (GameObject.Find("Zombie 2"))
        {
            zombie2 = GameObject.Find("Zombie 2");
        }
        if (GameObject.Find("Player"))
        {
            player = GameObject.Find("Player");
            animator = GameObject.Find("OrkAssasin").GetComponent<Animator>();
        }
        if (GameObject.Find("Zombie 1"))
        {
            zombie1 = GameObject.Find("Zombie 1");
        }

        if (GameObject.Find("Canvas2"))
        {
            scoresManagerScript = GameObject.Find("Canvas2").GetComponent<ScoresManager>();
        }
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

            if (combatManagerRef == null)
            {
                combatManagerRef = GameObject.Find("CombatManager").GetComponent<CombatManager>();
            }

            if (combatManagerRef.currentTurn != null && combatManagerRef.currentTurn.CompareTag("Player"))
            {
                
                combatManagerRef.playerTurnCompleted = true;
            }
            if (combatManagerRef.currentTurn != null && combatManagerRef.currentTurn.CompareTag("Monster"))
            {
                
                combatManagerRef.monsterTurnCompleted = true;
            }
        }
    }

    private void ExecuteMethodsInSequence()
    {
        RetrievePlayerGO();
        RetrieveMonsterGO();
        RetrieveTargetIO();
        RetrieveAttackerIO();
        InstantiateActionChoices();
        CreateButtonForPlayer();
        MonsterAttack();
    }

    void TriggerSequence()
    {
        ExecuteMethodsInSequence();
        flagForCoroutine = false;
    }
    private void RetrievePlayerGO()
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
            
        }
    }

    private void RetrieveMonsterGO()
    {
        if (combatManagerRef.currentTurn != null && combatManagerRef.currentTurn.CompareTag("Monster"))
        {
            monstersControllerGO = GameObject.Find("MonstersControllerPrefab(Clone)");
            monstersControllerRef = monstersControllerGO.GetComponent<MonstersController>();
            attackerGO = monstersControllerRef.monster.gameObject;
            targetCharacterGO = monstersControllerRef.playerTarget.gameObject;
            
        }
    }

    private void RetrieveTargetIO()
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
        
    }

    private void RetrieveAttackerIO()
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
        
    }
    private void InstantiateActionChoices()
    {
        if (!GameObject.Find("ActionChoicesPrefab(Clone)"))
        {
            actionChoicesGO = Instantiate(Resources.Load<GameObject>("Prefabs/ActionChoicesPrefab"));
            actionChoicesRef = actionChoicesGO.GetComponent<ActionChoices>();
            actionChoicesRef.Initialize(
                uiManager: this,
                combatManager: combatManagerRef,
                scoresManager: scoresManagerScript,
                animator: animator,
                zombie1: zombie1,
                zombie2: zombie2,
                player: player,
                animatorObj: animatorObj
            );
            
        }
    }

    private void CreateButtonForPlayer()
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
                    newButton.onClick.AddListener(() => actionChoicesRef.HandleAttackChoice(attackTypeChosen));
                }
            }
        }
       
    }

    private void MonsterAttack()
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
                InstantiateAndProceed();
            }
            if (actionChoicesGO)
            {
                Destroy(actionChoicesGO);
                InstantiateAndProceed();
            }
        }
        
    }
    void InstantiateAndProceed()
    {
        flagForAttackChoice = false;
        actionChoicesRef.HandleAttackChoice(attackTypeChosen);
        isCharacterTurnOver = true;
    }
    public void SetAttackChoiceHandled(bool value)
    {
        flagForAttackChoice = value;
    }
}
