using OscCore;
using UnityEngine;

public class OscClientData
{
    //���M�p�̃N���C�A���g�f�[�^
    public OscClient client;

    //���̃N���C�A���g���g�p���Ă��邩�ǂ���
    bool isUsing;

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    public OscClientData()
    {
        this.client = new OscClient(OSCManager.broadcastAddress, OSCManager.startPort);
        this.isUsing = false;

        return;
    }

    /// <summary>
    /// ���̃N���C�A���g��L��������
    /// </summary>
    /// <param name="_address">IP�A�h���X</param>
    /// <param name="_port">�|�[�g�ԍ�</param>
    public void Assign(string _address, int _port)
    {
        this.client = new OscClient(_address, _port);
        this.isUsing = true;

        return;
    }

    /// <summary>
    /// ���̃N���C�A���g�𖳌�������
    /// </summary>
    public void Release()
    {
        this.client = new OscClient(OSCManager.broadcastAddress, OSCManager.startPort);
        this.isUsing = false;

        return;
    }

    /// <summary>
    /// ���̃N���C�A���g���g�p���Ă��邩�ǂ����̎擾
    /// </summary>
    /// <returns>���̃N���C�A���g�̗L��������</returns>
    public bool IsUsing()
    {
        return isUsing;
    }
}
