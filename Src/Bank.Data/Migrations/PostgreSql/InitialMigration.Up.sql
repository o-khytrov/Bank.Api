CREATE TABLE Orders
(
    OrderId           SERIAL PRIMARY KEY,                   -- Auto-incremented unique ID for each order
    ClientId          VARCHAR(128)   NOT NULL,              -- Client's unique identifier
    DepartmentAddress VARCHAR(256)   NOT NULL,              -- Address of the Department
    Amount            DECIMAL(15, 2) NOT NULL,              -- Amount of money involved
    Currency          INT            NOT NULL,              -- Currency type (e.g., UAH, USD, EUR)
    ClientIp          VARCHAR(128)   NOT NULL,              -- IP address of the client (IPv4 and IPv6 compatible)
    Status            INT            NOT NULL DEFAULT 0,    --Status of the order
    CreatedAt         TIMESTAMP               DEFAULT NOW() -- Timestamp for when the order was created
);
CREATE INDEX idx_orders_client_department
    ON Orders (ClientId, DepartmentAddress);

CREATE OR REPLACE PROCEDURE sp_order_insert(
    p_client_id VARCHAR(128),
    p_department_address VARCHAR(256),
    p_amount DECIMAL(15, 2),
    p_currency INT,
    p_client_ip VARCHAR(128),
    p_order_id OUT INT
)
    LANGUAGE plpgsql
AS
$$
BEGIN
    INSERT INTO Orders (ClientId, DepartmentAddress, Amount, Currency, ClientIp)
    VALUES (p_client_id, p_department_address, p_amount, p_currency, p_client_ip)
    RETURNING orderid INTO p_order_id;
    COMMIT;
END;
$$;
CREATE OR REPLACE FUNCTION fn_orders_search(
    p_order_id INT DEFAULT NULL,
    p_client_id VARCHAR(128) DEFAULT NULL,
    p_department_address VARCHAR(256) DEFAULT NULL
) RETURNS SETOF Orders
    LANGUAGE plpgsql
AS
$$
BEGIN

    IF p_order_id IS NOT NULL THEN
        RETURN QUERY SELECT * FROM Orders WHERE OrderId = p_order_id;

    ELSIF p_client_id IS NOT NULL AND p_department_address IS NOT NULL THEN
        RETURN QUERY SELECT * FROM Orders WHERE ClientId = p_client_id AND DepartmentAddress = p_department_address;

    ELSE
        RAISE EXCEPTION 'Either OrderId or (ClientId and DepartmentAddress) must be provided.';
    END IF;
END;
$$;
