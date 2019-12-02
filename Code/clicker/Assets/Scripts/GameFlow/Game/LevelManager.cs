using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class LevelManager : SingletoneMonoBehaviour<LevelManager>
{
    public static event Action OnLevelSpawned;
    public static event Action<int, float> OnHPChanged;
    public static event Action OnLevelDestroyed;
    public static event Action<int> OnLevelPassed;
    public class MobsInfo
    {
        public Sprite image;
        public int health;
    }

    [Serializable]
    public class GameProgress
    {
        public int levelIndex;
        public float damage;
    }
    


    [SerializeField] List<MobsInfo> mobsInfos;
    [SerializeField] SpriteRenderer gameSprite;
    [SerializeField] AnimationCurve xCurve;
    [SerializeField] AnimationCurve yCurve;
    [SerializeField] float damageDuration;


    bool isLevelStarted = false;
    public int CurrentLevelIndex { get; set; }


    public List<MobsInfo> CurrentMobsInfo => mobsInfos;

    List<GameProgress> _currentProgress = null;
    List<GameProgress> CurrentProgress
    {
        get
        {
            if (_currentProgress == null)
            {
                _currentProgress = CustomPlayerPrefs.GetObjectsValue<GameProgress>("GAME_PROGRESS");
            }
            if (_currentProgress == null)
            {
                _currentProgress = new List<GameProgress>();
            }
            return _currentProgress;
        }
        set
        {
            _currentProgress = value;
            CustomPlayerPrefs.SetObjectsValue<GameProgress>("GAME_PROGRESS", _currentProgress, false);
        }
    }


    public bool IsContainsProgress(int index)
    {
        return CurrentProgress.Find((item) => item.levelIndex == index) != null;
    }
    

    public GameProgress ProgressForIndex(int index)
    {
        var data = CurrentProgress.Find((item) => item.levelIndex == index);
        if (data == null)
        {
            data = new GameProgress()
            {
                levelIndex = index,
                damage = 0f
            };
            CurrentProgress.Add(data);
        }

        return data;
    }


    public void SetProgressForIndex(int index, float damage)
    {
        var data = CurrentProgress.Find((item) => item.levelIndex == index);
        if (data != null)
        {
            CurrentProgress.Remove(data);
            CurrentProgress.Add(new GameProgress()
            {
                levelIndex = index,
                damage = damage
            });
            CurrentProgress = CurrentProgress;
        }

    }

    Camera _gameCamera;


    Camera GameCamera => _gameCamera ?? (_gameCamera = Camera.main);



    void Awake()
    {
        GameCamera.gameObject.SetActive(false);
    }


    void Update()
    {
        if (isLevelStarted)
        {
            DamageTimer();
        }
    }


    public void CreateLevel(int index)
    {
        GuiManager.Instance.ScreenController.Show(ScreenType.Game);
        GameCamera.gameObject.SetActive(true);
        index = Mathf.Clamp(index, 0, mobsInfos.Count - 1);
        CurrentLevelIndex = index;
        CreateLevel(mobsInfos[index]);
    }


    public void DestroyLevel()
    {
        isLevelStarted = false;
        GameCamera.gameObject.SetActive(false);
        OnLevelDestroyed?.Invoke();
    }


    void CreateLevel(MobsInfo info)
    {
        float currentMobHealth = (float) info.health - ProgressForIndex(CurrentLevelIndex).damage;

        if (Mathf.Approximately(currentMobHealth, 0f))
        {
            SetProgressForIndex(CurrentLevelIndex, 0f);
            currentMobHealth = (float) info.health;
        }
        ResertSprite();

        CurrentMobHealth = currentMobHealth;
        gameSprite.sprite = info.image;
        lastTimerDamage = 0f;
        isLevelStarted = true;
        OnLevelSpawned?.Invoke();
    }


    [Sirenix.OdinInspector.Button("Reset")]
    void ResetPrefs()
    {
        CustomPlayerPrefs.DeleteAll();
    }


    public void DamageClick()
    {
        ApplyDamage(DamageManager.Instance.beginClickDamage * DamageManager.Instance.GetMultiplier(AbilityType.ClickDamage));
    }

    void ResertSprite()
    {
        gameSprite.transform.localScale = Vector3.one;
    }

    float? _currentMobHealth;
    float CurrentMobHealth
    {
        get
        {
            return (_currentMobHealth != null) ? _currentMobHealth.Value : 0f;
        }
        set
        {
            if (_currentMobHealth == null || _currentMobHealth.Value != value)
            {
                _currentMobHealth = value;
                OnHPChanged?.Invoke(CurrentLevelIndex, _currentMobHealth.Value);

                if (Mathf.Approximately(value, 0f))
                {
                    OnLevelPassed?.Invoke(CurrentLevelIndex);

                    isLevelStarted = false;
                    GuiManager.Instance.PopupController.Show(PopupType.Win, false, false, (popup) =>
                    {
                        popup.AddHidedCallback((_) =>
                        {
                            int prevIndex = CurrentLevelIndex;
                            DestroyLevel();
                            CreateLevel(prevIndex + 1);
                        });
                    });
                }
            }
        }
    }


    void ApplyDamage(float value)
    {
        GameManager.Instance.Coins += value;
        float newValue = Mathf.Clamp(CurrentMobHealth - value, 0, CurrentMobHealth);
        CurrentMobHealth = newValue;
        
        var currentProgress = ProgressForIndex(CurrentLevelIndex);
        currentProgress.damage += value;

        SetProgressForIndex(CurrentLevelIndex, Mathf.Clamp(currentProgress.damage, 0f, (float)CurrentMobsInfo[CurrentLevelIndex].health));
        
        ResertSprite();
        DOTween.Kill(gameSprite);
        DOTween.To(() => 0f, (x) =>
        {
            float xVal = Mathf.Lerp(1f, 1.1f, xCurve.Evaluate(x));
            float yVal = Mathf.Lerp(1f, 1.1f, yCurve.Evaluate(x));
            gameSprite.transform.localScale = new Vector3(xVal, yVal, 1f);
        }, 1f, damageDuration).SetId(gameSprite);
    }


    float lastTimerDamage = 0f;
    public void DamageTimer()
    {
        if (Time.time - lastTimerDamage >= 1f)
        {
            lastTimerDamage = Time.time;
            ApplyDamage(DamageManager.Instance.beginTimerDamage * DamageManager.Instance.GetMultiplier(AbilityType.TimeDamage));
        }
    }


    public void ResetPrefsAll()
    {
        CustomPlayerPrefs.DeleteAll();
        _currentProgress = null;
    }
}
