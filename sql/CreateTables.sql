CREATE TABLE Categories (
    category_id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE Products (
    product_id SERIAL PRIMARY KEY,
    category_id INT REFERENCES Categories(category_id),
    name VARCHAR(255) NOT NULL,
    description TEXT,
    price DECIMAL(10, 2) NOT NULL CHECK (price >= 0),
    stock_quantity INT NOT NULL DEFAULT 0,
    is_component BOOLEAN DEFAULT TRUE
);

CREATE TABLE Builds (
    build_id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    total_price DECIMAL(10, 2) DEFAULT 0,
    description TEXT
);

CREATE TABLE Build_Items (
    build_id INT REFERENCES Builds(build_id) ON DELETE CASCADE,
    product_id INT REFERENCES Products(product_id),
    quantity INT DEFAULT 1,
    PRIMARY KEY (build_id, product_id)
);
