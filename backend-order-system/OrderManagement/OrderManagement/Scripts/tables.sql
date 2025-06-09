IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Customer' AND xtype='U')
BEGIN
    CREATE TABLE Customer (
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

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Order' AND xtype='U')
BEGIN
    CREATE TABLE [Order] (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        CustomerId UNIQUEIDENTIFIER NOT NULL,
        Status INT NOT NULL,
        CreationDate DATETIME NOT NULL,
        CancellationDate DATETIME NULL
    )
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='OrderXProduct' AND xtype='U')
BEGIN
    CREATE TABLE OrderXProduct (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        CreationDate DATETIME NOT NULL,
        ProductId UNIQUEIDENTIFIER NOT NULL,
        OrderId UNIQUEIDENTIFIER NOT NULL,
        QuantityPurchased INT NOT NULL,
        ProductValue FLOAT NOT NULL,
        FOREIGN KEY (OrderId) REFERENCES [Order](Id)
    )
END