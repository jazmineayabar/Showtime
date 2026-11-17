using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 2f;
    public DrillManager drillManager;

    void Update()
    {
        if (drillManager == null || !drillManager.IsRunning)
            return;

        Keyboard keyboard = Keyboard.current;

        if (keyboard == null)
            return;

        Vector2 direction = Vector2.zero;
        //WASD
        if (keyboard.wKey.isPressed)
            direction.y += 1;

        if (keyboard.sKey.isPressed)
            direction.y -= 1;

        if (keyboard.dKey.isPressed)
            direction.x += 1;

        if (keyboard.aKey.isPressed)
            direction.x -= 1;

        Vector3 nextPosition =
            transform.position +
            (Vector3)(direction.normalized * speed * Time.deltaTime);

        nextPosition.x = Mathf.Clamp(nextPosition.x, -50f, 50f);
        nextPosition.y = Mathf.Clamp(nextPosition.y, -26.665f, 26.665f);
        nextPosition.z = 0;

        transform.position = nextPosition;
    }
}