
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 08/09/2018 21:11:22
-- Generated from EDMX file: c:\users\dell\source\repos\EFModelFirstDemo\EFModelFirstDemo\ModelFirstDemoDB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ModelFirstDemoDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Students'
CREATE TABLE [dbo].[Students] (
    [StudentId] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [EnrollementDate] datetime  NOT NULL
);
GO

-- Creating table 'Enrollements'
CREATE TABLE [dbo].[Enrollements] (
    [EnrollementId] int IDENTITY(1,1) NOT NULL,
    [CourseId] int  NOT NULL,
    [StudentId] int  NOT NULL,
    [Grade] int  NOT NULL,
    [StudentStudentId] int  NOT NULL,
    [CourseCourseId] int  NOT NULL
);
GO

-- Creating table 'Courses'
CREATE TABLE [dbo].[Courses] (
    [CourseId] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [Credits] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [StudentId] in table 'Students'
ALTER TABLE [dbo].[Students]
ADD CONSTRAINT [PK_Students]
    PRIMARY KEY CLUSTERED ([StudentId] ASC);
GO

-- Creating primary key on [EnrollementId] in table 'Enrollements'
ALTER TABLE [dbo].[Enrollements]
ADD CONSTRAINT [PK_Enrollements]
    PRIMARY KEY CLUSTERED ([EnrollementId] ASC);
GO

-- Creating primary key on [CourseId] in table 'Courses'
ALTER TABLE [dbo].[Courses]
ADD CONSTRAINT [PK_Courses]
    PRIMARY KEY CLUSTERED ([CourseId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [StudentStudentId] in table 'Enrollements'
ALTER TABLE [dbo].[Enrollements]
ADD CONSTRAINT [FK_StudentEnrollement]
    FOREIGN KEY ([StudentStudentId])
    REFERENCES [dbo].[Students]
        ([StudentId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StudentEnrollement'
CREATE INDEX [IX_FK_StudentEnrollement]
ON [dbo].[Enrollements]
    ([StudentStudentId]);
GO

-- Creating foreign key on [CourseCourseId] in table 'Enrollements'
ALTER TABLE [dbo].[Enrollements]
ADD CONSTRAINT [FK_CourseEnrollement]
    FOREIGN KEY ([CourseCourseId])
    REFERENCES [dbo].[Courses]
        ([CourseId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CourseEnrollement'
CREATE INDEX [IX_FK_CourseEnrollement]
ON [dbo].[Enrollements]
    ([CourseCourseId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------