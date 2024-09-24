using System.Runtime.InteropServices;
using UnityEngine;

public class IngameData
{
    /// <summary>
    /// �f�[�^����M���Ɏg���f�[�^��S�Ă܂Ƃ߂����
    /// </summary>
    /// �\���̃C���X�^���X�쐬���Ƀ�������ł̕��т��Œ�
    [StructLayout(LayoutKind.Sequential)]
    public struct PlayerNetData
    {
        public PacketDataForPerFrame mainPacketData;
        public int PlayerID;
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
        //public int targetPort;    //��M�|�[�g�ԍ�
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]  //���M�f�[�^��string�̏ꍇ�K�{�@SizeConst�ɑ�������l���̃o�C�g���ŌŒ�
        public string myIP;           //���[�UIP
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        //public string targetIP;       //���[�UIP
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
        public Vector3 playerStickValue;
        public PLAYER_STATE playerState;
        public bool fire;
        public bool useSub;
        public bool alive;
        public RADIO_CHAT_ID playerChatID;
    }
}
