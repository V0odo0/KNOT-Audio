## 0.6.5 (2026-09-12)

* Added Pitch Over Velocity, Angular Pitch Over Velocity & Angular Volume Over Velocity playback behaviour Mods
* Fixed playback behaviour Mods not receiving Attach event and sharing runtime state between controllers (GetInstance result was discarded)
* Fixed KnotPlayChanceMod inverted chance (Chance = 1 never played)
* Fixed WithVolume, WithPitch & WithDelay handle extensions applying a random range instead of the exact value
* Fixed Pause() destroying OneShot controller instances
* Fixed default AudioMixerSnapshot weight oscillating every frame in Snapshot Volumes update
* Fixed Volume Sources blend weight computed from squared distance (blend falloff was too steep)
* Fixed AudioMixer parameter not restoring to default when one Parameters Volume is replaced by another in the same frame
* Fixed KnotInstanceLimitMod tracking dictionary growing indefinitely
* Fixed Volume Over Time Mod ignoring controller MaxVolume
* Fixed Screen Space Stereo Pan Mod updating on every behaviour event including Detach
* Fixed KnotVolumeOverVelocityMod losing SmoothStep on instantiation
* Fixed Bounds Volume Source gizmos not matching actual sampling position with assigned Pivot
* Fixed audio preview starting 10000 samples into the clip in KnotAudioDataAsset inspector

## 0.6.0 (2024-09-28)

* [breaking change] UPM (GitHub) - Moving common parts of code to Knot.Core assembly. Please, use scope registry installation method
* Migrating to Unity 2021.3+

## 0.5.4 (2024-08-23)

* Fixed KnotTypePicker'r attribute drawer not responding to property changes

## 0.5.3 (2024-05-03)

* Fixed Mod List not responging to changes on Instance AudioDataProvider

## 0.5.2 (2023-11-20)

* Fixed No Manager instance in builds

## 0.5.1 (2023-10-15)

* Editor menu path generalization

## 0.1.1 (2022-07-06)

* Fixed SetParent behavuiur on Audio Player

## 0.1.0 (2022-03-05)
