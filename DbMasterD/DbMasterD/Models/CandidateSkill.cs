using System;
using System.Collections.Generic;

namespace DbMasterD.Models;

public partial class CandidateSkill
{
    public int CandidateSkillId { get; set; }

    public int SkillId { get; set; }

    public int CandidateId { get; set; }

    public virtual Candidate Candidate { get; set; } = null!;

    public virtual Skill Skill { get; set; } = null!;
}
