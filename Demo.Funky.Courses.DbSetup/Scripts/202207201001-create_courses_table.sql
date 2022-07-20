CREATE TABLE [dbo].[tblCourses]
(
    [Id] INT NOT NULL IDENTITY PRIMARY KEY,
    [Name] NVARCHAR(50) NOT NULL,
    [EnrollmentDate] DATETIME NOT NULL
    )