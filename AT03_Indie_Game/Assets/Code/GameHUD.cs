using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    [SerializeField] private Image crosshairImg;
    [SerializeField] private Text objectiveTxt;
    [SerializeField] private string objectiveA;
    [SerializeField] private string objectiveB;
    [SerializeField] private string objectiveC;
    [SerializeField] private string objectiveD;
    [SerializeField] private string objectiveE;

    public static GameHUD Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        objectiveTxt.text = objectiveA;
        GoalCollect.ObjectiveActivate += delegate { objectiveTxt.text = objectiveB; };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
