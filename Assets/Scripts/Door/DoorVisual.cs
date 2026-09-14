using UnityEngine;

public class DoorVisual : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void openDoor()
    {
        animator.SetBool("IsOpen", true);
    }
}
