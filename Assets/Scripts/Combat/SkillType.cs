public enum SkillType {

    //Physical
    Slash,
    Double_Slash,
    Triple_Slash,
    Ultimate_Slash,
    Bash,
    Bite,
    Scratch,

    //RangedPhysical
    BowShot, //Single
    CriticalShot, //Single
    TripleShot, //All
    

    //Magical
    FireArrow,
    IceSpikes,
    WindGust,
    InvisibleSlash,

    //Support
    Heal,
    AreaHeal,
    Restore,
    AreaRestore,
}

public enum SkillCategory {
    All,
    Single
}

public enum SkillReqType {
    None,
    Health,
    Mana
}