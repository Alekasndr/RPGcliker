using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScreen : BaseScreen
{
    [SerializeField] Button gameButton;
    [SerializeField] Text levelInfo;
    [SerializeField] Slider hpSlider;
    [SerializeField] Button levelsButton;
    [SerializeField] Button abilsButton;
    [SerializeField] Text coins;


    protected override void WasStartShowing()
    {
        base.WasStartShowing();
        UpdateCoins();
        LevelManager.OnLevelSpawned += LevelManager_OnLevelSpawned;
        LevelManager.OnHPChanged += LevelManager_OnHPChanged;
        levelsButton.onClick.AddListener(LevelsButton_OnClick);
        abilsButton.onClick.AddListener(AbilsButton_OnClick);
        gameButton.onClick.AddListener(GameButton_OnClick);
        LevelManager.OnLevelDestroyed += LevelManager_OnLevelDestroyed;

        LevelManager_OnHPChanged(0, 0.5f);
    }


    protected override void WasHided()
    {
        base.WasHided();
        LevelManager.OnLevelSpawned -= LevelManager_OnLevelSpawned;
        LevelManager.OnHPChanged -= LevelManager_OnHPChanged;
        levelsButton.onClick.RemoveListener(LevelsButton_OnClick);
        abilsButton.onClick.RemoveListener(AbilsButton_OnClick);
        gameButton.onClick.RemoveListener(GameButton_OnClick);
        LevelManager.OnLevelDestroyed -= LevelManager_OnLevelDestroyed;
    }


    void LevelsButton_OnClick()
    {
        AddHidedCallback((_) =>
        {
            LevelManager.Instance.DestroyLevel();
            GuiManager.Instance.ScreenController.Show(ScreenType.Menu, false);
        });
        Hide(false);
    }


    void AbilsButton_OnClick()
    {
        GuiManager.Instance.PopupController.Show(PopupType.Abils, false, (popup) =>
        {
            
        });
    }


    void GameButton_OnClick()
    {
        LevelManager.Instance.DamageClick();
    }

    
    void LevelManager_OnLevelSpawned()
    {
        var currentProgressInfo = LevelManager.Instance.ProgressForIndex(LevelManager.Instance.CurrentLevelIndex);
        var currentLevelInfo = LevelManager.Instance.CurrentMobsInfo[LevelManager.Instance.CurrentLevelIndex];

        float hpFactor = ((float)currentProgressInfo.damage) / (float)currentLevelInfo.health;
        hpSlider.value = hpFactor;

        levelInfo.text = $"Level {LevelManager.Instance.CurrentLevelIndex + 1}";
    }


    void LevelManager_OnHPChanged(int levelIndex, float currentHP)
    {
        LevelManager_OnLevelSpawned();
    }


    void LevelManager_OnLevelDestroyed()
    {
        Hide(false);
    }


    void Update() 
    {
        UpdateCoins();
    }

    void UpdateCoins()
    {
        coins.text = Mathf.CeilToInt(GameManager.Instance.Coins).ToString();
    }
}
