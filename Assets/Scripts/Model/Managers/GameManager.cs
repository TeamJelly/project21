﻿using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Model.Managers
{
    public class GameManager
    {
        static GameManager instance;

        public List<Vector2Int> roomHistory = new List<Vector2Int>();
        public Room currentRoom = null;
        public Room[,] AllRooms = null;
        public List<Vector2Int>[] pathList = null;
        List<Unit> partyUnits = new List<Unit>();

        public static List<Unit> PartyUnits { get => Instance.partyUnits; }
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new GameManager();
                return instance;
            }
        }

        GameManager()
        {
            InitForTesting();
        }

        public static void Reset()
        {
            instance = new GameManager();
        }

        public static void AddPartyUnit(Unit unit)
        {
            PartyUnits.Add(unit);
            unit.Category = Category.Party;
        }

        public static void RemovePartyUnit(Unit unit)
        {
            PartyUnits.Remove(unit);
        }

        void InitForTesting()
        {
            partyUnits.Add(new Units.Unit_000
            {
                Name = "party1",
                Category = Category.Party,
                Position = new Vector2Int(1, 4)
            });
            partyUnits.Add(new Units.Unit_000
            {
                Name = "party2",
                Category = Category.Party,
                Position = new Vector2Int(1, 5)
            });
            partyUnits.Add(new Units.Unit_000
            {
                Name = "party3",
                Category = Category.Party,
                Position = new Vector2Int(1, 6)
            });
        }
    }
}