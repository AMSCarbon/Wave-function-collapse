# Wave Function Collapse

I had seen the wave function collapse (WFC) algorithm described a few times. I wanted to implement it so I could have a proper understanding of how it works. The algorithm really clicked for me when I watched [Martin Donald's Youtube video](https://www.youtube.com/watch?v=2SuvO4Gi7uY&ab_channel=MartinDonald) explaining how it works.

I implemented the algorithm in Unity engine, using tile maps. It requires two tile maps,
one as the source and one as the target. When you click the "run" button,
the algorithm looks at the source tile map and figures out which tiles are allowed to be next to each other. It then runs the WFC algorithm to slowly build up the new map.

This repo contains just the scripts used for the project, since Unity projects tend to not
work well across different computers.


[wfc sample](./wfc.png)
