IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Cliente' AND xtype='U')
BEGIN
    CREATE TABLE Cliente (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        DataDeCadastro DATETIME NOT NULL,
        Nome NVARCHAR(100) NOT NULL,
        Email NVARCHAR(100) NOT NULL,
        Telefone NVARCHAR(20) NULL
    )
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Produto' AND xtype='U')
BEGIN
    CREATE TABLE Produto (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        DataDeCadastro DATETIME NOT NULL,
        Nome NVARCHAR(100) NOT NULL,
        Descricao NVARCHAR(MAX) NULL,
        Preco FLOAT NOT NULL,
        QuantidadeDisponivel INT NOT NULL
    )
END