using System;
using System.Collections.Generic;

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
        GroundSlam,
        ShockHeavy,
        Spin,
        SwordCombo
    }

    private List<SkillType> _unlockedSkillTypeList;

    public PlayerSkills()
    {
        _unlockedSkillTypeList = new List<SkillType>();
    }

    // Unlock skills, called through UI
    private void UnlockSkill(SkillType skillType)
    {
        if (!IsSkillUnlocked(skillType))
        {
            _unlockedSkillTypeList.Add(skillType);

            // Event is fired off when skill is unlocked
            OnSkillUnlocked?.Invoke(this, new OnSkillUnlockedEventArgs { skillType = skillType });
        }
    }

    // Test if skill type has been unlocked
    public bool IsSkillUnlocked(SkillType skillType)
    {
        return _unlockedSkillTypeList.Contains(skillType);
    }

    public bool TryUnlockSkill(SkillType skillType)
    {
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
