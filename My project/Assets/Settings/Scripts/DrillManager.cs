using UnityEngine;
using UnityEngine.InputSystem;

public class DrillManager : MonoBehaviour
{
    public DrillMarcher[] marchers;

    public float bpm = 120f;
    public int countsPerMove = 16;

    public int CurrentMove { get; private set; }
    public float Progress { get; private set; }
    public bool IsRunning { get; private set; }

    public bool MenuOpen { get; set; }

    float elapsed;

    void Start()
    {
        ResetDrill();
    }

    void Update()
    {
        if (MenuOpen)
            return;

        Keyboard keyboard = Keyboard.current;

        if (keyboard != null && keyboard.rKey.wasPressedThisFrame)
        {
            ResetDrill();
            return;
        }

        if (keyboard != null &&
            keyboard.enterKey.wasPressedThisFrame &&
            !IsRunning)
        {
            BeginDrill();
        }

        if (!IsRunning)
            return;

        float moveDuration = countsPerMove * 60f / bpm;
        elapsed += Time.deltaTime;

        bool finished = elapsed >= moveDuration * 3f;

        CurrentMove = Mathf.Min(
            Mathf.FloorToInt(elapsed / moveDuration), 2
        );

        Progress = finished
            ? 1f
            : (elapsed - CurrentMove * moveDuration) / moveDuration;

        foreach (DrillMarcher marcher in marchers)
        {
            marcher.ApplyMove(CurrentMove, Progress);
        }

        if (finished)
            IsRunning = false;
    }

    void ResetDrill()
    {
        elapsed = 0;
        CurrentMove = 0;
        Progress = 0;
        IsRunning = false;

        foreach (DrillMarcher marcher in marchers)
        {
            marcher.PlaceAtStart();
        }
    }

    public void BeginDrill()
    {
        if (IsRunning || MenuOpen)
            return;

        ResetDrill();
        IsRunning = true;
    }
}