using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EnableControllersInVR : MonoBehaviour
{
    public XRBaseController leftController;
    public XRBaseController rightController;

    private void Start()
    {
#if UNITY_EDITOR
        leftController.gameObject.SetActive(false);
        rightController.gameObject.SetActive(false);
#elif UNITY_XR_OPENXR
        leftController.gameObject.SetActive(true);
        rightController.gameObject.SetActive(true);
#endif
    }
}