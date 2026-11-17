using UnityEngine;

public class DrillMarcher : MonoBehaviour
{
    public bool isPlayer;

    public Vector2[] dots = new Vector2[4];

    public void PlaceAtStart()
    {
        transform.position = ToWorld(dots[0]);
    }

    public Vector2 ExpectedPosition(int moveIndex, float progress)
    {
        return Vector2.Lerp(
            dots[moveIndex],
            dots[moveIndex + 1],
            progress
        );
    }

    public void ApplyMove(int moveIndex, float progress)
    {
        if (isPlayer)
            return;

        transform.position =
            ToWorld(ExpectedPosition(moveIndex, progress));
    }

    Vector3 ToWorld(Vector2 dot)
    {
        return new Vector3(dot.x, dot.y, 0);
    }
}