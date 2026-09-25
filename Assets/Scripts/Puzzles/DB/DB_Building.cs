using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DB_Building : MonoBehaviour
{
    [SerializeField] public static int _buildCount = 0;
    [SerializeField] private UnityEvent _onCompletion;
    [SerializeField] public static bool condition = false;

    [SerializeField] public BoxCollider _db_battery;

    private void Start()
    {
        if (PlayerPrefs.HasKey("DB_BuildingCompleted"))
        {
            _buildCount = PlayerPrefs.GetInt("DB_BuildingCompleted");
        }
        else
        {
            PlayerPrefs.SetInt("DB_BuildingCompleted", _buildCount);
        }
    }
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
        PlayerPrefs.SetInt("DB_BuildingCompleted", _buildCount);
    }

    public static void IncrementBuildCountWithCheck()
    {
        if (condition)
        {
            _buildCount++;
            PlayerPrefs.SetInt("DB_BuildingCompleted", _buildCount);
        }
    }

    public void Charged()
    {
        condition = true;
        _db_battery.GetComponent<BoxCollider>().enabled = true;
    }
}
