using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Subtitles;
using System;
using System.IO;

namespace StorybrewScripts
{
    class Lyrics : StoryboardObjectGenerator
    {
        public override void Generate()
        {
            var file = $"{ProjectPath}/assetlibrary/Lyrics";
            AddDependency(file);

            var beat = Beatmap.GetTimingPointAt(46).BeatDuration;
            var PositionY = 20;
            int FadeTime = 200;

            var japFont = LoadFont("sb/f/jap", new FontDescription()
            {
                FontPath = $"{ProjectPath}/assetlibrary/NotoSansJP-Thin.otf",
                FontSize = 50,
                Color = Color4.White,
                TrimTransparency = true
            });

            var font = LoadFont("sb/f", new FontDescription()
            {
                FontPath = $"{ProjectPath}/assetlibrary/Nova.otf",
                FontSize = 50,
                Color = Color4.White,
                TrimTransparency = true
            });

            using (var pool = new OsbSpritePools(GetLayer("")))
            {
                pool.MaxPoolDuration = 400000;
                foreach (var line in File.ReadAllLines(file)) 
                {
                    int o;
                    var part = line.Split(new[]{' '}, 3);

                    if (!String.IsNullOrEmpty(part[0]) && int.TryParse(part[0], out o))
                    {
                        var startTime = int.Parse(part[0]);
                        var endTime = startTime;
                        var inDelay = 0;
                        var outDelay = 0;

                        var unicode = int.Parse(part[1]) == 1 ? true : false;
                        var text = part[2];

                        var scale = .35f;
                        var width = 0f;
                        var letterX = 0f;

                        foreach (var letter in text)
                        {
                            switch (letter) 
                            {
                                case '_': endTime += (int)beat; break;
                                case ',': endTime += (int)beat / 4 * 3; break;
                                case '-': endTime += (int)beat / 2; break;
                                case '/': endTime += (int)beat / 4; break;
                                default: width += font.GetTexture(letter.ToString()).BaseWidth * scale; break;
                            }
                        }
                        
                        foreach (var letter in text) 
                        {
                            if (letter != '_' && letter != ',' && letter != '-' && letter != '/') 
                            {
                                var texture = font.GetTexture(letter.ToString());
                                if (unicode) texture = japFont.GetTexture(letter.ToString());

                                if (!texture.IsEmpty) 
                                {
                                    var position = new Vector2(320 - width / 2 + letterX, PositionY) + texture.OffsetFor(OsbOrigin.Centre) * scale;

                                    var s = pool.Get(startTime + inDelay - FadeTime, endTime + FadeTime, texture.Path, OsbOrigin.Centre, false);
                                    if (s.ScaleAt(startTime + inDelay - FadeTime).X != scale) s.Scale(startTime + inDelay - FadeTime, scale);
                                    s.Move(OsbEasing.OutSine, startTime + inDelay - FadeTime, startTime + inDelay + 200, position.X, position.Y - 6, position.X, position.Y);
                                    s.Fade(startTime + inDelay - FadeTime, startTime + inDelay, 0, 1);
                                    s.Move(OsbEasing.OutCubic, endTime, endTime + FadeTime, position.X, position.Y, position.X, position.Y + 4);
                                    s.Fade(OsbEasing.OutCubic, endTime, endTime + FadeTime, 1, 0);
                                }
                                letterX += texture.BaseWidth * scale;
                                outDelay += 6;
                            } 
                            else 
                            {
                                switch (letter) 
                                {
                                    case '_': inDelay += (int)beat; break;
                                    case ',': inDelay += (int)beat / 4 * 3; break;
                                    case '-': inDelay += (int)beat / 2; break;
                                    case '/': inDelay += (int)beat / 4; break;
                                    
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}