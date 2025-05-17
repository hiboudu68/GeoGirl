using UnityEngine;
#if UNITY_EDITOR || PLATFORM_STANDALONE_LINUX || UNITY_STANDALONE_LINUX
using UnityEditor;
#endif
using System;
using System.Runtime.InteropServices;

public class NativeFileDialog : MonoBehaviour
{
    // --- P/Invoke Win32 pour le build Windows ---
#if !UNITY_EDITOR && (UNITY_STANDALONE_WIN) && !PLATFORM_STANDALONE_LINUX && !UNITY_STANDALONE_LINUX
    [DllImport("comdlg32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern bool GetOpenFileName(ref OPENFILENAME ofn);
    [DllImport("comdlg32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern bool GetSaveFileName(ref OPENFILENAME ofn);
    [DllImport("comdlg32.dll", SetLastError = true)]
    private static extern int CommDlgExtendedError();
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    private const int OFN_FILEMUSTEXIST = 0x00001000;
    private const int OFN_PATHMUSTEXIST = 0x00000800;
    private const int OFN_NOCHANGEDIR   = 0x00000008;
    private const int OFN_EXPLORER      = 0x00080000;
    private const int OFN_OVERWRITEPROMPT= 0x00000002;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private struct OPENFILENAME
    {
        public int         lStructSize;
        public IntPtr      hwndOwner;
        public IntPtr      hInstance;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string      lpstrFilter;
        public IntPtr      lpstrCustomFilter;
        public int         nMaxCustFilter;
        public int         nFilterIndex;
        [MarshalAs(UnmanagedType.LPTStr, SizeConst = 260)]
        public string      lpstrFile;
        public int         nMaxFile;
        [MarshalAs(UnmanagedType.LPTStr, SizeConst = 260)]
        public string      lpstrFileTitle;
        public int         nMaxFileTitle;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string      lpstrInitialDir;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string      lpstrTitle;
        public int         Flags;
        public short       nFileOffset;
        public short       nFileExtension;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string      lpstrDefExt;
        public IntPtr      lCustData;
        public IntPtr      lpfnHook;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string      lpTemplateName;
        public IntPtr      pvReserved;
        public int         dwReserved;
        public int         FlagsEx;
    }
#endif

    public string SaveFileDialog(string defaultName, string extension)
    {
#if UNITY_STANDALONE_LINUX || PLATFORM_STANDALONE_LINUX
        return string.Empty;
#elif UNITY_EDITOR
        // ======= MODE ÉDITEUR =======
        return EditorUtility.SaveFilePanel(
            title: "Enregistrer un " + extension,
            directory: Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            defaultName: defaultName,
            extension: extension
        );
#elif UNITY_STANDALONE_WIN
    // ======= MODE BUILD WINDOWS =======
    var ofn = new OPENFILENAME {
        lStructSize     = Marshal.SizeOf<OPENFILENAME>(),
        hwndOwner       = GetForegroundWindow(),
        lpstrFilter     = extension + " files\0*." + extension + "\0All files\0*.*\0\0",
        nFilterIndex    = 1,
        lpstrFile       = new string('\0', 260),
        nMaxFile        = 260,
        lpstrFileTitle  = new string('\0', 260),
        nMaxFileTitle   = 260,
        lpstrInitialDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
        lpstrTitle      = "Enregistrer un " + extension,
        Flags           = OFN_PATHMUSTEXIST | OFN_OVERWRITEPROMPT | OFN_EXPLORER,
        lpstrDefExt     = extension
    };

    if (!GetSaveFileName(ref ofn))
    {
        int err = CommDlgExtendedError();
        if (err == 0)
            Debug.Log("Build : boîte fermée sans sauvegarde.");
        else
            Debug.LogError($"Build : GetSaveFileName a échoué (0x{err:X}).");
        return string.Empty;
    }
    else
    {
        Debug.Log("Build : emplacement de sauvegarde -> " + ofn.lpstrFile);
        return ofn.lpstrFile;
    }
#else
    Debug.LogWarning("Save dialog non supporté sur cette plateforme.");
    return string.Empty;
#endif
    }

    /// <summary>
    /// Ouvre la boîte de sélection de MP3 :
    /// - en Éditeur : EditorUtility.OpenFilePanel  
    /// - en Build Windows : P/Invoke GetOpenFileName  
    /// </summary>
    public string OpenFileDialog(string extension)
    {
#if UNITY_STANDALONE_LINUX || PLATFORM_STANDALONE_LINUX
        return string.Empty;
#elif UNITY_EDITOR
        // ======= MODE ÉDITEUR =======
        return EditorUtility.OpenFilePanel(
            title: "Sélectionnez un " + extension,
            directory: Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
            extension: extension
        );
#elif UNITY_STANDALONE_WIN
        // ======= MODE BUILD WINDOWS =======
        var ofn = new OPENFILENAME {
            lStructSize     = Marshal.SizeOf<OPENFILENAME>(),
            hwndOwner       = GetForegroundWindow(),
            lpstrFilter     = extension + " files\0*." + extension + "\0All files\0*.*\0\0",
            nFilterIndex    = 1,
            lpstrFile       = new string('\0', 260),
            nMaxFile        = 260,
            lpstrFileTitle  = new string('\0', 260),
            nMaxFileTitle   = 260,
            lpstrInitialDir = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
            lpstrTitle      = "Choisissez un " + extension,
            Flags           = OFN_FILEMUSTEXIST
                            | OFN_PATHMUSTEXIST
                            | OFN_NOCHANGEDIR
                            | OFN_EXPLORER,
            lpstrDefExt     = extension
        };

        if (!GetOpenFileName(ref ofn))
        {
            int err = CommDlgExtendedError();
            if (err == 0)
                Debug.Log("Build : boîte fermée sans sélection.");
            else
                Debug.LogError($"Build : GetOpenFileName a échoué (0x{err:X}).");
            return string.Empty;
        }
        else
        {
            Debug.Log("Build : fichier sélectionné -> " + ofn.lpstrFile);
            return ofn.lpstrFile;
        }
#else
        Debug.LogWarning("File dialog non supporté sur cette plateforme.");
#endif
    }
}
