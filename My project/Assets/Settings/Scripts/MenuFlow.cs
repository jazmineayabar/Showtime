using UnityEngine;

public class MenuFlow : MonoBehaviour
{
    public DrillManager drillManager;

    public GameObject titlePanel;
    public GameObject instructionsPanel;
    public GameObject moveText;

    void Awake()
    {
        drillManager.MenuOpen = true;

        titlePanel.SetActive(true);
        instructionsPanel.SetActive(false);
        moveText.SetActive(false);
    }

    public void ShowInstructions()
    {
        titlePanel.SetActive(false);
        instructionsPanel.SetActive(true);
    }

    public void StartGame()
    {
        titlePanel.SetActive(false);
        instructionsPanel.SetActive(false);
        moveText.SetActive(true);

        drillManager.MenuOpen = false;
    }
}