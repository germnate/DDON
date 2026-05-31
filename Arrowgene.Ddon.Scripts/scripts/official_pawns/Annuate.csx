public class OfficialPawn : IOfficialPawnScript
{
    public override string Name => "Annuate";
    public override JobId Job => JobId.Warrior;
    public override int MinLevel => 1;
    public override float Quality => PawnQuality.Good;
    public override float RentalCostMultiplier => PawnQuality.ToCostMultiplier(Quality);

    public override CDataEditInfo EditInfo => new CDataEditInfo
    {
        Sex = 1,
        Voice = 1,
        VoicePitch = 30000,
        Personality = (PawnPersonality)1,
        SpeechFreq = 1,
        BodyType = 0,
        Hair = 5,
        Beard = 17,
        Makeup = 2,
        Scar = 101,
        EyePresetNo = 3,
        NosePresetNo = 12,
        MouthPresetNo = 4,
        EyebrowTexNo = 0,
        ColorSkin = 21,
        ColorHair = 59,
        ColorBeard = 59,
        ColorEyebrow = 51,
        ColorREye = 52,
        ColorLEye = 52,
        ColorMakeup = 21,
        Sokutobu = 29450,
        Hitai = 30650,
        MimiJyouge = 30296,
        Kannkaku = 29566,
        MabisasiJyouge = 29571,
        HanakuchiJyouge = 29847,
        AgoSakiHaba = 29747,
        AgoZengo = 30032,
        AgoSakiJyouge = 30078,
        HitomiOokisa = 29884,
        MeOokisa = 29846,
        MeKaiten = 30245,
        MayuKaiten = 36900,
        MimiOokisa = 30000,
        MimiMuki = 30000,
        ElfMimi = 30000,
        MikenTakasa = 30616,
        MikenHaba = 30329,
        HohoboneRyou = 29705,
        HohoboneJyouge = 29936,
        Hohoniku = 29759,
        ErahoneJyouge = 30130,
        ErahoneHaba = 29768,
        HanaJyouge = 29660,
        HanaHaba = 30175,
        HanaTakasa = 30208,
        HanaKakudo = 30000,
        KuchiHaba = 29855,
        KuchiAtsusa = 29821,
        EyebrowUVOffsetX = 30357,
        EyebrowUVOffsetY = 29532,
        Wrinkle = 30035,
        WrinkleAlbedoBlendRate = 30000,
        WrinkleDetailNormalPower = 30000,
        MuscleAlbedoBlendRate = 30000,
        MuscleDetailNormalPower = 30000,
        Height = 48600,
        HeadSize = 40423,
        NeckOffset = 29580,
        NeckScale = 41050,
        UpperBodyScaleX = 40770,
        BellySize = 40818,
        TeatScale = 40000,
        TekubiSize = 40000,
        KoshiOffset = 29560,
        KoshiSize = 32000,
        AnkleOffset = 29599,
        Fat = 26600,
        Muscle = 37450,
        MotionFilter = 33300,
    };

    public override RentalPawnRecord Generate(OfficialPawnContext ctx)
    {
        ctx.Builder
            .WithAutoEquipment(Quality)
            .WithAutoCustomSkills(Quality)
            .WithAutoAbilities(Quality)
            .WithAutoCraft(Quality);

        return ctx.Builder.Build();
    }
}

return new OfficialPawn();
