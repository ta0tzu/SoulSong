using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public static GameData gameData;
    public string[] charNames;
    public string[] currentCharClass;
    public float[] charMaxHealth;
    public float[] charCurrentHealth;
    public float[] charMaxMana;
    public float[] charCurrentMana;
    public int[] charLvl;
    public float[] charXP, charClassXPGrieve, charClassXPMac, charClassXPField, charClassXPRiggs, charClassXPSolace, charClassXPBlue,
    charClassLevelGrieve, charClassLevelMac, charClassLevelField, charClassLevelRiggs, charClassLevelSolace, charClassLevelBlue;
    public float[] charLvlUpReq;
    public float[] charBaseDamage;
    public float[] charPhysicalDamage;
    public float[] charSkillScale;
    public int[] charSkillIndex;
    public int[] charAvailableSkillPoints;
    public float[] charStealChance;
    public float[] charHaste;

    public bool[] classCompleteGrieve, classCompleteMac, classCompleteField, classCompleteRiggs, classCompleteSolace, classCompleteBlue;

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
    public string[] grieveEquippedSkills, macEquippedSkills, fieldEquippedSkills, riggsEquippedSkills, solaceEquippedSkills, blueEquippedSkills;


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
    public string solaceWeaponRightEquip, solaceWeaponLeftEquip;
    public string blueWeaponRightEquip, blueWeaponLeftEquip;

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
    public int partyMoney;

    public string[] partyQuests;
    public int[,] partyQuestObjectiveCount;

    public string[] completedQuests;
    public bool[] questTurnedIn;
    public string scene;
    public int[] time;
    public bool daylight, am;
    public bool battleModeActive;
    public bool aboveLayer;
    public bool inWorldMap, partyShown, indoors, indoorLighting;
    public bool recentAutoSave;
    public string whichLayer;
    public bool[] grieveNodes, macNodes, fieldNodes, riggsNodes;
    public int[] charNodePositions;
    public string[] cutsceneData;
    public float[] partyPosition;
    public string onLoadSceneReference;
    public float[] charEXPMultiplier, charDropsEXPMultiplier;

    public GameData(Engine gameManager)
    {
        charNames = new string[gameManager.party.Length];
        charMaxHealth = new float[gameManager.party.Length];
        charMaxMana = new float[gameManager.party.Length];
        charLvl = new int[gameManager.party.Length];
        currentCharClass = new string[gameManager.party.Length];

        classCompleteGrieve = new bool[gameManager.charClasses.Length];
        classCompleteMac = new bool[gameManager.charClasses.Length];
        classCompleteField = new bool[gameManager.charClasses.Length];
        classCompleteRiggs = new bool[gameManager.charClasses.Length];
        classCompleteSolace = new bool[gameManager.charClasses.Length];
        classCompleteBlue = new bool[gameManager.charClasses.Length];

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
        charClassXPGrieve = new float[gameManager.charClasses.Length];
        charClassXPMac = new float[gameManager.charClasses.Length];
        charClassXPField = new float[gameManager.charClasses.Length];
        charClassXPRiggs = new float[gameManager.charClasses.Length];
        charClassXPSolace = new float[gameManager.charClasses.Length];
        charClassXPBlue = new float[gameManager.charClasses.Length];

        charClassLevelGrieve = new float[gameManager.charClasses.Length];
        charClassLevelMac = new float[gameManager.charClasses.Length];
        charClassLevelField = new float[gameManager.charClasses.Length];
        charClassLevelRiggs = new float[gameManager.charClasses.Length];
        charClassLevelSolace = new float[gameManager.charClasses.Length];
        charClassLevelBlue = new float[gameManager.charClasses.Length];

        charClassXPMac = new float[gameManager.charClasses.Length];
        charClassXPField = new float[gameManager.charClasses.Length];
        charClassXPRiggs = new float[gameManager.charClasses.Length];
        charClassXPSolace = new float[gameManager.charClasses.Length];
        charClassXPBlue = new float[gameManager.charClasses.Length];

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
        charEXPMultiplier = new float[gameManager.party.Length];
        charDropsEXPMultiplier = new float[gameManager.party.Length];

        charDrops = new string[Engine.e.party.Length, Engine.e.gameDrops.Length];
        charSkills = new string[Engine.e.party.Length, Engine.e.gameSkills.Length];

        grieveEquippedSkills = new string[Engine.e.playableCharacters[0].equippedSkills.Length];

        if (Engine.e.playableCharacters[1] != null)
        {
            macEquippedSkills = new string[Engine.e.playableCharacters[1].equippedSkills.Length];
        }
        if (Engine.e.playableCharacters[2] != null)
        {
            fieldEquippedSkills = new string[Engine.e.playableCharacters[2].equippedSkills.Length];
        }
        if (Engine.e.playableCharacters[3] != null)
        {
            riggsEquippedSkills = new string[Engine.e.playableCharacters[3].equippedSkills.Length];
        }
        if (Engine.e.playableCharacters[4] != null)
        {
            solaceEquippedSkills = new string[Engine.e.playableCharacters[4].equippedSkills.Length];
        }
        if (Engine.e.playableCharacters[5] != null)
        {
            blueEquippedSkills = new string[Engine.e.playableCharacters[5].equippedSkills.Length];
        }
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
                currentCharClass[i] = gameManager.party[i].GetComponent<Character>().currentClass;
                charEXPMultiplier[i] = gameManager.party[i].GetComponent<Character>().expMultiplier;
                charDropsEXPMultiplier[i] = gameManager.party[i].GetComponent<Character>().dropsEXPMultiplier;

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


                currentCharClass[i] = gameManager.party[i].GetComponent<Character>().currentClass;


                // Drops
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

                // Skills
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
                partySize++;
            }
        }

        for (int f = 0; f < 12; f++)
        {
            if (gameManager.party[0] != null)
            {
                classCompleteGrieve[f] = gameManager.party[0].GetComponent<Character>().classCompleted[f];
                charClassXPGrieve[f] = gameManager.party[0].GetComponent<Character>().classEXP[f];
                charClassLevelGrieve[f] = gameManager.party[0].GetComponent<Character>().classLvl[f];

            }
            if (gameManager.party[1] != null)
            {
                classCompleteMac[f] = gameManager.party[1].GetComponent<Character>().classCompleted[f];
                charClassXPMac[f] = gameManager.party[1].GetComponent<Character>().classEXP[f];
                charClassLevelMac[f] = gameManager.party[1].GetComponent<Character>().classLvl[f];

            }
            if (gameManager.party[2] != null)
            {
                classCompleteField[f] = gameManager.party[2].GetComponent<Character>().classCompleted[f];
                charClassXPField[f] = gameManager.party[2].GetComponent<Character>().classEXP[f];
                charClassLevelField[f] = gameManager.party[2].GetComponent<Character>().classLvl[f];

            }
            if (gameManager.party[3] != null)
            {
                classCompleteRiggs[f] = gameManager.party[3].GetComponent<Character>().classCompleted[f];
                charClassXPRiggs[f] = gameManager.party[3].GetComponent<Character>().classEXP[f];
                charClassLevelRiggs[f] = gameManager.party[3].GetComponent<Character>().classLvl[f];

            }
            if (gameManager.party[4] != null)
            {
                classCompleteSolace[f] = gameManager.playableCharacters[4].GetComponent<Character>().classCompleted[f];
                charClassXPSolace[f] = gameManager.party[4].GetComponent<Character>().classEXP[f];
                charClassLevelSolace[f] = gameManager.party[4].GetComponent<Character>().classLvl[f];

            }
            if (gameManager.party[5] != null)
            {
                classCompleteBlue[f] = gameManager.playableCharacters[5].GetComponent<Character>().classCompleted[f];
                charClassXPBlue[f] = gameManager.party[5].GetComponent<Character>().classEXP[f];
                charClassLevelBlue[f] = gameManager.party[5].GetComponent<Character>().classLvl[f];

            }
        }

        for (int i = 0; i < grieveEquippedSkills.Length; i++)
        {
            if (gameManager.playableCharacters[0].GetComponent<Character>().equippedSkills[i] != null)
            {
                grieveEquippedSkills[i] = gameManager.playableCharacters[0].GetComponent<Character>().equippedSkills[i].skillName;
            }
        }

        for (int i = 0; i < macEquippedSkills.Length; i++)
        {
            if (gameManager.playableCharacters[1].GetComponent<Character>().equippedSkills[i] != null)
            {
                macEquippedSkills[i] = gameManager.playableCharacters[1].GetComponent<Character>().equippedSkills[i].skillName;
            }
        }

        for (int i = 0; i < fieldEquippedSkills.Length; i++)
        {
            if (gameManager.playableCharacters[2].GetComponent<Character>().equippedSkills[i] != null)
            {
                fieldEquippedSkills[i] = gameManager.playableCharacters[2].GetComponent<Character>().equippedSkills[i].skillName;
            }
        }

        for (int i = 0; i < riggsEquippedSkills.Length; i++)
        {
            if (gameManager.playableCharacters[3].GetComponent<Character>().equippedSkills[i] != null)
            {
                riggsEquippedSkills[i] = gameManager.playableCharacters[3].GetComponent<Character>().equippedSkills[i].skillName;
            }
        }

        for (int i = 0; i < solaceEquippedSkills.Length; i++)
        {
            if (gameManager.playableCharacters[4].GetComponent<Character>().equippedSkills[i] != null)
            {
                solaceEquippedSkills[i] = gameManager.playableCharacters[4].GetComponent<Character>().equippedSkills[i].skillName;
            }
        }

        for (int i = 0; i < blueEquippedSkills.Length; i++)
        {
            if (gameManager.playableCharacters[5].GetComponent<Character>().equippedSkills[i] != null)
            {
                blueEquippedSkills[i] = gameManager.playableCharacters[5].GetComponent<Character>().equippedSkills[i].skillName;
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
        if (Engine.e.party[0].GetComponent<Character>().weaponRight != null)
        {
            grieveWeaponRightEquip = Engine.e.party[0].GetComponent<Character>().weaponRight.GetComponent<Weapon>().itemName;
        }
        /*if (Engine.e.party[0].GetComponent<Character>().weaponLeft != null)
        {
            grieveWeaponLeftEquip = Engine.e.party[0].GetComponent<Character>().weaponLeft.GetComponent<Weapon>().itemName;
        }*/

        if (Engine.e.party[1] != null)
        {
            if (Engine.e.party[1].GetComponent<Character>().weaponRight != null)
            {
                macWeaponRightEquip = Engine.e.party[1].GetComponent<Character>().weaponRight.GetComponent<Weapon>().itemName;
            }
        }
        if (Engine.e.party[2] != null)
        {
            if (Engine.e.party[2].GetComponent<Character>().weaponRight != null)
            {
                fieldWeaponRightEquip = Engine.e.party[2].GetComponent<Character>().weaponRight.GetComponent<Weapon>().itemName;
            }
        }
        if (Engine.e.party[3] != null)
        {
            if (Engine.e.party[3].GetComponent<Character>().weaponRight != null)
            {
                riggsWeaponRightEquip = Engine.e.party[3].GetComponent<Character>().weaponRight.GetComponent<Weapon>().itemName;
            }
        }
        if (Engine.e.party[4] != null)
        {
            if (Engine.e.party[4].GetComponent<Character>().weaponRight != null)
            {
                solaceWeaponRightEquip = Engine.e.party[4].GetComponent<Character>().weaponRight.GetComponent<Weapon>().itemName;
            }
        }
        if (Engine.e.party[5] != null)
        {
            if (Engine.e.party[5].GetComponent<Character>().weaponRight != null)
            {
                blueWeaponRightEquip = Engine.e.party[5].GetComponent<Character>().weaponRight.GetComponent<Weapon>().itemName;
            }
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
        charNodePositions[4] = Engine.e.gridReference.solacePosition;
        charNodePositions[3] = Engine.e.gridReference.bluePosition;

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
        partyPosition = new float[2];
        partyPosition[0] = Engine.e.activeParty.transform.position.x;
        partyPosition[1] = Engine.e.activeParty.transform.position.y;


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

        time = new int[3];

        time[0] = gameManager.hour;
        time[1] = gameManager.minute;
        time[2] = gameManager.militaryHour;
        daylight = gameManager.daylight;
        am = gameManager.am;

        cutsceneData = new string[Engine.e.oneTimeCutscenesForDataReference.Count];

        for (int i = 0; i < cutsceneData.Length; i++)
        {
            cutsceneData[i] = Engine.e.oneTimeCutscenesForDataReference[i];
        }

        inWorldMap = Engine.e.inWorldMap;
        indoors = Engine.e.indoors;
        indoorLighting = Engine.e.indoorLighting;
        partyShown = Engine.e.partyShown;
        // onLoadSceneReference = Engine.e.partyLocationDisplay.text;

        partyMoney = gameManager.partyMoney;
    }

    public void EraseData()
    {

    }
}
