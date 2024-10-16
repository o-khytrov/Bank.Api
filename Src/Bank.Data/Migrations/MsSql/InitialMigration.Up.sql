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


CREATE PROCEDURE sp_order_insert @clientId VARCHAR(128),
                                 @address VARCHAR(256),
                                 @amount DECIMAL(15, 2),
                                 @currency INT,
                                 @clientIp VARCHAR(128),
                                 @orderId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;


    INSERT INTO Orders (ClientId, Address, Amount, Currency, ClientIp)
    VALUES (@clientId, @address, @amount, @currency, @clientIp);


    SET @orderId = SCOPE_IDENTITY();
END;
GO


CREATE PROCEDURE sp_orders_search @orderId INT = NULL,
                                  @clientId VARCHAR(128) = NULL,
                                  @address VARCHAR(256) = NULL
AS
BEGIN
    SET NOCOUNT ON;


    IF @orderId IS NOT NULL
        BEGIN
            SELECT *
            FROM Orders
            WHERE OrderId = @orderId;
        END

    ELSE
        IF @clientId IS NOT NULL AND @address IS NOT NULL
            BEGIN
                SELECT *
                FROM Orders
                WHERE ClientId = @clientId
                  AND Address = @address;
            END

        ELSE
            BEGIN
                RAISERROR ('Either OrderId or (ClientId and Address) must be provided.', 16, 1);
            END
END;
GO
