using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilsPopup : BasePopup
{
    [SerializeField] GuiAbilsItem prefab;
    [SerializeField] List<AbilityType> types;
    [SerializeField] Transform spawnTransform;

    List<GuiAbilsItem> items = new List<GuiAbilsItem>();


    protected override void WasStartShowing()
    {
        base.WasStartShowing();

        foreach (var item in types)
        {
            var instance = ObjectCreator.CreateObject<GuiAbilsItem>(prefab.gameObject, spawnTransform);
            items.Add(instance);
            instance.Initialize(item);
        }
    }


    protected override void WasHided()
    {
        base.WasHided();
    }
}
