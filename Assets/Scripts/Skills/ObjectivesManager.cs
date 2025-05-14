using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectivesManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _objectivePrefab;

    [SerializeField]
    private Transform _objectivePanel;

    public Dictionary<string, bool> SkillObjectives;

    private bool _brawlerValue;
    private bool _gunmenValue;
    private bool _rollerValue;

    private bool _noonValue;

    private void Awake()
    {
        SkillObjectives = new()
        {
            { "Slay " + GameManager.Instance.LevelData.MaxBrawlerCount + " brawlers", false },
            { "Slay " + GameManager.Instance.LevelData.MaxGunmanCount + " gunmen", false },
            { "Slay " + GameManager.Instance.LevelData.MaxRollerCount + " rollers", false },
            { "survive past noon", false }
        };
        SaveValues();
    }

    private void Start()
    {
        DestroyObjectives();
        CheckObjectiveValue();
        SaveValues();
    }

    // Save values into variables so that they unlocks can persist when switching between normal and easy mode
    private void SaveValues()
    {
        _brawlerValue = SkillObjectives[
            "Slay " + GameManager.Instance.LevelData.MaxBrawlerCount + " brawlers"
        ];
        _gunmenValue = SkillObjectives[
            "Slay " + GameManager.Instance.LevelData.MaxGunmanCount + " gunmen"
        ];
        _rollerValue = SkillObjectives[
            "Slay " + GameManager.Instance.LevelData.MaxRollerCount + " rollers"
        ];
        _noonValue = SkillObjectives["survive past noon"];
    }

    // Switch modes
    public void SwitchObjectives()
    {
        DestroyObjectives();
        UpdateDictionary();
        CheckObjectiveValue();
    }

    // Create new dictionary with updated values
    public void UpdateDictionary()
    {
        SkillObjectives = new()
        {
            {
                "Slay " + GameManager.Instance.LevelData.MaxBrawlerCount + " brawlers",
                _brawlerValue
            },
            { "Slay " + GameManager.Instance.LevelData.MaxGunmanCount + " gunmen", _gunmenValue },
            { "Slay " + GameManager.Instance.LevelData.MaxRollerCount + " rollers", _rollerValue },
            { "survive past noon", _noonValue }
        };
    }

    // Instantiate objectives when main menu is loaded both before and after game is played
    public void CheckObjectiveValue()
    {
        foreach (var (key, value) in SkillObjectives)
        {
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
        foreach (Transform child in _objectivePanel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    // Update objective values after game has ended
    public void UpdateObjectiveValue(string objectiveString)
    {
        if (
            SkillObjectives.ContainsKey(objectiveString)
            && SkillObjectives[objectiveString] == false
        )
        {
            SkillObjectives[objectiveString] = true;
        }
        SaveValues();
    }

    // Check if value is true or false
    public bool CheckValue(string key)
    {
        if (SkillObjectives[key])
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
        _objectivePrefab.GetComponent<TMP_Text>().text = objective;
        _objectivePrefab.GetComponent<TMP_Text>().fontStyle = FontStyles.Normal;
        Instantiate(_objectivePrefab, _objectivePanel);
    }

    // Instantiate objective with strikethrough
    private void InstantiateCompletedObjective(string objective)
    {
        _objectivePrefab.GetComponent<TMP_Text>().text = objective;
        _objectivePrefab.GetComponent<TMP_Text>().fontStyle = FontStyles.Strikethrough;
        Instantiate(_objectivePrefab, _objectivePanel);
    }
}
