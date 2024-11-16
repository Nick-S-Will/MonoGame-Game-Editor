using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Editor.Engine;

internal interface ISoundEmitter
{
    public enum Sound
    {
        Select,
        Damage
    }

    public Dictionary<Sound, KeyValuePair<string, SoundEffectInstance>> SoundEffects { get; }

    public static KeyValuePair<string, SoundEffectInstance> CreateSoundEffect(ContentManager contentManager, string soundName)
    {
        SoundEffect soundEffect = contentManager.Load<SoundEffect>(soundName);
        SoundEffectInstance soundEffectInstance = soundEffect.CreateInstance();
        soundEffectInstance.Volume = 1;
        soundEffectInstance.IsLooped = false;

        return new(soundName, soundEffectInstance);
    }
}
