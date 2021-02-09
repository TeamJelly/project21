﻿using Common;
using Common.DB;
using Model.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public enum Grade { NULL, Normal, Rare, Legend, Boss }
    
    [System.Serializable]
    public class Skill
    {
        public int number;                                              // 스킬 번호
        public string name = "No Skill Name";                           // 스킬 이름
        public UnitClass unitClass = UnitClass.NULL;                    // 스킬 클래스
        public Grade grade = Grade.NULL;                                // 스킬 등급

        protected string spritePath;

        private Sprite sprite;
        public Sprite Sprite
        {
            get
            {
                if (sprite == null && spritePath != "")
                    sprite = Resources.Load<Sprite>(spritePath);
                return sprite;
            }
        }

        [TextArea(1, 10)]
        public string description;                                      // 스킬 설명
        public int enhancedLevel;                                       // 강화도
        public int reuseTime;                                           // 재사용 대기시간
        public int currentReuseTime;

        [Range(0, 100)]
        public float criticalRate;                                      // 크리티컬율

        public enum Type                                              // 스킬 사용가능 범위의 종류
        {
            NULL,
            Fixed,          // 나를 기준으로 범위가 회전하지 않는 스킬.
            Rotate,         // 나를 기준으로 범위가 회전하는 스킬.
        }

        public enum Target                                              // 스킬의 대상
        {
            NULL,
            Any,            // 모든 타일에 사용가능
            NoUnit,         // 유닛이 없는 곳에만 사용가능, 이동 혹은 소환류 스킬에 사용
            Party,
            Friendly,
            Enemy,
        }

        public Type type = Type.Fixed;          // Default : Fixed
        public Target target = Target.Any;      // Default : Any

        protected string APSchema;
        protected string RPSchema;              // relate Position
        public string extension; // 스킬 고유 특성

        public virtual bool IsUsable(Unit user)
        {
            if (user.SkillCount > 0 && currentReuseTime == 0)
                return true;
            else
                return false;
        }

        public virtual List<Vector2Int> GetAvailablePositions(Unit user, Vector2Int userPosition)
        {
            if (APSchema == null) return null;

            List<Vector2Int> positions = new List<Vector2Int>();

            foreach (var position in Common.Range.ParseRangeSchema(APSchema))
            {
                Vector2Int abs = userPosition + position;

                // 모든 타일에 사용가능
                if (target == Target.Any)
                    positions.Add(abs);
                // 유닛 없음타일에만 사용가능
                else if (target == Target.NoUnit &&
                    BattleManager.GetTile(position).IsUsable())
                    positions.Add(abs);
                // 파티 유닛에만 사용 가능
                else if (target == Target.Party &&
                    BattleManager.GetUnit(position)?.Category == Category.Party)
                    positions.Add(abs);
                // 우호적인 유닛에 사용 가능
                else if (target == Target.Friendly && (
                    BattleManager.GetUnit(position)?.Category == Category.Friendly ||
                    BattleManager.GetUnit(position)?.Category == Category.Party))
                    positions.Add(abs);
                // 적대적인 유닛에 사용 가능
                else if (target == Target.Enemy &&
                    BattleManager.GetUnit(position)?.Category == Category.Enemy)
                    positions.Add(abs);
                // 어디에도 속하지 않으면 false
                else
                    continue;
            }

            return positions;
        }

        public virtual List<Vector2Int> GetAvailablePositions(Unit user)
        {
            return GetAvailablePositions(user, user.Position);
        }

        // 메인 인디케이터의 위치가 position일때, 관련된 범위의 위치를 돌려줍니다.
        public virtual List<Vector2Int> GetRelatePositions(Unit user, Vector2Int position)
        {
            if (RPSchema == null) return null;

            List<Vector2Int> positions = new List<Vector2Int>();

            foreach (var vector in Common.Range.ParseRangeSchema(RPSchema))
            {
                Vector2Int abs = position + vector;
                positions.Add(abs);
            }

            return positions;
        }

        public virtual IEnumerator Use(Unit user, Vector2Int target)
        {
            Debug.LogError(name + " 스킬을 " + target + "에 사용!");
            currentReuseTime = reuseTime;
            yield return null;
        }


        /// <summary>
        /// 스킬을 초기화 합니다.
        /// </summary>
        /// <param name="skill_no">스킬번호</param>
        /// <returns></returns>
        protected void InitializeSkillFromDB(int skill_no)
        {
            var results = Query.Instance.SelectFrom<Skill>("skill_table", $"number={skill_no}").results;
            if (results.Length > 0)
            {
                var skill = results[0];
                number = skill.number;
                name = skill.name;
                unitClass = skill.unitClass;
                grade = skill.grade;
                spritePath = skill.spritePath;
                description = skill.description;
                criticalRate = skill.criticalRate;
                reuseTime = skill.reuseTime;
                type = skill.type;
                target = skill.target;
                APSchema = skill.APSchema;
                RPSchema = skill.RPSchema;
                extension = skill.extension;
            }
            else
            {
                Debug.LogError($"number={skill_no}에 해당하는 스킬이 없습니다.");
            }
        }

        /// <summary>
        /// 스킬 고유의 확장 스탯을 파싱합니다.
        /// </summary>
        /// <typeparam name="T">확장 클래스</typeparam>
        /// <param name="extension">확장용 스키마를 넣습니다</param>
        /// <returns>확장 클래스</returns>
        protected T ParseExtension<T>(string extension)
        {
            string[] stats = extension.Split(';');
            Dictionary<string, object> dict = new Dictionary<string, object>();

            foreach (var stat in stats)
            {
                var stat_split = stat.Split('=');
                var name = stat_split[0];
                var value = stat_split[1];
                dict.Add(name, value);
            }

            string jsonString = JSON.DictionaryToJsonString(dict);
            return JSON.ParseString<T>(jsonString);
        }
    }
}