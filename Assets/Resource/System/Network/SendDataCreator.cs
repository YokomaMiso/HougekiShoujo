using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SendDataCreator
{
    ////// c#�ł͍\���̓��̃A�N�Z�X�����������߂Ƃ肠�����Spublic�錾
    //////�i�\���̂̃A�N�Z�X�͌����public����Ԍy���݂����Ȃ̂ŏ����]�T������΃v���p�e�B�ŃJ�v�Z�����j

    /// <summary>
    /// �f�[�^����M���Ɏg���f�[�^��S�Ă܂Ƃ߂����
    /// </summary>
    /// �\���̃C���X�^���X�쐬���Ƀ�������ł̕��т��Œ�
    [StructLayout(LayoutKind.Sequential)]
    public struct PlayerNetData
    {
        public PacketDataForPerFrame mainPacketData;
        public byte[] byteData;
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
        public int myPort;        //���M�|�[�g�ԍ�
        public int targetPort;    //��M�|�[�g�ԍ�
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]  //���M�f�[�^��string�̏ꍇ�K�{�@SizeConst�ɑ�������l���̃o�C�g���ŌŒ�
        public string myIP;           //���[�UIP
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string targetIP;       //���[�UIP
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public string sendAddress;    //���M�A�h���X
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public string receiveAddress; //��M�A�h���X
    }

    /// <summary>
    /// �C���Q�[�����̒ʐM���������f�[�^�i�[�p�@�Q�[�����̃f�[�^�͂����ɒ�`����
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct GameData
    {
        public Vector3 playerPos;
    }

    //////////////////////////////
    //////// �{�Ԏg�p�ϐ� ////////
    //////////////////////////////

    // NONE

    ////////////////////////////////
    //////// �f�o�b�N�p�ϐ� ////////
    ////////////////////////////////

    // NONE

    //////////////////////
    //////// �֐� ////////
    //////////////////////

    /// <summary>
    /// �\���̂���o�C�g�z��֕ϊ����܂�
    /// </summary>
    /// <typeparam name="T">�o�C�g�z��ɂ������\����</typeparam>
    /// <param name="_data">�o�C�g�z��ɂ������\���̂̃C���X�^���X</param>
    /// <returns>�o�C�g�z��</returns>
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

        return bytes;
    }

    /// <summary>
    /// �o�C�g�z�񂩂�\���̂֕ϊ����܂�
    /// </summary>
    /// <typeparam name="T">�o�C�g�z�񂩂�ϊ����������\����</typeparam>
    /// <param name="_data">�o�C�g�z��</param>
    /// <returns>�e���v���[�g�Ŏw�肵���\���̃f�[�^</returns>
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
