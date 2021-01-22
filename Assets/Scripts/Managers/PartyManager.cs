﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public static PartyManager instance;

    public List<Unit> AllUnits;
    private void Awake()
    {
        instance = this;
    }

    public void AddUnit(Unit unit)
    {
        AllUnits.Add(unit);
    }

    public void SubUnit(Unit unit)
    {
        AllUnits.Remove(unit);
    }
}