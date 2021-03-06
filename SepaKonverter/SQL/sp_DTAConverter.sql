USE [SEPA_DEV]
GO
/****** Object:  StoredProcedure [dbo].[sp_DTAConverter]    Script Date: 10/25/2014 17:27:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Peter Ebel
-- Create date: 2013-09-04
-- Description:	Konvertiert DTA- in SEPA-Tabelle
-- =============================================
ALTER PROCEDURE [dbo].[sp_DTAConverter] (@OrganisationsID nchar(5))
AS
BEGIN

  DECLARE 
  /*SEPA*/
    @OrganisationsNr nchar(10),
    @MessageID nvarchar(35),
    @CreationDateTime nchar(19),
    @NumberOfTransactions int,
    @ControlSum decimal(15,2),
    @InitiatingPartyName nvarchar(70),
    @PaymentInformationID nvarchar(35),
    @PaymentMethod nchar(2),
    @BatchBooking nvarchar(5),
	@NumberOfTransactionsPaymentInfo int,
	@ControlSumPaymentInfo decimal(15,2),
	@ServiceLevelCode nchar(4),
	@LocalInstrumentCode nvarchar(50),
	@SequenceNumber int,
	@SequenceType nchar(4),
	@CategoryPurpose nvarchar(50),
	@RequestedCollectionDate nchar(19),
	@CreditorName nvarchar(70),
	@CreditorPostalAddressCountry nchar(2),
	@CreditorPostalAddressAddressLine nvarchar(140),
	@CreditorAccountIBAN nchar(34),
	@CreditorAccountCurrency nchar(3),
	@CreditorAgentBIC nchar(11),
	@ChargeBearer nchar(4),
	@CreditorIdentification nvarchar(35),
	@InstructionID nvarchar(35),
	@EndToEndID nvarchar(35),
	@InstructedAmount decimal(15,2),
	@MandateID nvarchar(35),
	@DateOfSignature nchar(19),
	@AmendmentIndicator nvarchar(5),
	@OriginalMandateID nvarchar(35),
	@OriginalCreditorName nvarchar(35),
	@OriginalCreditorIdentification nvarchar(35),
	@OriginalDebtorAccountIBAN nvarchar(34),
	@OriginalDebtorAgentBIC nchar(11),
	@CreditorIdentificationTransactionInfo nvarchar(35),
	@DebtorAgentBIC nchar(11),
	@DebtorName nvarchar(70),
	@DebtorPostalAddressCountry nchar(2),
	@DebtorPostalAddressAddressLine nvarchar(140),
	@DebtorAccountIBAN nvarchar(34),
	@PurposeCode nchar(10),
	@UnstructuredRemittanceInfo nvarchar(140), 
	@LastschriftStatus int,
	@DatumImport nchar(19),
	@DatumAbrechnung nchar(19),
    
   /*DTA*/
	@Bankleitzahl nvarchar(8),
	@Kontonummer nvarchar(10),
	@InterneKundennummer nvarchar(13),
	@Textschluessel nvarchar(2),
	@TextschluesselErgaenzung nvarchar(3),
	@BetragInDM decimal(15, 5),
	@GegenkontoBankleitzahl nvarchar(8),
	@GegenkontoNr nvarchar(10),
	@BetragInEuro decimal(15, 5),
	@Valutadatum nvarchar(6),
	@Kontoinhaber nvarchar(27),
	@GegenkontoInhaber nvarchar(27),
	@Verwendungszweck nvarchar(27),
	@Waehrungskennzeichen nvarchar(3),
  /*Other*/
    @CurrentDate smalldatetime,
    @Mitgliedsnummer nchar(10),
    @MandateAlreadyUsed int
	
  SELECT @CurrentDate = GETUTCDATE()
  SELECT @ControlSum = 0
  SELECT @NumberOfTransactions = 0
 
  DECLARE DTACursor CURSOR FOR
  SELECT  Bankleitzahl,
          Kontonummer,
          InterneKundennummer,
          Textschluessel,
          TextschluesselErgaenzung,
          BetragInDM,
          GegenkontoBankleitzahl,
          GegenkontoNr,
          BetragInEuro,
          Valutadatum,
          Kontoinhaber,
          GegenkontoInhaber,
          Verwendungszweck,
          Waehrungskennzeichen
/*FROM SEPA_DEV.dbo.DTA*/
  FROM DELL.Mitgliederverwaltung.dbo.DTA
  ORDER BY InterneKundennummer

  OPEN DTACursor

  FETCH NEXT FROM DTACursor INTO 
    @Bankleitzahl,
    @Kontonummer,
    @InterneKundennummer,
    @Textschluessel,
    @TextschluesselErgaenzung,
    @BetragInDM,
    @GegenkontoBankleitzahl,
    @GegenkontoNr,
    @BetragInEuro,
    @Valutadatum,
    @Kontoinhaber,
    @GegenkontoInhaber,
    @Verwendungszweck,
    @Waehrungskennzeichen

  WHILE @@FETCH_STATUS = 0
  BEGIN
    
    SELECT @OrganisationsNr = @OrganisationsID
    SELECT @Mitgliedsnummer = dbo.CreateMitgliedsnummer(@OrganisationsNr, @InterneKundennummer)
	SELECT @MandateID = @Mitgliedsnummer
    SELECT @Kontonummer = RTRIM(@Kontonummer)
  /*Fehlende führende Nullen in der Kontonummer auffüllen */  
    SELECT @Kontonummer = REPLICATE('0', 10 - LEN(@Kontonummer)) + @Kontonummer
    SELECT @DebtorAccountIBAN = dbo.GetIBAN(N'DE', @Bankleitzahl, @Kontonummer)
    SELECT @CreditorAccountIBAN = dbo.GetIBAN(N'DE', @GegenkontoBankleitzahl, @GegenkontoNr)
    SELECT @CreditorAgentBIC = BIC FROM dbo.View_Banken WHERE BLZ = @GegenkontoBankleitzahl
    SELECT @DebtorAgentBIC = BIC FROM dbo.View_Banken WHERE BLZ = @Bankleitzahl

  /*Prüfen, ob für den Teilnehmer bereits eine Lastschrift gezogen wurden und SequenceType entsprechend setzen */
    SELECT @MandateAlreadyUsed = 0
    SELECT @MandateAlreadyUsed = COUNT(*) FROM SEPA_DEV.dbo.Lastschriften WHERE OrganisationsNr = @OrganisationsID AND MandateID = @Mitgliedsnummer
    IF @MandateAlreadyUsed = 0
      BEGIN
        SELECT @SequenceNumber = 0
        SELECT @SequenceType = N'FRST'
      END  
    ELSE
      BEGIN
        SELECT @SequenceNumber = 1
        SELECT @SequenceType = N'RCUR'
      END

/*  SELECT @MessageID = RTRIM(@CreditorAccountIBAN) + CONVERT(VARCHAR(8),@CurrentDate,112)+ REPLACE(CONVERT(VARCHAR(8),@CurrentDate,108),':','') */
    SELECT @MessageID = RTRIM(@CreditorAccountIBAN) + CONVERT(VARCHAR(8),@CurrentDate,112)+ N'0000' + LTRIM(STR(@SequenceNumber))
    SELECT @CreationDateTime = REPLACE(CONVERT(nchar(23),@CurrentDate,126),' ', 'T')
    SELECT @NumberOfTransactions = NULL 
    SELECT @ControlSum = NULL
    SELECT @InitiatingPartyName = NAME FROM SEPA_DEV.dbo.Organisationen WHERE OrganisationsNr = @OrganisationsNr
/*  SELECT @PaymentInformationID = RTRIM(@OrganisationsNr) + CONVERT(varchar(8),@CurrentDate,112) + REPLACE(CONVERT(VARCHAR(8),@CurrentDate,108),':','') */
    SELECT @PaymentInformationID = RTRIM(@OrganisationsNr) + CONVERT(varchar(8),@CurrentDate,112) + N'0000000' + LTRIM(STR(@SequenceNumber))
    SELECT @PaymentMethod = N'DD'
    SELECT @BatchBooking = 'true'
	SELECT @NumberOfTransactionsPaymentInfo = @NumberOfTransactions
	SELECT @ControlSumPaymentInfo = NULL
	SELECT @ServiceLevelCode = N'SEPA'
	SELECT @LocalInstrumentCode = N'COR1'

 
	SELECT @CategoryPurpose = NULL
/*  SELECT @RequestedCollectionDate = CONVERT(varchar(10), '20' + RIGHT(@Valutadatum,2) + '-'+ SUBSTRING(@Valutadatum,3,2) + '-' + LEFT(@Valutadatum,2)) */
    SELECT @RequestedCollectionDate = CONVERT(varchar(10), '20' + RIGHT(@Valutadatum,2) + '-'+ SUBSTRING(@Valutadatum,3,2) + '-' + LEFT(@Valutadatum,2))
	SELECT @CreditorName = @GegenkontoInhaber
	SELECT @CreditorPostalAddressCountry = NULL
	SELECT @CreditorPostalAddressAddressLine = NULL
	SELECT @CreditorAccountCurrency =N'EUR'
	SELECT @ChargeBearer = N'SLEV'
	SELECT @CreditorIdentification = CreditorSchemeID FROM SEPA_DEV.dbo.Organisationen WHERE OrganisationsNr = @OrganisationsID
	SELECT @InstructionID = CONVERT(varchar(8),@CurrentDate,112) + REPLACE(CONVERT(VARCHAR(8),@CurrentDate,108),':','') + @Mitgliedsnummer
	SELECT @EndToEndID = @Mitgliedsnummer
	SELECT @InstructedAmount = @BetragInEuro
  /*SELECT @DateOfSignature = REPLACE(CONVERT(nchar(23),@CurrentDate,126),' ', 'T')*/
    SELECT @DateOfSignature = CONVERT(char(10),CONVERT (date, GETDATE()))
	SELECT @AmendmentIndicator = N'false'
	SELECT @OriginalMandateID = NULL
	SELECT @OriginalCreditorName = N'AG für kard. Reha und Präv. e.V.'
	SELECT @OriginalCreditorIdentification = NULL
	SELECT @OriginalDebtorAccountIBAN = NULL
	SELECT @OriginalDebtorAgentBIC = NULL
	SELECT @CreditorIdentificationTransactionInfo = NULL
	SELECT @DebtorName = @Kontoinhaber
	SELECT @DebtorPostalAddressCountry = NULL
	SELECT @DebtorPostalAddressAddressLine = NULL
	SELECT @PurposeCode = NULL
	SELECT @UnstructuredRemittanceInfo = @CategoryPurpose    
    SELECT @LastschriftStatus = 0
	SELECT @DatumImport = REPLACE(CONVERT(nchar(19),@CurrentDate,126),' ', 'T')
    SELECT @DatumAbrechnung = NULL

    INSERT INTO dbo.Lastschriften 
    (
        OrganisationsNr,
        MessageID,
        CreationDateTime,
        NumberOfTransactions,
        ControlSum,
        InitiatingPartyname,
        PaymentInformationID,
        PaymentMethod,
        BatchBooking,
        NumberOfTransactionsPaymentInfo,
        ControlSumPaymentInfo,
        ServiceLevelCode,
        LocalInstrumentCode,
        SequenceType,
        CategoryPurpose,
        RequestedCollectionDate,
        CreditorName,
        CreditorPostalAddressCountry,
        CreditorPostalAddressAddressLine,
        CreditorAccountIBAN,
        CreditorAccountCurrency,
        CreditorAgentBIC,
        ChargeBearer,
        CreditorIdentification,
        InstructionID,
        EndToEndID,
        InstructedAmount,
        MandateID,
        DateOfSignature,
        AmendmentIndicator,
        OriginalMandateID,
        OriginalCreditorName,
        OriginalCreditorIdentification,
        OriginalDebtorAccountIBAN,
        OriginalDebtorAgentBIC,
        CreditorIdentificationTransactionInfo,
        DebtorAgentBIC,
        DebtorName,
        DebtorPostalAddressCountry,
        DebtorPostalAddressAddressLine,
        DebtorAccountIBAN,
        PurposeCode,
        UnstructuredRemittanceInfo,
        LastschriftStatus,
        DatumImport,
        DatumAbrechnung
        
    )
    VALUES
    (
        @OrganisationsNr,
        @MessageID,
        @CreationDateTime,
        @NumberOfTransactions,
        @ControlSum,
        @InitiatingPartyname,
        @PaymentInformationID,
        @PaymentMethod,
        @BatchBooking,
        @NumberOfTransactionsPaymentInfo,
        @ControlSumPaymentInfo,
        @ServiceLevelCode,
        @LocalInstrumentCode,
        @SequenceType,
        @CategoryPurpose,
        @RequestedCollectionDate,
        @CreditorName,
        @CreditorPostalAddressCountry,
        @CreditorPostalAddressAddressLine,
        @CreditorAccountIBAN,
        @CreditorAccountCurrency,
        @CreditorAgentBIC,
        @ChargeBearer,
        @CreditorIdentification,
        @InstructionID,
        @EndToEndID,
        @InstructedAmount,
        @MandateID,
        @DateOfSignature,
        @AmendmentIndicator,
        @OriginalMandateID,
        @OriginalCreditorName,
        @OriginalCreditorIdentification,
        @OriginalDebtorAccountIBAN,
        @OriginalDebtorAgentBIC,
        @CreditorIdentificationTransactionInfo,
        @DebtorAgentBIC,
        @DebtorName,
        @DebtorPostalAddressCountry,
        @DebtorPostalAddressAddressLine,
        @DebtorAccountIBAN,
        @PurposeCode,
        @UnstructuredRemittanceInfo,
        @LastschriftStatus,
        @DatumImport,
        @DatumAbrechnung
    )
    FETCH NEXT FROM DTACursor INTO 
        @Bankleitzahl,
        @Kontonummer,
        @InterneKundennummer,
        @Textschluessel,
        @TextschluesselErgaenzung,
        @BetragInDM,
        @GegenkontoBankleitzahl,
        @GegenkontoNr,
        @BetragInEuro,
        @Valutadatum,
        @Kontoinhaber,
        @GegenkontoInhaber,
        @Verwendungszweck,
        @Waehrungskennzeichen
  END
  CLOSE DTACursor
  DEALLOCATE DTACursor

END
