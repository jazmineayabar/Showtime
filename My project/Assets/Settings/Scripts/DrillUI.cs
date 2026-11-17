using TMPro;
using UnityEngine;

public class DrillUI : MonoBehaviour
{
    public DrillManager drillManager;
    public TMP_Text moveText;

    void LateUpdate()
    {
        if (drillManager == null || moveText == null)
            return;

        bool finished =
            drillManager.CurrentMove == 2 &&
            drillManager.Progress >= 1f;

        if (finished)
        {
            moveText.text =
                "Drill complete!\nPress Enter to restart";
        }
        else if (!drillManager.IsRunning)
        {
            moveText.text =
                "Move 1 of 3\nPress Enter to begin";
        }
        else
        {
            int displayMove = drillManager.CurrentMove + 1;

            int count = Mathf.FloorToInt(
                drillManager.Progress * drillManager.countsPerMove
            ) + 1;

            count = Mathf.Clamp(
                count, 1, drillManager.countsPerMove
            );

            moveText.text =
                $"Move {displayMove} of 3\n" +
                $"Count: {count} / {drillManager.countsPerMove}";
        }
    }
}