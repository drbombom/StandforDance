using System;
using System.Runtime.InteropServices;

namespace FullscreenEditor.Linux {

    public class X11 {
        const int ClientMessage = 33;
        const int SubstructureRedirectMask = 0x100000;
        const int SubstructureNotifyMask = 0x80000;

        [StructLayout(LayoutKind.Sequential, Size = 24 * 8)]
        public struct XClientMessageEvent {
            public int type;
            public long serial;
            public int send_event;
            public IntPtr display;
            public IntPtr window;
            public IntPtr message_type;
            public int format;
            public long data0;
            public long data1;
            public long data2;
            public long data3;
            public long data4;
            public long data5;
        }

        [DllImport("libX11")]
        static extern IntPtr XOpenDisplay(IntPtr display);

        [DllImport("libX11")]
        static extern IntPtr XDefaultRootWindow(IntPtr display);

        [DllImport("libX11")]
        static extern IntPtr XRootWindow(IntPtr display, int screen_number);

        [DllImport("libX11")]
        static extern IntPtr XInternAtom(IntPtr display, string atom_name, bool only_if_exists);

        [DllImport("libX11")]
        static extern int XSendEvent(IntPtr display, IntPtr window, bool propagate, int event_mask, ref XClientMessageEvent event_send);

        [DllImport("libX11")]
        static extern void XFlush(IntPtr display);

        [DllImport("libX11")]
        static extern int XGetInputFocus(IntPtr display, out IntPtr focus_return, out int revert_to_return);

        [DllImport("libX11")]
        static extern int XGetWindowProperty(IntPtr display, IntPtr window, IntPtr property,
            IntPtr long_offset, IntPtr long_length, bool delete,
            IntPtr req_type, out IntPtr actual_type_return, out int actual_format_return,
            out IntPtr nitems_return, out IntPtr bytes_after_return, ref IntPtr prop_return);

        [DllImport("libX11")]
        static extern void XFree(IntPtr data);

        // check if envery X11 functions are available
        public static bool IsAvailable => Environment.GetEnvironmentVariable("DISPLAY") != null;

        public static void ToggleNativeFullscreen() {
            FullscreenFocusedWindow(2);
        }

        public static void SetNativeFullscreen(bool fullscreen) {
            FullscreenFocusedWindow(fullscreen ? 1 : 0);
        }

        private static void FullscreenFocusedWindow(int action) {
            IntPtr display = XOpenDisplay(IntPtr.Zero);

            if (display == IntPtr.Zero) {
                Logger.Error("Cannot open display.");
                return;
            }

            var window = GetActiveWindow(display);

            if (window == IntPtr.Zero) {
                Logger.Error("No window focused or invalid window handle.");
                return;
            }

            var wm_state = XInternAtom(display, "_NET_WM_STATE", false);
            var fullscreen = XInternAtom(display, "_NET_WM_STATE_FULLSCREEN", false);

            var xev = new XClientMessageEvent {
                type = ClientMessage,
                serial = 0,
                send_event = 1,
                display = IntPtr.Zero,
                window = window,
                message_type = wm_state,
                format = 32,
                data0 = action, // 0 = remove, 1 = add, 2 = toggle
                data1 = fullscreen.ToInt32(),
            };

            var root = XDefaultRootWindow(display);
            var result = XSendEvent(display, root, false, SubstructureNotifyMask | SubstructureRedirectMask, ref xev);

            XFlush(display);

            Logger.Debug("Root window: {0}", root.ToString("X"));
            Logger.Debug("Display: {0}", display.ToString("X"));
            Logger.Debug("Window: {0}", window.ToString("X"));
            Logger.Debug("Fullscreen: {0}", fullscreen.ToString("X"));
            Logger.Debug("WM State: {0}", wm_state.ToString("X"));

            Logger.Debug(IsWindowFullscreen(display, window, fullscreen) ? "Window is fullscreen" : "Window is not fullscreen");
        }

        private static bool IsWindowFullscreen(IntPtr display, IntPtr window, IntPtr fullscreenAtom) {
            var wm_state = XInternAtom(display, "_NET_WM_STATE", false);
            var prop = IntPtr.Zero;
            var status = XGetWindowProperty(
                display,
                window,
                wm_state,
                IntPtr.Zero,
                new IntPtr(1024),
                false,
                IntPtr.Zero,
                out var type,
                out var format,
                out var nitems,
                out var bytesAfter,
                ref prop);

            if (status == 0 && prop != IntPtr.Zero && nitems != IntPtr.Zero) {
                var atoms = new IntPtr[nitems.ToInt32()];
                Marshal.Copy(prop, atoms, 0, nitems.ToInt32());
                XFree(prop);

                Logger.Debug("Atoms: {0}", string.Join(", ", atoms));
                return Array.IndexOf(atoms, fullscreenAtom) >= 0;
            }

            if (prop != IntPtr.Zero)
                XFree(prop);

            return false;
        }

        public static IntPtr GetActiveWindow(IntPtr display) {
            if (!IsEwmhSupported(display, "_NET_ACTIVE_WINDOW")) {
                Logger.Warning("Window manager does not support _NET_ACTIVE_WINDOW");
                return IntPtr.Zero;
            }

            var request = XInternAtom(display, "_NET_ACTIVE_WINDOW", false);
            var root = XDefaultRootWindow(display);
            var data = IntPtr.Zero;
            var status = XGetWindowProperty(
                display,
                root,
                request,
                IntPtr.Zero,
                new IntPtr(1),
                false,
                IntPtr.Zero,
                out var type,
                out var format,
                out var nitems,
                out var bytesAfter,
                ref data);

            var activeWindow = IntPtr.Zero;

            if (status == 0 && nitems != IntPtr.Zero && data != IntPtr.Zero) {
                activeWindow = Marshal.ReadIntPtr(data);
                XFree(data);
            }

            return activeWindow;
        }

        private static bool IsEwmhSupported(IntPtr display, string atomName) {
            var netSupported = XInternAtom(display, "_NET_SUPPORTED", false);
            var root = XDefaultRootWindow(display);
            var data = IntPtr.Zero;
            var status = XGetWindowProperty(
                display,
                root,
                netSupported,
                IntPtr.Zero,
                new IntPtr(1024),
                false,
                XA_ATOM,
                out var type,
                out var format,
                out var nitems,
                out var bytesAfter,
                ref data);

            if (status != 0 || nitems == IntPtr.Zero || data == IntPtr.Zero) {
                if (data != IntPtr.Zero)
                    XFree(data);
                return false;
            }

            var requestedAtom = XInternAtom(display, atomName, false);
            var atoms = new IntPtr[nitems.ToInt32()];
            Marshal.Copy(data, atoms, 0, nitems.ToInt32());
            XFree(data);

            return Array.IndexOf(atoms, requestedAtom) >= 0;
        }

        static readonly IntPtr XA_ATOM = new IntPtr(4);
    }
}
