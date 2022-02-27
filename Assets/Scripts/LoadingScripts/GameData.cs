using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public static GameData gameData;
    public string[] charNames;
    public float[] charMaxHealth;
    public float[] charCurrentHealth;
    public float[] charMaxMana;
    public float[] charCurrentMana;
    public int[] charLvl;
    public float[] charXP;
    public float[] charLvlUpReq;
    public float[] charBaseDamage;
    public float[] charPhysicalDamage;
    public float[] charSkillScale;
    public int[] charSkillIndex;
    public int[] charAvailableSkillPoints;
    public float[] charStealChance;
    public float[] charHaste;

    public float[] charDodgeChance;
    public float[] charPhysicalDefense;
    public float[] charFireDefense;
    public float[] charIceDefense;
    public float[] charWaterDefense;
    public float[] charLightningDefense;
    public float[] charShadowDefense;
    public float[] charPoisonDefense;
    public float[] charSleepDefense;
    public float[] charConfuseDefense;



    public string[,] charDrops, charSkills;


    public float[] charFireDropLevel, charWaterDropLevel, charLightningDropLevel, charShadowDropLevel, charIceDropLevel, charHolyDropLevel;
    public float[] charFireDropExperience, charWaterDropExperience, charLightningDropExperience, charShadowDropExperience, charIceDropExperience, charHolyDropExperience;
    public float[] charFireDropLvlReq, charWaterDropLvlReq, charLightningDropLvlReq, charShadowDropLvlReq, charIceDropLvlReq, charHolyDropLvlReq;
    public float[] charFireDropAttackBonus, charWaterDropAttackBonus, charLightningDropAttackBonus, charShadowDropAttackBonus, charIceDropAttackBonus;
    public float[] charFirePhysicalAttackBonus, charWaterPhysicalAttackBonus, charLightningPhysicalAttackBonus, charShadowPhysicalAttackBonus, charIcePhysicalAttackBonus;

    public string[] weapons;
    public string grieveWeaponRightEquip, grieveWeaponLeftEquip;
    public string macWeaponRightEquip, macWeaponLeftEquip;
    public string fieldWeaponRightEquip, fieldWeaponLeftEquip;
    public string riggsWeaponRightEquip, riggsWeaponLeftEquip;

    public string[] charChestArmorEquip;
    public string[] charAccessory1Equip, charAccessory2Equip;

    public bool[] canUse2HWeapon, canDualWield;
    public bool[] charInParty;
    public string[] activeParty;
    public bool arrangePartyButtonActive;

    public string[] partyInvNames;
    public int[] partyInvAmounts;
    public int partySize;
    public string[] partyArmor;
    public float[] partyPosition;
    public int partyMoney;

    public string[] partyQuests;
    public int[,] partyQuestObjectiveCount;

    public string[] completedQuests;
    public bool[] questTurnedIn;
    public string scene;
    public float time;
    public bool battleModeActive;
    public bool aboveLayer;
    public bool inWorldMap;
    public bool recentAutoSave;
    public string whichLayer;
    public bool[] grieveNodes, macNodes, fieldNodes, riggsNodes;
    public int[] charNodePositions;
    public GameData(Engine gameManager)
    {
        charNames = new string[gameManager.party.Length];
        charMaxHealth = new float[gameManager.party.Length];
        charMaxMana = new float[gameManager.party.Length];
        charLvl = new int[gameManager.party.Length];

        charFireDropLevel = new float[gameManager.party.Length];
        charWaterDropLevel = new float[gameManager.party.Length];
        charLightningDropLevel = new float[gameManager.party.Length];
        charShadowDropLevel = new float[gameManager.party.Length];
        charIceDropLevel = new float[gameManager.party.Length];
        charHolyDropLevel = new float[gameManager.party.Length];

        charFireDropExperience = new float[gameManager.party.Length];
        charWaterDropExperience = new float[gameManager.party.Length];
        charLightningDropExperience = new float[gameManager.party.Length];
        charShadowDropExperience = new float[gameManager.party.Length];
        charIceDropExperience = new float[gameManager.party.Length];
        charHolyDropExperience = new float[gameManager.party.Length];

        charFireDropLvlReq = new float[gameManager.party.Length];
        charWaterDropLvlReq = new float[gameManager.party.Length];
        charLightningDropLvlReq = new float[gameManager.party.Length];
        charShadowDropLvlReq = new float[gameManager.party.Length];
        charIceDropLvlReq = new float[gameManager.party.Length];
        charHolyDropLvlReq = new float[gameManager.party.Length];

        charFireDropAttackBonus = new float[gameManager.party.Length];
        charWaterDropAttackBonus = new float[gameManager.party.Length];
        charLightningDropAttackBonus = new float[gameManager.party.Length];
        charShadowDropAttackBonus = new float[gameManager.party.Length];
        charIceDropAttackBonus = new float[gameManager.party.Length];

        charFirePhysicalAttackBonus = new float[gameManager.party.Length];
        charWaterPhysicalAttackBonus = new float[gameManager.party.Length];
        charLightningPhysicalAttackBonus = new float[gameManager.party.Length];
        charShadowPhysicalAttackBonus = new float[gameManager.party.Length];
        charIcePhysicalAttackBonus = new float[gameManager.party.Length];

        charBaseDamage = new float[gameManager.party.Length];
        charPhysicalDamage = new float[gameManager.party.Length];
        charXP = new float[gameManager.party.Length];
        charLvlUpReq = new float[gameManager.party.Length];
        charInParty = new bool[gameManager.party.Length];
        charDodgeChance = new float[gameManager.party.Length];
        charPhysicalDefense = new float[gameManager.party.Length];
        charFireDefense = new float[gameManager.party.Length];
        charWaterDefense = new float[gameManager.party.Length];
        charLightningDefense = new float[gameManager.party.Length];
        charShadowDefense = new float[gameManager.party.Length];
        charIceDefense = new float[gameManager.party.Length];
        charPoisonDefense = new float[gameManager.party.Length];
        charSleepDefense = new float[gameManager.party.Length];
        charConfuseDefense = new float[gameManager.party.Length];
        charHaste = new float[gameManager.party.Length];

        charDrops = new string[Engine.e.party.Length, Engine.e.gameDrops.Length];
        charSkills = new string[Engine.e.party.Length, Engine.e.gameSkills.Length];

        /*charFireDrops = new string[gameManager.party.Length, gameManager.fireDrops.Length];
        charHolyDrops = new string[gameManager.party.Length, gameManager.holyDrops.Length];
        charWaterDrops = new string[gameManager.party.Length, gameManager.waterDrops.Length];
        charLightningDrops = new string[gameManager.party.Length, gameManager.lightningDrops.Length];
        charShadowDrops = new string[gameManager.party.Length, gameManager.shadowDrops.Length];
        charIceDrops = new string[gameManager.party.Length, gameManager.iceDrops.Length];
*/
        charSkillScale = new float[gameManager.party.Length];
        charSkillIndex = new int[gameManager.party.Length];
        charAvailableSkillPoints = new int[gameManager.party.Length];
        charStealChance = new float[gameManager.party.Length];

        canUse2HWeapon = new bool[Engine.e.party.Length];
        canDualWield = new bool[Engine.e.party.Length];

        battleModeActive = gameManager.battleModeActive;
        arrangePartyButtonActive = gameManager.arrangePartyButtonActive;


        for (int i = 0; i < gameManager.party.Length; i++)
        {
            if (gameManager.party[i] != null)
            {
                charNames[i] = gameManager.party[i].GetComponent<Character>().characterName;
                charMaxHealth[i] = gameManager.party[i].GetComponent<Character>().maxHealth;
                charMaxMana[i] = gameManager.party[i].GetComponent<Character>().maxMana;
                charLvl[i] = gameManager.party[i].GetComponent<Character>().lvl;

                charFireDropLevel[i] = gameManager.party[i].GetComponent<Character>().fireDropsLevel;
                charWaterDropLevel[i] = gameManager.party[i].GetComponent<Character>().waterDropsLevel;
                charLightningDropLevel[i] = gameManager.party[i].GetComponent<Character>().lightningDropsLevel;
                charShadowDropLevel[i] = gameManager.party[i].GetComponent<Character>().shadowDropsLevel;
                charIceDropLevel[i] = gameManager.party[i].GetComponent<Character>().iceDropsLevel;
                charHolyDropLevel[i] = gameManager.party[i].GetComponent<Character>().holyDropsLevel;

                charFireDropExperience[i] = gameManager.party[i].GetComponent<Character>().fireDropsExperience;
                charWaterDropExperience[i] = gameManager.party[i].GetComponent<Character>().waterDropsExperience;
                charLightningDropExperience[i] = gameManager.party[i].GetComponent<Character>().lightningDropsExperience;
                charShadowDropExperience[i] = gameManager.party[i].GetComponent<Character>().shadowDropsExperience;
                charIceDropExperience[i] = gameManager.party[i].GetComponent<Character>().iceDropsExperience;
                charHolyDropExperience[i] = gameManager.party[i].GetComponent<Character>().holyDropsExperience;

                charFireDropLvlReq[i] = gameManager.party[i].GetComponent<Character>().fireDropsLvlReq;
                charWaterDropLvlReq[i] = gameManager.party[i].GetComponent<Character>().waterDropsLvlReq;
                charLightningDropLvlReq[i] = gameManager.party[i].GetComponent<Character>().lightningDropsLvlReq;
                charShadowDropLvlReq[i] = gameManager.party[i].GetComponent<Character>().shadowDropsLvlReq;
                charIceDropLvlReq[i] = gameManager.party[i].GetComponent<Character>().iceDropsLvlReq;
                charHolyDropLvlReq[i] = gameManager.party[i].GetComponent<Character>().holyDropsLvlReq;

                charFireDropAttackBonus[i] = gameManager.party[i].GetComponent<Character>().fireDropAttackBonus;
                charWaterDropAttackBonus[i] = gameManager.party[i].GetComponent<Character>().waterDropAttackBonus;
                charLightningDropAttackBonus[i] = gameManager.party[i].GetComponent<Character>().lightningDropAttackBonus;
                charShadowDropAttackBonus[i] = gameManager.party[i].GetComponent<Character>().shadowDropAttackBonus;
                charIceDropAttackBonus[i] = gameManager.party[i].GetComponent<Character>().iceDropAttackBonus;

                charFirePhysicalAttackBonus[i] = gameManager.party[i].GetComponent<Character>().firePhysicalAttackBonus;
                charWaterPhysicalAttackBonus[i] = gameManager.party[i].GetComponent<Character>().waterPhysicalAttackBonus;
                charLightningPhysicalAttackBonus[i] = gameManager.party[i].GetComponent<Character>().lightningPhysicalAttackBonus;
                charShadowPhysicalAttackBonus[i] = gameManager.party[i].GetComponent<Character>().shadowPhysicalAttackBonus;
                charIcePhysicalAttackBonus[i] = gameManager.party[i].GetComponent<Character>().icePhysicalAttackBonus;

                canUse2HWeapon[i] = Engine.e.party[i].GetComponent<Character>().canUse2HWeapon;
                canDualWield[i] = Engine.e.party[i].GetComponent<Character>().canDualWield;

                charPhysicalDamage[i] = gameManager.party[i].GetComponent<Character>().strength;
                charXP[i] = gameManager.party[i].GetComponent<Character>().experiencePoints;
                charLvlUpReq[i] = gameManager.party[i].GetComponent<Character>().levelUpReq;
                charInParty[i] = gameManager.party[i].GetComponent<Character>().isInParty;
                charDodgeChance[i] = gameManager.party[i].GetComponent<Character>().dodgeChance;
                charPhysicalDefense[i] = gameManager.party[i].GetComponent<Character>().physicalDefense;
                charFireDefense[i] = gameManager.party[i].GetComponent<Character>().fireDefense;
                charWaterDefense[i] = gameManager.party[i].GetComponent<Character>().waterDefense;
                charLightningDefense[i] = gameManager.party[i].GetComponent<Character>().lightningDefense;
                charShadowDefense[i] = gameManager.party[i].GetComponent<Character>().shadowDefense;
                charIceDefense[i] = gameManager.party[i].GetComponent<Character>().iceDefense;
                charPoisonDefense[i] = gameManager.party[i].GetComponent<Character>().poisonDefense;
                charSleepDefense[i] = gameManager.party[i].GetComponent<Character>().sleepDefense;
                charConfuseDefense[i] = gameManager.party[i].GetComponent<Character>().confuseDefense;

                charSkillScale[i] = gameManager.party[i].GetComponent<Character>().skillScale;
                charSkillIndex[i] = gameManager.party[i].GetComponent<Character>().skillIndex;
                charAvailableSkillPoints[i] = gameManager.party[i].GetComponent<Character>().availableSkillPoints;
                charStealChance[i] = gameManager.party[i].GetComponent<Character>().stealChance;
                charHaste[i] = gameManager.party[i].GetComponent<Character>().haste;

                for (int f = 0; f < Engine.e.party[i].GetComponent<Character>().drops.Length; f++)
                {
                    if (Engine.e.party[i].GetComponent<Character>().drops[f] != null)
                    {
                        if (Engine.e.party[i].GetComponent<Character>().drops[f] = Engine.e.gameDrops[f])
                        {
                            charDrops[i, f] = Engine.e.gameDrops[f].dropName;
                        }
                    }
                }

                for (int f = 0; f < Engine.e.party[i].GetComponent<Character>().skills.Length; f++)
                {
                    if (Engine.e.party[i].GetComponent<Character>().skills[f] != null)
                    {
                        if (Engine.e.party[i].GetComponent<Character>().skills[f] = Engine.e.gameSkills[f])
                        {
                            charSkills[i, f] = Engine.e.gameSkills[f].skillName;
                        }
                    }
                }
                /*  // Fire Drops
                  for (int f = 0; f < gameManager.party[i].GetComponent<Character>().fireDrops.Length; f++)
                  {
                      if (gameManager.party[i].GetComponent<Character>().fireDrops[f] != null)
                      {
                          if (gameManager.party[i].GetComponent<Character>().fireDrops[f] = gameManager.fireDrops[f])
                          {
                              charFireDrops[i, f] = gameManager.fireDrops[f].dropName;
                              fireDropsisKnown[f] = true;
                          }
                      }
                  }
                  // Holy Drops
                  for (int f = 0; f < gameManager.party[i].GetComponent<Character>().holyDrops.Length; f++)
                  {
                      if (gameManager.party[i].GetComponent<Character>().holyDrops[f] != null)
                      {
                          if (gameManager.party[i].GetComponent<Character>().holyDrops[f] = gameManager.holyDrops[f])
                          {
                              charHolyDrops[i, f] = gameManager.holyDrops[f].dropName;
                              holyDropsisKnown[f] = true;

                          }
                      }
                  }
                  // Water Drops
                  for (int f = 0; f < gameManager.party[i].GetComponent<Character>().waterDrops.Length; f++)
                  {
                      if (gameManager.party[i].GetComponent<Character>().waterDrops[f] != null)
                      {
                          if (gameManager.party[i].GetComponent<Character>().waterDrops[f] = gameManager.waterDrops[f])
                          {
                              charWaterDrops[i, f] = gameManager.waterDrops[f].dropName;
                              waterDropsisKnown[f] = true;

                          }
                      }
                  }
                  // Lightning Drops
                  for (int f = 0; f < gameManager.party[i].GetComponent<Character>().lightningDrops.Length; f++)
                  {
                      if (gameManager.party[i].GetComponent<Character>().lightningDrops[f] != null)
                      {
                          if (gameManager.party[i].GetComponent<Character>().lightningDrops[f] = gameManager.lightningDrops[f])
                          {
                              charLightningDrops[i, f] = gameManager.lightningDrops[f].dropName;
                              lightningDropsisKnown[f] = true;

                          }
                      }
                  }
                  // Shadow Drops
                  for (int f = 0; f < gameManager.party[i].GetComponent<Character>().shadowDrops.Length; f++)
                  {
                      if (gameManager.party[i].GetComponent<Character>().shadowDrops[f] != null)
                      {
                          if (gameManager.party[i].GetComponent<Character>().shadowDrops[f] = gameManager.shadowDrops[f])
                          {
                              charShadowDrops[i, f] = gameManager.shadowDrops[f].dropName;
                              shadowDropsisKnown[f] = true;

                          }
                      }
                  }
                  // Ice Drops
                  for (int f = 0; f < gameManager.party[i].GetComponent<Character>().iceDrops.Length; f++)
                  {
                      if (gameManager.party[i].GetComponent<Character>().iceDrops[f] != null)
                      {
                          if (gameManager.party[i].GetComponent<Character>().iceDrops[f] = gameManager.iceDrops[f])
                          {
                              charIceDrops[i, f] = gameManager.iceDrops[f].dropName;
                              iceDropsisKnown[f] = true;

                          }
                      }
                  }
  */
                partySize++;
            }
        }

        partyInvNames = new string[gameManager.partyInventoryReference.partyInventory.Length];
        partyInvAmounts = new int[gameManager.partyInventoryReference.partyInventory.Length];

        weapons = new string[gameManager.partyInventoryReference.weaponInventorySlots.Length];

        charChestArmorEquip = new string[Engine.e.party.Length];
        charAccessory1Equip = new string[Engine.e.party.Length];
        charAccessory2Equip = new string[Engine.e.party.Length];

        // Items / Weapons / Armor
        for (int i = 0; i < partyInvNames.Length; i++)
        {
            if (gameManager.partyInventoryReference.partyInventory[i] != null)
            {
                partyInvNames[i] = gameManager.partyInventoryReference.partyInventory[i].itemName;
                partyInvAmounts[i] = gameManager.partyInventoryReference.partyInventory[i].numberHeld;
            }
        }

        // Equipped Weapons
        grieveWeaponRightEquip = Engine.e.party[0].GetComponent<Character>().weaponRight.GetComponent<Weapon>().itemName;
        grieveWeaponLeftEquip = Engine.e.party[0].GetComponent<Character>().weaponLeft.GetComponent<Weapon>().itemName;

        if (Engine.e.party[1] != null)
        {
            macWeaponRightEquip = Engine.e.party[1].GetComponent<Character>().weaponRight.GetComponent<Weapon>().itemName;
            macWeaponLeftEquip = Engine.e.party[1].GetComponent<Character>().weaponLeft.GetComponent<Weapon>().itemName;

        }
        if (Engine.e.party[2] != null)
        {
            fieldWeaponRightEquip = Engine.e.party[2].GetComponent<Character>().weaponRight.GetComponent<Weapon>().itemName;
            fieldWeaponLeftEquip = Engine.e.party[2].GetComponent<Character>().weaponLeft.GetComponent<Weapon>().itemName;
        }
        if (Engine.e.party[3] != null)
        {
            riggsWeaponRightEquip = Engine.e.party[3].GetComponent<Character>().weaponRight.GetComponent<Weapon>().itemName;
            riggsWeaponLeftEquip = Engine.e.party[3].GetComponent<Character>().weaponLeft.GetComponent<Weapon>().itemName;
        }

        // Equipped Chest Armor
        for (int i = 0; i < charChestArmorEquip.Length; i++)
        {
            if (Engine.e.party[i] != null)
            {
                charChestArmorEquip[i] = Engine.e.party[i].GetComponent<Character>().chestArmor.GetComponent<ChestArmor>().itemName;
            }
        }

        // Equipped Accessory
        for (int i = 0; i < charAccessory1Equip.Length; i++)
        {
            if (Engine.e.party[i] != null)
            {
                if (Engine.e.party[i].GetComponent<Character>().accessory1 != null)
                {
                    charAccessory1Equip[i] = Engine.e.party[i].GetComponent<Character>().accessory1.GetComponent<Accessory>().itemName;
                }

                if (Engine.e.party[i].GetComponent<Character>().accessory2 != null)
                {
                    charAccessory2Equip[i] = Engine.e.party[i].GetComponent<Character>().accessory2.GetComponent<Accessory>().itemName;
                }
            }
        }

        // Node Information
        grieveNodes = new bool[Engine.e.gridReference.nodes.Length];
        macNodes = new bool[Engine.e.gridReference.nodes.Length];
        fieldNodes = new bool[Engine.e.gridReference.nodes.Length];
        riggsNodes = new bool[Engine.e.gridReference.nodes.Length];
        charNodePositions = new int[Engine.e.party.Length];

        charNodePositions[0] = Engine.e.gridReference.grievePosition;
        charNodePositions[1] = Engine.e.gridReference.macPosition;
        charNodePositions[2] = Engine.e.gridReference.fieldPosition;
        charNodePositions[3] = Engine.e.gridReference.riggsPosition;

        for (int i = 0; i < Engine.e.gridReference.nodes.Length; i++)
        {
            if (Engine.e.gridReference.nodes[i].grieveUnlocked)
            {
                grieveNodes[i] = true;
            }
            if (Engine.e.gridReference.nodes[i].macUnlocked)
            {
                macNodes[i] = true;
            }
            if (Engine.e.gridReference.nodes[i].fieldUnlocked)
            {
                fieldNodes[i] = true;
            }
            if (Engine.e.gridReference.nodes[i].riggsUnlocked)
            {
                riggsNodes[i] = true;
            }
        }

        // Quests
        partyQuests = new string[Engine.e.adventureLogReference.questLog.Length];
        partyQuestObjectiveCount = new int[Engine.e.adventureLogReference.questLog.Length, 3];
        completedQuests = new string[Engine.e.adventureLogReference.questLog.Length];
        questTurnedIn = new bool[Engine.e.adventureLogReference.questLog.Length];

        for (int i = 0; i < Engine.e.gameQuests.Length; i++)
        {

            if (Engine.e.gameQuests[i] != null)
            {
                if (Engine.e.gameQuests[i].inAdventureLog)
                {
                    if (Engine.e.gameQuests[i].isComplete && Engine.e.gameQuests[i].turnedIn)
                    {
                        completedQuests[i] = Engine.e.gameQuests[i].questName;
                    }
                    else
                    {
                        partyQuests[i] = Engine.e.gameQuests[i].questName;
                        questTurnedIn[i] = false;

                        for (int m = 0; m < Engine.e.gameQuests[i].objectiveCount.Length; m++)
                        {
                            int objectiveCount = Engine.e.gameQuests[i].objectiveCount[m];

                            partyQuestObjectiveCount[i, m] = objectiveCount;
                        }
                    }
                }
            }
        }

        // World Position
        partyPosition = new float[3];
        partyPosition[0] = Engine.e.activeParty.transform.position.x;
        partyPosition[1] = Engine.e.activeParty.transform.position.y;
        partyPosition[2] = Engine.e.activeParty.transform.position.z;

        activeParty = new string[3];

        activeParty[0] = gameManager.activeParty.activeParty[0].GetComponent<Character>().characterName;

        if (gameManager.activeParty.activeParty[1] != null)
        {
            activeParty[1] = gameManager.activeParty.activeParty[1].GetComponent<Character>().characterName;
        }
        if (gameManager.activeParty.activeParty[2] != null)
        {
            activeParty[2] = gameManager.activeParty.activeParty[2].GetComponent<Character>().characterName;
        }

        aboveLayer = Engine.e.aboveLayer;
        whichLayer = Engine.e.activeParty.GetComponent<SpriteRenderer>().sortingLayerName;
        scene = gameManager.currentScene;
        time = gameManager.timeOfDay;
        inWorldMap = Engine.e.inWorldMap;
        partyMoney = gameManager.partyMoney;
    }

    public void EraseData()
    {

    }
}
