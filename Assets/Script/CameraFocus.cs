using UnityEngine;
using Unity.Cinemachine;

public class CameraFocus : MonoBehaviour
{
    public CinemachineBrain Brain;
    public ICinemachineCamera Cam1;
    public ICinemachineCamera Cam2;
    void Start()
    {
        Cam1 = GetComponent<CinemachineCamera>();
        Cam2 = GetComponent<CinemachineCamera>();

        int layer = 1;
        int priority = 1;
        float weight = 1f;
        float blendTime = 1f;
        Brain.SetCameraOverride(layer, priority, Cam1,Cam2, weight, blendTime);
    }
}
