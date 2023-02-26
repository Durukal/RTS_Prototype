using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class Knight : Unit {
    private void Awake() {
        Health = (int)Enums.UnitHealts.Knight;
        Damage = (int)Enums.UnitDamages.Knight;
        FactionCheck();
        TargetSet();
        IsUnit = true;
    }

    void Start() { }

    // Update is called once per frame
    void Update() {
        AttackTimer += Time.deltaTime;


        if (Faction == (int)Enums.Factions.Allies && IsSelected && AITarget.gameObject.activeSelf) {
            GetComponent<Pathfinding.AIDestinationSetter>().target = AITarget.transform;
        }
    }
}