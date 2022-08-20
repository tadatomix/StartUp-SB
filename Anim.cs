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
            v.Scale(0, 0.7);
            
            #region Background

            Action<int, int, int> ColorBackground = (startTime, endTime, endFade) =>
            {
                var sprite0 = GetLayer("").CreateSprite("st up.jpg");
                sprite0.Fade(0, 0);

                var pixel = GetLayer("Background").CreateSprite("sb/p.png");
                pixel.Color(startTime, new Color4(1, 176, 193, 1));
                pixel.ScaleVec(startTime, 854, 480);
                pixel.Fade(startTime, 1);
                pixel.Fade(endTime, endFade, 1, 0);
            };
    
           ColorBackground(0, 312855, 329035);

           #endregion
           
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
                    .Add(endTime, RandEndPos.Z + 9000);
                    
                circle.SpriteScale.Add(startTime, 3f);
                circle.Coloring.Add(startTime, new Color4 (65,65,65,1));
           
                parent.Add(circle);
            }

            scene.Generate(camera, GetLayer("Circles"), startTime, endTime, beatDuration / 8);
        }
    }
}
