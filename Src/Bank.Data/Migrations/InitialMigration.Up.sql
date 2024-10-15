CREATE TABLE Orders
(
    OrderId   SERIAL PRIMARY KEY,      -- Auto-incremented unique ID for each order
    ClientId  VARCHAR(128)   NOT NULL, -- Client's unique identifier (adjust length as needed)
    Address   VARCHAR(256)   NOT NULL, -- Address of the client or related to the order
    Amount    DECIMAL(15, 2) NOT NULL, -- Amount of money involved, up to 999 trillion with 2 decimal places
    Currency  INT            NOT NULL, -- Currency type (e.g., UAH, USD, EUR)
    ClientIp  VARCHAR(128)   NOT NULL, -- IP address of the client (IPv4 and IPv6 compatible)
    CreatedAt TIMESTAMP DEFAULT NOW()  -- Timestamp for when the order was created
);
CREATE OR REPLACE PROCEDURE InsertOrderProc(
    p_ClientId VARCHAR(128),
    p_Address VARCHAR(256),
    p_Amount DECIMAL(15, 2),
    p_Currency INT,
    p_ClientIp VARCHAR(128)
)
    LANGUAGE plpgsql
AS
$$
BEGIN
    -- Insert data into Orders table
    INSERT INTO Orders (ClientId, Address, Amount, Currency, ClientIp)
    VALUES (p_ClientId, p_Address, p_Amount, p_Currency, p_ClientIp);

    -- Commit the transaction (you can also use ROLLBACK if needed)
    COMMIT;
END;
$$;
