using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Cinemachine;
using UnityEngine.SceneManagement;

public enum BattleState { START, CHAR1TURN, CHAR2TURN, CHAR3TURN, ENEMY1TURN, ENEMY2TURN, ENEMY3TURN, ENEMY4TURN, CONFCHAR1, CONFCHAR2, CONFCHAR3, ATBCHECK, QUEUECHECK, WON, LOST, LEVELUP, LEVELUPCHECK }
public class BattleSystem : MonoBehaviour
{

    // General Info
    public BattleState state;
    public BattleState currentState;
    public BattleState currentInQueue;
    public Queue<BattleState> battleQueue;
    public ActiveParty activeParty;
    public Enemy[] enemies;
    public EnemyGroup enemyGroup;
    public bool hpRestore, mpRestore;
    public bool physicalAttack = false;
    public bool skillPhysicalAttack = false, skillRangedAttack = false, skillTargetSupport = false, skillSelfSupport = false;
    public bool dropAttack, dropSupport = false;
    public bool usingItem = false;
    public float damageTotal;
    public float skillBoostTotal;
    public int char1Target, char2Target, char3Target;
    public bool char1Attacking, char1Supporting, char2Attacking, char2Supporting, char3Attacking, char3Supporting;
    public bool char1UsingItem, char2UsingItem, char3UsingItem;
    public bool char1DropAttack, char1SkillAttack, char2DropAttack, char2SkillAttack, char3DropAttack, char3SkillAttack,
    char1DropSupport, char2DropSupport, char3DropSupport;
    public bool char1TargetingTeam, char1TargetingEnemy, char2TargetingTeam, char2TargetingEnemy, char3TargetingTeam, char3TargetingEnemy;
    public Drops char1DropChoice, char2DropChoice, char3DropChoice;
    public Skills char1SkillChoice, char2SkillChoice, char3SkillChoice;
    public bool char1PhysicalAttack, char1SkillPhysicalAttack, char1SkillRangedAttack, char1SkillSelfSupport, char1SkillTargetSupport, char2PhysicalAttack, char2SkillPhysicalAttack, char2SkillRangedAttack,
    char2SkillSelfSupport, char2SkillTargetSupport, char3PhysicalAttack, char3SkillPhysicalAttack, char3SkillRangedAttack, char3SkillSelfSupport, char3SkillTargetSupport;
    public bool char1ConfusedReady, char2ConfusedReady, char3ConfusedReady;
    public int enemy1AttackTarget, enemy2AttackTarget, enemy3AttackTarget, enemy4AttackTarget;
    public bool enemy1Ready, enemy2Ready, enemy3Ready, enemy4Ready;
    public bool checkNextInQueue;
    public int previousTargetReferenceChar, previousTargetReferenceEnemy; // For HUD and Drop Reference
    public bool char1Switching, char2Switching, char3Switching;
    public int char1SwitchToIndex, char2SwitchToIndex, char3SwitchToIndex;
    public Item char1ItemToBeUsed, char2ItemToBeUsed, char3ItemToBeUsed;
    public bool isDead;
    public float enemyGroupExperiencePoints;
    public bool animExists = false;
    public bool enemyMoving = false, charMoving = false;
    public Vector3 leaderPos, activeParty2Pos, activeParty3Pos;
    public GameObject[] availableChar1Buttons, availableChar2Buttons, availableChar3Buttons;
    public GameObject char1BattlePanel, char1LevelUpPanel, char2BattlePanel, char2LevelUpPanel, char3BattlePanel, char3LevelUpPanel,
    enemyLootReference, enemyLootPanelReference, enemyPanel, enemyLootCountReference;
    bool charAtBattlePos = true;
    bool charAttacking = false;
    bool enemyAtBattlePos = true;
    bool enemyAttacking = false;
    bool charAttackDrop = false, enemyAttackDrop = false;
    bool confuseAttack = false;
    public Drops lastDropChoice;
    public Skills lastSkillChoice;
    public bool charUsingSkill;
    public int teamTargetChoice;
    public bool attackingTeam = false;
    public float char1ATB, char2ATB, char3ATB, enemy1ATB, enemy2ATB, enemy3ATB, enemy4ATB, ATBReady;
    public Slider char1ATBGuage, char2ATBGuage, char3ATBGuage, enemy1ATBGuage, enemy2ATBGuage, enemy3ATBGuage, enemy4ATBGuage;

    // UI References
    public bool inBattleMenu = false;

    public GameObject char1UI;
    public GameObject char2UI;
    public GameObject char3UI;

    public GameObject[] enemyUI;
    public GameObject[] allyTargetButtons, enemyTargetButtons;
    public TextMeshProUGUI dialogueText;

    public GameObject[] returnToPanelButton;
    public GameObject battleItemMenu;
    public BattleItemSlot[] battleItems;
    public GameObject[] char1MenuButtons;
    public GameObject[] char2MenuButtons;
    public GameObject[] char3MenuButtons;

    // Skills
    public GameObject[] skillButtons;

    // Drops
    public GameObject[] dropsButtons;
    public GameObject fireDropAnim;
    public GameObject iceDropAnim;
    public GameObject waterDropAnim;
    public GameObject lightningDropAnim;
    public GameObject holyDropAnim;
    public GameObject shadowDropAnim, poisonAnim;
    public GameObject sleepAnim, confuseAnim, siphonAnim;

    public GameObject[] switchButtons;
    public GameObject skillListReference;
    public GameObject dropsListReference;


    // Misc Info/References
    public string[] charIndexName;
    int currentIndex;
    int nextIndex;
    public bool dodgedAttack = false;
    public bool battleSwitchButtons = false;
    public bool dontDisplayDmgPopup = false;
    public BattleHUD hud;
    int currentMoveIndex;
    int enemyDropChoice;
    public GameObject damagePopup, levelUpPopup, deathTimerPopup;
    public Material originalMaterial;
    public Material damageFlash;
    public bool failedItemUse = false;
    public GameObject spriteTarget;
    int skillTierChoice;
    bool checkingForRandomDrop = false;
    int randomDropIndex;
    bool partyCheckNext = false;
    List<Drops> charRandomDropList;
    public bool char1Ready = false, char2Ready = false, char3Ready = false;
    bool targetCheck = false;
    public GameObject characterDropTarget;
    bool confuseTargetCheck = false;
    bool char1MenuButtonsActive = false, char2MenuButtonsActive = false, char3MenuButtonsActive = false;
    bool pressUp, pressDown;
    public int inventoryPointerIndex = 0, vertMove = 0;
    public TextMeshProUGUI battleHelpReference;

    bool partyTurn = false;
    [SerializeField]
    bool grieveStatBoost = false, macStatBoost = false, fieldStatBoost = false, riggsStatBoost, targetingTeam, targetingEnemy, settingTarget;
    [SerializeField]
    float grievePhysicalBoost, grieveHealthBoost, grieveManaBoost, grieveEnergyBoost, grieveDodgeBoost, macPhysicalBoost, macHealthBoost,
    macManaBoost, macEnergyBoost, macDodgeBoost, fieldPhysicalBoost, fieldHealthBoost, fieldManaBoost, fieldEnergyBoost, fieldDodgeBoost,
    riggsPhysicalBoost, riggsHealthBoost, riggsManaBoost, riggsEnergyBoost, riggsDodgeBoost, battleBodyTotal, menuTargetIndex;

    public IEnumerator SetupBattle()
    {
        Engine.e.activeParty.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        charAttacking = false;
        enemyAttacking = false;
        enemyAtBattlePos = true;
        damageTotal = 0;
        isDead = false;
        enemyGroup.groupInBattle = true;
        confuseAttack = false;
        ATBReady = 100;
        char1ItemToBeUsed = null;
        char2ItemToBeUsed = null;
        char3ItemToBeUsed = null;
        settingTarget = false;
        targetingTeam = false;
        targetingEnemy = false;

        char1ATB += 80;
        char2ATB = 0;
        char3ATB = 0;
        enemy1ATB = 0;
        enemy2ATB = 0;
        enemy3ATB = 0;
        enemy4ATB = 0;

        lastDropChoice = null;
        lastSkillChoice = null;
        // enemyTargetButtons[0].transform.position = enemies[0].transform.position;

        for (int i = 0; i < activeParty.activeParty.Length; i++)
        {
            if (activeParty.activeParty[i] != null)
            {
                battleBodyTotal++;
            }
        }

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                battleBodyTotal++;
            }
        }


        battleQueue = new Queue<BattleState>(7);

        Engine.e.partyInventoryReference.AddItemsToBattleInventory();

        leaderPos = enemyGroup.GetComponent<Teleport>().toLocation.transform.position;
        activeParty2Pos = enemyGroup.GetComponent<Teleport>().activeParty2Location.transform.position;
        activeParty3Pos = enemyGroup.GetComponent<Teleport>().activeParty3Location.transform.position;

        state = BattleState.START;
        dialogueText.text = string.Empty;
        enemyGroupExperiencePoints = enemyGroup.groupExperienceLevel;
        charIndexName = new string[activeParty.activeParty.Length];

        charIndexName[0] = activeParty.activeParty[0].gameObject.GetComponent<Character>().characterName;

        for (int i = 0; i < Engine.e.activeParty.activeParty.Length; i++)
        {
            if (Engine.e.activeParty.activeParty[1] != null)
            {
                char2UI.SetActive(true);
                charIndexName[1] = activeParty.activeParty[1].gameObject.GetComponent<Character>().characterName;
            }
            if (Engine.e.activeParty.activeParty[2] != null)
            {
                char3UI.SetActive(true);

                charIndexName[2] = activeParty.activeParty[2].gameObject.GetComponent<Character>().characterName;
            }
        }

        EnemyGroup.enemyGroup = this.enemyGroup;
        ActivateEnemyUI();


        yield return new WaitForSeconds(2f);
        //  state = BattleState.CHAR1TURN;
        state = BattleState.ATBCHECK;
        // Char1Turn();

    }

    public void HandleQueue()
    {

        if (!isDead && state != BattleState.WON && state != BattleState.LOST && state != BattleState.LEVELUP && state != BattleState.LEVELUPCHECK)
        {
            if (state == currentInQueue && state != BattleState.ATBCHECK)
            {
                state = BattleState.ATBCHECK;
            }

            if (!battleQueue.Contains(BattleState.CHAR1TURN) && !battleQueue.Contains(BattleState.CONFCHAR1))
            {
                if (activeParty.activeParty[0].GetComponent<Character>().currentHealth > 0
                && !activeParty.activeParty[0].GetComponent<Character>().isAsleep)
                {
                    if (char1ATB < ATBReady)
                    {
                        //char1ATB += (activeParty.activeParty[0].GetComponent<Character>().haste * 10) / 50;
                        char1ATB += activeParty.activeParty[0].GetComponent<Character>().haste * Time.deltaTime;
                        char1ATBGuage.value = char1ATB;

                    }
                    else
                    {
                        char1ATB = ATBReady;
                        char1ATBGuage.value = char1ATB;

                        if (!activeParty.activeParty[0].GetComponent<Character>().isConfused)
                        {
                            if (state == BattleState.ATBCHECK)
                            {
                                Char1Turn();
                            }
                            else
                            {
                                if (!char1Ready)
                                {
                                    if (!activeParty.activeParty[0].GetComponent<Character>().isConfused && !inBattleMenu)
                                    {
                                        ActivateChar1MenuButtons();
                                        char1Ready = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            char1Ready = true;
                            battleQueue.Enqueue(BattleState.CONFCHAR1);
                        }
                    }
                }
            }

            if (!battleQueue.Contains(BattleState.CHAR2TURN) && !battleQueue.Contains(BattleState.CONFCHAR2))
            {
                if (activeParty.activeParty[1] != null)
                {
                    if (activeParty.activeParty[1].GetComponent<Character>().currentHealth > 0
                    && !activeParty.activeParty[1].GetComponent<Character>().isAsleep)
                    {
                        if (char2ATB < ATBReady)
                        {

                            char2ATB += activeParty.activeParty[1].GetComponent<Character>().haste * Time.deltaTime;
                            char2ATBGuage.value = char2ATB;

                        }
                        else
                        {
                            char2ATB = ATBReady;
                            char2ATBGuage.value = char2ATB;

                            if (!activeParty.activeParty[1].GetComponent<Character>().isConfused)
                            {
                                if (state == BattleState.ATBCHECK)
                                //&& !enemyMoving && !enemyAttackDrop && !confuseTargetCheck && !confuseAttack)
                                {
                                    Char2Turn();
                                }
                                else
                                {
                                    if (!char2Ready)
                                    {
                                        if (!activeParty.activeParty[1].GetComponent<Character>().isConfused && !inBattleMenu)
                                        {
                                            ActivateChar2MenuButtons();
                                            char2Ready = true;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                char2Ready = true;
                                battleQueue.Enqueue(BattleState.CONFCHAR2);
                            }
                        }
                    }
                }
            }

            if (!battleQueue.Contains(BattleState.CHAR3TURN) && !battleQueue.Contains(BattleState.CONFCHAR3))
            {
                if (activeParty.activeParty[2] != null)
                {
                    if (activeParty.activeParty[2].GetComponent<Character>().currentHealth > 0
                    && !activeParty.activeParty[2].GetComponent<Character>().isAsleep)
                    {
                        if (char3ATB < ATBReady)
                        {
                            char3ATB += activeParty.activeParty[2].GetComponent<Character>().haste * Time.deltaTime;
                            char3ATBGuage.value = char3ATB;
                        }
                        else
                        {
                            char3ATB = ATBReady;
                            char3ATBGuage.value = char3ATB;

                            if (!activeParty.activeParty[2].GetComponent<Character>().isConfused)
                            {
                                if (state == BattleState.ATBCHECK)
                                //&& !enemyMoving && !enemyAttackDrop && !confuseTargetCheck && !confuseAttack)
                                {
                                    Char3Turn();
                                }
                                else
                                {
                                    if (!char3Ready)
                                    {
                                        if (!activeParty.activeParty[2].GetComponent<Character>().isConfused && !inBattleMenu)
                                        {
                                            ActivateChar3MenuButtons();
                                            char3Ready = true;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                char3Ready = true;
                                battleQueue.Enqueue(BattleState.CONFCHAR3);
                            }
                        }
                    }
                }
            }

            if (!battleQueue.Contains(BattleState.ENEMY1TURN))
            {
                if (enemies[0].GetComponent<Enemy>().health > 0 && !enemies[0].GetComponent<Enemy>().isAsleep)
                {
                    if (enemy1ATB < ATBReady)
                    {
                        float randomVariation = Random.Range(0.65f, 1f);
                        enemy1ATB += ((enemies[0].GetComponent<Enemy>().haste * randomVariation) * Time.deltaTime);
                        enemy1ATBGuage.value = enemy1ATB;
                    }
                    else
                    {
                        enemy1ATB = ATBReady;
                        enemy1Ready = true;
                        battleQueue.Enqueue(BattleState.ENEMY1TURN);
                    }
                }

                if (!battleQueue.Contains(BattleState.ENEMY2TURN))
                {
                    if (enemies[1] != null)
                    {
                        if (enemies[1].GetComponent<Enemy>().health > 0 && !enemies[1].GetComponent<Enemy>().isAsleep)
                        {
                            if (enemy2ATB < ATBReady)
                            {
                                float randomVariation = Random.Range(0.65f, 1f);
                                enemy2ATB += ((enemies[1].GetComponent<Enemy>().haste * randomVariation) * Time.deltaTime);
                                enemy2ATBGuage.value = enemy2ATB;

                            }
                            else
                            {
                                enemy2ATB = ATBReady;
                                enemy2Ready = true;
                                battleQueue.Enqueue(BattleState.ENEMY2TURN);
                            }
                        }
                    }
                }

                if (!battleQueue.Contains(BattleState.ENEMY3TURN))
                {
                    if (enemies[2] != null)
                    {
                        if (enemies[2].GetComponent<Enemy>().health > 0 && !enemies[2].GetComponent<Enemy>().isAsleep)
                        {
                            if (enemy3ATB < ATBReady)
                            {
                                float randomVariation = Random.Range(0.65f, 1f);
                                enemy3ATB += ((enemies[2].GetComponent<Enemy>().haste * randomVariation) * Time.deltaTime);
                                enemy3ATBGuage.value = enemy3ATB;
                            }
                            else
                            {
                                enemy3ATB = ATBReady;
                                enemy3Ready = true;
                                battleQueue.Enqueue(BattleState.ENEMY3TURN);
                            }
                        }
                    }
                }

                if (!battleQueue.Contains(BattleState.ENEMY4TURN))
                {
                    if (enemies[3] != null)
                    {
                        if (enemies[3].GetComponent<Enemy>().health > 0 && !enemies[3].GetComponent<Enemy>().isAsleep)
                        {
                            if (enemy4ATB < ATBReady)
                            {
                                float randomVariation = Random.Range(0.65f, 1f);
                                enemy4ATB += ((enemies[3].GetComponent<Enemy>().haste * randomVariation) * Time.deltaTime);
                                enemy4ATBGuage.value = enemy4ATB;
                            }
                            else
                            {
                                enemy4ATB = ATBReady;
                                enemy4Ready = true;
                                battleQueue.Enqueue(BattleState.ENEMY4TURN);
                            }
                        }
                    }
                }
            }
            //    }

            // Queue Control
            if (battleQueue.Count == 0)
            {
                currentInQueue = BattleState.QUEUECHECK;
            }
            else
            {
                if (currentInQueue == BattleState.QUEUECHECK)
                {
                    currentInQueue = battleQueue.Peek();
                }

                if (currentInQueue == BattleState.CHAR1TURN)
                {
                    if (activeParty.activeParty[0].GetComponent<Character>().currentHealth > 0 && !activeParty.activeParty[0].GetComponent<Character>().isAsleep)
                    {
                        partyTurn = true;

                        targetingEnemy = char1TargetingEnemy;
                        targetingTeam = char1TargetingTeam;

                        if (char1Attacking)
                        {

                            char1Attacking = false;
                            char1Ready = false;

                            physicalAttack = char1PhysicalAttack;
                            char1PhysicalAttack = false;

                            dropAttack = char1DropAttack;
                            char1DropAttack = false;

                            skillPhysicalAttack = char1SkillPhysicalAttack;
                            char1SkillPhysicalAttack = false;

                            skillRangedAttack = char1SkillRangedAttack;
                            char1SkillRangedAttack = false;


                            StartCoroutine(CharAttack(char1Target));


                        }

                        if (char1Supporting)
                        {
                            char1Supporting = false;

                            usingItem = char1UsingItem;
                            char1UsingItem = false;

                            dropSupport = char1DropSupport;
                            char1DropSupport = false;

                            skillSelfSupport = char1SkillSelfSupport;
                            char1SkillSelfSupport = false;

                            skillTargetSupport = char1SkillTargetSupport;
                            char1SkillTargetSupport = false;

                            StartCoroutine(CharSupport(char1Target));

                        }

                        if (char1Switching)
                        {
                            char1Switching = false;
                            StartCoroutine(CharSwitch(char1SwitchToIndex));
                        }
                    }
                    else
                    {
                        battleQueue.Dequeue();
                        currentInQueue = BattleState.QUEUECHECK;

                    }
                }

                if (currentInQueue == BattleState.CHAR2TURN)
                {
                    if (activeParty.activeParty[1].GetComponent<Character>().currentHealth > 0 && !activeParty.activeParty[1].GetComponent<Character>().isAsleep)
                    {
                        partyTurn = true;

                        targetingEnemy = char2TargetingEnemy;
                        targetingTeam = char2TargetingTeam;

                        if (char2Attacking)
                        {

                            char2Attacking = false;
                            char2Ready = false;

                            physicalAttack = char2PhysicalAttack;
                            char2PhysicalAttack = false;

                            dropAttack = char2DropAttack;
                            char2DropAttack = false;

                            skillPhysicalAttack = char2SkillPhysicalAttack;
                            char2SkillPhysicalAttack = false;

                            skillRangedAttack = char2SkillRangedAttack;
                            char2SkillRangedAttack = false;

                            StartCoroutine(CharAttack(char2Target));

                        }

                        if (char2Supporting)
                        {
                            char2Supporting = false;

                            usingItem = char2UsingItem;
                            char2UsingItem = false;

                            dropSupport = char2DropSupport;
                            char2DropSupport = false;

                            skillSelfSupport = char2SkillSelfSupport;
                            char2SkillSelfSupport = false;

                            skillTargetSupport = char2SkillTargetSupport;
                            char2SkillTargetSupport = false;

                            StartCoroutine(CharSupport(char2Target));

                        }

                        if (char2Switching)
                        {
                            char2Switching = false;
                            StartCoroutine(CharSwitch(char2SwitchToIndex));
                        }
                    }
                    else
                    {
                        battleQueue.Dequeue();
                        currentInQueue = BattleState.QUEUECHECK;

                    }
                }

                if (currentInQueue == BattleState.CHAR3TURN)
                {
                    if (activeParty.activeParty[2].GetComponent<Character>().currentHealth > 0 && !activeParty.activeParty[2].GetComponent<Character>().isAsleep)
                    {
                        partyTurn = true;

                        targetingEnemy = char3TargetingEnemy;
                        targetingTeam = char3TargetingTeam;

                        if (char3Attacking)
                        {

                            char3Attacking = false;
                            char3Ready = false;

                            physicalAttack = char3PhysicalAttack;
                            char3PhysicalAttack = false;

                            dropAttack = char3DropAttack;
                            char3DropAttack = false;

                            skillPhysicalAttack = char3SkillPhysicalAttack;
                            char3SkillPhysicalAttack = false;

                            skillRangedAttack = char3SkillRangedAttack;
                            char3SkillRangedAttack = false;

                            StartCoroutine(CharAttack(char3Target));
                        }

                        if (char3Supporting)
                        {
                            char3Supporting = false;

                            usingItem = char3UsingItem;
                            char3UsingItem = false;

                            dropSupport = char3DropSupport;
                            char3DropSupport = false;

                            skillSelfSupport = char3SkillSelfSupport;
                            char3SkillSelfSupport = false;

                            skillTargetSupport = char3SkillTargetSupport;
                            char3SkillTargetSupport = false;

                            StartCoroutine(CharSupport(char3Target));

                        }

                        if (char3Switching)
                        {
                            char3Switching = false;
                            StartCoroutine(CharSwitch(char3SwitchToIndex));
                        }
                    }
                    else
                    {
                        battleQueue.Dequeue();
                        currentInQueue = BattleState.QUEUECHECK;

                    }
                }

                if (currentInQueue == BattleState.CONFCHAR1 && char1Ready)
                {
                    if (activeParty.activeParty[0].GetComponent<Character>().currentHealth > 0 && !activeParty.activeParty[0].GetComponent<Character>().isAsleep)
                    {
                        partyTurn = true;

                        Char1ConfusedTurn();
                    }
                    else
                    {
                        battleQueue.Dequeue();
                        currentInQueue = BattleState.QUEUECHECK;

                    }
                }

                if (currentInQueue == BattleState.CONFCHAR2 && char2Ready)
                {
                    if (activeParty.activeParty[1].GetComponent<Character>().currentHealth > 0 && !activeParty.activeParty[1].GetComponent<Character>().isAsleep)
                    {
                        partyTurn = true;

                        Char2ConfusedTurn();
                    }
                    else
                    {
                        battleQueue.Dequeue();
                        currentInQueue = BattleState.QUEUECHECK;

                    }
                }

                if (currentInQueue == BattleState.CONFCHAR3 && char3Ready)
                {
                    if (activeParty.activeParty[2].GetComponent<Character>().currentHealth > 0 && !activeParty.activeParty[2].GetComponent<Character>().isAsleep)
                    {
                        partyTurn = true;

                        Char3ConfusedTurn();
                    }
                    else
                    {
                        battleQueue.Dequeue();
                        currentInQueue = BattleState.QUEUECHECK;

                    }
                }

                if (currentInQueue == BattleState.ENEMY1TURN && enemy1Ready)
                {
                    if (enemies[0].GetComponent<Enemy>().health > 0 && !enemies[0].GetComponent<Enemy>().isAsleep)
                    {
                        partyTurn = false;

                        Enemy1Turn();
                    }
                    else
                    {
                        battleQueue.Dequeue();
                        currentInQueue = BattleState.QUEUECHECK;

                    }
                }
                if (currentInQueue == BattleState.ENEMY2TURN && enemy2Ready)
                {
                    if (enemies[1].GetComponent<Enemy>().health > 0 && !enemies[1].GetComponent<Enemy>().isAsleep)
                    {
                        partyTurn = false;

                        Enemy2Turn();
                    }
                    else
                    {
                        battleQueue.Dequeue();
                        currentInQueue = BattleState.QUEUECHECK;

                    }
                }
                if (currentInQueue == BattleState.ENEMY3TURN && enemy3Ready)
                {
                    if (enemies[2].GetComponent<Enemy>().health > 0 && !enemies[2].GetComponent<Enemy>().isAsleep)
                    {
                        partyTurn = false;

                        Enemy3Turn();
                    }
                    else
                    {
                        battleQueue.Dequeue();
                        currentInQueue = BattleState.QUEUECHECK;

                    }
                }
                if (currentInQueue == BattleState.ENEMY4TURN && enemy4Ready)
                {
                    if (enemies[3].GetComponent<Enemy>().health > 0 && !enemies[3].GetComponent<Enemy>().isAsleep)
                    {
                        partyTurn = false;

                        Enemy4Turn();
                    }
                    else
                    {
                        battleQueue.Dequeue();
                        currentInQueue = BattleState.QUEUECHECK;

                    }
                }
            }
        }
    }

    public IEnumerator CharAttack(int _target)
    {

        DeactivateTargetButtons();
        inBattleMenu = false;
        int index = 0;
        int target = 0;
        GameObject characterAttacking = null;
        Character _characterAttacking = null;

        if (currentInQueue == BattleState.CHAR1TURN)
        {
            index = 0;
            characterAttacking = Engine.e.activeParty.gameObject;
            attackingTeam = char1TargetingTeam;
            _characterAttacking = activeParty.activeParty[0].GetComponent<Character>();
        }
        if (currentInQueue == BattleState.CHAR2TURN)
        {
            index = 1;
            characterAttacking = Engine.e.activePartyMember2;
            attackingTeam = char2TargetingTeam;
            _characterAttacking = activeParty.activeParty[1].GetComponent<Character>();

        }
        if (currentInQueue == BattleState.CHAR3TURN)
        {
            index = 2;
            characterAttacking = Engine.e.activePartyMember3;
            attackingTeam = char3TargetingTeam;
            _characterAttacking = activeParty.activeParty[2].GetComponent<Character>();

        }

        if (!attackingTeam)
        {
            if (enemies[_target].GetComponent<Enemy>().health <= 0)
            {
                target = enemyGroup.GetRandomRemainingEnemy();
            }
            else
            {
                target = _target;
            }

            previousTargetReferenceChar = target;

            dialogueText.text = string.Empty;

            if (physicalAttack == true)
            {
                charMoving = true;
                targetCheck = true;
                physicalAttack = false;
                charAttacking = true;
            }

            if (skillPhysicalAttack == true)
            {

                skillPhysicalAttack = false;

                Skills _skillChoice = null;

                if (index == 0)
                {
                    _skillChoice = char1SkillChoice;
                }
                if (index == 1)
                {
                    _skillChoice = char2SkillChoice;
                }
                if (index == 2)
                {
                    _skillChoice = char3SkillChoice;
                }

                float skillModifier;
                skillBoostTotal = 0f;


                skillModifier = ((_characterAttacking.skillScale) / 2);

                switch (_skillChoice.skillIndex)
                {
                    case 0: // Berserk
                        damageTotal = _characterAttacking.physicalDamage * 2;
                        break;

                    case 1:

                        break;
                    case 10: // Steal
                        dontDisplayDmgPopup = true;
                        damageTotal = 0;
                        break;
                    case 11: // Daze
                        dontDisplayDmgPopup = true;
                        damageTotal = 0;
                        break;
                    case 13: // Envenom
                        damageTotal = Mathf.Round(_skillChoice.skillPower + (_skillChoice.skillPower * _characterAttacking.shadowDropsLevel / 20) * skillModifier);
                        break;
                    case 14: // Assassinate
                        damageTotal = 99999;
                        break;

                }


                _characterAttacking.currentEnergy -= Mathf.Round((_skillChoice.skillCost
                - _skillChoice.skillCost * _characterAttacking.skillCostReduction / 100) + 0.45f);


                charUsingSkill = true;
                charMoving = true;
                targetCheck = true;
                charAttacking = true;

                // Check "AttackMoveToTargetChar()" for more details on each Skill

            }

            if (skillRangedAttack == true)
            {
                skillRangedAttack = false;

                Skills _skillChoice = null;

                if (index == 0)
                {
                    _skillChoice = char1SkillChoice;
                }
                if (index == 1)
                {
                    _skillChoice = char2SkillChoice;
                }
                if (index == 2)
                {
                    _skillChoice = char3SkillChoice;
                }

                float skillModifier;
                skillBoostTotal = 0f;


                skillModifier = ((_characterAttacking.skillScale) / 2);

                switch (_skillChoice.skillIndex)
                {

                    case 5:
                        mpRestore = true;
                        _characterAttacking.currentMana += (_skillChoice.skillPointReturn + (_skillChoice.skillPointReturn * skillModifier));
                        skillBoostTotal = (_skillChoice.skillPointReturn + (_skillChoice.skillPointReturn * skillModifier));
                        Instantiate(siphonAnim, enemies[target].transform.position, Quaternion.identity);

                        if (_characterAttacking.currentMana > _characterAttacking.maxMana)
                        {
                            _characterAttacking.currentMana = _characterAttacking.maxMana;
                        }
                        damageTotal = skillBoostTotal;
                        break;
                    case 16:
                        //skillRangedAttack = true;
                        damageTotal = Mathf.Round(_skillChoice.skillPower + (_skillChoice.skillPower * _characterAttacking.holyDropsLevel / 20) * skillModifier);
                        //Instantiate(lightningDropAnim, characterGameObject.transform.position, Quaternion.identity);
                        break;

                }


                _characterAttacking.currentEnergy -= Mathf.Round((_skillChoice.skillCost
                - _skillChoice.skillCost * _characterAttacking.skillCostReduction / 100) + 0.45f);

                targetCheck = true;
                charMoving = false;
                charAttackDrop = true;
                skillRangedAttack = false;
                enemies[_target].GetComponent<Enemy>().TakeSkillDamage(damageTotal, _target);
                //hud.displayMana[index].text = activeParty.activeParty[index].gameObject.GetComponent<Character>().currentMana.ToString();

                if (enemies[_target].gameObject.GetComponent<Enemy>().health <= 0)
                {
                    enemies[_target].gameObject.GetComponent<Enemy>().health = 0;

                    isDead = EnemyGroup.enemyGroup.CheckEndBattle();
                    yield return new WaitForSeconds(0.1f);
                }

            }

            if (dropAttack == true)
            {
                Drops dropChoice = null;

                if (index == 0)
                {
                    dropChoice = char1DropChoice;
                }
                if (index == 1)
                {
                    dropChoice = char2DropChoice;
                }
                if (index == 2)
                {
                    dropChoice = char3DropChoice;
                }

                charMoving = false;
                targetCheck = true;

                charAttackDrop = true;

                enemies[_target].gameObject.GetComponent<Enemy>().TakeDropDamage(_target, dropChoice);
                activeParty.activeParty[index].gameObject.GetComponent<Character>().currentMana -= Mathf.Round(dropChoice.dropCost
                - (dropChoice.dropCost * activeParty.activeParty[index].GetComponent<Character>().dropCostReduction / 100) + 0.45f);
                dropAttack = false;

                if (enemies[_target].gameObject.GetComponent<Enemy>().health <= 0)
                {
                    enemies[_target].gameObject.GetComponent<Enemy>().health = 0;

                    isDead = EnemyGroup.enemyGroup.CheckEndBattle();
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
        else
        {
            if (activeParty.activeParty[_target].GetComponent<Character>().currentHealth <= 0)
            {
                target = Engine.e.GetRandomRemainingPartyMember();
            }
            else
            {
                target = _target;
            }

            previousTargetReferenceChar = target;

            dialogueText.text = string.Empty;

            if (physicalAttack == true)
            {
                charMoving = true;
                targetCheck = true;
                physicalAttack = false;
                charAttacking = true;
            }

            if (skillPhysicalAttack == true)
            {
                charUsingSkill = true;
                charMoving = true;
                targetCheck = true;

                skillPhysicalAttack = false;
                charAttacking = true;
                skillPhysicalAttack = false;

                Skills _skillChoice = null;

                if (index == 0)
                {
                    _skillChoice = char1SkillChoice;
                }
                if (index == 1)
                {
                    _skillChoice = char2SkillChoice;
                }
                if (index == 2)
                {
                    _skillChoice = char3SkillChoice;
                }

                float skillModifier;
                skillBoostTotal = 0f;


                skillModifier = ((_characterAttacking.skillScale) / 2);

                switch (_skillChoice.skillIndex)
                {
                    case 0:
                        damageTotal = _characterAttacking.physicalDamage * 2;
                        break;

                    case 1:

                        break;
                    case 10:
                        dontDisplayDmgPopup = true;
                        damageTotal = 0;
                        break;
                    case 13:
                        //skillPhysicalAttack = true;
                        damageTotal = Mathf.Round(_skillChoice.skillPower + (_skillChoice.skillPower * _characterAttacking.shadowDropsLevel / 20) * skillModifier);
                        break;
                    case 14:
                        //skillPhysicalAttack = true;
                        damageTotal = 99999;
                        break;

                }

                if (_skillChoice.skillName != "Steal")
                {
                    _characterAttacking.currentEnergy -= Mathf.Round((_skillChoice.skillCost
                    - _skillChoice.skillCost * _characterAttacking.skillCostReduction / 100) + 0.45f);
                }

                //hud.displayEnergy[index].text = activeParty.activeParty[index].gameObject.GetComponent<Character>().currentEnergy.ToString();

            }

            if (skillRangedAttack == true)
            {
                skillPhysicalAttack = false;

                Skills _skillChoice = null;

                if (index == 0)
                {
                    _skillChoice = char1SkillChoice;
                }
                if (index == 1)
                {
                    _skillChoice = char2SkillChoice;
                }
                if (index == 2)
                {
                    _skillChoice = char3SkillChoice;
                }

                float skillModifier;
                skillBoostTotal = 0f;


                skillModifier = ((_characterAttacking.skillScale) / 2);

                switch (_skillChoice.skillIndex)
                {

                    case 5:
                        mpRestore = true;
                        _characterAttacking.currentMana += (_skillChoice.skillPointReturn + (_skillChoice.skillPointReturn * skillModifier));
                        skillBoostTotal = (_skillChoice.skillPointReturn + (_skillChoice.skillPointReturn * skillModifier));
                        GameObject targetGO = null;

                        if (target == 0)
                        {
                            targetGO = Engine.e.activeParty.gameObject;
                        }
                        if (target == 1)
                        {
                            targetGO = Engine.e.activePartyMember2.gameObject;
                        }
                        if (target == 2)
                        {
                            targetGO = Engine.e.activePartyMember3.gameObject;
                        }

                        Instantiate(siphonAnim, targetGO.transform.position, Quaternion.identity);

                        if (_characterAttacking.currentMana > _characterAttacking.maxMana)
                        {
                            _characterAttacking.currentMana = _characterAttacking.maxMana;
                        }
                        damageTotal = skillBoostTotal;
                        break;

                    case 16:
                        //skillRangedAttack = true;
                        damageTotal = Mathf.Round(_skillChoice.skillPower + (_skillChoice.skillPower * _characterAttacking.holyDropsLevel / 20) * skillModifier);
                        //Instantiate(lightningDropAnim, characterGameObject.transform.position, Quaternion.identity);
                        break;

                }


                _characterAttacking.currentEnergy -= Mathf.Round((_skillChoice.skillCost
                - _skillChoice.skillCost * _characterAttacking.skillCostReduction / 100) + 0.45f);

                targetCheck = true;
                charMoving = false;
                charAttackDrop = true;
                skillRangedAttack = false;

                Engine.e.TakeDamage(_target, damageTotal, 100);
                //hud.displayMana[index].text = activeParty.activeParty[index].gameObject.GetComponent<Character>().currentMana.ToString();

                targetCheck = true;
                charMoving = false;
                charAttackDrop = true;
                skillRangedAttack = false;
                //enemies[enemyTarget].GetComponent<Enemy>().TakeSkillDamage(damageTotal, enemyTarget);
                //hud.displayMana[index].text = activeParty.activeParty[index].gameObject.GetComponent<Character>().currentMana.ToString();

                // if (enemies[enemyTarget].gameObject.GetComponent<Enemy>().health <= 0)
                //{
                //    enemies[enemyTarget].gameObject.GetComponent<Enemy>().health = 0;

                //     isDead = EnemyGroup.enemyGroup.CheckEndBattle();
                //      yield return new WaitForSeconds(0.1f);
                //  }
            }

            if (dropAttack == true)
            {
                dropAttack = false;

                Drops dropChoice = null;

                if (index == 0)
                {
                    dropChoice = char1DropChoice;
                }
                if (index == 1)
                {
                    dropChoice = char2DropChoice;
                }
                if (index == 2)
                {
                    dropChoice = char3DropChoice;
                }

                charMoving = false;
                targetCheck = true;

                charAttackDrop = true;

                Engine.e.InstantiateEnemyDropTeam(characterAttacking, target);
                isDead = Engine.e.TakeElementalDamage(target, lastDropChoice.dropPower, lastDropChoice.dropType);
                _characterAttacking.currentMana -= Mathf.Round(dropChoice.dropCost
                - (dropChoice.dropCost * _characterAttacking.dropCostReduction / 100) + 0.45f);
                //hud.displayMana[index].text = activeParty.activeParty[index].gameObject.GetComponent<Character>().currentMana.ToString();

            }
        }
    }

    public IEnumerator AttackMoveToTargetChar()
    {
        int targetEnemy = 0;
        int index = 0;
        GameObject characterAttacking = null;
        Character characterAttackIndex = null;
        GameObject characterTarget = null;
        Enemy enemy = null;

        if (currentInQueue == BattleState.CHAR1TURN || currentInQueue == BattleState.CONFCHAR1)
        {
            index = 0;
            targetEnemy = char1Target;
            characterAttacking = Engine.e.activeParty.gameObject;
            characterAttackIndex = Engine.e.activeParty.activeParty[0].GetComponent<Character>();

            if (!attackingTeam)
            {
                enemy = enemies[targetEnemy].GetComponent<Enemy>();
            }
            else
            {
                if (char1Target == 0)
                {
                    characterTarget = Engine.e.activeParty.gameObject;
                }
                if (char1Target == 1)
                {
                    characterTarget = Engine.e.activePartyMember2;
                }
                if (char1Target == 2)
                {
                    characterTarget = Engine.e.activePartyMember3;
                }

                Engine.e.PhysicalDamageCalculation(char1Target, characterAttackIndex.physicalDamage);
            }
        }
        if (currentInQueue == BattleState.CHAR2TURN || currentInQueue == BattleState.CONFCHAR2)
        {
            index = 1;
            targetEnemy = char2Target;
            characterAttacking = Engine.e.activePartyMember2;
            characterAttackIndex = Engine.e.activeParty.activeParty[1].GetComponent<Character>();

            if (!attackingTeam)
            {
                enemy = enemies[targetEnemy].GetComponent<Enemy>();
            }
            else
            {
                if (char2Target == 0)
                {
                    characterTarget = Engine.e.activeParty.gameObject;
                }
                if (char2Target == 1)
                {
                    characterTarget = Engine.e.activePartyMember2;
                }
                if (char2Target == 2)
                {
                    characterTarget = Engine.e.activePartyMember3;
                }

                Engine.e.PhysicalDamageCalculation(char2Target, characterAttackIndex.physicalDamage);
            }
        }

        if (currentInQueue == BattleState.CHAR3TURN || currentInQueue == BattleState.CONFCHAR3)
        {
            index = 2;
            targetEnemy = char3Target;
            characterAttacking = Engine.e.activePartyMember3;
            characterAttackIndex = Engine.e.activeParty.activeParty[2].GetComponent<Character>();

            if (!attackingTeam)
            {
                enemy = enemies[targetEnemy].GetComponent<Enemy>();
            }
            else
            {
                if (char3Target == 0)
                {
                    characterTarget = Engine.e.activeParty.gameObject;
                }
                if (char3Target == 1)
                {
                    characterTarget = Engine.e.activePartyMember2;
                }
                if (char3Target == 2)
                {
                    characterTarget = Engine.e.activePartyMember3;
                }

                Engine.e.PhysicalDamageCalculation(char3Target, characterAttackIndex.physicalDamage);
            }
        }

        if (targetCheck)
        {
            targetCheck = false;


            if (Engine.e.battleModeActive && !activeParty.activeParty[index].GetComponent<Character>().isConfused)
            {
                //    ActiveCheckNext();
                //state = BattleState.ATBCHECK;

            }
        }

        if (charAttacking && charAtBattlePos)
        {
            switch (index)
            {
                case 0:
                    char1ATB = 0;
                    char1Ready = false;
                    break;
                case 1:
                    char2ATB = 0;
                    char2Ready = false;
                    break;
                case 2:
                    char3ATB = 0;
                    char3Ready = false;
                    break;
            }

            enemyGroup.moveToPosition = false;

            if (!attackingTeam)
            {
                Vector3 targetPos = Vector3.MoveTowards(characterAttacking.GetComponent<Rigidbody2D>().transform.position, enemies[targetEnemy].transform.position, 8f * Time.deltaTime);

                characterAttacking.GetComponent<Rigidbody2D>().MovePosition(targetPos);

                if (Vector3.Distance(characterAttacking.transform.position, enemies[targetEnemy].transform.position) < 1)
                {

                    charAttacking = false;
                    charAtBattlePos = false;

                    if (!charUsingSkill)
                    {
                        enemy.TakePhysicalDamage(targetEnemy, characterAttackIndex.physicalDamage, characterAttackIndex.hitChance);
                    }
                    else
                    {
                        Skills _skillChoice = null;

                        if (index == 0)
                        {
                            _skillChoice = char1SkillChoice;
                        }
                        if (index == 1)
                        {
                            _skillChoice = char2SkillChoice;
                        }
                        if (index == 2)
                        {
                            _skillChoice = char3SkillChoice;
                        }

                        switch (_skillChoice.skillIndex)
                        {
                            case 0: // Berserk
                                enemy.TakeSkillDamage(damageTotal, targetEnemy);
                                break;
                            case 10: // Steal
                                enemy.StealAttempt(targetEnemy, characterAttackIndex.stealChance);
                                break;
                            case 11: // Daze
                                float hitChanceReduction = 10 + Mathf.Round(characterAttackIndex.skillScale * 10 / 25);
                                enemy.hitChance -= hitChanceReduction;
                                break;
                            case 13: // Envenom
                                enemy.TakeSkillDamage(damageTotal, targetEnemy);
                                enemy.InflictPoisonAttack(index, 10);
                                break;
                            case 14: // Assassinate
                                enemy.InflictDeathAttack();
                                break;
                        }
                    }

                    if (!dontDisplayDmgPopup)
                    {
                        GameObject dmgPopup = Instantiate(damagePopup, enemies[targetEnemy].transform.position, Quaternion.identity);

                        if (dodgedAttack == true)
                        {
                            dmgPopup.transform.GetChild(0).GetComponent<TextMeshPro>().text = "Dodged";
                            dodgedAttack = false;

                        }
                        else
                        {
                            enemies[targetEnemy].GetComponent<SpriteRenderer>().material = damageFlash;
                            dmgPopup.transform.GetChild(0).GetComponent<TextMeshPro>().text = enemy.damageTotal.ToString();

                            yield return new WaitForSeconds(0.2f);
                            enemies[targetEnemy].GetComponent<SpriteRenderer>().material = originalMaterial;
                        }

                        Destroy(dmgPopup, 0.8f);
                    }
                    else
                    {
                        dontDisplayDmgPopup = false;
                    }
                }
                hud.displayEnemyHealth[targetEnemy].text = enemies[targetEnemy].gameObject.GetComponent<Enemy>().health.ToString();
            }
            else
            {
                Vector3 targetPos = Vector3.MoveTowards(characterAttacking.GetComponent<Rigidbody2D>().transform.position, characterTarget.transform.position, 8f * Time.deltaTime);

                characterAttacking.GetComponent<Rigidbody2D>().MovePosition(targetPos);

                if (Vector3.Distance(characterAttacking.transform.position, characterTarget.transform.position) < 1)
                {

                    charAttacking = false;
                    charAtBattlePos = false;


                    Engine.e.TakeDamage(targetEnemy, characterAttackIndex.physicalDamage, characterAttackIndex.hitChance);


                    if (!dontDisplayDmgPopup)
                    {
                        GameObject dmgPopup = Instantiate(damagePopup, characterTarget.transform.position, Quaternion.identity);

                        if (dodgedAttack == true)
                        {
                            dmgPopup.transform.GetChild(0).GetComponent<TextMeshPro>().text = "Dodged";
                            dodgedAttack = false;

                        }
                        else
                        {
                            characterTarget.GetComponent<SpriteRenderer>().material = damageFlash;
                            dmgPopup.transform.GetChild(0).GetComponent<TextMeshPro>().text = damageTotal.ToString();

                            yield return new WaitForSeconds(0.2f);
                            characterTarget.GetComponent<SpriteRenderer>().material = originalMaterial;
                        }

                        Destroy(dmgPopup, 0.8f);
                    }
                    else
                    {
                        dontDisplayDmgPopup = false;
                    }
                }
                hud.displayHealth[targetEnemy].text = activeParty.activeParty[targetEnemy].gameObject.GetComponent<Character>().currentHealth.ToString();
            }
        }

        if (!charAttacking && !charAtBattlePos)
        {
            enemyGroup.moveToPosition = true;

            if (index == 0)
            {
                if (characterAttacking.transform.position == leaderPos)
                {
                    charAtBattlePos = true;
                }
            }
            if (index == 1)
            {
                if (characterAttacking.transform.position == activeParty2Pos)
                {
                    charAtBattlePos = true;
                }
            }
            if (index == 2)
            {
                if (characterAttacking.transform.position == activeParty3Pos)
                {
                    charAtBattlePos = true;
                }
            }

            if (charAtBattlePos && !charAttacking)
            {
                if (!attackingTeam)
                {
                    if (enemies[targetEnemy].gameObject.GetComponent<Enemy>().health <= 0)
                    {
                        enemies[targetEnemy].gameObject.GetComponent<Enemy>().health = 0;

                        isDead = EnemyGroup.enemyGroup.CheckEndBattle();

                        if (isDead)
                        {
                            state = BattleState.LEVELUPCHECK;
                            yield return new WaitForSeconds(2f);
                            StartCoroutine(LevelUpCheck());
                        }
                    }
                }
                else
                {
                    attackingTeam = false;
                }

                EndTurn();
                //state = BattleState.ATBCHECK;
                //currentInQueue = nextInQueue;


                yield return new WaitForSeconds(0.3f);

                if (!Engine.e.battleModeActive)
                {
                    state = BattleState.ATBCHECK;
                }
                else
                {


                }
                // StartCoroutine(CheckNext());
            }
        }
    }

    IEnumerator CharDropAttack()
    {

        if (!animExists)
        {
            charAttackDrop = false;
            // currentInQueue = nextInQueue;
        }
        else
        {
            if (state == currentInQueue)
                state = BattleState.ATBCHECK;

        }
        if (targetCheck)
        {
            targetCheck = false;

            if (Engine.e.battleModeActive)
            {
                //  state = BattleState.ATBCHECK;
                //state = currentState;
            }
        }
        if (!animExists && !charAttackDrop)
        {

            EndTurn();

            //currentInQueue = nextInQueue;
            //if (s)
            //state = BattleState.ATBCHECK;

            yield return new WaitForSeconds(0.3f);

            if (isDead)
            {
                state = BattleState.LEVELUPCHECK;
                yield return new WaitForSeconds(1f);
                StartCoroutine(LevelUpCheck());
            }
            else
            {

                if (!Engine.e.battleModeActive)
                {
                    state = BattleState.ATBCHECK;
                }

            }
            //StartCoroutine(CheckNext());
        }
    }

    private void EnableButtonInteraction()
    {
        if (char1Ready)
        {
            for (int i = 0; i < char1MenuButtons.Length; i++)
            {
                char1MenuButtons[i].GetComponent<Button>().interactable = true;
            }
        }
        if (char2Ready)
        {
            for (int i = 0; i < char2MenuButtons.Length; i++)
            {
                char2MenuButtons[i].GetComponent<Button>().interactable = true;
            }
        }
        if (char3Ready)
        {
            for (int i = 0; i < char3MenuButtons.Length; i++)
            {
                char3MenuButtons[i].GetComponent<Button>().interactable = true;
            }
        }
    }

    private void DisableButtonInteraction()
    {
        if (char1Ready)
        {
            for (int i = 0; i < char1MenuButtons.Length; i++)
            {
                char1MenuButtons[i].GetComponent<Button>().interactable = false;
            }
        }
        if (char2Ready)
        {
            for (int i = 0; i < char2MenuButtons.Length; i++)
            {
                char2MenuButtons[i].GetComponent<Button>().interactable = false;
            }
        }
        if (char3Ready)
        {
            for (int i = 0; i < char3MenuButtons.Length; i++)
            {
                char3MenuButtons[i].GetComponent<Button>().interactable = false;
            }
        }
    }

    public IEnumerator CharConfuseAttack()
    {
        int index = 0;
        GameObject character = null;
        int target = 0;
        Character _characterAttacking = null;

        if (currentInQueue == BattleState.CONFCHAR1)
        {
            index = 0;
            character = Engine.e.activeParty.gameObject;
            target = char1Target;
            _characterAttacking = activeParty.activeParty[0].GetComponent<Character>();
        }
        if (currentInQueue == BattleState.CONFCHAR2)
        {
            index = 1;
            character = Engine.e.activePartyMember2;
            target = char2Target;
            _characterAttacking = activeParty.activeParty[1].GetComponent<Character>();

        }
        if (currentInQueue == BattleState.CONFCHAR3)
        {
            index = 2;
            character = Engine.e.activePartyMember3;
            target = char3Target;
            _characterAttacking = activeParty.activeParty[2].GetComponent<Character>();

        }

        yield return new WaitForSeconds(0.3f);


        Debug.Log("entered CharConfuseAttack");

        int choiceAttack = Random.Range(0, 100);
        Debug.Log(choiceAttack);
        if (choiceAttack < 70)
        {
            charMoving = true;
            physicalAttack = true;
            charAttacking = true;
            targetCheck = true;

        }
        else
        {
            if (Engine.e.activeParty.activeParty[index].GetComponent<Character>().currentMana >= 15)
            {
                charAttackDrop = true;

                Debug.Log("herewego");
                confuseAttack = false;
                checkingForRandomDrop = true;
                if (checkingForRandomDrop == true)
                {
                    checkingForRandomDrop = false;
                    CharRandomDropChoice();
                }

                if (lastDropChoice != null)
                {
                    if (teamTargetChoice == 0)
                    {
                        physicalAttack = false;
                        Engine.e.InstantiateEnemyDropTeam(character, index);
                        isDead = Engine.e.TakeElementalDamage(target, lastDropChoice.dropPower, lastDropChoice.dropType);
                    }
                    else
                    {

                        enemies[target].gameObject.GetComponent<Enemy>().TakeDropDamage(target, lastDropChoice);

                        if (enemies[target].gameObject.GetComponent<Enemy>().health <= 0)
                        {
                            enemies[target].gameObject.GetComponent<Enemy>().health = 0;

                            isDead = EnemyGroup.enemyGroup.CheckEndBattle();

                            //state = BattleState.ATBCHECK;
                            yield return new WaitForSeconds(0.1f);

                        }
                    }
                }

                confuseAttack = false;
                Debug.Log("Cost: " + lastDropChoice.dropCost);

                activeParty.activeParty[index].GetComponent<Character>().currentMana -= Mathf.Round(lastDropChoice.dropCost
            - (lastDropChoice.dropCost * activeParty.activeParty[index].GetComponent<Character>().dropCostReduction / 100) + 0.45f);

                confuseAttack = false;
            }
            else
            {

                charMoving = true;
                physicalAttack = true;
                charAttacking = true;
                targetCheck = true;

            }
        }
    }

    public IEnumerator CharSupport(int _target)
    {

        int index = 0;
        GameObject character = null;
        inBattleMenu = false;

        if (currentInQueue == BattleState.CHAR1TURN)
        {
            index = 0;
            character = Engine.e.activeParty.gameObject;

        }
        if (currentInQueue == BattleState.CHAR2TURN)
        {
            index = 1;
            character = Engine.e.activePartyMember2;

        }
        if (currentInQueue == BattleState.CHAR3TURN)
        {
            index = 2;
            character = Engine.e.activePartyMember3;

        }

        // Supporting target via an Item
        if (usingItem)
        {
            usingItem = false;

            Item _itemToBeUsed = null;

            if (index == 0)
            {
                _itemToBeUsed = char1ItemToBeUsed;
            }
            if (index == 1)
            {
                _itemToBeUsed = char2ItemToBeUsed;
            }
            if (index == 2)
            {
                _itemToBeUsed = char3ItemToBeUsed;
            }


            if (!targetingEnemy)
            {
                Engine.e.UseItemInBattle(_target);
            }
            else
            {
                enemies[_target].UseItem(_itemToBeUsed);
            }

            // EndTurn();
            // yield return new WaitForSeconds(1f);
            // if (!Engine.e.battleModeActive)
            // {
            //     state = BattleState.ATBCHECK;
            // }
        }

        // Supporting target via Drop
        if (dropSupport)
        {

            Drops _dropChoice = null;

            dropSupport = false;

            if (index == 0)
            {
                _dropChoice = char1DropChoice;
            }
            if (index == 1)
            {
                _dropChoice = char2DropChoice;

            }
            if (index == 2)
            {
                _dropChoice = char3DropChoice;

            }

            if (targetingTeam)
            {
                if (activeParty.activeParty[index].GetComponent<Character>().currentMana >= _dropChoice.dropCost)
                {
                    {

                        switch (_dropChoice.dropName)
                        {
                            case "Holy Light":
                                if (activeParty.activeParty[_target].GetComponent<Character>().currentHealth > 0)
                                {
                                    Instantiate(holyDropAnim, Engine.e.activeParty.transform.position, Quaternion.identity);
                                    activeParty.activeParty[index].GetComponent<Character>().currentMana -= _dropChoice.dropCost;

                                }
                                else
                                {
                                    //  yield return this;
                                }
                                break;

                            case "Raise":
                                if (activeParty.activeParty[_target].GetComponent<Character>().currentHealth <= 0)
                                {
                                    Instantiate(holyDropAnim, Engine.e.activeParty.transform.position, Quaternion.identity);
                                    activeParty.activeParty[index].GetComponent<Character>().Revive(_dropChoice.dropPower, _target);
                                    activeParty.activeParty[index].GetComponent<Character>().currentMana -= _dropChoice.dropCost;

                                }
                                else
                                {

                                    if (index == 0)
                                    {
                                        Char1Turn();
                                    }
                                    if (index == 1)
                                    {
                                        Char2Turn();
                                    }
                                    if (index == 2)
                                    {
                                        Char3Turn();
                                    }
                                }

                                break;

                            case "Repent":

                                Instantiate(holyDropAnim, Engine.e.activeParty.transform.position, Quaternion.identity);

                                if (activeParty.activeParty[index].GetComponent<Character>().holyDropsLevel >= 20)
                                {
                                    if (activeParty.activeParty[_target].GetComponent<Character>().isPoisoned)
                                    {
                                        activeParty.activeParty[_target].GetComponent<Character>().isPoisoned = false;
                                    }
                                    if (activeParty.activeParty[_target].GetComponent<Character>().isAsleep)
                                    {
                                        activeParty.activeParty[_target].GetComponent<Character>().isAsleep = false;
                                    }
                                    if (activeParty.activeParty[_target].GetComponent<Character>().isConfused)
                                    {
                                        activeParty.activeParty[_target].GetComponent<Character>().isConfused = false;
                                    }
                                    if (activeParty.activeParty[_target].GetComponent<Character>().deathInflicted)
                                    {
                                        activeParty.activeParty[_target].GetComponent<Character>().RemoveDeath();
                                    }
                                    activeParty.activeParty[_target].GetComponent<SpriteRenderer>().color = Color.white;

                                }
                                else
                                {
                                    if (activeParty.activeParty[index].GetComponent<Character>().holyDropsLevel >= 10
                                    && activeParty.activeParty[index].GetComponent<Character>().holyDropsLevel < 20)
                                    {
                                        if (activeParty.activeParty[_target].GetComponent<Character>().isPoisoned)
                                        {
                                            activeParty.activeParty[_target].GetComponent<Character>().isPoisoned = false;
                                            activeParty.activeParty[_target].GetComponent<SpriteRenderer>().color = Color.white;

                                        }
                                        if (activeParty.activeParty[_target].GetComponent<Character>().isAsleep)
                                        {
                                            activeParty.activeParty[_target].GetComponent<Character>().isAsleep = false;
                                            activeParty.activeParty[_target].GetComponent<SpriteRenderer>().color = Color.white;

                                        }

                                    }
                                    else
                                    {
                                        if (activeParty.activeParty[index].GetComponent<Character>().holyDropsLevel < 10)
                                        {
                                            if (activeParty.activeParty[_target].GetComponent<Character>().isAsleep)
                                            {
                                                activeParty.activeParty[_target].GetComponent<Character>().isAsleep = false;
                                                activeParty.activeParty[_target].GetComponent<SpriteRenderer>().color = Color.white;

                                            }
                                        }
                                    }
                                }

                                activeParty.activeParty[index].GetComponent<Character>().currentMana -= _dropChoice.dropCost;

                                break;

                            case "Soothing Rain":

                                Instantiate(holyDropAnim, Engine.e.activeParty.transform.position, Quaternion.identity);
                                activeParty.activeParty[index].GetComponent<Character>().SoothingRain(_dropChoice.dropPower);
                                activeParty.activeParty[index].GetComponent<Character>().currentMana -= _dropChoice.dropCost;


                                break;
                        }

                        if (_dropChoice.dropType == "Holy")
                        {

                            if (activeParty.activeParty[index].GetComponent<Character>().holyDropsLevel < 99)
                            {
                                activeParty.activeParty[index].GetComponent<Character>().holyDropsExperience += _dropChoice.experiencePoints;
                                // Level Up
                                if (activeParty.activeParty[index].GetComponent<Character>().holyDropsExperience >= activeParty.activeParty[index].GetComponent<Character>().holyDropsLvlReq)
                                {
                                    activeParty.activeParty[index].GetComponent<Character>().holyDropsLevel += 1f;
                                    activeParty.activeParty[index].GetComponent<Character>().holyDropsExperience -= activeParty.activeParty[index].GetComponent<Character>().holyDropsLvlReq;
                                    activeParty.activeParty[index].GetComponent<Character>().holyDropsLvlReq = (activeParty.activeParty[index].GetComponent<Character>().holyDropsLvlReq * 3 / 2);
                                    GameObject levelUp = Instantiate(levelUpPopup, character.transform.position, Quaternion.identity);
                                    levelUp.transform.GetChild(0).GetComponent<TextMeshPro>().text = "Holy Lvl Up (Lvl: " + activeParty.activeParty[index].GetComponent<Character>().holyDropsLevel + ")";
                                    Destroy(levelUp, 2f);
                                }
                            }
                        }

                    }

                }
                else
                {
                    Debug.Log("Exception Thrown");
                }
            }
            else
            {
                enemies[_target].TakeDropDamage(index, _dropChoice);

                activeParty.activeParty[index].gameObject.GetComponent<Character>().currentMana -= Mathf.Round(_dropChoice.dropCost
                - (_dropChoice.dropCost * activeParty.activeParty[index].GetComponent<Character>().dropCostReduction / 100) + 0.45f);
                dropAttack = false;

                if (enemies[_target].gameObject.GetComponent<Enemy>().health <= 0)
                {
                    enemies[_target].gameObject.GetComponent<Enemy>().health = 0;

                    isDead = EnemyGroup.enemyGroup.CheckEndBattle();
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }

        // Supporting self via Skill
        if (skillSelfSupport)
        {
            Skills _skillChoice = null;
            Character _selfTarget = activeParty.activeParty[index].GetComponent<Character>();
            skillSelfSupport = false;

            if (index == 0)
            {
                _skillChoice = char1SkillChoice;
            }
            if (index == 1)
            {
                _skillChoice = char2SkillChoice;

            }
            if (index == 2)
            {
                _skillChoice = char3SkillChoice;

            }

            float skillModifier;
            skillBoostTotal = 0f;


            skillModifier = ((_selfTarget.skillScale) / 2);

            if (_selfTarget.currentEnergy >= _skillChoice.skillCost)
            {
                {

                    switch (_skillChoice.skillIndex)
                    {
                        case 15:

                            //Instantiate(holyDropAnim, Engine.e.activeParty.transform.position, Quaternion.identity);
                            //activeParty.activeParty[index].GetComponent<Character>().
                            _selfTarget.currentEnergy -= _skillChoice.skillCost;

                            switch (_selfTarget.characterName)
                            {
                                case "Grieve":
                                    if (grievePhysicalBoost == 0)
                                    {
                                        grieveStatBoost = true;
                                        grievePhysicalBoost = Mathf.Round(_selfTarget.physicalDamage + (_selfTarget.skillScale * 10 / 25));
                                        _selfTarget.physicalDamage += grievePhysicalBoost;
                                    }

                                    break;

                                case "Mac":
                                    if (macPhysicalBoost == 0)
                                    {
                                        macStatBoost = true;
                                        macPhysicalBoost = Mathf.Round(_selfTarget.physicalDamage + (_selfTarget.skillScale * 10 / 25));
                                        _selfTarget.physicalDamage += macPhysicalBoost;
                                    }

                                    break;

                                case "Field":
                                    if (fieldPhysicalBoost == 0)
                                    {
                                        fieldStatBoost = true;
                                        fieldPhysicalBoost = Mathf.Round(_selfTarget.physicalDamage + (_selfTarget.skillScale * 10 / 25));
                                        _selfTarget.physicalDamage += fieldPhysicalBoost;
                                    }
                                    break;

                                case "Riggs":
                                    if (riggsPhysicalBoost == 0)
                                    {
                                        riggsStatBoost = true;
                                        riggsPhysicalBoost = Mathf.Round(_selfTarget.physicalDamage + (_selfTarget.skillScale * 10 / 25));
                                        _selfTarget.physicalDamage += riggsPhysicalBoost;

                                    }
                                    break;

                            }
                            break;
                    }

                }

                // Supporting target via Skill
                if (skillTargetSupport)
                {

                }

            }
        }
        EndTurn();
        yield return new WaitForSeconds(1f);
        if (!Engine.e.battleModeActive)
        {
            state = BattleState.ATBCHECK;
        }
    }


    public IEnumerator CharSwitch(int index)
    {

        DeactivateSupportButtons();
        DeactivateDropsUI();
        DeactivateSkillsUI();
        DeactivateChar1SwitchButtons();

        if (currentInQueue == BattleState.CHAR1TURN)
        {
            DeactivateChar1MenuButtons();
            DeactivateChar1SwitchButtons();
            char1ATB = 0;
            char1ATBGuage.value = char1ATB;
            char1Ready = false;
            Engine.e.activeParty.InstantiateBattleLeader(index);

            hud.displayNames[0].text = activeParty.activeParty[0].gameObject.GetComponent<Character>().characterName;
            hud.displayHealth[0].text = activeParty.activeParty[0].gameObject.GetComponent<Character>().currentHealth.ToString();
            hud.displayMana[0].text = activeParty.activeParty[0].gameObject.GetComponent<Character>().currentMana.ToString();

        }
        if (currentInQueue == BattleState.CHAR2TURN)
        {
            DeactivateChar2MenuButtons();
            DeactivateChar2SwitchButtons();
            char2ATB = 0;
            char2ATBGuage.value = char2ATB;
            char2Ready = false;
            Engine.e.activeParty.InstantiateBattleActiveParty2(index);

            hud.displayNames[1].text = activeParty.activeParty[1].gameObject.GetComponent<Character>().characterName;
            hud.displayHealth[1].text = activeParty.activeParty[1].gameObject.GetComponent<Character>().currentHealth.ToString();
            hud.displayMana[1].text = activeParty.activeParty[1].gameObject.GetComponent<Character>().currentMana.ToString();

        }
        if (currentInQueue == BattleState.CHAR3TURN)
        {
            DeactivateChar3MenuButtons();
            DeactivateChar3SwitchButtons();
            char3ATB = 0;
            char3ATBGuage.value = char3ATB;
            char3Ready = false;

            Engine.e.activeParty.InstantiateBattleActiveParty3(index);

            hud.displayNames[2].text = activeParty.activeParty[2].gameObject.GetComponent<Character>().characterName;
            hud.displayHealth[2].text = activeParty.activeParty[2].gameObject.GetComponent<Character>().currentHealth.ToString();
            hud.displayMana[2].text = activeParty.activeParty[2].gameObject.GetComponent<Character>().currentMana.ToString();
        }


        dialogueText.text = string.Empty;

        yield return new WaitForSeconds(1f);

        if (!Engine.e.battleModeActive)
        {
            StartCoroutine(CheckNext());
        }
        else
        {
            battleQueue.Dequeue();
            state = BattleState.ATBCHECK;
            currentInQueue = BattleState.QUEUECHECK;
        }
    }

    public IEnumerator ConfuseAttackMoveToTargetChar()
    {

        int index = 0;
        GameObject character = null;
        GameObject targetCharacter = null;
        int target = 0;

        if (currentInQueue == BattleState.CHAR1TURN || currentInQueue == BattleState.CONFCHAR1)
        {
            index = 0;
            character = Engine.e.activeParty.gameObject;
            target = char1Target;
        }
        if (currentInQueue == BattleState.CHAR2TURN || currentInQueue == BattleState.CONFCHAR2)
        {
            index = 1;
            character = Engine.e.activePartyMember2;
            target = char2Target;
        }
        if (currentInQueue == BattleState.CHAR3TURN || currentInQueue == BattleState.CONFCHAR3)
        {
            index = 2;
            character = Engine.e.activePartyMember3;
            target = char3Target;
        }

        if (target == 0)
        {
            targetCharacter = Engine.e.activeParty.gameObject;
        }
        if (target == 1)
        {
            targetCharacter = Engine.e.activePartyMember2.gameObject;
        }
        if (target == 2)
        {
            targetCharacter = Engine.e.activePartyMember3.gameObject;
        }

        // ActiveCheckNext();

        if (charAttacking && charAtBattlePos)
        {

            enemyGroup.moveToPosition = false;
            Vector3 targetPos = Vector3.MoveTowards(character.GetComponent<Rigidbody2D>().transform.position, targetCharacter.GetComponent<Rigidbody2D>().transform.position, 6 * Time.deltaTime);
            character.GetComponent<Rigidbody2D>().MovePosition(targetPos);

            if (Vector3.Distance(character.transform.position, targetCharacter.GetComponent<Rigidbody2D>().transform.position) < 1)
            {

                charAttacking = false;
                charAtBattlePos = false;

                isDead = Engine.e.TakeDamage(target, damageTotal, Engine.e.activeParty.activeParty[index].GetComponent<Character>().hitChance);

                GameObject dmgPopup = Instantiate(damagePopup, character.transform.position, Quaternion.identity);

                if (dodgedAttack == true)
                {
                    dmgPopup.transform.GetChild(0).GetComponent<TextMeshPro>().text = "Dodged";
                    dodgedAttack = false;

                }
                else
                {
                    targetCharacter.GetComponent<SpriteRenderer>().material = damageFlash;
                    dmgPopup.transform.GetChild(0).GetComponent<TextMeshPro>().text = damageTotal.ToString();

                    yield return new WaitForSeconds(0.2f);
                    targetCharacter.GetComponent<SpriteRenderer>().material = originalMaterial;
                }

                Destroy(dmgPopup, 0.8f);
            }
        }

        targetCheck = false;
        confuseAttack = false;

        if (!charAttacking && !charAtBattlePos)
        {
            enemyGroup.moveToPosition = true;

            if (index == 0)
            {
                if (character.transform.position == leaderPos)
                {
                    charAtBattlePos = true;
                }
            }
            if (index == 1)
            {
                if (character.transform.position == activeParty2Pos)
                {
                    charAtBattlePos = true;
                }
            }
            if (index == 2)
            {
                if (character.transform.position == activeParty3Pos)
                {
                    charAtBattlePos = true;
                }
            }

            if (charAtBattlePos && !charAttacking)
            {
                EndTurn();

            }

            yield return new WaitForSeconds(1f);

            // StartCoroutine(CheckNext());
            if (!Engine.e.battleModeActive)
            {
                state = BattleState.ATBCHECK;
            }
        }
    }


    void CharRandomDropChoice()
    {

        int index = 0;

        if (currentInQueue == BattleState.CHAR1TURN || currentInQueue == BattleState.CONFCHAR1)
        {
            index = 0;
        }
        if (currentInQueue == BattleState.CHAR2TURN || currentInQueue == BattleState.CONFCHAR2)
        {
            index = 1;
        }
        if (currentInQueue == BattleState.CHAR3TURN || currentInQueue == BattleState.CONFCHAR3)
        {
            index = 2;
        }

        checkingForRandomDrop = false;
        lastDropChoice = null;

        charRandomDropList = new List<Drops>();

        for (int i = 0; i < activeParty.activeParty[index].GetComponent<Character>().drops.Length; i++)
        {
            if (activeParty.activeParty[index].GetComponent<Character>().drops[i] != null)
            {
                charRandomDropList.Add(activeParty.activeParty[index].GetComponent<Character>().drops[i]);
            }
        }

        int randomDropIndex = Random.Range(0, charRandomDropList.Count);

        lastDropChoice = charRandomDropList[randomDropIndex];
    }

    IEnumerator EnemyAttack()
    {
        int index = 0;
        int randTarget = 0;

        if (currentInQueue == BattleState.ENEMY1TURN)
        {
            index = 0;
            enemy1AttackTarget = Engine.e.GetRandomRemainingPartyMember();
            randTarget = enemy1AttackTarget;
        }
        if (currentInQueue == BattleState.ENEMY2TURN)
        {
            index = 1;
            enemy2AttackTarget = Engine.e.GetRandomRemainingPartyMember();
            randTarget = enemy2AttackTarget;
        }
        if (currentInQueue == BattleState.ENEMY3TURN)
        {
            index = 2;
            enemy3AttackTarget = Engine.e.GetRandomRemainingPartyMember();
            randTarget = enemy3AttackTarget;
        }
        if (currentInQueue == BattleState.ENEMY4TURN)
        {
            index = 3;
            enemy4AttackTarget = Engine.e.GetRandomRemainingPartyMember();
            randTarget = enemy4AttackTarget;
        }


        previousTargetReferenceEnemy = randTarget;
        Debug.Log(randTarget);

        yield return new WaitForSeconds(1f);


        if (enemies[index].GetComponent<Enemy>().drops[0] != null)
        {
            int choiceAttack = Random.Range(0, 100);

            if (enemies[index].GetComponent<Enemy>().choiceAttack < choiceAttack)
            {
                enemyMoving = true;
                enemyAttacking = true;
                Engine.e.PhysicalDamageCalculation(randTarget, enemies[index].gameObject.GetComponent<Enemy>().damage);

            }
            else
            {
                enemyDropChoice = Random.Range(0, enemies[index].GetComponent<Enemy>().drops.Length);

                if (enemies[index].GetComponent<Enemy>().mana >= enemies[index].GetComponent<Enemy>().drops[enemyDropChoice].dropCost)
                {
                    enemyAttackDrop = true;

                    lastDropChoice = enemies[index].gameObject.GetComponent<Enemy>().drops[enemyDropChoice];
                    Engine.e.InstantiateEnemyDropEnemy(index, enemyDropChoice);

                    isDead = Engine.e.TakeElementalDamage(randTarget, enemies[index].gameObject.GetComponent<Enemy>().drops[enemyDropChoice].dropPower, enemies[index].gameObject.GetComponent<Enemy>().drops[enemyDropChoice].dropType);
                    enemies[index].GetComponent<Enemy>().mana -= enemies[index].GetComponent<Enemy>().drops[enemyDropChoice].dropCost;

                    enemyAttacking = false;

                }
                else
                {
                    enemyMoving = true;
                    enemyAttacking = true;
                    Engine.e.PhysicalDamageCalculation(randTarget, enemies[index].gameObject.GetComponent<Enemy>().damage);
                }
            }
        }
        else
        {
            enemyMoving = true;
            enemyAttacking = true;
            Engine.e.PhysicalDamageCalculation(randTarget, enemies[index].gameObject.GetComponent<Enemy>().damage);
        }
        partyCheckNext = false;
    }

    IEnumerator AttackMoveToTargetEnemy()
    {
        int index = 0;
        int enemyTarget = 0;
        GameObject character = null;

        if (currentInQueue == BattleState.ENEMY1TURN)
        {
            index = 0;
            enemyTarget = enemy1AttackTarget;
        }
        if (currentInQueue == BattleState.ENEMY2TURN)
        {
            index = 1;
            enemyTarget = enemy2AttackTarget;

        }
        if (currentInQueue == BattleState.ENEMY3TURN)
        {
            index = 2;
            enemyTarget = enemy3AttackTarget;

        }
        if (currentInQueue == BattleState.ENEMY4TURN)
        {
            index = 3;
            enemyTarget = enemy4AttackTarget;

        }

        if (enemyTarget == 0)
        {
            character = Engine.e.activeParty.gameObject;
        }

        if (enemyTarget == 1)
        {
            character = Engine.e.activePartyMember2.gameObject;
        }

        if (enemyTarget == 2)
        {
            character = Engine.e.activePartyMember3.gameObject;
        }

        if (enemyAttacking && enemyAtBattlePos)
        {
            Vector3 targetPos = Vector3.MoveTowards(enemies[index].gameObject.transform.position, character.transform.position, 8 * Time.deltaTime);

            enemies[index].GetComponent<Rigidbody2D>().MovePosition(targetPos);

            if (Vector3.Distance(enemies[index].transform.position, character.transform.position) < 1)
            {
                enemyAttacking = false;
                enemyAtBattlePos = false;


                isDead = Engine.e.TakeDamage(enemyTarget, damageTotal, enemies[index].GetComponent<Enemy>().hitChance);
                GameObject dmgPopup = Instantiate(damagePopup, character.transform.position, Quaternion.identity);

                if (dodgedAttack)
                {
                    dmgPopup.transform.GetChild(0).GetComponent<TextMeshPro>().text = "Dodged";
                    dodgedAttack = false;
                }
                else
                {
                    dmgPopup.transform.GetChild(0).GetComponent<TextMeshPro>().text = damageTotal.ToString();
                    character.GetComponent<SpriteRenderer>().material = damageFlash;
                    yield return new WaitForSeconds(0.2f);
                    character.GetComponent<SpriteRenderer>().material = originalMaterial;
                }
                Destroy(dmgPopup, 0.8f);
            }
        }

        if (!enemyAttacking && !enemyAtBattlePos)
        {
            Vector3 returnPos = Vector3.MoveTowards(enemies[index].gameObject.transform.position, enemies[index].GetComponent<Enemy>().currentBattlePos, 6 * Time.deltaTime);

            enemies[index].GetComponent<Rigidbody2D>().MovePosition(returnPos);

            if (enemies[index].gameObject.transform.position == enemies[index].GetComponent<Enemy>().currentBattlePos)
            {
                enemyAtBattlePos = true;
            }

            if (enemyAtBattlePos && !enemyAttacking)
            {
                enemyMoving = false;

                EndTurn();

                yield return new WaitForSeconds(0.3f);

                if (isDead)
                {
                    state = BattleState.LOST;
                    yield return new WaitForSeconds(2f);
                    StartCoroutine(EndBattle());
                }
                else

                //StartCoroutine(CheckNext());
                if (!Engine.e.battleModeActive)
                {
                    state = BattleState.ATBCHECK;
                }
            }
        }
    }



    IEnumerator ConfuseAttackMoveToTargetEnemy()
    {
        int index = 0;
        int randTarget = 0;

        if (currentInQueue == BattleState.ENEMY1TURN)
        {
            index = 0;
            randTarget = enemy1AttackTarget;
        }
        if (currentInQueue == BattleState.ENEMY2TURN)
        {
            index = 1;
            randTarget = enemy2AttackTarget;

        }
        if (currentInQueue == BattleState.ENEMY3TURN)
        {
            index = 2;
            randTarget = enemy3AttackTarget;

        }
        if (currentInQueue == BattleState.ENEMY4TURN)
        {
            index = 3;
            randTarget = enemy4AttackTarget;

        }


        if (enemyAttacking && enemyAtBattlePos)
        {
            switch (index)
            {
                case 0:
                    enemy1ATB = 0;
                    enemy1ATBGuage.value = enemy1ATB;
                    enemy1Ready = false;
                    break;
                case 1:
                    enemy2ATB = 0;
                    enemy2ATBGuage.value = enemy2ATB;
                    enemy2Ready = false;
                    break;
                case 2:
                    enemy3ATB = 0;
                    enemy3ATBGuage.value = enemy3ATB;
                    enemy3Ready = false;
                    break;
                case 3:
                    enemy4ATB = 0;
                    enemy4ATBGuage.value = enemy4ATB;
                    enemy4Ready = false;
                    break;
            }

            Vector3 targetPos = Vector3.MoveTowards(enemies[index].gameObject.transform.position, enemies[randTarget].transform.position, 6 * Time.deltaTime);

            enemies[index].GetComponent<Rigidbody2D>().MovePosition(targetPos);

            if (Vector3.Distance(enemies[index].transform.position, enemies[randTarget].transform.position) < 1)
            {
                enemyAttacking = false;
                enemyAtBattlePos = false;

                enemies[randTarget].GetComponent<Enemy>().TakePhysicalDamage(randTarget, damageTotal, enemies[index].GetComponent<Enemy>().hitChance);
                GameObject dmgPopup = Instantiate(damagePopup, enemies[randTarget].transform.position, Quaternion.identity);
                if (dodgedAttack)
                {
                    dmgPopup.transform.GetChild(0).GetComponent<TextMeshPro>().text = "Dodged";
                    dodgedAttack = false;
                }
                else
                {
                    dmgPopup.transform.GetChild(0).GetComponent<TextMeshPro>().text = damageTotal.ToString();
                    enemies[randTarget].GetComponent<SpriteRenderer>().material = damageFlash;
                    yield return new WaitForSeconds(0.2f);
                    enemies[randTarget].GetComponent<SpriteRenderer>().material = originalMaterial;
                }
                Destroy(dmgPopup, 0.8f);
            }
        }

        if (!enemyAttacking && !enemyAtBattlePos)
        {
            Vector3 returnPos = Vector3.MoveTowards(enemies[index].gameObject.transform.position, enemies[index].GetComponent<Enemy>().currentBattlePos, 6 * Time.deltaTime);

            enemies[index].GetComponent<Rigidbody2D>().MovePosition(returnPos);

            if (enemies[index].gameObject.transform.position == enemies[index].GetComponent<Enemy>().currentBattlePos)
            {
                enemyAtBattlePos = true;
            }

            if (enemyAtBattlePos && !enemyAttacking)
            {
                EndTurn();

                yield return new WaitForSeconds(0.3f);

                if (isDead)
                {
                    state = BattleState.LOST;
                    yield return new WaitForSeconds(2f);
                    StartCoroutine(EndBattle());
                }
                else

                    // StartCoroutine(CheckNext());
                    state = BattleState.ATBCHECK;

            }
        }
    }


    IEnumerator EnemyDropAttack()
    {

        if (!animExists)
        {
            enemyAttackDrop = false;
            // currentInQueue = nextInQueue;

        }
        else
        {
            if (state == currentInQueue)
                state = BattleState.ATBCHECK;
        }

        if (!animExists && !enemyAttackDrop)
        {
            EndTurn();
            yield return new WaitForSeconds(0.3f);

            if (isDead)
            {
                state = BattleState.LOST;
                yield return new WaitForSeconds(2f);
                StartCoroutine(EndBattle());

            }
            else
            {
                // StartCoroutine(CheckNext());
                if (!Engine.e.battleModeActive)
                    state = BattleState.ATBCHECK;

            }
        }
    }

    IEnumerator EnemyConfuseAttack()
    {
        int index = 0;
        int randTarget = 0;

        if (currentInQueue == BattleState.ENEMY1TURN)
        {
            index = 0;
            randTarget = enemy1AttackTarget;
        }
        if (currentInQueue == BattleState.ENEMY2TURN)
        {
            index = 1;
            randTarget = enemy2AttackTarget;

        }
        if (currentInQueue == BattleState.ENEMY3TURN)
        {
            index = 2;
            randTarget = enemy3AttackTarget;

        }
        if (currentInQueue == BattleState.ENEMY4TURN)
        {
            index = 3;
            randTarget = enemy4AttackTarget;

        }

        yield return new WaitForSeconds(0.3f);

        //  if (enemies[index].GetComponent<Enemy>().isPoisoned)
        //  {
        //      enemies[index].GetComponent<Enemy>().TakePoisonDamage(index, enemies[index].GetComponent<Enemy>().poisonDmg);
        //  }  

        if (enemies[index].GetComponent<Enemy>().drops[0] != null)
        {
            int choiceAttack = Random.Range(0, 100);

            if (enemies[index].GetComponent<Enemy>().choiceAttack < choiceAttack)
            {
                enemyMoving = true;
                enemyAttacking = true;

                if (!attackingTeam)
                {

                    Engine.e.PhysicalDamageCalculation(randTarget, enemies[index].gameObject.GetComponent<Enemy>().damage);
                }
                else
                {
                    if (enemies[randTarget] != null)
                    {

                        enemies[randTarget].GetComponent<Enemy>().TakePhysicalDamage(randTarget, enemies[index].GetComponent<Enemy>().damageTotal, enemies[index].GetComponent<Enemy>().hitChance);
                    }
                }
            }
            else
            {
                enemyDropChoice = Random.Range(0, enemies[index].GetComponent<Enemy>().drops.Length);

                if (enemies[index].GetComponent<Enemy>().mana >= enemies[index].GetComponent<Enemy>().drops[enemyDropChoice].dropCost)
                {
                    physicalAttack = false;
                    confuseAttack = false;

                    enemyAttackDrop = true;

                    lastDropChoice = enemies[index].gameObject.GetComponent<Enemy>().drops[enemyDropChoice];

                    if (!attackingTeam)
                    {
                        Engine.e.InstantiateEnemyDropEnemy(index, enemyDropChoice);
                        isDead = Engine.e.TakeElementalDamage(randTarget, enemies[index].gameObject.GetComponent<Enemy>().drops[enemyDropChoice].dropPower, enemies[index].gameObject.GetComponent<Enemy>().drops[enemyDropChoice].dropType);
                    }
                    else
                    {

                        if (enemies[index].gameObject.GetComponent<Enemy>().drops[enemyDropChoice].dropType == "Fire")
                        {
                            Instantiate(fireDropAnim, enemies[index].transform.position, Quaternion.identity);
                        }
                        if (enemies[index].gameObject.GetComponent<Enemy>().drops[enemyDropChoice].dropType == "Water")
                        {
                            Instantiate(waterDropAnim, enemies[index].transform.position, Quaternion.identity);
                        }
                        if (enemies[index].gameObject.GetComponent<Enemy>().drops[enemyDropChoice].dropType == "Lightning")
                        {
                            Instantiate(lightningDropAnim, enemies[index].transform.position, Quaternion.identity);
                        }
                        if (enemies[index].gameObject.GetComponent<Enemy>().drops[enemyDropChoice].dropType == "Shadow")
                        {
                            if (enemies[index].gameObject.GetComponent<Enemy>().drops[enemyDropChoice].dropName == "Bio" || enemies[index].gameObject.GetComponent<Enemy>().drops[enemyDropChoice].dropName == "Knockout"
                            || enemies[index].gameObject.GetComponent<Enemy>().drops[enemyDropChoice].dropName == "Blind")
                            {
                                if (enemies[index].gameObject.GetComponent<Enemy>().drops[enemyDropChoice].dropName == "Bio")
                                {
                                    Instantiate(poisonAnim, enemies[index].transform.position, Quaternion.identity);
                                }
                                if (enemies[index].gameObject.GetComponent<Enemy>().drops[enemyDropChoice].dropName == "Knockout")
                                {
                                    Instantiate(sleepAnim, enemies[index].transform.position, Quaternion.identity);
                                }
                                if (enemies[index].gameObject.GetComponent<Enemy>().drops[enemyDropChoice].dropName == "Blind")
                                {
                                    Instantiate(confuseAnim, enemies[index].transform.position, Quaternion.identity);
                                }
                            }
                            else
                                Instantiate(shadowDropAnim, enemies[index].transform.position, Quaternion.identity);
                        }
                        if (enemies[index].gameObject.GetComponent<Enemy>().drops[enemyDropChoice].dropType == "Ice")
                        {
                            Instantiate(iceDropAnim, enemies[index].transform.position, Quaternion.identity);
                        }


                        enemies[randTarget].GetComponent<Enemy>().ConfuseTakeDropDamage(randTarget, enemies[index].GetComponent<Enemy>().drops[enemyDropChoice]);


                        if (currentInQueue == BattleState.ENEMY1TURN)
                        {
                            enemy1ATB = 0;
                        }
                        if (currentInQueue == BattleState.ENEMY2TURN)
                        {
                            enemy2ATB = 0;
                        }
                        if (currentInQueue == BattleState.ENEMY3TURN)
                        {
                            enemy3ATB = 0;
                        }
                        if (currentInQueue == BattleState.ENEMY4TURN)
                        {
                            enemy4ATB = 0;
                        }

                        enemies[index].GetComponent<Enemy>().mana -= enemies[index].GetComponent<Enemy>().drops[enemyDropChoice].dropCost;

                        if (enemies[index].GetComponent<EnemyMovement>() != null)
                        {
                            enemies[index].GetComponent<EnemyMovement>().enabled = true;
                        }
                    }
                }
                else
                {
                    enemyMoving = true;
                    enemyAttacking = true;
                    if (teamTargetChoice == 0)
                    {
                        Engine.e.PhysicalDamageCalculation(randTarget, enemies[index].gameObject.GetComponent<Enemy>().damage);
                    }
                    else
                    {
                        enemies[randTarget].GetComponent<Enemy>().TakePhysicalDamage(randTarget, enemies[index].GetComponent<Enemy>().damageTotal, enemies[index].GetComponent<Enemy>().hitChance);
                    }
                }
            }
        }
        else
        {
            enemyMoving = true;
            enemyAttacking = true;
            if (!attackingTeam)
            {
                Engine.e.PhysicalDamageCalculation(randTarget, enemies[index].gameObject.GetComponent<Enemy>().damage);
            }
            else
            {
                enemies[randTarget].GetComponent<Enemy>().TakePhysicalDamage(randTarget, enemies[index].GetComponent<Enemy>().damageTotal, enemies[index].GetComponent<Enemy>().hitChance);
            }
        }
    }

    IEnumerator LevelUpCheck()
    {
        if (state == BattleState.LEVELUPCHECK)
        {
            DeactivateEnemyUI();
            //DeactivateChar1MenuButtons();
            DeactivateDropsUI();
            DeactivateSkillsUI();
            DeactivateChar1SwitchButtons();
            ResetPartyMemberStats();

            if (activeParty.activeParty[1] != null)
            {
                //DeactivateChar2MenuButtons();
                DeactivateDropsUI();
                DeactivateSkillsUI();
                DeactivateChar2SwitchButtons();
            }

            if (activeParty.activeParty[2] != null)
            {
                //DeactivateChar3MenuButtons();
                DeactivateDropsUI();
                DeactivateSkillsUI();
                DeactivateChar3SwitchButtons();
            }

            Engine.e.battleSystem.enemyPanel.SetActive(false);
            Engine.e.adventureLogReference.QuestManager(enemies);


            yield return new WaitForSeconds(1f);
            enemyGroup.GroupItemDrops();

            Engine.e.battleSystem.enemyLootPanelReference.SetActive(true);
            Engine.e.GiveExperiencePoints(enemyGroupExperiencePoints);

            if (Engine.e.char1LevelUp == true || Engine.e.char2LevelUp == true || Engine.e.char3LevelUp == true)
            {
                {
                    yield return new WaitForSeconds(0.1f);

                    state = BattleState.LEVELUP;
                    StartCoroutine(LevelUp());
                }
            }
            else
            {
                yield return new WaitForSeconds(3f);

                state = BattleState.WON;
                StartCoroutine(EndBattle());

                if (activeParty.activeParty[0].GetComponent<Character>().currentHealth <= 0)
                {
                    activeParty.activeParty[0].GetComponent<Character>().currentHealth = 1;
                }
                if (activeParty.activeParty[1] != null && activeParty.activeParty[1].GetComponent<Character>().currentHealth <= 0)
                {
                    activeParty.activeParty[1].GetComponent<Character>().currentHealth = 1;
                }
                if (activeParty.activeParty[2] != null && activeParty.activeParty[2].GetComponent<Character>().currentHealth <= 0)
                {
                    activeParty.activeParty[2].GetComponent<Character>().currentHealth = 1;
                }
            }
        }
    }

    IEnumerator EndBattle()
    {
        if (state == BattleState.WON)
        {
            Engine.e.activeParty.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            enemyGroup.battleCamera.gameObject.SetActive(false);
            Engine.e.char1LevelUp = false;
            Engine.e.char2LevelUp = false;
            Engine.e.char3LevelUp = false;

            enemyGroup.DestroyGroup();

            isDead = false;

            dialogueText.text = string.Empty;
            dialogueText.text = "Victory.";

            Engine.e.battleMenu.battleMenuUI.SetActive(false);
            Engine.e.battleSystem.enemyLootPanelReference.SetActive(false);
            Engine.e.battleSystem.enemyPanel.SetActive(true);
            Engine.e.battleSystem.enemyLootReference.GetComponent<TMP_Text>().text = string.Empty;
            Engine.e.inBattle = false;
            Engine.e.storeDialogueReference.gameObject.SetActive(true);

            for (int i = 0; i < battleItems.Length; i++)
            {
                battleItems[i].item = null;
                battleItems[i].itemName.GetComponent<TMP_Text>().text = string.Empty;
                battleItems[i].itemCount.GetComponent<TMP_Text>().text = string.Empty;

            }

            if (Engine.e.activeParty.activeParty[1] != null)
            {
                char2UI.SetActive(true);
                charIndexName[1] = activeParty.activeParty[1].gameObject.GetComponent<Character>().characterName;
            }
            if (Engine.e.activeParty.activeParty[2] != null)
            {
                char3UI.SetActive(true);
                charIndexName[2] = activeParty.activeParty[2].gameObject.GetComponent<Character>().characterName;
            }
            if (enemyGroup.battleCamera != null)
            {
                Engine.e.mainCamera.GetComponent<CinemachineVirtualCamera>().Priority = 10;

                yield return new WaitForSeconds(2f);

            }

            //state = BattleState.START;
            yield return new WaitForSeconds(0.1f);
        }
        else
        {
            if (state == BattleState.LOST)
            {
                dialogueText.text = string.Empty;
                yield return new WaitForSeconds(2f);
                dialogueText.text = "Game Over.";
                yield return new WaitForSeconds(5f);
                SceneManager.UnloadSceneAsync(Engine.e.currentScene);
                Engine.e.inBattle = false;
                Engine.e.battleSystemMenu.SetActive(false);
                Engine.e.mainMenu.SetActive(true);

            }
        }
        //       GameManager.gameManager.battleMusic.Stop();
    }

    IEnumerator LevelUp()
    {
        if (state == BattleState.LEVELUP)
        {
            if (Engine.e.char1LevelUp == true)
            {
                //char1BattlePanel.SetActive(false);
                char1LevelUpPanel.SetActive(true);
            }
            if (Engine.e.char2LevelUp == true)
            {
                // char1BattlePanel.SetActive(false);
                char2LevelUpPanel.SetActive(true);
            }
            if (Engine.e.char3LevelUp == true)
            {
                // char1BattlePanel.SetActive(false);
                char3LevelUpPanel.SetActive(true);
            }
        }

        dialogueText.text = string.Empty;

        yield return new WaitForSeconds(7f);

        char1LevelUpPanel.SetActive(false);
        char2LevelUpPanel.SetActive(false);
        char3LevelUpPanel.SetActive(false);

        char1BattlePanel.SetActive(true);

        if (activeParty.activeParty[1] != null)
        {
            char2BattlePanel.SetActive(true);
        }
        if (activeParty.activeParty[2] != null)
        {
            char3BattlePanel.SetActive(true);
        }

        state = BattleState.WON;
        StartCoroutine(EndBattle());
    }

    public void Char1Turn()
    {

        state = BattleState.CHAR1TURN;


        //    dialogueText.text = Engine.e.activeParty.activeParty[0].gameObject.GetComponent<Character>().characterName;


        ActivateChar1MenuButtons();
        DeactivateDropsUI();
        ActivateCharDropsUI();
        DeactivateSkillsUI();
        ActivateCharSkillsUI();
        EnableButtonInteraction();

        inBattleMenu = false;
        battleItemMenu.SetActive(false);
        physicalAttack = false;
        dropAttack = false;
        skillPhysicalAttack = false;
        skillRangedAttack = false;
        skillTargetSupport = false;
        skillSelfSupport = false;
        usingItem = false;
        failedItemUse = false;
        attackingTeam = false;
        partyCheckNext = false;

        char1Attacking = false;
        char1DropAttack = false;
        char1SkillAttack = false;
        char1SkillPhysicalAttack = false;
        char1SkillRangedAttack = false;
        char1Supporting = false;
        char1UsingItem = false;

        char1SkillChoice = null; // Could be used for "quick last skill choice"

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(char1MenuButtons[0]);

    }

    public void Char1ConfusedTurn()
    {
        char1Ready = false;

        //dialogueText.text = Engine.e.activeParty.activeParty[0].gameObject.GetComponent<Character>().characterName;
        confuseTargetCheck = true;
        confuseAttack = true;
        StartCoroutine(ConfuseTargetFinder());

    }

    public void Char2Turn()
    {
        if (failedItemUse)
        {
            char2ATB = ATBReady;
            failedItemUse = false;
        }


        state = BattleState.CHAR2TURN;


        //   dialogueText.text = Engine.e.activeParty.activeParty[1].gameObject.GetComponent<Character>().characterName;


        ActivateChar2MenuButtons();
        DeactivateDropsUI();
        ActivateCharDropsUI();
        DeactivateSkillsUI();
        ActivateCharSkillsUI();
        EnableButtonInteraction();

        inBattleMenu = false;
        battleItemMenu.SetActive(false);
        physicalAttack = false;
        dropAttack = false;
        skillPhysicalAttack = false;
        skillRangedAttack = false;
        skillTargetSupport = false;
        skillSelfSupport = false;
        usingItem = false;
        failedItemUse = false;
        attackingTeam = false;
        partyCheckNext = false;

        char2Attacking = false;
        char2DropAttack = false;
        char2SkillAttack = false;
        char2SkillPhysicalAttack = false;
        char2SkillRangedAttack = false;
        char2Supporting = false;
        char2UsingItem = false;

        char2SkillChoice = null; // Could be used for "quick last skill choice"

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(char2MenuButtons[0]);

    }
    public void Char2ConfusedTurn()
    {
        char2Ready = false;


        //dialogueText.text = Engine.e.activeParty.activeParty[1].gameObject.GetComponent<Character>().characterName;

        confuseTargetCheck = true;
        confuseAttack = true;
        StartCoroutine(ConfuseTargetFinder());

    }

    public void Char3Turn()
    {
        if (failedItemUse)
        {
            char3ATB = ATBReady;
            failedItemUse = false;
        }

        state = BattleState.CHAR3TURN;


        // dialogueText.text = Engine.e.activeParty.activeParty[2].gameObject.GetComponent<Character>().characterName;


        ActivateChar3MenuButtons();
        DeactivateDropsUI();
        ActivateCharDropsUI();
        DeactivateSkillsUI();
        ActivateCharSkillsUI();
        EnableButtonInteraction();

        inBattleMenu = false;
        battleItemMenu.SetActive(false);
        physicalAttack = false;
        dropAttack = false;
        skillPhysicalAttack = false;
        skillRangedAttack = false;
        skillTargetSupport = false;
        skillSelfSupport = false;
        usingItem = false;
        failedItemUse = false;
        attackingTeam = false;
        partyCheckNext = false;

        char3Attacking = false;
        char3DropAttack = false;
        char3SkillAttack = false;
        char3SkillPhysicalAttack = false;
        char3SkillRangedAttack = false;
        char3Supporting = false;
        char3UsingItem = false;

        char3SkillChoice = null; // Could be used for "quick last skill choice"

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(char3MenuButtons[0]);

    }

    public void Char3ConfusedTurn()
    {
        char3Ready = false;


        //dialogueText.text = Engine.e.activeParty.activeParty[2].gameObject.GetComponent<Character>().characterName;

        confuseTargetCheck = true;
        confuseAttack = true;
        StartCoroutine(ConfuseTargetFinder());

    }
    void Enemy1Turn()
    {
        enemy1Ready = false;
        if (enemies[0].GetComponent<EnemyMovement>() != null)
        {
            enemies[0].GetComponent<EnemyMovement>().enabled = false;
        }

        //DisableButtonInteraction();
        partyCheckNext = false;
        //dialogueText.text = string.Empty;
        // dialogueText.text = enemies[0].GetComponent<Enemy>().enemyName;

        if (!enemies[0].GetComponent<Enemy>().isConfused)
        {
            StartCoroutine(EnemyAttack());
        }
        else
        {
            confuseTargetCheck = true;
            confuseAttack = true;
            StartCoroutine(ConfuseTargetFinder());
        }
    }

    void Enemy2Turn()
    {
        enemy2Ready = false;

        if (enemies[1].GetComponent<EnemyMovement>() != null)
        {
            enemies[1].GetComponent<EnemyMovement>().enabled = false;
        }

        //dialogueText.text = string.Empty;
        // dialogueText.text = enemies[1].GetComponent<Enemy>().enemyName;
        partyCheckNext = false;

        if (!enemies[1].GetComponent<Enemy>().isConfused)
        {
            StartCoroutine(EnemyAttack());
        }
        else
        {
            confuseTargetCheck = true;
            confuseAttack = true;
            StartCoroutine(ConfuseTargetFinder());
        }
    }

    void Enemy3Turn()
    {
        enemy3Ready = false;

        if (enemies[2].GetComponent<EnemyMovement>() != null)
        {
            enemies[2].GetComponent<EnemyMovement>().enabled = false;
        }

        //  dialogueText.text = string.Empty;
        // dialogueText.text = enemies[2].GetComponent<Enemy>().enemyName;
        partyCheckNext = false;

        if (!enemies[2].GetComponent<Enemy>().isConfused)
        {
            StartCoroutine(EnemyAttack());
        }
        else
        {
            confuseTargetCheck = true;
            confuseAttack = true;
            StartCoroutine(ConfuseTargetFinder());
        }
    }

    void Enemy4Turn()
    {
        enemy4Ready = false;

        if (enemies[3].GetComponent<EnemyMovement>() != null)
        {
            enemies[3].GetComponent<EnemyMovement>().enabled = false;
        }

        // dialogueText.text = string.Empty;
        // dialogueText.text = enemies[3].GetComponent<Enemy>().enemyName;
        if (!enemies[3].GetComponent<Enemy>().isConfused)
        {
            StartCoroutine(EnemyAttack());
        }
        else
        {
            confuseTargetCheck = true;
            confuseAttack = true;
            StartCoroutine(ConfuseTargetFinder());
        }

    }

    IEnumerator ConfuseTargetFinder()
    {
        if (confuseTargetCheck)
        {
            Debug.Log("entered ConfuseTargetFinder");
            confuseTargetCheck = false;
            yield return new WaitForSeconds(0.3f);

            teamTargetChoice = Random.Range(0, 2);
            int index = 0;
            bool partyAttacking = false;
            int randTarget = 0;

            //if (state == BattleState.CHAR1TURN)
            //{
            if (currentInQueue == BattleState.CONFCHAR1)
            {
                index = 0;
                partyAttacking = true;
            }
            if (currentInQueue == BattleState.CONFCHAR2)
            {
                index = 1;
                partyAttacking = true;

            }
            if (currentInQueue == BattleState.CONFCHAR3)
            {
                index = 2;
                partyAttacking = true;

            }

            if (currentInQueue == BattleState.ENEMY1TURN)
            {
                index = 0;
            }
            if (currentInQueue == BattleState.ENEMY2TURN)
            {
                index = 1;
            }
            if (currentInQueue == BattleState.ENEMY3TURN)
            {
                index = 2;
            }
            if (currentInQueue == BattleState.ENEMY4TURN)
            {
                index = 3;
            }

            if (partyAttacking) // Team is attacking
            {
                if (teamTargetChoice == 0) // Team is attacking themselves
                {
                    if (currentInQueue == BattleState.CONFCHAR1)
                    {
                        char1Target = Engine.e.GetRandomRemainingPartyMember();
                        randTarget = char1Target;
                    }
                    if (currentInQueue == BattleState.CONFCHAR2)
                    {
                        char2Target = Engine.e.GetRandomRemainingPartyMember();
                        randTarget = char2Target;
                    }
                    if (currentInQueue == BattleState.CONFCHAR3)
                    {
                        char3Target = Engine.e.GetRandomRemainingPartyMember();
                        randTarget = char3Target;
                    }

                    Engine.e.PhysicalDamageCalculation(randTarget, Engine.e.activeParty.activeParty[index].GetComponent<Character>().physicalDamage);
                    attackingTeam = true;

                }
                else
                {
                    if (currentInQueue == BattleState.CONFCHAR1)
                    {
                        char1Target = enemyGroup.GetRandomRemainingEnemy();
                        randTarget = char1Target;
                    }
                    if (currentInQueue == BattleState.CONFCHAR2)
                    {
                        char2Target = enemyGroup.GetRandomRemainingEnemy();
                        randTarget = char2Target;
                    }
                    if (currentInQueue == BattleState.CONFCHAR3)
                    {
                        char3Target = enemyGroup.GetRandomRemainingEnemy();
                        randTarget = char3Target;
                    }
                }

                Debug.Log("Team Choice: " + teamTargetChoice + " Index: " + randTarget);
                StartCoroutine(CharConfuseAttack());
            }   // Enemy is attacking
            else
            {

                if (teamTargetChoice == 0) // Enemy is attacking Team
                {
                    if (currentInQueue == BattleState.ENEMY1TURN)
                    {
                        enemy1AttackTarget = Engine.e.GetRandomRemainingPartyMember();
                        randTarget = enemy1AttackTarget;
                    }
                    if (currentInQueue == BattleState.ENEMY2TURN)
                    {
                        enemy2AttackTarget = Engine.e.GetRandomRemainingPartyMember();
                        randTarget = enemy2AttackTarget;

                    }
                    if (currentInQueue == BattleState.ENEMY3TURN)
                    {
                        enemy3AttackTarget = Engine.e.GetRandomRemainingPartyMember();
                        randTarget = enemy3AttackTarget;

                    }
                    if (currentInQueue == BattleState.ENEMY4TURN)
                    {
                        enemy4AttackTarget = Engine.e.GetRandomRemainingPartyMember();
                        randTarget = enemy4AttackTarget;
                    }

                    Engine.e.PhysicalDamageCalculation(randTarget, enemies[index].GetComponent<Enemy>().damage);
                }   // Enemy is attacking themeselves
                else
                {
                    if (currentInQueue == BattleState.ENEMY1TURN)
                    {
                        enemy1AttackTarget = enemyGroup.GetRandomRemainingEnemy();
                        randTarget = enemy1AttackTarget;
                    }
                    if (currentInQueue == BattleState.ENEMY2TURN)
                    {
                        enemy2AttackTarget = enemyGroup.GetRandomRemainingEnemy();
                        randTarget = enemy2AttackTarget;

                    }
                    if (currentInQueue == BattleState.ENEMY3TURN)
                    {
                        enemy3AttackTarget = enemyGroup.GetRandomRemainingEnemy();
                        randTarget = enemy3AttackTarget;

                    }
                    if (currentInQueue == BattleState.ENEMY4TURN)
                    {
                        enemy4AttackTarget = enemyGroup.GetRandomRemainingEnemy();
                        randTarget = enemy4AttackTarget;

                    }
                    attackingTeam = true;

                }
                Debug.Log("Team Choice: " + teamTargetChoice + " Index: " + randTarget);

                StartCoroutine(EnemyConfuseAttack());
            }
        }
    }

    public void TargetTeam()
    {
        if (state == BattleState.CHAR1TURN)
        {
            if (!char1TargetingTeam)
            {
                char1TargetingTeam = true;
                char1TargetingEnemy = false;
            }
        }

        if (state == BattleState.CHAR2TURN)
        {
            if (!char2TargetingTeam)
            {
                char2TargetingTeam = true;
                char2TargetingEnemy = false;
            }
        }

        if (state == BattleState.CHAR3TURN)
        {
            if (!char3TargetingTeam)
            {
                char3TargetingTeam = true;
                char3TargetingEnemy = false;
            }
        }
    }

    public void TargetEnemy()
    {
        if (state == BattleState.CHAR1TURN)
        {
            if (!char1TargetingEnemy)
            {
                char1TargetingEnemy = true;
                char1TargetingTeam = false;
            }
        }

        if (state == BattleState.CHAR2TURN)
        {
            if (!char2TargetingEnemy)
            {
                char2TargetingEnemy = true;
                char2TargetingTeam = false;
            }
        }

        if (state == BattleState.CHAR3TURN)
        {
            if (!char3TargetingEnemy)
            {
                char3TargetingEnemy = true;
                char3TargetingTeam = false;
            }
        }
    }

    public void ClearTarget()
    {
        targetingTeam = false;
        targetingEnemy = false;
    }

    // To keep track of where we are targeting (for UI navigation)
    public void SetMenuTargetIndex(float index)
    {
        menuTargetIndex = index;
    }

    public void SetTarget(int _target)
    {

        if (state == BattleState.CHAR1TURN)
        {
            if (char1PhysicalAttack)
            {
                char1Attacking = true;
            }

            char1Target = _target;
            char1Ready = false;

            DeactivateChar1MenuButtons();
            battleQueue.Enqueue(BattleState.CHAR1TURN);

            if (activeParty.activeParty[1] != null)
            {
                if (char2Ready)
                {
                    ActivateChar2MenuButtons();
                }
            }

            if (activeParty.activeParty[2] != null)
            {
                if (char3Ready)
                {
                    ActivateChar3MenuButtons();
                }
            }

        }

        if (state == BattleState.CHAR2TURN)
        {
            if (char2PhysicalAttack)
            {
                char2Attacking = true;
            }

            char2Target = _target;
            char2Ready = false;

            DeactivateChar2MenuButtons();
            battleQueue.Enqueue(BattleState.CHAR2TURN);


            if (char1Ready)
            {
                ActivateChar1MenuButtons();
            }


            if (activeParty.activeParty[2] != null)
            {
                if (char3Ready)
                {
                    ActivateChar3MenuButtons();
                }
            }
        }

        if (state == BattleState.CHAR3TURN)
        {
            if (char3PhysicalAttack)
            {
                char3Attacking = true;
            }

            char3Target = _target;
            char3Ready = false;

            DeactivateChar3MenuButtons();
            battleQueue.Enqueue(BattleState.CHAR3TURN);

            if (char1Ready)
            {
                ActivateChar1MenuButtons();
            }

            if (char2Ready)
            {
                ActivateChar2MenuButtons();

            }

        }

        DeactivateTargetButtons();
        DeactivateTargetSprite();
        state = BattleState.ATBCHECK;

    }

    public void DisplayAvailableCharBattle()
    {
        if (state == BattleState.CHAR1TURN)
        {
            for (int i = 0; i < availableChar1Buttons.Length; i++)
            {
                availableChar1Buttons[i].SetActive(true);
            }
            if (Engine.e.activeParty.activeParty[0].GetComponent<Character>().characterName == "Grieve" ||
                Engine.e.activeParty.activeParty[1].GetComponent<Character>().characterName == "Grieve" ||
                Engine.e.activeParty.activeParty[2].GetComponent<Character>().characterName == "Grieve")
            {
                availableChar1Buttons[0].SetActive(false);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(GetComponent<BattleMenuControllerNav>().char1AvailSwitchGrieve);
            }

            if (Engine.e.activeParty.activeParty[0].GetComponent<Character>().characterName == "Mac" ||
           Engine.e.activeParty.activeParty[1].GetComponent<Character>().characterName == "Mac" ||
           Engine.e.activeParty.activeParty[2].GetComponent<Character>().characterName == "Mac")
            {
                availableChar1Buttons[1].SetActive(false);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(GetComponent<BattleMenuControllerNav>().char1AvailSwitchMac);
            }

            if (Engine.e.activeParty.activeParty[0].GetComponent<Character>().characterName == "Field" ||
          Engine.e.activeParty.activeParty[1].GetComponent<Character>().characterName == "Field" ||
          Engine.e.activeParty.activeParty[2].GetComponent<Character>().characterName == "Field")
            {
                availableChar1Buttons[2].SetActive(false);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(GetComponent<BattleMenuControllerNav>().char1AvailSwitchField);
            }

            if (Engine.e.activeParty.activeParty[0].GetComponent<Character>().characterName == "Riggs" ||
          Engine.e.activeParty.activeParty[1].GetComponent<Character>().characterName == "Riggs" ||
          Engine.e.activeParty.activeParty[2].GetComponent<Character>().characterName == "Riggs")
            {
                availableChar1Buttons[3].SetActive(false);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(GetComponent<BattleMenuControllerNav>().char1AvailSwitchRiggs);
            }
        }

        if (state == BattleState.CHAR2TURN)
        {
            for (int i = 0; i < availableChar2Buttons.Length; i++)
            {
                availableChar2Buttons[i].SetActive(true);
            }
            if (Engine.e.activeParty.activeParty[0].GetComponent<Character>().characterName == "Grieve" ||
                Engine.e.activeParty.activeParty[1].GetComponent<Character>().characterName == "Grieve" ||
                Engine.e.activeParty.activeParty[2].GetComponent<Character>().characterName == "Grieve")
            {
                availableChar2Buttons[0].SetActive(false);

            }
            else
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(GetComponent<BattleMenuControllerNav>().char2AvailSwitchGrieve);
            }

            if (Engine.e.activeParty.activeParty[0].GetComponent<Character>().characterName == "Mac" ||
           Engine.e.activeParty.activeParty[1].GetComponent<Character>().characterName == "Mac" ||
           Engine.e.activeParty.activeParty[2].GetComponent<Character>().characterName == "Mac")
            {
                availableChar2Buttons[1].SetActive(false);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(GetComponent<BattleMenuControllerNav>().char2AvailSwitchMac);
            }

            if (Engine.e.activeParty.activeParty[0].GetComponent<Character>().characterName == "Field" ||
          Engine.e.activeParty.activeParty[1].GetComponent<Character>().characterName == "Field" ||
          Engine.e.activeParty.activeParty[2].GetComponent<Character>().characterName == "Field")
            {
                availableChar2Buttons[2].SetActive(false);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(GetComponent<BattleMenuControllerNav>().char2AvailSwitchField);
            }

            if (Engine.e.activeParty.activeParty[0].GetComponent<Character>().characterName == "Riggs" ||
          Engine.e.activeParty.activeParty[1].GetComponent<Character>().characterName == "Riggs" ||
          Engine.e.activeParty.activeParty[2].GetComponent<Character>().characterName == "Riggs")
            {
                availableChar2Buttons[3].SetActive(false);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(GetComponent<BattleMenuControllerNav>().char2AvailSwitchRiggs);
            }
        }

        if (state == BattleState.CHAR3TURN)
        {
            for (int i = 0; i < availableChar3Buttons.Length; i++)
            {
                availableChar3Buttons[i].SetActive(true);
            }

            if (Engine.e.activeParty.activeParty[0].GetComponent<Character>().characterName == "Grieve" ||
                Engine.e.activeParty.activeParty[1].GetComponent<Character>().characterName == "Grieve" ||
                Engine.e.activeParty.activeParty[2].GetComponent<Character>().characterName == "Grieve")
            {
                availableChar3Buttons[0].SetActive(false);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(GetComponent<BattleMenuControllerNav>().char3AvailSwitchGrieve);
            }

            if (Engine.e.activeParty.activeParty[0].GetComponent<Character>().characterName == "Mac" ||
           Engine.e.activeParty.activeParty[1].GetComponent<Character>().characterName == "Mac" ||
           Engine.e.activeParty.activeParty[2].GetComponent<Character>().characterName == "Mac")
            {
                availableChar3Buttons[1].SetActive(false);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(GetComponent<BattleMenuControllerNav>().char3AvailSwitchMac);
            }

            if (Engine.e.activeParty.activeParty[0].GetComponent<Character>().characterName == "Field" ||
          Engine.e.activeParty.activeParty[1].GetComponent<Character>().characterName == "Field" ||
          Engine.e.activeParty.activeParty[2].GetComponent<Character>().characterName == "Field")
            {
                availableChar3Buttons[2].SetActive(false);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(GetComponent<BattleMenuControllerNav>().char3AvailSwitchField);
            }

            if (Engine.e.activeParty.activeParty[0].GetComponent<Character>().characterName == "Riggs" ||
          Engine.e.activeParty.activeParty[1].GetComponent<Character>().characterName == "Riggs" ||
          Engine.e.activeParty.activeParty[2].GetComponent<Character>().characterName == "Riggs")
            {
                availableChar3Buttons[3].SetActive(false);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(GetComponent<BattleMenuControllerNav>().char3AvailSwitchRiggs);
            }
        }
    }


    public void SwitchCharButton(int index)
    {

        if (state == BattleState.CHAR1TURN)
        {
            char1Switching = true;
            char1SwitchToIndex = index;
            DeactivateChar1MenuButtons();

            battleQueue.Enqueue(BattleState.CHAR1TURN);

        }
        if (state == BattleState.CHAR2TURN)
        {
            char2Switching = true;
            char2SwitchToIndex = index;
            DeactivateChar2MenuButtons();

            battleQueue.Enqueue(BattleState.CHAR2TURN);

        }
        if (state == BattleState.CHAR3TURN)
        {
            char3Switching = true;
            char3SwitchToIndex = index;
            DeactivateChar3MenuButtons();

            battleQueue.Enqueue(BattleState.CHAR3TURN);

        }

        state = BattleState.ATBCHECK;
    }

    public void ActivateChar1MenuButtons()
    {
        if (!isDead)
        {
            if (!char1MenuButtonsActive)
            {
                for (int i = 0; i < char1MenuButtons.Length; i++)
                {
                    char1MenuButtons[i].SetActive(true);
                    if (battleSwitchButtons == false)
                    {
                        char1MenuButtons[2].SetActive(false);
                    }
                }
            }
        }
    }
    public void ActivateChar2MenuButtons()
    {
        if (!isDead)
        {
            if (!char2MenuButtonsActive)
            {
                for (int i = 0; i < char2MenuButtons.Length; i++)
                {
                    char2MenuButtons[i].SetActive(true);
                    if (battleSwitchButtons == false)
                    {
                        char2MenuButtons[2].SetActive(false);
                    }
                }
            }
        }
    }

    public void ActivateChar3MenuButtons()
    {
        if (!isDead)
        {
            if (!char3MenuButtonsActive)
            {
                for (int i = 0; i < char3MenuButtons.Length; i++)
                {
                    char3MenuButtons[i].SetActive(true);
                    if (battleSwitchButtons == false)
                    {
                        char3MenuButtons[2].SetActive(false);
                    }
                }
            }
        }
    }

    public void DeactivateChar1MenuButtons()
    {
        for (int i = 0; i < char1MenuButtons.Length; i++)
        {
            char1MenuButtons[i].SetActive(false);
        }
    }
    public void DeactivateChar2MenuButtons()
    {
        for (int i = 0; i < char2MenuButtons.Length; i++)
        {
            char2MenuButtons[i].SetActive(false);
        }
    }
    public void DeactivateChar3MenuButtons()
    {
        for (int i = 0; i < char3MenuButtons.Length; i++)
        {
            char3MenuButtons[i].SetActive(false);
        }
    }
    public void DeactivateChar1SwitchButtons()
    {
        for (int i = 0; i < availableChar1Buttons.Length; i++)
        {
            availableChar1Buttons[i].SetActive(false);
        }
    }
    public void DeactivateChar2SwitchButtons()
    {
        for (int i = 0; i < availableChar2Buttons.Length; i++)
        {
            availableChar2Buttons[i].SetActive(false);
        }
    }
    public void DeactivateChar3SwitchButtons()
    {
        for (int i = 0; i < availableChar3Buttons.Length; i++)
        {
            availableChar3Buttons[i].SetActive(false);
        }
    }

    public void SetPhysicalAttack()
    {
        if (state == BattleState.CHAR1TURN)
        {
            char1PhysicalAttack = true;
        }
        if (state == BattleState.CHAR2TURN)
        {
            char2PhysicalAttack = true;
        }
        if (state == BattleState.CHAR3TURN)
        {
            char3PhysicalAttack = true;
        }
    }

    public void ActivateTargetButtons()
    {

        inBattleMenu = true;

        if (!skillSelfSupport && !char1SkillAttack
        && !char2SkillAttack && !char3SkillAttack)
        {
            for (int i = 0; i < allyTargetButtons.Length; i++)
            {
                if (activeParty.activeParty[i] != null)
                {
                    allyTargetButtons[i].SetActive(true);
                }
            }
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i] != null)
                {
                    if (enemies[i].health > 0)
                    {
                        enemyTargetButtons[i].SetActive(true);
                    }
                }
            }


            GetComponent<BattleMenuControllerNav>().OpenAttackFirstEnemy();

            if (state == BattleState.CHAR1TURN)
            {
                if (char1Attacking || char1PhysicalAttack)
                {
                    GetComponent<BattleMenuControllerNav>().OpenAttackFirstEnemy();
                }
                else
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(allyTargetButtons[0]);
                }
            }
            if (state == BattleState.CHAR2TURN)
            {
                if (char2Attacking || char2PhysicalAttack)
                {
                    GetComponent<BattleMenuControllerNav>().OpenAttackFirstEnemy();
                }
                else
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(allyTargetButtons[0]);
                }

            }
            if (state == BattleState.CHAR3TURN)
            {
                if (char3Attacking || char3PhysicalAttack)
                {
                    GetComponent<BattleMenuControllerNav>().OpenAttackFirstEnemy();
                }
                else
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(allyTargetButtons[0]);
                }
            }
        }
        else
        {
            if (skillSelfSupport)
            {
                if (state == BattleState.CHAR1TURN)
                {
                    allyTargetButtons[0].SetActive(true);
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(allyTargetButtons[0]);
                }
                if (state == BattleState.CHAR2TURN)
                {
                    allyTargetButtons[1].SetActive(true);
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(allyTargetButtons[1]);
                }
                if (state == BattleState.CHAR3TURN)
                {
                    allyTargetButtons[2].SetActive(true);
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(allyTargetButtons[2]);
                }
            }

            if (char1SkillAttack || char2SkillAttack || char3SkillAttack)
            {
                for (int i = 0; i < enemies.Length; i++)
                {
                    if (enemies[i] != null)
                    {
                        if (enemies[i].health > 0)
                        {
                            enemyTargetButtons[i].SetActive(true);
                        }
                    }
                }

                GetComponent<BattleMenuControllerNav>().OpenAttackFirstEnemy();
            }
        }

        DeactivateDropsUI();
        DeactivateSkillsUI();
        DeactivateChar1MenuButtons();
        DeactivateChar2MenuButtons();
        DeactivateChar3MenuButtons();

        if (state == BattleState.CHAR1TURN)
        {
            returnToPanelButton[0].SetActive(true);
        }
        if (state == BattleState.CHAR2TURN)
        {
            returnToPanelButton[1].SetActive(true);
        }
        if (state == BattleState.CHAR3TURN)
        {
            returnToPanelButton[2].SetActive(true);
        }
    }



    /*public void DeactivateSkillTargetButtons()
    {
        if (lastSkillChoice != null)
        {
            for (int i = 0; i < skillEnemyTargetButtons.Length; i++)
            {
                if (lastSkillChoice.dps)
                {
                    if (enemies[i] != null)
                    {
                        skillEnemyTargetButtons[i].SetActive(false);
                    }
                }
            }

            for (int i = 0; i < skillAllyTargetButtons.Length; i++)
            {
                if (lastSkillChoice.targetSupport)
                {
                    if (skillEnemyTargetButtons != null)
                    {
                        skillAllyTargetButtons[i].SetActive(false);
                    }
                }

                if (lastSkillChoice.selfSupport)
                {
                    if (activeParty.activeParty[i] != null)
                    {
                        returnToPanelButton[i].SetActive(false);
                    }
                }
            }
        }
    }*/
    public void ActivateAllyTargetSprite(int index)
    {
        if (index == 0)
        {
            Vector3 position = new Vector3(Engine.e.activeParty.transform.position.x - 0.5f, Engine.e.activeParty.transform.position.y, Engine.e.activeParty.transform.position.z);
            spriteTarget.transform.position = position;
        }
        if (index == 1)
        {
            Vector3 position = new Vector3(Engine.e.activePartyMember2.transform.position.x - 0.5f, Engine.e.activePartyMember2.transform.position.y, Engine.e.activePartyMember2.transform.position.z);
            spriteTarget.transform.position = position;
        }
        if (index == 2)
        {
            Vector3 position = new Vector3(Engine.e.activePartyMember3.transform.position.x - 0.5f, Engine.e.activePartyMember3.transform.position.y, Engine.e.activePartyMember3.transform.position.z);
            spriteTarget.transform.position = position;
        }

        spriteTarget.SetActive(true);

    }
    public void ActivateEnemyTargetSprite(int index)
    {
        Vector3 position = new Vector3(enemies[index].transform.position.x - 0.5f, enemies[index].transform.position.y, enemies[index].transform.position.z);
        spriteTarget.transform.position = position;
        spriteTarget.SetActive(true);

    }

    public void DeactivateTargetSprite()
    {
        spriteTarget.SetActive(false);
    }


    public void RestartTurn()
    {
        if (state == BattleState.CHAR1TURN)
        {
            Char1Turn();
        }
        if (state == BattleState.CHAR2TURN)
        {
            Char2Turn();
        }
        if (state == BattleState.CHAR3TURN)
        {
            Char3Turn();
        }
    }

    public void ReturnToCharPanel()
    {
        inBattleMenu = false;
        DeactivateTargetButtons();
        DeactivateBattleItems();

        if (state == BattleState.CHAR1TURN)
        {
            char1PhysicalAttack = false;
            char1DropSupport = false;
            char1DropAttack = false;
            char1UsingItem = false;
            char1TargetingEnemy = false;
            char1TargetingTeam = false;
            char1SkillPhysicalAttack = false;
            char1SkillRangedAttack = false;
            char1SkillSelfSupport = false;
            char1SkillTargetSupport = false;
            char1SkillAttack = false;
            char1Attacking = false;
            char1Supporting = false;
            char1Switching = false;
            //char1DropChoice = null;
            //char1SkillChoice = null;
        }

        if (state == BattleState.CHAR2TURN)
        {
            char2PhysicalAttack = false;
            char2DropSupport = false;
            char2DropAttack = false;
            char2UsingItem = false;
            char2TargetingEnemy = false;
            char2TargetingTeam = false;
            char2SkillPhysicalAttack = false;
            char2SkillRangedAttack = false;
            char2SkillSelfSupport = false;
            char2SkillTargetSupport = false;
            char2SkillAttack = false;
            char2Attacking = false;
            char2Supporting = false;
            char2Switching = false;
            //char2DropChoice = null;
            //char2SkillChoice = null;
        }

        if (state == BattleState.CHAR3TURN)
        {
            char3PhysicalAttack = false;
            char3DropSupport = false;
            char3DropAttack = false;
            char3UsingItem = false;
            char3TargetingEnemy = false;
            char3TargetingTeam = false;
            char3SkillPhysicalAttack = false;
            char3SkillRangedAttack = false;
            char3SkillSelfSupport = false;
            char3SkillTargetSupport = false;
            char3SkillAttack = false;
            char3Attacking = false;
            char3Supporting = false;
            char3Switching = false;
            //char3DropChoice = null;
            //char3SkillChoice = null;
        }

        if (char1ATB >= 100)
        {
            ActivateChar1MenuButtons();
        }
        if (char2ATB >= 100)
        {
            ActivateChar2MenuButtons();
        }
        if (char3ATB >= 100)
        {
            ActivateChar3MenuButtons();
        }

        if (state == BattleState.CHAR1TURN)
        {
            Char1Turn();
        }
        if (state == BattleState.CHAR2TURN)
        {
            Char2Turn();
        }
        if (state == BattleState.CHAR3TURN)
        {
            Char3Turn();
        }
    }

    public void DeactivateTargetButtons()
    {

        for (int i = 0; i < Engine.e.activeParty.activeParty.Length; i++)
        {
            if (Engine.e.activeParty.activeParty[i] != null)
            {
                returnToPanelButton[i].SetActive(false);
                allyTargetButtons[i].SetActive(false);
            }
        }

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                enemyTargetButtons[i].SetActive(false);
            }
        }

    }

    public void DisplayBattleItems()
    {
        battleItemMenu.SetActive(true);
        Engine.e.partyInventoryReference.OpenInventoryMenu();

        if (state == BattleState.CHAR1TURN)
        {
            DeactivateChar1MenuButtons();

            if (activeParty.activeParty[1] != null)
            {
                if (char2Ready)
                {
                    DeactivateChar2MenuButtons();
                }
            }

            if (activeParty.activeParty[2] != null)
            {
                if (char3Ready)
                {
                    DeactivateChar3MenuButtons();
                }
            }


        }
        if (state == BattleState.CHAR2TURN)
        {
            DeactivateChar2MenuButtons();


            if (char1Ready)
            {
                DeactivateChar1MenuButtons();
            }


            if (activeParty.activeParty[2] != null)
            {
                if (char3Ready)
                {
                    DeactivateChar3MenuButtons();
                }
            }
        }
        if (state == BattleState.CHAR3TURN)
        {
            DeactivateChar3MenuButtons();

            if (char1Ready)
            {
                DeactivateChar1MenuButtons();
            }



            if (char2Ready)
            {
                DeactivateChar2MenuButtons();

            }
        }
    }

    public void DeactivateBattleItems()
    {

        battleItemMenu.SetActive(false);
        usingItem = false;
        inBattleMenu = false;
        DeactivateSupportButtons();

        if (state == BattleState.CHAR1TURN)
        {
            ActivateChar1MenuButtons();
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(char1MenuButtons[0]);

            if (activeParty.activeParty[1] != null)
            {
                if (char2Ready)
                {
                    ActivateChar2MenuButtons();
                }
            }
            if (activeParty.activeParty[2] != null)
            {
                if (char3Ready)
                {
                    ActivateChar3MenuButtons();
                }
            }
        }
        if (state == BattleState.CHAR2TURN)
        {
            ActivateChar2MenuButtons();
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(char2MenuButtons[0]);


            if (char1Ready)
            {
                ActivateChar1MenuButtons();

            }
            if (activeParty.activeParty[2] != null)
            {
                if (char3Ready)
                {
                    ActivateChar3MenuButtons();
                }
            }
        }
        if (state == BattleState.CHAR3TURN)
        {
            ActivateChar3MenuButtons();
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(char3MenuButtons[0]);


            if (char1Ready)
            {
                ActivateChar1MenuButtons();
            }


            if (char2Ready)
            {
                ActivateChar2MenuButtons();

            }
        }

    }

    public void ActivateSupportButtons()
    {
        Engine.e.battleSystem.enemyPanel.SetActive(true);

        dropsListReference.SetActive(false);
        char1BattlePanel.SetActive(true);
        DeactivateChar1MenuButtons();
        if (Engine.e.activeParty.activeParty[1] != null)
        {
            char2BattlePanel.SetActive(true);
            DeactivateChar2MenuButtons();

        }
        if (Engine.e.activeParty.activeParty[2] != null)
        {
            char3BattlePanel.SetActive(true);
            DeactivateChar3MenuButtons();

        }

        for (int i = 0; i < activeParty.activeParty.Length; i++)
        {
            if (activeParty.activeParty[i] != null)
                allyTargetButtons[i].SetActive(true);
        }
        GetComponent<BattleMenuControllerNav>().OpenDropSupportTarget();

    }
    public void DeactivateSupportButtons()
    {

        for (int i = 0; i < activeParty.activeParty.Length; i++)
        {
            if (activeParty.activeParty[i] != null)
                allyTargetButtons[i].SetActive(false);
        }

    }

    public void ActivateEnemyUI()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] == null)
            {
                enemyUI[i].SetActive(false);
            }
        }
    }
    public void DeactivateEnemyUI()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            enemyUI[i].SetActive(true);
        }
    }

    public void ActivateCharSkillsUI()
    {
        for (int i = 0; i < skillButtons.Length; i++)
        {
            if (skillButtons[i].GetComponent<BattleSkillsUIHolder>().skill != null)
            {
                skillButtons[i].GetComponent<BattleSkillsUIHolder>().SetSkillText();
            }
        }
    }

    public void DeactivateSkillsUI()
    {
        char1BattlePanel.SetActive(true);

        if (Engine.e.activeParty.activeParty[1] != null)
        {
            {
                char2BattlePanel.SetActive(true);
            }
        }
        if (Engine.e.activeParty.activeParty[2] != null)
        {
            char3BattlePanel.SetActive(true);
        }
        skillListReference.SetActive(false);
        for (int i = 0; i < Engine.e.gameSkills.Length; i++)
        {
            skillButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "-";
        }

    }

    public void ActivateCharDropsUI()
    {
        for (int i = 0; i < dropsButtons.Length; i++)
        {
            if (dropsButtons[i].GetComponent<BattleDropsUIHolder>().drop != null)
            {
                dropsButtons[i].GetComponent<BattleDropsUIHolder>().SetDropText();
            }
        }
    }

    public void DeactivateDropsUI()
    {
        dropsListReference.SetActive(false);

        for (int i = 0; i < dropsButtons.Length; i++)
        {
            dropsButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "-";
        }
    }


    public void DropChoice(Drops dropChoice)
    {
        int index = 0;

        if (state == BattleState.CHAR1TURN)
        {
            index = 0;
        }
        if (state == BattleState.CHAR2TURN)
        {
            index = 1;
        }
        if (state == BattleState.CHAR3TURN)
        {
            index = 2;
        }


        if (Engine.e.activeParty.activeParty[index].gameObject.GetComponent<Character>().currentMana >= dropChoice.dropCost)
        {
            lastDropChoice = dropChoice;
            Engine.e.battleSystem.enemyPanel.SetActive(true);

            Engine.e.activeParty.activeParty[index].gameObject.GetComponent<Character>().UseDrop(dropChoice);
            if (index == 0)
            {
                char1DropChoice = dropChoice;
                DeactivateChar1MenuButtons();
            }
            if (index == 1)
            {
                char2DropChoice = dropChoice;
                DeactivateChar2MenuButtons();
            }
            if (index == 2)
            {
                char3DropChoice = dropChoice;
                DeactivateChar3MenuButtons();
            }
            DeactivateDropsUI();

        }
        // }

        else
        {
            return;
        }

    }


    public void SkillChoice(Skills skillChoice)
    {
        int index = 0;

        if (state == BattleState.CHAR1TURN)
        {
            index = 0;
        }
        if (state == BattleState.CHAR2TURN)
        {
            index = 1;
        }
        if (state == BattleState.CHAR3TURN)
        {
            index = 2;
        }


        if (Engine.e.activeParty.activeParty[index].gameObject.GetComponent<Character>().currentEnergy >= skillChoice.skillCost)
        {
            lastSkillChoice = skillChoice;

            Engine.e.battleSystem.enemyPanel.SetActive(true);

            Engine.e.activeParty.activeParty[index].gameObject.GetComponent<Character>().UseSkill(skillChoice);

            if (index == 0)
            {
                char1SkillChoice = skillChoice;
                DeactivateChar1MenuButtons();
            }
            if (index == 1)
            {
                char2SkillChoice = skillChoice;
                DeactivateChar2MenuButtons();
            }
            if (index == 2)
            {
                char3SkillChoice = skillChoice;
                DeactivateChar3MenuButtons();
            }

            DeactivateSkillsUI();

        }
        else
        {
            return;
        }

    }

    // For non-ATB battlesystem
    public IEnumerator CheckNext()
    {

        yield return new WaitForSeconds(0.1f);
        attackingTeam = false;
        charUsingSkill = false;
        GameObject character = null;

        if (state == BattleState.CHAR1TURN)
        {
            currentIndex = 0;
            nextIndex = 1;
            partyCheckNext = true;
            character = Engine.e.activeParty.gameObject;
        }

        if (state == BattleState.CHAR2TURN)
        {
            currentIndex = 1;
            nextIndex = 2;
            partyCheckNext = true;
            character = Engine.e.activePartyMember2.gameObject;
        }

        if (state == BattleState.CHAR3TURN)
        {
            currentIndex = 2;
            nextIndex = 0;
            partyCheckNext = true;
            character = Engine.e.activePartyMember3.gameObject;
        }
        if (state == BattleState.ENEMY1TURN)
        {
            currentIndex = 0;
            nextIndex = 1;
            partyCheckNext = false;

        }
        if (state == BattleState.ENEMY2TURN)
        {
            currentIndex = 1;
            nextIndex = 2;
            partyCheckNext = false;

        }
        if (state == BattleState.ENEMY3TURN)
        {
            currentIndex = 2;
            nextIndex = 3;
            partyCheckNext = false;

        }
        if (state == BattleState.ENEMY4TURN)
        {
            currentIndex = 3;
            nextIndex = 0;
            partyCheckNext = false;

        }

        if (!isDead)
        {
            if (partyCheckNext)
            {
                if (activeParty.activeParty[currentIndex].GetComponent<Character>().isConfused)
                {
                    activeParty.activeParty[currentIndex].GetComponent<Character>().confuseTimer++;
                    if (activeParty.activeParty[currentIndex].GetComponent<Character>().confuseTimer == 3)
                    {
                        activeParty.activeParty[currentIndex].GetComponent<Character>().isConfused = false;
                        activeParty.activeParty[currentIndex].GetComponent<Character>().confuseTimer = 0;
                    }
                }
                if (activeParty.activeParty[currentIndex].GetComponent<Character>().isPoisoned)
                {
                    GameObject dmgPopup = Instantiate(Engine.e.battleSystem.damagePopup, character.transform.position, Quaternion.identity);

                    isDead = Engine.e.TakePoisonDamage(currentIndex, activeParty.activeParty[currentIndex].GetComponent<Character>().poisonDmg);

                    dmgPopup.transform.GetChild(0).GetComponent<TextMeshPro>().text = activeParty.activeParty[currentIndex].GetComponent<Character>().poisonDmg.ToString();
                    Destroy(dmgPopup, 1f);
                }

                if (activeParty.activeParty[currentIndex].GetComponent<Character>().deathInflicted)
                {
                    isDead = activeParty.activeParty[currentIndex].GetComponent<Character>().TakeDeathDamage(currentIndex);
                }
            }
            else
            {
                if (enemies[currentIndex].GetComponent<Enemy>().isConfused)
                {
                    enemies[currentIndex].GetComponent<Enemy>().confuseTimer++;
                    if (enemies[currentIndex].GetComponent<Enemy>().confuseTimer == 3)
                    {
                        enemies[currentIndex].GetComponent<Enemy>().isConfused = false;
                        enemies[currentIndex].GetComponent<Enemy>().confuseTimer = 0;
                    }
                }

                if (enemies[currentIndex].GetComponent<Enemy>().isPoisoned)
                {
                    enemies[currentIndex].GetComponent<Enemy>().TakePoisonDamage(currentIndex, enemies[currentIndex].GetComponent<Enemy>().poisonDmg);


                }

                if (enemies[currentIndex].GetComponent<Enemy>().deathInflicted)
                {
                    enemies[currentIndex].GetComponent<Enemy>().TakeDeathDamage();
                }
                if (enemies[currentIndex].gameObject.GetComponent<Enemy>().health <= 0)
                {
                    enemies[currentIndex].gameObject.GetComponent<Enemy>().health = 0;

                    isDead = EnemyGroup.enemyGroup.CheckEndBattle();

                    if (isDead)
                    {
                        state = BattleState.LEVELUPCHECK;
                        yield return new WaitForSeconds(2f);
                        StartCoroutine(LevelUpCheck());
                    }
                }
            }
        }

        if (partyCheckNext)
        {
            if (!isDead)
            {
                if (currentIndex != 2)
                {
                    if (Engine.e.activeParty.activeParty[nextIndex] != null)
                    {
                        if (Engine.e.activeParty.activeParty[nextIndex].gameObject.GetComponent<Character>().currentHealth > 0)
                        {
                            if (!Engine.e.activeParty.activeParty[nextIndex].gameObject.GetComponent<Character>().isAsleep)
                            {
                                if (currentIndex == 0)
                                {
                                    state = BattleState.CHAR2TURN;
                                    Char2Turn();
                                }
                                if (currentIndex == 1)
                                {
                                    state = BattleState.CHAR3TURN;
                                    Char3Turn();
                                }
                                partyCheckNext = false;
                            }
                            else
                            {
                                if (currentIndex == 0)
                                {
                                    int newIndex = Engine.e.GetNextRemainingPartyMember();

                                    if (activeParty.activeParty[newIndex] != null)
                                    {
                                        if (newIndex == 1)
                                        {
                                            state = BattleState.CHAR2TURN;
                                            Char2Turn();
                                        }
                                        if (newIndex == 2)
                                        {
                                            state = BattleState.CHAR3TURN;
                                            Char3Turn();
                                        }

                                        if (newIndex == 0)
                                        {
                                            int newEnemyIndex = enemyGroup.GetNextRemainingEnemy();

                                            if (newEnemyIndex == 0)
                                            {
                                                state = BattleState.ENEMY1TURN;
                                                Enemy1Turn();
                                            }
                                            if (newEnemyIndex == 1)
                                            {
                                                state = BattleState.ENEMY2TURN;
                                                Enemy2Turn();
                                            }
                                            if (newEnemyIndex == 2)
                                            {
                                                state = BattleState.ENEMY3TURN;
                                                Enemy3Turn();
                                            }
                                            if (newEnemyIndex == 3)
                                            {
                                                state = BattleState.ENEMY4TURN;
                                                Enemy4Turn();
                                            }
                                        }
                                    }
                                }

                                if (currentIndex == 1)
                                {
                                    if (!Engine.e.activeParty.activeParty[2].GetComponent<Character>().isAsleep)
                                    {
                                        state = BattleState.CHAR3TURN;
                                        Char3Turn();
                                    }
                                    else
                                    {
                                        int newIndex = enemyGroup.GetNextRemainingEnemy();

                                        if (newIndex == 0)
                                        {
                                            state = BattleState.ENEMY1TURN;
                                            Enemy1Turn();
                                        }
                                        if (newIndex == 1)
                                        {
                                            state = BattleState.ENEMY2TURN;
                                            Enemy2Turn();
                                        }
                                        if (newIndex == 2)
                                        {
                                            state = BattleState.ENEMY3TURN;
                                            Enemy3Turn();
                                        }
                                        if (newIndex == 3)
                                        {
                                            state = BattleState.ENEMY4TURN;
                                            Enemy4Turn();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            int newIndex = Engine.e.GetNextRemainingPartyMember();

                            if (newIndex == 0)
                            {
                                newIndex = enemyGroup.GetNextRemainingEnemy();

                                if (newIndex == 0)
                                {
                                    state = BattleState.ENEMY1TURN;
                                    Enemy1Turn();
                                }
                                if (newIndex == 1)
                                {
                                    state = BattleState.ENEMY2TURN;
                                    Enemy2Turn();
                                }
                                if (newIndex == 2)
                                {
                                    state = BattleState.ENEMY3TURN;
                                    Enemy3Turn();
                                }
                                if (newIndex == 3)
                                {
                                    state = BattleState.ENEMY4TURN;
                                    Enemy4Turn();
                                }
                            }

                            if (newIndex == 1)
                            {
                                state = BattleState.CHAR2TURN;
                                Char2Turn();
                            }
                            if (newIndex == 2)
                            {
                                state = BattleState.CHAR3TURN;
                                Char3Turn();
                            }
                        }
                        partyCheckNext = false;
                    }
                    else
                    {
                        int newIndex = enemyGroup.GetNextRemainingEnemy();

                        if (newIndex == 0)
                        {
                            state = BattleState.ENEMY1TURN;
                            Enemy1Turn();
                        }
                        if (newIndex == 1)
                        {
                            state = BattleState.ENEMY2TURN;
                            Enemy2Turn();
                        }

                        if (newIndex == 2)
                        {

                            state = BattleState.ENEMY3TURN;
                            Enemy3Turn();
                        }

                        if (newIndex == 3)
                        {

                            state = BattleState.ENEMY4TURN;
                            Enemy4Turn();
                        }
                    }
                    partyCheckNext = false;
                }

                if (currentIndex == 2)
                {
                    if (enemies[nextIndex].gameObject.GetComponent<Enemy>().health > 0 && !enemies[nextIndex].GetComponent<Enemy>().isAsleep)
                    {
                        state = BattleState.ENEMY1TURN;
                        Enemy1Turn();
                        partyCheckNext = false;
                    }
                    else
                    {
                        int newIndex = enemyGroup.GetNextRemainingEnemy();

                        if (newIndex == 1)
                        {
                            state = BattleState.ENEMY2TURN;
                            Enemy2Turn();
                        }

                        if (newIndex == 2)
                        {

                            state = BattleState.ENEMY3TURN;
                            Enemy3Turn();
                        }

                        if (newIndex == 3)
                        {

                            state = BattleState.ENEMY4TURN;
                            Enemy4Turn();
                        }
                    }
                    partyCheckNext = false;
                }
            }
            else

            {
                state = BattleState.LOST;
                StartCoroutine(EndBattle());
            }
        }
        else
        {
            if (!isDead)
            {
                if (currentIndex != 3)
                {
                    if (enemies[nextIndex] != null)
                    {
                        if (enemies[nextIndex].gameObject.GetComponent<Enemy>().health > 0)
                        {
                            if (!enemies[nextIndex].gameObject.GetComponent<Enemy>().isAsleep)
                            {
                                if (currentIndex == 0)
                                {
                                    state = BattleState.ENEMY2TURN;
                                    Enemy2Turn();
                                }
                                if (currentIndex == 1)
                                {
                                    state = BattleState.ENEMY3TURN;
                                    Enemy3Turn();
                                }
                                if (currentIndex == 2)
                                {
                                    state = BattleState.ENEMY4TURN;
                                    Enemy4Turn();
                                }
                            }
                            else
                            {
                                int newIndex = enemyGroup.GetNextRemainingEnemy();
                                Debug.Log("New Index: " + newIndex);


                                if (currentIndex == 0)
                                {
                                    if (newIndex == 0)
                                    {
                                        int newCharacterIndex = Engine.e.GetNextRemainingPartyMember();

                                        if (newCharacterIndex == 0)
                                        {
                                            state = BattleState.CHAR1TURN;
                                            Char1Turn();
                                        }

                                        if (newCharacterIndex == 1)
                                        {

                                            state = BattleState.CHAR2TURN;
                                            Char2Turn();
                                        }

                                        if (newCharacterIndex == 2)
                                        {
                                            state = BattleState.CHAR3TURN;
                                            Char3Turn();
                                        }
                                    }

                                    if (newIndex == 1)
                                    {
                                        state = BattleState.ENEMY2TURN;
                                        Enemy2Turn();
                                    }
                                    if (newIndex == 2)
                                    {
                                        state = BattleState.ENEMY3TURN;
                                        Enemy3Turn();
                                    }
                                    if (newIndex == 3)
                                    {
                                        state = BattleState.ENEMY4TURN;
                                        Enemy4Turn();
                                    }
                                }

                                if (currentIndex == 1)
                                {

                                    if (newIndex == 0 || newIndex == 1)
                                    {
                                        int newCharacterIndex = Engine.e.GetNextRemainingPartyMember();

                                        if (newCharacterIndex == 0)
                                        {
                                            state = BattleState.CHAR1TURN;
                                            Char1Turn();
                                        }

                                        if (newCharacterIndex == 1)
                                        {

                                            state = BattleState.CHAR2TURN;
                                            Char2Turn();
                                        }

                                        if (newCharacterIndex == 2)
                                        {
                                            state = BattleState.CHAR3TURN;
                                            Char3Turn();
                                        }
                                    }

                                    if (newIndex == 2)
                                    {
                                        state = BattleState.ENEMY3TURN;
                                        Enemy3Turn();
                                    }
                                    if (newIndex == 3)
                                    {
                                        state = BattleState.ENEMY4TURN;
                                        Enemy4Turn();
                                    }
                                }

                                if (currentIndex == 2)
                                {
                                    if (newIndex == 0 || newIndex == 1 || newIndex == 2)
                                    {
                                        int newCharacterIndex = Engine.e.GetNextRemainingPartyMember();

                                        if (newCharacterIndex == 0)
                                        {
                                            state = BattleState.CHAR1TURN;
                                            Char1Turn();
                                        }

                                        if (newCharacterIndex == 1)
                                        {

                                            state = BattleState.CHAR2TURN;
                                            Char2Turn();
                                        }

                                        if (newCharacterIndex == 2)
                                        {
                                            state = BattleState.CHAR3TURN;
                                            Char3Turn();
                                        }
                                    }
                                    if (newIndex == 3)
                                    {
                                        state = BattleState.ENEMY4TURN;
                                        Enemy4Turn();
                                    }
                                }

                                if (currentIndex == 3)
                                {

                                    int newCharacterIndex = Engine.e.GetNextRemainingPartyMember();

                                    if (newCharacterIndex == 0)
                                    {
                                        state = BattleState.CHAR1TURN;
                                        Char1Turn();
                                    }

                                    if (newCharacterIndex == 1)
                                    {

                                        state = BattleState.CHAR2TURN;
                                        Char2Turn();
                                    }

                                    if (newCharacterIndex == 2)
                                    {
                                        state = BattleState.CHAR3TURN;
                                        Char3Turn();
                                    }
                                }

                            }
                        }
                        else
                        {
                            int newIndex = enemyGroup.GetNextRemainingEnemy();
                            Debug.Log("New Index: " + newIndex);


                            if (currentIndex == 0)
                            {
                                if (newIndex == 0)
                                {
                                    int newCharacterIndex = Engine.e.GetNextRemainingPartyMember();

                                    if (newCharacterIndex == 0)
                                    {
                                        state = BattleState.CHAR1TURN;
                                        Char1Turn();
                                    }

                                    if (newCharacterIndex == 1)
                                    {

                                        state = BattleState.CHAR2TURN;
                                        Char2Turn();
                                    }

                                    if (newCharacterIndex == 2)
                                    {
                                        state = BattleState.CHAR3TURN;
                                        Char3Turn();
                                    }
                                }

                                if (newIndex == 1)
                                {
                                    state = BattleState.ENEMY2TURN;
                                    Enemy2Turn();
                                }
                                if (newIndex == 2)
                                {
                                    state = BattleState.ENEMY3TURN;
                                    Enemy3Turn();
                                }
                                if (newIndex == 3)
                                {
                                    state = BattleState.ENEMY4TURN;
                                    Enemy4Turn();
                                }
                            }

                            if (currentIndex == 1)
                            {

                                if (newIndex == 0 || newIndex == 1)
                                {
                                    int newCharacterIndex = Engine.e.GetNextRemainingPartyMember();

                                    if (newCharacterIndex == 0)
                                    {
                                        state = BattleState.CHAR1TURN;
                                        Char1Turn();
                                    }

                                    if (newCharacterIndex == 1)
                                    {

                                        state = BattleState.CHAR2TURN;
                                        Char2Turn();
                                    }

                                    if (newCharacterIndex == 2)
                                    {
                                        state = BattleState.CHAR3TURN;
                                        Char3Turn();
                                    }
                                }

                                if (newIndex == 2)
                                {
                                    state = BattleState.ENEMY3TURN;
                                    Enemy3Turn();
                                }
                                if (newIndex == 3)
                                {
                                    state = BattleState.ENEMY4TURN;
                                    Enemy4Turn();
                                }
                            }

                            if (currentIndex == 2)
                            {
                                if (newIndex == 0 || newIndex == 1 || newIndex == 2)
                                {
                                    int newCharacterIndex = Engine.e.GetNextRemainingPartyMember();

                                    if (newCharacterIndex == 0)
                                    {
                                        state = BattleState.CHAR1TURN;
                                        Char1Turn();
                                    }

                                    if (newCharacterIndex == 1)
                                    {

                                        state = BattleState.CHAR2TURN;
                                        Char2Turn();
                                    }

                                    if (newCharacterIndex == 2)
                                    {
                                        state = BattleState.CHAR3TURN;
                                        Char3Turn();
                                    }
                                }
                                if (newIndex == 3)
                                {
                                    state = BattleState.ENEMY4TURN;
                                    Enemy4Turn();
                                }
                            }

                            if (currentIndex == 3)
                            {

                                int newCharacterIndex = Engine.e.GetNextRemainingPartyMember();

                                if (newCharacterIndex == 0)
                                {
                                    state = BattleState.CHAR1TURN;
                                    Char1Turn();
                                }

                                if (newCharacterIndex == 1)
                                {

                                    state = BattleState.CHAR2TURN;
                                    Char2Turn();
                                }

                                if (newCharacterIndex == 2)
                                {
                                    state = BattleState.CHAR3TURN;
                                    Char3Turn();
                                }
                            }
                        }
                    }

                    else
                    {
                        int newIndex = Engine.e.GetNextRemainingPartyMember();

                        Debug.Log("NewIndex: " + newIndex);

                        if (!activeParty.activeParty[newIndex].GetComponent<Character>().isAsleep)
                        {
                            if (newIndex == 0)
                            {
                                state = BattleState.CHAR1TURN;
                                Char1Turn();
                            }
                            if (newIndex == 1)
                            {
                                state = BattleState.CHAR2TURN;
                                Char2Turn();
                            }
                            if (newIndex == 2)
                            {
                                state = BattleState.CHAR3TURN;
                                Char3Turn();
                            }
                        }
                        else
                        {
                            if (currentIndex == 0)
                            {
                                state = BattleState.ENEMY1TURN;
                                Enemy1Turn();
                            }
                            if (currentIndex == 1)
                            {
                                state = BattleState.ENEMY2TURN;
                                Enemy2Turn();
                            }
                            if (currentIndex == 2)
                            {
                                state = BattleState.ENEMY3TURN;
                                Enemy3Turn();
                            }
                            if (currentIndex == 3)
                            {
                                state = BattleState.ENEMY4TURN;
                                Enemy4Turn();
                            }
                        }
                    }
                    partyCheckNext = false;
                }

                if (currentIndex == 3)
                {
                    if (Engine.e.activeParty.activeParty[nextIndex].GetComponent<Character>().currentHealth > 0 && !Engine.e.activeParty.activeParty[nextIndex].GetComponent<Character>().isAsleep)
                    {
                        state = BattleState.CHAR1TURN;
                        Char1Turn();
                        partyCheckNext = false;
                    }
                    else
                    {
                        int newIndex = Engine.e.GetNextRemainingPartyMember();

                        if (newIndex == 1)
                        {
                            state = BattleState.CHAR2TURN;
                            Char2Turn();
                        }

                        if (newIndex == 2)
                        {

                            state = BattleState.CHAR3TURN;
                            Char3Turn();
                        }
                    }
                    partyCheckNext = false;
                }
            }
        }
    }



    // General check for enemies, mainly regarding poison and death damage (if they die, should the battle end)
    public bool CheckIsDeadEnemy()
    {
        if (enemies[3] != null)
        {
            if (enemies[3].GetComponent<Enemy>().health == 0 && enemies[2].GetComponent<Enemy>().health == 0
            && enemies[1].GetComponent<Enemy>().health == 0 && enemies[0].GetComponent<Enemy>().health == 0)
            {
                return true;
            }
        }

        if (enemies[3] == null)
        {
            if (enemies[2] != null)
            {
                if (enemies[2].GetComponent<Enemy>().health == 0 && enemies[1].GetComponent<Enemy>().health == 0
                && enemies[0].GetComponent<Enemy>().health == 0)
                {
                    return true;
                }
            }
        }

        if (enemies[3] == null && enemies[2] == null)
        {
            if (enemies[1] != null)
            {
                if (enemies[1].GetComponent<Enemy>().health == 0 && enemies[0].GetComponent<Enemy>().health == 0)
                {
                    return true;
                }
            }
        }
        if (enemies[3] == null && enemies[2] == null && enemies[1] == null)
        {

            if (enemies[0].GetComponent<Enemy>().health == 0)
            {
                return true;
            }
        }
        return false;
    }


    public void EndTurn()
    {
        charMoving = false;
        targetCheck = false;
        attackingTeam = false;

        GameObject characterAttacking = null;
        int index = 0;

        if (partyTurn)
        {
            isDead = CheckIsDeadEnemy();

            partyTurn = false;

            if (currentInQueue == BattleState.CHAR1TURN || currentInQueue == BattleState.CONFCHAR1)
            {
                index = 0;

                char1PhysicalAttack = false;
                char1DropSupport = false;
                char1DropAttack = false;
                char1UsingItem = false;
                char1TargetingEnemy = false;
                char1TargetingTeam = false;
                char1SkillPhysicalAttack = false;
                char1SkillRangedAttack = false;
                char1SkillSelfSupport = false;
                char1SkillTargetSupport = false;
                char1SkillAttack = false;
                char1Attacking = false;
                char1Supporting = false;
                char1Ready = false;
                char1ConfusedReady = false;
                char1Switching = false;

                char1ItemToBeUsed = null;

                confuseAttack = false;

                physicalAttack = false;
                dropAttack = false;
                dropSupport = false;
                skillPhysicalAttack = false;
                skillRangedAttack = false;
                skillSelfSupport = false;
                skillTargetSupport = false;
                charUsingSkill = false;

                targetingTeam = false;
                targetingEnemy = false;
                //char1DropChoice = null;
                //char1SkillChoice = null;

                char1ATB = 0;
                char1ATBGuage.value = char1ATB;
            }

            if (currentInQueue == BattleState.CHAR2TURN || currentInQueue == BattleState.CONFCHAR2)
            {
                index = 1;

                char2PhysicalAttack = false;
                char2DropSupport = false;
                char2DropAttack = false;
                char2UsingItem = false;
                char2TargetingEnemy = false;
                char2TargetingTeam = false;
                char2SkillPhysicalAttack = false;
                char2SkillRangedAttack = false;
                char2SkillSelfSupport = false;
                char2SkillTargetSupport = false;
                char2SkillAttack = false;
                char2Attacking = false;
                char2Supporting = false;
                char2Ready = false;
                char2ConfusedReady = false;
                char2Switching = false;

                char2ItemToBeUsed = null;

                physicalAttack = false;
                dropAttack = false;
                dropSupport = false;
                skillPhysicalAttack = false;
                skillRangedAttack = false;
                skillSelfSupport = false;
                skillTargetSupport = false;
                charUsingSkill = false;

                targetingTeam = false;
                targetingEnemy = false;
                char2ATB = 0;
                char2ATBGuage.value = char1ATB;
            }

            if (currentInQueue == BattleState.CHAR3TURN || currentInQueue == BattleState.CONFCHAR3)
            {
                index = 2;

                char3PhysicalAttack = false;
                char3DropSupport = false;
                char3DropAttack = false;
                char3UsingItem = false;
                char3TargetingEnemy = false;
                char3TargetingTeam = false;
                char3SkillPhysicalAttack = false;
                char3SkillRangedAttack = false;
                char3SkillSelfSupport = false;
                char3SkillTargetSupport = false;
                char3SkillAttack = false;
                char3Attacking = false;
                char3Supporting = false;
                char3Ready = false;
                char3ConfusedReady = false;
                char3Switching = false;

                char3ItemToBeUsed = null;

                physicalAttack = false;
                dropAttack = false;
                dropSupport = false;
                skillPhysicalAttack = false;
                skillRangedAttack = false;
                skillSelfSupport = false;
                skillTargetSupport = false;
                charUsingSkill = false;

                targetingTeam = false;
                targetingEnemy = false;

                char3ATB = 0;
                char3ATBGuage.value = char3ATB;
            }

            if (activeParty.activeParty[index].GetComponent<Character>().isPoisoned)
            {
                GameObject dmgPopup = Instantiate(Engine.e.battleSystem.damagePopup, characterAttacking.transform.position, Quaternion.identity);

                isDead = Engine.e.TakePoisonDamage(currentIndex, activeParty.activeParty[index].GetComponent<Character>().poisonDmg);

                dmgPopup.transform.GetChild(0).GetComponent<TextMeshPro>().text = activeParty.activeParty[index].GetComponent<Character>().poisonDmg.ToString();
                Destroy(dmgPopup, 1f);
            }

            if (activeParty.activeParty[index].GetComponent<Character>().deathInflicted)
            {
                isDead = activeParty.activeParty[index].GetComponent<Character>().TakeDeathDamage(index);
            }

            if (Engine.e.activeParty.activeParty[index].GetComponent<Character>().currentHealth <= 0)
            {
                Engine.e.activeParty.activeParty[index].GetComponent<Character>().currentHealth = 0;

                if (isDead)
                {
                    state = BattleState.LOST;
                    StartCoroutine(EndBattle());
                }
            }

            if (activeParty.activeParty[index].GetComponent<Character>().isConfused)
            {
                confuseAttack = false;
                Debug.Log("Adding confuse timer");
                activeParty.activeParty[index].GetComponent<Character>().confuseTimer++;
                if (activeParty.activeParty[index].GetComponent<Character>().confuseTimer == 3)
                {
                    activeParty.activeParty[index].GetComponent<Character>().isConfused = false;
                    activeParty.activeParty[index].GetComponent<Character>().inflicted = false;
                    activeParty.activeParty[index].GetComponent<Character>().confuseTimer = 0;
                }
            }
        }
        else
        {
            if (currentInQueue == BattleState.ENEMY1TURN)
            {
                index = 0;

                enemy1Ready = false;
                enemy1ATB = 0;
                enemy1ATBGuage.value = enemy1ATB;
            }
            if (currentInQueue == BattleState.ENEMY2TURN)
            {
                index = 1;

                enemy2Ready = false;
                enemy2ATB = 0;
                enemy2ATBGuage.value = enemy2ATB;
            }
            if (currentInQueue == BattleState.ENEMY3TURN)
            {
                index = 2;

                enemy3Ready = false;
                enemy3ATB = 0;
                enemy3ATBGuage.value = enemy3ATB;
            }
            if (currentInQueue == BattleState.ENEMY4TURN)
            {
                index = 3;

                enemy4Ready = false;
                enemy4ATB = 0;
                enemy4ATBGuage.value = enemy4ATB;
            }

            if (enemies[index].GetComponent<Enemy>().isConfused)
            {
                enemies[index].GetComponent<Enemy>().confuseTimer++;
                if (enemies[index].GetComponent<Enemy>().confuseTimer == 3)
                {
                    enemies[index].GetComponent<Enemy>().isConfused = false;
                    enemies[index].GetComponent<Enemy>().inflicted = false;
                    enemies[index].GetComponent<Enemy>().confuseTimer = 0;
                }
            }

            if (enemies[index].GetComponent<Enemy>().isPoisoned)
            {
                enemies[index].GetComponent<Enemy>().TakePoisonDamage(index, enemies[index].GetComponent<Enemy>().poisonDmg);
                isDead = CheckIsDeadEnemy();

            }

            if (enemies[index].GetComponent<Enemy>().deathInflicted)
            {
                enemies[index].GetComponent<Enemy>().TakeDeathDamage();
                isDead = CheckIsDeadEnemy();

            }
            if (enemies[index].GetComponent<EnemyMovement>() != null)
            {
                enemies[index].GetComponent<EnemyMovement>().enabled = true;
            }
        }


        battleQueue.Dequeue();

        if (battleQueue.Count > 0)
        {
            currentInQueue = battleQueue.Peek();
        }
        else
        {
            currentInQueue = BattleState.ATBCHECK;
        }

        // state = BattleState.ATBCHECK;
    }

    public void ResetPartyMemberStats()
    {

        char1ATBGuage.value = 0;
        char2ATBGuage.value = 0;
        char3ATBGuage.value = 0;

        enemy1ATBGuage.value = 0;
        enemy2ATBGuage.value = 0;
        enemy3ATBGuage.value = 0;
        enemy4ATBGuage.value = 0;

        char1ATB = 0;
        char2ATB = 0;
        char3ATB = 0;
        enemy1ATB = 0;
        enemy2ATB = 0;
        enemy3ATB = 0;
        enemy4ATB = 0;

        char1Attacking = false;
        char1ConfusedReady = false;
        char1DropAttack = false;
        char1PhysicalAttack = false;
        char1Ready = false;
        char1SkillAttack = false;
        char1SkillPhysicalAttack = false;
        char1SkillRangedAttack = false;
        char1Supporting = false;
        char1UsingItem = false;
        char1Switching = false;

        char2Attacking = false;
        char2ConfusedReady = false;
        char2DropAttack = false;
        char2PhysicalAttack = false;
        char2Ready = false;
        char2SkillAttack = false;
        char2SkillPhysicalAttack = false;
        char2SkillRangedAttack = false;
        char2Supporting = false;
        char2UsingItem = false;
        char2Switching = false;

        char3Attacking = false;
        char3ConfusedReady = false;
        char3DropAttack = false;
        char3PhysicalAttack = false;
        char3Ready = false;
        char3SkillAttack = false;
        char3SkillPhysicalAttack = false;
        char3SkillRangedAttack = false;
        char3Supporting = false;
        char3UsingItem = false;
        char3Switching = false;

        if (grieveStatBoost)
            if (Engine.e.party[0] != null)
            {
                Engine.e.party[0].GetComponent<Grieve>().physicalDamage -= grievePhysicalBoost;
                grievePhysicalBoost = 0;
                Engine.e.party[0].GetComponent<Grieve>().currentHealth -= grieveHealthBoost;
                grieveHealthBoost = 0;
                Engine.e.party[0].GetComponent<Grieve>().currentMana -= grieveManaBoost;
                grieveManaBoost = 0;
                Engine.e.party[0].GetComponent<Grieve>().currentEnergy -= grieveEnergyBoost;
                grieveEnergyBoost = 0;
                Engine.e.party[0].GetComponent<Grieve>().dodgeChance -= grieveDodgeBoost;
                grieveDodgeBoost = 0;
            }

        if (macStatBoost)
            if (Engine.e.party[1] != null)
            {
                Engine.e.party[1].GetComponent<Mac>().physicalDamage -= macPhysicalBoost;
                macPhysicalBoost = 0;
                Engine.e.party[1].GetComponent<Mac>().currentHealth -= macHealthBoost;
                macHealthBoost = 0;
                Engine.e.party[1].GetComponent<Mac>().currentMana -= macManaBoost;
                macManaBoost = 0;
                Engine.e.party[1].GetComponent<Mac>().currentEnergy -= macEnergyBoost;
                macEnergyBoost = 0;
                Engine.e.party[1].GetComponent<Mac>().dodgeChance -= macDodgeBoost;
                macDodgeBoost = 0;
            }

        if (fieldStatBoost)
            if (Engine.e.party[2] != null)
            {
                Engine.e.party[2].GetComponent<Field>().physicalDamage -= fieldPhysicalBoost;
                fieldPhysicalBoost = 0;
                Engine.e.party[2].GetComponent<Field>().currentHealth -= fieldHealthBoost;
                fieldHealthBoost = 0;
                Engine.e.party[2].GetComponent<Field>().currentMana -= fieldManaBoost;
                fieldManaBoost = 0;
                Engine.e.party[2].GetComponent<Field>().currentEnergy -= fieldEnergyBoost;
                fieldEnergyBoost = 0;
                Engine.e.party[2].GetComponent<Field>().dodgeChance -= fieldDodgeBoost;
                fieldDodgeBoost = 0;
            }

        if (riggsStatBoost)
            if (Engine.e.party[3] != null)
            {
                Engine.e.party[3].GetComponent<Riggs>().physicalDamage -= riggsPhysicalBoost;
                riggsPhysicalBoost = 0;
                Engine.e.party[3].GetComponent<Riggs>().currentHealth -= riggsHealthBoost;
                riggsHealthBoost = 0;
                Engine.e.party[3].GetComponent<Riggs>().currentMana -= riggsManaBoost;
                riggsManaBoost = 0;
                Engine.e.party[3].GetComponent<Riggs>().currentEnergy -= riggsEnergyBoost;
                riggsEnergyBoost = 0;
                Engine.e.party[3].GetComponent<Riggs>().dodgeChance -= riggsDodgeBoost;
                riggsDodgeBoost = 0;
            }

        for (int i = 0; i < Engine.e.party.Length; i++)
        {
            if (Engine.e.party[i] != null)
            {
                Engine.e.party[i].GetComponent<Character>().isPoisoned = false;
                Engine.e.party[i].GetComponent<Character>().isAsleep = false;
                Engine.e.party[i].GetComponent<Character>().isConfused = false;
                Engine.e.party[i].GetComponent<Character>().deathInflicted = false;
                Engine.e.party[i].GetComponent<Character>().inflicted = false;
                Engine.e.party[i].GetComponent<Character>().poisonDmg = 0;
                Engine.e.party[i].GetComponent<Character>().sleepTimer = 0;
                Engine.e.party[i].GetComponent<Character>().confuseTimer = 0;
                Engine.e.party[i].GetComponent<Character>().deathTimer = 3;
                Engine.e.party[i].GetComponent<SpriteRenderer>().color = Color.white;
                Destroy(Engine.e.party[i].GetComponent<Character>().deathTimerPopup);
            }
        }
    }

    private void ChangeCharState()
    {

        if (!inBattleMenu)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (state == BattleState.CHAR1TURN)
                {
                    if (activeParty.activeParty[2] != null)
                    {
                        if (char2Ready)
                        {
                            DeactivateSkillsUI();
                            DeactivateDropsUI();
                            Char2Turn();
                        }
                        else
                        {
                            if (char3Ready)
                            {
                                DeactivateSkillsUI();
                                DeactivateDropsUI();
                                Char3Turn();
                            }
                        }
                    }
                    else
                    {
                        if (char2Ready)
                        {
                            DeactivateSkillsUI();
                            DeactivateDropsUI();
                            Char2Turn();
                        }
                    }
                }
                else
                {
                    if (state == BattleState.CHAR2TURN)
                    {
                        if (activeParty.activeParty[2] != null)
                        {
                            if (char3Ready)
                            {
                                DeactivateSkillsUI();
                                DeactivateDropsUI();
                                Char3Turn();
                            }
                        }
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                if (activeParty.activeParty[2] != null)
                {
                    if (state == BattleState.CHAR3TURN)
                    {
                        if (char2Ready)
                        {
                            DeactivateSkillsUI();
                            DeactivateDropsUI();
                            Char2Turn();
                        }
                        else
                        {
                            if (char1Ready)
                            {
                                DeactivateSkillsUI();
                                DeactivateDropsUI();
                                Char1Turn();
                            }
                        }
                    }
                    else
                    {
                        if (state == BattleState.CHAR2TURN)
                        {
                            if (char1Ready)
                            {
                                DeactivateSkillsUI();
                                DeactivateDropsUI();
                                Char1Turn();
                            }
                        }
                    }
                }
                else
                {
                    if (state == BattleState.CHAR2TURN)
                    {
                        if (char1Ready)
                        {
                            DeactivateSkillsUI();
                            DeactivateDropsUI();
                            Char1Turn();
                        }
                    }
                }
            }
        }
    }


    public void SetInBattleMenuTrue()
    {
        inBattleMenu = true;
    }

    public void SetInBattleMenuFalse()
    {
        inBattleMenu = false;
    }

    public void DisableBattleMenus()
    {
        Engine.e.battleSystem.enemyPanel.SetActive(true);
        inBattleMenu = false;

        DeactivateTargetSprite();
        DeactivateDropsUI();
        DeactivateSkillsUI();
        DeactivateBattleItems();
        DeactivateSupportButtons();
        DeactivateTargetButtons();

    }

    private void Update()
    {
        ChangeCharState();
        allyTargetButtons[0].transform.position = Engine.e.activeParty.transform.position;
        allyTargetButtons[1].transform.position = Engine.e.activePartyMember2.transform.position;
        allyTargetButtons[2].transform.position = Engine.e.activePartyMember3.transform.position;

        if (enemies[0] != null && enemies[0].GetComponent<Enemy>().health > 0)
        {
            enemyTargetButtons[0].transform.position = enemies[0].transform.position;
        }
        if (enemies[1] != null && enemies[1].GetComponent<Enemy>().health > 0)
        {
            enemyTargetButtons[1].transform.position = enemies[1].transform.position;
        }
        if (enemies[2] != null && enemies[2].GetComponent<Enemy>().health > 0)
        {
            enemyTargetButtons[2].transform.position = enemies[2].transform.position;
        }
        if (enemies[3] != null && enemies[3].GetComponent<Enemy>().health > 0)
        {
            enemyTargetButtons[3].transform.position = enemies[3].transform.position;
        }

        if (state == BattleState.CHAR1TURN)
        {
            dialogueText.text = activeParty.activeParty[0].GetComponent<Character>().characterName;
        }
        if (state == BattleState.CHAR2TURN)
        {
            dialogueText.text = activeParty.activeParty[1].GetComponent<Character>().characterName;
        }
        if (state == BattleState.CHAR3TURN)
        {
            dialogueText.text = activeParty.activeParty[2].GetComponent<Character>().characterName;
        }

        if (state == BattleState.ATBCHECK)
        {
            // if (currentInQueue == BattleState.QUEUECHECK || currentInQueue == BattleState.ATBCHECK)
            //  {
            dialogueText.text = string.Empty;
            // }
            if (currentInQueue == BattleState.CHAR1TURN || currentInQueue == BattleState.CONFCHAR1)
            {
                dialogueText.text = activeParty.activeParty[0].GetComponent<Character>().characterName;
            }
            if (currentInQueue == BattleState.CHAR2TURN || currentInQueue == BattleState.CONFCHAR2)
            {
                dialogueText.text = activeParty.activeParty[1].GetComponent<Character>().characterName;
            }
            if (currentInQueue == BattleState.CHAR3TURN || currentInQueue == BattleState.CONFCHAR3)
            {
                dialogueText.text = activeParty.activeParty[2].GetComponent<Character>().characterName;
            }

            if (currentInQueue == BattleState.ENEMY1TURN)
            {
                dialogueText.text = enemies[0].GetComponent<Enemy>().name;
            }
            if (currentInQueue == BattleState.ENEMY2TURN)
            {
                dialogueText.text = enemies[1].GetComponent<Enemy>().name;
            }
            if (currentInQueue == BattleState.ENEMY3TURN)
            {
                dialogueText.text = enemies[2].GetComponent<Enemy>().name;
            }
            if (currentInQueue == BattleState.ENEMY4TURN)
            {
                dialogueText.text = enemies[3].GetComponent<Enemy>().name;
            }
        }

        if (Engine.e.battleModeActive)
        {
            if (isDead && state != BattleState.LEVELUPCHECK && state != BattleState.LEVELUP && state != BattleState.WON && state != BattleState.LOST)
            {
                DeactivateTargetSprite();
                DeactivateChar1MenuButtons();

                if (activeParty.activeParty[1] != null)
                {
                    DeactivateChar2MenuButtons();
                }
                if (activeParty.activeParty[2] != null)
                {
                    DeactivateChar3MenuButtons();
                }


                if (activeParty.activeParty[2] != null)
                {
                    if (activeParty.activeParty[0].GetComponent<Character>().currentHealth <= 0 && activeParty.activeParty[1].GetComponent<Character>().currentHealth <= 0
                    && activeParty.activeParty[2].GetComponent<Character>().currentHealth <= 0)
                    {
                        state = BattleState.LOST;
                    }
                    else
                    {
                        state = BattleState.LEVELUPCHECK;
                    }
                }
                else
                {
                    if (activeParty.activeParty[1] != null)
                    {
                        if (activeParty.activeParty[0].GetComponent<Character>().currentHealth <= 0 && activeParty.activeParty[1].GetComponent<Character>().currentHealth <= 0)
                        {
                            state = BattleState.LOST;
                        }
                        else
                        {
                            state = BattleState.LEVELUPCHECK;
                        }
                    }
                    else
                    {
                        if (activeParty.activeParty[0].GetComponent<Character>().currentHealth <= 0)
                        {
                            state = BattleState.LOST;
                        }
                        else
                        {
                            state = BattleState.LEVELUPCHECK;

                        }
                    }
                }

                if (state == BattleState.LEVELUPCHECK)
                {
                    StartCoroutine(LevelUpCheck());
                }
                if (state == BattleState.LOST)
                {
                    StartCoroutine(EndBattle());
                }

            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach (BattleState state in battleQueue)
            {
                Debug.Log(state);
            }
        }

        if (currentInQueue != BattleState.ENEMY1TURN && currentInQueue != BattleState.ENEMY2TURN
        && currentInQueue != BattleState.ENEMY3TURN && currentInQueue != BattleState.ENEMY4TURN
        && currentInQueue != BattleState.CONFCHAR1 && currentInQueue != BattleState.CONFCHAR2
        && currentInQueue != BattleState.CONFCHAR3)
        {

            currentState = state;
        }

        // Do Something Like This (to make sure stats are up-to-date)

        // if ()

        if (!animExists)
        {
            if (Engine.e.battleSystem.hud.displayHealth[0].text != Engine.e.activeParty.activeParty[0].GetComponent<Character>().currentHealth.ToString())
            {
                Engine.e.battleSystem.hud.displayHealth[0].text = Engine.e.activeParty.activeParty[0].GetComponent<Character>().currentHealth.ToString();
            }
        }
        if (Engine.e.battleSystem.hud.displayMaxHealth[0].text != Engine.e.activeParty.activeParty[0].GetComponent<Character>().maxHealth.ToString())
        {
            Engine.e.battleSystem.hud.displayMaxHealth[0].text = Engine.e.activeParty.activeParty[0].GetComponent<Character>().maxHealth.ToString();
        }

        if (Engine.e.battleSystem.hud.displayMana[0].text != Engine.e.activeParty.activeParty[0].GetComponent<Character>().currentMana.ToString())
        {
            Engine.e.battleSystem.hud.displayMana[0].text = Engine.e.activeParty.activeParty[0].GetComponent<Character>().currentMana.ToString();
        }
        if (Engine.e.battleSystem.hud.displayMaxMana[0].text != Engine.e.activeParty.activeParty[0].GetComponent<Character>().maxMana.ToString())
        {
            Engine.e.battleSystem.hud.displayMaxMana[0].text = Engine.e.activeParty.activeParty[0].GetComponent<Character>().maxMana.ToString();
        }

        if (Engine.e.battleSystem.hud.displayEnergy[0].text != Engine.e.activeParty.activeParty[0].GetComponent<Character>().currentEnergy.ToString())
        {
            Engine.e.battleSystem.hud.displayEnergy[0].text = Engine.e.activeParty.activeParty[0].GetComponent<Character>().currentEnergy.ToString();
        }
        if (Engine.e.battleSystem.hud.displayMaxEnergy[0].text != Engine.e.activeParty.activeParty[0].GetComponent<Character>().maxEnergy.ToString())
        {
            Engine.e.battleSystem.hud.displayMaxEnergy[0].text = Engine.e.activeParty.activeParty[0].GetComponent<Character>().maxEnergy.ToString();
        }

        if (activeParty.activeParty[0].GetComponent<Character>().isAsleep || activeParty.activeParty[0].GetComponent<Character>().isConfused)
        {
            if (!activeParty.activeParty[0].GetComponent<Character>().inflicted)
            {
                activeParty.activeParty[0].GetComponent<Character>().inflicted = true;
            }

            if (activeParty.activeParty[0].GetComponent<SpriteRenderer>().color == Color.white && !animExists)
            {
                activeParty.activeParty[0].GetComponent<SpriteRenderer>().color = Color.grey;

            }
        }

        if (activeParty.activeParty[0].GetComponent<Character>().isPoisoned)
        {
            if (!activeParty.activeParty[0].GetComponent<Character>().inflicted)
            {
                activeParty.activeParty[0].GetComponent<Character>().inflicted = true;
            }

            if (activeParty.activeParty[0].GetComponent<SpriteRenderer>().color == Color.white && !animExists)
            {
                activeParty.activeParty[0].GetComponent<SpriteRenderer>().color = Color.green;
            }
        }

        if (!activeParty.activeParty[0].GetComponent<Character>().inflicted)
        {
            if (activeParty.activeParty[0].GetComponent<SpriteRenderer>().color != Color.white)
            {
                activeParty.activeParty[0].GetComponent<SpriteRenderer>().color = Color.white;
            }
        }

        if (activeParty.activeParty[1] != null)
        {
            if (!animExists)
            {
                if (Engine.e.battleSystem.hud.displayHealth[1].text != Engine.e.activeParty.activeParty[1].GetComponent<Character>().currentHealth.ToString())
                {
                    Engine.e.battleSystem.hud.displayHealth[1].text = Engine.e.activeParty.activeParty[1].GetComponent<Character>().currentHealth.ToString();
                }
                if (Engine.e.battleSystem.hud.displayMaxHealth[1].text != Engine.e.activeParty.activeParty[1].GetComponent<Character>().maxHealth.ToString())
                {
                    Engine.e.battleSystem.hud.displayMaxHealth[1].text = Engine.e.activeParty.activeParty[1].GetComponent<Character>().maxHealth.ToString();
                }
            }

            if (Engine.e.battleSystem.hud.displayMana[1].text != Engine.e.activeParty.activeParty[1].GetComponent<Character>().currentMana.ToString())
            {
                Engine.e.battleSystem.hud.displayMana[1].text = Engine.e.activeParty.activeParty[1].GetComponent<Character>().currentMana.ToString();
            }
            if (Engine.e.battleSystem.hud.displayMaxMana[1].text != Engine.e.activeParty.activeParty[1].GetComponent<Character>().maxMana.ToString())
            {
                Engine.e.battleSystem.hud.displayMaxMana[1].text = Engine.e.activeParty.activeParty[1].GetComponent<Character>().maxMana.ToString();
            }

            if (Engine.e.battleSystem.hud.displayEnergy[1].text != Engine.e.activeParty.activeParty[1].GetComponent<Character>().currentEnergy.ToString())
            {
                Engine.e.battleSystem.hud.displayEnergy[1].text = Engine.e.activeParty.activeParty[1].GetComponent<Character>().currentEnergy.ToString();
            }
            if (Engine.e.battleSystem.hud.displayMaxEnergy[1].text != Engine.e.activeParty.activeParty[1].GetComponent<Character>().maxEnergy.ToString())
            {
                Engine.e.battleSystem.hud.displayMaxEnergy[1].text = Engine.e.activeParty.activeParty[1].GetComponent<Character>().maxEnergy.ToString();
            }

            if (activeParty.activeParty[1].GetComponent<Character>().isAsleep || activeParty.activeParty[1].GetComponent<Character>().isConfused)
            {
                if (!activeParty.activeParty[1].GetComponent<Character>().inflicted)
                {
                    activeParty.activeParty[1].GetComponent<Character>().inflicted = true;
                }

                if (activeParty.activeParty[1].GetComponent<SpriteRenderer>().color == Color.white && !animExists)
                {
                    activeParty.activeParty[1].GetComponent<SpriteRenderer>().color = Color.grey;
                }
            }

            if (activeParty.activeParty[1].GetComponent<Character>().isPoisoned)
            {
                if (!activeParty.activeParty[1].GetComponent<Character>().inflicted)
                {
                    activeParty.activeParty[1].GetComponent<Character>().inflicted = true;
                }

                if (activeParty.activeParty[1].GetComponent<SpriteRenderer>().color == Color.white && !animExists)
                {
                    activeParty.activeParty[1].GetComponent<SpriteRenderer>().color = Color.green;
                }
            }

            if (!activeParty.activeParty[1].GetComponent<Character>().inflicted)
            {
                if (activeParty.activeParty[1].GetComponent<SpriteRenderer>().color != Color.white)
                {
                    activeParty.activeParty[1].GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
        }


        if (activeParty.activeParty[2] != null)
        {
            if (!animExists)
            {
                if (Engine.e.battleSystem.hud.displayHealth[2].text != Engine.e.activeParty.activeParty[2].GetComponent<Character>().currentHealth.ToString())
                {
                    Engine.e.battleSystem.hud.displayHealth[2].text = Engine.e.activeParty.activeParty[2].GetComponent<Character>().currentHealth.ToString();
                }
                if (Engine.e.battleSystem.hud.displayMaxHealth[2].text != Engine.e.activeParty.activeParty[2].GetComponent<Character>().maxHealth.ToString())
                {
                    Engine.e.battleSystem.hud.displayMaxHealth[2].text = Engine.e.activeParty.activeParty[2].GetComponent<Character>().maxHealth.ToString();
                }
            }
            if (Engine.e.battleSystem.hud.displayMana[2].text != Engine.e.activeParty.activeParty[2].GetComponent<Character>().currentHealth.ToString())
            {
                Engine.e.battleSystem.hud.displayMana[2].text = Engine.e.activeParty.activeParty[2].GetComponent<Character>().currentMana.ToString();
            }
            if (Engine.e.battleSystem.hud.displayMaxMana[2].text != Engine.e.activeParty.activeParty[2].GetComponent<Character>().maxMana.ToString())
            {
                Engine.e.battleSystem.hud.displayMaxMana[2].text = Engine.e.activeParty.activeParty[2].GetComponent<Character>().maxMana.ToString();
            }

            if (Engine.e.battleSystem.hud.displayEnergy[2].text != Engine.e.activeParty.activeParty[2].GetComponent<Character>().currentHealth.ToString())
            {
                Engine.e.battleSystem.hud.displayEnergy[2].text = Engine.e.activeParty.activeParty[2].GetComponent<Character>().currentEnergy.ToString();
            }
            if (Engine.e.battleSystem.hud.displayMaxEnergy[2].text != Engine.e.activeParty.activeParty[2].GetComponent<Character>().maxEnergy.ToString())
            {
                Engine.e.battleSystem.hud.displayMaxEnergy[2].text = Engine.e.activeParty.activeParty[2].GetComponent<Character>().maxEnergy.ToString();
            }

            if (activeParty.activeParty[2].GetComponent<Character>().isAsleep || activeParty.activeParty[2].GetComponent<Character>().isConfused)
            {
                if (!activeParty.activeParty[2].GetComponent<Character>().inflicted)
                {
                    activeParty.activeParty[2].GetComponent<Character>().inflicted = true;
                }

                if (activeParty.activeParty[2].GetComponent<SpriteRenderer>().color == Color.white && !animExists)
                {
                    activeParty.activeParty[2].GetComponent<SpriteRenderer>().color = Color.grey;
                }
            }

            if (activeParty.activeParty[2].GetComponent<Character>().isPoisoned)
            {
                if (!activeParty.activeParty[2].GetComponent<Character>().inflicted)
                {
                    activeParty.activeParty[2].GetComponent<Character>().inflicted = true;
                }

                if (activeParty.activeParty[2].GetComponent<SpriteRenderer>().color == Color.white && !animExists)
                {
                    activeParty.activeParty[2].GetComponent<SpriteRenderer>().color = Color.green;
                }
            }

            if (!activeParty.activeParty[2].GetComponent<Character>().inflicted)
            {
                if (activeParty.activeParty[2].GetComponent<SpriteRenderer>().color != Color.white)
                {
                    activeParty.activeParty[2].GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
        }


        Engine.e.activeParty.GetComponent<SpriteRenderer>().color = activeParty.activeParty[0].GetComponent<SpriteRenderer>().color;

        if (activeParty.activeParty[1] != null)
        {
            Engine.e.activePartyMember2.GetComponent<SpriteRenderer>().color = activeParty.activeParty[1].GetComponent<SpriteRenderer>().color;
        }
        if (activeParty.activeParty[2] != null)
        {
            Engine.e.activePartyMember3.GetComponent<SpriteRenderer>().color = activeParty.activeParty[2].GetComponent<SpriteRenderer>().color;
        }

        if (!animExists)
        {
            if (Engine.e.battleSystem.hud.displayEnemyHealth[0].text != enemies[0].GetComponent<Enemy>().health.ToString())
            {
                Engine.e.battleSystem.hud.displayEnemyHealth[0].text = enemies[0].GetComponent<Enemy>().health.ToString();
            }
        }


        if (enemies[0].GetComponent<Enemy>().isAsleep || enemies[0].GetComponent<Enemy>().isConfused)
        {
            if (!enemies[0].GetComponent<Enemy>().inflicted)
            {
                enemies[0].GetComponent<Enemy>().inflicted = true;
            }

            if (enemies[0].GetComponent<SpriteRenderer>().color == Color.white && !animExists)
            {
                enemies[0].GetComponent<SpriteRenderer>().color = Color.grey;
            }
        }

        if (enemies[0].GetComponent<Enemy>().isPoisoned)
        {
            if (!enemies[0].GetComponent<Enemy>().inflicted)
            {
                enemies[0].GetComponent<Enemy>().inflicted = true;
            }

            if (enemies[0].GetComponent<SpriteRenderer>().color == Color.white && !animExists)
            {
                enemies[0].GetComponent<SpriteRenderer>().color = Color.green;
            }
        }

        if (!enemies[0].GetComponent<Enemy>().inflicted)
        {
            if (enemies[0].GetComponent<SpriteRenderer>().color != Color.white)
            {
                enemies[0].GetComponent<SpriteRenderer>().color = Color.white;
            }
        }

        if (enemies[1] != null)
        {
            if (!animExists)
            {
                if (Engine.e.battleSystem.hud.displayEnemyHealth[1].text != enemies[1].GetComponent<Enemy>().health.ToString())
                {
                    Engine.e.battleSystem.hud.displayEnemyHealth[1].text = enemies[1].GetComponent<Enemy>().health.ToString();
                }
            }


            if (enemies[1].GetComponent<Enemy>().isAsleep || enemies[1].GetComponent<Enemy>().isConfused)
            {
                if (!enemies[1].GetComponent<Enemy>().inflicted)
                {
                    enemies[1].GetComponent<Enemy>().inflicted = true;
                }

                if (enemies[1].GetComponent<SpriteRenderer>().color == Color.white && !animExists)
                {
                    enemies[1].GetComponent<SpriteRenderer>().color = Color.grey;
                }
            }

            if (enemies[1].GetComponent<Enemy>().isPoisoned)
            {
                if (!enemies[1].GetComponent<Enemy>().inflicted)
                {
                    enemies[1].GetComponent<Enemy>().inflicted = true;
                }

                if (enemies[1].GetComponent<SpriteRenderer>().color == Color.white && !animExists)
                {
                    enemies[1].GetComponent<SpriteRenderer>().color = Color.green;
                }
            }

            if (!enemies[1].GetComponent<Enemy>().inflicted)
            {
                if (enemies[1].GetComponent<SpriteRenderer>().color != Color.white)
                {
                    enemies[1].GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
        }

        if (enemies[2] != null)
        {
            if (!animExists)
            {
                if (Engine.e.battleSystem.hud.displayEnemyHealth[2].text != enemies[2].GetComponent<Enemy>().health.ToString())
                {
                    Engine.e.battleSystem.hud.displayEnemyHealth[2].text = enemies[2].GetComponent<Enemy>().health.ToString();
                }
            }


            if (enemies[2].GetComponent<Enemy>().isAsleep || enemies[2].GetComponent<Enemy>().isConfused)
            {
                if (!enemies[2].GetComponent<Enemy>().inflicted)
                {
                    enemies[2].GetComponent<Enemy>().inflicted = true;
                }

                if (enemies[2].GetComponent<SpriteRenderer>().color == Color.white && !animExists)
                {
                    enemies[2].GetComponent<SpriteRenderer>().color = Color.grey;
                }
            }

            if (enemies[2].GetComponent<Enemy>().isPoisoned)
            {
                if (!enemies[2].GetComponent<Enemy>().inflicted)
                {
                    enemies[2].GetComponent<Enemy>().inflicted = true;
                }

                if (enemies[2].GetComponent<SpriteRenderer>().color == Color.white && !animExists)
                {
                    enemies[2].GetComponent<SpriteRenderer>().color = Color.green;
                }
            }

            if (!enemies[2].GetComponent<Enemy>().inflicted)
            {
                if (enemies[2].GetComponent<SpriteRenderer>().color != Color.white)
                {
                    enemies[2].GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
        }

        if (enemies[3] != null)
        {
            if (!animExists)
            {
                if (Engine.e.battleSystem.hud.displayEnemyHealth[3].text != enemies[3].GetComponent<Enemy>().health.ToString())
                {
                    Engine.e.battleSystem.hud.displayEnemyHealth[3].text = enemies[3].GetComponent<Enemy>().health.ToString();
                }
            }


            if (enemies[3].GetComponent<Enemy>().isAsleep || enemies[3].GetComponent<Enemy>().isConfused)
            {
                if (!enemies[3].GetComponent<Enemy>().inflicted)
                {
                    enemies[3].GetComponent<Enemy>().inflicted = true;
                }

                if (enemies[3].GetComponent<SpriteRenderer>().color == Color.white && !animExists)
                {
                    enemies[3].GetComponent<SpriteRenderer>().color = Color.grey;
                }
            }

            if (enemies[3].GetComponent<Enemy>().isPoisoned)
            {
                if (!enemies[3].GetComponent<Enemy>().inflicted)
                {
                    enemies[3].GetComponent<Enemy>().inflicted = true;
                }

                if (enemies[3].GetComponent<SpriteRenderer>().color == Color.white && !animExists)
                {
                    enemies[3].GetComponent<SpriteRenderer>().color = Color.green;
                }
            }

            if (!enemies[3].GetComponent<Enemy>().inflicted)
            {
                if (enemies[3].GetComponent<SpriteRenderer>().color != Color.white)
                {
                    enemies[3].GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
        }

        if (!isDead)
        {
            if (!Engine.e.battleModeActive)
            {
                if (state == BattleState.ATBCHECK)
                {
                    dialogueText.text = string.Empty;
                    if (activeParty.activeParty[0].GetComponent<Character>().currentHealth > 0)
                    {
                        if (char1ATB < ATBReady)
                        {
                            //char1ATB += (activeParty.activeParty[0].GetComponent<Character>().haste * 10) / 50;
                            char1ATB += activeParty.activeParty[0].GetComponent<Character>().haste * Time.deltaTime;
                            char1ATBGuage.value = char1ATB;
                        }
                        else
                        {
                            state = BattleState.CHAR1TURN;
                            Char1Turn();
                        }
                    }

                    if (activeParty.activeParty[1] != null)
                    {
                        if (activeParty.activeParty[1].GetComponent<Character>().currentHealth > 0)
                        {
                            if (char2ATB < ATBReady)
                            {
                                //char2ATB += (activeParty.activeParty[1].GetComponent<Character>().haste * 10) / 50;

                                char2ATB += activeParty.activeParty[1].GetComponent<Character>().haste * Time.deltaTime;
                                char2ATBGuage.value = char2ATB;
                            }
                            else
                            {
                                state = BattleState.CHAR2TURN;
                                Char2Turn();
                            }
                        }
                    }

                    if (activeParty.activeParty[2] != null)
                    {
                        if (activeParty.activeParty[2].GetComponent<Character>().currentHealth > 0)
                        {
                            if (char3ATB < ATBReady)
                            {
                                char3ATB += activeParty.activeParty[2].GetComponent<Character>().haste * Time.deltaTime;
                                char3ATBGuage.value = char3ATB;
                            }
                            else
                            {
                                state = BattleState.CHAR3TURN;
                                Char3Turn();
                            }
                        }
                    }

                    if (enemies[0].GetComponent<Enemy>().health > 0)
                    {
                        if (enemy1ATB < ATBReady)
                        {
                            enemy1ATB += (enemies[0].GetComponent<Enemy>().haste * Time.deltaTime);
                            enemy1ATBGuage.value = enemy1ATB;
                        }
                        else
                        {
                            Enemy1Turn();
                        }
                    }

                    if (enemies[1] != null)
                    {
                        if (enemies[1].GetComponent<Enemy>().health > 0)
                        {
                            if (enemy2ATB < ATBReady)
                            {
                                enemy2ATB += (enemies[1].GetComponent<Enemy>().haste * Time.deltaTime);
                                enemy2ATBGuage.value = enemy2ATB;

                            }
                            else
                            {
                                Enemy2Turn();
                            }
                        }
                    }

                    if (enemies[2] != null)
                    {
                        if (enemies[2].GetComponent<Enemy>().health > 0)
                        {
                            if (enemy3ATB < ATBReady)
                            {
                                enemy3ATB += (enemies[2].GetComponent<Enemy>().haste * Time.deltaTime);
                                enemy3ATBGuage.value = enemy3ATB;
                            }
                            else
                            {
                                Enemy3Turn();
                            }
                        }
                    }

                    if (enemies[3] != null)
                    {
                        if (enemies[3].GetComponent<Enemy>().health > 0)
                        {
                            if (enemy4ATB < ATBReady)
                            {
                                enemy4ATB += (enemies[3].GetComponent<Enemy>().haste * Time.deltaTime);
                                enemy4ATBGuage.value = enemy4ATB;
                            }
                            else
                            {
                                Enemy4Turn();
                            }
                        }
                    }
                }
            }
            else
            {
                HandleQueue();
            }
        }


        if (Engine.e.partyInventoryReference.battleScreenInventorySet)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                PressDown();
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                ReleaseDown();
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                PressUp();
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                ReleaseUp();
            }
            if (pressDown && !pressUp)
            {
                vertMove = 1;
            }
            if (!pressDown && pressUp)
            {
                vertMove = -1;
            }

            HandleInventory();
        }
    }

    void HandleInventory()
    {
        // "Battle Items Menu" Inventory
        if (Engine.e.partyInventoryReference.battleScreenInventorySet)
        {
            if (vertMove > 0 && pressDown)
            {
                pressDown = false;
                if (inventoryPointerIndex < Engine.e.battleSystem.battleItems.Length)
                {
                    inventoryPointerIndex += vertMove;

                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(battleItems[inventoryPointerIndex].gameObject);

                    if (inventoryPointerIndex > 2 && inventoryPointerIndex < Engine.e.battleSystem.battleItems.Length)
                    {
                        Engine.e.partyInventoryReference.battleItemsRectTransform.offsetMax -= new Vector2(0, -30);
                    }
                }
            }

            if (vertMove < 0 && pressUp)
            {
                pressUp = false;
                if (inventoryPointerIndex > 0)
                {
                    inventoryPointerIndex += vertMove;

                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(battleItems[inventoryPointerIndex].gameObject);


                    if (inventoryPointerIndex >= 2 && inventoryPointerIndex > 0)
                    {
                        Engine.e.partyInventoryReference.battleItemsRectTransform.offsetMax -= new Vector2(0, 30);
                    }
                }
            }
        }
    }

    void PressDown()
    {
        pressDown = true;
        vertMove = 1;
    }
    void ReleaseDown()
    {
        pressDown = false;
        vertMove = 0;
    }
    void PressUp()
    {
        pressUp = true;
        vertMove = -1;
    }
    void ReleaseUp()
    {
        pressUp = false;
        vertMove = 0;
    }

    private void FixedUpdate()
    {

        if (charMoving == true)
        {

            if (currentInQueue == BattleState.CHAR1TURN || currentInQueue == BattleState.CONFCHAR1)
            {
                currentMoveIndex = 0;
            }
            if (currentInQueue == BattleState.CHAR2TURN || currentInQueue == BattleState.CONFCHAR2)
            {
                currentMoveIndex = 1;

            }
            if (currentInQueue == BattleState.CHAR3TURN || currentInQueue == BattleState.CONFCHAR3)
            {
                currentMoveIndex = 2;

            }

            if (!Engine.e.activeParty.activeParty[currentMoveIndex].GetComponent<Character>().isConfused)
            {
                StartCoroutine(AttackMoveToTargetChar());
            }
            else
            {
                if (!attackingTeam)
                {
                    StartCoroutine(AttackMoveToTargetChar());
                }
                else
                {
                    StartCoroutine(ConfuseAttackMoveToTargetChar());

                }
            }
        }

        if (charAttackDrop == true)
        {
            StartCoroutine(CharDropAttack());
        }

        if (enemyMoving == true)
        {

            if (currentInQueue == BattleState.ENEMY1TURN)
            {
                currentMoveIndex = 0;
            }
            if (currentInQueue == BattleState.ENEMY2TURN)
            {
                currentMoveIndex = 1;

            }
            if (currentInQueue == BattleState.ENEMY3TURN)
            {
                currentMoveIndex = 2;
            }
            if (currentInQueue == BattleState.ENEMY4TURN)
            {
                currentMoveIndex = 3;
            }

            if (!enemies[currentMoveIndex].GetComponent<Enemy>().isConfused)
            {
                StartCoroutine(AttackMoveToTargetEnemy());
            }
            else
            {
                if (!attackingTeam)
                {
                    StartCoroutine(AttackMoveToTargetEnemy());
                }
                else
                {
                    StartCoroutine(ConfuseAttackMoveToTargetEnemy());

                }
            }
        }

        if (enemyAttackDrop == true)
        {
            StartCoroutine(EnemyDropAttack());
        }
    }
}