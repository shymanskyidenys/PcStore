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
    stock_quantity INT NOT NULL DEFAULT 0
);

CREATE TABLE Attributes (
    attribute_id SERIAL PRIMARY KEY,
    name VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE AttributeValues (
    value_id SERIAL PRIMARY KEY,
    attribute_id INT REFERENCES Attributes(attribute_id),
    value VARCHAR(100) NOT NULL,
    UNIQUE(attribute_id, value)
);

CREATE TABLE Product_Attributes (
    product_id INT REFERENCES Products(product_id) ON DELETE CASCADE,
    value_id INT REFERENCES AttributeValues(value_id) ON DELETE CASCADE,
    PRIMARY KEY (product_id, value_id)
);

CREATE TABLE Category_Attributes (
    category_id INT REFERENCES Categories(category_id),
    attribute_id INT REFERENCES Attributes(attribute_id),
    PRIMARY KEY (category_id, attribute_id)
)

CREATE TABLE Product_Components (
    parent_product_id INT REFERENCES Products(product_id) ON DELETE CASCADE,
    child_product_id INT REFERENCES Products(product_id),
    quantity INT DEFAULT 1 CHECK (quantity > 0),
    PRIMARY KEY (parent_product_id, child_product_id)
);
