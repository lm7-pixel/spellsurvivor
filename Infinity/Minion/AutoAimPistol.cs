﻿using fms.Faction;
using Godot;

namespace fms.Minion;

public partial class AutoAimPistol : MinionBase
{
    [ExportGroup("Internal Reference")]
    [Export]
    private PackedScene _bulletPackedScene = null!;

    [Export]
    private Node _bulletSpawnNode = null!;

    [Export]
    private CollisionShape2D _collisionShape = null!;

    [Export]
    private Area2D _searchArea = null!;

    // この Minion が所属する Faction の一覧
    private static readonly FactionBase[] _factions =
    {
        new Duelist(),
        new Bruiser()
    };

    private protected override int BaseCoolDownFrame => 120;

    public override FactionBase[] Factions => _factions;

    public override void _Ready()
    {
        base._Ready();
        UpdateRadius();
    }

    private protected override void DoAttack()
    {
        if (!TryGetNearestEnemy(out var enemy))
        {
            return;
        }

        // Fire to targetEnemy
        var direction = (enemy!.GlobalPosition - GlobalPosition).Normalized();

        // Spawn bullet
        var bullet = _bulletPackedScene.Instantiate<ProjectileBase>();
        {
            bullet.Damage = ItemSettings.BaseAttack;
            bullet.Direction = direction;
            bullet.GlobalPosition = GlobalPosition;
            bullet.InitialSpeed = 1000f;
        }
        _bulletSpawnNode.AddChild(bullet);
    }

    private bool TryGetNearestEnemy(out Enemy? nearestEnemy)
    {
        nearestEnemy = null;

        // Search near enemy
        var overlappingBodies = _searchArea.GetOverlappingBodies();
        if (overlappingBodies.Count <= 0)
        {
            return false;
        }

        // 最も近い敵を検索する
        var distance = 999f;
        foreach (var body in overlappingBodies)
        {
            if (body is Enemy e)
            {
                var d = GlobalPosition.DistanceTo(e.GlobalPosition);
                if (d < distance)
                {
                    distance = d;
                    nearestEnemy = e;
                }
            }
        }

        return nearestEnemy is not null;
    }

    private void UpdateRadius()
    {
        _collisionShape.Scale = new Vector2(ItemSettings.Range, ItemSettings.Range);
    }
}