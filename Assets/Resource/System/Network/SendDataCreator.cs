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
    public struct PlayerNetData
    {
        public PacketDataForPerFrame mainPacketData;
        public byte[] sendData;
        public byte[] receiveData;
    }

    /// <summary>
    /// ���t���[������ׂ��f�[�^�Q�\����
    /// </summary>
    public struct PacketDataForPerFrame
    {
        public UsersBaseData comData;
        public GameData inGameData;
    }

    /// <summary>
    /// �ʐM���ɕK�{�ȃf�[�^�����\����
    /// </summary>
    public struct UsersBaseData
    {
        public int myPort;       //���M�|�[�g�ԍ�
        public int targetPort;    //��M�|�[�g�ԍ�
        public string myIP;           //���[�UIP
        public string targetIP;       //���[�UIP
        public string sendAddress;    //���M�A�h���X
        public string receiveAddress; //��M�A�h���X
    }

    /// <summary>
    /// �C���Q�[�����̒ʐM���������f�[�^�i�[�p
    /// </summary>
    public struct GameData
    {

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
    public byte[] StructToByte(PlayerNetData _data)
    {
        int size = Marshal.SizeOf(_data.mainPacketData);

        byte[] bytes = new byte[size];

        GCHandle gchw = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        Marshal.StructureToPtr(_data, gchw.AddrOfPinnedObject(), false);
        gchw.Free();

        return bytes;
    }

    /// <summary>
    /// �o�C�g�z�񂩂�\���̂ɑg�ݒ����܂�
    /// </summary>
    /// <returns>�g�܂ꂽ�\���̂ł�</returns>
    public PacketDataForPerFrame ByteToStruct()
    {
        PacketDataForPerFrame _data = new PacketDataForPerFrame();
        int size = Marshal.SizeOf(_data);

        byte[] buffer = new byte[size];

        GCHandle gch = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        _data = (PacketDataForPerFrame)Marshal.PtrToStructure(gch.AddrOfPinnedObject(), typeof(PacketDataForPerFrame));
        gch.Free();

        return _data;
    }
}
