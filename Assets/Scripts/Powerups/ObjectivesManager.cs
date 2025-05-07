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

    private void Awake()
    {
        skillObjectives = new()
        {
            { "Kill 10 Zombies", false },
            { "Kill 20 Zombies", false },
            { "Kill 30 Zombies", true },
            { "Kill 40 Zombies", false }
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
