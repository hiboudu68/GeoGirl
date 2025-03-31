using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SwipeController : MonoBehaviour, IEndDragHandler
{
    [SerializeField] int maxPage;
    int currentPage;
    Vector3 targetPos;
    [SerializeField] Vector3 pageStep;
    [SerializeField] RectTransform levelPagesRect;

    [SerializeField] float tweenTime;
    [SerializeField] LeanTweenType tweenType;
    float dragThreshould;

    private void Awake()
    {
        currentPage = 1;
        targetPos = levelPagesRect.localPosition;
        dragThreshould = Screen.width / 15;
    }

    public void Next()
    {
        if (currentPage < maxPage) {
            currentPage++;
            targetPos += pageStep;
            MovePage();
        }

    }

    public void Previous()
    {
        if (currentPage > 1)
        {
            currentPage--;
            targetPos -= pageStep;
            MovePage();
        }
    }


    public void goToPage(int page)
    {
        if (currentPage != page)
        {
            int temp = page - currentPage;
            currentPage = page;

            targetPos += pageStep * temp;
            MovePage();
        }
    }

    void MovePage()
    {
        levelPagesRect.LeanMoveLocal(targetPos, tweenTime).setEase(tweenType);
    }

    public void playLevel()
    {
        SceneManager.LoadScene("Level" + currentPage);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Mathf.Abs(eventData.position.x - eventData.pressPosition.x) > dragThreshould)
        {
            if (eventData.position.x > eventData.pressPosition.x) Previous();
            else Next();
        }
        else
        {
            MovePage();
        }
    }

}
