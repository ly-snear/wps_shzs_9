using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalligraphyAssistantMain.Code
{
    public unsafe class SDLHelperEx
    {
        private static object lockObj = new object();
        private static bool isInit = false;
        public static bool InitSDL()
        {
            if (isInit)
            {
                return true;
            }
            if (SDL2.SDL_Init(SDL2.SDL_INIT_VIDEO | SDL2.SDL_INIT_AUDIO | SDL2.SDL_INIT_TIMER) < 0)
            {
                return false;
            }
            isInit = true;
            return true;
        }

        public static void QuitSDL()
        {
            SDL2.SDL_Quit();
            isInit = false;
        }

        public SDLHelperEx()
        {

        }

        public static SDLVideoInfo CreateVideoPlayer(IntPtr controlHandle, int imageidth, int imageHeight)
        {
            if (!isInit)
            {
                InitSDL();
            }
            SDLVideoInfo videoInfo = new SDLVideoInfo();
            videoInfo.SDLWindow = SDL2.SDL_CreateWindowFrom(controlHandle);
            SDL2.SDL_ShowWindow(videoInfo.SDLWindow);
            SDL2.SDL_SetWindowSize(videoInfo.SDLWindow, imageidth, imageHeight);
            if (videoInfo.SDLWindow == IntPtr.Zero)
            {
                return null;
            }
            //创建渲染器
            videoInfo.SDLRenderer = SDL2.SDL_CreateRenderer(videoInfo.SDLWindow, -1, SDL2.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);
            //创建纹理 
            videoInfo.SDLTexture = SDL2.SDL_CreateTexture(videoInfo.SDLRenderer, SDL2.SDL_PIXELFORMAT_BGR24, (int)SDL2.SDL_TextureAccess.SDL_TEXTUREACCESS_STREAMING, imageidth, imageHeight);
            videoInfo.ControlHandle = controlHandle;
            videoInfo.ImageWidth = imageidth;
            videoInfo.ImageHeight = imageHeight;
            return videoInfo;
        }

        public static void CloseVideoPlayer(SDLVideoInfo videoInfo)
        {
            if (videoInfo == null)
            {
                return;
            }
            if (videoInfo.SDLTexture != IntPtr.Zero)
            {
                SDL2.SDL_DestroyTexture(videoInfo.SDLTexture);
            }
            if (videoInfo.SDLRenderer != IntPtr.Zero)
            {
                SDL2.SDL_DestroyRenderer(videoInfo.SDLRenderer);
            }
            if (videoInfo.SDLWindow != IntPtr.Zero)
            {
                SDL2.SDL_DestroyWindow(videoInfo.SDLWindow);
                SDL2.SDL_RaiseWindow(videoInfo.SDLWindow);
                SDL2.SDL_RestoreWindow(videoInfo.SDLWindow);
            } 
        }

        public static void ShowImage(SDLVideoInfo videoInfo, int width, int height, IntPtr scan0, int stride)
        {
            lock (videoInfo)
            {
                #region SDL 视频数据渲染播放
                videoInfo.SDLRect.x = 0;
                videoInfo.SDLRect.y = 0;
                videoInfo.SDLRect.w = width;
                videoInfo.SDLRect.h = height;
                SDL2.SDL_UpdateTexture(videoInfo.SDLTexture, ref videoInfo.SDLRect, scan0, stride);
                SDL2.SDL_RenderClear(videoInfo.SDLTexture);
                SDL2.SDL_RenderCopy(videoInfo.SDLRenderer, videoInfo.SDLTexture, IntPtr.Zero, IntPtr.Zero);
                SDL2.SDL_RenderPresent(videoInfo.SDLRenderer);
                #endregion
            }
        }

        public static SDLAudioInfo CreateAudioPlayer(int sampleRate, int channels, int sampleCount, SDL2.SDL_AudioCallback callback)
        {
            lock (lockObj)
            {
                if (!isInit)
                {
                    InitSDL();
                }
                SDLAudioInfo audioInfo = new SDLAudioInfo();
                audioInfo.AudioSpec.freq = sampleRate;//根据你录制的PCM采样率决定
                audioInfo.AudioSpec.format = SDL2.AUDIO_S16SYS;
                audioInfo.AudioSpec.channels = (byte)channels; //单声道
                audioInfo.AudioSpec.silence = 0;
                audioInfo.AudioSpec.samples = (ushort)sampleCount;
                audioInfo.AudioSpec.callback = callback;
                audioInfo.AudioSpec.userdata = IntPtr.Zero;
                if (SDL2.SDL_GetAudioDeviceStatus(1) == SDL2.SDL_AudioStatus.SDL_AUDIO_STOPPED)
                {
                    int result = SDL2.SDL_OpenAudio(ref  audioInfo.AudioSpec, IntPtr.Zero);
                    if (result == 0)
                    {
                        audioInfo.AudioDeviceId = 1;
                        return audioInfo;
                    }
                    return null;
                }
                else
                {
                    SDL2.SDL_AudioSpec spec2;
                    uint audioDeviceID = SDL2.SDL_OpenAudioDevice(null, 0, ref audioInfo.AudioSpec, out spec2, 0);
                    if (audioDeviceID > 1)
                    {
                        audioInfo.AudioDeviceId = audioDeviceID;
                        return audioInfo;
                    }
                    return null;
                }
            } 
        }

        public static void PlayAudio(SDLAudioInfo audioInfo)
        {
            if (audioInfo == null)
            {
                return;
            }
            audioInfo.IsPlaying = true;
            SDL2.SDL_PauseAudioDevice(audioInfo.AudioDeviceId, 0);
            while (audioInfo.IsPlaying)
            {
                SDL2.SDL_Delay(1);
            }
            SDL2.SDL_CloseAudioDevice(audioInfo.AudioDeviceId);
        }

        public static void StopAudio(SDLAudioInfo audioInfo)
        {
            if (audioInfo == null)
            {
                return;
            }
            audioInfo.IsPlaying = false;
        }
    }
}
