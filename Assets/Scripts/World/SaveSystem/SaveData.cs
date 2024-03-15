using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    [System.Serializable]
    public struct LanternData
    {
        public float lightRange;
        public List<float> lanternPos;
        // holderIndex set to child index from holder's parent object, -99 if no holder
        public int holderIndex;
        public bool canBeGrabbed;
    }

    [System.Serializable]
    public struct HolderData
    {
        public bool holdingSomething;
        public float currentPower;
    }

	public string sceneName;
	public int checkpoint = -99;
    public Quaternion playerRotation = Quaternion.identity;
	public List<LanternData> lanternData = new List<LanternData>();
    public List<HolderData> holderData = new List<HolderData>();

    public string SaveToJson()
    {
        return JsonUtility.ToJson(this);
	}

    public void LoadFromJson(string jsonData)
    {
        JsonUtility.FromJsonOverwrite(jsonData, this);
    }
}
