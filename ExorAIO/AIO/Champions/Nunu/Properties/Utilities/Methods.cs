namespace ExorAIO.Champions.Nunu
{
    using LeagueSharp;
    using LeagueSharp.SDK;

    /// <summary>
    ///     The methods class.
    /// </summary>
    internal class Methods
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Sets the methods.
        /// </summary>
        public static void Initialize()
        {
            Game.OnUpdate += Nunu.OnUpdate;
            Variables.Orbwalker.OnAction += Nunu.OnAction;
        }

        #endregion
    }
}