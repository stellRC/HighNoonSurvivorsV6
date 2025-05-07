using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// HANDLE ALL PLAYER SKILLS
public class PlayerSkills
{
    public event EventHandler<OnSkillUnlockedEventArgs> OnSkillUnlocked;

    public class OnSkillUnlockedEventArgs : EventArgs
    {
        public SkillType skillType;
    }

    public enum SkillType
    {
        None,
        Earthshatter,
        Electrocute,
        SpeedBoost,
        ThrowOverarm
    }

    private List<SkillType> unlockedSkillTypeList;

    public PlayerSkills()
    {
        unlockedSkillTypeList = new List<SkillType>();
    }

    private void UnlockSkill(SkillType skillType)
    {
        if (!IsSkillUnlocked(skillType))
        {
            unlockedSkillTypeList.Add(skillType);
            OnSkillUnlocked.Invoke(this, new OnSkillUnlockedEventArgs { skillType = skillType });
        }
    }

    // Test if skill type has been unlocked
    public bool IsSkillUnlocked(SkillType skillType)
    {
        return unlockedSkillTypeList.Contains(skillType);
    }

    public bool TryUnlockSkill(SkillType skillType)
    {
        Debug.Log("unlocked: " + skillType);

        UnlockSkill(skillType);
        return true;
    }

    public bool CanUnlockSkill(SkillType skillType)
    {
        bool objectiveRequirement = IsSkillUnlocked(skillType);

        if (objectiveRequirement != false)
        {
            return true;
        }
        return false;
    }
}
