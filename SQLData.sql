-- Seed data for SQLTable.sql. Run once after SQLTable.sql on an empty GreenShop database.
-- Product image filenames and size rows below represent the final product data.
USE [GreenShop]
GO
INSERT [dbo].[Role] ([Id], [Name], [Status]) VALUES (N'f2031a63-203c-41fa-92d9-140e70ce529b', N'Guest', 0)
GO
INSERT [dbo].[Role] ([Id], [Name], [Status]) VALUES (N'f0634a25-d7aa-4b28-ab22-9bcc82756584', N'User', 1)
GO
INSERT [dbo].[Role] ([Id], [Name], [Status]) VALUES (N'dfedbf03-6f5a-4d58-ab6a-b3c4b4a73cc8', N'Admin', 1)
GO
INSERT [dbo].[User] ([Id], [Username], [Password], [Email], [DOB], [Create_at], [Token], [RoleId], [Status]) VALUES (N'd0ed4098-df55-4929-a37e-0d96602ae379', N'charlie_brown', N'$2a$11$tUra/L8Tjk3GkECoFDo9O.ajG1ctiqAEVHwQuGO0J/QGItiz4ICIG', N'charlie.brown@example.com', N'1990-01-01', CAST(N'2024-08-06T17:04:44.1266667' AS DateTime2), NULL, N'dfedbf03-6f5a-4d58-ab6a-b3c4b4a73cc8', 1)
GO
INSERT [dbo].[User] ([Id], [Username], [Password], [Email], [DOB], [Create_at], [Token], [RoleId], [Status]) VALUES (N'520d9c4d-8fd9-4a22-abf4-8748479b0ee1', N'alice_smith', N'$2a$11$tUra/L8Tjk3GkECoFDo9O.ajG1ctiqAEVHwQuGO0J/QGItiz4ICIG', N'alice.smith@example.com', N'1990-01-01', CAST(N'2024-08-06T17:04:44.1266667' AS DateTime2), NULL, N'f0634a25-d7aa-4b28-ab22-9bcc82756584', 1)
GO
INSERT [dbo].[User] ([Id], [Username], [Password], [Email], [DOB], [Create_at], [Token], [RoleId], [Status]) VALUES (N'f7bd7f60-e5cf-472f-8ef0-a33d02414fc1', N'bob_jones', N'$2a$11$tUra/L8Tjk3GkECoFDo9O.ajG1ctiqAEVHwQuGO0J/QGItiz4ICIG', N'bob.jones@example.com', N'1990-01-01', CAST(N'2024-08-06T17:04:44.1266667' AS DateTime2), NULL, N'f2031a63-203c-41fa-92d9-140e70ce529b', 1)
GO
INSERT [dbo].[User] ([Id], [Username], [Password], [Email], [DOB], [Create_at], [Token], [RoleId], [Status]) VALUES (N'2a22e62c-2e5a-43a9-88d4-c439804f731b', N'jane_doe', N'$2a$11$tUra/L8Tjk3GkECoFDo9O.ajG1ctiqAEVHwQuGO0J/QGItiz4ICIG', N'jane.doe@example.com', N'1990-01-01', CAST(N'2024-08-06T17:04:44.1266667' AS DateTime2), NULL, N'f0634a25-d7aa-4b28-ab22-9bcc82756584', 1)
GO
INSERT [dbo].[User] ([Id], [Username], [Password], [Email], [DOB], [Create_at], [Token], [RoleId], [Status]) VALUES (N'cdbd2a31-7754-4e2d-97b9-f0d3b30cff9f', N'john_doe', N'$2a$11$tUra/L8Tjk3GkECoFDo9O.ajG1ctiqAEVHwQuGO0J/QGItiz4ICIG', N'1s2p3s0410@gmail.com', N'1990-01-01', CAST(N'2024-08-06T17:04:44.1266667' AS DateTime2), NULL, N'dfedbf03-6f5a-4d58-ab6a-b3c4b4a73cc8', 1)
GO
-- Categories must exist before products because Product.CategoryId is a foreign key.
INSERT [dbo].[Category] ([Id], [Name], [Status]) VALUES (N'd8f15903-efa0-423a-a9f8-856909c4e27a', N'Đồ dùng nhà bếp', 1);
INSERT [dbo].[Category] ([Id], [Name], [Status]) VALUES (N'd8f15903-efa0-423a-a9f8-856909c4e27b', N'Đồ dùng nhà tắm', 1);
GO
-- Product.Image values are local filenames under wwwroot/img/product.
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description])
VALUES
('FADE0338-BB3F-469D-8B5E-968CB71BC9A8', N'Miếng rửa chén tròn', 25000, 80, N'fade0338-bb3f-469d-8b5e-968cb71bc9a8.jpg', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27a', 1,
N'Dùng để cọ rửa chén, xoong nồi...hiệu quả mà không làm trầy xước bề mặt..');
GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description])
VALUES
('5A71FE69-CBAF-4E56-96F2-468A0A882EC7', N'Cây cọ rửa đa năng', 60000, 80, N'5a71fe69-cbaf-4e56-96f2-468a0a882ec7.jpg', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27a', 1,
N'Dùng để cọ rửa chén, xoong nồi...hiệu quả mà không làm trầy xước bề mặt');
GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description])
VALUES
('C91D5631-4640-45EA-9D1B-5F089D0B4C87', N'Miếng rửa chén lớn hình chữ nhật', 25000, 80, N'c91d5631-4640-45ea-9d1b-5f089d0b4c87.webp', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27a', 1,
N'Làm sạch bụi bẩn, dầu mỡ hiệu quả mà không làm trầy xước bề mặt vật dụng');
GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description])
VALUES
('959C6113-CB61-4822-8AAD-F18F8AF31EE8', N'Miếng rửa chén nhỏ hình chữ nhật', 20000, 80, N'959c6113-cb61-4822-8aad-f18f8af31ee8.webp', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27a', 1,
N'Công dụng: Làm sạch bụi bẩn, dầu mỡ hiệu quả mà không làm trầy xước bề mặt vật dụng');
GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description])
VALUES
('4A75361B-B4C1-411E-AE89-F7EE95294B2C', N'Miếng rửa chén lớn hình giọt nước', 25000, 80, N'4a75361b-b4c1-411e-ae89-f7ee95294b2c.jpg', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27a', 1,
N'Công dụng: Làm sạch bụi bẩn, dầu mỡ hiệu quả mà không làm trầy xước bề mặt vật dụng.');
GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description])
VALUES
('DA77FE7D-2C8B-4CB1-BCB9-5505E4BF4269', N'Miếng rửa chén nhỏ hình giọt nước', 20000, 80, N'da77fe7d-2c8b-4cb1-bcb9-5505e4bf4269.jpg', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27a', 1,
N'Công dụng: Làm sạch bụi bẩn, dầu mỡ hiệu quả mà không làm trầy xước bề mặt vật dụng.');
GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description])
VALUES
('52D2735F-93E1-41EE-8CD1-B6F1C5E4D0B6', N'Miếng thay cây rửa ly 2 cánh', 20000, 80, N'52d2735f-93e1-41ee-8cd1-b6f1c5e4d0b6.jpg', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27a', 1,
N' Dùng để thay vào cây rửa ly (2 cánh) khi miếng xơ mướp cũ bị mòn.');
GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description])
VALUES
('30E44D97-DF3E-403D-9277-C60B178320D8', N'Cây rửa ly xơ mướp 2 cánh ', 25000, 80, N'30e44d97-df3e-403d-9277-c60b178320d8.jpg', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27a', 1,
N'Kích thước: 6,5*28cm - Thành phần: gỗ thông, xơ mướp. - Thiết kế có thể thay thế khi miếng xơ mướp đã mòn.');
GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description])
VALUES
('65C41CE3-D06B-4045-A0C3-D64372B45F6B', N'Miếng rửa chén cỡ trung bình hình giọt nước', 26500, 80, N'65c41ce3-d06b-4045-a0c3-d64372b45f6b.jpg', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27a', 1,
N'Dùng để cọ rửa chén, xoong nồi...hiệu quả mà không làm trầy xước bề mặt..');
GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description])
VALUES
('D3E4A3B2-66F7-4089-9ADF-A05D790F554A', N'Miếng rửa chén trung bình hình chữ nhật', 26500, 80, N'd3e4a3b2-66f7-4089-9adf-a05d790f554a.webp', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27a', 1,
N' Làm sạch và cải thiện tình trạng nứt gót chân nhờ kết cấu sợi đan xen tự nhiên của Xơ Mướp');
GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description])
VALUES
('6F74BBFC-E630-4576-8565-0492E6997E86', N'Cây chà gót ', 60000, 80, N'6f74bbfc-e630-4576-8565-0492e6997e86.jpg', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27b', 1,
N'Dùng để cọ rửa chén, xoong nồi...hiệu quả mà không làm trầy xước bề mặt..');
GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description])
VALUES
('922C78CD-529C-4ECD-961A-3411AF81CAD2', N'Bông tắm giọt nước', 70000, 80, N'922c78cd-529c-4ecd-961a-3411af81cad2.jpg', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27b', 1,
N'Từ xa xưa, ông bà ta đã biết đến những công dụng tuyệt vời của xơ mướp trong việc làm sạch, dưỡng da, làm chậm quá trình lão hóa. Và những kinh nghiệm quý báu đó đã truyền cho đến tận ngày nay.
Với kết cấu sợi đan xen và những chất làm sạch tự nhiên ( Glycerine giữ ẩm, Vitamin E tạo ra tính đàn hồi và nhiều chất khác), khi thẩm thấu vào da, xơ mướp giúp cải tạo làn da, tăng tính đàn hồi và giảm khả năng hình thành vết nhăn, loại bỏ tế bào chết, thúc đẩy tái tạo collagen, giúp cho làn da sạch mịn đầy sức sống.
Là món quà tặng vô giá từ tự nhiên, xơ mướp an toàn cho mọi loại da, không gây kích ứng, có thể sử dụng hàng ngày hoặc 01 đến 03 lần/ tuần tùy vào mức độ sần sùi và tính nhạy cảm của làn da.');
GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description])
VALUES
('CA1694C7-03A9-42D6-989D-A4B0867F06D7', N'Dây chà lưng', 90000, 80, N'ca1694c7-03a9-42d6-989d-a4b0867f06d7.jpg', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27b', 1,
N'Từ xa xưa, ông bà ta đã biết đến những công dụng tuyệt vời của xơ mướp trong việc làm sạch, dưỡng da, làm chậm quá trình lão hóa. Và những kinh nghiệm quý báu đó đã truyền cho đến tận ngày nay.
Với kết cấu sợi đan xen và những chất làm sạch tự nhiên ( Glycerine giữ ẩm, Vitamin E tạo ra tính đàn hồi và nhiều chất khác), khi thẩm thấu vào da, xơ mướp giúp cải tạo làn da, tăng tính đàn hồi và giảm khả năng hình thành vết nhăn, loại bỏ tế bào chết, thúc đẩy tái tạo collagen, giúp cho làn da sạch mịn đầy sức sống.
Là món quà tặng vô giá từ tự nhiên, xơ mướp an toàn cho mọi loại da, không gây kích ứng, có thể sử dụng hàng ngày hoặc 01 đến 03 lần/ tuần tùy vào mức độ sần sùi và tính nhạy cảm của làn da.');
GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description])
VALUES
('E46CD738-3E89-4D81-9E74-61243E2C793E', N'Bao tay tắm', 70000, 80, N'e46cd738-3e89-4d81-9e74-61243e2c793e.jpg', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27b', 1,
N'Từ xa xưa, ông bà ta đã biết đến những công dụng tuyệt vời của xơ mướp trong việc làm sạch, dưỡng da, làm chậm quá trình lão hóa. Và những kinh nghiệm quý báu đó đã truyền cho đến tận ngày nay.
Với kết cấu sợi đan xen và những chất làm sạch tự nhiên ( Glycerine giữ ẩm, Vitamin E tạo ra tính đàn hồi và nhiều chất khác), khi thẩm thấu vào da, xơ mướp giúp cải tạo làn da, tăng tính đàn hồi và giảm khả năng hình thành vết nhăn, loại bỏ tế bào chết, thúc đẩy tái tạo collagen, giúp cho làn da sạch mịn đầy sức sống.
Là món quà tặng vô giá từ tự nhiên, xơ mướp an toàn cho mọi loại da, không gây kích ứng, có thể sử dụng hàng ngày hoặc 01 đến 03 lần/ tuần tùy vào mức độ sần sùi và tính nhạy cảm của làn da.
*Cách dùng: thấm ướt trước khi sử dụng, dùng kèm với sữa tắm. Treo nơi thông thoáng sau mỗi lần sử dụng. Nên thay sau khoảng 02 tháng để tránh sinh khuẩn.');
GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description])
VALUES
('EC3B893C-636A-4533-AA9E-F66878D3A5D1', N'Cây chà lưng ', 115000, 80, N'ec3b893c-636a-4533-aa9e-f66878d3a5d1.jpg', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27b', 1,
N'Từ xa xưa, ông bà ta đã biết đến những công dụng tuyệt vời của xơ mướp trong việc làm sạch, dưỡng da, làm chậm quá trình lão hóa. Và những kinh nghiệm quý báu đó đã truyền cho đến tận ngày nay.
Với kết cấu sợi đan xen và những chất làm sạch tự nhiên ( Glycerine giữ ẩm, Vitamin E tạo ra tính đàn hồi và nhiều chất khác), khi thẩm thấu vào da, xơ mướp giúp cải tạo làn da, tăng tính đàn hồi và giảm khả năng hình thành vết nhăn, loại bỏ tế bào chết, thúc đẩy tái tạo collagen, giúp cho làn da sạch mịn đầy sức sống.
Là món quà tặng vô giá từ tự nhiên, xơ mướp an toàn cho mọi loại da, không gây kích ứng, có thể sử dụng hàng ngày hoặc 01 đến 03 lần/ tuần tùy vào mức độ sần sùi và tính nhạy cảm của làn da.');
GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description])
VALUES
('C561E44A-F9A2-4D62-B635-10A463C933F7', N'Miếng rửa mặt', 30000, 80, N'c561e44a-f9a2-4d62-b635-10a463c933f7.jpg', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27b', 1,
N'Từ xa xưa, ông bà ta đã biết đến những công dụng tuyệt vời của xơ mướp trong việc làm sạch, dưỡng da, làm chậm quá trình lão hóa. Và những kinh nghiệm quý báu đó đã truyền cho đến tận ngày nay.
Với kết cấu sợi đan xen và những chất làm sạch tự nhiên ( Glycerine giữ ẩm, Vitamin E tạo ra tính đàn hồi và nhiều chất khác), khi thẩm thấu vào da, xơ mướp giúp cải tạo làn da, tăng tính đàn hồi và giảm khả năng hình thành vết nhăn, loại bỏ tế bào chết, thúc đẩy tái tạo collagen, giúp cho làn da sạch mịn đầy sức sống.
Là món quà tặng vô giá từ tự nhiên, xơ mướp an toàn cho mọi loại da, không gây kích ứng, dùng từ 01 đến 03 lần/ tuần tùy vào mức độ sần sùi và tính nhạy cảm của làn da.
Cách dùng: thấm ướt trước khi sử dụng, dùng kèm với sữa rửa mặt. Treo nơi thông thoáng sau mỗi lần sử dụng. Nên thay sau khoảng 02 tháng để tránh sinh khuẩn.');
GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description])
VALUES
('379B5902-A901-4190-A4AA-E46C101B007B', N'Miếng thay cây chà lưng ', 18000, 80, N'379b5902-a901-4190-a4aa-e46c101b007b.jpg', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27b', 1,
N'Dùng để thay vào cây chà lưng khi miếng xơ mướp cũ bị mòn.');
GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description])
VALUES
('CC81723F-38F2-4D5A-81F0-AFDCB368C135', N'Miếng thay cây chà gót xơ mướp', 8000, 80, N'cc81723f-38f2-4d5a-81f0-afdcb368c135.jpg', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27b', 1,
N'Dùng để thay vào cây chà gót khi miếng xơ mướp cũ bị mòn.');
GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description])
VALUES
('EB7A902D-3229-48AB-ADAB-C8C4FE5EFE4E', N'Xơ mướp nguyên trái size M', 30000, 80, N'eb7a902d-3229-48ab-adab-c8c4fe5efe4e.webp', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27b', 1,
N'Từ xa xưa, ông bà ta đã biết đến những công dụng tuyệt vời của xơ mướp trong việc làm sạch, dưỡng da, làm chậm quá trình lão hóa. Và những kinh nghiệm quý báu đó đã truyền cho đến tận ngày nay.
Với kết cấu sợi đan xen và những chất làm sạch tự nhiên ( Glycerine giữ ẩm, Vitamin E tạo ra tính đàn hồi và nhiều chất khác), khi thẩm thấu vào da, xơ mướp giúp cải tạo làn da, tăng tính đàn hồi và giảm khả năng hình thành vết nhăn, loại bỏ tế bào chết, thúc đẩy tái tạo collagen, giúp cho làn da sạch mịn đầy sức sống.
Là món quà tặng vô giá từ tự nhiên, xơ mướp an toàn cho mọi loại da, không gây kích ứng, có thể sử dụng hàng ngày hoặc 01 đến 03 lần/ tuần tùy vào mức độ sần sùi và tính nhạy cảm của làn da.');
GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description])
VALUES
('AC33AFC2-9997-42D9-8E17-6DB242AE2690', N'Xơ mướp nguyên trái size S ', 12000, 80, N'ac33afc2-9997-42d9-8e17-6db242ae2690.webp', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27b', 1,
N'Từ xa xưa, ông bà ta đã biết đến những công dụng tuyệt vời của xơ mướp trong việc làm sạch, dưỡng da, làm chậm quá trình lão hóa. Và những kinh nghiệm quý báu đó đã truyền cho đến tận ngày nay.
Với kết cấu sợi đan xen và những chất làm sạch tự nhiên ( Glycerine giữ ẩm, Vitamin E tạo ra tính đàn hồi và nhiều chất khác), khi thẩm thấu vào da, xơ mướp giúp cải tạo làn da, tăng tính đàn hồi và giảm khả năng hình thành vết nhăn, loại bỏ tế bào chết, thúc đẩy tái tạo collagen, giúp cho làn da sạch mịn đầy sức sống.
Là món quà tặng vô giá từ tự nhiên, xơ mướp an toàn cho mọi loại da, không gây kích ứng, có thể sử dụng hàng ngày hoặc 01 đến 03 lần/ tuần tùy vào mức độ sần sùi và tính nhạy cảm của làn da.
Cách dùng: thấm ướt trước khi sử dụng, dùng kèm với sữa tắm. Treo nơi thông thoáng sau mỗi lần sử dụng. Nên thay sau khoảng 02 tháng để tránh sinh khuẩn');
GO
-- Final sizes: three per product, with stock split 20/40/20 (80 total). These rows replace the removed separate update script.
INSERT [dbo].[ProductSize] ([Id], [ProductId], [SizeName], [Price], [Quantity])
VALUES
    (NEWID(), 'FADE0338-BB3F-469D-8B5E-968CB71BC9A8', N'6 cm', 25000, 20),
    (NEWID(), 'FADE0338-BB3F-469D-8B5E-968CB71BC9A8', N'8 cm', 25000, 40),
    (NEWID(), 'FADE0338-BB3F-469D-8B5E-968CB71BC9A8', N'10 cm', 25000, 20),
    (NEWID(), '5A71FE69-CBAF-4E56-96F2-468A0A882EC7', N'20 cm', 60000, 20),
    (NEWID(), '5A71FE69-CBAF-4E56-96F2-468A0A882EC7', N'25 cm', 60000, 40),
    (NEWID(), '5A71FE69-CBAF-4E56-96F2-468A0A882EC7', N'30 cm', 60000, 20),
    (NEWID(), 'C91D5631-4640-45EA-9D1B-5F089D0B4C87', N'11 x 8 cm', 25000, 20),
    (NEWID(), 'C91D5631-4640-45EA-9D1B-5F089D0B4C87', N'13 x 10 cm', 25000, 40),
    (NEWID(), 'C91D5631-4640-45EA-9D1B-5F089D0B4C87', N'15 x 12 cm', 25000, 20),
    (NEWID(), '959C6113-CB61-4822-8AAD-F18F8AF31EE8', N'8 x 6 cm', 20000, 20),
    (NEWID(), '959C6113-CB61-4822-8AAD-F18F8AF31EE8', N'10 x 8 cm', 20000, 40),
    (NEWID(), '959C6113-CB61-4822-8AAD-F18F8AF31EE8', N'12 x 10 cm', 20000, 20),
    (NEWID(), '4A75361B-B4C1-411E-AE89-F7EE95294B2C', N'12.5 x 8 cm', 25000, 20),
    (NEWID(), '4A75361B-B4C1-411E-AE89-F7EE95294B2C', N'14 x 9 cm', 25000, 40),
    (NEWID(), '4A75361B-B4C1-411E-AE89-F7EE95294B2C', N'16 x 10.5 cm', 25000, 20),
    (NEWID(), 'DA77FE7D-2C8B-4CB1-BCB9-5505E4BF4269', N'8 x 12.5 cm', 20000, 20),
    (NEWID(), 'DA77FE7D-2C8B-4CB1-BCB9-5505E4BF4269', N'9 x 13.5 cm', 20000, 40),
    (NEWID(), 'DA77FE7D-2C8B-4CB1-BCB9-5505E4BF4269', N'10.5 x 16 cm', 20000, 20),
    (NEWID(), '52D2735F-93E1-41EE-8CD1-B6F1C5E4D0B6', N'6 cm', 20000, 20),
    (NEWID(), '52D2735F-93E1-41EE-8CD1-B6F1C5E4D0B6', N'8 cm', 20000, 40),
    (NEWID(), '52D2735F-93E1-41EE-8CD1-B6F1C5E4D0B6', N'10 cm', 20000, 20),
    (NEWID(), '30E44D97-DF3E-403D-9277-C60B178320D8', N'22 cm', 25000, 20),
    (NEWID(), '30E44D97-DF3E-403D-9277-C60B178320D8', N'25 cm', 25000, 40),
    (NEWID(), '30E44D97-DF3E-403D-9277-C60B178320D8', N'28 cm', 25000, 20),
    (NEWID(), '65C41CE3-D06B-4045-A0C3-D64372B45F6B', N'12.5 x 8 cm', 26500, 20),
    (NEWID(), '65C41CE3-D06B-4045-A0C3-D64372B45F6B', N'14 x 9 cm', 26500, 40),
    (NEWID(), '65C41CE3-D06B-4045-A0C3-D64372B45F6B', N'16 x 10.5 cm', 26500, 20),
    (NEWID(), 'D3E4A3B2-66F7-4089-9ADF-A05D790F554A', N'8 x 11 cm', 26500, 20),
    (NEWID(), 'D3E4A3B2-66F7-4089-9ADF-A05D790F554A', N'10 x 13 cm', 26500, 40),
    (NEWID(), 'D3E4A3B2-66F7-4089-9ADF-A05D790F554A', N'12 x 15 cm', 26500, 20),
    (NEWID(), '6F74BBFC-E630-4576-8565-0492E6997E86', N'18 cm', 60000, 20),
    (NEWID(), '6F74BBFC-E630-4576-8565-0492E6997E86', N'22 cm', 60000, 40),
    (NEWID(), '6F74BBFC-E630-4576-8565-0492E6997E86', N'26 cm', 60000, 20),
    (NEWID(), '922C78CD-529C-4ECD-961A-3411AF81CAD2', N'16 x 10 cm', 70000, 20),
    (NEWID(), '922C78CD-529C-4ECD-961A-3411AF81CAD2', N'18.5 x 12 cm', 70000, 40),
    (NEWID(), '922C78CD-529C-4ECD-961A-3411AF81CAD2', N'20 x 13 cm', 70000, 20),
    (NEWID(), 'CA1694C7-03A9-42D6-989D-A4B0867F06D7', N'60 cm', 90000, 20),
    (NEWID(), 'CA1694C7-03A9-42D6-989D-A4B0867F06D7', N'80 cm', 90000, 40),
    (NEWID(), 'CA1694C7-03A9-42D6-989D-A4B0867F06D7', N'100 cm', 90000, 20),
    (NEWID(), 'E46CD738-3E89-4D81-9E74-61243E2C793E', N'13 x 12 cm', 70000, 20),
    (NEWID(), 'E46CD738-3E89-4D81-9E74-61243E2C793E', N'15 x 14 cm', 70000, 40),
    (NEWID(), 'E46CD738-3E89-4D81-9E74-61243E2C793E', N'17 x 16 cm', 70000, 20),
    (NEWID(), 'EC3B893C-636A-4533-AA9E-F66878D3A5D1', N'30 cm', 115000, 20),
    (NEWID(), 'EC3B893C-636A-4533-AA9E-F66878D3A5D1', N'35 cm', 115000, 40),
    (NEWID(), 'EC3B893C-636A-4533-AA9E-F66878D3A5D1', N'40 cm', 115000, 20),
    (NEWID(), 'C561E44A-F9A2-4D62-B635-10A463C933F7', N'6 x 4 cm', 30000, 20),
    (NEWID(), 'C561E44A-F9A2-4D62-B635-10A463C933F7', N'8 x 5 cm', 30000, 40),
    (NEWID(), 'C561E44A-F9A2-4D62-B635-10A463C933F7', N'10 x 6.5 cm', 30000, 20),
    (NEWID(), '379B5902-A901-4190-A4AA-E46C101B007B', N'8 x 10 cm', 18000, 20),
    (NEWID(), '379B5902-A901-4190-A4AA-E46C101B007B', N'10 x 12 cm', 18000, 40),
    (NEWID(), '379B5902-A901-4190-A4AA-E46C101B007B', N'12 x 14 cm', 18000, 20),
    (NEWID(), 'CC81723F-38F2-4D5A-81F0-AFDCB368C135', N'5 x 3 cm', 8000, 20),
    (NEWID(), 'CC81723F-38F2-4D5A-81F0-AFDCB368C135', N'6 x 4 cm', 8000, 40),
    (NEWID(), 'CC81723F-38F2-4D5A-81F0-AFDCB368C135', N'7 x 5 cm', 8000, 20),
    (NEWID(), 'EB7A902D-3229-48AB-ADAB-C8C4FE5EFE4E', N'12 cm', 30000, 20),
    (NEWID(), 'EB7A902D-3229-48AB-ADAB-C8C4FE5EFE4E', N'15 cm', 30000, 40),
    (NEWID(), 'EB7A902D-3229-48AB-ADAB-C8C4FE5EFE4E', N'18 cm', 30000, 20),
    (NEWID(), 'AC33AFC2-9997-42D9-8E17-6DB242AE2690', N'8 cm', 12000, 20),
    (NEWID(), 'AC33AFC2-9997-42D9-8E17-6DB242AE2690', N'10 cm', 12000, 40),
    (NEWID(), 'AC33AFC2-9997-42D9-8E17-6DB242AE2690', N'12 cm', 12000, 20);
GO
-- Cart.ProductSizeId is nullable; existing sample rows have no selected size.
INSERT [dbo].[Cart] ([Id], [UserId], [ProductId], [Quantity], [ProductSizeId]) VALUES (N'4F6A84FA-C638-4D7D-B5A1-1D8C55044B28',N'D0ED4098-DF55-4929-A37E-0D96602AE379',N'AC33AFC2-9997-42D9-8E17-6DB242AE2690',2, NULL);
INSERT [dbo].[Cart] ([Id], [UserId], [ProductId], [Quantity], [ProductSizeId]) VALUES (N'F9581755-B951-4448-8AEB-343A56C3D881',N'D0ED4098-DF55-4929-A37E-0D96602AE379',N'EB7A902D-3229-48AB-ADAB-C8C4FE5EFE4E',3, NULL);
INSERT [dbo].[Cart] ([Id], [UserId], [ProductId], [Quantity], [ProductSizeId]) VALUES (N'5E47216B-CFBC-455A-A788-5B5E22EE7BAE',N'D0ED4098-DF55-4929-A37E-0D96602AE379',N'CC81723F-38F2-4D5A-81F0-AFDCB368C135',2, NULL);
INSERT [dbo].[Cart] ([Id], [UserId], [ProductId], [Quantity], [ProductSizeId]) VALUES (N'50A6B77A-E359-4D23-A4BB-DA8537CBA3B3',N'D0ED4098-DF55-4929-A37E-0D96602AE379',N'E46CD738-3E89-4D81-9E74-61243E2C793E',1, NULL);
GO
INSERT [dbo].[ShipmentDetails] ([Id], [Address], [PhoneNumber], [receiver], [UserId], [Status]) VALUES (N'16ac5885-70d8-4491-adcd-4493bd5afe2c', N'Hanoi City', N'1234567890', N'John Doe', N'd0ed4098-df55-4929-a37e-0d96602ae379', 1)
INSERT [dbo].[ShipmentDetails] ([Id], [Address], [PhoneNumber], [receiver], [UserId], [Status]) VALUES (N'A2C1D704-3524-4866-90B6-0C1A48E17092', N'Ho Chi Minh City', N'1234217890', N'Suzy', N'd0ed4098-df55-4929-a37e-0d96602ae379', 1)
INSERT [dbo].[ShipmentDetails] ([Id], [Address], [PhoneNumber], [receiver], [UserId], [Status]) VALUES (N'1A24834B-527C-49D2-AF61-A960FCE8D1DB', N'Da Nang City', N'1234237890', N'Michael Nguyen', N'd0ed4098-df55-4929-a37e-0d96602ae379', 1)
GO
INSERT [dbo].[Order] ([Id], [UserId], [ShipmentDetailsId], [Create_at], [TotalPrice], [Note], [Status]) VALUES (N'd6c97cc3-9db5-468a-8dfc-0c473d562f10', N'd0ed4098-df55-4929-a37e-0d96602ae379', N'16ac5885-70d8-4491-adcd-4493bd5afe2c', CAST(N'2023-08-06T00:00:00.0000000' AS DateTime2), 100, NULL, 1)
GO
INSERT [dbo].[Order] ([Id], [UserId], [ShipmentDetailsId], [Create_at], [TotalPrice], [Note], [Status]) VALUES (N'1a510b1c-78a3-454c-a04d-919b09d59fe7', N'd0ed4098-df55-4929-a37e-0d96602ae379', N'16ac5885-70d8-4491-adcd-4493bd5afe2c', CAST(N'2023-08-06T00:00:00.0000000' AS DateTime2), 500, NULL, 1)
GO
INSERT [dbo].[Order] ([Id], [UserId], [ShipmentDetailsId], [Create_at], [TotalPrice], [Note], [Status]) VALUES (N'c2c7ad74-b34e-4920-8cb9-940f0bc98058', N'd0ed4098-df55-4929-a37e-0d96602ae379', N'16ac5885-70d8-4491-adcd-4493bd5afe2c', CAST(N'2024-08-06T00:00:00.0000000' AS DateTime2), 50, NULL, 1)
GO
INSERT [dbo].[Order] ([Id], [UserId], [ShipmentDetailsId], [Create_at], [TotalPrice], [Note], [Status]) VALUES (N'55745535-ce45-4cdb-b0bb-a10030dfcd95', N'd0ed4098-df55-4929-a37e-0d96602ae379', N'16ac5885-70d8-4491-adcd-4493bd5afe2c', CAST(N'2024-08-06T00:00:00.0000000' AS DateTime2), 200, NULL, 1)
GO
INSERT [dbo].[Order] ([Id], [UserId], [ShipmentDetailsId], [Create_at], [TotalPrice], [Note], [Status]) VALUES (N'7e1f63c3-53c4-4f30-ad6a-ae453565d9cb', N'd0ed4098-df55-4929-a37e-0d96602ae379', N'16ac5885-70d8-4491-adcd-4493bd5afe2c', CAST(N'2022-08-06T00:00:00.0000000' AS DateTime2), 200, NULL, 1)
GO
INSERT [dbo].[Order] ([Id], [UserId], [ShipmentDetailsId], [Create_at], [TotalPrice], [Note], [Status]) VALUES (N'666e3e12-1918-4ab2-94c4-cdfb850d8297', N'd0ed4098-df55-4929-a37e-0d96602ae379', N'16ac5885-70d8-4491-adcd-4493bd5afe2c', CAST(N'2024-08-06T00:00:00.0000000' AS DateTime2), 50, NULL, 1)
GO
INSERT [dbo].[Order_Details] ([Id], [OrderId], [ProductId], [Quantity], [Price], [Status]) VALUES (N'19e6912d-ee60-408a-a125-280d968af0db', N'd6c97cc3-9db5-468a-8dfc-0c473d562f10', N'6F74BBFC-E630-4576-8565-0492E6997E86', 1, 200, 1)
GO
INSERT INTO [dbo].[News] ([Id], [Title], [Img], [Created_At], [View], [Author], [Source], [Status], [UserId])
VALUES
(N'BBE9FECE-807C-4FD5-B434-278B62564338', N'First Title', N'https://i.etsystatic.com/21344545/r/il/10180d/3830748305/il_fullxfull.3830748305_i3hy.jpg', GETDATE(), 10, N'Alexander', N'Source One', 1, N'D0ED4098-DF55-4929-A37E-0D96602AE379'),
(N'7789F18A-1CA2-4235-A66F-5CD25B7953B9', N'First Title', N'https://i.etsystatic.com/21344545/r/il/10180d/3830748305/il_fullxfull.3830748305_i3hy.jpg', GETDATE(), 5, N'Geogrie', N'Source Two', 1, N'D0ED4098-DF55-4929-A37E-0D96602AE379'),
(N'D5643AEF-3E1C-45ED-9697-FF0B592D3D74', N'Third Title', N'https://i.etsystatic.com/21344545/r/il/10180d/3830748305/il_fullxfull.3830748305_i3hy.jpg', GETDATE(), 0, N'Clorie', N'Source Three', 1, N'D0ED4098-DF55-4929-A37E-0D96602AE379');
GO
INSERT INTO [dbo].[New_Details] (Id, Content, ContentIndex, [Type], Status, NewId)
VALUES
    -- Đối với bài viết với Id là 'BBE9FECE-807C-4FD5-B434-278B62564338'
    (NEWID(), 'This is the first paragraph of the first news article.', 1, 0, 1, 'BBE9FECE-807C-4FD5-B434-278B62564338'),
    (NEWID(), 'https://i.etsystatic.com/21344545/r/il/10180d/3830748305/il_fullxfull.3830748305_i3hy.jpg', 2, 1, 1, 'BBE9FECE-807C-4FD5-B434-278B62564338'),
    (NEWID(), 'This is the second paragraph of the first news article.', 3, 0, 1, 'BBE9FECE-807C-4FD5-B434-278B62564338'),
    -- Đối với bài viết với Id là '7789F18A-1CA2-4235-A66F-5CD25B7953B9'
    (NEWID(), 'This is the first paragraph of the second news article.', 1, 0, 1, '7789F18A-1CA2-4235-A66F-5CD25B7953B9'),
    (NEWID(), 'https://i.etsystatic.com/21344545/r/il/10180d/3830748305/il_fullxfull.3830748305_i3hy.jpg', 2, 1, 1, '7789F18A-1CA2-4235-A66F-5CD25B7953B9'),
    -- Đối với bài viết với Id là 'D5643AEF-3E1C-45ED-9697-FF0B592D3D74'
    (NEWID(), 'This is the first paragraph of the third news article.', 1, 0, 1, 'D5643AEF-3E1C-45ED-9697-FF0B592D3D74'),
    (NEWID(), 'https://i.etsystatic.com/21344545/r/il/10180d/3830748305/il_fullxfull.3830748305_i3hy.jpg', 2, 1, 1, 'D5643AEF-3E1C-45ED-9697-FF0B592D3D74'),
    (NEWID(), 'This is the second paragraph of the third news article.', 3, 0, 1, 'D5643AEF-3E1C-45ED-9697-FF0B592D3D74');
