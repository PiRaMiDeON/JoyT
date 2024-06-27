<p align="center">
      <video src='https://github.com/PiRaMiDeON/JoyT/blob/main/GitHubMP4.mp4'>
</p>

<p align="center">
    <img src="https://build.burning-lab.com/app/rest/builds/buildType:id:UnityAssets_ComBurningLabSwipedetector_DevelopmentBuild/statusIcon.svg" alt="Build Status">
    <a href="https://burning-lab.youtrack.cloud/agiles/131-11/current"><img src="https://img.shields.io/badge/Roadmap-YouTrack-orange" alt="Roadmap Link"></a>
    <img src="https://img.shields.io/badge/Engine-2021.3-blueviolet" alt="Unity Version">
    <img src="https://img.shields.io/badge/Version-1.2.1-blue" alt="Game Version">
    <img src="https://img.shields.io/badge/License-MIT-success" alt="License">
</p>

## Проект

Одиночный сюжетный 2D платформер в пиксельном стиле, рассказывающий историю о неизвестной инфекции в кубической системе. Проект был создан на движке Unity 2021, а позже перенесен на версию 2022 года. Приятная пиксельная графика и хорошее звуковое сопровождение, вызванные порывом вдохновения автора, не оставят игрока равнодушным. Изначально JoyT задумывалась как бесконечный платформер на основе генерации комнат, но в силу нехватки знаний и опыта автора на момент разработки переросла в сюжетный проект с финалом и ограниченным количеством локаций (да и сама идея сюжета мне показалась более приятной). Сражения с боссами, головоломки и паркур - это JoyT!

## Установка

1. Вы можете получит JoyT бесплатно на странице магазина VKPlay - https://vkplay.ru/play/game/joyt/
2. Также вы можете найти JoyT на Itch.io - https://piramideon.itch.io/joyt
3. Или вы можете скачать архив с проектом здесь, на GitHub - 

**Burning-Lab Registry:**
```json
    {
      "name": "Burning-Lab Registry",
      "url": "https://packages.burning-lab.com",
      "scopes": [
        "com.burning-lab"
      ]
    }
```

## Documentation

### Settings:

- **-** **`Swipe Detection Mode (DetectionMode)`** - Swipe recognition mode. Completed or incomplete swipe.

- **-** **`Detect multiple swipes (bool)`** - Enable it if you need to recognize multiple swipes without taking your finger off the screen.

- **-** **`Handle Keyboard Arrows Clicks (bool)`** - Enable it if you need to trigger swipe processing events when pressing the arrows on the keyboard.

- **-** **`Min Swipe Distance (float)`** - Minimum swipe length.

- **-** **`Is Paused (bool)`** - Pause. If the value is `true`, the component does not process swipes and does not raise events.

### Events:
- **-** **`On Swipe Start (UnityEvent<Vector2>)`** - An event that is triggered when the user touches the screen.

- **-** **`On Swipe End (UnityEvent<Vector2>)`** - An event that is triggered when the user releases the screen.

- **-** **`On Swipe Detected (SwipeDirection)`** - Called when the swipe is recognized.

### Methods:
- **-** **`SwipeInput.SetPause()`** **`void`** - Sets the pause.

- **-** **`SwipeInput.UnsetPause()`** **`void`** - Removes the pause.

### Configuration defines:

- `DEBUG_BURNING_LAB_SDK` - Output all Burning-Lab sdk logs.

- `DEBUG_SWIPE_DETECTOR` - Output swipe detector logs only.

## Distribute

- [packages.burning-lab.com](https://packages.burning-lab.com/-/web/detail/com.burning-lab.swipedetector)

## Developers

- [n.fridman](https://github.com/n-fridman)

## License

Project Burning-Lab.SwipeDetector is distributed under the MIT license.
