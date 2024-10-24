CREATE DATABASE candidate_db
GO
USE candidate_db
GO
CREATE TABLE Skills
(
	skillId INT PRIMARY KEY IDENTITY,
	skillName NVARCHAR(50) NOT NULL
)
GO
CREATE TABLE Candidates 
(
    CandidateId   INT IDENTITY NOT NULL PRIMARY KEY,
    CandidateName NVARCHAR (50)  NOT NULL,
    DateOfBirth   DATE           NOT NULL,
    Phone         NVARCHAR (30) NOT NULL,
    Image         NVARCHAR (MAX) NULL,
    Fresher       BIT            NOT NULL
)
GO
CREATE TABLE CandidateSkills 
(
    CandidateSkillId INT IDENTITY NOT NULL PRIMARY KEY,
    SkillId          INT NOT NULL REFERENCES Skills(skillId),
    CandidateId      INT NOT NULL REFERENCES Candidates(CandidateId)
)
