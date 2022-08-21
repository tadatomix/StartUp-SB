using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Animations;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Storyboarding3d;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using System.Globalization;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace StorybrewScripts
{
    public class Anim : StoryboardObjectGenerator
    {
        public override void Generate()
        {
            var v = GetLayer("Vignette").CreateSprite("sb/v.png");
            v.Fade(0, 1);
            v.Fade(312855, 329035, 1, 0);
            v.Scale(0, 0.6);
            
            #region Background

            Action<int, int, int> ColorBackground = (startTime, endTime, endFade) =>
            {
                var sprite0 = GetLayer("").CreateSprite("st up.jpg");
                sprite0.Fade(0, 0);

                var pixel = GetLayer("Background").CreateSprite("sb/p.png");
                pixel.Color(startTime, new Color4(1, 176, 193, 1));
                pixel.ScaleVec(startTime, 854, 480);
                pixel.Fade(startTime, 1);
                pixel.Fade(48502 - 100, 48502, 1, 0);
                pixel.Fade(113221 - 100, 113221, 0, 1);
                pixel.Fade(210300 - 100, 210300, 1, 0);
                pixel.Fade(237266 - 100, 237266, 0, 1);
                pixel.Fade(endTime, endFade, 1, 0);
            };
    
           ColorBackground(0, 312855, 329035);

           #endregion

           #region Girl
            
            var sprite = GetLayer("Girl").CreateSprite("sb/girl.png");
            sprite.Scale(10833, 0.29);

            var blur = GetLayer("Girl").CreateSprite("sb/blurgirl.png");
            blur.Scale(43193, 0.29);

            Action<int, int, int, int, bool> FloatingGirl = (start, end, fadeIn, fadeOut, kiaiTime) =>
            {
                sprite.Fade(start, start + fadeIn, 0, 1);
                sprite.Fade(OsbEasing.InOutSine, end, end + fadeOut, 1, 0);

                if (kiaiTime)
                {
                    var pos1 = new Vector2(320, 240);
                    var pos2 = new Vector2(Random(310, 330), Random(230, 250));
                    var pos3 = new Vector2(Random(310, 330), Random(230, 250));
                    var pos4 = new Vector2(Random(310, 330), Random(230, 250));
                    var rad1 = 0;
                    var rad2 = MathHelper.DegreesToRadians(Random(-2.5f, 2.5f));
                    var rad3 = MathHelper.DegreesToRadians(Random(-2.5f, 2.5f));

                    sprite.StartLoopGroup(start, (end + fadeOut - start) / 7500 + 1);
                    sprite.Move(OsbEasing.InOutSine, 0, 1875, pos1, pos2);
                    sprite.Move(OsbEasing.InOutSine, 1875, 3750, pos2, pos3);
                    sprite.Move(OsbEasing.InOutSine, 3750, 5625, pos3, pos4);
                    sprite.Move(OsbEasing.InOutSine, 5625, 7500, pos4, pos1);
                    sprite.Rotate(OsbEasing.InOutSine, 0, 2500, rad1, rad2);
                    sprite.Rotate(OsbEasing.InOutSine, 2500, 5000, rad2, rad3);
                    sprite.Rotate(OsbEasing.InOutSine, 5000, 7500, rad3, rad1);
                    sprite.EndGroup();
                }
            };

            Action<int, int, int, int> BlurGirl = (start, end, fadeIn1, fadeOut1) =>
            {
                blur.Fade(start, start + fadeIn1, 0, 1);
                blur.Fade(OsbEasing.InOutSine, end, end + fadeOut1, 1, 0);
            };

            FloatingGirl(10833, 43193, 2500, 2500, false);
            FloatingGirl(113221, 199597, 0, 5000, true);
            FloatingGirl(237166, 302069, 100, 10000, true);

            BlurGirl(43193, 48402, 2500, 100);
            BlurGirl(199597, 210200, 2500, 100);

           #endregion

            var beat = Beatmap.GetTimingPointAt(-37).BeatDuration;

            #region SideLight

            var leftLight = GetLayer("Sides").CreateSprite("sb/side.png", OsbOrigin.CentreLeft, new Vector2(-107, 240));
            var rightLight = GetLayer("Sides").CreateSprite("sb/side.png", OsbOrigin.CentreRight, new Vector2(747, 240));

            Action<int, int, double> SideFlashes = (startTime, endTime, opacity) =>
            {
                var duration = endTime - startTime;

                leftLight.StartLoopGroup(startTime, (int)(duration / beat / 2));
                leftLight.Fade(OsbEasing.Out, 0, beat * 2, opacity, 0);
                leftLight.EndGroup();

                rightLight.StartLoopGroup(startTime + beat, (int)((duration - beat) / beat / 2));
                rightLight.Fade(OsbEasing.Out, 0, beat * 2, opacity, 0);
                rightLight.EndGroup();

                leftLight.ScaleVec(startTime, 14, 8);
                leftLight.Color(startTime, Color4.Cyan);
                leftLight.Additive(startTime, endTime);

                rightLight.ScaleVec(startTime + beat, 14, 8);
                rightLight.Color(startTime + beat, Color4.SeaGreen);
                rightLight.Additive(startTime, endTime);
                rightLight.FlipH(startTime, endTime);
            };

            SideFlashes(113221, 155019, 0.5);
            SideFlashes(156367, 188727, 0.4);
            SideFlashes(237266, 257828, 0.6);
            SideFlashes(258839, 301985, 0.7);

            #endregion
            var font = LoadFont("sb/f", new FontDescription()
            {
                FontPath = "assetlibrary/RobotoMono.ttf",
                FontSize = 100,
                Color = Color4.White,
                TrimTransparency = false
            });
            GenerateOverlay(113221, 156367, font);
            GenerateOverlay(237266, 301985, font);
            
		    starsWorld(48502, 111873); 
            circleWorld(210300, 231873);   
        }
        public void starsWorld(double startTime, double endTime)
        {
            var beatDuration = Beatmap.GetTimingPointAt((int)startTime).BeatDuration;

            var scene = new Scene3d();
            var camera = new PerspectiveCamera();
            camera.PositionX
                .Add(startTime, 0)
                .Add(198030, 0)
                .Add(200853, -30, EasingFunctions.CubicInOut)
                .Add(203677, 0, EasingFunctions.CubicInOut);
                
            camera.PositionY
                .Add(startTime, 0)
                .Add(198030, 0)
                .Add(200853, 40, EasingFunctions.CubicInOut)
                .Add(203677, 0, EasingFunctions.CubicInOut);

            camera.PositionZ
                .Add(startTime, 100)
                .Add(198030, 50, EasingFunctions.CubicInOut)
                .Add(200853, 200, EasingFunctions.CubicInOut)
                .Add(203677, 100, EasingFunctions.CubicInOut);
            
            camera.NearClip.Add(startTime, 50); //ближе к камере начинает появляется
            camera.NearFade.Add(startTime, 100); //момент где уже полностью спрайт появился */

            camera.FarFade.Add(startTime, 2000); //спрайт начинает исчезать в далеке
            camera.FarClip.Add(startTime, 3300); //спрайт полностью исчезнет в далеке

            var parent = scene.Root;

            for(int i = 0; i < 3400; i++)
            {
                Vector3 RandEndPos = new Vector3(Random(-5024, 5024), Random(-3600, 3600), -i * 10);
                Vector3 RandEndPos2 = new Vector3(Random(-5024, 5024), Random(-3600, 3600), -i * 10);

                var Star = new Sprite3d()
                {
                    SpritePath = "sb/dot.png",
                    UseDistanceFade = true,
                    RotationMode = RotationMode.Fixed,
                };
                
                Star.ConfigureGenerators(g =>
                {
                    g.PositionDecimals = 5;
                    g.ScaleDecimals = 10;
                    g.ColorTolerance = 10;
                });
                Star.PositionX
                    .Add(startTime, RandEndPos2.X)
                    .Add(endTime, RandEndPos2.X, EasingFunctions.ExpoOut);
                Star.PositionY
                    .Add(startTime, RandEndPos2.Y)
                    .Add(endTime, RandEndPos2.Y, EasingFunctions.ExpoOut);
                Star.PositionZ
                    .Add(startTime, RandEndPos.Z + 4500)
                    .Add(70075, RandEndPos.Z + 6000)
                    .Add(91648, RandEndPos.Z + 6500)
                    .Add(102434, RandEndPos.Z + 8500)
                    .Add(107828, RandEndPos.Z + 12500)
                    .Add(110524, RandEndPos.Z + 16500)
                    .Add(endTime, RandEndPos.Z + 20625);
                    
                Star.SpriteScale.Add(startTime, 3f);
           
                parent.Add(Star);
            }

            scene.Generate(camera, GetLayer("Stars"), startTime, endTime, beatDuration / 8);
        }

        public void circleWorld(double startTime, double endTime)
        {
            var beatDuration = Beatmap.GetTimingPointAt((int)startTime).BeatDuration;

            var scene = new Scene3d();
            var camera = new PerspectiveCamera();
            camera.PositionX
                .Add(startTime, 0);
                
            camera.PositionY
                .Add(startTime, 0);

            camera.PositionZ
                .Add(startTime, 100);
            
            camera.NearClip.Add(startTime, 50); //ближе к камере начинает появляется
            camera.NearFade.Add(startTime, 100); //момент где уже полностью спрайт появился */

            camera.FarFade.Add(startTime, 2000); //спрайт начинает исчезать в далеке
            camera.FarClip.Add(startTime, 3300); //спрайт полностью исчезнет в далеке

            var parent = scene.Root;

            for(int i = 0; i < 3400; i++)
            {
                Vector3 RandEndPos = new Vector3(0, 0, -i * 150);
                Vector3 RandEndPos2 = new Vector3(0, 0, -i * 150);

                var circle = new Sprite3d()
                {
                    SpritePath = "sb/Ellipse.png",
                    UseDistanceFade = true,
                    RotationMode = RotationMode.Fixed,
                };
                
                circle.ConfigureGenerators(g =>
                {
                    g.PositionDecimals = 5;
                    g.ScaleDecimals = 10;
                    g.ColorTolerance = 10;
                });
                circle.PositionX
                    .Add(startTime, RandEndPos2.X)
                    .Add(endTime, RandEndPos2.X, EasingFunctions.ExpoOut);
                circle.PositionY
                    .Add(startTime, RandEndPos2.Y)
                    .Add(endTime, RandEndPos2.Y, EasingFunctions.ExpoOut);
                circle.PositionZ
                    .Add(startTime, RandEndPos.Z + 20625)
                    .Add(221086, RandEndPos.Z + 19125)
                    .Add(226479, RandEndPos.Z + 17125)
                    .Add(229176, RandEndPos.Z + 13125)
                    .Add(endTime, RandEndPos.Z + 5125);
                    
                circle.SpriteScale.Add(startTime, 3f);
                circle.Coloring.Add(startTime, new Color4 (65,65,65,1));
           
                parent.Add(circle);
            }

            scene.Generate(camera, GetLayer("Circles"), startTime, endTime, beatDuration / 8);
        }
        void GenerateOverlay(int startTime, int endTime, FontGenerator font)
        {
            Action<int, int, Vector2, float> Timer = (start, end, position, scale) =>
            {
                // for (var i = 0; i < 10; i++)
                // {
                //     var num = font.GetTexture(i.ToString());

                //     var numPath = $"{MapsetPath}/sb/f/t_{i}.png";
                //     if (File.Exists(numPath)) File.Delete(numPath);
                    
                //     File.Move($"{MapsetPath}/{num.Path}", numPath);
                // }

                // var colon = font.GetTexture(":");
                // var colonPath = $"{MapsetPath}/sb/f/t.png";
                // if (File.Exists(colonPath)) File.Delete(colonPath);

                // File.Move($"{MapsetPath}/{colon.Path}", colonPath);

                var delay = new int[] { 60000, 0, 10000, 1000 };
                for (int i = 0; i < delay.Count(); i++)
                {
                    OsbSprite sprite;
                    if (delay[i] <= 0) sprite = GetLayer("Overlay").CreateSprite("sb/f/t.png", OsbOrigin.Centre, position + new Vector2(10 * i + scale, 1f));
                    else sprite = GetLayer("Overlay").CreateAnimation("sb/f/t_.png", delay[i] == 10000 ? 6 : 10, delay[i], OsbLoopType.LoopForever, OsbOrigin.Centre, position + new Vector2(10.3f * i, 0));

                    sprite.Scale(0, scale);
                    sprite.Fade(start, start + 1000, 0, 1);
                    sprite.Fade(end - 1000, end, 1, 0);
                }
            };

            Timer(startTime, endTime, new Vector2(-70, 35), 0.15f);
        }
    }
}
