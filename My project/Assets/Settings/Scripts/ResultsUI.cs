using System.Text;
using TMPro;
using UnityEngine;

public class ResultsUI : MonoBehaviour
{
    public DrillScorer drillScorer;
    public GameObject resultsPanel;

    public TMP_Text resultsText;
    public TMP_Text totalsText;

    const string BestDrillKey = "Marching_BestDrill";
    const string BestOverallKey = "Marching_BestOverall";

    int bestDrill;
    int bestOverall;
    bool hasOverallRecord;
    bool runFinished;

    void Awake()
    {
        bestDrill = PlayerPrefs.GetInt(BestDrillKey, 0);
        bestOverall = PlayerPrefs.GetInt(BestOverallKey, 0);

        hasOverallRecord = PlayerPrefs.HasKey(BestOverallKey);
    }
    void OnEnable()
    {
        if (drillScorer == null)
            return;

        drillScorer.ResultsReady += ShowResults;
        drillScorer.ResultsCleared += ClearDisplay;
        drillScorer.ResultsChanged += RefreshText;
    }

    void OnDisable()
    {
        if (drillScorer == null)
            return;

        drillScorer.ResultsReady -= ShowResults;
        drillScorer.ResultsCleared -= ClearDisplay;
        drillScorer.ResultsChanged -= RefreshText;
    }

    void ShowResults()
    {
        runFinished = true;

        RefreshText();
        resultsPanel.SetActive(true);
    }

    void ClearDisplay()
    {
        runFinished = false;
        resultsPanel.SetActive(false);
    }

    void UpdateHighScores()
    {
        if (!runFinished)
            return;

        bool changed = false;

        if (drillScorer.TotalDrillPoints > bestDrill)
        {
            bestDrill = drillScorer.TotalDrillPoints;

            PlayerPrefs.SetInt(BestDrillKey, bestDrill);
            changed = true;
        }

        if (drillScorer.AllMusicScoresReady &&
            (!hasOverallRecord ||
             drillScorer.OverallPoints > bestOverall))
        {
            bestOverall = drillScorer.OverallPoints;
            hasOverallRecord = true;

            PlayerPrefs.SetInt(BestOverallKey, bestOverall);
            changed = true;
        }

        if (changed)
            PlayerPrefs.Save();
    }
    void RefreshText()
    {
        if (resultsText == null || totalsText == null)
            return;

        UpdateHighScores();
        UpdateRoundColumn();
        UpdateTotalsColumn();
    }

    void UpdateRoundColumn()
    {
        StringBuilder text = new StringBuilder();

        text.AppendLine(
            "<size=40><b>PERFORMANCE RESULTS</b></size>"
        );

        foreach (DrillScorer.MoveResult result in drillScorer.Results)
        {
            text.AppendLine();

            text.AppendLine(
                $"<size=34><b><color=#80D8FF>" +
                $"ROUND {result.moveNumber}" +
                "</color></b></size>"
            );

            text.AppendLine("<indent=24>");

            text.AppendLine(
                $"Drill: {FormatScore(result.drillPoints, 100)}"
            );

            text.AppendLine(
                "<size=23><color=#CCCCCC>" +
                $"Distance from dot: {result.distanceFromDot:F2} yards" +
                "</color></size>"
            );

            if (result.hasMusicScore)
            {
                text.AppendLine(
                    $"Music: {FormatScore(result.musicPoints, 100)}"
                );

                text.AppendLine(
                    "<b>Round total:</b> " +
                    FormatScore(result.RoundTotal, 200)
                );
            }
            else
            {
                text.AppendLine(
                    "Music: <color=#CCCCCC>Pending</color>"
                );

                text.AppendLine(
                    "<b>Round total:</b> " +
                    "<color=#CCCCCC>Pending</color>"
                );
            }

            text.AppendLine("</indent>");
        }

        resultsText.text = text.ToString();
    }

    void UpdateTotalsColumn()
    {
        StringBuilder text = new StringBuilder();

        text.AppendLine("<align=center>");

        text.AppendLine(
            "<size=52><b><color=#80D8FF>" +
            "Final Score:</color></b></size>"
        );

        text.AppendLine();

        text.AppendLine("<size=34><b>DRILL</b></size>");

        text.AppendLine(
            "<size=56>" +
            FormatScore(drillScorer.TotalDrillPoints, 300) +
            "</size>"
        );

        text.AppendLine();

        text.AppendLine("<size=34><b>MUSIC</b></size>");

        if (drillScorer.AllMusicScoresReady)
        {
            text.AppendLine(
                "<size=56>" +
                FormatScore(drillScorer.TotalMusicPoints, 300) +
                "</size>"
            );
        }
        else
        {
            text.AppendLine(
                "<size=40><color=#CCCCCC>" +
                "Pending</color></size>"
            );
        }

        text.AppendLine();

        if (drillScorer.AllMusicScoresReady)
        {
            text.AppendLine(
                "<size=36><b>OVERALL SCORE:</b></size>"
            );

            text.AppendLine(
                "<size=76>" +
                FormatScore(drillScorer.OverallPoints, 600) +
                "</size>"
            );
        }
        else
        {
            text.AppendLine(
                "<size=36><b>OVERALL SCORE:</b></size>"
            );

            text.AppendLine(
                $"<size=76><b><color=#80D8FF>{drillScorer.OverallPoints}/600" +
                "</color></b></size>"
            );

            text.AppendLine(
                "<size=24><color=#CCCCCC>" +
                "</color></size>"
            );
        }

        text.AppendLine();

        text.AppendLine(
            "<size=24><color=#CCCCCC>" +
            "\nEnter: play again\n\nR: reset" +
            "</color></size>"
        );

        text.AppendLine();

        text.AppendLine(
            "<size=30><b><color=#80D8FF>" +
            "PERSONAL BEST</color></b></size>"
        );

        text.AppendLine(
            $"<size=28>Drill: <b>{bestDrill}/300</b></size>"
        );

        if (hasOverallRecord)
        {
            text.AppendLine(
                $"<size=28>Overall: <b>{bestOverall}/600</b></size>"
            );
        }
        else
        {
            text.AppendLine(
                "<size=24><color=#CCCCCC>" +
                "Overall: waiting for a complete music score" +
                "</color></size>"
            );
        }

        text.AppendLine("</align>");

        totalsText.text = text.ToString();
    }

    string FormatScore(int points, int maximum)
    {
        float percentage = (float)points / maximum;
        string color = GetScoreColor(percentage);

        return $"<color={color}><b>{points}/{maximum}</b></color>";
    }

    string GetScoreColor(float percentage)
    {
        if (percentage >= 0.9f)
            return "#69F0AE";

        if (percentage >= 0.7f)
            return "#FFD740";

        if (percentage >= 0.5f)
            return "#FFAB40";

        return "#FF8080";
    }
}