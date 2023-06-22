using UnityEngine;

public class TargetFPS : MonoBehaviour
{
    [SerializeField] private int target = 60;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = target;
    }
}