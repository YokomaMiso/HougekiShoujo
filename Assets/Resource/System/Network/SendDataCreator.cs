using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SendDataCreator
{
    ////// c#�ł͍\���̓��̃A�N�Z�X���������Q�b�^�[�Z�b�^�[���ЂƂÂ�邩�N���X�ɂ��邩����K�v�����邽�߂Ƃ肠�����Spublic�錾

    /// <summary>
    /// �f�[�^����M���Ɏg���f�[�^��S�Ă܂Ƃ߂����
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PlayerNetData
    {
        public PacketDataForPerFrame mainPacketData;
        public byte[] sendByteData;
        public byte[] receivedByteData;
    }

    /// <summary>
    /// ���t���[������ׂ��f�[�^�Q�\����
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PacketDataForPerFrame
    {
        public UsersBaseData comData;
        public GameData inGameData;
    }

    /// <summary>
    /// �ʐM���ɕK�{�ȃf�[�^�����\����
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct UsersBaseData
    {
        public int myPort;       //���M�|�[�g�ԍ�
        public int targetPort;    //��M�|�[�g�ԍ�
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string myIP;           //���[�UIP
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string targetIP;       //���[�UIP
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public string sendAddress;    //���M�A�h���X
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public string receiveAddress; //��M�A�h���X
    }

    /// <summary>
    /// �C���Q�[�����̒ʐM���������f�[�^�i�[�p
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct GameData
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string test;
        public int num;
    }

    //////////////////////////////
    //////// �{�Ԏg�p�ϐ� ////////
    //////////////////////////////


    ////////////////////////////////
    //////// �f�o�b�N�p�ϐ� ////////
    ////////////////////////////////

    List<PlayerNetData> userData;
    

    //////////////////////
    //////// �֐� ////////
    //////////////////////
    
    /// <summary>
    /// �Q�[���f�[�^���o�C�g�z��֕ϊ����܂�
    /// </summary>
    /// <param name="_data">�O���ō쐬���ꂽ�C���X�^���X���w�肵�܂�</param>
    /// <returns>�n�����C���X�^���X���o�C�g�z��Ƃ��ĕԂ��܂�</returns>
    public byte[] StructToByte<T>(T _data)where T : struct
    {
        int size = Marshal.SizeOf(_data);

        byte[] bytes = new byte[size];

        GCHandle gchw = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        

        try
        {
            IntPtr ptr = gchw.AddrOfPinnedObject();
            Marshal.StructureToPtr(_data, ptr, true);
        }
        finally
        {
            gchw.Free();
        }

        Debug.Log("�o�C�g�z��� " + bytes.Length);

        return bytes;
    }

    /// <summary>
    /// �o�C�g�z�񂩂�\���̂ɑg�ݒ����܂�
    /// </summary>
    /// <returns>�g�܂ꂽ�\���̂ł�</returns>
    public T ByteToStruct<T>(byte[] _data) where T : struct
    {
        T strData = default(T);

        GCHandle gch = GCHandle.Alloc(_data, GCHandleType.Pinned);

        try
        {
            IntPtr ptr = gch.AddrOfPinnedObject();
            strData = Marshal.PtrToStructure<T>(ptr);
        }
        finally
        {
            gch.Free();
        }

        return strData;
    }
}
