ALTER PROCEDURE [dbo].[spCalCulateBillig]
AS
BEGIN
	SET NOCOUNT ON
	SET XACT_ABORT ON
	
	DECLARE	@iBillingID			INT,
			@sDst				VARCHAR(80),
			@iDuration			FLOAT,
			@fCost				FLOAT,
			@sDestination		VARCHAR(255),
			@fTelkomNat			FLOAT,
			@iCount				INT,
			@fConnCost			FLOAT,
			@iCDRSourceID		INT,
			@sServerDesc		VARCHAR(50),
			@dLastDownload		DATETIME,
			@sLastDownload		VARCHAR(20)

	--Import From MySql CDR to Billing Table
	DECLARE Time_cursor CURSOR FOR	SELECT	CDRSourceID, ServerDesc, ISNULL(LastDownload, '01 Jan 2011 00:00')
									FROM	CDRSource
	OPEN Time_cursor
	FETCH NEXT FROM Time_cursor INTO @iCDRSourceID, @sServerDesc, @dLastDownload
	WHILE @@FETCH_STATUS = 0
	BEGIN
		Select @sLastDownload = convert(varchar(20), @dLastDownload, 20)
		
		--EXEC sp_executesql 'Insert Into Billing(CallDate, CLID, Src, Dst, Duration, BillSec, AccountCode, dContext, Channel, DstChannel, CDRSourceID) EXEC(''Select calldate, clid, src, dst, duration, billsec, accountcode, dcontext, channel, dstchannel, '' + @iCDRSourceID + '' from cdr where calldate >= '''' + @sLastDownload + '''' and disposition = '''answered''' And dst<> '''s''' And length(src) < 5 And length(dst) > 5'') AT LaserNet;'
		Insert Into Billing(CallDate, CLID, Src, Dst, Duration, BillSec, AccountCode, dContext, Channel, DstChannel, CDRSourceID)  
		EXEC('Select calldate, clid, src, dst, duration, billsec, accountcode, dcontext, channel, dstchannel, ' + @iCDRSourceID + ' from cdr where calldate >= ''' + @sLastDownload + ''' and disposition = ''answered'' And dst<> ''s'' And length(src) < 5 And length(dst) > 5') AT LaserNet;
		
		Update	CDRSource
		Set		LastDownload = GETDATE()
		Where	CDRSourceID = @iCDRSourceID 
		
		FETCH NEXT FROM Time_cursor INTO @iCDRSourceID, @sServerDesc, @dLastDownload
	END
	CLOSE Time_cursor
	DEALLOCATE Time_cursor
		
	-- Update AccountCode
	Update	Billing
	Set		ClientID = Client.ClientID 
	From	Client
	Where	AccountCode = Client.ClientCode
	And		Billing.ClientID IS NULL
			
	-- Get Telkom national billing rate
    Select	@fTelkomNat = CostPerSec
    From	CostLocal
    Where	Destination = 'Telkom-National'

    -- Update BillingDesc, calculate billingamount  
	DECLARE Time_cursor CURSOR FOR	SELECT	BillingID, dst, duration
									FROM	Billing
									Where	Destination IS NULL
	OPEN Time_cursor
	FETCH NEXT FROM Time_cursor INTO @iBillingID, @sDst, @iDuration
	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF LEFT(@sDst, 2) = '00'
        BEGIN
            -- International Calls
            Select @iCount = 3
            While @iCount < 12
            BEGIN
				-- Get Internation Value
				Select	@fCost = 0
				Select	@fCost = CostPerSec, @sDestination = Destination, @fConnCost = ConnectionCost
				From	Cost
				Where	'00' + Prefix + CityCode = Left(@sDst, @iCount)
				
				IF @fCost <> 0
					-- Update BillingAmount
					Update	Billing
					Set		Destination = @sDestination, BillingAmount = @fConnCost + (@fCost * @iDuration)
					Where	BillingID = @iBillingID
            
				Select @iCount = @iCount + 1
            END
        END
        ELSE
        BEGIN
            -- Local calls
            Select	@fCost = 0
            Select	@fCost = CostPerSec, @sDestination = Destination
            From	CostLocal
            Where	Prefix + CityCode = Left(@sDst, 3)
			
            IF @fCost <> 0
                -- Mobile Or JHB
                Update	Billing
                Set		Destination = @sDestination, BillingAmount = @fCost * @iDuration
                Where	BillingID = @iBillingID
            ELSE
                -- Telkom National
                Update	Billing
                Set		Destination = 'Telkom-National', BillingAmount = (@iDuration * @fTelkomNat)
                Where	BillingID = @iBillingID
		END

		FETCH NEXT FROM Time_cursor INTO @iBillingID, @sDst, @iDuration
	END
	CLOSE Time_cursor
	DEALLOCATE Time_cursor

	SET NOCOUNT OFF
END
