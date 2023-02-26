using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class Peasant : Unit {
    private void Awake() {
        Health = (int)Enums.UnitHealts.Peasant;
        Damage = (int)Enums.UnitDamages.Peasant;
        FactionCheck();
        TargetSet();
        IsUnit = true;
    }

    void FixedUpdate() {
        AttackTimer += Time.fixedDeltaTime;


        if (Faction == (int)Enums.Factions.Allies && IsSelected && AITarget.gameObject.activeSelf) {
            GetComponent<Pathfinding.AIDestinationSetter>().target = AITarget.transform;
        }
    }
}