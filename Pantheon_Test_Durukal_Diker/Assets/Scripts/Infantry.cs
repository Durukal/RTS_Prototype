using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class Infantry : Unit {
    private void Awake() {
        Health = (int)Enums.UnitHealts.Infantry;
        Damage = (int)Enums.UnitDamages.Infantry;
        FactionCheck();
        TargetSet();
        IsUnit = true;
    }

    void Update() {
        AttackTimer += Time.deltaTime;


        if (Faction == (int)Enums.Factions.Allies && IsSelected && AITarget.gameObject.activeSelf) {
            GetComponent<Pathfinding.AIDestinationSetter>().target = AITarget.transform;
        }
    }
}