/*===============================================================================
Copyright (c) 2019 PTC Inc. All Rights Reserved.

Confidential and Proprietary - Protected under copyright and other laws.
Vuforia is a trademark of PTC Inc., registered in the United States and other 
countries.
===============================================================================*/

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering;
#if PLATFORM_ANDROID
using UnityEngine.Android;

#endif

namespace Vuforia.UnityCompiled
{
    public class RuntimeOpenSourceInitializer
    {
        private static IUnityCompiledFacade sFacade;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnRuntimeMethodLoad()
        {
            InitializeFacade();
        }

        private static void InitializeFacade()
        {
            if (sFacade != null) return;

            sFacade = new OpenSourceUnityCompiledFacade();
            UnityCompiledFacade.Instance = sFacade;
        }

        private class OpenSourceUnityCompiledFacade : IUnityCompiledFacade
        {
            public IUnityRenderPipeline UnityRenderPipeline { get; } = new UnityRenderPipeline();

            public IUnityAndroidPermissions UnityAndroidPermissions { get; } = new UnityAndroidPermissions();


            public bool IsUnityUICurrentlySelected()
            {
                return !(EventSystem.current == null || EventSystem.current.currentSelectedGameObject == null);
            }
        }

        private class UnityRenderPipeline : IUnityRenderPipeline
        {
            public UnityRenderPipeline()
            {
#if UNITY_2018
                RenderPipeline.beginFrameRendering += OnBeginFrameRendering;
                RenderPipeline.beginCameraRendering += OnBeginCameraRendering;
#else
                UnityEngine.Rendering.RenderPipelineManager.beginFrameRendering += OnBeginFrameRendering;
                UnityEngine.Rendering.RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
#endif
            }

            public event Action<Camera[]> BeginFrameRendering;
            public event Action<Camera> BeginCameraRendering;

#if UNITY_2018
            private void OnBeginCameraRendering(Camera camera)
#else
            void OnBeginCameraRendering(UnityEngine.Rendering.ScriptableRenderContext context, Camera camera)
#endif
            {
                if (BeginCameraRendering != null)
                    BeginCameraRendering(camera);
            }

#if UNITY_2018
            private void OnBeginFrameRendering(Camera[] cameras)
#else
            void OnBeginFrameRendering(UnityEngine.Rendering.ScriptableRenderContext context, Camera[] cameras)
#endif
            {
                if (BeginFrameRendering != null)
                    BeginFrameRendering(cameras);
            }
        }

        private class UnityAndroidPermissions : IUnityAndroidPermissions
        {
            public bool HasRequiredPermissions()
            {
#if PLATFORM_ANDROID
                return Permission.HasUserAuthorizedPermission(Permission.Camera);
#else
                return true;
#endif
            }

            public void AskForPermissions()
            {
#if PLATFORM_ANDROID
                Permission.RequestUserPermission(Permission.Camera);
#endif
            }
        }
    }
}