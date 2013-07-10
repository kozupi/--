using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;

namespace Project2
{
    class SoundManager:GameComponent
    {
        public static SoundManager instance { get; protected set; }
        AudioEngine audioEngine;
        WaveBank waveBank;
        AudioListener listener;
        AudioCategory musicCategory;
        List<Cue> cuelist;
        List<Cue> BGMCuelist;
        Func<Vector3> updateListenerPosition, updateListenerForward, updateListenerUp;
        public SoundBank soundBank;
        public static int volume { get; set; }     

        private SoundManager(Game game, Func<Vector3> getListenerPosition, Func<Vector3> getListenerForward, Func<Vector3> getListenerUp)
            :base(game)
        {
            audioEngine = new AudioEngine(@"Content\Audio\GameAudio.xgs");
            waveBank = new WaveBank(audioEngine, @"Content\Audio\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, @"Content\Audio\Sound Bank.xsb");
            musicCategory = audioEngine.GetCategory("Default");
            volume = 10;
            musicCategory.SetVolume(volume);

            
            cuelist = new List<Cue>();
            BGMCuelist = new List<Cue>();
           
            updateListenerForward = getListenerForward;
            updateListenerUp = getListenerUp;
            updateListenerPosition = getListenerPosition;
            listener = new AudioListener();
        }

        public static void init(Game game, Func<Vector3> getListenerPosition, Func<Vector3> getListenerForward, Func<Vector3> getListenerUp)
        {
            instance = new SoundManager(game, getListenerPosition,getListenerForward, getListenerUp);            
        }

        public static void playSound(string soundName)
        {
            instance.soundBank.PlayCue(soundName);
        }

        public static void playSound(string soundName, Vector3 position)
        {
            Cue cueToPlay = instance.soundBank.GetCue(soundName);
            AudioEmitter emitter = new AudioEmitter();
            emitter.Position = position;
            cueToPlay.Apply3D(instance.listener, emitter);
            cueToPlay.Play();
        }

        public static void createCue(string soundName)
        {
            Cue cueToPlay = instance.soundBank.GetCue(soundName);
            bool added = false;
            for (int i = 0; i < instance.cuelist.Count; i++)
			{
                if (instance.cuelist[i].Name == soundName)
                {
                    instance.cuelist[i] = cueToPlay;
                    added = true;
                }
            }
            if (!added)
            {
                instance.cuelist.Add(cueToPlay);
            }
            instance.cuelist[instance.cuelist.Count - 1].Play();
        }

        public static void pauseSound(string soundName)
        {
            Cue cueToPause = null;
            foreach (Cue c in instance.cuelist)
            {
                if (soundName == c.Name)
                {
                    cueToPause = c;
                    break;
                }
            }
            if (cueToPause.IsPlaying)
            {
                cueToPause.Pause();
            }
        }

        public static void resumeSound(string soundName)
        {
            Cue cueToResume = null;
            foreach (Cue c in instance.cuelist)
            {
                if (soundName == c.Name)
                {
                    cueToResume = c;
                    break;
                }
            }
            if (cueToResume.IsPaused)
            {
                cueToResume.Resume();
            }
        }

        public static void stopSound(string soundName)
        {
            instance.soundBank.GetCue(soundName).Stop(AudioStopOptions.Immediate);
        }

        public static void stopCue(string soundName)
        {
            bool stopped = false;
            for (int i = 0; i < instance.cuelist.Count && !stopped; i++)
            {
                if (instance.cuelist[i].Name == soundName)
                {
                    instance.cuelist[i].Stop(AudioStopOptions.Immediate);
                    stopped = true;
                } 
            }
        }

        public override void Update(GameTime gameTime)
        {
            musicCategory.SetVolume(volume / 10.0f);
            listener.Position = updateListenerPosition();
            listener.Forward = updateListenerForward();
            listener.Up = updateListenerUp();
            KeyboardState keyboard = Keyboard.GetState();

            for (int i = 0; i < cuelist.Count; i++)
            {
                if (cuelist[i].IsStopped)
                {
                    cuelist.RemoveAt(i--);
                }
            }
            audioEngine.Update();

            base.Update(gameTime);
        }


        private void initSounds(SoundBank soundBank)
        {
            cuelist.Add(soundBank.GetCue(("engine_1")));
            cuelist[cuelist.Count - 1].Play();
            cuelist[cuelist.Count - 1].Pause();
            cuelist.Add(soundBank.GetCue(("afterburner_1")));
            cuelist[cuelist.Count - 1].Play();
            cuelist[cuelist.Count - 1].Pause();
        }

        internal static void start()
        {
            instance.BGMCuelist.Add(instance.soundBank.GetCue("AbsoluteRush"));
            instance.BGMCuelist.Add(instance.soundBank.GetCue("RadioChatter"));
            instance.BGMCuelist.Add(instance.soundBank.GetCue("Radar"));
            instance.BGMCuelist.Add(instance.soundBank.GetCue("wind"));

            foreach (Cue c in instance.BGMCuelist)
            {
                c.Play();
            }
            instance.initSounds(instance.soundBank);
        }
    }
}
