using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using System;

namespace StorybrewScripts
{
    class HitObject : StoryboardObjectGenerator
    {
        public override void Generate()
        {
            using (var pool = new OsbSpritePool(GetLayer(""), "sb/p.png", OsbOrigin.Centre, (sprite, startTime, endTime) => 
            {
                sprite.Additive(0);
                sprite.Fade(0, 0);
            }))
            {
                pool.MaxPoolDuration = 300000;

                Action<int, int> Beam = (startTime, endTime) =>
                {
                    var lastPos = new Vector2(0, 0);
                    double lastDir = 0;
                    var scale = 8;
                    var fade = 0.6;

                    foreach (var hitobject in Beatmap.HitObjects)
                    {
                        if (hitobject.StartTime < startTime - 5 | endTime - 5 <= hitobject.StartTime) continue;

                        var angle = Math.Sqrt(Math.Pow(lastPos.X - hitobject.Position.X, 2) + 
                        Math.Pow(lastPos.Y - hitobject.Position.Y, 2)) > 10 ? Random(-0.1, 0.1) + Math.PI / 2 : lastDir - 0.1;

                        var sprite = pool.Get(hitobject.StartTime, hitobject.StartTime + 1000);
                        sprite.StartTriggerGroup("HitSound", hitobject.StartTime - 35, hitobject.StartTime + 35);
                        sprite.Move(0, hitobject.Position);
                        sprite.Rotate(0, Math.Round(angle, 2));
                        sprite.ScaleVec(OsbEasing.OutQuint, 0, 1000, 1000, scale, 1000, 0);
                        sprite.Fade(OsbEasing.OutExpo, 0, 1000, fade, 0.3);
                        sprite.EndGroup();

                        lastPos = hitobject.Position;
                        lastDir = angle;
                    }
                };

                Beam(102518, 111872);
                Beam(134878, 155103);
                Beam(188811, 199513);
                Beam(226563, 232125);
                Beam(258923, 301816);
            }
        }
    }
}