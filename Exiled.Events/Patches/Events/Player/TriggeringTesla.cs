// -----------------------------------------------------------------------
// <copyright file="TriggeringTesla.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.Patches.Events.Player
{
    using System;
#pragma warning disable SA1313
    using System.Collections.Generic;
    using System.Reflection.Emit;

    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using Exiled.Events.Handlers;

    using HarmonyLib;

    using NorthwoodLib.Pools;

    using UnityEngine;

    using static HarmonyLib.AccessTools;

    using BaseTeslaGate = TeslaGate;
    using Player = Exiled.API.Features.Player;

    /// <summary>
    /// Patches <see cref="TeslaGateController.FixedUpdate"/>.
    /// Adds the <see cref="Handlers.Player.TriggeringTesla"/> event.
    /// </summary>
    [HarmonyPatch(typeof(TeslaGateController), nameof(TeslaGateController.FixedUpdate))]
    internal static class TriggeringTesla
    {
        private static bool Prefix(TeslaGateController __instance)
        {
            try
            {
                if (TeslaGate.TeslasValue.Count == 0)
                    return true;
                foreach (BaseTeslaGate baseTeslaGate in __instance.TeslaGates)
                {
                    if (!baseTeslaGate.isActiveAndEnabled || baseTeslaGate.InProgress)
                        continue;

                    if (baseTeslaGate.NetworkInactiveTime > 0f)
                    {
                        baseTeslaGate.NetworkInactiveTime = Mathf.Max(0f, baseTeslaGate.InactiveTime - Time.fixedDeltaTime);
                        continue;
                    }

                    TeslaGate teslaGate = TeslaGate.Get(baseTeslaGate);
                    bool inIdleRange = false;

                    foreach (Player player in Player.List)
                    {
                        if (!teslaGate.CanBeIdle(player))
                            continue;

                        TriggeringTeslaEventArgs ev = new TriggeringTeslaEventArgs(player, teslaGate);
                        Handlers.Player.OnTriggeringTesla(ev);

                        if (ev.IsInIdleRange && !inIdleRange)
                            inIdleRange = ev.IsInIdleRange;

                        if (ev.IsTriggerable)
                        {
                            teslaGate.Trigger();
                            break;
                        }
                    }

                    if (inIdleRange != teslaGate.IsIdling)
                        teslaGate.IsIdling = inIdleRange;
                }

                return false;
            }
            catch (Exception e)
            {
                API.Features.Log.Error($"Exiled.Events.Patches.Events.Player.TriggeringTesla: {e}\n{e.StackTrace}");
                return true;
            }
        }
    }
}
