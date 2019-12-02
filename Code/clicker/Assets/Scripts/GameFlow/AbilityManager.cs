using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : SingletoneMonoBehaviour<AbilityManager>
{
    public const string AbilityLevelKey = "Ability_Level";
    public class AbilityInfo
    {
        public string name;
        public float minCost;
        public float maxCost;

        public int maxLevel;
        public AnimationCurve costCurve;
    }

    public Dictionary<AbilityType, AbilityInfo> abilityInfo;

    public int AbilityLevel(AbilityType abilityType)
    {
        return CustomPlayerPrefs.GetInt($"{AbilityLevelKey}_{abilityType}", 0);
    }

    public void SetAbilityLevel(AbilityType abilityType, int value)
    {
        value = Mathf.Clamp(value, 0, abilityInfo[abilityType].maxLevel + 1);
        CustomPlayerPrefs.SetInt($"{AbilityLevelKey}_{abilityType}", value, true);
    }

    
    public int AbilityCost(AbilityType abilityType)
    {
        int currnetAbilityLevel = AbilityLevel(abilityType);
        float levelFactor = (float)currnetAbilityLevel / (float)(abilityInfo[abilityType].maxLevel - 1);
        
        return Mathf.RoundToInt(Mathf.Lerp(abilityInfo[abilityType].minCost, abilityInfo[abilityType].maxCost, abilityInfo[abilityType].costCurve.Evaluate(levelFactor)));
    }
}
