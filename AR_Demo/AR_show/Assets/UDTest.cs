using UnityEngine;
using System.Collections;
using System.Linq;
using Vuforia;
using System.Collections.Generic;

public class UDTest : MonoBehaviour, IUserDefinedTargetEventHandler
{
    ObjectTracker m_objectTracker;
    DataSet m_BuildDataSet;
    ImageTargetBuilder.FrameQuality m_FrameQuality;
    public int m_TargetCounter;
    public ImageTargetBehaviour ImageTargetTemplate;
    UserDefinedTargetBuildingBehaviour m_TargetBuildingBehaviour;
    // Use this for initialization
    void Start()
    {

        m_TargetBuildingBehaviour = this.GetComponent<UserDefinedTargetBuildingBehaviour>();
        if (m_TargetBuildingBehaviour != null)
        {
            m_TargetBuildingBehaviour.RegisterEventHandler(this);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OnFrameQualityChanged(ImageTargetBuilder.FrameQuality frameQuality)
    {
        m_FrameQuality = frameQuality;
        if (m_FrameQuality == ImageTargetBuilder.FrameQuality.FRAME_QUALITY_LOW)
        {
            Debug.Log("Low Camera Image Quality ");
        }
    }


    public void OnInitialized()
    {
        m_objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        if (m_objectTracker != null)
        {
            m_BuildDataSet = m_objectTracker.CreateDataSet();
            m_objectTracker.ActivateDataSet(m_BuildDataSet);    //激活数据集  
        }
    }


    public void OnNewTrackableSource(TrackableSource trackableSource)
    {
        m_objectTracker.Stop();  //停止追踪才能激活数据集  
        m_TargetCounter++;
        m_objectTracker.DeactivateDataSet(m_BuildDataSet);//创建一个新的数据集  
        //如果trackable满了或者已经定义了5个识别对象，那么删除旧的DataSet  
        if (m_BuildDataSet.HasReachedTrackableLimit() || m_BuildDataSet.GetTrackables().Count() > 5)
        {
            IEnumerable<Trackable> trackables = m_BuildDataSet.GetTrackables();
            Trackable oldest = null;
            foreach (Trackable trackable in trackables)
            {
                if (oldest == null || trackable.ID < oldest.ID)
                {
                    oldest = trackable;
                }
            }

            if (oldest != null)
            {
                m_BuildDataSet.Destroy(oldest, true);
            }
        }


        //定义Trackable并实例化  
        ImageTargetBehaviour imageTargetCopy = Instantiate(ImageTargetTemplate);
        //Instantiate<ImageTargetBehaviour>()(ImageTargetTemplate);  
        imageTargetCopy.gameObject.name = "UserDefinedTarget-" + m_TargetCounter;
        //复制一个Trckable到imageTargetCopy上并让它激活  
        m_BuildDataSet.CreateTrackable(trackableSource, imageTargetCopy.gameObject);
        m_objectTracker.ActivateDataSet(m_BuildDataSet);
        m_objectTracker.Start();
    }


    public void OnBuildButtonClick()
    {
        m_TargetBuildingBehaviour.BuildNewTarget("Test2", 50);

    }
}
    

