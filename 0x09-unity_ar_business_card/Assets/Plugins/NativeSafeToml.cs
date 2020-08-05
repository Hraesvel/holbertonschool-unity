using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Plugins
{
    /// <summary>
    ///     Native bindings
    ///     Very Basic FFI Binding To rust toml (read-only ops)
    /// </summary>
    internal static class NativeSafeToml
    {
        [DllImport("safeToml", EntryPoint = "get_value")]
        public static extern bool GetValue(IntPtr handle, string key);

        [DllImport("safeToml", EntryPoint = "release_safe_toml")]
        public static extern void ReleaseSafeToml(IntPtr handle);

        [DllImport("safeToml", EntryPoint = "has_key")]
        public static extern bool HasKey(IntPtr handle, string key);

        /// <summary>
        ///     Creates new safe toml handle
        /// </summary>
        /// <param name="filePath">toml file parse toml</param>
        /// <returns></returns>
        [DllImport("safeToml", EntryPoint = "new_safe_toml")]
        public static extern SafeTomlHandle NewSafeToml(string filePath);
    }


    internal class SafeTomlHandle : SafeHandle
    {
        public SafeTomlHandle() : base(IntPtr.Zero, true)
        {
        }

        public override bool IsInvalid => handle == IntPtr.Zero;

        protected override bool ReleaseHandle()
        {
            if (!IsInvalid)
                NativeSafeToml.ReleaseSafeToml(handle);

            return true;
        }

        public bool GetValue(string key, out string val)
        {
            var len = 0;
            val = null;
            if (!NativeSafeToml.GetValue(handle, key))
                return false;
            try
            {
                while (Marshal.ReadByte(handle, len) != 0)
                    ++len;
                var buffer = new byte[len];
                Marshal.Copy(handle, buffer, 0, buffer.Length);
                val = Encoding.UTF8.GetString(buffer);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
                return false;
            }

            return true;
        }

        public bool HasKey(string key)
        {
            return NativeSafeToml.HasKey(handle, key);
        }
    }

    public class Toml : IDisposable
    {
        private readonly SafeTomlHandle _handle;

        private Toml(string path)
        {
            FilePath = path;
            _handle = NativeSafeToml.NewSafeToml(path);
        }

        public string FilePath { get; }


        public void Dispose()
        {
            _handle.Dispose();
        }

        public bool HasKey(string key)
        {
            return _handle.HasKey(key);
        }

        public bool TryGetValue(string key, out string val)
        {
            return _handle.GetValue(key, out val);
        }

        public string GetValue(string key)
        {
            _handle.GetValue(key, out var val);
            return val;
        }
    }
}