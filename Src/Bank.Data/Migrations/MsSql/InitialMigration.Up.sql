CREATE TABLE Orders
(
    OrderId           INT IDENTITY (1,1) PRIMARY KEY,           -- Auto-incrementing unique ID for each order
    ClientId          VARCHAR(128)   NOT NULL,                  -- Client's unique identifier
    DepartmentAddress VARCHAR(256)   NOT NULL,                  -- DepartmentAddress of the client or order-related departmentAddress
    Amount            DECIMAL(15, 2) NOT NULL,                  -- Amount involved
    Currency          INT            NOT NULL,                  -- Currency type (e.g., UAH, USD, EUR)
    ClientIp          VARCHAR(128)   NOT NULL,                  -- Client's IP departmentAddress (IPv4/IPv6)
    CreatedAt         DATETIME2      NOT NULL DEFAULT GETDATE() -- Timestamp of when the order was created
);
GO
CREATE NONCLUSTERED INDEX idx_orders_client_department
    ON Orders (ClientId, DepartmentAddress);
GO

CREATE PROCEDURE sp_order_insert @clientId VARCHAR(128),
                                 @departmentAddress VARCHAR(256),
                                 @amount DECIMAL(15, 2),
                                 @currency INT,
                                 @clientIp VARCHAR(128),
                                 @orderId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;


    INSERT INTO Orders (ClientId, DepartmentAddress, Amount, Currency, ClientIp)
    VALUES (@clientId, @departmentAddress, @amount, @currency, @clientIp);


    SET @orderId = SCOPE_IDENTITY();
END;
GO


CREATE PROCEDURE sp_orders_search @orderId INT = NULL,
                                  @clientId VARCHAR(128) = NULL,
                                  @departmentAddress VARCHAR(256) = NULL
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
        IF @clientId IS NOT NULL AND @departmentAddress IS NOT NULL
            BEGIN
                SELECT *
                FROM Orders
                WHERE ClientId = @clientId
                  AND DepartmentAddress = @departmentAddress;
            END

        ELSE
            BEGIN
                RAISERROR ('Either OrderId or (ClientId and DepartmentAddress) must be provided.', 16, 1);
            END
END;
GO
