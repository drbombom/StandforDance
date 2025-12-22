using UnityEditor;

namespace FullscreenEditor.Linux {
    internal static class NativeFullscreenHooks {

        [InitializeOnLoadMethod]
        private static void Init() {
            if (!FullscreenUtility.IsLinux)
                return;

            FullscreenCallbacks.afterFullscreenOpen += (fs) => {
                if (X11.IsAvailable && FullscreenPreferences.NativeX11Fullscreen.Value)
                    X11.SetNativeFullscreen(true);
            };
        }

    }
}
