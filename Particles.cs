using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using System;

namespace StorybrewScripts
{
    class Particles : StoryboardObjectGenerator
    {
        public override void Generate()
        {
            using (var pool = new OsbSpritePool(GetLayer(""), "sb/dot.png", OsbOrigin.Centre, false))
            {
                pool.MaxPoolDuration = 300000;

                Action<int, int> RadialParticles = (startTime, endTime) =>
                {
                    for (int i = startTime; i < endTime; i += 15)
                    {
                        var duration = Random(1000, 6000);
                        var angle = Random(0, Math.PI * 2);
                        var radius = Random(600, 900);
                        
                        var pos = new Vector2(
                            (int)(320 + Math.Cos(angle) * radius),
                            (int)(240 + Math.Sin(angle) * radius));

                        var sprite = pool.Get(i, i + duration);
                        sprite.Fade(i, i + 500, 0, 1);
                        sprite.Move(i, i + duration, new Vector2(320, 240), pos);
                        sprite.Scale(OsbEasing.InOutSine, i, i + duration, Random(0.2, 0.6), 0);
                    }
                };
                Action<int, int> RisingParticles = (startTime, endTime) =>
                {
                    for (int i = startTime; i < endTime; i += 30)
                    {
                        var duration = Random(800, 5000);
                        var startPos = new Vector2(Random(-107, 747), Random(200, 600));
                        var endPos = new Vector2(startPos.X - Random(-20, 20), Random(-100, 10));

                        var sprite = pool.Get(i, i + duration);
                        sprite.Fade(i, i + 500, 0, 1);
                        sprite.Move(i, i + duration, startPos, endPos);
                        sprite.Scale(OsbEasing.InOutSine, i, i + duration, Random(0.3, 0.8), 0);
                    }
                };
                
                RadialParticles(113305, 156451);
                RisingParticles(156451, 188811);
                RisingParticles(210384, 231957);
                RadialParticles(237350, 302069);
                RisingParticles(237350, 302069);
            }
            using (var pool = new OsbSpritePool(GetLayer("Squares"), "sb/p.png", OsbOrigin.Centre, false))
            {
                pool.MaxPoolDuration = 300000;

                Action<int, int, float> SquareParticles = (startTime, endTime, maxFade) =>
                {
                    for (int i = startTime; i < endTime - 2000; i += 40)
                    {
                        var duration = Random(1500, 5000);
                        var fade = Random(0.5f, maxFade);
                        var pos = new Vector2(Random(-157, 347), 240);
                        var endPos = new Vector2(Random(320, 767), Random(100, 380));

                        var sprite = pool.Get(i, i + duration);
                        sprite.Fade(i, i + 150, 0, fade);
                        sprite.Move((OsbEasing)Random(2), i, i + duration, pos, endPos);
                        sprite.Scale(i, Random(2, 10));
                        sprite.Fade(i + duration - 150, i + duration, fade, 0);
                        sprite.Rotate((OsbEasing)Random(2), i, i + duration, Random(-Math.PI / 4, Math.PI / 4), Random(-Math.PI / 4, Math.PI / 4));
                    }
                };
                
                SquareParticles(91732, 102518, 1);
                SquareParticles(199597, 210384, 0.7f);
            }
            using (var pool = new OsbSpritePool(GetLayer("Squares2"), "sb/p.png", OsbOrigin.Centre, false))
            {
                Action<int, int, float, bool> ExpandingSquareParticles = (startTime, endTime, maxFade, right) => 
                {
                    for (int i = startTime; i < endTime - 2000; i += 40)
                    {
                        var duration = Random(1000, 4000);
                        var fade = Random(0.5f, maxFade);
                        var pos = new Vector2(right ? 767 : -127, Random(-100, 600));
                        var endPos = new Vector2(320, 240);

                        var sprite = pool.Get(i, i + duration);
                        sprite.Fade(i, i + 150, 0, fade);
                        sprite.Move((OsbEasing)Random(2), i, i + duration, pos, endPos);
                        sprite.Scale(i, Random(2, 10));
                        sprite.Fade(i + duration - 150, i + duration, fade, 0);
                        sprite.Rotate((OsbEasing)Random(2), i, i + duration, Random(-Math.PI / 4, Math.PI / 4), Random(-Math.PI / 4, Math.PI / 4));
                    }
                };

                ExpandingSquareParticles(113305, 155103, 0.8f, true);
                ExpandingSquareParticles(113305, 155103, 0.8f, false);

                ExpandingSquareParticles(258923, 300721, 0.8f, true);
                ExpandingSquareParticles(258923, 300721, 0.8f, false);
            }
        }
    }
}