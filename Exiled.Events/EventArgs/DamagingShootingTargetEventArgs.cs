// -----------------------------------------------------------------------
// <copyright file="DamagingShootingTargetEventArgs.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.EventArgs
{
    using System;

    using Exiled.API.Features;
    using Exiled.API.Features.Items;

    using PlayerStatsSystem;

    using UnityEngine;

    /// <summary>
    /// Contains all information before a player damages a shooting target.
    /// </summary>
    public class DamagingShootingTargetEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DamagingShootingTargetEventArgs"/> class.
        /// </summary>
        /// <param name="player"><inheritdoc cref="Player"/></param>
        /// <param name="damage"><inheritdoc cref="Amount"/></param>
        /// <param name="distance"><inheritdoc cref="Distance"/></param>
        /// <param name="shootingTarget"><inheritdoc cref="ShootingTarget"/></param>
        /// <param name="damageHandler"><inheritdoc cref="Item"/></param>
        /// <param name="hitLocation"><inheritdoc cref="HitLocation"/></param>
        /// <param name="isAllowed"><inheritdoc cref="IsAllowed"/></param>
        public DamagingShootingTargetEventArgs(Player player, float damage, float distance, Vector3 hitLocation, AdminToys.ShootingTarget shootingTarget, DamageHandlerBase damageHandler, bool isAllowed = true)
        {
            Player = player;
            Amount = damage;
            Distance = distance;
            ShootingTarget = ShootingTarget.Get(shootingTarget);
            Item = player.CurrentItem;
            DamageHandler = (AttackerDamageHandler)damageHandler;
            HitLocation = hitLocation;
            IsAllowed = isAllowed;
        }

        /// <summary>
        /// Gets the player who's damaging the shooting target.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets or sets the damage amount.
        /// </summary>
        public float Amount { get; set; }

        /// <summary>
        /// Gets the distance between the shooter and the shooting target.
        /// </summary>
        public float Distance { get; }

        /// <summary>
        /// Gets the shooting target which is being damaged.
        /// </summary>
        public ShootingTarget ShootingTarget { get; }

        /// <summary>
        /// Gets the item which is being used to deal the damage.
        /// </summary>
        public Item Item { get; }

        /// <summary>
        /// Gets the <see cref="AttackerDamageHandler"/>.
        /// </summary>
        public AttackerDamageHandler DamageHandler { get; }

        /// <summary>
        /// Gets the exact world location the bullet impacted the target.
        /// </summary>
        public Vector3 HitLocation { get; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the target can be damaged.
        /// </summary>
        public bool IsAllowed { get; set; }
    }
}
