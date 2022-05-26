// ★秀丸クラス
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public sealed partial class hmRbDynamicLib
{
    public sealed partial class Hidemaru
    {

        public HmExplorerPane ExplorerPane = new HmExplorerPane();
        public class HmExplorerPane
        {
            public HmExplorerPane()
            {
                SetUnManagedDll();
            }

            // モードの設定
            public int SetMode(int mode)
            {
                try
                {
                    int result = pExplorerPane_SetMode(Hidemaru.WindowHandle, (IntPtr)mode);
                    return result;
                }
                catch (Exception e)
                {
                    OutputDebugStream(ErrorMsg.MethodNeedExplorerOperation + ":\n" + e.Message);
                }

                return 0;
            }

            // モードの取得
            public int GetMode()
            {
                try
                {
                    int result = pExplorerPane_GetMode(Hidemaru.WindowHandle);
                    return result;
                }
                catch (Exception e)
                {
                    OutputDebugStream(ErrorMsg.MethodNeedExplorerOperation + ":\n" + e.Message);
                }

                return 0;
            }

            // LoadProjectする
            public int LoadProject(string filepath)
            {
                try
                {
                    byte[] encode_data = HmOriginalEncodeFunc.EncodeWStringToOriginalEncodeVector(filepath);
                    int result = pExplorerPane_LoadProject(Hidemaru.WindowHandle, encode_data);
                    return result;
                }
                catch (Exception e)
                {
                    OutputDebugStream(ErrorMsg.MethodNeedExplorerOperation + ":\n" + e.Message);
                }

                return 0;
            }

            // SaveProjectする
            public int SaveProject(string filepath)
            {
                try
                {
                    byte[] encode_data = HmOriginalEncodeFunc.EncodeWStringToOriginalEncodeVector(filepath);
                    int result = pExplorerPane_SaveProject(Hidemaru.WindowHandle, encode_data);
                    return result;
                }
                catch (Exception e)
                {
                    OutputDebugStream(ErrorMsg.MethodNeedExplorerOperation + ":\n" + e.Message);
                }

                return 0;
            }

            // GetProjectする
            public string GetProject()
            {
                try
                {
                    IntPtr startpointer = pExplorerPane_GetProject(Hidemaru.WindowHandle);
                    List<byte> blist = GetPointerToByteArray(startpointer);

                    string project_name = Hidemaru.HmOriginalDecodeFunc.DecodeOriginalEncodeVector(blist);

                    if (String.IsNullOrEmpty(project_name))
                    {
                        return null;
                    }
                    return project_name;

                }
                catch (Exception e)
                {
                    OutputDebugStream(ErrorMsg.MethodNeedExplorerOperation + ":\n" + e.Message);
                }

                return null;
            }

            private static List<byte> GetPointerToByteArray(IntPtr startpointer)
            {
                List<byte> blist = new List<byte>();

                int index = 0;
                while (true)
                {
                    var b = Marshal.ReadByte(startpointer, index);

                    blist.Add(b);

                    // 文字列の終端はやはり0
                    if (b == 0)
                    {
                        break;
                    }

                    index++;
                }

                return blist;
            }

            // GetCurrentDirする
            public string GetCurrentDir()
            {
                if (_ver < 885)
                {
                    OutputDebugStream(ErrorMsg.MethodNeedExplorerNotFound + ":\n" + "GetCurrentDir");
                    return "";
                }
                try
                {
                    IntPtr startpointer = pExplorerPane_GetCurrentDir(Hidemaru.WindowHandle);
                    List<byte> blist = GetPointerToByteArray(startpointer);

                    string currentdir_name = Hidemaru.HmOriginalDecodeFunc.DecodeOriginalEncodeVector(blist);

                    if (String.IsNullOrEmpty(currentdir_name))
                    {
                        return null;
                    }
                    return currentdir_name;
                }
                catch (Exception e)
                {
                    OutputDebugStream(ErrorMsg.MethodNeedExplorerOperation + ":\n" + e.Message);
                }

                return null;
            }

            // GetUpdated
            public int GetUpdated()
            {
                try
                {
                    int result = pExplorerPane_GetUpdated(Hidemaru.WindowHandle);
                    return result;
                }
                catch (Exception e)
                {
                    OutputDebugStream(ErrorMsg.MethodNeedExplorerOperation + ":\n" + e.Message);
                }

                return 0;
            }

            public IntPtr WindowHandle
            {
                get
                {
                    return pExplorerPane_GetWindowHandle(Hidemaru.WindowHandle);
                }
            }

            public IntPtr SendMessage(int commandID)
            {
                //
                // loaddll "HmExplorerPane.dll";
                // #h=dllfunc("GetWindowHandle",hidemaruhandle(0));
                // #ret=sendmessage(#h,0x111/*WM_COMMAND*/,251,0); //251=１つ上のフォルダ
                //
                return hmRbDynamicLib.SendMessage(pExplorerPane_GetWindowHandle(Hidemaru.WindowHandle), 0x111, commandID, IntPtr.Zero);
            }

        }
    }
}

