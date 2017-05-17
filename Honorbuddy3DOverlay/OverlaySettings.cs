using System.ComponentModel;
using System.IO;
using ff14bot.Helpers;
using Color = System.Drawing.Color;

namespace RB3DOverlay
{
    public class OverlaySettings : JsonSettings
    {
        public OverlaySettings(string settingsPath)
            : base(settingsPath)
        {
        }

        private static OverlaySettings _instance;
        public static OverlaySettings Instance { get { return _instance ?? (_instance = new OverlaySettings(Path.Combine(CharacterSettingsDirectory, "RenderOverlay.json"))); }}

        #region Game Stats

        [DefaultValue(true)]
        [Category("Game Stats")]
        [DisplayName("Draw game stats")]
        [Description("Enables drawing game stats on scren.")]
        public bool DrawGameStats { get; set; }

        [DefaultValue(true)]
        [Category("Game Stats")]
        [DisplayName("Use shadowed text")]
        [Description("Drawing the game stats shadowed.")]
        public bool UseShadowedText { get; set; }


        [DefaultValue(12f)]
        [Category("Game Stats")]
        [DisplayName("Font size")]
        [Description("The font size to be used for drawing the stats.")]
        public float GameStatsFontSize { get; set; }


        [DefaultValue(40)]
        [Category("Game Stats")]
        [DisplayName("Position X")]
        [Description("The X position of the game stats.")]
        public int GameStatsPositionX { get; set; }


        [DefaultValue(100)]
        [Category("Game Stats")]
        [DisplayName("Position Y")]
        [Description("The Y position of the game stats.")]
        public int GameStatsPositionY { get; set; }

        [Category("Game Stats")]
        [DisplayName("Foreground color")]
        [Description("The foreground color of the game stats.")]
        public Color GameStatsForegroundColor
        {
            get { return Color.FromArgb(GameStatsForegroundColorArgb); }
            set { GameStatsForegroundColorArgb = value.ToArgb(); }
        }


        [Browsable(false)]
        [DefaultValue(-1)]
        public int GameStatsForegroundColorArgb { get; set; }

        [Category("Game Stats")]
        [DisplayName("Shadow color")]
        [Description("The shadow color of the game stats.")]
        public Color GameStatsShadowColor
        {
            get { return Color.FromArgb(GameStatsShadowColorArgb); }
            set { GameStatsShadowColorArgb = value.ToArgb(); }
        }


        [Browsable(false)]
        [DefaultValue(-16777216)]
        public int GameStatsShadowColorArgb { get; set; }

        #endregion

        #region General


        [DefaultValue(false)]
        [Category("General")]
        [DisplayName("Only draw in foreground")]
        [Description("Only draw when World of Warcraft is in the foreground.")]
        public bool OnlyDrawInForeground { get; set; }

        #endregion

        #region Misc


        [DefaultValue(true)]
        [Category("Misc")]
        [DisplayName("Draw current path")]
        [Description("Enables drawing the current path Honorbuddy is following.")]
        public bool DrawCurrentPath { get; set; }

        #endregion

        #region Units


        [DefaultValue(false)]
        [Category("Units")]
        [DisplayName("Draw unit names")]
        [Description("Enables drawing unit names in game")]
        public bool DrawUnitNames { get; set; }


        [DefaultValue(false)]
        [Category("Units")]
        [DisplayName("Draw aggro range")]
        [Description("Enables drawing of aggro range circles towards units.")]
        public bool DrawAggroRangeCircles { get; set; }


        [DefaultValue(true)]
        [Category("Units")]
        [DisplayName("Draw hostility boxes")]
        [Description("Enables drawing of hostility boxes on units.")]
        public bool DrawHostilityBoxes { get; set; }


        [DefaultValue(false)]
        [Category("Units")]
        [DisplayName("Draw unit lines")]
        [Description("Enables drawing lines to all units around the player.")]
        public bool DrawUnitLines { get; set; }


        [DefaultValue(false)]
        [Category("Units")]
        [DisplayName("Check los")]
        [Description("Toggles if you always want lines to be drawn only when you are in line of sight of units.")]
        public bool DrawUnitLinesLos { get; set; }

        #endregion

        #region Gameobjects


        [DefaultValue(false)]
        [Category("Gameobjects")]
        [DisplayName("Draw gameobject names")]
        [Description("Enables drawing game object names in game")]
        public bool DrawGameObjectNames { get; set; }


        [DefaultValue(true)]
        [Category("Gameobjects")]
        [DisplayName("Draw gameobject boxes")]
        [Description("Enables drawing of boxes around gameobjects.")]
        public bool DrawGameObjectBoxes { get; set; }


        [DefaultValue(false)]
        [Category("Gameobjects")]
        [DisplayName("Draw gameobject lines")]
        [Description("Enables drawing lines to all gameobjects around the player.")]
        public bool DrawGameObjectLines { get; set; }


        [DefaultValue(false)]
        [Category("Gameobjects")]
        [DisplayName("Check los")]
        [Description("Toggles if you always want lines to be drawn only when you are in line of sight of gameobjects.")]
        public bool DrawGameObjectLinesLos { get; set; }

        //[Category("Gameobjects")]
        //[DisplayName("Gameobjects color")]
        //[Description("The color of lines and boxes for gameobjects.")]
        //public Color GameobjectsColor
        //{
        //    get { return Color.FromArgb(GameobjectsColorArgb); }
        //    set { GameobjectsColorArgb = value.ToArgb(); }
        //}

        //[Setting]
        //[Browsable(false)]
        //[DefaultValue(-38476)] // Hot Pink
        //public int GameobjectsColorArgb { get; set; }

        #endregion

        #region Players


        [DefaultValue(false)]
        [Category("Players")]
        [DisplayName("Draw player names")]
        [Description("Enables drawing player names in game")]
        public bool DrawPlayerNames { get; set; }

        #endregion

        #region Profile


        [DefaultValue(false)]
        [Category("Profile")]
        [DisplayName("Draw hotspots")]
        [Description("Enables drawing the hotspots in the current profile.")]
        public bool DrawHotspots { get; set; }


        [DefaultValue(false)]
        [Category("Profile")]
        [DisplayName("Draw blackpots")]
        [Description("Enables drawing the blackspots in the current profile.")]
        public bool DrawBlackspots { get; set; }

        #endregion

        #region PvP


        [DefaultValue(false)]
        [Category("PvP")]
        [DisplayName("Draw BgBuddy mapboxes")]
        [Description("Enables drawing the mapboxes that BgBuddy uses.")]
        public bool DrawBgMapboxes { get; set; }

        #endregion
    }
}
