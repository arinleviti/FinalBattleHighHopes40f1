using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class PotionButton : MonoBehaviour
{
    
    public CombatManager combatManagerScript;
    public GameObject combatManagerGO;
    private GameObject canvasPotionGO;
    private Canvas canvasPotion;
    //private ScoresManager scoresManagerScript;
    private Button potionButton;
    private GameObject actionChoicesGO;
    private ActionChoices actionChoicesScript;
    private Animator animator;
    private GameObject zombie1;
    private GameObject zombie2;
    private GameObject attacker;
    private GameObject animatorObj;

    private GameObject playerGO;
    private PlayerStats playerStats;
    private int potionsLeftInt;
    private TextMeshProUGUI potionsLeftText;
    private GameObject scoresManagerGO;
    private ScoresManager scoresManagerScript;

    // Start is called before the first frame update
    void Awake()
    {
        combatManagerGO = GameObject.Find("CombatManager");
        combatManagerScript = combatManagerGO.GetComponent<CombatManager>();
        
        combatManagerScript.OnPlayerTurnPotionButtonOn += OnPlayerTurnPotionButtonOn;
        combatManagerScript.OnPlayerTurnPotionButtonOff += OnPlayerTurnPotionButtonOff;

        playerGO = GameObject.Find("Player");
        playerStats = playerGO.GetComponent<PlayerStats>();
        potionsLeftInt = playerStats.PotionsAvailable;

        scoresManagerGO = GameObject.Find("Canvas2");
        scoresManagerScript = scoresManagerGO.GetComponent<ScoresManager>();
        animator = GameObject.Find("OrkAssasin").GetComponent<Animator>();
        zombie1 = GameObject.Find("Zombie 1");
        zombie2 = GameObject.Find("Zombie 2");
        attacker = playerGO;
        animatorObj = GameObject.Find("AnimatorObj");
    }
    public void ResetValues()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreatePotionButton()
    {
        if (combatManagerScript.currentTurn != null && combatManagerScript.currentTurn.CompareTag("Player"))
        {
            if (!GameObject.Find("PotionCanvasPrefab(Clone)"))
            {
                canvasPotionGO = Instantiate(Resources.Load<GameObject>("Prefabs/PotionCanvasPrefab"));
                
            }
            else
            {
                canvasPotionGO = GameObject.Find("PotionCanvasPrefab(Clone)");
            }
            canvasPotion = canvasPotionGO.GetComponent<Canvas>();
            canvasPotion.enabled = false;
            //scoresManagerScript = GameObject.Find("Canvas2").GetComponent<ScoresManager>();
            UpdatePotionNumber();
            if (potionsLeftInt > 0)
            {
                canvasPotion.enabled = true;
                potionButton = canvasPotionGO.GetComponentInChildren<Button>();
                potionButton.onClick.RemoveAllListeners();
                potionButton.onClick.AddListener(() => OnPotionButtonClick());
            }
        }
    }
    private void OnPotionButtonClick()
    {
        if (!GameObject.Find("ActionChoicesPrefab(Clone)"))
        {
            actionChoicesGO = Instantiate(Resources.Load<GameObject>("Prefabs/ActionChoicesPrefab"));
            actionChoicesScript = actionChoicesGO.GetComponent<ActionChoices>();
            actionChoicesScript.Initialize(
                combatManager: combatManagerScript,
                scoresManager: scoresManagerScript,
                animator: animator,
                zombie1: zombie1,
                zombie2: zombie2,
                player: attacker,
                animatorObj: animatorObj
            );
        }
        actionChoicesScript = actionChoicesGO.GetComponent<ActionChoices>();
        actionChoicesScript.HandleAttackChoice(AttackType.Potion);
        if (potionsLeftInt <= 1)
        {
            canvasPotion.enabled = false;
        }
        UpdatePotionNumber();
    }
    public void OnPlayerTurnPotionButtonOn()
    {
        CreatePotionButton();
    }
    public void OnPlayerTurnPotionButtonOff()
    {
        canvasPotion.enabled = false;
    }

    public void UpdatePotionNumber()
    {
        if (combatManagerScript.currentTurn.CompareTag("Player") /*&& potionButton != null*/)
        {
            potionButton = canvasPotionGO.GetComponentInChildren<Button>();
            potionsLeftText = potionButton.GetComponentInChildren<TextMeshProUGUI>();
            potionsLeftInt = playerStats.PotionsAvailable;
            potionsLeftText.text = $"{potionsLeftInt}";
        }
    }
}
