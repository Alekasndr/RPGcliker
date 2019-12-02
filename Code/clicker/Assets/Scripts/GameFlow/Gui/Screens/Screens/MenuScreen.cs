using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreen : BaseScreen
{
    [SerializeField] LevelButton levelButtonPrefab;
    [SerializeField] Transform spawnTransform;

    List<LevelButton> spawnedButtons = new List<LevelButton>();


    protected override void WasStartShowing()
    {
        base.WasStartShowing();
        SpawnItems();
    }


    void SpawnItems()
    {

        int levelsCount =  LevelManager.Instance.CurrentMobsInfo.Count;
        for (int i = 0; i < levelsCount; i++)
        {
            var item = ObjectCreator.CreateObject<LevelButton>(levelButtonPrefab.gameObject, spawnTransform);
            item.TextLabel = $"Level {i + 1}";
            item.LevelIndex = i;
            item.OnClick += Item_OnClick;
            item.button.interactable = (i == 0 || LevelManager.Instance.IsContainsProgress(i));
            spawnedButtons.Add(item);
        }
    }

    void DestroyItems()
    {
        foreach (var item in spawnedButtons)
        {
            item.OnClick -= Item_OnClick;
            Destroy(item.gameObject);
        }
        spawnedButtons.Clear();
    }


    protected override void WasHided()
    {
        base.WasHided();
        DestroyItems();
    }



    void Item_OnClick(LevelButton buttonItem)
    {
        Hide(false);
        LevelManager.Instance.CreateLevel(buttonItem.LevelIndex);
    }


    public void ResetGame()
    {
        GuiManager.Instance.PopupController.Show(PopupType.Dialog, false, (popup) =>
        {
            DialogPopup dialogPopup = popup as DialogPopup;
            dialogPopup.topLabel.text = "Are you shure?";
            dialogPopup.yesButtonLabel.text = "Yes, reset game";
            dialogPopup.noButtonLabel.text = "No, continue game";

            dialogPopup.yesButton.onClick.AddListener(() =>
            {
                dialogPopup.noButton.onClick.RemoveAllListeners();
                dialogPopup.yesButton.onClick.RemoveAllListeners();
                LevelManager.Instance.ResetPrefsAll();

                DestroyItems();
                SpawnItems();
                
                dialogPopup.Hide(false);
            });

            dialogPopup.noButton.onClick.AddListener(() =>
            {
                dialogPopup.noButton.onClick.RemoveAllListeners();
                dialogPopup.yesButton.onClick.RemoveAllListeners();
                dialogPopup.Hide(false);
            });
        });
    }
}
