using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : SingletoneMonoBehaviour<DamageManager>
{
    public float beginTimerDamage = 1f;
    public float beginClickDamage = 1f;

    public AnimationCurve timerDamageCurve;
    public AnimationCurve clickDamageCurve;



    public float GetMultiplier(AbilityType abilityType)
    {
        AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        int maxLevel = AbilityManager.Instance.abilityInfo[abilityType].maxLevel;
        switch (abilityType)
        {
            case AbilityType.ClickDamage:
            curve = clickDamageCurve;
            break;
            case AbilityType.TimeDamage:
            curve = timerDamageCurve;
            break;
        }

        float level = AbilityManager.Instance.AbilityLevel(abilityType);
        return curve.Evaluate(level / (float)maxLevel);
    }
}
