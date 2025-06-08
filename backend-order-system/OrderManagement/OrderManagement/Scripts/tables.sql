IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Costumer' AND xtype='U')
BEGIN
    CREATE TABLE Costumer (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        CreationDate DATETIME NOT NULL,
        Name NVARCHAR(100) NOT NULL,
        Email NVARCHAR(100) NOT NULL,
        Cellphone NVARCHAR(20) NULL
    )
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Product' AND xtype='U')
BEGIN
    CREATE TABLE Product (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        CreationDate DATETIME NOT NULL,
        Name NVARCHAR(100) NOT NULL,
        Description NVARCHAR(MAX) NULL,
        Price FLOAT NOT NULL,
        QuantityAvailable INT NOT NULL
    )
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='OperationCostumerXProduct' AND xtype='U')
BEGIN
    CREATE TABLE OperationCostumerXProduct (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        CreationDate DATETIME NOT NULL,
        ProductId UNIQUEIDENTIFIER NOT NULL,
        CostumerId UNIQUEIDENTIFIER NOT NULL,
        Status INT NOT NULL,
        PurchasedQuantity INT NOT NULL,
        FOREIGN KEY (ProductId) REFERENCES Product(Id),
        FOREIGN KEY (CostumerId) REFERENCES Costumer(Id)
    )
END