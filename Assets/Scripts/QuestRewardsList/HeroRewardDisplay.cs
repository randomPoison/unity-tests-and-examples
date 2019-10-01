using DisplayList;
using UnityEngine;

public class HeroRewardDisplay : MonoBehaviour, IDisplayElement<HeroReward>, IDisplayElement<RandomHeroReward>
{
    public void Populate(HeroReward data)
    {
        throw new System.NotImplementedException();
    }

    public void Populate(RandomHeroReward data)
    {
        throw new System.NotImplementedException();
    }
}
