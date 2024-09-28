using UnityEngine;
using UnityEngine.UI;

public class TabBarUI : MonoBehaviour
{
    [SerializeField] private Button homeButton;

    [Header("Screens")] 
    [SerializeField] private GameObject HomeScreen;

    private GameObject previousScreen = default;
    private GameObject currentScreen = default;

    private void OnEnable()
    {
        homeButton.Select();
        currentScreen = HomeScreen;
    }

    public void ShowScreen(GameObject screenToSet)
    {
        previousScreen = currentScreen;
        currentScreen = screenToSet;
        
        previousScreen.SetActive(false);
        screenToSet.SetActive(true);
    }
}
