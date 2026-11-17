using UnityEngine;

public class DrillTarget : MonoBehaviour
{
    public DrillManager drillManager;
    public DrillMarcher playerDrill;

    void LateUpdate()
    {
        if (drillManager == null || playerDrill == null)
            return;

        int nextDot = drillManager.CurrentMove + 1;

        Vector2 destination = playerDrill.dots[nextDot];

        transform.position = new Vector3(
            destination.x,
            destination.y,
            0
        );
    }
}