
#pragma warning disable 1587

namespace ExorAIO.Champions.Cassiopeia
{
    using System;
    using System.Linq;

    using ExorAIO.Utilities;

    using LeagueSharp;
    using LeagueSharp.Data.Enumerations;
    using LeagueSharp.SDK;
    using LeagueSharp.SDK.UI;
    using LeagueSharp.SDK.Utils;

    /// <summary>
    ///     The logics class.
    /// </summary>
    internal partial class Logics
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        public static void Clear(EventArgs args)
        {
            if (Bools.HasSheenBuff())
            {
                return;
            }

            /// <summary>
            ///     The E Clear Logics.
            /// </summary>
            if (Vars.E.IsReady())
            {
                /// <summary>
                ///     The E JungleClear Logic.
                /// </summary>
                if (Targets.JungleMinions.Any())
                {
                    if (GameObjects.Player.ManaPercent
                        > ManaManager.GetNeededMana(Vars.E.Slot, Vars.Menu["spells"]["e"]["clear"])
                        && Vars.Menu["spells"]["e"]["clear"].GetValue<MenuSliderButton>().BValue)
                    {
                        DelayAction.Add(
                            Vars.Menu["spells"]["e"]["delay"].GetValue<MenuSlider>().Value,
                            () => { Vars.E.CastOnUnit(Targets.JungleMinions[0]); });
                    }
                }

                /// <summary>
                ///     The E LaneClear Logics.
                /// </summary>
                else if (Targets.Minions.Any())
                {
                    if (GameObjects.Player.ManaPercent
                        < ManaManager.GetNeededMana(Vars.E.Slot, Vars.Menu["spells"]["e"]["lasthit"])
                        && Vars.Menu["spells"]["e"]["lasthit"].GetValue<MenuSliderButton>().BValue)
                    {
                        DelayAction.Add(
                            Vars.Menu["spells"]["e"]["delay"].GetValue<MenuSlider>().Value,
                            () =>
                                {
                                    foreach (var minion in
                                        Targets.Minions.Where(
                                            m =>
                                            Vars.GetRealHealth(m)
                                            < (float)GameObjects.Player.GetSpellDamage(m, SpellSlot.E)
                                            + (m.HasBuffOfType(BuffType.Poison)
                                                   ? (float)
                                                     GameObjects.Player.GetSpellDamage(
                                                         m,
                                                         SpellSlot.E,
                                                         DamageStage.Empowered)
                                                   : 0)))
                                    {
                                        Vars.E.CastOnUnit(minion);
                                    }
                                });
                    }
                    else if (GameObjects.Player.ManaPercent
                             >= ManaManager.GetNeededMana(Vars.E.Slot, Vars.Menu["spells"]["e"]["clear"])
                             && Vars.Menu["spells"]["e"]["clear"].GetValue<MenuSliderButton>().BValue)
                    {
                        DelayAction.Add(
                            Vars.Menu["spells"]["e"]["delay"].GetValue<MenuSlider>().Value,
                            () =>
                                {
                                    foreach (var minion in
                                        Targets.Minions.Where(m => m.HasBuffOfType(BuffType.Poison)))
                                    {
                                        Vars.E.CastOnUnit(minion);
                                    }
                                });
                    }
                }
            }

            /// <summary>
            ///     The Q Clear Logic.
            /// </summary>
            if (Vars.Q.IsReady()
                && GameObjects.Player.ManaPercent
                > ManaManager.GetNeededMana(Vars.Q.Slot, Vars.Menu["spells"]["q"]["clear"])
                && Vars.Menu["spells"]["q"]["clear"].GetValue<MenuSliderButton>().BValue)
            {
                var qFarmLocation =
                    Vars.Q.GetCircularFarmLocation(
                        Targets.Minions.Where(m => !m.HasBuffOfType(BuffType.Poison)).ToList(),
                        Vars.Q.Width);

                /// <summary>
                ///     The Q JungleClear Logic.
                /// </summary>
                if (Targets.JungleMinions.Any())
                {
                    Vars.Q.Cast(Targets.JungleMinions[0].ServerPosition);
                }

                /// <summary>
                ///     The Q LaneClear Logic.
                /// </summary>
                else if (qFarmLocation.MinionsHit >= 2)
                {
                    Vars.Q.Cast(qFarmLocation.Position);
                }
            }

            /// <summary>
            ///     The W Clear Logic.
            /// </summary>
            if (Vars.W.IsReady()
                && GameObjects.Player.ManaPercent
                > ManaManager.GetNeededMana(Vars.W.Slot, Vars.Menu["spells"]["w"]["clear"])
                && Vars.Menu["spells"]["w"]["clear"].GetValue<MenuSliderButton>().BValue)
            {
                var wFarmLocation =
                    Vars.W.GetCircularFarmLocation(
                        Targets.Minions.Where(m => !m.HasBuffOfType(BuffType.Poison)).ToList(),
                        Vars.W.Width);

                /// <summary>
                ///     The W JungleClear Logic.
                /// </summary>
                if (Targets.JungleMinions.Any())
                {
                    Vars.W.Cast(Targets.JungleMinions[0].ServerPosition);
                }

                /// <summary>
                ///     The W LaneClear Logic.
                /// </summary>
                else if (wFarmLocation.MinionsHit >= 3)
                {
                    Vars.W.Cast(wFarmLocation.Position);
                }
            }
        }

        #endregion
    }
}