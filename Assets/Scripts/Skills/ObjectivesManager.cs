using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectivesManager : MonoBehaviour
{
    [SerializeField]
    private GameObject objectivePrefab;

    [SerializeField]
    private Transform objectivePanel;

    public Dictionary<string, bool> skillObjectives;

    private bool brawlerValue;
    private bool gunmenValue;
    private bool rollerValue;

    private bool noonValue;

    private void Awake()
    {
        skillObjectives = new()
        {
            { "Slay " + GameManager.Instance.levelData.maxBrawlerCount + " brawlers", false },
            { "Slay " + GameManager.Instance.levelData.maxGunmanCount + " gunmen", false },
            { "Slay " + GameManager.Instance.levelData.maxRollerCount + " rollers", false },
            { "survive past noon", false }
        };
        SaveValues();
    }

    private void SaveValues()
    {
        brawlerValue = skillObjectives[
            "Slay " + GameManager.Instance.levelData.maxBrawlerCount + " brawlers"
        ];
        gunmenValue = skillObjectives[
            "Slay " + GameManager.Instance.levelData.maxGunmanCount + " gunmen"
        ];
        rollerValue = skillObjectives[
            "Slay " + GameManager.Instance.levelData.maxRollerCount + " rollers"
        ];
        noonValue = skillObjectives["survive past noon"];
    }

    private void Start()
    {
        DestroyObjectives();
        CheckObjectiveValue();
    }

    public void SwitchObjectives()
    {
        DestroyObjectives();
        UpdateDictionary();
        CheckObjectiveValue();
    }

    public void UpdateDictionary()
    {
        skillObjectives = new()
        {
            {
                "Slay " + GameManager.Instance.levelData.maxBrawlerCount + " brawlers",
                brawlerValue
            },
            { "Slay " + GameManager.Instance.levelData.maxGunmanCount + " gunmen", gunmenValue },
            { "Slay " + GameManager.Instance.levelData.maxRollerCount + " rollers", rollerValue },
            { "survive past noon", noonValue }
        };
        SaveValues();
    }

    // Instantiate objectives when main menu is loaded both before and after game is played
    public void CheckObjectiveValue()
    {
        foreach (var (key, value) in skillObjectives)
        {
            Debug.Log(value);
            if (value == false)
            {
                InstantiateObjective(key);
            }
            else
            {
                InstantiateCompletedObjective(key);
            }
        }
    }

    // Destroy previous objectives so player can see what objectives have yet to be accomplished
    public void DestroyObjectives()
    {
        foreach (Transform child in objectivePanel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    // Update objective values after game has ended
    public void UpdateObjectiveValue(string objectiveString)
    {
        if (
            skillObjectives.ContainsKey(objectiveString)
            && skillObjectives[objectiveString] == false
        )
        {
            skillObjectives[objectiveString] = true;
        }
    }

    // Check if value is true or false
    public bool CheckValue(string key)
    {
        if (skillObjectives[key])
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Instantiate objective without strikethrough
    private void InstantiateObjective(string objective)
    {
        objectivePrefab.GetComponent<TMP_Text>().text = objective;
        objectivePrefab.GetComponent<TMP_Text>().fontStyle = FontStyles.Normal;
        Instantiate(objectivePrefab, objectivePanel);
    }

    // Instantiate objective with strikethrough
    private void InstantiateCompletedObjective(string objective)
    {
        objectivePrefab.GetComponent<TMP_Text>().text = objective;
        objectivePrefab.GetComponent<TMP_Text>().fontStyle = FontStyles.Strikethrough;
        Instantiate(objectivePrefab, objectivePanel);
    }
}
