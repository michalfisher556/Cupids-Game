using UnityEngine;
using UnityEngine.SceneManagement;

public class CupidsManger : MonoBehaviour
{
    public static CupidsManger instance;
    private const int MainGameSceneIndex = 1; // Change this if needed

    private GameObject[] myPannels;
    private Transform canvas;
    private Transform holder;
    private SlidingMenu slide;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Prevents the object from being destroyed
        }
        else
        {
            Destroy(gameObject); // Ensures only one instance exists
        }

         slide = GetComponent<SlidingMenu>();

        canvas = transform.GetChild(0);
        holder = canvas.GetChild(0);

        int counter = holder.childCount;

        myPannels = new GameObject[counter];

        for(int i = 0; i < counter; i++)
        {
           myPannels[i] = holder.GetChild(i).gameObject;
        }
    }

    public void NewGame()
    {
        SceneManager.LoadScene(MainGameSceneIndex);
    }

    public void LoadGame()
    {
       // slide.CloseMenu();
        if (PlayerPrefs.HasKey("SavedScene"))
        {
            int savedScene = PlayerPrefs.GetInt("SavedScene");

            // Ensure scene index is within valid range
            if (savedScene >= 0 && savedScene < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(savedScene);
                Debug.Log($"Game Loaded! Loading Scene: {savedScene}");
            }
            else
            {
                Debug.LogWarning("Saved scene index is out of range!");
            }
        }
        else
        {
            Debug.LogWarning("No saved game found!");
        }
    }

    public void Resume()
    {
        SetPanels(20);
        slide.CloseMenu();
    }
    public void MainMenu()
    {
         SetPanels(0);
        slide.OpenMenu();

    }

    public void ClosePannel()
    {
        SetPanels(0);
        slide.OpenMenu();
    }



    public void Help()
    {
       SetPanels(1);
    }

    public void Settings()
    {
        SetPanels(2);
    }

    public void Selection()
    {
        SetPanels(3);
    }

    private void SetPanels(int panel)
    {
        for(int i = 0; i < myPannels.Length; i++)
        {
            myPannels[i].SetActive(i == panel);
        }

    }
}



