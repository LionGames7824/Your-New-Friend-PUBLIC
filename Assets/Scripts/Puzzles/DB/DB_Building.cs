using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DB_Building : MonoBehaviour
{
    [SerializeField] public static int _buildCount = 0;
    [SerializeField] private UnityEvent _onCompletion;
    [SerializeField] public static bool condition = false;
    
    void Update()
    {
        if (_buildCount == 3)
        {
            _onCompletion.Invoke();
        }
    }

    public static void IncrementBuildCount()
    {
        _buildCount++;
    }

    public static void IncrementBuildCountWithCheck()
    {
        if (condition)
        {
            _buildCount++;
        }
    }

    public static void Charged()
    {
        condition = true;
    }
}
