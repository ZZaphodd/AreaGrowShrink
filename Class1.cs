using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace AreaGrowShrink
{
    [StaticConstructorOnStartup]
    public static class MyMod_Startup
    {
        static MyMod_Startup()
        {
            var harmony = new Harmony("kkmbaek");
            harmony.PatchAll();

            Log.Message("[MyMod] Harmony patch applied");
        }
    }
    /*

    [HarmonyPatch(typeof(Pawn), "GetGizmos")]
    public static class Pawn_GetGizmos_Patch
    {
        public static IEnumerable<Gizmo> Postfix(IEnumerable<Gizmo> __result, Pawn __instance)
        {
            foreach (Gizmo g in __result)
                yield return g;
            Area a = __instance.playerSettings.AreaRestrictionInPawnCurrentMap;

            yield return new Command_Action
            {
                defaultLabel = "Grow",
                defaultDesc = "Use area Grow tool on this pawn's allowed area.",
                icon = ContentFinder<Texture2D>.Get("UI/Designators/ExpandAreaAllowed"),
                action = () =>
                {
                    Designator_AreaAllowed.selectedArea = a;
                    Designator_AreaAllowedExpand des = new Designator_AreaAllowedExpand();

                    Find.DesignatorManager.Select(des);
                }
            };

            yield return new Command_Action
            {
                defaultLabel = "Shrink",
                defaultDesc = "Use area Shrink tool on this pawn's allowed area.",
                icon = ContentFinder<Texture2D>.Get("UI/Designators/ShrinkAreaAllowed"),
                action = () =>
                {
                    Designator_AreaAllowed.selectedArea = a;
                    Designator_AreaAllowedClear des = new Designator_AreaAllowedClear();
                    Find.DesignatorManager.Select(des);
                }
            };
        }
    }

    [HarmonyPatch(typeof(InspectPaneFiller), "DrawAreaAllowed")]
    public static class Patch_DrawAreaAllowed
    {
        static void Postfix(WidgetRow row, Pawn pawn)
        {
            if (pawn.playerSettings == null || !pawn.playerSettings.SupportsAllowedAreas || pawn.Faction != Faction.OfPlayer || pawn.HostFaction != null || (pawn.IsMutant && !pawn.mutant.Def.respectsAllowedArea))
            {
                return;
            }
            // 기존 row에 이어서 버튼 추가
            row.Gap(4f);

            if (row.ButtonText("+"))
            {
                Designator_AreaAllowed.selectedArea = pawn.playerSettings.AreaRestrictionInPawnCurrentMap;
                Find.DesignatorManager.Select(new Designator_AreaAllowedExpand());
            }

            if (row.ButtonText("-"))
            {
                Designator_AreaAllowed.selectedArea = pawn.playerSettings.AreaRestrictionInPawnCurrentMap;
                Find.DesignatorManager.Select(new Designator_AreaAllowedClear());
            }
        }
    }
     */

    [HarmonyPatch(typeof(InspectPaneFiller), "DrawInspectStringFor")]
    public static class Patch_DrawInspectStringFor
    {
        static void Postfix(ISelectable sel, Rect rect)
        {
            if (sel is Pawn)
            {
                Pawn pawn = (Pawn) sel;
                Area area = pawn.playerSettings?.AreaRestrictionInPawnCurrentMap;
                if (area != null)
                {
                    float buttonWidth = 24f;
                    float buttonHeight = 20f;
                    float gap = 2f;

                    Rect plusRect = new Rect(rect.xMax - buttonWidth - 4f, rect.y + 2f, buttonWidth, buttonHeight);
                    if (Widgets.ButtonText(plusRect, "+"))
                    {
                        Designator_AreaAllowed.selectedArea = area;
                        Find.DesignatorManager.Select(new Designator_AreaAllowedExpand());
                    }

                    Rect minusRect = new Rect(rect.xMax - buttonWidth - 4f, plusRect.yMax + gap, buttonWidth, buttonHeight);
                    if (Widgets.ButtonText(minusRect, "-"))
                    {
                        Designator_AreaAllowed.selectedArea = area;
                        Find.DesignatorManager.Select(new Designator_AreaAllowedClear());
                    }
                }
            }
        }
    }
}