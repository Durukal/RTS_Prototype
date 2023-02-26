using System;
using Assets.Scripts;
using UnityEngine;

public abstract class Entity : MonoBehaviour {
    [Header("Vitality Values")]
    [SerializeField]
    internal int _Health;

    public int Health {
        get => _Health;
        set => _Health = value;
    }

    [SerializeField]
    internal int CurrentHealth;

    public bool IsDead = false;

    [Space(10)]
    [Header("Utilities")]
    public string Name;

    [SerializeField]
    private int _Faction;

    [SerializeField]
    private bool _IsAI;

    public bool IsAI {
        get => _IsAI;
        set => _IsAI = value;
    }

    public int Faction {
        get => _Faction;
        set => _Faction = value;
    }

    internal bool IsBuilding = false;
    internal bool IsUnit = false;
    public bool IsColliding = false;

    protected void Start() {
        CurrentHealth = Health;
        IsDead = false;
    }

    private void Update() { }

    public void Takedamage(int damage) {
        this.CurrentHealth -= damage;
        CheckDeath();
    }

    private void CheckDeath() {
        if (CurrentHealth <= 0) {
            IsDead = true;
            this.gameObject.SetActive(false);
            if (this.gameObject.layer == 8) {
                AstarPath.active.Scan();
            }

            OnDead();
        }
    }

    protected virtual void OnDead() { }

    protected void FactionCheck() {
        if (!IsAI) {
            Faction = (int)Enums.Factions.Allies;
        } else {
            Faction = (int)Enums.Factions.Enemies;
        }
    }
}