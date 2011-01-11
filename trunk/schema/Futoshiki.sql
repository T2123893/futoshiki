/*
 * $Id$
 * 
 * Coursework ¨C Futoshiki.DB
 *
 * This file is the result of my own work. Any contributions to the work by 
 * third parties, other than tutors, are stated clearly below this declaration. 
 * Should this statement prove to be untrue I recognise the right and duty of 
 * the Board of Examiners to take appropriate action in line with the university's 
 * regulations on assessment. 
 */




USE [m_SOFT40081_T2123893]

-- ============================================================================
-- Create table
-- Table Name: dbo.puzzel_Grids
-- Description: To store information of Futoshiki object
-- Field number: 4
-- ============================================================================
DROP TABLE [dbo].[puzzel_Grids]
CREATE TABLE [dbo].[puzzel_Grids](
	[UserId] [uniqueidentifier] NOT NULL,
	[Id] [uniqueidentifier] NOT NULL PRIMARY KEY,
	[Scale] [tinyint] NOT NULL,
	[Status] [tinyint] NOT NULL,
)
GO


-- ============================================================================
-- Create table
-- Table Name: dbo.puzzel_Cells
-- Description: To store information of Futoshiki cells
-- Field number: 6
-- ============================================================================
DROP TABLE [dbo].[puzzel_Cells]
CREATE TABLE [dbo].[puzzel_Cells](
	[Id] [uniqueidentifier] NOT NULL PRIMARY KEY,
	[GridId] [uniqueidentifier] NOT NULL,
	[Row] [int] NOT NULL,
	[Col] [int] NOT NULL,
	[Val] [nchar](4) NULL,
	[IsWritable] [bit] NOT NULL
) 
GO