using UnityEditor;

namespace FullscreenEditor {
    [InitializeOnLoad]
    public class EnableGameViewShortcuts {

        static EnableGameViewShortcuts() {
            // Fixes an issue where users were getting soft locking because the game view shortcuts were disabled by default
            // when the game view was in fullscreen mode, and thus the user could not exit fullscreen mode.
            FullscreenCallbacks.afterFullscreenOpen += (fs) => {
                // UnityEditor.ShortcutManagement.ShortcutIntegration.ignoreWhenPlayModeFocused = false;
                var shortcutManager = ReflectionUtility.FindClass("UnityEditor.ShortcutManagement.ShortcutIntegration");

                if (shortcutManager != null && shortcutManager.HasProperty("ignoreWhenPlayModeFocused"))
                    shortcutManager.SetPropertyValue("ignoreWhenPlayModeFocused", false);
            };
        }
    }
}
