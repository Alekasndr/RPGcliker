using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuiAbilsItem : MonoBehaviour
{
    [SerializeField] Button upgradeButton;
    [SerializeField] Text upgradeName;
    [SerializeField] Text level;
    [SerializeField] Text cost;

    bool isAllowUpgrade = false;


    public bool IsAllowUpgrade
    {
        get
        {
            return isAllowUpgrade;
        }
        set
        {
            if (isAllowUpgrade != value)
            {
                isAllowUpgrade = value;
            }
        }
    }

    public AbilityType AbilityType
    {
        get;
        private set;
    }



    public void Initialize(AbilityType abilityType)
    {
        AbilityType = abilityType;
        upgradeName.text = AbilityManager.Instance.abilityInfo[abilityType].name;
        AffectButton(false);
        UpdateUI();
    }



    void UpdateUI()
    {
        level.text = AbilityManager.Instance.AbilityLevel(AbilityType).ToString();
        cost.text = $"{AbilityManager.Instance.AbilityCost(AbilityType).ToString()} coins";
    }


    void Update()
    {
        AffectButton(Mathf.CeilToInt(GameManager.Instance.Coins) >= AbilityManager.Instance.AbilityCost(AbilityType));
    }


    void AffectButton(bool isEnabled)
    {
        upgradeButton.interactable = isEnabled;
    }


    public void Buy()
    {
        int currentCoins = Mathf.CeilToInt(GameManager.Instance.Coins);
        int currentCost = AbilityManager.Instance.AbilityCost(AbilityType);
        
        if (currentCoins >= currentCost)
        {
            GameManager.Instance.Coins -= (float)currentCost;
            AbilityManager.Instance.SetAbilityLevel(AbilityType, AbilityManager.Instance.AbilityLevel(AbilityType) + 1);
        }
        UpdateUI();
    }
}
