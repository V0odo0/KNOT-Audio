using System.Collections.Generic;
using Knot.Core;
using UnityEngine;

namespace Knot.Audio
{
    [AddComponentMenu(KnotAudio.CorePath + "Audio Player", 0)]
    public class KnotAudioPlayer : KnotTrackedMonoBehaviour<KnotAudioPlayer>
    {
        public virtual IKnotAudioDataProvider AudioDataProvider
        {
            get => _audioDataProvider;
            set => _audioDataProvider = value;
        }
        [SerializeReference, KnotTypePicker(typeof(IKnotAudioDataProvider))]
        private IKnotAudioDataProvider _audioDataProvider = new KnotInstanceAudioDataProvider();

        public virtual List<IKnotControllerMod> ControllerMods => 
            _controllerMods ?? (_controllerMods = new List<IKnotControllerMod>());
        [SerializeReference, KnotTypePicker(typeof(IKnotControllerMod), false)]
        private List<IKnotControllerMod> _controllerMods;

        public virtual bool PlayOnAwake
        {
            get => _playOnAwake;
            set => _playOnAwake = value;
        }
        [SerializeField] private bool _playOnAwake;

        public virtual KnotAudioPlayMode PlayMode
        {
            get => _playMode;
            set => _playMode = value;
        }
        [SerializeField] private KnotAudioPlayMode _playMode;


        protected override void Awake()
        {
            base.Awake();

            if (PlayOnAwake)
                Play();
        }


        protected virtual void Reset()
        {
            AudioDataProvider = new KnotInstanceAudioDataProvider();
            ControllerMods.Add(new KnotSetParentMod(transform));
            PlayOnAwake = false;
        }

        public virtual void Play() => PlayGetHandle();

        public virtual KnotAudioControllerHandle PlayGetHandle()
        {
            return AudioDataProvider?.Play(PlayMode, ControllerMods.ToArray()) ?? default;
        }

        public virtual void PlayVariant(int variantId) => PlayVariantGetHandle(variantId);

        public virtual KnotAudioControllerHandle PlayVariantGetHandle(int variantId)
        {
            if (variantId < 0 || AudioDataProvider == null)
                return default;

            if (AudioDataProvider is KnotAssetAudioDataVariantProvider assetVariant && variantId < assetVariant.Variants.Count)
                return assetVariant.Variants[variantId]?.AudioData?.Play(PlayMode, ControllerMods.ToArray()) ?? default;
            if (AudioDataProvider is KnotLibraryEntryVariantProvider libraryVariant && variantId < libraryVariant.Variants.Count)
                return libraryVariant.Variants[variantId]?.AudioData?.Play(PlayMode, ControllerMods.ToArray()) ?? default;
            if (AudioDataProvider is KnotInstanceAudioDataVariantProvider instanceVariant && variantId < instanceVariant.Variants.Count)
                return instanceVariant.Variants[variantId]?.AudioData?.Play(PlayMode, ControllerMods.ToArray()) ?? default;

            return default;
        }
    }
}
