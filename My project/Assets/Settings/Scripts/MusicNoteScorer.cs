using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicNoteScorer : MonoBehaviour
{
    public DrillManager drillManager;
    public DrillScorer drillScorer;

    public float maxAccuracyDistance = 1.5f;
    public float timingWindow = 0.25f;

    float[] roundPoints = new float[3];
    int[] notesHit = new int[3];

    void OnEnable()
    {
        drillManager.MoveCompleted += FinishRound;
        drillManager.DrillReset += ResetScores;
    }

    void OnDisable()
    {
        drillManager.MoveCompleted -= FinishRound;
        drillManager.DrillReset -= ResetScores;
    }

    public void ScoreNote(MarchingNote note, Transform player)
    {
        int round = drillManager.CurrentMove;

        float moveDuration =
            drillManager.countsPerMove * 60f / drillManager.bpm;

        // Calculate timing accuracy
        float timingError = Mathf.Abs(
            drillManager.Progress - note.expectedProgress
        ) * moveDuration;

        float timingScore = 50f * Mathf.Clamp01(
            1f - timingError / timingWindow
        );

        // Calculate physical accuracy
        float distance = Vector2.Distance(
            player.position,
            note.transform.position
        );

        float accuracyScore = 50f * Mathf.Clamp01(
            1f - distance / maxAccuracyDistance
        );

        roundPoints[round] += timingScore + accuracyScore;
        notesHit[round]++;
    }

    void FinishRound(int round)
    {
        int score = 0;

        if (notesHit[round] > 0)
        {
            score = Mathf.RoundToInt(
                roundPoints[round] / notesHit[round]
            );
        }

        drillScorer.SetMusicScore(round, score);
    }

    void ResetScores()
    {
        for (int i = 0; i < 3; i++)
        {
            roundPoints[i] = 0;
            notesHit[i] = 0;
        }
    }
}