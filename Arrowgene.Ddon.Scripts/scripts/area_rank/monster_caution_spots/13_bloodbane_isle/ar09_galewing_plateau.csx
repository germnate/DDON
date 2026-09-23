#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.BloodbaneIsle0.AsStageLayoutId(11);
    public override QuestAreaId AreaId => QuestAreaId.BloodbaneIsle;
    public override uint RequiredAreaRank => 2;

    public override void Initialize()
    {
        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.InfectedGriffin, 65, 0),
            LibDdon.Enemy.CreateAuto(EnemyId.InfectedGriffin, 65, 1),
        });
    }
}

return new MonsterSpotInfo();
