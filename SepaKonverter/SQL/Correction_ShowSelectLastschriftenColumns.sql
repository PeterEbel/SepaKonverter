/****** Skript für SelectTopNRows-Befehl aus SSMS  ******/
/*SELECT * FROM [SEPA].[dbo].[Lastschriften]*/
SELECT DebtorName, MessageID, PaymentInformationID, SequenceType, InstructedAmount FROM [SEPA].[dbo].[Lastschriften] order by DebtorName