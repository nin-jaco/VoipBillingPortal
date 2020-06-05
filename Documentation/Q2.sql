-- --------------------------------------------------------------------------------
-- Routine DDL
-- Note: comments before and after the routine body will not be stored by the server
-- --------------------------------------------------------------------------------
DELIMITER $$

CREATE DEFINER=`reports`@`%` PROCEDURE `DoMonthlyInvoiceRun`(   IN StartDate DATETIME,
                                                                IN EndDate DATETIME)
BEGIN
    DECLARE done            INT         DEFAULT 0;
    DECLARE iBillingID      INT;
    DECLARE sDst            VARCHAR(80);
    DECLARE iDuration       FLOAT;
    DECLARE fCost           FLOAT;
    DECLARE sDestination    VARCHAR(255);
    DECLARE fTelkomNat      FLOAT;
    
    DECLARE BillingRecs CURSOR FOR SELECT BillingID, dst, duration FROM Billing Where BillingDesc IS NULL;
    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = 1;
    DECLARE CONTINUE HANDLER FOR SQLSTATE '02000' BEGIN END;
    
    -- Insert base records per date range
	Insert Into Billing( calldate, clid, src, dst, duration, billsec, accountcode,
			    dcontext, channel, dstchannel)
	Select  calldate, clid, src, dst, duration, billsec, accountcode,
		dcontext, channel, dstchannel
	From    cdr
	-- Where   calldate >= StartDate
	-- And     calldate <= EndDate
	WHERE     disposition = 'answered'
	And     dst<> 's'
	And     length(src) < 5
	And     length(dst) > 5;

	-- Update Client ID
	Update  Billing LEFT JOIN Client ON Billing.AccountCode = Client.ClientCode
	Set     Billing.ClientID = Client.ClientID
    Where   Billing.billedind is null;
    
    
    -- Get Telkom national billing rate
    Select CostPerSec into fTelkomNat From CostLocal Where Destination = 'Telkom-National';

    -- Update BillingDesc, calculate billingamount    
    OPEN BillingRecs;
    
    read_loop: LOOP
        FETCH BillingRecs INTO iBillingID, sDst, iDuration;
        IF done THEN
            LEAVE read_loop;
        END IF;
        
        IF LEFT(sDst, 2) = '00' THEN
            -- International Calls
            SET sDst = 1;
        ELSE
            -- Local calls
            Select 0.0 into fCost;
            Select CostPerSec into fCost From CostLocal Where Prefix + CityCode = Left(sDst, 3);
            -- , sDestination = Destination
select fCost;
            IF fCost <> 0 THEN
                -- Mobile Or JHB
                Update Billing Set billingdesc = sDestination, billingamount = fCost * iDuration Where billingid = iBillingID;
            ELSE
                -- Telkom National
                Update Billing Set billingdesc = 'Telkom-National', billingamount = (iDuration * fTelkomNat) Where billingid = iBillingID;
            END IF;
        END IF;
    END LOOP;
    
    CLOSE BillingRecs;
    
END


