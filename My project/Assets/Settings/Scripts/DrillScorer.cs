using System;
using UnityEngine;

public class DrillScorer : MonoBehaviour
{
    [Serializable]
    public class MoveResult
    {
        public int moveNumber;
        public float distanceFromDot;
        public int drillPoints;

        public bool hasMusicScore;
        public int musicPoints;

        public int RoundTotal => drillPoints + musicPoints;
    }

    public DrillManager drillManager;
    public DrillMarcher playerDrill;

    [Min(0.01f)]
    public float zeroScoreDistance = 10f;

    public MoveResult[] Results { get; private set; }
        = CreateResults();

    public int TotalDrillPoints
    {
        get
        {
            int total = 0;

            foreach (MoveResult result in Results)
                total += result.drillPoints;

            return total;
        }
    }

    public int TotalMusicPoints
    {
        get
        {
            int total = 0;

            foreach (MoveResult result in Results)
                total += result.musicPoints;

            return total;
        }
    }

    public int OverallPoints =>
        TotalDrillPoints + TotalMusicPoints;

    public bool AllMusicScoresReady
    {
        get
        {
            foreach (MoveResult result in Results)
            {
                if (!result.hasMusicScore)
                    return false;
            }

            return true;
        }
    }

    public event Action ResultsReady;
    public event Action ResultsCleared;
    public event Action ResultsChanged;

    static MoveResult[] CreateResults()
    {
        MoveResult[] results = new MoveResult[3];

        for (int i = 0; i < results.Length; i++)
        {
            results[i] = new MoveResult
            {
                moveNumber = i + 1
            };
        }

        return results;
    }

    void OnEnable()
    {
        if (drillManager == null)
            return;

        drillManager.MoveCompleted += ScoreMove;
        drillManager.DrillFinished += FinishScoring;
        drillManager.DrillReset += ClearResults;
    }

    void OnDisable()
    {
        if (drillManager == null)
            return;

        drillManager.MoveCompleted -= ScoreMove;
        drillManager.DrillFinished -= FinishScoring;
        drillManager.DrillReset -= ClearResults;
    }

    void ScoreMove(int moveIndex)
    {
        if (playerDrill == null)
            return;

        Vector2 actualPosition = playerDrill.transform.position;
        Vector2 targetPosition = playerDrill.dots[moveIndex + 1];

        float distance = Vector2.Distance(
            actualPosition, targetPosition
        );

        float scoreFraction = Mathf.Clamp01(
            1f - distance / Mathf.Max(0.01f, zeroScoreDistance)
        );

        MoveResult result = Results[moveIndex];

        result.distanceFromDot = distance;
        result.drillPoints = Mathf.RoundToInt(
            scoreFraction * 100f
        );

        ResultsChanged?.Invoke();
    }

    public void SetMusicScore(int moveIndex, int points)
    {
        if (moveIndex < 0 || moveIndex >= Results.Length)
        {
            Debug.LogWarning("Music move index must be 0, 1, or 2.");
            return;
        }

        MoveResult result = Results[moveIndex];

        result.musicPoints = Mathf.Clamp(points, 0, 100);
        result.hasMusicScore = true;

        ResultsChanged?.Invoke();
    }

    void FinishScoring()
    {
        ResultsReady?.Invoke();
    }

    void ClearResults()
    {
        Results = CreateResults();
        ResultsCleared?.Invoke();
    }
}