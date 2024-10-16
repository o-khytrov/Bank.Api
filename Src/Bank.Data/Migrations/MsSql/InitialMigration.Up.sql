CREATE TABLE Orders
(
    OrderId   INT IDENTITY (1,1) PRIMARY KEY,           -- Auto-incrementing unique ID for each order
    ClientId  VARCHAR(128)   NOT NULL,                  -- Client's unique identifier
    Address   VARCHAR(256)   NOT NULL,                  -- Address of the client or order-related address
    Amount    DECIMAL(15, 2) NOT NULL,                  -- Amount involved, up to 999 trillion with 2 decimal places
    Currency  INT            NOT NULL,                  -- Currency type (e.g., UAH, USD, EUR)
    ClientIp  VARCHAR(128)   NOT NULL,                  -- Client's IP address (IPv4/IPv6)
    CreatedAt DATETIME2      NOT NULL DEFAULT GETDATE() -- Timestamp of when the order was created
);
GO


CREATE PROCEDURE InsertOrderProc @p_ClientId VARCHAR(128),
                                 @p_Address VARCHAR(256),
                                 @p_Amount DECIMAL(15, 2),
                                 @p_Currency INT,
                                 @p_ClientIp VARCHAR(128),
                                 @p_OrderId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;


    INSERT INTO Orders (ClientId, Address, Amount, Currency, ClientIp)
    VALUES (@p_ClientId, @p_Address, @p_Amount, @p_Currency, @p_ClientIp);


    SET @p_OrderId = SCOPE_IDENTITY();
END;
GO


CREATE PROCEDURE SearchOrdersProc @p_OrderId INT = NULL,
                                  @p_ClientId VARCHAR(128) = NULL,
                                  @p_Address VARCHAR(256) = NULL
AS
BEGIN
    SET NOCOUNT ON;


    IF @p_OrderId IS NOT NULL
        BEGIN
            SELECT *
            FROM Orders
            WHERE OrderId = @p_OrderId;
        END

    ELSE
        IF @p_ClientId IS NOT NULL AND @p_Address IS NOT NULL
            BEGIN
                SELECT *
                FROM Orders
                WHERE ClientId = @p_ClientId
                  AND Address = @p_Address;
            END

        ELSE
            BEGIN
                RAISERROR ('Either OrderId or (ClientId and Address) must be provided.', 16, 1);
            END
END;
GO
