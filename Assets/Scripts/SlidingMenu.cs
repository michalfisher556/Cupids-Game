using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;

public class SlidingMenu : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private RectTransform gamePanel;  // Assign the Game Panel
    public RectTransform menuPanel;  // Assign the Menu Panel
    public float moveDistance = 300f; // How far the panels should move
    public float threshold = 50f;   // How much drag is needed to trigger open/close
    public float slideSpeed = 0.5f; // Smooth transition speed

    private Vector2 gamePanelStartPos;
    private Vector2 menuPanelStartPos;
    private Vector2 gamePanelTargetPos;
    private Vector2 menuPanelTargetPos;
    private bool isMenuOpen = false;

    private GameObject GamePanel;
    

    private CupidsManger cupid;

    private Vector3 rightpos = new Vector3(-17,0,0);

    


    void Start()
    {
         cupid = GetComponent<CupidsManger>();
        FindGamePanel(); // Assign gamePanel each scene
    }

    void FindGamePanel()
    {
        StopAllCoroutines(); // Prevent coroutine errors
         GamePanel = GameObject.FindGameObjectWithTag("GamePanel");
       
        if (GamePanel != null)
        {
            menuPanel.anchoredPosition = rightpos;
            gamePanel = GamePanel.GetComponent<RectTransform>();
            gamePanelStartPos = gamePanel.anchoredPosition;
            menuPanelStartPos = menuPanel.anchoredPosition;
            gamePanelTargetPos = gamePanelStartPos;
            menuPanelTargetPos = menuPanelStartPos;

           
        }
        else
        {
            Debug.LogError("GamePanel not found in this scene! Make sure it's tagged correctly.");
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(gamePanel == null)
        {
            Debug.LogWarning("no game panel");
        }
        float newX = Mathf.Clamp(gamePanel.anchoredPosition.x + eventData.delta.x, gamePanelStartPos.x, gamePanelStartPos.x + moveDistance);
        
        // Move both panels together
        gamePanel.anchoredPosition = new Vector2(newX, gamePanelStartPos.y);
        menuPanel.anchoredPosition = new Vector2(menuPanelStartPos.x + (newX - gamePanelStartPos.x), menuPanelStartPos.y);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float movedDistance = gamePanelStartPos.x - gamePanel.anchoredPosition.x;

        if (movedDistance > threshold && !isMenuOpen)
        {
            OpenMenu(); // Move both panels right
        }
        else if (movedDistance < -threshold && isMenuOpen)
        {
            CloseMenu(); // Move both panels left
        }
        else
        {
            ResetPosition(); // Reset if not enough movement
        }
    }

   public void OpenMenu()
    {
        gamePanelTargetPos = new Vector2(gamePanelStartPos.x + moveDistance, gamePanelStartPos.y);
        menuPanelTargetPos = new Vector2(menuPanelStartPos.x + moveDistance, menuPanelStartPos.y);
        isMenuOpen = true;
        StartCoroutine(SmoothMove());
    }

    public void CloseMenu()
    {
        gamePanelTargetPos = gamePanelStartPos;
        menuPanelTargetPos = menuPanelStartPos;
        isMenuOpen = false;
        StartCoroutine(SmoothMove());
    }

    void ResetPosition()
    {
        gamePanelTargetPos = isMenuOpen ? new Vector2(gamePanelStartPos.x + moveDistance, gamePanelStartPos.y) : gamePanelStartPos;
        menuPanelTargetPos = isMenuOpen ? new Vector2(menuPanelStartPos.x + moveDistance, menuPanelStartPos.y) : menuPanelStartPos;
        StartCoroutine(SmoothMove());
    }

    IEnumerator SmoothMove()
    {
        while (Vector2.Distance(gamePanel.anchoredPosition, gamePanelTargetPos) > 1f)
        {
            gamePanel.anchoredPosition = Vector2.Lerp(gamePanel.anchoredPosition, gamePanelTargetPos, slideSpeed * Time.deltaTime * 10);
            menuPanel.anchoredPosition = Vector2.Lerp(menuPanel.anchoredPosition, menuPanelTargetPos, slideSpeed * Time.deltaTime * 10);
            yield return null;
        }
        gamePanel.anchoredPosition = gamePanelTargetPos;
        menuPanel.anchoredPosition = menuPanelTargetPos;
    }

   void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
       
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindGamePanel(); // Reassign GamePanel for each new scene
    }
}

