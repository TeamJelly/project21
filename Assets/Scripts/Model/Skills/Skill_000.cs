﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
/// <summary>
/// 스킬 이름: 베기
/// </summary>
public class Skill_000 : Skill
{
    private int strengthToDamageRatio;

    public Skill_000()
    {
        number = 0;
        name = "베기";
        unitClass = UnitClass.Warrior;
        grade = Grade.Normal;
        skillImagePath = "HandMade/SkillImage/000_베기";
        description = "한칸 안에 있는 단일 적에게 데미지를 입힌다.";
        criticalRate = 5;
        reuseTime = 0;
        domain = Domain.SelectOne;
        target = Target.AnyUnit;
        APSchema = "3;010;101;010";
        RPSchema = "1;1";
        strengthToDamageRatio = 1;
    }

    public override void UseSkillToUnit(Unit owner, Unit unit)
    {
        Debug.LogError(name + " 스킬을 " + unit.name + "에 사용!");
        unit.GetDamage(owner.strength * strengthToDamageRatio + enhancedLevel * 2);
        base.UseSkillToUnit(owner,unit);
    }
}
