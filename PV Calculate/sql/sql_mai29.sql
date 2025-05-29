
create proc [dbo].[get_technique_tb]
as
SELECT [ParametreId] as'ID'
      ,[ChargeElementaire] AS 'Charge élémentaire',
		[ConstanteBoltzmann] AS 'Constante de Boltzmann',
		[TempReference] AS 'Température de référence',
		[IrradianceReference] AS 'Irradiance de référence',
		[NbCellulesSerie] AS 'Nombre de cellules en série',
		[Isc] AS 'Courant de court-circuit (Isc)',
		[Voc] AS 'Tension en circuit ouvert (Voc)',
		[Impp] AS 'Courant au point de puissance maximale (Impp)',
		[Vmpp] AS 'Tension au point de puissance maximale (Vmpp)',
		[Impp]*[Vmpp] AS 'Puissance maximale (Pmpp)',
		[CoeffTemperature] AS 'Coefficient de température',
		[ResistanceSerie] AS 'Résistance série',
		[ResistanceParallele] AS 'Résistance parallèle',
		[FacteurIdealite] AS 'Facteur d’idéalité',
		[NbPanneauxSerie] AS 'Nombre de panneaux en série',
		[NbChainesParallele] AS 'Nombre de chaînes en parallèle',
		[NOCT] AS 'Température de fonctionnement nominale (NOCT)' 
  FROM [dbo].[ParametresTechniques]

  ------------------------------------------------------------------------
  USE [db_uc_projet]
GO

-- Create the stored procedure
CREATE PROCEDURE [dbo].[InsertParametresTechniques]
    @ChargeElementaire FLOAT,
    @ConstanteBoltzmann FLOAT,
    @TempReference FLOAT,
    @IrradianceReference FLOAT,
    @NbCellulesSerie INT,
    @Isc FLOAT,
    @Voc FLOAT,
    @Impp FLOAT,
    @Vmpp FLOAT,
    @Pmp FLOAT,
    @CoeffTemperature FLOAT,
    @ResistanceSerie FLOAT,
    @ResistanceParallele FLOAT,
    @FacteurIdealite FLOAT,
    @NbPanneauxSerie INT,
    @NbChainesParallele INT,
    @NOCT FLOAT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [dbo].[ParametresTechniques] (
        [ChargeElementaire],
        [ConstanteBoltzmann],
        [TempReference],
        [IrradianceReference],
        [NbCellulesSerie],
        [Isc],
        [Voc],
        [Impp],
        [Vmpp],
        [Pmp],
        [CoeffTemperature],
        [ResistanceSerie],
        [ResistanceParallele],
        [FacteurIdealite],
        [NbPanneauxSerie],
        [NbChainesParallele],
        [NOCT]
    )
    VALUES (
        @ChargeElementaire,
        @ConstanteBoltzmann,
        @TempReference,
        @IrradianceReference,
        @NbCellulesSerie,
        @Isc,
        @Voc,
        @Impp,
        @Vmpp,
        @Pmp,
        @CoeffTemperature,
        @ResistanceSerie,
        @ResistanceParallele,
        @FacteurIdealite,
        @NbPanneauxSerie,
        @NbChainesParallele,
        @NOCT
    );
END
GO
------------------------------------------------------
create proc delete_technique
@para_id int
as
DELETE FROM [dbo].[ParametresTechniques]
      WHERE ParametreId = @para_id
GO

----------------------------------------------------
create proc get_liste_centrale
as
SELECT [ID]  
      ,[NAME] as'Nom'
      ,[puissance] as'Puissance'
      ,[Latitude] as'Latitude'
      ,[Longitude] as'Longitude'
      ,[technique_id] as'Code téchnique'
      ,[id_bus] as'Code bus'
  FROM [dbo].[TB_PLACEMENT_CENTRALE_PV] 
GO

--29/052025 labo 106 
 create PROC edit_centrale_info
 @ID int
,@NAME		nvarchar(50) 
,@puissance		float
,@technique_id	int
,@id_bus		nvarchar(50)
AS
UPDATE [dbo].[TB_PLACEMENT_CENTRALE_PV]
   SET [NAME] = @NAME
      ,[Latitude] = [Latitude]
      ,[Longitude] =[Longitude]
      ,[technique_id] = @technique_id
      ,[puissance] = @puissance
      ,[id_bus] = @id_bus
 WHERE ID = @ID
GO
-----------------------------------------------------
