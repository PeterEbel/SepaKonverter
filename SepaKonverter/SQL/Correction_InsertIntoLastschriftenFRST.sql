INSERT INTO [SEPA].[dbo].[Lastschriften]
           ([OrganisationsNr]
           ,[MessageID]
           ,[CreationDateTime]
           ,[NumberOfTransactions]
           ,[ControlSum]
           ,[InitiatingPartyname]
           ,[PaymentInformationID]
           ,[PaymentMethod]
           ,[BatchBooking]
           ,[NumberOfTransactionsPaymentInfo]
           ,[ControlSumPaymentInfo]
           ,[ServiceLevelCode]
           ,[LocalInstrumentCode]
           ,[SequenceType]
           ,[CategoryPurpose]
           ,[RequestedCollectionDate]
           ,[CreditorName]
           ,[CreditorPostalAddressCountry]
           ,[CreditorPostalAddressAddressLine]
           ,[CreditorAccountIBAN]
           ,[CreditorAccountCurrency]
           ,[CreditorAgentBIC]
           ,[ChargeBearer]
           ,[CreditorIdentification]
           ,[InstructionID]
           ,[EndToEndID]
           ,[InstructedAmount]
           ,[MandateID]
           ,[DateOfSignature]
           ,[AmendmentIndicator]
           ,[OriginalMandateID]
           ,[OriginalCreditorName]
           ,[OriginalCreditorIdentification]
           ,[OriginalDebtorAccountIBAN]
           ,[OriginalDebtorAgentBIC]
           ,[CreditorIdentificationTransactionInfo]
           ,[DebtorAgentBIC]
           ,[DebtorName]
           ,[DebtorPostalAddressCountry]
           ,[DebtorPostalAddressAddressLine]
           ,[DebtorAccountIBAN]
           ,[PurposeCode]
           ,[UnstructuredRemittanceInfo]
           ,[LastschriftStatus]
           ,[DatumImport]
           ,[DatumAbrechnung])
     VALUES
           ('10001'
           ,'DE223055000000934479932015100600000'
           ,'2015-10-06T17:04:00'
           , NULL
           , NULL
           ,'Arbeitsgemeinschaft für Kardiolog. Rehabilitation und Prävention e.V.'
           ,'100012015100600000000'
           ,'DD'
           ,'true'
           , NULL
           , NULL
           ,'SEPA'
           ,'COR1'
           ,'FRST'
           , NULL
           ,'2015-10-14'
           ,'Kard. AG Rehab u. Praevent.'
           , NULL
           , NULL
           ,'DE22305500000093447993'
           ,'EUR'
           ,'WELADEDNXXX'
           ,'SLEV'
           ,'DE88ZZZ00000553807'
           ,'201510061704001000100645'
           ,'1000100645'
           , 40
           ,'1000100645'
           ,'2015-10-14'
           ,'false'
           , NULL
           ,'Kard. AG Rehab u. Praevent.'
           , NULL
           , NULL
           , NULL
           , NULL
           ,'DEUTDEDBDUE'
           ,'Zimmermann, Reinhard'
           , NULL
           , NULL
           ,'DE92300700240779943000'
           , NULL
           , NULL
           , 0
           ,'2015-10-06T17:04:00'
           ,'2015-10-06T17:04:00'
           )


