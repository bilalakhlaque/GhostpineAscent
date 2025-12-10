using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameLogger : MonoBehaviour
{
    public static GameLogger Instance;

    [Header("UI References")]
    public Transform logContainer;
    public GameObject textPrefab;  

    [Header("Settings")]
    public int maxLogLines = 8;
    private List<GameObject> logLines = new List<GameObject>();

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void Log(string message)
    {
        // 1. Create text
        GameObject newText = Instantiate(textPrefab, logContainer);
        newText.GetComponent<TextMeshProUGUI>().text = $"> {message}";
        newText.transform.SetAsLastSibling(); // Add to bottom

        // 2. Add to list
        logLines.Add(newText);

        // 3. Cleanup old
        if (logLines.Count > maxLogLines)
        {
            Destroy(logLines[0]);
            logLines.RemoveAt(0);
        }
    }
}