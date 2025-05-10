using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectivesManager : MonoBehaviour
{
    [SerializeField]
    private GameObject objectivePrefab;

    [SerializeField]
    private Transform objectivePanel;

    public Dictionary<string, bool> skillObjectives = new();

    private void Awake()
    {
        skillObjectives.Add(
            "Slay " + GameManager.Instance.levelData.maxBrawlerCount + " brawlers",
            false
        );
        skillObjectives.Add(
            "Slay " + GameManager.Instance.levelData.maxGunmanCount + " gunmen",
            false
        );
        skillObjectives.Add(
            "Destroy " + GameManager.Instance.levelData.maxProjectileCount + " projectiles",
            true
        );
        skillObjectives.Add("Survive noon", false);
    }

    private void Start() { }

    private void OnEnable()
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
        if (
            skillObjectives.ContainsKey(objectiveString)
            && skillObjectives[objectiveString] == false
        )
        {
            skillObjectives[objectiveString] = true;
        }
        else
        {
            Debug.Log("dictionary: " + objectiveString + ", " + skillObjectives[objectiveString]);
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
        Debug.Log("objective: " + objective);
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
