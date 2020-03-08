namespace ThreeS.Jobs
{
    public interface ISQLJobs
    {
        //void RebuildAllIndex();
        void RebuildOrReorganizeIndexes();
        void ShrinkDataBases();
    }
}