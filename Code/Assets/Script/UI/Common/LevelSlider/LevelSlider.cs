using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelSlider : MonoBehaviour
{
    [SerializeField] private GameObject LevelPrefab;
    [SerializeField] private AnimationCurve transitionCurve;
    [SerializeField] private float animationSpeed = 1f;
    private List<LevelComponent> levelComponents = new();
    private Level[] levels;
    private LevelComponent currentLevel;
    private int levelIndex = 0;
    private Vector3 addOffset;
    private float elapsedTime = 0f;

    private bool isShowing = false;
    private bool isHidding = false;
    private bool isAnimating = false;

    public bool IsVisible => isShowing;
    public Level Level => currentLevel.LevelInfos;
    private LevelComponent NextLevel
    {
        get => levelIndex == levelComponents.Count - 1 || levelComponents.Count == 1 ? levelComponents[0] : levelComponents[levelIndex + 1];
    }
    private LevelComponent PreviousLevel
    {
        get => levelIndex == 0 || levelComponents.Count == 1 ? levelComponents[levelComponents.Count - 1] : levelComponents[levelIndex - 1];
    }
    public Level GetLevel() => currentLevel.LevelInfos;

    public void SetLevels(Level[] levels)
    {
        if (isShowing)
        {
            Vector3 offset = levelComponents.Count * addOffset;
            foreach (Level level in levels)
            {
                if (levelComponents.FirstOrDefault(x => x.LevelInfos.Id == level.Id) == null)
                {
                    GameObject levelObj = GameObject.Instantiate(LevelPrefab);
                    LevelComponent lvlComponent = levelObj.GetComponent<LevelComponent>();
                    lvlComponent.StartPosition = offset;
                    lvlComponent.TargetPosition = offset;
                    levelComponents.Add(lvlComponent);

                    lvlComponent.SetInfos(level);
                    levelObj.transform.parent = transform;
                    levelObj.transform.localPosition = offset;
                    offset += addOffset;
                }
            }

            LevelComponent[] components = levelComponents.ToArray();
            foreach (LevelComponent component in components)
            {
                if (levels.FirstOrDefault(x => x.Id == component.LevelInfos.Id) == null)
                {
                    if (component.LevelInfos.Id == GetLevel().Id)
                        Next();

                    levelComponents.Remove(component);
                    Destroy(component.gameObject);
                }
            }
        }

        this.levels = levels;
    }
    public void HideButtons()
    {
        isAnimating = false;
        isShowing = false;
        isHidding = true;

        foreach (BtnLevelSwap btn in GetComponentsInChildren<BtnLevelSwap>())
            btn.Hide();
    }
    public void ShowButtons()
    {
        levelIndex = 0;
        isAnimating = false;
        isShowing = true;

        addOffset = Vector3.right * Screen.width;
        Vector3 offset = Vector3.zero;

        foreach (Level lvl in levels)
        {
            GameObject levelObj = GameObject.Instantiate(LevelPrefab);
            LevelComponent lvlComponent = levelObj.GetComponent<LevelComponent>();
            lvlComponent.StartPosition = offset;
            lvlComponent.TargetPosition = offset;
            levelComponents.Add(lvlComponent);

            lvlComponent.SetInfos(lvl);
            levelObj.transform.SetParent(transform);
            levelObj.transform.localPosition = offset;
            offset += addOffset;
        }

        levelIndex = 0;
        if (levelComponents.Count == 0)
        {
            currentLevel = null;
            return;
        }

        currentLevel = levelComponents[0];
        currentLevel.transform.localScale = Vector3.zero;
        FindAnyObjectByType<GameGrid>().LoadLevel(currentLevel.LevelInfos);

        if (levelComponents.Count > 0)
        {
            currentLevel = levelComponents[0];
            currentLevel.transform.localScale = Vector3.zero;
        }

        foreach (BtnLevelSwap btn in GetComponentsInChildren<BtnLevelSwap>())
            btn.Show();

    }
    void Update()
    {
        if (currentLevel == null)
            return;

        if (isAnimating)
        {
            elapsedTime += Time.unscaledDeltaTime * animationSpeed;
            float t = transitionCurve.Evaluate(elapsedTime);
            currentLevel.transform.localPosition = currentLevel.StartPosition + (currentLevel.TargetPosition - currentLevel.StartPosition) * t;
            NextLevel.transform.localPosition = NextLevel.StartPosition + (NextLevel.TargetPosition - NextLevel.StartPosition) * t;
            PreviousLevel.transform.localPosition = PreviousLevel.StartPosition + (PreviousLevel.TargetPosition - PreviousLevel.StartPosition) * t;

            if (elapsedTime >= 1f)
                isAnimating = false;
        }
        else
        {
            if (isShowing)
            {
                currentLevel.transform.localScale = Vector3.Lerp(currentLevel.transform.localScale, Vector3.one, Time.unscaledDeltaTime * 10f);
            }
            else if (isHidding)
            {
                currentLevel.transform.localScale = Vector3.Lerp(currentLevel.transform.localScale, Vector3.one * 0.2f, Time.unscaledDeltaTime * 15f);
                if ((currentLevel.transform.localScale - Vector3.one * 0.2f).magnitude < 0.1f)
                {
                    foreach (LevelComponent lvl in levelComponents)
                        Destroy(lvl.gameObject);

                    levelComponents.Clear();
                    currentLevel = null;
                }
            }
        }
    }
    public void Next()
    {
        if (levelComponents.Count < 2)
            return;

        elapsedTime = 0f;
        isAnimating = true;
        levelIndex = (levelIndex + 1) % levelComponents.Count;
        currentLevel = levelComponents[levelIndex];
        currentLevel.StartPosition = addOffset;
        currentLevel.TargetPosition = Vector3.zero;

        NextLevel.StartPosition = addOffset;
        NextLevel.TargetPosition = addOffset;

        PreviousLevel.StartPosition = Vector3.zero;
        PreviousLevel.TargetPosition = -addOffset;

        FindAnyObjectByType<GameGrid>().LoadLevel(currentLevel.LevelInfos);
    }
    public void Previous()
    {
        if (levelComponents.Count < 2)
            return;

        elapsedTime = 0f;
        isAnimating = true;

        levelIndex = (levelIndex - 1 + levelComponents.Count) % levelComponents.Count;
        currentLevel = levelComponents[levelIndex];
        currentLevel.StartPosition = -addOffset;
        currentLevel.TargetPosition = Vector3.zero;

        NextLevel.StartPosition = Vector3.zero;
        NextLevel.TargetPosition = addOffset;

        PreviousLevel.TargetPosition = -addOffset;
        PreviousLevel.StartPosition = -addOffset;

        FindAnyObjectByType<GameGrid>().LoadLevel(currentLevel.LevelInfos);
    }
}
