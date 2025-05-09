using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectivesManager : MonoBehaviour
{
    [SerializeField]
    private GameObject objectivePrefab;

    [SerializeField]
    private Transform objectivePanel;

    private GameManager gameManager;

    public Dictionary<string, bool> skillObjectives;

    private void Awake()
    {
        gameManager = GetComponentInParent<GameManager>();

        skillObjectives = new()
        {
            { "Stay alive past noon", false },
            { "Slay 5 brawlers", false },
            { "Slay 10 gunmen", false },
            { "Destroy 15 projectiles", false }
        };
    }

    private void Start()
    {
        CheckObjectiveValue();
    }

    public void CheckObjectiveValue()
    {
        foreach (var (key, value) in skillObjectives)
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

    public void UpdateObjectiveValue(string objectiveString)
    {
        foreach (var (key, value) in skillObjectives)
        {
            if (key == objectiveString && value == false)
            {
                skillObjectives[objectiveString] = true;
            }
        }
    }

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

    private void InstantiateObjective(string objective)
    {
        objectivePrefab.GetComponent<TMP_Text>().text = objective;
        objectivePrefab.GetComponent<TMP_Text>().fontStyle = FontStyles.Normal;
        Instantiate(objectivePrefab, objectivePanel);
    }

    private void InstantiateCompletedObjective(string objective)
    {
        objectivePrefab.GetComponent<TMP_Text>().text = objective;
        objectivePrefab.GetComponent<TMP_Text>().fontStyle = FontStyles.Strikethrough;
        Instantiate(objectivePrefab, objectivePanel);
    }
}
