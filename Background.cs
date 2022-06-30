using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using System;

namespace StorybrewScripts
{
    class Background : StoryboardObjectGenerator
    {
        public override void Generate()
        {
		    ColorBackground(-50, 312855, 329035);
            FloatingGirl(10833, 43193, 2500, 2500);
            BlurGirl(43193, 91732);
            FloatingGirl(102518, 111957, 8000, 1000);
            FloatingGirl(113305, 199597, 0, 5000, true);
            BlurGirl(199597, 221170);
            FloatingGirl(221170, 302069, 2500, 10000, true);

            var beat = Beatmap.GetTimingPointAt(46).BeatDuration;

            #region Flashes

            var flash = GetLayer("Flash").CreateSprite("sb/p.png");
            flash.ScaleVec(102518, 854, 480);
            flash.Additive(102518);
            flash.Color(102518, Color4.White);

            Action<double, double, double, bool> Flashes = (startTime, endTime, fade, color) =>
            {
                flash.Fade(startTime, endTime, fade, 0);

                if (color)
                {
                    foreach (var hitobject in Beatmap.HitObjects)
                    {
                        if (hitobject.StartTime >= startTime - 1 && hitobject.StartTime <= startTime + 1)
                        {
                            flash.Color(startTime, hitobject.Color);
                        }
                    }
                }
                else
                {
                    flash.Color(startTime, Color4.White);
                }
            };

            flash.StartLoopGroup(102518, (int)(5394 / beat));
            flash.Fade(0, beat, 0.2, 0);
            flash.EndGroup();

            flash.StartLoopGroup(107912, (int)(2612 / (beat / 2)) + 1);
            flash.Fade(0, beat / 2 - 1, 0.2, 0);
            flash.EndGroup();

            flash.StartLoopGroup(110608, (int)(672 / (beat / 3)) + 1);
            flash.Fade(0, beat / 3 - 1, 0.2, 0);
            flash.EndGroup();

            for (double i = 111282; i < 111957; i += beat / 4)
            {
                Flashes(i, i + beat / 4, 0.25, true);
            }

            Flashes(113305, 114653, 0.8, false);
            Flashes(124091, 125440, 0.8, true);
            Flashes(134204, 134288, 0.8, true);
            Flashes(134541, 134625, 0.8, true);
            Flashes(134878, 136226, 0.8, false);
            Flashes(145664, 147013, 0.8, true);
            Flashes(155103, 155777, 0.8, false);
            Flashes(156451, 161844, 0.8, true);
            Flashes(178024, 179372, 0.6, false);
            Flashes(188136, 188221, 0.8, true);
            Flashes(188473, 188558, 0.8, true);

            flash.Color(188811, Color4.White);
            flash.StartLoopGroup(188811, (int)(5394 / beat));
            flash.Fade(0, beat, 0.2, 0);
            flash.EndGroup();
            
            flash.StartLoopGroup(194204, (int)(2612 / (beat / 2)) + 1);
            flash.Fade(0, beat / 2 - 1, 0.2, 0);
            flash.EndGroup();

            flash.StartLoopGroup(196900, (int)(672 / (beat / 3)) + 1);
            flash.Fade(0, beat / 3 - 1, 0.2, 0);
            flash.EndGroup();

            for (double i = 197575; i < 198247; i += beat / 4)
            {
                Flashes(i, i + beat / 4, 0.25, true);
            }

            Flashes(198249, 198586, 0.5, true);
            Flashes(198586, 198923, 0.5, true);
            Flashes(198923, 199260, 0.5, true);
            Flashes(199597, 204990, 0.8, false);

            flash.StartLoopGroup(210384, (int)(16095 / beat) + 1);
            flash.Fade(0, beat - 1, 0.2, 0);
            flash.EndGroup();

            flash.StartLoopGroup(226563, (int)(2697 / (beat / 2)));
            flash.Fade(0, beat / 2, 0.2, 0);
            flash.EndGroup();

            flash.StartLoopGroup(229260, (int)(1348 / (beat / 3)) + 1);
            flash.Fade(0, beat / 3 - 1, 0.2, 0);
            flash.EndGroup();

            for (double i = 230608; i < 231940; i += beat / 4)
            {
                Flashes(i, i + beat / 4, 0.25, true);
            }

            Flashes(231957, 236002, 0.8, false);
            Flashes(237350, 238698, 0.8, false);
            Flashes(248136, 249485, 0.8, true);
            Flashes(258923, 260271, 0.8, false);
            Flashes(268361, 269035, 0.6, true);
            Flashes(269035, 269709, 0.7, true);
            Flashes(269709, 271058, 0.8, true);
            Flashes(277799, 279148, 0.6, true);
            Flashes(279148, 280496, 0.7, true);
            Flashes(280496, 281844, 0.8, true);
            Flashes(289934, 290271, 0.4, true);
            Flashes(290271, 290608, 0.5, true);
            Flashes(290608, 290945, 0.6, true);
            Flashes(290945, 291282, 0.7, true);
            Flashes(291282, 292631, 0.8, true);
            Flashes(299372, 300046, 0.5, true);
            Flashes(300046, 300721, 0.6, true);
            Flashes(300721, 302069, 0.7, true);
            Flashes(302069, 303417, 0.8, false);

            #endregion

            Action<int, int, double> SideFlashes = (startTime, endTime, opacity) =>
            {
                var leftLight = GetLayer("Sides").CreateSprite("sb/side.png", OsbOrigin.CentreLeft, new Vector2(-107, 240));
                var rightLight = GetLayer("Sides").CreateSprite("sb/side.png", OsbOrigin.CentreRight, new Vector2(747, 240));
                var duration = endTime - startTime;

                leftLight.ScaleVec(startTime, 14, 8);
                leftLight.Color(startTime, Color4.Cyan);
                leftLight.Additive(startTime);

                leftLight.StartLoopGroup(startTime, (int)(duration / beat / 2));
                leftLight.Fade(OsbEasing.Out, 0, beat * 2, opacity, 0);
                leftLight.EndGroup();

                rightLight.ScaleVec(startTime + beat, 14, 8);
                rightLight.Color(startTime + beat, Color4.SeaGreen);
                rightLight.Additive(startTime + beat);
                rightLight.FlipH(startTime + beat);

                rightLight.StartLoopGroup(startTime + beat, (int)((duration - beat) / beat / 2));
                rightLight.Fade(OsbEasing.Out, 0, beat * 2, opacity, 0);
                rightLight.EndGroup();
            };

            SideFlashes(113305, 155103, 0.5);
            SideFlashes(156451, 188811, 0.4);
            SideFlashes(237350, 257827, 0.6);
            SideFlashes(258923, 302069, 0.7);
        }
        void ColorBackground(int startTime, int endTime, int endFade)
        {
            var sprite0 = GetLayer("Delete").CreateSprite("st up.jpg");
            sprite0.Fade(0, 0);

            var sprite = GetLayer("").CreateSprite("sb/p.png");
            sprite.Color(startTime, new Color4(1, 176, 193, 1));
            sprite.ScaleVec(startTime, 854, 480);
            sprite.Fade(startTime, 1);
            sprite.Fade(endTime, endFade, 1, 0);
        }
        void FloatingGirl(int startTime, int endTime, int fadeIn, int fadeOut, bool kiaiTime = false)
        {
            var sprite = GetLayer("Girl").CreateSprite("sb/girl.png");
            sprite.Fade(startTime, startTime + fadeIn, 0, 1);
            sprite.Scale(startTime, 0.12);
            sprite.Fade(OsbEasing.InOutSine, endTime, endTime + fadeOut, 1, 0);

            if (kiaiTime)
            {
                var pos1 = new Vector2(320, 240);
                var pos2 = new Vector2(Random(310, 330), Random(230, 250));
                var pos3 = new Vector2(Random(310, 330), Random(230, 250));
                var pos4 = new Vector2(Random(310, 330), Random(230, 250));
                var rad1 = 0;
                var rad2 = MathHelper.DegreesToRadians(Random(5));

                sprite.StartLoopGroup(startTime, (endTime + fadeOut - startTime) / 7500 + 1);
                sprite.Move(OsbEasing.InOutSine, 0, 1875, pos1, pos2);
                sprite.Move(OsbEasing.InOutSine, 1875, 3750, pos2, pos3);
                sprite.Move(OsbEasing.InOutSine, 3750, 5625, pos3, pos4);
                sprite.Move(OsbEasing.InOutSine, 5625, 7500, pos4, pos1);
                sprite.Rotate(OsbEasing.InOutSine, 0, 3750, rad1, rad2);
                sprite.Rotate(OsbEasing.InOutSine, 3750, 7500, rad2, rad1);
            }
        }
        void BlurGirl(int startTime, int endTime)
        {
            var sprite = GetLayer("Girl").CreateSprite("sb/blurgirl.png");
            sprite.Fade(startTime, startTime + 2500, 0, 1);
            sprite.Scale(startTime, 0.12);
            sprite.Fade(OsbEasing.InOutSine, endTime, endTime + 2500, 1, 0);
        }
    }
}
