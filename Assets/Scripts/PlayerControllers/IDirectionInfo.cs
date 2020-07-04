using UnityEngine;

public abstract class IDirectionInfo
{

    protected GlobalUtils.Direction m_dir;

    protected bool isRightOriented(){
        return m_dir == GlobalUtils.Direction.Right;
    }

    protected bool isLeftOriented(){
        return m_dir == GlobalUtils.Direction.Left;
    }

    public virtual GlobalUtils.Direction GetDirection(){
        return m_dir;
    }

}
