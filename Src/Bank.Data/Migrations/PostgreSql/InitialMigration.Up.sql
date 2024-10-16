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
    p_ClientIp VARCHAR(128),
    p_OrderId OUT INT -- Output parameter
)
    LANGUAGE plpgsql
AS
$$
BEGIN
    -- Insert data into Orders table
    INSERT INTO Orders (ClientId, Address, Amount, Currency, ClientIp)
    VALUES (p_ClientId, p_Address, p_Amount, p_Currency, p_ClientIp)
    RETURNING orderid INTO p_orderid;
    -- Return generated order ID
    -- Commit the transaction (you can also use ROLLBACK if needed)
    COMMIT;
END;
$$;
CREATE OR REPLACE FUNCTION SearchOrdersProc(
    p_OrderId INT DEFAULT NULL,
    p_ClientId VARCHAR(128) DEFAULT NULL,
    p_Address VARCHAR(256) DEFAULT NULL
) RETURNS SETOF Orders
    LANGUAGE plpgsql
AS
$$
BEGIN
    -- Search by OrderId if it's provided
    IF p_OrderId IS NOT NULL THEN
        RETURN QUERY SELECT * FROM Orders WHERE OrderId = p_OrderId;

        -- Otherwise, search by ClientId and Address if both are provided
    ELSIF p_ClientId IS NOT NULL AND p_Address IS NOT NULL THEN
        RETURN QUERY SELECT * FROM Orders WHERE ClientId = p_ClientId AND Address = p_Address;

        -- Raise an exception if neither criteria is provided
    ELSE
        RAISE EXCEPTION 'Either OrderId or (ClientId and Address) must be provided.';
    END IF;
END;
$$;
