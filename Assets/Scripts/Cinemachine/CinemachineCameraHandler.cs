using UnityEngine;
using Unity.Cinemachine;
public class CinemachineCameraHandler : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cineMachineCamera;
    private float NORMAL_ORTHOGRAPHIC_SIZE = 10f;
    private float targetOrthographicSize = 10f;

    private void Update()
    {
        float zoomSpeed = 2f;
        this.cineMachineCamera.Lens.OrthographicSize = Mathf.Lerp(this.cineMachineCamera.Lens.OrthographicSize, this.targetOrthographicSize, zoomSpeed * Time.deltaTime);
    }
    public void levelLoaded(GameLevel gameLevel)
    {
        this.cineMachineCamera.Target.TrackingTarget = gameLevel.getCameraStartPosition();
        this.setTargetOrthographicSize(gameLevel.getZoomedOutOrthographicSize());
    }

    public void levelStarted()
    {
        this.cineMachineCamera.Target.TrackingTarget = Lander.Instance.transform;
        this.setTargetOrthographicSize(this.NORMAL_ORTHOGRAPHIC_SIZE);
    }

    public void setTargetOrthographicSize(float targetOrthographicSize)
    {
        this.targetOrthographicSize = targetOrthographicSize;
    }

}
