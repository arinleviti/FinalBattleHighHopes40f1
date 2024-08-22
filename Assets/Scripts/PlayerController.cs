using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Canvas canvas;
    private GameObject marker;
    public float movespeed = 5f;
    private Vector3 clickPosition;
    public RaycastHit Hit;
    private bool isUIManagerActivated = false;
    public GameObject attacker;
    public GameObject UIManagerPrefab;
    private GameObject rangeIndicatorGO;
    public RangeIndicator rangeIndicatorScript;
    public CombatManager CombatManagerScript;
    private Animator animator;
    private AnimScript animScriptS;
    private float maxDistanceFromMonster = 1.2f;
    public bool isWalking = false;
    public bool isIdle = false;
    private Vector3 direction = new Vector3();
    private GameObject animatorObj;
    public GameObject midpoint;
    private NavMeshAgent navMeshAgentPlayer;
    private bool flag1 = false;
    private bool flag2 = false;
    private bool flag3 = false;
    private bool reachedTarget = false;
    private Rigidbody rb;
    public float turnSpeed = 50;
    public Canvas canvasPotion;
    private GameObject canvasPotionGO;
    private GameObject actionChoicesGO;
    private ActionChoices actionChoicesScript;
    private ScoresManager scoresManagerScript;
    private Button potionButton;
    private GameObject zombie1;
    private GameObject zombie2;
    //private GameObject player;

    public void ResetValues()
    {
        zombie1 = GameObject.Find("Zombie 1");
        zombie2 = GameObject.Find("Zombie 2");
        //player = GameObject.Find("Player");
        canvas = GameObject.Find("Canvas1").GetComponent<Canvas>();
        Debug.Log(canvas);
        marker = GameObject.Find("MarkerPrefab");
        attacker = GameObject.Find("Player");
        marker.transform.position = new Vector3(attacker.transform.position.x, attacker.transform.position.y + 2, attacker.transform.position.z);
        canvas.enabled = false;
        //rangeIndicatorGO = Instantiate(Resources.Load<GameObject>("Prefabs/RangeIndicatorPrefab"));
        rangeIndicatorGO = ObjPoolManager.Instance.GetPooledObject("RangeIndicator");
        
        rangeIndicatorGO.transform.position = new Vector3(attacker.transform.position.x, 0.5f, attacker.transform.position.z);
        rangeIndicatorScript = rangeIndicatorGO.GetComponent<RangeIndicator>();
        //rangeIndicatorScript.ResetValues();
        
        CombatManagerScript = GameObject.Find("CombatManager").GetComponent<CombatManager>();
        animator = GameObject.Find("OrkAssasin").GetComponent<Animator>();
        midpoint = GameObject.Find("Midpoint");
        animatorObj = GameObject.Find("AnimatorObj");
        animScriptS = animatorObj.GetComponent<AnimScript>();
        navMeshAgentPlayer = attacker.GetComponent<NavMeshAgent>();
        
        direction = new Vector3();
        flag1 = false;
        flag2 = false;
        flag3 = false;
        reachedTarget = false;
        isWalking = false;
        isIdle = false;
        isUIManagerActivated = false;
        ConfigureNavMeshAgent(navMeshAgentPlayer);
        if (attacker != null)
        {
            rb = attacker.GetComponent<Rigidbody>();
            rb.isKinematic = true;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        zombie1 = GameObject.Find("Zombie 1");
        zombie2 = GameObject.Find("Zombie 2");
        //player = GameObject.Find("Player");
        canvas = GameObject.Find("Canvas1").GetComponent<Canvas>();
        Debug.Log(canvas);
        marker = GameObject.Find("MarkerPrefab");
        attacker = GameObject.Find("Player");
        marker.transform.position = new Vector3(attacker.transform.position.x, attacker.transform.position.y + 2, attacker.transform.position.z);
        canvas.enabled = false;
        if(rangeIndicatorGO == null)
        {
            rangeIndicatorGO = Resources.Load<GameObject>("Prefabs/RangeIndicatorPrefab");
            ObjPoolManager.Instance.CreateObjectPool(rangeIndicatorGO, "RangeIndicator");
        }
        
        rangeIndicatorGO = ObjPoolManager.Instance.GetPooledObject("RangeIndicator");
        
        rangeIndicatorGO.transform.position = new Vector3(attacker.transform.position.x, 0.5f, attacker.transform.position.z);
        rangeIndicatorScript = rangeIndicatorGO.GetComponent<RangeIndicator>();
        CombatManagerScript = GameObject.Find("CombatManager").GetComponent<CombatManager>();
        animator = GameObject.Find("OrkAssasin").GetComponent<Animator>();
        midpoint = GameObject.Find("Midpoint");
        animatorObj = GameObject.Find("AnimatorObj");
        animScriptS = animatorObj.GetComponent<AnimScript>();
        navMeshAgentPlayer = attacker.GetComponent<NavMeshAgent>();
        ConfigureNavMeshAgent(navMeshAgentPlayer);
        if (attacker != null)
        {
            rb = attacker.GetComponent<Rigidbody>();
            rb.isKinematic = true;
        }
        //CreatePotionButton();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        marker.transform.position = new Vector3(attacker.transform.position.x, attacker.transform.position.y + 2, attacker.transform.position.z);
        ClickOnMonster();
    }

    void ClickOnMonster()
    {
        if (!reachedTarget)
        {

            if (Input.GetMouseButtonDown(0) && !IsPointerOverUIElement())
            {

                clickPosition = new Vector3();
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // Define a layer mask that excludes the FogLayer (assuming it's layer 8)
                int layerMask = ~LayerMask.GetMask("FogSystem");
                
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                {
                    clickPosition = hit.point;
                    Hit = hit;
                }
            }
            if (navMeshAgentPlayer.hasPath)
            {
                Vector3 dir = (navMeshAgentPlayer.steeringTarget - attacker.transform.position).normalized;
                Vector3 animDir = attacker.transform.InverseTransformDirection(dir);
                attacker.transform.rotation = Quaternion.RotateTowards(attacker.transform.rotation, Quaternion.LookRotation(dir), 360 * Time.deltaTime);
            }

            if (clickPosition != Vector3.zero && Hit.collider.CompareTag("Monster") && rangeIndicatorScript.targetsInRange.Contains(Hit.transform.gameObject)
                && Vector3.Distance(attacker.transform.position, Hit.transform.position) > maxDistanceFromMonster)
            {
                if (!flag1)
                {
                    rb.isKinematic = false;
                    animScriptS.SetupWalking(attacker, animator);
                    navMeshAgentPlayer.isStopped = false;
                    navMeshAgentPlayer.SetDestination(Hit.transform.position);
                    flag1 = true;
                }
            }

            if (clickPosition != Vector3.zero && Hit.collider.CompareTag("Monster") && rangeIndicatorScript.targetsInRange.Contains(Hit.transform.gameObject) &&
                 Vector3.Distance(attacker.transform.position, Hit.transform.position) <= maxDistanceFromMonster)
            {
                animScriptS.SetUpIdle(attacker, animator);
                rb.isKinematic = true;
                clickPosition = Vector3.zero;
                StopMoving();
                TurnToTarget(40, Hit.transform.gameObject);
                navMeshAgentPlayer.isStopped = true;
                reachedTarget = true;
                ObjPoolManager.Instance.ReturnToPool("RangeIndicator", rangeIndicatorGO);
                ActivateUIManager();
                //DeactivateUIManager();
            }

            if (clickPosition != Vector3.zero && Hit.collider.CompareTag("RangeIndicator"))
            {
                navMeshAgentPlayer.isStopped = false;
                if (!flag2)
                {
                    rb.isKinematic = false;
                    navMeshAgentPlayer.SetDestination(clickPosition);
                    animScriptS.SetupWalking(attacker, animator);
                    flag2 = true;
                }

                if (!navMeshAgentPlayer.pathPending && navMeshAgentPlayer.remainingDistance <= navMeshAgentPlayer.stoppingDistance && !flag3)
                {
                    rb.isKinematic = true;
                    clickPosition = Vector3.zero;
                    reachedTarget = true;
                    animScriptS.SetUpIdle(attacker, animator);
                    navMeshAgentPlayer.isStopped = true;
                    flag3 = true;
                    animScriptS.isRotating = true;
                    ObjPoolManager.Instance.ReturnToPool("RangeIndicator", rangeIndicatorGO);
                    CombatManagerScript.playerTurnCompleted = true;
                }
            }
        }
    }

    void ActivateUIManager()
    {
        if (UIManagerPrefab == null)
        {
            if (!ObjPoolManager.Instance.IsObjectInPool("UIManagerPool"))
            {
                UIManagerPrefab = Resources.Load<GameObject>("Prefabs/UIManagerPrefab");
                ObjPoolManager.Instance.CreateObjectPool(UIManagerPrefab, "UIManagerPool");
                UIManagerPrefab = ObjPoolManager.Instance.GetPooledObject("UIManagerPool");
            }
            else if (ObjPoolManager.Instance.IsObjectInPool("UIManagerPool"))
            {
                UIManagerPrefab = ObjPoolManager.Instance.GetPooledObject("UIManagerPool");
            }

        }
        else if (UIManagerPrefab != null)
        {
            if (ObjPoolManager.Instance.IsObjectInPoolAndInactive("UIManagerPool"))
            {
                UIManagerPrefab = ObjPoolManager.Instance.GetPooledObject("UIManagerPool");
            }
            
        }


    }
    void DeactivateUIManager()
    {
        if (UIManagerPrefab != null)
        {
            ObjPoolManager.Instance.ReturnToPool("UIManagerPool", UIManagerPrefab);
        }        
    }

    private bool IsPointerOverUIElement()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
    private void TurnToTarget(float speed, GameObject target)
    {
        Vector3 direction = (target.transform.position - attacker.transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        attacker.transform.rotation = Quaternion.Lerp(attacker.transform.rotation, targetRotation, Time.deltaTime * speed);
    }

    private void StopMoving()
    {

        if (attacker != null)
        {
            if (navMeshAgentPlayer.isOnNavMesh)
            {
                navMeshAgentPlayer.isStopped = true; // Stop the agent from moving further
                navMeshAgentPlayer.velocity = Vector3.zero; // Ensure the velocity is zero to prevent sliding
            }
        }

    }

    private void ConfigureNavMeshAgent(NavMeshAgent agent)
    {
        agent.avoidancePriority = 50;
        agent.updateRotation = true;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        agent.enabled = true;
        agent.stoppingDistance = 0.1f;

    }

    //private void CreatePotionButton()
    //{
    //	if (CombatManagerScript.currentTurn != null && CombatManagerScript.currentTurn.CompareTag("Player") )
    //	{
    //		if(!GameObject.Find("PotionCanvasPrefab(Clone)"))
    //		{
    //			canvasPotionGO = Instantiate(Resources.Load<GameObject>("Prefabs/PotionCanvasPrefab"));			
    //		}
    //		else
    //		{
    //			canvasPotionGO = GameObject.Find("PotionCanvasPrefab(Clone)");
    //		}			
    //		canvasPotion = canvasPotionGO.GetComponent<Canvas>();
    //		canvasPotion.enabled = false;
    //		scoresManagerScript = GameObject.Find("Canvas2").GetComponent<ScoresManager>();
    //		if (scoresManagerScript.potionsLeftInt >0)
    //		{
    //			canvasPotion.enabled = true;
    //			potionButton = canvasPotionGO.GetComponentInChildren<Button>();			
    //			potionButton.onClick.AddListener(() => OnPotionButtonClick());
    //		}			
    //	}		
    //}
    //private void OnPotionButtonClick()
    //{	
    //	if (!GameObject.Find("ActionChoicesPrefab(Clone)"))
    //	{
    //		actionChoicesGO = Instantiate(Resources.Load<GameObject>("Prefabs/ActionChoicesPrefab"));
    //		actionChoicesScript = actionChoicesGO.GetComponent<ActionChoices>();
    //		actionChoicesScript.Initialize(
    //			combatManager: CombatManagerScript,
    //			scoresManager: scoresManagerScript,
    //			animator: animator,
    //			zombie1: zombie1,
    //			zombie2: zombie2,
    //			player: attacker,
    //			animatorObj: animatorObj
    //		);
    //	}	
    //	actionChoicesScript = actionChoicesGO.GetComponent<ActionChoices>();		
    //	actionChoicesScript.HandleAttackChoice(AttackType.Potion);
    //	if (scoresManagerScript.potionsLeftInt <= 1)
    //	{
    //		canvasPotion.enabled = false;
    //	}
    //}


}
