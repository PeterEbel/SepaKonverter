SELECT * FROM [SEPA].[dbo].[Lastschriften] 
where SequenceType = 'FRST'
order by DatumImport desc, SequenceType asc
