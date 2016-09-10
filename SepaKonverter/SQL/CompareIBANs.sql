
 select A.DebtorName, A.DebtorAgentBIC, A.DebtorAccountIBAN from Lastschriften A
  
  where A.SequenceType = 'FRST' and A.DatumAbrechnung is null
  order by a.DebtorName
 
  
