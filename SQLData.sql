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

GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description]) 
VALUES 
('FADE0338-BB3F-469D-8B5E-968CB71BC9A8', N'Miếng rửa chén tròn', 25000, 80, N'https://chus.vn/images/detailed/261/10753_13_F2.jpg', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27a', 1, 
N'Dùng để cọ rửa chén, xoong nồi...hiệu quả mà không làm trầy xước bề mặt..');

GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description]) 
VALUES 
('5A71FE69-CBAF-4E56-96F2-468A0A882EC7', N'Cây cọ rửa đa năng', 60000, 80, N'https://chus.vn/images/detailed/261/10753_06_F2.jpg', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27a', 1, 
N'Dùng để cọ rửa chén, xoong nồi...hiệu quả mà không làm trầy xước bề mặt');

GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description]) 
VALUES 
('C91D5631-4640-45EA-9D1B-5F089D0B4C87', N'Miếng rửa chén lớn hình chữ nhật', 25000, 80, N'https://bizweb.dktcdn.net/thumb/grande/100/424/988/products/mie-ng-ru-a-che-n-xo-mu-o-p-lo-n.png?v=1652284575223', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27a', 1, 
N'Làm sạch bụi bẩn, dầu mỡ hiệu quả mà không làm trầy xước bề mặt vật dụng');

GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description]) 
VALUES 
('959C6113-CB61-4822-8AAD-F18F8AF31EE8', N'Miếng rửa chén nhỏ hình chữ nhật', 20000, 80, N'https://xomuopvilam.com/upload/sanpham/4033cd32-ef91-4a9c-b92d-7ffc166b9ec4-8977.png', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27a', 1, 
N'Công dụng: Làm sạch bụi bẩn, dầu mỡ hiệu quả mà không làm trầy xước bề mặt vật dụng');

GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description]) 
VALUES 
('4A75361B-B4C1-411E-AE89-F7EE95294B2C', N'Miếng rửa chén lớn hình giọt nước', 25000, 80, N'https://cf.shopee.vn/file/3ce6cc41b2a1cba8014c129af8f03623', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27a', 1, 
N'Công dụng: Làm sạch bụi bẩn, dầu mỡ hiệu quả mà không làm trầy xước bề mặt vật dụng.');

GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description]) 
VALUES 
('DA77FE7D-2C8B-4CB1-BCB9-5505E4BF4269', N'Miếng rửa chén nhỏ hình giọt nước', 20000, 80, N'https://static.chus.vn/images/thumbnails/850/566/detailed/261/10753_14_C3_xwl0-zt.jpg', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27a', 1, 
N'Công dụng: Làm sạch bụi bẩn, dầu mỡ hiệu quả mà không làm trầy xước bề mặt vật dụng.');

GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description]) 
VALUES 
('52D2735F-93E1-41EE-8CD1-B6F1C5E4D0B6', N'Miếng thay cây rửa ly 2 cánh', 20000, 80, N'https://ivynature.vn/wp-content/uploads/2023/07/z4551852718350_6df61b47708a8f36a1ec561e6b6c3975.jpg', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27a', 1, 
N' Dùng để thay vào cây rửa ly (2 cánh) khi miếng xơ mướp cũ bị mòn.');

GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description]) 
VALUES 
('30E44D97-DF3E-403D-9277-C60B178320D8', N'Cây rửa ly xơ mướp 2 cánh ', 25000, 80, N'https://ivynature.vn/wp-content/uploads/2023/12/kiotviet_1638fa2d12d8c39db5104b54a8841e42.jpg', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27a', 1, 
N'Kích thước: 6,5*28cm - Thành phần: gỗ thông, xơ mướp. - Thiết kế có thể thay thế khi miếng xơ mướp đã mòn.');

GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description]) 
VALUES 
('65C41CE3-D06B-4045-A0C3-D64372B45F6B', N'Miếng rửa chén cỡ trung bình hình giọt nước', 26500, 80, N'https://chus.vn/images/detailed/261/10753_14_F3.jpg', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27a', 1, 
N'Dùng để cọ rửa chén, xoong nồi...hiệu quả mà không làm trầy xước bề mặt..');


GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description]) 
VALUES 
('D3E4A3B2-66F7-4089-9ADF-A05D790F554A', N'Miếng rửa chén trung bình hình chữ nhật', 26500, 80, N'https://static.chus.vn/images/thumbnails/850/566/product_mood_image/261/10753_15_M1.jpg', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27a', 1, 
N' Làm sạch và cải thiện tình trạng nứt gót chân nhờ kết cấu sợi đan xen tự nhiên của Xơ Mướp');


GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description]) 
VALUES 
('6F74BBFC-E630-4576-8565-0492E6997E86', N'Cây chà gót ', 60000, 80, N'https://salt.tikicdn.com/ts/tmp/68/58/96/d15ce778ad4d3c13e67f0245589b03ed.jpg', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27b', 1, 
N'Dùng để cọ rửa chén, xoong nồi...hiệu quả mà không làm trầy xước bề mặt..');


GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description]) 
VALUES 

('922C78CD-529C-4ECD-961A-3411AF81CAD2', N'Bông tắm giọt nước', 70000, 80, N'https://static.chus.vn/images/thumbnails/850/566/detailed/261/10753_03_C1_f3h4-1k.jpg', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27b', 1, 
N'Từ xa xưa, ông bà ta đã biết đến những công dụng tuyệt vời của xơ mướp trong việc làm sạch, dưỡng da, làm chậm quá trình lão hóa. Và những kinh nghiệm quý báu đó đã truyền cho đến tận ngày nay.

Với kết cấu sợi đan xen và những chất làm sạch tự nhiên ( Glycerine giữ ẩm, Vitamin E tạo ra tính đàn hồi và nhiều chất khác), khi thẩm thấu vào da, xơ mướp giúp cải tạo làn da, tăng tính đàn hồi và giảm khả năng hình thành vết nhăn, loại bỏ tế bào chết, thúc đẩy tái tạo collagen, giúp cho làn da sạch mịn đầy sức sống.

Là món quà tặng vô giá từ tự nhiên, xơ mướp an toàn cho mọi loại da, không gây kích ứng, có thể sử dụng hàng ngày hoặc 01 đến 03 lần/ tuần tùy vào mức độ sần sùi và tính nhạy cảm của làn da.');


GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description]) 
VALUES 

('CA1694C7-03A9-42D6-989D-A4B0867F06D7', N'Dây chà lưng', 90000, 80, N'https://chus.vn/images/detailed/261/10753_08_F2.jpg', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27b', 1, 
N'Từ xa xưa, ông bà ta đã biết đến những công dụng tuyệt vời của xơ mướp trong việc làm sạch, dưỡng da, làm chậm quá trình lão hóa. Và những kinh nghiệm quý báu đó đã truyền cho đến tận ngày nay.

Với kết cấu sợi đan xen và những chất làm sạch tự nhiên ( Glycerine giữ ẩm, Vitamin E tạo ra tính đàn hồi và nhiều chất khác), khi thẩm thấu vào da, xơ mướp giúp cải tạo làn da, tăng tính đàn hồi và giảm khả năng hình thành vết nhăn, loại bỏ tế bào chết, thúc đẩy tái tạo collagen, giúp cho làn da sạch mịn đầy sức sống.

Là món quà tặng vô giá từ tự nhiên, xơ mướp an toàn cho mọi loại da, không gây kích ứng, có thể sử dụng hàng ngày hoặc 01 đến 03 lần/ tuần tùy vào mức độ sần sùi và tính nhạy cảm của làn da.');


GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description]) 
VALUES 

('E46CD738-3E89-4D81-9E74-61243E2C793E', N'Bao tay tắm', 70000, 80, N'https://media3.scdn.vn/img4/2020/10_02/rSYxdj19BeL2MTTGBlN6_simg_de2fe0_500x500_maxb.jpg', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27b', 1, 
N'Từ xa xưa, ông bà ta đã biết đến những công dụng tuyệt vời của xơ mướp trong việc làm sạch, dưỡng da, làm chậm quá trình lão hóa. Và những kinh nghiệm quý báu đó đã truyền cho đến tận ngày nay.

Với kết cấu sợi đan xen và những chất làm sạch tự nhiên ( Glycerine giữ ẩm, Vitamin E tạo ra tính đàn hồi và nhiều chất khác), khi thẩm thấu vào da, xơ mướp giúp cải tạo làn da, tăng tính đàn hồi và giảm khả năng hình thành vết nhăn, loại bỏ tế bào chết, thúc đẩy tái tạo collagen, giúp cho làn da sạch mịn đầy sức sống.

Là món quà tặng vô giá từ tự nhiên, xơ mướp an toàn cho mọi loại da, không gây kích ứng, có thể sử dụng hàng ngày hoặc 01 đến 03 lần/ tuần tùy vào mức độ sần sùi và tính nhạy cảm của làn da.

*Cách dùng: thấm ướt trước khi sử dụng, dùng kèm với sữa tắm. Treo nơi thông thoáng sau mỗi lần sử dụng. Nên thay sau khoảng 02 tháng để tránh sinh khuẩn.');


GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description]) 
VALUES 

('EC3B893C-636A-4533-AA9E-F66878D3A5D1', N'Cây chà lưng ', 115000, 80, N'https://down-vn.img.susercontent.com/file/59939022ed156cf8f0ee18306c47025a', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27b', 1, 
N'Từ xa xưa, ông bà ta đã biết đến những công dụng tuyệt vời của xơ mướp trong việc làm sạch, dưỡng da, làm chậm quá trình lão hóa. Và những kinh nghiệm quý báu đó đã truyền cho đến tận ngày nay.

Với kết cấu sợi đan xen và những chất làm sạch tự nhiên ( Glycerine giữ ẩm, Vitamin E tạo ra tính đàn hồi và nhiều chất khác), khi thẩm thấu vào da, xơ mướp giúp cải tạo làn da, tăng tính đàn hồi và giảm khả năng hình thành vết nhăn, loại bỏ tế bào chết, thúc đẩy tái tạo collagen, giúp cho làn da sạch mịn đầy sức sống.

Là món quà tặng vô giá từ tự nhiên, xơ mướp an toàn cho mọi loại da, không gây kích ứng, có thể sử dụng hàng ngày hoặc 01 đến 03 lần/ tuần tùy vào mức độ sần sùi và tính nhạy cảm của làn da.');


GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description]) 
VALUES 

('C561E44A-F9A2-4D62-B635-10A463C933F7', N'Miếng rửa mặt', 30000, 80, N'https://static.chus.vn/images/thumbnails/850/566/detailed/261/10753_02_C1_shzz-3u.jpg', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27b', 1, 
N'Từ xa xưa, ông bà ta đã biết đến những công dụng tuyệt vời của xơ mướp trong việc làm sạch, dưỡng da, làm chậm quá trình lão hóa. Và những kinh nghiệm quý báu đó đã truyền cho đến tận ngày nay.

Với kết cấu sợi đan xen và những chất làm sạch tự nhiên ( Glycerine giữ ẩm, Vitamin E tạo ra tính đàn hồi và nhiều chất khác), khi thẩm thấu vào da, xơ mướp giúp cải tạo làn da, tăng tính đàn hồi và giảm khả năng hình thành vết nhăn, loại bỏ tế bào chết, thúc đẩy tái tạo collagen, giúp cho làn da sạch mịn đầy sức sống.

Là món quà tặng vô giá từ tự nhiên, xơ mướp an toàn cho mọi loại da, không gây kích ứng, dùng từ 01 đến 03 lần/ tuần tùy vào mức độ sần sùi và tính nhạy cảm của làn da.

Cách dùng: thấm ướt trước khi sử dụng, dùng kèm với sữa rửa mặt. Treo nơi thông thoáng sau mỗi lần sử dụng. Nên thay sau khoảng 02 tháng để tránh sinh khuẩn.');


GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description]) 
VALUES 

('379B5902-A901-4190-A4AA-E46C101B007B', N'Miếng thay cây chà lưng ', 18000, 80, N'https://ivynature.vn/wp-content/uploads/2023/07/LEO01390-min-scaled.jpg', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27b', 1, 
N'Dùng để thay vào cây chà lưng khi miếng xơ mướp cũ bị mòn.');


GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description]) 
VALUES 

('CC81723F-38F2-4D5A-81F0-AFDCB368C135', N'Miếng thay cây chà gót xơ mướp', 8000, 80, N'https://ivynature.vn/wp-content/uploads/2023/07/LEO01387-min-1-scaled.jpg', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27b', 1, 
N'Dùng để thay vào cây chà gót khi miếng xơ mướp cũ bị mòn.');


GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description]) 
VALUES 

('EB7A902D-3229-48AB-ADAB-C8C4FE5EFE4E', N'Xơ mướp nguyên trái size M', 30000, 80, N'https://bizweb.dktcdn.net/thumb/grande/100/408/734/products/xo-muop-nguyen-trai-600x600-1-5e2f7ea0c88b400a9b086d0be11a6dfc-master.png?v=1670484863300', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27b', 1, 
N'Từ xa xưa, ông bà ta đã biết đến những công dụng tuyệt vời của xơ mướp trong việc làm sạch, dưỡng da, làm chậm quá trình lão hóa. Và những kinh nghiệm quý báu đó đã truyền cho đến tận ngày nay.

Với kết cấu sợi đan xen và những chất làm sạch tự nhiên ( Glycerine giữ ẩm, Vitamin E tạo ra tính đàn hồi và nhiều chất khác), khi thẩm thấu vào da, xơ mướp giúp cải tạo làn da, tăng tính đàn hồi và giảm khả năng hình thành vết nhăn, loại bỏ tế bào chết, thúc đẩy tái tạo collagen, giúp cho làn da sạch mịn đầy sức sống.

Là món quà tặng vô giá từ tự nhiên, xơ mướp an toàn cho mọi loại da, không gây kích ứng, có thể sử dụng hàng ngày hoặc 01 đến 03 lần/ tuần tùy vào mức độ sần sùi và tính nhạy cảm của làn da.');


GO
INSERT [dbo].[Product] ([Id], [Name], [Price], [Quantity], [Image], [Rate], [CategoryId], [Status], [Description]) 
VALUES 

('AC33AFC2-9997-42D9-8E17-6DB242AE2690', N'Xơ mướp nguyên trái size S ', 12000, 80, N'https://bizweb.dktcdn.net/thumb/grande/100/408/734/products/mieng-xo-muop-7-8cm-600x600-44f863143cf14871bf8588b2b70bbf5d-master.png?v=1670484846483', 5, N'd8f15903-efa0-423a-a9f8-856909c4e27b', 1, 
N'Từ xa xưa, ông bà ta đã biết đến những công dụng tuyệt vời của xơ mướp trong việc làm sạch, dưỡng da, làm chậm quá trình lão hóa. Và những kinh nghiệm quý báu đó đã truyền cho đến tận ngày nay.

Với kết cấu sợi đan xen và những chất làm sạch tự nhiên ( Glycerine giữ ẩm, Vitamin E tạo ra tính đàn hồi và nhiều chất khác), khi thẩm thấu vào da, xơ mướp giúp cải tạo làn da, tăng tính đàn hồi và giảm khả năng hình thành vết nhăn, loại bỏ tế bào chết, thúc đẩy tái tạo collagen, giúp cho làn da sạch mịn đầy sức sống.

Là món quà tặng vô giá từ tự nhiên, xơ mướp an toàn cho mọi loại da, không gây kích ứng, có thể sử dụng hàng ngày hoặc 01 đến 03 lần/ tuần tùy vào mức độ sần sùi và tính nhạy cảm của làn da.

Cách dùng: thấm ướt trước khi sử dụng, dùng kèm với sữa tắm. Treo nơi thông thoáng sau mỗi lần sử dụng. Nên thay sau khoảng 02 tháng để tránh sinh khuẩn');

GO
insert into Cart values(N'4F6A84FA-C638-4D7D-B5A1-1D8C55044B28',N'D0ED4098-DF55-4929-A37E-0D96602AE379',N'AC33AFC2-9997-42D9-8E17-6DB242AE2690',2)
insert into Cart values(N'F9581755-B951-4448-8AEB-343A56C3D881',N'D0ED4098-DF55-4929-A37E-0D96602AE379',N'EB7A902D-3229-48AB-ADAB-C8C4FE5EFE4E',3)
insert into Cart values(N'5E47216B-CFBC-455A-A788-5B5E22EE7BAE',N'D0ED4098-DF55-4929-A37E-0D96602AE379',N'CC81723F-38F2-4D5A-81F0-AFDCB368C135',2)
insert into Cart values(N'50A6B77A-E359-4D23-A4BB-DA8537CBA3B3',N'D0ED4098-DF55-4929-A37E-0D96602AE379',N'E46CD738-3E89-4D81-9E74-61243E2C793E',1)
GO
CREATE TRIGGER trg_UpdateProductRate_AfterInsertComment
ON [dbo].[Comment]
AFTER INSERT
AS
BEGIN
    -- Update the product rate based on the average of comments' rates
    UPDATE p
    SET p.Rate = (
        SELECT AVG(c.Rate)
        FROM [dbo].[Comment] c
        WHERE c.ProductId = p.Id
    )
    FROM [dbo].[Product] p
    INNER JOIN inserted i ON p.Id = i.ProductId;
END;
GO

CREATE TRIGGER trg_UpdateProductRate_AfterUpdateComment
ON [dbo].[Comment]
AFTER UPDATE
AS
BEGIN
    -- Update the product rate based on the average of comments' rates
    UPDATE p
    SET p.Rate = (
        SELECT AVG(c.Rate)
        FROM [dbo].[Comment] c
        WHERE c.ProductId = p.Id
    )
    FROM [dbo].[Product] p
    INNER JOIN inserted i ON p.Id = i.ProductId;
END;
GO

CREATE TRIGGER trg_UpdateProductRate_AfterDeleteComment
ON [dbo].[Comment]
AFTER DELETE
AS
BEGIN
    -- Update the product rate based on the average of comments' rates.
    -- If no comments are left, set the product rate to 0.
    UPDATE p
    SET p.Rate = ISNULL(
        (
            SELECT AVG(c.Rate)
            FROM [dbo].[Comment] c
            WHERE c.ProductId = p.Id
        ), 0) -- If the AVG result is NULL, set Rate to 0
    FROM [dbo].[Product] p
    INNER JOIN deleted d ON p.Id = d.ProductId;
END;
GO



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
INSERT [dbo].[Category] ([Id], [Name], [Status]) VALUES (N'd8f15903-efa0-423a-a9f8-856909c4e27a', N'Đồ dùng nhà bếp', 1)
INSERT [dbo].[Category] ([Id], [Name], [Status]) VALUES (N'd8f15903-efa0-423a-a9f8-856909c4e27b', N'Đồ dùng nhà tắm', 1)


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

