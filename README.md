# 4DVFXProject
Volumetric captured dance videos captured at Rememory Studio powered by DepthKit Studio

## Ariaray

![screenshot](./images/AriarayTest01_high_res.gif)


## Roy

![screenshot](./images/Roy002.gif)


## Mao

Coming soon..


## How dancers are captured & how an app works on Unity


I requested to take Volumetric Video of dancers to [Rememory Studio](https://rememory.jp/jp/studio.html) organized by Curiocity, inc (If you wanna use outside of Japan, please check out [DepthKitStudio](https://www.depthkit.tv/depthkit-studio) ). Here is how the studio looks like.

![Rememory Studio](./images/RememoryStudio.jpg)

And while we take the volumetric videos, we could see point cloud data in real-time bellow.

![How We Captured](./images/HowWeCaptured.jpg)

After we finished to take the volumetric videos, [Rememory Studio](https://rememory.jp/jp/studio.html) converted the point cloud data into a mp4 file and a json file.

![HowWeCaptured](./images/VolumetricVideoOnUnity.jpg)

And I added these files on Unity and played the Volumetric Video with DepthKitStudio's scripts. Then I could see the real-time constructed mesh on Unity.
