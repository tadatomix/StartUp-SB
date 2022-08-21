# StartUp-SB

Storyboard this [map](https://osu.ppy.sh/b/3758152) made by [me](https://osu.ppy.sh/users/18012118) and [Nolife99](https://osu.ppy.sh/users/21286857)

requires [storybrew](https://github.com/Damnae/storybrew) to run

# Notes:

What Nolife did:
- used Action<> delegate to reduce sprite usage, easier OsbSpritePool usage, and not use new OsbSprite
- used #region to decrease my messiness
- used a input lyrics file for instant update and for timing reasons

What I did:
- make a fork of his SB
- Added some 3D parts (stars at the beginning & circles before the drop)
- Rewrite lyrics script (based on Damnae's karaoke script)
- Copied Kiai from Nolife's SB
- Added a timer
