using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameHUD : MonoBehaviour
{
    [SerializeField] private float ObjectiveCount;
    [SerializeField] private Image crosshairImg;
    [SerializeField] private Text objectiveTxt;
    [SerializeField] private string objectiveA;
    [SerializeField] private string objectiveB;
    [SerializeField] private string objectiveC;
    [SerializeField] private string objectiveD;
    [SerializeField] private string objectiveE;
    [SerializeField] private GameObject promptPanel;
    [SerializeField] private Text promptText;
    [SerializeField] [TextArea] private string victoryMessage;
    [SerializeField] [TextArea] private string gameOverMessage;
    public static GameHUD Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        GoalCollect.GoalActivate += Count;
    }

    // Start is called before the first frame update
    void Start()
    {
        promptPanel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        objectiveTxt.text = objectiveA;
        GoalCollect.ObjectiveActivate += delegate { objectiveTxt.text = objectiveE; };
        ObjectiveCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) == true)
        {
            Application.Quit();
        }
    }

    public void SetCrosshairColour(Color colour)
    {
        if(crosshairImg.color != colour)
        {
            crosshairImg.color = colour;
        }
    }

    private void Count()
    {
        ObjectiveCount = ObjectiveCount + 1;
        if (ObjectiveCount == 1)
        {
            objectiveTxt.text = objectiveB;
        }
        if (ObjectiveCount == 2)
        {
            objectiveTxt.text = objectiveC;
        }
        if (ObjectiveCount == 3)
        {
            objectiveTxt.text = objectiveD;
        }
    }

    public void ActivateEndPrompt(bool victory)
    {
        promptPanel.SetActive(true);
        if(victory == true)
        {
            promptText.text = victoryMessage;
        }
        else
        {
            promptText.text = gameOverMessage;
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void LoadFirstSceneInBuild()
    {
        SceneManager.LoadScene(0);
    }
}
