using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DynamicButtonGenerator : MonoBehaviour
{
    public Canvas canvas;
    public Button buttonPrefab;
    public List<AttackType> attackTypes;


	public GameObject targetCharacterGO;
    public GameObject attackerGO;

    public ICharacter targetCharacterIO;
    public ICharacter attackerIO;

	private AttackType attackTypeChosen;

    private GameObject actionPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        canvas.enabled = true;
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
		
        Debug.Log(targetCharacterIO);

		if (attackerGO != null && attackerGO.CompareTag("Monster"))
		{
			ICharacter attacker = attackerGO.GetComponent<MonsterStats>();
			attackerIO = attacker;
		}
		else if (attackerGO != null && attackerGO.CompareTag("Player"))
		{
			ICharacter attacker = attackerGO.GetComponent<PlayerStats>();
            Debug.Log(attacker);
			attackerIO = attacker;
			Debug.Log(attackerIO);
		}

        //ICharacter attacker = attackerGO.GetComponent<PlayerStats>();
        //      attackerIO = attacker;
        //attackTypes = new List<AttackType>();
        attackTypes = attackerIO.AttackT;

        foreach (AttackType attackType in attackTypes)
        {
            Button newButton = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity);
            newButton.transform.SetParent(canvas.transform, false);
            Debug.Log("Attack type: " + attackType);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = attackType.ToString();
            attackTypeChosen = attackType;
            newButton.onClick.AddListener(() =>  HandleButtonClick());
        }
    }

    void HandleButtonClick()
    {
        
        switch(attackTypeChosen)
        {
            case AttackType.Punch:
                actionPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/PunchPrefab"));
                Punch punchScriptRef = actionPrefab.GetComponent<Punch>();
                if (punchScriptRef != null)
                {
					targetCharacterIO.HP = punchScriptRef.Hit(targetCharacterIO, attackerIO);
                    Debug.Log("Punch Hit method executed");
				}
                else
                {
					Debug.LogError("Punch component not found on instantiated prefab!");
				}
                break;
            case AttackType.BoneCrunch:
                actionPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/BoneCrunchPrefab"));
                BoneCrunch boneCrunchScriptRef = actionPrefab.GetComponent<BoneCrunch>();
                if (boneCrunchScriptRef != null)
                {
					targetCharacterIO.HP = boneCrunchScriptRef.Hit(targetCharacterIO, attackerIO);
				}
				else
				{
					Debug.LogError("Punch component not found on instantiated prefab!");
				}
				break;
			case AttackType.MistyFist:
				actionPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/MistyFistPrefab"));
				MistyFist mistyFistScriptRef = actionPrefab.GetComponent<MistyFist>();
				if (mistyFistScriptRef != null)
				{
					targetCharacterIO.HP = mistyFistScriptRef.Hit(targetCharacterIO, attackerIO);
				}
				else
				{
					Debug.LogError("Punch component not found on instantiated prefab!");
				}
                break;
			case AttackType.Claw:
				actionPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/ClawPrefab"));
				Claw clawScriptRef = actionPrefab.GetComponent<Claw>();
				if (clawScriptRef != null)
				{
					targetCharacterIO.HP = clawScriptRef.Hit(targetCharacterIO, attackerIO);
				}
				else
				{
					Debug.LogError("Punch component not found on instantiated prefab!");
				}
				break;
			default:
				Debug.Log("Invalid attack type");
				break;
		}
        canvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
