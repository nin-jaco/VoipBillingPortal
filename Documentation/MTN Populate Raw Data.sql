Delete From rawCost;

LOAD DATA LOCAL INFILE 'c:\\Book1.csv'
INTO TABLE rawCost
FIELDS TERMINATED BY ','
LINES TERMINATED BY '\n'
(Destination, Prefix, CostPerMin, CityCode); 

-- SELECT length(citycode) FROM `asteriskcdrdb`.`rawBilling` where rawBillingID = 1143 order by 1 desc;

-- SELECT max(length(citycode)) FROM `asteriskcdrdb`.`rawBilling`;

-- SELECT * FROM `asteriskcdrdb`.`rawBilling` order by 1 desc;

Select * from rawCost Order by 1 desc;