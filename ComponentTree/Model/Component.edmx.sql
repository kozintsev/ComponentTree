
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/29/2018 13:30:15
-- Generated from EDMX file: C:\Users\okozi\Documents\GitHub\ComponentTree\ComponentTree\Model\Component.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [components];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Child]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Link] DROP CONSTRAINT [FK_Child];
GO
IF OBJECT_ID(N'[dbo].[FK_Parent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Link] DROP CONSTRAINT [FK_Parent];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Component]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Component];
GO
IF OBJECT_ID(N'[dbo].[Link]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Link];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Component'
CREATE TABLE [dbo].[Component] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Link'
CREATE TABLE [dbo].[Link] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [IdParent] bigint  NULL,
    [IdChild] bigint  NULL,
    [Quantity] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Component'
ALTER TABLE [dbo].[Component]
ADD CONSTRAINT [PK_Component]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Link'
ALTER TABLE [dbo].[Link]
ADD CONSTRAINT [PK_Link]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [IdChild] in table 'Link'
ALTER TABLE [dbo].[Link]
ADD CONSTRAINT [FK_Child]
    FOREIGN KEY ([IdChild])
    REFERENCES [dbo].[Component]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Child'
CREATE INDEX [IX_FK_Child]
ON [dbo].[Link]
    ([IdChild]);
GO

-- Creating foreign key on [IdParent] in table 'Link'
ALTER TABLE [dbo].[Link]
ADD CONSTRAINT [FK_Parent]
    FOREIGN KEY ([IdParent])
    REFERENCES [dbo].[Component]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Parent'
CREATE INDEX [IX_FK_Parent]
ON [dbo].[Link]
    ([IdParent]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------