SELECT DebtorName,
       MessageID,
       PaymentInformationID,
       SequenceType,
       InstructionID,
       EndToEndID,
       MandateID,
       DebtorAgentBIC,
       DebtorAccountIBAN,
       InstructedAmount 
FROM [SEPA].[dbo].[Lastschriften] 
WHERE SequenceType = 'FRST'
ORDER BY DebtorName