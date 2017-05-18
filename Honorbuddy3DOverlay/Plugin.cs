//!CompilerOption:AddRef:SlimDx.dll

using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ff14bot;
using ff14bot.AClasses;
using ff14bot.Enums;
using ff14bot.Interfaces;
using ff14bot.Managers;
using ff14bot.NeoProfiles;
using ff14bot.Objects;
using RB3DOverlay.Overlay;
using Vector3 = SlimDX.Vector3;
using NVector3 = Clio.Utilities.Vector3;

namespace RB3DOverlay
{
    public class Plugin : BotPlugin
    {
        private RenderForm _renderForm;

        public override void OnPulse()
        {
        }

        public override void OnInitialize()
        {
            
        }

        public override void OnShutdown()
        {
            Task.Run(OnDisableAsync);
        }

        public override void OnEnabled()
        {
            Task.Factory.StartNew(RunRenderForm, TaskCreationOptions.LongRunning);
        }

        private void RunRenderForm()
        {
            OverlayManager.Drawing += Drawing;

            IntPtr targetWindow = Core.Memory.Process.MainWindowHandle;

            _renderForm = new RenderForm(targetWindow);

            Application.Run(_renderForm);
        }

        public override void OnDisabled()
        {
            Task.Run(OnDisableAsync);
        }

        private async Task OnDisableAsync()
        {
            OverlayManager.Drawing -= Drawing;

            await _renderForm.ShutdownAsync();
        }

        public override string Name => "3D Overlay";
        public override string Description => "Reborn Buddy 3d Overlay";

        public override string Author => "The Buddy Team & ZZI";

        public override Version Version => new Version(1, 0, 0);

        public override string ButtonText => "Settings";

        public override bool WantButton => true;

        private SettingsForm _settingsForm;
        public override void OnButtonPress()
        {
            if (_settingsForm == null)
                _settingsForm = new SettingsForm();

            _settingsForm.ShowDialog();
        }

        private void Drawing(DrawingContext ctx)
        {
            OverlaySettings settings = OverlaySettings.Instance;

            if (settings.OnlyDrawInForeground &&
                Imports.GetForegroundWindow() != Core.Memory.Process.MainWindowHandle)
                return;

            if (!TreeRoot.IsRunning)
            {
                GameObjectManager.Update();
            }

            NVector3 mypos = Core.Me.Location;
            Vector3 vecStart = new Vector3(mypos.X, mypos.Y, mypos.Z);
            int myLevel = Core.Me.ClassLevel;


            if (settings.DrawGameStats)
            {
                StringBuilder sb = new StringBuilder();

                GameObject currentTarget = Core.Me.CurrentTarget;


                sb.AppendLine("My Position: " + Core.Me.Location);
                if (currentTarget != null)
                {
                    sb.AppendLine("Current Target: " + currentTarget.Name + ", Distance: " +
                                  Math.Round(currentTarget.Distance(), 3));

                    NVector3 end = currentTarget.Location;
                    Vector3 vecEnd = new Vector3(end.X, end.Y, end.Z);

                    ctx.DrawLine(vecStart, vecEnd, Color.DeepSkyBlue);
                }
                else
                {
                    sb.AppendLine("");
                }
                sb.AppendLine("");
                sb.AppendLine(string.Format("XP Per Hour: {0:F0}", GameStatsManager.XPPerHour));
                sb.AppendLine(string.Format("Deaths Per Hour: {0:F0}", GameStatsManager.DeathsPerHour));

                if (myLevel < 90)
                    sb.AppendLine(string.Format("Time to Level: {0}", GameStatsManager.TimeToLevel));

                sb.AppendLine(string.Format("TPS: {0:F2}", GameStatsManager.TicksPerSecond));

                sb.AppendLine();

                if (settings.UseShadowedText)
                {
                    ctx.DrawOutlinedText(sb.ToString(),
                                         settings.GameStatsPositionX,
                                         settings.GameStatsPositionY,
                                         settings.GameStatsForegroundColor,
                                         settings.GameStatsShadowColor,
                                         settings.GameStatsFontSize
                        );
                }
                else
                {
                    ctx.DrawText(sb.ToString(),
                                 settings.GameStatsPositionX,
                                 settings.GameStatsPositionY,
                                 settings.GameStatsForegroundColor,
                                 settings.GameStatsFontSize
                        );
                }

                ctx.DrawOutlinedBox(Core.Me.Location.Convert() + new Vector3(0, 1, 0), new Vector3(1),
                    Color.FromArgb(255, Color.Blue));
            }

            if (settings.DrawHostilityBoxes || settings.DrawUnitLines ||
                settings.DrawGameObjectBoxes || settings.DrawGameObjectLines)
            {

                foreach (GameObject obj in GameObjectManager.GameObjects.Where(i => i.Type == GameObjectType.GatheringPoint || i.Type == GameObjectType.BattleNpc || i.Type == GameObjectType.EventObject || i.Type == GameObjectType.Treasure || i.Type == GameObjectType.Pc))
                {
                    var name = obj.Name;
                    var vecCenter = obj.Location.Convert() + new Vector3(0, 1, 0);


                    var color = Color.FromArgb(150, Color.Blue);

                    //some generic objects. If you want to add a specific object it should probably go here or in it's own block below this.
                    if (obj.Type == GameObjectType.GatheringPoint || obj.Type == GameObjectType.EventObject ||
                        obj.Type == GameObjectType.Treasure)
                    {
                        if (obj.Type == GameObjectType.GatheringPoint)
                            color = Color.FromArgb(150, Color.BlueViolet);
                        if (obj.Type == GameObjectType.EventObject)
                            color = Color.FromArgb(150, Color.Fuchsia);
                        if (obj.Type == GameObjectType.Treasure)
                            color = Color.SandyBrown;

                        if (settings.DrawGameObjectNames && !string.IsNullOrEmpty(name))
                            ctx.Draw3DText(name, vecCenter);

                        if (settings.DrawGameObjectBoxes)
                        {
                            ctx.DrawOutlinedBox(vecCenter, new Vector3(1),
                                Color.FromArgb(150, color));
                        }
                        //if (settings.DrawGameObjectLines)
                        //{
                        //    if (!settings.DrawGameObjectLinesLos || obj.InLineOfSight())
                        //        ctx.DrawLine(vecStart, vecCenter, Color.FromArgb(150, color));
                        //}
                    }

                    var u = obj as Character;
                    if (u != null)
                    {
                        var hostilityColor = Color.FromArgb(150, Color.Green);

                        if (u.StatusFlags.HasFlag(StatusFlags.Hostile))
                        {
                            hostilityColor = Color.FromArgb(150, Color.Red);

                            //if (settings.DrawAggroRangeCircles)
                            //    ctx.DrawCircle(vecCenter, u.MyAggroRange, 64,
                            //                   Color.FromArgb(75, Color.DeepSkyBlue));
                        }

                        if (u.StatusFlags == StatusFlags.None)
                            hostilityColor = Color.FromArgb(150, Color.Yellow);

                        if (u.StatusFlags.HasFlag(StatusFlags.Friend) ||
                            u.StatusFlags.HasFlag(StatusFlags.PartyMember) ||
                            u.StatusFlags.HasFlag(StatusFlags.AllianceMember))
                            hostilityColor = Color.FromArgb(150, Color.Green);
                        if (!string.IsNullOrEmpty(name) && settings.DrawGameObjectNames)
                            ctx.Draw3DText(name, vecCenter);
                        ctx.DrawOutlinedBox(vecCenter, new Vector3(1),
                            Color.FromArgb(255, hostilityColor));
                    }
                }
            }


            //Profile curProfile = ProfileManager.CurrentProfile;
            //if (curProfile != null)
            //{
            //    if (settings.DrawHotspots)
            //    {
            //        GrindArea ga = QuestState.Instance.CurrentGrindArea;
            //        if (ga != null)
            //        {
            //            if (ga.Hotspots != null)
            //            {
            //                foreach (Hotspot hs in ga.Hotspots)
            //                {
            //                    var p = hs.Position;
            //                    Vector3 vec = new Vector3(p.X, p.Y, p.Z);
            //                    ctx.DrawCircle(vec, 10.0f, 32, Color.FromArgb(200, Color.Red));

            //                    if (!string.IsNullOrWhiteSpace(hs.Name))
            //                        ctx.Draw3DText("Hotspot: " + hs.Name, vec);
            //                    else
            //                    {
            //                        ctx.Draw3DText("Hotspot", vec);
            //                    }
            //                }
            //            }
            //        }

            //        // This is only used by grind profiles.
            //        if (curProfile.HotspotManager != null)
            //        {
            //            foreach (NVector3 p in curProfile.HotspotManager.Hotspots)
            //            {
            //                Vector3 vec = new Vector3(p.X, p.Y, p.Z);
            //                ctx.DrawCircle(vec, 10.0f, 32, Color.FromArgb(200, Color.Red));
            //                ctx.Draw3DText("Hotspot", vec);
            //            }
            //        }
            //    }

            //    if (settings.DrawBlackspots)
            //    {
            //        foreach (Blackspot bs in BlackspotManager.GetAllCurrentBlackspots())
            //        {
            //            var p = bs.Location;
            //            Vector3 vec = new Vector3(p.X, p.Y, p.Z);
            //            ctx.DrawCircle(vec, bs.Radius, 32, Color.FromArgb(200, Color.Black));

            //            if (!string.IsNullOrWhiteSpace(bs.Name))
            //                ctx.Draw3DText("Blackspot: " + bs.Name, vec);
            //            else
            //            {
            //                ctx.Draw3DText("Blackspot: " + vec, vec);
            //            }
            //        }
            //    }
            //}
        }
    }
}
