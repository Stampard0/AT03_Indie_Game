using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCounting : MonoBehaviour
{
    [SerializeField] private float GoalCount;
    public float GoalTotal;
    public delegate void FinalDelegate();

    public static event FinalDelegate FinalActivate = delegate { FinalActivate = delegate { }; };
    // Start is called before the first frame update
    void Start()
    {
        GoalCount = 0;
        GoalTotal = 0;
    }

    // Update is called once per frame
    void Update()
    {
        GoalTotal = GoalCount;
    }
    private void Awake()
    {
        GoalCollect.GoalActivate += Count;
    }

    private void Count()
    {
        if(GoalCount <= 3)
        {
            GoalCount = GoalCount + 1;
        }
        if(GoalCount > 3)
        {
            FinalActivate.Invoke();
        }
    }
}
