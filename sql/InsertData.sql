-- Категорії --
INSERT INTO Categories (name) VALUES 
('Процесори'),
('Відеокарти'),
('Материнські плати'),
('Системи охолодження'),
('Блоки живлення'),
('Оперативна пам''ять'),
('SSD твердотільні накопичувачі'),
('HDD жорсткі диски'),
('Корпуси'),
('Вентилятори'),
('Комп''ютери (системні блоки)')
ON CONFLICT (name) DO NOTHING;

-- Відеокарти --
INSERT INTO Products (name, price, description, category_id) VALUES
('NVIDIA GeForce RTX 4090', 78999.00, '24GB GDDR6X, Flagship performance', (SELECT category_id FROM Categories WHERE name = 'Відеокарти')),
('NVIDIA GeForce RTX 4080 Super', 45500.00, '16GB GDDR6X, High-end 4K gaming', (SELECT category_id FROM Categories WHERE name = 'Відеокарти')),
('NVIDIA GeForce RTX 4070 Ti Super', 36200.00, '16GB GDDR6X, Great for 1440p', (SELECT category_id FROM Categories WHERE name = 'Відеокарти')),
('NVIDIA GeForce RTX 4060 Ti', 18500.00, '8GB GDDR6, Best mid-range', (SELECT category_id FROM Categories WHERE name = 'Відеокарти')),
('NVIDIA GeForce RTX 4060', 13200.00, '8GB GDDR6, Efficient 1080p gaming', (SELECT category_id FROM Categories WHERE name = 'Відеокарти')),
('AMD Radeon RX 7900 XTX', 42000.00, '24GB GDDR6, AMD flagship', (SELECT category_id FROM Categories WHERE name = 'Відеокарти')),
('AMD Radeon RX 7900 XT', 34500.00, '20GB GDDR6, Powerful 4K card', (SELECT category_id FROM Categories WHERE name = 'Відеокарти')),
('AMD Radeon RX 7800 XT', 22800.00, '16GB GDDR6, King of 1440p', (SELECT category_id FROM Categories WHERE name = 'Відеокарти')),
('AMD Radeon RX 7700 XT', 19400.00, '12GB GDDR6, Solid performance', (SELECT category_id FROM Categories WHERE name = 'Відеокарти')),
('AMD Radeon RX 7600', 11500.00, '8GB GDDR6, Budget friendly', (SELECT category_id FROM Categories WHERE name = 'Відеокарти')),
('ASUS ROG Strix RTX 4090 OC', 85000.00, 'Premium cooling and high clocks', (SELECT category_id FROM Categories WHERE name = 'Відеокарти')),
('MSI Suprim X RTX 4080', 48000.00, 'Silent and cold operation', (SELECT category_id FROM Categories WHERE name = 'Відеокарти')),
('Gigabyte AORUS RTX 4070', 29500.00, 'RGB and great performance', (SELECT category_id FROM Categories WHERE name = 'Відеокарти')),
('Zotac Trinity RTX 4070 Ti', 33000.00, 'Triple fan design', (SELECT category_id FROM Categories WHERE name = 'Відеокарти')),
('Palit Dual RTX 4060', 12800.00, 'Compact design for small builds', (SELECT category_id FROM Categories WHERE name = 'Відеокарти')),
('Sapphire Nitro+ RX 7900 XTX', 44000.00, 'Best AMD custom card', (SELECT category_id FROM Categories WHERE name = 'Відеокарти')),
('PowerColor Red Devil RX 7800 XT', 24500.00, 'Extreme cooling performance', (SELECT category_id FROM Categories WHERE name = 'Відеокарти')),
('NVIDIA GeForce RTX 3060', 11000.00, '12GB GDDR6, Old but gold', (SELECT category_id FROM Categories WHERE name = 'Відеокарти')),
('ASUS Dual RTX 3050', 9500.00, 'Entry level ray tracing', (SELECT category_id FROM Categories WHERE name = 'Відеокарти')),
('Intel Arc A770', 14200.00, '16GB VRAM, Intel flagship GPU', (SELECT category_id FROM Categories WHERE name = 'Відеокарти'));

-- Процесори --
INSERT INTO Products (name, price, description, category_id) VALUES
('AMD Ryzen 9 7950X3D', 28500.00, '16 cores, 32 threads, 128MB L3 Cache, 5.7GHz Boost', (SELECT category_id FROM Categories WHERE name = 'Процесори')),
('AMD Ryzen 7 7800X3D', 18200.00, '8 cores, 16 threads, Best gaming CPU with 3D V-Cache', (SELECT category_id FROM Categories WHERE name = 'Процесори')),
('AMD Ryzen 9 9950X', 31000.00, '16 cores, Zen 5 architecture, 5.7GHz, Flagship', (SELECT category_id FROM Categories WHERE name = 'Процесори')),
('AMD Ryzen 5 7600X', 9400.00, '6 cores, 12 threads, Great entry for AM5 platform', (SELECT category_id FROM Categories WHERE name = 'Процесори')),
('AMD Ryzen 7 9700X', 16500.00, '8 cores, Zen 5, Energy efficient high performance', (SELECT category_id FROM Categories WHERE name = 'Процесори')),
('Intel Core i9-14900K', 26800.00, '24 cores, 32 threads, up to 6.0GHz, LGA1700', (SELECT category_id FROM Categories WHERE name = 'Процесори')),
('Intel Core i7-14700K', 18900.00, '20 cores, 28 threads, Excellent for work and gaming', (SELECT category_id FROM Categories WHERE name = 'Процесори')),
('Intel Core i5-14600K', 13500.00, '14 cores, 20 threads, Best mid-range performance', (SELECT category_id FROM Categories WHERE name = 'Процесори')),
('Intel Core i5-13400F', 8200.00, '10 cores, 16 threads, Budget king for gaming', (SELECT category_id FROM Categories WHERE name = 'Процесори')),
('Intel Core i3-14100', 5800.00, '4 cores, 8 threads, Basic office and light gaming', (SELECT category_id FROM Categories WHERE name = 'Процесори')),
('AMD Ryzen 9 7900X', 17800.00, '12 cores, 24 threads, High-end productivity', (SELECT category_id FROM Categories WHERE name = 'Процесори')),
('AMD Ryzen 5 8600G', 9800.00, '6 cores, Integrated Radeon 760M graphics', (SELECT category_id FROM Categories WHERE name = 'Процесори')),
('AMD Ryzen 7 8700G', 13800.00, '8 cores, Powerful Radeon 780M integrated GPU', (SELECT category_id FROM Categories WHERE name = 'Процесори')),
('Intel Core i9-13900KS', 29500.00, 'Special Edition, up to 6GHz out of the box', (SELECT category_id FROM Categories WHERE name = 'Процесори')),
('AMD Ryzen 5 7500F', 7200.00, '6 cores, Budget AM5 option without iGPU', (SELECT category_id FROM Categories WHERE name = 'Процесори')),
('Intel Core i7-13700F', 15800.00, '16 cores, Without integrated graphics', (SELECT category_id FROM Categories WHERE name = 'Процесори')),
('AMD Ryzen 9 5950X', 16000.00, '16 cores, Legacy AM4 flagship, still powerful', (SELECT category_id FROM Categories WHERE name = 'Процесори')),
('AMD Ryzen 7 5800X3D', 12500.00, 'Legendary gaming CPU for AM4 socket', (SELECT category_id FROM Categories WHERE name = 'Процесори')),
('Intel Core i5-12400F', 5400.00, '6 cores, 12 threads, Great value for DDR4/DDR5 builds', (SELECT category_id FROM Categories WHERE name = 'Процесори')),
('AMD Ryzen 5 5600', 4800.00, '6 cores, 12 threads, Ultra budget gaming king', (SELECT category_id FROM Categories WHERE name = 'Процесори'));

-- Материнські плати --
INSERT INTO Products (name, price, description, category_id) VALUES
('ASUS ROG MAXIMUS Z790 HERO', 26500.00, 'LGA1700, Intel Z790, DDR5, Wi-Fi 6E, ATX', (SELECT category_id FROM Categories WHERE name = 'Материнські плати')),
('MSI MAG B760 TOMAHAWK WIFI', 8400.00, 'LGA1700, Intel B760, DDR5, PCIe 5.0, ATX', (SELECT category_id FROM Categories WHERE name = 'Материнські плати')),
('ASUS TUF GAMING B650-PLUS WIFI', 9200.00, 'AM5, AMD B650, DDR5, 3x M.2 slots, ATX', (SELECT category_id FROM Categories WHERE name = 'Материнські плати')),
('Gigabyte X670E AORUS MASTER', 19800.00, 'AM5, AMD X670, DDR5, PCIe 5.0, E-ATX flagship', (SELECT category_id FROM Categories WHERE name = 'Материнські плати')),
('ASRock B450M-HDV R4.0', 2600.00, 'AM4, AMD B450, DDR4, Micro-ATX ultra budget', (SELECT category_id FROM Categories WHERE name = 'Материнські плати')),
('MSI MPG Z790 CARBON WIFI', 17500.00, 'LGA1700, Intel Z790, DDR5, Premium VRM, ATX', (SELECT category_id FROM Categories WHERE name = 'Материнські плати')),
('ASUS PRIME Z790-P', 9800.00, 'LGA1700, Intel Z790, DDR5, Reliable workhorse, ATX', (SELECT category_id FROM Categories WHERE name = 'Материнські плати')),
('Gigabyte B550 AORUS ELITE V2', 5400.00, 'AM4, AMD B550, DDR4, Best value for Ryzen 5000', (SELECT category_id FROM Categories WHERE name = 'Материнські плати')),
('ASRock Z790 Steel Legend WiFi', 11200.00, 'LGA1700, Intel Z790, DDR5, Snow-white design, ATX', (SELECT category_id FROM Categories WHERE name = 'Материнські плати')),
('MSI PRO H610M-E DDR4', 3100.00, 'LGA1700, Intel H610, DDR4, Basic office build, mATX', (SELECT category_id FROM Categories WHERE name = 'Материнські плати'));

-- Системи охолодження --
INSERT INTO Products (name, price, description, category_id) VALUES
('Noctua NH-D15 chromax.black', 4850.00, 'Dual-tower premium air cooler, 2x 140mm fans, Silent', (SELECT category_id FROM Categories WHERE name = 'Системи охолодження')),
('be quiet! Dark Rock Pro 5', 3950.00, 'High-end air cooler, 270W TDP, virtually inaudible', (SELECT category_id FROM Categories WHERE name = 'Системи охолодження')),
('DeepCool AK620 Digital', 3100.00, 'Dual-tower with status display, 260W TDP, Black', (SELECT category_id FROM Categories WHERE name = 'Системи охолодження')),
('Arctic Liquid Freezer III 360 A-RGB', 5200.00, '360mm All-in-One Liquid Cooler, VRM fan, Black', (SELECT category_id FROM Categories WHERE name = 'Системи охолодження')),
('NZXT Kraken Elite 360 RGB', 12400.00, 'Premium AIO, LCD Display on pump, 360mm radiator, White', (SELECT category_id FROM Categories WHERE name = 'Системи охолодження'));

-- Блоки живлення --
INSERT INTO Products (name, price, description, category_id) VALUES
('Corsair RM850x (2021) 850W', 5800.00, '80+ Gold, Fully Modular, Magnetic Levitation Fan', (SELECT category_id FROM Categories WHERE name = 'Блоки живлення')),
('be quiet! Straight Power 12 1000W', 8200.00, '80+ Platinum, ATX 3.0, PCIe 5.0 12VHPWR, Silent', (SELECT category_id FROM Categories WHERE name = 'Блоки живлення')),
('MSI MAG A650BN 650W', 2450.00, '80+ Bronze, 120mm fan, Great budget option', (SELECT category_id FROM Categories WHERE name = 'Блоки живлення')),
('ASUS ROG Thor 1200W Platinum II', 16500.00, '80+ Platinum, OLED Display, Aura Sync RGB', (SELECT category_id FROM Categories WHERE name = 'Блоки живлення')),
('SeaSonic Focus GX-750 750W', 4900.00, '80+ Gold, Fully Modular, Compact 140mm depth', (SELECT category_id FROM Categories WHERE name = 'Блоки живлення'));

-- Оперативна пам'ять --
INSERT INTO Products (name, price, description, category_id) VALUES
('G.Skill Trident Z5 RGB 32GB DDR5-6400', 5200.00, 'CL32-39-39-102, 1.40V, High-performance DDR5', (SELECT category_id FROM Categories WHERE name = 'Оперативна пам''ять')),
('Corsair Vengeance RGB 32GB DDR5-6000', 4800.00, 'CL36, optimized for AMD EXPO and Intel XMP', (SELECT category_id FROM Categories WHERE name = 'Оперативна пам''ять')),
('Kingston FURY Beast 16GB DDR5-5200', 2300.00, 'Single module, Low-profile heat spreader', (SELECT category_id FROM Categories WHERE name = 'Оперативна пам''ять')),
('Team T-Force Delta RGB 32GB DDR5-7200', 6100.00, 'Extreme speed for overclocking enthusiasts', (SELECT category_id FROM Categories WHERE name = 'Оперативна пам''ять')),
('Kingston FURY Renegade 32GB DDR4-3600', 3400.00, 'CL16, Best performance for AM4/LGA1200', (SELECT category_id FROM Categories WHERE name = 'Оперативна пам''ять')),
('Corsair Vengeance LPX 16GB DDR4-3200', 1650.00, '2x8GB kit, Low-profile design, Black', (SELECT category_id FROM Categories WHERE name = 'Оперативна пам''ять')),
('G.Skill Ripjaws V 32GB DDR4-3600', 3100.00, 'Classic design, reliable performance', (SELECT category_id FROM Categories WHERE name = 'Оперативна пам''ять')),
('Patriot Viper Steel 16GB DDR4-4400', 2900.00, 'Extreme speed DDR4 for benchmark chasing', (SELECT category_id FROM Categories WHERE name = 'Оперативна пам''ять')),
('Crucial Pro 32GB DDR5-5600', 3950.00, 'Plug-and-play high speed, no RGB', (SELECT category_id FROM Categories WHERE name = 'Оперативна пам''ять')),
('ADATA XPG Lancer RGB 16GB DDR5-6000', 2550.00, 'Stunning RGB, high-quality ICs', (SELECT category_id FROM Categories WHERE name = 'Оперативна пам''ять'));

-- SSD твердотільні накопичувачі --
INSERT INTO Products (name, price, description, category_id) VALUES
('Samsung 990 Pro 2TB', 7400.00, 'M.2 NVMe PCIe 4.0, up to 7450 MB/s, Best for gaming', (SELECT category_id FROM Categories WHERE name = 'SSD твердотільні накопичувачі')),
('Crucial T705 1TB', 8900.00, 'M.2 NVMe PCIe 5.0, Ultra-fast up to 14500 MB/s', (SELECT category_id FROM Categories WHERE name = 'SSD твердотільні накопичувачі')),
('WD Black SN850X 1TB', 4200.00, 'M.2 NVMe PCIe 4.0, with Heatsink, optimized for PS5/PC', (SELECT category_id FROM Categories WHERE name = 'SSD твердотільні накопичувачі')),
('Kingston KC3000 2TB', 6200.00, 'M.2 NVMe PCIe 4.0, High endurance, 7000 MB/s', (SELECT category_id FROM Categories WHERE name = 'SSD твердотільні накопичувачі')),
('Samsung 980 500GB', 1950.00, 'M.2 NVMe PCIe 3.0, Reliable budget OS drive', (SELECT category_id FROM Categories WHERE name = 'SSD твердотільні накопичувачі')),
('Crucial MX500 1TB', 3100.00, '2.5 inch SATA III, 3D NAND, classic reliable storage', (SELECT category_id FROM Categories WHERE name = 'SSD твердотільні накопичувачі')),
('Samsung 870 EVO 2TB', 6800.00, '2.5 inch SATA III, Best-in-class SATA performance', (SELECT category_id FROM Categories WHERE name = 'SSD твердотільні накопичувачі')),
('ADATA XPG Gammix S70 Blade 1TB', 3800.00, 'M.2 NVMe PCIe 4.0, Slim design for laptops and consoles', (SELECT category_id FROM Categories WHERE name = 'SSD твердотільні накопичувачі')),
('Lexar NM790 4TB', 11500.00, 'M.2 NVMe PCIe 4.0, Massive capacity, 7400 MB/s', (SELECT category_id FROM Categories WHERE name = 'SSD твердотільні накопичувачі')),
('Kingston NV2 1TB', 2450.00, 'M.2 NVMe PCIe 4.0, Entry level NVMe storage', (SELECT category_id FROM Categories WHERE name = 'SSD твердотільні накопичувачі'));

-- HDD жорсткі диски --
INSERT INTO Products (name, price, description, category_id) VALUES
('Western Digital Blue 1TB', 1850.00, '7200RPM, 64MB Cache, Reliable everyday HDD', (SELECT category_id FROM Categories WHERE name = 'HDD жорсткі диски')),
('Seagate BarraCuda 2TB', 2400.00, '7200RPM, 256MB Cache, Fast desktop storage', (SELECT category_id FROM Categories WHERE name = 'HDD жорсткі диски')),
('Western Digital Black 2TB', 4100.00, '7200RPM, 64MB Cache, Performance drive with 5Y warranty', (SELECT category_id FROM Categories WHERE name = 'HDD жорсткі диски')),
('Seagate IronWolf 4TB', 5200.00, '5400RPM, Optimized for NAS and 24/7 operation', (SELECT category_id FROM Categories WHERE name = 'HDD жорсткі диски')),
('Western Digital Red Plus 6TB', 8100.00, '5400RPM, CRM technology, Best for Home Servers', (SELECT category_id FROM Categories WHERE name = 'HDD жорсткі диски')),
('Toshiba X300 4TB', 4800.00, '7200RPM, 256MB Cache, Performance for Pro users', (SELECT category_id FROM Categories WHERE name = 'HDD жорсткі диски')),
('Seagate SkyHawk 2TB', 2600.00, '5900RPM, Optimized for Surveillance/CCTV', (SELECT category_id FROM Categories WHERE name = 'HDD жорсткі диски')),
('Western Digital Purple 4TB', 4950.00, 'AllFrame technology, AI surveillance support', (SELECT category_id FROM Categories WHERE name = 'HDD жорсткі диски')),
('Seagate Exos X18 18TB', 14500.00, '7200RPM, Enterprise-grade helium drive', (SELECT category_id FROM Categories WHERE name = 'HDD жорсткі диски')),
('Toshiba P300 1TB', 1700.00, '7200RPM, High performance budget HDD', (SELECT category_id FROM Categories WHERE name = 'HDD жорсткі диски'));

-- Корпуси --
INSERT INTO Products (name, price, description, category_id) VALUES
('Fractal Design North Charcoal Black', 6800.00, 'Mid-Tower, Walnut wood front, Mesh side panel, ATX', (SELECT category_id FROM Categories WHERE name = 'Корпуси')),
('Lian Li PC-O11 Dynamic EVO', 7950.00, 'Dual-chamber design, Tempered glass, Modular, E-ATX', (SELECT category_id FROM Categories WHERE name = 'Корпуси')),
('NZXT H5 Flow Black', 3850.00, 'Compact Mid-Tower, High-airflow mesh front, 2x 120mm fans', (SELECT category_id FROM Categories WHERE name = 'Корпуси')),
('Corsair 4000D Airflow', 4200.00, 'Mid-Tower ATX, High-airflow front panel, RapidRoute cable mgmt', (SELECT category_id FROM Categories WHERE name = 'Корпуси')),
('be quiet! Shadow Base 800 DX', 6100.00, 'Full-Tower, ARGB lighting, Pure Wings 3 fans, White', (SELECT category_id FROM Categories WHERE name = 'Корпуси'));

-- Вентилятори --
INSERT INTO Products (name, price, description, category_id) VALUES
('Lian Li UNI Fan SL-Infinity 120 RGB', 1450.00, 'Modular 120mm fan, Infinity mirror effect, 2100 RPM', (SELECT category_id FROM Categories WHERE name = 'Вентилятори')),
('Noctua NF-A12x25 PWM', 1350.00, 'Premium quiet 120mm fan, Sterrox LCP material, 2000 RPM', (SELECT category_id FROM Categories WHERE name = 'Вентилятори')),
('be quiet! Silent Wings 4 140mm PWM', 1100.00, 'Virtually inaudible 140mm fan, 1100 RPM, optimized for airflow', (SELECT category_id FROM Categories WHERE name = 'Вентилятори')),
('Arctic P12 PWM PST Black', 350.00, 'Pressure-optimized 120mm fan, Great value for money', (SELECT category_id FROM Categories WHERE name = 'Вентилятори')),
('Corsair iCUE AF120 RGB ELITE', 1200.00, 'High-performance 120mm fan with AirGuide technology', (SELECT category_id FROM Categories WHERE name = 'Вентилятори'));

-- Збірки (системні блоки) --
