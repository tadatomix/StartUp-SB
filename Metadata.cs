using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Scripting;
using StorybrewCommon.Animations;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Subtitles;
using System;

namespace StorybrewScripts
{
    class Metadata : StoryboardObjectGenerator
    {
        public override void Generate()
        {
		    Box(10833, 329035);
        }
        void Box(int startTime, int endTime)
        {
            var box = GetLayer("").CreateSprite("sb/box.png", OsbOrigin.BottomLeft, new Vector2(-85, 460));
            box.Fade(startTime, 0.8);
            box.ScaleVec(OsbEasing.OutElasticHalf, startTime, startTime + 1000, 0, 0.4, 0.4, 0.4);
            box.ScaleVec(OsbEasing.OutQuad, endTime, endTime + 1000, 0.4, 0.4, 0, 0.4);

            var bar = GetLayer("Bar").CreateSprite("sb/p.png", OsbOrigin.BottomCentre, new Vector2(710, 205));
            bar.ScaleVec(startTime, 40, 190);
            bar.ScaleVec(OsbEasing.OutQuad, endTime, 330384, 40, 190, 40, 0);

            var scaleBar = GetLayer("Bar").CreateSprite("sb/p.png", OsbOrigin.BottomCentre, new Vector2(710, 200));
            scaleBar.Color(startTime, new Color4(1, 176, 193, 1));
            scaleBar.Color(312855, endTime, new Color4(1, 176, 193, 1), Color4.Black);

            Action<int, int> ProgressBar = (sTime, eTime) =>
            {
                scaleBar.Fade(OsbEasing.OutExpo, sTime, sTime + 500, 0, 1);
                scaleBar.ScaleVec(sTime, eTime, 30, 180, 30, 0);
            };

            ProgressBar(startTime, 43193);
            ProgressBar(48586, 111957);
            ProgressBar(113305, 134541);
            ProgressBar(134878, 155777);
            ProgressBar(156451, 188811);
            ProgressBar(188811, 199597);
            ProgressBar(199597, 231957);
            ProgressBar(237350, 258586);
            ProgressBar(258923, 302069);
            ProgressBar(302069, endTime);

            GenerateText(startTime, endTime, new Vector2(-28, 367), 0.6f, "Heavy", "PSYQUI ft. Such");
            GenerateText(startTime, endTime, new Vector2(166, 415), 0.4f, "Thin", "Start Up");

            Spectrum(startTime, endTime);
        }
        void Spectrum(int StartTime, int EndTime)
        {
            var MinimalHeight = 0.1f;
            Vector2 Scale = new Vector2(1, 15);
            float LogScale = 5;
            var Position = new Vector2(-80, 354);
            var Width = 378f;

            int BarCount = 50;
            int fftCount = BarCount * 2;

            var heightKeyframes = new KeyframedValue<float>[fftCount];
            for (var i = 0; i < fftCount; i++)
                heightKeyframes[i] = new KeyframedValue<float>(null);

            var timeStep = Beatmap.GetTimingPointAt(StartTime).BeatDuration / 8;
            var offset = timeStep * 0.2;
            
            for (var t = (double)StartTime; t <= EndTime; t += timeStep)
            {
                var fft = GetFft(t + offset, fftCount, null, OsbEasing.InExpo);
                for (var i = 0; i < fftCount; i++)
                {
                    var height = (float)Math.Log10(1 + fft[i] * LogScale) * Scale.Y;
                    if (height < MinimalHeight) height = MinimalHeight;

                    heightKeyframes[i].Add(t, height);
                }
            }
            var barWidth = Width / BarCount;
            for (var i = 0; i < BarCount; i++)
            {
                var keyframes = heightKeyframes[i];
                keyframes.Simplify1dKeyframes(1.5, h => h);

                var bar = GetLayer("").CreateSprite("sb/bar.png", OsbOrigin.BottomCentre, new Vector2(Position.X + i * barWidth, Position.Y));
                bar.Fade(StartTime, StartTime + 1000, 0, 1);
                bar.Fade(EndTime - 1000, EndTime, 1, 0);

                keyframes.ForEachPair(
                    (start, end) =>
                    {
                        bar.ScaleVec(start.Time, end.Time,
                        Scale.X, start.Value,
                        Scale.X, end.Value);
                    },
                    MinimalHeight,
                    s => s
                );
            }
        }
        void GenerateText(int startTime, int endTime, Vector2 Position, float scale, string fontName, string text)
        {
            var font = LoadFont($"sb/f/{fontName}", new FontDescription()
            {
                FontPath = $"assetlibrary/Uni Sans {fontName}.otf",
                FontSize = 50,
                Color = Color4.White,
                TrimTransparency = true
            });

            var offsetX = 0f;
            var delay = 0;
            var width = 0f;

            foreach (var letter in text)
            {
                var texture = font.GetTexture(letter.ToString());
                var letterPos = new Vector2(Position.X + offsetX, Position.Y) + texture.OffsetFor(OsbOrigin.Centre) * scale;
                
                if (!texture.IsEmpty)
                {
                    var sprite = GetLayer("").CreateSprite(texture.Path);
                    sprite.Move(OsbEasing.OutElasticHalf, startTime, startTime + 1000, new Vector2(Position.X - 10, letterPos.Y), letterPos);
                    sprite.Fade(startTime + delay, startTime + delay + 1000, 0, 1);
                    sprite.Fade(endTime - 1000, endTime, 1, 0);
                    sprite.Scale(startTime + delay, scale);
                }
                offsetX += texture.BaseWidth * scale;
                width += texture.BaseWidth * scale;

                delay += 20;
            }
        }
    }
}