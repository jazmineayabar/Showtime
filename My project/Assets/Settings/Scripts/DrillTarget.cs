/**using UnityEngine;

public class DrillTarget : MonoBehaviour
{
    public DrillManager drillManager;
    public DrillMarcher playerDrill;
    public NoteGenerator NoteGenerator;

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
}*/


using UnityEngine;

public class DrillTarget : MonoBehaviour
{
    public DrillManager drillManager;
    public DrillMarcher playerDrill;
    public NoteGenerator noteGenerator;

    int currentDot = -1;

    void OnEnable()
    {
        if (drillManager != null)
            drillManager.DrillReset += ResetTarget;
    }

    void OnDisable()
    {
        if (drillManager != null)
            drillManager.DrillReset -= ResetTarget;
    }

    void Start()
    {
        UpdateTarget(true);
    }

    void LateUpdate()
    {
        UpdateTarget(false);
    }

    void ResetTarget()
    {
        currentDot = -1;
        UpdateTarget(true);
    }

    void UpdateTarget(bool forceUpdate)
    {
        if (drillManager == null || playerDrill == null)
            return;

        int nextDot = drillManager.CurrentMove + 1;

        if (nextDot >= playerDrill.dots.Length)
            return;

        Vector2 destination = playerDrill.dots[nextDot];

        transform.position = new Vector3(
            destination.x,
            destination.y,
            0
        );

        // Generate notes only when the target changes
        if (forceUpdate || nextDot != currentDot)
        {
            currentDot = nextDot;

            if (noteGenerator != null)
            {
                noteGenerator.GenerateNotes(
                    playerDrill.transform,
                    transform
                );
            }
        }
    }
}
