﻿/*
 * Originally from
 * http://code.msdn.microsoft.com/CSOfficeDocumentFileExtract-e5afce86
 * 
 */

/****************************** Module Header ******************************\
Module Name:  Ole10Native.cs
Project:      CSOfficeDocumentFileExtrator
Copyright (c) Microsoft Corporation.

This file contains the code for extracting the embedded object from the 
embedded files which are stored as structured storage files. 
Example: oleObject1.bin

This source is subject to the Microsoft Public License.
See http://www.microsoft.com/en-us/openness/licenses.aspx#MPL
All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Hanlin.Common.Windows
{
    public class Ole10Native
    {

        // Interface and Enumeration declarations.
        #region IEnumSTATSTG
        [ComImport]
        [Guid("0000000d-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IEnumSTATSTG
        {
            // The user needs to allocate an STATSTG array whose size is celt.
            [PreserveSig]
            uint Next(uint celt, [MarshalAs(UnmanagedType.LPArray), Out] System.Runtime.InteropServices.ComTypes.STATSTG[] rgelt, out uint pceltFetched);

            void Skip(uint celt);

            void Reset();

            [return:MarshalAs(UnmanagedType.Interface)]
            IEnumSTATSTG Clone();
        }
        #endregion

        #region STATFLAG

        [Flags]
        public enum STATFLAG :uint
        {
            STATFLAG_DEFAULT = 0,
            STATFLAG_NONAME = 1,
            STATFLAG_NOOPEN = 2
        }

        #endregion

        #region IStorage
        [ComImport]
        [Guid("0000000b-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        interface IStorage 
        {
            void CreateStream( 
                /* [string][in] */ string pwcsName,
                /* [in] */ uint grfMode,
                /* [in] */ uint reserved1,
                /* [in] */ uint reserved2,
                /* [out] */ out System.Runtime.InteropServices.ComTypes.IStream ppstm);
            void OpenStream( 
                /* [string][in] */ string pwcsName,
                /* [unique][in] */ IntPtr reserved1,
                /* [in] */ uint grfMode,
                /* [in] */ uint reserved2,
                /* [out] */ out System.Runtime.InteropServices.ComTypes.IStream ppstm);

            void CreateStorage( 
                /* [string][in] */ string pwcsName,
                /* [in] */ uint grfMode,
                /* [in] */ uint reserved1,
                /* [in] */ uint reserved2,
                /* [out] */ out IStorage ppstg);

            void OpenStorage( 
                /* [string][unique][in] */ string pwcsName,
                /* [unique][in] */ IStorage pstgPriority,
                /* [in] */ uint grfMode,
                /* [unique][in] */ IntPtr snbExclude,
                /* [in] */ uint reserved,
                /* [out] */ out IStorage ppstg);

            void CopyTo( 
                /* [in] */ uint ciidExclude,
                /* [size_is][unique][in] */ Guid rgiidExclude, // should this be an array?
                /* [unique][in] */ IntPtr snbExclude,
                /* [unique][in] */ IStorage pstgDest);

            void MoveElementTo( 
                /* [string][in] */ string pwcsName,
                /* [unique][in] */ IStorage pstgDest,
                /* [string][in] */ string pwcsNewName,
                /* [in] */ uint grfFlags);

            void Commit( 
                /* [in] */ uint grfCommitFlags);

            void Revert();

            void EnumElements( 
                /* [in] */ uint reserved1,
                /* [size_is][unique][in] */ IntPtr reserved2,
                /* [in] */ uint reserved3,
                /* [out] */ out IEnumSTATSTG ppenum);

            void DestroyElement( 
                /* [string][in] */ string pwcsName);

            void RenameElement( 
                /* [string][in] */ string pwcsOldName,
                /* [string][in] */ string pwcsNewName);

            void SetElementTimes( 
                /* [string][unique][in] */ string pwcsName,
                /* [unique][in] */ System.Runtime.InteropServices.ComTypes.FILETIME pctime,
                /* [unique][in] */ System.Runtime.InteropServices.ComTypes.FILETIME patime,
                /* [unique][in] */ System.Runtime.InteropServices.ComTypes.FILETIME pmtime);

            void SetClass( 
                /* [in] */ Guid clsid);

            void SetStateBits( 
                /* [in] */ uint grfStateBits,
                /* [in] */ uint grfMask);

            void Stat( 
                /* [out] */ out System.Runtime.InteropServices.ComTypes.STATSTG pstatstg,
                /* [in] */ uint grfStatFlag);
        }

        #endregion

        #region STGM
        [Flags]
        public enum STGM : int
        {
            DIRECT           = 0x00000000,
            TRANSACTED       = 0x00010000,
            SIMPLE           = 0x08000000,
            READ             = 0x00000000,
            WRITE            = 0x00000001,
            READWRITE        = 0x00000002,
            SHARE_DENY_NONE  = 0x00000040,
            SHARE_DENY_READ  = 0x00000030,
            SHARE_DENY_WRITE = 0x00000020,
            SHARE_EXCLUSIVE  = 0x00000010,
            PRIORITY         = 0x00040000,
            DELETEONRELEASE  = 0x04000000,
            NOSCRATCH        = 0x00100000,
            CREATE           = 0x00001000,
            CONVERT          = 0x00020000,
            FAILIFTHERE      = 0x00000000,
            NOSNAPSHOT       = 0x00200000,
            DIRECT_SWMR      = 0x00400000,
        }

        #endregion

        #region StgIsStorageFile

        [DllImport("Ole32.dll")]
        static extern int StgIsStorageFile([MarshalAs(UnmanagedType.LPWStr)]string filename);

        #endregion

        #region StgOpenStorage

        [DllImport("Ole32.dll")]
        static extern int StgOpenStorage([MarshalAs(UnmanagedType.LPWStr)]string pwcsName, IStorage pstgPriority, STGM grfmode, IntPtr snbExclude, uint researved, out IStorage ppstgOpen);

        #endregion

        /// <summary>
        /// ExtractFile method opens the structured storage file (oleObjectX.bin) which is extracted from the document.
        /// This checks if ole10Native structure exists in the file. If it exists, the contents will be extracted
        /// The Ole10Native structure will be in the following format.
        /// a) Name of the embedded file starts from 7th byte
        /// b) The location of the original file is after the file name
        /// c) The 4 bytes after that is unknown. Skip it
        /// d) The next 4 bytes gives the lenght of the temporary location of the file before it is inserted. (In little endian format)
        /// e) The temporary location path comes next.
        /// f) The size of the embedded file comes in next 4 bytes. (In little endian format)
        /// g) The actual file starts after that.
        /// </summary>
        /// <param name="sourceFilePath">The oleObjectX.bin file which is extracted from the document</param>
        /// <param name="destinationFolder">The destination folder where the file will be kept</param>

        public static void ExtractFile(string sourceFilePath, string destinationFolder)
        {
            IStorage iStorage1;
            int result;

            // Open the structured storage file in READWRITE and SHARE_EXCLUSIVE
            StgOpenStorage(sourceFilePath, null, STGM.READWRITE | STGM.SHARE_EXCLUSIVE, IntPtr.Zero, 0, out iStorage1);
            
            IEnumSTATSTG pEnumStatStg;
            uint numReturned;

            // Retrieve the array of STATSTG structures in the structured storage.
            iStorage1.EnumElements(0, IntPtr.Zero, 0, out pEnumStatStg);

            ProcessPackage(iStorage1, destinationFolder);

            
            Marshal.ReleaseComObject(iStorage1);
            iStorage1 = null;
            Marshal.ReleaseComObject(pEnumStatStg);
            pEnumStatStg = null;
            GC.Collect();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        static void ProcessPackage(IStorage pStg, string destinationFolder)
        {
            System.Runtime.InteropServices.ComTypes.IStream pStream;
            IEnumSTATSTG pEnumStatStg;
            uint numReturned;
            pStg.EnumElements(0, IntPtr.Zero, 0, out pEnumStatStg);
            System.Runtime.InteropServices.ComTypes.STATSTG[] ss = new System.Runtime.InteropServices.ComTypes.STATSTG[1];
            // Loop through the STATSTG structures in the storage.
            do
            {
                // Retrieve the STATSTG structure
                pEnumStatStg.Next(1, ss, out numReturned);
                if (numReturned != 0)
                {
                    //System.Runtime.InteropServices.ComTypes.STATSTG statstm;
                    byte[] bytT = new byte[4];
                    // Check if the pwcsName contains "Ole10Native" stream which contain the actual embedded object
                    if (ss[0].pwcsName.Contains("Ole10Native") == true)
                    {
                        // Get the stream objectOpen the stream
                        pStg.OpenStream(ss[0].pwcsName, IntPtr.Zero, (uint)STGM.READ | (uint)STGM.SHARE_EXCLUSIVE, 0, out pStream);
                        //pStream.Stat(out statstm, (int) STATFLAG.STATFLAG_DEFAULT);

                        IntPtr position = IntPtr.Zero;
                        // File name starts from 7th Byte.
                        // Position the cursor to the 7th Byte.
                        pStream.Seek(6, 0, position);

                        IntPtr ulRead = new IntPtr();
                        char[] filename = new char[260];
                        int i;

                        // Read the File name of the embedded object
                        for (i = 0; i < 260; i++)
                        {
                            pStream.Read(bytT, 1, ulRead);
                            pStream.Seek(0, 1, position);
                            filename[i] = (char)bytT[0];
                            if (bytT[0] == 0)
                            {
                                break;
                            }
                        }
                        string path = new string(filename, 0, i);

                        // Next part is the source path of the embedded object.
                        // Length is unknown. Hence, loop through each byte to read the 0 terminated string
                        // Read the source path.
                        for (i = 0; i < 260; i++)
                        {
                            pStream.Read(bytT, 1, ulRead);
                            pStream.Seek(0, 1, position);
                            filename[i] = (char)bytT[0];
                            if (bytT[0] == 0)
                            {
                                break;
                            }
                        }
                        // Source File path
                        string fullpath = new string(filename, 0, i);

                        // Unknown 4 bytes
                        pStream.Seek(4, 1, position);

                        // Next 4 byte gives the length of the temporary file path 
                        // (Office uses a temporary location to copy the files before inserting to the document)
                        // The length is in little endian format. Hence conversion is needed
                        pStream.Read(bytT, 4, ulRead);
                        ulong dwSize, dwTemp;
                        dwSize = 0;
                        dwTemp = (ulong)bytT[3];
                        dwSize += (ulong)(bytT[3] << 24);
                        dwSize += (ulong)(bytT[2] << 16);
                        dwSize += (ulong)(bytT[1] << 8);
                        dwSize += bytT[0];

                        // Skip the temporary file path
                        pStream.Seek((long)dwSize, 1, position);

                        // Next four bytes gives the size of the actual data in little endian format.
                        // Convert the format.
                        pStream.Read(bytT, 4, ulRead);
                        dwTemp = 0;
                        dwSize = 0;
                        dwTemp = (ulong)bytT[3];
                        dwSize += (ulong)(bytT[3] << 24);
                        dwSize += (ulong)(bytT[2] << 16);
                        dwSize += (ulong)(bytT[1] << 8);
                        dwSize += (ulong)bytT[0];

                        // Read the actual file content 
                        byte[] byData = new byte[dwSize];
                        pStream.Read(byData, (int)dwSize, ulRead);
                        
                        // Create the file
                        var bWriter = new BinaryWriter(File.Open(Path.Combine(destinationFolder, GetFileName(path)), FileMode.Create));
                        bWriter.Write(byData);
                        bWriter.Close();
                    }
                }
            }
            while (numReturned > 0);
        }

        private static string GetFileName(string filePath)
        {
            return Regex.Replace(filePath, @"^.*[\\]", "");
        }

    }
}
