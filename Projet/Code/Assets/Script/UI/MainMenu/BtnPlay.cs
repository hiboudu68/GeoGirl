using UnityEngine;
using UnityEngine.EventSystems;

public class BtnPlay : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        MainMenuController menuController = GetComponentInParent<MainMenuController>();
        LevelMenu levelMenu = menuController.transform.parent.GetComponentInChildren<LevelMenu>();
        EditableLevelListMenu editableLevelMenu = menuController.transform.parent.GetComponentInChildren<EditableLevelListMenu>();
        if (!levelMenu.IsVisible && !editableLevelMenu.IsVisible)
        {
            menuController.Hide();
            levelMenu.Show();
            editableLevelMenu.GetComponent<UIToggler>().IsVisible = false;
        }
        else
        {
            if (editableLevelMenu.IsVisible && editableLevelMenu.GetSelectedLevel() == null)
                return;

            GetComponent<Slidable>().Hide();
            if (editableLevelMenu.IsVisible)
            {
                editableLevelMenu.Hide();
                foreach (Slidable btn in editableLevelMenu.GetComponentsInChildren<Slidable>())
                    btn.gameObject.SetActive(false);

                Player.StopPlaying += OnStopPlayingFromEditableLevels;
                FindAnyObjectByType<GameGrid>().LoadLevel(editableLevelMenu.GetSelectedLevel());
            }
            else if (levelMenu.IsVisible)
            {
                levelMenu.transform.parent.GetComponentInChildren<BtnSound>().GetComponent<Slidable>().Hide();
                levelMenu.HideBack(false);
                levelMenu.Hide();
                Player.StopPlaying += OnStopPlayingFromLegacyLevelList;
            }

            menuController.transform.parent.GetComponentInChildren<InGameMenu>().Show();
            Player.Play(FindAnyObjectByType<GameGrid>().PlayerPrefab);
        }

        UIAudio.Instance.PlayTick();
    }
    private void OnStopPlayingFromEditableLevels()
    {
        MainMenuController menuController = GetComponentInParent<MainMenuController>();
        EditableLevelListMenu editableLevelMenu = menuController.transform.parent.GetComponentInChildren<EditableLevelListMenu>();
        menuController.Hide();

        editableLevelMenu.GetComponent<UIToggler>().IsVisible = false;
        editableLevelMenu.Show();
        editableLevelMenu.ShowBack();
        foreach (Slidable btn in editableLevelMenu.GetComponentsInChildren<Slidable>(true))
            btn.gameObject.SetActive(true);

        menuController.transform.parent.GetComponentInChildren<InGameMenu>().Hide();
        Player.StopPlaying -= OnStopPlayingFromEditableLevels;
    }
    private void OnStopPlayingFromLegacyLevelList()
    {
        MainMenuController menuController = GetComponentInParent<MainMenuController>();
        LevelMenu levelMenu = menuController.transform.parent.GetComponentInChildren<LevelMenu>();
        menuController.Hide();
        levelMenu.Show();
        levelMenu.ShowBack();
        GetComponent<Slidable>().Show();

        menuController.transform.parent.GetComponentInChildren<InGameMenu>().Hide();
        Player.StopPlaying -= OnStopPlayingFromLegacyLevelList;
    }
}
