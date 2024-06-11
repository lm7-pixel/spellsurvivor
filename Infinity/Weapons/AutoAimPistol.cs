﻿using fms.Projectile;
using Godot;

namespace fms.Weapon;

public partial class AutoAimPistol : WeaponBase
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

    public override void _Ready()
    {
        // 範囲 100 px
        _collisionShape.Scale = new Vector2(100, 100);
    }

    private protected override void DoAttack(uint level)
    {
        if (!TryGetNearestEnemy(out var enemy))
        {
            return;
        }

        // Fire to targetEnemy
        var direction = (enemy!.GlobalPosition - GlobalPosition).Normalized();

        // Spawn bullet
        var bullet = _bulletPackedScene.Instantiate<SimpleBullet>();
        {
            bullet.BaseDamage = 34;
            bullet.InitialVelocity = direction;
            bullet.InitialPosition = GlobalPosition;
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
}